using System.Collections.Generic;
using UnityEngine;

public class SceneData
{
    public int ImageWidth { get; set; }
    public int ImageHeight { get; set; }
    public Color BackgroundColor { get; set; }
    public List<Transformation> Transformations { get; set; } = new List<Transformation>();
    public List<MaterialProperties> Materials { get; set; } = new List<MaterialProperties>();
    public MaterialProperties CurrentMaterial { get; set; }
    public CameraData Camera { get; set; }
    public List<LightData> Lights { get; set; } = new List<LightData>();
}