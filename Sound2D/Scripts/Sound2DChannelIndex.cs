using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound2DChannelIndex
{
    #region Public Properties
    public int Index => index;
    public Sound2DChannel Channel => Sound2DSettings.Get(this);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Index of the sound channel in the sound settings")]
    private int index;
    #endregion

    #region Constructors
    public Sound2DChannelIndex(int index)
    {
        this.index = index;
    }
    #endregion
}
