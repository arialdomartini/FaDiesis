// ReSharper disable InconsistentNaming

namespace CSTest.Session05.FunctorBuilder;

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
}

record Person(
    string Name,
    int Age);

public class FunctorBuilder
{
    Func<int, Person> BuildPerson(string name) => age => new(name, age);

    string GetName(string s) => s;
    Result<int> GetAge(string s)
    {
        var success = int.TryParse(s, out var number);

        return success
            ? Result<int>.Success(number) :
            Result<int>.Failure([$"'{s}' is not a number"]);
    }

    [Fact]
    void parsing_age_success_case()
    {
        var name = GetName("Joe");
        var age = GetAge("42");

        var buildPersonR = BuildPerson(name).Map();

        var person = buildPersonR(age);

        var expected = new Person(Name: "Joe", Age: 42);

        Assert.Equal(expected, person.Value());
    }

    [Fact]
    void parsing_age_may_fail()
    {
        var name = GetName("Joe");
        var age = GetAge("eleven");

        var buildPersonR = BuildPerson(name).Map();

        var person = buildPersonR(age);

        Assert.Equal(["'eleven' is not a number"], person.Errors());
    }
}
