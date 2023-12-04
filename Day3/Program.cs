var inputFileContent = File.ReadAllLines("Input.txt");
Console.WriteLine("First: " + First(inputFileContent) + "(<<559667>>)");
Console.WriteLine("Second: " + Second(inputFileContent) + "(<<86841457>>)");

int First(string[] input)
{
    var symbols = GetAllPointOfInterests(input, x => !char.IsDigit(x) && x != '.');
    var numbers = GetAllNumbers(input);
    
    foreach (var symbol in symbols)
    {
        foreach (var number in numbers)
        {
            if (number.X.Any(x => symbol.IsAdjacentToCell(x, number.Y)))
                symbol.AdjacentNumbers.Add(number.Value);
        }
    }
    
    return symbols.Select(x => x.SymbolValue).Sum();
}

int Second(string[] input)
{
    var gears = GetAllPointOfInterests(input, x => x == '*'); 
    var numbers = GetAllNumbers(input);
    
    foreach (var gear in gears)
    {
        foreach (var number in numbers)
        {
           if (number.X.Any(x => gear.IsAdjacentToCell(x, number.Y)))
               gear.AdjacentNumbers.Add(number.Value);
        }
    }
    
    return gears.Where(x => x.IsValidGear).Select(x => x.GearValue).Sum();
}

List<PointOfInterest> GetAllPointOfInterests(string[] input, Predicate<char> predicate)
{
    var gears = new List<PointOfInterest>();
    
    foreach (var (line, y) in input.Select((l, y) => (l, y)))
    {
        foreach (var (element, x) in line.Select((e, x) => (e, x)))
        {
            if (predicate.Invoke(element)) gears.Add(new PointOfInterest{X = x, Y = y});
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

internal class PointOfInterest
{
    public int X { get; set;  }
    public int Y { get; set; }
    public List<int> AdjacentNumbers { get; } = new ();
    public bool IsValidGear => AdjacentNumbers.Count == 2;
    public int GearValue => AdjacentNumbers.Aggregate((sum, val) => sum * val);
    public int SymbolValue => AdjacentNumbers.Sum();
    public bool IsAdjacentToCell(int x, int y) => Math.Abs(X - x) <= 1 && Math.Abs(Y - y) <= 1;
}
internal record struct Number(int Y, List<int> X, int Value) { }





