using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioLibrary
{
    [System.Serializable]
    public class AudioChannelIndex
    {
        #region Public Properties
        public AudioMixerIndex MixerIndex => mixerIndex;
        public int ChannelIndex => channelIndex;
        public AudioChannel Channel => AudioSettings.GetChannel(this);
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Index of the audio mixer's data")]
        private AudioMixerIndex mixerIndex;
        [SerializeField]
        [Tooltip("Index of the sound channel in the mixer's data")]
        private int channelIndex;
        #endregion

        #region Constructors 
        public AudioChannelIndex(int channelIndex) : 
            this(channelIndex, 0) {}
        public AudioChannelIndex(int mixerIndex, int channelIndex) : 
            this(new AudioMixerIndex(mixerIndex), channelIndex) {}
        public AudioChannelIndex(AudioMixerIndex mixerIndex, int channelIndex)
        {
            this.mixerIndex = mixerIndex;
            this.channelIndex = channelIndex;
        }
        #endregion
    }
}
