using System.Linq;
using UnityEngine;

public class ScriptableObjectSingleton<BaseType> : ScriptableObject
    where BaseType : ScriptableObjectSingleton<BaseType>
{
    #region Private Properties
    public virtual string FilePath => typeof(BaseType).Name;
    #endregion

    #region Private Fields
    private static BaseType instance;
    #endregion

    #region Protected Properties
    protected static BaseType Instance
    {
        get
        {
            if (!instance)
            {
                // Create a dummy object that will not be saved to assets
                // This is used to get the overridden file path
                BaseType dummyObject = CreateInstance<BaseType>();
                string filePath = dummyObject.FilePath;

                // Try to get a scriptable object at the file path
                BaseType scriptableObject = Resources.Load<BaseType>(dummyObject.FilePath);

                // If some objects were found then set the instance to the first one
                if (scriptableObject) instance = scriptableObject;
                // If no instances found then throw exception
                else
                {
                    string typename = typeof(BaseType).Name;
                    throw new MissingReferenceException(
                        $"No scriptable object of type '{typename}' " +
                        $"could be found at any resources path 'Resources/{filePath}'. " +
                        $"Make sure a scriptable object with this type " +
                        $"exists at this path, or override the '{nameof(FilePath)}' " +
                        $"property in the '{typename}' source code");
                }
            }
            // If instance is not null return it
            return instance;
        }
    }
    #endregion
}
