using System;
using UnityEngine;

[Serializable]
public class ENM_EroBaseConfig
{
	public bool IsAllowedToGrab;

	public float eroGracePeriod;

	public GameCore.FetishType[] FetishTags;

	public Vector2 offsetStruggleBarPos;

	public Vector2 offsetDamageDisplay;

	public Vector2 offsetPlayerHidden;

	public Vector2 offsetPlayerRelease;

	public Vector2 eroCollDimensions;

	public Vector2 eroCollOffset;

	public bool eroCollSwitchTrigger;

	public GameObject basicEroInternals;

	public bool layeredEroInternal;

	public bool eroIsFatal;

	public int minInternalsPhase;

	public int maxInternalsPhase;

	public GameCore.EroAnimType eroAnimType;

	public ENM_EroPhaseControl[] eroPhase;

	public ENM_EroSoloSceneVariations[] eroSoloAlts;

	public bool cantRepeatLastAlt;

	public bool NonRepeatDoSequential;

	public bool canChasePlayerAliveGrabbed;

	public bool forceDefaultFacing;

	public ENM_EroComboScenes[] eroComboScenes;
}
