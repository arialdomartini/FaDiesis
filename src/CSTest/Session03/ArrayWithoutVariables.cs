using static CSTest.Session03.Option<int>;

namespace CSTest.Session03;

abstract record Option<T>
{
    internal static Option<T> Some(T t) => new SomeValue<T>(t);
    internal static Option<T> None() => new NoneValue<T>();

}
record SomeValue<T>(T Value) : Option<T>;
record NoneValue<T> :Option<T>;


internal static class ArrayExtensions
{
    internal static Func<int, Option<T>> Set<T>(this Func<int, Option<T>> array, int indexSet, T valueSet) =>
        index => index == indexSet
            ? Option<T>.Some(valueSet)
            : array(index);
}

public class ArrayWithoutVariables
{
    [Fact]
    void array_test()
    {
        // int -> int

        Func<int, Option<int>> emptyArray =
            _ => None();

        var array =
            emptyArray
                .Set(1, 200)
                .Set(0, 300)
                .Set(3, 200)
                .Set(1, 42)
                .Set(10_000, 42_000);


        // array[0]
        Assert.Equal(Some(300), array(0));
        Assert.Equal(Some(42), array(1));
        Assert.Equal(None(), array(500));
        Assert.Equal(Some(42_000), array(10_000));

    }
}
