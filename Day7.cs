using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   enum Operation
   {
      Add,
      Multiply,
      Concat,
   }
   internal static class Day7
   {
      public static string Solve()
      {
         var content = File.ReadAllText("Day07.txt");
         /*

         content = @"190: 10 19
3267: 81 40 27
83: 17 5
156: 15 6
7290: 6 8 6 15
161011: 16 10 13
192: 17 8 14
21037: 9 7 18 13
292: 11 6 16 20
";
*/


         var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

         long[] results = lines.Select(x => long.Parse(x.Split(':')[0])).ToArray();

         int[][] segments = lines.Select(x => x.Split(':')[1].Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).ToArray()).ToArray();

         var longestCount = segments.Max(x => x.Length);


         long result = 0;
         for (int i = 0; i < results.Length; i++)
         {
            result += GetResultIfPossible(results[i], segments[i]);
         }

         return $"Sum of valid operations {result}";
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static long Add(long x, int y) => x + y;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static long Multiply(long x, int y) => x * y;

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static long Concat(long x, long y)
      {
         var digits = CountDigits(y);
         var result = x * (long)Math.Pow(10, digits);
         result += y;
         return result;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static long GetResultIfPossible(long v, int[] ints)
      {
         var functions = GetRequiredFunctions(ints.Length - 1);

         ParallelLoopResult parRes = Parallel.For(0, functions.Count, (a, s) =>
         {
            long funcRes = ints[0];
            for (int j = 0; j < functions[a].Length; j++)
            {
               funcRes = functions[a][j] switch
               {
                  Operation.Add => Add(funcRes, ints[j + 1]),
                  Operation.Multiply => Multiply(funcRes, ints[j + 1]),
                  Operation.Concat => Concat(funcRes, ints[j + 1]),
                  _ => throw new ArgumentOutOfRangeException()
               };
               if (funcRes > v)
               {
                  break;
               }
            }
            if (funcRes == v)
            {
               s.Break();
            }
         });

         if(parRes.IsCompleted || parRes.LowestBreakIteration == null)
         {
            return 0;
         }

         return v;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static List<Operation[]> GetRequiredFunctions(int v)
      {
         List<Operation[]> result = new List<Operation[]>(400);
         if (v == 0)
         {
            return [];
         }
         if (v == 1)
         {
            return [[Operation.Add], [Operation.Multiply], [Operation.Concat]];
         }

         var functions = GetRequiredFunctions(v - 1);
         for (int k = 0; k < functions.Count; k++)
         {
            Operation[]? function = functions[k];
            result.Add([.. function, Operation.Add]);
            result.Add([.. function, Operation.Multiply]);
            result.Add([.. function, Operation.Concat]);
         }

         return result;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static int CountDigits(long x)
      {
         int count = 0;
         do
         {
            x /= 10;
            count++;
         }
         while (x != 0);

         return count;
      }
   }
}
