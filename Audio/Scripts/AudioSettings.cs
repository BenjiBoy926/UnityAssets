using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioLibrary
{
    [CreateAssetMenu(fileName = nameof(AudioSettings))]
    public class AudioSettings : ScriptableObjectSingleton<AudioSettings>
    {
        #region Public Properties
        public static AudioMixerGroup Master => Instance.master;
        public static AudioChannel MusicChannel => Instance.musicChannel;
        public static AudioChannel SFXChannel => Instance.sfxChannel;
        public static AudioChannel[] AdditionalChannels => Instance.additionalChannels;
        public static int ChannelCount => 2 + AdditionalChannels.Length;
        public static AudioMixerData DefaultMixer => Instance.defaultMixer;
        public static AudioMixerData[] AdditionalMixers => Instance.additionalMixers;
        public static AudioMixerData[] AllMixers
        {
            get
            {
                AudioMixerData[] result = new AudioMixerData[AdditionalMixers.Length + 1];
                result[0] = DefaultMixer;
                AdditionalMixers.CopyTo(result, 1);
                return result;
            }
        }
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Audio mixer that all other channels should output to")]
        private AudioMixerGroup master;
        [SerializeField]
        [Tooltip("Setup for the music channel")]
        private AudioChannel musicChannel;
        [SerializeField]
        [Tooltip("Setup for the sfx channel")]
        private AudioChannel sfxChannel;
        [SerializeField]
        [Tooltip("List of channels to use for the 2D sound")]
        private AudioChannel[] additionalChannels = new AudioChannel[0];
        [SerializeField]
        [Tooltip("Default audio mixer for the audio settings")]
        private AudioMixerData defaultMixer;
        [SerializeField]
        [Tooltip("List of additional audio mixers for the audio settings")]
        private AudioMixerData[] additionalMixers = new AudioMixerData[0];
        #endregion

        #region Public Access Methods
        public static AudioChannel GetChannel(AudioChannelIndex index)
        {
            return GetChannel(index.Index);
        }
        public static AudioChannel GetChannel(int index)
        {
            // If the index is 0 then return music channel
            if (index == 0) return MusicChannel;
            // If index is 1 then reteurn sfx channel
            else if (index == 1) return SFXChannel;
            // If something other than 0 or 1 then return an additional channel
            else return GetAdditionalChannel(index - 2);
        }
        public static AudioChannel GetAdditionalChannel(int index)
        {
            // If index is in range then return it
            if (index >= 0 && index < AdditionalChannels.Length)
            {
                return AdditionalChannels[index];
            }
            // If not found then throw an exception
            else throw new IndexOutOfRangeException(
                $"No additional sound channel associated with index '{index}'. " +
                $"Total channels: {AdditionalChannels.Length}");
        }

        public static AudioMixerData GetMixer(AudioMixerIndex index)
        {
            return GetMixer(index.Index);
        }
        public static AudioMixerData GetMixer(int index)
        {
            AudioMixerData[] mixers = AllMixers;

            // If index is in range then return the correct mixer
            if (index >= 0 && index < mixers.Length)
            {
                return mixers[index];
            }
            // If index is out of range then throw an exception
            else throw new IndexOutOfRangeException(
                $"No audio mixer data associated with index '{index}'. " +
                $"Total mixers: {mixers.Length}");
        }
        #endregion

        #region Find Methods
        private static AudioChannelIndex FindInternal(Predicate<AudioChannel> predicate, params object[] searchParameters)
        {
            // If the channel exists then return it
            if (Exists(predicate, out AudioChannelIndex index))
            {
                return index;
            }
            // Otherwise throw an exception with details on the search parameters
            else
            {
                string parametersMessage = string.Empty;

                // Add information on each search parameter
                if (searchParameters != null && searchParameters.Length > 0)
                {
                    parametersMessage = "Search parameters:";

                    for(int i = 0; i < searchParameters.Length; i++)
                    {
                        parametersMessage += $"\n\tParameter {i + 1}: {searchParameters[i]}";
                    }
                }

                // Throw an exception with some information on the search parameters
                throw new AudioChannelNotFoundException(
                    $"No audio channel could be found that matched the given predicate. {parametersMessage}");
            }
        }
        #endregion

        #region Exists Methods
        public static bool Exists(Predicate<AudioChannel> predicate, out AudioChannelIndex index)
        {
            // If the predicate is true for the music channel return the index for the music channel
            if (predicate.Invoke(MusicChannel)) index = new AudioChannelIndex(0);
            // If the predicate is true for the sfx channel return the index for the sfx channel
            else if (predicate.Invoke(SFXChannel)) index = new AudioChannelIndex(1);
            // Try to find a match in additional channels
            else
            {
                int i = Array.FindIndex(AdditionalChannels, predicate);
                index = new AudioChannelIndex(i);
            }

            // Return true if the index is valid
            return index.Index >= 0;
        }
        #endregion
    }
}
