namespace DOTOAuth2;

public class GitHubUserAccess
{
    public string Code { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
    public string TokenType { get; set; }
    public string ExpiresIn { get; set; }
    public string IdToken { get; set; }

    public GitHubUserAccess(
        string Code,
        string AccessToken,
        string RefreshToken,
        string TokenType,
        string ExpiresIn,
        string IdToken
    )
    {
        this.Code = Code;
        this.AccessToken = AccessToken;
        this.RefreshToken = RefreshToken;
        this.TokenType = TokenType;
        this.ExpiresIn = ExpiresIn;
        this.IdToken = IdToken;
    }
};