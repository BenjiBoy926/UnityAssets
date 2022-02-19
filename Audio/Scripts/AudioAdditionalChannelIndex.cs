using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Audio
{
    [System.Serializable]
    public struct AudioAdditionalChannelIndex
    {
        #region Public Properties
        public int Index => index;
        public AudioChannel Channel => AudioSettings.GetAdditionalChannel(this);
        #endregion

        #region Private Editor Fields
        [SerializeField]
        [Tooltip("Index of the additional channel")]
        private int index;
        #endregion

        #region Constructors
        public AudioAdditionalChannelIndex(int index)
        {
            this.index = index;
        }
        #endregion
    }
}
