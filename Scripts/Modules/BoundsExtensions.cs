using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BoundsExtensions
{
    /// <summary>
    /// Get a point inside the bounds using interpolation along
    /// the x, y, and z axes
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="t"></param>
    /// <returns></returns>
    public static Vector3 GetPointInside(this Bounds bounds, Vector3 t)
    {
        for (int i = 0; i < 3; i++)
            t[i] = Mathf.Lerp(bounds.min[i], bounds.max[i], t[i]);
        return t;
    }
    /// <summary>
    /// Get a point inside the bounds using interpolation along
    /// the x, y, and z axes
    /// </summary>
    /// <param name="bounds"></param>
    /// <param name="tx">Number from 0-1 to interpolate between min.x and max.x</param>
    /// <param name="ty">Number from 0-1 to interpolate between min.y and max.y</param>
    /// <param name="tz">Number from 0-1 to interpolate between min.z and max.z</param>
    /// <returns></returns>
    public static Vector3 GetPointInside(this Bounds bounds, float tx, float ty, float tz)
    {
        return bounds.GetPointInside(new Vector3(tx, ty, tz));
    }
}
