using ShoppingCart.ShoppingCart;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ShoppingCart;

public interface IProductCatalogClient
{
    Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogueIds);
}

public class ProductCatalogClient : IProductCatalogClient
{
    private readonly HttpClient _httpClient;
    private static readonly string ProductCatalogueBaseUrl = @"https://git.io/JeHiE";
    private static readonly string GetProductPathTemplate = "?productIds=[{0}]";

    public ProductCatalogClient(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri(ProductCatalogueBaseUrl);
        httpClient
            .DefaultRequestHeaders
            .Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient = httpClient;
    }

    public async Task<IEnumerable<ShoppingCartItem>> GetShoppingCartItems(int[] productCatalogIds)
    {
        using var response = await RequestProductFromProductCatalogue(productCatalogIds);
        return await ConvertToShoppingCartItems(response);
    }

    private async Task<HttpResponseMessage> RequestProductFromProductCatalogue(int[] productCatalogueIds)
    {
        var productsResource = string.Format(GetProductPathTemplate, string.Join(",", productCatalogueIds));
        return await _httpClient.GetAsync(productsResource);
    }

    private static async Task<IEnumerable<ShoppingCartItem>> ConvertToShoppingCartItems(HttpResponseMessage response)
    {
        response.EnsureSuccessStatusCode();
        var products = await
          JsonSerializer.DeserializeAsync<List<ProductCatalogProduct>>(
            await response.Content.ReadAsStreamAsync(),
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
          ?? new();
        return products
          .Select(p =>
            new ShoppingCartItem(
              p.ProductId,
              p.ProductName,
              p.ProductDescription,
              p.Price
          ));
    }

    private record ProductCatalogProduct(int ProductId, string ProductName, string ProductDescription, Money Price);
}
