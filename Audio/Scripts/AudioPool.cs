using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioLibrary
{
    [System.Serializable]
    public class AudioPool
    {
        #region Private Editor Fields
        [SerializeField]
        [Tooltip("List of audio sources in the pool")]
        private AudioSource[] sources;
        [SerializeField]
        [Tooltip("Current audio source that will be played " +
            "on the next auto play")]
        private int current = 0;
        #endregion

        #region Private Fields
        private int channelIndex;
        #endregion

        #region Constructors
        public AudioPool(int channelIndex, GameObject parent)
        {
            // Set this template channel
            this.channelIndex = channelIndex;

            // Get the channel referenced by the index
            AudioChannel channel = AudioSettings.GetChannel(channelIndex);

            // Initialize the array
            sources = new AudioSource[channel.AudioSourceCount];

            // Create an object under my transform
            GameObject self = new GameObject($"Output: '{channel.Output.name}'");
            self.transform.SetParent(parent.transform);

            // Create an audio source for each element in the array
            for (int i = 0; i < sources.Length; i++)
            {
                // Create an object for the source and set its parent
                GameObject sourceObject = new GameObject($"Source {i}: '{channel.Output.name}'");
                sourceObject.transform.SetParent(self.transform);

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
                        $"{nameof(AudioSettings)}");
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

            // Continue to loop until we find a source that is not playing
            // or until we loop all the way around to the start again
            while (current != start && sources[current].isPlaying)
            {
                current = (current + 1) % sources.Length;
            }
        }
        #endregion
    }
}
