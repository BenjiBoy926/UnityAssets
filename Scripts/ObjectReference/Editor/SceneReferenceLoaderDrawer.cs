using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(SceneReferenceLoader))]
public class SceneReferenceLoaderDrawer : PropertyDrawer
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
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        SerializedProperty identifier = property.FindPropertyRelative(nameof(identifier));
        SerializedProperty parent = property.FindPropertyRelative(nameof(parent));
        SerializedProperty componentReference = property.FindPropertyRelative(nameof(componentReference));

        // Set the height for just one control
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // In any case layout the reference type
        EditorGUI.PropertyField(position, referenceType);
        position.y += position.height;

        if(referenceType.enumValueIndex != 0)
        {
            // If we find by tag then edit identifier as a tag
            if (referenceType.enumValueIndex == 1)
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
            else if (referenceType.enumValueIndex == 2 || referenceType.enumValueIndex == 3)
            {
                EditorGUI.PropertyField(position, identifier);
                position.y += position.height;
            }

            // Edit the parent if enum value is three
            if (referenceType.enumValueIndex == 3)
            {
                EditorGUI.PropertyField(position, parent);
                position.y += position.height;
            }

            // Edit the component reference
            EditorGUI.PropertyField(position, componentReference, true);
        }
    }
    public static float GetPropertyHeightNoFoldout(SerializedProperty property)
    {
        // Get the reference type and component reference
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        SerializedProperty componentReference = property.FindPropertyRelative(nameof(componentReference));

        // Set the height to start for just one control
        float singleControlHeight = EditorGUIUtility.standardVerticalSpacing + EditorGUIUtility.singleLineHeight;
        float height = singleControlHeight;

        if(referenceType.enumValueIndex != 0)
        {
            // Add the height for the component reference drawer and the identifier of the game object
            height += EditorGUI.GetPropertyHeight(componentReference) + singleControlHeight;

            // If finding in children then add the height for the parent transform
            if (referenceType.enumValueIndex == 3) height += singleControlHeight;
        }

        return height;
    }
    #endregion
}
