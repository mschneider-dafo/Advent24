using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent24;

internal static class Day9
{
   public static string Solve()
   {
      var content = File.ReadAllText("Day09.txt");
      /*content = @"2333133121414131402";*/

      List<(int size, int id)> contentList = new(content.Length);

      bool file = true;
      int idx = 0;
      int index = 0;
      for (int i = 0; i < content.Length; i++)
      {
         if (file)
         {
            var digit = (int)char.GetNumericValue(content[i]);
            contentList.Add((digit, idx));
            idx++;
         }
         else
         {
            var digit = (int)char.GetNumericValue(content[i]);
            contentList.Add((digit, -1));
         }
         index++;
         file = !file;
      }

      long result = 0;

      for (int i = contentList.Count - 1; i >= 0; i--)
      {
         if (contentList[i].id != -1)
         {
            contentList = GetFitting(contentList[i].size, contentList, i);
            contentList = MergeContiguous(contentList);
         }
      }

      var multiplier = 0;

      for (int i = 0; i < contentList.Count; i++)
      {
         if (contentList[i].id == -1)
         {
            for (int k = 0; k < contentList[i].size; k++)
            {
               multiplier++;
            }

            continue;
         }

         for (int k = 0; k < contentList[i].size; k++)
         {
            result += contentList[i].id * multiplier;
            multiplier++;
         }
      }


      return $"Result: {result}";
   }

   private static List<(int size, int id)> MergeContiguous(List<(int size, int id)> contentList)
   {
      for (int i = 0; i < contentList.Count - 1; i++)
      {
         var cur = contentList[i];
         var next = contentList[i + 1];

         if (cur.id == -1 && next.id == -1)
         {
            contentList[i] = (cur.size + next.size, -1);
            contentList.RemoveAt(i + 1);
         }
      }

      return contentList;
   }

   private static List<(int size, int id)> GetFitting(int size, List<(int size, int id)> list, int idx)
   {
      (int size, int id) val = default;
      int indexOfMove = -1;
      for (int i = 0; i < idx; i++)
      {
         if (list[i].id != -1)
         {
            continue;
         }
         val = list[i];

         if (val.size >= size)
         {
            indexOfMove = i;
            break;
         }
      }

      if (indexOfMove == -1)
      {
         return list;
      }


      var diff = val.size - size;

      if(diff ==0)
      {
         list[indexOfMove] = (size, list[idx].id);
         list[idx] = (size, -1);
         return list;
      }

      var toAdd = (size, list[idx].id);

      list[indexOfMove] = (diff, val.id);

      list[idx] = (size, -1);

      list.Insert(indexOfMove, toAdd);

      return list;
   }

   private static int IndexOf(StringBuilder sb, char c)
   {
      for (int i = 0; i < sb.Length; i++)
      {
         if (sb[i] == c)
         {
            return i;
         }
      }
      return -1;
   }
   private static int LastIndexOfNot(this List<int> sb, int c)
   {
      for (int i = sb.Count - 1; i >= 0; i--)
      {
         if (sb[i] != c)
         {
            return i;
         }
      }
      return -1;
   }
}

