using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StringExtensions
{
    public static string Repeat(this string str, int count)
    {
        return new StringBuilder(str.Length * count)
            .Insert(0, str, count)
            .ToString();
    }
}
