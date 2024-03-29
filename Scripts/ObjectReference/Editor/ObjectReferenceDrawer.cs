﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ObjectReference))]
public class ObjectReferenceDrawer : PropertyDrawer
{
    #region Public Methods
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        SerializedProperty canCacheObject = property.FindPropertyRelative(nameof(canCacheObject));
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        SerializedProperty directReference = property.FindPropertyRelative(nameof(directReference));
        SerializedProperty sceneReference = property.FindPropertyRelative(nameof(sceneReference));
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
            else
            {
                // Edit the can cache object field
                EditorGUI.PropertyField(position, canCacheObject);
                position.y += position.height;

                if(referenceType.enumValueIndex == 1) SceneReferenceLoaderDrawer.OnGUINoFoldout(position, sceneReference);
                else ResourcesReferenceDrawer.OnGUINoFoldout(position, resourcesReference);
            }

            // Restore normal indent
            EditorGUI.indentLevel--;
            EditorGUI.indentLevel--;
        }
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        SerializedProperty referenceType = property.FindPropertyRelative(nameof(referenceType));
        SerializedProperty sceneReference = property.FindPropertyRelative(nameof(sceneReference));
        SerializedProperty resourcesReference = property.FindPropertyRelative(nameof(resourcesReference));

        float standardControlHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        float height = standardControlHeight;

        if(property.isExpanded)
        {
            // Add height for reference type edit
            height += standardControlHeight;

            // Add height for the direct reference drawer
            if (referenceType.enumValueIndex == 0)
            {
                height += standardControlHeight;
            }
            else
            {
                // Add height for the can cache object property
                height += standardControlHeight;

                if(referenceType.enumValueIndex == 1)
                {
                    height += SceneReferenceLoaderDrawer.GetPropertyHeightNoFoldout(sceneReference);
                }
                else height += ResourcesReferenceDrawer.GetPropertyHeightNoFoldout(resourcesReference);
            }
        }

        return height;
    }
    #endregion
}
