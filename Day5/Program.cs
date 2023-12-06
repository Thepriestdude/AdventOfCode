Console.WriteLine("--- Day 5: If You Give A Seed A Fertilizer ---");
var input = File.ReadLines("input.txt").ToArray();
//var input = File.ReadLines("small_input.txt").ToArray();

Console.WriteLine($"part 1: {First(input)} : <<214922730>>");
Console.WriteLine($"part 2: {Second(input)} : <<214922730>>");

long First(string[] input)
{
    var seeds = input[0].Split(": ")[1].Split(' ').Select(long.Parse);
    var transformers = GetMapTransformers(input);
    var transformedSeeds = TransformSeeds(seeds, transformers);

    return transformedSeeds.Min();
}

long Second(string[] strings)
{
    var seeds = input[0].Split(": ")[1].Split(' ').Select(long.Parse);
    var seedRanges = new List<SeedRange>();
    var transformers = GetMapTransformers(input);
    
    for (var i = 0; i < seeds.Count(); i +=2)
    {
        var seed = seeds.ElementAt(i);
        var seedRange = seeds.ElementAt(i + 1);
        seedRanges.Add(new SeedRange(seed, seedRange));
    }

    var minSeed = long.MaxValue;
    foreach (var seedRange in seedRanges)
    {
        var seedsInRange = seedRange.GetSeedRange();
        foreach (var seed in seedsInRange)
        {
            var transformedSeed = seed;
            foreach (var transformer in transformers)
            {
                transformedSeed = transformer.Transform(transformedSeed);
                   
            }
            
            if (transformedSeed < minSeed) minSeed = transformedSeed; 
        }
        Console.WriteLine($"seed range: {seedRange} min seed: {minSeed}");
    }

    return minSeed;
}

List<long> TransformSeeds(IEnumerable<long> seeds, List<IMapTransformer> mapTransformers)
{
    var transformedSeeds = new List<long>();
    foreach (var seed in seeds)
    {
        long transformedSeed = seed;
        foreach (var transformer in mapTransformers)
        {
            transformedSeed = transformer.Transform(transformedSeed);
        }
    
        transformedSeeds.Add(transformedSeed);
    }

    return transformedSeeds;
}

List<IMapTransformer> GetMapTransformers(IEnumerable<string> input)
{
    var transformers = new List<IMapTransformer>();
    var maps = new List<Map>();
    foreach (var line in input.Skip(2))
    {
        if (line.Contains("map:")) continue;
        if (string.IsNullOrWhiteSpace(line) || input.Last() == line)
        {
            transformers.Add(new MapTransformer(maps.ToArray()));
            maps.Clear();
            continue;
        }

        var rawMap = line.Split(' ').Select(long.Parse).ToArray();
        maps.Add(new Map(rawMap[0], rawMap[1], rawMap[2]));
    }

    return transformers;
}

class SeedRange(long start, long range)
{
    public long[] GetSeedRange()
    {
        var seedRange = new long[range];
        for (var i = 0; i < range; i++)
        {
            seedRange[i] = start + i;
        }

        return seedRange;
    }
}

class Map
{
    private long destinationStart;
    private long sourceStart;
    private long length;
    
    public Map(long destinationRangeStart, long sourceRangeStart, long rangeLength)
    {
        destinationStart = destinationRangeStart;
        sourceStart = sourceRangeStart;
        length = rangeLength;
    }
    
    public bool TryMap(long value, out long mappedValue)
    {
        if (value < sourceStart || value > sourceStart + length)
        {
            mappedValue = value;
            return false;
        }
        
        mappedValue = destinationStart + (value - sourceStart);
        return true;
    }
}

interface IMapTransformer
{
    long Transform(long value);
}

class MapTransformer(Map[] maps) : IMapTransformer
{
    public long Transform(long value)
    {
        foreach (var map in maps)
        {
            if (map.TryMap(value, out var mappedValue)) return mappedValue;
        }

        return value;
    }
}