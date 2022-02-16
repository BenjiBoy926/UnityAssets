using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Sound2DChannelIndex))]
public class Sound2DChannelIndexDrawer : PropertyDrawer
{
    #region Property Drawer Overrides
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        // Go to the first sub-property
        property.Next(true);

        // Get a list of the names of the audio mixer outputs
        GUIContent[] additionalChannelNames = Sound2DSettings
            .AdditionalChannels
            .Select(channel => new GUIContent(channel.Output.name))
            .ToArray();

        GUIContent[] allChannelNames = new GUIContent[2 + additionalChannelNames.Length];

        // Add names for music then SFX
        allChannelNames[0] = new GUIContent(Sound2DSettings.MusicChannel.Output.name);
        allChannelNames[1] = new GUIContent(Sound2DSettings.SFXChannel.Output.name);
        
        // Copy in additional channel names
        Array.Copy(additionalChannelNames, 0, allChannelNames, 2, additionalChannelNames.Length);

        // Use a popup to select a channel
        property.intValue = EditorGUI.Popup(position, label, property.intValue, additionalChannelNames);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUIUtility.singleLineHeight;
    }
    #endregion
}
