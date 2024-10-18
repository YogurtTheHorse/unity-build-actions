using System.IO;
using SuperUnityBuild.BuildTool;

namespace YogurtTheHorse.Unity.BuildActions
{
    public class WriteBuildNumber : BuildAction, IPostBuildAction
    {
        public string buildNumberFilePath = "build_number.txt";

        public override void Execute()
        {
            var buildNumber = BuildSettings.productParameters.buildVersion;
            var fullPath = Path.Join(BuildSettings.basicSettings.baseBuildFolder, buildNumberFilePath);

            File.WriteAllText(fullPath, buildNumber);
        }
    }
}