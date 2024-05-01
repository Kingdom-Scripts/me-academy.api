using me_academy.core.Constants;
using me_academy.core.Interfaces;
using me_academy.core.Models.ApiVideo.Response;
using me_academy.core.Models.Utilities;
using Newtonsoft.Json;

namespace me_academy.core.Services;

public class VideoService : IVideoService
{
    private readonly HttpClient _client;

    public VideoService(IHttpClientFactory factory)
    {
        if (factory is null) throw new ArgumentNullException(nameof(factory));
        _client = factory.CreateClient(HttpClientKeys.ApiVideo);
    }

    public async Task<Result<ApiVideoToken>> CreateUploadObject()
    {
        var response = await _client.PostAsync("upload-tokens", null);
        if (!response.IsSuccessStatusCode)
            return new ErrorResult<ApiVideoToken>("Failed to get video upload token");

        string content = await response.Content.ReadAsStringAsync();
        var token = JsonConvert.DeserializeObject<ApiVideoToken>(content);

        return new SuccessResult<ApiVideoToken>(token);
    }
}