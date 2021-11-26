using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectReference
{
    #region Public Typedefs
    public enum Type
    {
        DirectReference,
        FindReference,
        ComponentReference,
        LoadFromResourcesReference
    }
    #endregion

    #region Public Properties
    public Type ReferenceType => referenceType;
    public Object DirectReference => directReference;
    public FindReference FindReference => findReference;
    public ComponentReference ComponentReference => componentReference;
    public ResourcesReference ResourcesReference => resourcesReference;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Determines how the object will be referenced")]
    private Type referenceType;
    [SerializeField]
    [Tooltip("Direct value of the object reference")]
    private Object directReference;
    [SerializeField]
    [Tooltip("Used to compute the reference if it is to be found in the scene")]
    private FindReference findReference;
    [SerializeField]
    [Tooltip("Used to compute the component reference")]
    private ComponentReference componentReference;
    [SerializeField]
    [Tooltip("Used to compute the object reference when loading from resources")]
    private ResourcesReference resourcesReference;
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
        switch(referenceType)
        {
            case Type.DirectReference:
                // Try to return the direct reference, and throw an exception if it fails
                if (directReference) return directReference;
                else throw new MissingReferenceException($"{nameof(ObjectReference)}: " +
                    $"no direct reference found. You probably need to set the value in the editor");
            // Use member variables to compute more complex references
            case Type.FindReference: return findReference.Value(type);
            case Type.ComponentReference: return componentReference.Value(type);
            case Type.LoadFromResourcesReference: return resourcesReference.Value(type);
            // If no previous type is matched then throw not implemented exception
            default: throw new System.NotImplementedException($"{nameof(ObjectReference)}: " +
                $"reference type '{referenceType}' not implemented");
        }
    }
    #endregion
}
