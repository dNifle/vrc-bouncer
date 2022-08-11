namespace VrChatBouncerBot.UI
{
    internal class ConsoleContentManager : IConsoleContentManager
    {
        private readonly List<ConsoleContent> CONTENT = new List<ConsoleContent>();
        public IConsoleContent Register<T>()
        {
            var content = new ConsoleContent(typeof(T).Name);
            CONTENT.Add(content);
            return content;
        }

        public void DrawContent()
        {
            Console.ForegroundColor = ConsoleColor.Gray;
            foreach (ConsoleContent content in CONTENT)
            {
                WriteLine($"-{content.Title}-");
                WriteLine(content.Data);
            }
        }

        public void WriteLine(string text)
        {
            Console.WriteLine(text + new String(' ', Console.BufferWidth - text.Length));
        }
    }
}
