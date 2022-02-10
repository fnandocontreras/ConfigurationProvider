using ConfigurationProvider;
using ConfigurationProvider.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Shared;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IFeatureRepository, FeatureRepository>();
builder.Services.AddMemoryCache();

builder.Services.AddSignalR();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/featuretoggle/{appName}", ([FromRoute] string appName, IFeatureRepository repository)
    => repository.GetFeatureToggles(appName))
;


app.MapPost("/featuretoggle/{appName}",
    async (
        [FromBody] IEnumerable<FeatureToggle> featureToggles,
        [FromRoute] string appName,
        IFeatureRepository repository,
        IHubContext<NotificationHub, INotificationHub> hub) =>
    {
        repository.SaveFeatureToggles(appName, featureToggles);
        await hub.Clients.Group(appName)
            .ConfigurationChanged("A new configuration version exist");
    });


app.MapHub<NotificationHub>("notificationshub");

await app.RunAsync();