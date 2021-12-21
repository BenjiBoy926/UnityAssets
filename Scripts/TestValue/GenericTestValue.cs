using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GenericTestValue<TValue>
{
    #region Public Properties
    public TValue Value
    {
        get
        {
            if (contextSensitive)
            {
                if (Application.isEditor) return testValue;
                else return releaseValue;
            }
            else return releaseValue;
        }
    }
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("If true, use a different value when testing in the unity editor " +
        "as opposed to the released game")]
    private bool contextSensitive = false;
    [SerializeField]
    [Tooltip("Value to use during tests in the Unity Editor")]
    private TValue testValue;
    [SerializeField]
    [Tooltip("Value to use in the released version of the game")]
    private TValue releaseValue;
    #endregion
}
