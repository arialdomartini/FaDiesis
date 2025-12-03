namespace CSTest.Session09.ParseDontValidate.SafeHead;

class Void(Void Void);

public class ParseDontValidate01
{
    [Fact(Skip = "Cannot define a function returning Void")]
    void cannot_define_a_function_returning_Void()
    {
        Func<int, Void> f = i => throw new NotImplementedException();
    }


    T Head<T>(List<T> xs) =>
        xs switch
        {
            [var head, ..] => head,
            // [] => throw new NotImplementedException()
        };
    
    [Fact]
    void head_test()
    {
        List<int> xs = [1, 2, 3];
        Assert.Equal(1, Head(xs));
    }
}
