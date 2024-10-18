using System;
using System.IO;
using System.Linq;
using SuperUnityBuild.BuildTool;
using UnityEditor;

namespace YogurtTheHorse.Unity.BuildActions
{
    public class WebGLBuildPathFix: BuildAction, IPostBuildPerPlatformAction
    {
        public override void PerBuildExecute(BuildReleaseType releaseType, BuildPlatform platform, BuildArchitecture architecture,
            BuildScriptingBackend scriptingBackend, BuildDistribution distribution, DateTime buildTime,
            ref BuildOptions options, string configKey, string buildPath)
        {
            if (!platform.platformName.Contains("WebGL")) return;
            
            var wrongDirectory = Path.Join(buildPath, releaseType.appBuildName);

            foreach (var file in Directory.EnumerateFiles(wrongDirectory))
            {
                var dest = Path.Combine(buildPath, Path.GetFileName(file));
                File.Move(file, dest);
            }

            foreach (var dir in Directory.EnumerateDirectories(wrongDirectory))
            {
                var dest = Path.Combine(buildPath, Path.GetFileName(dir));
                Directory.Move(dir, dest);
            }            

            if (!Directory.EnumerateDirectories(wrongDirectory).Any())
            {
                Directory.Delete(wrongDirectory);
            }
        }
    }
}