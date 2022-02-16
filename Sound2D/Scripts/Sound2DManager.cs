using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound2DManager : MonoBehaviour
{
    #region Private Properties
    private static Sound2DManager Instance
    {
        get
        {
            // If no instance exists then create it
            if (!instance) instance = Create();
            return instance;
        }
    }
    #endregion

    #region Private Fields
    private static Sound2DManager instance;
    #endregion

    #region Private Methods
    private void Setup()
    {
        foreach (Sound2DChannel channel in Sound2DSettings.AdditionalChannels)
        {
            // Create an object under my transform
            GameObject parent = new GameObject($"Output: '{channel.Output.name}'");
            parent.transform.SetParent(transform);

            // Create an audio source for each source the channel has
            for (int i = 0; i < channel.AudioSourceCount; i++)
            {
                // Create the source object and parent it under the current parent
                GameObject sourceObject = new GameObject($"Source {i}: '{channel.Output.name}'");
                sourceObject.transform.SetParent(parent.transform);

                // Set the output of the audio source
                AudioSource source = sourceObject.AddComponent<AudioSource>();
                source.outputAudioMixerGroup = channel.Output;
            }
        }
    }
    private static Sound2DManager Create()
    {
        // Create and setup the manager
        GameObject managerObject = new GameObject("Sound 2D Manager");
        Sound2DManager manager = managerObject.AddComponent<Sound2DManager>();
        manager.Setup();
        
        // Make it indestructible on load
        DontDestroyOnLoad(managerObject);

        return manager;
    }
    #endregion
}
