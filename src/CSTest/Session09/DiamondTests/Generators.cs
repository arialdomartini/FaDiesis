using FsCheck;
using FsCheck.Fluent;
using static CSTest.Session09.DiamondTests.Generators;

namespace CSTest.Session09.DiamondTests;

class Quadrant
{
    static Arbitrary<List<string>> GenQuadrants => Quadrants.ToArbitrary();

    private static Gen<List<string>> Quadrants =>
        from diamond in Diamonds
        let size = diamond.Count / 2 + 1
        let firstLines = diamond.Take(size)
        let cut = firstLines.Select(row => row[..size]).ToList()
        select cut;
}

internal static class Generators
{
    private static Gen<char> Letters =>
            from i in Gen.Choose('a', 'z')
            select (char)i;

    internal static Gen<List<string>> Diamonds =>
        from letter in Letters
        select Impl.Diamond(letter);


    internal static Gen<(List<string> Diamond, char Target)> DiamondsAndTargets =>
        from letter in Letters
        let diamond = Impl.Diamond(letter)
        select (diamond, letter);
}
