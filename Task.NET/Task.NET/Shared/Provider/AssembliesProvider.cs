using System.Reflection;

namespace Task.NET.Shared.Provider;

public static class AssembliesProvider
{
    public static IEnumerable<Assembly> Get(List<string> allowedPrefix)
    {
        if (allowedPrefix == null)
        {
            throw new ArgumentException("AllowedPrefix is null.");
        }

        return from dll in (from fileName in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
                            where fileName.IsAllowed(allowedPrefix)
                            select fileName).ToList()
               select Assembly.Load(AssemblyName.GetAssemblyName(dll));
    }

    public static bool IsAllowed(this string name, List<string> searchNamesPrefix)
    {
        string name2 = name;
        return searchNamesPrefix.Exists((string searchNamePrefix) => name2.IsAllowed(searchNamePrefix));
    }

    private static bool IsAllowed(this string name, string searchNamePrefix)
    {
        if (searchNamePrefix == null)
        {
            throw new ArgumentException("Search assembly name is null.");
        }

        return Path.GetFileNameWithoutExtension(name).Contains(searchNamePrefix, StringComparison.InvariantCultureIgnoreCase);
    }
}
