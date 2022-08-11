using VRChat.API.Model;

namespace VrChatBouncerBot
{
    internal class EntryQueue : IEntryQueue
    {
        private readonly Queue<Notification> ENTRY_QUEUE = new();
        private readonly HashSet<string> KNOWN_USERS = new();
        private readonly object LOCK = new();

        /// <summary>
        /// Uses an invite request notification to queue up a user if that user has not been queued up before.
        /// This means that every user can only be queued up once until the queue is reset.
        /// </summary>
        /// <param name="notification">An invite request notification.</param>
        /// <returns>True if the user was unknown and queued up.</returns>
        public bool TryEnqueueIfUnknown(Notification notification)
        {
            Monitor.Enter(LOCK);

            if (!KNOWN_USERS.Contains(notification.SenderUserId))
            {
                ENTRY_QUEUE.Enqueue(notification);
                KNOWN_USERS.Add(notification.SenderUserId);
                return true;
            }

            Monitor.Exit(LOCK);

            return false;
        }

        public void RemoveNotificationFromKnown(Notification notification)
        {
            Monitor.Enter(LOCK);
            KNOWN_USERS.Remove(notification.SenderUserId);
            Monitor.Exit(LOCK);
        }

        public IEnumerable<Notification> DequeueJoinRequestNotifications(int count)
        {
            Monitor.Enter(LOCK);
            var notifications = new List<Notification>();

            for (int i = 0; i < count; i++)
            {
                if (ENTRY_QUEUE.TryDequeue(out var notification))
                {
                    notifications.Add(notification);
                }
                else
                {
                    break;
                }
            }

            Monitor.Exit(LOCK);

            return notifications;
        }
    }
}
