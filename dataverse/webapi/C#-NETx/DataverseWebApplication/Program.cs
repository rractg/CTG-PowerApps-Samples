using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using DataverseWebApplication;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

// Get configuration data about the Web API set in wwwroot/appsettings.json
var CDSWebApiConfig = builder.Configuration.GetSection("CDSWebAPI");
var resourceUrl = CDSWebApiConfig.GetSection("ResourceUrl").Value;
var version = CDSWebApiConfig.GetSection("Version").Value;
var timeoutSeconds = int.Parse(CDSWebApiConfig.GetSection("TimeoutSeconds").Value);

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    // Add access to Dataverse to the scope of the access token when the user signs in
    options.ProviderOptions.DefaultAccessTokenScopes.Add($"{resourceUrl}/user_impersonation");
});

// Create an named definition of an HttpClient that can be created in a component page
builder.Services.AddHttpClient("CDSClient", client =>
{
    // See https://learn.microsoft.com/powerapps/developer/data-platform/webapi/compose-http-requests-handle-errors
    client.BaseAddress = new Uri($"{resourceUrl}/api/data/{version}/");
    client.Timeout = TimeSpan.FromSeconds(timeoutSeconds);
    client.DefaultRequestHeaders.Add("OData-Version", "4.0");
    client.DefaultRequestHeaders.Add("OData-MaxVersion", "4.0");
});

await builder.Build().RunAsync();
