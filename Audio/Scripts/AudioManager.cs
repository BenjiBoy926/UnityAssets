using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioLibrary
{
    public class AudioManager : MonoBehaviour
    {
        #region Private Properties
        private static AudioManager Instance
        {
            get
            {
                // If no instance exists then create it
                if (!instance) instance = Create();
                return instance;
            }
        }
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("List of all audio pools used to play audio")]
        private AudioPool[] audioPools = new AudioPool[0];
        #endregion

        #region Private Fields
        private static AudioManager instance;
        #endregion

        #region Private Methods
        private void Setup()
        {
            // Create an audio pool for each audio channel
            audioPools = new AudioPool[AudioSettings.ChannelCount];

            // Setup an audio pool for every audio channel
            for (int i = 0; i < audioPools.Length; i++)
            {
                audioPools[i] = new AudioPool(i, gameObject);
            }
        }
        private static AudioManager Create()
        {
            // Create and setup the manager
            GameObject managerObject = new GameObject("Audio Manager");
            AudioManager manager = managerObject.AddComponent<AudioManager>();
            manager.Setup();

            // Make it indestructible on load
            DontDestroyOnLoad(managerObject);

            return manager;
        }
        #endregion

        #region Play Methods
        // Music
        public static AudioSource PlayMusic(AudioClip clip, bool looping = false)
        {
            return Play(clip, 0, looping);
        }
        public static AudioSource PlayMusic(AudioClip clip, int sourceIndex, bool looping = false)
        {
            return Play(clip, 0, sourceIndex, looping);
        }

        // SFX
        public static AudioSource PlaySFX(AudioClip clip, bool looping = false)
        {
            return Play(clip, 1, looping);
        }
        public static AudioSource PlaySFX(AudioClip clip, int sourceIndex, bool looping = false)
        {
            return Play(clip, 1, sourceIndex, looping);
        }

        // General play
        public static AudioSource Play(AudioClip clip, AudioChannelIndex index, bool looping = false)
        {
            return Play(clip, index.Index, looping);
        }
        public static AudioSource Play(AudioClip clip, AudioChannelIndex index, int sourceIndex, bool looping = false)
        {
            return Play(clip, index.Index, sourceIndex, looping);
        }
        public static AudioSource Play(AudioClip clip, int poolIndex, bool looping = false)
        {
            // If the index is in range then play from the pool
            if (poolIndex >= 0 && poolIndex < Instance.audioPools.Length)
            {
                return Instance.audioPools[poolIndex].Play(clip, looping);
            }
            // throw an exception if the index is out of range
            else throw new System.IndexOutOfRangeException(
                $"No audio pool associated with {poolIndex}. " +
                $"Total pools: {Instance.audioPools}");
        }
        public static AudioSource Play(AudioClip clip, int poolIndex, int sourceIndex, bool looping = false)
        {
            // If the index is in range then play from the pool
            if (poolIndex >= 0 && poolIndex < Instance.audioPools.Length)
            {
                return Instance.audioPools[poolIndex].Play(clip, sourceIndex, looping);
            }
            // throw an exception if the index is out of range
            else throw new System.IndexOutOfRangeException(
                $"No audio pool associated with {poolIndex}. " +
                $"Total pools: {Instance.audioPools}");
        }
        #endregion
    }
}
