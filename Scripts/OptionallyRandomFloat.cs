using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct OptionallyRandomFloat
{
    [SerializeField]
    [Tooltip("Determine whether the float supplied is a constant value or a random value")]
    private RandomOption option;
    [SerializeField]
    [Tooltip("Value of the float")]
    private float value;
    [SerializeField]
    [Tooltip("Range of values that the float can have")]
    private FloatRange valueRange;

    private OptionallyRandomFloat(RandomOption option, float value, FloatRange valueRange)
    {
        this.option = option;
        this.value = value;
        this.valueRange = valueRange;
    }

    public static OptionallyRandomFloat Constant(float value)
    {
        return new OptionallyRandomFloat(RandomOption.Constant, value, new FloatRange());
    }

    public static OptionallyRandomFloat Random(FloatRange range)
    {
        return new OptionallyRandomFloat(RandomOption.Random, 0f, range);
    }

    public float Get()
    {
        if (option == RandomOption.Constant)
        {
            return value;
        }
        else return UnityEngine.Random.Range(valueRange.min, valueRange.max);
    }
}
