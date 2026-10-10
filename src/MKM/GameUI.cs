using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
	[HideInInspector]
	public enum BarType
	{
		Life,
		Spirit,
		Durability,
		Struggle
	}

	public UI_MainOverlay UIMainOverlay;

	public UI_MainGameplay UIGameplay;

	public UI_Dialogue UIDialogue;

	public UI_Misc UIMisc;

	public UI_StruggleSystem UIStruggle;

	public UI_BeltEquipMain clsBeltSlots;

	public UI_Debuff UIDebuff;

	public UI_Debug UIDebug;

	public UI_AnimationController UIAnimControl;

	private bool isShowStageName;

	private bool isShowSystemMessage;

	private bool hasUpdatedDebugKeysDescription;

	private bool boolLastKnowErOFatalState;

	private bool isDisplayingDialogue;

	private bool tutorialState;

	private bool isMayaIconMoving;

	private bool isIconStruggleMoving;

	private int currentDialogueIndex;

	private int timerBossWWCurseMax;

	private int timerBossWWCurseCurrent;

	private int chanceIconBase;

	private int chanceIconCurrent;

	private float freqInvokeFade;

	private float rateFadeInOut;

	private float speedShowStageName;

	private float positionYStageName;

	private float speedDisplaySysMsg;

	private float countFractionSecond;

	private float maxCurrentShowTime;

	private float freqInvokeCountSeconds;

	private float freqInvokedIconSpawn;

	private float freqInvokedOldIconSpawn;

	private float rangeIconStart;

	private float rangeCollectPerfect;

	private float rangeCollectNear;

	private float rangeCollectMiss;

	private float speedHeartMove;

	private string textToDisplay;

	private string textNameDisplay;

	private string timeText;

	private string strKillCount;

	private string strKillGoal;

	private const string invokedIconSpawnName = "STG_IVK_IconsMainGenerationLogic";

	private const string invokedOldIconSpawnName = "STG_INVK_SpawnIconLogic";

	private TimeSpan timeSpan;

	private MKMSystemMessages[] clsSysMessages;

	private Vector2 currentDialogueCameraMove;

	private Vector2 originalStageNamePainelPos;

	private Vector3 posIconMaya;

	private Vector3 posIconHeart;

	private Vector3 posIconTarget;

	private Vector3 posTargetBoardMove;

	private Vector3 posDialogueBoxOriginal;

	private Color colorBlackScreen;

	private Color colorMirrorScreen;

	private Color colorFadeInFrame;

	private Color colorFadeSN1;

	private Color colorFadeSN2;

	private Color clrTuto1;

	private Color clrTuto2;

	private Color colorHidden;

	private GameCore.NPCType currentNPC;

	private Coroutine coroutTypeCharacters;

	private Coroutine crtDelayStruggleBar;

	private Coroutine coroutDelayCancelKeyPress;

	private Coroutine crtShowStageName;

	private Coroutine crtShowStageNameDelayed;

	private Coroutine crtShowFlavorMessage;

	private Coroutine crtWaitBoardHasIcons;

	private ControllerCamera coreCAM;

	private DialogueList listDialogue;

	private GameCore.SpeakerID _letterSoundTone;

	private GameCore GC;

	public ControllerCamera CameraReference
	{
		get
		{
			CheckCameraCoreRef();
			return coreCAM;
		}
	}

	private void Start()
	{
		CheckGameCoreRef();
		posDialogueBoxOriginal = UIDialogue.Window.transform.position;
		speedShowStageName = 5f;
		positionYStageName = 1.35f;
		speedDisplaySysMsg = 8f;
		countFractionSecond = 0f;
		tutorialState = false;
		isDisplayingDialogue = false;
		hasUpdatedDebugKeysDescription = false;
		isShowStageName = false;
		isShowSystemMessage = false;
		isMayaIconMoving = false;
		crtShowStageName = null;
		crtShowFlavorMessage = null;
		crtWaitBoardHasIcons = null;
		clsSysMessages = new MKMSystemMessages[3];
		for (int i = 0; i < clsSysMessages.Length; i++)
		{
			clsSysMessages[i] = new MKMSystemMessages();
		}
		CheckIconToDisplay(disable: true);
		colorBlackScreen = Color.black;
		colorHidden = new Color(1f, 1f, 1f, 0f);
		freqInvokeFade = 0.05f;
		freqInvokeCountSeconds = 0.1f;
		timerBossWWCurseMax = 600;
		originalStageNamePainelPos = UIMisc.StageNamePainel.transform.position;
		InvokeRepeating("InvokedCountSeconds", 0f, freqInvokeCountSeconds);
	}

	private void FixedUpdate()
	{
		FXU_HandleSysMsgWindow();
		FXU_HandleFlavorSysMsgWindow();
	}

	private void Update()
	{
		if (!GC.isGamePaused && GC.strCurrentActiveSceneName == "AnimGallery" && UIAnimControl.objAnimControlWindow.activeInHierarchy && GC.GetSubmitButtonUniversalPress())
		{
			GC.SubmitSelectableClickedUniversal();
		}
		if (UIStruggle.StruggleBarFill.isActiveAndEnabled)
		{
			STG_UpdateWatchForBarStyleChange();
		}
		if (UIStruggle.objFill.isActiveAndEnabled)
		{
			STG_UPD_FadeInOutBoardIcons();
		}
		if (isShowStageName)
		{
			if (UIMisc.StageNamePainel.transform.position.y != originalStageNamePainelPos.y / positionYStageName)
			{
				UIMisc.StageNamePainel.transform.position = new Vector3(UIMisc.StageNamePainel.transform.position.x, Mathf.SmoothStep(UIMisc.StageNamePainel.transform.position.y, originalStageNamePainelPos.y / positionYStageName, speedShowStageName * Time.deltaTime), UIMisc.StageNamePainel.transform.position.z);
			}
			if (colorFadeSN1.a < 1f)
			{
				colorFadeSN1.a = Mathf.SmoothStep(colorFadeSN1.a, 1f, speedShowStageName * Time.deltaTime);
				UIMisc.StageNamePainel.color = colorFadeSN1;
			}
			if (colorFadeSN2.a < 1f)
			{
				colorFadeSN2.a = Mathf.SmoothStep(colorFadeSN2.a, 1f, speedShowStageName * Time.deltaTime);
				UIMisc.StageName.color = colorFadeSN2;
			}
		}
		else
		{
			if (UIMisc.StageNamePainel.transform.position.y != originalStageNamePainelPos.y)
			{
				UIMisc.StageNamePainel.transform.position = new Vector3(UIMisc.StageNamePainel.transform.position.x, Mathf.SmoothStep(UIMisc.StageNamePainel.transform.position.y, originalStageNamePainelPos.y, speedShowStageName * Time.deltaTime), UIMisc.StageNamePainel.transform.position.z);
			}
			if (colorFadeSN1.a > 0f)
			{
				colorFadeSN1.a = Mathf.SmoothStep(colorFadeSN1.a, 0f, speedShowStageName * Time.deltaTime);
				UIMisc.StageNamePainel.color = colorFadeSN1;
			}
			if (colorFadeSN2.a > 0f)
			{
				colorFadeSN2.a = Mathf.SmoothStep(colorFadeSN2.a, 0f, speedShowStageName * Time.deltaTime);
				UIMisc.StageName.color = colorFadeSN2;
			}
		}
		TutorialBoxFadeLogic();
		if (UIDebuff.BossWerewolfCurse.gameObject.activeInHierarchy)
		{
			timeSpan = TimeSpan.FromSeconds(timerBossWWCurseCurrent);
			timeText = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
			UIDebuff.BossWerewolfTimer.text = timeText;
			UIDebuff.BossWerewolfTimerShadow.text = timeText;
			if (timerBossWWCurseCurrent <= 0)
			{
				GC.GlobalApplyDamageToPlayer(999999, Vector3.zero, afterEroEscaped: false, bypassDefense: true);
			}
		}
		if (UIMisc.objChallengeKillCountUI.gameObject.activeInHierarchy && (bool)GC && GC.isGauntletFight)
		{
			strKillCount = GC.countGauntletKills.ToString();
			strKillGoal = GC.countGauntletGoal.ToString();
			if (strKillCount.Length == 1)
			{
				strKillCount = "0" + strKillCount;
			}
			if (strKillGoal.Length == 1)
			{
				strKillGoal = "0" + strKillGoal;
			}
			UIMisc.txtCountMain.text = strKillCount + "/" + strKillGoal;
			UIMisc.txtCountShadow.text = UIMisc.txtCountMain.text;
		}
		CheckDebugKeysDescription();
		ToggleAnimationControlWindow();
		UpdateActiveItemMainGameplayUI();
		if (isMayaIconMoving)
		{
			STG_UPD_DoMayaIconMove();
		}
		if (isIconStruggleMoving)
		{
			STG_UPD_DoHeartIconMove();
		}
		if ((bool)GC && GC.isDisplayingDialogue != isDisplayingDialogue)
		{
			GC.isDisplayingDialogue = isDisplayingDialogue;
		}
	}

	private void CheckDebugKeysDescription()
	{
		if ((bool)GC && !hasUpdatedDebugKeysDescription)
		{
			if (GC.GameBuildCurrent == GameCore.GameBuildType.WebGL)
			{
				UIDebug.debugKey5.text = "5: Heal Life";
				UIDebug.debugKey6.text = "6: Heal Spirit";
				UIDebug.debugKey7.text = "7: Restore Durability";
				UIDebug.debugKey8.text = "8: Reset Armor to Full";
				UIDebug.debugKey9.text = "9: Unlock all TP points and skills";
				UIDebug.debugKey0.text = "0: Boss guards TP device";
				UIDebug.debugKeyMinus.text = ", (comma): Destroy Layer of Armor";
				UIDebug.debugKeyPlus.text = ". (period): Instant Defeat";
			}
			else
			{
				UIDebug.debugKey5.text = "F5: Heal Life";
				UIDebug.debugKey6.text = "F6: Heal Spirit";
				UIDebug.debugKey7.text = "F7: Restore Durability";
				UIDebug.debugKey8.text = "F8: Reset Armor to Full";
				UIDebug.debugKey9.text = "F9: Unlock TP points and skills";
				UIDebug.debugKey0.text = "F10: Boss guards TP device";
				UIDebug.debugKeyMinus.text = "F11: Destroy Layer of Armor";
				UIDebug.debugKeyPlus.text = "F12: Instant Defeat";
			}
			hasUpdatedDebugKeysDescription = true;
		}
	}

	public void UpdateStatusBars(BarType typeOfBar, float currentAmount, float maxAmount, Camera coreCam = null)
	{
		switch (typeOfBar)
		{
		case BarType.Life:
			if ((bool)UIGameplay.HealthBarFill)
			{
				UIGameplay.HealthBarFill.fillAmount = currentAmount / maxAmount;
			}
			if ((bool)UIGameplay.HealthValues)
			{
				UIGameplay.HealthValues.text = currentAmount + " / " + maxAmount;
			}
			if (UIStruggle.objFill.isActiveAndEnabled)
			{
				UIStruggle.imgMinibarHP.fillAmount = currentAmount / maxAmount;
			}
			break;
		case BarType.Spirit:
			if ((bool)UIGameplay.SpiritBarFill)
			{
				UIGameplay.SpiritBarFill.fillAmount = currentAmount / maxAmount;
			}
			if ((bool)UIGameplay.SpiritValues)
			{
				UIGameplay.SpiritValues.text = Mathf.RoundToInt(currentAmount) + " / " + maxAmount;
			}
			if (UIStruggle.objFill.isActiveAndEnabled)
			{
				UIStruggle.imgMinibarSP.fillAmount = currentAmount / maxAmount;
			}
			break;
		case BarType.Durability:
			if (currentAmount < 0f)
			{
				currentAmount = 0f;
			}
			if ((bool)UIGameplay.DurabilityBarFill)
			{
				UIGameplay.DurabilityBarFill.fillAmount = currentAmount / maxAmount;
			}
			if ((bool)UIGameplay.DurabilityValues && (bool)GC)
			{
				UIGameplay.DurabilityValues.text = GC.MATH_ResultPercentileOfTotal(currentAmount, maxAmount) + "%";
			}
			break;
		case BarType.Struggle:
			STG_UpdateStruggleAmount(currentAmount, maxAmount);
			break;
		}
	}

	public void UpdateItemSlotsDisplay()
	{
		int num = GC.arraySlotsOfBelt.Length;
		Image[] array = new Image[num];
		Text[] array2 = new Text[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = clsBeltSlots.arraySlots[i].imgDisplayIcon;
			array2[i] = clsBeltSlots.arraySlots[i].txtQuantity;
		}
		GC.RefreshBeltItemVisualSlotsDisplay(array, array2, clsBeltSlots.listInvIcons);
	}

	public void UpdateActiveItemMainGameplayUI()
	{
		GC.RefreshBeltItemVisualSlotsDisplay(UIGameplay.imgActiveItemIcon, UIGameplay.imgItemNext, UIGameplay.imgItemPrevious, UIGameplay.txtActiveItemAmount, clsBeltSlots.listInvIcons, returnBlankForAccessories: true);
	}

	public void UpdateBossCurrentLife(float currentAmount, float maxAmount)
	{
		if (UIMisc.BossLifeBarBG.gameObject.activeInHierarchy && (bool)UIMisc.BossLifeBarFill)
		{
			UIMisc.BossLifeBarFill.fillAmount = currentAmount / maxAmount;
		}
	}

	public void ToggleMainMenu(bool state)
	{
		if (state)
		{
			StartDelayForCancelKeyPress();
		}
		if ((bool)UIMainOverlay.MainMenu)
		{
			UIMainOverlay.MainMenu.SetActive(state);
		}
		if (((bool)UIMainOverlay.FirstSelected & state) && !GC.IsButtonAlreadySelected(UIMainOverlay.FirstSelected))
		{
			UIMainOverlay.FirstSelected.Select();
		}
		if (!state)
		{
			UIMainOverlay.CloseAllWindows.onClick.Invoke();
		}
	}

	public void ToggleUIElementsOnFainted()
	{
		if ((bool)UIMainOverlay.Fainted && !UIMainOverlay.Fainted.gameObject.activeInHierarchy)
		{
			UIMainOverlay.Fainted.gameObject.SetActive(value: true);
		}
		ToggleChallengeKillCountUI(state: false);
		InvokeRepeating("FadeInGameOverFrame", 0f, 0.1f);
		InvokeRepeating("INVOKED_UI_DisableElementsOnFainted", 0f, 1f);
	}

	public void ToggleEventFrame(bool setThisState)
	{
		if ((bool)UIMainOverlay.EventFocus)
		{
			UIMainOverlay.EventFocus.gameObject.SetActive(setThisState);
			ToggleMainBasicUI(!setThisState);
		}
	}

	public void ToggleMainBasicUI(bool setThisState)
	{
		UIGameplay.HealthBarFill.transform.parent.gameObject.SetActive(setThisState);
		UIGameplay.DurabilityBarFill.transform.parent.gameObject.SetActive(setThisState);
		UIGameplay.SpiritBarFill.transform.parent.gameObject.SetActive(setThisState);
		clsBeltSlots.objContainerMaster.SetActive(setThisState);
		UIGameplay.objActiveItemContainer.SetActive(setThisState);
		ToggleDebugDescription(setThisState);
	}

	public void ToggleDebugDescription(bool state)
	{
		if (!GC.doAllowDebug)
		{
			state = false;
		}
		UIDebug.containerMaster.SetActive(state);
	}

	private void FadeInGameOverFrame()
	{
		colorFadeInFrame = UIMainOverlay.Fainted.color;
		colorFadeInFrame.a += 0.05f;
		UIMainOverlay.Fainted.color = colorFadeInFrame;
		if (UIMainOverlay.Fainted.color.a >= 1f)
		{
			CancelInvoke("FadeInGameOverFrame");
		}
	}

	public void ToggleBossLifeBar(bool setThisState)
	{
		if ((bool)UIMisc.BossLifeBarBG)
		{
			UIMisc.BossLifeBarBG.gameObject.SetActive(setThisState);
		}
		if ((bool)UIMisc.BossNameShadow)
		{
			UIMisc.BossNameShadow.gameObject.SetActive(setThisState);
		}
	}

	private void INVOKED_UI_DisableElementsOnFainted()
	{
		if (UIGameplay.HealthBarFill.transform.parent.gameObject.activeInHierarchy)
		{
			ToggleMainBasicUI(setThisState: false);
		}
		if (UIMisc.BossLifeBarBG.gameObject.activeInHierarchy)
		{
			ToggleBossLifeBar(setThisState: false);
		}
		if (UIDebuff.BossWerewolfCurse.gameObject.activeInHierarchy)
		{
			ToggleWerewolfCurse(setThisState: false);
		}
	}

	public void DisplayBADENDPartsInfo(int current, int max)
	{
		if (!UIMisc.txtBADENDparts.transform.parent.gameObject.activeInHierarchy)
		{
			UIMisc.txtBADENDparts.transform.parent.gameObject.SetActive(value: true);
		}
		UIMisc.txtBADENDparts.text = current + " / " + max;
	}

	public void ToggleChallengeKillCountUI(bool state)
	{
		UIMisc.objChallengeKillCountUI.SetActive(state);
	}

	public void STG_ToggleStruggleUI(bool setState, bool isEroFatal = false)
	{
		if (setState)
		{
			if (GC.revertToOldStruggleBar)
			{
				STG_DelayStruggleBarAppearance(start: true);
				STG_ChangeStyleOfStrugglebar(isEroFatal);
				STG_ToggleButtonMashingIconLogic(turnON: true);
				UIStruggle.StruggleBarBG.gameObject.SetActive(value: true);
			}
			else
			{
				STG_InitializeBoardState();
				UIStruggle.objFill.gameObject.SetActive(value: true);
			}
		}
		else
		{
			UIStruggle.StruggleBarBG.gameObject.SetActive(value: false);
			UIStruggle.objFill.gameObject.SetActive(value: false);
			STG_ToggleButtonMashingIconLogic(turnON: false);
			STG_ToggleInvokeSpawnIcons(turnON: false);
		}
	}

	private void STG_InitializeBoardState()
	{
		STG_InitializeArrayPositions();
		crtWaitBoardHasIcons = null;
		freqInvokedIconSpawn = 5f;
		posTargetBoardMove = Vector3.zero;
		isMayaIconMoving = false;
		posIconMaya = UIStruggle.arrayIconsPos[4].transform.localPosition;
		UIStruggle.iconMaya.transform.localPosition = posIconMaya;
		UIStruggle.iconMaya.sprite = UIStruggle.spriteMayaDefault;
		STG_ToggleInvokeSpawnIcons(turnON: true);
	}

	private void STG_ToggleInvokeSpawnIcons(bool turnON)
	{
		if (turnON)
		{
			InvokeRepeating("STG_IVK_IconsMainGenerationLogic", 0.2f, freqInvokedIconSpawn);
		}
		else if (IsInvoking("STG_IVK_IconsMainGenerationLogic"))
		{
			CancelInvoke("STG_IVK_IconsMainGenerationLogic");
			STG_WipeBoardClean(isResetTotalWipe: true);
		}
	}

	private void STG_UpdateStruggleAmount(float currentAmount, float maxAmount)
	{
		if (UIStruggle.objFill.IsActive())
		{
			UIStruggle.objFill.fillAmount = currentAmount / maxAmount;
		}
		if (UIStruggle.StruggleBarFill.IsActive())
		{
			UIStruggle.StruggleBarFill.fillAmount = currentAmount / maxAmount;
		}
	}

	public bool STG_TryPerformMayaMove(GameCore.InputLib thisDir)
	{
		if (!isMayaIconMoving && STG_CheckDirectionValidity(thisDir))
		{
			posTargetBoardMove = posIconMaya;
			switch (thisDir)
			{
			case GameCore.InputLib.Up:
				posTargetBoardMove.y += 16f;
				break;
			case GameCore.InputLib.Right:
				posTargetBoardMove.x += 16f;
				break;
			case GameCore.InputLib.Down:
				posTargetBoardMove.y -= 16f;
				break;
			case GameCore.InputLib.Left:
				posTargetBoardMove.x -= 16f;
				break;
			}
			isMayaIconMoving = true;
			return true;
		}
		return false;
	}

	private bool STG_CheckDirectionValidity(GameCore.InputLib thisDir)
	{
		switch (thisDir)
		{
		case GameCore.InputLib.Up:
			if (posIconMaya.y < 10f)
			{
				return true;
			}
			break;
		case GameCore.InputLib.Right:
			if (posIconMaya.x < 10f)
			{
				return true;
			}
			break;
		case GameCore.InputLib.Down:
			if (posIconMaya.y > -10f)
			{
				return true;
			}
			break;
		case GameCore.InputLib.Left:
			if (posIconMaya.x > -10f)
			{
				return true;
			}
			break;
		}
		return false;
	}

	private void STG_UPD_DoMayaIconMove()
	{
		posIconMaya = new Vector3(Mathf.MoveTowards(posIconMaya.x, posTargetBoardMove.x, STG_UPD_GetMayaMoveSpeed() * Time.deltaTime), Mathf.MoveTowards(posIconMaya.y, posTargetBoardMove.y, STG_UPD_GetMayaMoveSpeed() * Time.deltaTime), posIconMaya.z);
		UIStruggle.iconMaya.transform.localPosition = posIconMaya;
		STG_UPD_CheckTargetReached();
	}

	private void STG_UPD_CheckTargetReached()
	{
		if (!GC.MATH_ComparePositionsAddMargin(posIconMaya, posTargetBoardMove, 0f, 0f, 1f, -1f))
		{
			return;
		}
		posIconMaya.x = Mathf.RoundToInt(posTargetBoardMove.x);
		posIconMaya.y = Mathf.RoundToInt(posTargetBoardMove.y);
		UIStruggle.iconMaya.transform.localPosition = posIconMaya;
		isMayaIconMoving = false;
		posTargetBoardMove = Vector3.zero;
		Image[] arrayIconsPos = UIStruggle.arrayIconsPos;
		foreach (Image image in arrayIconsPos)
		{
			if (image.transform.localPosition == posIconMaya && image.sprite != null)
			{
				STG_ValidateIconTouched(image.sprite);
				image.sprite = null;
				image.color = Color.yellow;
				break;
			}
		}
	}

	private void STG_ValidateIconTouched(Sprite thisIcon)
	{
		if (thisIcon == UIStruggle.spritePunchRelease)
		{
			GC.GlobalApplyStruggleAmountToPlayer(15f);
			UIStruggle.iconMaya.sprite = UIStruggle.spriteMayaHappy;
			GC.PlaySFX(GameCore.SFXBasic1.KitsuneCast1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
		else if (thisIcon == UIStruggle.spriteLoveEssence)
		{
			GC.ESSENCE_Spawn(GameCore.EssenceSpawnType.EroGrab, new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, 0f), GC.objEnemyGrabbingPlayer.GetComponent<EnemyGeneralLogic>().enmBaseStats.maxLife);
			UIStruggle.iconMaya.sprite = UIStruggle.spriteMayaClimax;
			GC.CallReferenceMayaVoiceGC("moanLight8", "STG", "STG");
		}
		else if (thisIcon == UIStruggle.spriteHP)
		{
			GC.HealThePlayer(5f);
			UIStruggle.iconMaya.sprite = UIStruggle.spriteMayaDefault;
			GC.PlaySFX(GameCore.SFXBasic1.PotionGet, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
		else if (thisIcon == UIStruggle.spriteSP)
		{
			GC.HealTheSpirit(20f);
			UIStruggle.iconMaya.sprite = UIStruggle.spriteMayaHappy;
			GC.PlaySFX(GameCore.SFXBasic1.PotionGet, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
		else if (thisIcon == UIStruggle.spriteEvilBat)
		{
			GC.GlobalApplyDamageToPlayer(Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(15f, GC.playerMaxHealth)), Vector3.zero, afterEroEscaped: false, bypassDefense: true);
			UIStruggle.iconMaya.sprite = UIStruggle.spriteMayaAnnoyed;
			GC.CallReferenceMayaVoiceGC("hurtPain13", "STG", "STG");
		}
		else if (thisIcon == UIStruggle.spriteEvilLust)
		{
			GC.GlobalApplyStruggleAmountToPlayer(-10f);
			UIStruggle.iconMaya.sprite = UIStruggle.spriteMayaClimax;
			GC.CallReferenceMayaVoiceGC("moanMedium3", "STG", "STG");
		}
		else if (thisIcon == UIStruggle.spriteSpawnSpeed)
		{
			STG_ToggleInvokeSpawnIcons(turnON: false);
			StopCoroutine(crtWaitBoardHasIcons);
			crtWaitBoardHasIcons = null;
			if (freqInvokedIconSpawn > 3f)
			{
				freqInvokedIconSpawn--;
			}
			STG_ToggleInvokeSpawnIcons(turnON: true);
			UIStruggle.iconMaya.sprite = UIStruggle.spriteMayaDefault;
			GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
	}

	private float STG_UPD_GetMayaMoveSpeed()
	{
		return 75f;
	}

	private void STG_IVK_IconsMainGenerationLogic()
	{
		List<int> list = new List<int>();
		STG_WipeBoardClean(isResetTotalWipe: false);
		crtWaitBoardHasIcons = StartCoroutine(waitBoardHasIcons(freqInvokedIconSpawn - 0.5f));
		for (int i = 0; i < UIStruggle.arrayIconsPos.Length; i++)
		{
			if ((isMayaIconMoving && UIStruggle.arrayIconsPos[i].transform.localPosition == posTargetBoardMove) || (!isMayaIconMoving && UIStruggle.arrayIconsPos[i].transform.localPosition == posIconMaya))
			{
				list.Add(i);
				break;
			}
		}
		int[] array = GC.MATH_RNG_TYPE_ARRAY(UIStruggle.arrayIconsPos, GC.MATH_RNG_INT(3, 5, "STGGen1"), list);
		List<Sprite> list2 = new List<Sprite>();
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		do
		{
			if (num < 1)
			{
				num++;
				if (GC.MATH_RNG_INT(1, 2, "STGgrp1") == 1)
				{
					list2.Add(UIStruggle.spritePunchRelease);
				}
				else
				{
					list2.Add(UIStruggle.spriteLoveEssence);
				}
			}
			if (num2 < 1 && GC.MATH_RNG_ROLL(15))
			{
				num2++;
				if (GC.MATH_RNG_INT(1, 2, "STGgrp2") == 1)
				{
					list2.Add(UIStruggle.spriteHP);
				}
				else
				{
					list2.Add(UIStruggle.spriteSP);
				}
			}
			if (GC.MATH_RNG_INT(1, 2, "STGgrp3") == 1)
			{
				list2.Add(UIStruggle.spriteEvilBat);
			}
			else
			{
				list2.Add(UIStruggle.spriteEvilLust);
			}
			if (num < 2 && GC.MATH_RNG_ROLL(5))
			{
				num++;
				list2.Add(UIStruggle.spriteLoveEssence);
			}
			if (num3 < 1 && GC.MATH_RNG_ROLL(5) && freqInvokedIconSpawn > 3f)
			{
				num3++;
				list2.Add(UIStruggle.spriteSpawnSpeed);
			}
		}
		while (list2.Count < array.Length);
		for (int j = 0; j < array.Length; j++)
		{
			UIStruggle.arrayIconsPos[array[j]].sprite = list2[j];
			UIStruggle.arrayIconsPos[array[j]].color = colorHidden;
		}
	}

	private void STG_UPD_FadeInOutBoardIcons()
	{
		for (int i = 0; i < UIStruggle.arrayIconsPos.Length; i++)
		{
			if (UIStruggle.arrayIconsPos[i].sprite != null)
			{
				if (crtWaitBoardHasIcons != null && UIStruggle.arrayIconsPos[i].color != Color.white)
				{
					STG_UPD_ApplyFade(isAppear: true, i, 3f);
				}
				if (crtWaitBoardHasIcons == null && UIStruggle.arrayIconsPos[i].color != colorHidden)
				{
					STG_UPD_ApplyFade(isAppear: false, i, 3f);
				}
			}
			else if (UIStruggle.arrayIconsPos[i].color != colorHidden)
			{
				STG_UPD_ApplyFade(isAppear: false, i, 6f);
			}
		}
	}

	private void STG_UPD_ApplyFade(bool isAppear, int index, float fadeSpeed)
	{
		if (isAppear)
		{
			UIStruggle.arrayIconsPos[index].color = new Color(1f, 1f, 1f, Mathf.MoveTowards(UIStruggle.arrayIconsPos[index].color.a, 1f, fadeSpeed * Time.deltaTime));
		}
		else
		{
			UIStruggle.arrayIconsPos[index].color = new Color(1f, 1f, 1f, Mathf.MoveTowards(UIStruggle.arrayIconsPos[index].color.a, 0f, fadeSpeed * Time.deltaTime));
		}
	}

	private void STG_WipeBoardClean(bool isResetTotalWipe)
	{
		Image[] arrayIconsPos = UIStruggle.arrayIconsPos;
		foreach (Image image in arrayIconsPos)
		{
			if (isResetTotalWipe)
			{
				image.sprite = null;
				image.color = colorHidden;
			}
			else if (image.sprite != null && image.color == colorHidden)
			{
				image.sprite = null;
			}
		}
	}

	private void STG_InitializeArrayPositions()
	{
		UIStruggle.arrayIconsPos[0].transform.localPosition = new Vector3(-16f, 16f, 0f);
		UIStruggle.arrayIconsPos[1].transform.localPosition = new Vector3(0f, 16f, 0f);
		UIStruggle.arrayIconsPos[2].transform.localPosition = new Vector3(16f, 16f, 0f);
		UIStruggle.arrayIconsPos[3].transform.localPosition = new Vector3(-16f, 0f, 0f);
		UIStruggle.arrayIconsPos[4].transform.localPosition = new Vector3(0f, 0f, 0f);
		UIStruggle.arrayIconsPos[5].transform.localPosition = new Vector3(16f, 0f, 0f);
		UIStruggle.arrayIconsPos[6].transform.localPosition = new Vector3(-16f, -16f, 0f);
		UIStruggle.arrayIconsPos[7].transform.localPosition = new Vector3(0f, -16f, 0f);
		UIStruggle.arrayIconsPos[8].transform.localPosition = new Vector3(16f, -16f, 0f);
	}

	private IEnumerator waitBoardHasIcons(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtWaitBoardHasIcons = null;
	}

	private void STG_UpdateWatchForBarStyleChange()
	{
		if (UIStruggle.StruggleBarBG.IsActive() && (bool)GC && GC.currentEnemyGrabbingIsEroFatal != boolLastKnowErOFatalState)
		{
			STG_ChangeStyleOfStrugglebar(GC.currentEnemyGrabbingIsEroFatal);
		}
	}

	private void STG_ChangeStyleOfStrugglebar(bool isEroFatal)
	{
		if (!isEroFatal)
		{
			UIStruggle.StruggleBarFrame.sprite = UIStruggle.StruggleNormalFrame;
			UIStruggle.StruggleBarFill.sprite = UIStruggle.StruggleNormalFill;
			UIStruggle.StruggleBarBG.sprite = UIStruggle.StruggleNormalBG;
		}
		else
		{
			UIStruggle.StruggleBarFrame.sprite = UIStruggle.StruggleFatalFrame;
			UIStruggle.StruggleBarFill.sprite = UIStruggle.StruggleFatalFill;
			UIStruggle.StruggleBarBG.sprite = UIStruggle.StruggleFatalBG;
		}
		boolLastKnowErOFatalState = isEroFatal;
	}

	private void STG_DelayStruggleBarAppearance(bool start)
	{
		Color color = ((!start) ? Color.white : new Color(1f, 1f, 1f, 0f));
		UIStruggle.StruggleBarBG.color = color;
		UIStruggle.StruggleBarFill.color = color;
		UIStruggle.StruggleBarFrame.color = color;
		if (start)
		{
			if (crtDelayStruggleBar != null)
			{
				StopCoroutine(crtDelayStruggleBar);
			}
			crtDelayStruggleBar = StartCoroutine(waitStruggleBarDelay(0.1f));
		}
	}

	private void STG_ToggleButtonMashingIconLogic(bool turnON)
	{
		STG_Reset_Icon_Moving(didCollect: false);
		if (turnON)
		{
			freqInvokedOldIconSpawn = 0.5f;
			InvokeRepeating("STG_INVK_SpawnIconLogic", 0f, freqInvokedOldIconSpawn);
		}
		else if (IsInvoking("STG_INVK_SpawnIconLogic"))
		{
			CancelInvoke("STG_INVK_SpawnIconLogic");
		}
	}

	private void STG_INVK_SpawnIconLogic()
	{
		if (isIconStruggleMoving)
		{
			return;
		}
		if (GC.MATH_RNG_ROLL(chanceIconCurrent))
		{
			posIconTarget = UIStruggle.iconCenterCollect.transform.localPosition;
			posIconHeart = UIStruggle.iconCenterCollect.transform.localPosition;
			if (GC.MATH_RNG_ROLL(50))
			{
				posIconHeart.x += rangeIconStart;
				posIconTarget.x -= rangeIconStart;
			}
			else
			{
				posIconHeart.x -= rangeIconStart;
				posIconTarget.x += rangeIconStart;
			}
			UIStruggle.iconHeartCollect.transform.localPosition = posIconHeart;
			UIStruggle.iconCenterCollect.color = Color.white;
			UIStruggle.iconHeartCollect.color = Color.white;
			isIconStruggleMoving = true;
			GC.PlaySFX(GameCore.SFXBasic1.HeartIconSpawn, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
		}
		else
		{
			chanceIconCurrent += chanceIconBase;
		}
	}

	private void STG_UPD_DoHeartIconMove()
	{
		posIconHeart = new Vector3(Mathf.MoveTowards(posIconHeart.x, posIconTarget.x, speedHeartMove * Time.deltaTime), posIconHeart.y, posIconHeart.z);
		UIStruggle.iconHeartCollect.transform.localPosition = posIconHeart;
		if (posIconTarget.x < UIStruggle.iconCenterCollect.transform.localPosition.x && posIconHeart.x <= posIconTarget.x)
		{
			STG_Reset_Icon_Moving(didCollect: false);
		}
		if (posIconTarget.x > UIStruggle.iconCenterCollect.transform.localPosition.x && posIconHeart.x >= posIconTarget.x)
		{
			STG_Reset_Icon_Moving(didCollect: false);
		}
	}

	private void STG_Reset_Icon_Moving(bool didCollect, bool perfectCollect = false, bool failZone = false)
	{
		if (didCollect)
		{
			int num = 20;
			if (perfectCollect)
			{
				num = 40;
			}
			if (GC.ACC_Check(GameCore.AccessoryBehavior.HairFertility))
			{
				num += 20;
			}
			Vector3 spawnPos = new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, 0f);
			GC.ESSENCE_Spawn(GameCore.EssenceSpawnType.EroGrab, spawnPos, GC.objEnemyGrabbingPlayer.GetComponent<EnemyGeneralLogic>().enmBaseStats.maxLife, num);
			GC.PlaySFX(GameCore.SFXBasic1.CrystalEngaged, 0, checkForDuplicity: false, Vector2.zero, 1.4f, 1.5f);
		}
		if (failZone)
		{
			GC.GlobalApplyStruggleAmountToPlayer(-35f);
			GC.PlaySFX(GameCore.SFXBasic1.uiWrongError, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
		}
		speedHeartMove = 300f;
		rangeIconStart = 725f;
		rangeCollectPerfect = 12.5f;
		rangeCollectNear = 35f;
		rangeCollectMiss = 115f;
		chanceIconBase = 1;
		if (GC.playerLust > 20)
		{
			chanceIconBase++;
		}
		if (GC.playerLust > 40)
		{
			chanceIconBase++;
		}
		if (GC.playerLust > 60)
		{
			chanceIconBase++;
		}
		if (GC.playerLust > 80)
		{
			chanceIconBase++;
		}
		if (GC.ACC_Check(GameCore.AccessoryBehavior.HairFertility))
		{
			chanceIconBase *= 2;
		}
		chanceIconCurrent = 0;
		UIStruggle.iconCenterCollect.color = colorHidden;
		UIStruggle.iconHeartCollect.color = colorHidden;
		isIconStruggleMoving = false;
	}

	private bool STG_IconCollectInRange(float rangeCheck)
	{
		if (UIStruggle.iconHeartCollect.transform.localPosition.x <= UIStruggle.iconCenterCollect.transform.localPosition.x + rangeCheck && UIStruggle.iconHeartCollect.transform.localPosition.x >= UIStruggle.iconCenterCollect.transform.localPosition.x - rangeCheck)
		{
			return true;
		}
		return false;
	}

	public void STG_PlayerPressedUpOnStruggle()
	{
		if (isIconStruggleMoving)
		{
			if (STG_IconCollectInRange(rangeCollectPerfect))
			{
				STG_Reset_Icon_Moving(didCollect: true, perfectCollect: true);
			}
			else if (STG_IconCollectInRange(rangeCollectNear))
			{
				STG_Reset_Icon_Moving(didCollect: true);
			}
			else if (STG_IconCollectInRange(rangeCollectMiss))
			{
				STG_Reset_Icon_Moving(didCollect: false, perfectCollect: false, failZone: true);
			}
		}
	}

	private IEnumerator waitStruggleBarDelay(float waittime)
	{
		yield return new WaitForSeconds(waittime);
		STG_DelayStruggleBarAppearance(start: false);
		crtDelayStruggleBar = null;
	}

	public void AddSystemMessage(string newMSG, float runTime, bool isUniqueFlavorMessageCentralized)
	{
		if (runTime <= 0f)
		{
			runTime = 3f;
		}
		newMSG = GC.ParseDialogueLineTags(newMSG);
		if (isUniqueFlavorMessageCentralized)
		{
			SetupUniqueFlavorSystemMessage(newMSG, runTime);
			return;
		}
		if (newMSG != clsSysMessages[0].strMessage)
		{
			for (int num = clsSysMessages.Length - 1; num >= 0; num--)
			{
				if (num - 1 >= 0 && clsSysMessages[num - 1].strMessage != "")
				{
					clsSysMessages[num].strMessage = clsSysMessages[num - 1].strMessage;
					clsSysMessages[num].startTime = clsSysMessages[num - 1].startTime;
					clsSysMessages[num].endTime = clsSysMessages[num - 1].endTime;
				}
			}
		}
		clsSysMessages[0].strMessage = newMSG;
		clsSysMessages[0].startTime = countFractionSecond;
		clsSysMessages[0].endTime = countFractionSecond + runTime;
		UpdateSystemMessageDisplay();
	}

	private void UpdateSystemMessageDisplay()
	{
		string text = "";
		for (int num = clsSysMessages.Length - 1; num >= 0; num--)
		{
			if (clsSysMessages[num].strMessage != "")
			{
				if (num == 0)
				{
					text += clsSysMessages[num].strMessage;
				}
				else
				{
					text = text + "<color=grey>" + clsSysMessages[num].strMessage + "</color>";
					text += "\r\n";
				}
			}
		}
		UIMisc.SystemMessageText.text = text;
	}

	private void FXU_HandleSysMsgWindow()
	{
		if (isShowSystemMessage && UIMisc.SystemMessagePainel.transform.position.y < 25f)
		{
			UIMisc.SystemMessagePainel.transform.position = new Vector3(UIMisc.SystemMessagePainel.transform.position.x, Mathf.SmoothStep(UIMisc.SystemMessagePainel.transform.position.y, 25f, speedDisplaySysMsg * Time.deltaTime), UIMisc.SystemMessagePainel.transform.position.z);
		}
		else if (!isShowSystemMessage && UIMisc.SystemMessagePainel.transform.position.y > -100f)
		{
			UIMisc.SystemMessagePainel.transform.position = new Vector3(UIMisc.SystemMessagePainel.transform.position.x, Mathf.SmoothStep(UIMisc.SystemMessagePainel.transform.position.y, -100f, speedDisplaySysMsg * Time.deltaTime), UIMisc.SystemMessagePainel.transform.position.z);
		}
	}

	private void FXU_HandleFlavorSysMsgWindow()
	{
		if (crtShowFlavorMessage != null && !UIMisc.SystemFlavorPainel.gameObject.activeInHierarchy)
		{
			UIMisc.SystemFlavorPainel.gameObject.SetActive(value: true);
		}
		if (crtShowFlavorMessage == null && UIMisc.SystemFlavorPainel.gameObject.activeInHierarchy)
		{
			UIMisc.SystemFlavorPainel.gameObject.SetActive(value: false);
		}
	}

	private void RemoveSystemMessage(int index)
	{
		clsSysMessages[index].Reset();
		UpdateSystemMessageDisplay();
	}

	private void InvokedCountSeconds()
	{
		countFractionSecond += freqInvokeCountSeconds;
		maxCurrentShowTime = 0f;
		for (int i = 0; i < clsSysMessages.Length; i++)
		{
			if (countFractionSecond >= clsSysMessages[i].endTime && i > 0)
			{
				RemoveSystemMessage(i);
			}
			if (clsSysMessages[i].endTime > maxCurrentShowTime)
			{
				maxCurrentShowTime = clsSysMessages[i].endTime;
			}
		}
		if (countFractionSecond < maxCurrentShowTime)
		{
			isShowSystemMessage = true;
		}
		else
		{
			isShowSystemMessage = false;
		}
	}

	private void SetupUniqueFlavorSystemMessage(string text, float displayTime)
	{
		UIMisc.SystemFlavorText.text = text;
		if (crtShowFlavorMessage != null)
		{
			StopCoroutine(crtShowFlavorMessage);
		}
		crtShowFlavorMessage = StartCoroutine(waitSystemFlavorMessage(displayTime));
	}

	private IEnumerator waitSystemFlavorMessage(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtShowFlavorMessage = null;
	}

	public void StartDialogue(DialogueList filteredDialogueList, GameCore.NPCType sentNPC, bool IsZoomOnDialogue)
	{
		currentDialogueIndex = 0;
		listDialogue = filteredDialogueList;
		currentDialogueCameraMove = Vector2.zero;
		CheckGameCoreRef();
		if ((bool)GC)
		{
			DisplayNextDialogue();
			UIDialogue.Window.gameObject.SetActive(value: true);
			UIDialogue.Window.transform.position = posDialogueBoxOriginal;
			isDisplayingDialogue = true;
			GC.isDisplayingDialogue = isDisplayingDialogue;
			if (!GC.eventIsPlaying)
			{
				GC.isPlayerInteractingLocked = true;
			}
			currentNPC = sentNPC;
		}
		if (CheckCameraCoreRef() & IsZoomOnDialogue)
		{
			coreCAM.InitiateZoom(ControllerCamera.ZoomType.Dialogue, hasTimeLimit: false, 0f);
		}
	}

	public void DisplayNextDialogue()
	{
		if (coroutTypeCharacters != null)
		{
			StopCoroutine(coroutTypeCharacters);
			coroutTypeCharacters = null;
			UIDialogue.MainText.text = textToDisplay;
			CheckIconToDisplay();
			return;
		}
		if (listDialogue != null && listDialogue.listOfDialogue != null && currentDialogueIndex >= listDialogue.listOfDialogue.Count)
		{
			FinishDialogue();
			return;
		}
		if (listDialogue.listOfDialogue[currentDialogueIndex].speakerName.Trim() == "!@!")
		{
			int actionID = int.Parse(listDialogue.listOfDialogue[currentDialogueIndex].speakerDialogue.Trim().Substring(3, 3));
			GC.DIAG_DoSpecialActionsAndTriggers(actionID);
			currentDialogueIndex++;
			DisplayNextDialogue();
			return;
		}
		CheckForDialogueAnimationActions();
		CheckForDialogueSoundActions();
		_letterSoundTone = listDialogue.listOfDialogue[currentDialogueIndex].letterTone;
		if (currentDialogueCameraMove != listDialogue.listOfDialogue[currentDialogueIndex].cameraMovement)
		{
			DialogueCameraMoveLogic();
		}
		textNameDisplay = listDialogue.listOfDialogue[currentDialogueIndex].speakerName.Trim();
		textToDisplay = GC.ParseDialogueLineTags(listDialogue.listOfDialogue[currentDialogueIndex].speakerDialogue.Trim());
		UIDialogue.SpeakerName.text = textNameDisplay;
		UIDialogue.SpeakerName.color = listDialogue.listOfDialogue[currentDialogueIndex].speakerColorRGB;
		if (listDialogue.listOfDialogue[currentDialogueIndex].isInstantText)
		{
			UIDialogue.MainText.text = textToDisplay;
			CheckIconToDisplay(disable: false, CheckIndexEarly: true);
		}
		else
		{
			float num = 0f;
			coroutTypeCharacters = StartCoroutine(TypeSentence(interval: GC.configTextSpeed switch
			{
				1 => 0.02f, 
				2 => 0.005f, 
				_ => 0.035f, 
			}, sentence: textToDisplay, playLetterSound: _letterSoundTone));
		}
		if (currentDialogueIndex > 0)
		{
			GC.PlaySFX(GameCore.SFXBasic1.DialogueNext, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
		currentDialogueIndex++;
	}

	public void FinishDialogue()
	{
		if ((bool)coreCAM)
		{
			coreCAM.ManualEndZoomEffect();
		}
		GC.FinishAllDialogueActionsVariables();
		DialogueCameraMoveLogic(resetDefault: true);
		UIDialogue.Window.gameObject.SetActive(value: false);
		UIDialogue.IconNext.gameObject.SetActive(value: false);
		UIDialogue.IconEnd.gameObject.SetActive(value: false);
		UIDialogue.IconNPCMenu.gameObject.SetActive(value: false);
		if ((bool)GC && !GC.eventIsPlaying)
		{
			GC.isPlayerInteractingLocked = false;
		}
		isDisplayingDialogue = false;
		GC.isDisplayingDialogue = isDisplayingDialogue;
		GC.isDisplayingNPCMenuTalkDialogue = isDisplayingDialogue;
		textToDisplay = "";
		if (coroutTypeCharacters != null)
		{
			StopCoroutine(coroutTypeCharacters);
			coroutTypeCharacters = null;
		}
		GC.ToggleSpecialMenuNPC(currentNPC, state: false);
		GC.HandleNextDialogueID(currentNPC);
		GC.PlaySFX(GameCore.SFXBasic1.DialogueEnd, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
	}

	private IEnumerator TypeSentence(string sentence, float interval, GameCore.SpeakerID playLetterSound)
	{
		UIDialogue.MainText.text = "";
		CheckIconToDisplay(disable: true);
		char[] array = sentence.ToCharArray();
		foreach (char c in array)
		{
			UIDialogue.MainText.text += c;
			GC.PlayLetterSoundTone(playLetterSound);
			yield return new WaitForSecondsRealtime(interval);
		}
		coroutTypeCharacters = null;
		CheckIconToDisplay();
	}

	private void CheckForDialogueAnimationActions()
	{
		if (listDialogue.listOfDialogue[currentDialogueIndex].actionList.Count > 0)
		{
			GC.ManageDialogueAnimationsActions(listDialogue.listOfDialogue[currentDialogueIndex].actionList);
		}
	}

	private void CheckForDialogueSoundActions()
	{
		if (listDialogue.listOfDialogue[currentDialogueIndex].soundList.Count > 0)
		{
			GC.ManageDialogueSoundsActions(listDialogue.listOfDialogue[currentDialogueIndex].soundList);
		}
	}

	private void CheckIconToDisplay(bool disable = false, bool CheckIndexEarly = false)
	{
		int num = 0;
		if (disable)
		{
			UIDialogue.IconNext.gameObject.SetActive(value: false);
			UIDialogue.IconEnd.gameObject.SetActive(value: false);
			UIDialogue.IconNPCMenu.gameObject.SetActive(value: false);
			return;
		}
		if (CheckIndexEarly)
		{
			num = -1;
		}
		if (currentDialogueIndex >= listDialogue.listOfDialogue.Count + num)
		{
			UIDialogue.IconNext.gameObject.SetActive(value: false);
			UIDialogue.IconEnd.gameObject.SetActive(value: true);
			UIDialogue.IconNPCMenu.gameObject.SetActive(value: false);
			GC.isDisplayingNPCMenuTalkDialogue = false;
		}
		else
		{
			UIDialogue.IconNext.gameObject.SetActive(value: true);
			UIDialogue.IconEnd.gameObject.SetActive(value: false);
			UIDialogue.IconNPCMenu.gameObject.SetActive(value: false);
		}
		if (GC.isDisplayingSpecialMenuNPC && !GC.isDisplayingNPCMenuTalkDialogue)
		{
			UIDialogue.IconNext.gameObject.SetActive(value: false);
			UIDialogue.IconEnd.gameObject.SetActive(value: false);
			UIDialogue.IconNPCMenu.gameObject.SetActive(value: true);
		}
	}

	private void DialogueCameraMoveLogic(bool resetDefault = false)
	{
		if (currentDialogueIndex < listDialogue.listOfDialogue.Count)
		{
			currentDialogueCameraMove = listDialogue.listOfDialogue[currentDialogueIndex].cameraMovement;
		}
		else
		{
			currentDialogueCameraMove = Vector2.zero;
		}
		if (currentDialogueCameraMove == Vector2.zero)
		{
			resetDefault = true;
		}
		if (CheckCameraCoreRef())
		{
			if (resetDefault)
			{
				coreCAM.StopCameraFixedDirectionOffset(ControllerCamera.camDirection.Custom);
			}
			else
			{
				coreCAM.MoveCameraFixedDirectionOffset(ControllerCamera.camDirection.Custom, 0f, 0.75f, currentDialogueCameraMove, isDialogueChange: true);
			}
		}
	}

	public void SetDialogueWindowPosition(Vector3 newPos, bool doDefault = false)
	{
		if (doDefault)
		{
			UIDialogue.Window.transform.position = posDialogueBoxOriginal;
		}
		else
		{
			UIDialogue.Window.transform.position = newPos;
		}
	}

	public void FadeInOutBlackScreen(bool FadeIntoBlack, float durationMainFadeIN, float durationOptionalOUT = 0f)
	{
		if (IsInvoking("FadeIn"))
		{
			CancelInvoke("FadeIn");
		}
		if (IsInvoking("FadeOut"))
		{
			CancelInvoke("FadeOut");
		}
		if (FadeIntoBlack)
		{
			colorBlackScreen.a = 0f;
			UIMainOverlay.BlackScreen.color = colorBlackScreen;
			UIMainOverlay.BlackScreen.gameObject.SetActive(value: true);
			rateFadeInOut = 1f / (durationMainFadeIN / freqInvokeFade);
			InvokeRepeating("FadeIn", 0f, freqInvokeFade);
			return;
		}
		if (durationOptionalOUT <= 0f)
		{
			durationOptionalOUT = durationMainFadeIN;
		}
		colorBlackScreen.a = 1f;
		UIMainOverlay.BlackScreen.color = colorBlackScreen;
		UIMainOverlay.BlackScreen.gameObject.SetActive(value: true);
		rateFadeInOut = 1f / (durationOptionalOUT / freqInvokeFade);
		InvokeRepeating("FadeOut", 0f, freqInvokeFade);
	}

	private void FadeIn()
	{
		colorBlackScreen.a += rateFadeInOut;
		UIMainOverlay.BlackScreen.color = colorBlackScreen;
		if (UIMainOverlay.BlackScreen.color.a >= 1f)
		{
			GC.FadeIn_SignalFinished();
			CancelInvoke("FadeIn");
		}
	}

	private void FadeOut()
	{
		colorBlackScreen.a -= rateFadeInOut;
		UIMainOverlay.BlackScreen.color = colorBlackScreen;
		if (UIMainOverlay.BlackScreen.color.a <= 0f)
		{
			UIMainOverlay.BlackScreen.gameObject.SetActive(value: false);
			CancelInvoke("FadeOut");
		}
	}

	public void DeactivateCanvasDuringLoad(bool dontBlackScreen = false)
	{
		ToggleMainBasicUI(setThisState: false);
		if (!dontBlackScreen)
		{
			UIMainOverlay.BlackScreen.gameObject.SetActive(value: true);
			UIMainOverlay.BlackScreen.color = Color.black;
		}
	}

	public void FadeOutMirrorScreen(float duration, bool colorRed = false)
	{
		if (IsInvoking("FadeOutMirror"))
		{
			CancelInvoke("FadeOutMirror");
		}
		colorMirrorScreen = new Color(0.5f, 0.5f, 0.5f, 1f);
		if (colorRed)
		{
			colorMirrorScreen = new Color(0.5f, 0f, 0f, 1f);
		}
		UIMainOverlay.MirrorScreen.color = colorMirrorScreen;
		UIMainOverlay.MirrorScreen.gameObject.SetActive(value: true);
		rateFadeInOut = 1f / (duration / freqInvokeFade);
		InvokeRepeating("FadeOutMirror", 0f, freqInvokeFade);
	}

	private void FadeOutMirror()
	{
		colorMirrorScreen.a -= rateFadeInOut;
		UIMainOverlay.MirrorScreen.color = colorMirrorScreen;
		if (UIMainOverlay.MirrorScreen.color.a <= 0f)
		{
			UIMainOverlay.MirrorScreen.gameObject.SetActive(value: false);
			CancelInvoke("FadeOutMirror");
		}
	}

	public void ShowStageNameDelayed()
	{
		if (crtShowStageNameDelayed != null)
		{
			StopCoroutine(crtShowStageNameDelayed);
		}
		crtShowStageNameDelayed = StartCoroutine(waitBeforeShowStageGame(0.1f));
	}

	private IEnumerator waitBeforeShowStageGame(float wait)
	{
		yield return new WaitForSeconds(wait);
		if ((bool)UIMisc.StageNamePainel && (bool)UIMisc.StageName)
		{
			colorFadeSN1 = UIMisc.StageNamePainel.color;
			colorFadeSN1.a = 0f;
			UIMisc.StageNamePainel.color = colorFadeSN1;
			colorFadeSN2 = UIMisc.StageName.color;
			colorFadeSN2.a = 0f;
			UIMisc.StageName.color = colorFadeSN2;
			isShowStageName = true;
			if (crtShowStageName != null)
			{
				StopCoroutine(crtShowStageName);
			}
			crtShowStageName = StartCoroutine(waitDelayShowStageName(3.25f));
		}
	}

	private IEnumerator waitDelayShowStageName(float wait)
	{
		yield return new WaitForSeconds(wait);
		isShowStageName = false;
		crtShowStageName = null;
	}

	public GameObject GetVisualEffectOBJ(GameCore.VisualEffect thisVFX)
	{
		if ((bool)UIMisc.ListVFX)
		{
			return UIMisc.ListVFX.ReturnVisualEffectByType(thisVFX);
		}
		Debug.Log("Could not find reference to the VisualEffect sriptable object!");
		return null;
	}

	public void CallShakeTheCamera(float duration, float magnitude, float damping)
	{
		if (CheckCameraCoreRef())
		{
			coreCAM.ShakeTheCamera(duration, magnitude, damping);
		}
	}

	public void ShowDisplayStatusEffectUIBox(GameCore.TempPlayerEffect thisEffect, float countUpTimer, float maxDuration)
	{
		Sprite sprite = null;
		GameObject original = UIDebuff.objStatusEffectBoxBUFF;
		switch (thisEffect)
		{
		case GameCore.TempPlayerEffect.DrinkLifeRegen:
			sprite = UIDebuff.spriteDrinkRegenLife;
			break;
		case GameCore.TempPlayerEffect.DrinkManaRegen:
			sprite = UIDebuff.spriteDrinkRegenSP;
			break;
		case GameCore.TempPlayerEffect.DrinkATK:
			sprite = UIDebuff.spriteDrinkATK;
			break;
		case GameCore.TempPlayerEffect.DrinkDEF:
			sprite = UIDebuff.spriteDrinkDEF;
			break;
		case GameCore.TempPlayerEffect.DrinkSpeed:
			sprite = UIDebuff.spriteDrinkSPEED;
			break;
		case GameCore.TempPlayerEffect.FruitSlice:
			sprite = UIDebuff.spriteFruitSlice;
			break;
		case GameCore.TempPlayerEffect.FruitDistract:
			sprite = UIDebuff.spriteFruitDistract;
			break;
		case GameCore.TempPlayerEffect.FruitWarrior:
			sprite = UIDebuff.spriteFruitWarrior;
			break;
		case GameCore.TempPlayerEffect.MealHero:
			sprite = UIDebuff.spriteHeroMeal;
			break;
		case GameCore.TempPlayerEffect.Charmed:
			sprite = UIDebuff.spriteCharmed;
			original = UIDebuff.objStatusEffectBoxDEBUFF;
			break;
		case GameCore.TempPlayerEffect.Petrifying:
			sprite = UIDebuff.spritePetrifying;
			original = UIDebuff.objStatusEffectBoxDEBUFF;
			break;
		case GameCore.TempPlayerEffect.PullGrab:
			sprite = UIDebuff.spritePullGrab;
			original = UIDebuff.objStatusEffectBoxDEBUFF;
			break;
		case GameCore.TempPlayerEffect.Stuned:
			sprite = UIDebuff.spriteStun;
			original = UIDebuff.objStatusEffectBoxDEBUFF;
			break;
		case GameCore.TempPlayerEffect.WerewolfBossCurse:
			sprite = UIDebuff.spriteWerewolfCurse;
			original = UIDebuff.objStatusEffectBoxDEBUFF;
			break;
		case GameCore.TempPlayerEffect.Exhibitionist:
			sprite = UIDebuff.spriteExhibi;
			original = UIDebuff.objStatusEffectBoxDEBUFF;
			break;
		}
		if (sprite != null)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate(original, Vector3.zero, Quaternion.identity, UIDebuff.StatusEffectContainerUI);
			StatusEffectUI component = gameObject.GetComponent<StatusEffectUI>();
			component.setThisSpriteIcon = sprite;
			component.thisStatusEffect = thisEffect;
			component.countUp = countUpTimer;
			component.maxDuration = maxDuration;
			GC.StatusEffectRefenreceToUIElement(gameObject, thisEffect);
		}
	}

	public void ToggleWerewolfCurse(bool setThisState)
	{
		if (setThisState)
		{
			timerBossWWCurseCurrent = timerBossWWCurseMax;
			UIDebuff.BossWerewolfCurse.gameObject.SetActive(value: true);
			InvokeRepeating("InvokedWerewolfCurseCountdown", 0f, 1f);
		}
		else
		{
			UIDebuff.BossWerewolfCurse.gameObject.SetActive(value: false);
			CancelInvoke("InvokedWerewolfCurseCountdown");
		}
	}

	private void InvokedWerewolfCurseCountdown()
	{
		timerBossWWCurseCurrent--;
	}

	private void ToggleAnimationControlWindow()
	{
		if (GC.ValidateAnimationControlWindow())
		{
			if (!UIAnimControl.objAnimControlWindow.activeInHierarchy)
			{
				UIAnimControl.objAnimControlWindow.SetActive(value: true);
				UIAnimControl.btnPlay.Select();
			}
		}
		else if (UIAnimControl.objAnimControlWindow.activeInHierarchy)
		{
			UIAnimControl.objAnimControlWindow.SetActive(value: false);
		}
	}

	public void AnimControlButtonPressed(int action)
	{
		GC.RelayAnimControlAction(action);
		ToggleLoopLock(action);
	}

	private void ToggleLoopLock(int thisAction)
	{
		switch (thisAction)
		{
		case 1:
			if (UIAnimControl.imgLoopLock.activeInHierarchy)
			{
				UIAnimControl.imgLoopLock.SetActive(value: false);
			}
			break;
		case 2:
			if (!UIAnimControl.imgLoopLock.activeInHierarchy)
			{
				UIAnimControl.imgLoopLock.SetActive(value: true);
			}
			break;
		}
	}

	private void CheckGameCoreRef()
	{
		GC = GameCore.Instance;
	}

	private bool CheckCameraCoreRef()
	{
		if (coreCAM != null)
		{
			return true;
		}
		GameObject gameObject = GameObject.Find("Main Camera");
		if (gameObject != null)
		{
			coreCAM = gameObject.GetComponent<ControllerCamera>();
		}
		if (coreCAM != null)
		{
			return true;
		}
		return false;
	}

	public void TutorialToggleBox(bool state, string message)
	{
		tutorialState = state;
		UIMisc.tutorialText.text = message;
		if (!UIMisc.tutorialBox.gameObject.activeInHierarchy)
		{
			UIMisc.tutorialBox.gameObject.SetActive(value: true);
		}
	}

	private void TutorialBoxFadeLogic()
	{
		clrTuto1 = UIMisc.tutorialBox.color;
		clrTuto2 = UIMisc.tutorialText.color;
		if (tutorialState)
		{
			if (UIMisc.tutorialBox.gameObject.activeInHierarchy && UIMisc.tutorialBox.color.a < 1f && UIMisc.tutorialText.color.a < 1f)
			{
				clrTuto1.a = Mathf.SmoothStep(clrTuto1.a, 1f, speedShowStageName * 3f * Time.deltaTime);
				UIMisc.tutorialBox.color = clrTuto1;
				clrTuto2.a = Mathf.SmoothStep(clrTuto2.a, 1f, speedShowStageName * 3f * Time.deltaTime);
				UIMisc.tutorialText.color = clrTuto2;
			}
		}
		else if (UIMisc.tutorialBox.gameObject.activeInHierarchy && UIMisc.tutorialBox.color.a > 0f && UIMisc.tutorialText.color.a > 0f)
		{
			clrTuto1.a = Mathf.SmoothStep(clrTuto1.a, 0f, speedShowStageName * 3f * Time.deltaTime);
			UIMisc.tutorialBox.color = clrTuto1;
			clrTuto2.a = Mathf.SmoothStep(clrTuto2.a, 0f, speedShowStageName * 3f * Time.deltaTime);
			UIMisc.tutorialText.color = clrTuto2;
		}
	}

	private void StartDelayForCancelKeyPress()
	{
		GC.isDelayCancelKeyPress = true;
		if (coroutDelayCancelKeyPress != null)
		{
			StopCoroutine(coroutDelayCancelKeyPress);
		}
		coroutDelayCancelKeyPress = StartCoroutine(waitForDelayCancelKeyPress(0.2f));
	}

	private IEnumerator waitForDelayCancelKeyPress(float wait)
	{
		yield return new WaitForSecondsRealtime(wait);
		GC.isDelayCancelKeyPress = false;
		coroutDelayCancelKeyPress = null;
	}
}
