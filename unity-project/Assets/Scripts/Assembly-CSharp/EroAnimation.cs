using System;
using System.Collections.Generic;
using UnityEngine;

public class EroAnimation : MonoBehaviour
{
	private enum DoubleInternal
	{
		LayerOff_CircleOff = 0,
		LayerOff_CircleOn = 1,
		LayerOn_CircleOff = 2,
		LayerOn_CircleOn = 3
	}

	public bool isSubAnimator;

	public bool isSyncInternalShotEveryPhase;

	public bool isMaintainHidePlayerOnFaint;

	public GameObject objComboReceptor;

	public GameObject[] arrayFatalVoreReplaceInternal;

	public ENM_BadEndScene BadEndConfiguration;

	[HideInInspector]
	public bool isPlayingComboEroScene;

	[HideInInspector]
	public int _eroPhaseAnimator;

	[HideInInspector]
	public int _eroIDVariant;

	private const string FLB_Invoke1 = "UNIQUE_FlowerBlossomSinkPlayer";

	private const string FLB_Invoke2 = "UNIQUE_FlowerBlossomBeginMainEro";

	private const float injectIncrements = 0.15f;

	private const float lowerInjectLimit = 0.2f;

	private const float upperInjectLimit = 2.5f;

	private List<GameObject> listOfComboParticipatingEnemies;

	private GameObject tmpObjPlayerReference;

	private GameObject genericOBJ;

	private GameObject objOtherEnemyComboEro;

	private Animator currentAN;

	private Animator addInternalShotsAN;

	private ENM_EroPhaseControl[] currentEroControl;

	private Vector2 offsetPlayerDamageDisplay;

	private Vector3 offsetPlayerHiddenPos;

	private Vector3 offsetPlayerReleasedPos;

	private Vector3 targetPosGeneric;

	private Vector3 tmpOffset3;

	private Vector3 tmpOffset4;

	private bool isKeepPlayerHiddenPosUpdated;

	private bool localInternalShots;

	private bool blockIView;

	private bool applySync;

	private bool isInitialDualStateCheck;

	private bool ignoreEroEndLogicAfterSpecial;

	private bool lockCurrentLoop;

	private bool boolGeneric;

	private bool hackHasShowBadEndSystemMessage;

	private bool hasUpdatedCurrentFetishInfo;

	private int intGetCurrentEroPhase;

	private int countEroLoop;

	private int baseTouchDamage;

	private int dmgOfEro;

	private int countGeneric;

	private float injectSpeed;

	private DoubleInternal currentDualInternalState;

	private EroAnimation ParentEroScript;

	private EnemyGeneralLogic refMainEnemyScript;

	private Controller_Player playerScript;

	private GameCore GC;

	private void Start()
	{
		RefreshInitialReferences();
	}

	private void RefreshInitialReferences()
	{
		GC = GameCore.Instance;
		currentDualInternalState = DoubleInternal.LayerOff_CircleOff;
		isKeepPlayerHiddenPosUpdated = true;
		isInitialDualStateCheck = true;
		localInternalShots = false;
		isPlayingComboEroScene = false;
		applySync = false;
		blockIView = false;
		boolGeneric = false;
		ignoreEroEndLogicAfterSpecial = false;
		hackHasShowBadEndSystemMessage = false;
		intGetCurrentEroPhase = 0;
		countEroLoop = 0;
		countGeneric = 0;
		offsetPlayerHiddenPos = Vector3.zero;
		offsetPlayerReleasedPos = Vector3.zero;
		targetPosGeneric = Vector3.zero;
		tmpOffset3 = Vector3.zero;
		tmpOffset4 = Vector3.zero;
		genericOBJ = null;
	}

	private void FixedUpdate()
	{
		if (isKeepPlayerHiddenPosUpdated)
		{
			if (offsetPlayerHiddenPos != base.transform.position + tmpOffset3)
			{
				offsetPlayerHiddenPos = base.transform.position + tmpOffset3;
			}
			if (offsetPlayerReleasedPos != base.transform.position + tmpOffset4)
			{
				offsetPlayerReleasedPos = base.transform.position + tmpOffset4;
			}
		}
		if (tmpObjPlayerReference != null && GC.playerIsBeingGrabed && isKeepPlayerHiddenPosUpdated && !isSubAnimator)
		{
			SetPlayerEroHiddenPosition();
		}
		if ((bool)refMainEnemyScript && refMainEnemyScript.GetIsCurrentlyGrabing() && BadEndConfiguration.hasBadEnd && BadEndConfiguration.eroVariantID == _eroIDVariant && GC.doAllowBadEnd && !hackHasShowBadEndSystemMessage && GC.isPlayerFainted)
		{
			hackHasShowBadEndSystemMessage = true;
			GC.DisplaySystemMessageUI(0, 127, 8f);
		}
	}

	private void Update()
	{
		CheckIfInternalsShotsAreAllowedToDisplay();
		if (applySync && (bool)addInternalShotsAN)
		{
			addInternalShotsAN.Play(0, 0, currentAN.GetCurrentAnimatorStateInfo(0).normalizedTime);
		}
		if ((bool)refMainEnemyScript && (bool)currentAN)
		{
			refMainEnemyScript.SetEroPhase(currentAN.GetInteger("eroPhase"));
		}
		if ((bool)refMainEnemyScript && refMainEnemyScript.currentEnemyType == GameCore.EnemyType.BossMinotaur)
		{
			HACK_MINOTAUR_BOSS_BADEND();
		}
		if ((bool)refMainEnemyScript && refMainEnemyScript.currentEnemyType == GameCore.EnemyType.BossWerewolf)
		{
			HACK_WEREWOLF_BOSS_BADEND(checkGeneric: true);
		}
	}

	public void BeginEroLogic(GameObject objPL, Vector2 vecOffs1, EnemyGeneralLogic refEnemy, Animator localAN, int refTouchDMG, Vector2 vecOffs2, Vector2 vecOffs3, Vector2 vecOffs4)
	{
		try
		{
			refMainEnemyScript = refEnemy;
			currentAN = localAN;
			baseTouchDamage = refTouchDMG;
			offsetPlayerDamageDisplay = vecOffs2;
			tmpOffset3 = vecOffs3;
			offsetPlayerHiddenPos = base.transform.position + (Vector3)vecOffs3;
			tmpOffset4 = vecOffs4;
			offsetPlayerReleasedPos = base.transform.position + (Vector3)vecOffs4;
			if (InitialCheckEroValidity())
			{
				GC.PlaySFX(GameCore.SFXBasic1.HitHurtTouch2, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				GC.playerIsBeingGrabed = true;
				GC.enemyGrabbingEroPhase = 0;
				tmpObjPlayerReference = objPL;
				hasUpdatedCurrentFetishInfo = false;
				lockCurrentLoop = false;
				injectSpeed = 0f;
				playerScript = tmpObjPlayerReference.GetComponent<Controller_Player>();
				refMainEnemyScript.SetGrabbing(state: true);
				refMainEnemyScript.ERO_ToggleSpecialEroCollider(activate: true);
				if ((bool)objComboReceptor)
				{
					objComboReceptor.SetActive(value: true);
				}
				playerScript.PlayerWasEroGrabbed(vecOffs1);
				refMainEnemyScript.SetAddToRendererLayerOrder(1);
				refMainEnemyScript.ERO_ToggleCountEroTotalTime(turnON: true);
				CheckForEroVariants();
				ValidateEroInternalsAtStart();
				if (refMainEnemyScript.isStatic && !refMainEnemyScript.eroCore.forceDefaultFacing && GC.MATH_RNG_INT(0, 1) == 1)
				{
					refMainEnemyScript.SetEnemyFacing(-1f);
				}
				CheckSpecialTypesOfEroAnimation(stateON: true);
			}
		}
		catch (Exception ex)
		{
			Debug.Log("ERO_ANIMATION[BeginEroLogic]: " + ex.Message + " -> " + ex.ToString());
			EndEroLogic();
		}
	}

	public void EndEroLogic()
	{
		try
		{
			float num = refMainEnemyScript.GetEroGracePeriod();
			_eroIDVariant = 0;
			if (GC.isPlayerFainted)
			{
				num *= 2f;
			}
			if (refMainEnemyScript.isGalleryView)
			{
				num = 0.001f;
				KillPlayer();
			}
			CheckIfPlayerIsGoneForGood();
			if ((bool)tmpObjPlayerReference)
			{
				playerScript = tmpObjPlayerReference.GetComponent<Controller_Player>();
			}
			CheckSpecialTypesOfEroAnimation(stateON: false);
			if (ignoreEroEndLogicAfterSpecial)
			{
				return;
			}
			if (refMainEnemyScript.isStatic && refMainEnemyScript.GetEnemyFacing() == -1f)
			{
				refMainEnemyScript.SetEnemyFacing(1f);
			}
			refMainEnemyScript.SetGrabbing(state: false);
			if (!GC.isPlayerGoneForGood)
			{
				GC.playerIsBeingGrabed = false;
				SetPlayerEroReleasedPosition();
			}
			refMainEnemyScript.ERO_ToggleSpecialEroCollider(activate: false);
			if ((bool)objComboReceptor)
			{
				objComboReceptor.SetActive(value: false);
			}
			if (listOfComboParticipatingEnemies != null && listOfComboParticipatingEnemies.Count > 0)
			{
				foreach (GameObject listOfComboParticipatingEnemy in listOfComboParticipatingEnemies)
				{
					listOfComboParticipatingEnemy.GetComponent<EnemyGeneralLogic>().enabled = true;
					listOfComboParticipatingEnemy.SetActive(value: true);
					listOfComboParticipatingEnemy.GetComponent<EnemyGeneralLogic>().RunTheCoroutinesAfterEroEnd(4f);
					if ((bool)base.transform.parent)
					{
						listOfComboParticipatingEnemy.transform.SetParent(base.transform.parent, worldPositionStays: true);
					}
				}
				listOfComboParticipatingEnemies.Clear();
				listOfComboParticipatingEnemies = null;
			}
			if (refMainEnemyScript.eroCore.basicEroInternals != null && refMainEnemyScript.eroCore.layeredEroInternal)
			{
				ToggleFourStatesOfDoubleInternalsLogic(mainState: false, eroEnd: true);
			}
			else
			{
				ToggleInternalView(state: false);
			}
			applySync = false;
			isPlayingComboEroScene = false;
			refMainEnemyScript.SetAddToRendererLayerOrder(-1);
			refMainEnemyScript.ERO_ToggleCountEroTotalTime(turnON: false);
			if ((bool)playerScript)
			{
				playerScript.PlayerWasEroReleased(GC.isPlayerGoneForGood, refMainEnemyScript.GetIsGalleryView());
			}
			tmpObjPlayerReference = null;
			refMainEnemyScript.RunTheCoroutinesAfterEroEnd(num);
			if (BadEndConfiguration.hasBadEnd)
			{
				refMainEnemyScript.BAD_ResetBadEndState();
			}
			addInternalShotsAN = null;
			playerScript = null;
			objOtherEnemyComboEro = null;
			tmpOffset3 = Vector3.zero;
			hackHasShowBadEndSystemMessage = false;
		}
		catch (Exception ex)
		{
			Debug.Log("ERO_ANIMATION[EndEroLogic]: " + ex.Message);
			throw;
		}
	}

	public void InterruptSoloSceneAndStartComboScene(int intPlayThisComboID, GameObject objOtherEnemy)
	{
		float speedAnimation = currentEroControl[1].speedAnimation;
		isPlayingComboEroScene = true;
		countEroLoop = 0;
		currentAN.SetInteger("eroPhase", GetNewEroPhaseDueToComboStart(refMainEnemyScript.currentEnemyType));
		_eroPhaseAnimator = currentAN.GetInteger("eroPhase");
		currentAN.SetBool("isGrabbing", value: true);
		currentAN.SetBool("isEro", value: true);
		currentAN.SetFloat("speedAnimation", speedAnimation);
		ToggleInternalView(state: false);
		CheckForComboInternalShot(intPlayThisComboID);
		ValidateEroInternalsAtStart();
		UpdateAnimInformationOnInternalShot(1, speedAnimation);
		GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, refMainEnemyScript.transform.position, "EROANIMATION:InterruptSoloSceneAndStartComboScene");
		_eroIDVariant = intPlayThisComboID;
		currentAN.SetInteger("eroVariantID", intPlayThisComboID);
		currentAN.Play("ComboStartPoint");
		if (listOfComboParticipatingEnemies == null)
		{
			listOfComboParticipatingEnemies = new List<GameObject>();
		}
		objOtherEnemyComboEro = objOtherEnemy;
		objOtherEnemyComboEro.transform.SetParent(null, worldPositionStays: true);
		objOtherEnemyComboEro.GetComponent<EnemyGeneralLogic>().enabled = false;
		objOtherEnemyComboEro.SetActive(value: false);
		listOfComboParticipatingEnemies.Add(objOtherEnemyComboEro);
	}

