using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ComponentReference))]
public class ComponentReferenceEditor : PropertyDrawer
{
    #region Public Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Set the height for just one control
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Layout the foldout
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += position.height;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            OnGUINoFoldout(position, property);
            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.isExpanded)
        {
            return GetPropertyHeightNoFoldout(property) + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
        else return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    }
    public static void OnGUINoFoldout(Rect position, SerializedProperty property)
    {
        SerializedProperty canCacheObject = property.FindPropertyRelative(nameof(canCacheObject));
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        SerializedProperty includeInactive = property.FindPropertyRelative(nameof(includeInactive));
        SerializedProperty gameObject = property.FindPropertyRelative(nameof(gameObject));

        // Set the height for just one control
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Layout the can cache value field
        EditorGUI.PropertyField(position, canCacheObject);
        position.y += position.height;

        // In any case layout the reference type
        EditorGUI.PropertyField(position, referenceType);
        position.y += position.height;

        // If we find by tag then edit identifier as a tag
        if (referenceType.enumValueIndex == 2)
        {
            EditorGUI.PropertyField(position, includeInactive);
            position.y += position.height;
        }

        // Layout the game object
        EditorGUI.PropertyField(position, gameObject);
        position.y += position.height;
    }
    public static float GetPropertyHeightNoFoldout(SerializedProperty property)
    {
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        float singleControlHeight = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;

        if (referenceType.enumValueIndex == 2) return singleControlHeight * 4f;
        else return singleControlHeight * 3f;
    }
    #endregion
}
