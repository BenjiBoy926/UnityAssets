using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioLibrary
{
    public class AudioDashboard : MonoBehaviour
    {
        #region Public Fields
        public const float minVolume = 0.01f;
        #endregion

        #region Public Methods
        public static string GetVolumeParameterName(AudioChannelIndex index)
        {
            return $"{index.Channel.Output.name}Volume";
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
