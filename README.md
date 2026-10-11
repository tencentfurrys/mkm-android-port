# Mystic Knight Maya 0.7.5 — PC-to-Android port

Private reverse-engineering backup + build tree for porting the Windows demo
(`mystic-knight-maya-0.7.5-demo.zip`, Unity **2019.2.6f1**, Mono) to Android.

## Layout

- `unity-project/` — full Unity 2019.2.6f1 project, ripped from the PC build with
  AssetRipper 2.0, scripts restored (Hybrid decompile), Android port patches applied.
  Open with Unity **2019.2.6f1 + Android Build Support** (exact version required).
- `src/` — reference decompile of `Assembly-CSharp.dll` (ILSpy), pre-patch.
- `build/`
  - `MKMBuild.cs` — headless build driver (also at
    `unity-project/Assets/Editor/MKMBuild.cs`). IL2CPP, ARMv7+ARM64 FAT APK,
    minSdk 19, targetSdk 28, GLES3, stripping disabled, landscape.
  - `apply_patches.py` — the Android port patches (P0 save path, P1 JSON loading).
- `patches/plan.md` — port patch set incl. pending P2 touch controls.

## Android port patches (already applied to `unity-project/`)

- **P0** `GameCore.cs`: `UserData` relative paths → `Application.persistentDataPath`
  (helper `MKMSaveDir()`; 13 sites).
- **P1** JSON loading on Android uses the embedded TextAssets (WebGL code path):
  `GameCore.WEBGL_CheckIfIsWebBuild()` + `SplashScreen` feed condition check
  `Application.platform == RuntimePlatform.Android`.
- **P1c** splash text fix: Farina TextAsset wired + null-guards in
  `WEBGL_FeedLocalVariablesToString` (all 7 slots).
- **P1d** loading-screen crash fix: the ripped `Assets/TextAsset/*.json` were
  trimmed WebGL copies (UserInterface.json had only 123 of 348 UI ids, ItemList
  18 of 99) → `Find(...).text` NRE on id 136 during MainMenu load killed the app
  on the loading screen. All TextAssets now hold the complete
  `StreamingAssets/Languages/ENG/` data.
- **P1e** Nene dialogue TextAsset wired into the Nene feed slot (was Haruna),
  and all 43 `Find(...).member` JSON lookups are null-safe (`?.`), plus guards in
  `LoadUIFile`/`LoadItemDatabaseFile`/`LoadIntroStoryFile`/`LoadGameConfiguration`
  and a build-settings check in `LoadYourAsyncScene` — a missing entry now
  degrades to empty text instead of crashing the app.
- **P2 (TODO)** touch controls injected at `GameCore.UnifiedInputHandler`.

## Toolchain (all installed on the build machine, D: drive)

- Unity 2019.2.6f1 (`fe82a0e88406`) + Android module, Personal license
- Android SDK (platforms 28/29, build-tools 28.0.3, legacy `tools/`),
  **NDK r16b** (`16.1.4479499` — hard requirement of 2019.2.6f1),
  bundled OpenJDK 8
- Env for headless builds: `ANDROID_HOME`/`ANDROID_SDK_ROOT`=sdk,
  `ANDROID_NDK_ROOT`/`ANDROID_NDK_HOME`=ndk-r16b, `JAVA_HOME`=bundled OpenJDK.

## Build

```bat
set ANDROID_HOME=D:\Android\sdk
set ANDROID_SDK_ROOT=D:\Android\sdk
set ANDROID_NDK_ROOT=D:\Android\ndk\android-ndk-r16b
set ANDROID_NDK_HOME=D:\Android\ndk\android-ndk-r16b
set JAVA_HOME=D:\Unity\2019.2.6f1\Editor\Data\PlaybackEngines\AndroidPlayer\Tools\OpenJDK\Windows
Unity.exe -batchmode -quit -projectPath <repo>\unity-project -executeMethod MKMBuild.BuildAndroid
```

Output: `unity-project/MKM-0.7.5-android.apk` (debug-signed; both ABIs).
Release APKs are attached to GitHub Releases, not stored in the repo.

## Notes

- Package id of the first APK: `com.MegaBlueBall.MysticKnightMaya` (Ripper default;
  the `SetApplicationIdentifier` fix is staged for the next build script revision).
- First import of `unity-project/` takes ~40 min (1106 textures, 850 clips, 33 scenes).
- Game source is the property of its developers; this repo is private backup for
  interoperability/porting work only.
