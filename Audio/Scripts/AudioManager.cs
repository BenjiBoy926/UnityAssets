using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioUtility
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
        public static AudioSource PlayMaster(AudioClip clip, AudioMixerIndex index = null, int sourceIndex = -1, bool looping = false)
        {
            return PlayInternal(clip, index, sourceIndex, looping, "Master");
        }

        // Music channel
        public static AudioSource PlayMusic(AudioClip clip, AudioMixerIndex index = null, int sourceIndex = -1, bool looping = false)
        {
            return PlayInternal(clip, index, sourceIndex, looping, "Music");
        }

        // SFX channel
        public static AudioSource PlaySFX(AudioClip clip, AudioMixerIndex index = null, int sourceIndex = -1, bool looping = false)
        {
            return PlayInternal(clip, index, sourceIndex, looping, "SFX");
        } 

        // Arbitrary channel
        public static AudioSource PlayFromChannel(AudioClip clip, AudioChannelIndex index, int sourceIndex = -1, bool looping = false)
        {
            // If index is null then throw an exception
            if (index == null)
                throw new ArgumentNullException(nameof(index));

            // If the number is invalid then throw an exception
            if (index.MixerIndex.Index < 0 || index.MixerIndex.Index >= Instance.audioPools.Length)
                throw IndexOutOfRangeException(index.MixerIndex);

            // Play from the channel on the correct audio pool
            return Instance.audioPools[index.MixerIndex.Index].PlayFromChannel(clip, index.ChannelIndex, sourceIndex, looping);
        }
        #endregion

        #region Private Methods
        private static AudioSource PlayInternal(AudioClip clip, AudioMixerIndex index, int sourceIndex, bool looping, string methodSuffix)
        {
            // If index is null then set it to default
            if (index == null) index = AudioMixerIndex.Default;

            // If index is invalid then throw an exception
            if (index.Index < 0 || index.Index >= Instance.audioPools.Length)
                throw IndexOutOfRangeException(index);

            // Get the audio pool and identified method
            AudioMixerPool pool = Instance.audioPools[index.Index];
            string methodName = $"Play{methodSuffix}";
            MethodInfo playMethod = pool.GetType().GetMethod($"Play{methodSuffix}");

            // If reflection did not find the method then throw an internal error
            if (playMethod == null)
                throw new Exception($"INTERNAL ERROR: " +
                    $"no method defined in type '{nameof(AudioMixerPool)}' " +
                    $"with the name '{methodName}'");

            // Get the result of the method
            object result = playMethod.Invoke(pool, new object[] { clip, sourceIndex, looping });

            // Check to make sure the object is not null
            if (result is null)
                throw new Exception("INTERNAL ERROR: " +
                    $"The return value of the method '{methodName}' " +
                    $"for the type '{nameof(AudioMixerPool)}' " +
                    $"is not permitted to be null");

            // Check to make sure the result is an audio source
            if (!(result is AudioSource))
                throw new InvalidCastException($"INTERNAL ERROR: " +
                    $"The return value of the method '{methodName}' " +
                    $"for the type '{nameof(AudioMixerPool)}' must be of type " +
                    $"'{nameof(AudioSource)}', not type '{result.GetType().Name}'");

            return result as AudioSource;
        }
        private static IndexOutOfRangeException IndexOutOfRangeException(AudioMixerIndex index)
        {
            return new IndexOutOfRangeException(
                $"No audio pool associated with {index.Index}. " +
                $"Total pools: {Instance.audioPools}");
        }
        #endregion
    }
}
