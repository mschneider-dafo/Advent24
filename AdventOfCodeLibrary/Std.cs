using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCodeLibrary
{
   public static class Std
   {
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static char[][] ToCharMap(this string input) => input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => x.ToCharArray()).ToArray();
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static string[] ToLines(this string input) => input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);


      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static bool SquareAssumption(char[][] map) => map.Length == 0 || (map.All(x => x.Length == map[0].Length) && map.Length == map[0].Length);
      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static bool IsSquare(this char[][] map) => map.Length == 0 || (map.All(x => x.Length == map[0].Length) && map.Length == map[0].Length);


      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static int GetNumberOfDigits(int number)
      {
         int count = 0;
         do
         {
            number /= 10;
            count++;
         }
         while (number != 0);
         return count;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static int GetNumberOfDigits(long number)
      {
         int count = 0;
         do
         {
            number /= 10;
            count++;
         }
         while (number != 0);
         return count;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static int GetNumberOfDigits(string input)
      {
         if (!long.TryParse(input, out long result))
            return -1;

         return GetNumberOfDigits(result);
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      public static double FastParseDouble(ReadOnlySpan<char> input)
      {
         int result = 0;
         int hasdot = 0;
         double dot = 0;
         while (input.Length > 0)
         {
            if (input[0] == '.')
            {
               hasdot = 1;
               input = input[1..];
               continue;
            }
            result = (result * 10) + (input[0] - '0');
            input = input[1..];
            dot += hasdot;
         }

         return result / dot;
      }

   }
}
