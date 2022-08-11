namespace VrChatBouncerBot.UI
{
    internal interface IConsoleContentManager
    {
        void WriteLine(string text);
        void DrawContent();
        IConsoleContent Register<T>();
    }
}