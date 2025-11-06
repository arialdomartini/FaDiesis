// ReSharper disable InconsistentNaming

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session05.ApplicativeBuilder;

record Success<A>(
    A Value) : Result<A>;

record Failure<A>(
    List<string> Errors) : Result<A>;

abstract record Result<A>
{
    internal static Result<A> Success(A a) => new Success<A>(a);
    internal static Result<A> Failure(List<string> errors) => new Failure<A>(errors);
}

static class ResultExtensions
{
    internal static A Value<A>(this Result<A> result) =>
        result switch
        {
            Success<A> success => success.Value,
            _ => throw new ArgumentException("Not a success")
        };

    internal static List<string> Errors<A>(this Result<A> result) =>
        result switch
        {
            Failure<A> failure => failure.Errors,
            _ => throw new ArgumentException("Not a failure")
        };

    // (A -> B) -> (Result<A> ->  Result<B>)
    internal static Func<Result<A>, Result<B>> Map<A, B>(this Func<A, B> f) =>
        rA =>
            rA switch
            {
                Failure<A> failure => Result<B>.Failure(failure.Errors),
                Success<A> a => Result<B>.Success(f(a.Value)),
                _ => throw new ArgumentOutOfRangeException(nameof(rA))
            };

    // f: Result<int -> (int -> DateTime -> Person)>
    // v: Result<int>

    // f: Result<Func<A, B>>
    // v: Result<A>
    // Result<B>
    internal static Result<B> Ap<A, B>(this Result<Func<A, B>> fR, Result<A> aR) =>
        fR switch
        {
            Success<Func<A, B>> fSuccess =>
                aR switch
                {
                    Success<A> aSuccess => Result<B>.Success(fSuccess.Value(aSuccess.Value)),
                    Failure<A> aFailure => Result<B>.Failure(aFailure.Errors)
                },
            Failure<Func<A, B>> fFailure =>
                aR switch
                {
                    Success<A> => Result<B>.Failure(fFailure.Errors),
                    Failure<A> aFailure => Result<B>.Failure(aFailure.Errors.Concat(fFailure.Errors).ToList())
                }

        };

    // ((a * b * c * d) -> e) -> (a -> b -> c -> d -> e)
    internal static Func<A, Func<B, Func<C, Func<D, E>>>> Curried<A, B, C, D, E>(this Func<A, B, C, D, E> f) =>
        a => b => c => d => f(a, b, c, d);

    internal static Result<B> With<A, B>(this Result<Func<A, B>> f, Result<A> rA) =>
        f.Ap(rA);

    internal static Result<Func<B, Func<C, Func<D, E>>>> With<A, B, C, D, E>(this Func<A, B, C, D, E> f, Result<A> rA) =>
        f.Curried().Map()(rA);
}

record Person(
    string Name,
    int Age,
    int Weight,
    DateTime Birthday);

public class FunctorBuilder
{
    Person BuildPerson(string name, int age, int weight, DateTime birthday) =>
        new(name, age, weight, birthday);

    Result<string> ParseName(string s) => Result<string>.Success(s);

    private static Result<int> TryParseInteger(string s) =>
        int.TryParse(s, out var number)
            ? Result<int>.Success(number)
            : Result<int>.Failure([$"'{s}' is not a number"]);

    Result<int> ParseAge(string s) => TryParseInteger(s);

    Result<int> ParseWeight(string s) => TryParseInteger(s);

    Result<DateTime> ParseBirthday(string s) =>
        DateTime.TryParse(s, out var dateTime)
            ? Result<DateTime>.Success(dateTime)
            : Result<DateTime>.Failure([$"'{s}' is not a date"]);


    [Fact]
    void parsing_age_success_case()
    {
        Result<string> nameR = ParseName("Joe");
        // Result<string> nameR = ParseName("Joe");
        Result<int> ageR = ParseAge("42");
        Result<int> weightR = ParseWeight("80");
        Result<DateTime> birthDayR = ParseBirthday("1978-11-12");

        // string -> (int -> int -> DateTime -> Person)
        // Result<string> -> Result<int -> int -> DateTime -> Person>
        var buildPerson = BuildPerson;

        var personR =
            buildPerson.Curried().Map()
                (nameR)
                .Ap(ageR)
                .Ap(weightR)
                .Ap(birthDayR);

        var expected = new Person(Name: "Joe", Age: 42, Weight: 80, Birthday: new DateTime(1978, 11, 12));

        Assert.Equal(expected, personR.Value());
    }

    [Fact]
    void parsing_age_may_fail()
    {
        var nameR = ParseName("Joe");
        var ageR = ParseAge("eleven");
        var weightR = ParseWeight("too thin");
        var birthDayR = ParseBirthday("many years ago");

        var buildPerson = BuildPerson;

        var personR =
            buildPerson
                .With(nameR)
                .With(ageR)
                .With(weightR)
                .With(birthDayR);

        List<string> expected =
        [
            "'many years ago' is not a date",
            "'too thin' is not a number",
            "'eleven' is not a number",
        ];

        Assert.Equal(expected, personR.Errors());
    }
}
