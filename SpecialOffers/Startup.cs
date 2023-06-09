﻿namespace SpecialOffers;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.Scan(selector => selector.FromAssemblyOf<Startup>()
            .AddClasses((c => c.Where(t => t.GetMethods().All(m => m.Name != "<Clone>$"))))
            .AsImplementedInterfaces());
        services.AddControllers();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment()) app.UseDeveloperExceptionPage();

        app.UseRouting();
        app.UseEndpoints(endpoints => endpoints.MapControllers());
    }
}
