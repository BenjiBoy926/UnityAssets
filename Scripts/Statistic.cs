using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Statistic
{
    #region Fields
    [SerializeField]
    private int _sampleCount;
    [SerializeField]
    private float _total = 0;
    [SerializeField]
    private float _average = 0;
    [SerializeField]
    private float _minimum = Mathf.Infinity;
    [SerializeField]
    private float _maximum = Mathf.NegativeInfinity;
    #endregion

    #region Properties
    public int SampleCount => _sampleCount;
    public float Total => _total;
    public float Average => _average;
    public float Maximum => _maximum;
    public float Minimum => _minimum;
    #endregion

    #region Methods
    public void AddSample(float sample)
    {
        _sampleCount++;
        _total += sample;
        _average = _total / _sampleCount;
        _minimum = Mathf.Min(sample, _minimum);
        _maximum = Mathf.Max(sample, _maximum);
    }
    #endregion
}
