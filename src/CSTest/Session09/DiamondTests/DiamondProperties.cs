using static CSTest.Session09.DiamondTests.Impl;

namespace CSTest.Session09.DiamondTests;

internal static class DiamondProperties
{
    internal static bool IsASquare(List<string> diamond) =>
        diamond.TrueForAll(row =>
            row.Length == diamond.Count);

    private static bool IsHorizontallySymmetric(this string row) =>
        row.Reverse().Zip(row).All(tuple => tuple.First == tuple.Second);

    internal static bool IsHorizontallySymmetric(this List<string> diamond) =>
        diamond.TrueForAll(row => row.IsHorizontallySymmetric());

    internal static bool IsVerticallySymmetric(this List<string>  diamond)
    {
        var enumerable = diamond.Transpose();

        return enumerable.ToList().IsHorizontallySymmetric();
    }

    private static IEnumerable<string> Transpose(this List<string> diamond)
    {
        return Enumerable.Range(0, diamond.Count)
            .Select(row => string.Join("", diamond.Select(r => r[row])));
    }


    internal static bool ContainsAllLettersUpTo(this List<string> diamond, char target)
    {
        var lettersUpToTarget = LettersUpTo(target);
        var diamondAsAString = string.Join("", diamond);

        return lettersUpToTarget.TrueForAll(
            l => diamondAsAString.Contains(l));
    }

    private static bool ContainsOnlyOneLetter(string row) =>
        row.Replace(Space.ToString(), "").Length == 1;

    internal static bool ContainsOnlyOneLetter(this List<string> quadrant) =>
        quadrant.TrueForAll(ContainsOnlyOneLetter);

    internal static bool ContainsLettersOnDiagonal(this List<string> quadrant) =>
        quadrant.Select((row, index) =>
            row[row.Length-1-index] != Space)
            .All(Identity());

    private static Func<bool, bool> Identity() => r => r;
}
