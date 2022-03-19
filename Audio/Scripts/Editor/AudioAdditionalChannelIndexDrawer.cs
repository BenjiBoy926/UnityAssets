using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AudioLibrary.Editor
{
    [CustomPropertyDrawer(typeof(AudioAdditionalChannelIndex))]
    public class AudioAdditionalChannelIndexDrawer : PropertyDrawer
    {
        #region Property Drawer Overrides
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Go to the first sub-property
            property.Next(true);

            // Get a list of the names of the audio mixer outputs
            GUIContent[] outputNames = AudioSettings
                .AdditionalChannels
                .Select(channel => new GUIContent(channel.Output.name))
                .ToArray();

            // Use a popup to select a channel
            property.intValue = EditorGUI.Popup(position, label, property.intValue, outputNames);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }
        #endregion
    }
}
