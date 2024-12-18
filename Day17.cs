using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Advent24
{
   enum OpCode
   {
      adv = 0,
      bxl = 1,
      bst = 2,
      jnz = 3,
      bxc = 4,
      outcmd = 5,
      bdv = 6,
      cdv = 7,
   }
   public static class Day17
   {
      [ThreadStatic]
      private static long A;
      [ThreadStatic]
      private static long B;
      [ThreadStatic]
      private static long C;

      [ThreadStatic]
      private static long idx = 0;

      [ThreadStatic]
      private static List<long> results = new();

      public static string Solve()
      {
         var content = File.ReadAllText("Day17.txt");
         /*
         content = @"Register A: 2024
Register B: 0
Register C: 0

Program: 0,3,5,4,3,0";
         */

         results ??= new();
         var segments = content.Split(Environment.NewLine + Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

         long[] registers = segments[0].Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => long.Parse(x.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1])).ToArray();
         A = registers[0];
         B = registers[1];
         C = registers[2];

         long[] commands = segments[1].Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)[1].Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(long.Parse).ToArray();

         while (idx < commands.Length - 1)
         {
            var opCode = (OpCode)commands[idx];
            var operand = commands[idx + 1];

            if (ExecuteOpCode(opCode, operand))
            {
               idx += 2;
            }
         }

         string resString = String.Join(',', results);





         long Aval = Backwards(commands).Min();



         return $"Part1 :{resString}, Part2: {Aval}";
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static IEnumerable<long> Backwards(long[] output)
      {
         if (output.Length == 0)
         {
            yield return 0;
            yield break;
         }

         foreach (var item in Backwards(output[1..]))
         {
            for (int i = 0; i < 8; i++)
            {
               //Shift item by 3 bits to the left and add i, this creates an A value
               var valToTest = (item << 3) + i;
               //Get the 3 last bits of A
               long Alow = i;
               //xOr with 3
               Alow ^= 3;
               //Shift right by the value of the last 3 bits of A
               var c = valToTest >> (int)Alow;
               //Xor with c
               Alow = Alow ^ c;
               //Xor with 5
               Alow = Alow ^ 5;

               if (output[0] == (Alow & 7))
               {
                  yield return valToTest;
               }
            }
         }

      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool ExecuteOpCode(OpCode opCode, long operand)
      {
         return opCode switch
         {
            OpCode.adv => ADivision(operand),
            OpCode.bxl => Bxor(operand),
            OpCode.bst => BModuloEight(operand),
            OpCode.jnz => Jump(operand),
            OpCode.bxc => BCxor(operand),
            OpCode.outcmd => CommandOut(operand),
            OpCode.bdv => BDivision(operand),
            OpCode.cdv => CDivision(operand),
            _ => throw new Exception("NEVER"),
         };
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool CDivision(long operand)
      {
         operand = FindComboOperand(operand);
         var pow = Math.Pow(2, operand);

         C = (long)Math.Truncate(A / pow);
         return true;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool BDivision(long operand)
      {
         operand = FindComboOperand(operand);
         var pow = Math.Pow(2, operand);

         B = (long)Math.Truncate(A / pow);
         return true;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool CommandOut(long operand)
      {
         var combo = FindComboOperand(operand);

         results.Add(combo & 7);

         return true;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool BCxor(long operand)
      {
         B ^= C;

         return true;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool Jump(long operand)
      {
         if (A == 0)
            return true;

         idx = operand;
         return false;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool BModuloEight(long operand)
      {
         var combo = FindComboOperand(operand);
         B = combo & 7;

         return true;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool Bxor(long operand)
      {
         B ^= operand;

         return true;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static bool ADivision(long operand)
      {
         operand = FindComboOperand(operand);
         var pow = Math.Pow(2, operand);

         A = (long)Math.Truncate(A / pow);
         return true;
      }

      [MethodImpl(MethodImplOptions.AggressiveInlining)]
      private static long FindComboOperand(long input)
      {
         return input switch
         {
            >= 0 and <= 3 => input,
            4 => A,
            5 => B,
            6 => C,
            7 => throw new InvalidOperationException(),
            _ => throw new Exception("NEVER"),
         };
      }
   }
}
