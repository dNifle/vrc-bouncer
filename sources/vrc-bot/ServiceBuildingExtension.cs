using Microsoft.Extensions.Configuration;
using VRChat.API.Api;
using VrChatBouncerBot;
using VrChatBouncerBot.AutoMode;
using VrChatBouncerBot.Client;
using VrChatBouncerBot.Fetching;
using VrChatBouncerBot.Inviting;
using VrChatBouncerBot.UI;

namespace Microsoft.Extensions.DependencyInjection
{
    internal static class ServiceBuildingExtension
    {
        public static IServiceCollection AddVrChatBotServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddLogging();

            services.AddSingleton(configuration.GetRequiredSection("UserFetcher").Get<UserFetcherOptions>());
            services.AddTransient<IUserFetcher, VrChatApiUserFetcher>();

            services.AddSingleton<IEntryQueue, EntryQueue>();

            services.AddTransient<IUserInviter, VrChatApiUserInviter>();

            services.AddSingleton(configuration.GetRequiredSection("InviteHandler").Get<InviteHandlerOptions>());
            services.AddTransient<IInviteHandler, InviteHandler>();

            services.AddHostedService<UserFetchWorker>();

            services.AddSingleton(configuration.GetRequiredSection("ApiClient").Get<ApiClientOptions>());
            services.AddSingleton<IApiClientFactory, ApiClientFactory>();

            services.AddHostedService<UIWorker>();

            services.AddTransient<IAuthenticationApi>(services => services.GetRequiredService<IApiClientFactory>().CreateAuthClient());
            services.AddTransient<INotificationsApi>(services => services.GetRequiredService<IApiClientFactory>().CreateNotificationClient());
            services.AddTransient<IInviteApi>(services => services.GetRequiredService<IApiClientFactory>().CreateInviteClient());
            services.AddTransient<IWorldsApi>(services => services.GetRequiredService<IApiClientFactory>().CreateWorldClient());
            services.AddTransient<IUsersApi>(services => services.GetRequiredService<IApiClientFactory>().CreatUsersClient());

            services.AddSingleton<IConsoleContentManager, ConsoleContentManager>();

            services.AddSingleton<UserFetcherController>();
            services.AddSingleton<IUserFetcherController>(s => s.GetRequiredService<UserFetcherController>());
            services.AddSingleton<IUserFetcherState>(s => s.GetRequiredService<UserFetcherController>());
            services.AddTransient<IInstanceCapacityChecker, InstanceCapacityChecker>();
            services.AddSingleton<IAutoModeWorker, AutoModeWorker>();

            return services;
        }
    }
}
