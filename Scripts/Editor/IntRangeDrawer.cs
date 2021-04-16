using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(IntRange))]
public class IntRangeDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Store min-max sub-properties for ease of use
        SerializedProperty min = property.FindPropertyRelative("_min");
        SerializedProperty max = property.FindPropertyRelative("_max");

        // Put in the prefix label
        position = EditorGUI.PrefixLabel(position, label);

        // Store the old indent
        int oldIndent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        // Build a single-line layout where the float editors stretch and the labels stay at the same width
        Layout.Builder builder = new Layout.Builder();
        builder.PushChild(LayoutChild.Width(LayoutSize.Exact(35f)));
        builder.PushChild(LayoutChild.Width(LayoutSize.RatioOfRemainder(0.5f), LayoutMargin.Right(5f)));
        builder.PushChild(LayoutChild.Width(LayoutSize.Exact(35f)));
        builder.PushChild(LayoutChild.Width(LayoutSize.RatioOfRemainder(0.5f)));
        Layout layout = builder.Compile(position);

        // Create the min label
        EditorGUI.LabelField(layout.Next(), new GUIContent("Min:"));

        // Create the min editor
        EditorGUI.BeginChangeCheck();
        min.intValue = EditorGUI.DelayedIntField(layout.Next(), min.intValue);

        // If the max was modified, ensure that the max is not smaller than the new min
        if (EditorGUI.EndChangeCheck())
        {
            max.intValue = Mathf.Max(min.intValue, max.intValue);
        }

        // Create the max label
        EditorGUI.LabelField(layout.Next(), new GUIContent("Max:"));

        // Create the max editor
        EditorGUI.BeginChangeCheck();
        max.intValue = EditorGUI.DelayedIntField(layout.Next(), max.intValue);

        // If the max was modified, ensure that the min is not bigger than the new max
        if (EditorGUI.EndChangeCheck())
        {
            min.intValue = Mathf.Min(min.intValue, max.intValue);
        }

        // Restore indent level
        EditorGUI.indentLevel = oldIndent;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return LayoutUtilities.standardControlHeight;
    }
}
