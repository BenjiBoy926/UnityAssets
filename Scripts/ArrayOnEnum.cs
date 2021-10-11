using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArrayOnEnum<TEnum, TData> where TEnum : Enum
{
    #region Public Propeties
    public TData[] Data => data;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Data in the array")]
    private TData[] data;
    #endregion

    #region Constructors
    public ArrayOnEnum()
    {
        string[] names = Enum.GetNames(typeof(TEnum));
        data = new TData[names.Length];
    }
    #endregion

    #region Public Methods
    public TData Get(TEnum e)
    {
        return data[Index(e)];
    }
    public void Set(TEnum e, TData d)
    {
        data[Index(e)] = d;
    }
    public int Index(TEnum e) => Array.IndexOf(Enum.GetValues(typeof(TEnum)), e);
    #endregion
}
