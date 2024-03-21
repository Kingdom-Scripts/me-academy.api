using LazyCache;
using me_academy.core.Constants;
using me_academy.core.Interfaces;

namespace me_academy.core.Services;

public class CacheService : ICacheService
{
    private readonly IAppCache _cache;

    public CacheService(IAppCache cache)
    {
        _cache = cache;
    }

    public void AddToken(string key, string token, DateTime expiresAt)
        => _cache.Add($"{AuthKeys.CacheKey}:{key}", token, expiresAt);

    public async Task<string> GetToken(string key)
        => await _cache.GetAsync<string>($"{AuthKeys.CacheKey}:{key}");

    public void RemoveToken(string key)
        => _cache.Remove($"{AuthKeys.CacheKey}:{key}");
}