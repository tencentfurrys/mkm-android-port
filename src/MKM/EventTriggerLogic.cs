using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTriggerLogic : MonoBehaviour
{
	public enum EventList
	{
		Null,
		ForestBoss_FirstMatch,
		BeachBoss_FirstMatch,
		HillBoss_FirstMatch,
		ForestBoss_Rematch,
		BeachBoss_Rematch,
		HillBoss_Rematch,
		WerewolfBoss_First,
		Checkpoint,
		WerewolfBoss_Rematch,
		MinotaurBoss_First,
		MinotaurBoss_Rematch,
		CastleOpenSilverKey,
		CastleOpenDungeonKey,
		PrincessBoss_First,
		PrincessBoss_Rematch,
		WitchBoss_First,
		WitchBoss_Rematch,
		OpenFinalGateway,
		BlueFox_First,
		BlueFox_Rematch,
		GalleryPortals,
		SwampBoss_First,
		SwampBoss_Rematch,
		Beach01,
		Farina01,
		Farina02,
		Gauntlet1,
		Farina03,
		OpenGenericSideGates,
		Forest01
	}

	public EventList thisEvent;

	public GameObject[] Waypoints;

	public GameObject[] MiscObjects;

	public bool needToPressInteract;

	public bool runSelfAtStart;

	public GameObject objIconToDisplay;

	public GameObject objEventMarker;

	public string strGeneric;

	private GameCore.MusicTheme defaultBossTheme = GameCore.MusicTheme.BossFight1;

	private GameCore.MusicTheme heroicBossTheme = GameCore.MusicTheme.BossFight2;

	private bool thisEventHasBeenTriggered;

	private bool dummyBool;

	private float valueGenericFloat;

	private float playerEventDefaultMoveSpeed;

	private Vector2 vecPositionGeneric;

	private SpriteRenderer objRE;

	private BoxCollider2D objBox2D;

	private Coroutine mainEventCoroutine;

	private Coroutine subEventCoroutine;

	private GameObject refMarkerClapper;

	private GameUI refCoreUI;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		objRE = GetComponent<SpriteRenderer>();
		objBox2D = GetComponent<BoxCollider2D>();
		playerEventDefaultMoveSpeed = 5f;
		objRE.enabled = false;
		if (strGeneric == "ChkPtMsg")
		{
			objRE.enabled = true;
		}
		thisEventHasBeenTriggered = false;
		mainEventCoroutine = null;
		subEventCoroutine = null;
		if (!GC)
		{
			DisplayMessageNullEvent();
			DeactivateEvent();
			return;
		}
		if (!CheckIfThisEventAreaCanStayActive())
		{
			DeactivateEvent();
			return;
		}
		if ((bool)objEventMarker)
		{
			refMarkerClapper = Object.Instantiate(objEventMarker, transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
		}
		if (runSelfAtStart)
		{
			StartEventMainLogic();
		}
	}

	private bool CheckIfThisEventAreaCanStayActive()
	{
		switch (thisEvent)
		{
		case EventList.Null:
			return false;
		case EventList.ForestBoss_FirstMatch:
			if (GC.flagsMain.isForestBossDefeatd)
			{
				return false;
			}
			break;
		case EventList.BeachBoss_FirstMatch:
			if (GC.flagsMain.isBeachBossDefeated)
			{
				return false;
			}
			break;
		case EventList.HillBoss_FirstMatch:
			if (GC.flagsMain.isHillBossDefeated)
			{
				return false;
			}
			break;
		case EventList.WerewolfBoss_First:
			if (GC.flagsMain.isWerewolfBossDefeated)
			{
				return false;
			}
			break;
		case EventList.MinotaurBoss_First:
			if (GC.flagsMain.isMinotaurBossDefeated)
			{
				return false;
			}
			break;
		case EventList.PrincessBoss_First:
			if (GC.flagsMain.isPrincessBossDefeated)
			{
				return false;
			}
			break;
		case EventList.WitchBoss_First:
			if (GC.flagsMain.isWitchBossDefeated)
			{
				return false;
			}
			break;
		case EventList.BlueFox_First:
			if (GC.flagsMain.isBlueFoxDefeated)
			{
				return false;
			}
			break;
		case EventList.SwampBoss_First:
			if (GC.flagsMain.isBossSwampDefeated)
			{
				return false;
			}
			break;
		case EventList.ForestBoss_Rematch:
			if (!GC.flagsMain.isForestBossDefeatd || !GC.skillFlags.hasDash)
			{
				return false;
			}
			break;
		case EventList.BeachBoss_Rematch:
			if (!GC.flagsMain.isBeachBossDefeated || !GC.CheckPlayerHasItem(100))
			{
				return false;
			}
			break;
		case EventList.HillBoss_Rematch:
			if (!GC.flagsMain.isHillBossDefeated || !GC.skillFlags.hasSlidingTackle)
			{
				return false;
			}
			break;
		case EventList.WerewolfBoss_Rematch:
			if (!GC.flagsMain.isWerewolfBossDefeated)
			{
				return false;
			}
			break;
		case EventList.MinotaurBoss_Rematch:
			if (!GC.flagsMain.isMinotaurBossDefeated)
			{
				return false;
			}
			break;
		case EventList.PrincessBoss_Rematch:
			if (!GC.flagsMain.isPrincessBossDefeated)
			{
				return false;
			}
			break;
		case EventList.WitchBoss_Rematch:
			if (!GC.flagsMain.isWitchBossDefeated)
			{
				return false;
			}
			break;
		case EventList.BlueFox_Rematch:
			if (!GC.flagsMain.isBlueFoxDefeated)
			{
				return false;
			}
			break;
		case EventList.SwampBoss_Rematch:
			if (!GC.flagsMain.isBossSwampDefeated)
			{
				return false;
			}
			break;
		case EventList.Beach01:
			if (GC.flagsMain.seenEventBeach01)
			{
				return false;
			}
			break;
		case EventList.Forest01:
			if (GC.flagsMain.seenEventForest01)
			{
				return false;
			}
			break;
		case EventList.Farina01:
			if (GC.flagsMain.seenEV_Farina01_meet1)
			{
				return false;
			}
			break;
		case EventList.Farina02:
			if (GC.flagsMain.seenEV_Farina02_meet2)
			{
				return false;
			}
			break;
		case EventList.Farina03:
			if (GC.CheckPlayerHasItem(903))
			{
				return false;
			}
			if (GC.flagsMain.seenEV_Farina03_giveRuby)
			{
				return false;
			}
			break;
		case EventList.Gauntlet1:
			if (GC.flagsMain.winGauntlet1)
			{
				return false;
			}
			break;
		}
		return true;
	}

	public void StartEventMainLogic()
	{
		if (!thisEventHasBeenTriggered)
		{
			thisEventHasBeenTriggered = true;
			switch (thisEvent)
			{
			case EventList.ForestBoss_FirstMatch:
				mainEventCoroutine = StartCoroutine(EventFunction_001());
				break;
			case EventList.BeachBoss_FirstMatch:
				mainEventCoroutine = StartCoroutine(EventFunction_002());
				break;
			case EventList.HillBoss_FirstMatch:
				mainEventCoroutine = StartCoroutine(EventFunction_003());
				break;
			case EventList.WerewolfBoss_First:
				mainEventCoroutine = StartCoroutine(EventFunction_004());
				break;
			case EventList.MinotaurBoss_First:
				mainEventCoroutine = StartCoroutine(EventFunction_005());
				break;
			case EventList.PrincessBoss_First:
				mainEventCoroutine = StartCoroutine(EventFunction_006());
				break;
			case EventList.WitchBoss_First:
				mainEventCoroutine = StartCoroutine(EventFunction_007());
				break;
			case EventList.BlueFox_First:
				mainEventCoroutine = StartCoroutine(EventFunction_008());
				break;
			case EventList.SwampBoss_First:
				mainEventCoroutine = StartCoroutine(EventFunction_009());
				break;
			case EventList.Checkpoint:
				mainEventCoroutine = StartCoroutine(EventFunction_998());
				break;
			case EventList.ForestBoss_Rematch:
			case EventList.BeachBoss_Rematch:
			case EventList.HillBoss_Rematch:
			case EventList.WerewolfBoss_Rematch:
			case EventList.MinotaurBoss_Rematch:
			case EventList.PrincessBoss_Rematch:
			case EventList.WitchBoss_Rematch:
			case EventList.BlueFox_Rematch:
			case EventList.SwampBoss_Rematch:
				mainEventCoroutine = StartCoroutine(EventFunction_999());
				break;
			case EventList.CastleOpenSilverKey:
				EVT_CastleSilverKey();
				break;
			case EventList.CastleOpenDungeonKey:
				EVT_CastleDungeonKey();
				break;
			case EventList.OpenFinalGateway:
				EVT_OpenFinalGateway();
				break;
			case EventList.GalleryPortals:
				EVT_GalleryEnablePortals();
				break;
			case EventList.OpenGenericSideGates:
				EVT_UnlockGenericSideGates();
				break;
			case EventList.Beach01:
				mainEventCoroutine = StartCoroutine(EventFunction_Beach01());
				break;
			case EventList.Farina01:
				mainEventCoroutine = StartCoroutine(EVT_Farina01());
				break;
			case EventList.Farina03:
				mainEventCoroutine = StartCoroutine(EVT_Farina03());
				break;
			case EventList.Forest01:
				mainEventCoroutine = StartCoroutine(EventFunction_Forest01());
				break;
			case EventList.Gauntlet1:
				mainEventCoroutine = StartCoroutine(EventFunction_601());
				break;
			}
			if ((bool)refMarkerClapper)
			{
				refMarkerClapper.SetActive(value: false);
			}
		}
	}

	private IEnumerator EventFunction_001()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_001)
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1001, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		vecPositionGeneric = Waypoints[1].transform.position;
		valueGenericFloat = playerEventDefaultMoveSpeed;
		yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
		if (!GC.sessionFlags.hasSeenEvent_001)
		{
			StartEventDialogue(1002, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_001);
	}

	private IEnumerator EventFunction_002()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_002)
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = 3f;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1003, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		vecPositionGeneric = Waypoints[1].transform.position;
		valueGenericFloat = playerEventDefaultMoveSpeed;
		yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
		SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_002);
	}

	private IEnumerator EventFunction_003()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_003)
		{
			vecPositionGeneric = Waypoints[1].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1004, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		else
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		}
		SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_003);
	}

	private IEnumerator EventFunction_004()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_004)
		{
			GC.PlayEventSpecificAnimationDialogue("BossWerewolf", isFindGameObjectByTag: true, "Enemy-Boss", "Diag-Stand1");
			vecPositionGeneric = Waypoints[1].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1005, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
			vecPositionGeneric = Waypoints[2].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[2].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1006, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
			GC.OverrideCameraMove(new Vector2(1f, 0f));
			GC.OverrideCameraZoom(ControllerCamera.ZoomType.Dialogue);
			GC.PlayEventSpecificAnimationDialogue("BossWerewolf", isFindGameObjectByTag: true, "Enemy-Boss", "Diag-Transform");
			GC.PlaySFXGeneric("WerewolfBoss", 1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f, "voice");
			yield return new WaitUntil(() => GC.eventAnimationIsFinished);
			StartEventDialogue(1007, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
			GC.OverrideCameraMove(new Vector2(1f, 0f));
			GC.OverrideCameraZoom(ControllerCamera.ZoomType.Dialogue);
			GC.SetPlayerSpriteRendererSortingOrder(5);
			GC.PlayEventSpecificAnimationDialogue("BossWerewolf", isFindGameObjectByTag: true, "Enemy-Boss", "Diag-Angry2");
			GC.PlayEventSpecificAnimationMaya(GameCore.SpeakerACT.Hurt1);
			GC.PlaySFXGeneric("WerewolfBoss", 4, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f, "voice");
			yield return new WaitUntil(() => GC.eventAnimationIsFinished);
			GC.SetPlayerSpriteRendererSortingOrder(0, resetToOriginal: true);
			StartEventDialogue(1008, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		else
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		}
		SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_004);
		GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.WerewolfBossCurse, 9999f, "EventWerewolfBoss1st");
		if (CheckRefUI())
		{
			refCoreUI.ToggleWerewolfCurse(setThisState: true);
		}
	}

	private IEnumerator EventFunction_005()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_005)
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1009, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		else
		{
			vecPositionGeneric = Waypoints[1].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		}
		SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_005);
	}

	private IEnumerator EventFunction_006()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_006)
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1010, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		else
		{
			List<AnimAction> list = new List<AnimAction>();
			list.Add(new AnimAction
			{
				whoID = GameCore.SpeakerID.PrincessBoss,
				whichACT = GameCore.SpeakerACT.InvertFacing
			});
			GC.ManageDialogueAnimationsActions(list);
			vecPositionGeneric = Waypoints[1].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		}
		SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_006);
	}

	private IEnumerator EventFunction_007()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_007)
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1011, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		else
		{
			vecPositionGeneric = Waypoints[1].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		}
		SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_007);
	}

	private IEnumerator EventFunction_008()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_008)
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1012, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		else
		{
			vecPositionGeneric = Waypoints[1].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		}
		SignalBossFightToStart(heroicBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_008);
	}

	private IEnumerator EventFunction_009()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		if (!GC.sessionFlags.hasSeenEvent_009)
		{
			vecPositionGeneric = Waypoints[0].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
			StartEventDialogue(1013, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		else
		{
			vecPositionGeneric = Waypoints[1].transform.position;
			valueGenericFloat = playerEventDefaultMoveSpeed;
			yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		}
		SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: true, ref GC.sessionFlags.hasSeenEvent_009);
	}

	private IEnumerator EventFunction_Beach01()
	{
		GC.flagsMain.seenEventBeach01 = true;
		GC.StopMusicTheme(doFadeOut: true, 0.1f);
		yield return new WaitForSeconds(0.5f);
		GC.SceneFadeHandler(actionFadeToBlack: false, 3f);
		StartEventDialogue(7001, doZoomDuringDialogue: true);
		yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		EventCompleted();
		GC.PlayMusicTheme(GameCore.MusicTheme.Beach1, doFadeIn: true, 3f, doLoop: true);
	}

	private IEnumerator EVT_Farina01()
	{
		GC.flagsMain.seenEV_Farina01_meet1 = true;
		vecPositionGeneric = new Vector3(-77.5f, 1.5f, 0f);
		MiscObjects[0].transform.position = vecPositionGeneric;
		MiscObjects[0].GetComponent<SpriteRenderer>().flipX = true;
		MiscObjects[0].GetComponent<BoxCollider2D>().enabled = false;
		MiscObjects[0].GetComponent<NpcLogic>().enabled = false;
		StartEventDialogue(7002, doZoomDuringDialogue: true);
		yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		vecPositionGeneric = Waypoints[0].transform.position;
		valueGenericFloat = playerEventDefaultMoveSpeed;
		yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
		StartEventDialogue(7003, doZoomDuringDialogue: true);
		yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		vecPositionGeneric = Waypoints[1].transform.position;
		valueGenericFloat = 0f - playerEventDefaultMoveSpeed;
		yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		StartEventDialogue(7004, doZoomDuringDialogue: true);
		yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		MiscObjects[0].transform.position = new Vector3(-84f, 25.5f, 0f);
		MiscObjects[0].GetComponent<SpriteRenderer>().flipX = false;
		MiscObjects[0].GetComponent<BoxCollider2D>().enabled = true;
		MiscObjects[0].GetComponent<NpcLogic>().enabled = true;
		Object.Instantiate(MiscObjects[1], vecPositionGeneric, Quaternion.identity, GC.refOBJEffectsParent.transform);
		EventCompleted();
	}

	private IEnumerator EVT_Farina03()
	{
		GC.flagsMain.seenEV_Farina03_giveRuby = true;
		vecPositionGeneric = new Vector3(-46f, 1.75f, 0f);
		MiscObjects[0].transform.position = vecPositionGeneric;
		MiscObjects[0].GetComponent<BoxCollider2D>().enabled = false;
		MiscObjects[0].GetComponent<NpcLogic>().enabled = false;
		StartEventDialogue(7005, doZoomDuringDialogue: true);
		yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		vecPositionGeneric = Waypoints[0].transform.position;
		valueGenericFloat = playerEventDefaultMoveSpeed;
		yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		Vector3 position = Waypoints[1].transform.position;
		position.y -= 0.5f;
		GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, position, "EventLogic:Farina03");
		MiscObjects[2].transform.position = position;
		MiscObjects[2].GetComponent<Collider2D>().enabled = false;
		StartEventDialogue(7006, doZoomDuringDialogue: true);
		yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		vecPositionGeneric = Waypoints[1].transform.position;
		valueGenericFloat = playerEventDefaultMoveSpeed;
		yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[1].name, vecPositionGeneric, valueGenericFloat));
		MiscObjects[0].transform.position = new Vector3(-84f, 25.5f, 0f);
		MiscObjects[0].GetComponent<BoxCollider2D>().enabled = true;
		MiscObjects[0].GetComponent<NpcLogic>().enabled = true;
		Object.Instantiate(MiscObjects[1], vecPositionGeneric, Quaternion.identity, GC.refOBJEffectsParent.transform);
		EventCompleted();
		MiscObjects[2].GetComponent<Collider2D>().enabled = true;
	}

	private IEnumerator EventFunction_Forest01()
	{
		GC.flagsMain.seenEventForest01 = true;
		StartEventDialogue(7007, doZoomDuringDialogue: true);
		yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		EventCompleted();
	}

	private IEnumerator EventFunction_601()
	{
		GC.StopMusicTheme(doFadeOut: true, 2f);
		MiscObjects[0].SetActive(value: true);
		MiscObjects[0].GetComponent<SpriteRenderer>().flipX = true;
		MiscObjects[0].GetComponent<BoxCollider2D>().enabled = false;
		MiscObjects[0].GetComponent<NpcLogic>().enabled = false;
		vecPositionGeneric = Waypoints[0].transform.position;
		valueGenericFloat = playerEventDefaultMoveSpeed;
		yield return subEventCoroutine = StartCoroutine(waitForPlayerToReachPoint(Waypoints[0].name, vecPositionGeneric, valueGenericFloat));
		if (!GC.sessionFlags.hasSeenEvent_601)
		{
			StartEventDialogue(6001, doZoomDuringDialogue: true);
			yield return new WaitUntil(() => GC.eventPlayerFinishedDialogue);
		}
		GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, MiscObjects[0].transform.position, "Event_601");
		MiscObjects[0].GetComponent<SpriteRenderer>().flipX = false;
		MiscObjects[0].GetComponent<BoxCollider2D>().enabled = true;
		MiscObjects[0].GetComponent<NpcLogic>().enabled = true;
		MiscObjects[0].SetActive(value: false);
		Object.Instantiate(MiscObjects[1], MiscObjects[0].transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
		GC.sessionFlags.hasSeenEvent_601 = true;
		GC.GAUNTLET_Toggle(state: true, 8);
		EventCompleted();
	}

	private void EVT_UnlockGenericSideGates()
	{
		if (GC.DIAG_EVT_UnlockSideGatesLogicSceneName(2))
		{
			MiscObjects[0].SetActive(value: false);
			MiscObjects[1].SetActive(value: false);
			thisEventHasBeenTriggered = true;
		}
		else
		{
			thisEventHasBeenTriggered = false;
		}
	}

	private IEnumerator EventFunction_998()
	{
		if (ValidateMiscObjects(0))
		{
			GC.StoreRetryGameCore(MiscObjects[0].transform.position.x, MiscObjects[0].transform.position.y, "EVENT_TRIGGER-998", ignoreCameraOffset: true);
			if (strGeneric == "ChkPtMsg")
			{
				GC.DisplaySystemMessageUI(0, 185, 3f, "", isFlavorCentralMessage: true);
				GetComponent<Animator>().Play("animActive");
			}
			DisableSelfSpecial(disableSelfCollider: true, disableObjectChildren: true);
		}
		yield break;
	}

	private IEnumerator EventFunction_999()
	{
		if (ValidateMiscObjects(0))
		{
			GC.StopMusicTheme(doFadeOut: false, 0f);
			GC.isBossRematch = true;
			DisableSelfSpecial(disableSelfCollider: true, disableObjectChildren: true);
			GC.InstantiateVFX(GameCore.VisualEffect.GenericPortal, MiscObjects[0].transform.position, "EventTriggerLogic.EventFunction999()");
			GC.PlaySFX(GameCore.SFXBasic2.ElectricSurge, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			GC.ToggleBossFightArena();
			yield return new WaitForSeconds(1f);
			MiscObjects[0].SetActive(value: true);
			MiscObjects[0].GetComponent<EnemyGeneralLogic>().BossRematchInitializeBehavior();
			SignalBossFightToStart(defaultBossTheme, doUpdateBoolFlag: false, ref dummyBool);
			Event999BossRematch_ExecuteSpecificBehaviorPerType();
		}
	}

	private void Event999BossRematch_ExecuteSpecificBehaviorPerType()
	{
		EventList eventList = thisEvent;
		if (eventList == EventList.WerewolfBoss_Rematch)
		{
			if (CheckRefUI())
			{
				refCoreUI.ToggleWerewolfCurse(setThisState: true);
			}
			GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.WerewolfBossCurse, 9999f, "EventWerewolfBossRematch");
		}
	}

	private void EVT_CastleSilverKey()
	{
		if (GC.flagsMain.hasUsedCastleSilverKey)
		{
			MiscObjects[1].SetActive(value: true);
			MiscObjects[0].SetActive(value: false);
			thisEventHasBeenTriggered = true;
		}
		else
		{
			thisEventHasBeenTriggered = false;
		}
	}

	private void EVT_CastleDungeonKey()
	{
		if (GC.flagsMain.hasUsedCastleDungeonKey)
		{
			MiscObjects[1].SetActive(value: true);
			MiscObjects[0].SetActive(value: false);
			thisEventHasBeenTriggered = true;
		}
		else
		{
			thisEventHasBeenTriggered = false;
		}
	}

	private void EVT_OpenFinalGateway()
	{
		if (GC.flagsMain.isWitchBossDefeated)
		{
			MiscObjects[1].SetActive(value: true);
			MiscObjects[0].SetActive(value: false);
			thisEventHasBeenTriggered = true;
		}
		else
		{
			thisEventHasBeenTriggered = false;
		}
	}

	private void EVT_GalleryEnablePortals()
	{
		bool flag = false;
		switch (strGeneric)
		{
		case "beach":
			if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Beach))
			{
				flag = true;
			}
			break;
		case "forest":
			if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Forest))
			{
				flag = true;
			}
			break;
		case "hill":
			if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Hill))
			{
				flag = true;
			}
			break;
		case "city":
			if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.City))
			{
				flag = true;
			}
			break;
		case "castle":
			if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Castle))
			{
				flag = true;
			}
			break;
		case "cave":
			if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Cave))
			{
				flag = true;
			}
			break;
		case "swamp":
			if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Swamp))
			{
				flag = true;
			}
			break;
		case "machine":
			if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.HighTech))
			{
				flag = true;
			}
			break;
		}
		if (flag)
		{
			MiscObjects[0].SetActive(value: true);
		}
		thisEventHasBeenTriggered = true;
	}

	private void StartEventDialogue(int idOfDialogue, bool doZoomDuringDialogue)
	{
		GC.SignalPlayerToStartDialogue(idOfDialogue, doZoomDuringDialogue);
	}

	private IEnumerator waitForPlayerToReachPoint(string waypointName, Vector2 pointInCurrentScene, float customMovSpeed)
	{
		if (!GC.SignalPlayerMoveHorizontalyUntilPoint(waypointName, pointInCurrentScene, customMovSpeed))
		{
			StopAllEventLogic();
		}
		yield return new WaitUntil(() => GC.eventPlayerReachedPoint);
		GC.eventPlayerReachedPoint = false;
	}

	private void SignalBossFightToStart(GameCore.MusicTheme thisTheme, bool doUpdateBoolFlag, ref bool flagToSet)
	{
		GC.PlayMusicTheme(thisTheme, doFadeIn: false, 0f, doLoop: true);
		GC.SignalBossFightStart();
		if (doUpdateBoolFlag)
		{
			flagToSet = true;
		}
		EventCompleted();
	}

	private void DisableSelfSpecial(bool disableSelfCollider, bool disableObjectChildren)
	{
		if (disableSelfCollider)
		{
			objBox2D.enabled = false;
		}
		if (!disableObjectChildren)
		{
			return;
		}
		foreach (Transform item in transform)
		{
			item.gameObject.SetActive(value: false);
		}
	}

	private void EventCompleted()
	{
		GC.SignalPlayerEventHasBeenFinished();
		DeactivateEvent();
	}

	private void DeactivateEvent()
	{
		StopAllCoroutines();
		if ((bool)refMarkerClapper)
		{
			refMarkerClapper.SetActive(value: false);
		}
		gameObject.SetActive(value: false);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if ((bool)objIconToDisplay && other.tag == "Player" && !GC.isPlayerFainted)
		{
			CheckForPlayerInteraction(start: true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if ((bool)objIconToDisplay && other.tag == "Player")
		{
			CheckForPlayerInteraction(start: false);
		}
	}

	private void CheckForPlayerInteraction(bool start)
	{
		if (start)
		{
			objIconToDisplay.SetActive(value: true);
			InvokeRepeating("RepeatingInteractionCheck", 0f, 0.1f);
		}
		else
		{
			CancelInvoke("RepeatingInteractionCheck");
			objIconToDisplay.SetActive(value: false);
		}
	}

	private void RepeatingInteractionCheck()
	{
		if (GC.isPlayerInteractingLocked && objIconToDisplay.activeInHierarchy)
		{
			objIconToDisplay.SetActive(value: false);
		}
		else if (!GC.isPlayerInteractingLocked && !objIconToDisplay.activeInHierarchy)
		{
			objIconToDisplay.SetActive(value: true);
		}
	}

	private bool CheckRefUI()
	{
		if ((bool)refCoreUI)
		{
			return true;
		}
		GameObject gameObject = GameObject.Find("[CANVAS]");
		refCoreUI = gameObject.GetComponent<GameUI>();
		if ((bool)refCoreUI)
		{
			return true;
		}
		return false;
	}

	private bool ValidateMiscObjects(int indexToValidate)
	{
		if (MiscObjects != null && MiscObjects.Length >= indexToValidate + 1)
		{
			return true;
		}
		Debug.Log("[EVENT]ValidateMiscObjects: Array of misc objects (or specified index) is NULL!");
		return false;
	}

	private void StopAllEventLogic()
	{
		Debug.Log("Critical Error or Unexpected Behaviour encoutered while processing the event at scene " + GC.strCurrentActiveSceneName + ". GameObject: " + gameObject.name);
		StopAllCoroutines();
		GC.SignalPlayerEventHasBeenFinished();
		DeactivateEvent();
	}

	private void DisplayMessageNullEvent()
	{
		Debug.Log("GC is [NULL] (at scene " + GC.strCurrentActiveSceneName + "). Deactivating the event attached to the gameobject named: " + gameObject.name);
	}
}
