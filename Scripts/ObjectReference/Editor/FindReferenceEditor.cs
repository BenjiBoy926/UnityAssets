using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FindReference))]
public class FindReferenceEditor : PropertyDrawer
{
    #region Public Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Set the height for just one control
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Layout the foldout
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += position.height;

        if(property.isExpanded)
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
        SerializedProperty identifier = property.FindPropertyRelative(nameof(identifier));
        SerializedProperty parent = property.FindPropertyRelative(nameof(parent));

        // Set the height for just one control
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Layout the can cache value field
        EditorGUI.PropertyField(position, canCacheObject);
        position.y += position.height;

        // In any case layout the reference type
        EditorGUI.PropertyField(position, referenceType);
        position.y += position.height;

        // If we find by tag then edit identifier as a tag
        if(referenceType.enumValueIndex == 1)
        {
            Rect prefix = EditorGUI.PrefixLabel(position, new GUIContent(identifier.displayName));

            // Reset the indent, then output the tag field
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            identifier.stringValue = EditorGUI.TagField(prefix, identifier.stringValue);
            EditorGUI.indentLevel = oldIndent;
            
            position.y += position.height;
        }
        // If we find by name of in children then edit identifier as general string
        else if(referenceType.enumValueIndex == 2 || referenceType.enumValueIndex == 3)
        {
            EditorGUI.PropertyField(position, identifier);
            position.y += position.height;
        }

        // Edit the parent if enum value is three
        if(referenceType.enumValueIndex == 3)
        {
            EditorGUI.PropertyField(position, parent);
        }
    }
    public static float GetPropertyHeightNoFoldout(SerializedProperty property)
    {
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        float singleControlHeight = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;

        if (referenceType.enumValueIndex == 0) return singleControlHeight * 2f;
        else if (referenceType.enumValueIndex == 1 || referenceType.enumValueIndex == 2) return singleControlHeight * 3f;
        else return singleControlHeight * 4f;
    }
    #endregion
}
