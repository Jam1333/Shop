using Carter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using Shop.Api.Abstractions.Authentication;
using Shop.Api.Domain.Constants;
using Shop.Api.Domain.Entities;
using Shop.Api.Infrastructure.Authentication;
using Shop.Api.Infrastructure.Data;
using Shop.Api.Infrastructure.Extensions;
using Shop.Api.Middlewares;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();

builder.Services.AddDbContext<ApplicationDbContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services
    .AddAuthentication(opt =>
    {
        opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters.ValidIssuer = builder.Configuration["Jwt:Issuer"];
        opt.TokenValidationParameters.ValidAudience = builder.Configuration["Jwt:Audience"];
        opt.TokenValidationParameters.IssuerSigningKey =
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!));
    });
builder.Services.AddAuthorization();

builder.Services.AddSingleton<IJwtProvider, JwtProvider>();

builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});

builder.Services.AddCarter();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await app.ApplyMigrationsAsync();
}

app.UseMiddleware<JwtMiddleware>();

app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/ping", () => "pong");
app.MapGet("/data", [Authorize(Roles = Roles.Admin)] () => "data");

app.MapCarter();

app.Run();
