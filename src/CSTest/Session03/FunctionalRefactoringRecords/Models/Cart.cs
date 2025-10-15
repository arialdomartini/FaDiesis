namespace CSTest.Session03.FunctionalRefactoringRecords.Models;

record Cart(
    CartId Id,
    CustomerId CustomerId,
    Amount Amount)
{
    public static readonly Cart MissingCart = new(new CartId(""), new CustomerId(""), new Amount(0));
}
