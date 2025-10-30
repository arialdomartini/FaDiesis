using static CSTest.Session04.Option<int>;

namespace CSTest.Session04;

//
// type Option<'a> =
//     | Some of 'a
//     | None

abstract record Option<TA>
{
    // () -> Option<TA>
    internal static Option<TA> None => new None<TA>();
    internal static Option<TA> Some(TA a) => new Some<TA>(a);
}

record Some<TA>(
    TA Value) : Option<TA>;

record None<TA> : Option<TA>;

static class OptionExtensions
{
    // A -> Option<A>
    static Option<TA> Pure<TA>(TA a) => new Some<TA>(a);

    // Option<A> -> (A -> B) -> Option<B>
    internal static Option<B> Map<A, B>(this Option<A> option, Func<A, B> f) =>
        option switch
        {
            Some<A> some => Option<B>.Some(f(some.Value)),
            _ => Option<B>.None
        };

    // A = string, B= Option<int>
    // string -> Option<int>
    // (A -> B) -> (Option<A> -> Option<B>)
    // (string -> Option<int>) -> (Option<string> -> Option<Option<int>>)
    internal static Func<Option<A>, Option<B>> MapC<A, B>(this Func<A, B> f)
        => option =>
            option switch
            {
                Some<A> some => Option<B>.Some(f(some.Value)),
                _ => Option<B>.None
            };

    // (A -> Option<B>) -> (Option<A> -> Option<B>)
    internal static Func<Option<A>, Option<B>> Bind<A, B>(this Func<A, Option<B>> f) =>
        optionA =>
            optionA switch
            {
                Some<A> some => f(some.Value),
                _ => Option<B>.None
            };
}

public class OptionTest
{
    Option<string> GetValue()
    {
        return Option<string>.Some("foo");
    }

    // string -> int
    private Func<string, int> Len => s => s.Length;
    private Func<int, int> Twice => i => i * 2;

    // string -> Option<int>
    private Func<string, Option<int>> LenSt => s =>
        s == "foo"
            ? None
            : Some(s.Length);

    // int -> Option<int>
    private Func<int, Option<int>> TwiceSt =>
        i =>
            i == 5
                ? None
                : Some(i * 2);

    [Fact]
    void test_bind()
    {
        var s = Option<string>.Some("foo");
        Func<Option<string>, Option<int>> lenStO = LenSt.Bind();
        Option<int> lenSt = lenStO(s);
    }

    [Fact]
    void map_Option_content()
    {
        {
            var b = Len("foo");
            Assert.Equal(3, b);
        }
        {
            Option<string> option = GetValue();
            // var b = Len(option);
            Option<int> b =
                option
                    .Map(Len)
                    .Map(Twice);
        }
    }

    [Fact]
    void map_OptionC_content()
    {
        Func<string, int> len = Len;
        Option<string> option = GetValue();

        // lift
        var lenO = len.MapC();
        var twiceO = Twice.MapC();

        var b = twiceO(lenO(option));

        // var b = Len(option);
        Option<int> c =
            option
                .Map(Len)
                .Map(Twice);
    }

    [Fact]
    void may_fail()
    {
        Option<int> v = Some(42);
        Option<int> n = None;

        Assert.Equal(Some(42), v);

        var contentV =
            v switch
            {
                None<int> => "There is no value",
                Some<int> c => $"Value is {c.Value}"
            };

        var contentN =
            v switch
            {
                None<int> => "There is no value",
                Some<int> c => $"Value is {c.Value}"
            };

        Assert.Equal("Value is 42", contentV);
    }
}
