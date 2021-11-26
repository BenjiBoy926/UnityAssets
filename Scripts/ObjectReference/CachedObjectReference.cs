using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class CachedObjectReference
{
    #region Public Properties
    public bool CanCacheObject { get => canCacheObject; set => canCacheObject = value; }
    public Object Cache { get => cache; set => cache = value; }
    /// <summary>
    /// Determine if the reference is "dirty"
    /// If it is, then next time a client attempts to load the value,
    /// the cached value is reloaded even if it is null
    /// </summary>
    public bool Dirty { get; set; }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Determine if the object should be cached " +
        "or if it should be manually re-discovered each time it is accessed")]
    private bool canCacheObject;
    #endregion

    #region Private Fields
    private Object cache;
    #endregion

    #region Public Methods
    public T Value<T>() where T : Object
    {
        // Get the object reference and try to type cast it
        Object objectReference = Value(typeof(T));
        T reference = objectReference as T;

        // If the typecast succeeds then return the reference
        if (reference) return reference;
        // If the typecast fails then throw an exception
        else throw new System.InvalidCastException($"{nameof(CachedObjectReference)}: " +
            $"unable to convert object '{objectReference}' (type '{objectReference.GetType().Name}') to type '{typeof(T).Name}'");
    }
    public Object Value(System.Type type)
    {
        // If the object can be cached, the get the cached object
        if (canCacheObject)
        {
            // If the cache doesn't exist or is dirty then reload it
            if (!cache || Dirty)
            {
                cache = LoadObject(type);
            }
            return cache;
        }
        // If the object cannot be cached then load it manually
        else return LoadObject(type);
    }
    /// <summary>
    /// Manually load the object reference. 
    /// This function should never return null! 
    /// Instead it throws an exception if the object could not be loaded
    /// </summary>
    /// <returns></returns>
    public abstract Object LoadObject(System.Type type);
    #endregion
}
