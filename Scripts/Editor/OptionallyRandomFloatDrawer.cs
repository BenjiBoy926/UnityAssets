using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(OptionallyRandomFloat))]
public class OptionallyRandomFloatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.PrefixLabel(position, label);

        Layout.Builder builder = new Layout.Builder();
        // The layout child for the enum dropdown
        builder.PushChild(LayoutChild.Width(LayoutSize.Exact(70f), LayoutMargin.Right(10f)))
            // The layout child for the float or float range
            .PushChild(LayoutChild.Width(LayoutSize.RatioOfRemainder(1f)));

        // Compile the layout
        Layout layout = builder.Compile(EditorGUI.IndentedRect(position));

        // Set the indent down to zero
        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        SerializedProperty option = property.FindPropertyRelative("option");
        EditorGUI.PropertyField(layout.Next(), option, GUIContent.none);

        // If the enum is the first value, only let the value be edited
        if(option.enumValueIndex == 0)
        {
            EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("value"), GUIContent.none);
        }
        // If the enum is the second value, only let the value range be edited
        else
        {
            EditorGUI.PropertyField(layout.Next(), property.FindPropertyRelative("valueRange"), GUIContent.none);
        }

        // Restore original indent
        EditorGUI.indentLevel = oldIndent;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return LayoutUtilities.standardControlHeight;
    }
}
