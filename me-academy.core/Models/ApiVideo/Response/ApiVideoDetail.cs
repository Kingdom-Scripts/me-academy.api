namespace me_academy.core.Models.ApiVideo.Response;

public class ApiVideoDetail
{
    public ApiVideoAsset Assets { get; set; } = null!;
}

public class ApiVideoAsset
{
    public string Player { get; set; } = null!;
}