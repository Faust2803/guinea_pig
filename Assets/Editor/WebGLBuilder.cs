using UnityEditor;
using UnityEngine;

public class WebGLBuilder
{
    [MenuItem("Build/Build WebGL")]
    public static void BuildWebGL()
    {
        string buildPath = "Build/WebGL";
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
        {
            scenes = new[] 
            { 
                "Assets/Scenes/Boot.unity",
                "Assets/Scenes/Lobby.unity",
                "Assets/Scenes/Game.unity"
            }, // Add your scenes here
            locationPathName = buildPath,
            target = BuildTarget.WebGL,
            options = BuildOptions.None
        };

        PlayerSettings.stripEngineCode = false;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.WebGL, ManagedStrippingLevel.Minimal);
        // Set WebGL compression format
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;

        BuildPipeline.BuildPlayer(buildPlayerOptions);

        Debug.Log("WebGL build complete.");
    }
}