	private void CheckIfPlayerIsGoneForGood()
	{
		if ((bool)GC && GC.debugForceReleaseEroGrab)
		{
			GC.isPlayerGoneForGood = false;
			GC.debugForceReleaseEroGrab = false;
			return;
		}
		if (refMainEnemyScript.currentEnemyType == GameCore.EnemyType.LatexMaiden)
		{
			GC.isPlayerGoneForGood = false;
			return;
		}
		if (refMainEnemyScript.currentEnemyType == GameCore.EnemyType.BossWerewolf)
		{
			GC.isPlayerGoneForGood = false;
			return;
		}
		if (!GC.isPlayerGoneForGood && isMaintainHidePlayerOnFaint && GC.isPlayerFainted)
		{
			GC.isPlayerGoneForGood = true;
		}
		if (!GC.isPlayerGoneForGood && GC.isPlayerFainted && GC.currentEnemyGrabbingIsEroFatal)
		{
			GC.isPlayerGoneForGood = true;
		}
	}

	public void SpecialVoreFatalCheck()
	{
		if (isMaintainHidePlayerOnFaint)
		{
			return;
		}
		GameCore.FetishType[] fetishTags = refMainEnemyScript.eroCore.FetishTags;
		for (int i = 0; i < fetishTags.Length; i++)
		{
			if (fetishTags[i] == GameCore.FetishType.Vore && GC.doAllVoreScenesAreFatal)
			{
				isMaintainHidePlayerOnFaint = true;
				KillPlayer();
				return;
			}
		}
		ENM_EroSoloSceneVariations[] eroSoloAlts = refMainEnemyScript.eroCore.eroSoloAlts;
		foreach (ENM_EroSoloSceneVariations eNM_EroSoloSceneVariations in eroSoloAlts)
		{
			if (eNM_EroSoloSceneVariations.GenericID == currentAN.GetInteger("eroVariantID") && eNM_EroSoloSceneVariations.SceneTag == GameCore.FetishType.Vore && GC.doAllVoreScenesAreFatal)
			{
				isMaintainHidePlayerOnFaint = true;
				KillPlayer();
				break;
			}
		}
	}

	public void SpecialVoreReleaseCheck(int checkType)
	{
		if (checkType == 1 && (bool)GC && !GC.doAllVoreScenesAreFatal)
		{
			EndEroLogic();
		}
	}

	private void UpdateCurrentFetishInfo()
	{
		hasUpdatedCurrentFetishInfo = true;
		GC.enemyGrabbingEroFetishType = GC.GetFetishTypeByEroSceneVariantID(_eroIDVariant, refMainEnemyScript.eroCore);
	}

	public void EroGrabLoop()
	{
		if ((refMainEnemyScript.HasAnyEroVariants() || refMainEnemyScript.HasAnyComboscenes()) && _eroIDVariant != currentAN.GetInteger("eroVariantID"))
		{
			_eroIDVariant = currentAN.GetInteger("eroVariantID");
		}
		if (!lockCurrentLoop)
		{
			countEroLoop++;
		}
		GC.enemyGrabbingEroVariantID = _eroIDVariant;
		if (!hasUpdatedCurrentFetishInfo)
		{
			UpdateCurrentFetishInfo();
		}
		SetAnimationSpeed(currentEroControl[intGetCurrentEroPhase].speedAnimation);
		if (countEroLoop >= currentEroControl[0].runLoops)
		{
			DoUniqueBehavioursOnGrabEnd();
			currentAN.SetBool("isEro", value: true);
			countEroLoop = 0;
			currentAN.SetInteger("eroPhase", 1);
			_eroPhaseAnimator = 1;
			SetAnimationSpeed(currentEroControl[intGetCurrentEroPhase].speedAnimation);
		}
		LustDuringEroGrab();
		if (!refMainEnemyScript.GetIsGalleryView() && GC.MATH_IsDivisibleBy(countEroLoop + 1, currentEroControl[0].frequencyOfDmg))
		{
			ApplyEroGrabDamage(baseTouchDamage / currentEroControl[0].divisionOfDmg, offsetPlayerDamageDisplay);
		}
	}

	public void EroBasicMainLoop()
	{
		if ((refMainEnemyScript.HasAnyEroVariants() || refMainEnemyScript.HasAnyComboscenes()) && _eroIDVariant != currentAN.GetInteger("eroVariantID"))
		{
			_eroIDVariant = currentAN.GetInteger("eroVariantID");
		}
		intGetCurrentEroPhase = currentAN.GetInteger("eroPhase");
		GC.enemyGrabbingEroVariantID = _eroIDVariant;
		GC.enemyGrabbingEroPhase = intGetCurrentEroPhase;
		if (!hasUpdatedCurrentFetishInfo)
		{
			UpdateCurrentFetishInfo();
		}
		if (intGetCurrentEroPhase >= currentEroControl.Length)
		{
			EroLoopCutToEnd();
			return;
		}
		SetAnimationSpeed(currentEroControl[intGetCurrentEroPhase].speedAnimation);
		if (countEroLoop >= currentEroControl[intGetCurrentEroPhase].runLoops)
		{
			intGetCurrentEroPhase++;
			UpdateAnimInformationOnInternalShot(intGetCurrentEroPhase, 0f);
			currentAN.SetInteger("eroPhase", intGetCurrentEroPhase);
			_eroPhaseAnimator = intGetCurrentEroPhase;
			countEroLoop = 0;
			DoUniqueBehavioursOnEroPhaseChange("normal");
			if (intGetCurrentEroPhase >= currentEroControl.Length)
			{
				EroLoopCutToEnd();
				return;
			}
			SetAnimationSpeed(currentEroControl[intGetCurrentEroPhase].speedAnimation);
			if (currentEroControl[intGetCurrentEroPhase].divisionOfDmg > 0)
			{
				dmgOfEro = baseTouchDamage / currentEroControl[intGetCurrentEroPhase].divisionOfDmg;
			}
		}
		CheckIfPlayerIsGoneForGood();
		LustDuringEroGrab();
		if (!refMainEnemyScript.GetIsGalleryView() && GC.MATH_IsDivisibleBy(countEroLoop + 1, currentEroControl[intGetCurrentEroPhase].frequencyOfDmg))
		{
			ApplyEroGrabDamage(dmgOfEro, offsetPlayerDamageDisplay);
		}
		if (!lockCurrentLoop)
		{
			countEroLoop++;
		}
	}

	private void EroLoopCutToEnd()
	{
		if (!isSubAnimator)
		{
			EndEroLogic();
		}
		else
		{
			CheckSpecialTypesOfEroAnimation(stateON: false);
		}
	}

	private void UpdateGrabInfoToGameCore()
	{
	}

	public void MidEroManipulateValues(bool resetLoopCounters, int setThisEroPhase = -1, string playThisEroAnim = "", int alterMaxLoopOfPhase = -1)
	{
		try
		{
			if (refMainEnemyScript != null && refMainEnemyScript.eroCore.eroAnimType == GameCore.EroAnimType.SwampBoss)
			{
				HACK_SWAMP_BOSS_REEVALUATE_CONTROL(currentAN.GetInteger("eroVariantID"));
			}
			if (resetLoopCounters)
			{
				countEroLoop = 0;
			}
			if (setThisEroPhase != -1)
			{
				currentAN.SetInteger("eroPhase", setThisEroPhase);
				_eroPhaseAnimator = setThisEroPhase;
				currentAN.SetFloat("speedAnimation", currentEroControl[setThisEroPhase].speedAnimation);
			}
			if (playThisEroAnim != "")
			{
				currentAN.Play(playThisEroAnim);
			}
			if (alterMaxLoopOfPhase != -1 && setThisEroPhase != -1)
			{
				currentEroControl[setThisEroPhase].runLoops = alterMaxLoopOfPhase;
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
			Debug.Log("[ERO_ANIM:MidEroManipulateValues()] ERROR!");
			Debug.Log("resetLoopCounters: " + resetLoopCounters);
			Debug.Log("setThisEroPhase: " + setThisEroPhase);
			Debug.Log("playThisEroAnim: " + playThisEroAnim.ToString());
			Debug.Log("alterMaxLoopOfPhase: " + alterMaxLoopOfPhase);
			throw;
		}
	}

	private void SetAnimationSpeed(float currentSpeedRef)
	{
		float num = currentSpeedRef;
		if (refMainEnemyScript.isGalleryView && injectSpeed >= 0.2f && injectSpeed <= 2.5f)
		{
			num = injectSpeed;
		}
		currentAN.SetFloat("speedAnimation", num);
		if (isSubAnimator)
		{
			UpdateSpeedAnimationOnParent(num);
		}
		UpdateAnimInformationOnInternalShot(intGetCurrentEroPhase, num);
	}

	private void LustDuringEroGrab()
	{
		if (GC.enemyGrabbingEroFetishType == GameCore.FetishType.Vore && GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.VoreWhore))
		{
			GC.AddRemoveLustToPlayer(1, doVFX: true);
		}
		else if (GC.MATH_RNG_ROLL(8))
		{
			GC.AddRemoveLustToPlayer(1, doVFX: true);
		}
	}

