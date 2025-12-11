using static CSTest.Session10.ParseDontValidate.SafeHead.ParseDontValidate02.Maybe<int>;

namespace CSTest.Session10.ParseDontValidate.SafeHead;

public class ParseDontValidate02
{
    internal abstract record Maybe<T>
    {
        internal static Maybe<T> Just(T t) => new Just<T>(t);
        internal static Maybe<T> Nothing => new Nothing<T>();
    }

    record Just<T>(
        T Tv) : Maybe<T>;

    record Nothing<T> : Maybe<T>;

    Maybe<T> Head<T>(List<T> xs) =>
        xs switch
        {
            [var head, ..] => Maybe<T>.Just(head),
            [] => Maybe<T>.Nothing
        };

    [Fact]
    void head_test()
    {
        Assert.Equal(Just(1), Head([1, 2, 3]));
        Assert.Equal(Nothing, Head<int>([]));
    }
}
