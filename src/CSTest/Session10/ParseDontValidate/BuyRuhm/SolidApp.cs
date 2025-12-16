using System.Text.Json;

// ReSharper disable InconsistentNaming
#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).

namespace CSTest.Session10.ParseDontValidate.NoDuplicates.BuyWeapons;

internal abstract record Maybe<T>
{
    internal static Maybe<T> Just(T t) => new Just<T>(t);
    internal static Maybe<T> Nothing => new Nothing<T>();
}

record Just<T>(
    T Value) : Maybe<T>;

record Nothing<T> : Maybe<T>;

internal static class LinqExtensions
{
    internal static Maybe<B> Select<A, B>(this Maybe<A> ma, Func<A, B> f) =>
        ma.Map(f);

    internal static Maybe<C> SelectMany<A, B, C>(
        this Maybe<A> ma,
        Func<A, Maybe<B>> bind,
        Func<A, B, C> project) =>
        ma.Bind(a => bind(a).Map(b => project(a, b)));
}

internal static class MaybeExtensions
{
    internal static Maybe<B> Map<A, B>(this Maybe<A> ma, Func<A, B> f) =>
        ma switch
        {
            Just<A> just => Maybe<B>.Just(f(just.Value)),
            Nothing<A> => Maybe<B>.Nothing
        };

    internal static Maybe<B> Bind<A, B>(this Maybe<A> ma, Func<A, Maybe<B>> f) =>
        ma switch
        {
            Just<A> just => f(just.Value),
            Nothing<A> => Maybe<B>.Nothing
        };
}

public static class ParseDontValidate03SolidApp
{
    static string GetHttpRequest(bool broken)
    {
        if (broken)
        {
            return """
                   {
                     "name": "Franco",
                     "secondName": "Bozzone",
                     "age": "not an age
                   }
                   """;
        }
        else
        {
            return """
                   {
                     "name": "Mario",
                     "secondName": "Cioni",
                     "age": 42
                   }
                   """;
        }
    }

    static Maybe<PersonDto> ParsePersonDto(this string json)
    {
        try
        {
            var personDto = JsonSerializer.Deserialize<PersonDto>(json);
            if (personDto == null)
                return Maybe<PersonDto>.Nothing;

            return Maybe<PersonDto>.Just(personDto);
        }
        catch (JsonException)
        {
            return Maybe<PersonDto>.Nothing;
        }
    }

    static Maybe<Person> ParsePerson(this PersonDto personDto)
    {
        if (personDto.Age > 0)
            return new Just<Person>(
                new Person(
                    Name: personDto.Name,
                    SecondName: personDto.SecondName,
                    Age: (uint)personDto.Age));
        else return Maybe<Person>.Nothing;
    }

    internal record NumberGreaterThan18
    {
        public uint Value { get; }

        private NumberGreaterThan18(uint value)
        {
            Value = value;
        }

        internal static Maybe<NumberGreaterThan18> Of(uint value)
        {
            if (value < 18)
                return Maybe<NumberGreaterThan18>.Nothing;

            return Maybe<NumberGreaterThan18>.Just(new NumberGreaterThan18(value));
        }
    }

    record PersonDto(
        string Name,
        string SecondName,
        int Age);

    record Person(
        string Name,
        string SecondName,
        uint Age);

    record Adult(
        string Name,
        string SecondName,
        NumberGreaterThan18 Age);

    static Maybe<Adult> ParseAdult(this Person person)
    {
        var age = NumberGreaterThan18.Of(person.Age);

        return age.Map(a =>
            new Adult(
                Name: person.Name,
                SecondName: person.SecondName,
                Age: a
            ));
    }

    private static bool BuyWeapon(Adult person)
    {
        Console.WriteLine($"No need to check, {person.Age.Value} is surely >18!");
        return true;
    }

    static Maybe<bool> SolidApp() =>
        GetHttpRequest(true)
            .ParsePersonDto()
            .Bind(ParsePerson)
            .Bind(ParseAdult)
            .Map(BuyWeapon);

    static Maybe<bool> LinqApp() =>
        from httpRequest in Maybe<string>.Just(GetHttpRequest(true))
        from personDto in httpRequest.ParsePersonDto()
        from person in personDto.ParsePerson()
        from adult in person.ParseAdult()
        select BuyWeapon(adult);
}
