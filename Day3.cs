using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   internal static class Day3
   {
      public static string Solve()
      {
         var content = File.ReadAllText("Day03.txt").AsSpan();
         //content = "nxmul(2,4)&mul[3,7]!^don't()_mul(5,5)+mul(32,64](mul(11,8)undo()?mul(8,5))";

         bool DoIt = true;

         List<int> results = new(300);
         int idx = content.IndexOf("mul(");
         int idx2 = content.IndexOf("don't()");
         while (idx >= 0)
         {
            if (!DoIt)
            {
               idx = content.IndexOf("do()");
               if (idx < 0)
               {
                  break;
               }
               else
               {
                  content = content[(idx + 4)..];
                  DoIt = true;
               }
               idx = content.IndexOf("mul(");
               idx2 = content.IndexOf("don't()");
            }

            if (idx2 >= 0 && idx2 < idx)
            {
               content = content[(idx2 + 6)..];
               DoIt = false;
               continue;
            }

            content = content[(idx + 4)..];

            int count = 0;
            count = CountDigits(content);
            if (count is 0 or 4)
            {
               idx = content.IndexOf("mul(");
               idx2 = content.IndexOf("don't()");
               continue;
            }
            int first = int.Parse(content[..count]);
            content = content[count..];
            if (content[0] != ',')
            {
               idx = content.IndexOf("mul(");
               idx2 = content.IndexOf("don't()");
               continue;
            }
            content = content[1..];
            count = CountDigits(content);
            if (count is 0 or 4)
            {
               idx = content.IndexOf("mul(");
               idx2 = content.IndexOf("don't()");
               continue;
            }
            int second = int.Parse(content[..count]);

            content = content[count..];
            if (content[0] != ')')
            {
               idx = content.IndexOf("mul(");
               idx2 = content.IndexOf("don't()");
               continue;
            }
            content = content[1..];
            results.Add(first * second);
            idx = content.IndexOf("mul(");
            idx2 = content.IndexOf("don't()");
         }


         long sum = 0;
         for (int i = 0; i < results.Count; i++)
         {
            sum += results[i];
         }

         return $"Mult Result: {sum}";
      }

      private static int CountDigits(ReadOnlySpan<char> content)
      {
         int count = 0;
         for (int i = 0; i <= 4; i++)
         {
            if (char.IsDigit(content[i]))
            {
               count++;
            }
            else
            {
               break;
            }
         }

         return count;
      }

      enum State
      {
         None,
         Operation,
         Number,
         Seperator,
         SecondNumber,
      }

   }
}
