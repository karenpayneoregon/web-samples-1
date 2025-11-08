using CommonLibrary;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjectPropertiesSampleApp.Pages;
public class IndexModel : PageModel
{


    public void OnGet()
    {
        var title = ProjectInformation.GetTitle();
        Console.WriteLine(title);
    }
}
