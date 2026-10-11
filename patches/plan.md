# MKM Android port — patch set (apply to the ripped project's Assets scripts)

## P0 — Save/config path: `UserData` -> persistentDataPath (REQUIRED to boot sanely)
`GameCore.cs` uses relative `Path.Combine("UserData", ...)` in ~12 places
(5229, 5258, 5260, 5380, 5392, 5548, 5563, 5565, 5567, 5604, 5606, 9721).
On Android the working directory is not writable -> add:

```csharp
private static string SaveDir() { return System.IO.Path.Combine(Application.persistentDataPath, "UserData"); }
```

and rewrite every `"UserData"` literal / `Path.Combine("UserData", x)` to use
`SaveDir()`. Mechanical, GameCore.cs only. Screenshot save (9721) rides along.

## P1 — Language JSON on Android: use embedded TextAssets (REQUIRED, else black screen / crash)
`File.ReadAllText(streamingAssetsPath...)` fails on Android (APK = jar URL).
The game already embeds every JSON as TextAssets for its WebGL build
(`WEBGL_FeedLocalVariablesToString`, used at 4453-4478, 6340-6398, 7728-7730).
One-line patch:

```csharp
private bool WEBGL_CheckIfIsWebBuild()
{
    if (_gameBuild == GameBuildType.WebGL || Application.platform == RuntimePlatform.Android)
        return true;
    return false;
}
```

Requires the ripped scenes to keep the TextAsset refs on the bootstrap object
that calls `WEBGL_FeedLocalVariablesToString` — verify post-rip.

## P2 — Touch input (REQUIRED for playability, do after first bootable APK)
All gameplay input funnels through `GameCore.UnifiedInputHandler(InputLib, InputPressType)`
(plus ~34 direct `Input.GetKey/GetButton` call sites, mostly UI/cancel keys).
Plan:
1. New `TouchInput.cs`: static sets `held/down/up` per `InputLib`, fed by
   on-screen buttons (uGUI + EventTrigger PointerDown/Up), cleared each frame.
2. `UnifiedInputHandler`: OR virtual state in first (all three press types).
3. Map the ~34 direct `Input.*` calls to `UnifiedInputHandler` equivalents or
   virtual buttons (Pause, CancelUI, ConfirmUI at minimum).
4. Add touch button canvas to the boot scene; hide on non-Android
   (`#if UNITY_ANDROID` / platform check).
5. `Screen.sleepTimeout = SleepTimeout.NeverSleep` while playing.
Fallback: USB/BT gamepad already works via `UnifiedInputHandlerGamepadDirectional`.

## Build config (see build/BuildAndroid.cs)
IL2CPP + ARMv7|ARM64 FAT APK, minSdk 19 (J3 = API 28 OK), targetSdk 28,
GLES3 only, stripping Disabled, landscape.
First build: P0+P1 only (boots to menu, touch comes next).

## P1c - Splash text fix (boot NRE from null Farina ref)
Ripped `_InitialScreen.unity` had `jsonFileDialogueFarina: {fileID: 0}` (no
Farina TextAsset was ripped). `WEBGL_FeedLocalVariablesToString` threw NRE on
`jsonDiagFarina.text`, aborting `InitialScreenSetup()` before
`LoadUIFile()`/`ApplyLanguageOfWarnings()` - splash kept default key texts
(`game_start:splash_screen`, `press_any_key`, `alpha_disclaimer_here`).
Fix:
- `Assets/TextAsset/Dialogue_Farina.json` (+ `.meta`) copied from
  `Assets/StreamingAssets/Languages/ENG/`; scene ref wired (use the guid Unity
  assigns on import, not a hand-made one - else `Fatal Error! guid is
  uninitialized` at build).
- `GameCore.WEBGL_FeedLocalVariablesToString`: null-guards on all 7 slots.
Note: feed call passes Haruna asset into the Nene slot (pre-existing mapping
quirk) - Nene NPC shows Haruna lines until a Nene TextAsset is wired.
