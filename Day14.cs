using AdventOfCodeLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Advent24;

internal static class Day14
{
   const int sizey = 103;
   const int sizex = 101;
   public static string Solve()
   {
      var content = File.ReadAllText("Day14.txt");

      var lines = Std.ToLines(content);
      ((int y, int x), (int vy, int vx))[] guards = ParseGuards(lines);

      (int y, int x)[] output = Iterate(guards, 100);

      int center = sizex / 2;
      int middle = sizey / 2;

      var fqad = output.Count(output => output.y < middle && output.x < center);
      var sqad = output.Count(output => output.y < middle && output.x > center);
      var tqad = output.Count(output => output.y > middle && output.x < center);
      var fquad = output.Count(output => output.y > middle && output.x > center);

      int frame = 0;
      List<double> variances = new();
      for (int i = 0; i < 48328; i++)
      {
         output = Iterate(guards, i);
         double x = Variance(output);
         variances.Add(x);
      }

      frame = variances.IndexOf(variances.Min());

      return $"Safety Faktor: {fqad * sqad * tqad * fquad}, Image at {frame}";
   }

   private static double Variance((int y, int x)[] output)
   {
      double meanx = 0;
      double meany = 0;

      double varx = 0;
      double vary = 0;

      for (int i = 0; i < output.Length; i++)
      {
         meanx += output[i].x;
         meany += output[i].y;
      }

      meanx /= output.Length;
      meany /= output.Length;

      for (int i = 0; i < output.Length; i++)
      {
         varx += Math.Pow(output[i].x - meanx, 2);
         vary += Math.Pow(output[i].y - meany, 2);
      }

      return varx * vary;
   }

   private static double CalculateEntropy((int y, int x)[] output)
   {
      double entropy = 0;
      long[] counts = new long[sizey];
      for (int i = 0; i < output.Length; i++)
      {
         counts[output[i].y]++;
         counts[output[i].x]++;
      }

      for (int i = 0; i < counts.Length; i++)
      {
         double count = counts[i];
         if (count > 0)
         {
            var prob = count / output.Length;
            entropy -= prob * Math.Log(prob, sizey);
         }
      }
      return entropy;
   }

   private static string PrintFrame((int y, int x)[] values)
   {
      char[][] map = new char[sizey][];
      for (int i = 0; i < sizey; i++)
      {
         map[i] = new char[sizex];
         for (int j = 0; j < sizex; j++)
         {
            if (values.Any(x => x.y == i && x.x == j))
            {
               map[i][j] = '#';
            }
            else
            {
               map[i][j] = '.';
            }
         }
      }

      return String.Join(Environment.NewLine, map.Select(x => new string(x)));

   }


   private static (int y, int x)[] Iterate(((int y, int x) pos, (int vy, int vx) velo)[] guards, int amount)
   {
      var result = new (int y, int x)[guards.Length];

      for (int i = 0; i < guards.Length; i++)
      {
         var guard = guards[i];
         int ypos = (guard.Item1.y + (guard.Item2.vy * amount)) % sizey;
         if (ypos < 0)
            ypos = sizey + ypos;
         int xpos = (guard.Item1.x + (guard.Item2.vx * amount)) % sizex;
         if (xpos < 0)
            xpos = sizex + xpos;

         result[i] = (ypos, xpos);


      }

      return result;
   }

   private static ((int y, int x), (int vy, int vx))[] ParseGuards(string[] lines)
   {
      ((int, int), (int, int))[] result = new ((int, int), (int, int))[lines.Length];
      for (int i = 0; i < lines.Length; i++)
      {
         var parts = lines[i].Split(" ");
         (int, int) pos, vel;
         var posarr = parts[0].Split('=')[1].Split(',').Select(int.Parse).ToArray();
         var velarr = parts[1].Split('=')[1].Split(',').Select(int.Parse).ToArray();

         pos = (posarr[1], posarr[0]);
         vel = (velarr[1], velarr[0]);

         result[i] = (pos, vel);
      }

      return result;
   }
}
