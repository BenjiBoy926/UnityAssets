using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AudioUtility.Editor
{
    [CustomPropertyDrawer(typeof(AudioChannel))]
    public class AudioChannelDrawer : PropertyDrawer
    {
        #region Private Fields
        private static readonly string[] propertyNames =
        {
            "output",
            "audioSourceCount"
        };
        #endregion

        #region Property Drawer Overrides
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Display the GUI for an audio channel without a foldout
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        public static void OnGUIWithoutFoldout(ref Rect position, SerializedProperty property)
        {
            foreach (string name in propertyNames)
            {
                SerializedProperty child = property.FindPropertyRelative(name);
                EditorGUIAuto.PropertyField(ref position, child, true);
            }
        }
        public static float GetPropertyHeightWithoutFoldout(SerializedProperty property)
        {
            float height = 0f;

            foreach (string name in propertyNames)
            {
                SerializedProperty child = property.FindPropertyRelative(name);
                height += EditorGUI.GetPropertyHeight(property, true);
            }

            return height;
        }
        #endregion
    }
}
