using AdventOfCodeLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Advent24;

internal static class Day12
{
   public static string Solve()
   {
      var content = File.ReadAllText("Day12.txt");

      var map = content.ToCharMap();

      Debug.Assert(map.IsSquare());

      List<(List<(int, int)>, char)> regions = FindRegionsBasedOnChar(map);

      int[] borderCount = CountBorders(regions);

      int[] discountBorderCount = CountDiscordBorders(regions);

      Debug.Assert(borderCount.Length == regions.Count);

      int[] price = new int[regions.Count];

      for (int i = 0; i < regions.Count; i++)
      {
         price[i] = regions[i].Item1.Count * borderCount[i];
      }

      (int, char)[] discounts = new (int, char)[regions.Count];
      for (int i = 0; i < regions.Count; i++)
      {
         discounts[i] = (regions[i].Item1.Count * discountBorderCount[i], regions[i].Item2);
      }

      return $"Price: {price.Sum()}, Discounted: {discounts.Sum(x => x.Item1)}";
   }

   private static int[] CountDiscordBorders(List<(List<(int y, int x)>, char)> regions)
   {
      int[] result = new int[regions.Count];

      for (int i = 0; i < regions.Count; i++)
      {
         var current = regions[i].Item1;
         int count = 0;
         List<(int y, int x)> horizontalTop = new(regions.Count);
         List<(int y, int x)> horizontalBottom = new(regions.Count);
         List<(int y, int x)> verticalLeft = new(regions.Count);
         List<(int y, int x)> verticalRight = new(regions.Count);


         foreach (var item in current)
         {
            if (!current.Any(x => x.x == item.x && x.y - item.y == 1))
            {
               if (!horizontalBottom.Any(x => x.y == item.y && Math.Abs(x.x - item.x) == 1))
                  count++;

               if (horizontalBottom.Where(x => x.y == item.y && Math.Abs(x.x - item.x) == 1).Count() == 2)
                  count--;

               horizontalBottom.Add(item);
            }
            if (!current.Any(x => x.x == item.x && x.y - item.y == -1))
            {
               if (!horizontalTop.Any(x => x.y == item.y && Math.Abs(x.x - item.x) == 1))
                  count++;

               if (horizontalTop.Where(x => x.y == item.y && Math.Abs(x.x - item.x) == 1).Count() == 2)
                  count--;

               horizontalTop.Add(item);
            }
            if (!current.Any(x => x.y == item.y && x.x - item.x == 1))
            {
               if (!verticalLeft.Any(x => x.x == item.x && Math.Abs(x.y - item.y) == 1))
                  count++;

               if (verticalLeft.Where(x => x.x == item.x && Math.Abs(x.y - item.y) == 1).Count() == 2)
                  count--;

               verticalLeft.Add(item);
            }
            if (!current.Any(x => x.y == item.y && x.x - item.x == -1))
            {
               if (!verticalRight.Any(x => x.x == item.x && Math.Abs(x.y - item.y) == 1))
                  count++;

               if (verticalRight.Where(x => x.x == item.x && Math.Abs(x.y - item.y) == 1).Count() == 2)
                  count--;

               verticalRight.Add(item);
            }
         }

         result[i] = count;
      }

      return result;
   }

   private static int[] CountBorders(List<(List<(int y, int x)>, char)> regions)
   {
      int[] result = new int[regions.Count];

      for (int i = 0; i < regions.Count; i++)
      {
         var current = regions[i].Item1;
         int count = 0;

         foreach (var item in current)
         {
            if (!current.Any(x => x.x == item.x && x.y - item.y == 1))
            {
               count++;
            }
            if (!current.Any(x => x.x == item.x && x.y - item.y == -1))
            {
               count++;
            }
            if (!current.Any(x => x.y == item.y && x.x - item.x == 1))
            {
               count++;
            }
            if (!current.Any(x => x.y == item.y && x.x - item.x == -1))
            {
               count++;
            }
         }

         result[i] = count;
      }

      return result;
   }

   private static List<(List<(int, int)>, char)> FindRegionsBasedOnChar(char[][] map)
   {
      var chars = map.SelectMany(x => x).Distinct();
      var l = map.Length * map[0].Length;

      List<(List<(int, int)>, char)> result = [];
      foreach (var c in chars)
      {
         var regions = FindAllRegions(map, c, l);
         result.AddRange(regions);
      }

      return result;
   }

   private static List<(List<(int y, int x)>, char)> FindAllRegions(char[][] map, char value, int length)
   {
      Span<(int, int)> result = stackalloc (int, int)[length];
      int idx = 0;

      for (int y = 0; y < map.Length; y++)
      {
         for (int x = 0; x < map[y].Length; x++)
         {
            if (map[y][x] == value)
            {
               result[idx] = (y, x);
               idx++;
            }
         }
      }

      return FindAreas(result[..idx], value);
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   public static List<(List<(int, int)>, char)> FindAreas(ReadOnlySpan<(int y, int x)> input, char value)
   {
      if (input.Length == 0)
         return [];

      List<List<(int y, int x)>> result = [];

      var cur = input[0];
      result.Add([cur]);
      int fIdx = -1;

      for (int i = 1; i < input.Length; i++)
      {
         fIdx = -1;
         cur = input[i];

         for (int k = 0; k < result.Count; k++)
         {
            var list = result[k];
            if (CheckEuclideanDistance(cur, list))
            {
               fIdx = k;
               break;
            }
         }

         if (fIdx == -1)
         {
            result.Add([cur]);
         }
         else
         {
            result[fIdx].Add(cur);
         }
      }

      return Combine(result, value);
   }

   private static List<(List<(int, int)>, char)> Combine(List<List<(int y, int x)>> input, char value)
   {
      List<List<(int, int)>> result = new(input.Count);
      bool cont = true;
      bool found = false;
      List<List<(int y, int x)>> iter = DeepCopy(input);

      while (cont)
      {
         result.Clear();
         result.Add(iter[0]);
         cont = false;
         for (int i = 1; i < iter.Count; i++)
         {
            found = false;
            var toCheck = iter[i];

            for (int j = 0; j < result.Count; j++)
            {
               var cur = result[j];
               if (cur.Any(x => CheckEuclideanDistance(x, toCheck)))
               {
                  cur.AddRange(toCheck);
                  cont = true;
                  found = true;
                  break;
               }
            }
            if (!found)
            {
               result.Add(toCheck);
            }
         }

         iter = DeepCopy(result);
      }

      return result.Select(x => (x, value)).ToList();
   }

   private static List<List<(int y, int x)>> DeepCopy(List<List<(int y, int x)>> input)
   {
      List<List<(int x, int y)>> result = new(input.Count);

      for (int i = 0; i < input.Count; i++)
      {
         var cur = new List<(int, int)>(input[i].Count);
         for (int j = 0; j < input[i].Count; j++)
         {
            cur.Add(input[i][j]);
         }
         result.Add(cur);
      }

      return result;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static bool CheckEuclideanDistance((int y, int x) cur, List<(int y, int x)> list)
   {
      return list.Any(x => Math.Abs(x.y - cur.y) + Math.Abs(x.x - cur.x) <= 1);
   }
}

