using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct IntRange
{
    [SerializeField]
    [Tooltip("Minimum value of the float")]
    private int _min;
    [SerializeField]
    [Tooltip("Maximum value of the float")]
    private int _max;

    public int min => _min;
    public int max => _max;
    public int length => _max - _min;

    public IntRange(int min, int max)
    {
        _min = min;
        _max = max;
    }

    public IntRange Intersect(IntRange other)
    {
        int newMin = Mathf.Max(_min, other._min);
        int newMax = Mathf.Min(_max, other._max);
        return new IntRange(newMin, newMax);
    }
}
