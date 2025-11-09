using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml.Linq;

namespace CommonLibrary;

/// <summary>
/// Provides utility methods to retrieve assembly metadata such as company, product, copyright, and version information.
/// </summary>
public static partial class ProjectInformation
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
    /// No title information found.
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
    /// Gets the Description from the calling project's .csproj file.
    /// </summary>
    /// <returns>
    /// The value of the &lt;Description&gt; element if found; otherwise
    /// "No description information found."
    /// </returns>
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static string GetProjectDescription()
    {
        var asm = Assembly.GetCallingAssembly();
        var projectFile = TryFindProjectFile(asm);

        if (projectFile is null)
            return "No description information found.";

        return ReadDescriptionFromProjectFile(projectFile);
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

}

