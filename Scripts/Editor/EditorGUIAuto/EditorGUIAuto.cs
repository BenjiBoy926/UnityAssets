using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorGUIAuto
{
    #region Public Fields
    public static float SingleControlHeight => EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    #endregion

    #region Foldout
    public static bool Foldout(ref Rect position, bool foldout, string content, GUIStyle style = null)
    {
        return Foldout(ref position, foldout, content, true, style);
    }
    public static bool Foldout(ref Rect position, bool foldout, GUIContent content, GUIStyle style = null)
    {
        return Foldout(ref position, foldout, content, true, style);
    }
    public static bool Foldout(ref Rect position, bool foldout, string content, bool toggleOnLabelClick, GUIStyle style = null)
    {
        return Foldout(ref position, foldout, new GUIContent(content), toggleOnLabelClick, style);
    }
    public static bool Foldout(ref Rect position, bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style = null)
    {
        foldout = EditorGUI.Foldout(position, foldout, content, toggleOnLabelClick, style);
        position.y += SingleControlHeight;
        return foldout;
    }
    #endregion

    #region Property Field
    public static bool PropertyField(ref Rect position, SerializedProperty property, bool includeChildren = false)
    {
        return PropertyField(ref position, property, new GUIContent(property.displayName), includeChildren);
    }
    public static bool PropertyField(ref Rect position, SerializedProperty property, GUIContent label, bool includeChildren = false)
    {
        bool result = EditorGUI.PropertyField(position, property, label, includeChildren);
        position.y += EditorGUI.GetPropertyHeight(property, label, includeChildren);
        return result;
    }
    #endregion
}
