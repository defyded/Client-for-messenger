using Client_For_Messenger.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using MudBlazor.Services;
using Web_Client_for_Messenger;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

string apiBaseAddress = "https://localhost:7007/"; // ЗАМЕНИТЕ НА ВАШ URL

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(apiBaseAddress)
});
builder.Services.AddMudServices();
// 2. Регистрируем ваши сервисы (не забудьте добавить using)
builder.Services.AddScoped<TokenStore>();
builder.Services.AddScoped<ApiService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<UserService>();


await builder.Build().RunAsync();
