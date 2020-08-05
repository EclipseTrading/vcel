using System;
using System.Threading.Tasks;
using VCEL.Monad;

namespace VCEL.Core.Monad.Tasks
{
    public class TaskMonad : IMonad<Task<object>>
    {
        public Task<object> Unit { get; } = Task.FromResult(default(object));

        public async Task<object> Bind(Task<object> m, Func<object, Task<object>> f)
        {
            var b = await m;
            var r = await f(b);
            return r;
        }

        public Task<object> Lift(object value)
        {
            if(!(value is Task task))
            {
                return Task.FromResult(value);
            }

            if(value is Task<object> to)
            {
                return to;
            }
            var type = value?.GetType();
            if(type.GetGenericArguments().Length == 1)
            {
                return GetTaskResult(task, type.GetGenericArguments()[0]);
            }
            // Type is a Task with no return value
            return Task.FromResult(value);
        }

        public static TaskMonad Instance { get; } = new TaskMonad();

        private async Task<object> GetTaskResult(Task task, Type genericType)
        {
            await task;
            var genericTaskType = typeof(Task<>).MakeGenericType(genericType);
            var resultProp = genericTaskType.GetProperty(nameof(Task<object>.Result));
            return resultProp.GetValue(task);
        }

        public Task<object> Bind(Task<object> a, Task<object> b, Func<object, object, Task<object>> f)
            => BindEx.Bind(a, b, f, this);
    }
}
