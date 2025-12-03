#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session09.ParseDontValidate.SafeHead;

public static class ParseDontValidate05
{
    internal abstract record Maybe<T>
    {
        internal static Maybe<T> Just(T t) => new Just<T>(t);
        internal static Maybe<T> Nothing => new Nothing<T>();
    }

    record Just<T>( T Tv) : Maybe<T>;
    record Nothing<T> : Maybe<T>;

    internal record NonEmpty<T>(T Head, List<T> Tail);

    internal static Maybe<NonEmpty<T>> SafeBuild<T>(List<T> xs) =>
        xs switch
        {
            [] => Maybe<NonEmpty<T>>.Nothing,
            var l => Maybe<NonEmpty<T>>.Just(new NonEmpty<T>(l. First(), l.Skip(1).ToList()))
        };

    internal static T UnsafeHead<T>(List<T> xs) =>
        xs.First();

    internal static T NonEmptyHead<T>(NonEmpty<T> nonEmpty) =>
        nonEmpty.Head;

    internal static Maybe<T> SafeHead<T>(List<T> xs)
    {
        Func<List<T>, Maybe<NonEmpty<T>>> safeBuild = SafeBuild;
        Func<NonEmpty<T>, T> nonEmptyHead = NonEmptyHead;

        return safeBuild.Then(Map(nonEmptyHead))(xs);
    }

    internal static Func<Maybe<TA>, Maybe<TB>> Map<TA, TB>(Func<TA, TB> f) =>
        ma =>
            ma switch
            {
                Just<TA> just => Maybe<TB>.Just(f(just.Tv)),
                Nothing<TA> => Maybe<TB>.Nothing
            };

    internal static Func<TA, TC> Then<TA, TB, TC>(this Func<TA, TB> f, Func<TB, TC> g) =>
        a => g(f(a));


}
