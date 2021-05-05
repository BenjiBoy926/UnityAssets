using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(TagSelectorAttribute))]
public class TagSelectorDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            Rect newPos = EditorGUI.PrefixLabel(position, label);

            // Make sure this part is not indented
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            property.stringValue = EditorGUI.TagField(newPos, property.stringValue);

            // Restore old indents
            EditorGUI.indentLevel = oldIndent;
        }
        else
        {
            base.OnGUI(position, property, label);
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (property.propertyType == SerializedPropertyType.String)
        {
            return EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
        }
        else
        {
            return base.GetPropertyHeight(property, label);
        }
    }
}
