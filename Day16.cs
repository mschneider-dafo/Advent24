using AdventOfCodeLibrary;
using System.Diagnostics;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Advent24;

public enum ReindeerDirection
{
   Up,
   Down,
   Left,
   Right
}
internal static class Day16
{
   public static string Solve()
   {
      var content = File.ReadAllText("Day16.txt");

      /*
      content = @"###############
#.......#....E#
#.#.###.#.###.#
#.....#.#...#.#
#.###.#####.#.#
#.#.#.......#.#
#.#.#####.###.#
#...........#.#
###.#.#####.#.#
#...#.....#.#.#
#.#.#.###.#.#.#
#.....#...#.#.#
#.###.#.#.#.#.#
#S..#.....#...#
###############";
      */

      var map = content.ToCharMap();

      var start = map.FindLocation('S');
      var end = map.FindLocation('E');

      Debug.Assert(start != (-1, -1));
      Debug.Assert(end != (-1, -1));

      var cur = new ReindeerPath()
      {
         Direction = ReindeerDirection.Right,
         Location = start,
         Score = 0,
         Visited = [start],
      };


      Dictionary<(int y, int x), long> visited = new()
      {
         { cur.Location, 0 }
      };

      PriorityQueue<ReindeerPath, long> queue = new();
      queue.Enqueue(cur, 0);

      List<ReindeerPath> paths = new(100);

      long maxScore = int.MaxValue;
      while (queue.Count > 0)
      {
         var relevant = queue.Dequeue();

         (ReindeerPath? a, ReindeerPath? b, ReindeerPath? c) = GetTargets(relevant, map);

         if (a.HasValue)
         {

            ReindeerPath reindeer = a.Value;

            if (maxScore < reindeer.Score)
               continue;

            reindeer.Visited = [.. relevant.Visited, reindeer.Location];

            if (visited.TryGetValue(reindeer.Location, out long score) && (score + 1050) < reindeer.Score)
            {
               continue;
            }

            if (score == 0)
            {
               visited.Add(reindeer.Location, reindeer.Score);
            }
            else if (score > reindeer.Score)
            {
               visited[reindeer.Location] = reindeer.Score;
            }

            if (reindeer.Location == end)
            {
               if (maxScore > reindeer.Score)
               {
                  maxScore = reindeer.Score;
               }
               paths.Add(reindeer);
               continue;
            }


            queue.Enqueue(reindeer, reindeer.Score);
         }
         if (b.HasValue)
         {

            ReindeerPath reindeer = b.Value;

            if (maxScore < reindeer.Score)
               continue;

            reindeer.Visited = [.. relevant.Visited, reindeer.Location];

            if (visited.TryGetValue(reindeer.Location, out long score) && (score + 1050) < reindeer.Score)
            {
               continue;
            }

            if (score == 0)
            {
               visited.Add(reindeer.Location, reindeer.Score);
            }
            else if (score > reindeer.Score)
            {
               visited[reindeer.Location] = reindeer.Score;
            }

            if (reindeer.Location == end)
            {
               if (maxScore > reindeer.Score)
               {
                  maxScore = reindeer.Score;
               }
               paths.Add(reindeer);
               continue;
            }


            queue.Enqueue(reindeer, reindeer.Score);
         }
         if (c.HasValue)
         {

            ReindeerPath reindeer = c.Value;

            if (maxScore < reindeer.Score)
               continue;

            if (visited.TryGetValue(reindeer.Location, out long score) && (score + 1050) < reindeer.Score)
            {
               continue;
            }

            if (score == 0)
            {
               visited.Add(reindeer.Location, reindeer.Score);
            }
            else if (score > reindeer.Score)
            {
               visited[reindeer.Location] = reindeer.Score;
            }

            if (reindeer.Location == end)
            {
               if (maxScore > reindeer.Score)
               {
                  maxScore = reindeer.Score;
               }
               paths.Add(reindeer);
               continue;
            }


            queue.Enqueue(reindeer, reindeer.Score);
         }
      }

      var x = paths.Select(x => x.Visited);

      var uniques = x.SelectMany(x => x).Distinct().Count();


      var count = uniques;

      return $"Part1 Score: {maxScore}, Uniques: {count}";
   }

