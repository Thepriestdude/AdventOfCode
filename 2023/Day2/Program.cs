var games = File.ReadAllLines("input.txt").Select(x => x.Split(": ").Last().Trim()).ToList();

var gameId = 1;
var possibleGames = new List<int>();
var gamePowers = new List<int>();

foreach (var game in games)
{
    var sets = game.Split(";");
    var maxBlue = 0;
    var maxRed = 0;
    var maxGreen = 0;
    var minBlue = 1;
    var minRed = 1;
    var minGreen = 1;
    
    foreach (var set in sets)
    {
        var revealedCubes = set.Split(", ");

        foreach (var nrForColor in revealedCubes)
        {
            if (nrForColor.Contains("blue"))
            {
                var number = int.Parse(nrForColor.Replace("blue", ""));
                if (number > maxBlue) maxBlue = number;
                if (number < minBlue) minBlue = number;
            }
            
            else if (nrForColor.Contains("red"))
            {
                var number = int.Parse(nrForColor.Replace("red", ""));
                if (number > maxRed) maxRed = number;
                if (number < minRed) minRed = number;
                
            }
            
            else if (nrForColor.Contains("green"))
            {
                var number = int.Parse(nrForColor.Replace("green", ""));
                if (number > maxGreen) maxGreen = number;
                if (number < minGreen) minGreen = number;
            }
        }
    }
    
    gamePowers.Add(maxBlue * maxRed * maxGreen);
    if (maxRed < 13 && maxGreen < 14 && maxBlue < 15)
    {
        possibleGames.Add(gameId);
    }
    
    gameId++;
}

Console.WriteLine("possibleGames: " + possibleGames.Sum());
Console.WriteLine("power: " + gamePowers.Sum());