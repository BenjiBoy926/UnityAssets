using System.Collections;
using UnityEngine;

public static class GameObjectModule
{
    public static TComponent GetOrAddComponent<TComponent>(this GameObject gameObject) 
        where TComponent : Component
    {
        TComponent component = gameObject.GetComponent<TComponent>();
        if (!component) component = gameObject.AddComponent<TComponent>();
        return component;
    }

    public static bool CompareTagInParent(this GameObject gameObject, string tag)
    {
        if (gameObject.CompareTag(tag)) return true;
        else
        {
            // Get the first parent of the object
            Transform parent = gameObject.transform.parent;

            // Loop until the parent is null
            while(parent != null)
            {
                // If this parent has the same tag, return true
                if (parent.CompareTag(tag)) return true;
                // If not, update to the parent of this parent
                else parent = parent.transform.parent;
            }

            // If we reach the end of the loop, we did not find any parents with the tag
            return false;
        }
    }

    public static bool CompareTagInParent(this Component component, string tag)
    {
        return component.gameObject.CompareTagInParent(tag);
    }
}
