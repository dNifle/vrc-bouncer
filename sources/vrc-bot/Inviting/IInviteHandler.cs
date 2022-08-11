namespace VrChatBouncerBot.Inviting
{
    internal interface IInviteHandler
    {
        Task InviteNextUserBatchAsync();
    }
}