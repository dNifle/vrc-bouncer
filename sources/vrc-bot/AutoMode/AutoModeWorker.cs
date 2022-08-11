using VrChatBouncerBot.Inviting;
using VrChatBouncerBot.UI;

namespace VrChatBouncerBot.AutoMode
{
    internal class AutoModeWorker : IAutoModeWorker
    {
        private const int CheckFrequencyInMilliseconds = 10000;
        private readonly IInstanceCapacityChecker CAPACITY_CHECKER;
        private readonly IInviteHandler INVITE_HANDLER;
        private readonly InviteHandlerOptions OPTIONS;
        private Task checkTask = null!;
        private CancellationTokenSource tokenSource = null!;
        private readonly IConsoleContent CONTENT;

        private bool _running = false;

        public AutoModeWorker(IInstanceCapacityChecker capacityChecker, IInviteHandler inviteHandler, InviteHandlerOptions options, IConsoleContentManager contentManager)
        {
            CAPACITY_CHECKER = capacityChecker ?? throw new ArgumentNullException(nameof(capacityChecker));
            INVITE_HANDLER = inviteHandler ?? throw new ArgumentNullException(nameof(inviteHandler));
            OPTIONS = options ?? throw new ArgumentNullException(nameof(options));
            CONTENT = contentManager.Register<AutoModeWorker>();
        }

        private async void CheckAndInviteLoopAsync()
        {
            while (_running)
            {
                CONTENT.Set("Automode is running.");
                int openSlots = await CAPACITY_CHECKER.GetOpenSlotsForCurrentInstanceAsync();
                CONTENT.AddLine($"There are {openSlots} open slots. Inviting players...");

                if (openSlots <= OPTIONS.UserInviteBatchSize)
                    await INVITE_HANDLER.InviteNextUserBatchAsync();

                try
                {
                    await Task.Delay(CheckFrequencyInMilliseconds, tokenSource.Token);
                }
                catch { }
            }
        }

        public void Start()
        {
            if (_running)
                return;

            _running = true;
            tokenSource = new CancellationTokenSource();
            checkTask = new Task(CheckAndInviteLoopAsync);
            checkTask.Start();
        }

        public void Stop()
        {
            if (!_running)
                return;

            tokenSource.Cancel();
            _running = false;
            CONTENT.Set("Automode is stopped.");
        }
    }
}
