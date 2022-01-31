using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector2Module
{
    public static Vector2 Abs(this Vector2 vector)
    {
        return new Vector2(
            Mathf.Abs(vector.x),
            Mathf.Abs(vector.y));
    }
}
