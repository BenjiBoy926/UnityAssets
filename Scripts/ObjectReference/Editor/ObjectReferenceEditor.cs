using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ObjectReference))]
public class ObjectReferenceEditor : PropertyDrawer
{
    #region Public Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        SerializedProperty directReference = property.FindPropertyRelative(nameof(directReference));
        SerializedProperty findReference = property.FindPropertyRelative(nameof(findReference));
        SerializedProperty componentReference = property.FindPropertyRelative(nameof(componentReference));
        SerializedProperty resourcesReference = property.FindPropertyRelative(nameof(resourcesReference));

        // Set height for just one control
        position.height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        // Layout the foldout
        EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += position.height;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            // Layout the reference type
            EditorGUI.PropertyField(position, referenceType);
            position.y += position.height;

            // Increase indent level for fields that depend on the reference type
            EditorGUI.indentLevel++;

            // Display correct property based on the reference type
            if (referenceType.enumValueIndex == 0)
            {
                EditorGUI.PropertyField(position, directReference);
            }
            else if (referenceType.enumValueIndex == 1)
            {
                FindReferenceEditor.OnGUINoFoldout(position, findReference);
            }
            else if (referenceType.enumValueIndex == 2)
            {
                ComponentReferenceEditor.OnGUINoFoldout(position, componentReference);
            }
            else ResourcesReferenceEditor.OnGUINoFoldout(position, resourcesReference);

            // Restore normal indent
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        SerializedProperty directReference = property.FindPropertyRelative(nameof(directReference));
        SerializedProperty findReference = property.FindPropertyRelative(nameof(findReference));
        SerializedProperty componentReference = property.FindPropertyRelative(nameof(componentReference));
        SerializedProperty resourcesReference = property.FindPropertyRelative(nameof(resourcesReference));

        float height = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;

        if(property.isExpanded)
        {
            height *= 2f;

            // Add correct height based on the reference type
            if (referenceType.enumValueIndex == 0)
            {
                height += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing; ;
            }
            else if (referenceType.enumValueIndex == 1)
            {
                height += FindReferenceEditor.GetPropertyHeightNoFoldout(findReference);
            }
            else if (referenceType.enumValueIndex == 2)
            {
                height += ComponentReferenceEditor.GetPropertyHeightNoFoldout(componentReference);
            }
            else height += ResourcesReferenceEditor.GetPropertyHeightNoFoldout(resourcesReference);
        }

        return height;
    }
    #endregion
}
