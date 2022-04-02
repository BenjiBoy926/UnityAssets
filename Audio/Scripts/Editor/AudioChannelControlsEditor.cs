using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AudioLibrary.Editor
{
    [CustomEditor(typeof(AudioChannelControls))]
    public class AudioChannelControlsEditor : UnityEditor.Editor
    {
        #region Editor Overrides
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            // Check if the mixer has the parameter exposed
            AudioChannelControls audioChannelControls = target as AudioChannelControls;
            bool hasParameter = audioChannelControls.Mixer.ClearFloat(audioChannelControls.VolumeParameterName);

            // If the audio mixer does not have the parameter then show the warning in the editor
            if (!hasParameter)
            {
                EditorGUILayout.HelpBox(audioChannelControls.MissingParameterWarning, MessageType.Warning);
            }
        }
        #endregion
    }
}
