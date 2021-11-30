using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectReferenceUtilities
{
    #region Public Methods
    public static Object GetGameObjectOrComponent(System.Func<GameObject> gameObjectGetter, System.Type type, out bool typeValid, out GameObject gameObjectFound)
    {
        bool typeofComponent = type.IsSubclassOf(typeof(Component));
        bool typeofGameObject = type.IsSubclassOf(typeof(GameObject));

        // Type is valid if this is a game object or component type
        typeValid = typeofComponent || typeofGameObject;
        // Assume game object is not found to start
        gameObjectFound = null;

        if (typeofComponent || typeofGameObject)
        {
            gameObjectFound = gameObjectGetter.Invoke();

            // If the game object was found check whether we want the game object or a component on it
            if (gameObjectFound)
            {
                // If the type we are looking for is a component then return the component on the object
                if (typeofComponent) return gameObjectFound.GetComponent(type);
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
