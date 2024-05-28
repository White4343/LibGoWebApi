using Chapter.API.Data;
using Chapter.API.Data.Entities;
using Chapter.API.Middleware;
using Chapter.API.Repositories;
using Chapter.API.Repositories.Interfaces;
using Chapter.API.Services;
using Chapter.API.Services.Interfaces;
using Chapter.API.Validations;
using FluentValidation;
using Genre.API;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Chapter.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var configuration = GetConfiguration();

            // Add services to the container.

            WebApiLinks.BookApi = configuration["BookApi"];
            WebApiLinks.UserApi = configuration["UserApi"];

            var authority = configuration["IdentityServer:Authority"];

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Audience = "chaptersapi";
                    options.Authority = authority;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };

                    options.RequireHttpsMetadata = false;
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Chapters.Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "chapters.admin");
                });
                options.AddPolicy("Chapters.Client", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "chapters.client");
                });
            });
            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IChapterRepository, ChapterRepository>();
            builder.Services.AddScoped<IChapterService, ChapterService>();
            builder.Services.AddScoped<IBookService, BookService>();
            builder.Services.AddScoped<IBoughtBooksService, BoughtBooksService>();
            builder.Services.AddScoped<IValidator<Chapters>, ChapterValidator>();


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

            builder.Services.AddControllers();
            builder.Services.AddApiVersioning();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Chapter API", Version = "v1" });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows
                    {
                        Implicit = new OpenApiOAuthFlow
                        {
                            AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                            TokenUrl = new Uri($"{authority}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "chapters.admin", "Chapters Admin" },
                                { "chapters.client", "Chapters Client" }
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "oauth2"
                            }
                        },
                        new[] { "chapters.admin", "chapters.client" }
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
                    setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Chapter API V1");
                    setup.OAuthClientId("chapterswaggerui");
                    setup.OAuthAppName("Chapter Swagger UI");
                });
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

            app.UseAuthorization();
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
