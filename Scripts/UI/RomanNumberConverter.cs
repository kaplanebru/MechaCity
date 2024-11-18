using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class RomanNumberConverter
{
    public static string IntToRoman(int number)
    {
        if (number <= 0 || number > 3999)
            return null;

        var romanMap = new (int Value, string Symbol)[]
        {
            (1000, "M"), (900, "CM"), (500, "D"), (400, "CD"),
            (100, "C"), (90, "XC"), (50, "L"), (40, "XL"),
            (10, "X"), (9, "IX"), (5, "V"), (4, "IV"),
            (1, "I")
        };

        var result = new StringBuilder();

        foreach (var (value, symbol) in romanMap)
        {
            while (number >= value)
            {
                result.Append(symbol);
                number -= value;
            }
        }

        return result.ToString();
    }
}