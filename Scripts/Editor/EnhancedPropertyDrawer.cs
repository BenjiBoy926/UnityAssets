using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EnhancedPropertyDrawer : PropertyDrawer
{
    #region Fields
    protected bool _editorGUI = false;
    protected float _propertyHeight = 0f;
    #endregion

    #region Methods
    // Key Method ------------------------------------------------------------------------------------
    protected virtual void ExecutePropertyLogic(Rect position, SerializedProperty property, GUIContent label)
    {

    }
    // Overrides ---------------------------------------------------------------------------
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _editorGUI = true;
        ExecutePropertyLogic(position, property, label);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        _editorGUI = false;
        _propertyHeight = 0f;
        ExecutePropertyLogic(Rect.zero, property, label);
        return _propertyHeight;
    }
    // Helper Methods ---------------------------------------------------------------
    private TResult InvokeEditorGUIMethod<TResult>(string methodName, ref Rect position, TResult initialValue, params object[] otherArgs)
    {
        float height = 0f;

        if (_editorGUI)
        {
            // Setup the types
            position.height = height;
            Type type = typeof(EditorGUI);

            // Setup the arguments
            object[] args = new object[otherArgs.Length + 1];
            args[0] = position;
            for (int i = 1; i < args.Length; i++)
                args[i] = otherArgs[i];

            // Setup argument types
            Type[] argTypes = new Type[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == null)
                    throw GetGUIException($"Argument #{i + 1} cannot be null");
                argTypes[i] = args[i].GetType();
            }

            // Try to get the method
            MethodInfo method = type.GetMethod(methodName, argTypes);
            object resultObject = method.Invoke(null, args);
            try
            {
                return (TResult)resultObject;
            }
            catch (InvalidCastException)
            {
                throw GetGUIException(
                    $"Method '{type}.{methodName}({GetTypesString(argTypes)})' returned an object " +
                    $"of type '{resultObject.GetType()}' " +
                    $"where an object of type '{typeof(TResult)}' was expected");
            }
        }
        else
        {
            _propertyHeight += height;
            return initialValue;
        }
    }
    private MethodInfo GetEditorGUIMethod(string methodName, ref Rect position, params object[] otherArgs)
    {
        // Setup the position height based on control height
        position.height = 0f;

        // Setup the arguments
        Type type = typeof(EditorGUI);
        object[] args = new object[otherArgs.Length + 1];
        args[0] = position;
        for (int i = 1; i < args.Length; i++)
            args[i] = otherArgs[i];

        // Setup argument types
        Type[] argTypes = new Type[args.Length];
        for (int i = 0; i < args.Length; i++)
        {
            if (args[i] == null)
                throw GetGUIException($"Argument #{i + 1} cannot be null");
            argTypes[i] = args[i].GetType();
        }

        // Try to get the method
        MethodInfo method = type.GetMethod(methodName, argTypes);
        if (method == null)
            throw GetGUIException($"Method '{type}.{methodName}({GetTypesString(argTypes)})' does not exist");
        position.y += position.height;

        return method;
    }
    private string GetTypesString(Type[] types)
    {
        return string.Join(", ", (IEnumerable<Type>)types);
    }
    private ExitGUIException GetGUIException(string message)
    {
        Debug.LogError(message);
        return new ExitGUIException();
    }
    #endregion
}
