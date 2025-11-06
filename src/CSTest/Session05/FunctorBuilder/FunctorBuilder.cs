// // ReSharper disable InconsistentNaming
//
// namespace CSTest.Session05.FunctorBuilder;
//
// abstract record Result<A>
// {
//     internal static Result<A> Success(A a) => new Success<A>(a);
//     internal static Result<A> Failure(List<string> errors) => new Failure<A>(errors);
// }
//
// static class ResultExtensions
// {
//     internal static A Value<A>(this Result<A> result) =>
//         result switch
//         {
//             Success<A> success => success.Value,
//             _ => throw new ArgumentException("Not a success")
//         };
//
//     internal static List<string> Errors<A>(this Result<A> result) =>
//         result switch
//         {
//             Failure<A> failure => failure.Errors,
//             _ => throw new ArgumentException("Not a failure")
//         };
//
//     internal static void Map<A, B>(this Func<A, B> f) =>
//         throw new NotImplementedException();
// }
//
// record Person(
//     string Name,
//     int Age);
//
// public class FunctorBuilder
// {
//     Func<int, Person> BuildPerson() => throw new NotImplementedException();
//
//     string ParseName(string s) => s;
//     Result<int> ParseAge(string s)
//     {
//         var success = int.TryParse(s, out var number);
//
//         return success
//             ? Result<int>.Success(number) :
//             Result<int>.Failure([$"'{s}' is not a number"]);
//     }
//
//     [Fact]
//     void parsing_age_success_case()
//     {
//         var name = ParseName("Joe");
//         var ageR = ParseAge("42");
//
//         var buildPersonR = BuildPerson(name).Map();
//
//         var person = buildPersonR(ageR);
//
//         var expected = new Person(Name: "Joe", Age: 42);
//
//         Assert.Equal(expected, person.Value());
//     }
//
//     [Fact]
//     void parsing_age_may_fail()
//     {
//         var name = ParseName("Joe");
//         var ageR = ParseAge("eleven");
//
//         var buildPersonR = BuildPerson(name).Map();
//
//         var person = buildPersonR(ageR);
//
//         Assert.Equal(["'eleven' is not a number"], person.Errors());
//     }
// }
