using System.Reflection;
using System.Xml.Linq;

namespace CommonLibrary;
public static partial class ProjectInformation
{
    /// <summary>
    /// Attempts to locate the .csproj file associated with the specified assembly by traversing
    /// upwards from the assembly's directory.
    /// </summary>
    /// <param name="assembly">
    /// The <see cref="Assembly"/> for which to locate the corresponding .csproj file.
    /// </param>
    /// <returns>
    /// The full path to the .csproj file if found; otherwise, <see langword="null"/>.
    /// </returns>
    /// <remarks>
    /// This method searches for a .csproj file in the directory of the specified assembly and 
    /// continues searching in parent directories until a match is found or the root directory is reached.
    /// </remarks>
    private static string? TryFindProjectFile(Assembly assembly)
    {
        var location = assembly.Location;

        // Single-file or dynamic assemblies may not have a location.
        if (string.IsNullOrWhiteSpace(location))
            return null;

        var currentDirPath = Path.GetDirectoryName(location);
        if (string.IsNullOrWhiteSpace(currentDirPath))
            return null;

        var currentDir = new DirectoryInfo(currentDirPath);

        while (currentDir is not null)
        {
            var matches = currentDir.GetFiles("*.csproj", SearchOption.TopDirectoryOnly);
            if (matches.Length > 0)
                return matches[0].FullName;

            currentDir = currentDir.Parent;
        }

        return null;
    }

    /// <summary>
    /// Reads the <c>&lt;Title&gt;</c> property from a specified .csproj file.
    /// </summary>
    /// <param name="projectFilePath">The absolute path to the .csproj file.</param>
    /// <returns>
    /// A <see cref="string"/> containing the value of the <c>&lt;Title&gt;</c> property if found; 
    /// otherwise, "No title information found."
    /// </returns>
    /// <remarks>
    /// This method supports both SDK-style projects (without an MSBuild namespace) and 
    /// old-style .csproj files with the MSBuild namespace.
    /// </remarks>
    private static string ReadTitleFromProjectFile(string projectFilePath)
    {
        if (!File.Exists(projectFilePath))
            return "No title information found.";

        var doc = XDocument.Load(projectFilePath);

        // SDK-style projects (no MSBuild namespace)
        var title = doc
            .Descendants("PropertyGroup")
            .Elements("Title")
            .Select(e => e.Value)
            .FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(title))
            return title;

        // Old-style .csproj with MSBuild namespace
        XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

        title = doc
            .Descendants(msbuild + "PropertyGroup")
            .Elements(msbuild + "Title")
            .Select(e => e.Value)
            .FirstOrDefault();

        return string.IsNullOrWhiteSpace(title)
            ? "No title information found."
            : title;
    }

    /// <summary>
    /// Reads the <c>&lt;Description&gt;</c> property from a specified .csproj file.
    /// </summary>
    /// <param name="projectFilePath">The absolute path to the .csproj file.</param>
    /// <returns>
    /// A <see cref="string"/> containing the value of the <c>&lt;Description&gt;</c> property if found; 
    /// otherwise, "No description information found."
    /// </returns>
    /// <remarks>
    /// This method supports both SDK-style projects (without an MSBuild namespace) and 
    /// old-style .csproj files with the MSBuild namespace.
    /// </remarks>
    private static string ReadDescriptionFromProjectFile(string projectFilePath)
    {
        if (!File.Exists(projectFilePath))
            return "No description information found.";

        var doc = XDocument.Load(projectFilePath);

        // SDK-style projects (no MSBuild namespace)
        var description = doc
            .Descendants("PropertyGroup")
            .Elements("Description")
            .Select(e => e.Value)
            .FirstOrDefault();

        if (!string.IsNullOrWhiteSpace(description))
            return description;

        // Old-style .csproj with MSBuild namespace
        XNamespace msbuild = "http://schemas.microsoft.com/developer/msbuild/2003";

        description = doc
            .Descendants(msbuild + "PropertyGroup")
            .Elements(msbuild + "Description")
            .Select(e => e.Value)
            .FirstOrDefault();

        return string.IsNullOrWhiteSpace(description)
            ? "No description information found."
            : description;
    }

}