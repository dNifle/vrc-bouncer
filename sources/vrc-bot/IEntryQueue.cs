using VRChat.API.Model;

namespace VrChatBouncerBot
{
    internal interface IEntryQueue
    {
        IEnumerable<Notification> DequeueJoinRequestNotifications(int count);
        void RemoveNotificationFromKnown(Notification notification);
        bool TryEnqueueIfUnknown(Notification notification);
    }
}