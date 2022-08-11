using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using VRChat.API.Api;

var builder = new HostBuilder();
builder.ConfigureDefaults(args);
builder.ConfigureAppConfiguration(config => {
    config.AddJsonFile("appsettings.local.json");
});
builder.ConfigureServices((context, services) =>
{
    services.AddVrChatBotServices(context.Configuration);
});

var app = builder.Build();

try
{
    app.Run();
}
finally
{
    app.Dispose();
}

Console.WriteLine("test");