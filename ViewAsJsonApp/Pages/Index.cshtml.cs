using Microsoft.AspNetCore.Mvc.RazorPages;
using NorthWind2024LocalLibrary.Data;

namespace ViewAsJsonApp.Pages;
public class IndexModel(Context context) : PageModel
{
    private static readonly Random Rnd = new Random();

    public int ContactId { get; set; }

    /// <summary>
    /// This method generates a random <see cref="ContactId"/> within the range of existing contact IDs
    /// from the database context and assigns it to the <see cref="ContactId"/> property.
    /// </summary>
    public void OnGet()
    {
        //var rnd = new Random();
        var min = 1;
        var max = context.Contacts.Max(c => c.ContactId);
        ContactId = Rnd.Next(min, max);
    }
}
