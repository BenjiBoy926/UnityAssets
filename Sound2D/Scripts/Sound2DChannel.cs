using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public struct Sound2DChannel
{
    #region Public Properties
    public AudioMixerGroup Output => output;
    public int AudioSourceCount => audioSourceCount;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Reference to the audio mixer this channel outputs to")]
    private AudioMixerGroup output;
    [SerializeField]
    [Tooltip("Number of audio sources pooled for this channel. " +
        "This number should be higher for channels where many " +
        "concurrent sounds are expected to play, such as a channel " +
        "for sound effects")]
    private int audioSourceCount;
    #endregion
}
