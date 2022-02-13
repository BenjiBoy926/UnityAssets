using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

#if UNITY_2020_1_OR_NEWER
[CustomPropertyDrawer(typeof(ArrayOnEnum<,>))]
public class ArrayOnEnumDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        label = EditorGUI.BeginProperty(position, label, property);

        // The data array for this property
        SerializedProperty data = property.FindPropertyRelative(nameof(data));

        // Get the enums named in the attribute
        System.Type enumType = fieldInfo.FieldType.GetGenericArguments()[0];
        string[] enumNames = System.Enum.GetNames(enumType);

        // If the data does not have the correct array size then set it
        if (data.arraySize != enumNames.Length) data.arraySize = enumNames.Length;

        // Set height for just one control
        position.height = LayoutUtilities.standardControlHeight;

        // Add a foldout for the property
        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);
        position.y += position.height;

        if(property.isExpanded)
        {
            // Increase indent
            EditorGUI.indentLevel++;

            for(int i = 0; i < data.arraySize; i++)
            {
                // Edit the element and advance the position further
                SerializedProperty element = data.GetArrayElementAtIndex(i);
                EditorGUI.PropertyField(position, element, new GUIContent(enumNames[i]), true);
                position.y += EditorGUI.GetPropertyHeight(element, true) + EditorGUIUtility.standardVerticalSpacing;
            }

            // Restore indent
            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        float height = LayoutUtilities.standardControlHeight;

        // If property is expanded then accumulate heights of all array elements
        if(property.isExpanded)
        {
            // The data array for this property
            SerializedProperty data = property.FindPropertyRelative(nameof(data));

            // Loop through and increment the height for each data element
            for (int i = 0; i < data.arraySize; i++)
            {
                height += EditorGUI.GetPropertyHeight(data.GetArrayElementAtIndex(i), true) + EditorGUIUtility.standardVerticalSpacing;
            }
        }

        return height;
    }
}
#endif