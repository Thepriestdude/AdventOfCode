var inputFileContent = File.ReadAllLines("Input.txt");
Console.WriteLine("First: " + First(inputFileContent));

int First(string[] input)
{
    var numberHits = new List<int>();
    var hitMatrix = GetHitMatrix(input);
    
    foreach (var line in input.Select((line, y) => (line, y)))
    {
        var potentialNumberIndex = new List<int>();
        var potentialNumber = "";
        foreach (var element in line.line.Select((element, x) => (element, x)))
        {
            if (char.IsDigit(element.element))
            {
                potentialNumberIndex.Add(element.x);
                potentialNumber += element.element;
            }
            else
            {
                if (!string.IsNullOrEmpty(potentialNumber))
                {
                    foreach (var i in potentialNumberIndex)
                    {
                        if (hitMatrix[i, line.y])
                        {
                            numberHits.Add(int.Parse(potentialNumber));
                            break;
                        }
                    }
                    potentialNumberIndex.Clear();
                    potentialNumber = "";
                } 
            }
        }
        if (!string.IsNullOrEmpty(potentialNumber))
        {
            foreach (var i in potentialNumberIndex)
            {
                if (hitMatrix[i, line.y])
                {
                    numberHits.Add(int.Parse(potentialNumber));
                    break;
                }
            }
            potentialNumberIndex.Clear();
            potentialNumber = "";
        }
    }
    
    return numberHits.Sum();
}


//MatrixConsoleWrite(hitMatrix);

void MatrixConsoleWrite(bool[,] matrix)
{
    for (int y = 0; y < matrix.GetLength(1); y++)
    {
        for (int x = 0; x < matrix.GetLength(0); x++)
        {
            Console.Write(matrix[x, y] ? '#' : '.');
        }
        Console.WriteLine();
    }
}

bool[,] GetHitMatrix(string[] schemantics)
{
    var symbolHits = new bool[schemantics[0].Length, schemantics.Length];
    
    foreach(var line in schemantics.Select((line, y) => (line, y)))
    {
        foreach (var element in line.line.Select((element, x) => (element, x)))
        {
            if (!char.IsDigit(element.element) && element.element != '.')
            {
                symbolHits[element.x,   line.y] = true;
                symbolHits[element.x,   line.y+1] = true;
                symbolHits[element.x,   line.y-1] = true;
                symbolHits[element.x+1, line.y] = true;
                symbolHits[element.x+1, line.y+1] = true;
                symbolHits[element.x+1, line.y-1] = true;
                symbolHits[element.x-1, line.y] = true;
                symbolHits[element.x-1, line.y+1] = true;
                symbolHits[element.x-1, line.y-1] = true;
            }
        }
    }

    return symbolHits;
}



