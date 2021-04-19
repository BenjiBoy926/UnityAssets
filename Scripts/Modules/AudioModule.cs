using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class AudioModule
{
    public static IEnumerator FadeOut(this AudioSource source, float time)
    {
        UnityAction<float> updateVolume = t =>
        {
            source.volume = Mathf.Lerp(1f, 0f, t);
        };
        yield return CoroutineModule.LerpForTime(time, updateVolume);
        source.Stop();
    }
    public static IEnumerator FadeIn(this AudioSource source, float time)
    {
        source.Play();
        UnityAction<float> updateVolume = t =>
        {
            source.volume = Mathf.Lerp(0f, 1f, t);
        };
        yield return CoroutineModule.LerpForTime(time, updateVolume);
    }
}
