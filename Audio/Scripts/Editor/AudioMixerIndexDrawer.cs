using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEditor;

namespace AudioLibrary.Editor
{
    [CustomPropertyDrawer(typeof(AudioMixerIndex))]
    public class AudioMixerIndexDrawer : PropertyDrawer
    {
        #region Property Drawer Overrides
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Go to first property
            property.Next(true);
            position = EditorGUI.PrefixLabel(position, label);

            // Edit the int as a popup
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            property.intValue = EditorGUI.Popup(position, property.intValue, AllMixerContent());
            EditorGUI.indentLevel = oldIndent;
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIAuto.SingleControlHeight;
        }
        #endregion

        #region Public Methods
        public static GUIContent[] AllMixerContent()
        {
            return AudioSettings
                .AllMixers
                .Select(data => new GUIContent(EditorGUIUtility.ObjectContent(data.Mixer, typeof(AudioMixer))))
                .ToArray();
        }
        #endregion
    }
}
