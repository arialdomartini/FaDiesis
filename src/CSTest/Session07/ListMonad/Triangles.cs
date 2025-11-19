namespace CSTest.Session07.ListMonad;

using Xunit;

public class Triangles
{
    Func<int, bool> IsEven = i => i % 2 == 0;

    IEnumerable<int> A() => Enumerable.Range(1, 20);
    IEnumerable<int> B() => Enumerable.Range(1, 20).Where(i => i % 2 == 0);

    IEnumerable<int> C()
    {
        yield return 1;
        yield return 2;
        yield return 3;
        yield return 4;
        yield return 5;
        yield return 6;
        yield return 7;
        yield return 8;
        yield return 9;
        yield return 10;
        yield return 11;
        yield return 12;
        yield return 13;
        yield return 14;
        yield return 15;
        yield return 16;
        yield return 17;
        yield return 18;
        yield return 19;
        yield return 20;
    }

    bool IsRectangle(int a, int b, int c) => Math.Sqrt(a * a + b * b) == c;

    [Fact]
    void find_right_angle_triangles_having_a_and_b_even()
    {
        var triangles =
            from a in A()
            from b in B()
            from c in C()
            where IsEven(c)
            where IsRectangle(a, b, c)
            select (a, b, c);

        List<(int, int, int)> expected =
        [
            (6, 8, 10),
            (8, 6, 10),
            (12, 16, 20),
            (16, 12, 20)
        ];
        
        Assert.Equal(expected, triangles);
    }
}
