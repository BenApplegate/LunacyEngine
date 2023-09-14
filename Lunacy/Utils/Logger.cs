using System.Diagnostics;
using System.Reflection;

namespace Lunacy.Utils;

public class Logger
{
    
#pragma warning disable CS8620
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
        MethodBase currentMethod = new StackTrace().GetFrame(1).GetMethod();
        if (currentMethod != null)
            MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Cyan, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", $"[{currentMethod.ReflectedType?.Name}]", "[INFO]: ", contents});
        else
            MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Cyan, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", "[MissingClass::MissingMethod]", "[INFO]: ", contents});
        
    }
    
    public static void Error(string? contents = "")
    {
        MethodBase currentMethod = new StackTrace().GetFrame(1).GetMethod();
        if (currentMethod != null)
            MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Red, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", $"[{currentMethod.ReflectedType?.Name}]", "[ERROR]: ", contents});
        else
            MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Red, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", "[MissingClass::MissingMethod]", "[ERROR]: ", contents});
        
    }
    
    public static void Warning(string? contents = "")
    {
        MethodBase currentMethod = new StackTrace().GetFrame(1).GetMethod();
        if (currentMethod != null)
            MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Yellow, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", $"[{currentMethod.ReflectedType?.Name}]", "[WARNING]: ", contents});
        else
            MessageBuilder(new[] {ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.Yellow, ConsoleColor.White}, new[] {$"[{DateTime.Now.ToString()}]", "[MissingClass::MissingMethod]", "[WARNING]: ", contents});
        
    }
#pragma warning restore CS8620
}