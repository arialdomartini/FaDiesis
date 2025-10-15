namespace CSTest.Session03.FunctionalRefactoringPrimitiveObsession.Models;

record Cart(
    string Id,
    string CustomerId,
    decimal Amount)
{
    internal static readonly Cart MissingCart = new("", "", 0);
}
