using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound2DAdditionalChannelIndex
{
    #region Public Properties
    public int Index => index;
    public Sound2DChannel Channel => Sound2DSettings.Get(this);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Index of the additional channel")]
    private int index;
    #endregion

    #region Constructors
    public Sound2DAdditionalChannelIndex(int index)
    {
        this.index = index;
    }
    #endregion
}
