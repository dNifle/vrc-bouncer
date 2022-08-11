using VRChat.API.Api;

namespace VrChatBouncerBot.Client
{
    internal interface IApiClientFactory
    {
        IAuthenticationApi CreateAuthClient();
        IInviteApi CreateInviteClient();
        INotificationsApi CreateNotificationClient();
        IWorldsApi CreateWorldClient();
    }
}