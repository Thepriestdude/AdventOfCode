using System.Diagnostics.CodeAnalysis;

var inputFileContent = File.ReadAllLines("Input.txt");
Console.WriteLine("First: " + First(inputFileContent) + "(<<559667>>)");
Console.WriteLine("Second: " + Second(inputFileContent) + "(<<86841457>>)");

#region part1
int First(string[] input)
{
    var numberHits = new List<int>();
    var hitMatrix = CalculateHitMatrix(input, x => !char.IsDigit(x) && x != '.');
    
    foreach (var (line, y) in input.Select((line, y) => (line, y)))
    {
        ProcessLine(line, hitMatrix, y, numberHits);
    }
    
    return numberHits.Sum();
}

void ProcessLine(string s, bool[,] hitMatrix, int y, List<int> numberHits)
{
    var potentialNumberIndex = new List<int>();
    var potentialNumber = "";
    
    foreach (var element in s.Select((element, x) => (element, x)))
    {
        if (char.IsDigit(element.element))
        {
            potentialNumberIndex.Add(element.x);
            potentialNumber += element.element;
        }
        else
        {
            if (string.IsNullOrEmpty(potentialNumber)) continue;
            if (potentialNumberIndex.Any(i => hitMatrix[i, y])) numberHits.Add(int.Parse(potentialNumber));

            potentialNumberIndex.Clear();
            potentialNumber = "";
        }
    }

    if (string.IsNullOrEmpty(potentialNumber)) return;
    if (potentialNumberIndex.Any(i => hitMatrix[i, y])) numberHits.Add(int.Parse(potentialNumber));
}

bool[,] CalculateHitMatrix(string[] schematics, Predicate<char> predicate)
{
    var symbolHits = new bool[schematics[0].Length, schematics.Length];
    
    foreach(var (line, y) in schematics.Select((line, y) => (line, y)))
    {
        foreach (var element in line.Select((element, x) => (element, x)))
        {
            if (predicate(element.element))
            {
                SetHitsAroundCharacter(symbolHits, element.x, y);
            }
        }
    }
    
    return symbolHits;
}

static void SetHitsAroundCharacter(bool[,] hitMatrix, int x, int y)
{
    for (var yOffset = -1; yOffset <= 1; yOffset++)
    for (var xOffset = -1; xOffset <= 1; xOffset++)
        hitMatrix[x + xOffset, y + yOffset] = true;
}


#endregion

int Second(string[] input)
{
    var gears = GetAllGears(input); 
    var numbers = GetAllNumbers(input);
    
    foreach (var gear in gears)
    {
        foreach (var number in numbers)
        {
           if (number.X.Any(x => gear.IsAdjacentToCell(x, number.Y)))
               gear.AdjacentNumbers.Add(number.Value);
        }
    }
    
    return gears.Where(x => x.IsValid).Select(x => x.GearValue).Sum();
}

List<Gear> GetAllGears(string[] input)
{
    var gears = new List<Gear>();
    
    foreach (var (line, y) in input.Select((l, y) => (l, y)))
    {
        foreach (var (element, x) in line.Select((e, x) => (e, x)))
        {
            if (element == '*') gears.Add(new Gear{X = x, Y = y});
        }
    }

    return gears;
}

List<Number> GetAllNumbers(string[] input)
{
    var numbers = new List<Number>();

    foreach (var (row, y) in input.Select((r, y) => (r, y)))
    {
        var potentialNumberIndexes = new List<int>();
        var potentialNumber = "";
        
        foreach (var (element, x) in row.Select((e, x) => (e, x)))
        {
            if (char.IsDigit(element))
            {
                potentialNumberIndexes.Add(x);
                potentialNumber += element;
            }
            else
            {
                if (string.IsNullOrEmpty(potentialNumber)) continue;
                numbers.Add(new Number(y, potentialNumberIndexes.Select(v => v).ToList(), int.Parse(potentialNumber)));
        
                potentialNumberIndexes.Clear();
                potentialNumber = "";
            }
        }
        
        if (!string.IsNullOrEmpty(potentialNumber))
            numbers.Add(new Number(
                y, 
                potentialNumberIndexes.Select(v => v).ToList(), 
                int.Parse(potentialNumber)));;
        
    }

    return numbers;
}

internal class Gear
{
    public int X { get; set;  }
    public int Y { get; set; }
    public List<int> AdjacentNumbers { get; set; } = new ();
    public bool IsValid => AdjacentNumbers.Count == 2;
    public int GearValue => AdjacentNumbers.Aggregate((sum, val) => sum * val);
    public bool IsAdjacentToCell(int x, int y) => Math.Abs(X - x) <= 1 && Math.Abs(Y - y) <= 1;
}
internal record struct Number(int Y, List<int> X, int Value) { }





