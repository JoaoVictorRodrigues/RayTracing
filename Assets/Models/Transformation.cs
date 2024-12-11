using UnityEngine;
// Class to represent transformations for an object, including position, rotation, and scale
[System.Serializable]
public class Transformation
{
    public Vector3 translation { get; set; } = Vector3.zero;
    public Vector3 rotation { get; set; } = Vector3.zero;
    public Vector3 scale { get; set; } = Vector3.one;
}