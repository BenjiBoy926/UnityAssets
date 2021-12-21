using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class ObjectReferenceLoader
{
    #region Public Properties
    public string ObjectNotFoundReason => objectNotFoundReason;
    #endregion

    #region Protected Fields
    protected string objectNotFoundReason = string.Empty;
    #endregion

    #region Virtual Methods
    /// <summary>
    /// Manually load the object reference. Returns null if the object cannot be found
    /// If null is returned then "objectNotFoundReason" should be set to a verbal description
    /// of why it could not be found
    /// </summary>
    /// <returns></returns>
    public abstract Object LoadObject(System.Type type);
    protected virtual Object NotFound(string why)
    {
        objectNotFoundReason = why;
        return null;
    }
    #endregion
}
