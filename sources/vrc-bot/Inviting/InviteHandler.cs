using VrChatBouncerBot.UI;

namespace VrChatBouncerBot.Inviting
{
    internal class InviteHandler : IInviteHandler
    {
        private readonly IEntryQueue QUEUE;
        private readonly IUserInviter INVITER;
        private readonly IConsoleContent CONTENT;
        private readonly InviteHandlerOptions OPTIONS;

        public InviteHandler(IEntryQueue queue, IUserInviter inviter, IConsoleContentManager manager, InviteHandlerOptions options)
        {
            QUEUE = queue ?? throw new ArgumentNullException(nameof(queue));
            INVITER = inviter ?? throw new ArgumentNullException(nameof(inviter));
            CONTENT = manager.Register<InviteHandler>();
            OPTIONS = options ?? throw new ArgumentNullException(nameof(options));
        }

        public async Task InviteNextUserBatchAsync()
        {
            CONTENT.Set("Starting to invite users.");
            var notifications = QUEUE.DequeueJoinRequestNotifications(OPTIONS.UserInviteBatchSize);

            int count = 0;

            foreach (var notification in notifications)
            {
                if (await INVITER.TryApproveRequestedInviteAsync(notification))
                {
                    count++;
                }
                else
                {
                    // If the invite failed, the user should be able to queue again.
                    QUEUE.RemoveNotificationFromKnown(notification);
                }
            }

            CONTENT.AddLine($"Invited {count} users.");
        }
    }
}
