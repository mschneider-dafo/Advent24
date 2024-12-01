using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   public static class Day1
   {
      public static string Solve()
      {
         var content = File.ReadAllText("Day01.txt");

         var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

         (var numbers1, var numbers2) = (new int[lines.Length], new int[lines.Length]);

         for (int i = 0; i < lines.Length; i++)
         {
            var parts = lines[i].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            numbers1[i] = int.Parse(parts[0]);
            numbers2[i] = int.Parse(parts[1]);
         }

         Array.Sort(numbers2);
         Array.Sort(numbers1);

         Debug.Assert(numbers1.Length == numbers2.Length, "Arrays should have the same length");

         long sum = 0;
         long sum2 = 0;
         for (int i = 0; i < numbers1.Length; i++)
         {
            sum += Math.Abs(numbers1[i] - numbers2[i]);
            sum2 += numbers1[i] * GetAppearances(numbers1[i], numbers2);
         }


         return $"Summed difference is {sum}, Summed appearances are {sum2}";
      }

      static int GetAppearances(int x, int[] arr)
      {
         var sum = 0;
         for (int i = 0; i < arr.Length; i++)
         {
            if (x == arr[i])
            {
               sum++;
            }
         }

         return sum;
      }

   }
}
