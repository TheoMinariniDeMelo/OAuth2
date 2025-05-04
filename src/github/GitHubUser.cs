namespace OAuth2;
public class GitHubUser
{
    public string Login { get; set; }
    public long Id { get; set; }
    public string Node_Id { get; set; }
    public string Avatar_Url { get; set; }
    public string Url { get; set; }
    public string Html_Url { get; set; }
    public string Name { get; set; }
    public string? Company { get; set; }
    public string Blog { get; set; }
    public string Location { get; set; }
    public string? Email { get; set; }
    public string Bio { get; set; }
    public int Public_Repos { get; set; }
    public int Followers { get; set; }
    public int Following { get; set; }
    public DateTime Created_At { get; set; }
    public DateTime Updated_At { get; set; }
}
