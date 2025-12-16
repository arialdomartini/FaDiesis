#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session10.ParseDontValidate.NoDuplicates.CheckDuplicates;

public static class Fragile
{
    static void CheckNoDuplicateKeys(List<int> xs)
    {
        if (xs.Distinct().Count() != xs.Count)
            throw new ArgumentException("There are duplicates!");
    }

    static List<int> ReturnsSomeList() => [1, 2, 3, 1, 43, 22];

    private static int FragileContinue(List<int> keys)
    {
        return keys.Count;
    }

    static void FragileApp()
    {
        var keys = ReturnsSomeList();

        CheckNoDuplicateKeys(keys);

        FragileContinue(keys);
    }
}
