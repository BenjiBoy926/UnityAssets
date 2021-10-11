using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourSingleton<BehaviourType> : MonoBehaviour 
    where BehaviourType : MonoBehaviourSingleton<BehaviourType>
{
    #region Protected Properties
    protected static BehaviourType Instance
    {
        get
        {
            if(!instance)
            {
                // Try to instantiate the component from the resources folder
                string typename = typeof(BehaviourType).Name;
                instance = ResourcesExtensions.InstantiateFromResources<BehaviourType>(typename, null);

                // Make the instance not destroyed on load
                DontDestroyOnLoad(instance);
            }
            return instance;
        }
    }
    #endregion

    #region Private Fields
    // The instance of the behaviour
    private static BehaviourType instance;
    #endregion
}
