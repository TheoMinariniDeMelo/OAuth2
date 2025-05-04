using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace OAuth2
{
    public class GitHubMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private static string RedirectUrl { get; set; } = "/login";
        private readonly string _userAgent;

        public GitHubMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, string userAgent)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
            _userAgent = userAgent;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            context.Request.Cookies.TryGetValue("github_code", out var code);
            context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader);

            string accessToken = authorizationHeader.ToString().Replace("Bearer", "").Trim();

            if (code == null && accessToken == null)
            {
                context.Response.Redirect(RedirectUrl);
                return; 
            }

            if (accessToken != null)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    client.BaseAddress = new Uri("https://api.github.com/user");
                    client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                    client.DefaultRequestHeaders.Add("Accept", "application/json");
                    client.DefaultRequestHeaders.Add("User-Agent", _userAgent);

                    var response = await client.GetAsync("");

                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        var user = JsonSerializer.Deserialize<GitHubUser>(await response.Content.ReadAsStringAsync());
                        if (user != null)
                        {
                            context.Items.Add("github_user", user);
                            context.Items.Add("github_access_token", accessToken);
                        }
                    }
                    else if (response.StatusCode == HttpStatusCode.Unauthorized)
                    {
                        context.Response.Redirect(RedirectUrl);
                        return;
                    }
                }
                catch (Exception ex)
                {
                    context.Response.Redirect(RedirectUrl);
                    return; 
                }
            }

            await _next(context); // Chama o próximo middleware
        }
    }
}
