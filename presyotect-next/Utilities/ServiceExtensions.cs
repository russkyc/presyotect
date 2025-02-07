using System.Text;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using Presyotect.Features.Authentication;

namespace Presyotect.Utilities;

public static class ServiceExtensions
{
    public static void AddCoreServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCors();
        builder.Services.AddSwaggerGen(
            options =>
            {
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description =
                        "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer"
                });
                options.AddSecurityDefinition("ApiKey", new OpenApiSecurityScheme
                {
                    Description = "Api Key header using the ApiKey scheme. Example: \"x-api-key: {api-key}\"",
                    Name = "x-api-key",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });
            });
    }

    public static void AddAuthServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(
                authenticationOptions =>
                {
                    authenticationOptions.DefaultScheme = "JwtOrApiKey";
                    authenticationOptions.DefaultAuthenticateScheme = "JwtOrApiKey";
                })
            .AddJwtBearer(
                bearerOptions =>
                {
                    bearerOptions.Audience = builder.Configuration["JWT:Audience"];
                    bearerOptions.Authority = builder.Configuration["JWT:Audience"];
                    bearerOptions.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = true,
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        ValidateAudience = true,
                        IssuerSigningKey =
                            new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["JWT:Key"]!))
                    };
                    bearerOptions.Configuration = new();
                })
            .AddScheme<AuthenticationSchemeOptions, ApiKeyAuthenticator>(
                ApiKeyAuthenticator.AuthenticationScheme,
                null)
            .AddPolicyScheme(
                "JwtOrApiKey",
                "JwtOrApiKey",
                schemeOptions =>
                {
                    schemeOptions.ForwardDefaultSelector = context =>
                    {
                        if (context.Request.Headers.TryGetValue(
                                HeaderNames.Authorization,
                                out var authHeader) &&
                            authHeader.FirstOrDefault()?.StartsWith("Bearer ") is true)
                        {
                            return JwtBearerDefaults.AuthenticationScheme;
                        }

                        return ApiKeyAuthenticator.AuthenticationScheme;
                    };
                });
        builder.Services.AddAuthorization();
    }
}