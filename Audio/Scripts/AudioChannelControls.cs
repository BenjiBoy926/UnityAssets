using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AudioLibrary
{
    public class AudioChannelControls : MonoBehaviour
    {
        #region Public Properties
        public AudioChannelIndex Index
        {
            get => index;
            set
            {
                index = value;
                UpdateUI();
            }
        }
        public string MissingParameterWarning => AudioDashboard.GetMissingParameterWarning(index);
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

        #region Private Fields
        private float unmutedVolume;
        #endregion

        #region Public Methods
        /// <summary>
        /// Update the values in the UI to reflect 
        /// the volume of the identified channel
        /// </summary>
        public void UpdateUI()
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
                titleText.text = $"{index.Channel.Output.name} Volume";
            }
        }
        /// <summary>
        /// Apply the values in the UI to
        /// the volume of the identified channel
        /// </summary>
        public void ApplyUI()
        {
            if (muteToggle.isOn) AudioDashboard.SetVolume(index, 0f);
            else AudioDashboard.SetVolume(index, volumeSlider.value);
        }
        #endregion

        #region Monobehaviour Messages
        private void Start()
        {
            UpdateUI();

            // Listen for changes to the slider and toggle
            volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
            muteToggle.onValueChanged.AddListener(OnMuteChanged);
        }
        private void OnValidate()
        {
            UpdateUI();
        }
        #endregion

        #region Event Listeners
        private void OnVolumeChanged(float value)
        {
            // Enable mute toggle if the value is very low
            muteToggle.SetIsOnWithoutNotify(value < 0.01f);

            // If the value is reasonably large then set the unmuted volume
            if (value >= 0.01f)
            {
                unmutedVolume = value;
            }

            ApplyUI();
        }
        private void OnMuteChanged(bool value)
        {
            // If we muted then set the slider value to 0
            if (value)
            {
                volumeSlider.SetValueWithoutNotify(0f);
            }
            // If we unmuted then set the slider unmuted volume
            else volumeSlider.SetValueWithoutNotify(unmutedVolume);

            ApplyUI();
        }
        #endregion
    }
}
