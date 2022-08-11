namespace VrChatBouncerBot.Fetching
{
    internal interface IUserFetcher
    {
        Task<FetchResult> FetchInviteRequestingUsersAsync(CancellationToken token = default);
    }
}