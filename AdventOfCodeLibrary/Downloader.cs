using System.Net;

namespace AdventOfCodeLibrary;
public static class Downloader
{
   public static async Task<string> DownloadInput(int year, int day, string cookie)
   {

      CookieContainer container = new();
      container.Add(new Uri("https://adventofcode.com"), new Cookie("session", cookie));
      var client = new HttpClient(new HttpClientHandler() { CookieContainer = container });

      HttpResponseMessage x = await client.GetAsync($"https://adventofcode.com/{year}/day/{day}/input", CancellationToken.None);

      if (x.IsSuccessStatusCode)
      {
        var content = await x.Content.ReadAsStringAsync();

         return content.ReplaceLineEndings();
      }

      else
      {
         return "";
      }
   }
}
