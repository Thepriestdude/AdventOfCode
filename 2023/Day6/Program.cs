using System.Text.RegularExpressions;

Console.WriteLine("--- Day 6: Wait For It ---");
var input = File.ReadAllLines("input.txt").ToArray();

Console.WriteLine($"part-1 : {First(input)} : <<840336>>");
Console.WriteLine($"part-2 : {Second(input)} : <<41382569>>");
return;

int First(string[] input)
{
    return CreateRaces(input).Aggregate(1, (current, race) => current * race.CalculateFasterTimes().Count);
}

int Second(string[] input)
{
    return CreateRace(input).CalculateFasterTimes().Count;
}

Race CreateRace(string[] input)
{
    var times = GetNumbers(RemoveWhiteSpace(input[0]));
    var recordDistances = GetNumbers(RemoveWhiteSpace(input[1]));
    return new Race(times[0], recordDistances[0]);
}

IEnumerable<Race> CreateRaces(string[] input)
{
    var times = GetNumbers(input[0]);
    var recordDistances = GetNumbers(input[1]);
    return times.Select((t, i) => new Race(t, recordDistances[i])).ToList();
}

string RemoveWhiteSpace(string input) => Regex.Replace(input, @"\s+", "");
long[] GetNumbers(string input) => Regex.Matches(input, "([0-9]+)").Select(x => long.Parse(x.Value)).ToArray();

record Race(long Time, long RecordDistance, long MillimetersPerMillisecond = 1)
{
    public List<long> CalculateFasterTimes()
    {
        var distances = new List<long>();
        for(var i = 0; i < Time; i++)
        {
            var distance = (Time - i) * i * MillimetersPerMillisecond;
            if (distance >= RecordDistance)
            {
               distances.Add(i);
            }
        }

        return distances;
    }
}