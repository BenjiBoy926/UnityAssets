using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AudioLibrary
{
    public class AudioChannelUI : MonoBehaviour
    {
        #region Public Properties
        public string MissingParameterWarning
        {
            get
            {
                return $"Expected audio mixer '{Mixer}' " +
                    $"to have exposed parameter with the name '{VolumeParameterName}', " +
                    $"but no such parameter could be found. This UI will not be " +
                    $"able to control the volume of the audio mixer " +
                    $"unless an exposed parameter with this name exists";
            }
        }
        public AudioMixer Mixer => index.MixerIndex.Data.Mixer;
        public AudioMixerGroup Output => index.Channel.Output;
        public string VolumeParameterName => AudioDashboard.GetVolumeParameterName(index);
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Index of the channel in the audio settings")]
        private AudioChannelIndex index;
        [SerializeField]
        [Tooltip("Reference to the slider that changes the volume for this channel")]
        private Slider volumeSlider;
        [SerializeField]
        [Tooltip("Reference to the toggle that mutes this channel")]
        private Toggle muteToggle;
        [SerializeField]
        [Tooltip("Text used to display the name of the channel")]
        private Text titleText;
        #endregion

        #region Monobehaviour Messages
        private void Start()
        {
            titleText.text = $"{index.Channel.Output.name} Channel";
        }
        private void OnValidate()
        {
            // If we have volume slider then set it up
            if (volumeSlider)
            {
                volumeSlider.minValue = 0f;
                volumeSlider.maxValue = 100f;

                // If we get a float then set the slider value
                if (Mixer.GetFloat(VolumeParameterName, out float decibels))
                {
                    volumeSlider.SetValueWithoutNotify(AudioDashboard.DecibelsToVolume(decibels));
                }
            }

            // If we have a mute toggle then set it up
            if (muteToggle)
            {
                // Set mute toggle on if volume is very low
                if (Mixer.GetFloat(VolumeParameterName, out float decibels))
                {
                    float volume = AudioDashboard.DecibelsToVolume(decibels);
                    muteToggle.SetIsOnWithoutNotify(volume <= 0.001f);
                }
            }

            // If we have title text then set it up
            if (titleText)
            {
                titleText.text = $"{index.Channel.Output.name} Channel";
            }
        }
        #endregion

        #region Event Listeners
        private void OnVolumeChanged(float value)
        {

        }
        private void OnMuteChanged(bool value)
        {

        }
        #endregion
    }
}
