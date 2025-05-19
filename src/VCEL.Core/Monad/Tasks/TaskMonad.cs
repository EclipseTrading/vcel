using System;
using System.Threading.Tasks;
using VCEL.Monad;

namespace VCEL.Core.Monad.Tasks;

public class TaskMonad : IMonad<Task<object?>>
{
    public Task<object?> Unit { get; } = Task.FromResult(default(object));

    public async Task<object?> Bind(Task<object?> m, Func<object?, Task<object?>> f)
    {
        var b = await m;
        var r = await f(b);
        return r;
    }

    public async Task<object?> Bind(Task<object?> m, IContext<Task<object?>> context,
        Func<object?, IContext<Task<object?>>, Task<object?>> f)
    {
        var b = await m;
        var r = await f(b, context);
        return r;
    }

    public async Task<object?> Bind<TValue>(Task<object?> m, IContext<Task<object?>> context,
        Func<object?, IContext<Task<object?>>, TValue, Task<object?>> f, TValue value)
    {
        var b = await m;
        var r = await f(b, context, value);
        return r;
    }

    public Task<object?> Lift<TValue>(TValue value)
    {
        if (!(value is Task task))
        {
            return Task.FromResult((object?)value);
        }

        if (value is Task<object?> to)
        {
            return to;
        }

        var type = value?.GetType();
        if (type?.GetGenericArguments().Length == 1)
        {
            return GetTaskResult(task, type.GetGenericArguments()[0]);
        }

        // Type is a Task with no return value
        return Task.FromResult((object?)value);
    }

    public static TaskMonad Instance { get; } = new TaskMonad();

    private static async Task<object?> GetTaskResult(Task task, Type genericType)
    {
        await task;
        var genericTaskType = typeof(Task<>).MakeGenericType(genericType);
        var resultProp = genericTaskType.GetProperty(nameof(Task<object>.Result));
        return resultProp?.GetValue(task);
    }

    public Task<object?> Bind(Task<object?> a, Task<object?> b, Func<object?, object?, Task<object?>> f)
        => BindExtensions.Bind(a, b, f, this);
}