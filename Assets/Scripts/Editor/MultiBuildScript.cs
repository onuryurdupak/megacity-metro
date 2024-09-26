using UnityEngine;
using UnityEditor;
using System.IO;
using System.Diagnostics;

public class MultiBuildScript : MonoBehaviour
{
    private const string buildPath = "Builds/MyGame";
    private const string logFilePath = "Builds/build_times_log.txt";
    private const int numBuilds = 10;

    [MenuItem("Build/Build Game Multiple Times")]
    public static void BuildMultipleTimes()
    {
        if (!Directory.Exists("Builds"))
        {
            Directory.CreateDirectory("Builds");
        }

        var scenesInBuild = EditorBuildSettings.scenes;
        if (scenesInBuild.Length == 0)
        {
            UnityEngine.Debug.LogError("No scenes found in Build Settings. Please add scenes before building.");
            return;
        }

        string[] scenePaths = new string[scenesInBuild.Length];
        for (int i = 0; i < scenesInBuild.Length; i++)
        {
            scenePaths[i] = scenesInBuild[i].path;
        }

        using (StreamWriter writer = new StreamWriter(logFilePath, false))
        {
            writer.WriteLine("Build Times Log");
            writer.WriteLine("================");

            for (int i = 1; i <= numBuilds; i++)
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions
                {
                    scenes = scenePaths,
                    locationPathName = $"{buildPath}_{i}",
                    target = EditorUserBuildSettings.activeBuildTarget,
                    options = BuildOptions.None
                };

                BuildPipeline.BuildPlayer(buildPlayerOptions);

                stopwatch.Stop();

                writer.WriteLine($"Build {i}: {stopwatch.Elapsed.TotalSeconds} seconds");
                UnityEngine.Debug.Log($"Build {i} completed in {stopwatch.Elapsed.TotalSeconds} seconds");
            }
        }

        UnityEngine.Debug.Log($"All {numBuilds} builds completed. Log file written to: {logFilePath}");
    }
}
