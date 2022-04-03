using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AudioUtility
{
    public class AudioMixerControls : MonoBehaviour
    {
        #region Public Properties
        public AudioMixerIndex MixerIndex
        {
            get => mixerIndex;
            set
            {
                mixerIndex = value;
                UpdateUI();
            }
        }
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Index of the mixer that this UI element will control")]
        private AudioMixerIndex mixerIndex;
        [SerializeField]
        [Tooltip("Prefab of the object to use for each channel control")]
        private AudioChannelControls channelControlsPrefab;
        [SerializeField]
        [Tooltip("Transform to use as the parent for each of the audio channels")]
        private Transform channelControlsParent;
        [SerializeField]
        [Tooltip("Text used to display the name of the audio mixer")]
        private Text titleText;
        [SerializeField]
        [Tooltip("Button that resets the volume of each channel to its original value")]
        private Button resetButton;
        #endregion

        #region Private Fields
        private List<AudioChannelControls> channelControls = new List<AudioChannelControls>();
        #endregion

        #region Public Methods
        /// <summary>
        /// Update the UI to reflect the state of the audio mixer
        /// </summary>
        public void UpdateUI()
        {
            // Set the title text
            if (titleText)
                titleText.text = $"{mixerIndex.Data.Mixer.name} Controls";

            if (channelControlsPrefab && channelControlsParent)
            {
                foreach (AudioChannelControls control in channelControls)
                {
                    // Make sure the control still exists
                    if (control)
                    {
                        Destroy(control.gameObject);
                    }
                }
                channelControls.Clear();

                // Store the total number of channels
                int totalChannels = mixerIndex.Data.AllChannels.Length;

                // Create a new control for every channel
                for (int i = 0; i < totalChannels; i++)
                {
                    AudioChannelControls control = Instantiate(channelControlsPrefab, channelControlsParent);
                    control.ChannelIndex = new AudioChannelIndex(mixerIndex, i);
                    channelControls.Add(control);
                }
            }
        }
        #endregion

        #region Monobehaviour Messages
        private void Start()
        {
            UpdateUI();

            // Reset the controls when the button is clicked
            resetButton.onClick.AddListener(Reset);
        }
        public void Reset()
        {
            foreach (AudioChannelControls controls in channelControls)
            {
                controls.Reset();
            }
        }
        #endregion
    }
}
