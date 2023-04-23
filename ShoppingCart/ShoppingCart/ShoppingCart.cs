namespace ShoppingCart.ShoppingCart;

public class ShoppingCart
{
    private readonly HashSet<ShoppingCartItem> items = new();
    public int UserId { get; }
    public IEnumerable<ShoppingCartItem> Items => items;
    public ShoppingCart(int userId) => UserId = userId;

    public void AddItems(IEnumerable<ShoppingCartItem> shoppingCartItems)
    {
        foreach (var item in shoppingCartItems)
        {
            items.Add(item);
        }
    }

    public void RemoveItems(int[] productCatalogueIds) =>
        items.RemoveWhere(i => productCatalogueIds.Contains(
            i.ProductCatalogueId));
}

public record ShoppingCartItem(
    int ProductCatalogueId,
    string ProductName,
    string Description,
    Money Price)
{
    public virtual bool Equals(ShoppingCartItem? obj) =>
        obj != null && ProductCatalogueId.Equals(obj.ProductCatalogueId);

    public override int GetHashCode() =>
        this.ProductCatalogueId.GetHashCode();
}

public record Money(string Currency, decimal Amount);
