using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectReference : ObjectReferenceLoader
{
    #region Public Typedefs
    public enum Type
    {
        DirectReference,
        SceneReference,
        ResourcesReference
    }
    #endregion

    #region Public Properties
    public Type ReferenceType => referenceType;
    public bool CanCacheObject { get => canCacheObject; set => canCacheObject = value; }
    public Object Cache { get => cache; set => cache = value; }
    public Object DirectReference => directReference;
    public SceneReferenceLoader SceneReference => sceneReference;
    public ResourcesReferenceLoader ResourcesReference => resourcesReference;
    #endregion

    #region Private Properties
    private MissingReferenceException ObjectNotFoundException => new MissingReferenceException($"{nameof(ObjectReference)}: {objectNotFoundReason}");
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Determine if the object should be cached " +
        "or if it should be manually re-discovered each time it is accessed")]
    private bool canCacheObject;
    [SerializeField]
    [Tooltip("Determines how the object will be referenced")]
    private Type referenceType;
    [SerializeField]
    [Tooltip("Direct value of the object reference")]
    private Object directReference;
    [SerializeField]
    [Tooltip("Used to compute the reference if it is to be found in the scene")]
    private SceneReferenceLoader sceneReference;
    [SerializeField]
    [Tooltip("Used to compute the object reference when loading from resources")]
    private ResourcesReferenceLoader resourcesReference;
    #endregion

    #region Private Fields
    private Object cache = null;
    private bool cacheIsDirty = false;
    #endregion

    #region Public Methods
    public T Get<T>() where T : Object
    {
        // Get the object reference and try to type cast it
        Object objectReference = Get(typeof(T));
        T reference = objectReference as T;

        // If the typecast succeeds then return the reference
        if (reference) return reference;
        // If the typecast fails then throw an exception
        else throw new System.InvalidCastException($"{nameof(ObjectReferenceLoader)}: " +
            $"Unable to convert object '{objectReference}' (type '{objectReference.GetType().Name}') to type '{typeof(T).Name}'");
    }
    public Object Get(System.Type type)
    {
        Object reference = LoadFromCache(type);

        // If reference not found then throw exception
        if (reference) return reference;
        else throw ObjectNotFoundException;
    }
    public T OrElseNull<T>() where T : Object => OrElse<T>(null);
    public Object OrElseNull(System.Type type) => OrElse(type, null);
    public T OrElse<T>(T ifNull) where T : Object
    {
        T reference = OrElse(typeof(T), ifNull) as T;

        if (reference) return reference;
        else return ifNull;
    }
    public Object OrElse(System.Type type, Object ifNull)
    {
        Object reference = LoadFromCache(type);

        // If reference exists return it, otherwise return other value
        if (reference) return reference;
        else return ifNull;
    }
    public bool GetCacheDirty<T>() where T : Object => GetCacheDirty(typeof(T));
    public bool GetCacheDirty(System.Type type)
    {
        return !cache || type != cache.GetType() || cacheIsDirty;
    }
    public bool SetCacheDirty() => cacheIsDirty = true;
    #endregion

    #region Overridden Methods
    public override Object LoadObject(System.Type type)
    {
        Object reference = null;

        switch (referenceType)
        {
            case Type.DirectReference: 
                reference = directReference;
                break;
            // Use member variables to compute more complex references
            case Type.SceneReference: 
                reference = sceneReference.LoadObject(type);
                break;
            case Type.ResourcesReference: 
                reference = resourcesReference.LoadObject(type);
                break;
        }

        // If the reference could not be found then set the reason why
        if(!reference)
        {
            switch (referenceType)
            {
                case Type.DirectReference:
                    objectNotFoundReason = $"No direct reference found. You probably need to set the value in the editor";
                    break;
                // Use member variables to compute more complex references
                case Type.SceneReference:
                    objectNotFoundReason = sceneReference.ObjectNotFoundReason;
                    break;
                case Type.ResourcesReference:
                    objectNotFoundReason = resourcesReference.ObjectNotFoundReason;
                    break;
            }
        }

        return reference;
    }
    #endregion

    #region Private Methods
    private Object LoadFromCache(System.Type type)
    {
        // If the object can be cached, then get the cached object
        if (canCacheObject)
        {
            // If the cache is dirty then reload it
            if (GetCacheDirty(type))
            {
                Object result = LoadObject(type);

                // If result is not null then set the cache
                if (result) cache = result; 
                // otherwise return null
                else return result;
            }
            return cache;
        }
        else return LoadObject(type);
    }
    #endregion
}
