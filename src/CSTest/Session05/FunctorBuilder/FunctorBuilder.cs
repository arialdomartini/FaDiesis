// ReSharper disable InconsistentNaming

namespace CSTest.Session05.FunctorBuilder;

abstract record Result<A>
{
    internal static Result<A> Success(A a) => new Success<A>(a);
    internal static Result<A> Failure(List<string> errors) => new Failure<A>(errors);
}
record Success<A>(A Value) : Result<A>;
record Failure<A>(List<string> Errors) : Result<A>;

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

    // (a -> b) -> (Tree a -> Tree b)
    // (a -> b) -> (Option a -> Option b)
    // (a -> b) -> (Result a -> Result b)
    internal static Func<Result<A>, Result<B>> Map<A, B>(this Func<A, B> f) =>
        rA => rA switch
        {
            Success<A> success => Result<B>.Success(f(success.Value)),
            _ => Result<B>.Failure(rA.Errors())
        };

    internal static Result<B> With<A, B>(this Func<A, B> f, Result<A> aR) =>
        f.Map()(aR);

    // Func<string, int, Person> -> (Func<string, Func<int, Person>>)
    internal static Func<A, Func<B, C>> Curried<A, B, C>(this Func<A, B, C> f) =>
        a => b => f(a, b);

    internal static Func<B, C> With<A, B, C>(this Func<A, B, C> f, A a) =>
        b => f(a, b);
}

record Person(
    string Name,
    int Age);

public class FunctorBuilder
{
    // Person BuildPerson(string name, int age) => new(name, age);
    // Person BuildPerson(sting name, int age) => new(name, age);
    // Func<int, Person> BuildPerson(sting name) =>
    //     (int age) =>
    //         new(name, age);

    // (string * int) -> Person
    // string -> int -> Person
    Person BuildPerson(string name, int age) => new(name, age);

    string ParseName(string s) => s;
    Result<int> ParseAge(string s)
    {
        var success = int.TryParse(s, out var number);

        return success
            ? Result<int>.Success(number) :
            Result<int>.Failure([$"'{s}' is not a number"]);
    }

    [Fact]
    void parsing_age_success_case()
    {
        string name = ParseName("Joe");
        Result<int> ageR = ParseAge("42");

        //var person = BuildPerson(name).Map()(ageR);
        var buildPerson = BuildPerson;

        var person = buildPerson.With(name).With(ageR);

        var expected = new Person(Name: "Joe", Age: 42);

        Assert.Equal(expected, person.Value());
    }

    [Fact]
    void parsing_age_may_fail()
    {
        var name = ParseName("Joe");
        var ageR = ParseAge("eleven");

        var buildPerson = BuildPerson;
        var person = buildPerson.With(name).With(ageR);

        Assert.Equal(["'eleven' is not a number"], person.Errors());
    }
}
