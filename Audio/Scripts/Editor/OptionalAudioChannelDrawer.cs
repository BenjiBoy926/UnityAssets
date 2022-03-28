using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AudioLibrary.Editor
{
    [CustomPropertyDrawer(typeof(OptionalAudioChannel))]
    public class OptionalAudioChannelDrawer : PropertyDrawer
    {
        #region Property Drawer Overrides
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SerializedProperty hasChannel = property.FindPropertyRelative(nameof(hasChannel));
            SerializedProperty channel = property.FindPropertyRelative(nameof(channel));

            EditorGUI.PropertyField(position, property, label, true);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        #endregion
    }
}
