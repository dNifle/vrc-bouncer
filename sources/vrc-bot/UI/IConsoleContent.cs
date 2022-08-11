namespace VrChatBouncerBot.UI
{
    internal interface IConsoleContent
    {
        string Data { get; }

        void Set(string data);
        void AddLine(string data);
    }
}