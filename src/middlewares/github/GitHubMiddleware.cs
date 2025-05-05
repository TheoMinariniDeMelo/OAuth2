using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace OAuth2
{
    public class GitHubMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly GitHubMiddlewareOptions _options;
        private readonly string _userAgent;

        public GitHubMiddleware(RequestDelegate next, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _next = next;
            _httpClientFactory = httpClientFactory;
            _userAgent = configuration["UserAgent"];
            _options = configuration.GetSection("GitHubMiddleware").Get<GitHubMiddlewareOptions>();
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Verifica o código ou token de acesso
            if (IsPathExcluded(context.Request.Path.Value))
            {
                await _next(context);
                return;
            }

            context.Request.Cookies.TryGetValue("github_code", out var code);
            context.Request.Headers.TryGetValue("Authorization", out var authorizationHeader);

            string accessToken = authorizationHeader.ToString().Replace("Bearer", "").Trim();

            if (string.IsNullOrEmpty(code) && string.IsNullOrEmpty(accessToken))
            {
                context.Response.Redirect(_options.RedirectUrl);
                return;
            }

            if (!string.IsNullOrEmpty(accessToken))
            {
                await HandleGitHubAuthAsync(context, accessToken);
            }

            await _next(context);
        }

        private bool IsPathExcluded(string path)
        {
            return path.Contains(_options.LoginUrl) || path.Contains(_options.LogoutUrl) || path.Contains(_options.RegisterUrl);
        }

        private async Task HandleGitHubAuthAsync(HttpContext context, string accessToken)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.BaseAddress = new Uri("https://api.github.com/user");
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessToken}");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                client.DefaultRequestHeaders.Add("User-Agent", _options.UserAgent);

                var response = await client.GetAsync("");

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var user = JsonSerializer.Deserialize<GitHubUser>(await response.Content.ReadAsStringAsync());
                    if (user != null)
                    {
                        context.Items["github_user"] = user;
                        context.Items["github_access_token"] = accessToken;
                    }
                }
                else if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    context.Response.Redirect(_options.RedirectUrl);
                }
            }
            catch (Exception)
            {
                context.Response.Redirect(_options.RedirectUrl);
            }
        }
    }

   
}
