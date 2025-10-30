namespace CSTest.MyOption;

using System;

abstract record Option<TA>
{
    // () -> Option<TA>
    internal static Option<TA> None => new None<TA>();
    internal static Option<TA> Some(TA a) => new Some<TA>(a);
}

record Some<TA>(
    TA Value) : Option<TA>;

record None<TA> : Option<TA>;


static class OptionLinqExtensions
{
    public static Option<B> Select<A, B>(
        this Option<A> option,
        Func<A, B> f) => option.Map(f);

    public static Option<C> SelectMany<A, B, C>(
        this Option<A> option,
        Func<A, Option<B>> bind,
        Func<A, B, C> project) =>
        option.Bind(a =>
            bind(a)
                .Map(b => project(a, b)));

    public static Option<A> Where<A>(
        this Option<A> option,
        Func<A, bool> predicate) =>
        option.Bind(a => predicate(a)
            ? Option<A>.Some(a)
            : Option<A>.None);
}

static class OptionExtensions
{
    // A -> Option<A>
    public static Option<A> Pure<A>(A a) =>
        Option<A>.Some(a);

    // Option<A> -> (A -> B) -> Option<B>
    public static Option<B> Map<A, B>(
        this Option<A> option,
        Func<A, B> f) =>
        option switch
        {
            Some<A> some => Option<B>.Some(f(some.Value)),
            _ => Option<B>.None
        };

    // Option<A> -> (A -> Option<B>) -> Option<B>
    public static Option<B> Bind<A, B>(
        this Option<A> option,
        Func<A, Option<B>> f) =>
        option switch
        {
            Some<A> some => f(some.Value),
            _ => Option<B>.None
        };
}

public class MyOption
{
    [Fact]
    void using_linq()
    {
        var x =
            from s in Value()
            let len = s.Length
            let lenTwice = len * 2
            select $"Len is {lenTwice}";

        /*
         * do
         *   s <- Value()
         *   len = s.Lenght
         *   lenTwice = len * 2
         *    return $"Len is {lenTwice}";
         */
        /*
         * option {
         *    let! s = Value()
         *    let len = s.Lenght
         *    let lenTwice = len * 2
         *    return $"Len is {lenTwice}";
         * }
         */
    }

    private static Option<string> Value()
    {
        // return Option<string>.Some("foo");
        return Option<string>.None;
    }
}
