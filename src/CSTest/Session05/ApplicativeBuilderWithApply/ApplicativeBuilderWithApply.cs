// ReSharper disable InconsistentNaming

#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session05.ApplicativeBuilderWithApply;

abstract record Result<A>
{
    internal static Result<A> Success(A a) => new Success<A>(a);
    internal static Result<A> Failure(List<string> errors) => new Failure<A>(errors);
}

record Success<A>(
    A Value) : Result<A>;

record Failure<A>(
    List<string> Errors) : Result<A>;

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

    internal static Func<Result<A>, Result<B>> Map<A, B>(this Func<A, B> f) =>
        rA =>
            rA switch
            {
                Failure<A> failure => Result<B>.Failure(failure.Errors),
                Success<A> a => Result<B>.Success(f(a.Value)),
                _ => throw new ArgumentOutOfRangeException(nameof(rA))
            };

    internal static Result<B> With<A, B>(this Result<Func<A, B>> f, Result<A> rA) =>
        f switch
        {
            Success<Func<A, B>> fSuccess => rA switch
            {
                Success<A> aSuccess => Result<B>.Success(fSuccess.Value(aSuccess.Value)),
                Failure<A> aFailure => Result<B>.Failure(aFailure.Errors)
            },
            Failure<Func<A, B>> fF => rA switch
            {
                Failure<A> aF => Result<B>.Failure(fF.Errors.Concat(aF.Errors).ToList()),
                _ => Result<B>.Failure(fF.Errors)
            }
        };

    private static Func<A, Func<B, Func<C, Func<D, E>>>> Curried<A, B, C, D, E>(this Func<A, B, C, D, E> f) =>
        a => b => c => d => f(a, b, c, d);

    internal static Success<Func<A, Func<B, Func<C, Func<D, E>>>>> Apply<A, B, C, D, E>(this Func<A, B, C, D, E> f) =>
        new(f.Curried());
}

record Person(
    string Name,
    int Age,
    int Weight,
    DateTime Birthday);

public class FunctorBuilder
{
    Func<string, int, int, DateTime, Person> BuildPerson => (name, age, weight, birthday) =>
        new(name, age, weight, birthday);

    Result<string> GetName(string s) => Result<string>.Success(s);

    private static Result<int> TryParseInteger(string s) =>
        int.TryParse(s, out var number)
            ? Result<int>.Success(number)
            : Result<int>.Failure([$"'{s}' is not a number"]);

    Result<int> GetAge(string s) => TryParseInteger(s);

    Result<int> GetWeight(string s) => TryParseInteger(s);

    Result<DateTime> GetBirthday(string s) =>
        DateTime.TryParse(s, out var dateTime)
            ? Result<DateTime>.Success(dateTime)
            : Result<DateTime>.Failure([$"'{s}' is not a date"]);

    [Fact]
    void parsing_age_success_case()
    {
        var personR =
            BuildPerson.Apply()
                .With(GetName("Joe"))
                .With(GetAge("42"))
                .With(GetWeight("80"))
                .With(GetBirthday("1978-11-12"));

        var expected = new Person(Name: "Joe", Age: 42, Weight: 80, Birthday: new DateTime(1978, 11, 12));

        Assert.Equal(expected, personR.Value());
    }

    [Fact]
    void parsing_age_may_fail()
    {
        var personR =
            BuildPerson.Apply()
                .With(GetName("Joe"))
                .With(GetAge("eleven"))
                .With(GetWeight("too thin"))
                .With(GetBirthday("many years ago"));

        List<string> expected =
        [
            "'eleven' is not a number",
            "'too thin' is not a number",
            "'many years ago' is not a date"
        ];

        Assert.Equal(expected, personR.Errors());
    }
}
