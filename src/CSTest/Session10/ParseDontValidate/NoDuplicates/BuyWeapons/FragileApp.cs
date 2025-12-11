#pragma warning disable CS8509 // The switch expression does not handle all possible values of its input type (it is not exhaustive).
namespace CSTest.Session10.ParseDontValidate.NoDuplicates.BuyWeapons;

public static class ParseDontValidate03
{
    record Person(
        string Name,
        string SecondName,
        int Age);

    static void CheckAdult(int age)
    {
        if (age < 18)
            throw new ArgumentException("Validation error: must be an adult!");
    }

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

    static Person? ParsePerson(string json) =>
        System.Text.Json.JsonSerializer.Deserialize<Person>(json);

    static bool BuyWeapon(Person person) => true;

    static void FragileApp()
    {
        var httpRequest = GetHttpRequest(true);

        var person = ParsePerson(httpRequest);

        CheckAdult(person.Age);

        BuyWeapon(person);
    }
}
