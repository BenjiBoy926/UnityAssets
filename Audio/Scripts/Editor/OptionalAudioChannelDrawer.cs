using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AudioUtility.Editor
{
    [CustomPropertyDrawer(typeof(OptionalAudioChannel))]
    public class OptionalAudioChannelDrawer : PropertyDrawer
    {
        #region Property Drawer Overrides
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Get the properties
            SerializedProperty hasChannel = property.FindPropertyRelative(nameof(hasChannel));
            SerializedProperty channel = property.FindPropertyRelative(nameof(channel));

            // Use a toggle to display the hasChannel property 
            hasChannel.boolValue = EditorGUIAuto.ToggleLeft(ref position, label, hasChannel.boolValue);
        
            if (hasChannel.boolValue)
            {
                // Layout the channel without the foldout
                EditorGUI.indentLevel++;
                AudioChannelDrawer.OnGUIWithoutFoldout(ref position, channel);
                EditorGUI.indentLevel--;
            }
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Height for a single control
            float height = EditorGUIAuto.SingleControlHeight;

            // Get the properties
            SerializedProperty hasChannel = property.FindPropertyRelative(nameof(hasChannel));
            SerializedProperty channel = property.FindPropertyRelative(nameof(channel));

            if (hasChannel.boolValue)
            {
                // Layout the channel without the foldout
                height += AudioChannelDrawer.GetPropertyHeightWithoutFoldout(channel);
            }

            // Return the height
            return height;
        }
        #endregion
    }
}
