// SceneService.cs
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SceneService
{
    public SceneData LoadSceneData(string filePath)
    {
        SceneData sceneData = new SceneData();

        if (!File.Exists(filePath))
        {
            Debug.LogError($"File not found: {filePath}");
            return sceneData;
        }

        string[] lines = File.ReadAllLines(filePath);
        string currentSection = string.Empty;

        for (int i = 0; i < lines.Length; i++)
        {
            string currentLine = lines[i].Trim();

            if (string.IsNullOrEmpty(currentLine) || currentLine.StartsWith("//") || currentLine.StartsWith("{") || currentLine.StartsWith("}"))
                continue;

            switch (currentLine)
            {
                case "Image":
                    i = ParseImage(lines, i, sceneData);
                    break;
                case "Transformation":
                    i = ParseTransformation(lines,i, sceneData);
                    break;
                case "Material":
                    i = ParseMaterial(lines,i, sceneData);
                    break;
                case "Camera":
                    i = ParseCamera(lines,i, sceneData);
                    break;
                case "Light":
                    i = ParseLight(lines,i, sceneData);
                    break;
            }
        }

        return sceneData;
    }

    private int ParseImage(string[] lines, int currentIndex, SceneData sceneData)
    {
        for (int i = currentIndex + 1; i < lines.Length; i++)
        {
            string currentLine = lines[i].Trim();

            if (currentLine == "}")
                return i;

            string[] parts = currentLine.Split(' ');

            if (parts.Length == 2)
            {
                sceneData.ImageWidth = int.Parse(parts[0]);
                sceneData.ImageHeight = int.Parse(parts[1]);
                Debug.Log($"Scene Width: {sceneData.ImageWidth}x Scene Heigth:{sceneData.ImageHeight}");
            }
            else if (parts.Length == 3)
            {
                sceneData.BackgroundColor = new Color(
                    float.Parse(parts[0]),
                    float.Parse(parts[1]),
                    float.Parse(parts[2])
                );
                Debug.Log($"BackgroundColor: {sceneData.BackgroundColor}");
            }
        }

        return currentIndex;
    }

    private int ParseTransformation(string[] lines, int currentIndex, SceneData sceneData)
    {
        for (int i = currentIndex + 1; i < lines.Length; i++)
        {
            string trimmedLine = lines[i].Trim();

            if (trimmedLine == "}")
                return i;

            Transformation transformation = new();
            string[] parts = trimmedLine.Split(' ');
            foreach (var part in parts)
            {
                if (part.StartsWith("T"))
                {
                    transformation.translation = ParseVector3(part.Substring(1));
                    Debug.Log($"Translation: {transformation.translation}");
                }
                else if (part.StartsWith("Rx"))
                {
                    var rotation = transformation.rotation;
                    rotation.x = float.Parse(parts[1]);
                    transformation.rotation = rotation;
                    Debug.Log($"Rotation: {transformation.rotation}");
                }
                else if (part.StartsWith("Ry"))
                {
                    var rotation = transformation.rotation;
                    rotation.y = float.Parse(parts[1]);
                    transformation.rotation = rotation;
                    Debug.Log($"Rotation: {transformation.rotation}");
                }
                else if (part.StartsWith("Rz"))
                {
                    var rotation = transformation.rotation;
                    rotation.z = float.Parse(parts[1]);
                    transformation.rotation = rotation;
                    Debug.Log($"Rotation: {transformation.rotation}");
                }
                else if (part.StartsWith("S"))
                {
                    transformation.scale = ParseVector3(part.Substring(1));
                    Debug.Log($"Scale: {transformation.scale}");
                }
            }
            sceneData.Transformations.Add(transformation);
        }
        return currentIndex;
    }

    private int ParseMaterial(string[] lines, int currentIndex, SceneData sceneData)
    {
        MaterialProperties material = new();
        for (int i = currentIndex + 1; i < lines.Length; i++)
        {
            string trimmedLine = lines[i].Trim();

            if (trimmedLine == "}")
                return i;

            string[] parts = trimmedLine.Split(' ');
            if (parts.Length == 3)
            {
                material.color = new Color(float.Parse(parts[0]), float.Parse(parts[1]), float.Parse(parts[2]));
                Debug.Log($"Material Color: {material.color}");
            }
            else if (parts.Length == 2)
            {
                material.shininess = float.Parse(parts[0]);
                material.metallic = float.Parse(parts[1]);
                Debug.Log($"Shininess: {material.shininess} X Methalic: {material.metallic}");
            }

            sceneData.Materials.Add(material);
        }

        return currentIndex;
    }

    private int ParseCamera(string[] lines, int currentIndex, SceneData sceneData)
    {
        for (int i = currentIndex + 1; i < lines.Length; i++)
        {
            string trimmedLine = lines[i].Trim();

            if (trimmedLine == "}")
                return i;

            string[] parts = trimmedLine.Split(' ');
            if (parts.Length > 1) { 
                sceneData.Camera = new CameraData
                {
                    FocalLength = float.Parse(parts[0]),
                    FOV = float.Parse(parts[1]),
                    AspectRatio = float.Parse(parts[2])
                };
                Debug.Log($"Camera: {sceneData.Camera}");
            }
            
        }

        return currentIndex;
    }

    private int ParseLight(string[] lines, int currentIndex, SceneData sceneData)
    {
        for (int i = currentIndex + 1; i < lines.Length; i++)
        {
            string trimmedLine = lines[i].Trim();

            if (trimmedLine == "}")
                return i;

            string[] parts = trimmedLine.Split(' ');
            if (parts.Length > 1)
            {
                sceneData.Lights.Add(new LightData
                {
                    Intensity = float.Parse(parts[0]),
                    Color = new Color(float.Parse(parts[1]), float.Parse(parts[2]), float.Parse(parts[3]))
                });
            }
                
            Debug.Log($"Camera: {sceneData.Lights}");
        }

        return currentIndex;
    }

    private Vector3 ParseVector3(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            Debug.LogError("ParseVector3 received an empty or null string.");
            return Vector3.zero; // Retorna um vetor padrão (0, 0, 0)
        }

        string[] values = input.Split(',');
        if (values.Length != 3)
        {
            Debug.LogWarning($"ParseVector3 expected 3 values but got {values.Length}: {input}");
            return Vector3.zero;
        }

        try
        {
            float x = float.Parse(values[0]);
            float y = float.Parse(values[1]);
            float z = float.Parse(values[2]);
            return new Vector3(x, y, z);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error parsing Vector3 from input '{input}': {ex.Message}");
            return Vector3.zero; // Retorna um vetor padrão em caso de erro
        }
    }
}

// Supporting classes

public class CameraData
{
    public float FocalLength { get; set; }
    public float FOV { get; set; }
    public float AspectRatio { get; set; }
}

public class LightData
{
    public float Intensity { get; set; }
    public Color Color { get; set; }
}
