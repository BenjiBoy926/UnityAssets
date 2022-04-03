using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace AudioUtility
{
    public class AudioChannelControls : MonoBehaviour
    {
        #region Public Properties
        public AudioChannelIndex ChannelIndex
        {
            get => channelIndex;
            set
            {
                channelIndex = value;
                UpdateUI();
            }
        }
        public string MissingParameterWarning => AudioDashboard.GetMissingParameterWarning(channelIndex);
        public AudioMixer Mixer => channelIndex.MixerIndex.Data.Mixer;
        public AudioMixerGroup Output => channelIndex.Channel.Output;
        public string VolumeParameterName => AudioDashboard.GetVolumeParameterName(channelIndex);
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Index of the channel in the audio settings")]
        private AudioChannelIndex channelIndex;
        [SerializeField]
        [Tooltip("Reference to the slider that changes the volume for this channel")]
        private Slider volumeSlider;
        [SerializeField]
        [Tooltip("Text component that displays the current volume")]
        private Text volumeText;
        [SerializeField]
        [Tooltip("Reference to the toggle that mutes this channel")]
        private Toggle muteToggle;
        [SerializeField]
        [Tooltip("Text used to display the name of the channel")]
        private Text titleText;
        #endregion

        #region Private Fields
        private float unmutedVolume;
        private bool updatingUI;
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
                if (AudioDashboard.GetVolume(channelIndex, out float volume))
                {
                    volumeSlider.SetValueWithoutNotify(volume);

                    if (volumeSlider.value > 0f)
                    {
                        unmutedVolume = volumeSlider.value;
                    }
                }
            }

            // If the text exists then set it
            if (volumeText)
            {
                volumeText.text = $"Volume: {volumeSlider.value}%";
            }

            // If we have a mute toggle then set it up
            if (muteToggle)
            {
                if (AudioDashboard.GetVolume(channelIndex, out float volume))
                {
                    muteToggle.SetIsOnWithoutNotify(volume < 1f);
                }
            }

            // If we have title text then set it up
            if (titleText)
            {
                titleText.text = $"{channelIndex.Channel.Output.name} Channel";
            }

            // Set updating UI to true
            updatingUI = true;
        }
        /// <summary>
        /// Apply the values in the UI to
        /// the volume of the identified channel
        /// </summary>
        public void ApplyUI()
        {
            if (muteToggle.isOn) AudioDashboard.SetVolume(channelIndex, 0f);
            else AudioDashboard.SetVolume(channelIndex, volumeSlider.value);

            // Set the volume text
            volumeText.text = $"Volume: {volumeSlider.value}%";
            updatingUI = false;
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
        private void Update()
        {
            // Re-update the ui on each frame
            // This mechanism is used because setting the volume on the audio mixer
            // results in smooth transitions that are not immediate, we need to make sure
            // the UI updates each frame
            if (updatingUI)
                UpdateUI();
        }
        private void OnValidate()
        {
            UpdateUI();
        }
        public void Reset()
        {
            AudioDashboard.ResetDecibels(channelIndex);
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
