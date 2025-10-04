using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using XiletradeAuth;
using XiletradeAuth.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<WeatherService>();

var navManager = builder.Services.BuildServiceProvider().GetRequiredService<NavigationManager>();
var baseUri = new Uri(navManager.BaseUri);
string currentHost = baseUri.Host;
//var environment = hostname.Contains("localhost") ? "Development" : "Production";
var auth0Section = currentHost.Contains("localhost") 
    ? builder.Configuration.GetSection("Auth0localhost") : builder.Configuration.GetSection("Auth0");

builder.Services.AddOidcAuthentication(options =>
{
    auth0Section.Bind(options.ProviderOptions);
    //options.ProviderOptions.AdditionalProviderParameters.Add("prompt", "none");
    options.ProviderOptions.ResponseType = "code"; // PKCE flow
    options.ProviderOptions.DefaultScopes.Add("openid");
    options.ProviderOptions.DefaultScopes.Add("profile");
    options.ProviderOptions.DefaultScopes.Add("email");

    options.AuthenticationPaths.LogOutCallbackPath = "/authentication/logout-callback";
    options.AuthenticationPaths.LogOutFailedPath = "/authentication/logout-failed";

    // override the end session endpoint
    /* .NET 10
    options.ProviderOptions.MetadataSeed = new Dictionary<string, object>
    {
        ["end_session_endpoint"] = "https://xiletrade.eu.auth0.com/v2/logout"
    };
    */
});

await builder.Build().RunAsync();
