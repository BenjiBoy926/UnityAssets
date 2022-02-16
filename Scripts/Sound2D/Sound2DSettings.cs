using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Sound2D", fileName = "Sound2DSettings")]
public class Sound2DSettings : ScriptableObjectSingleton<Sound2DSettings>
{
    #region Public Properties
    public static Sound2DChannel[] Channels => Instance.channels;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("List of channels to use for the 2D sound")]
    private Sound2DChannel[] channels = new Sound2DChannel[0];
    #endregion

    #region Public Methods
    public static Sound2DChannel Get(int index)
    {
        if (index >= 0 && index < Instance.channels.Length)
        {
            return Instance.channels[index];
        }
        else throw new System.IndexOutOfRangeException(
            $"No sound channel exists at index {index}. " +
            $"Total channels: {Instance.channels.Length}");
    }
    #endregion
}
