using System.Collections.Generic;
using UnityEngine;

// Unity component responsible for constructing and rendering the scene based on loaded data
public class SceneBuilder : MonoBehaviour
{
    public Material baseMaterial; // Base material used as a template for object materials
    private SceneService sceneService = new SceneService(); // Service instance to load scene data
    private SceneData sceneData; // Scene data parsed from the configuration file

    void Start()
    {
        string filePath = Application.dataPath + "/Resources/Config/TestScene1.txt"; // Path to the configuration file
        sceneData = sceneService.LoadSceneData(filePath); // Load scene data from configuration
        BuildScene(); // Build and display the scene
    }

    // Build the scene based on the parsed data
    void BuildScene()
    {
        ApplyImageSettings();
        CreateTransformations();
        CreateMaterials();
        CreateCamera();
        CreateLights();
    }

    void ApplyImageSettings()
    {
        Camera.main.backgroundColor = sceneData.BackgroundColor;
        Debug.Log($"Scene resolution: {sceneData.ImageWidth}x{sceneData.ImageHeight}");
    }

    void CreateTransformations()
    {
        foreach (var transformation in sceneData.Transformations)
        {
            GameObject obj = GameObject.CreatePrimitive(PrimitiveType.Cube); // Placeholder for testing
            ApplyTransformations(obj, transformation);
        }
    }

    void CreateMaterials()
    {
        foreach (var material in sceneData.Materials)
        {
            Material newMaterial = new Material(baseMaterial)
            {
                color = material.color
            };
            newMaterial.SetFloat("_Shininess", material.shininess);
            newMaterial.SetFloat("_Metallic", material.metallic);
            Debug.Log($"Created material with color {material.color}");
        }
    }

    void CreateCamera()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null && sceneData.Camera != null)
        {
            mainCamera.fieldOfView = sceneData.Camera.FOV;
            Debug.Log($"Camera FOV set to {sceneData.Camera.FOV}");
        }
    }

    void CreateLights()
    {
        foreach (var lightData in sceneData.Lights)
        {
            GameObject lightObj = new GameObject("Light");
            Light light = lightObj.AddComponent<Light>();
            light.color = lightData.Color;
            light.intensity = lightData.Intensity;
            Debug.Log($"Created light with intensity {lightData.Intensity}");
        }
    }

    // Apply transformations to a given object
    void ApplyTransformations(GameObject obj, Transformation transformation)
    {
        obj.transform.Translate(transformation.translation, Space.World);
        obj.transform.Rotate(transformation.rotation);
        obj.transform.localScale = transformation.scale;
    }
}
