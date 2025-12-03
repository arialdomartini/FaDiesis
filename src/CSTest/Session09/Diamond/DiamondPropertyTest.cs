using FsCheck;

namespace CSTest.Session09.Diamond;
using FsCheck.Xunit;

public class Person
{
    public string Name { get; set; }
    public string SecondName { get; set; }
    public int Age { get; set; }
}

public class DiamondPropertyTest
{
    bool IsEven(int n) => n % 2 == 0;

    [Property]
    bool twice_a_number_is_even(int n) =>
        IsEven(n);

    [Property]
    bool age_is_even(Person n) =>
        IsEven(n.Age);
}
