#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session10.ParseDontValidate.BuyRuhm;

public static class ParseDontValidate032
{
    // Extern
    record PersonDto(
        string Name,
        string SecondName,
        int Age);

    internal record Age
    {
        public uint Value { get; }

        private Age(uint value)
        {
            Value = value;
        }

        internal static Age Of(int value)
        {
            if (value < 0 || value > 120)
                throw new NotImplementedException();

            return new Age((uint)value);
        }

        internal bool IsAdult => Value >= 18;
    }

    internal record AgeGreaterThan18
    {
        internal Age Value { get; }

        private AgeGreaterThan18(Age value)
        {
            Value = value;
        }

        internal static AgeGreaterThan18 Of(Age value)
        {
            if (!value.IsAdult)
                throw new NotImplementedException();

            return new AgeGreaterThan18(value);
        }

    }

    // Validated
    record Person(
        string Name,
        string SecondName,
        Age Age);

    record Adult(
        string Name,
        string SecondName,
        AgeGreaterThan18 Age)
    {
        public static Adult Of(Person person)
        {
            if (!person.Age.IsAdult)
            {
                AlertParent();
                throw new NotImplementedException();
            }

            return new Adult(Name: person.Name, SecondName: person.SecondName, Age: AgeGreaterThan18.Of(person.Age));
        }
    }

    static Adult CheckAdult(Person person)
    {
        return Adult.Of(person);
    }

    private static void AlertParent()
    {
        Console.WriteLine("Hey guys, your child is a bad guy");
    }

    static string GetHttpRequest(bool broken)
    {
        if (broken)
        {
            return """
                   {
                     "name": "Franco",
                     "secondName": "Bozzone",
                     "age": "not an age"
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

    static PersonDto? ParsePersonDto(string json) =>
        System.Text.Json.JsonSerializer.Deserialize<PersonDto>(json);

    private static Person ParsePerson(PersonDto? personDto)
    {
        if(personDto==null || personDto.Age < 0)
            throw new NotImplementedException();

        return new Person(
            Name: personDto.Name,
            SecondName: personDto.SecondName,
            Age: Age.Of(personDto.Age));
    }

    static bool BuyRuhm(Adult adult)
    {
        Console.WriteLine("Alla salute");
        return true;
    }

    static bool FragileApp()
    {
        string httpRequest = GetHttpRequest(true);
        PersonDto? personDto = ParsePersonDto(httpRequest);
        Person person = ParsePerson(personDto);

        //------------------

        var adult = CheckAdult(person);
            return BuyRuhm(adult);
    }

    // static bool AnotherPart()
    // {
    //     var person = new Person("Joe", "Doe", Age.Of(12));
    //
    //     return BuyRuhm(person);
    // }
}
