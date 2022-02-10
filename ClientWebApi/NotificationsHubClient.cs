using Microsoft.AspNetCore.SignalR.Client;
using Shared;
using System.Text.Json;

namespace ClientWebApi
{
    public class NotificationsHubClient : IHostedService
    {
        private readonly HubConnection _connection;
        private readonly string _applicationName;
        private readonly IHttpClientFactory _httpClientFactory;

        public NotificationsHubClient(
            IConfiguration configuration, 
            IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;

            _applicationName = configuration.GetValue<string>("AppName");
            var serviceUri = configuration.GetValue<string>("ConfigurationServiceUri");
            

            _connection = new HubConnectionBuilder()
                .WithUrl($"{serviceUri}/notificationshub?appName={_applicationName}")
                .Build();

            _connection.On<string>("ConfigurationChanged", async (string _)
                => await RetrieveConfigurationAsync());
        }
        public async Task StartAsync(CancellationToken cancellationToken)
        {
            while(true)
            {
                try
                {
                    await _connection.StartAsync(cancellationToken);
                    break;
                }
                catch (Exception)
                {
                    //put some logs here
                }
            }
            
            await RetrieveConfigurationAsync();
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await _connection.StopAsync(cancellationToken);
        }

        private async Task RetrieveConfigurationAsync()
        {
            var httpClient =  _httpClientFactory
                .CreateClient(nameof(NotificationsHubClient));

            var toggles = await httpClient.GetFromJsonAsync<FeatureToggle[]>(
                $"featuretoggle/{_applicationName}");

            var configSerialized = JsonSerializer.Serialize(new
            {
                FeatureToggles =
                new FeatureToggles
                {
                    Toggles = toggles
                }
            });

            await File.WriteAllTextAsync("featureToggles.json", configSerialized);
        }
    }
}
