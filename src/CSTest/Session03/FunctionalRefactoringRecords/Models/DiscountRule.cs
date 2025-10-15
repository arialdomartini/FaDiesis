namespace CSTest.Session03.FunctionalRefactoringRecords.Models;

record DiscountRule(
    Func<Cart, Amount> Compute)
{
    internal static readonly DiscountRule NoDiscount = new(_ => throw new InvalidOperationException("no discount"));
}
