using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class ColorModule
{
    public static Color SetAlpha(this Color color, float alpha)
    {
        return new Color(color.r, color.g, color.b, alpha);
    }

    // Flicker the color between the two colors given
    public static IEnumerator Flicker(Color startColor, Color endColor, float totalTime, float flickerTime, UnityAction<Color> callback)
    {
        Color currentColor = startColor;
        float inverseFadeTime = 1f / flickerTime;

        UnityAction<float> update = currentTime =>
        {
            // Ping-pong the interpolator between 0 and 1, and use that for the current color
            currentTime = Mathf.PingPong(currentTime * inverseFadeTime, 1f);
            currentColor = Color.Lerp(startColor, endColor, currentTime);

            // Invoke the callback so that calling method can receive the current color
            callback.Invoke(currentColor);
        };

        yield return CoroutineModule.UpdateForTime(totalTime, update);
    }

    public static IEnumerator Flicker(Color startColor, Color endColor, float totalTime, int numFlickers, UnityAction<Color> callback)
    {
        yield return Flicker(startColor, endColor, totalTime, totalTime / numFlickers, callback);
    }

    public static IEnumerator Fade(Color startColor, Color endColor, float time, UnityAction<Color> callback)
    {
        Color currentColor = startColor;

        // Called on each frame in the update for time routine
        UnityAction<float> update = t =>
        {
            // Lerp the color from start to end
            currentColor = Color.Lerp(startColor, endColor, t);
            
            // Invoke the callback to get the returned color
            callback.Invoke(currentColor);
        };

        yield return CoroutineModule.LerpForTime(time, update);
    }

    public static IEnumerator FadeIn(Color endColor, float time, UnityAction<Color> callback)
    {
        Color startColor = new Color(endColor.r, endColor.g, endColor.b, 0f);
        yield return Fade(startColor, endColor, time, callback);
    }
    public static IEnumerator FadeOut(Color startColor, float time, UnityAction<Color> callback)
    {
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        yield return Fade(startColor, endColor, time, callback);
    }
}
