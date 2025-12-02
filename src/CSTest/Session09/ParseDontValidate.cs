namespace CSTest.Session09;

class Void(Void Void);

public class ParseDontValidate
{
    [Fact(Skip = "Cannot define a function returning Void")]
    void cannot_define_a_function_returning_Void()
    {
        Func<int, Void> f = i => throw new NotImplementedException();
    }
}
