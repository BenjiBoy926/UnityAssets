using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(AbstractTestValue))]
public class AbstractTestValueDrawer : PropertyDrawer
{
    #region Public Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Get the properties
        SerializedProperty contextSensitive = property.FindPropertyRelative(nameof(contextSensitive));
        SerializedProperty testValue = property.FindPropertyRelative(nameof(testValue));
        SerializedProperty releaseValue = property.FindPropertyRelative(nameof(releaseValue));

        // Set height for one control
        position.height = LayoutUtilities.standardControlHeight;

        // Layout the foldout
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += position.height;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // Layout the context sensitive bool
            EditorGUI.PropertyField(position, contextSensitive);
            position.y += position.height;

            // Increase indent
            EditorGUI.indentLevel++;

            // Layout the release value
            EditorGUI.PropertyField(position, releaseValue);
            position.y += position.height;

            // If it is context sensitive then layout the test value
            if(contextSensitive.boolValue)
            {
                EditorGUI.PropertyField(position, testValue);
                position.y += position.height;
            }

            // Restore indents
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = LayoutUtilities.standardControlHeight;

        if(property.isExpanded)
        {
            height += LayoutUtilities.standardControlHeight * 2;

            // If the property is context sensitive then add more height
            SerializedProperty contextSensitive = property.FindPropertyRelative(nameof(contextSensitive));
            if (contextSensitive.boolValue)
            {
                height += LayoutUtilities.standardControlHeight;
            }
        }

        return height;
    }
    #endregion
}