	private void CheckSpecialTypesOfEroAnimation(bool stateON)
	{
		GameObject subObjectToAnimate = null;
		EroAnimation subOtherScript = null;
		Animator subAN = null;
		bool isReturnToIdle = false;
		switch (refMainEnemyScript.eroCore.eroAnimType)
		{
		default:
			if (stateON)
			{
				string stateName = "Ero-Grab";
				currentAN.SetInteger("eroPhase", 0);
				_eroPhaseAnimator = 0;
				currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
				currentAN.SetBool("isGrabbing", value: true);
				currentAN.Play(stateName);
			}
			else
			{
				isReturnToIdle = true;
			}
			break;
		case GameCore.EroAnimType.FlowerBlossom:
			FLOWER_BLOSSOM_SPECIAL_ANIM(stateON, subObjectToAnimate, subOtherScript, subAN, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.PaintingEvil:
			PAINTINGEVIL_SPECIAL_GRABS(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.WerewolfBoss:
			WEREWOLF_BOSS_SPECIAL_GRABS(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.MinotaurBoss:
			MINOTAUR_BOSS_SPECIAL_GRABS(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.CentaurGirl:
			CENTAUR_GIRL_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.GhostButler:
			GHOST_BUTLER_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.PrincessBoss:
			PRINCESS_BOSS_SPECIAL_GRABS(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.DungeonLatex:
			LATEX_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.DogBoy:
			DOG_BOY_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.WormVore:
			WORMVORE_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.WitchBoss:
			WITCH_BOSS_SPECIAL_GRABS(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.BlueFoxBoss:
			BLUEFOX_BOSS_SPECIAL_GRABS(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.Medusa:
			MEDUSA_SPECIAL_GRABS(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.SlimeGirl:
			SLIME_GIRL_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.SwampBoss:
			SWAMP_BOSS_SPECIAL_GRABS(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.CatGirl:
			CAT_GIRL_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.BatGirl:
			BAT_GIRL_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.DwarfGirl:
			DWARF_GIRL_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.MushroomFuta:
			MUSHROOM_FUTAGIRL_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.DarkDamsel:
			DARKDAMSEL_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.SwampFairy:
			SWAMPFAIRY_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.CentaurFutaDruid:
			CENTAURFUTADRUID_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		case GameCore.EroAnimType.SwampGolem:
			SWAMPGOLEM_SPECIAL_GRAB(stateON, ref isReturnToIdle);
			break;
		}
		if (isReturnToIdle)
		{
			CleanAnimMainParameters();
			currentAN.Play("Basic-Idle");
		}
	}

	private void FLOWER_BLOSSOM_SPECIAL_ANIM(bool setState, GameObject subObjectToAnimate, EroAnimation subOtherScript, Animator subAN, ref bool isReturnToIdle)
	{
		subObjectToAnimate = refMainEnemyScript.GetMiscGameObject(2);
		subOtherScript = subObjectToAnimate.GetComponent<EroAnimation>();
		subAN = subObjectToAnimate.GetComponent<Animator>();
		if (setState)
		{
			refMainEnemyScript.GetMiscGameObject(0).SetActive(value: true);
			subObjectToAnimate.SetActive(value: true);
			genericOBJ = subObjectToAnimate;
			isKeepPlayerHiddenPosUpdated = false;
			SetPlayerEroHiddenPosition();
			currentAN.SetBool("isGrabbing", value: true);
			subOtherScript.SetupSpecialReferences(this, refMainEnemyScript, subAN, refMainEnemyScript.GetDmgEro(), refMainEnemyScript.GetDmgDisplayOffset(), "Ero-Grab", tmpObjPlayerReference);
		}
		else if (isSubAnimator)
		{
			base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
			CleanSubSpecialParameters();
		}
		else
		{
			isReturnToIdle = true;
			subObjectToAnimate.SetActive(value: false);
			subOtherScript.CancelInvoke("UNIQUE_FlowerBlossomSinkPlayer");
			subObjectToAnimate.transform.localPosition = Vector3.zero;
			refMainEnemyScript.SetParentRendererAlpha(1f);
			refMainEnemyScript.GetMiscGameObject(0).SetActive(value: false);
		}
	}

	private void WEREWOLF_BOSS_SPECIAL_GRABS(bool setState, ref bool isReturnToIdle)
	{
		if (setState)
		{
			bool flag = false;
			if (refMainEnemyScript.isGalleryView && currentAN.GetInteger("eroVariantID") == 2)
			{
				flag = true;
			}
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			if (!flag && (!GC.isPlayerFainted || (GC.isPlayerFainted && GC.playerSpecialAnimCount < 5)))
			{
				currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
				currentAN.Play("CWW-EroA_1_Grab");
			}
			else
			{
				HACK_WEREWOLF_BOSS_BADEND(checkGeneric: false);
				currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
				currentAN.Play("CWW-EroB1_1_Grab");
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private void MINOTAUR_BOSS_SPECIAL_GRABS(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = MINOTAUR_BOSS_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("MNT-Ero1Doggy-01Grab");
				break;
			case 2:
				currentAN.Play("MNT-Ero2Stand-01Grab");
				break;
			case 3:
				currentAN.Play("MNT-Ero3LegUp-01Grab");
				break;
			}
			ignoreEroEndLogicAfterSpecial = false;
		}
		else if (GC.isPlayerFainted || refMainEnemyScript.isGalleryView)
		{
			MINOTAUR_BOSS_ENDLESS_ERO(integer);
			ignoreEroEndLogicAfterSpecial = true;
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private void MINOTAUR_BOSS_ENDLESS_ERO(int tmpEroVariantID)
	{
		int num = 0;
		if (tmpEroVariantID == 1 || tmpEroVariantID == 2 || tmpEroVariantID == 3)
		{
			num = 1;
		}
		currentAN.SetInteger("eroPhase", num);
		_eroPhaseAnimator = num;
		currentAN.SetBool("isGrabbing", value: true);
		switch (tmpEroVariantID)
		{
		case 1:
			currentAN.Play("StartPreStand");
			tmpEroVariantID = 11;
			break;
		case 2:
			currentAN.Play("StartPreAnal");
			tmpEroVariantID = 12;
			break;
		case 3:
			currentAN.Play("StartPreDoggy");
			tmpEroVariantID = 13;
			break;
		case 11:
			currentAN.Play("MNT-Ero2Stand-01Grab");
			tmpEroVariantID = 2;
			break;
		case 12:
			currentAN.Play("MNT-Ero3LegUp-01Grab");
			tmpEroVariantID = 3;
			break;
		case 13:
			currentAN.Play("MNT-Ero1Doggy-01Grab");
			tmpEroVariantID = 1;
			break;
		}
		currentEroControl = MINOTAUR_BOSS_GetPhaseControl(tmpEroVariantID);
		currentAN.SetFloat("speedAnimation", currentEroControl[num].speedAnimation);
		currentAN.SetInteger("eroVariantID", tmpEroVariantID);
	}

	private ENM_EroPhaseControl[] MINOTAUR_BOSS_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		switch (eroID)
		{
		default:
			result = GC.defaultEroPhases;
			break;
		case 1:
			result = new ENM_EroPhaseControl[5]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.65f,
					frequencyOfDmg = 3,
					divisionOfDmg = 10
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.95f,
					frequencyOfDmg = 3,
					divisionOfDmg = 8
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 1.3f,
					frequencyOfDmg = 4,
					divisionOfDmg = 7
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.5f,
					frequencyOfDmg = 2,
					divisionOfDmg = 6
				}
			};
			break;
		case 2:
			result = new ENM_EroPhaseControl[8]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.65f,
					frequencyOfDmg = 3,
					divisionOfDmg = 10
				},
				new ENM_EroPhaseControl
				{
					runLoops = 7,
					speedAnimation = 1f,
					frequencyOfDmg = 2,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 9,
					speedAnimation = 0.75f,
					frequencyOfDmg = 2,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 6,
					speedAnimation = 0.65f,
					frequencyOfDmg = 2,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.95f,
					frequencyOfDmg = 3,
					divisionOfDmg = 8
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 1.3f,
					frequencyOfDmg = 4,
					divisionOfDmg = 7
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.5f,
					frequencyOfDmg = 2,
					divisionOfDmg = 6
				}
			};
			break;
		case 3:
			result = new ENM_EroPhaseControl[9]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.65f,
					frequencyOfDmg = 3,
					divisionOfDmg = 10
				},
				new ENM_EroPhaseControl
				{
					runLoops = 7,
					speedAnimation = 1f,
					frequencyOfDmg = 2,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 9,
					speedAnimation = 0.75f,
					frequencyOfDmg = 2,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 6,
					speedAnimation = 0.65f,
					frequencyOfDmg = 2,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.95f,
					frequencyOfDmg = 3,
					divisionOfDmg = 8
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 1.3f,
					frequencyOfDmg = 4,
					divisionOfDmg = 7
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.5f,
					frequencyOfDmg = 2,
					divisionOfDmg = 6
				}
			};
			break;
		case 11:
			result = new ENM_EroPhaseControl[4]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 0,
					speedAnimation = 0f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.65f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		case 12:
		case 13:
			result = new ENM_EroPhaseControl[3]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 0,
					speedAnimation = 0f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.65f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		}
		return result;
	}

	private void PRINCESS_BOSS_SPECIAL_GRABS(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			Vector2 targetPosition = base.transform.position;
			targetPosition.y -= 0.75f;
			GC.InstantiateVFX(GameCore.VisualEffect.PrincessDress, targetPosition, "EroAnimation:Princess");
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = PRINCESS_BOSS_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("IcePEro01-1Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 1;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 2:
				currentAN.Play("IcePEro04-01Grab1");
				refMainEnemyScript.eroCore.minInternalsPhase = 4;
				refMainEnemyScript.eroCore.maxInternalsPhase = 8;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 3:
				currentAN.Play("IcePEro03-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 4:
				currentAN.Play("IcePEro02-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 4;
				refMainEnemyScript.eroCore.maxInternalsPhase = 7;
				if (GC.doAllVoreScenesAreFatal)
				{
					GC.currentEnemyGrabbingIsEroFatal = true;
				}
				else
				{
					GC.currentEnemyGrabbingIsEroFatal = false;
				}
				break;
			}
			ignoreEroEndLogicAfterSpecial = false;
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] PRINCESS_BOSS_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		switch (eroID)
		{
		default:
			result = GC.defaultEroPhases;
			break;
		case 1:
		case 3:
			result = new ENM_EroPhaseControl[5]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.65f,
					frequencyOfDmg = 3,
					divisionOfDmg = 16
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.85f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 1.1f,
					frequencyOfDmg = 4,
					divisionOfDmg = 13
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 12
				}
			};
			break;
		case 4:
			result = new ENM_EroPhaseControl[9]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.65f,
					frequencyOfDmg = 3,
					divisionOfDmg = 16
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.9f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.8f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 1f,
					frequencyOfDmg = 4,
					divisionOfDmg = 13
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 12
				},
				new ENM_EroPhaseControl
				{
					runLoops = 9999999,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		case 2:
			result = new ENM_EroPhaseControl[9]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.65f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 4,
					speedAnimation = 0.65f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 4,
					speedAnimation = 0.65f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 4,
					speedAnimation = 0.65f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.85f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 1.1f,
					frequencyOfDmg = 4,
					divisionOfDmg = 13
				},
				new ENM_EroPhaseControl
				{
					runLoops = 4,
					speedAnimation = 0.65f,
					frequencyOfDmg = 1,
					divisionOfDmg = 12
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 12
				}
			};
			break;
		}
		return result;
	}

	private void CENTAUR_GIRL_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = CENTAUR_GIRL_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("CTG-Ero1-Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 5;
				refMainEnemyScript.eroCore.maxInternalsPhase = 7;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			case 2:
				currentAN.Play("CTG-Ero2-Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 1;
				refMainEnemyScript.eroCore.maxInternalsPhase = 5;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] CENTAUR_GIRL_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		switch (eroID)
		{
		default:
			result = GC.defaultEroPhases;
			break;
		case 1:
			result = new ENM_EroPhaseControl[9]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.65f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.95f,
					frequencyOfDmg = 2,
					divisionOfDmg = 13
				},
				new ENM_EroPhaseControl
				{
					runLoops = 6,
					speedAnimation = 0.5f,
					frequencyOfDmg = 2,
					divisionOfDmg = 13
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.8f,
					frequencyOfDmg = 2,
					divisionOfDmg = 13
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 1f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.5f,
					frequencyOfDmg = 4,
					divisionOfDmg = 12
				},
				new ENM_EroPhaseControl
				{
					runLoops = 15,
					speedAnimation = 0.8f,
					frequencyOfDmg = 4,
					divisionOfDmg = 11
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 1.2f,
					frequencyOfDmg = 4,
					divisionOfDmg = 10
				},
				new ENM_EroPhaseControl
				{
					runLoops = 99999,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		case 2:
			result = new ENM_EroPhaseControl[6]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.65f,
					frequencyOfDmg = 4,
					divisionOfDmg = 13
				},
				new ENM_EroPhaseControl
				{
					runLoops = 15,
					speedAnimation = 0.85f,
					frequencyOfDmg = 4,
					divisionOfDmg = 12
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 1.2f,
					frequencyOfDmg = 4,
					divisionOfDmg = 11
				},
				new ENM_EroPhaseControl
				{
					runLoops = 6,
					speedAnimation = 0.5f,
					frequencyOfDmg = 2,
					divisionOfDmg = 10
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.5f,
					frequencyOfDmg = 4,
					divisionOfDmg = 10
				}
			};
			break;
		}
		return result;
	}

