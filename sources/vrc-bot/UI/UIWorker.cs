using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRChat.API.Api;
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


        public UIWorker(
            InviteHandlerOptions inviteOptions, 
            IInviteHandler inviteHandler,
            IHostApplicationLifetime hostApplicationLifetime,
            IConsoleContentManager consoleContentManager,
            IAuthenticationApi authApi)
        {
            CONTENT_MANAGER = consoleContentManager ?? throw new ArgumentNullException(nameof(consoleContentManager));
            INVITE_OPTIONS = inviteOptions ?? throw new ArgumentNullException(nameof(inviteOptions));
            INVITE_HANDLER = inviteHandler ?? throw new ArgumentNullException(nameof(inviteHandler));
            LIFETIME = hostApplicationLifetime ?? throw new ArgumentNullException(nameof(hostApplicationLifetime));
            UI_TASK = new Task(UILoop);
            INPUT_TASK = new Task(InputLoop);
            AUTH_API = authApi ?? throw new ArgumentNullException(nameof(authApi));
        }

        private async void UILoop()
        {
            Console.Clear();
            while (!TOKEN_SOURCE.IsCancellationRequested)
            {
                Console.SetCursorPosition(0, 0);
                CONTENT_MANAGER.DrawContent();
                Console.ForegroundColor = ConsoleColor.White;
                CONTENT_MANAGER.WriteLine($"Press escape to exit or spacebar to let {INVITE_OPTIONS.UserInviteBatchSize} players in.");
                await Task.Delay(RefreshRateInMilliseconds);
            }
        }

        private async void InputLoop()
        {
            while (!TOKEN_SOURCE.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
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
