using System;
using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public abstract class EnhancedPropertyDrawer : PropertyDrawer
{
    #region Types
    protected enum LogicType
    {
        GetPropertyHeight, OnGUI
    }
    protected class HorizontalScope : IDisposable
    {
        private EnhancedPropertyDrawer _drawer;
        private int _controlsInScope = 0;
        private float _maxControlHeight = 0f;
        private bool _isDisposed = false;

        public int ControlsInScope => _controlsInScope;
        public bool IsDisposed => _isDisposed;

        public HorizontalScope(EnhancedPropertyDrawer drawer)
        {
            _drawer = drawer;
            _controlsInScope = 0;
            _maxControlHeight = 0f;
            _isDisposed = false;
        }
        public void IncrementControlsInScope(float height)
        {
            _controlsInScope++;
            _maxControlHeight = Mathf.Max(_maxControlHeight, height);
        }
        public void Dispose()
        {
            _drawer.AdvanceCurrentPositionVertically(_maxControlHeight);
            _drawer.CurrentPropertyHeight += _maxControlHeight;
            _isDisposed = true;
        }
    }
    #endregion

    #region Fields
    private Rect _totalPosition = Rect.zero;
    private Rect _currentPosition = Rect.zero;
    private bool _currentControlIsPrefix = false;
    private HorizontalScope _currentHorizontalScope = null;
    #endregion

    #region Properties
    protected LogicType CurrentLogic { get; private set; } = LogicType.GetPropertyHeight;
    protected float CurrentPropertyHeight { get; private set; } = 0f;
    protected Rect TotalPosition => _totalPosition;
    protected Rect CurrentPosition => _currentPosition;
    protected object CurrentControlResult { get; private set; } = null;
    protected float CurrentViewIndentedWidth => EditorGUI.IndentedRect(_totalPosition).width;
    protected bool IsInHorizontalScope => _currentHorizontalScope != null && !_currentHorizontalScope.IsDisposed;
    #endregion

    #region Methods
    // Key Methods ------------------------------------------------------------------------------------
    protected abstract void ExecutePropertyLogic(SerializedProperty property, GUIContent label);
    protected void ExecuteControlLogic(Action gui, float width = 0f, float height = 0f)
    {
        if (height <= 0.001f)
            height = EditorGUIUtility.singleLineHeight;

        if (IsInHorizontalScope)
            _currentHorizontalScope.IncrementControlsInScope(height);

        if (CurrentLogic == LogicType.OnGUI)
        {
            if (width > 0.001f)
                _currentPosition.width = width;
            _currentPosition.height = height;

            // Use no indentation at all for controls in the horizontal scope after the first control
            EditorGUI.IndentLevelScope indentScope = null;
            if (IsInHorizontalScope && _currentHorizontalScope.ControlsInScope > 1)
                indentScope = new EditorGUI.IndentLevelScope(-EditorGUI.indentLevel);
            
            // Invoke the gui for the control
            gui.Invoke();

            // Dispose of any indent scope that may have been used
            if (indentScope != null)
                indentScope.Dispose();

            if (_currentControlIsPrefix)
                _currentPosition = (Rect)CurrentControlResult;
            else
            {
                // Change how the current position updates based on horizontal or vertical scope
                if (IsInHorizontalScope)
                    _currentPosition.x += _currentPosition.width + EditorGUIUtility.standardVerticalSpacing;
                else AdvanceCurrentPositionVertically(height);
            }
        }
        // Only update the current property height when not in a horizontal scope
        else if (!IsInHorizontalScope && !_currentControlIsPrefix)
            CurrentPropertyHeight += height + EditorGUIUtility.standardVerticalSpacing;

        // Set current control is prefix to false
        _currentControlIsPrefix = false;
    }
    protected HorizontalScope GetHorizontalScope()
    {
        _currentHorizontalScope = new HorizontalScope(this);
        return _currentHorizontalScope;
    }
    // Overrides ---------------------------------------------------------------------------
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        if (position.size.sqrMagnitude > 5f)
        {
            _totalPosition = position;
            _currentPosition = position;
        }
        CurrentLogic = LogicType.OnGUI;
        ExecutePropertyLogic(property, label);
    }
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        CurrentLogic = LogicType.GetPropertyHeight;
        CurrentPropertyHeight = 0f;
        ExecutePropertyLogic(property, label);
        return CurrentPropertyHeight;
    }
    // Delegate Methods -----------------------------------------------------------
    protected Bounds BoundsField(Bounds value, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(2), nameof(BoundsField), value);
    }
    protected Bounds BoundsField(GUIContent label, Bounds value, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(2), nameof(BoundsField), label, value);
    }

    protected BoundsInt BoundsIntField(BoundsInt value, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(2), nameof(BoundsIntField), value);
    }
    protected BoundsInt BoundsIntField(string label, BoundsInt value, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(2), nameof(BoundsIntField), label, value);
    }
    protected BoundsInt BoundsIntField(GUIContent label, BoundsInt value, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(2), nameof(BoundsIntField), label, value);
    }
    
    protected Color ColorField(Color value, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(), nameof(ColorField), value);
    }
    protected Color ColorField(string label, Color value, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(), nameof(ColorField), label, value);
    }
    protected Color ColorField(GUIContent label, Color value, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(), nameof(ColorField), label, value);
    }
    protected Color ColorField(GUIContent label, Color value, bool showEyedropper, bool showAlpha, bool hdr, float width = 0f)
    {
        return EditorGUIControl(value, width, GetTotalControlHeight(), nameof(ColorField), label, value, showEyedropper, showAlpha, hdr);
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
    protected bool Foldout(bool foldout, string content, GUIStyle style = null, float width = 0f)
    {
        if (style == null)
            style = EditorStyles.foldout;
        return EditorGUIControl(foldout, width, GetTotalControlHeight(), nameof(Foldout), foldout, content, style);
    }
    protected bool Foldout(bool foldout, string content, bool toggleOnLabelClick, GUIStyle style = null, float width = 0f)
    {
        if (style == null)
            style = EditorStyles.foldout;
        return EditorGUIControl(foldout, width, GetTotalControlHeight(), nameof(Foldout), foldout, content, toggleOnLabelClick, style);
    }
    protected bool Foldout(bool foldout, GUIContent content, GUIStyle style = null, float width = 0f)
    {
        if (style == null)
            style = EditorStyles.foldout;
        return EditorGUIControl(foldout, width, GetTotalControlHeight(), nameof(Foldout), foldout, content, style);
    }
    protected bool Foldout(bool foldout, GUIContent content, bool toggleOnLabelClick, GUIStyle style = null, float width = 0f)
    {
        if (style == null)
            style = EditorStyles.foldout;
        return EditorGUIControl(foldout, width, GetTotalControlHeight(), nameof(Foldout), foldout, content, toggleOnLabelClick, style);
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

    protected int IntField(int value, GUIStyle style = null, float width = 0f)
    {
        if (style == null)
            style = EditorStyles.numberField;
        return EditorGUIControl(value, width, GetTotalControlHeight(), nameof(IntField), value, style);
    }
    protected int IntField(string label, int value, GUIStyle style = null, float width = 0f)
    {
        if (style == null)
            style = EditorStyles.numberField;
        return EditorGUIControl(value, width, GetTotalControlHeight(), nameof(IntField), label, value, style);
    }
    protected int IntField(GUIContent label, int value, GUIStyle style = null, float width = 0f)
    {
        if (style == null)
            style = EditorStyles.numberField;
        return EditorGUIControl(value, width, GetTotalControlHeight(), nameof(IntField), label, value, style);
    }

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
    protected Rect PrefixLabel(GUIContent label, float width = 0f)
    {
        _currentControlIsPrefix = true;
        return EditorGUIControl(_currentPosition, width, GetTotalControlHeight(), nameof(PrefixLabel), label);
    }
    protected Rect PrefixLabel(GUIContent label, GUIStyle style, float width = 0f)
    {
        _currentControlIsPrefix = true;
        return EditorGUIControl(_currentPosition, width, GetTotalControlHeight(), nameof(PrefixLabel), label, style);
    }
    protected Rect PrefixLabel(int id, GUIContent label, float width = 0f)
    {
        _currentControlIsPrefix = true;
        return EditorGUIControl(_currentPosition, width, GetTotalControlHeight(), nameof(PrefixLabel), id, label);
    }
    protected Rect PrefixLabel(int id, GUIContent label, GUIStyle style, float width = 0f)
    {
        _currentControlIsPrefix = true;
        return EditorGUIControl(_currentPosition, width, GetTotalControlHeight(), nameof(PrefixLabel), id, label, style);
    }
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
    private void AdvanceCurrentPositionVertically(float height)
    {
        _currentPosition.x = _totalPosition.x;
        _currentPosition.y += height + EditorGUIUtility.standardVerticalSpacing;
        _currentPosition.width = _totalPosition.width;
    }
    private float GetTotalControlHeight(int lines = 1)
    {
        return (lines * EditorGUIUtility.singleLineHeight) + ((lines - 1) * EditorGUIUtility.standardVerticalSpacing);
    }
    private TResult EditorGUIControl<TResult>(TResult value, float width, float height, string methodName, params object[] argsWithoutPosition)
    {
        ExecuteControlLogic(GetEditorGUIMethodInvoker(methodName, argsWithoutPosition), width, height);
        if (CurrentLogic == LogicType.OnGUI)
            return (TResult)CurrentControlResult;
        else
            return value;
    }
    private Action GetEditorGUIMethodInvoker(string methodName, params object[] argsWithoutPosition)
    {
        // Setup the arguments
        Type type = typeof(EditorGUI);
        Type[] argTypes = new Type[argsWithoutPosition.Length + 1];
        argTypes[0] = typeof(Rect);
        for (int i = 0; i < argsWithoutPosition.Length; i++)
        {
            if (argsWithoutPosition[i] == null)
                throw GetGUIException($"Argument #{i + 1} cannot be null");
            argTypes[i + 1] = argsWithoutPosition[i].GetType();
        }

        // Try to get the method
        MethodInfo method = type.GetMethod(methodName, argTypes);
        if (method == null)
            throw GetGUIException($"Method '{type}.{methodName}({GetTypesString(argTypes)})' does not exist");

        // We have to use a function that sets the position at invokation time
        void InvokeMethod()
        {
            object[] args = new object[argsWithoutPosition.Length + 1];
            args[0] = _currentPosition;
            argsWithoutPosition.CopyTo(args, 1);
            CurrentControlResult = method.Invoke(null, args);
        }

        return InvokeMethod;
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
