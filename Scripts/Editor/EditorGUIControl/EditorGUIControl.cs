using System;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EditorGUIControl
{
    #region Constants
    public readonly float StandardControlHeight = EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
    #endregion

    #region Fields
    private string _methodName;
    private SerializedProperty _property;
    private GUIContent _label = GUIContent.none;
    private bool _includeChildren = false;
    private object[] _args;
    #endregion

    #region Constructors
    private EditorGUIControl(string methodName, params object[] args)
        : this(methodName, null, GUIContent.none, false, args) { }
    private EditorGUIControl(string methodName, SerializedProperty property, GUIContent label, bool includeChildren, params object[] args) 
    {
        _methodName = methodName;
        _property = property;
        _label = label;
        _includeChildren = includeChildren;
        _args = args;
    }
    #endregion

    #region Methods
    // Public Methods --------------------
    public object OnGUI(ref Rect position)
    {
        return InvokeEditorGUIMethod(ref position, _args);
    }
    public float GetControlHeight()
    {
        string heightMethodName = $"Get{_methodName}ControlHeight";
        MethodInfo heightMethod = GetType().GetMethod(heightMethodName);
        if (heightMethod == null)
            throw GetGUIException($"Method '{GetType()}.{heightMethodName}()' does not exist");
        return (int)heightMethod.Invoke(this, new object[] { });
    }
    // Factories --------------------------------------------------------------------
    public static EditorGUIControl BoundsField(Bounds value)
    {
        return BoundsField(GUIContent.none, value);
    }
    public static EditorGUIControl BoundsField(GUIContent label, Bounds value)
    {
        return new EditorGUIControl(nameof(BoundsField), null, label, false, value);
    }
    //public static EditorGUIControl BoundsIntField()
    //{
    //    return BoundsField();
    //}
    //public static EditorGUIControl ColorField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl CurveField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl DelayedDoubleField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl DelayedFloatField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl DelayedIntField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl DelayedTextField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl DoubleField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl DropdownButton()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl EnumFlagsField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl EnumPopup()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl FloatField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl Foldout()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl GradientField()
    //{
    //    return Standard;
    //}
    //// FLAG
    //public static EditorGUIControl HelpBox()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl IntField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl IntPopup()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl IntSlider()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl LabelField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl LayerField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl LinkButton()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl LongField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl MaskField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl MinMaxSlider()
    //{
    //    return Standard;
    //}
    //// FLAG
    //public static EditorGUIControl MultiFloatField()
    //{
    //    return Standard;
    //}
    //// FLAG
    //public static EditorGUIControl MultiIntField()
    //{
    //    return Standard;
    //}
    //// FLAG
    //public static EditorGUIControl MultiPropertyField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl ObjectField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl PasswordField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl Popup()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl PrefixLabel()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl ProgressBar()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl PropertyField()
    //{
    //    if (_property == null)
    //        throw GUIException($"Cannot  the  for this editor GUI  " +
    //            $"because no serialized property has been specified for it");
    //    return EditorGUI.Property(_property, _label, _includeChildren);
    //}
    //public static EditorGUIControl RectField()
    //{
    //    return BoundsField();
    //}
    //public static EditorGUIControl RectIntField()
    //{
    //    return BoundsField();
    //}
    //public static EditorGUIControl SelectableLabel()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl Slider()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl TagField()
    //{
    //    return Standard;
    //}
    //// FLAG
    //public static EditorGUIControl TextArea()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl TextField()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl Toggle()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl ToggleLeft()
    //{
    //    return Standard;
    //}
    //public static EditorGUIControl Vector2Field()
    //{
    //    if (EditorGUIUtility.wideMode)
    //        return Standard;
    //    else
    //        return Standard * 2f;
    //}
    //public static EditorGUIControl Vector2Int()
    //{
    //    return Vector2Field();
    //}
    //public static EditorGUIControl Vector3Field()
    //{
    //    return Vector2Field();
    //}
    //public static EditorGUIControl Vector3IntField()
    //{
    //    return Vector2Field();
    //}
    //public static EditorGUIControl Vector4Field()
    //{
    //    return Vector2Field();
    //}
    // Height Methods ---------------------------------------------------------------
    private float GetBoundsFieldControlHeight()
    {
        return StandardControlHeight * 3f;
    }
    private float GetBoundsIntFieldControlHeight()
    {
        return GetBoundsFieldControlHeight();
    }
    private float GetColorFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetCurveFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetDelayedDoubleFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetDelayedFloatFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetDelayedIntFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetDelayedTextFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetDoubleFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetDropdownButtonControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetEnumFlagsFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetEnumPopupControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetFloatFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetFoldoutControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetGradientFieldControlHeight()
    {
        return StandardControlHeight;
    }
    // FLAG
    private float GetHelpBoxControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetIntFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetIntPopupControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetIntSliderControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetLabelFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetLayerFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetLinkButtonControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetLongFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetMaskFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetMinMaxSliderControlHeight()
    {
        return StandardControlHeight;
    }
    // FLAG
    private float GetMultiFloatFieldControlHeight()
    {
        return StandardControlHeight;
    }
    // FLAG
    private float GetMultiIntFieldControlHeight()
    {
        return StandardControlHeight;
    }
    // FLAG
    private float GetMultiPropertyFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetObjectFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetPasswordFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetPopupControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetPrefixLabelControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetProgressBarControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetPropertyFieldControlHeight()
    {
        if (_property == null)
            throw GetGUIException($"Cannot get the height for this editor GUI control " +
                $"because no serialized property has been specified for it");
        return EditorGUI.GetPropertyHeight(_property, _label, _includeChildren);
    }
    private float GetRectFieldControlHeight()
    {
        return GetBoundsFieldControlHeight();
    }
    private float GetRectIntFieldControlHeight()
    {
        return GetBoundsFieldControlHeight();
    }
    private float GetSelectableLabelControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetSliderControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetTagFieldControlHeight()
    {
        return StandardControlHeight;
    }
    // FLAG
    private float GetTextAreaControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetTextFieldControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetToggleControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetToggleLeftControlHeight()
    {
        return StandardControlHeight;
    }
    private float GetVector2FieldControlHeight()
    {
        if (EditorGUIUtility.wideMode)
            return StandardControlHeight;
        else
            return StandardControlHeight * 2f;
    }
    private float GetVector2IntControlHeight()
    {
        return GetVector2FieldControlHeight();
    }
    private float GetVector3FieldControlHeight()
    {
        return GetVector2FieldControlHeight();
    }
    private float GetVector3IntFieldControlHeight()
    {
        return GetVector2FieldControlHeight();
    }
    private float GetVector4FieldControlHeight()
    {
        return GetVector2FieldControlHeight();
    }
    // Helper Methods ---------------------------------------------------------------
    private object InvokeEditorGUIMethod(ref Rect position, params object[] otherArgs)
    {
        // Setup the types
        position.height = GetControlHeight();
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
        MethodInfo method = type.GetMethod(_methodName, argTypes);
        if (method == null)
            throw GetGUIException($"Method '{type}.{_methodName}({GetTypesString(argTypes)})' does not exist");
        position.y += position.height;

        // Invoke the method and return the result
        return method.Invoke(null, args);
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
