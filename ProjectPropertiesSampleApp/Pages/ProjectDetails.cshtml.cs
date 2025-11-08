using Microsoft.AspNetCore.Mvc.RazorPages;
using CommonLibrary.Models;
using static CommonLibrary.ProjectInformation;

namespace ProjectPropertiesSampleApp.Pages;

public class ProjectDetailsModel : PageModel
{
    /// <summary>
    /// Sets the details of the project, including its name, company, copyright information, and version.
    /// </summary>
    /// <value>
    /// A <see cref="CommonLibrary.Models.ProjectDetails"/> object containing the project's metadata.
    /// </value>
    public required ProjectDetails Details { get; set; } = new()
    {
        Copyright = GetCopyright(),
        ProjectName = GetProduct(),
        Company = GetCompany(),
        Version = GetVersion(),
        Title = GetTitle(),
        ProjectDescription = GetProjectDescription()
    };

    public void OnGet()
    {
    }
}