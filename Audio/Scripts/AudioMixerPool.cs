using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioUtility
{
    [System.Serializable]
    public class AudioMixerPool
    {
        #region Private Editor Fields
        [SerializeField]
        [Tooltip("List of pools used for each channel of this mixer")]
        private AudioChannelPool[] pools;
        [SerializeField]
        [Tooltip("Game object that holds all the audio pools for this mixer")]
        private GameObject gameObject;
        [SerializeField]
        [Tooltip("Index of the audio mixer in the audio settings")]
        private AudioMixerIndex index;
        #endregion

        #region Constructors
        public AudioMixerPool(int index, GameObject parent)
        {
            // Setup the index for the pool
            this.index = new AudioMixerIndex(index);

            // Create a new game object with the same name as the mixer
            gameObject = new GameObject($"Mixer {index}: {this.index.Data.Mixer}");
            gameObject.transform.SetParent(parent.transform);

            // Get a list of all channels
            AudioChannel[] channels = this.index.Data.AllChannels;
            pools = new AudioChannelPool[channels.Length];

            // Create a pool for each channel
            for (int i = 0; i < channels.Length; i++)
            {
                pools[i] = new AudioChannelPool(index, i, gameObject);
            }
        }
        #endregion

        #region Public Methods
        // Master
        public AudioSource PlayMaster(AudioClip clip, int sourceIndex = -1, bool looping = false)
        {
            return PlayFromChannel(clip, index.Data.MasterChannelIndex, sourceIndex, looping);
        }

        // Music
        public AudioSource PlayMusic(AudioClip clip, int sourceIndex = -1, bool looping = false)
        {
            return PlayFromChannel(clip, index.Data.MusicChannelIndex, sourceIndex, looping);
        }

        // SFX
        public AudioSource PlaySFX(AudioClip clip, int sourceIndex = -1, bool looping = false)
        {
            return PlayFromChannel(clip, index.Data.SFXChannelIndex, sourceIndex, looping);
        }

        // Arbitrary channel
        public AudioSource PlayFromChannel(AudioClip clip, int channelIndex, int sourceIndex = -1, bool looping = false)
        {
            // Check if the channel index is within range
            if (channelIndex >= 0 && channelIndex < pools.Length)
            {
                return pools[channelIndex].Play(clip, sourceIndex, looping);
            }
            // If index is out of range then throw an exception
            else throw IndexOutOfRangeException(channelIndex);
        }
        #endregion

        #region Private Methods
        private System.IndexOutOfRangeException IndexOutOfRangeException(int channelIndex)
        {
            return new System.IndexOutOfRangeException(
                $"No audio channel pool associated with index '{channelIndex}' " +
                $"on audio mixer pool '{index.Data.Mixer}' " +
                $"Total pools: {channelIndex}");
        }
        #endregion
    }
}
