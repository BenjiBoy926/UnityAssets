using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AudioLibrary.Editor
{
    [CustomEditor(typeof(AudioChannelUI))]
    public class AudioChannelUIEditor : UnityEditor.Editor
    {
        #region Editor Overrides
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // Check if the mixer has the parameter exposed
            AudioChannelUI audioChannelUI = target as AudioChannelUI;
            bool hasParameter = audioChannelUI.Mixer.ClearFloat(audioChannelUI.VolumeParameterName);

            // If the audio mixer does not have the parameter then show the warning in the editor
            if (!hasParameter)
            {
                EditorGUILayout.HelpBox(audioChannelUI.MissingParameterWarning, MessageType.Warning);
            }
        }
        #endregion
    }
}
