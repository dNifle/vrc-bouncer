using Microsoft.Extensions.Hosting;
using VRChat.API.Api;
using VrChatBouncerBot.AutoMode;
using VrChatBouncerBot.Fetching;
using VrChatBouncerBot.Inviting;

namespace VrChatBouncerBot.UI
{
    internal class UIWorker : IHostedService
    {
        private const int RefreshRateInMilliseconds = 200;
        private const int PollingRateInMilliseconds = 20;
        private readonly Task UI_TASK;
        private readonly Task INPUT_TASK;
        private readonly CancellationTokenSource TOKEN_SOURCE = new CancellationTokenSource();
        private readonly InviteHandlerOptions INVITE_OPTIONS;
        private readonly IInviteHandler INVITE_HANDLER;
        private readonly IHostApplicationLifetime LIFETIME;
        private readonly IConsoleContentManager CONTENT_MANAGER;
        private readonly IAuthenticationApi AUTH_API;
        private readonly IUserFetcherController FETCHER_CONTROLLER;
        private readonly IAutoModeWorker AUTO_WORKER;

        private bool _automaticMode = false;


        public UIWorker(
            InviteHandlerOptions inviteOptions, 
            IInviteHandler inviteHandler,
            IHostApplicationLifetime hostApplicationLifetime,
            IConsoleContentManager consoleContentManager,
            IAuthenticationApi authApi,
            IUserFetcherController fetcherController,
            IAutoModeWorker autoWorker)
        {
            CONTENT_MANAGER = consoleContentManager ?? throw new ArgumentNullException(nameof(consoleContentManager));
            INVITE_OPTIONS = inviteOptions ?? throw new ArgumentNullException(nameof(inviteOptions));
            INVITE_HANDLER = inviteHandler ?? throw new ArgumentNullException(nameof(inviteHandler));
            LIFETIME = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
            UI_TASK = new Task(UILoop);
            INPUT_TASK = new Task(InputLoop);
            AUTH_API = authApi ?? throw new ArgumentNullException(nameof(authApi));
            FETCHER_CONTROLLER = fetcherController ?? throw new ArgumentNullException(nameof(fetcherController));
            AUTO_WORKER = autoWorker ?? throw new ArgumentNullException(nameof(autoWorker));
        }

        private async void UILoop()
        {
            Console.Clear();
            while (!TOKEN_SOURCE.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, 0);
                CONTENT_MANAGER.DrawContent();
                Console.ForegroundColor = ConsoleColor.White;
                if (!_automaticMode)
                {
                    CONTENT_MANAGER.WriteLine($"Controls:");
                    CONTENT_MANAGER.WriteLine($"  Press escape to exit");
                    CONTENT_MANAGER.WriteLine($"  Press spacebar to let {INVITE_OPTIONS.UserInviteBatchSize} players in.");
                    CONTENT_MANAGER.WriteLine($"  Press a to automatically let players in when there is enough room.");
                    CONTENT_MANAGER.WriteLine($"  Press x to stop/start fetching player invitation requests.");
                }
                else
                {
                    CONTENT_MANAGER.WriteLine($"Press any key to stop automatic invite mode.");
                }
                await Task.Delay(RefreshRateInMilliseconds);
            }
        }

        private async void InputLoop()
        {
            while (!TOKEN_SOURCE.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    if (_automaticMode)
                    {
                        _automaticMode = false;
                        AUTO_WORKER.Stop();
                    }
                    else
                    {
                        ConsoleKeyInfo key = Console.ReadKey();

                        if (key.Key == ConsoleKey.Spacebar)
                        {
                            await INVITE_HANDLER.InviteNextUserBatchAsync();
                        }
                        else if (key.Key == ConsoleKey.Escape)
                        {
                            LIFETIME.StopApplication();
                        }
                        else if (key.Key == ConsoleKey.X)
                        {
                            if (FETCHER_CONTROLLER.IsRunning)
                            {
                                FETCHER_CONTROLLER.Stop();
                            }
                            else
                            {
                                FETCHER_CONTROLLER.Start();
                            }
                        }
                        else if (key.Key == ConsoleKey.A)
                        {
                            _automaticMode = true;
                            AUTO_WORKER.Start();
                        }

                        await Task.Delay(1000);
                    }
                }

                await Task.Delay(PollingRateInMilliseconds);
            }
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            UI_TASK.Start();
            INPUT_TASK.Start();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            TOKEN_SOURCE.Cancel();
            UI_TASK.Wait();
            INPUT_TASK.Wait();
            AUTH_API.Logout();
            return Task.CompletedTask;
        }
    }
}
