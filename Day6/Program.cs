using System.Text.RegularExpressions;

Console.WriteLine("--- Day 6: Wait For It ---");
var input = File.ReadAllLines("input.txt").ToArray();

Console.WriteLine($"part-1 : {First(input)} : <<840336>>");
Console.WriteLine($"part-2 : {Second(input)} : <<41382569>>");


int First(string[] input)
{
    var races = CreateRaces(input);
    return races.Aggregate(1, (current, race) => current * race.CalculateFasterTimes().Count);
}

int Second(string[] input)
{
    var race = CreateRace(input);
    return race.CalculateFasterTimes().Count;
}

Race CreateRace(string[] input)
{
    var line1 = RemoveWhiteSpace(input[0]);
    var line2 = RemoveWhiteSpace(input[1]);
    var times = Regex.Matches(line1, "([0-9]+)").Select(x => long.Parse(x.Value)).ToArray();
    var recordDistances = Regex.Matches(line2, "([0-9]+)").Select(x => long.Parse(x.Value)).ToArray();
    return new Race(times[0], recordDistances[0]);
}

string RemoveWhiteSpace(string input) => Regex.Replace(input, @"\s+", "");
long[] GetNumbers(string input) => Regex.Matches(input, "([0-9]+)").Select(x => long.Parse(x.Value)).ToArray();

List<Race> CreateRaces(string[] input)
{
    var times = Regex.Matches(input[0], "([0-9]+)").Select(x => int.Parse(x.Value)).ToArray();
    var recordDistances = Regex.Matches(input[1], "([0-9]+)").Select(x => int.Parse(x.Value)).ToArray();
    return times.Select((t, i) => new Race(t, recordDistances[i])).ToList();
}


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