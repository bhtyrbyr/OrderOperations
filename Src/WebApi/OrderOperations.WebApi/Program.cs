using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Localization;
using OrderOperations.Application;
using OrderOperations.Persistence;
using OrderOperations.Security;
using OrderOperations.WebApi.Middlewares;
using OrderOperations.WebApi.Services;
using System.Globalization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<ILoggerService, ConsoleLogger>();
builder.Services.AddLocalization(options => options.ResourcesPath = "Resources");
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "en", "tr" }; // Add more cultures as needed
    options.DefaultRequestCulture = new RequestCulture("tr");
    options.SupportedCultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();
    options.SupportedUICultures = supportedCultures.Select(c => new CultureInfo(c)).ToList();

    // Set the culture based on the URL segment
    options.RequestCultureProviders = new List<IRequestCultureProvider>
        {
    new AcceptLanguageHeaderRequestCultureProvider()
        };
});

// Add services to the container.
var dbConnectionString = builder.Configuration.GetConnectionString("PostgreSql");

if(string.IsNullOrEmpty(dbConnectionString))
    throw new InvalidOperationException("Database connection string is not configured.");

builder.Services
    .AddApplicationServices()
    .AddPersistenceServices(dbConnectionString)
    .AddSecurityServices(builder.Configuration);


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
    opt.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        BearerFormat = "JWT",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Enter JWT token like this: Bearer {your token}"
    });

    opt.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(10);
    options.SlidingExpiration = false;
    options.Events.OnRedirectToLogin = context =>
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        return Task.CompletedTask;
    };
});

builder.Services.AddCors(opt =>
{
    opt.AddPolicy("AllCors", opts =>
    {
        opts.AllowAnyOrigin()
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseCors("AllCors");

app.UseRequestLocalization();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCustomExceptionMiddle();

app.MapControllers();

app.Run();

