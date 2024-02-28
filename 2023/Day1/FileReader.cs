namespace Day1;

public static class FileReader
{
   public static async Task<string[]> ReadAllLinesAsync(string path)
   {
      return await File.ReadAllLinesAsync(path);
   } 
}