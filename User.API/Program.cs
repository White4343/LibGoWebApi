using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using User.API.Data;
using User.API.Data.Entities;
using User.API.Middleware;
using User.API.Models.Dtos;
using User.API.Repositories;
using User.API.Repositories.Interfaces;
using User.API.Services;
using User.API.Services.Interfaces;
using User.API.Validation;

namespace User.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            Stripe.StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
            WebApiLinks.BookApi = configuration["BookApi"];

            var builder = WebApplication.CreateBuilder(args);

            var authority = configuration["IdentityServer:Authority"];

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Audience = "usersapi";
                    options.Authority = authority;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false,
                    };

                    options.RequireHttpsMetadata = false;
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Users.Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "users.admin");
                });
                options.AddPolicy("Users.Client", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "users.client");
                });
            });

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IBoughtBooksRepository, BoughtBooksRepository>();
            builder.Services.AddScoped<IBoughtBooksService, BoughtBooksService>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IUserValidationRepository, UserRepository>();

            builder.Services.AddScoped<IUserSubscriptionsRepository, UserSubscriptionsRepository>();
            builder.Services.AddScoped<ISubscriptionsRepository, SubscriptionsRepository>();
            builder.Services.AddScoped<ISubscriptionsService, SubscriptionsService>();
            builder.Services.AddScoped<IUserSubscriptionsService, UserSubscriptionsService>();

            builder.Services.AddScoped<IBooksService, BooksService>();
            builder.Services.AddScoped<ICheckoutService, CheckoutService>();
            builder.Services.AddScoped<IUserService, UserService>();

            builder.Services.AddScoped<IValidator<string>, UserPasswordValidator>();
            builder.Services.AddScoped<IValidator<UserEmailDto>, UserEmailValidator>();
            builder.Services.AddScoped<IValidator<Users>, UserValidator>();
            builder.Services.AddScoped<IValidator<UserPatchDto>, UserPatchValidator>();


            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString")));

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.SetIsOriginAllowed((host => true))
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            builder.Services.AddRouting(options => options.LowercaseUrls = true);

            builder.Services.AddControllers();
            builder.Services.AddApiVersioning();


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "User.API", Version = "v1" });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new System.Uri($"{authority}/connect/authorize"),
                            TokenUrl = new System.Uri($"{authority}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "users.admin", "Users Admin" },
                                { "users.client", "Users Client" }
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new[] { "users.admin", "users.client" }
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(setup =>
                {
                    setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "User.API v1");
                    setup.OAuthClientId("usersswaggerui");
                    setup.OAuthAppName("Users Swagger UI");
                });
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            CreateDbIfNotExists(app);
            app.Run();

            IConfiguration GetConfiguration()
            {
                var builder = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.json", false, true)
                    .AddEnvironmentVariables();

                return builder.Build();
            }

            void CreateDbIfNotExists(IHost host)
            {
                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        var context = services.GetRequiredService<AppDbContext>();

                        DbInit.InitializeAsync(context).Wait();
                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred creating the DB.");
                    }
                }
            }
        }
    }
}