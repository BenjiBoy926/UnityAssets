using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioUtility
{
    [System.Serializable]
    public class AudioChannelPool
    {
        #region Private Editor Fields
        [SerializeField]
        [Tooltip("List of audio sources in the pool")]
        private AudioSource[] pool;
        [SerializeField]
        [Tooltip("Game object that each of the audio sources are attached under")]
        private GameObject gameObject;
        [SerializeField]
        [Tooltip("Index of the audio channel in the audio settings")]
        private AudioChannelIndex index;
        [SerializeField]
        [Tooltip("Current audio source that will be played " +
            "on the next auto play")]
        private int current = 0;
        #endregion

        #region Constructors
        public AudioChannelPool(int mixerIndex, int channelIndex, GameObject parent)
        {
            // Set this template channel
            index = (mixerIndex, channelIndex);

            // Get the channel referenced by the index
            AudioChannel channel = AudioSettings.GetChannel(index);

            // Create an object under my transform
            gameObject = new GameObject($"Output {channelIndex}: '{channel.Output}'");
            gameObject.transform.SetParent(parent.transform);

            // Initialize the array
            pool = new AudioSource[channel.AudioSourceCount];

            // Create an audio source for each element in the array
            for (int i = 0; i < pool.Length; i++)
            {
                // Create an object for the source and set its parent
                GameObject sourceObject = new GameObject($"Source {i}");
                sourceObject.transform.SetParent(gameObject.transform);

                // Add the audio source and set it up
                pool[i] = sourceObject.AddComponent<AudioSource>();
                pool[i].outputAudioMixerGroup = channel.Output;
                pool[i].playOnAwake = false;
            }
        }
        #endregion

        #region Play Methods
        public AudioSource Play(AudioClip clip, int sourceIndex = -1, bool looping = false)
        {
            if (sourceIndex < 0 || sourceIndex >= pool.Length)
            {
                sourceIndex = current;
                UpdateCurrent();
            }

            // Get the audio source
            AudioSource source = pool[sourceIndex];
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
        #endregion

        #region Private Methods
        private void UpdateCurrent()
        {
            // Set what current was at the start
            int start = current;

            // Update current to the next source
            current = (current + 1) % pool.Length;

            // Continue to loop until we find a source that is not playing
            // or until we loop all the way around to the start again
            while (current != start && pool[current].isPlaying)
            {
                current = (current + 1) % pool.Length;
            }
        }
        #endregion
    }
}
