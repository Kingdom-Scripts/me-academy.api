using me_academy.core.Models.Input.Videos;

namespace me_academy.core.Models.ApiVideo.Request;

public class ApiVideoObject
{
    public string title { get; set; } = null!;
    public bool @public { get; set; } = true;
    public bool mp4Support { get; set; } = true;
    public string playerId { get; set; } = null!;
}

public class ApiVideoObjectWithClip : ApiVideoObject
{
    public ApiVideoClipModel clip { get; set; } = new();
}