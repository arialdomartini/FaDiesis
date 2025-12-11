#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session10.ParseDontValidate.SafeHead;

public class ParseDontValidate04
{

    internal abstract record NonEmpty<T>;
    record OnlyHead<T>( T Head) : NonEmpty<T>;
    record HeadAndTail<T>( T Head, List<T> Tail) : NonEmpty<T>;

    internal static T Head<T>(NonEmpty<T> nonEmpty) =>
        nonEmpty switch
        {
            OnlyHead<T> onlyHead => onlyHead.Head,
            HeadAndTail<T> headAndTail => headAndTail.Head
        };

    class Program
    {
        static NonEmpty<string> GetConfigurationDirectories()
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
                [var head] => new OnlyHead<string>(head),
                [var head, ..var rest] => new HeadAndTail<string>(head, rest)
            };
        }

        static void InitializeCache(string cacheDir)
        {
            // Replace with actual cache initialization.
            Console.WriteLine($"Initializing cache in: {cacheDir}");
        }

        void Main()
        {
            var configDirs = GetConfigurationDirectories();
            var cacheDir = Head(configDirs);
            InitializeCache(cacheDir);
        }
    }
}
