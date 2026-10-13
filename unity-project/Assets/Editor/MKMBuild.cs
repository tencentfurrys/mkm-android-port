// MKM Android build driver (Unity 2019.2.6f1). Drop into Assets/Editor/ of the
// ripped project, then run headless:
//   Unity.exe -batchmode -quit -projectPath <ripped> -executeMethod MKMBuild.BuildAndroid -logFile build.log
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class MKMBuild
{
    public static void BuildAndroid()
    {
        // NOTE: SDK/NDK/JDK paths come from env (ANDROID_HOME / ANDROID_NDK_ROOT,
        // JAVA_HOME) since 2019.2 has no external-tools path API.
        Debug.Log("env SDK=" + System.Environment.GetEnvironmentVariable("ANDROID_HOME"));            // --- Player settings ---
        PlayerSettings.companyName = "Mega Blue Ball";
        PlayerSettings.productName = "Mystic Knight Maya";
        PlayerSettings.applicationIdentifier = "com.megablueball.mkm";
        // 2019.2: the plain setter follows the selected build target, so set
        // the Android identifier explicitly (a batch session defaulting to
        // Standalone silently shipped the ripper default package id once).
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.megablueball.mkm");
        PlayerSettings.bundleVersion = "0.7.5";
        PlayerSettings.Android.bundleVersionCode = 3;
        PlayerSettings.Android.minSdkVersion = AndroidSdkVersions.AndroidApiLevel19;
        PlayerSettings.Android.targetSdkVersion = AndroidSdkVersions.AndroidApiLevel28;
        PlayerSettings.Android.preferredInstallLocation = AndroidPreferredInstallLocation.Auto;
        PlayerSettings.defaultInterfaceOrientation = UIOrientation.LandscapeLeft;
        PlayerSettings.allowedAutorotateToLandscapeLeft = true;
        PlayerSettings.allowedAutorotateToLandscapeRight = true;
        PlayerSettings.allowedAutorotateToPortrait = false;
        PlayerSettings.allowedAutorotateToPortraitUpsideDown = false;
        // GLES3 only: max compat (old Mali/Adreno on the J3), no Vulkan (2019.2-era risk).
        PlayerSettings.SetGraphicsAPIs(BuildTarget.Android, new[] { UnityEngine.Rendering.GraphicsDeviceType.OpenGLES3 });
        // Dual ABI, one FAT APK for the 32-bit J3 and the 64-bit A16.
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        // Keep stripping OFF until the decompiled code is proven: JsonUtility + reflection heavy.
        PlayerSettings.stripEngineCode = false;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Disabled);
        // Keep the Unity splash simple; Personal license shows its own splash regardless.
        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.statusBarHidden = true;

        // --- Scenes: level0 first (boot), then everything else by name ---
        string[] ordered = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
        Debug.Log("MKM scenes:\n" + string.Join("\n", ordered));

        string outApk = Path.GetFullPath(Path.Combine(Application.dataPath, "..", "MKM-0.7.5-android.apk"));
        BuildPlayerOptions opt = new BuildPlayerOptions
        {
            scenes = ordered.ToArray(),
            locationPathName = outApk,
            target = BuildTarget.Android,
            options = BuildOptions.None,
        };
        var report = BuildPipeline.BuildPlayer(opt);
        Debug.Log("MKM build result: " + report.summary.result + " -> " + outApk);
        if (report.summary.result != UnityEditor.Build.Reporting.BuildResult.Succeeded)
            EditorApplication.Exit(1);
    }
}
