using AdventOfCodeLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Advent24;

enum Direction
{
   Up,
   Down,
   Left,
   Right
}
internal static class Day15
{
   public static string Solve()
   {
      var content = File.ReadAllText("Day15.txt");

      content = @"##########
#..O..O.O#
#......O.#
#.OO..O.O#
#..O@..O.#
#O#..O...#
#O..O..O.#
#.OO.O.OO#
#....O...#
##########

<vv>^<v^>v>^vv^v>v<>v^v<v<^vv<<<^><<><>>v<vvv<>^v^>^<<<><<v<<<v^vv^v>^
vvv<<^>^v^^><<>>><>^<<><^vv^^<>vvv<>><^^v>^>vv<>v<<<<v<^v>^<^^>>>^<v<v
><>vv>v^v^<>><>>>><^^>vv>v<^^^>>v^v^<^^>v^^>v^<^v>v<>>v^v^<v>v^^<^^vv<
<<v<^>>^^^^>>>v^<>vvv^><v<<<>^^^vv^<vvv>^>v<^^^^v<>^>vvvv><>>v^<<^^^^^
^><^><>>><>^^<<^^v>>><^<v>^<vv>>v>>>^v><>^v><<<<v>>v<v<v>vvv>^<><<>^><
^>><>^v<><^vvv<^^<><v<<<<<><^v<<<><<<^^<v<^^^><^>>^<v^><<<^>>^v<v^v<v^
>^>>^v>vv>^<<^v<>><<><<v<<v><>v<^vv<<<>^^v^>^^>>><<^v>>v^v><^^>>^<>vv^
<><^^>^^^<><vvvvv^v<v<<>^v<v>v<<^><<><<><<<^^<<<^<<>><<><^^^>^^<>^>v<>
^^>vv<^v^v<vv>^<><v<^v>^^^>>>^^vvv^>vvv<>>>^<^>>>>>^<<^v>^vvv<>^<><<v>
v^^>>><<^^<>>^v^<v^vv<>v^<<>^<^v^v><^<<<><<^<v><v<>vv>>v><v^<vv<>v^<<^";

      content = @"#######
#...#.#
#.....#
#..OO@#
#..O..#
#.....#
#######

<vv<<^^<<^^
";

      var segments = content.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

      var map = segments[0].ToCharMap();
      Direction[] directions = GetDirections(segments[1].Replace(Environment.NewLine, ""));

      (HashSet<(int, int)> Walls, HashSet<(int, int)> Boxes, (int, int) Robot) = ParseMap(map);

      foreach (var dir in directions)
      {
         (Robot, _) = Move(Robot, dir, Walls, Boxes);
      }

      long score = ScoreBoxes(Boxes);

      var wideMap = ConvertMapToDoubleWidth(map);

      (Walls, HashSet<((int, int) left, (int, int) right)> BoxesWide, Robot) = ParseMapWide(wideMap);

      foreach (var dir in directions)
      {
         (Robot, _) = MoveWide(Robot, dir, BoxesWide, Walls);
      }

      long wideScore = ScoreBoxes(BoxesWide.Select(x => x.left).ToHashSet());


      return $"Score Part1: {score}, Wide : {wideScore}";
   }

   private static void Move(this HashSet<((int, int), (int, int))> val, Direction dir, (int, int) target)
   {
      ((int, int), (int, int)) item = default;

      foreach (var elem in val)
      {
         if (elem.Item1 == target || elem.Item2 == target)
         {
            item = elem;

         }
      }
      if(item == default)
      {
         return;
      }
      val.Remove(item);

      item = dir switch
      {
         Direction.Up => ((item.Item1.Item1 - 1, item.Item1.Item2), (item.Item2.Item1 - 1, item.Item2.Item2)),
         Direction.Down => ((item.Item1.Item1 + 1, item.Item1.Item2), (item.Item2.Item1 + 1, item.Item2.Item2)),
         Direction.Left => ((item.Item1.Item1, item.Item1.Item2 - 1), (item.Item2.Item1, item.Item2.Item2 - 1)),
         Direction.Right => ((item.Item1.Item1, item.Item1.Item2 + 1), (item.Item2.Item1, item.Item2.Item2 + 1)),
         _ => throw new Exception("Invalid direction")
      };
      val.Add(item);
   }
   private static bool Contains(this HashSet<((int, int), (int, int))> val, (int, int) item)
   {
      foreach (var elem in val)
      {
         if (elem.Item1 == item || elem.Item2 == item)
            return true;
      }
      return false;
   }

