using Microsoft.AspNetCore.Localization;
using OrderOperations.Application;
using OrderOperations.Persistence;
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

builder.Services
    .AddApplicationServices(builder.Configuration)
    .AddPersistenceServices(dbConnectionString);


builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


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
