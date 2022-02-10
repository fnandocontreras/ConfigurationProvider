using ClientWebApi;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .AddJsonFile("appSettings.json")
    .AddJsonFile("featureToggles.json",
        optional: true, 
        reloadOnChange: true);

builder.Services.Configure<FeatureToggles>(
    builder.Configuration.GetSection("FeatureToggles"));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHostedService<NotificationsHubClient>();
builder.Services.AddHttpClient(nameof(NotificationsHubClient), (svcProvider, client) => 
        client.BaseAddress = new Uri(svcProvider
            .GetRequiredService<IConfiguration>()
            .GetValue<string>("ConfigurationServiceUri"))
);

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/series/{customerId}", (
    [FromRoute]int customerId,
    IOptionsSnapshot<FeatureToggles> featureToggles) =>
{
    int n = 10;
    var toggles = GetCustomerToggles(customerId, featureToggles.Value.Toggles);
    return new
    {
        Fibonacci = toggles.Contains(nameof(Series.Fibonacci)) ? Series.Fibonacci(n) : null,
        Factorial = toggles.Contains(nameof(Series.Factorial)) ? Series.Factorial(n) : null,
        Random = toggles.Contains(nameof(Series.Random)) ? Series.Random(n) : null
    };
});


IEnumerable<string> GetCustomerToggles(int customerId, IEnumerable<FeatureToggle> toggles)
{
    return toggles?
        .Where(t => t.CustomerIds.Contains(customerId))
        .Select(t => t.Name) ?? Enumerable.Empty<string>();
}


await app.RunAsync();


