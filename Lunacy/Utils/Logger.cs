using System.Diagnostics;
using System.Reflection;

namespace Lunacy.Utils;

public static class Logger
{
    
#pragma warning disable CS8620
    
    private static Queue<(int, string, MethodBase)> _messageQueue = new Queue<(int, string, MethodBase)>();
    private static bool shouldClose = false;

    private static void LoggerThread()
    {
        while (!shouldClose || _messageQueue.Count > 0)
        {
            if (_messageQueue.Count > 0)
            {
                (int, string, MethodBase) message;
                lock (_messageQueue)
                {
                    message = _messageQueue.Dequeue();
                }

                if (message.Item1 == 0)
                {
                    if (message.Item3 != null)
                        MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Cyan, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", $"[{message.Item3.ReflectedType?.Name}]", "[INFO]: ", message.Item2});
                    else
                        MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Cyan, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", "[MissingClass::MissingMethod]", "[INFO]: ", message.Item2});
                }

                else if (message.Item1 == 1)
                {
                    if (message.Item3 != null)
                        MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Yellow, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", $"[{message.Item3.ReflectedType?.Name}]", "[WARNING]: ", message.Item2});
                    else
                        MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Yellow, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", "[MissingClass::MissingMethod]", "[WARNING]: ", message.Item2});
                }
                
                else if (message.Item1 == 2)
                {
                    if (message.Item3 != null)
                        MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Red, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", $"[{message.Item3.ReflectedType?.Name}]", "[ERROR]: ", message.Item2});
                    else
                        MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Red, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", "[MissingClass::MissingMethod]", "[ERROR]: ", message.Item2});
                }
            }
        }
    }

    internal static void Initialize()
    {
        new Thread(LoggerThread).Start();
    }

    internal static void Dispose()
    {
        shouldClose = true;
    }

    private static void MessageBuilder(ConsoleColor[] colors, string[] contents)
    {
        for (int i = 0; i < colors.Length; i++)
        {
            Console.ForegroundColor = colors[i];
            Console.Write(contents[i]);
        }
        Console.Write("\n");
    }

    public static void Info(string? contents = "")
    {
        lock (_messageQueue)
        {
            _messageQueue.Enqueue((0, contents, new StackTrace().GetFrame(1).GetMethod()));
        }
        
    }
    
    public static void Error(string? contents = "")
    {
        lock (_messageQueue)
        {
            _messageQueue.Enqueue((2, contents, new StackTrace().GetFrame(1).GetMethod()));
        }
        
    }
    
    public static void Warning(string? contents = "")
    {
        lock (_messageQueue)
        {
            _messageQueue.Enqueue((1, contents, new StackTrace().GetFrame(1).GetMethod()));
        }
    }
#pragma warning restore CS8620
}