using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringModule
{
    public const float WORDS_PER_MINUTE = 100;
    public static float wordsPerSecond => WORDS_PER_MINUTE / 60f;

    // Estimate the amount of seconds it would take to read the given string,
    // based on a slow 100 wpm rate
    public static float EstimateReadTime(this string str)
    {
        return str.Split(' ').Length * wordsPerSecond;
    }
    /// <summary>
    /// Get substring. If there is text before or after the substring in the original string,
    /// appends ellipsis '...' on the end
    /// </summary>
    /// <param name="str"></param>
    /// <param name="index"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public static string RedactedSubstring(this string str, int index, int length)
    {
        string result = "";
        index = Mathf.Max(index, 0);
        if (index > 0) result += "...";
        if (index + length >= str.Length)
            result += str.Substring(index);
        else
            result += $"{str.Substring(index, length)}...";
        return result;
    }
}
