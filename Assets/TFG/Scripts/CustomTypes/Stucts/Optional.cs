using System;
using UnityEngine;

[Serializable]
public struct Optional <T>
{

    [SerializeField] private bool enabled;
    [SerializeField] private T value;
    [SerializeField] private Vector3 position;

    public Optional (T initialValue, Vector3 initPosition)
    {
        enabled = true;
        value = initialValue;
        position = initPosition;
    }


    public bool Enabled => enabled;
    public T Value => value;
    public Vector3 Position => position;
}
