using CommonLibrary;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ProjectPropertiesSampleApp.Pages;
public class IndexModel : PageModel
{
    public required string Description { get; set; }
    public void OnGet()
    {
        Description = ProjectInformation.GetProjectDescription();
    }
}
