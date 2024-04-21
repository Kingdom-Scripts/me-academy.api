namespace me_academy.core.Models.Configurations;

public class AppConfig
{
    public string TinifyKey { get; set; }
    public string ApiVideoKey { get; set; }
    public FileSettings FileSettings { get; set; }
    public BaseURLs BaseURLs { get; set; }
}

public class BaseURLs
{
    public string AdminClient { get; set; }
    public string Client { get; set; }
    public string ApiVideo { get; set; }
}