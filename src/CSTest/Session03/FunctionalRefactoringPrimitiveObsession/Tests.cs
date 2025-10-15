using CSTest.Session03.FunctionalRefactoringPrimitiveObsession.Models;

namespace CSTest.Session03.FunctionalRefactoringPrimitiveObsession;

public class AppTests
{
    [Fact]
    void HappyPath()
    {
        var cartId = "some-gold-cart";
        var storage = new SpyStorage();

        App.ApplyDiscount(cartId, storage);

        var expected = new Cart("some-gold-cart", "gold-customer", 50);

        Assert.Equal(expected, storage.Saved);
    }

    [Fact]
    void NoDiscount()
    {
        var cartId = "some-normal-cart";
        var storage = new SpyStorage();

        App.ApplyDiscount(cartId, storage);

        Assert.Null(storage.Saved);
    }

    [Fact]
    void MissingCart()
    {
        var cartId = "missing-cart";
        var storage = new SpyStorage();

        App.ApplyDiscount(cartId, storage);

        Assert.Null(storage.Saved);
    }

    class SpyStorage : IStorage<Cart>
    {
        internal Cart? Saved { get; private set; }

        public void Flush(Cart item)
        {
            Saved = item;
        }
    }
}
