namespace OAuth2;

public class GitHubMiddlewareOptions
{
    public string UserAgent { get; set; }
    public string RedirectUrl { get; set; }
    public string LoginUrl { get; set; }
    public string LogoutUrl { get; set; }
    public string RegisterUrl { get; set; }
}