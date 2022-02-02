using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualFunc<T>
{
    #region Public Properties
    public bool HasVirtualFunc => virtualFunc != null;
    public bool HasOverrideFunc => overrideFunc != null;
    #endregion

    #region Private Fields
    private Func<T> virtualFunc;
    private Func<T> overrideFunc;
    #endregion

    #region Public Methods
    public void SetVirtual(Func<T> virtualFunc)
    {
        this.virtualFunc = virtualFunc;
    }
    public void SetOverride(Func<T> overrideFunc)
    {
        this.overrideFunc = overrideFunc;
    }
    /// <summary>
    /// Invoke the virtual method on this action
    /// </summary>
    /// <returns>True if the virtual action was successfully invoked</returns>
    public T InvokeVirtual()
    {
        if (virtualFunc != null) return virtualFunc.Invoke();
        else throw new NullReferenceException($"{nameof(VirtualFunc<T>)}: " +
            $"No virtual function supplied, so we cannot get a result");
    }
    /// <summary>
    /// Invoke the override method if this action is overridden, 
    /// otherwise invoke the virtual method
    /// </summary>
    /// <returns>True if either the override action of virtual action was successfully invoked</returns>
    public T Invoke()
    {
        if (overrideFunc != null) return overrideFunc.Invoke();
        else return InvokeVirtual();
    }
    #endregion
}
