using System;
using System.Text.RegularExpressions;

public static class Utils
{
    // Validate if the input string contains only capital letters (A-Z)
    public static bool IsValidInputString(string input)
    {
        return Regex.IsMatch(input, @"^[A-Z]+$") && input.Length <= 40;
    }

    // Validate shift value to be between -25 and 25
    public static bool IsValidShiftValue(int shift)
    {
        return shift >= -25 && shift <= 25;
    }
}
using System;

public class Class1
{
	public Class1()
	{
	}
}
