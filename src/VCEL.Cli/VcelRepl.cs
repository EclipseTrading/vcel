﻿using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using VCEL.Core.Lang;
using VCEL.CSharp;
using VCEL.Expression;
using VCEL.JS;
using VCEL.Monad.Maybe;

namespace VCEL.Cli;

internal sealed class VcelRepl
{
    private const string Name = "VCEL CLI";
    private const string NameFormatted = "[gold1]" + Name + "[/]";

    private readonly string version;
    private readonly Dictionary<string, object> context;
    private readonly List<(string Expression, ParseResult<Maybe<object>> Parsed, Maybe<object> Outcome)> history;

    private enum Mode
    {
        EXPR,
        CSHARP,
        JS,
    }

    private Mode mode = Mode.EXPR;

    public VcelRepl(string verion, Dictionary<string, object> context)
    {
        this.version = verion;
        this.context = context;
        history = new();
    }

    public void Run()
    {
        AnsiConsole.MarkupLine($"{NameFormatted} v{version.EscapeMarkup()}");
        Console.Title = Name;

        AnsiConsole.MarkupLine($"Type {".help".FormatAsCommand()} for more information.");

        while (true)
        {
            Evaluate(Read());
        }
    }

    private static string? Read()
    {
        Console.Write("> ");
        var input = Console.ReadLine();
        return input;
    }

    private void Evaluate(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return;
        }

        if (input.StartsWith('.'))
        {
            EvaluateCommand(input);
            return;
        }

