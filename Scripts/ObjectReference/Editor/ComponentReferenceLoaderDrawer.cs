using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ComponentReferenceLoader))]
public class ComponentReferenceLoaderDrawer : PropertyDrawer
{
    #region Public Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        SerializedProperty includeInactive = property.FindPropertyRelative(nameof(includeInactive));

        // Set height for only one control
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Edit the foldout
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += position.height;

        if(property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // Edit the reference type
            EditorGUI.PropertyField(position, referenceType);
            position.y += position.height;

            EditorGUI.indentLevel++;

            // Only edit include inactive bool property if getting a component in children
            if(referenceType.enumValueIndex == 2)
            {
                EditorGUI.PropertyField(position, includeInactive);
            }

            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));

        float standardControlHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        float height = standardControlHeight;

        if(property.isExpanded)
        {
            // Space for the reference type property
            height += standardControlHeight;

            // Space for the "include children" property
            if (referenceType.enumValueIndex == 2) height += standardControlHeight;
        }

        return height;
    }
    #endregion
}
