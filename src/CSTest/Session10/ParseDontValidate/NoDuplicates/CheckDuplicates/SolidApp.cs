#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session10.ParseDontValidate.NoDuplicates.CheckDuplicates;

public static class Solid
{
    static void CheckNoDuplicateKeys(List<int> xs)
    {
        if (xs.Distinct().Count() != xs.Count)
            throw new ArgumentException("There are duplicates!");
    }

    static HashSet<int> ParseNoDuplicateKeys(List<int> xs)
    {
        if (xs.Distinct().Count() != xs.Count)
            throw new ArgumentException("There are duplicates!");

        return xs.ToHashSet();
    }

    static List<int> ReturnsSomeList() => [1, 2, 3, 1, 43, 22];

    private static int SolidContinue(HashSet<int> keys)
    {
        return keys.Count;
    }

    static void SolidApp()
    {
        var keys = ReturnsSomeList();

        var noDuplicateKeys = ParseNoDuplicateKeys(keys);

        SolidContinue(noDuplicateKeys);
    }
}
