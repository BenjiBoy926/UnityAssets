using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioLibrary
{
    public class AudioDashboard : MonoBehaviour
    {
        #region Public Fields
        public const float minVolume = 0.01f;
        #endregion

        #region Public Methods
        public static string GetMissingParameterWarning(AudioChannelIndex index)
        {
            return $"Expected audio mixer '{index.MixerIndex.Data.Mixer}' " +
                $"to have exposed parameter with the name '{GetVolumeParameterName(index)}', " +
                $"but no such parameter could be found.";
        }
        public static string GetVolumeParameterName(AudioChannelIndex index)
        {
            return $"{index.Channel.Output.name}Volume";
        }
        public static float GetDecibels(AudioChannelIndex index)
        {
            AudioMixer mixer = index.MixerIndex.Data.Mixer;
            if (mixer.GetFloat(GetVolumeParameterName(index), out float decibels))
            {
                return decibels;
            }
            else throw new System.InvalidOperationException(GetMissingParameterWarning(index));
        }
        public static float GetVolume(AudioChannelIndex index)
        {
            return DecibelsToVolume(GetDecibels(index));
        }
        public static void SetVolume(AudioChannelIndex index, float volume)
        {
            AudioMixer mixer = index.MixerIndex.Data.Mixer;
            bool success = mixer.SetFloat(
                GetVolumeParameterName(index), 
                VolumeToDecibels(volume));

            if (!success)
            {
                Debug.LogWarning(GetMissingParameterWarning(index));
            }
        }
        public static float VolumeToDecibels(float volume)
        {
            if (volume > minVolume) return 20f * Mathf.Log10(volume / 100f);
            else return -144f;
        }
        public static float DecibelsToVolume(float decibels)
        {
            float volume = Mathf.Pow(10f, decibels / 20f) * 100f;
            if (volume <= minVolume) volume = 0f;
            return volume;
        }
        #endregion
    }
}
