﻿using AdventOfCodeLibrary;
using System.Diagnostics;
using System.Globalization;

namespace Advent24
{
   internal class Program
   {
      static async Task Main(string[] args)
      {
         if (!int.TryParse(args[0], CultureInfo.InvariantCulture, out int day))
         {
            Console.WriteLine("Not a valid day");
            return;
         }
         string sessionToken = "";

         if (args.Length == 2)
         {
            sessionToken = args[1];
         }
         else
         {
            sessionToken = File.ReadAllText("Secrets.txt").Trim();
         }

         if (!File.Exists(string.Format(CultureInfo.InvariantCulture, "Day{0}.txt", day.ToString("D2"))))
         {
            var fContent = await Downloader.DownloadInput(2024, day, sessionToken);

            File.WriteAllText($"Day{day:D2}.txt", fContent);
         }

         var tStamp = Stopwatch.GetTimestamp();
         string result = day switch
         {
         1 => Day1.Solve(),
         2 => Day2.Solve(),
         3 => Day3.Solve(),
         4 => Day4.Solve(),
         5 => Day5.Solve(),
         6 => Day6.Solve(),
         7 => Day7.Solve(),
         8 => Day8.Solve(),
         9 => Day9.Solve(),
         10 => Day10.Solve(),
         11 => Day11.Solve(),
         12 => Day12.Solve(),
         13 => Day13.Solve(),
         14 => Day14.Solve(),
         15 => Day15.Solve(),
         16 => Day16.Solve(),
         17 => Day17.Solve(),
         18 => Day18.Solve(),
         19 => Day19.Solve(),
            _ => "Day not implemented",
         };

         Console.WriteLine(result);
         Console.WriteLine($"Time: {(Stopwatch.GetTimestamp() - tStamp) / (double)Stopwatch.Frequency * 1000} ms");
      }
   }
}
