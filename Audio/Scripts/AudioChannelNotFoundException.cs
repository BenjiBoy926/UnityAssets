using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AudioUtility
{
    [System.Serializable]
    public class AudioChannelNotFoundException : System.Exception
    {
        public AudioChannelNotFoundException() { }
        public AudioChannelNotFoundException(string message) : base(message) { }
        public AudioChannelNotFoundException(string message, System.Exception inner) : base(message, inner) { }
        protected AudioChannelNotFoundException(
          System.Runtime.Serialization.SerializationInfo info,
          System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}
