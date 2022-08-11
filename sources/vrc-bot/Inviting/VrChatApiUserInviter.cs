using VRChat.API.Api;
using VRChat.API.Client;
using VRChat.API.Model;
using VrChatBouncerBot.UI;

namespace VrChatBouncerBot.Inviting
{
    internal class VrChatApiUserInviter : IUserInviter
    {
        private readonly IInviteApi API;
        private readonly IConsoleContent CONTENT;

        public VrChatApiUserInviter(IInviteApi api, IConsoleContentManager manager)
        {
            API = api ?? throw new ArgumentNullException(nameof(api));
            CONTENT = manager.Register<VrChatApiUserInviter>();
        }

        public async Task<bool> TryApproveRequestedInviteAsync(Notification notification, CancellationToken token = default)
        {
            try
            {
                var result = await API.RespondInviteAsync(notification.Id, null, token);
                return true;
            }
            catch (ApiException e)
            {
                CONTENT.Set($"Failed to respond to user invitation request: {e.Message}");
                return false;
            }
        }
    }
}
