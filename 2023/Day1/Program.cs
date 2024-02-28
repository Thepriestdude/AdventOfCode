using Day1;

var lines = await FileReader.ReadAllLinesAsync("./TrebuchetCoordinates.txt");
var decodedNumbers = new List<int>();
foreach (var line in lines)
{
    var translatedLine = TranslateElfNumbers(line);
    var firstNumber = translatedLine.First(x => int.TryParse(x.ToString(), out _));
    var secondNumber = translatedLine.Last(x => int.TryParse(x.ToString(), out _));
    decodedNumbers.Add(int.Parse(firstNumber.ToString() + secondNumber.ToString()));
}

Console.WriteLine(decodedNumbers.Sum());

string TranslateElfNumbers(string line)
{
   var newLine = line.Replace("one", "o1e");
   newLine = newLine.Replace("two", "t2o");
   newLine = newLine.Replace("three", "t3e");
   newLine = newLine.Replace("four", "four4r");
   newLine = newLine.Replace("five", "5e");
   newLine = newLine.Replace("six", "s6x");
   newLine = newLine.Replace("seven", "s7n");
   newLine = newLine.Replace("eight", "e8t");
   return newLine.Replace("nine", "n9e");
}