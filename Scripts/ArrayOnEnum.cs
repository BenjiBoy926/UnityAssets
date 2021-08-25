using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArrayOnEnum<TEnum, TData> where TEnum : Enum
{
    [SerializeField]
    [Tooltip("Data in the array")]
    private TData[] data;

    public TData Get(TEnum e)
    {
        Array enumValues = Enum.GetValues(typeof(TEnum));
        int i = Array.IndexOf(enumValues, e);
        return data[i];
    }
}
