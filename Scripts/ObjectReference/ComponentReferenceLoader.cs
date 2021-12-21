using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ComponentReferenceLoader : ObjectReferenceLoader
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
    public GameObject GameObject { get; set; }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Determines how to get the component reference")]
    private Type referenceType;
    [SerializeField]
    [Tooltip("Whether to include inactive game objects when searching for the component")]
    private bool includeInactive;
    [SerializeField]
    [Tooltip("Game object used to get the component reference")]
    private GameObject gameObject;
    #endregion

    #region Overridden Methods
    public override Object LoadObject(System.Type type)
    {
        if (GameObject)
        {
            if (type.IsSubclassOf(typeof(Component)))
            {
                // Get the component based on if we should get it from children, from parents, or get it directly
                Component component = null;
                switch (referenceType)
                {
                    case Type.GetDirectly: component = GameObject.GetComponent(type); break;
                    case Type.GetInChildren: component = GameObject.GetComponentInChildren(type, includeInactive); break;
                    case Type.GetInParent: component = GameObject.GetComponentInParent(type); break;
                };

                // If you got no component then set the not found reason before continuing
                if(!component)
                {
                    switch (referenceType)
                    {
                        case Type.GetDirectly:
                            objectNotFoundReason = $"Game object '{GameObject}' has no component of type '{type.Name}' attached";
                            break;
                        case Type.GetInChildren: 
                            objectNotFoundReason = $"Game object '{GameObject}' has no component of type '{type.Name}' attached " +
                                $"to itself or any of its children";
                            break;
                        case Type.GetInParent: 
                            objectNotFoundReason = $"Game object '{GameObject}' has no component of type '{type.Name}' attached " +
                                $"to itself or any of its parents";
                            break;
                        default:
                            objectNotFoundReason = $"Component reference not implemented for reference type '{referenceType}'";
                            break;
                    };
                }

                return component;
            }
            // If the type supplied does not inherit from component then return not found
            else return NotFound($"Cannot get a component because type '{type.Name}' is not a component");
        }
        else return NotFound($"Could not get a component of type '{type.Name}' " +
            $"because the game object property has not been set");
    }
    #endregion
}
