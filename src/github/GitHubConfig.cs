namespace OAuth2;

public class GitHubConfig
{
    public string ClientId { get; set; }
    public string ClientSecret { get; set; }
    public string RedirectUri { get; set; }
    public string Scope { get; set; }
    public string State { get; set; }
    public bool AllowSignup { get; set; }

    public GitHubConfig(
        string clientId,
        string clientSecret,
        string redirectUri,
        string scope = "user:email",
        string state = "",
        bool allowSignup = true
    )
    {
        ClientId = clientId;
        ClientSecret = clientSecret;
        RedirectUri = redirectUri;
        Scope = scope;
        State = state;
        AllowSignup = allowSignup;
    }
}
