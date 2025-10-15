using CSTest.Session03.FunctionalRefactoringRecords.Models;

namespace CSTest.Session03.FunctionalRefactoringRecords;

public class AppTests
{
    [Fact]
    void HappyPath()
    {
        var cartId = new CartId("some-gold-cart");
        var storage = new SpyStorage();

        App.ApplyDiscount(cartId, storage);

        var expected = new Cart(new CartId("some-gold-cart"), new CustomerId("gold-customer"), new Amount(50));

        Assert.Equal(expected, storage.Saved);
    }

    [Fact]
    void NoDiscount()
    {
        var cartId = new CartId("some-normal-cart");
        var storage = new SpyStorage();

        App.ApplyDiscount(cartId, storage);

        Assert.Null(storage.Saved);
    }

    [Fact]
    void MissingCart()
    {
        var cartId = new CartId("missing-cart");
        var storage = new SpyStorage();

        App.ApplyDiscount(cartId, storage);

        Assert.Null(storage.Saved);
    }

    class SpyStorage : IStorage<Cart>
    {
        public Cart Saved { get; private set; }

        public void Flush(Cart item)
        {
            Saved = item;
        }
    }
}
