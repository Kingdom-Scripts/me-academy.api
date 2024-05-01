using LazyCache;
using me_academy.core.Constants;
using me_academy.core.Models.Configurations;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Serilog;
using System.Text;

namespace me_academy.core.Utilities
{
    public static class InitializeApiVideoToken
    {
        public static async Task InitializeToken(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var cacheService = serviceScope.ServiceProvider.GetService<IAppCache>();
                if (cacheService == null) throw new ArgumentNullException(nameof(cacheService));

                var httpClientFactory = serviceScope.ServiceProvider.GetService<IHttpClientFactory>();
                if (httpClientFactory == null) throw new ArgumentNullException(nameof(httpClientFactory));

                var httpClient = httpClientFactory.CreateClient(HttpClientKeys.ApiVideo);

                var appConfig = serviceScope.ServiceProvider.GetService<IOptions<AppConfig>>();
                var apiVideoKey = appConfig.Value.ApiVideoKey;

                var request = new { apiKey = apiVideoKey };
                var content = new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync("auth/api-key", content);

                if (!response.IsSuccessStatusCode)
                {
                    Log.Error("--> Could not get Api.Video Token");
                    return;
                }

                string resStri = await response.Content.ReadAsStringAsync();
                dynamic tokenObj = JsonConvert.DeserializeObject<dynamic>(resStri);
                string token = tokenObj!.access_token;
                string refreshToken = tokenObj.refresh_token;

                cacheService.Remove(AuthKeys.ApiVideoToken);
                cacheService.Remove(AuthKeys.ApiVideoRefreshToken);
                cacheService.Add(AuthKeys.ApiVideoToken, token, DateTime.UtcNow.AddSeconds(3590));
                cacheService.Add(AuthKeys.ApiVideoRefreshToken, refreshToken, DateTime.UtcNow.AddYears(20));

                Log.Information("--> Api.Video Token initialized.");
            }
        }
    }
}