using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   public static class Day18
   {

      public static string Solve()
      {
         ReadOnlySpan<char> content = File.ReadAllText("Day18.txt");
         /*
         content = @"5,4
4,2
4,5
3,0
2,1
6,3
2,4
1,5
0,6
3,3
2,6
5,1
1,2
5,5
2,5
6,5
1,4
0,4
6,4
1,1
6,1
1,0
0,5
1,6
2,0";
         */
         Span<Range> ranges = stackalloc Range[6000];

         Span<Range> intern = stackalloc Range[2];
         var x = content.Split(ranges, '\n', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);

         int[][] lines = new int[x][];
         for (int i = 0; i < lines.Length; i++)
         {
            var span = content[ranges[i]];
            lines[i] = new int[2];
            span.Split(intern, ',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            lines[i][0] = int.Parse(span[intern[0]], NumberFormatInfo.InvariantInfo);
            lines[i][1] = int.Parse(span[intern[1]], NumberFormatInfo.InvariantInfo);
         }

         var target = (70, 70);

         int minIllegal = lines.Length;
         int maxLegal = 1023;

         int currentCount = ((minIllegal - maxLegal) / 2) + maxLegal;

         Queue<(int x, int y)> stack = new();
         (int, int) coords = (0, 0);
         Dictionary<(int x, int y), int> dict = new();
         while (maxLegal < lines.Length)
         {
            bool[][] map = SetupMap(lines, currentCount);


            stack.Clear();
            stack.Enqueue((0, 0));
            dict.Clear();
            dict.Add((0, 0), 0);



            int steps = int.MaxValue;

            while (stack.Count > 0)
            {
               var current = stack.Dequeue();

               if (!dict.TryGetValue(current, out int currentSteps))
               {
                  throw new Exception("Should not happen");
               }

               if (currentSteps > steps)
               {
                  continue;
               }

               if (current == target)
               {
                  steps = currentSteps;
                  continue;
               }

               foreach ((int y, int x) dir in new[] { (0, 1), (0, -1), (1, 0), (-1, 0) })
               {
                  (int x, int y) next = (current.x + dir.x, current.y + dir.y);

                  if (next.x < 0 || next.y < 0 || next.x >= 71 || next.y >= 71)
                  {
                     continue;
                  }

                  if (map[next.y][next.x])
                  {
                     continue;
                  }

                  if (dict.TryGetValue(next, out int nextSteps) && nextSteps <= currentSteps + 1)
                  {
                     continue;
                  }



                  dict[next] = currentSteps + 1;
                  stack.Enqueue(next);
               }
            }

            if (steps == int.MaxValue)
            {
               if (minIllegal > currentCount)
               {
                  minIllegal = currentCount;
                  coords = (lines[currentCount - 1][0], lines[currentCount - 1][1]);
               }

               currentCount = ((minIllegal - maxLegal) / 2) + maxLegal;
            }
            else
            {
               maxLegal = currentCount;
               currentCount = ((minIllegal - maxLegal) / 2) + maxLegal;
            }

            if (minIllegal - maxLegal == 1)
               break;
         }

         return string.Format(CultureInfo.InvariantCulture, "Coordinates Part2: {0},{1}", coords.Item1, coords.Item2);
      }

      private static bool[][] SetupMap(int[][] lines, int count)
      {
         bool[][] map = new bool[71][];
         for (int i = 0; i < map.Length; i++)
         {
            map[i] = new bool[71];
         }

         for (int i = 0; i < count; i++)
         {
            var l = lines[i];
            map[l[1]][l[0]] = true;
         }

         return map;
      }

      private static int Distance((int y, int x) a, (int y, int x) b)
      {
         return Math.Abs(a.y - b.y) + Math.Abs(a.x - b.x);
      }
   }
}
