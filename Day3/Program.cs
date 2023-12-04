var inputFileContent = File.ReadAllLines("Input.txt");
Console.WriteLine("First: " + First(inputFileContent) + "(<<559667>>)");

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





