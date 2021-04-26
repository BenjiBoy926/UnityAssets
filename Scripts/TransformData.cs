using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TransformData
{
    [SerializeField]
    [Tooltip("Position for the transform")]
    private Vector3 _position;
    [SerializeField]
    [Tooltip("Rotation for the transform")]
    private Vector3 _rotation;
    [SerializeField]
    [Tooltip("Scale for the transform")]
    private Vector3 _scale;

    public Vector3 position => _position;
    public Vector3 rotation => _rotation;
    public Vector3 scale => _scale;

    public TransformData(Vector3 position, Vector3 rotation, Vector3 scale)
    {
        _position = position;
        _rotation = rotation;
        _scale = scale;
    }

    public void SetTransform(Transform transform, Space space = Space.Self)
    {
        if(space == Space.Self)
        {
            transform.localPosition = _position;
            transform.localRotation = Quaternion.Euler(_rotation);
        }
        else
        {
            transform.position = _position;
            transform.rotation = Quaternion.Euler(_rotation);
        }
        
        transform.localScale = _scale;
    }
}
