using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentReference : CachedObjectReference
{
    #region Public Typedefs
    public enum Type
    {
        GetDirectly,
        GetInParent,
        GetInChildren
    }
    #endregion

    #region Public Properties
    public Type ReferenceType => referenceType;
    public bool IncludeInactive => includeInactive;
    public GameObject GameObject => gameObject;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Determines how to get the component reference")]
    private Type referenceType;
    [SerializeField]
    [Tooltip("Whether to include inactive game objects when searching for the component")]
    private bool includeInactive;
    [SerializeField]
    [Tooltip("Reference to the game object to use to search for the component")]
    private GameObject gameObject;
    #endregion

    #region Public Methods
    public override Object LoadObject(System.Type type)
    {
        if (gameObject)
        {
            if (type.IsSubclassOf(typeof(Component)))
            {
                // Get the component based on if we should get it from children, from parents, or get it directly
                Component component = null;
                switch(referenceType)
                {
                    case Type.GetDirectly: component = gameObject.GetComponent(type); break;
                    case Type.GetInChildren: component = gameObject.GetComponentInChildren(type, includeInactive); break;
                    case Type.GetInParent: component = gameObject.GetComponentInParent(type); break;
                    default: throw new System.NotImplementedException($"{nameof(ComponentReference)}: " +
                        $"component reference not implemented for reference type '{referenceType}'");
                };

                // If you got a component then return it
                if (component) return component;
                // If you got no component then return a missing component exception
                else
                {
                    switch(referenceType)
                    {
                        case Type.GetDirectly: throw new MissingComponentException($"{nameof(ComponentReference)}: " +
                            $"game object '{gameObject}' has no component of type '{type.Name}' attached");
                        case Type.GetInChildren: throw new MissingComponentException($"{nameof(ComponentReference)}: " +
                            $"game object '{gameObject}' has no component of type '{type.Name}' attached" +
                            $"to itself or any of its children");
                        case Type.GetInParent: throw new MissingComponentException($"{nameof(ComponentReference)}: " +
                            $"game object '{gameObject}' has no component of type '{type.Name}' attached" +
                            $"to itself or any of its parents");
                        default: throw new System.NotImplementedException($"{nameof(ComponentReference)}: " +
                            $"component reference not implemented for reference type '{referenceType}'");
                    };
                }
            }
            // If the type supplied does not inherit from component then throw invalid cast exception
            else throw new System.ArgumentException($"{nameof(ComponentReference)}: " +
                $"cannot get a component because type '{type.Name}' is not a component");
        }
        else throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
            $"could not get a component of type '{type.Name}' because no game object was supplied. " +
            $"Make sure you set the value of '{nameof(gameObject)}' in the editor");
    }
    #endregion
}
