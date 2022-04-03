using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

namespace AudioUtility.Editor
{
    [CustomPropertyDrawer(typeof(AudioChannelIndex))]
    public class AudioChannelIndexDrawer : PropertyDrawer
    {
        #region Property Drawer Overrides
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Edit the foldout
            property.isExpanded = EditorGUIAuto.Foldout(ref position, property.isExpanded, label);

            if (property.isExpanded)
            {
                // Increase indent
                EditorGUI.indentLevel++;

                // Edit the first sub property
                property.Next(true);
                EditorGUIAuto.PropertyField(ref position, property, true);

                // Display the popup for the channel
                SerializedProperty mixerIndex = property.FindPropertyRelative("index");
                property.Next(false);
                position = EditorGUI.PrefixLabel(position, new GUIContent(property.displayName));
                int oldIndent = EditorGUI.indentLevel;
                EditorGUI.indentLevel = 0;
                property.intValue = EditorGUI.Popup(position, property.intValue, AllChannelContent(mixerIndex.intValue));
                EditorGUI.indentLevel = oldIndent;

                // Restore old indent
                EditorGUI.indentLevel--;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded) return EditorGUIUtility.singleLineHeight * 3f;
            else return EditorGUIUtility.singleLineHeight;
        }
        #endregion

        #region Public Methods
        public static GUIContent[] AllChannelContent(int mixerIndex)
        {
            // Get the mixer at the given index
            AudioMixerData mixer = AudioSettings.GetMixer(mixerIndex);

            return mixer
                .AllChannels
                .Select(channel => new GUIContent(EditorGUIUtility.ObjectContent(channel.Output, typeof(AudioMixerGroup))))
                .ToArray();
        }
        #endregion
    }
}