        switch (this.mode)
        {
            case Mode.EXPR:
                EvaluateExpression(input);
                break;
            case Mode.CSHARP:
                ToCSharpCode(input);
                break;
            case Mode.JS:
                ToJsCode(input);
                break;
        }
    }

    private void EvaluateExpression(string input)
    {
        ParseResult<Maybe<object>>? parsed = null;
        try
        {
            parsed = VCExpression.ParseMaybe(input);
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine(
                $"Exception when parsing expression. {"This should be considered a bug in VCEL.".FormatAsErrorHighlighted()}");
            AnsiConsole.WriteException(exception);
        }

        if (parsed == null)
        {
            return;
        }

        if (!parsed.Success)
        {
            AnsiConsole.WriteLine($"{parsed.ParseErrors.Count} parse errors");

            foreach (var parseError in parsed.ParseErrors)
            {
                AnsiConsole.MarkupLine(input.EscapeMarkup().Insert(parseError.Stop + 1, "[/]")
                    .Insert(parseError.Start, "[red underline]"));
                AnsiConsole.MarkupLine(CultureInfo.InvariantCulture, 
                    "{0} (line {1}, start {2}, stop {3}, token '{4}')",
                    parseError.Message.FormatAsError(),
                    parseError.Line,
                    parseError.Start,
                    parseError.Stop,
                    parseError.Token.EscapeMarkup());
            }

            return;
        }

        Maybe<object>? outcome = null;
        try
        {
            outcome = parsed.Expression.Evaluate(context);
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine(
                $"Exception when evaluating the expression. {"This should be considered a bug in VCEL.".FormatAsErrorHighlighted()}");
            AnsiConsole.WriteException(exception);
        }

        if (outcome is not { Value: { } value })
        {
            return;
        }

        history.Add((input, parsed, outcome.Value));
        var formattedValue = (value switch
        {
            string s => s,
            IEnumerable<object> e => $"[{string.Join(", ", e)}]",
            null => "null",
            _ => value.ToString(),
        }).EscapeMarkup();
        var formattedValueType = value?.GetType().Name.EscapeMarkup() ?? "null";
        var padding = new string(' ', Console.WindowWidth - (formattedValue.Length + formattedValueType.Length));
        AnsiConsole.MarkupLine(CultureInfo.InvariantCulture,
            "[gold1]{0}[/]{1}[green3]{2}[/]", formattedValue, padding, formattedValueType);
    }

    private static void ToCSharpCode(string input)
    {
        ParseResult<string>? parsed = null;
        try
        {
            parsed = CSharpExpression.ParseCode(input);
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine(
                $"Exception when parsing expression. {"This should be considered a bug in VCEL.".FormatAsErrorHighlighted()}");
            AnsiConsole.WriteException(exception);
        }

        if (parsed == null)
        {
            return;
        }

        if (!parsed.Success)
        {
            AnsiConsole.WriteLine($"{parsed.ParseErrors.Count} parse errors");

            foreach (var parseError in parsed.ParseErrors)
            {
                AnsiConsole.MarkupLine(input.EscapeMarkup().Insert(parseError.Stop + 1, "[/]")
                    .Insert(parseError.Start, "[red underline]"));
                AnsiConsole.MarkupLine(CultureInfo.InvariantCulture,
                    "{0} (line {1}, start {2}, stop {3}, token '{4}')",
                    parseError.Message.FormatAsError(),
                    parseError.Line,
                    parseError.Start,
                    parseError.Stop,
                    parseError.Token.EscapeMarkup());
            }

            return;
        }

        var outcome = parsed.Expression.Evaluate(new { });
        AnsiConsole.MarkupLine(outcome.FormatAsValue());
    }

    private static void ToJsCode(string input)
    {
        var jsParserFactory = new ToJsCodeFactory<string>(ConcatStringMonad.Instance);
        var parser = new ExpressionParser<string>(jsParserFactory);

        ParseResult<string>? parsed = null;
        try
        {
            parsed = parser.Parse(input);
        }
        catch (Exception exception)
        {
            AnsiConsole.MarkupLine(
                $"Exception when parsing expression. {"This should be considered a bug in VCEL.".FormatAsErrorHighlighted()}");
            AnsiConsole.WriteException(exception);
        }

        if (parsed == null)
        {
            return;
        }

        if (!parsed.Success)
        {
            AnsiConsole.WriteLine($"{parsed.ParseErrors.Count} parse errors");

            foreach (var parseError in parsed.ParseErrors)
            {
                AnsiConsole.MarkupLine(input.EscapeMarkup().Insert(parseError.Stop + 1, "[/]")
                    .Insert(parseError.Start, "[red underline]"));
                AnsiConsole.MarkupLine(CultureInfo.InvariantCulture, 
                    "{0} (line {1}, start {2}, stop {3}, token '{4}')",
                    parseError.Message.FormatAsError(),
                    parseError.Line,
                    parseError.Start,
                    parseError.Stop,
                    parseError.Token.EscapeMarkup());
            }

            return;
        }

        var outcome = parsed.Expression.Evaluate(new { });
        AnsiConsole.MarkupLine(outcome.FormatAsValue());
    }

    private void EvaluateCommand(string input)
    {
        var inputParts = input.Split(' ');
        var firstPart = inputParts.FirstOrDefault()?.Trim() ?? string.Empty;
        switch (firstPart)
        {
            case ".help":
                HelpCommand();
                break;
            case ".version":
                AnsiConsole.MarkupLine($"v{version.EscapeMarkup()}");
                break;
            case ".exit":
                Environment.Exit(0);
                break;
            case ".set" when inputParts.Length is 2:
                SetCommand(inputParts[1], 0);
                break;
            case ".set" when inputParts.Length is 3:
                if (int.TryParse(inputParts[2], out var index))
                {
                    SetCommand(inputParts[1], index);
                }
                else
                {
                    AnsiConsole.MarkupLine(
                        $"{".set".FormatAsCommand()} command history index must be a valid integer value");
                }

                break;
            case ".set":
                AnsiConsole.MarkupLine(
                    $"{".set".FormatAsCommand()} command must define exactly one property name and optionally a history index");
                break;
            case ".parse" when inputParts.Length is 2:
                ParseCommand(inputParts[1]);
                break;
            case ".list":
                ListCommand();
                break;
            case ".history":
                HistoryCommand();
                break;
            case ".toggle":
                ToggleCommand(inputParts);
                break;
            default:
                AnsiConsole.MarkupLine(
                    $"{firstPart.FormatAsCommand()} command is not recognised. Run {".help".FormatAsCommand()} for help");
                break;
        }
    }

    private void HelpCommand()
    {
        AnsiConsole.MarkupLine($"{NameFormatted} v{version.EscapeMarkup()}");
        AnsiConsole.MarkupLine($"Commands (always start with '{".".FormatAsCommand()}'):");
        AnsiConsole.MarkupLine(".help".FormatAsHelpItem("Shows help information"));
        AnsiConsole.MarkupLine(".version".FormatAsHelpItem("Shows version"));
        AnsiConsole.MarkupLine(".exit".FormatAsHelpItem("Exits the application"));
        AnsiConsole.MarkupLine(
            ".set".FormatAsHelpItem("Adds the most recent expression outcome as a named property", "NAME"));
        AnsiConsole.MarkupLine(
            ".set".FormatAsHelpItem(
                "Adds the expression outcome at the provided history index as a named property",
                "NAME", "INDEX"));
        AnsiConsole.MarkupLine(
            ".parse".FormatAsHelpItem(
                "Parse expressions in the file into CSharp code. Display failed ones",
                "FILENAME"));
        AnsiConsole.MarkupLine(
            ".list".FormatAsHelpItem("Shows a list of set properties; their names, values and types"));
        AnsiConsole.MarkupLine(
            ".history".FormatAsHelpItem("Shows the history of evaluated expressions and their outcomes"));
        AnsiConsole.MarkupLine(
            ".toggle".FormatAsHelpItem("Change the mode of the parser, accepts {EXPR, CSHARP, JS}",
                "MODE"));
        AnsiConsole.MarkupLine("\nInput is evaluated as an expression if not a command.");
    }

    private void SetCommand(string propertyName, int index)
    {
        if (index < 0 || index >= history.Count)
        {
            AnsiConsole.MarkupLine($"{".set".FormatAsCommand()} history index must be a valid index");
            return;
        }

        var value = history[history.Count - 1 - index];
        context[propertyName] = value.Outcome.Value;
        AnsiConsole.MarkupLine(
            $"{propertyName.FormatAsOption()} = {value.Expression} = {value.Outcome.FormatAsValue()}");
    }

    private static void ParseCommand(string filePath)
    {
        try
        {
            using var file = new StreamReader(filePath);
            var processedExpr = "";
            while (file.ReadLine() is { } rawExpr)
            {
                try
                {
                    processedExpr = rawExpr.Trim('\"');
                    processedExpr = processedExpr.Replace(@"\r\n", string.Empty);
                    processedExpr = string.IsNullOrWhiteSpace(processedExpr) ? "null" : processedExpr;

                    var vcelResult = VCExpression.ParseMaybe(processedExpr);
                    if (!vcelResult.Success)
                    {
                        AnsiConsole.MarkupLine("Failed to parse into VCEL expressions".FormatAsError());
                        AnsiConsole.MarkupLine($"Raw Vcel expression:{$"{rawExpr}".FormatAsValue()}");
                        AnsiConsole.MarkupLine($"Vcel expression:{$"{processedExpr}".FormatAsValue()}");
                        foreach (var error in vcelResult.ParseErrors)
                            AnsiConsole.MarkupLine($"Error:{$"{error.Message}".FormatAsValue()}");
                        AnsiConsole.MarkupLine("");
                    }

                    var csharpResult = CSharpExpression.ParseDelegate(processedExpr);
                    if (!csharpResult.Success)
                    {
                        var csharpExpr = CSharpExpression.ParseCode(processedExpr).Expression.Evaluate(null!);
                        AnsiConsole.MarkupLine("Failed to compile CSharp code".FormatAsError());
                        AnsiConsole.MarkupLine($"Raw Vcel expression:{$"{rawExpr}".FormatAsValue()}");
                        AnsiConsole.MarkupLine($"Vcel expression:{$"{processedExpr}".FormatAsValue()}");
                        AnsiConsole.MarkupLine($"CSharp expression:{$"{csharpExpr}".FormatAsValue()}");
                        foreach (var error in csharpResult.ParseErrors)
                            AnsiConsole.MarkupLine($"Error:{$"{error.Message}".FormatAsValue()}");
                        AnsiConsole.MarkupLine("");
                    }
                }
                catch (Exception e)
                {
                    AnsiConsole.MarkupLine(
                        $"Exception when parsing expression. {"This should be considered a bug in VCEL.".FormatAsErrorHighlighted()}");
                    AnsiConsole.WriteException(e);
                    AnsiConsole.MarkupLine($"Vcel expression:{$"{processedExpr}".FormatAsValue()}");
                }
            }
        }
        catch (Exception e)
        {
            AnsiConsole.MarkupLine($"Failed to process {filePath}: {e.Message}");
        }
    }

    private void ListCommand()
    {
        if (context.Count == 0)
        {
            AnsiConsole.WriteLine("Empty");
            return;
        }

        var list = new Table();
        list.AddColumns("Property name", "Value", "Type");
        foreach (var (key, value) in context)
        {
            list.AddRow(key.FormatAsOption(), value.FormatAsValue(), value.FormatAsType());
        }

        AnsiConsole.Write(list);
    }

    private void HistoryCommand()
    {
        if (history.Count == 0)
        {
            AnsiConsole.WriteLine("Empty");
            return;
        }

        var historyTable = new Table();
        historyTable.AddColumns("History index", "Expression", "Outcome", "Type", "Dependencies");
        for (var i = 0; i < history.Count; i++)
        {
            var (expression, parsed, outcome) = history[i];
            var historyIndex = (history.Count - 1 - i).ToString(CultureInfo.InvariantCulture);
            var dependencies = string.Join(
                ", ", parsed.Expression.Dependencies.Select(dep => $"{dep.Name}({dep.GetType().Name})"));
            historyTable.AddRow(
                historyIndex, expression, outcome.FormatAsValue(), outcome.FormatAsType(), dependencies);
        }

        AnsiConsole.Write(historyTable);
    }

    private void ToggleCommand(string[] inputParts)
    {
        if (inputParts.Length is 2)
        {
            var choice = inputParts[1].ToUpper(CultureInfo.InvariantCulture);
            mode = choice switch
            {
                "EXPR" => Mode.EXPR,
                "CSHARP" => Mode.CSHARP,
                "JS" => Mode.JS,
                _ => mode
            };
        }

        AnsiConsole.MarkupLine($"Current mode is {$"{mode}".FormatAsValue()}.");
    }
}