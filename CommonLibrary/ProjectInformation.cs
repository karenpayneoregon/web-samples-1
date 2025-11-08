using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Xml.Linq;

namespace CommonLibrary;

/// <summary>
/// Provides utility methods to retrieve assembly metadata such as company, product, copyright, and version information.
/// </summary>
public static class ProjectInformation
{

    /// <summary>
    /// Retrieves the copyright information associated with the calling assembly.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> containing the copyright information, or "No copyright information found" 
    /// if the copyright is not specified in the assembly metadata.
    /// </returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetCopyright()
    {
        var asm = Assembly.GetCallingAssembly();
        var attr = asm.GetCustomAttribute<AssemblyCopyrightAttribute>();
        return attr?.Copyright ?? "No copyright information found.";
    }

    /// <summary>
    /// Gets the Title from the calling project's .csproj file.
    /// </summary>
    /// <returns>
    /// The value of the &lt;Title&gt; element if found; otherwise
    /// "No title information found."
    /// </returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetTitle()
    {
        var asm = Assembly.GetCallingAssembly();
        var projectFile = TryFindProjectFile(asm);

        if (projectFile is null)
            return "No title information found.";

        return ReadTitleFromProjectFile(projectFile);
    }

    /// <summary>
    /// Retrieves the company name associated with the calling assembly.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> containing the company name, or "No company information found" 
    /// if the company name is not specified in the assembly metadata.
    /// </returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetCompany()
    {
        var asm = Assembly.GetCallingAssembly();
        var attr = asm.GetCustomAttribute<AssemblyCompanyAttribute>();
        return attr?.Company ?? "No company information found.";
    }

    /// <summary>
    /// Retrieves the product name associated with the calling assembly.
    /// </summary>
    /// <returns>
    /// A <see cref="string"/> containing the product name, or "No product information found" 
    /// if the product name is not specified in the assembly metadata.
    /// </returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetProduct()
    {
        var asm = Assembly.GetCallingAssembly();
        var attr = asm.GetCustomAttribute<AssemblyProductAttribute>();
        return attr?.Product ?? "No product information found.";
    }

    /// <summary>
    /// Retrieves the version information of the calling assembly.
    /// </summary>
    /// <returns>
    /// A <see cref="Version"/> object representing the version of the calling assembly.
    /// If the version information is not specified, a default version of 1.0.0.0 is returned.
    /// </returns>

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Version GetVersion()
    {
        var asm = Assembly.GetCallingAssembly();
        return asm.GetName().Version ?? new Version(1, 0, 0, 0);
    }

    /// <summary>
    /// Represents details about the caller of a method, including assembly information,
    /// target framework, type and method names, file path, and line number.
    /// </summary>
    public readonly record struct CallerDetails(
        string? AssemblyName,
        string? AssemblyVersion,
        string? TargetFramework,
        string? TypeName,
        string? MethodName,
        string? FilePath,
        int LineNumber);

    /// <summary>
    /// Retrieves the copyright information associated with the calling assembly.
    /// </summary>
    /// <param name="caller">
    /// When this method returns, contains details about the caller, including the assembly name, 
    /// version, target framework, type name, method name, file path, and line number.
    /// </param>
    /// <param name="memberName">
    /// The name of the member that invoked this method. This parameter is optional and is automatically 
    /// populated by the compiler if not explicitly provided.
    /// </param>
    /// <param name="filePath">
    /// The file path of the source code that invoked this method. This parameter is optional and is 
    /// automatically populated by the compiler if not explicitly provided.
    /// </param>
    /// <param name="lineNumber">
    /// The line number in the source code file that invoked this method. This parameter is optional 
    /// and is automatically populated by the compiler if not explicitly provided.
    /// </param>
    /// <returns>
    /// A <see cref="string"/> containing the copyright information, or "No copyright information found" 
    /// if the copyright is not specified in the assembly metadata.
    /// </returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetCopyright(out CallerDetails caller, [CallerMemberName] string? memberName = null,
        [CallerFilePath] string? filePath = null, [CallerLineNumber] int lineNumber = 0)
    {
        caller = BuildCallerDetails(memberName, filePath, lineNumber);
        return GetCopyright();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetCompany(
        out CallerDetails caller,
        [CallerMemberName] string? memberName = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        caller = BuildCallerDetails(memberName, filePath, lineNumber);
        return GetCompany();
    }


    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetProduct(
        out CallerDetails caller,
        [CallerMemberName] string? memberName = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        caller = BuildCallerDetails(memberName, filePath, lineNumber);
        return GetProduct();
    }


    [MethodImpl(MethodImplOptions.NoInlining)]
    private static CallerDetails BuildCallerDetails(string? memberName, string? filePath, int lineNumber)
    {
        var callingAsm = Assembly.GetCallingAssembly();
        string? typeName = null;

        try
        {
            // Skip this helper frame; capture the immediate external frame.
            var st = new StackTrace(skipFrames: 1, fNeedFileInfo: false);
            var frame = st.GetFrame(0);
            typeName = frame?.GetMethod()?.DeclaringType?.FullName;
        }
        catch
        {
            // Best-effort; leave typeName null if anything goes sideways.
        }

        var asmName = callingAsm.GetName();
        var framework = callingAsm.GetCustomAttribute<TargetFrameworkAttribute>()?.FrameworkName;

        return new CallerDetails(
            AssemblyName: asmName?.Name,
            AssemblyVersion: asmName?.Version?.ToString(),
            TargetFramework: framework,
            TypeName: typeName,
            MethodName: memberName,
            FilePath: filePath,
            LineNumber: lineNumber);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static Version GetVersion(
        out CallerDetails caller,
        [CallerMemberName] string? memberName = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        caller = BuildCallerDetails(memberName, filePath, lineNumber);
        return GetVersion();
    }


    /// <summary>
    /// Attempts to locate a .csproj file for the specified assembly by walking up
    /// from the assembly's directory.
    /// </summary>
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
    /// Gets the Title from the calling project's .csproj file and captures caller details.
    /// </summary>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetTitle(
        out CallerDetails caller,
        [CallerMemberName] string? memberName = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int lineNumber = 0)
    {
        caller = BuildCallerDetails(memberName, filePath, lineNumber);
        return GetTitle();
    }


}