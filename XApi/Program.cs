using DAO.DBConnection;
using DAO.DBConnection.SqlServer;
using DTO.API.Auth;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Globalization;
using Useful.Service;
using XApi.ExceptionFilter;

var allowDeveloperOrigins = "_myAllowSpecificOrigins";
var config = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json")
    .AddJsonFile($"appsettings.{(EnvironmentService.Get() == EnvironmentService.Dev ? $"{Environments.Development}." : "")}json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables()
    .Build();

var appSettings = new XDataDatabaseSettings();
new ConfigureFromConfigurationOptions<XDataDatabaseSettings>(config.GetSection("XDataDatabaseSettings")).Configure(appSettings);

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<XDataDatabaseSettings>(config.GetSection(nameof(XDataDatabaseSettings)));
builder.Services.AddSingleton(sp => sp.GetRequiredService<IOptions<XDataDatabaseSettings>>().Value);

builder.Services.AddControllers(options => options.Filters.Add(new HttpResponseExceptionFilter(appSettings)));
ConfigureJsonSerializer();
ConfigureLocalizationService(builder.Services);
ConfigureAuthService(builder.Services);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "XApi", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
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
                        new List<string>()
                    }
    });
    c.TagActionsBy(api =>
    {
        if (api.GroupName != null)
            return new[] { api.GroupName };

        var controllerActionDescriptor = api.ActionDescriptor as ControllerActionDescriptor;
        if (controllerActionDescriptor != null)
            return new[] { controllerActionDescriptor.ControllerName };

        throw new InvalidOperationException("Unable to determine tag for endpoint.");
    });
    c.DocInclusionPredicate((name, api) => true);
});

builder.Services.AddCors(policy =>
{
    policy.AddPolicy(allowDeveloperOrigins, builders =>
    {
        builders.WithOrigins("http://localhost:4200")
            .AllowAnyMethod().AllowAnyHeader();
    });
});


var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();

app.UseSwagger();

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "XApi v1"));

app.UseCors(options => options.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

if (app.Environment.IsDevelopment())
    app.UseCors(allowDeveloperOrigins);

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.Run();

/// <summary>
/// https://medium.com/@renato.groffe/asp-net-core-2-0-autentica%C3%A7%C3%A3o-em-apis-utilizando-jwt-json-web-tokens-4b1871efd
/// </summary>
void ConfigureAuthService(IServiceCollection services)
{
    var signingConfigurations = new SigningConfigurations();
    services.AddSingleton(signingConfigurations);

    var tokenConfigurations = new TokenConfigurations();
    services.Configure<TokenConfigurations>(con => config.GetSection("TokenConfigurations").Bind(con));
    new ConfigureFromConfigurationOptions<TokenConfigurations>(config.GetSection("TokenConfigurations")).Configure(tokenConfigurations);
    services.AddSingleton(tokenConfigurations);

    services.AddAuthentication(authOptions =>
    {
        authOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        authOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(bearerOptions =>
    {
        var paramsValidation = bearerOptions.TokenValidationParameters;
        paramsValidation.IssuerSigningKey = signingConfigurations.Key;
        paramsValidation.ValidAudience = tokenConfigurations.Audience;
        paramsValidation.ValidIssuer = tokenConfigurations.Issuer;

        // Valida a assinatura de um token recebido
        paramsValidation.ValidateIssuerSigningKey = true;

        // Verifica se um token recebido ainda � v�lido
        paramsValidation.ValidateLifetime = true;

        // Tempo de toler�ncia para a expira��o de um token (utilizado
        // caso haja problemas de sincronismo de hor�rio entre diferentes
        // computadores envolvidos no processo de comunica��o)
        paramsValidation.ClockSkew = TimeSpan.Zero;
    });

    // Ativa o uso do token como forma de autorizar o acesso
    // a recursos deste projeto
    services.AddAuthorization(auth =>
    {
        auth.AddPolicy(Policies.Bearer, new AuthorizationPolicyBuilder()
            .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
            .RequireAuthenticatedUser().Build());
        auth.AddPolicy(Policies.AppUser, policy => policy.RequireClaim(AuthTypeData.UserId));
    });
}

void ConfigureLocalizationService(IServiceCollection services) => services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[]
    {
                new CultureInfo("pt-BR"),
    };

    // State what the default culture for your application is. This will be used if no specific culture
    // can be determined for a given request.
    options.DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR");

    // You must explicitly state which cultures your application supports.
    // These are the cultures the app supports for formatting numbers, dates, etc.
    options.SupportedCultures = supportedCultures;

    // These are the cultures the app supports for UI strings, i.e. we have localized resources for.
    options.SupportedUICultures = supportedCultures;
});

void ConfigureJsonSerializer() => JsonConvert.DefaultSettings = () => new JsonSerializerSettings
{
    Formatting = Formatting.Indented,
    ContractResolver = new CamelCasePropertyNamesContractResolver(),
    Culture = new CultureInfo("pt-br"),
    DateTimeZoneHandling = DateTimeZoneHandling.Local,
    MissingMemberHandling = MissingMemberHandling.Ignore,
    NullValueHandling = NullValueHandling.Ignore,
    DefaultValueHandling = DefaultValueHandling.Populate
};