using System;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace czip
{
    public class ConsoleEventArgs : EventArgs
    {
        public ConsoleEventArgs(string msg) { Message = msg; }
        public string Message { get; }
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
        public static Stopwatch stopwatch;
        public static bool AgreeToPrompts;
        public static bool Verbose;
        public static bool Prints = true;
        public static event EventHandler<MessageEventArgs> MessageEvent;
        public static event EventHandler<InfoEventArgs> InfoEvent;
        public static event EventHandler<WarningEventArgs> WarningEvent;
        public static event EventHandler<ErrorEventArgs> ErrorEvent;

        private static void RaiseMessageEvent(MessageEventArgs e)
        {
            MessageEvent?.Invoke(typeof(ConsoleUtil), e);
        }

        private static void RaiseInfoEvent(InfoEventArgs e)
        {
            InfoEvent?.Invoke(typeof(ConsoleUtil), e);
        }

        private static void RaiseWarningEvent(WarningEventArgs e)
        {
            WarningEvent?.Invoke(typeof(ConsoleUtil), e);
        }

        private static void RaiseErrorEvent(ErrorEventArgs e)
        {
            ErrorEvent?.Invoke(typeof(ConsoleUtil), e);
        }

        public static string ReadLine()
        {
            stopwatch.Stop();
            string s = Console.ReadLine();
            stopwatch.Start();
            return s;
        }

        public static bool PromptYN(string q)
        {
            if (AgreeToPrompts) return true;
            Console.Write(q);
            Console.Write(" [y/n]: ");
            return ReadLine().ToLower() == "y";
        }

        //public static string FilteredPrompt(string msg, char[] filter)
        //{
        //    Console.Write(msg);
        //    StringBuilder res = new StringBuilder();
        //    ConsoleKeyInfo c = new ConsoleKeyInfo();
        //    stopwatch.Stop();
        //    while (c.Key != ConsoleKey.Enter) {
        //        c = Console.ReadKey(true);
        //        if (c.Key == ConsoleKey.Backspace && res.Length > 0)
        //        {
        //            Console.Write("\b \b");
        //            res.Remove(res.Length-1, 1);
        //            continue;
        //        }
        //        if (filter.Contains(c.KeyChar)) continue;
        //        Console.Write(c.KeyChar);
        //        res.Append(c.KeyChar);
        //    }
        //    Console.WriteLine();
        //    stopwatch.Start();
        //    return res.ToString();
        //}

        public static void PrintMessage(string msg)
        {
            RaiseMessageEvent(new MessageEventArgs(msg));
            Console.WriteLine(msg);
        }

        public static void PrintInfo(string msg)
        {
            if (!Verbose) return;
            RaiseInfoEvent(new InfoEventArgs(msg));
            if (Prints)
            {
                ConsoleColor tempStore = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine(msg);
                Console.ForegroundColor = tempStore;
            }
        }

        public static void PrintWarning(string msg)
        {
            RaiseWarningEvent(new WarningEventArgs(msg));
            if (Prints)
            {
                ConsoleColor tempStore = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(msg);
                Console.ForegroundColor = tempStore;
            }
        }

        public static void PrintError(string msg)
        {
            RaiseErrorEvent(new ErrorEventArgs(msg));
            if (Prints)
            {
                ConsoleColor tempStore = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(msg);
                Console.ForegroundColor = tempStore;
            }
        }
    }
}
