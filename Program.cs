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
            _ => "Day not implemented",
         };

         Console.WriteLine(result);
      }
   }
}
