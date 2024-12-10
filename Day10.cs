using AdventOfCodeLibrary;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent24;

internal static class Day10
{
   public static string Solve()
   {
      var content = File.ReadAllText("Day10.txt");
      /*
          content = @"89010123
    78121874
    87430965
    96549874
    45678903
    32019012
    01329801
    10456732";
    */
      int length = content.Length;
      var map = content.ToCharMap();

      (int, int)[] zeroLocations = GetAllLocations(map, '0', length);

      int[] reachablePeaks = new int[zeroLocations.Length];
      int[] hikingTrails = new int[zeroLocations.Length];
      for (int i = 0; i < zeroLocations.Length; i++)
      {
         reachablePeaks[i] = CountReachablePeaks(map, zeroLocations[i]);
      }

      for (int i = 0; i < zeroLocations.Length; i++)
      {
         hikingTrails[i] = CountHikingTrails(map, zeroLocations[i]);
      }

      return $"Reachable Nines Part1: {reachablePeaks.Sum()}. Part 2: {hikingTrails.Sum()}";
   }

   private static int CountHikingTrails(char[][] map, (int, int) source)
   {
      Queue<(int, int)> queue = new(1000);

      queue.Enqueue(source);


      int result = 0;
      char currentVal = 'Ö';

      while (queue.Count > 0)
      {
         (int y, int x) pos = queue.Dequeue();

         currentVal = map[pos.y][pos.x];
         if (currentVal == '9')
         {
            result++;
            continue;
         }

         if (pos.y > 0)
         {
            if (map[pos.y - 1][pos.x] - currentVal == 1)
            {
               queue.Enqueue((pos.y - 1, pos.x));
            }
         }
         if (pos.y < map.Length - 1)
         {
            if (map[pos.y + 1][pos.x] - currentVal == 1)
            {
               queue.Enqueue((pos.y + 1, pos.x));
            }
         }
         if (pos.x > 0)
         {
            if (map[pos.y][pos.x - 1] - currentVal == 1)
            {
               queue.Enqueue((pos.y, pos.x - 1));
            }
         }
         if (pos.x < map[pos.y].Length - 1)
         {
            if (map[pos.y][pos.x + 1] - currentVal == 1)
            {
               queue.Enqueue((pos.y, pos.x + 1));
            }
         }
      }

      return result;
   }

   private static int CountReachablePeaks(char[][] map, (int, int) source)
   {
      Queue<(int, int)> queue = new(1000);

      queue.Enqueue(source);


      HashSet<(int, int)> visitedNines = new();
      char currentVal = 'Ö';

      while (queue.Count > 0)
      {
         (int y, int x) pos = queue.Dequeue();

         currentVal = map[pos.y][pos.x];
         if (currentVal == '9')
         {
            visitedNines.Add(pos);
            continue;
         }

         if (pos.y > 0)
         {
            if (map[pos.y - 1][pos.x] - currentVal == 1)
            {
               queue.Enqueue((pos.y - 1, pos.x));
            }
         }
         if (pos.y < map.Length - 1)
         {
            if (map[pos.y + 1][pos.x] - currentVal == 1)
            {
               queue.Enqueue((pos.y + 1, pos.x));
            }
         }
         if (pos.x > 0)
         {
            if (map[pos.y][pos.x - 1] - currentVal == 1)
            {
               queue.Enqueue((pos.y, pos.x - 1));
            }
         }
         if (pos.x < map[pos.y].Length - 1)
         {
            if (map[pos.y][pos.x + 1] - currentVal == 1)
            {
               queue.Enqueue((pos.y, pos.x + 1));
            }
         }
      }

      return visitedNines.Count;
   }

   private static (int, int)[] GetAllLocations(char[][] map, char v, int maxLength)
   {
      int count = 0;
      Span<(int, int)> locations = stackalloc (int, int)[maxLength];

      for (int i = 0; i < map.Length; i++)
      {
         for (int j = 0; j < map[i].Length; j++)
         {
            if (map[i][j] == v)
            {
               locations[count] = (i, j);
               count++;
            }
         }
      }

      return locations.Slice(0, count).ToArray();
   }
}

