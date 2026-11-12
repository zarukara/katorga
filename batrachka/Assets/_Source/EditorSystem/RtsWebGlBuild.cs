using System.IO;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace EditorSystem
{
    public static class RtsWebGlBuild
    {
        [MenuItem("ECS Practice/Configure WebGL")]
        public static void Configure()
        {
            PlayerSettings.companyName = "ECS Practice";
            PlayerSettings.productName = "ECS Practice";
            PlayerSettings.defaultWebScreenWidth = 1600;
            PlayerSettings.defaultWebScreenHeight = 900;
            PlayerSettings.runInBackground = false;
            PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Gzip;
            PlayerSettings.WebGL.decompressionFallback = true;
            PlayerSettings.WebGL.dataCaching = true;
            PlayerSettings.WebGL.template = "APPLICATION:Default";
            PlayerSettings.WebGL.exceptionSupport = WebGLExceptionSupport.ExplicitlyThrownExceptionsOnly;
            PlayerSettings.SetScriptingBackend(BuildTargetGroup.WebGL, ScriptingImplementation.IL2CPP);
            PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.WebGL, ManagedStrippingLevel.Low);
            AssetDatabase.SaveAssets();
        }

        [MenuItem("ECS Practice/Build WebGL for GitHub Pages")]
        public static void Build()
        {
            Configure();
            if (!File.Exists(RtsSceneBuilder.ScenePath)) RtsSceneBuilder.Rebuild();
            const string output = "Builds/WebGL";
            var report = BuildPipeline.BuildPlayer(new BuildPlayerOptions
            {
                scenes = new[] { RtsSceneBuilder.ScenePath }, locationPathName = output,
                target = BuildTarget.WebGL, options = BuildOptions.None
            });
            if (report.summary.result != BuildResult.Succeeded)
                throw new BuildFailedException("WebGL: " + report.summary.result);
            Debug.Log($"RTS_WEBGL_READY: {Path.GetFullPath(output)} ({report.summary.totalSize} bytes)");
        }
    }

    public sealed class GitHubPagesPostprocess : IPostprocessBuildWithReport
    {
        public int callbackOrder => 100;
        public void OnPostprocessBuild(BuildReport report)
        {
            if (report.summary.platform != BuildTarget.WebGL) return;
            string output = report.summary.outputPath;
            File.WriteAllText(Path.Combine(output, ".nojekyll"), string.Empty);
            string index = Path.Combine(output, "index.html");
            string html = File.ReadAllText(index);
            const string additions = "<style>html,body{margin:0;background:#777;width:100%;height:100%;overflow:hidden}#unity-container.unity-desktop,#unity-container.unity-mobile{position:absolute;left:0;top:0;transform:none;width:100%;height:100%}#unity-canvas{width:100%!important;height:100%!important;display:block}#unity-footer{display:none}</style><script>document.addEventListener('contextmenu',function(e){if(e.target.tagName==='CANVAS')e.preventDefault();});</script>";
            File.WriteAllText(index, html.Replace("</body>", additions + "</body>"));
            File.Copy("Packages/ru.leopotam.ecsproto/LICENSE.md", Path.Combine(output, "LeoECS-Proto-LICENSE.md"), true);
        }
    }
}
