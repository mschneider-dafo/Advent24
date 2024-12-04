using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   internal static class Day4
   {
      const string target = "XMAS";
      const string reverse = "SAMX";
      const string insane = "MAS";
      const string insaneRev = "SAM";
      public static string Solve()
      {
         var content = File.ReadAllText("Day04.txt");

         char[][] array = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => x.ToCharArray()).ToArray();


         var x = array[0].Length;
         var y = array.Length;

         Console.WriteLine($"x: {x}, y: {y}");

         int count = 0;
         int insane = 0;

         for (int i = 0; i < y; i++)
         {
            for (int j = 0; j < x; j++)
            {
               if (array[i][j] == target[0])
               {
                  count += Check(i, j, array, target);
               }
               if (array[i][j] == reverse[0])
               {
                  count += Check(i, j, array, reverse);
               }
            }
         }


         insane = Insanity(array);

         return $"Count: {count}, InsaneCount: {insane}";
      }

      private static int Insanity(char[][] array)
      {
         int result = 0;
         for (int y = 1; y < array.Length - 1; y++)
         {
            for (int x = 1; x < array[y].Length - 1; x++)
            {
               if (array[y][x] == 'A')
               {
                  if (CheckInsane(y, x, array, insane))
                  {
                     result++;
                  }
               }
            }
         }

         return result;
      }

      private static bool CheckInsane(int y, int x, char[][] array, string target)
      {
         Debug.Assert(y > 0);
         Debug.Assert(x > 0);
         Debug.Assert(y < array.Length - 1);
         Debug.Assert(y < array[0].Length - 1);
         Debug.Assert(!string.IsNullOrEmpty(target));
         Debug.Assert(target.Length == 3);

         bool result = false;

         result = CheckTopLeft(y, x, array, target) && CheckTopRight(y, x, array, target);


         return result;
      }

      private static bool CheckTopRight(int y, int x, char[][] array, string target)
      {
         bool main = array[y - 1][x + 1] == target[0] && array[y + 1][x - 1] == target[2];
         bool rev = array[y - 1][x + 1] == target[2] && array[y + 1][x - 1] == target[0];

         return main || rev;
      }

      private static bool CheckTopLeft(int y, int x, char[][] array, string target)
      {
         bool main = array[y - 1][x - 1] == target[0] && array[y + 1][x + 1] == target[2];
         bool rev = array[y - 1][x - 1] == target[2] && array[y + 1][x + 1] == target[0];

         return main || rev;
      }

      private static int Check(int y, int x, char[][] array, string target)
      {
         int result = 0;

         result += CheckVertical(y, x, array, target);
         result += CheckHorizontal(y, x, array, target);
         result += CheckDiagonalLeft(y, x, array, target);
         result += CheckDiagonalRight(y, x, array, target);


         return result;
      }

      private static int CheckDiagonalRight(int y, int x, char[][] array, string target)
      {
         if (x + target.Length > array[0].Length || y + target.Length > array.Length)
         {
            return 0;
         }

         for (int i = 0; i < target.Length; i++)
         {
            if (array[y + i][x + i] != target[i])
            {
               return 0;
            }
         }

         return 1;
      }

      private static int CheckDiagonalLeft(int y, int x, char[][] array, string target)
      {
         if (x - (target.Length - 1) < 0 || y + target.Length > array.Length)
         {
            return 0;
         }

         for (int i = 0; i < target.Length; i++)
         {
            if (array[y + i][x - i] != target[i])
            {
               return 0;
            }
         }

         return 1;
      }

      private static int CheckHorizontal(int y, int x, char[][] array, string target)
      {
         if (x + target.Length > array[0].Length)
         {
            return 0;
         }
         for (int i = 0; i < target.Length; i++)
         {
            if (array[y][x + i] != target[i])
            {
               return 0;
            }
         }
         return 1;
      }

      private static int CheckVertical(int y, int x, char[][] array, string target)
      {
         if (y + target.Length > array.Length)
         {
            return 0;
         }
         for (int i = 0; i < target.Length; i++)
         {
            if (array[y + i][x] != target[i])
            {
               return 0;
            }
         }
         return 1;
      }
   }
}
