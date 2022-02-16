using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound2DChannelRuntime
{
    #region Private Fields
    private Sound2DChannelIndex index;
    private AudioSource[] sources;
    private int current = 0;
    #endregion

    #region Constructors
    public Sound2DChannelRuntime(Sound2DChannelIndex index, GameObject parent)
    {
        // Set this template channel
        this.index = index;

        // Get the channel referenced by the index
        Sound2DChannel channel = index.Channel;

        // Initialize the array
        sources = new AudioSource[channel.AudioSourceCount];

        // Create an audio source for each element in the array
        for (int i = 0; i < sources.Length; i++)
        {
            // Create an object for the source and set its parent
            GameObject sourceObject = new GameObject($"Source {i}: '{channel.Output.name}'");
            sourceObject.transform.SetParent(parent.transform);

            // Add the audio source and set it up
            sources[i] = sourceObject.AddComponent<AudioSource>();
            sources[i].outputAudioMixerGroup = channel.Output;
            sources[i].playOnAwake = false;
        }
    }
    #endregion

    #region Play Methods
    public AudioSource Play(AudioClip clip, bool looping)
    {
        AudioSource source = Play(clip, current, looping);
        UpdateCurrent();

        // Return the source that played the clip
        return source;
    }
    public AudioSource Play(AudioClip clip, int index, bool looping)
    {
        if (index >= 0 && index < sources.Length)
        {
            // Get the audio source
            AudioSource source = sources[index];
            bool wasPlaying = source.isPlaying;

            // Play the source
            source.clip = clip;
            source.loop = looping;
            source.Play();

            // If the source was playing before,
            // log a warning to let the player know
            if (wasPlaying)
            {
                Debug.LogWarning($"The audio source '{source}' " +
                    $"was already playing before you started playing " +
                    $"the audio clip '{clip}' on it. If this was an " +
                    $"auto-play, then the sound channel that outputs " +
                    $"to '{source.outputAudioMixerGroup}' should be " +
                    $"given a higher audio source count on the " +
                    $"Sound2DSettings");
            }

            // Return the source that played the sound
            return source;
        }
        else throw new System.IndexOutOfRangeException(
            $"No audio source associated with index {index}. " +
            $"Total audio sources: {sources.Length}");
    }
    #endregion

    #region Private Methods
    private void UpdateCurrent()
    {
        // Set what current was at the start
        int start = current;
        
        // Update current to the next source
        current = (current + 1) % sources.Length;

        // Continue to loop until we find a source that is not looping
        // or until we loop all the way around to the start again
        while (current != start && sources[current].loop)
        {
            current = (current + 1) % sources.Length;
        }
    }
    #endregion
}