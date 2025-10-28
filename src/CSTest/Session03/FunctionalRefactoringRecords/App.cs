using CSTest.Session03.FunctionalRefactoringRecords.Models;

namespace CSTest.Session03.FunctionalRefactoringRecords;

interface Result<T>;
record TrueResult<T>(T Value) : Result<T>;
record FalseResult<T> : Result<T>;

static class Functor<A>
{
    internal static Result<B> Apply<B>(Result<A> result, Func<A, B> f) =>
        result switch
        {
            FalseResult<A> => new FalseResult<B>(),
            TrueResult<A> trueResult => new TrueResult<B>(f(trueResult.Value)),
        };
}

static class ResultExtensions
{
    // Map: corresponds to LINQ Select
    public static Result<B> Select<A, B>(this Result<A> result, Func<A, B> f) =>
        Functor<A>.Apply<B>(result, f);

    // Bind: corresponds to LINQ SelectMany
    public static Result<C> SelectMany<A, B, C>(this Result<A> result,
        Func<A, Result<B>> binder,
        Func<A, B, C> projector)
    {
        return result switch
        {
            FalseResult<A> => new FalseResult<C>(),
            TrueResult<A> trueResult =>
                binder(trueResult.Value) switch
                {
                    FalseResult<B> => new FalseResult<C>(),
                    TrueResult<B> bResult =>
                        new TrueResult<C>(projector(trueResult.Value, bResult.Value)),
                    _ => throw new ArgumentException("Unknown Result type")
                },
            _ => throw new ArgumentException("Unknown Result type")
        };
    }
}

static class App
{


    private static Cart TryApplyDiscountAndSave(Cart cart,DiscountRule discountRule, IStorage<Cart> storage)
    {
        Amount discount = discountRule.Compute(cart);
        var updatedCart = UpdateAmount(cart, discount);
        return Save(updatedCart, storage);
    }

    internal static Result<Cart> ApplyDiscount(CartId cartId, IStorage<Cart> storage) =>
        from cart in LoadCart(cartId)
        from rule in LookupDiscountRule(cart.CustomerId)
        select TryApplyDiscountAndSave(cart, rule, storage);

    // functor
    // Result<A> -> (A -> B) -> Result<B>

    static Result<Cart> LoadCart(CartId id)
    {
        if (id.Value == "some-gold-cart")
            return new TrueResult<Cart>(new Cart(id, new CustomerId("gold-customer"), new Amount(100)));

        if (id.Value == "some-normal-cart")
            return new TrueResult<Cart>(new Cart(id, new CustomerId("normal-customer"), new Amount(100)));

        return new FalseResult<Cart>();
    }

    static Result<DiscountRule> LookupDiscountRule(CustomerId id)
    {
        if (id.Value == "gold-customer") return new TrueResult<DiscountRule>(new DiscountRule(Half));

        return new FalseResult<DiscountRule>();
    }

    static Cart UpdateAmount(Cart cart, Amount discount)
    {
        return new Cart(cart.Id, cart.CustomerId, new Amount(cart.Amount.Value - discount.Value));
    }

    static Cart Save(Cart cart, IStorage<Cart> storage)
    {
        storage.Flush(cart);
        return cart;
    }

    static Amount Half(Cart cart) =>
        new (cart.Amount.Value / 2);
}
