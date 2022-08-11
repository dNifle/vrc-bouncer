namespace VrChatBouncerBot.Fetching
{
    internal class UserFetcherController : IUserFetcherController, IUserFetcherState
    {
        private bool _running = false;
        public bool IsRunning { get { return _running; } }

        public void Start()
        {
            _running = true;
        }

        public void Stop()
        {
            _running = false;
        }


    }
}
