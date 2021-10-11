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
            return instance;
        }
    }
    #endregion

    #region Private Fields
    // The instance of the behaviour
    private static BehaviourType instance;
    #endregion
}
