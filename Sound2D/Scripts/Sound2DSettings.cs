using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Sound2D", fileName = "Sound2DSettings")]
public class Sound2DSettings : ScriptableObjectSingleton<Sound2DSettings>
{
    #region Public Properties
    public static AudioMixerGroup Master => Instance.master;
    public static Sound2DChannel MusicChannel => Instance.musicChannel;
    public static Sound2DChannel SFXChannel => Instance.sfxChannel;
    public static Sound2DChannel[] AdditionalChannels => Instance.additionalChannels;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Audio mixer that all other channels should output to")]
    private AudioMixerGroup master;
    [SerializeField]
    [Tooltip("Setup for the music channel")]
    private Sound2DChannel musicChannel;
    [SerializeField]
    [Tooltip("Setup for the sfx channel")]
    private Sound2DChannel sfxChannel;
    [SerializeField]
    [Tooltip("List of channels to use for the 2D sound")]
    private Sound2DChannel[] additionalChannels = new Sound2DChannel[0];
    #endregion

    #region Public Methods
    public static Sound2DChannel Get(Sound2DChannelIndex index)
    {
        // If the index is 0 then return music channel
        if (index.Index == 0) return MusicChannel;
        // If index is 1 then reteurn sfx channel
        else if (index.Index == 1) return SFXChannel;
        // If something other than 0 or 1 then return an additional channel
        else return Get(new Sound2DAdditionalChannelIndex(index.Index - 2));
    }
    public static Sound2DChannel Get(Sound2DAdditionalChannelIndex index)
    {
        // If index is in range then return it
        if (index.Index >= 0 && index.Index < AdditionalChannels.Length)
        {
            return AdditionalChannels[index.Index];
        }
        // If not found then throw an exception
        else throw new System.IndexOutOfRangeException(
            $"No additional sound channel associated with index '{index.Index}'. " +
            $"Total channels: {AdditionalChannels.Length}");
    }
    #endregion
}
