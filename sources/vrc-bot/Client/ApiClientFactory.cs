using VRChat.API.Api;
using VRChat.API.Client;
using VrChatBouncerBot.UI;

namespace VrChatBouncerBot.Client
{
    internal class ApiClientFactory : IApiClientFactory
    {
        private readonly AuthenticationApi API;
        private readonly IConsoleContent CONTENT;
        private bool _loggedIn = false;

        public ApiClientFactory(ApiClientOptions options, IConsoleContentManager manager)
        {
            CONTENT = manager.Register<ApiClientFactory>();
            ArgumentNullException.ThrowIfNull(options);

            Configuration config = new Configuration();
            config.BasePath = options.BasePath;
            // Configure API key authorization: authCookie
            config.AddApiKey("auth", options.ApiKey);
            // Configure HTTP basic authorization: authHeader
            config.Username = options.Username;
            config.Password = options.Password;
            // Configure API key authorization: twoFactorAuthCookie
            config.AddApiKey("twoFactorAuth", options.ApiKey);
            API = new AuthenticationApi(config);
        }

        public IAuthenticationApi CreateAuthClient()
        {
            if (!_loggedIn) LogIn();
            return new AuthenticationApi(API.Client, API.AsynchronousClient, API.Configuration);
        }

        public IInviteApi CreateInviteClient()
        {
            if (!_loggedIn) LogIn();
            return new InviteApi(API.Client, API.AsynchronousClient, API.Configuration);
        }

        public INotificationsApi CreateNotificationClient()
        {
            if (!_loggedIn) LogIn();
            return new NotificationsApi(API.Client, API.AsynchronousClient, API.Configuration);
        }

        public IWorldsApi CreateWorldClient()
        {
            if (!_loggedIn) LogIn();
            return new WorldsApi(API.Client, API.AsynchronousClient, API.Configuration);
        }

        public IUsersApi CreatUsersClient()
        {
            if (!_loggedIn) LogIn();
            return new UsersApi(API.Client, API.AsynchronousClient, API.Configuration);
        }

        public IInstancesApi CreatInstanceClient()
        {
            if (!_loggedIn) LogIn();
            return new InstancesApi(API.Client, API.AsynchronousClient, API.Configuration);
        }

        private void LogIn()
        {
            API.GetCurrentUser();
            CONTENT.Set( "Successfully logged in.");
            _loggedIn = true;
        }
    }
}
