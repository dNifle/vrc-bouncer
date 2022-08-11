using VRChat.API.Model;

namespace VrChatBouncerBot.Inviting
{
    internal interface IUserInviter
    {
        Task<bool> TryApproveRequestedInviteAsync(Notification notification, CancellationToken token = default);
    }
}