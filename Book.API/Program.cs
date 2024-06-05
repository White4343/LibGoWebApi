using Book.API.Data;
using Book.API.Data.Entities;
using Book.API.Middleware;
using Book.API.Repositories;
using Book.API.Repositories.Interfaces;
using Book.API.Services;
using Book.API.Services.Interfaces;
using Book.API.Validation;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using StackExchange.Redis;

namespace Book.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var configuration = GetConfiguration();

            var builder = WebApplication.CreateBuilder(args);

            var redisConfiguration = new ConfigurationOptions
            { 
                EndPoints = { { "localhost", 90 } },
                AbortOnConnectFail = false
            };

            WebApiLinks.GenresApi = configuration["GenresApi"];
            WebApiLinks.ChaptersApi = configuration["ChaptersApi"];

            var authority = configuration["IdentityServer:Authority"];

            builder.Services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Audience = "booksapi";
                    options.Authority = authority;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };

                    options.RequireHttpsMetadata = false;
                });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Books.Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "books.admin");
                });
                options.AddPolicy("Books.Client", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "books.client");
                });
                options.AddPolicy("Comments.Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "comments.Admin");
                });
                options.AddPolicy("Comments.Client", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "comments.client");
                });
                options.AddPolicy("Readers.Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "readers.admin");
                });
                options.AddPolicy("Readers.Client", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "readers.client");
                });
                options.AddPolicy("Genres.Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "genres.admin");
                });
                options.AddPolicy("Genres.Client", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "genres.client");
                });
                options.AddPolicy("BookGenres.Admin", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "bookgenres.admin");
                });
                options.AddPolicy("BookGenres.Client", policy =>
                {
                    policy.RequireAuthenticatedUser();
                    policy.RequireClaim("scope", "bookgenres.client");
                });
            });

            builder.Services.AddAutoMapper(typeof(Program));

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IGenresRepository, GenresRepository>();
            builder.Services.AddScoped<IGenresService, GenresService>();
            builder.Services.AddScoped<IBookGenresRepository, BookGenresRepository>();
            builder.Services.AddScoped<IBookGenresService, BookGenresService>();
            builder.Services.AddScoped<IBooksRepository, BooksRepository>();
            builder.Services.AddScoped<IBooksService, BooksService>();

            builder.Services.AddScoped<ICommentsRepository, CommentsRepository>();
            builder.Services.AddScoped<ICommentsService, CommentsService>();

            builder.Services.AddScoped<IChapterService, ChaptersService>();

            builder.Services.AddScoped<IReadersRepository, ReadersRepository>();
            builder.Services.AddScoped<IReadersService, ReadersService>();

            builder.Services.AddScoped<IValidator<Comments>, CommentsValidator>();

            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseNpgsql(builder.Configuration.GetConnectionString("ConnectionString")));

            builder.Services.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(redisConfiguration));

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
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "Books.API", Version = "v1" });

                options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.OAuth2,
                    Flows = new OpenApiOAuthFlows()
                    {
                        Implicit = new OpenApiOAuthFlow()
                        {
                            AuthorizationUrl = new Uri($"{authority}/connect/authorize"),
                            TokenUrl = new Uri($"{authority}/connect/token"),
                            Scopes = new Dictionary<string, string>
                            {
                                { "books.admin", "Admin Books API" },
                                { "books.client", "Client Books API" },
                                { "comments.admin", "Admin Comments API" },
                                { "comments.client", "Client Comments API" },
                                { "readers.admin", "Admin Readers API" },
                                { "readers.client", "Client Readers API" },
                                { "genres.admin", "Admin Genres API"},
                                { "genres.client", "Client Genres API"},                                
                                { "bookgenres.admin", "Admin BookGenres API"},
                                { "bookgenres.client", "Client BookGenres API"},
                                { "nickname", "Client Nickname" },
                                { "role", "Client Role" },
                                { "photoUrl", "Client Profile Photo Url" },
                            }
                        }
                    }
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme()
                        {
                            Reference = new OpenApiReference()
                            {
                                Id = "oauth2",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new[]
                        {
                            "books.admin", "books.client", 
                            "comments.admin", "comments.client", 
                            "readers.admin", "readers.client",
                            "genres.admin", "genres.client",
                            "bookgenres.admin", "bookgenres.client",
                            "nickname", "role",
                            "photoUrl"
                        }
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
                    setup.SwaggerEndpoint($"{configuration["PathBase"]}/swagger/v1/swagger.json", "Books.API v1");
                    setup.OAuthClientId("booksswaggerui");
                    setup.OAuthAppName("Books Swagger UI");
                });
            }

            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseHttpsRedirection();
            app.UseCors("CorsPolicy");

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