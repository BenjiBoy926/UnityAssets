using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace AudioLibrary
{
    /// <summary>
    /// Data for a single audio mixer and the mixer groups
    /// to expose to the AudioLibrary API
    /// </summary>
    [System.Serializable]
    public class AudioMixerData
    {
        #region Public Properties
        public AudioMixer Mixer => mixer;
        public OptionalAudioChannel MasterChannel => masterChannel;
        public OptionalAudioChannel MusicChannel => musicChannel;
        public OptionalAudioChannel SFXChannel => sfxChannel;
        public AudioChannel[] AdditionalChannels => additionalChannels;
        public AudioChannel[] AllChannels
        {
            get
            {
                List<AudioChannel> channels = new List<AudioChannel>();

                // Add each of the optional channels
                if (masterChannel.HasChannel) channels.Add(masterChannel.Channel);
                if (musicChannel.HasChannel) channels.Add(musicChannel.Channel);
                if (sfxChannel.HasChannel) channels.Add(sfxChannel.Channel);

                // Add all of the additional channels
                channels.AddRange(additionalChannels);

                // Return the list of channels
                return channels.ToArray();
            }
        }
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Reference to the audio mixer")]
        private AudioMixer mixer;
        [SerializeField]
        [Tooltip("Optional master channel for this mixer")]
        private OptionalAudioChannel masterChannel;
        [SerializeField]
        [Tooltip("Optional music channel for this mixer")]
        private OptionalAudioChannel musicChannel;
        [SerializeField]
        [Tooltip("Optional sfx channel for this mixer")]
        private OptionalAudioChannel sfxChannel;
        [SerializeField]
        [Tooltip("List of additional channels for this mixer")]
        private AudioChannel[] additionalChannels;
        #endregion

        #region Public Methods
        public AudioChannel GetChannel(int index)
        {
            AudioChannel[] allChannels = AllChannels;

            if (index >= 0 && index < allChannels.Length)
            {
                return allChannels[index];
            }
            else throw new System.IndexOutOfRangeException(
                $"No channel associated with index '{index}' " +
                $"for mixer '{mixer}'. Total channels: {allChannels.Length}");
        }
        #endregion
    }
}
