using System.Linq;
using UnityEngine;
using UnityEditor;

public class ScriptableObjectSingleton<BaseType> : ScriptableObject
    where BaseType : ScriptableObjectSingleton<BaseType>
{
    #region Private Fields
    private static BaseType instance;
    private static string filePath => typeof(BaseType).Name;
    #endregion

    #region Protected Properties
    protected static BaseType Instance
    {
        get
        {
            if (!instance)
            {
                // Try to get a scriptable object at the file path
                BaseType scriptableObject = Resources.Load<BaseType>(filePath);

                // If some objects were found then set the instance to the first one
                if (scriptableObject) instance = scriptableObject;
                // If no instances found then throw exception
                else
                {
                    // Check if the project already has a resources folder
                    bool hasResourcesFolder = AssetDatabase.IsValidFolder("Assets/Resources");

                    // If the folder does not exist then create it
                    if (!hasResourcesFolder) AssetDatabase.CreateFolder("Assets", "Resources");

                    // Create the scriptable object and save it to the asset database
                    scriptableObject = CreateInstance<BaseType>();
                    AssetDatabase.CreateAsset(scriptableObject, "Assets/Resources");
                    AssetDatabase.SaveAssets();
                }
            }
            // If instance is not null return it
            return instance;
        }
    }
    #endregion
}
