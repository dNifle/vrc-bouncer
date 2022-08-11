using Microsoft.Extensions.Hosting;

namespace VrChatBouncerBot.Fetching
{
    internal class UserFetchWorker : IHostedService
    {
        private readonly Task FETCHING_TASK;
        private readonly UserFetcherOptions OPTIONS;
        private readonly IUserFetcher FETCHER;
        private readonly CancellationTokenSource TOKEN_SOURCE = new CancellationTokenSource();
        private readonly IUserFetcherState STATE;

        public UserFetchWorker(IUserFetcher fetcher, UserFetcherOptions options, IUserFetcherState state)
        {
            FETCHER = fetcher ?? throw new ArgumentNullException(nameof(fetcher));
            OPTIONS = options ?? throw new ArgumentNullException(nameof(options));
            FETCHING_TASK = new Task(Fetch);
            STATE = state ?? throw new ArgumentNullException(nameof(state));
        }

        private async void Fetch()
        {
            while (!TOKEN_SOURCE.IsCancellationRequested)
            {
                if(STATE.IsRunning)
                    await FETCHER.FetchInviteRequestingUsersAsync(TOKEN_SOURCE.Token);

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(OPTIONS.FetchFrequencyInSeconds), TOKEN_SOURCE.Token);
                }
                catch { }
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            FETCHING_TASK.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TOKEN_SOURCE.Cancel();
            FETCHING_TASK.Wait();
            TOKEN_SOURCE.Dispose();
            return Task.CompletedTask;
        }
    }
}
