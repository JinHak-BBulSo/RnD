using UnityEngine;
using UnityEditor;
using UnityEditor.Build.Reporting;
using System.Collections;
using System.Collections.Generic;

public class TestBuilder
{
    [MenuItem("Build/JenkinsTest")]
    public static void BuildTest()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        //List<string> scenes = new List<string>();
        //foreach(var scene in EditorBuildSettings.scenes)
        //{
        //    if(!scene.enabled) continue;
        //    scenes.Add(scene.path);
        //}


        buildPlayerOptions.scenes = new[] { "Assets/Scenes/Main.unity" };
        buildPlayerOptions.locationPathName = "Build/Test_Window.exe";
        Debug.Log(buildPlayerOptions.locationPathName);
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;

        BuildReport report = BuildPipeline.BuildPlayer(buildPlayerOptions);
        BuildSummary summary = report.summary;

        if(summary.result == BuildResult.Succeeded)
        {
            Debug.Log("Build succeeded");
            System.Console.WriteLine("Build succeeded");
        }

        if(summary.result == BuildResult.Failed)
        {
            Debug.Log("Build fail");
            System.Console.WriteLine("Build fail");
        }
    }
}
