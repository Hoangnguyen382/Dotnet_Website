using Blazored.LocalStorage;
using Layout_Admin;
using Layout_Admin.Service;
using Microsoft.AspNetCore.Components.Web;  
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.Components;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddSingleton(sp =>
{
    return new HubConnectionBuilder()
        .WithUrl("http://localhost:5176/notificationHub") 
        .WithAutomaticReconnect()
        .Build();
});
builder.Services.AddScoped(sp =>
{
    var navigation = sp.GetRequiredService<NavigationManager>();
    return new HubConnectionBuilder()
        .WithUrl("http://localhost:5176/chatHub", options =>
        {
            options.AccessTokenProvider = async () =>
            {
                var localStorage = sp.GetRequiredService<ILocalStorageService>();
                return await localStorage.GetItemAsync<string>("authToken");
            };
        })
        .WithAutomaticReconnect()
        .Build();
});
builder.Services.AddScoped(sp => new HttpClient 
                       { BaseAddress = new Uri("http://localhost:5176/") });
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthHttpClientFactory>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<MenuItemService>();
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<PromoCodeService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<ComboService>();
builder.Services.AddScoped<ComboDetailService>();
builder.Services.AddSingleton<NotificationService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<ChatHubService>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddAuthorizationCore();

await builder.Build().RunAsync();
