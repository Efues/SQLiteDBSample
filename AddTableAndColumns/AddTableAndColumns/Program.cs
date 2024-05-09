using System;
using System.IO;

namespace AddTableAndColumns
{
  internal class Program
  {
    static void Main(string[] args)
    {
      var dbFilePath = @"../../DBFile/Sample.db";
      if (!File.Exists(dbFilePath))
      {
        Console.WriteLine($"Cannot find {dbFilePath}");
        return;
      }

      try
      {
        { // copy backup 
          var dtNow = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss");
          var dstName = $"backup_{dtNow}.db";
          var dstPath = Path.Combine(Path.GetDirectoryName(dbFilePath), dstName);
          File.Copy(dbFilePath, dstPath, true);
        }

        var updator = new Updator(dbFilePath);
        updator.Execute();
      }
      catch (Exception ex)
      {
        Console.WriteLine($"Unhandled exception.");
        Console.WriteLine($"{ex.Message}");
        return;
      }
    }
  }
}
