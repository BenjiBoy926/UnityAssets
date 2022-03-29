using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioLibrary
{
    [System.Serializable]
    public class AudioMixerIndex
    {
        #region Public Properties
        public int Index => index;
        public AudioMixerData Data => AudioSettings.GetMixer(this);
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Index of the audio mixer in the audio settings")]
        private int index;
        #endregion
    }
}
