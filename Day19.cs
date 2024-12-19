using System.Runtime.InteropServices;

namespace Advent24
{
   public struct Towel
   {
      public char[] Colors;

      public override string ToString()
      {
         return String.Join("", Colors);
      }
   }
   public static class Day19
   {

      public static string Solve()
      {
         var content = File.ReadAllText("Day19.txt");


         var segments = content.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

         var availableTowels = GetTowels(segments[0].Split(',', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries));

         char[] SingleColors = availableTowels.Where(x => x.Colors.Length == 1).SelectMany(x => x.Colors).Distinct().ToArray();

         var lines = segments[1].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => x.ToCharArray()).ToArray();

         ulong count = 0;

         List<ulong> bag = new();

         /*
         Parallel.ForEach(lines, (line) =>
         {
            bag.Add(SlowCase(line, availableTowels));
         });
         */

         for (int i = 0; i < lines.Length; i++)
         {
            count += SlowCase(lines[i], availableTowels);
         }



         return $"Possible towels Part1: {count}";
      }

      private static ulong SlowCase(Span<char> chars, Towel[] availableTowels)
      {
         List<(int Start, int End)> indices = new();

         foreach (var towel in availableTowels)
         {
            indices.AddRange(AllIndices(chars, towel));
         }

         return HasUniqueCombination(indices, chars.Length);
      }

      private static ulong HasUniqueCombination(List<(int Start, int End)> indices, int length)
      {
         if (indices.All(x => x.End != length))
         {
            return 0;
         }

         if (indices.All(x => x.Start != 0))
         {
            return 0;
         }
         ulong result = 0;

         (int Start, int End)[] starts = indices.Where(x => x.Start == 0).ToArray();

         Span<((int, int), ulong)> sp = stackalloc ((int, int), ulong)[indices.Count];

         int idx = 0;

         for (int i = 0; i < indices.Count; i++)
         {
            var x = indices[i];

            if (x.Start == 0)
            {
               sp[idx++] = (x, 1);
            }
         }

         result = Recursive(indices, sp[..idx], length);

         return result;
      }

      private static Dictionary<(int, int), ulong> numbers = new();

      private static ulong Recursive(List<(int Start, int End)> list, Span<((int start, int end) pos, ulong count)> start, int length)
      {
         if (start.Length == 0)
         {
            return 0;
         }
         numbers.Clear();
         List<(int, int)> values = new();

         for (int i = 0; i < start.Length; i++)
         {
            var st = start[i];
            for (int j = 0; j < list.Count; j++)
            {
               var x = list[j];

               if (st.pos.end == x.Start)
               {
                  CollectionsMarshal.GetValueRefOrAddDefault(numbers, x, out _) += st.count;
               }

            }
         }

         ((int Start, int End) Pos, ulong Count)[] newL = numbers.Select(x => (x.Key, x.Value)).ToArray();

         ulong resHere = 0;
         foreach (var item in newL.Where(x => x.Pos.End == length))
         {
            resHere += item.Count;
         }


         return Recursive(list, newL, length) + resHere;
      }


      private static List<(int, int)> AllIndices(Span<char> chars, Towel towelStruct)
      {
         Span<char> towel = towelStruct.Colors;
         List<(int, int)> result = new();
         int startIdx = 0;
         int idx = -1;
         do
         {
            idx = chars.IndexOf(towel);
            if (idx == -1)
            {
               break;
            }
            result.Add((startIdx + idx, startIdx + idx + towel.Length));
            chars = chars[(idx + 1)..];
            startIdx += idx + 1;
         } while (chars.Length > 0);


         return result;
      }

      private static bool QuickCase(char[] chars, char[] singleColors)
      {
         return chars.All(x => singleColors.Contains(x));
      }

      private static Towel[] GetTowels(string[] strings)
      {
         var result = new Towel[strings.Length];

         for (int i = 0; i < strings.Length; i++)
         {
            result[i] = new Towel { Colors = strings[i].ToCharArray() };
         }

         return result.OrderByDescending(x => x.Colors.Length).ToArray();
      }
   }
}
