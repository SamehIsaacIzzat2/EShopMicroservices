var builder = WebApplication.CreateBuilder(args);

//Add services to the container
builder.Services.AddCarter();
builder.Services.AddMediatR(configuration=>
{
    configuration.RegisterServicesFromAssemblies(typeof(Program).Assembly);
});
builder.Services.AddMarten(opts =>
{
    opts.Connection(builder.Configuration.GetConnectionString("Database")!);
}).UseLightweightSessions();

var app = builder.Build();

//Configure the http request pipeline
app.MapCarter();

app.Run();
