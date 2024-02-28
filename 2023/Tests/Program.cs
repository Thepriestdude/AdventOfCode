using System;

class Program
{
    static void Main()
    {
        string input = "LRRLRRRLLRRLRRLRRRLRLRRLRRLRRRLRRRLRRLRLLRRLRLRRLRRLRLRLRRLRRLRRRLLRLLRRLRLRRRLRRRLLRRRLRRLRLLRRLRRRLRLLRLRLLRRRLRLRRRLLRRRLRRRLRRLLRLRLLRRLRRLLRRRLLRLLRRLRRRLRLRRRLRLRRLRLRLRRLRRLRRLLLRRRLRLRLLLRRRLLRLRRLRRRLRRLRRLRRRLRRRLRRLLRLLRRLRRRLLRRRLRLRLRRRLRRRLRRLRRLRLLRLRRLLRRLLRRRR";

        FindLongestRepeatingPattern(input);
    }

    static void FindLongestRepeatingPattern(string input)
    {
        int length = input.Length;
        string longestRepeatingPattern = "";
        
        for (int patternLength = 1; patternLength <= length / 2; patternLength++)
        {
            for (int i = 0; i < length - patternLength; i++)
            {
                string pattern = input.Substring(i, patternLength);
                int nextIndex = input.IndexOf(pattern, i + patternLength);

                if (nextIndex != -1)
                {
                    if (pattern.Length > longestRepeatingPattern.Length)
                    {
                        longestRepeatingPattern = pattern;
                    }
                }
            }
        }

        if (longestRepeatingPattern.Length > 0)
        {
            Console.WriteLine($"Longest repeating pattern found: {longestRepeatingPattern}");
        }
        else
        {
            Console.WriteLine("No repeating patterns found.");
        }
    }
}