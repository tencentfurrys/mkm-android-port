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
(Fixed in P1e below.)

## P1d - Complete language TextAssets (fixes the "kicked out at loading screen" crash)
Symptom: splash plays, 3 key presses, loading screen (`imgLoading`) shows, then
the app is killed during `LoadSceneAsync("MainMenu")`.

Root cause: the ripped `Assets/TextAsset/*.json` are TRIMMED copies of the game's
WebGL-embedded JSONs. `Assets/TextAsset/UserInterface.json` only carried 123 of
the 348 UI ids (it stops at id 99, jumps to 950+). On Android all JSON reads go
through the embedded TextAssets (P1), so `GetTranslatedStringUI(136)`
(`menuInGame.btnTextContinue`) hits a missing entry:
`listOfUserInterfaceTranslation.Find(...)` returns null -> `.text` NRE ->
unhandled exception in `MenuBehaviour.InitialSetup`/`ApplyLanguageOfMenu` ->
IL2CPP aborts the process mid-scene-load. (On PC this never happens: the build
reads the full files from StreamingAssets.)
ItemList (18 of 99 items), Dialogue_Various (12 of 143 ids) and
Dialogue_BoardSign (30 of 57) were trimmed the same way.
Fix: all `Assets/TextAsset/*.json` now hold the complete
`Assets/StreamingAssets/Languages/ENG/` versions (Farina was already done in
P1c). Counts now: UI 348, Items 99, Various 143, Board 57.

## P1f - Diagnostic logger (MKMDiag, temporary)
`Assets/Scripts/Assembly-CSharp/MKMDiag.cs`: `RuntimeInitializeOnLoadMethod`
(BeforeSceneLoad) logger, DontDestroyOnLoad. Records device info, scene
transitions, every Unity error/exception and explicit step markers to
`persistentDataPath/MKMDiag.log` plus an on-screen overlay (current step +
last error). All I/O try/catch-guarded; no gameplay behavior changed.
Step markers cover: Splash Start/Setup/feed/LoadUIFile, async scene load,
Menu Start/Setup per-call, AudioSystem clip resolution, TeleportScreenLogic
and ItemScrollManager entry. Remove for final release builds.

## P1g - OOM fix: lazy-load all audio (fixes LMK kill during MainMenu load)
Symptom: A16 (3.4GB RAM) died inside `LoadSceneAsync("MainMenu")` with no
managed exception - diag log healthy up to `calling LoadSceneAsync`, then
silence. Root cause: all 850 AudioClips were `DecompressOnLoad` +
`preloadAudioData`, and all are referenced by the shared `AudioClipsList`
ScriptableObject that MainMenu loads - ~94MB Vorbis expanded to ~1GB PCM
in one spike, LMK killed the app.
Fix: all 850 `Assets/AudioClip/*.meta` set to `loadType:
CompressedInMemory (1)`, `preloadAudioData: 0`, `loadInBackground: 1`.
Clips stay compressed and decode only when actually played (menu footprint
now title music + UI sounds only).

## P2a - IntroStory touch advance + wide-screen UI fit
- `IntroStory.Update` was keyboard-only (ConfirmUI/Return/Space) and the
  Next/Skip prompts are labels, not buttons - touch players stuck on page 1.
  Any tap/click now advances; two-finger tap (or right-click) skips;
  hints read Tap / 2-Finger Tap on Android.
- Ripped canvases are 1920x1080 Match-Width: on wider phones (2340x1080)
  everything scaled ~22% and top/bottom UI cropped off-screen.
  `Assets/Scripts/Assembly-CSharp/MKMCanvasFit.cs` runtime hook forces
  Expand mode on every ScaleWithScreenSize scaler at each scene load -
  full design space visible on any aspect, no scene-file edits.

## P1e - Nene dialogue wired + Find() crash hardening
- `SplashScreen.jsonFileDialogueNene` added (+ `Assets/TextAsset/Dialogue_Nene.json`
  and its import guid) and wired in `_InitialScreen.unity`; the feed call now
  passes the Nene asset into the Nene slot instead of Haruna's (old quirk).
- `GameCore.GetTranslatedStringUI/ITEM`, `ParseDialogueLineTags`, `LoadUIFile`,
  `LoadItemDatabaseFile`, `LoadIntroStoryFile`, `LoadGameConfiguration`
  (`File.Exists` guard) and `LoadYourAsyncScene` (new `MKM_IsSceneInBuild`
  build-settings check, name- or path-matching) are now null-safe.
- All 43 `list.Find(...).member` sites across GameCore/Controller_Player/
  MenuBehaviour/SplashScreen/MayaVoiceGroups use `?.member` so a missing entry
  degrades to an empty string instead of killing the app.
