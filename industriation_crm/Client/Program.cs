using Blazored.LocalStorage;
using Bwasm.Cookie.Handler;
using Bwasm.Cookie.Providers;
using industriation_crm.Client;
using industriation_crm.Client.Handlers;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.SignalR.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");


builder.Services.AddScoped<CookieHandler>();
builder.Services.AddOptions();
builder.Services.AddAuthorizationCore();
builder.Services.AddBlazoredLocalStorage();
builder.Services
.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddSingleton<HubConnection>(sp =>
{
    var navigationManager = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
      .WithUrl(navigationManager.ToAbsoluteUri("/statushub"))
      .WithAutomaticReconnect()
      .Build();
});

//Возможно поменять при продакшене
//builder.Services.AddHttpClient("API", options => {
//    options.BaseAddress = new Uri("https://localhost:7244/");
//})
//.AddHttpMessageHandler<CookieHandler>();


builder.Services.AddScoped(sp => new HttpClient(new AddHeadersDelegatingHandler())
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

await builder.Build().RunAsync();
