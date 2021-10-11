using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public static class ImageModule
{
    public static IEnumerator Fade(this Image image, Color start, Color end, float time)
    {
        UnityAction<Color> colorUpdate = color =>
        {
            image.color = color;
        };
        yield return ColorModule.Fade(start, end, time, colorUpdate);
    }
    public static IEnumerator FadeIn(this Image image, Color end, float time)
    {
        image.enabled = true;
        yield return image.Fade(Color.clear, end, time);
    }
    public static IEnumerator FadeOut(this Image image, Color start, float time)
    {
        image.enabled = true;
        yield return image.Fade(start, Color.clear, time);
        image.enabled = false;
    }
}
