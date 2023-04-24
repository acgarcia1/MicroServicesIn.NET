namespace ShoppingCart
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;

    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.Scan(selector => selector
              .FromAssemblyOf<Startup>()
              .AddClasses(c => c.Where(t => t != typeof(ProductCatalogClient) && t.GetMethods().All(m => m.Name != "<Clone>$")))
              .AsImplementedInterfaces());
            services.AddHttpClient<IProductCatalogClient, ProductCatalogClient>();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
