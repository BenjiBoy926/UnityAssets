using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class EnhancedPropertyDrawer : PropertyDrawer
{
    #region Fields
    private Rect _currentPosition = Rect.zero;
    private object _currentControlResult = null;
    #endregion

    #region Properties
    protected bool EditorGUI { get; private set; } = false;
    protected float PropertyHeight { get; private set; } = 0f;
    protected Rect CurrentPosition => _currentPosition;
    protected object CurrentControlResult => _currentControlResult;
    #endregion

    #region Methods
    // Key Methods ------------------------------------------------------------------------------------
    protected abstract void ExecutePropertyLogic(SerializedProperty property, GUIContent label);
    protected void ExecuteControlLogic(Action gui, float height = -1f)
    {
        if (height <= 0f)
            height = EditorGUIUtility.singleLineHeight;

        if (EditorGUI)
        {
            _currentPosition.height = height;
            gui.Invoke();
            _currentPosition.y += height + EditorGUIUtility.standardVerticalSpacing;
        }
        else
            PropertyHeight += height + EditorGUIUtility.standardVerticalSpacing;
    }
    // Overrides ---------------------------------------------------------------------------
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        _currentPosition = position;
        EditorGUI = true;
        ExecutePropertyLogic(property, label);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        _currentPosition = Rect.zero;
        EditorGUI = false;
        PropertyHeight = 0f;
        ExecutePropertyLogic(property, label);
        return PropertyHeight;
    }
    // Delegate Methods -----------------------------------------------------------
    protected Bounds BoundsField(Bounds value)
    {
        return EditorGUIControl(value, GetLineHeight(2), nameof(BoundsField), value);
    }
    protected Bounds BoundsField(GUIContent label, Bounds value)
    {
        return EditorGUIControl(value, GetLineHeight(2), nameof(BoundsField), label, value);
    }
    protected BoundsInt BoundsIntField(BoundsInt value)
    {
        return EditorGUIControl(value, GetLineHeight(2), nameof(BoundsIntField), value);
    }
    protected BoundsInt BoundsIntField(string label, BoundsInt value)
    {
        return EditorGUIControl(value, GetLineHeight(2), nameof(BoundsIntField), label, value);
    }
    protected BoundsInt BoundsIntField(GUIContent label, BoundsInt value)
    {
        return EditorGUIControl(value, GetLineHeight(2), nameof(BoundsIntField), label, value);
    }
    protected Color ColorField(Color value)
    {
        return EditorGUIControl(value, GetLineHeight(), nameof(ColorField), value);
    }
    protected Color ColorField(string label, Color value)
    {
        return EditorGUIControl(value, GetLineHeight(), nameof(ColorField), label, value);
    }
    protected Color ColorField(GUIContent label, Color value)
    {
        return EditorGUIControl(value, GetLineHeight(), nameof(ColorField), label, value);
    }
    protected Color ColorField(GUIContent label, Color value, bool showEyedropper, bool showAlpha, bool hdr)
    {
        return EditorGUIControl(value, GetLineHeight(), nameof(ColorField), label, value, showEyedropper, showAlpha, hdr);
    }
    //protected EditorGUIControl CurveField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl DelayedDoubleField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl DelayedFloatField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl DelayedIntField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl DelayedTextField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl DoubleField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl DropdownButton()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl EnumFlagsField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl EnumPopup()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl FloatField()
    //{
    //    return Standard;
    //}
    protected bool Foldout(bool foldout, string content, GUIStyle style = null)
    {
        if (style == null)
            style = EditorStyles.foldout;
        return EditorGUIControl(foldout, GetLineHeight(), nameof(Foldout), foldout, content, style);
    }
    protected bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style = null)
    {
        if (style == null)
            style = EditorStyles.foldout;
        return EditorGUIControl(foldout, GetLineHeight(), nameof(Foldout), foldout, content, toggleOnLabelClick, style);
    }
    protected bool Foldout(bool foldout, GUIContent content, GUIStyle style = null)
    {
        if (style == null)
            style = EditorStyles.foldout;
        return EditorGUIControl(foldout, GetLineHeight(), nameof(Foldout), foldout, content, style);
    }
    protected bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style = null)
    {
        if (style == null)
            style = EditorStyles.foldout;
        return EditorGUIControl(foldout, GetLineHeight(), nameof(Foldout), foldout, content, toggleOnLabelClick, style);
    }
    //protected EditorGUIControl GradientField()
    //{
    //    return Standard;
    //}
    //// FLAG
    //protected EditorGUIControl HelpBox()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl IntField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl IntPopup()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl IntSlider()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl LabelField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl LayerField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl LinkButton()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl LongField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl MaskField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl MinMaxSlider()
    //{
    //    return Standard;
    //}
    //// FLAG
    //protected EditorGUIControl MultiFloatField()
    //{
    //    return Standard;
    //}
    //// FLAG
    //protected EditorGUIControl MultiIntField()
    //{
    //    return Standard;
    //}
    //// FLAG
    //protected EditorGUIControl MultiPropertyField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl ObjectField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl PasswordField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl Popup()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl PrefixLabel()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl ProgressBar()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl PropertyField()
    //{
    //    if (_property == null)
    //        throw GUIException($"Cannot  the  for this editor GUI  " +
    //            $"because no serialized property has been specified for it");
    //    return EditorGUI.Property(_property, _label, _includeChildren);
    //}
    //protected EditorGUIControl RectField()
    //{
    //    return BoundsField();
    //}
    //protected EditorGUIControl RectIntField()
    //{
    //    return BoundsField();
    //}
    //protected EditorGUIControl SelectableLabel()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl Slider()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl TagField()
    //{
    //    return Standard;
    //}
    //// FLAG
    //protected EditorGUIControl TextArea()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl TextField()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl Toggle()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl ToggleLeft()
    //{
    //    return Standard;
    //}
    //protected EditorGUIControl Vector2Field()
    //{
    //    if (EditorGUIUtility.wideMode)
    //        return Standard;
    //    else
    //        return Standard * 2f;
    //}
    //protected EditorGUIControl Vector2Int()
    //{
    //    return Vector2Field();
    //}
    //protected EditorGUIControl Vector3Field()
    //{
    //    return Vector2Field();
    //}
    //protected EditorGUIControl Vector3IntField()
    //{
    //    return Vector2Field();
    //}
    //protected EditorGUIControl Vector4Field()
    //{
    //    return Vector2Field();
    //}
    // Helper Methods ---------------------------------------------------------------
    private float GetLineHeight(int lines = 1)
    {
        return (lines * EditorGUIUtility.singleLineHeight) + ((lines - 1) * EditorGUIUtility.standardVerticalSpacing);
    }
    private TResult EditorGUIControl<TResult>(TResult value, float height, string methodName, params object[] args)
    {
        ExecuteControlLogic(GetEditorGUIMethodInvoker(methodName, args), height);
        return GetInitialValueOrCurrentControlResult(value);
    }
    private Action GetEditorGUIMethodInvoker(string methodName, params object[] argsSansRect)
    {
        // Setup the arguments
        Type type = typeof(EditorGUI);
        Type[] argTypes = new Type[argsSansRect.Length + 1];
        argTypes[0] = typeof(Rect);
        for (int i = 0; i < argsSansRect.Length; i++)
        {
            if (argsSansRect[i] == null)
                throw GetGUIException($"Argument #{i + 1} cannot be null");
            argTypes[i + 1] = argsSansRect[i].GetType();
        }

        // Try to get the method
        MethodInfo method = type.GetMethod(methodName, argTypes);
        if (method == null)
            throw GetGUIException($"Method '{type}.{methodName}({GetTypesString(argTypes)})' does not exist");

        // We have to use a function that sets the position at invokation time
        void InvokeMethod()
        {
            object[] args = new object[argsSansRect.Length + 1];
            args[0] = _currentPosition;
            argsSansRect.CopyTo(args, 1);
            _currentControlResult = method.Invoke(null, args);
        }

        return InvokeMethod;
    }
    private TResult GetInitialValueOrCurrentControlResult<TResult>(TResult initialValue)
    {
        if (EditorGUI)
            return (TResult)_currentControlResult;
        else
            return initialValue;
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
