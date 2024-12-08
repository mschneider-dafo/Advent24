using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   internal static class Day8
   {
      public static string Solve()
      {
         var content = File.ReadAllText("Day08.txt");
        /* content = @"T....#....
...T......
.T....#...
.........#
..#.......
..........
...#......
..........
....#.....
..........";
         */
         char[][] map = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => x.ToCharArray()).ToArray();

         Dictionary<char, List<(int, int)>> antennas = new();

         for (int i = 0; i < map.Length; i++)
         {
            for (int j = 0; j < map[i].Length; j++)
            {
               if (char.IsLetter(map[i][j]) || char.IsDigit(map[i][j]))
               {
                  if (!antennas.ContainsKey(map[i][j]))
                  {
                     antennas[map[i][j]] = [];
                  }
                  antennas[map[i][j]].Add((i, j));
               }
            }
         }

         HashSet<(int, int)> resultSet = new();

         for (int i = 0; i < map.Length; i++)
         {
            for (int j = 0; j < map[i].Length; j++)
            {
               foreach (List<(int, int)> antenna in antennas.Values)
               {
                  if (IsAntiNode(antenna, (i, j)))
                  {
                     resultSet.Add((i, j));
                  }
               }
            }
         }
         HashSet<(int, int)> secondSet = new();

         for (int i = 0; i < map.Length; i++)
         {
            for (int j = 0; j < map[i].Length; j++)
            {
               foreach (List<(int, int)> antenna in antennas.Values)
               {
                  if (IsExtendedAntiNode(antenna, (i, j)))
                  {
                     secondSet.Add((i, j));
                  }
               }
            }
         }

         return $"Unique Locations: {resultSet.Count}, Alternative: {secondSet.Count}";
      }

      private static bool IsAntiNode(List<(int, int)> antennas, (int i, int j) position)
      {
         var distances = antennas.Select(x => GetLineDistance(x, position)).ToArray();

         return distances.HasDoubleDistance();

      }
      private static bool IsExtendedAntiNode(List<(int, int)> antennas, (int i, int j) position)
      {
         var distances = antennas.Select(x => GetLineDistance(x, position)).ToArray();

         return distances.HasAlteredLineDistance();
      }

      private static (int, int) GetLineDistance((int, int) a, (int, int) b)
      {
         return (a.Item1 - b.Item1, a.Item2 - b.Item2);
      }

      private static bool HasDoubleDistance(this (int, int)[] distances)
      {
         for (int i = 0; i < distances.Length; i++)
         {
            (int, int) val = distances[i];

            for (int j = 0; j < distances.Length; j++)
            {
               if (j == i)
               {
                  continue;
               }
               if (distances[j] == (val.Item1 * 2, val.Item2 * 2))
               {
                  return true;
               }
            }
         }
         return false;
      }

      private static bool HasAlteredLineDistance(this (int, int)[] distances)
      {
         for (int i = 0; i < distances.Length; i++)
         {
            (int, int) val = distances[i];
            if (distances.Length > 1 && val == (0, 0))
               return true;

            if(val.Item1 == 0 || val.Item2 == 0)
            {
               continue;
            }

            for (int j = 0; j < distances.Length; j++)
            {
               if (j == i)
               {
                  continue;
               }
               var compare = distances[j];
               if(compare.Item1 == 0 || compare.Item2 == 0)
               {
                  continue;
               }
               double factor = (double)val.Item1 / (double)compare.Item1;

               if (compare.Item2 * factor == val.Item2)
               {
                  return true;
               }

            }
         }
         return false;
      }

   }
}
