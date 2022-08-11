using VRChat.API.Api;
using VRChat.API.Client;
using VrChatBouncerBot.UI;

namespace VrChatBouncerBot.Fetching
{
    internal class VrChatApiUserFetcher : IUserFetcher
    {
        private readonly IEntryQueue QUEUE;
        private readonly INotificationsApi API;
        private readonly IConsoleContent CONTENT;

        public VrChatApiUserFetcher(IEntryQueue queue, INotificationsApi api, IConsoleContentManager manager)
        {
            QUEUE = queue ?? throw new ArgumentNullException(nameof(queue));
            API = api ?? throw new ArgumentNullException(nameof(api));
            CONTENT = manager.Register<VrChatApiUserFetcher>();
        }

        public async Task<FetchResult> FetchInviteRequestingUsersAsync(CancellationToken token = default)
        {
            CONTENT.Set("Fetching invite requests...");
            try
            {
                var notifications = await API.GetNotificationsAsync("requestInvite", false, false, null, 60, 0, token);
                CONTENT.AddLine($"Received {notifications.Count} invite request notifications.");

                int addedUsers = 0;

                foreach (var entry in notifications)
                {
                    if (QUEUE.TryEnqueueIfUnknown(entry))
                        addedUsers++;

                    API.MarkNotificationAsRead(entry.Id);
                }

                
                return new FetchResult(notifications.Count, addedUsers);
            }
            catch (ApiException e)
            {
                CONTENT.AddLine($"Failed to respond to user invitation request: {e.Message}");
                return new FetchResult();
            }
        }
    }
}
