using System.Globalization;

namespace Advent24
{
   internal class Program
   {
      static void Main(string[] args)
      {
         if (!int.TryParse(args[0], CultureInfo.InvariantCulture, out int day))
         {
            Console.WriteLine("Not a valid day");
            return;
         }

         string result = day switch
         {
            1 => Day1.Solve(),
            2 => Day2.Solve(),
            3 => Day3.Solve(),
            4 => Day4.Solve(),
            5 => Day5.Solve(),
            6 => Day6.Solve(),
            _ => "Day not implemented",
         };

         Console.WriteLine(result);
      }
   }
}
