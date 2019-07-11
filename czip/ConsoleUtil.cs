using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace czip
{
    public class ConsoleEventArgs : EventArgs
    {
        public ConsoleEventArgs(string msg) { message = msg; }
        private string message;
        public string Message { get => message; }
    }

    public class MessageEventArgs : ConsoleEventArgs
    {
        public MessageEventArgs(string msg) : base(msg) { }
    }

    public class InfoEventArgs : ConsoleEventArgs
    {
        public InfoEventArgs(string msg) : base(msg) { }
    }

    public class WarningEventArgs : ConsoleEventArgs
    {
        public WarningEventArgs(string msg) : base(msg) { }
    }

    public class ErrorEventArgs : ConsoleEventArgs
    {
        public ErrorEventArgs(string msg) : base(msg) { }
    }

    public static class ConsoleUtil
    {
        public static bool AgreeToPrompts;
        public static bool Verbose;
        public static event EventHandler<MessageEventArgs> RaiseMessageEvent;
        public static event EventHandler<InfoEventArgs> RaiseInfoEvent;
        public static event EventHandler<WarningEventArgs> RaiseWarningEvent;
        public static event EventHandler<ErrorEventArgs> RaiseErrorEvent;

        private static void OnMessageEvent(MessageEventArgs e)
        {
            EventHandler<MessageEventArgs> handler = RaiseMessageEvent;
            if (handler != null) handler(typeof(ConsoleUtil), e);
        }

        private static void OnInfoEvent(InfoEventArgs e)
        {
            EventHandler<InfoEventArgs> handler = RaiseInfoEvent;
            if (handler != null) handler(typeof(ConsoleUtil), e);
        }

        private static void OnWarningEvent(WarningEventArgs e)
        {
            EventHandler<WarningEventArgs> handler = RaiseWarningEvent;
            if (handler != null) handler(typeof(ConsoleUtil), e);
        }

        private static void OnErrorEvent(ErrorEventArgs e)
        {
            EventHandler<ErrorEventArgs> handler = RaiseErrorEvent;
            if (handler != null) handler(typeof(ConsoleUtil), e);
        }

        public static bool PromptYN(string q)
        {
            if (AgreeToPrompts) return true;
            Console.Write(q);
            Console.Write(" [y/n]: ");
            if (Console.ReadLine().ToString().ToLower() == "y") return true;
            return false;
        }

        public static void PrintMessage(string msg)
        {
            OnMessageEvent(new MessageEventArgs(msg));
            Console.WriteLine(msg);
        }

        public static void PrintInfo(string msg)
        {
            if (!Verbose) return;
            ConsoleColor tempStore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            OnInfoEvent(new InfoEventArgs(msg));
            Console.WriteLine(msg);
            Console.ForegroundColor = tempStore;
        }

        public static void PrintWarning(string msg)
        {
            ConsoleColor tempStore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            OnWarningEvent(new WarningEventArgs(msg));
            Console.WriteLine(msg);
            Console.ForegroundColor = tempStore;
        }

        public static void PrintError(string msg)
        {
            ConsoleColor tempStore = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.DarkRed;
            OnErrorEvent(new ErrorEventArgs(msg));
            Console.WriteLine(msg);
            Console.ForegroundColor = tempStore;
        }
    }
}
