using Microsoft.EntityFrameworkCore;
using NorthWind2024LocalLibrary.Data;
using NorthWind2024LocalLibrary.Models;

namespace NorthWind2024LocalLibrary.Classes;
public class ContactOperations
{
    public static async Task<Contact?> GetByIdentifier(Context context, int id) =>
        await context.Contacts
            .Include(x => x.ContactTypeIdentifierNavigation)
            .Include(x => x.ContactDevices)
            .ThenInclude(x => x.PhoneTypeIdentifierNavigation)
            .FirstOrDefaultAsync(m => m.ContactId == id);
}
