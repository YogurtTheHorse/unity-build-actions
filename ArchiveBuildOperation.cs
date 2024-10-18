using SuperUnityBuild.BuildTool;
using System;
using System.IO;
using System.IO.Compression;
using UnityEngine;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace YogurtTheHorse.Unity.BuildActions
{
    public class ArchiveBuildOperation : BuildAction, IPreBuildAction, IPreBuildPerPlatformAction, IPostBuildAction,
        IPostBuildPerPlatformAction
    {
        public string inputPath = "$BUILDPATH";
        public string outputPath = "$BUILDPATH";
        public string outputFileName = "$PRODUCT_NAME-$RELEASE_TYPE-$VERSION.zip";

        public override void PerBuildExecute(BuildReleaseType releaseType, BuildPlatform platform,
            BuildArchitecture architecture, BuildScriptingBackend scriptingBackend, BuildDistribution distribution,
            DateTime buildTime, ref UnityEditor.BuildOptions options, string configKey, string buildPath)
        {
            var combinedOutputPath = Path.Combine(outputPath, outputFileName);
            var resolvedOutputPath = ResolvePerBuildExecuteTokens(combinedOutputPath, releaseType, platform,
                architecture, scriptingBackend, distribution, buildTime, buildPath);
            var resolvedInputPath = ResolvePerBuildExecuteTokens(inputPath, releaseType, platform, architecture,
                scriptingBackend, distribution, buildTime, buildPath);

            if (!resolvedOutputPath.EndsWith(".zip"))
                resolvedOutputPath += ".zip";

            PerformZip(Path.GetFullPath(resolvedInputPath), Path.GetFullPath(resolvedOutputPath));
        }

        private void PerformZip(string inputPath, string outputPath)
        {
            try
            {
                if (!Directory.Exists(inputPath))
                {
                    BuildNotificationList.instance.AddNotification(new BuildNotification(
                        BuildNotification.Category.Error,
                        "Zip Operation Failed.", $"Input path does not exist: {inputPath}",
                        true, null));
                    return;
                }

                var parentPath = Path.GetDirectoryName(outputPath);
                if (!Directory.Exists(parentPath) && !string.IsNullOrEmpty(parentPath))
                {
                    Directory.CreateDirectory(parentPath);
                }

                if (File.Exists(outputPath))
                    File.Delete(outputPath);

                using var zip = ZipFile.Open(outputPath, ZipArchiveMode.Create);
                AddFilesToZip(zip, inputPath, inputPath);
            }
            catch (Exception ex)
            {
                Debug.LogError(ex.ToString());
            }
        }

        private void AddFilesToZip(ZipArchive zip, string currentPath, string basePath)
        {
            var directories = Directory.GetDirectories(currentPath);
            foreach (var dir in directories)
            {
                if (Path.GetFileName(dir).ContainsInvariantCultureIgnoreCase("DoNotShip"))
                {
                    Debug.Log($"Excluding directory: {dir}");
                    continue;
                }

                AddFilesToZip(zip, dir, basePath);
            }

            var files = Directory.GetFiles(currentPath);
            foreach (var filePath in files)
            {
                var relativePath = Path.GetRelativePath(basePath, filePath);
                zip.CreateEntryFromFile(filePath, relativePath, CompressionLevel.Optimal);

                Debug.Log($"Added to ZIP: {relativePath}");
            }
        }
    }
}