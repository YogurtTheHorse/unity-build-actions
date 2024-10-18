using System;
using System.IO;
using SuperUnityBuild.BuildTool;
using UnityEngine;

namespace YogurtTheHorse.Unity.BuildActions
{
    public class PrintBuildInfo : BuildAction, IPostBuildAction, IPostBuildPerPlatformAction
    {
        public override void PerBuildExecute(BuildReleaseType releaseType, BuildPlatform platform,
            BuildArchitecture architecture, BuildScriptingBackend scriptingBackend, BuildDistribution distribution,
            DateTime buildTime, ref UnityEditor.BuildOptions options, string configKey, string buildPath)
        {
            var root = Directory.GetParent(Application.dataPath)!.FullName;
            var relativeBuildPath = Path.GetRelativePath(root, buildPath);
            
            Debug.Log($"Finish build of {configKey}\n" +
                      $"Release type: {releaseType}\n" +
                      $"Platform: {platform}\n" +
                      $"Architecture: {architecture}\n" +
                      $"Scripting backend: {scriptingBackend}\n" +
                      $"Distribution: {distribution}\n" +
                      $"Build time: {buildTime}\n" +
                      $"Build path: {relativeBuildPath}");
        }
    }
}