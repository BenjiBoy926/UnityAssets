using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioUtility
{
    /// <summary>
    /// An audio channel that might not be provided
    /// </summary>
    [System.Serializable]
    public class OptionalAudioChannel
    {
        #region Public Properties
        public bool HasChannel => hasChannel;
        public AudioChannel Channel => channel;
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Whether this audio channel should be used or not")]
        private bool hasChannel;
        [SerializeField]
        [Tooltip("Data for the audio channel")]
        private AudioChannel channel;
        #endregion
    }
}
