using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

/// <summary>
/// Used to create a fading effect over the whole screen
/// </summary>
public class FadingPanel : MonoBehaviour
{
    #region Public Properties
    // Instance of the fading panel
    public static FadingPanel Instance { get; private set; }
    public static string PanelPath => nameof(FadingPanel);
    public static Image Image => Instance.image;
    public static bool FadeOutOnStart => Instance.fadeOutOnStart;
    public static float DefaultFadeOutTime => Instance.defaultFadeOutTime;
    public static float DefaultFadeInTime => Instance.defaultFadeInTime;
    #endregion

    #region Private Editor Fields
    [SerializeField]
    [Tooltip("Image to use to create the fading effect")]
    private Image image;
    [SerializeField]
    [Tooltip("If true, the panel should start all black and fade out")]
    private bool fadeOutOnStart;
    [SerializeField]
    [Tooltip("Default amount of time that it takes to perform a fade out")]
    private float defaultFadeOutTime = 1f;
    [SerializeField]
    [Tooltip("Default amount of time it takes to perform a fade in")]
    private float defaultFadeInTime = 0.3f;
    #endregion

    #region Initialize Method
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void Initialize()
    {
        // Load the panel from resources
        GameObject resource = Resources.Load<GameObject>(PanelPath);

        // Check if the resource exists
        if (resource)
        {
            Instance = resource.GetComponent<FadingPanel>();

            // Check if the resource has the component on it
            if (Instance)
            {
                // Instantiate the game object
                resource = Instantiate(resource);
                Instance = resource.GetComponent<FadingPanel>();

                SceneManager.sceneLoaded += Instance.OnSceneLoaded;

                // Make sure it does not destroy on load
                DontDestroyOnLoad(resource);
            }
            else Debug.LogWarning($"The game object {resource} " +
                $"has no fading panel script attached, so the fading panel " +
                $"will not be operable");
        }
        else Debug.LogWarning("No game object could be found at any path " +
            $"Resources/{PanelPath}, so the fading panel will not be operable");
    }
    #endregion

    #region Public Methods
    public static Tweener FadeIn(float time = -1)
    {
        // If the time is invalid then use the default
        if (time <= 0) time = DefaultFadeInTime;

        // Perform the tween
        return Image.DOColor(Color.black, time);
    }
    public static Tweener FadeOut(float time = -1)
    {
        // Use the default if the value is invalid
        if (time <= 0) time = DefaultFadeOutTime;

        // Perform the tween
        return Image.DOColor(Color.clear, time);
    }
    #endregion

    #region Private Methods
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // If we should fade out on start then do it
        if (FadeOutOnStart)
        {
            Image.color = Color.black;
            FadeOut();
        }
        // If we do not fade out on start then make the image invisible
        else Image.color = Color.clear;
    }
    #endregion
}