   private static (ReindeerPath?, ReindeerPath?, ReindeerPath?) GetTargets(ReindeerPath relevant, char[][] map)
   {
      ReindeerPath? cont = null, ninety = null, twoSeventy = null;
      switch (relevant.Direction)
      {
         case ReindeerDirection.Up:
            if (map[relevant.Location.y - 1][relevant.Location.x] != '#')
            {
               cont = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Up,
                  Location = (relevant.Location.y - 1, relevant.Location.x),
                  Score = relevant.Score + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y - 1, relevant.Location.x)]
               };
            }
            if (map[relevant.Location.y][relevant.Location.x + 1] != '#')
            {
               ninety = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Right,
                  Location = (relevant.Location.y, relevant.Location.x + 1),
                  Score = relevant.Score + 1000 + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y, relevant.Location.x + 1)]
               };
            }
            if (map[relevant.Location.y][relevant.Location.x - 1] != '#')
            {
               twoSeventy = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Left,
                  Location = (relevant.Location.y, relevant.Location.x - 1),
                  Score = relevant.Score + 1000 + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y, relevant.Location.x - 1)]
               };
            }
            break;
         case ReindeerDirection.Down:
            if (map[relevant.Location.y + 1][relevant.Location.x] != '#')
            {
               cont = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Down,
                  Location = (relevant.Location.y + 1, relevant.Location.x),
                  Score = relevant.Score + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y + 1, relevant.Location.x)]
               };
            }
            if (map[relevant.Location.y][relevant.Location.x - 1] != '#')
            {
               ninety = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Left,
                  Location = (relevant.Location.y, relevant.Location.x - 1),
                  Score = relevant.Score + 1000 + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y, relevant.Location.x - 1)]
               };
            }
            if (map[relevant.Location.y][relevant.Location.x + 1] != '#')
            {
               twoSeventy = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Right,
                  Location = (relevant.Location.y, relevant.Location.x + 1),
                  Score = relevant.Score + 1000 + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y, relevant.Location.x + 1)]
               };
            }
            break;
         case ReindeerDirection.Left:
            if (map[relevant.Location.y][relevant.Location.x - 1] != '#')
            {
               cont = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Left,
                  Location = (relevant.Location.y, relevant.Location.x - 1),
                  Score = relevant.Score + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y, relevant.Location.x - 1)]
               };
            }
            if (map[relevant.Location.y - 1][relevant.Location.x] != '#')
            {
               ninety = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Up,
                  Location = (relevant.Location.y - 1, relevant.Location.x),
                  Score = relevant.Score + 1000 + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y - 1, relevant.Location.x)]
               };
            }
            if (map[relevant.Location.y + 1][relevant.Location.x] != '#')
            {
               twoSeventy = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Down,
                  Location = (relevant.Location.y + 1, relevant.Location.x),
                  Score = relevant.Score + 1000 + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y + 1, relevant.Location.x)]
               };
            }
            break;
         case ReindeerDirection.Right:
            if (map[relevant.Location.y][relevant.Location.x + 1] != '#')
            {
               cont = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Right,
                  Location = (relevant.Location.y, relevant.Location.x + 1),
                  Score = relevant.Score + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y, relevant.Location.x + 1)]
               };
            }
            if (map[relevant.Location.y + 1][relevant.Location.x] != '#')
            {
               ninety = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Down,
                  Location = (relevant.Location.y + 1, relevant.Location.x),
                  Score = relevant.Score + 1000 + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y + 1, relevant.Location.x)]
               };
            }
            if (map[relevant.Location.y - 1][relevant.Location.x] != '#')
            {
               twoSeventy = new ReindeerPath()
               {
                  Direction = ReindeerDirection.Up,
                  Location = (relevant.Location.y - 1, relevant.Location.x),
                  Score = relevant.Score + 1000 + 1,
                  Visited = [.. relevant.Visited, (relevant.Location.y - 1, relevant.Location.x)]
               };
            }
            break;
         default:
            throw new InvalidOperationException();
      }

      return (cont, ninety, twoSeventy);
   }
}

public struct ReindeerPath
{
   public ReindeerDirection Direction;
   public long Score;
   public (int y, int x) Location;
   public List<(int y, int x)> Visited;
}
