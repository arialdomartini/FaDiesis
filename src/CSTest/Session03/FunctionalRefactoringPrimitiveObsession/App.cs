using CSTest.Session03.FunctionalRefactoringPrimitiveObsession.Models;

namespace CSTest.Session03.FunctionalRefactoringPrimitiveObsession;

static class App
{
    internal static void ApplyDiscount(string cartId, IStorage<Cart> storage)
    {
        var cart = LoadCart(cartId);
        if (cart != Cart.MissingCart)
        {
            var rule = LookupDiscountRule(cart.CustomerId);
            if (rule != DiscountRule.NoDiscount)
            {
                var discount = rule.Compute(cart);
                var updatedCart = UpdateAmount(cart, discount);
                Save(updatedCart, storage);
            }
        }
    }

    static Cart LoadCart(string id)
    {
        if (id == "some-gold-cart")
            return new Cart(id, "gold-customer", 100);

        if (id == "some-normal-cart")
            return new Cart(id, "normal-customer", 100);

        return Cart.MissingCart;
    }

    static DiscountRule LookupDiscountRule(string id)
    {
        if (id == "gold-customer") return new DiscountRule(Half);

        return DiscountRule.NoDiscount;
    }

    static Cart UpdateAmount(Cart cart, decimal discount)
    {
        return new Cart(cart.Id, cart.CustomerId, cart.Amount - discount);
    }

    static void Save(Cart cart, IStorage<Cart> storage)
    {
        storage.Flush(cart);
    }

    static decimal Half(Cart cart) =>
        cart.Amount / 2;
}
