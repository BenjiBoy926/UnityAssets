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
        private AudioMixerPool[] audioPools = new AudioMixerPool[0];
        #endregion

        #region Private Fields
        private static AudioManager instance;
        #endregion

        #region Private Methods
        private void Setup()
        {
            // Create an audio pool for each audio channel
            AudioMixerData[] allMixers = AudioSettings.AllMixers;
            audioPools = new AudioMixerPool[allMixers.Length];

            // Setup an audio pool for every audio channel
            for (int i = 0; i < audioPools.Length; i++)
            {
                audioPools[i] = new AudioMixerPool(i, gameObject);
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
        // Master channel
        public static AudioSource PlayMaster(AudioClip clip, bool looping = false)
        {
            return PlayMaster(clip, 0, looping);
        }
        public static AudioSource PlayMaster(AudioClip clip, AudioMixerIndex index, bool looping = false)
        {
            return PlayMaster(clip, index.Index, looping);
        }
        public static AudioSource PlayMaster(AudioClip clip, AudioMixerIndex index, int sourceIndex, bool looping = false)
        {
            return PlayMaster(clip, index.Index, sourceIndex, looping);
        }
        public static AudioSource PlayMaster(AudioClip clip, int mixerIndex, bool looping = false)
        {
            if (mixerIndex >= 0 && mixerIndex < Instance.audioPools.Length)
            {
                return Instance.audioPools[mixerIndex].PlayMaster(clip, looping);
            }
            else throw IndexOutOfRangeException(mixerIndex);
        }
        public static AudioSource PlayMaster(AudioClip clip, int mixerIndex, int sourceIndex, bool looping = false)
        {
            if (mixerIndex >= 0 && mixerIndex < Instance.audioPools.Length)
            {
                return Instance.audioPools[mixerIndex].PlayMaster(clip, sourceIndex, looping);
            }
            else throw IndexOutOfRangeException(mixerIndex);
        }


        // Music channel
        public static AudioSource PlayMusic(AudioClip clip, bool looping = false)
        {
            return PlayMusic(clip, 0, looping);
        }
        public static AudioSource PlayMusic(AudioClip clip, AudioMixerIndex index, bool looping = false)
        {
            return PlayMusic(clip, index.Index, looping);
        }
        public static AudioSource PlayMusic(AudioClip clip, AudioMixerIndex index, int sourceIndex, bool looping = false)
        {
            return PlayMusic(clip, index.Index, sourceIndex, looping);
        }
        public static AudioSource PlayMusic(AudioClip clip, int mixerIndex, bool looping = false)
        {
            if (mixerIndex >= 0 && mixerIndex < Instance.audioPools.Length)
            {
                return Instance.audioPools[mixerIndex].PlayMusic(clip, looping);
            }
            else throw IndexOutOfRangeException(mixerIndex);
        }
        public static AudioSource PlayMusic(AudioClip clip, int mixerIndex, int sourceIndex, bool looping = false)
        {
            if (mixerIndex >= 0 && mixerIndex < Instance.audioPools.Length)
            {
                return Instance.audioPools[mixerIndex].PlayMusic(clip, sourceIndex, looping);
            }
            else throw IndexOutOfRangeException(mixerIndex);
        }

        // SFX channel
        public static AudioSource PlaySFX(AudioClip clip, bool looping = false)
        {
            return PlaySFX(clip, 0, looping);
        }
        public static AudioSource PlaySFX(AudioClip clip, AudioMixerIndex index, bool looping = false)
        {
            return PlaySFX(clip, index.Index, looping);
        }
        public static AudioSource PlaySFX(AudioClip clip, AudioMixerIndex index, int sourceIndex, bool looping = false)
        {
            return PlaySFX(clip, index.Index, sourceIndex, looping);
        } 
        public static AudioSource PlaySFX(AudioClip clip, int mixerIndex, bool looping = false)
        {
            if (mixerIndex >= 0 && mixerIndex < Instance.audioPools.Length)
            {
                return Instance.audioPools[mixerIndex].PlaySFX(clip, looping);
            }
            else throw IndexOutOfRangeException(mixerIndex);
        }
        public static AudioSource PlaySFX(AudioClip clip, int mixerIndex, int sourceIndex, bool looping = false)
        {
            if (mixerIndex >= 0 && mixerIndex < Instance.audioPools.Length)
            {
                return Instance.audioPools[mixerIndex].PlaySFX(clip, sourceIndex, looping);
            }
            else throw IndexOutOfRangeException(mixerIndex);
        }

        // Arbitrary channel
        public static AudioSource PlayFromChannel(AudioClip clip, int channelIndex, bool looping = false)
        {
            return PlayFromChannel(clip, 0, channelIndex, looping);
        }
        public static AudioSource PlayFromChannel(AudioClip clip, AudioChannelIndex index, bool looping = false)
        {
            return PlayFromChannel(clip, index.MixerIndex.Index, index.ChannelIndex, looping);
        }
        public static AudioSource PlayFromChannel(AudioClip clip, AudioChannelIndex index, int sourceIndex, bool looping = false)
        {
            return PlayFromChannel(clip, index.MixerIndex.Index, index.ChannelIndex, sourceIndex, looping);
        }
        public static AudioSource PlayFromChannel(AudioClip clip, int mixerIndex, int channelIndex, bool looping = false)
        {
            if (mixerIndex >= 0 && mixerIndex <= Instance.audioPools.Length)
            {
                return Instance.audioPools[mixerIndex].PlayFromChannel(clip, channelIndex, looping);
            }
            else throw IndexOutOfRangeException(mixerIndex);
        }
        public static AudioSource PlayFromChannel(AudioClip clip, int mixerIndex, int channelIndex, int sourceIndex, bool looping = false)
        {
            if (mixerIndex >= 0 && mixerIndex <= Instance.audioPools.Length)
            {
                return Instance.audioPools[mixerIndex].PlayFromChannel(clip, channelIndex, sourceIndex, looping);
            }
            else throw IndexOutOfRangeException(mixerIndex);
        }
        #endregion

        #region Private Methods
        private static System.IndexOutOfRangeException IndexOutOfRangeException(int mixerIndex)
        {
            return new System.IndexOutOfRangeException(
                $"No audio pool associated with {mixerIndex}. " +
                $"Total pools: {Instance.audioPools}");
        }
        #endregion
    }
}
