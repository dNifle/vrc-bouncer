namespace VrChatBouncerBot.AutoMode
{
    internal interface IInstanceCapacityChecker
    {
        Task<int> GetOpenSlotsForCurrentInstanceAsync();
    }
}