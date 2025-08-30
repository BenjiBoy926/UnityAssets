﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public static class MovementModule
{
    // SHIFT
    // Change the position by the given direction
    public static void Shift(this Rigidbody2D rb2D, Vector2 dir, float speed)
    {
        dir = dir.normalized * speed * Time.fixedDeltaTime;
        rb2D.Shift(dir);
    }
    public static void Shift(this Rigidbody2D rb2D, Vector2 delta)
    {
        rb2D.MovePosition(rb2D.position + delta);
    }
    // SEND
    // Send the rigidbody away with the given velocity
    public static void Send(this Rigidbody2D rb2D, Vector2 dir, float speed)
    {
        dir = dir.normalized * speed;
        rb2D.Send(dir);
    }
    public static void Send(this Rigidbody2D rb2D, Vector2 velocity)
    {
        rb2D.velocity = velocity;
    }

    // Shift the rigidbody by designated amount over given amount of time
    public static IEnumerator ShiftOverTime(this Rigidbody2D rb2D, Vector2 shift, float time)
    {
        yield return rb2D.MoveOverTime(rb2D.position + shift, time);
    }

    public static IEnumerator MoveOverTime(this Transform transform, Vector3 endingPos, float time)
    {
        // Store the starting position of the transform
        Vector3 startPos = transform.position;

        UnityAction<float> updatePosition = t =>
        {
            transform.position = Vector3.Lerp(startPos, endingPos, t);
        };

        // Run an update loop for time, updating position
        yield return CoroutineModule.LerpForTime(time, updatePosition);
    }

    // Infinitely ping-pong the transform
    public static IEnumerator PingPong(this Transform transform, Vector3 startingPos, Vector3 endingPos, float moveTime)
    {
        while(true)
        {
            yield return transform.MoveOverTime(endingPos, moveTime);
            yield return transform.MoveOverTime(startingPos, moveTime);
        }
    }
    // Ping pong the transform the designated number of times
    public static IEnumerator PingPong(this Transform transform, Vector3 startingPos, Vector3 endingPos, float moveTime, int numMoves)
    {
        for(int i = 0; i < numMoves; i++)
        {
            yield return transform.MoveOverTime(endingPos, moveTime);
            yield return transform.MoveOverTime(startingPos, moveTime);
        }
    }
    // Infinitely shake the transform
    public static IEnumerator Shake2D(this Transform transform, Vector3 basePosition, float shakeMagnitude, float shakeInterval)
    {
        WaitForSeconds wait = new(shakeInterval);
        while(true)
        {
            transform.position = basePosition + (Vector3)(Random.insideUnitCircle * shakeMagnitude);
            yield return wait;
        }
    }

    public static IEnumerator MoveOverTime(this Rigidbody2D rb2D, Vector2 endingPos, float time)
    {
        // Store the inverse of the time so we can use multiplication, which is more efficient than repeated division
        float inverseTime = 1f / time;

        // Store the starting position of the rigidbody.  We use this to linearly interpolate to the end positiong
        Vector2 startingPos = rb2D.position;

        // This function updates the rigidbody position by linearly interpolating it between the start and ending position
        void UpdatePosition(float currentTime)
        {
            rb2D.MovePosition(Vector2.Lerp(startingPos, endingPos, currentTime * inverseTime));
        }

        // Run fixed update for time, where the action updates the rigidbody position
        yield return CoroutineModule.FixedUpdateForTime(time, UpdatePosition);
    }

    public static IEnumerator RotateOverTime(this Rigidbody2D rb2D, float amount, float time, RotationDirection direction)
    {
        // Store the inverse time so we can use multiplication, which is more efficient than division
        float inverseTime = 1f / time;

        // Store starting and ending rotation values to interpolate between
        float startingRotation = rb2D.rotation;

        // Initiaize the ending rotation. If rotating clockwise, subtract the rotation amount
        float endingRotation;
        if (direction == RotationDirection.Clockwise)
        {
            endingRotation = startingRotation - amount;
        }
        else
        {
            endingRotation = startingRotation + amount;
        }

        // This function linearly interpolates between starting and ending rotations for the rigidbody
        void UpdateRotation(float currentTime)
        {
            rb2D.MoveRotation(Mathf.Lerp(startingRotation, endingRotation, currentTime * inverseTime));
        }

        // Run the fixed update for some time, updating the rigidbody rotation
        yield return CoroutineModule.FixedUpdateForTime(time, UpdateRotation);
        
        // Make sure that the ending rotation is exactly the roation the rigidbody ends on
        rb2D.rotation = endingRotation;
    }
}

public enum RotationDirection
{
    Clockwise, CounterClockwise
}
