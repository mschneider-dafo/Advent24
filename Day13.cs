using AdventOfCodeLibrary;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Advent24;

internal static class Day13
{
   const long Offsett = 10000000000000;
   public static string Solve()
   {
      var content = File.ReadAllText("Day13.txt");

      var segments = content.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

      long result = 0;
      long p2result = 0;

      foreach (var seg in segments)
      {
         var lines = Std.ToLines(seg);

         var aMove = ParseMove(lines[0]);
         var bMove = ParseMove(lines[1]);
         long[] prize = ParsePrice(lines[2]);

         result += SolveAlgebra(aMove, bMove, prize);
         prize[0] += Offsett;
         prize[1] += Offsett;

         p2result += SolveAlgebra(aMove, bMove, prize);
      }



      return $"Minumum part 1: {result}, Part 2: {p2result}";
   }

   private static long SolveAlgebra(int[] aMove, int[] bMove, long[] prize)
   {
      double prizeX = prize[0];
      double prizeY = prize[1];

      double xOfA = aMove[0];
      double yOfA = aMove[1];
      double xOfB = bMove[0];
      double yOfB = bMove[1];

      double determinant = (xOfA * yOfB) - (yOfA * xOfB);

      if (determinant == 0)
      {
         Console.WriteLine("No unique solution");
         return 0;
      }

      double x = ((prizeX * yOfB) - (prizeY * xOfB)) / determinant;
      double y = ((xOfA * prizeY) - (yOfA * prizeX)) / determinant;

      long resA = 0;
      long resB = 0;

      if ((resA = (long)Math.Floor(x)) != x || (resB = (long)Math.Floor(y)) != y)
      {
         return 0;
      }

      return CalculateCost((resA, resB));
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static long CalculateCost((long A, long B) buttonclicks)
   {

      return (buttonclicks.A * 3) + buttonclicks.B;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static long[] ParsePrice(ReadOnlySpan<char> input)
   {
      var indexX = input.IndexOf("X=");
      if (indexX == -1)
         throw new Exception("X= not found");
      var idxCommata = input.IndexOf(',');
      var indexY = input.IndexOf("Y=");
      if (indexY == -1)
         throw new Exception("Y= not found");

      long[] result = [int.Parse(input[(indexX + 2)..idxCommata]), int.Parse(input[(indexY + 2)..])];
      return result;
   }

   [MethodImpl(MethodImplOptions.AggressiveInlining)]
   private static int[] ParseMove(ReadOnlySpan<char> input)
   {
      var indexX = input.IndexOf("X+");
      if (indexX == -1)
         throw new Exception("X+ not found");
      var idxCommata = input.IndexOf(',');
      var indexY = input.IndexOf("Y+");
      if (indexY == -1)
         throw new Exception("Y+ not found");

      int[] result = [int.Parse(input[(indexX + 2)..idxCommata]), int.Parse(input[(indexY + 2)..])];
      return result;
   }
}
