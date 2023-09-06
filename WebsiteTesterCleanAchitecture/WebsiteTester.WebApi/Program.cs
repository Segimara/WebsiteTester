using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Interfaces;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebsiteTester.Infrastructure;
using WebsiteTester.WebApi.TokenValidators;

namespace WebsiteTester.WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.RegisterServices();

            builder.Services.RegisterDbContext("Server=localhost\\SQLEXPRESS;Database=WebsiteTesterDB;Trust Server Certificate=True;Encrypt=True;User Id=sa;Password=123qwe123qwe;");

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                AddSwaggerOAuth2Configuration(c);
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "wellship_svc_app", Version = "v1" });
            });

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            })
            .AddJwtBearer(o =>
            {
                o.IncludeErrorDetails = true;
                o.SecurityTokenValidators.Clear();
                o.SecurityTokenValidators.Add(new GoogleTokenValidator());
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDeveloperExceptionPage();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { { "redirect_url", "https://localhost:44324/swagger/oauth2-redirect.html" } });
                c.OAuthClientId("1063110563568-vr83cpjlcfpsbmvofr6doch2p1oiqor1.apps.googleusercontent.com");
                c.OAuthClientSecret("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");
                c.InjectJavascript("swaggerGoogleAuth.js");
            });

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors("AllowAll");

            app.MapControllers();

            app.Run();
        }

        private static void AddSwaggerOAuth2Configuration(SwaggerGenOptions swaggerGenOptions)
        {

            var securityScheme = new OpenApiSecurityScheme
            {
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows()
                {
                    Implicit = new OpenApiOAuthFlow()
                    {
                        AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                        Scopes = new Dictionary<string, string> { { "email", "email" }, { "profile", "profile" } }
                    }
                },
                Extensions = new Dictionary<string, IOpenApiExtension>
                {
                    {"x-tokenName", new OpenApiString("id_token")}
                },
            };

            swaggerGenOptions.AddSecurityDefinition("Bearer", securityScheme);

            var securityRequirements = new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string> {"email", "profile"}
                }
            };

            swaggerGenOptions.AddSecurityRequirement(securityRequirements);
        }
    }
}