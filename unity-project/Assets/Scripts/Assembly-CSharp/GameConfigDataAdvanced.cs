using System;

[Serializable]
public class GameConfigDataAdvanced
{
	public bool toggleSlowZoomOnDressBreak;

	public bool toggleAutoZoomOnEroGrab;

	public bool toggleShowBlockedDamage;

	public bool toggleQuickSave;

	public bool toggleAutoSave;

	public bool toggleAutoEquipOnPickup;

	public bool toggleInternalShots;

	public bool toggleRememberLastZoom;

	public bool ToggleDisplayLinesMechanism;

	public bool ToggleAllVoreIsFatal;

	public bool ToggleGalleryHideScenes;

	public bool ToggleDisplayDialogueBadEnd;

	public bool ToggleBadEnd;

	public bool ToggleLock60fps;

	public bool ToggleToOldStruggleBar;

	public bool doAllowDebug;

	public bool disableCursorAnimUI;

	public bool disableVoiceLogo;

	public GameConfigDataAdvanced()
	{
		toggleSlowZoomOnDressBreak = true;
		toggleAutoZoomOnEroGrab = true;
		toggleShowBlockedDamage = false;
		toggleQuickSave = true;
		toggleAutoSave = false;
		toggleAutoEquipOnPickup = true;
		toggleInternalShots = true;
		toggleRememberLastZoom = false;
		ToggleDisplayLinesMechanism = false;
		ToggleAllVoreIsFatal = false;
		ToggleGalleryHideScenes = true;
		ToggleDisplayDialogueBadEnd = true;
		ToggleBadEnd = true;
		ToggleLock60fps = true;
		ToggleToOldStruggleBar = true;
		doAllowDebug = true;
		disableCursorAnimUI = false;
		disableVoiceLogo = false;
	}
}
