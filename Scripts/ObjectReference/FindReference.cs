using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class FindReference : CachedObjectReference
{
    #region Public Typedefs
    public enum Type
    {
        FindDirectly,
        FindWithTag,
        FindWithName,
        FindInChildren
    }
    #endregion

    #region Public Properties
    public Type ReferenceType => referenceType;
    public string Identifier => identifier;
    public Transform Parent => parent;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("How the object should be found in the current scene")]
    private Type referenceType;
    [SerializeField]
    [Tooltip("Tag of the object to find in the scene")]
    private string identifier;
    [SerializeField]
    [Tooltip("Parent to search for the children")]
    private Transform parent;
    #endregion

    #region Public Methods
    public override Object LoadObject(System.Type type)
    {
        bool typeValid = true;
        GameObject gameObjectFound = null;

        // Get the object depending on the type of find reference
        Object reference = null;
        switch(referenceType)
        {
            case Type.FindDirectly: reference = Object.FindObjectOfType(type); break;
            // Get the game object or the component attached depending on the type
            case Type.FindWithName:
                reference = ObjectReferenceUtilities.ReturnGameObjectOrComponent(
                    () => GameObject.Find(identifier), type, out typeValid, out gameObjectFound);
                break;
            case Type.FindWithTag:
                reference = ObjectReferenceUtilities.ReturnGameObjectOrComponent(
                    () => GameObject.FindWithTag(identifier), type, out typeValid, out gameObjectFound);
                break;
            case Type.FindInChildren:
                reference = ObjectReferenceUtilities.ReturnGameObjectOrComponent(() =>
                {
                    if (parent)
                    {
                        Transform child = parent.Find(identifier);
                        if (child) return child.gameObject;
                        else return null;
                    }
                    else return null;
                }, type, out typeValid, out gameObjectFound); 
                break;
        };

        if (reference) return reference;
        // If no reference exists then throw the correct exception
        else
        {
            if(referenceType == Type.FindDirectly) throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
                $"no object of type '{type.Name}' could be found in the scene");
            else if(!typeValid) throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
                $"cannot find a reference of type '{type.Name}' in the scene - the type must be a game object or component");
            // If game object was not found, then specify if we were looking for the game object with the tag,
            // with a name, or in the children of another object
            else if(!gameObjectFound)
            {
                if (referenceType == Type.FindWithName) throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
                     $"no game object found with name '{identifier}'");
                else if (referenceType == Type.FindWithTag) throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
                     $"no game object found with tag '{identifier}'");
                // If we tried to find it in children, specify whether the parent or child was null
                else if (referenceType == Type.FindInChildren)
                {
                    if (!parent) throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
                        $"the variable '{parent}' is null - make sure you set the value of the variable in the editor");
                    else throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
                        $"game object '{parent.gameObject}' has no child named '{identifier}'");
                }
                else throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
                    $"game object could not be found");
            }
            else throw new MissingReferenceException($"{nameof(ComponentReference)}: " +
                $"game object '{gameObjectFound}' has no component of type '{type.Name}' attached");
        }
    }
    #endregion
}
