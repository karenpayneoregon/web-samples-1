using Spectre.Console;
using Spectre.Console.Json;

namespace SpectreConsoleJsonLibrary;

public class Utilities
{
    /// <summary>
    /// Displays a JSON string in the console with syntax highlighting and a custom title.
    /// </summary>
    /// <param name="json">
    /// The JSON string to be displayed. This string is expected to be in a valid JSON format.
    /// </param>
    /// <param name="text">
    /// The title or label to display above the JSON output in the console.
    /// </param>
    /// <remarks>
    /// This method utilizes the Spectre.Console library to render the JSON string with
    /// customizable colors for various JSON elements, such as braces, brackets, and values.
    /// </remarks>
    public static void DisplayJsonConsole(string json, string text)
    {
        Console.WriteLine();
        AnsiConsole.MarkupLine($"[cyan u]{text}[/]");
        AnsiConsole.Write(
            new JsonText(json)
                .BracesColor(Color.Red)
                .BracketColor(Color.Green)
                .ColonColor(Color.White)
                .CommaColor(Color.Cyan1)
                .StringColor(Color.White)
                .NumberColor(Color.White)
                .BooleanColor(Color.Red)
                .MemberColor(Color.Pink1)
                .NullColor(Color.Green));
    }
}
