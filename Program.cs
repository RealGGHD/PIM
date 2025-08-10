using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.HttpOverrides;
using AspNet.Security.OAuth.GitHub;
using Microsoft.AspNetCore.Authentication;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        // ВАЖНО: DefaultChallengeScheme НЕ задаём — будем вызывать провайдера по имени вручную.
    })
    .AddCookie(options =>
    {
        options.LoginPath = "/auth/login";   // будем показывать страницу выбора
        options.LogoutPath = "/auth/logout";
    })
    .AddGoogle(options =>
    {
        options.ClientId = builder.Configuration["Google:ClientId"]!;
        options.ClientSecret = builder.Configuration["Google:ClientSecret"]!;

        // ВАЖНО: маппинг картинки в клейм
        options.ClaimActions.MapJsonKey("urn:google:picture", "picture");
        // опционально:
        // options.ClaimActions.MapJsonKey("urn:google:locale", "locale");

        // (не обязательно, но можно явно указать)
        // options.Scope.Add("profile");
        // options.Scope.Add("email");
    })
    .AddGitHub(options =>
    {
        options.ClientId = builder.Configuration["GitHub:ClientId"]!;
        options.ClientSecret = builder.Configuration["GitHub:ClientSecret"]!;
        options.SaveTokens = true;
        options.Scope.Add("user:email"); // чтобы получить email (primary/verified)
        options.ClaimActions.MapJsonKey("urn:github:login", "login");
        options.ClaimActions.MapJsonKey("urn:github:url", "html_url");
        options.ClaimActions.MapJsonKey("urn:github:avatar", "avatar_url");
    });

var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedProto | ForwardedHeaders.XForwardedFor
});

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