	private void CAT_GIRL_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = CAT_GIRL_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("CAT-Ero1-01Grab");
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 2:
				currentAN.Play("CAT-Ero2-01Grab");
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 3:
				currentAN.Play("CAT-Ero3-01Grab");
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] CAT_GIRL_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] array;
		if (eroID != 3)
		{
			array = GC.defaultEroPhases;
		}
		else
		{
			array = new ENM_EroPhaseControl[6];
			for (int i = 0; i < GC.defaultEroPhases.Length; i++)
			{
				array[i] = GC.defaultEroPhases[i];
			}
			array[5] = new ENM_EroPhaseControl
			{
				runLoops = 99999,
				speedAnimation = 0.6f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
		}
		return array;
	}

	private void GHOST_BUTLER_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("GrabFootjob");
				break;
			case 2:
				currentAN.Play("GrabCowgirl");
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private void LATEX_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		if (setState)
		{
			bool flag = false;
			if (refMainEnemyScript.isGalleryView && (currentAN.GetInteger("eroVariantID") == 9 || currentAN.GetInteger("eroVariantID") == 10))
			{
				flag = true;
			}
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			if (((bool)GC && GC.isPlayerFainted && GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.LatexBodySuit)) | flag)
			{
				HACK_LATEX_GRAB_FOR_BRUTE();
				currentAN.Play("Grab-Latex");
			}
			else
			{
				currentAN.Play("Ero-Grab");
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private void HACK_LATEX_GRAB_FOR_BRUTE()
	{
		if (refMainEnemyScript.currentEnemyType == GameCore.EnemyType.PrisonBrute && currentAN.GetInteger("eroVariantID") == 9)
		{
			refMainEnemyScript.eroCore.basicEroInternals = refMainEnemyScript.GetInternalsObjVariantByID(1);
			addInternalShotsAN = refMainEnemyScript.eroCore.basicEroInternals.GetComponent<Animator>();
			addInternalShotsAN.SetInteger("eroPhase", 0);
			UpdateAnimInformationOnInternalShot(0, 1f);
			applySync = true;
		}
	}

	private void PAINTINGEVIL_SPECIAL_GRABS(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentEroControl = new ENM_EroPhaseControl[6];
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = PAINTINGEVIL_GetPhaseControl();
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			currentAN.Play("Ero-Grab");
			switch (integer)
			{
			case 1:
			{
				Vector2 targetPosition = base.transform.position;
				targetPosition.y++;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				break;
			}
			case 2:
			{
				Vector2 targetPosition = base.transform.position;
				targetPosition.y++;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				targetPosition.y--;
				targetPosition.x++;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				break;
			}
			case 3:
			{
				Vector2 targetPosition = base.transform.position;
				targetPosition.y++;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				targetPosition.y--;
				targetPosition.x++;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				targetPosition.y--;
				targetPosition.x--;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				break;
			}
			case 4:
			{
				Vector2 targetPosition = base.transform.position;
				targetPosition.y++;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				targetPosition.y--;
				targetPosition.x++;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				targetPosition.y--;
				targetPosition.x--;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				targetPosition.y++;
				targetPosition.x--;
				GC.InstantiateVFX(GameCore.VisualEffect.PortraitEvil, targetPosition, "EroAnimation:PAINTINGEVIL_SPECIAL_GRABS");
				break;
			}
			}
		}
		else
		{
			integer++;
			if (integer > 4)
			{
				integer = 0;
			}
			currentAN.SetInteger("eroVariantID", integer);
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] PAINTINGEVIL_GetPhaseControl()
	{
		return new ENM_EroPhaseControl[7]
		{
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.6f,
				frequencyOfDmg = 3,
				divisionOfDmg = 14
			},
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.65f,
				frequencyOfDmg = 2,
				divisionOfDmg = 13
			},
			new ENM_EroPhaseControl
			{
				runLoops = 15,
				speedAnimation = 0.85f,
				frequencyOfDmg = 2,
				divisionOfDmg = 12
			},
			new ENM_EroPhaseControl
			{
				runLoops = 18,
				speedAnimation = 1.2f,
				frequencyOfDmg = 1,
				divisionOfDmg = 11
			},
			new ENM_EroPhaseControl
			{
				runLoops = 8,
				speedAnimation = 0.5f,
				frequencyOfDmg = 1,
				divisionOfDmg = 10
			},
			new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 0.5f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 999,
				speedAnimation = 0.5f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			}
		};
	}

	private void PAINTINGEVIL_PORTRAIT()
	{
		int num = currentAN.GetInteger("eroVariantID");
		if (num == 0)
		{
			num = GC.MATH_RNG_INT(1, 4);
		}
		UnityEngine.Object.Instantiate(refMainEnemyScript.GetMiscGameObject(num), base.transform.position, Quaternion.identity);
	}

	private void DOG_BOY_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			currentAN.Play("Ero-GrabA 1");
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private void WORMVORE_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = WORMVORE_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("GrabA");
				break;
			case 2:
				currentAN.Play("GrabB");
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] WORMVORE_GetPhaseControl(int eroID)
	{
		return new ENM_EroPhaseControl[7]
		{
			new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 0.6f,
				frequencyOfDmg = 3,
				divisionOfDmg = 15
			},
			new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 0.7f,
				frequencyOfDmg = 3,
				divisionOfDmg = 15
			},
			new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 0.4f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 14,
				speedAnimation = 0.8f,
				frequencyOfDmg = 2,
				divisionOfDmg = 15
			},
			new ENM_EroPhaseControl
			{
				runLoops = 18,
				speedAnimation = 1f,
				frequencyOfDmg = 1,
				divisionOfDmg = 15
			},
			new ENM_EroPhaseControl
			{
				runLoops = 20,
				speedAnimation = 1.1f,
				frequencyOfDmg = 1,
				divisionOfDmg = 6
			},
			new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 0.2f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			}
		};
	}

	private void MEDUSA_SPECIAL_GRABS(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			_ = (Vector2)base.transform.position;
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = MEDUSA_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("Ero1-Grab");
				break;
			case 2:
				currentAN.Play("Ero2-Grab");
				break;
			case 3:
				currentAN.Play("Ero3-Grab");
				break;
			case 4:
				currentAN.Play("Ero4-Grab");
				break;
			}
			ignoreEroEndLogicAfterSpecial = false;
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private void MEDUSA_SPAWN_STATUE()
	{
		Vector3 position = new Vector3(-0.5f, 0f, 0f);
		position += base.transform.position;
		UnityEngine.Object.Instantiate(refMainEnemyScript.GetMiscGameObject(currentAN.GetInteger("eroVariantID")), position, Quaternion.identity, GC.refOBJEffectsParent.transform);
		EndEroLogic();
	}

	private void MEDUSA_SPAWN_DISCHARGE()
	{
		Vector3 vector = default;
		int index = 4;
		vector.y = -0.75f;
		if (refMainEnemyScript.GetEnemyFacing() == 1f)
		{
			vector.x = -1.5f;
		}
		else
		{
			vector.x = 1.5f;
		}
		vector += base.transform.position;
		for (int i = 0; i < 6; i++)
		{
			if (i > 0)
			{
				index = 5;
			}
			UnityEngine.Object.Instantiate(refMainEnemyScript.GetMiscGameObject(index), vector, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>().specialForce = new Vector2(2f, GC.MATH_RNG_FLOAT(1f, 3f));
		}
		GC.InstantiateVFX(GameCore.VisualEffect.Spit, vector, "Medusa:EroScript");
		EndEroLogic();
	}

	private ENM_EroPhaseControl[] MEDUSA_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		if (eroID != 4)
		{
			refMainEnemyScript.eroCore.minInternalsPhase = 0;
			refMainEnemyScript.eroCore.maxInternalsPhase = 0;
			result = new ENM_EroPhaseControl[6]
			{
				GC.defaultEroPhases[0],
				GC.defaultEroPhases[1],
				GC.defaultEroPhases[2],
				GC.defaultEroPhases[3],
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.4f,
					divisionOfDmg = 0,
					frequencyOfDmg = 0
				},
				GC.defaultEroPhases[4]
			};
		}
		else
		{
			refMainEnemyScript.eroCore.minInternalsPhase = 7;
			refMainEnemyScript.eroCore.maxInternalsPhase = 9;
			result = new ENM_EroPhaseControl[12]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.65f,
					frequencyOfDmg = 3,
					divisionOfDmg = 30
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.8f,
					frequencyOfDmg = 2,
					divisionOfDmg = 50
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 1f,
					frequencyOfDmg = 2,
					divisionOfDmg = 50
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.9f,
					frequencyOfDmg = 2,
					divisionOfDmg = 50
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.8f,
					frequencyOfDmg = 2,
					divisionOfDmg = 50
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.5f,
					frequencyOfDmg = 3,
					divisionOfDmg = 20
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 18
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.8f,
					frequencyOfDmg = 2,
					divisionOfDmg = 17
				},
				new ENM_EroPhaseControl
				{
					runLoops = 22,
					speedAnimation = 1.1f,
					frequencyOfDmg = 2,
					divisionOfDmg = 16
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
		}
		return result;
	}

	private void SLIME_GIRL_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = SLIME_GIRL_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("SLG-Ero1-Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 1;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 2:
				currentAN.Play("SLG-Ero2-Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 0;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			case 3:
				currentAN.Play("SLG-Ero3-Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 0;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] SLIME_GIRL_GetPhaseControl(int eroID)
	{
		return (eroID == 1 || eroID != 2) ? GC.defaultEroPhases : new ENM_EroPhaseControl[9]
		{
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.6f,
				frequencyOfDmg = 4,
				divisionOfDmg = 35
			},
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.7f,
				frequencyOfDmg = 3,
				divisionOfDmg = 34
			},
			new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 0.9f,
				frequencyOfDmg = 4,
				divisionOfDmg = 33
			},
			new ENM_EroPhaseControl
			{
				runLoops = 24,
				speedAnimation = 1.1f,
				frequencyOfDmg = 3,
				divisionOfDmg = 32
			},
			new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.6f,
				frequencyOfDmg = 1,
				divisionOfDmg = 30
			},
			new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.6f,
				frequencyOfDmg = 1,
				divisionOfDmg = 30
			},
			new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.6f,
				frequencyOfDmg = 1,
				divisionOfDmg = 30
			},
			new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.6f,
				frequencyOfDmg = 1,
				divisionOfDmg = 30
			},
			new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 0.4f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			}
		};
	}

	private void WITCH_BOSS_SPECIAL_GRABS(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			_ = (Vector2)base.transform.position;
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = WITCH_BOSS_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("W_Ero01-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 0;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 2:
				currentAN.Play("W-Ero02-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 3;
				refMainEnemyScript.eroCore.maxInternalsPhase = 6;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 3:
				currentAN.Play("W-Ero03-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 8;
				refMainEnemyScript.eroCore.maxInternalsPhase = 10;
				if (GC.doAllVoreScenesAreFatal)
				{
					GC.currentEnemyGrabbingIsEroFatal = true;
				}
				else
				{
					GC.currentEnemyGrabbingIsEroFatal = false;
				}
				break;
			}
			ignoreEroEndLogicAfterSpecial = false;
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] WITCH_BOSS_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] array;
		switch (eroID)
		{
		default:
			array = GC.defaultEroPhases;
			array[0].frequencyOfDmg = 1;
			array[0].divisionOfDmg = 17;
			break;
		case 2:
			array = new ENM_EroPhaseControl[7]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.65f,
					frequencyOfDmg = 1,
					divisionOfDmg = 18
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.8f,
					frequencyOfDmg = 2,
					divisionOfDmg = 17
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.85f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 1.1f,
					frequencyOfDmg = 4,
					divisionOfDmg = 13
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 12
				}
			};
			break;
		case 3:
			array = new ENM_EroPhaseControl[12]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.65f,
					frequencyOfDmg = 1,
					divisionOfDmg = 20
				},
				new ENM_EroPhaseControl
				{
					runLoops = 4,
					speedAnimation = 0.7f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.85f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 1.1f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.6f,
					frequencyOfDmg = 5,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.6f,
					frequencyOfDmg = 3,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 22,
					speedAnimation = 0.85f,
					frequencyOfDmg = 2,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 24,
					speedAnimation = 1.1f,
					frequencyOfDmg = 2,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 9999999,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		}
		return array;
	}

	private void BLUEFOX_BOSS_SPECIAL_GRABS(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			_ = (Vector2)base.transform.position;
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = BLUEFOX_BOSS_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("BLF-Ero01-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 4;
				refMainEnemyScript.eroCore.maxInternalsPhase = 7;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 2:
				currentAN.Play("BLF-Ero02-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 2;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			case 3:
				currentAN.Play("BLF-Ero03-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 2;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			}
			ignoreEroEndLogicAfterSpecial = false;
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] BLUEFOX_BOSS_GetPhaseControl(int eroID)
	{
		return ((uint)(eroID - 2) > 1u) ? new ENM_EroPhaseControl[8]
		{
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.9f,
				frequencyOfDmg = 1,
				divisionOfDmg = 35
			},
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.8f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 0.9f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 0.5f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 18,
				speedAnimation = 0.9f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 22,
				speedAnimation = 1.25f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 26,
				speedAnimation = 1.5f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.6f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			}
		} : new ENM_EroPhaseControl[6]
		{
			new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 0.65f,
				frequencyOfDmg = 2,
				divisionOfDmg = 35
			},
			new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 0.5f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 18,
				speedAnimation = 0.7f,
				frequencyOfDmg = 4,
				divisionOfDmg = 34
			},
			new ENM_EroPhaseControl
			{
				runLoops = 24,
				speedAnimation = 0.9f,
				frequencyOfDmg = 3,
				divisionOfDmg = 33
			},
			new ENM_EroPhaseControl
			{
				runLoops = 30,
				speedAnimation = 1.1f,
				frequencyOfDmg = 3,
				divisionOfDmg = 32
			},
			new ENM_EroPhaseControl
			{
				runLoops = 999999,
				speedAnimation = 0.55f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			}
		};
	}

	private void SWAMP_BOSS_SPECIAL_GRABS(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			_ = (Vector2)base.transform.position;
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = SWAMP_BOSS_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("BossSwamp-E1-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 0;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 2:
				currentAN.Play("SwampBoss-2-01Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 0;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			}
			ignoreEroEndLogicAfterSpecial = false;
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] SWAMP_BOSS_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] defaultEroPhases;
		return (eroID != 2) ? (defaultEroPhases = GC.defaultEroPhases) : new ENM_EroPhaseControl[10]
		{
			new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 0.7f,
				frequencyOfDmg = 1,
				divisionOfDmg = 35
			},
			new ENM_EroPhaseControl
			{
				runLoops = 18,
				speedAnimation = 0.9f,
				frequencyOfDmg = 1,
				divisionOfDmg = 34
			},
			new ENM_EroPhaseControl
			{
				runLoops = 24,
				speedAnimation = 1.1f,
				frequencyOfDmg = 1,
				divisionOfDmg = 33
			},
			new ENM_EroPhaseControl
			{
				runLoops = 999999,
				speedAnimation = 0.8f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 111111,
				speedAnimation = 0.1f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 999999,
				speedAnimation = 0.65f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 8,
				speedAnimation = 0.6f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.9f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 1.2f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 999999,
				speedAnimation = 0.5f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			}
		};
	}

	private void HACK_SWAMP_BOSS_REEVALUATE_CONTROL(int eroID)
	{
		currentEroControl = SWAMP_BOSS_GetPhaseControl(eroID);
	}

	private void BAT_GIRL_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = BAT_GIRL_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			currentAN.Play("Ero-Grab");
			switch (integer)
			{
			case 1:
				refMainEnemyScript.eroCore.minInternalsPhase = 1;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				break;
			case 2:
				refMainEnemyScript.eroCore.minInternalsPhase = 1;
				refMainEnemyScript.eroCore.maxInternalsPhase = 3;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] BAT_GIRL_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		if (eroID == 1)
		{
			result = new ENM_EroPhaseControl[5]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.65f,
					frequencyOfDmg = 1,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.8f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 1.2f,
					frequencyOfDmg = 3,
					divisionOfDmg = 12
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.7f,
					frequencyOfDmg = 2,
					divisionOfDmg = 11
				}
			};
		}
		else
		{
			_ = 2;
			result = GC.defaultEroPhases;
		}
		return result;
	}

	private void DWARF_GIRL_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = DWARF_GIRL_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("Ero1-VoreGrab");
				refMainEnemyScript.eroCore.minInternalsPhase = 4;
				refMainEnemyScript.eroCore.maxInternalsPhase = 6;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			case 2:
				currentAN.Play("Ero2-LesbianDildo");
				refMainEnemyScript.eroCore.minInternalsPhase = 1;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] DWARF_GIRL_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		switch (eroID)
		{
		default:
			result = GC.defaultEroPhases;
			break;
		case 1:
			result = new ENM_EroPhaseControl[8]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 8
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.75f,
					frequencyOfDmg = 2,
					divisionOfDmg = 8
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.9f,
					frequencyOfDmg = 2,
					divisionOfDmg = 7
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.7f,
					frequencyOfDmg = 1,
					divisionOfDmg = 6
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.6f,
					frequencyOfDmg = 1,
					divisionOfDmg = 5
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.6f,
					frequencyOfDmg = 1,
					divisionOfDmg = 4
				},
				new ENM_EroPhaseControl
				{
					runLoops = 9999999,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		case 2:
			result = GC.defaultEroPhases;
			break;
		}
		return result;
	}

	private void MUSHROOM_FUTAGIRL_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = MUSHROOM_FUTAGIRL_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("Ero-Grab1");
				refMainEnemyScript.eroCore.minInternalsPhase = 3;
				refMainEnemyScript.eroCore.maxInternalsPhase = 5;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			case 2:
				currentAN.Play("Ero-Grab2");
				refMainEnemyScript.eroCore.minInternalsPhase = 1;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] MUSHROOM_FUTAGIRL_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		switch (eroID)
		{
		default:
			result = GC.defaultEroPhases;
			break;
		case 1:
			result = new ENM_EroPhaseControl[8]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.7f,
					frequencyOfDmg = 2,
					divisionOfDmg = 10
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 9
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 8,
					speedAnimation = 0.7f,
					frequencyOfDmg = 2,
					divisionOfDmg = 8
				},
				new ENM_EroPhaseControl
				{
					runLoops = 9,
					speedAnimation = 0.95f,
					frequencyOfDmg = 2,
					divisionOfDmg = 7
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 1.15f,
					frequencyOfDmg = 1,
					divisionOfDmg = 7
				},
				new ENM_EroPhaseControl
				{
					runLoops = 4,
					speedAnimation = 0.6f,
					frequencyOfDmg = 1,
					divisionOfDmg = 1
				},
				new ENM_EroPhaseControl
				{
					runLoops = 9999999,
					speedAnimation = 0.4f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		case 2:
			result = GC.defaultEroPhases;
			break;
		}
		return result;
	}

	private void DARKDAMSEL_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = DARKDAMSEL_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("Ero-Grab1");
				refMainEnemyScript.eroCore.minInternalsPhase = 2;
				refMainEnemyScript.eroCore.maxInternalsPhase = 5;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 2:
				currentAN.Play("Ero-Grab2");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 0;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] DARKDAMSEL_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		switch (eroID)
		{
		default:
			result = GC.defaultEroPhases;
			break;
		case 1:
			result = new ENM_EroPhaseControl[6]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.6f,
					frequencyOfDmg = 2,
					divisionOfDmg = 30
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 29
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 0.9f,
					frequencyOfDmg = 3,
					divisionOfDmg = 28
				},
				new ENM_EroPhaseControl
				{
					runLoops = 26,
					speedAnimation = 1.2f,
					frequencyOfDmg = 3,
					divisionOfDmg = 27
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.5f,
					frequencyOfDmg = 2,
					divisionOfDmg = 26
				}
			};
			break;
		case 2:
			result = GC.defaultEroPhases;
			break;
		}
		return result;
	}

	private void SWAMPFAIRY_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = SWAMPFAIRY_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
			case 2:
				currentAN.Play("Ero-Grab");
				refMainEnemyScript.eroCore.minInternalsPhase = 3;
				refMainEnemyScript.eroCore.maxInternalsPhase = 6;
				GC.currentEnemyGrabbingIsEroFatal = false;
				break;
			case 3:
				currentAN.Play("Ero-Grab3");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 0;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] SWAMPFAIRY_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		switch (eroID)
		{
		default:
			result = GC.defaultEroPhases;
			break;
		case 1:
		case 2:
			result = new ENM_EroPhaseControl[7]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 6,
					speedAnimation = 0.8f,
					frequencyOfDmg = 5,
					divisionOfDmg = 20
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.7f,
					frequencyOfDmg = 5,
					divisionOfDmg = 20
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.7f,
					frequencyOfDmg = 2,
					divisionOfDmg = 20
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 1f,
					frequencyOfDmg = 3,
					divisionOfDmg = 19
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 1.35f,
					frequencyOfDmg = 4,
					divisionOfDmg = 19
				},
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		case 3:
			result = new ENM_EroPhaseControl[6]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.65f,
					frequencyOfDmg = 3,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 15
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 0.9f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 24,
					speedAnimation = 1.2f,
					frequencyOfDmg = 4,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.5f,
					frequencyOfDmg = 3,
					divisionOfDmg = 14
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 1f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		}
		return result;
	}

	private void CENTAURFUTADRUID_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = CENTAURFUTADRUID_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("Ero-Grab1");
				refMainEnemyScript.eroCore.minInternalsPhase = 0;
				refMainEnemyScript.eroCore.maxInternalsPhase = 0;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			case 2:
				currentAN.Play("Ero-Grab2");
				refMainEnemyScript.eroCore.minInternalsPhase = 4;
				refMainEnemyScript.eroCore.maxInternalsPhase = 6;
				GC.currentEnemyGrabbingIsEroFatal = true;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] CENTAURFUTADRUID_GetPhaseControl(int eroID)
	{
		ENM_EroPhaseControl[] result;
		switch (eroID)
		{
		default:
			result = GC.defaultEroPhases;
			break;
		case 1:
			result = new ENM_EroPhaseControl[11]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.7f,
					frequencyOfDmg = 1,
					divisionOfDmg = 22
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.6f,
					frequencyOfDmg = 3,
					divisionOfDmg = 21
				},
				new ENM_EroPhaseControl
				{
					runLoops = 24,
					speedAnimation = 0.85f,
					frequencyOfDmg = 3,
					divisionOfDmg = 20
				},
				new ENM_EroPhaseControl
				{
					runLoops = 30,
					speedAnimation = 1.1f,
					frequencyOfDmg = 3,
					divisionOfDmg = 19
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 111111,
					speedAnimation = 0.1f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.6f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.7f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.9f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 22,
					speedAnimation = 1.1f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 999999,
					speedAnimation = 0.7f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		case 2:
			result = new ENM_EroPhaseControl[9]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.65f,
					frequencyOfDmg = 4,
					divisionOfDmg = 30
				},
				new ENM_EroPhaseControl
				{
					runLoops = 14,
					speedAnimation = 0.8f,
					frequencyOfDmg = 4,
					divisionOfDmg = 30
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 1f,
					frequencyOfDmg = 3,
					divisionOfDmg = 29
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 18,
					speedAnimation = 0.6f,
					frequencyOfDmg = 4,
					divisionOfDmg = 28
				},
				new ENM_EroPhaseControl
				{
					runLoops = 22,
					speedAnimation = 0.85f,
					frequencyOfDmg = 4,
					divisionOfDmg = 27
				},
				new ENM_EroPhaseControl
				{
					runLoops = 26,
					speedAnimation = 1.15f,
					frequencyOfDmg = 3,
					divisionOfDmg = 27
				},
				new ENM_EroPhaseControl
				{
					runLoops = 1,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				},
				new ENM_EroPhaseControl
				{
					runLoops = 999999,
					speedAnimation = 0.5f,
					frequencyOfDmg = 0,
					divisionOfDmg = 0
				}
			};
			break;
		}
		return result;
	}

	private void SWAMPGOLEM_SPECIAL_GRAB(bool setState, ref bool isReturnToIdle)
	{
		int integer = currentAN.GetInteger("eroVariantID");
		if (setState)
		{
			currentAN.SetInteger("eroPhase", 0);
			_eroPhaseAnimator = 0;
			currentAN.SetBool("isGrabbing", value: true);
			currentEroControl = SWAMPGOLEM_GetPhaseControl(integer);
			currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
			switch (integer)
			{
			case 1:
				currentAN.Play("Ero-A-start");
				refMainEnemyScript.eroCore.minInternalsPhase = 6;
				refMainEnemyScript.eroCore.maxInternalsPhase = 9;
				break;
			case 2:
				currentAN.Play("Ero-B-start");
				refMainEnemyScript.eroCore.minInternalsPhase = 1;
				refMainEnemyScript.eroCore.maxInternalsPhase = 4;
				break;
			}
		}
		else
		{
			isReturnToIdle = true;
		}
	}

	private ENM_EroPhaseControl[] SWAMPGOLEM_GetPhaseControl(int eroID)
	{
		return (eroID != 1) ? GC.defaultEroPhases : new ENM_EroPhaseControl[10]
		{
			new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.6f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 8,
				speedAnimation = 0.8f,
				frequencyOfDmg = 6,
				divisionOfDmg = 32
			},
			new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.6f,
				frequencyOfDmg = 3,
				divisionOfDmg = 30
			},
			new ENM_EroPhaseControl
			{
				runLoops = 8,
				speedAnimation = 0.8f,
				frequencyOfDmg = 6,
				divisionOfDmg = 32
			},
			new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.6f,
				frequencyOfDmg = 3,
				divisionOfDmg = 30
			},
			new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 1f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			},
			new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.7f,
				frequencyOfDmg = 4,
				divisionOfDmg = 30
			},
			new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 0.9f,
				frequencyOfDmg = 3,
				divisionOfDmg = 29
			},
			new ENM_EroPhaseControl
			{
				runLoops = 22,
				speedAnimation = 1.2f,
				frequencyOfDmg = 2,
				divisionOfDmg = 28
			},
			new ENM_EroPhaseControl
			{
				runLoops = 10,
				speedAnimation = 0.5f,
				frequencyOfDmg = 2,
				divisionOfDmg = 27
			}
		};
	}

	public void SetupSpecialReferences(EroAnimation senderScript, EnemyGeneralLogic refMain, Animator subAnim, int localTouchDamage, Vector2 dmgbarOffset, string animName, GameObject playerOBJREF)
	{
		RefreshInitialReferences();
		ParentEroScript = senderScript;
		refMainEnemyScript = refMain;
		currentAN = subAnim;
		baseTouchDamage = localTouchDamage;
		offsetPlayerDamageDisplay = dmgbarOffset;
		tmpObjPlayerReference = playerOBJREF;
		if (!InitialCheckEroValidity())
		{
			DisplayError(4);
			return;
		}
		StartAnimatorVariablesSpecial(animName);
		DoUniqueBehavioursOnEroStartSpecial();
	}

	private void StartAnimatorVariablesSpecial(string animNAME)
	{
		currentAN.SetInteger("eroPhase", 0);
		_eroPhaseAnimator = 0;
		currentAN.SetFloat("speedAnimation", currentEroControl[0].speedAnimation);
		UpdateSpeedAnimationOnParent(currentEroControl[0].speedAnimation);
		currentAN.SetBool("isGrabbing", value: true);
		currentAN.SetInteger("varAnim", 1);
		if (animNAME != "")
		{
			currentAN.Play(animNAME);
		}
	}

	private void UpdateSpeedAnimationOnParent(float speed)
	{
		refMainEnemyScript.SetAnimSpeed(speed);
	}

	private void CleanSubSpecialParameters()
	{
		CleanAnimMainParameters();
		tmpObjPlayerReference = null;
		refMainEnemyScript = null;
		currentAN = null;
		playerScript = null;
		currentEroControl = null;
		intGetCurrentEroPhase = 0;
		baseTouchDamage = 0;
		dmgOfEro = 0;
		offsetPlayerDamageDisplay = Vector2.zero;
		ParentEroScript.EndEroLogic();
		ParentEroScript = null;
		refMainEnemyScript = null;
	}

	private void CleanAnimMainParameters()
	{
		dmgOfEro = 0;
		countEroLoop = 0;
		intGetCurrentEroPhase = 0;
		currentAN.SetInteger("eroPhase", 0);
		_eroPhaseAnimator = 0;
		currentAN.SetFloat("speedAnimation", 1f);
		currentAN.SetBool("isGrabbing", value: false);
		currentAN.SetBool("isEro", value: false);
	}

	private void DoUniqueBehavioursOnEroStartSpecial()
	{
		GameCore.EnemyType currentEnemyType = refMainEnemyScript.currentEnemyType;
		if (currentEnemyType == GameCore.EnemyType.FlowerBlossom)
		{
			ResetSelfLocalTransform();
			InvokeRepeating("UNIQUE_FlowerBlossomSinkPlayer", 1f, 0.5f);
		}
	}

	private void DoUniqueBehavioursOnGrabEnd()
	{
		Vector3 vector = Vector2.zero;
		switch (refMainEnemyScript.currentEnemyType)
		{
		default:
			vector = refMainEnemyScript.GetEnemyCenterPos();
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, vector, "OnGrabEndTransitionSpawnSmokes");
			break;
		case GameCore.EnemyType.FlowerBlossom:
			vector = refMainEnemyScript.gameObject.transform.position;
			vector.y += 0.75f;
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, vector, "EROANIMATION:DoUniqueBehavioursOnGrabEnd");
			PlayEroSound("piston3");
			CancelInvoke("UNIQUE_FlowerBlossomSinkPlayer");
			InvokeRepeating("UNIQUE_FlowerBlossomBeginMainEro", 0.1f, 10f);
			base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 9;
			break;
		case GameCore.EnemyType.MushroomGirl:
			vector = refMainEnemyScript.gameObject.transform.position;
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, vector, "EROANIMATION:DoUniqueBehavioursOnGrabEnd");
			GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			GC.PlaySFXEro(GameCore.EroSound.Cum4, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			break;
		case GameCore.EnemyType.Goblin:
		case GameCore.EnemyType.Goblina:
			if (_eroIDVariant <= 1)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "enemyGoblin");
			}
			break;
		case GameCore.EnemyType.Catgirl:
			vector = refMainEnemyScript.GetEnemyCenterPos();
			vector.y -= 0.25f;
			if (_eroIDVariant == 3)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, vector, "EroAnimCatGirl");
				PlayEroSound("swallow4deep");
				refMainEnemyScript.PlayMonsterSoundsFromAnim("nyaOther");
			}
			break;
		}
	}

	public void DoUniqueBehavioursOnEroPhaseChange(string overrideSelf)
	{
		Vector2 targetPosition = base.transform.position;
		switch (refMainEnemyScript.currentEnemyType)
		{
		case GameCore.EnemyType.FlowerBlossom:
			if (intGetCurrentEroPhase >= 2 && intGetCurrentEroPhase <= 4)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.SporeTransform, base.transform.position, "EROANIMATION:DoUniqueBehavioursOnEroPhaseChange");
				PlayEroSound("odd2");
			}
			else if (overrideSelf == "override")
			{
				PlayEroSound("odd3");
				GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "EROANIMATION:DoUniqueBehavioursOnEroPhaseChange");
				Vector3 position = ParentEroScript.transform.position;
				position.y += 0.85f;
				ParentEroScript.transform.position = position;
				base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 4;
			}
			break;
		case GameCore.EnemyType.PaintingEvil:
			if (intGetCurrentEroPhase == 6 && GC.isPlayerFainted)
			{
				PAINTINGEVIL_PORTRAIT();
			}
			break;
		case GameCore.EnemyType.LatexMaiden:
			if (intGetCurrentEroPhase == 7 && GC.isPlayerFainted)
			{
				GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.LatexBodySuit, 99999f, "EroAnimation:DoUniqueBehaviorOnPhaseChange");
			}
			break;
		case GameCore.EnemyType.WormOverGround:
			if (currentAN.GetInteger("eroPhase") == 3)
			{
				tmpOffset3.y--;
			}
			break;
		case GameCore.EnemyType.Bot1:
		case GameCore.EnemyType.Bot2:
		case GameCore.EnemyType.Bot3:
			if (overrideSelf == "GrabSmoke")
			{
				GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "EroAnimation");
			}
			break;
		case GameCore.EnemyType.Medusa:
			if (intGetCurrentEroPhase < 10)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.Petrified2, base.transform.position, "EroAnimation:DoUniqueBehavioursOnEroPhaseChange");
			}
			if (intGetCurrentEroPhase == 2)
			{
				GC.DisplaySystemMessageUI(0, 114, 5f, "", isFlavorCentralMessage: true);
			}
			if (intGetCurrentEroPhase == 5 && _eroIDVariant != 4)
			{
				MEDUSA_SPAWN_STATUE();
				GC.DisplaySystemMessageUI(0, 115, 10f, "", isFlavorCentralMessage: true);
			}
			if (intGetCurrentEroPhase == 11 && _eroIDVariant == 4)
			{
				MEDUSA_SPAWN_DISCHARGE();
				GC.DisplaySystemMessageUI(0, 115, 10f, "", isFlavorCentralMessage: true);
			}
			break;
		case GameCore.EnemyType.SlimeGirl:
			if ((bool)currentAN && currentAN.GetInteger("eroVariantID") == 2 && intGetCurrentEroPhase == 4)
			{
				GC.DisplaySystemMessageUI(0, 120, 5f, "", isFlavorCentralMessage: true);
			}
			if ((bool)currentAN && currentAN.GetInteger("eroVariantID") == 2 && intGetCurrentEroPhase == 8)
			{
				if (GC.CompareFetishToggle(GameCore.FetishType.Vore))
				{
					GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.SlimeMelt, 99999f, "EroAnimation:SlimeGirl");
					GC.DisplaySystemMessageUI(0, 122, 5f, "", isFlavorCentralMessage: true);
				}
				else
				{
					GC.DisplaySystemMessageUI(0, 121, 5f, "", isFlavorCentralMessage: true);
				}
			}
			break;
		case GameCore.EnemyType.BossSwampSlime:
			if ((bool)currentAN && currentAN.GetInteger("eroVariantID") == 2 && intGetCurrentEroPhase == 3)
			{
				GC.DisplaySystemMessageUI(0, 119, 10f, "", isFlavorCentralMessage: true);
			}
			break;
		case GameCore.EnemyType.BigInsect:
			if (GC.isPlayerFainted && !boolGeneric)
			{
				boolGeneric = true;
				for (int i = 0; i < 7; i++)
				{
					currentEroControl[i].runLoops /= 2;
					if (currentEroControl[i].runLoops <= 0)
					{
						currentEroControl[i].runLoops = 1;
					}
				}
			}
			if (intGetCurrentEroPhase == 8)
			{
				EroLoopCutToEnd();
			}
			break;
		case GameCore.EnemyType.FutaDruidCentauress:
			if (_eroIDVariant == 1 && intGetCurrentEroPhase == 5)
			{
				EroLoopCutToEnd();
			}
			if (_eroIDVariant == 2)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "EROANIMATION:DoUniqueBehavioursOnEroPhaseChange");
				if (intGetCurrentEroPhase == 8)
				{
					GC.InstantiateVFX(GameCore.VisualEffect.CreamExplosion, targetPosition, "EroAnim:Succubus");
				}
			}
			break;
		case GameCore.EnemyType.Kobold:
			if (intGetCurrentEroPhase == 5)
			{
				EroLoopCutToEnd();
			}
			if (GC.isRunningBadEndLogic && intGetCurrentEroPhase >= 12 && intGetCurrentEroPhase <= 18)
			{
				targetPosition = base.transform.position;
				targetPosition.y--;
				if (intGetCurrentEroPhase == 18)
				{
					GC.InstantiateVFX(GameCore.VisualEffect.playerTeleportStars, targetPosition, "EroAnim:KoboldTF");
					GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, targetPosition, "EroAnim:KoboldTF");
					GC.PlaySFX(GameCore.SFXBasic2.Boing1, 0, checkForDuplicity: true, base.transform.position, 1.5f, 1.5f);
				}
				else
				{
					GC.InstantiateVFX(GameCore.VisualEffect.SporeTransform, targetPosition, "EroAnim:KoboldTF");
					GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
				}
			}
			break;
		case GameCore.EnemyType.Sinccubus:
			targetPosition = base.transform.position;
			switch (intGetCurrentEroPhase)
			{
			case 8:
				EroLoopCutToEnd();
				break;
			case 13:
				GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, base.transform.position, "EroAnim:Succubus");
				break;
			case 19:
				GC.InstantiateVFX(GameCore.VisualEffect.CreamExplosion, targetPosition, "EroAnim:Succubus");
				break;
			case 20:
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(refMainEnemyScript.GetMiscGameObject(2), targetPosition, Quaternion.identity, GC.refOBJEffectsParent.transform);
				Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), ignore: true);
				if (refMainEnemyScript.GetEnemyFacing() < 0f)
				{
					gameObject.GetComponent<ProjectileLogic>().specialForce.x *= -1f;
				}
				refMainEnemyScript.BAD_UpCount();
				break;
			}
			}
			break;
		case GameCore.EnemyType.Arachne:
			switch (intGetCurrentEroPhase)
			{
			case 15:
				EroLoopCutToEnd();
				break;
			case 18:
				countGeneric++;
				if (countGeneric < 3)
				{
					currentAN.SetInteger("eroPhase", 16);
					break;
				}
				currentAN.SetInteger("eroPhase", 19);
				countGeneric = 0;
				break;
			case 24:
				targetPosition = refMainEnemyScript.GetEnemyCenterPos();
				targetPosition.x += 0.8f;
				targetPosition.y -= 0.65f;
				GC.InstantiateVFX(GameCore.VisualEffect.CreamExplosionPurple, targetPosition, "EroAnim:Arachne");
				PlayEroSound("cum6");
				break;
			}
			break;
		case GameCore.EnemyType.BossWerewolf:
		{
			int num = intGetCurrentEroPhase;
			if (num == 22)
			{
				countGeneric++;
				if (countGeneric < 3)
				{
					currentAN.SetInteger("eroPhase", 20);
					break;
				}
				currentAN.SetInteger("eroPhase", 23);
				countGeneric = 0;
			}
			break;
		}
		case GameCore.EnemyType.Catgirl:
			targetPosition = refMainEnemyScript.GetEnemyCenterPos();
			targetPosition.y -= 0.25f;
			if (_eroIDVariant == 3)
			{
				if (intGetCurrentEroPhase == 5)
				{
					GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
					refMainEnemyScript.GENERIC_TouchedWayPoint();
				}
				else
				{
					GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, targetPosition, "EroAnimCatGirl");
					PlayEroSound("swallow4deep");
					refMainEnemyScript.PlayMonsterSoundsFromAnim("nyaOther");
				}
			}
			break;
		}
	}

	public void ResetSelfLocalTransform()
	{
		base.transform.localPosition = Vector3.zero;
	}

	private void HACK_MINOTAUR_BOSS_BADEND()
	{
		if (!boolGeneric && currentAN != null && currentEroControl != null && currentAN.GetInteger("eroVariantID") == 99)
		{
			boolGeneric = true;
			currentEroControl = new ENM_EroPhaseControl[8];
			currentEroControl[0] = new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 0.5f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			currentEroControl[1] = new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 0.75f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			currentEroControl[2] = new ENM_EroPhaseControl
			{
				runLoops = 12,
				speedAnimation = 1f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			currentEroControl[3] = new ENM_EroPhaseControl
			{
				runLoops = 16,
				speedAnimation = 0.7f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			currentEroControl[4] = new ENM_EroPhaseControl
			{
				runLoops = 20,
				speedAnimation = 0.9f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			currentEroControl[5] = new ENM_EroPhaseControl
			{
				runLoops = 26,
				speedAnimation = 1.2f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			currentEroControl[6] = new ENM_EroPhaseControl
			{
				runLoops = 14,
				speedAnimation = 0.6f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			currentEroControl[7] = new ENM_EroPhaseControl
			{
				runLoops = 9999999,
				speedAnimation = 0.6f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
		}
	}

	private void HACK_WEREWOLF_BOSS_BADEND(bool checkGeneric)
	{
		if (checkGeneric)
		{
			if (boolGeneric || !GC.isRunningBadEndLogic || !GC.isPlayerFainted || currentAN == null || currentEroControl == null || currentAN.GetInteger("eroVariantID") != 99)
			{
				return;
			}
			boolGeneric = true;
		}
		currentEroControl = new ENM_EroPhaseControl[36];
		currentEroControl[0] = new ENM_EroPhaseControl
		{
			runLoops = 10,
			speedAnimation = 0.6f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[1] = new ENM_EroPhaseControl
		{
			runLoops = 10,
			speedAnimation = 0.75f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[2] = new ENM_EroPhaseControl
		{
			runLoops = 12,
			speedAnimation = 1.1f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[3] = new ENM_EroPhaseControl
		{
			runLoops = 17,
			speedAnimation = 1.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[4] = new ENM_EroPhaseControl
		{
			runLoops = 4,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[5] = new ENM_EroPhaseControl
		{
			runLoops = 4,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[6] = new ENM_EroPhaseControl
		{
			runLoops = 5,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[7] = new ENM_EroPhaseControl
		{
			runLoops = 6,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[8] = new ENM_EroPhaseControl
		{
			runLoops = 6,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[9] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[10] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[11] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[12] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[13] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[14] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[15] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[16] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[17] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[18] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[19] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[20] = new ENM_EroPhaseControl
		{
			runLoops = 8,
			speedAnimation = 0.7f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[21] = new ENM_EroPhaseControl
		{
			runLoops = 12,
			speedAnimation = 0.85f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[22] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[23] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[24] = new ENM_EroPhaseControl
		{
			runLoops = 16,
			speedAnimation = 0.7f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[25] = new ENM_EroPhaseControl
		{
			runLoops = 20,
			speedAnimation = 1f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[26] = new ENM_EroPhaseControl
		{
			runLoops = 30,
			speedAnimation = 1.4f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[27] = new ENM_EroPhaseControl
		{
			runLoops = 1,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[28] = new ENM_EroPhaseControl
		{
			runLoops = 12,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[29] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[30] = new ENM_EroPhaseControl
		{
			runLoops = 16,
			speedAnimation = 0.6f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[31] = new ENM_EroPhaseControl
		{
			runLoops = 20,
			speedAnimation = 0.85f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[32] = new ENM_EroPhaseControl
		{
			runLoops = 28,
			speedAnimation = 1.05f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[33] = new ENM_EroPhaseControl
		{
			runLoops = 1,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[34] = new ENM_EroPhaseControl
		{
			runLoops = 12,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
		currentEroControl[35] = new ENM_EroPhaseControl
		{
			runLoops = 999999,
			speedAnimation = 0.5f,
			frequencyOfDmg = 0,
			divisionOfDmg = 0
		};
	}

	private void UNIQUE_FlowerBlossomSinkPlayer()
	{
		if ((bool)tmpObjPlayerReference)
		{
			targetPosGeneric = base.transform.localPosition;
			targetPosGeneric.y -= 0.05f;
			base.transform.localPosition = targetPosGeneric;
			targetPosGeneric = tmpObjPlayerReference.transform.position;
			targetPosGeneric.y -= 0.05f;
			tmpObjPlayerReference.transform.position = targetPosGeneric;
		}
		else
		{
			CancelInvoke("UNIQUE_FlowerBlossomSinkPlayer");
		}
	}

	private void UNIQUE_FlowerBlossomBeginMainEro()
	{
		ResetSelfLocalTransform();
		currentAN.Play("FLB-EroGrabEnd");
		CancelInvoke("UNIQUE_FlowerBlossomBeginMainEro");
		ToggleInternalView(GC.showInternalShots);
	}

	public void UNIQUE_FLB_EroEnd()
	{
		refMainEnemyScript.GetMiscGameObject(3).SetActive(value: false);
		ToggleInternalView(state: true);
		blockIView = true;
	}

	private void ValidateEroInternalsAtStart()
	{
		if ((bool)refMainEnemyScript.eroCore.basicEroInternals)
		{
			addInternalShotsAN = refMainEnemyScript.eroCore.basicEroInternals.GetComponent<Animator>();
			addInternalShotsAN.SetInteger("eroPhase", 0);
			UpdateAnimInformationOnInternalShot(0, 1f);
			applySync = true;
		}
	}

	private bool InitialCheckEroValidity()
	{
		if (refMainEnemyScript.eroCore.eroPhase == null || refMainEnemyScript.eroCore.eroPhase.Length == 0)
		{
			if (!GC || GC.defaultEroPhases == null)
			{
				DisplayError(1);
				return false;
			}
			currentEroControl = GC.defaultEroPhases;
		}
		else
		{
			currentEroControl = refMainEnemyScript.eroCore.eroPhase;
		}
		if (currentEroControl == null || currentEroControl.Length == 0)
		{
			DisplayError(2);
			return false;
		}
		return true;
	}

	private void CheckForEroVariants()
	{
		int randomEroSceneVariantID = refMainEnemyScript.GetRandomEroSceneVariantID();
		if (randomEroSceneVariantID > 0)
		{
			currentAN.SetInteger("eroVariantID", randomEroSceneVariantID);
			refMainEnemyScript.eroCore.basicEroInternals = refMainEnemyScript.GetInternalsObjVariantByID(randomEroSceneVariantID);
		}
		refMainEnemyScript.CheckSpecialVoreFatal(randomEroSceneVariantID);
	}

	private void CheckForComboInternalShot(int comboID)
	{
		refMainEnemyScript.eroCore.basicEroInternals = refMainEnemyScript.GetInternalsObjComboByID(comboID);
	}

	private void ApplyEroGrabDamage(int dmg, Vector3 offsetDisplayDmg, bool bypassNymphoDmgResistance = false)
	{
		if (dmg <= 0)
		{
			dmg = 1;
		}
		if (!bypassNymphoDmgResistance && GC.MATH_RNG_ROLL(50) && GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.Nymphomaniac))
		{
			return;
		}
		if (!isSubAnimator)
		{
			if ((bool)playerScript && GC.playerCurrentHealth > 0f)
			{
				playerScript.ApplyDamageToPlayer(dmg, offsetDisplayDmg, afterEroEscaped: false, bypassDefense: true);
			}
		}
		else if ((bool)tmpObjPlayerReference && GC.playerCurrentHealth > 0f)
		{
			tmpObjPlayerReference.GetComponent<Controller_Player>().ApplyDamageToPlayer(dmg, offsetDisplayDmg);
		}
	}

	public void KillPlayer(int ignoreDeathScream = 0)
	{
		bool ignoreScream = false;
		if (ignoreDeathScream > 0)
		{
			ignoreScream = true;
		}
		GC.PlayMayaDeathScream(ignoreScream);
		if (!GC.isPlayerFainted && GC.playerCurrentHealth > 0f && !GC.isSceneTransitioning)
		{
			ApplyEroGrabDamage(GC.playerMaxHealth * 10, offsetPlayerDamageDisplay, bypassNymphoDmgResistance: true);
		}
	}

	public void PlayerIsGone()
	{
		if (GC.isPlayerFainted)
		{
			GC.isPlayerGoneForGood = true;
		}
	}

	public void RandomAnimationVariation(int variationQuantity)
	{
		if (!GC || currentAN == null || variationQuantity <= 1)
		{
			DisplayError(3);
			return;
		}
		int value = GC.MATH_RNG_INT(1, variationQuantity);
		currentAN.SetInteger("varAnim", value);
	}

	private void SetPlayerEroHiddenPosition()
	{
		if (tmpObjPlayerReference.transform.position != offsetPlayerHiddenPos)
		{
			tmpObjPlayerReference.transform.position = offsetPlayerHiddenPos;
		}
	}

	private void SetPlayerEroReleasedPosition()
	{
		if (tmpObjPlayerReference.transform.position != offsetPlayerReleasedPos)
		{
			tmpObjPlayerReference.transform.position = offsetPlayerReleasedPos;
		}
	}

	private int GetNewEroPhaseDueToComboStart(GameCore.EnemyType selfEnemyType)
	{
		int result = 1;
		switch (selfEnemyType)
		{
		case GameCore.EnemyType.HillFairy:
		case GameCore.EnemyType.LizardShield:
		case GameCore.EnemyType.LizardAxe:
		case GameCore.EnemyType.Goblin:
		case GameCore.EnemyType.Goblina:
			if (currentAN.GetInteger("eroPhase") == 3 || currentAN.GetInteger("eroPhase") == 2)
			{
				result = 2;
			}
			break;
		}
		return result;
	}

	public void PlayEroSound(string sfxName)
	{
		switch (sfxName)
		{
		case "piston1":
			GC.PlaySFXEro(GameCore.EroSound.Piston1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.15f);
			break;
		case "piston2":
			GC.PlaySFXEro(GameCore.EroSound.Piston2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.15f);
			break;
		case "piston3":
			GC.PlaySFXEro(GameCore.EroSound.Piston3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "piston4":
			GC.PlaySFXEro(GameCore.EroSound.Piston4, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "suck1":
			GC.PlaySFXEro(GameCore.EroSound.Suck1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "suck2":
			GC.PlaySFXEro(GameCore.EroSound.Suck2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "suck3":
			GC.PlaySFXEro(GameCore.EroSound.Suck3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "suck4":
			GC.PlaySFXEro(GameCore.EroSound.Suck4, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "suck5":
			GC.PlaySFXEro(GameCore.EroSound.Suck4, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "suck-rng":
			GC.PlaySFXEro(GameCore.EroSound.SuckRandom, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "lick1":
			GC.PlaySFXEro(GameCore.EroSound.Lick1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "lick2":
			GC.PlaySFXEro(GameCore.EroSound.Lick2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "lick3":
			GC.PlaySFXEro(GameCore.EroSound.Lick3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "lick-rng":
			GC.PlaySFXEro(GameCore.EroSound.LickRandom, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swallow1":
			GC.PlaySFXEro(GameCore.EroSound.Swallow1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swallow2":
			GC.PlaySFXEro(GameCore.EroSound.Swallow2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swallow3":
			GC.PlaySFXEro(GameCore.EroSound.Swallow3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swallow4":
			GC.PlaySFXEro(GameCore.EroSound.Swallow4, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swallow4deep":
			GC.PlaySFXEro(GameCore.EroSound.Swallow4, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 0.9f);
			break;
		case "swallow5":
			GC.PlaySFXEro(GameCore.EroSound.Swallow5, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swallow1_maya":
			GC.CallReferenceMayaVoiceGC("Swallow1", "EroAnimation.PlayEroSound()", "SpecialMayaVoiceDirectOverride");
			break;
		case "swallow2_maya":
			GC.CallReferenceMayaVoiceGC("Swallow2", "EroAnimation.PlayEroSound()", "SpecialMayaVoiceDirectOverride");
			break;
		case "swallow-rng":
			GC.PlaySFXEro(GameCore.EroSound.SwallowRandom, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swallow-rng2":
			GC.PlaySFXEro(GameCore.EroSound.SwallowRandom2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "cum1":
			GC.PlaySFXEro(GameCore.EroSound.Cum1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "cum2":
			GC.PlaySFXEro(GameCore.EroSound.Cum2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "cum3":
			GC.PlaySFXEro(GameCore.EroSound.Cum3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "cum4":
			GC.PlaySFXEro(GameCore.EroSound.Cum4, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "cum5":
			GC.PlaySFXEro(GameCore.EroSound.Cum5, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "cum6":
			GC.PlaySFXEro(GameCore.EroSound.Cum6, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "pull1":
			GC.PlaySFXEro(GameCore.EroSound.Pull1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "pull1-rng":
			GC.PlaySFXEro(GameCore.EroSound.Pull1, 60, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "vore1":
			GC.PlaySFXEro(GameCore.EroSound.Vore1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "vore2":
			GC.PlaySFXEro(GameCore.EroSound.Vore2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "vore3":
			GC.PlaySFXEro(GameCore.EroSound.Vore3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "vore4":
			GC.PlaySFXEro(GameCore.EroSound.Vore4, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "weird1":
			GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "fast1":
			GC.PlaySFX(GameCore.SFXBasic1.Fast1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "odd1":
			GC.PlaySFX(GameCore.SFXBasic2.Odd1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "odd1-rng":
			GC.PlaySFX(GameCore.SFXBasic2.Odd1, 40, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "odd2":
			GC.PlaySFX(GameCore.SFXBasic2.Odd2, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "odd3":
			GC.PlaySFX(GameCore.SFXBasic2.Odd3, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "odd4":
			GC.PlaySFX(GameCore.SFXBasic2.Odd4, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "odd5":
			GC.PlaySFX(GameCore.SFXBasic2.Odd5, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "odd6":
			GC.PlaySFX(GameCore.SFXBasic2.Odd6, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "odd7":
			GC.PlaySFX(GameCore.SFXBasic2.Odd7, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "odd8":
			GC.PlaySFX(GameCore.SFXBasic2.Odd8, 0, checkForDuplicity: true, Vector2.zero, 1.5f, 1.7f);
			break;
		case "miss1":
			GC.PlaySFX(GameCore.SFXBasic2.Throw1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "vore-rng-A":
			GC.PlaySFXEro(GameCore.EroSound.VoreRandomA, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "vore-rng-B":
			GC.PlaySFXEro(GameCore.EroSound.VoreRandomB, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "touch1":
			GC.PlaySFXEro(GameCore.EroSound.Touch1, 0, checkForDuplicity: false, Vector2.zero, 1f, 1.3f);
			break;
		case "touch2":
			GC.PlaySFXEro(GameCore.EroSound.Touch2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "touch3":
			GC.PlaySFXEro(GameCore.EroSound.Touch3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "slap1":
			GC.PlaySFXEro(GameCore.EroSound.Slap1, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 1.2f);
			break;
		case "slap2":
			GC.PlaySFXEro(GameCore.EroSound.Slap2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "contact1":
			GC.PlaySFXEro(GameCore.EroSound.Contact1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "kiss1":
			GC.PlaySFXEro(GameCore.EroSound.Kiss1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "kiss2":
			GC.PlaySFXEro(GameCore.EroSound.Kiss2, 0, checkForDuplicity: false, Vector2.zero, 0.95f, 1.05f);
			break;
		case "kiss3":
			GC.PlaySFXEro(GameCore.EroSound.Kiss3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "kiss-rng":
			GC.PlaySFXEro(GameCore.EroSound.KissRandom, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "wet1":
			GC.PlaySFXEro(GameCore.EroSound.Wet1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "wet2":
			GC.PlaySFXEro(GameCore.EroSound.Wet2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "wet3":
			GC.PlaySFXEro(GameCore.EroSound.Wet3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "wet4":
			GC.PlaySFXEro(GameCore.EroSound.Wet4, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "wet5":
			GC.PlaySFXEro(GameCore.EroSound.Wet5, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "wet6":
			GC.PlaySFXEro(GameCore.EroSound.Wet6, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "wet-rng":
			GC.PlaySFXEro(GameCore.EroSound.WetRandom, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "crystal":
			GC.PlaySFX(GameCore.SFXBasic1.CrystalDisengaged, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "bubble1":
			GC.PlaySFXEro(GameCore.EroSound.Bubble1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "bubble2":
			GC.PlaySFXEro(GameCore.EroSound.Bubble2, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "bubble3":
			GC.PlaySFXEro(GameCore.EroSound.Bubble3, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "bubble4":
			GC.PlaySFXEro(GameCore.EroSound.Bubble3, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "bubble5":
			GC.PlaySFXEro(GameCore.EroSound.Bubble5, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "bubble-rng":
			GC.PlaySFXEro(GameCore.EroSound.BubbleRandom, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case "grow1":
			GC.PlaySFX(GameCore.SFXBasic2.Grow1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "ripping1":
			GC.PlaySFX(GameCore.SFXBasic2.Ripping1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "tearing1":
			GC.PlaySFX(GameCore.SFXBasic2.Tearing1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "boing1":
			GC.PlaySFX(GameCore.SFXBasic2.Boing1, 0, checkForDuplicity: true, Vector2.zero, 0.95f, 1.05f);
			break;
		case "boing2":
			GC.PlaySFX(GameCore.SFXBasic2.Boing2, 0, checkForDuplicity: true, Vector2.zero, 0.95f, 1.05f);
			break;
		case "boing3":
			GC.PlaySFX(GameCore.SFXBasic2.Boing3, 0, checkForDuplicity: true, Vector2.zero, 0.95f, 1.05f);
			break;
		case "boing4":
			GC.PlaySFX(GameCore.SFXBasic2.Boing4, 0, checkForDuplicity: true, Vector2.zero, 0.95f, 1.05f);
			break;
		case "crystal1":
			GC.PlaySFX(GameCore.SFXBasic2.Crystalize, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case "rubber1":
			GC.PlaySFXEro(GameCore.EroSound.Rubber1, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "rubber3":
			GC.PlaySFXEro(GameCore.EroSound.Rubber3, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "rubber4":
			GC.PlaySFXEro(GameCore.EroSound.Rubber4, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "rubber-rng":
			GC.PlaySFXEro(GameCore.EroSound.RubberRandom, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "swing1":
			GC.PlaySFX(GameCore.SFXBasic2.Swing1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "heartbeat1":
			GC.PlaySFXEro(GameCore.EroSound.HeartBeat1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		default:
			Debug.Log("Tried to PlayEroSound but string [" + sfxName + "] was not on the switch list!");
			break;
		}
	}

	public void PlayMayaVoice(string sfxName)
	{
		GC.CallReferenceMayaVoiceGC(sfxName, GC.enemyGrabbingPlayer.ToString() + ".EroAnimation.PlayMayaVoice()", "", muffledEffect: false, maleEffect: false, Mathf.RoundToInt(GC.configVoiceRate * 10f));
	}

	public void PlayMayaVoice50(string sfxName)
	{
		int overrideChancePercent = Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(GC.configVoiceRate * 10f, 50f));
		GC.CallReferenceMayaVoiceGC(sfxName, GC.enemyGrabbingPlayer.ToString() + ".EroAnimation.PlayMayaVoice50()", "", muffledEffect: false, maleEffect: false, overrideChancePercent);
	}

	public void PlayMayaVoiceMuffled(string sfxName)
	{
		GC.CallReferenceMayaVoiceGC(sfxName, GC.enemyGrabbingPlayer.ToString() + ".EroAnimation.PlayMayaVoiceMuffled()", "", muffledEffect: true, maleEffect: false, Mathf.RoundToInt(GC.configVoiceRate * 10f));
	}

	public void PlayMayaVoiceMale(string sfxName)
	{
		int overrideChancePercent = Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(GC.configVoiceRate * 10f, 50f));
		GC.CallReferenceMayaVoiceGC(sfxName, GC.enemyGrabbingPlayer.ToString() + ".EroAnimation.PlayMayaVoiceMale()", "", muffledEffect: false, maleEffect: true, overrideChancePercent);
	}

	public void PlayMayaVoiceMaleGUARANTEED(string sfxName)
	{
		GC.CallReferenceMayaVoiceGC(sfxName, GC.enemyGrabbingPlayer.ToString() + ".EroAnimation.PlayMayaVoiceMaleGUARANTEED()", "", muffledEffect: false, maleEffect: true);
	}

	public void HarunaVoice(string sfxName)
	{
		int idSFX = 1;
		int[] array = null;
		switch (sfxName)
		{
		case "Climax":
			idSFX = 0;
			break;
		case "3":
			idSFX = 3;
			break;
		case "5":
			idSFX = 5;
			break;
		case "rng-Moan1":
			array = new int[5] { 1, 2, 4, 5, 6 };
			break;
		case "rng-Moan2":
			array = new int[5] { 6, 7, 9, 10, 11 };
			break;
		case "rng-Moan3":
			array = new int[5] { 3, 11, 12, 13, 14 };
			break;
		}
		if (array != null)
		{
			idSFX = GC.MATH_RNG_INT_ARRAY(array);
		}
		GC.PlaySFXGeneric("HarunaNPC", idSFX, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f, "voice", "HaruMoan");
	}

	private void ToggleInternalView(bool state)
	{
		Color color = new Color(1f, 1f, 1f, 0f);
		if (state)
		{
			color.a = 1f;
		}
		localInternalShots = state;
		GameCore.EnemyType currentEnemyType = refMainEnemyScript.currentEnemyType;
		if (currentEnemyType != GameCore.EnemyType.FlowerBlossom)
		{
			if (refMainEnemyScript.eroCore.basicEroInternals != null)
			{
				if (refMainEnemyScript.eroCore.layeredEroInternal)
				{
					ToggleFourStatesOfDoubleInternalsLogic(state, eroEnd: false);
				}
				else
				{
					refMainEnemyScript.eroCore.basicEroInternals.GetComponent<SpriteRenderer>().color = color;
				}
			}
			else if (refMainEnemyScript.eroCore.layeredEroInternal)
			{
				ChangeAnimLayerOfEroInternals(state);
			}
		}
		else if (isSubAnimator && !blockIView)
		{
			if (state)
			{
				refMainEnemyScript.GetMiscGameObject(0).SetActive(value: false);
				refMainEnemyScript.SetParentRendererAlpha(0f);
			}
			else
			{
				refMainEnemyScript.GetMiscGameObject(0).SetActive(value: true);
				refMainEnemyScript.SetParentRendererAlpha(1f);
			}
		}
	}

	private void CheckIfInternalsShotsAreAllowedToDisplay()
	{
		if (!GC || !GC.playerIsBeingGrabed || !refMainEnemyScript || !tmpObjPlayerReference)
		{
			return;
		}
		int minInternalsPhase = refMainEnemyScript.eroCore.minInternalsPhase;
		int maxInternalsPhase = refMainEnemyScript.eroCore.maxInternalsPhase;
		if (GC.showInternalShots && intGetCurrentEroPhase >= minInternalsPhase && intGetCurrentEroPhase <= maxInternalsPhase)
		{
			if (!localInternalShots)
			{
				ToggleInternalView(state: true);
			}
		}
		else if (localInternalShots)
		{
			ToggleInternalView(state: false);
		}
	}

	private void UpdateAnimInformationOnInternalShot(int eroPhase, float speedAnimation)
	{
		if (addInternalShotsAN != null)
		{
			addInternalShotsAN.SetInteger("eroPhase", eroPhase);
			if (speedAnimation != 0f)
			{
				addInternalShotsAN.SetFloat("speedAnimation", speedAnimation);
			}
		}
	}

	private void ChangeAnimLayerOfEroInternals(bool state)
	{
		if (state)
		{
			if (currentAN.GetLayerWeight(1) < 1f)
			{
				currentAN.SetLayerWeight(1, 1f);
				currentAN.SetLayerWeight(0, 0f);
			}
		}
		else if (currentAN.GetLayerWeight(0) < 1f)
		{
			currentAN.SetLayerWeight(0, 1f);
			currentAN.SetLayerWeight(1, 0f);
		}
	}

	public void ForceToggleInternalView(int intState)
	{
		bool state = false;
		if (intState == 1)
		{
			state = true;
		}
		ToggleInternalView(state);
	}

	private void ToggleFourStatesOfDoubleInternalsLogic(bool mainState, bool eroEnd)
	{
		Color color = new Color(1f, 1f, 1f, 0f);
		bool state = false;
		if (eroEnd)
		{
			currentDualInternalState = DoubleInternal.LayerOff_CircleOff;
		}
		else
		{
			DetermineCurrentDoubleInternalState(mainState);
		}
		switch (currentDualInternalState)
		{
		case DoubleInternal.LayerOff_CircleOff:
			state = false;
			localInternalShots = false;
			break;
		case DoubleInternal.LayerOff_CircleOn:
			state = false;
			color.a = 1f;
			localInternalShots = true;
			break;
		case DoubleInternal.LayerOn_CircleOff:
			state = true;
			localInternalShots = false;
			break;
		case DoubleInternal.LayerOn_CircleOn:
			state = true;
			color.a = 1f;
			localInternalShots = true;
			break;
		}
		refMainEnemyScript.eroCore.basicEroInternals.GetComponent<SpriteRenderer>().color = color;
		ChangeAnimLayerOfEroInternals(state);
	}

	private void DetermineCurrentDoubleInternalState(bool sentState)
	{
		if (isInitialDualStateCheck)
		{
			if (sentState)
			{
				currentDualInternalState = DoubleInternal.LayerOn_CircleOn;
			}
			else
			{
				currentDualInternalState = DoubleInternal.LayerOff_CircleOff;
			}
			isInitialDualStateCheck = false;
		}
		else if (currentDualInternalState == DoubleInternal.LayerOn_CircleOn)
		{
			currentDualInternalState = DoubleInternal.LayerOff_CircleOff;
		}
		else
		{
			currentDualInternalState++;
		}
	}

	public void PerformAnimationControlAction(GameCore.AnimPlayback action)
	{
		switch (action)
		{
		case GameCore.AnimPlayback.Restart:
			if (HACK_FlowerBlossomRestart())
			{
				return;
			}
			GC.enemyGrabbingEroPhase = 0;
			intGetCurrentEroPhase = 0;
			countEroLoop = 0;
			currentAN.SetInteger("eroPhase", intGetCurrentEroPhase);
			currentAN.SetBool("isEro", value: false);
			CheckSpecialTypesOfEroAnimation(stateON: true);
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, refMainEnemyScript.transform.position, "[EroAnimation]:ResetAnimControl");
			GC.DisplaySystemMessageUI(0, 104);
			break;
		case GameCore.AnimPlayback.Play:
			lockCurrentLoop = false;
			GC.DisplaySystemMessageUI(0, 105);
			break;
		case GameCore.AnimPlayback.LoopLock:
			if (HACK_FlowerBlossomLock())
			{
				return;
			}
			lockCurrentLoop = true;
			GC.DisplaySystemMessageUI(0, 106);
			break;
		case GameCore.AnimPlayback.NextPhase:
			countEroLoop = 99;
			GC.DisplaySystemMessageUI(0, 107);
			break;
		case GameCore.AnimPlayback.SpeedLess:
			if (injectSpeed == 0f)
			{
				injectSpeed = currentEroControl[intGetCurrentEroPhase].speedAnimation;
			}
			injectSpeed -= 0.15f;
			if (injectSpeed < 0.2f)
			{
				injectSpeed = 0.2f;
				GC.DisplaySystemMessageUI(0, 111);
			}
			else
			{
				GC.DisplaySystemMessageUI(0, 108);
			}
			SetAnimationSpeed(currentEroControl[intGetCurrentEroPhase].speedAnimation);
			break;
		case GameCore.AnimPlayback.SpeedMore:
			if (injectSpeed == 0f)
			{
				injectSpeed = currentEroControl[intGetCurrentEroPhase].speedAnimation;
			}
			injectSpeed += 0.15f;
			if (injectSpeed > 2.5f)
			{
				injectSpeed = 2.5f;
				GC.DisplaySystemMessageUI(0, 112);
			}
			else
			{
				GC.DisplaySystemMessageUI(0, 109);
			}
			SetAnimationSpeed(currentEroControl[intGetCurrentEroPhase].speedAnimation);
			break;
		case GameCore.AnimPlayback.Exit:
			GC.DisplaySystemMessageUI(0, 110);
			GC.TrySetupGameLoad(getLatestSave: false, 0, GameCore.MenuScene.InGame, isRetry: true);
			break;
		case GameCore.AnimPlayback.Faint:
			GC.DisplaySystemMessageUI(0, 167, 5f);
			if (!GC.playerIsInvincible && !GC.isPlayerFainted)
			{
				GC.GlobalApplyDamageToPlayer(GC.playerMaxHealth + 1, Vector3.zero, afterEroEscaped: false, bypassDefense: true);
			}
			break;
		}
		HACK_FlowerBlossomSpecialCase(action);
	}

	private void HACK_FlowerBlossomSpecialCase(GameCore.AnimPlayback paramAction)
	{
		if (refMainEnemyScript != null && refMainEnemyScript.currentEnemyType == GameCore.EnemyType.FlowerBlossom && !isSubAnimator)
		{
			refMainEnemyScript.GetMiscGameObject(2).GetComponent<EroAnimation>().PerformAnimationControlAction(paramAction);
		}
	}

	private bool HACK_FlowerBlossomRestart()
	{
		if (refMainEnemyScript != null && refMainEnemyScript.currentEnemyType == GameCore.EnemyType.FlowerBlossom && !isSubAnimator)
		{
			GC.TrySetupGameLoad(getLatestSave: false, 0, GameCore.MenuScene.InGame, isRetry: true);
			return true;
		}
		return false;
	}

	private bool HACK_FlowerBlossomLock()
	{
		if (refMainEnemyScript != null && refMainEnemyScript.currentEnemyType == GameCore.EnemyType.FlowerBlossom && isSubAnimator && currentAN.GetInteger("eroPhase") == 0)
		{
			return true;
		}
		return false;
	}

	private void DisplayError(int messageID)
	{
		string text = "[ERO_ANIMATION][" + base.gameObject.name + "] Error " + messageID + ": ";
		switch (messageID)
		{
		case 0:
			text += "Cannot get reference to parent ENEMY_SCRIPT! Canceling application of specific animation corner case!";
			break;
		case 1:
			text += "Reference to GAMECORE was NULL!";
			break;
		case 2:
			text += "Tried to use the validate the localEroPhase but it was either NULL or empty!";
			break;
		case 3:
			text += "Could not get reference to GC or is null or quantity is zero!";
			break;
		case 4:
			text += "Could not get valid eroPhase[] array in sub animator!";
			break;
		}
		Debug.Log(text);
	}
}
