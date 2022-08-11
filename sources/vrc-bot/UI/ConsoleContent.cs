using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VrChatBouncerBot.UI
{

    internal class ConsoleContent: IConsoleContent
    {
        public ConsoleContent(string title)
        {
            Title = title;
        }

        public string Title { get; }
        public string Data { get; set; } = string.Empty;

        public void AddLine(string data)
        {
            Data += "\n" + data;
        }

        public void Set(string data)
        {
            Data = data;
        }
    }
}
