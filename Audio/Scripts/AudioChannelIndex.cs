using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public struct AudioChannelIndex
    {
        #region Public Properties
        public int Index => index;
        public AudioChannel Channel => AudioSettings.GetChannel(this);
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Index of the sound channel in the sound settings")]
        private int index;
        #endregion

        #region Constructors
        public AudioChannelIndex(int index)
        {
            this.index = index;
        }
        #endregion
    }
}
