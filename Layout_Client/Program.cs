using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

using Layout_Client;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Layout_Client.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient 
                       { BaseAddress = new Uri("http://localhost:5176/") });
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthHttpClientFactory>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<RestaurantService>();
builder.Services.AddScoped<MenuItemService>();
builder.Services.AddScoped<UploadService>();
builder.Services.AddScoped<PromoCodeService>();
builder.Services.AddScoped<ReviewService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<PaymentService>();

await builder.Build().RunAsync();
