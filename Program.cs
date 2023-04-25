using System.Diagnostics;
using System.Text.RegularExpressions;

namespace ShutdownTimer;

public static partial class Program
{
    private const int Depth = 4;

    private static void Main()
    {
        int seconds = 0;

        Regex regex = TimeInput();

        while (seconds < 1)
        {
            string toParse = Console.ReadLine();

            if (toParse == "a")
            {
                Process.Start(Path.Combine(GetParentDirectory(Directory.GetCurrentDirectory(), Depth), @"TimedSleep\bin\Debug\net7.0\TimedSleep.exe"), "a");
                return;
            }

            Match match = regex.Match(toParse);

            if (!match.Success)
            {
                Console.WriteLine("Input string was not in a correct format. Please provide number followed by one of these [smhd]");
                Main();
                return;
            }

            seconds = int.Parse(match.Groups["count"].Value) * (match.Groups["type"].Value switch
            {
                "s" => 1,
                "m" => 60,
                "h" => 3600,
                "d" => 86400,
                "y" => 31536000,
                _ => 0
            });
        }

#if DEBUG
        Process.Start(Path.Combine(GetParentDirectory(Directory.GetCurrentDirectory(), Depth), @"TimedSleep\bin\Debug\net7.0\TimedSleep.exe"), (seconds * 1000).ToString());
#else
        Process.Start(Path.Combine(GetParentDirectory(Directory.GetCurrentDirectory(), 5), @"TimedSleep\bin\Release\net7.0\TimedSleep.exe"), (seconds * 1000).ToString());
#endif
    }

    private static string GetParentDirectory(string currentDir, int depth)
    {
        return depth == 0 ? currentDir : GetParentDirectory(Directory.GetParent(currentDir).FullName, depth - 1);
    }


    [GeneratedRegex(@"^(?<count>\d+)(?<type>[smhdy])$")]
    private static partial Regex TimeInput();
}