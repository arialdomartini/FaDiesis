namespace CSTest.Session03.FunctionalRefactoringPrimitiveObsession.Models;

record DiscountRule(
    Func<Cart, decimal> Compute)
{
    internal static readonly DiscountRule NoDiscount = new(_ => throw new InvalidOperationException("no discount"));
}