   private static ((int, int) newPos, bool hitWall) MoveWide((int, int) toMove, Direction dir, HashSet<((int, int) left, (int, int) right)> boxes, HashSet<(int, int)> walls)
   {
      var target = dir switch
      {
         Direction.Up => (toMove.Item1 - 1, toMove.Item2),
         Direction.Down => (toMove.Item1 + 1, toMove.Item2),
         Direction.Left => (toMove.Item1, toMove.Item2 - 1),
         Direction.Right => (toMove.Item1, toMove.Item2 + 1),
         _ => throw new Exception("Invalid direction")
      };

      if (walls.Contains(target))
         return (toMove, true);

      if (boxes.Contains(target))
      {
         ((int, int) newPos, bool hitWall) temp = MoveWide(target, dir, boxes, walls);

         if (temp.hitWall)
         {
            return (toMove, true);
         }
         else
         {
            boxes.Move(dir, target);
            return (target, false);
         }
      }

      return (target, false);
   }

   private static long ScoreBoxes(HashSet<(int, int)> boxes)
   {
      long sum = 0;
      foreach (var elem in boxes)
      {
         sum += (elem.Item1 * 100) + elem.Item2;
      }
      return sum;
   }

   private static ((int, int) newPos, bool hitWall) Move((int, int) toMove, Direction dir, HashSet<(int, int)> walls, HashSet<(int, int)> boxes)
   {
      var target = dir switch
      {
         Direction.Up => (toMove.Item1 - 1, toMove.Item2),
         Direction.Down => (toMove.Item1 + 1, toMove.Item2),
         Direction.Left => (toMove.Item1, toMove.Item2 - 1),
         Direction.Right => (toMove.Item1, toMove.Item2 + 1),
         _ => throw new Exception("Invalid direction")
      };

      if (walls.Contains(target))
         return (toMove, true);

      if (boxes.Contains(target))
      {
         ((int, int) newPos, bool hitWall) temp = Move(target, dir, walls, boxes);

         if (temp.hitWall)
         {
            return (toMove, true);
         }
         else
         {
            boxes.Remove(target);
            boxes.Add(temp.newPos);
            return (target, false);
         }
      }

      return (target, false);
   }

   private static (HashSet<(int, int)> walls, HashSet<((int, int) left, (int, int) right)> boxes, (int, int) robot) ParseMapWide(char[][] map)
   {
      HashSet<(int, int)> result = new HashSet<(int, int)>();
      HashSet<((int, int), (int, int))> boxes = new();
      (int, int) robot = (0, 0);
      for (int i = 0; i < map.Length; i++)
      {
         for (int j = 0; j < map[i].Length; j++)
         {
            if (map[i][j] == '#')
            {
               result.Add((i, j));
            }
            else if (map[i][j] == '[')
            {
               boxes.Add(((i, j), (i, j + 1)));
               j++;
            }
            else if (map[i][j] == '@')
            {
               robot = (i, j);
            }
         }
      }
      return (result, boxes, robot);
   }
   private static (HashSet<(int, int)> walls, HashSet<(int, int)> boxes, (int, int) robot) ParseMap(char[][] map)
   {
      HashSet<(int, int)> result = new HashSet<(int, int)>();
      HashSet<(int, int)> boxes = new HashSet<(int, int)>();
      (int, int) robot = (0, 0);
      for (int i = 0; i < map.Length; i++)
      {
         for (int j = 0; j < map[i].Length; j++)
         {
            if (map[i][j] == '#')
               result.Add((i, j));
            else if (map[i][j] == 'O')
               boxes.Add((i, j));
            else if (map[i][j] == '@')
               robot = (i, j);
         }
      }
      return (result, boxes, robot);
   }
   private static char[][] ConvertMapToDoubleWidth(char[][] v)
   {
      char[][] map = new char[v.Length][];
      for (int i = 0; i < v.Length; i++)
      {
         var line = v[i];
         char[] newLine = new char[line.Length * 2];
         for (int j = 0; j < line.Length; j++)
         {
            if (line[j] == 'O')
            {
               newLine[j * 2] = '[';
               newLine[j * 2 + 1] = ']';
            }
            else if (line[j] == '@')
            {
               newLine[j * 2] = '@';
               newLine[j * 2 + 1] = '.';
            }
            else
            {
               newLine[j * 2] = line[j];
               newLine[j * 2 + 1] = line[j];
            }
            map[i] = newLine;
         }
      }

      return map;
   }

   private static Direction[] GetDirections(string v)
   {
      Direction[] result = new Direction[v.Length];
      for (int i = 0; i < v.Length; i++)
      {
         result[i] = v[i] switch
         {
            '^' => Direction.Up,
            'v' => Direction.Down,
            '<' => Direction.Left,
            '>' => Direction.Right,
            _ => throw new Exception("Invalid direction")
         };
      }

      return result;
   }
}
