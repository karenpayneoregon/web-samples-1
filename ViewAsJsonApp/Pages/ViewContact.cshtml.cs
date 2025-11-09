using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NorthWind2024LocalLibrary.Classes;
using NorthWind2024LocalLibrary.Data;
using NorthWind2024LocalLibrary.Models;
using System.Text.Json;
using System.Text.Json.Serialization;
using Spectre.Console;
using JsonOptions = Microsoft.AspNetCore.Http.Json.JsonOptions;
using Spectre.Console.Json;
using SpectreConsoleJsonLibrary;
using Color = Spectre.Console.Color;

namespace ViewAsJsonApp.Pages;

public class ViewContactModel(Context context, IOptions<JsonOptions> jsonOptions) : PageModel
{

    public Contact Contact { get; set; } = null!;

    public async Task<IActionResult> OnGetAsync(int id)
    {

        var contact = await ContactOperations.GetByIdentifier(context, id);

        if (contact is not null)
        {
            Contact = contact;

            var json = JsonSerializer.Serialize(Contact, JsonSerializerOptions());

            Utilities.DisplayJsonConsole(json, "Contact");
            
            return Page();
        }

        return NotFound();
    }

    /// <summary>
    /// Provides custom JSON serialization options for serializing objects.
    /// </summary>
    /// <remarks>
    /// This method configures the JSON serialization settings, such as handling of reference cycles,
    /// by utilizing the options provided by the <see cref="Microsoft.AspNetCore.Http.Json.JsonOptions"/> service.
    /// </remarks>
    /// <returns>
    /// A <see cref="System.Text.Json.JsonSerializerOptions"/> instance configured with the desired serialization settings.
    /// </returns>
    private JsonSerializerOptions JsonSerializerOptions() => new(jsonOptions.Value.SerializerOptions)
    {
        ReferenceHandler = ReferenceHandler.IgnoreCycles
    };
}