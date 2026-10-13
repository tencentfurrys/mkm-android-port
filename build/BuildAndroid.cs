// MKM Android build driver (Unity 2019.2.6f1). Drop into Assets/Editor/ of the
// ripped project, then run headless:
//   Unity.exe -batchmode -quit -projectPath <ripped> -executeMethod MKMBuild.BuildAndroid64 -logFile build.log
// Split APKs (P2m+): BuildAndroid32 (ARMv7 32-bit, optimized) and BuildAndroid64 (ARM64).
// versionCode scheme: build number * 10 + ABI (1 = ARMv7, 2 = ARM64).
// P2m = build 5 -> 51 / 52. FAT line (BuildAndroid) frozen at 4.
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public static class MKMBuild
{
    // 32-bit, optimized for low-end (J3): ARMv7 only + Master (LTO) IL2CPP.
    // Same managed code + patches as 64-bit; stripping stays OFF (reflection-heavy
    // decompiled code, per handoff rule) so this is apples-to-apples for patch parity.
    public static void BuildAndroid32()
    {
        ApplyCommon(51);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7;
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Master);
        Build("MKM-0.7.5-android-armv7.apk");
    }

    // 64-bit (A16): ARM64 only, Release IL2CPP.
    public static void BuildAndroid64()
    {
        ApplyCommon(52);
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARM64;
        PlayerSettings.SetIl2CppCompilerConfiguration(BuildTargetGroup.Android, Il2CppCompilerConfiguration.Release);
        Build("MKM-0.7.5-android-arm64.apk");
    }

    // Legacy FAT (P2l and earlier). Frozen, kept for reference.
    public static void BuildAndroid()
    {
        ApplyCommon(4);
        // Dual ABI, one FAT APK for the 32-bit J3 and the 64-bit A16.
        PlayerSettings.Android.targetArchitectures = AndroidArchitecture.ARMv7 | AndroidArchitecture.ARM64;
        Build("MKM-0.7.5-android.apk");
    }

    private static void ApplyCommon(int versionCode)
    {
        // NOTE: SDK/NDK/JDK paths come from env (ANDROID_HOME / ANDROID_NDK_ROOT,
        // JAVA_HOME) since 2019.2 has no external-tools path API.
        Debug.Log("env SDK=" + System.Environment.GetEnvironmentVariable("ANDROID_HOME"));
        // --- Player settings ---
        PlayerSettings.companyName = "Mega Blue Ball";
        PlayerSettings.productName = "Mystic Knight Maya";
        PlayerSettings.applicationIdentifier = "com.megablueball.mkm";
        // 2019.2: the plain setter follows the selected build target, so set
        // the Android identifier explicitly (a batch session defaulting to
        // Standalone silently shipped the ripper default package id once).
        PlayerSettings.SetApplicationIdentifier(BuildTargetGroup.Android, "com.megablueball.mkm");
        PlayerSettings.bundleVersion = "0.7.5";
        PlayerSettings.Android.bundleVersionCode = versionCode;
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
        PlayerSettings.SetScriptingBackend(BuildTargetGroup.Android, ScriptingImplementation.IL2CPP);
        // Keep stripping OFF until the decompiled code is proven: JsonUtility + reflection heavy.
        PlayerSettings.stripEngineCode = false;
        PlayerSettings.SetManagedStrippingLevel(BuildTargetGroup.Android, ManagedStrippingLevel.Disabled);
        // Keep the Unity splash simple; Personal license shows its own splash regardless.
        PlayerSettings.SplashScreen.show = false;
        PlayerSettings.statusBarHidden = true;
    }

    private static void Build(string fileName)
    {
        // --- Scenes: level0 first (boot), then everything else by name ---
        string[] ordered = EditorBuildSettings.scenes.Select(s => s.path).ToArray();
        Debug.Log("MKM scenes:\n" + string.Join("\n", ordered));

        string outApk = Path.GetFullPath(Path.Combine(Application.dataPath, "..", fileName));
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
