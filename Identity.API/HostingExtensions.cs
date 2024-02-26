using Duende.IdentityServer;
using Identity.API;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Serilog;

namespace Identity.API;

internal static class HostingExtensions
{
    public static WebApplication ConfigureServices(this WebApplicationBuilder builder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddEnvironmentVariables().Build();

        WebApiLinks.GenresApi = configuration["GenresApi"];

        builder.Services.AddRazorPages();

        builder.Services.AddIdentityServer(options =>
            {
                // see https://docs.duendesoftware.com/identityserver/v6/fundamentals/resources/
                options.EmitStaticAudienceClaim = true;
            })
            .AddInMemoryIdentityResources(Config.IdentityResources)
            .AddInMemoryApiScopes(Config.ApiScopes)
            .AddInMemoryClients(Config.Clients)
            .AddTestUsers(TestUsers.Users)
            .AddDeveloperSigningCredential();

        builder.Services.ConfigureApplicationCookie(options =>
        {
            options.Cookie.SameSite = SameSiteMode.None;
            options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
        });

        builder.Services.AddCors();

        return builder.Build();
    }
    
    public static WebApplication ConfigurePipeline(this WebApplication app)
    {
        app.UseSerilogRequestLogging();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        app.UseCors(config => config
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod()
        );

        // uncomment if you want to add a UI
        app.UseStaticFiles();
        app.UseRouting();

        app.UseIdentityServer();
        app.UseCookiePolicy(new CookiePolicyOptions { MinimumSameSitePolicy = SameSiteMode.Strict });

        app.UseAuthentication();

        // uncomment if you want to add a UI
        app.UseAuthorization();
        app.MapRazorPages().RequireAuthorization();

        app.UseEndpoints(endpoints => endpoints.MapDefaultControllerRoute());

        return app;
    }
}