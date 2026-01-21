namespace CSTest.Session14;

using CancelOrder = Func<Order, bool>;
using GetCurrentUserOrders = Func<Unit, List<Order>>;

internal record Unit;

internal abstract record MaybeCapability<T>;
internal record Allowed<T>(T Value) : MaybeCapability<T>;
internal record Denied<T> : MaybeCapability<T>;

internal record Order(int Price);

internal record struct Account(int Id);

internal record struct Principal(int UserId);


internal class OrderRepository
{
    internal GetCurrentUserOrders MakeGetOrders(Account account) => _ =>
    {
        return account.Id switch
        {
            42 => [new Order(1), new Order(2), new Order(3)],
            99 => [new Order(99)],
            _ => []
        };
    };

    internal CancelOrder CancelOrder = (o) => true;

    // internal List<Order> GetCurrentUserOrders(Account account)
    // {
    //     if (account.Id == 42) return [new Order(1), new Order(2), new  Order(3)];
    //     if (account.Id == 99) return [new Order(99)];
    //     return [];
    // }
}

record OrderCapabilities(
    MaybeCapability<GetCurrentUserOrders> Get,
    MaybeCapability<CancelOrder> Cancel
);

internal static class UserSession
{
    internal static Principal GetCurrentUser()
    {
        return new Principal(42);
    }
}

internal class CapabilityProvider(OrderRepository orderRepository)
{
    private static readonly HashSet<int> AllowedAccounts = [42, 99];

    internal OrderCapabilities GetCapabilities()
    {
        var currentUserAccount = new Account(UserSession.GetCurrentUser().UserId);
        return new OrderCapabilities(
            orderRepository
                .MakeGetOrders(currentUserAccount)
                .UserIsAllowed(currentUserAccount),
            orderRepository.CancelOrder
                .OnlyOnce()
                .UserIsAllowed(currentUserAccount)
        );
    }
}

public static class Combinators
{
    public static Func<TA, TB?> OnlyOnce<TA, TB>(this Func<TA, TB> what)
    {
        var hasRun = false;
        return ta =>
        {
            if (hasRun) return default;
            hasRun = true;

            return what(ta);
        };
    }

    private static readonly HashSet<int> AllowedAccounts = [42, 99];
    internal static MaybeCapability<Func<TA, TB>> UserIsAllowed<TA, TB>(
        this Func<TA, TB> what,
        Account currentUserAccount)
    {
        return AllowedAccounts.Contains(currentUserAccount.Id)
            ? new Allowed<Func<TA, TB>>(what)
            : new Denied<Func<TA, TB>>();
    }
}

internal class Application(CapabilityProvider capabilityProvider)
{
    internal void Run()
    {
        var (maybeGet, maybeCancel) = capabilityProvider.GetCapabilities();
        switch (maybeGet)
        {
            case Denied<GetCurrentUserOrders>:
                throw new InvalidOperationException();
            case Allowed<GetCurrentUserOrders> allowed:
                allowed.Value(new Unit());
                break;
        }
    }
}

public class Test
{
    [Fact]
    void Run()
    {
        var orderRepository = new OrderRepository();
        var application = new Application(orderRepository);
        application.Run();
    }
}