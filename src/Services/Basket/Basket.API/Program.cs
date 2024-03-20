
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;

var builder = WebApplication.CreateBuilder(args);
//Add services to the container
var assembly = typeof(Program).Assembly;
builder.Services.AddMediatR(configuration =>
{
    configuration.RegisterServicesFromAssemblies(assembly);
    configuration.AddOpenBehavior(typeof(ValidationBehavior<,>));
    configuration.AddOpenBehavior(typeof(LoggingBehavior<,>));
});
builder.Services.AddValidatorsFromAssembly(assembly);
builder.Services.AddCarter();
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
    opts.Schema.For<ShoppingCart>().Identity(x => x.UserName);
}).UseLightweightSessions();
builder.Services.AddScoped<IBasketRepository, BasketRepository>();
builder.Services.Decorate<IBasketRepository, CachedBasketRepository>();
builder.Services.AddStackExchangeRedisCache(options =>
{
    options.Configuration = builder.Configuration.GetConnectionString("Redis");
    //options.InstanceName = "Basket";
});

builder.Services.AddExceptionHandler<CustomExceptionHandler>();
builder.Services.AddHealthChecks()
    .AddNpgSql(builder.Configuration.GetConnectionString("Database")!)
    .AddRedis(builder.Configuration.GetConnectionString("Redis")!);


var app = builder.Build();

//Configure the http request pipeline
app.MapCarter();
app.UseExceptionHandler(opt => { });
app.UseHealthChecks("/health",
    new HealthCheckOptions
    {
        ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
    });

app.Run();
