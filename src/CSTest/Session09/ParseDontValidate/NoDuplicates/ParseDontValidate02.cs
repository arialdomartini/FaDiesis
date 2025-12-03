#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session09.ParseDontValidate.NoDuplicates;

public static class ParseDontValidate02
{
    internal abstract record Maybe<T>
    {
        internal static Maybe<T> Just(T t) => new Just<T>(t);
        internal static Maybe<T> Nothing => new Nothing<T>();
    }

    record Just<T>(
        T Tv) : Maybe<T>;

    record Nothing<T> : Maybe<T>;

    record Person(
        string Name,
        string SecondName,
        int Age);

    static void CheckAdult(int age)
    {
        if (age < 18)
            throw new ArgumentException("Validation error: must be an adult!");
    }

    static Person GetPerson() =>
        new("Mario", "Cioni", 42);

    static bool BuyWeapon(Person person) => true;

    static void FragileApp()
    {
        var person = GetPerson();

        CheckAdult(person.Age);

        BuyWeapon(person);
    }

    internal record NumberGreaterThan18
    {
        public int Value { get; }

        private NumberGreaterThan18(int value)
        {
            Value = value;
        }

        internal static NumberGreaterThan18 Build(int value)
        {
            if (value < 18)
                throw new ArgumentException("Validation error: must be an adult!");

            return new NumberGreaterThan18(value: value);
        }
    }

    record Adult(
        string Name,
        string SecondName,
        NumberGreaterThan18 Age);

    static Adult ParseAdult(Person person)
    {
        var age = NumberGreaterThan18.Build(person.Age);

        return new Adult(
            Name: person.Name,
            SecondName: person.SecondName,
            Age: age
        );
    }

    private static bool BuyWeapon(Adult person)
    {
        Console.WriteLine($"No need to check, {person.Age.Value} is surely >18!");
        return true;
    }

    static void SolidApp()
    {
        var person = GetPerson();

        var adult = ParseAdult(person);

        BuyWeapon(adult);
    }
}
