namespace VrChatBouncerBot.Fetching
{
    internal interface IUserFetcherController
    {
        bool IsRunning { get; }

        void Start();
        void Stop();
    }
}