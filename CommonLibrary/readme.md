

## ProjectInformation

| Method | Description |
| :-- | :-- |
| `GetCopyright()` | Returns the copyright string from the calling assembly’s `AssemblyCopyrightAttribute`, or `"No copyright information found."` if none is defined. |
| `GetTitle()` | Reads the `<Title>` element from the calling project’s `.csproj` file by walking up from the assembly location; returns the title if found, otherwise `"No title information found."` |
| `GetCompany()` | Returns the company name from the calling assembly’s `AssemblyCompanyAttribute`, or `"No company information found."` if none is defined. |
| `GetProduct()` | Returns the product name from the calling assembly’s `AssemblyProductAttribute`, or `"No product information found."` if none is defined. |
| `GetProjectDescription()` | Reads the `<Description>` element from the calling project’s `.csproj` file; returns the description if found, otherwise `"No description information found."` |
| `GetVersion()` | Returns the `Version` of the calling assembly (`Assembly.GetCallingAssembly().GetName().Version`), or a default `1.0.0.0` if version information is not available. |
