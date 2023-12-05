Console.WriteLine("--- Day 4: Scratchcards ---");
var input = File.ReadAllLines("input.txt");

Console.WriteLine("part 1: " + Part1(input) + "<<18619>>");
Console.WriteLine("part 2: " + Part2(input) + "<<8063216>>");

string Part1(string[] input)
{
    var scratchcards = GetAllScratchcards(input);
    return scratchcards!.Sum(x => x.WinningAmounts).ToString();
}

string Part2(string[] input)
{
    var scratchcards = GetAllScratchcards(input).ToArray();
    
    for(var i = scratchcards.Length-1; i >= 0; i--)
    {
        var card = scratchcards[i];
        if (card.WonCopies > 0)
        {
            card.WonScratchcards.AddRange(scratchcards[(i + 1) .. (i + 1 + card.WonCopies)]);
        }
    }
    
    return scratchcards.Sum(x => x.TotalWonCards()).ToString();
}

IEnumerable<Scratchcard> GetAllScratchcards(string[] input)
{
    var rows = input.Select(x => x.Split(": ")[1]);
    foreach (var row in rows)
    {
        var winningAndNumbers = row.Split(" | ");
        var winningNumbers = winningAndNumbers[0].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
        var numbers = winningAndNumbers[1].Split(" ").Where(x => !string.IsNullOrEmpty(x)).Select(int.Parse).ToArray();
        yield return new Scratchcard
        {
            Numbers = numbers,
            WinningNumbers = winningNumbers
        };
    }
}

public record Scratchcard
{
    public required int[] Numbers { get; init; } = Array.Empty<int>();
    public required int[] WinningNumbers { get; init; }  = Array.Empty<int>();
    public int WinningAmounts => (int)Math.Pow(2, WonCopies - 1);
    public int WonCopies => Numbers.Intersect(WinningNumbers).Count();
    public List<Scratchcard> WonScratchcards { get; set; } = new();
    public int TotalWonCards() {
        return 1 + WonScratchcards.Sum(x => x.TotalWonCards());
    }
}
