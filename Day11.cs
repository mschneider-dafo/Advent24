using AdventOfCodeLibrary;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent24;

enum Action
{
   Split,
   Multiply,
   Replace,
}
internal static class Day11
{
   public static string Solve()
   {
      var content = File.ReadAllText("Day11.txt");

      //content = @"125 17";

      List<(long, long)> numbers = [];

      foreach (var item in content.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries))
      {
         numbers.Add((1, long.Parse(item)));
      }

      for (int i = 0; i < 75; i++)
      {
         Action[] toDo = GetActions(numbers);

         numbers = ApplyActions(numbers, toDo);

         numbers = Consolidate(numbers);
      }


      return $"Count: {numbers.Sum(x=> x.Item1)}";
   }

   private static List<(long, long)> Consolidate(List<(long, long)> numbers)
   {
      return numbers.GroupBy(x => x.Item2).Select(x=> (x.Sum(x=> x.Item1), x.Key)).ToList();
   }

   private static List<(long, long)> ApplyActions(List<(long, long)> numbers, Action[] toDo)
   {
      Debug.Assert(numbers.Count == toDo.Length);

      List<(long, long)> ints = new(numbers.Count * 2);
      for (int i = 0; i < numbers.Count; i++)
      {
         Action action = toDo[i];
         var number = numbers[i];
         if (action == Action.Replace)
         {
            ints.Add((number.Item1, 1));
         }
         else if (action == Action.Split)
         {
            ints.AddRange(SplitNumber(number));
         }
         else
         {
            ints.Add((number.Item1, number.Item2 * 2024));
         }
      }

      return ints;
   }

   private static (long, long)[] SplitNumber((long, long) number)
   {
      var digits = Std.GetNumberOfDigits(number.Item2);

      var numb = number.Item2;
      long first = 0;
      long second = 0;
      for (int i = 0; i < digits / 2; i++)
      {
         first += (numb % 10) * (long)Math.Pow(10, i);
         numb /= 10;
      }
      int c = 0;
      while (numb != 0)
      {
         second += (numb % 10) * (int)Math.Pow(10, c);
         numb /= 10;
         c++;
      }

      return [(number.Item1, second), (number.Item1, first)];
   }

   private static Action[] GetActions(List<(long, long)> numbers)
   {
      Action[] actions = new Action[numbers.Count];

      for (int i = 0; i < numbers.Count; i++)
      {
         if (numbers[i].Item2 == 0)
         {
            actions[i] = Action.Replace;
         }
         else if (Std.GetNumberOfDigits(numbers[i].Item2) % 2 == 0)
         {
            actions[i] = Action.Split;
         }
         else
         {
            actions[i] = Action.Multiply;
         }
      }

      return actions;
   }
}

