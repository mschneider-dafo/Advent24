namespace Advent24
{
   internal static class Day2
   {
      public static string Solve()
      {
         var content = File.ReadAllText("Day02.txt");
         /*    content = @"7 6 4 2 1
    1 2 7 8 9
    9 7 6 2 1
    1 3 2 4 5
    8 6 4 4 1
    1 3 6 7 9";

             */
         var lines = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);

         List<int[]> parsedLines = new(lines.Length);
         for (int i = 0; i < lines.Length; i++)
         {
            parsedLines.Add(lines[i].Split(' ').Select(int.Parse).ToArray());
         }

         int counter = lines.Length;
         bool direction;
         List<int[]> UnsafeLines = new();

         for (int i = 0; i < lines.Length; i++)
         {
            int[] line = parsedLines[i];
            (_, counter) = Parse(counter, UnsafeLines, line);
         }

         int add = UnsafeLines.Count;
         var newCounter = 0;

         for (int i = 0; i < add; i++)
         {
            bool succ = false;
            var line = UnsafeLines[i];
            for (int j = 0; j < line.Length; j++)
            {
               int[] arr = new int[line.Length - 1];
               line[0..j].CopyTo(arr, 0);
               if (j < line.Length - 1)
                  line[(j + 1)..].CopyTo(arr, j);
               (bool x, _) = Parse(newCounter, [], arr);
               if (x)
               {
                  succ = true;
                  break;
               }
            }
            if (succ)
            {
               newCounter++;
            }
         }


         return $"SAFE Lines: {counter}, Potential: {newCounter}, Full {newCounter + counter}";
      }

      private static (bool, int) Parse(int counter, List<int[]> UnsafeLines, int[] line)
      {
         bool succ = true;
         bool firstTime = true;
         bool direction = line[1] > line[0];
         int last = line[0];
         for (int j = 1; j < line.Length; j++)
         {
            int current = line[j];
            var curDir = current > last;
            if (curDir != direction)
            {
               if (firstTime)
               {
                  firstTime = false;
                  succ = false;
                  continue;
               }
               counter--;
               UnsafeLines.Add(line);
               break;
            }
            var difference = Math.Abs(current - last);

            if (difference is < 1 or > 3)
            {
               if (firstTime)
               {
                  firstTime = false;
                  succ = false;
                  continue;
               }
               counter--;
               UnsafeLines.Add(line);
               break;
            }

            last = current;
         }

         return (succ, counter);
      }
   }
}
