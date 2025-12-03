namespace CSTest.Session09.ParseDontValidate.SafeHead;

public class ParseDontValidate03
{
    class Program
    {
        static List<string> GetConfigurationDirectories()
        {
            var configDirsString = Environment.GetEnvironmentVariable("CONFIG_DIRS");
            if (configDirsString == null)
                throw new InvalidOperationException("CONFIG_DIRS environment variable is not set");

            var configDirsList = configDirsString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(dir => dir.Trim())
                .ToList();

            if (configDirsList.Count == 0)
                throw new InvalidOperationException("CONFIG_DIRS cannot be empty");

            return configDirsList;
        }

        static void InitializeCache(string cacheDir)
        {
            // Replace with actual cache initialization.
            Console.WriteLine($"Initializing cache in: {cacheDir}");
        }

        void Main()
        {
            var configDirs = GetConfigurationDirectories();
            var cacheDir = configDirs.FirstOrDefault();
            if (cacheDir != null)
            {
                InitializeCache(cacheDir);
            }
            else
            {
                throw new Exception("should never happen; already checked configDirs is non-empty");
            }
        }
    }
}
