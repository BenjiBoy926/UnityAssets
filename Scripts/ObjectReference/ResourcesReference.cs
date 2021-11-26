using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ResourcesReference : CachedObjectReference
{
    #region Public Typedefs
    public enum Type
    {
        LoadDirectly,
        LoadAny
    }
    #endregion

    #region Public Properties
    public Type ReferenceType => referenceType;
    public string ResourcePath => resourcePath;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("How the resource should be loaded")]
    private Type referenceType;
    [SerializeField]
    [Tooltip("Path of the resource to load, relative to a 'Resources' folder")]
    private string resourcePath;
    #endregion

    #region Public Methods
    public override Object LoadObject(System.Type type)
    {
        // Assume the reference is null to start
        Object reference = null;

        // Set the reference based on the reference type
        switch(referenceType)
        {
            case Type.LoadDirectly:
                reference = Resources.Load(resourcePath, type);
                break;
            case Type.LoadAny:
                if(type.IsSubclassOf(typeof(Component)))
                {
                    GameObject[] gameObjects = Resources.LoadAll<GameObject>(resourcePath);
                    int index = 0;

                    // While the reference does not exist and we are in range of the objects,
                    // try to set the reference equal to the component on the game object
                    while(!reference && index < gameObjects.Length)
                    {
                        reference = gameObjects[0].GetComponent(type);
                        index++;
                    }
                }
                else
                {
                    Object[] array = Resources.LoadAll(resourcePath, type);
                    // If some objects were loaded then take the first one
                    if (array.Length > 0) reference = array[0];
                }
                break;
        }

        if (reference) return reference;
        else
        {
            // Throw the correct exception based on how the reference failed
            switch (referenceType)
            {
                case Type.LoadDirectly: throw new MissingReferenceException($"{nameof(ResourcesReference)}: " +
                    $"no object of type '{type.Name}' found at resources path '{resourcePath}'");
                case Type.LoadAny:
                    if (type.IsSubclassOf(typeof(Component))) throw new MissingReferenceException($"{nameof(ResourcesReference)}: " +
                        $"no component of type '{type.Name}' could be found in any game object at resource path '{resourcePath}'");
                    else throw new MissingReferenceException($"{nameof(ResourcesReference)}: " +
                        $"no object of type '{type.Name}' found in resource path '{resourcePath}'");
                default: throw new System.NotImplementedException($"{nameof(ResourcesReference)}: " +
                    $"reference type '{referenceType}' not implemented");
            }
        }
    }
    #endregion
}
