using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualAction
{
    #region Private Fields
    private Action virtualAction;
    private Action overrideAction;
    #endregion

    #region Public Methods
    public void SetVirtual(Action virtualAction)
    {
        this.virtualAction = virtualAction;
    }
    public void SetOverride(Action overrideAction)
    {
        this.overrideAction = overrideAction;
    }
    /// <summary>
    /// Invoke the virtual method on this action
    /// </summary>
    /// <returns>True if the virtual action was successfully invoked</returns>
    public bool InvokeVirtual()
    {
        virtualAction?.Invoke();
        return virtualAction != null;
    }
    /// <summary>
    /// Invoke the override method if this action is overridden, 
    /// otherwise invoke the virtual method
    /// </summary>
    /// <returns>True if either the override action of virtual action was successfully invoked</returns>
    public bool Invoke()
    {
        if (overrideAction != null)
        {
            overrideAction.Invoke();
            return true;
        }
        else return InvokeVirtual();
    }
    #endregion
}

public class VirtualAction<T>
{
    #region Private Fields
    private Action<T> virtualAction;
    private Action<T> overrideAction;
    #endregion

    #region Public Methods
    public void SetVirtual(Action<T> virtualAction)
    {
        this.virtualAction = virtualAction;
    }
    public void SetOverride(Action<T> overrideAction)
    {
        this.overrideAction = overrideAction;
    }
    /// <summary>
    /// Invoke the virtual method on this action
    /// </summary>
    /// <returns>True if the virtual action was successfully invoked</returns>
    public bool InvokeVirtual(T arg0)
    {
        virtualAction?.Invoke(arg0);
        return virtualAction != null;
    }
    /// <summary>
    /// Invoke the override method if this action is overridden, 
    /// otherwise invoke the virtual method
    /// </summary>
    /// <returns>True if either the override action of virtual action was successfully invoked</returns>
    public bool Invoke(T arg0)
    {
        if (overrideAction != null)
        {
            overrideAction.Invoke(arg0);
            return true;
        }
        else return InvokeVirtual(arg0);
    }
    #endregion
}

