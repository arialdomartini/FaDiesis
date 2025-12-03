namespace CSTest.Session09.DiamondTests;

public static class Impl
{
    internal const char Space = ' ';

    internal static List<string> LettersUpTo(char target) =>
        Enumerable.Range('a', target-'a'+1)
            .Select(s => ((char)s).ToString())
            .ToList();

    internal static List<string> Diamond(char target) =>
        Quadrant(target - 'a' +1)
            .Select(row => row.HMirrored()).ToList()
            .VMirrored()
            .ToList();

    private static List<T> VMirrored<T>(this List<T> s) =>
        s.Concat(Enumerable.Reverse(s).Skip(1)).ToList();

    private static string HMirrored(this string s) =>
        s.ToList().VMirrored().Joined();

    private static string Joined(this IEnumerable<char> xs) => string.Join("", xs);

    private static string Row(int i, int quadrantSize)
    {
        var c = (char)('a' + i);
        var spaces = Spaces(quadrantSize).ToList();
        spaces[quadrantSize-i-1] = c;
        return spaces.Joined();
    }

    private static string Spaces(int quadrantSize) => new(Space, quadrantSize);

    private static List<string> Quadrant(int quadrantSize) =>
        Enumerable.Range(0, quadrantSize)
            .Select(r => Row(r, quadrantSize))
            .ToList();

}
