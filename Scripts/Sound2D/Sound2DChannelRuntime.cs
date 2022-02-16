using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound2DChannelRuntime
{
    #region Private Fields
    private Sound2DChannel templateChannel;
    private AudioSource[] sources;
    private int current = 0;
    #endregion

    #region Constructors
    public Sound2DChannelRuntime(Sound2DChannel templateChannel, GameObject parent)
    {
        // Set this template channel
        this.templateChannel = templateChannel;

        // Initialize the array
        sources = new AudioSource[templateChannel.AudioSourceCount];

        // Create an audio source for each element in the array
        for (int i = 0; i < sources.Length; i++)
        {
            // Create an object for the source and set its parent
            GameObject sourceObject = new GameObject($"Source {i}: '{templateChannel.Output.name}'");
            sourceObject.transform.SetParent(parent.transform);

            // Add the audio source and set it up
            sources[i] = sourceObject.AddComponent<AudioSource>();
            sources[i].outputAudioMixerGroup = templateChannel.Output;
            sources[i].playOnAwake = false;
        }
    }
    #endregion

    #region Public Methods

    #endregion
}
