using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   internal static class Day5
   {
      public static string Solve()
      {
         var content = File.ReadAllText("Day05.txt");
         /*content = @"47|53
       97|13
       97|61
       97|47
       75|29
       61|13
       75|53
       29|13
       97|29
       53|29
       61|53
       97|53
       61|29
       47|13
       75|47
       97|75
       47|61
       75|61
       47|29
       75|13
       53|13

       75,47,61,53,29
       97,61,53,29,13
       75,29,13
       75,97,47,61,53
       61,13,29
       97,13,75,29,47";*/
         var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

         int[][] rules = lines.Where(x => x.Contains('|')).Select(x => x.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray()).ToArray();
         Debug.Assert(rules.All(x => x.Length == 2));

         int[][] prints = lines.Where(x => x.Contains(',')).Select(x => x.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(int.Parse).ToArray()).ToArray();
         Debug.Assert(prints.All(x => x.Length % 2 != 0));

         long sum = 0;
         long incorrectSum = 0;

         for (int i = 0; i < prints.Length; i++)
         {
            var line = prints[i];
            Debug.Assert(line.Distinct().Count() == line.Length, "Duplicates in array");
            (bool check, int[][] relevant) = CheckRulebook(line, rules);
            if (!check)
            {
               line = OrderLine(line, relevant);
               incorrectSum += line[line.Length / 2];
            }
            else
            {
               int middle = line[line.Length / 2];
               sum += middle;
            }

         }




         return $"Sum of middle: {sum}, Sum of Incorrect: {incorrectSum}";
      }

      private static int[] OrderLine(int[] line, int[][] rules)
      {
         bool check = false;
         int[] result = new int[line.Length];
         line.CopyTo(result, 0);
         do
         {
            check = false;
            for (int i = 0; i < rules.Length; i++)
            {
               var rule = rules[i];

               Debug.Assert(result.Contains(rule[0]) && result.Contains(rule[1]), "Rule not found in array");

               int idx1 = Array.IndexOf(result, rule[0]);
               int idx2 = Array.IndexOf(result, rule[1]);

               if (idx1 > idx2)
               {
                  result[idx1] = rule[1];
                  result[idx2] = rule[0];
                  check = true;
               }
            }
         } while (check);


         return result;
      }

      private static (bool, int[][]) CheckRulebook(int[] arrOfNumbers, int[][] rules)
      {
         bool check = true;
         List<int[]> relevantRules = new();
         for (int i = 0; i < rules.Length; i++)
         {
            var rule = rules[i];
            if (!arrOfNumbers.Contains(rule[0]) || !arrOfNumbers.Contains(rule[1]))
            {
               continue;
            }
            int idx1 = Array.IndexOf(arrOfNumbers, rule[0]);
            int idx2 = Array.IndexOf(arrOfNumbers, rule[1]);

            Debug.Assert(idx1 >= 0 && idx2 >= 0, "Rule not found in array");

            relevantRules.Add([rule[0], rule[1]]);

            if (idx1 > idx2)
            {
               check = false;
            }
         }


         return (check, relevantRules.ToArray());
      }

   }
}
