using AdventOfCodeLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

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

      int[] discountBorderCount = CountDiscountBorders(regions);

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

   private static int[] CountDiscountBorders(List<(List<(int y, int x)>, char)> regions)
   {
      int[] result = new int[regions.Count];

      for (int i = 0; i < regions.Count; i++)
      {
         var current = CollectionsMarshal.AsSpan(regions[i].Item1);

         Dictionary<int, List<int>> horizontalTop = new(regions.Count);
         Dictionary<int, List<int>> horizontalBottom = new(regions.Count);
         Dictionary<int, List<int>> verticalLeft = new(regions.Count);
         Dictionary<int, List<int>> verticalRight = new(regions.Count);

         for (int j = 0; j < current.Length; j++)
         {
            bool top = true, bot = true, left = true, right = true;
            (int y, int x) item = current[j];

            for (int k = 0; k < current.Length; k++)
            {
               if (k == j)
                  continue;

               (int y, int x) other = current[k];

               if (item.y == other.y && item.x - other.x == 1)
               {
                  left = false;
               }

               if (item.y == other.y && item.x - other.x == -1)
               {
                  right = false;
               }

               if (item.x == other.x && item.y - other.y == 1)
               {
                  top = false;
               }

               if (item.x == other.x && item.y - other.y == -1)
               {
                  bot = false;
               }
            }

            if (top)
            {
               if (!horizontalTop.TryGetValue(item.y, out var list))
               {
                  list = new();
                  horizontalTop.Add(item.y, list);
               }
               list.Add(item.x);
            }
            if (bot)
            {
               if (!horizontalBottom.TryGetValue(item.y, out var list))
               {
                  list = new();
                  horizontalBottom.Add(item.y, list);
               }
               list.Add(item.x);
            }
            if (left)
            {
               if (!verticalLeft.TryGetValue(item.x, out var list))
               {
                  list = new();
                  verticalLeft.Add(item.x, list);
               }
               list.Add(item.y);
            }
            if (right)
            {
               if (!verticalRight.TryGetValue(item.x, out var list))
               {
                  list = new();
                  verticalRight.Add(item.x, list);
               }
               list.Add(item.y);
            }
         }

         result[i] += CountLines(horizontalTop);
         result[i] += CountLines(horizontalBottom);
         result[i] += CountLines(verticalLeft);
         result[i] += CountLines(verticalRight);
      }

      return result;
   }

   private static int CountLines(Dictionary<int, List<int>> dict)
   {
      int result = 0;

      foreach (var item in dict)
      {
         var list = item.Value;
         list.Sort();
         int count = 1;
         for (int i = 1; i < list.Count; i++)
         {
            if (list[i] - list[i - 1] is > 1)
            {
               count++;
            }
         }
         result += count;
      }
      return result;
   }

   private static int[] CountBorders(List<(List<(int y, int x)>, char)> regions)
   {
      int[] result = new int[regions.Count];

      for (int i = 0; i < regions.Count; i++)
      {
         var current = CollectionsMarshal.AsSpan(regions[i].Item1);
         int count = 0;

         for (int j = 0; j < current.Length; j++)
         {
            bool top = true, bot = true, left = true, right = true;
            (int y, int x) item = current[j];

            for (int k = 0; k < current.Length; k++)
            {
               (int y, int x) other = current[k];

               if (item.y == other.y && item.x - other.x == 1)
                  right = false;

               if (item.y == other.y && item.x - other.x == -1)
                  left = false;

               if (item.x == other.x && item.y - other.y == 1)
                  bot = false;

               if (item.x == other.x && item.y - other.y == -1)
                  top = false;
            }

            if (top)
               count++;
            if (bot)
               count++;
            if (left)
               count++;
            if (right)
               count++;

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
            var list = CollectionsMarshal.AsSpan(result[k]);
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
            var toCheck = CollectionsMarshal.AsSpan(iter[i]);

            for (int j = 0; j < result.Count; j++)
            {
               var cur = result[j];

               bool escapeCheck = false;
               for (int k = 0; !escapeCheck && k < cur.Count; k++)
               {
                  escapeCheck |= CheckEuclideanDistance(cur[k], toCheck);
               }

               if (escapeCheck)
               {
                  cur.AddRange(toCheck);
                  cont = true;
                  found = true;
                  break;
               }
            }
            if (!found)
            {
               result.Add(iter[i]);
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
   private static bool CheckEuclideanDistance((int y, int x) cur, Span<(int y, int x)> list)
   {
      for (int i = 0; i < list.Length; i++)
      {
         var ydist = list[i].y - cur.y;
         var xdist = list[i].x - cur.x;

         if (ydist * ydist + xdist * xdist == 1)
         {
            return true;
         }
      }
      return false;
   }
}

