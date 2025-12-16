#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session10.ParseDontValidate.SafeHead;

public class ParseDontValidate04bis
{
    record NonEmptyList<T>(
        T Head,
        List<T> Tail);

    class Program
    {
        static NonEmptyList<string> GetConfigurationDirectories()
        {
            var configDirsString = Environment.GetEnvironmentVariable("CONFIG_DIRS");
            if (configDirsString == null)
                throw new InvalidOperationException("CONFIG_DIRS environment variable is not set");

            var configDirsList = configDirsString
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(dir => dir.Trim())
                .ToList();

            return configDirsList switch
            {
                [] => throw new NotImplementedException(),
                var list => new NonEmptyList<string>(list.First(), list.Skip(1).ToList())
            };
        }

        static void InitializeCache(string cacheDir)
        {
            Console.WriteLine($"Initializing cache in: {cacheDir}");
        }

        void Main()
        {
            NonEmptyList<string> configDirs = GetConfigurationDirectories();

            var cacheDir = configDirs.Head;

            InitializeCache(cacheDir);
        }
    }
}
