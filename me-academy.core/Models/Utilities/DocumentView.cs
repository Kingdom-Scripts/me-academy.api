using System.Text.Json.Serialization;
using me_academy.core.Models.App.Constants;

namespace me_academy.core.Models.Utilities;

public class DocumentView
{
    public int Id { get; set; }

    public required string Name { get; set; }

    public required string Type { get; set; }

    public required string Url { get; set; }

    public required string ThumbnailUrl { get; set; }
}