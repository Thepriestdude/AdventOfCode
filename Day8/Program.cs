Console.WriteLine("--- Day 8: Haunted Wasteland ---");
var input = File.ReadAllLines("input.txt");
//var input = File.ReadAllLines("small2_input.txt");


//Console.WriteLine($"Part 1: {PartOne(input)} | <<<19667>>>");
Console.WriteLine($"Part 2: {PartTwo(input)}| <<<131298>>>");

int PartOne(string[] input)
{
    var directions = input[0].Select(x => x == 'L' ? 0 : 1).ToArray();
    var steps = 0;

    var node = ParseNode(input.Skip(2).ToList());

    while (true)
    {
        steps += node.Traverse(directions, out node);
        if (node.End) break;
    }
    return steps;
}

long PartTwo(string[] input)
{
    var directions = input[0].Select(x => x == 'L' ? 0 : 1).ToArray();
    var ghostNodes = ParseGhostNodes(input.Skip(2).ToList());

    long steps;
    for (long step = 0;; step++)
    {
        // test to store each step in a hashSet with the step as the key and the bool END as the value, if all are true, we have a winner
        var direction = directions[WrapAroundIndex(step, directions.Length)];
        for (var i = 0; i < ghostNodes.Length; i++)
        {
            ghostNodes[i] = ghostNodes[i].GhostStep(direction);
        }
        
        if (step % 1000000 == 0) Console.WriteLine(step);
        if (ghostNodes.Any(x => !x.End)) continue;
        steps = step;
        break;
    }
    
    return steps + 1;
}

static long WrapAroundIndex(long index, int arrayLength) => (index % arrayLength + arrayLength) % arrayLength;

/*
 * while (true)
   {
       var endReached = false;
       
       foreach (var direction in directions)
       {
           if (endReached) break;
           
           for (var i = 0; i < ghostStartNodes.Length; i++)
           {
               steps += 1;
               ghostStartNodes[i].GhostStepTraverse(direction, out ghostStartNodes[i]);
           }

           if (!ghostStartNodes.All(x => x.End)) continue;
           
           endReached = true;
           break;
       }
       
       if (endReached) break;
 */

Node[] GhostCreateNodeTree(List<Node> nodes)
{
    foreach (var node in nodes)
    {
        node.Left = nodes.First(x => x.Name == node.LeftName);
        node.Right = nodes.First(x => x.Name == node.RightName);
    }
    
    nodes.Where(x => x.Name.EndsWith("Z")).ToList().ForEach(x => x.End = true);
    return nodes.Where(x => x.Name.EndsWith("A")).ToArray();
}

Node[] ParseGhostNodes(List<string> input)
{
    var nodes = input.Select(ParseLine).ToList();
    return GhostCreateNodeTree(nodes);
}

Node ParseNode(List<string> input)
{
    var nodes = input.Select(ParseLine).ToList();
    nodes.First(x => x.Name == "ZZZ").End = true;
    return CreateNodeTree(nodes);
}

Node CreateNodeTree(List<Node> nodes) // always create child nodes only
{
    foreach (var node in nodes)
    {
        node.Left = nodes.First(x => x.Name == node.LeftName);
        node.Right = nodes.First(x => x.Name == node.RightName);
    }
    
    nodes.First(x => x.Name == "ZZZ").End = true;
    return nodes.First(x => x.Name == "AAA");
}

Node ParseLine(string line)
{
    var splitLine = line.Split(" = ");
    var name = splitLine[0];
    var leftRight = splitLine[1]
        .Replace("(", "")
        .Replace(")", "")
        .Split(", ");
    
    return new Node
    {
        Name = name,
        LeftName = leftRight[0],
        RightName = leftRight[1]
    };
}

class Node
{
    public string Name { get; set; }
    public string LeftName { get; set; }
    public string RightName { get; set; }
    public bool End { get; set; }
    public Node? Left { get; set; } = null;
    public Node? Right { get; set; } = null;
    
    public int Traverse(int[] directions, out Node node)
    {
        node = this;
        if (!End && directions.Length > 0)
            return directions[0] == 0
                ? 1 + Left!.Traverse(directions[1..], out node)
                : 1 + Right!.Traverse(directions[1..], out node);
        
        node = this; 
        return 0;
    }
    
    public Node GhostStep(int direction)
    {
        return direction == 0 ?  Left! : Right!;
    }
}

// check what node we will end up at foreach direction loop
