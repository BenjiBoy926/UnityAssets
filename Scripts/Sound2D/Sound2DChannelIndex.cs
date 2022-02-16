using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Sound2DChannelIndex
{
    #region Public Properties
    public int ChannelIndex => channelIndex;
    public Sound2DChannel Channel => Sound2DSettings.Get(channelIndex);
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Index of the sound channel in the sound settings")]
    private int channelIndex;
    #endregion
}
