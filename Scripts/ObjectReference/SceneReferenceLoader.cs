using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneReferenceLoader : ObjectReferenceLoader
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
    public ComponentReferenceLoader ComponentReference => componentReference;
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
    [SerializeField]
    [Tooltip("Method for getting a component from the Game Object reference")]
    private ComponentReferenceLoader componentReference;
    #endregion

    #region Overridden Methods
    public override Object LoadObject(System.Type type)
    {
        // Determine if type is valid or game object is found
        bool typeValid = false;
        GameObject gameObjectFound = null;

        // Load object or get null
        Object reference = null;

        if (referenceType == Type.FindDirectly) reference = Object.FindObjectOfType(type);
        else if (referenceType == Type.FindWithName || referenceType == Type.FindWithTag || referenceType == Type.FindInChildren)
        {
            reference = GetGameObjectOrComponent(type, out typeValid, out gameObjectFound);
        }

        // If no reference exists then set the reason why it was not found
        if(!reference)
        {
            if (referenceType == Type.FindDirectly)
                objectNotFoundReason = $"No object of type '{type.Name}' could be found in the scene";
            else if (!typeValid)
                objectNotFoundReason = $"Cannot find a reference of type '{type.Name}' in the scene " +
                    $"- the type must be a game object or component";
            // If game object was not found, then specify if we were looking for the game object with the tag,
            // with a name, or in the children of another object
            else if(!gameObjectFound)
            {
                if (referenceType == Type.FindWithName)
                    objectNotFoundReason = $"No game object found with name '{identifier}'";
                else if (referenceType == Type.FindWithTag)
                    objectNotFoundReason = $"No game object found with tag '{identifier}'";
                // If we tried to find it in children, specify whether the parent or child was null
                else if (referenceType == Type.FindInChildren)
                {
                    if (!parent) objectNotFoundReason = $"The parent transform variable was not set in the editor";
                    else objectNotFoundReason = $"Game object '{parent.gameObject}' has no child named '{identifier}'";
                }
                else objectNotFoundReason = "Game object could not be found";
            }
            else objectNotFoundReason = $"'{gameObjectFound}' has no component of type '{type.Name}' attached. " +
                $"Error details: {componentReference.ObjectNotFoundReason}";
        }

        return reference;
    }
    #endregion

    #region Private Methods
    private GameObject GetGameObject()
    {
        if (referenceType == Type.FindWithName) return GameObject.Find(identifier);
        else if (referenceType == Type.FindWithTag) return GameObject.FindWithTag(identifier);
        else if(referenceType == Type.FindInChildren)
        {
            if (parent)
            {
                Transform child = parent.Find(identifier);
                if (child) return child.gameObject;
                else return null;
            }
            else return null;
        }
        else return null;
    }
    private Object GetGameObjectOrComponent(System.Type type, out bool typeValid, out GameObject gameObjectFound)
    {
        bool typeofComponent = type.IsSubclassOf(typeof(Component));
        bool typeofGameObject = type.IsSubclassOf(typeof(GameObject));

        // Type is valid if this is a game object or component type
        typeValid = typeofComponent || typeofGameObject;
        // Assume game object is not found to start
        gameObjectFound = null;

        if (typeofComponent || typeofGameObject)
        {
            gameObjectFound = GetGameObject();

            // If the game object was found check whether we want the game object or a component on it
            if (gameObjectFound)
            {
                // If the type we are looking for is a component then return the component on the object
                if (typeofComponent)
                {
                    componentReference.GameObject = gameObjectFound;
                    return componentReference.LoadObject(type);
                }
                // If the type we are looking for is a game object then return it
                else return gameObjectFound;
            }
            // If the game object was not found then return null
            else return null;
        }
        else return null;
    }
    #endregion
}
