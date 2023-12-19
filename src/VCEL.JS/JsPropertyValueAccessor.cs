using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using VCEL.Core.Helper;
using VCEL.Monad;

namespace VCEL.JS
{
    public class JsPropertyValueAccessor : IValueAccessor<string>
    {
        private readonly IReadOnlyDictionary<string, string> jsDatePropertyMethods = new Dictionary<string, string>()
        {
            { "Now", "getTime()" },
            { "Today", "getDate()" },
            { "Year", "getFullYear()" },
            { "Month", "getMonth()" },
            { "Day", "getDay()" },
            { "Date", "getDate()" },
            { "Hour", "getHours()" },
            { "Millisecond", "getMilliseconds()"},
            { "Minute", "getMinutes()"},
            { "Second", "getSeconds()"}
        };

        private readonly string jsDateObjPattern = @"\(new Date\(([0-9]+|)\)\)";
        private readonly IMonad<string> monad;
        private readonly string propName;
        private readonly IReadOnlyDictionary<string, Func<string>>? overridePropertyFunc;

        public JsPropertyValueAccessor(IMonad<string> monad, string propName, IReadOnlyDictionary<string, Func<string>>? overridePropertyFunc = null)
        {
            this.monad = monad;
            this.propName = propName;
            this.overridePropertyFunc = overridePropertyFunc;
        }
        public string GetValue(IContext<string> context)
        {
            if (overridePropertyFunc != null && overridePropertyFunc.TryGetValue(propName, out var func))
            {
                return monad.Lift(func());
            }

            string finalPropOrMethod = propName;
            if (Regex.Match(context.Value, jsDateObjPattern).Success && jsDatePropertyMethods.TryGetValue(propName, out var jsDateMethod))
            {
                finalPropOrMethod = jsDateMethod;
            }

            var jsObjContext = context as JsObjectContext;
            return monad.Lift(jsObjContext?.Object is string
                ? $"{context.Value}.{finalPropOrMethod}"
                : finalPropOrMethod.Equals("_") 
                    ? $"{finalPropOrMethod}" 
                    : $"{Constants.DefaultContext}.{finalPropOrMethod}");
        }
    }
}
