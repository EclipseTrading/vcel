using Spectre.Console;
using System.Collections.Generic;
using System.Linq;
using VCEL.Monad.Maybe;

namespace VCEL.Cli
{
    internal static class AnsiConsoleExtensions
    {
        public static string FormatAsType(this Maybe<object>? evaluatedExpression) => FormatAsType(evaluatedExpression?.Value);

        public static string FormatAsType(this object? value) => $"[green3]{value?.GetType().Name.EscapeMarkup()}[/]";

        public static string FormatAsValue(this Maybe<object>? evaluatedExpression) => FormatAsValue(evaluatedExpression?.Value);

        public static string FormatAsValue(this object? value) => $"[gold1]{value?.ToString().EscapeMarkup()}[/]";

        public static string FormatAsOption(this object? value) => $"[blue]{value?.ToString().EscapeMarkup()}[/]";

        public static string FormatAsError(this object? value) => $"[red]{value?.ToString().EscapeMarkup()}[/]";

        public static string FormatAsErrorHighlighted(this object? value) => $"[red underline]{value?.ToString().EscapeMarkup()}[/]";

        public static string FormatAsHelpItem(this string commandName, string description, params string[] options)
        {
            var item = new List<string> { commandName.FormatAsCommand() };
            item.AddRange(options.Select(option => option.FormatAsOption()));
            item.Add(description);
            return string.Join(" ", item);
        }

        public static string FormatAsCommand(this string command) => $"[purple_2]{command.EscapeMarkup()}[/]";
    }
}
