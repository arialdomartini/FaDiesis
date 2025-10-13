namespace CSTest;

static class FP
{
    internal static int Sum(int a, int b) => a + b;
}

class OOP
{
    private string _log;

    public OOP(string log)
    {
        _log = log;
    }

    internal int Sum(int a, int b)
    {
        _log += "ciao";
        return a + b;
    }
}

public class UnitTest1
{
    [Fact]
    public void sum_of_2_numbers()
    {
        var result = FP.Sum(2, 3);

        Assert.Equal(5, result);
    }


    delegate int Sum(int a, int b);

    [Fact]
    public void mille_sfumature_di_function()
    {
        int Sum1(int a, int b) => a + b;
        Func<int, int, int> uncurried = (a, b) => a + b;
        var x = 42;
        var sum3 = new Func<int, int, int>((a, b) => a + b);
        Sum sum4 = new Sum((a, b) => a + b);

        Func<int, int> sommaParziale(int a)
        {
            return b => a + b;
        }

        Func<int, int> aggiungi10 = sommaParziale(10);

        Assert.Equal(12, aggiungi10(2));


        // int -> (int -> int)
        // int -> int -> int
        Func<int, Func<int, int>> curried =
            a =>
                b =>
                    a + b;

        Assert.Equal(5, uncurried (2,3) );
        Assert.Equal(5, curried(2)(3));
    }
}
