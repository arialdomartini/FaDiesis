// // ReSharper disable InconsistentNaming
//
// #pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
// namespace CSTest.Session05.ApplicativeBuilder;
//
// abstract record Result<A>
// {
//     internal static Result<A> Success(A a) => new Success<A>(a);
//     internal static Result<A> Failure(List<string> errors) => new Failure<A>(errors);
// }
//
// record Success<A>(
//     A Value) : Result<A>;
//
// record Failure<A>(
//     List<string> Errors) : Result<A>;
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
//     internal static Func<Result<A>, Result<B>> Map<A, B>(this Func<A, B> f) =>
//         rA =>
//             rA switch
//             {
//                 Failure<A> failure => Result<B>.Failure(failure.Errors),
//                 Success<A> a => Result<B>.Success(f(a.Value)),
//                 _ => throw new ArgumentOutOfRangeException(nameof(rA))
//             };
//
//     internal static void Ap<A, B>(this A) => throw new NotImplementedException();
// }
//
// record Person(
//     string Name,
//     int Age,
//     int Weight,
//     DateTime Birthday);
//
// public class FunctorBuilder
// {
//     void BuildPerson() => throw new NotImplementedException();;
//
//     Result<string> ParseName(string s) => Result<string>.Success(s);
//
//     private static Result<int> TryParseInteger(string s) =>
//         int.TryParse(s, out var number)
//             ? Result<int>.Success(number)
//             : Result<int>.Failure([$"'{s}' is not a number"]);
//
//     Result<int> ParseAge(string s) => TryParseInteger(s);
//
//     Result<int> ParseWeight(string s) => TryParseInteger(s);
//
//     Result<DateTime> ParseBirthday(string s) =>
//         DateTime.TryParse(s, out var dateTime)
//             ? Result<DateTime>.Success(dateTime)
//             : Result<DateTime>.Failure([$"'{s}' is not a date"]);
//
//     [Fact]
//     void parsing_age_success_case()
//     {
//         var name = ParseName("Joe");
//         var age = ParseAge("42");
//         var weight = ParseWeight("80");
//         var birthDay = ParseBirthday("1978-11-12");
//
//         var personR =
//             BuildPerson().Map()(name).Ap(age).Ap(weight).Ap(birthDay);
//
//         var expected = new Person(Name: "Joe", Age: 42, Weight: 80, Birthday: new DateTime(1978, 11, 12));
//
//         Assert.Equal(expected, personR.Value());
//     }
//
//     [Fact]
//     void parsing_age_may_fail()
//     {
//         var name = ParseName("Joe");
//         var age = ParseAge("eleven");
//         var weight = ParseWeight("too thin");
//         var birthDay = ParseBirthday("many years ago");
//
//         var personR =
//             BuildPerson().Map()(name).Ap(age).Ap(weight).Ap(birthDay);
//
//         List<string> expected =
//         [
//             "'eleven' is not a number",
//             "'too thin' is not a number",
//             "'many years ago' is not a date"
//         ];
//
//         Assert.Equal(expected, personR.Errors());
//     }
// }
