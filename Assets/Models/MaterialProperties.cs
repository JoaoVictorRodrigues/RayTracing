using UnityEngine;
// Class to define the properties of a material, such as color, shininess, and metallic factor
[System.Serializable]
public class MaterialProperties
{
    public Color color { get; set; } = Color.white;
    public float shininess { get; set; }
    public float metallic { get; set; }
}