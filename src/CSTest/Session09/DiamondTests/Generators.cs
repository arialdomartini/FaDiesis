using FsCheck;
using FsCheck.Fluent;

namespace CSTest.Session09.DiamondTests;

internal static class Generators
{
    internal static string Joined(this List<string> xs) => string.Join("", xs);

internal static Gen<char> Letters =>
        from i in Gen.Choose('a', 'z')
        select (char)i;

    internal static Gen<List<string>> Diamonds =>
        from letter in Letters
        select Impl.Diamond(letter);

    internal static Gen<List<string>> Quadrants =>
        from diamond in Diamonds
        let size = diamond.Count / 2 + 1
        let firstLines = diamond.Take(size)
        let cut = firstLines.Select(row => row[..size]).ToList()
        select cut;


    internal static Gen<(List<string> Diamond, char Target)> DiamondsAndTargets =>
        from letter in Letters
        let diamond = Impl.Diamond(letter)
        select (diamond, letter);
}
