using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioUtility
{
    [CreateAssetMenu(fileName = nameof(AudioSettings))]
    public class AudioSettings : ScriptableObjectSingleton<AudioSettings>
    {
        #region Public Properties
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
        [Tooltip("Default audio mixer for the audio settings")]
        private AudioMixerData defaultMixer;
        [SerializeField]
        [Tooltip("List of additional audio mixers for the audio settings")]
        private AudioMixerData[] additionalMixers = new AudioMixerData[0];
        #endregion

        #region Public Access Methods
        public static AudioMixerData GetMixer(AudioMixerIndex index)
        {
            AudioMixerData[] mixers = AllMixers;

            // If index is in range then return the correct mixer
            if (index.Index >= 0 && index.Index < mixers.Length)
            {
                return mixers[index.Index];
            }
            // If index is out of range then throw an exception
            else throw new IndexOutOfRangeException(
                $"No audio mixer data associated with index '{index.Index}'. " +
                $"Total mixers: {mixers.Length}");
        }
        public static AudioChannel GetChannel(AudioChannelIndex index)
        {
            AudioMixerData mixer = GetMixer(index.MixerIndex);
            return mixer.GetChannel(index.ChannelIndex);
        }
        #endregion
    }
}
