using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   internal static class Day6
   {
      enum Direction
      {
         UP,
         RIGHT,
         DOWN,
         LEFT
      }

      const int EnumModulo = 4;

      public static string Solve()
      {
         var content = File.ReadAllText("Day06.txt");
         /* content = @"....#.....
 .........#
 ..........
 ..#.......
 .......#..
 ..........
 .#..^.....
 ........#.
 #.........
 ......#...";
          */


         char[][] map = content.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => x.ToCharArray()).ToArray();

         ((int y, int x) pos, Direction direct) = GetPositionAndDirection(ref map);
         var orgPos = pos;
         bool finished = false;


         int numberOfBLocks = map.SelectMany(x => x).Count(c => c == '#');

         do
         {
            (pos, direct, finished) = Move(ref map, pos, direct);
         } while (!finished);

         var list = ChangeMap(map, orgPos).ToList();
         int count2 = 0;
         int count = map.SelectMany(x => x).Count(x => x == '0');

         for (int i = 0; i < list.Count; i++)
         {
            direct = Direction.UP;
            pos = orgPos;
            finished = false;
            bool broke = false;
            int iterations = 0;
            char[][]? m = list[i];
            int numberOfAlteredBlocks = m.SelectMany(x => x).Count(c => c == '#');
            Debug.Assert(numberOfBLocks + 1 == numberOfAlteredBlocks);
            do
            {
               (pos, direct, finished) = Move(ref m, pos, direct);
               iterations++;
               if (iterations > count * 2)
               {
                  broke = true;
                  break;
               }

            } while (!finished);

            if (broke)
            {
               count2++;
            }
         }


         return $"Visited Positions {count}, AltCount {count2}";
      }

      private static ((int y, int x) pos, Direction direct, bool finished) Move(ref char[][] map, (int y, int x) pos, Direction direct)
      {
         (int y, int x) = direct switch
         {
            Direction.UP => (pos.y - 1, pos.x),
            Direction.RIGHT => (pos.y, pos.x + 1),
            Direction.DOWN => (pos.y + 1, pos.x),
            Direction.LEFT => (pos.y, pos.x - 1),
            _ => throw new Exception("Invalid direction")
         };

         if (y < 0 || y >= map.Length || x < 0 || x >= map[y].Length)
         {
            return (pos, direct, true);
         }

         if (map[y][x] == '#')
         {
            direct = ChangeDirection(direct);
            return Move(ref map, pos, direct);
         }


         map[y][x] = '0';
         return ((y, x), direct, false);
      }

      private static Direction ChangeDirection(Direction direct)
      {
         return (Direction)(((int)direct + 1) % EnumModulo);
      }

      private static ((int y, int x) pos, Direction direct) GetPositionAndDirection(ref char[][] map)
      {

         FileInfo c = new FileInfo("abc");
         for (int i = 0; i < map.Length; i++)
         {
            for (int j = 0; j < map[i].Length; j++)
            {
               if (map[i][j] == '^')
               {
                  map[i][j] = '0';
                  return ((i, j), Direction.UP);
               }
               else if (map[i][j] == '>')
               {
                  map[i][j] = '0';
                  return ((i, j), Direction.RIGHT);
               }
               else if (map[i][j] == 'v')
               {
                  map[i][j] = '0';
                  return ((i, j), Direction.DOWN);
               }
               else if (map[i][j] == '<')
               {
                  map[i][j] = '0';
                  return ((i, j), Direction.LEFT);
               }
            }
         }
         throw new Exception("No direction found");
      }


      private static List<char[][]> ChangeMap(char[][] map, (int y, int x) start)
      {
         List<char[][]> result = new();

         for (int i = 0; i < map.Length; i++)
         {
            for (int j = 0; j < map[i].Length; j++)
            {
               if (i == start.y && j == start.x)
               {
                  continue;
               }

               if (map[i][j] == '0')
               {
                  var cp = Copy2DArray(map);
                  cp[i][j] = '#';
                  result.Add(cp);
               }
            }
         }

         return result;

      }

      private static char[][] Copy2DArray(char[][] map)
      {
         var newMap = new char[map.Length][];
         for (int i = 0; i < map.Length; i++)
         {
            char[] arr = new char[map[i].Length];
            map[i].CopyTo(arr, 0);
            newMap[i] = arr;
         }

         return newMap;
      }
   }
}
