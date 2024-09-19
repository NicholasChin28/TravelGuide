using Microsoft.Playwright;
using RabbitMQ.Client;
using TravelGuide.Services;
using TravelGuide.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(Program).Assembly));

builder.Services.AddScoped<IAirbnbScraperService, AirbnbScraperService>();
builder.Services.AddSingleton<IRabbitMQService, RabbitMQService>();

builder.Services.AddSingleton<IPlaywright>(serviceProvider =>
{
    return Playwright.CreateAsync().GetAwaiter().GetResult();
});

builder.Services.AddSingleton<IBrowserContext>(serviceProvider => 
{
    var playwright = serviceProvider.GetRequiredService<IPlaywright>();
    var browser = playwright.Firefox.LaunchAsync().GetAwaiter().GetResult();
    return browser.NewContextAsync().GetAwaiter().GetResult();
});

builder.Services.AddSingleton<IConnectionFactory>(sp =>
    new ConnectionFactory() { HostName = "amqps://miwvnspp:0lJcDuGUtVvy7UQB7n8kayOuL_gvU3IY@armadillo.rmq.cloudamqp.com/miwvnspp" });

builder.Services.AddHostedService<ScrapingBackgroundService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
