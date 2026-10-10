using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MenuBehaviour : MonoBehaviour
{
	private enum controlTypes
	{
		Difficulty = 0,
		Language = 1,
		Camera = 2,
		Input = 3,
		Resolution = 4
	}

	private enum SaveLoadScreenTypes
	{
		Null = 0,
		Save = 1,
		Load = 2
	}

	public GameCore.MenuScene currentMenuSceneType;

	public Button FirstSelectedOnTitleMenu;

	public Button FirstSelectedOnInGameMenu;

	public Color DefaultColorOfTexts;

	public SavePointDatabase databaseSavePoint;

	public InputGamepadIconList listGPIcon;

	public UI_MainMenuTitle menuMainTitle;

	public UI_MenuInGame menuInGame;

	public UI_MenuOptions menuOptions;

	public UI_MenuControlConfig menuControls;

	public UI_Inventory menuInventory;

	public UI_AdvancedOptions menuAdvanced;

	public UI_SaveLoad menuSaveLoad;

	public UI_StatsScreen menuStats;

	public UI_DressStateWindow menuDressState;

	public UI_FetishConfig menuFetish;

	public UI_FarinaMenu menu_NPC_Farina;

	public UI_NeneMenu menu_NPC_Nene;

	public UI_CloseableWindows closeMenuWindows;

	private float freqInvokeMainBehaviorUpdate;

	private float fltListeningCounter;

	private float fltListeningInverval;

	private bool isListeningForKeybindings;

	private bool isDelayAfterKeyBind;

	private bool isProcessingSaveLoad;

	private bool hasTransformedIntoRetryMenu;

	private bool teleportScreenEnabled;

	private bool galleryScreenEnabled;

	private bool isItemWasAlreadyEquipedOnDiffSlot;

	private string strShiftWasPressedToBind;

	private string timeText;

	private string strHP;

	private string strSP;

	private string strATK;

	private string strDEF;

	private string strESSE;

	private TimeSpan timeSpan;

	private CountItemsQuantity currentItemToBeEquiped;

	private Coroutine coroutDelayCancelKeyPress;

	private Coroutine crtDelayAfterkeybind;

	private Coroutine crtDelayAfterItemEquiped;

	private Coroutine crtHackyEscapePressOnFainted;

	private Coroutine crtFlashText;

	private Coroutine crtDelayPrekeybind;

	private Coroutine crtShopAfterEffect;

	private Coroutine crtHoldShopBuy;

	private Button currentButtonBind;

	private Button currentButtonInventory;

	private Text currentTextBind;

	private Color clrBGselected;

	private Color clrBGUnselected;

	private Color clrLayer0;

	private Color clrLayer1;

	private Color clrLayer2;

	private Vector3 _vecOriginFarinaMenu;

	private Vector3 _vecOriginNeneMenu;

	private Vector3 _vecRefShopAfterEffect;

	private SaveLoadScreenTypes currentSaveLoadScreen;

	private GameCore.InputLib duplicateKey;

	private GameCore GC;

	private GameCore.DifficultyType[] arrayDifficultyType;

	private GameCore.CameraType[] arrayCameraType;

	private GameCore.InputType[] arrayControlType;

	private void Start()
	{
		GC = GameCore.Instance;
		InitializeVariables();
		InitialSetup();
		InvokeRepeating("Invoked_SpecialBehaviorsUpdated", 0f, freqInvokeMainBehaviorUpdate);
	}

	private void InitializeVariables()
	{
		currentSaveLoadScreen = SaveLoadScreenTypes.Null;
		duplicateKey = GameCore.InputLib.Null;
		strShiftWasPressedToBind = "";
		isListeningForKeybindings = false;
		isDelayAfterKeyBind = false;
		isProcessingSaveLoad = false;
		hasTransformedIntoRetryMenu = false;
		teleportScreenEnabled = false;
		galleryScreenEnabled = false;
		isItemWasAlreadyEquipedOnDiffSlot = false;
		crtHackyEscapePressOnFainted = null;
		freqInvokeMainBehaviorUpdate = 0.1f;
		clrBGselected = new Color(1f, 1f, 0.7f, 1f);
		clrBGUnselected = new Color(0.18f, 0.36f, 0.36f, 0.5f);
		clrLayer0 = new Color(0.5f, 1f, 1f, 1f);
		clrLayer1 = new Color(1f, 1f, 0.5f, 1f);
		clrLayer2 = new Color(1f, 0.5f, 0.75f, 1f);
		_vecOriginFarinaMenu = menu_NPC_Farina.objTalkSubHolder.transform.position;
		_vecOriginNeneMenu = menu_NPC_Nene.objTalkSubHolder.transform.position;
	}

	private void InitialSetup()
	{
		switch (currentMenuSceneType)
		{
		case GameCore.MenuScene.InGame:
			RetrieveBaseLists();
			LoadLanguageOfMenu(isLanguageChanged: true);
			ApplyInitialValuesToSelections();
			return;
		case GameCore.MenuScene.IntroStory:
			GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 1f, 1f);
			return;
		}
		GC.PlayMusicTheme(GameCore.MusicTheme.TitleScreen, doFadeIn: true, 1f, doLoop: true);
		GC.ResetGameCoreGlobals();
		GC.MainGameDataInitialize(RenewAll: true);
		RetrieveBaseLists();
		LoadLanguageOfMenu();
		GC.LoadItemDatabaseFile();
		ApplyInitialValuesToSelections();
		CheckFirstButtonToSelect();
		if (GC.isSceneTransitioning)
		{
			GC.isSceneTransitioning = false;
		}
	}

	private void CheckFirstButtonToSelect()
	{
		if (!GC.IsButtonAlreadySelected(FirstSelectedOnTitleMenu))
		{
			FirstSelectedOnTitleMenu.Select();
		}
	}

	private void Update()
	{
		if (isListeningForKeybindings)
		{
			ListeningDecreaseCounter();
		}
		if (GC.isListeningForGeneralKeyBindings != isListeningForKeybindings)
		{
			GC.isListeningForGeneralKeyBindings = isListeningForKeybindings;
		}
		if (GC.delayAfterkeyBound != isDelayAfterKeyBind)
		{
			GC.delayAfterkeyBound = isDelayAfterKeyBind;
		}
		if (isListeningForKeybindings && (bool)GC && GC.isGamepadCfgInput)
		{
			UPDT_SniffGamepadBinding();
		}
		if (closeMenuWindows.windowMainMenu.activeInHierarchy)
		{
			UpdateStatsScreen();
		}
		if (crtShopAfterEffect != null)
		{
			menu_NPC_Nene.txtPriceBought.transform.position = new Vector3(menu_NPC_Nene.txtPriceBought.transform.position.x, Mathf.Lerp(menu_NPC_Nene.txtPriceBought.transform.position.y, menu_NPC_Nene.txtEssenceHeld.transform.position.y + 150f, 1f * Time.deltaTime), menu_NPC_Nene.txtPriceBought.transform.position.z);
		}
		if ((bool)GC && GC.GetCancelButtonUniversalPress() && !GC.isDelayCancelKeyPress && !isListeningForKeybindings && !isDelayAfterKeyBind)
		{
			if (crtHackyEscapePressOnFainted == null)
			{
				crtHackyEscapePressOnFainted = StartCoroutine(waitForHackyEscapePressedOnFainted(0.1f));
			}
			CloseIndividualWindowOnCancelKeyPress();
		}
		if ((GC.isGamePaused || GC.strCurrentActiveSceneName == "MainMenu" || GC.isDisplayingSpecialMenuNPC) && GC.GetSubmitButtonUniversalPress() && !isListeningForKeybindings && !isDelayAfterKeyBind)
		{
			GC.SubmitSelectableClickedUniversal();
			if (crtHoldShopBuy != null)
			{
				StopCoroutine(crtHoldShopBuy);
			}
			crtHoldShopBuy = StartCoroutine(waitHoldShopBuy(0.5f));
		}
		else if (GC.isDisplayingSpecialMenuNPC && GC.lastNPCTypeInteracted == GameCore.NPCType.NeneShop && GC.GetSubmitButtonUniversalPress(isHold: true) && crtHoldShopBuy == null)
		{
			GC.SubmitSelectableClickedUniversal(hasHoldDelay: true, 0.1f);
		}
		if (Input.GetKeyDown(KeyCode.LeftShift) && isListeningForKeybindings)
		{
			strShiftWasPressedToBind = "LEFT";
			OnGUI();
		}
		if (Input.GetKeyDown(KeyCode.RightShift) && isListeningForKeybindings)
		{
			strShiftWasPressedToBind = "RIGHT";
			OnGUI();
		}
		if ((bool)GC && GC.strCurrentActiveSceneName == "AnimGallery")
		{
			if (GC.playerCurrentWorldXPosition > GC.galleryPlayerX_ArenaLimit && menuInGame.btnTextInGameGallery.text != GC.GetTranslatedStringUI(4))
			{
				menuInGame.btnTextInGameGallery.text = GC.GetTranslatedStringUI(4);
			}
			if (GC.playerCurrentWorldXPosition <= GC.galleryPlayerX_ArenaLimit && menuInGame.btnTextInGameGallery.text != GC.GetTranslatedStringUI(103))
			{
				menuInGame.btnTextInGameGallery.text = GC.GetTranslatedStringUI(103);
			}
		}
		if ((bool)GC && menuDressState.txtLayerDescription.isActiveAndEnabled)
		{
			WatchDressLayerButtonSelectionToChangeTextDescription();
		}
		if (menuControls.titleControlConfig.isActiveAndEnabled)
		{
			ListenToggleDebugButtonsConfig();
		}
		UPDT_UnhideNPCTalkWindowAfterSelectedDialogueEnds();
		if (menu_NPC_Nene.objShopNode.activeInHierarchy)
		{
			menu_NPC_Nene.labelEssHeld.text = GC.GetTranslatedStringUI(183) + ":";
			menu_NPC_Nene.txtEssenceHeld.text = GC.playerCurrentEssenceAmount.ToString();
		}
		if ((bool)GC)
		{
			GC.UI_SELECTION_StoreLastSelected();
			GC.UI_SELECTION_HighlightElementIfNoneAreCurrentlySelected();
		}
	}

	private void Invoked_SpecialBehaviorsUpdated()
	{
		if (!teleportScreenEnabled)
		{
			if (GC.CheckPlayerHasItem(100) && !GC.isPlayerFainted)
			{
				teleportScreenEnabled = true;
				menuInGame.btnTextTeleport.transform.parent.gameObject.SetActive(value: true);
			}
			else
			{
				menuInGame.btnTextTeleport.transform.parent.gameObject.SetActive(value: false);
			}
		}
		else if ((GC.isPlayerInteractingLocked || GC.isBossFight || GC.playerIsBeingGrabed || GC.playerIsHurt || !GC.playerIsOnGround || GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Stuned)) && menuInGame.btnTeleportOption.interactable)
		{
			menuInGame.btnTeleportOption.interactable = false;
			menuInGame.btnTextTeleport.color = Color.gray;
		}
		else if (GC.playerIsOnGround && !GC.isPlayerInteractingLocked && !GC.isBossFight && !GC.playerIsBeingGrabed && !GC.playerIsHurt && !GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Stuned) && !menuInGame.btnTeleportOption.interactable)
		{
			menuInGame.btnTeleportOption.interactable = true;
			menuInGame.btnTextTeleport.color = Color.white;
		}
		if (!galleryScreenEnabled)
		{
			if (GC.strCurrentActiveSceneName == "AnimGallery" && GC.LogicCheckGameState(GameCore.LogicCheckType.AnyGalleryItemAcquired))
			{
				menuInGame.btnTextInGameGallery.transform.parent.gameObject.SetActive(value: true);
				galleryScreenEnabled = true;
			}
			else
			{
				menuInGame.btnTextInGameGallery.transform.parent.gameObject.SetActive(value: false);
				galleryScreenEnabled = false;
			}
		}
		if (GC.canPlayerSaveGame)
		{
			if (!menuInGame.btnSaveOption.interactable)
			{
				menuInGame.btnSaveOption.interactable = true;
				menuInGame.btnTextSave.color = Color.white;
			}
		}
		else if (menuInGame.btnSaveOption.interactable)
		{
			menuInGame.btnSaveOption.interactable = false;
			menuInGame.btnTextSave.color = Color.gray;
		}
		if (GC.isPlayerFainted && !hasTransformedIntoRetryMenu)
		{
			hasTransformedIntoRetryMenu = true;
			menuInGame.btnTextRetry.transform.parent.gameObject.SetActive(value: true);
			menuInGame.btnTextInventory.transform.parent.gameObject.SetActive(value: false);
			menuInGame.btnTextSave.transform.parent.gameObject.SetActive(value: false);
			menuInGame.btnTextTeleport.transform.parent.gameObject.SetActive(value: false);
			Navigation navigation = menuInGame.btnContinue.navigation;
			navigation.selectOnDown = menuInGame.btnRetry;
			menuInGame.btnContinue.navigation = navigation;
		}
		if (CheckRefreshUIDressState())
		{
			ApplyTextDressState();
		}
	}

	private IEnumerator waitHoldShopBuy(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtHoldShopBuy = null;
	}

	public void ButtonPressNPCMenu(string action)
	{
		switch (action)
		{
		case "Talk":
			NPC_ToggleTalkListWindow(state: true);
			return;
		case "Heal":
			GC.HealThePlayer(100f);
			GC.BeginDialogueLogic(GameCore.NPCType.FarinaSupport, 900, isZoomDialogue: true);
			return;
		case "Repair":
			if (!GC.CallRestorePlayerArmorLayers(checkValidity: true))
			{
				GC.BeginDialogueLogic(GameCore.NPCType.FarinaSupport, 901, isZoomDialogue: true);
			}
			return;
		case "BackToPreviousWindow":
			if (!GC.isDisplayingNPCMenuTalkDialogue)
			{
				switch (GC.lastNPCTypeInteracted)
				{
				case GameCore.NPCType.FarinaSupport:
					NPC_ToggleTalkListWindow(state: false);
					GC.BeginDialogueLogic(GameCore.NPCType.FarinaSupport, 99999, isZoomDialogue: true);
					break;
				case GameCore.NPCType.NeneShop:
					NPC_ToggleTalkListWindow(state: false);
					GC.BeginDialogueLogic(GameCore.NPCType.NeneShop, 90001, isZoomDialogue: true);
					break;
				}
			}
			return;
		case "Exit":
			GC.ForceFinishDialogue();
			return;
		case "Shop":
			NPC_ToggleShopWindow(state: true);
			return;
		}
		if (!GC.isDisplayingNPCMenuTalkDialogue)
		{
			GC.AddDelayOnStartNPCTalk(0.2f);
			GC.isDisplayingNPCMenuTalkDialogue = true;
			menu_NPC_Farina.objTalkSubHolder.transform.position = new Vector3(_vecOriginFarinaMenu.x, _vecOriginFarinaMenu.y + 1777f, _vecOriginFarinaMenu.z);
			menu_NPC_Nene.objTalkSubHolder.transform.position = new Vector3(_vecOriginNeneMenu.x, _vecOriginNeneMenu.y + 1777f, _vecOriginNeneMenu.z);
			GC.BeginDialogueLogic(GC.lastNPCTypeInteracted, int.Parse(action), isZoomDialogue: true);
			if (!GC.HasSeenReadNPCMenuDialogueAdd(GC.lastNPCTypeInteracted, int.Parse(action), doAdd: true))
			{
				NPC_DoTalkListLogic();
			}
		}
	}

	private void UPDT_UnhideNPCTalkWindowAfterSelectedDialogueEnds()
	{
		switch (GC.lastNPCTypeInteracted)
		{
		case GameCore.NPCType.FarinaSupport:
			if (menu_NPC_Farina.objTalkNode.activeInHierarchy && menu_NPC_Farina.objTalkSubHolder.transform.position != _vecOriginFarinaMenu && GC.isDisplayingDialogue && !GC.isDisplayingNPCMenuTalkDialogue)
			{
				menu_NPC_Farina.objTalkSubHolder.transform.position = _vecOriginFarinaMenu;
			}
			if (menu_NPC_Farina.objTalkNode.activeInHierarchy && !GC.isDisplayingDialogue)
			{
				menu_NPC_Farina.objTalkNode.SetActive(value: false);
			}
			break;
		case GameCore.NPCType.NeneShop:
			if (menu_NPC_Nene.objTalkNode.activeInHierarchy && menu_NPC_Nene.objTalkSubHolder.transform.position != _vecOriginNeneMenu && GC.isDisplayingDialogue && !GC.isDisplayingNPCMenuTalkDialogue)
			{
				menu_NPC_Nene.objTalkSubHolder.transform.position = _vecOriginNeneMenu;
			}
			if (menu_NPC_Nene.objTalkNode.activeInHierarchy && !GC.isDisplayingDialogue)
			{
				menu_NPC_Nene.objTalkNode.SetActive(value: false);
			}
			if (menu_NPC_Nene.objShopNode.activeInHierarchy && !GC.isDisplayingDialogue)
			{
				NPC_ToggleShopWindow(state: false);
			}
			break;
		}
	}

	public void NPC_ToggleMainMenu(bool setState)
	{
		NPC_GetObjects(out var objMain, out var _, out var btnMain, out var _);
		if (objMain != null)
		{
			objMain.SetActive(setState);
			if (setState)
			{
				btnMain.Select();
				menu_NPC_Farina.objTalkSubHolder.transform.position = _vecOriginFarinaMenu;
				menu_NPC_Nene.objTalkSubHolder.transform.position = _vecOriginNeneMenu;
			}
		}
	}

	private void NPC_ToggleTalkListWindow(bool state)
	{
		NPC_GetObjects(out var objMain, out var objTalk, out var btnMain, out var btnReturnToMain);
		if (objMain != null)
		{
			if (state)
			{
				NPC_DoTalkListLogic();
				objMain.SetActive(value: false);
				objTalk.SetActive(value: true);
				btnReturnToMain.Select();
			}
			else
			{
				objMain.SetActive(value: true);
				objTalk.SetActive(value: false);
				btnMain.Select();
			}
		}
	}

	private void NPC_DoTalkListLogic()
	{
		List<int> nPCTalkSpecialList = menu_NPC_Farina.SobjDiagList.GetNPCTalkSpecialList(GC.lastNPCTypeInteracted);
		Transform transform = null;
		int idDiag = 0;
		int num = 0;
		string text = "<color=#FF7969><b>!!!</b></color>";
		switch (GC.lastNPCTypeInteracted)
		{
		case GameCore.NPCType.FarinaSupport:
			transform = menu_NPC_Farina.parentDiagHolder;
			break;
		case GameCore.NPCType.NeneShop:
			transform = menu_NPC_Nene.parentDiagHolder;
			break;
		}
		if (transform == null)
		{
			Debug.Log("NPC_DoTalkListLogic() ERROR!");
			return;
		}
		num = transform.childCount - 2;
		foreach (Transform item in transform)
		{
			if (int.TryParse(item.name, out idDiag))
			{
				string text2 = "";
				if (!GC.HasSeenReadNPCMenuDialogueAdd(GC.lastNPCTypeInteracted, idDiag))
				{
					text2 = text;
				}
				item.gameObject.GetComponentInChildren<Text>().text = num + " - " + text2 + GC.DIALOG_GetTitleOfTalk(GC.lastNPCTypeInteracted, idDiag) + text2;
				if (nPCTalkSpecialList.Exists((int x) => x == idDiag))
				{
					item.gameObject.SetActive(value: true);
				}
				else
				{
					item.gameObject.SetActive(value: false);
				}
				num--;
			}
		}
	}

	private void NPC_GetObjects(out GameObject objMain, out GameObject objTalk, out Button btnMain, out Button btnReturnToMain)
	{
		objMain = null;
		objTalk = null;
		btnMain = null;
		btnReturnToMain = null;
		switch (GC.lastNPCTypeInteracted)
		{
		case GameCore.NPCType.FarinaSupport:
			objMain = menu_NPC_Farina.objMainNode;
			objTalk = menu_NPC_Farina.objTalkNode;
			btnMain = menu_NPC_Farina.btnTalk;
			btnReturnToMain = menu_NPC_Farina.btnBackFromTalk;
			break;
		case GameCore.NPCType.NeneShop:
			objMain = menu_NPC_Nene.objMainNode;
			objTalk = menu_NPC_Nene.objTalkNode;
			btnMain = menu_NPC_Nene.btnShop;
			btnReturnToMain = menu_NPC_Nene.btnBackFromTalk;
			break;
		}
	}

	public void NPC_ToggleShopWindow(bool state)
	{
		if (state)
		{
			menu_NPC_Nene.objMainNode.SetActive(value: false);
			menu_NPC_Nene.objShopNode.SetActive(value: true);
			menu_NPC_Nene.btnConsumables.Select();
			GC.SetNewPositionDialogueWindow(new Vector3(-9999f, -9999f, -9999f));
		}
		else
		{
			menu_NPC_Nene.objShopNode.SetActive(value: false);
			menu_NPC_Nene.objMainNode.SetActive(value: true);
			menu_NPC_Nene.btnShop.Select();
			GC.SetNewPositionDialogueWindow(Vector3.zero, restoreDefault: true);
			GC.BeginDialogueLogic(GameCore.NPCType.NeneShop, 90002, isZoomDialogue: true);
			menu_NPC_Nene.txtPriceBought.gameObject.SetActive(value: false);
		}
	}

	public void NPC_StartShopEffectAfterBought(string price, float time)
	{
		if (crtShopAfterEffect != null)
		{
			StopCoroutine(crtShopAfterEffect);
		}
		crtShopAfterEffect = StartCoroutine(waitShopAfterEffect(time));
		_vecRefShopAfterEffect = menu_NPC_Nene.txtEssenceHeld.transform.position;
		_vecRefShopAfterEffect.x += 125f;
		price = "-" + price;
		menu_NPC_Nene.txtPriceBought.gameObject.SetActive(value: true);
		menu_NPC_Nene.txtPriceBought.transform.position = _vecRefShopAfterEffect;
		menu_NPC_Nene.txtPriceBought.text = price;
		menu_NPC_Nene.txtPriceBought2.text = price;
	}

	private IEnumerator waitShopAfterEffect(float wait)
	{
		yield return new WaitForSeconds(wait);
		menu_NPC_Nene.txtPriceBought.text = "";
		menu_NPC_Nene.txtPriceBought2.text = "";
		menu_NPC_Nene.txtPriceBought.gameObject.SetActive(value: false);
		crtShopAfterEffect = null;
	}

	public void ShowBeltEquipSlotScreen(CountItemsQuantity sentItem, Button sentInventoryButton)
	{
		currentItemToBeEquiped = sentItem;
		if (currentItemToBeEquiped == null)
		{
			return;
		}
		int num = 0;
		currentButtonInventory = sentInventoryButton;
		HackSetInteractableTheButtonsOnInventoryScreen(setState: false);
		menuInventory.objEquipRoot.SetActive(value: true);
		menuInventory.objBeltDisplayOnly.SetActive(value: false);
		menuInventory.uiMenuBelt.imgCurrentItem.sprite = menuInventory.uiMenuBelt.listInvIcons.GetIconFromID(currentItemToBeEquiped.id);
		menuInventory.uiMenuBelt.labelItemName.text = GC.GetItemNameFromList(currentItemToBeEquiped.id);
		UpdateEquipScreenSlotsWithCurrentLogicBelt();
		for (int i = 0; i < GC.arraySlotsOfBelt.Length; i++)
		{
			if ((bool)GC && GC.arraySlotsOfBelt != null && GC.arraySlotsOfBelt[i] != null && GC.arraySlotsOfBelt[i].id == sentItem.id)
			{
				num = i;
				isItemWasAlreadyEquipedOnDiffSlot = true;
				break;
			}
		}
		if (!isItemWasAlreadyEquipedOnDiffSlot)
		{
			for (int j = 0; j < GC.arraySlotsOfBelt.Length; j++)
			{
				if ((bool)GC && GC.arraySlotsOfBelt != null && (GC.arraySlotsOfBelt[j] == null || (GC.arraySlotsOfBelt[j] != null && GC.arraySlotsOfBelt[j].id <= 0)))
				{
					num = j;
					break;
				}
			}
		}
		menuInventory.uiMenuBelt.arraySlots[num].btnSelection.Select();
	}

	private void HackSetInteractableTheButtonsOnInventoryScreen(bool setState)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("specialButton1");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<Button>().interactable = setState;
		}
	}

	private void CloseBeltEquipSlotScreen(int selectedSlotID = -1)
	{
		menuInventory.objEquipRoot.SetActive(value: false);
		menuInventory.objBeltDisplayOnly.SetActive(value: true);
		HackSetInteractableTheButtonsOnInventoryScreen(setState: true);
		isItemWasAlreadyEquipedOnDiffSlot = false;
		currentItemToBeEquiped = null;
		if ((bool)currentButtonInventory && !GC.IsButtonAlreadySelected(currentButtonInventory))
		{
			currentButtonInventory.Select();
		}
		else if (!currentButtonInventory && selectedSlotID > -1)
		{
			menuInventory.uiMenuBelt.arraySlots[selectedSlotID].btnMirror.Select();
		}
		else
		{
			menuInventory.uiMenuBelt.arraySlots[0].btnMirror.Select();
		}
		currentButtonInventory = null;
	}

	private void UpdateEquipScreenSlotsWithCurrentLogicBelt()
	{
		int num = GC.arraySlotsOfBelt.Length;
		Image[] array = new Image[num];
		Text[] array2 = new Text[num];
		for (int i = 0; i < num; i++)
		{
			array[i] = menuInventory.uiMenuBelt.arraySlots[i].imgDisplayIcon;
			array2[i] = menuInventory.uiMenuBelt.arraySlots[i].txtQuantity;
		}
		GC.RefreshBeltItemVisualSlotsDisplay(array, array2, menuInventory.uiMenuBelt.listInvIcons);
	}

	public void ButtonEquipSlotWasClickedPressed(int slotID)
	{
		if (crtDelayAfterItemEquiped != null)
		{
			return;
		}
		bool flag = false;
		int num = -1;
		CountItemsQuantity countItemsQuantity = null;
		for (int i = 0; i < GC.arraySlotsOfBelt.Length; i++)
		{
			if (GC.arraySlotsOfBelt[i] != null && GC.arraySlotsOfBelt[i].id == currentItemToBeEquiped.id)
			{
				if (slotID == i)
				{
					flag = true;
				}
				else
				{
					num = i;
				}
				break;
			}
		}
		for (int j = 0; j < GC.arraySlotsOfBelt.Length; j++)
		{
			if (GC.arraySlotsOfBelt[j] != null && GC.arraySlotsOfBelt[j].id != 0 && GC.arraySlotsOfBelt[j].id != currentItemToBeEquiped.id && slotID == j && !flag && isItemWasAlreadyEquipedOnDiffSlot)
			{
				countItemsQuantity = new CountItemsQuantity();
				countItemsQuantity = GC.arraySlotsOfBelt[j];
				break;
			}
		}
		if (isItemWasAlreadyEquipedOnDiffSlot && num >= 0)
		{
			GC.RemoveItemFromBeltSlot(num);
			if (countItemsQuantity != null)
			{
				GC.AddItemToBeltSlot(num, countItemsQuantity);
			}
		}
		if (!flag)
		{
			GC.AddItemToBeltSlot(slotID, currentItemToBeEquiped);
			GC.PlaySFX(GameCore.SFXBasic1.uiEquipItem, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
		else
		{
			GC.RemoveItemFromBeltSlot(slotID);
			GC.PlaySFX(GameCore.SFXBasic1.uiUnequipItem, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
		UpdateEquipScreenSlotsWithCurrentLogicBelt();
		isItemWasAlreadyEquipedOnDiffSlot = false;
		crtDelayAfterItemEquiped = StartCoroutine(waitForDelayAfterItemEquiped(0.1f, slotID));
	}

	public void BeltMirrorUISlotClicked(int slotID)
	{
		ItemInfo itemInfo = null;
		if (GC.arraySlotsOfBelt[slotID] != null)
		{
			itemInfo = GC.GetItemInfoFromDatabase(GC.arraySlotsOfBelt[slotID].id);
			if (itemInfo != null)
			{
				GC.ItemButtonWasClickedOnInventoryScreenToOpenEquipScreen(itemInfo.id, GC.ItemQuantityInPlayerInventory(itemInfo.id), null);
			}
			else
			{
				menuInventory.btnTabAccessories.Select();
			}
		}
	}

	public void InventoryTextRefreshAccConsolidateStatEffect()
	{
		menuInventory.txtAccTotalStats.text = GC.ACC_TextRefreshConsolidateStatChanges();
	}

	private IEnumerator waitForDelayAfterItemEquiped(float wait, int equipedSlotID)
	{
		yield return new WaitForSecondsRealtime(wait);
		CloseBeltEquipSlotScreen(equipedSlotID);
		crtDelayAfterItemEquiped = null;
	}

	private void RetrieveBaseLists()
	{
		arrayDifficultyType = Enum.GetValues(typeof(GameCore.DifficultyType)).Cast<GameCore.DifficultyType>().ToArray();
		arrayCameraType = Enum.GetValues(typeof(GameCore.CameraType)).Cast<GameCore.CameraType>().ToArray();
		arrayControlType = Enum.GetValues(typeof(GameCore.InputType)).Cast<GameCore.InputType>().ToArray();
	}

	private void ApplyInitialValuesToSelections()
	{
		GameCore.DifficultyType[] array = arrayDifficultyType;
		foreach (GameCore.DifficultyType difficultyType in array)
		{
			if (GC.configDifficultyType == difficultyType)
			{
				ChangeValueOfSelectionRealTime(controlTypes.Difficulty, isChangeID: false, 0);
				break;
			}
		}
		ChangeValueOfSelectionRealTime(controlTypes.Language, isChangeID: false, 0);
		GameCore.CameraType[] array2 = arrayCameraType;
		foreach (GameCore.CameraType cameraType in array2)
		{
			if (GC.configCameraType == cameraType)
			{
				ChangeValueOfSelectionRealTime(controlTypes.Camera, isChangeID: false, 0);
				break;
			}
		}
		GameCore.InputType[] array3 = arrayControlType;
		foreach (GameCore.InputType inputType in array3)
		{
			if (GC.configInputType == inputType)
			{
				ChangeValueOfSelectionRealTime(controlTypes.Input, isChangeID: false, 0);
				break;
			}
		}
		menuOptions.sliderTextSpeed.value = GC.configTextSpeed;
		menuOptions.sliderTextSound.value = GC.configTextVolume;
		menuOptions.sliderMusic.value = GC.configMusicVolume;
		menuOptions.sliderSound.value = GC.configSoundEffectVolume;
		menuOptions.sliderVoice.value = GC.configVoiceVolume;
		menuOptions.sliderVoiceRate.value = GC.configVoiceRate;
	}

	private void ChangeValueOfSelectionRealTime(controlTypes controlType, bool isChangeID, int idToChange)
	{
		switch (controlType)
		{
		case controlTypes.Difficulty:
			if (isChangeID)
			{
				GC.configDifficultyType = (GameCore.DifficultyType)idToChange;
			}
			if ((bool)menuOptions.txtDifficulty)
			{
				menuOptions.txtDifficulty.text = GC.clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 8 && item.sub_id == (int)GC.configDifficultyType).text;
			}
			break;
		case controlTypes.Language:
		{
			foreach (LineOfLanguage item in GC.clsLanguage.listOfLanguage)
			{
				if (isChangeID && item.id == idToChange)
				{
					GC.configLanguageType = idToChange;
					menuOptions.txtLanguage.text = item.native_name;
					break;
				}
				if (!isChangeID && GC.configLanguageType == item.id)
				{
					menuOptions.txtLanguage.text = item.native_name;
					break;
				}
			}
			break;
		}
		case controlTypes.Camera:
			if (isChangeID)
			{
				GC.configCameraType = (GameCore.CameraType)idToChange;
			}
			if ((bool)menuOptions.txtCamera)
			{
				menuOptions.txtCamera.text = GC.clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 12 && item.sub_id == (int)GC.configCameraType).text;
			}
			break;
		case controlTypes.Input:
			if (isChangeID)
			{
				GC.configInputType = (GameCore.InputType)idToChange;
			}
			if ((bool)menuControls.buttonTextInputType)
			{
				menuControls.buttonTextInputType.text = GC.clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 32 && item.sub_id == (int)GC.configInputType).text;
			}
			if (GC.isGamepadCfgInput)
			{
				menuControls.txtGPWarning.gameObject.SetActive(value: true);
			}
			else
			{
				menuControls.txtGPWarning.gameObject.SetActive(value: false);
			}
			break;
		}
	}

	public void OptionButtonClicked(string buttonName)
	{
		int num = 0;
		switch (buttonName)
		{
		case "Continue":
		{
			GameCore.MenuScene menuScene = currentMenuSceneType;
			if (menuScene == GameCore.MenuScene.InGame)
			{
				GC.TogglePauseForGameMenu();
			}
			break;
		}
		case "Retry":
		{
			GameCore.MenuScene menuScene = currentMenuSceneType;
			if (menuScene == GameCore.MenuScene.InGame && GC.isPlayerFainted && crtHackyEscapePressOnFainted == null)
			{
				TryToLoadTheGame(0, isRetry: true);
			}
			break;
		}
		case "Difficulty":
			num = (int)GC.configDifficultyType;
			num = ((num + 1 < arrayDifficultyType.Length) ? (num + 1) : 0);
			ChangeValueOfSelectionRealTime(controlTypes.Difficulty, isChangeID: true, num);
			break;
		case "Language":
			num = GC.configLanguageType;
			num = ((num + 1 < GC.clsLanguage.listOfLanguage.Count) ? (num + 1) : 0);
			ChangeValueOfSelectionRealTime(controlTypes.Language, isChangeID: true, num);
			GC.LoadItemDatabaseFile();
			LoadLanguageOfMenu(isLanguageChanged: true);
			break;
		case "Camera":
			num = (int)GC.configCameraType;
			num = ((num + 1 < arrayCameraType.Length) ? (num + 1) : 0);
			ChangeValueOfSelectionRealTime(controlTypes.Camera, isChangeID: true, num);
			break;
		case "ControlConfig:Type":
			num = (int)GC.configInputType;
			num = ((num + 1 < arrayControlType.Length) ? (num + 1) : 0);
			ChangeValueOfSelectionRealTime(controlTypes.Input, isChangeID: true, num);
			ReflectCurrentInputListOnTheControlWindow();
			menuControls.scroll_controls.value = 1f;
			break;
		case "ReturnTitle":
			GC.TogglePauseForGameMenu();
			SaveConfigData();
			GC.LoadNextScene("MainMenu", Vector2.zero, Vector2.zero, forceMovePlayer: false, applyFade: true, Color.black, GC.globalDefaultSceneFadeTimer, lockPlayerInput: true);
			break;
		}
	}

	public void SliderValueChanged(string sliderName)
	{
		switch (sliderName)
		{
		case "TextSpeed":
			GC.configTextSpeed = (int)menuOptions.sliderTextSpeed.value;
			break;
		case "TextSound":
			GC.configTextVolume = menuOptions.sliderTextSound.value;
			break;
		case "Music":
			GC.configMusicVolume = menuOptions.sliderMusic.value;
			GC.UpdateMusicVolumeRealTime();
			break;
		case "Sound":
			GC.configSoundEffectVolume = menuOptions.sliderSound.value;
			break;
		case "Voice":
			GC.configVoiceVolume = menuOptions.sliderVoice.value;
			break;
		case "VoiceRate":
			GC.configVoiceRate = menuOptions.sliderVoiceRate.value;
			break;
		}
	}

	public void ControlConfigBinding(Button sentButton)
	{
		InitiateKeyBindingLogic(sentButton);
	}

	private IEnumerator waitForHackyEscapePressedOnFainted(float wait)
	{
		yield return new WaitForSecondsRealtime(wait);
		crtHackyEscapePressOnFainted = null;
	}

	public void AdvancedButtonClicked(int buttonID)
	{
		ChangeValueOfSelectionRealTimeAdvanced(buttonID, isChangeState: true);
	}

	public void ApplyInitialValuesAdvanced()
	{
		int num = 17;
		for (int i = 1; i <= num; i++)
		{
			ChangeValueOfSelectionRealTimeAdvanced(i, isChangeState: false);
		}
	}

	private void ChangeValueOfSelectionRealTimeAdvanced(int buttonID, bool isChangeState)
	{
		switch (buttonID)
		{
		case 1:
			if (isChangeState)
			{
				GC.canApplySlowZoomEffectOnDressBreak = !GC.canApplySlowZoomEffectOnDressBreak;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv1, GC.canApplySlowZoomEffectOnDressBreak);
			break;
		case 2:
			if (isChangeState)
			{
				GC.canApplyAutoZoomOnEroGrab = !GC.canApplyAutoZoomOnEroGrab;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv2, GC.canApplyAutoZoomOnEroGrab);
			break;
		case 3:
			if (isChangeState)
			{
				GC.canShowBlockedDamage = !GC.canShowBlockedDamage;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv3, GC.canShowBlockedDamage);
			break;
		case 4:
			if (isChangeState)
			{
				GC.doQuickSaveFeature = !GC.doQuickSaveFeature;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv4, GC.doQuickSaveFeature);
			break;
		case 5:
			if (isChangeState)
			{
				GC.doAutoSaveFeature = !GC.doAutoSaveFeature;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv5, GC.doAutoSaveFeature);
			break;
		case 6:
			if (isChangeState)
			{
				GC.doAutoEquipFeature = !GC.doAutoEquipFeature;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv6, GC.doAutoEquipFeature);
			break;
		case 7:
			if (isChangeState)
			{
				GC.doRememberLastZoomAmount = !GC.doRememberLastZoomAmount;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv7, GC.doRememberLastZoomAmount);
			break;
		case 8:
			if (isChangeState)
			{
				GC.doDisplayLinesMechanisms = !GC.doDisplayLinesMechanisms;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv8, GC.doDisplayLinesMechanisms);
			break;
		case 9:
			if (isChangeState)
			{
				GC.doAllVoreScenesAreFatal = !GC.doAllVoreScenesAreFatal;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv9, GC.doAllVoreScenesAreFatal);
			break;
		case 10:
			if (isChangeState)
			{
				GC.doGalleryHideBlockedScenes = !GC.doGalleryHideBlockedScenes;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv10, GC.doGalleryHideBlockedScenes);
			break;
		case 11:
			if (isChangeState)
			{
				GC.doAllowBadEnd = !GC.doAllowBadEnd;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv11, GC.doAllowBadEnd);
			break;
		case 12:
			if (isChangeState)
			{
				GC.doAllowDialogueDuringBadEnd = !GC.doAllowDialogueDuringBadEnd;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv12, GC.doAllowDialogueDuringBadEnd);
			break;
		case 13:
			if (isChangeState)
			{
				GC.lock60fps = !GC.lock60fps;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv13, GC.lock60fps);
			break;
		case 14:
			if (isChangeState)
			{
				GC.revertToOldStruggleBar = !GC.revertToOldStruggleBar;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv14, GC.revertToOldStruggleBar);
			break;
		case 15:
			if (isChangeState)
			{
				GC.doAllowDebug = !GC.doAllowDebug;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv15, GC.doAllowDebug);
			GC.UIDebugDescriptionToggle(GC.doAllowDebug);
			break;
		case 16:
			if (isChangeState)
			{
				GC.disableCursorAnimUI = !GC.disableCursorAnimUI;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv16, GC.disableCursorAnimUI);
			break;
		case 17:
			if (isChangeState)
			{
				GC.disableVoiceLogo = !GC.disableVoiceLogo;
			}
			SetArtOfToggleOption(menuAdvanced.buttonAdv17, GC.disableVoiceLogo);
			break;
		}
	}

	public void FetishButtonClicked(int buttonID)
	{
		ChangeValueOfSelectionRealTimeFetish(buttonID, isChangeState: true);
	}

	public void ApplyInitialValuesFetish()
	{
		int num = 10;
		for (int i = 1; i <= num; i++)
		{
			ChangeValueOfSelectionRealTimeFetish(i, isChangeState: false);
		}
	}

	private void ChangeValueOfSelectionRealTimeFetish(int buttonID, bool isChangeState)
	{
		switch (buttonID)
		{
		case 1:
			if (isChangeState)
			{
				GC.configFetishes.showVanilla = !GC.configFetishes.showVanilla;
			}
			SetArtOfToggleOption(menuFetish.buttonVanilla, GC.configFetishes.showVanilla);
			break;
		case 2:
			if (isChangeState)
			{
				GC.configFetishes.showBlowjob = !GC.configFetishes.showBlowjob;
			}
			SetArtOfToggleOption(menuFetish.buttonBlowjob, GC.configFetishes.showBlowjob);
			break;
		case 3:
			if (isChangeState)
			{
				GC.configFetishes.showFeet = !GC.configFetishes.showFeet;
			}
			SetArtOfToggleOption(menuFetish.buttonFeet, GC.configFetishes.showFeet);
			break;
		case 4:
			if (isChangeState)
			{
				GC.configFetishes.showLesbian = !GC.configFetishes.showLesbian;
			}
			SetArtOfToggleOption(menuFetish.buttonLesbian, GC.configFetishes.showLesbian);
			break;
		case 5:
			if (isChangeState)
			{
				GC.configFetishes.showFutanari = !GC.configFetishes.showFutanari;
			}
			SetArtOfToggleOption(menuFetish.buttonFutanari, GC.configFetishes.showFutanari);
			break;
		case 6:
			if (isChangeState)
			{
				GC.configFetishes.showTentacle = !GC.configFetishes.showTentacle;
			}
			SetArtOfToggleOption(menuFetish.buttonTentacle, GC.configFetishes.showTentacle);
			break;
		case 7:
			if (isChangeState)
			{
				GC.configFetishes.showBeast = !GC.configFetishes.showBeast;
			}
			SetArtOfToggleOption(menuFetish.buttonBeast, GC.configFetishes.showBeast);
			break;
		case 8:
			if (isChangeState)
			{
				GC.configFetishes.showInsect = !GC.configFetishes.showInsect;
			}
			SetArtOfToggleOption(menuFetish.buttonInsect, GC.configFetishes.showInsect);
			break;
		case 9:
			if (isChangeState)
			{
				GC.configFetishes.showRyona = !GC.configFetishes.showRyona;
			}
			SetArtOfToggleOption(menuFetish.buttonRyona, GC.configFetishes.showRyona);
			break;
		case 10:
			if (isChangeState)
			{
				GC.configFetishes.showVore = !GC.configFetishes.showVore;
			}
			SetArtOfToggleOption(menuFetish.buttonVore, GC.configFetishes.showVore);
			break;
		}
	}

	public void ButtonSaveLoadSlotWasClicked(int slotID)
	{
		if (!isProcessingSaveLoad)
		{
			isProcessingSaveLoad = true;
			switch (currentSaveLoadScreen)
			{
			case SaveLoadScreenTypes.Save:
				TryToSaveTheGame(slotID);
				break;
			case SaveLoadScreenTypes.Load:
				TryToLoadTheGame(slotID);
				break;
			}
			isProcessingSaveLoad = false;
		}
	}

	public void PopulateSaveFileSlotArrayAndApplyToUI()
	{
		Color color = new Color(0.25f, 0.25f, 0.25f, 1f);
		GC.ValidateSaveFilesIntoSlots();
		for (int i = 0; i < GC.arraySlotsOfSaveLoad.Length; i++)
		{
			if (GC.arraySlotsOfSaveLoad[i] != null)
			{
				menuSaveLoad.arraySaveSlots[i].txtStageName.text = GC.TranslateSceneNameToStageName(isCurrentlyActive: false, GC.arraySlotsOfSaveLoad[i].textSceneNameSaved);
				menuSaveLoad.arraySaveSlots[i].txtDateTime.text = GC.ReturnParsedDateTime(GC.arraySlotsOfSaveLoad[i].textDateTimeSaved).ToString();
				menuSaveLoad.arraySaveSlots[i].imgSavePoint.sprite = databaseSavePoint.ReturnScreenshotByID(GC.arraySlotsOfSaveLoad[i].idSaveScreenshot);
				menuSaveLoad.arraySaveSlots[i].imgSavePoint.color = Color.white;
				continue;
			}
			menuSaveLoad.arraySaveSlots[i].txtStageName.text = "-" + GC.clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 43).text + "-";
			menuSaveLoad.arraySaveSlots[i].txtDateTime.text = "";
			menuSaveLoad.arraySaveSlots[i].imgSavePoint.sprite = null;
			menuSaveLoad.arraySaveSlots[i].imgSavePoint.color = color;
		}
		if ((bool)menuSaveLoad.titleSaveLoad)
		{
			menuSaveLoad.titleSaveLoad.text = GC.clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == ReturnSaveLoad_TitleID()).text;
		}
	}

	public void SetCurrentSaveLoadScreenType(string type)
	{
		if (!(type == "SAVE"))
		{
			if (type == "LOAD")
			{
				currentSaveLoadScreen = SaveLoadScreenTypes.Load;
			}
			else
			{
				currentSaveLoadScreen = SaveLoadScreenTypes.Null;
			}
		}
		else
		{
			currentSaveLoadScreen = SaveLoadScreenTypes.Save;
		}
	}

	private int ReturnSaveLoad_TitleID()
	{
		switch (currentSaveLoadScreen)
		{
		case SaveLoadScreenTypes.Save:
			return 42;
		case SaveLoadScreenTypes.Load:
			return 3;
		default:
			return 0;
		}
	}

	public void StartNewGame()
	{
		if (!GC.isSceneTransitioning)
		{
			GC.LoadNextScene("Intro-Story", Vector2.zero, Vector2.zero, forceMovePlayer: false, applyFade: true, Color.black, GC.globalDefaultSceneFadeTimer, lockPlayerInput: true);
		}
	}

	private void TryToSaveTheGame(int slotID)
	{
		if (GC.SaveGameData(slotID))
		{
			PopulateSaveFileSlotArrayAndApplyToUI();
			GC.DisplaySystemMessageUI(0, 61);
		}
	}

	private void TryToLoadTheGame(int slotID, bool isRetry = false)
	{
		if (!GC.isSceneTransitioning && GC.TrySetupGameLoad(getLatestSave: false, slotID, currentMenuSceneType, isRetry))
		{
			SpecialCloseAllWindows();
		}
	}

	public void OpenGalleryMode()
	{
		if (!GC.TrySetupGameLoad(getLatestSave: true, 0, currentMenuSceneType, isRetry: false, GC.globalDefaultSceneFadeTimer, isTitleScreenGallery: true))
		{
			InitiateTextFlashing(menuMainTitle.txtUnderConstruction, 3f, 0.2f, Color.white, Color.red);
		}
	}

	public void ContinueGame()
	{
		if (!GC.isSceneTransitioning)
		{
			if (CheckIfCanEnableTheContinueButton())
			{
				GC.TrySetupGameLoad(getLatestSave: true, 0, currentMenuSceneType);
			}
			else if ((bool)GC)
			{
				GC.DisplaySystemMessageUI(0, 175, 2f, "", isFlavorCentralMessage: true);
			}
		}
	}

	public void SaveConfigData()
	{
		GC.SaveGameConfiguration();
	}

	public void QuitGame()
	{
		SaveConfigData();
		Application.Quit();
	}

	private bool CheckIfCanEnableTheContinueButton()
	{
		for (int i = 0; i < GC.arraySlotsOfSaveLoad.Length; i++)
		{
			if (GC.arraySlotsOfSaveLoad[i] != null)
			{
				return true;
			}
		}
		return false;
	}

	private void OnGUI()
	{
		OnGUI_KeyboardBindMainLogic();
		OnGUI_GamepadBindingMainLogic();
	}

	private void InitiateKeyBindingLogic(Button sentButton)
	{
		if (crtDelayPrekeybind == null && crtDelayAfterkeybind == null)
		{
			if (!isListeningForKeybindings)
			{
				fltListeningCounter = 5f;
				fltListeningInverval = 1f;
				currentButtonBind = sentButton;
				currentTextBind = sentButton.GetComponentInChildren<Text>();
				currentTextBind.text = "???";
				crtDelayPrekeybind = StartCoroutine(waitDelayPreKeyBinding(1f));
				GC.MouseCursorToggle(isLocked: true, isVisible: false);
				menuControls.txtListeningForInputs.text = GC.GetTranslatedStringUI(139);
				menuControls.txtListeningSeconds.text = Mathf.RoundToInt(fltListeningCounter).ToString();
				menuControls.objImageBGListening.SetActive(value: true);
			}
			else if (!GC.IsButtonAlreadySelected(currentButtonBind))
			{
				currentButtonBind.Select();
			}
		}
	}

	private void FinalizeKeyBindingLogic(bool cancelBindingProcess = false)
	{
		if (crtDelayAfterkeybind == null)
		{
			crtDelayAfterkeybind = StartCoroutine(waitForDelayAfterSuccesfulKeybind(0.1f));
		}
		isListeningForKeybindings = false;
		strShiftWasPressedToBind = "";
		duplicateKey = GameCore.InputLib.Null;
		GC.ListenGamepadBinding = KeyCode.Asterisk;
		GC.MouseCursorToggle(isLocked: false, isVisible: true);
		menuControls.objImageBGListening.SetActive(value: false);
		if (cancelBindingProcess)
		{
			GameCore.InputLib newType = ReturnTheTypeOfCurrentBindedButton();
			GC.InsertNewInputKeyCode(newType, KeyCode.None);
			GC.SaveGameConfiguration();
			ReflectCurrentInputListOnTheControlWindow();
		}
	}

	private void ListeningDecreaseCounter()
	{
		fltListeningCounter -= fltListeningInverval * Time.unscaledDeltaTime;
		if (menuControls.txtListeningSeconds.isActiveAndEnabled)
		{
			menuControls.txtListeningSeconds.text = Mathf.RoundToInt(fltListeningCounter).ToString();
		}
		if (fltListeningCounter <= 0f)
		{
			FinalizeKeyBindingLogic(cancelBindingProcess: true);
		}
	}

	private void OnGUI_KeyboardBindMainLogic()
	{
		Event current = Event.current;
		if (!isListeningForKeybindings || !CheckKeyboardBindConditions(current))
		{
			return;
		}
		KeyCode keyCode = KeyCode.Asterisk;
		keyCode = ((strShiftWasPressedToBind == "LEFT") ? KeyCode.LeftShift : ((!(strShiftWasPressedToBind == "RIGHT")) ? current.keyCode : KeyCode.RightShift));
		if (crtDelayAfterkeybind != null)
		{
			StopCoroutine(crtDelayAfterkeybind);
		}
		crtDelayAfterkeybind = StartCoroutine(waitForDelayAfterSuccesfulKeybind(0.25f));
		if (IsDuplicateBinding(keyCode))
		{
			if (SwapDuplicateKeyBindings(keyCode))
			{
				ReflectCurrentInputListOnTheControlWindow();
			}
		}
		else if (ApplyNewKeyBinding(keyCode))
		{
			ReflectCurrentInputListOnTheControlWindow();
		}
		FinalizeKeyBindingLogic();
	}

	private bool CheckKeyboardBindConditions(Event eventThis)
	{
		if ((eventThis != null && eventThis.isKey && CheckForDefaultKeys(eventThis.keyCode)) || strShiftWasPressedToBind != "")
		{
			return true;
		}
		return false;
	}

	private void OnGUI_GamepadBindingMainLogic()
	{
		if (!OnGUI_ValidateGamepadBinding())
		{
			return;
		}
		KeyCode listenGamepadBinding = GC.ListenGamepadBinding;
		if (crtDelayAfterkeybind != null)
		{
			StopCoroutine(crtDelayAfterkeybind);
		}
		crtDelayAfterkeybind = StartCoroutine(waitForDelayAfterSuccesfulKeybind(0.25f));
		if (IsDuplicateBinding(listenGamepadBinding))
		{
			if (SwapDuplicateKeyBindings(listenGamepadBinding))
			{
				ReflectCurrentInputListOnTheControlWindow();
			}
		}
		else if (ApplyNewKeyBinding(listenGamepadBinding))
		{
			ReflectCurrentInputListOnTheControlWindow();
		}
		FinalizeKeyBindingLogic();
	}

	private bool OnGUI_ValidateGamepadBinding()
	{
		if (GC.configInputType == GameCore.InputType.GPTypePSX && GC.ListenGamepadBinding == KeyCode.JoystickButton12)
		{
			return false;
		}
		if (isListeningForKeybindings && GC.isGamepadCfgInput && GC.ListenGamepadBinding != KeyCode.Asterisk)
		{
			return true;
		}
		return false;
	}

	private void UPDT_SniffGamepadBinding()
	{
		KeyCode keyCode = KeyCode.Asterisk;
		keyCode = GC.TranslateGamepadInput();
		if (keyCode != KeyCode.Asterisk)
		{
			GC.ListenGamepadBinding = keyCode;
			return;
		}
		foreach (KeyCode value in Enum.GetValues(typeof(KeyCode)))
		{
			if (Input.GetKeyDown(value) && value.ToString().Contains("Joystick"))
			{
				keyCode = value;
				GC.ListenGamepadBinding = AlterJoystickGamepadInputName(keyCode);
				break;
			}
		}
	}

	private KeyCode AlterJoystickGamepadInputName(KeyCode toAlter)
	{
		string text = toAlter.ToString();
		if (text.Substring(8, 1) != "B")
		{
			text = text.Substring(0, 8) + text.Substring(9, text.Length - 9).TrimEnd();
		}
		Enum.TryParse<KeyCode>(text, out toAlter);
		return toAlter;
	}

	private bool ApplyNewKeyBinding(KeyCode newKey)
	{
		GameCore.InputLib inputLib = ReturnTheTypeOfCurrentBindedButton();
		if (inputLib != GameCore.InputLib.Null)
		{
			GC.InsertNewInputKeyCode(inputLib, newKey);
			GC.SaveGameConfiguration();
			return true;
		}
		Debug.Log("MenuBehavior.ApplyNewKeyBinding: InputLib returned NULL!!");
		return false;
	}

	private bool SwapDuplicateKeyBindings(KeyCode newKey)
	{
		GameCore.InputLib inputLib = ReturnTheTypeOfCurrentBindedButton();
		if (inputLib != GameCore.InputLib.Null && duplicateKey != GameCore.InputLib.Null)
		{
			GC.SwapDuplicateKeybindings(inputLib, newKey, duplicateKey);
			GC.SaveGameConfiguration();
			return true;
		}
		Debug.Log("MenuBehavior.SwapDuplicateKeyBindings: InputLib returned NULL!!");
		return false;
	}

	private GameCore.InputLib ReturnTheTypeOfCurrentBindedButton()
	{
		switch (currentButtonBind.gameObject.name)
		{
		case "Button-Attack":
			return GameCore.InputLib.BasicAttack;
		case "Button-Jump":
			return GameCore.InputLib.Jump;
		case "Button-ConfirmUI":
			return GameCore.InputLib.ConfirmUI;
		case "Button-CancelUI":
			return GameCore.InputLib.CancelUI;
		case "Button-AttackSkills":
			return GameCore.InputLib.AttackSkills;
		case "Button-DefenseSkills":
			return GameCore.InputLib.DefenseSkills;
		case "Button-Camera":
			return GameCore.InputLib.Camera;
		case "Button-InternalView":
			return GameCore.InputLib.ToggleInternalView;
		case "Button-Interact":
			return GameCore.InputLib.Interact;
		case "Button-Pause/Menu":
			return GameCore.InputLib.Pause;
		case "Button-Up":
			return GameCore.InputLib.Up;
		case "Button-Down":
			return GameCore.InputLib.Down;
		case "Button-Left":
			return GameCore.InputLib.Left;
		case "Button-Right":
			return GameCore.InputLib.Right;
		case "Button-UseItem":
			return GameCore.InputLib.UseItemActive;
		case "Button-NextItem":
			return GameCore.InputLib.NextItem;
		case "Button-PreviousItem":
			return GameCore.InputLib.PreviousItem;
		case "Button-DebugHealLife":
			return GameCore.InputLib.DebugF5;
		case "Button-DebugHealSpirit":
			return GameCore.InputLib.DebugF6;
		case "Button-DebugRestoreDurability":
			return GameCore.InputLib.DebugF7;
		case "Button-DebugResetArmor":
			return GameCore.InputLib.DebugF8;
		case "Button-UpdateTeleportList":
			return GameCore.InputLib.DebugF9;
		case "Button-DebugTPDevice":
			return GameCore.InputLib.DebugF10;
		case "Button-DebugDestroyArmor":
			return GameCore.InputLib.DebugF11;
		case "Button-DebugInstantFainted":
			return GameCore.InputLib.DebugF12;
		default:
			return GameCore.InputLib.Null;
		}
	}

	public void ReflectCurrentInputListOnTheControlWindow()
	{
		switch (GC.configInputType)
		{
		default:
			menuControls.imgInputTypeIcon.sprite = menuControls.spriteKeyboard;
			break;
		case GameCore.InputType.GPTypePSX:
			menuControls.imgInputTypeIcon.sprite = menuControls.spriteType1;
			break;
		case GameCore.InputType.GPTypeXBOX:
			menuControls.imgInputTypeIcon.sprite = menuControls.spriteType2;
			break;
		}
		ReflectTextIcon(menuControls.texts.buttonTextAtack, GameCore.InputLib.BasicAttack);
		ReflectTextIcon(menuControls.texts.buttonTextJump, GameCore.InputLib.Jump);
		ReflectTextIcon(menuControls.texts.buttonAttackSkills, GameCore.InputLib.AttackSkills);
		ReflectTextIcon(menuControls.texts.buttonTextDefenseSkills, GameCore.InputLib.DefenseSkills);
		ReflectTextIcon(menuControls.texts.buttonTextCamera, GameCore.InputLib.Camera);
		ReflectTextIcon(menuControls.texts.buttonTextInternalView, GameCore.InputLib.ToggleInternalView);
		ReflectTextIcon(menuControls.texts.buttonTextInteract, GameCore.InputLib.Interact);
		ReflectTextIcon(menuControls.texts.buttonTextPauseMenu, GameCore.InputLib.Pause);
		ReflectTextIcon(menuControls.texts.buttonTextUp, GameCore.InputLib.Up);
		ReflectTextIcon(menuControls.texts.buttonTextLeft, GameCore.InputLib.Left);
		ReflectTextIcon(menuControls.texts.buttonTextDown, GameCore.InputLib.Down);
		ReflectTextIcon(menuControls.texts.buttonTextRight, GameCore.InputLib.Right);
		ReflectTextIcon(menuControls.texts.btnConfirmUI, GameCore.InputLib.ConfirmUI);
		ReflectTextIcon(menuControls.texts.btnCancelUI, GameCore.InputLib.CancelUI);
		ReflectTextIcon(menuControls.texts.btnUseItemActive, GameCore.InputLib.UseItemActive);
		ReflectTextIcon(menuControls.texts.btnNextItem, GameCore.InputLib.NextItem);
		ReflectTextIcon(menuControls.texts.btnPreviousItem, GameCore.InputLib.PreviousItem);
		ReflectTextIcon(menuControls.texts.btn_DbgHealLife, GameCore.InputLib.DebugF5);
		ReflectTextIcon(menuControls.texts.btn_DbgHealSpirit, GameCore.InputLib.DebugF6);
		ReflectTextIcon(menuControls.texts.btn_DbgRestoreDurability, GameCore.InputLib.DebugF7);
		ReflectTextIcon(menuControls.texts.btn_DbgResetArmor, GameCore.InputLib.DebugF8);
		ReflectTextIcon(menuControls.texts.btn_DbgTeleportList, GameCore.InputLib.DebugF9);
		ReflectTextIcon(menuControls.texts.btn_DbgTPDeviceBoss, GameCore.InputLib.DebugF10);
		ReflectTextIcon(menuControls.texts.btn_DbgDestroyArmor, GameCore.InputLib.DebugF11);
		ReflectTextIcon(menuControls.texts.btn_DbgInstantFainted, GameCore.InputLib.DebugF12);
	}

	private void ReflectTextIcon(Text uiTXT, GameCore.InputLib input)
	{
		bool wasGPIconValid = InputListParseIcons(uiTXT.GetComponentInChildren<Image>(), input);
		uiTXT.text = InputListParseNames(input, wasGPIconValid);
	}

	private string InputListParseNames(GameCore.InputLib thisInput, bool wasGPIconValid)
	{
		string text = GC.IKey(thisInput).ToString();
		if (GC.isGamepadCfgInput)
		{
			text = ((!wasGPIconValid) ? GC.GetTranslatedStringUI(listGPIcon.GetButtonTLID(text, GC.configInputType)) : "");
		}
		return text;
	}

	private bool InputListParseIcons(Image imgIcon, GameCore.InputLib thisInput)
	{
		GameCore.InputType configInputType = GC.configInputType;
		if (configInputType == GameCore.InputType.Keyboard || (uint)(configInputType - 1) > 1u)
		{
			imgIcon.sprite = null;
			imgIcon.color = new Color(1f, 1f, 1f, 0f);
		}
		else
		{
			imgIcon.sprite = listGPIcon.GetIcon(GC.IKey(thisInput).ToString(), GC.configInputType);
			if (imgIcon.sprite != null)
			{
				imgIcon.color = new Color(1f, 1f, 1f, 1f);
				return true;
			}
			imgIcon.color = new Color(1f, 1f, 1f, 0f);
		}
		return false;
	}

	public void ResetControlsToDefault(bool onlyResetTheCurrentlySelectedInputType)
	{
		GC.SetInputDefaultValues(onlyResetTheCurrentlySelectedInputType);
		GC.SaveGameConfiguration();
		ReflectCurrentInputListOnTheControlWindow();
		menuControls.scroll_controls.value = 1f;
	}

	private bool CheckForDefaultKeys(KeyCode thisKey)
	{
		if (thisKey == KeyCode.Return || thisKey == KeyCode.KeypadEnter || thisKey == KeyCode.Escape)
		{
			return false;
		}
		return true;
	}

	private bool IsDuplicateBinding(KeyCode thisKey)
	{
		for (int i = 0; i < GC.inputList.Count; i++)
		{
			if (currentButtonBind.gameObject.name == "Button-ConfirmUI")
			{
				if (thisKey == (KeyCode)GC.inputList[i].key && 29 == GC.inputList[i].type)
				{
					duplicateKey = (GameCore.InputLib)GC.inputList[i].type;
					return true;
				}
			}
			else if (currentButtonBind.gameObject.name == "Button-CancelUI")
			{
				if (thisKey == (KeyCode)GC.inputList[i].key && 28 == GC.inputList[i].type)
				{
					duplicateKey = (GameCore.InputLib)GC.inputList[i].type;
					return true;
				}
			}
			else if (thisKey == (KeyCode)GC.inputList[i].key && 28 != GC.inputList[i].type && 29 != GC.inputList[i].type)
			{
				duplicateKey = (GameCore.InputLib)GC.inputList[i].type;
				return true;
			}
		}
		return false;
	}

	private IEnumerator waitDelayPreKeyBinding(float wait)
	{
		yield return new WaitForSecondsRealtime(wait);
		isListeningForKeybindings = true;
		crtDelayPrekeybind = null;
	}

	private IEnumerator waitForDelayAfterSuccesfulKeybind(float wait)
	{
		isDelayAfterKeyBind = true;
		GC.delayAfterkeyBound = true;
		yield return new WaitForSecondsRealtime(wait);
		isDelayAfterKeyBind = false;
		GC.delayAfterkeyBound = false;
		crtDelayAfterkeybind = null;
	}

	private void ListenToggleDebugButtonsConfig()
	{
		if (GC.doAllowDebug && !menuControls.texts.btn_DbgHealLife.transform.parent.transform.parent.gameObject.activeInHierarchy)
		{
			menuControls.texts.btn_DbgHealLife.transform.parent.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.btn_DbgHealSpirit.transform.parent.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.btn_DbgRestoreDurability.transform.parent.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.btn_DbgResetArmor.transform.parent.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.btn_DbgTeleportList.transform.parent.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.btn_DbgTPDeviceBoss.transform.parent.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.btn_DbgDestroyArmor.transform.parent.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.btn_DbgInstantFainted.transform.parent.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.lbl_DbgHealLife.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.lbl_DbgHealSpirit.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.lbl_DbgRestoreDurability.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.lbl_DbgResetArmor.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.lbl_DbgTeleportList.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.lbl_DbgTPDeviceBoss.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.lbl_DbgDestroyArmor.transform.parent.gameObject.SetActive(value: true);
			menuControls.texts.lbl_DbgInstantFainted.transform.parent.gameObject.SetActive(value: true);
		}
		else if (!GC.doAllowDebug && menuControls.texts.btn_DbgHealLife.transform.parent.transform.parent.gameObject.activeInHierarchy)
		{
			menuControls.texts.btn_DbgHealLife.transform.parent.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.btn_DbgHealSpirit.transform.parent.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.btn_DbgRestoreDurability.transform.parent.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.btn_DbgResetArmor.transform.parent.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.btn_DbgTeleportList.transform.parent.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.btn_DbgTPDeviceBoss.transform.parent.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.btn_DbgDestroyArmor.transform.parent.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.btn_DbgInstantFainted.transform.parent.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.lbl_DbgHealLife.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.lbl_DbgHealSpirit.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.lbl_DbgRestoreDurability.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.lbl_DbgResetArmor.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.lbl_DbgTeleportList.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.lbl_DbgTPDeviceBoss.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.lbl_DbgDestroyArmor.transform.parent.gameObject.SetActive(value: false);
			menuControls.texts.lbl_DbgInstantFainted.transform.parent.gameObject.SetActive(value: false);
		}
	}

	private void LoadLanguageOfMenu(bool isLanguageChanged = false)
	{
		if (isLanguageChanged)
		{
			GC.ConcatenateSelectedLanguageFolderPath();
		}
		if (GC.LoadUIFile())
		{
			ApplyLanguageOfMenu();
		}
	}

	private void ApplyLanguageOfMenu()
	{
		if ((bool)menuMainTitle.txtVersion)
		{
			menuMainTitle.txtVersion.text = GC.strGameVersion + " (" + GC.GameExecType.ToString() + ")";
		}
		if ((bool)menuMainTitle.labelTitleGame)
		{
			menuMainTitle.labelTitleGame.text = GC.GetTranslatedStringUI(0);
		}
		if ((bool)menuMainTitle.labelTitleShadow)
		{
			menuMainTitle.labelTitleShadow.text = GC.GetTranslatedStringUI(0);
		}
		if ((bool)menuMainTitle.txtContinue)
		{
			menuMainTitle.txtContinue.text = GC.GetTranslatedStringUI(1);
		}
		if ((bool)menuMainTitle.txtNewGame)
		{
			menuMainTitle.txtNewGame.text = GC.GetTranslatedStringUI(2);
		}
		if ((bool)menuMainTitle.txtLoad)
		{
			menuMainTitle.txtLoad.text = GC.GetTranslatedStringUI(3);
		}
		if ((bool)menuMainTitle.txtGallery)
		{
			menuMainTitle.txtGallery.text = GC.GetTranslatedStringUI(4);
		}
		if ((bool)menuMainTitle.txtOptions)
		{
			menuMainTitle.txtOptions.text = GC.GetTranslatedStringUI(5);
		}
		if ((bool)menuMainTitle.txtQuit)
		{
			menuMainTitle.txtQuit.text = GC.GetTranslatedStringUI(6);
		}
		if ((bool)menuInGame.btnTextContinue)
		{
			menuInGame.btnTextContinue.text = GC.GetTranslatedStringUI(136);
		}
		if ((bool)menuInGame.btnTextRetry)
		{
			menuInGame.btnTextRetry.text = GC.GetTranslatedStringUI(46);
		}
		if ((bool)menuInGame.btnTextLoad)
		{
			menuInGame.btnTextLoad.text = GC.GetTranslatedStringUI(3);
		}
		if ((bool)menuInGame.btnTextSave)
		{
			menuInGame.btnTextSave.text = GC.GetTranslatedStringUI(42);
		}
		if ((bool)menuInGame.btnTextTeleport)
		{
			menuInGame.btnTextTeleport.text = GC.GetTranslatedStringUI(84);
		}
		if ((bool)menuInGame.btnTextInGameGallery)
		{
			menuInGame.btnTextInGameGallery.text = GC.GetTranslatedStringUI(4);
		}
		if ((bool)menuInGame.btnTextInventory)
		{
			menuInGame.btnTextInventory.text = GC.GetTranslatedStringUI(37);
		}
		if ((bool)menuInGame.btnTextStats)
		{
			menuInGame.btnTextStats.text = GC.GetTranslatedStringUI(38);
		}
		if ((bool)menuInGame.btnTextOptions)
		{
			menuInGame.btnTextOptions.text = GC.GetTranslatedStringUI(5);
		}
		if ((bool)menuInGame.btnTextReturnToTitle)
		{
			menuInGame.btnTextReturnToTitle.text = GC.GetTranslatedStringUI(39);
		}
		if ((bool)menuInGame.btnTextQuit)
		{
			menuInGame.btnTextQuit.text = GC.GetTranslatedStringUI(6);
		}
		if ((bool)menuOptions.labelTitleOptions)
		{
			menuOptions.labelTitleOptions.text = GC.GetTranslatedStringUI(5);
		}
		if ((bool)menuOptions.labelDifficulty)
		{
			menuOptions.labelDifficulty.text = GC.GetTranslatedStringUI(7) + ":";
		}
		if ((bool)menuOptions.txtDifficulty)
		{
			ChangeValueOfSelectionRealTime(controlTypes.Difficulty, isChangeID: false, 0);
		}
		if ((bool)menuOptions.labelLanguage)
		{
			menuOptions.labelLanguage.text = GC.GetTranslatedStringUI(10) + ":";
		}
		if ((bool)menuOptions.labelCamera)
		{
			menuOptions.labelCamera.text = GC.GetTranslatedStringUI(11) + ":";
		}
		if ((bool)menuOptions.txtCamera)
		{
			ChangeValueOfSelectionRealTime(controlTypes.Camera, isChangeID: false, 0);
		}
		if ((bool)menuOptions.labelTextSpeed)
		{
			menuOptions.labelTextSpeed.text = GC.GetTranslatedStringUI(34) + ":";
		}
		if ((bool)menuOptions.labelTextSound)
		{
			menuOptions.labelTextSound.text = GC.GetTranslatedStringUI(174) + ":";
		}
		if ((bool)menuOptions.labelMusic)
		{
			menuOptions.labelMusic.text = GC.GetTranslatedStringUI(14) + ":";
		}
		if ((bool)menuOptions.labelSound)
		{
			menuOptions.labelSound.text = GC.GetTranslatedStringUI(15) + ":";
		}
		if ((bool)menuOptions.labelVoice)
		{
			menuOptions.labelVoice.text = GC.GetTranslatedStringUI(16) + ":";
		}
		if ((bool)menuOptions.labelVoiceRate)
		{
			menuOptions.labelVoiceRate.text = GC.GetTranslatedStringUI(182) + ":";
		}
		if ((bool)menuOptions.txtControls)
		{
			menuOptions.txtControls.text = GC.GetTranslatedStringUI(17) + "...";
		}
		if ((bool)menuOptions.txtFetish)
		{
			menuOptions.txtFetish.text = GC.GetTranslatedStringUI(18) + "...";
		}
		if ((bool)menuOptions.txtAdvanced)
		{
			menuOptions.txtAdvanced.text = GC.GetTranslatedStringUI(35) + "...";
		}
		if ((bool)menuOptions.txtReturn)
		{
			menuOptions.txtReturn.text = GC.GetTranslatedStringUI(19);
		}
		if ((bool)menuControls.labelInputType)
		{
			menuControls.labelInputType.text = GC.GetTranslatedStringUI(31) + ":";
		}
		if ((bool)menuControls.buttonTextInputType)
		{
			ChangeValueOfSelectionRealTime(controlTypes.Input, isChangeID: false, 0);
		}
		if ((bool)menuControls.texts.labelAtack)
		{
			menuControls.texts.labelAtack.text = GC.GetTranslatedStringUI(20) + ":";
		}
		if ((bool)menuControls.texts.labelJump)
		{
			menuControls.texts.labelJump.text = GC.GetTranslatedStringUI(21) + ":";
		}
		if ((bool)menuControls.texts.labelAttackSkills)
		{
			menuControls.texts.labelAttackSkills.text = GC.GetTranslatedStringUI(99) + ":";
		}
		if ((bool)menuControls.texts.labelDefenseSkills)
		{
			menuControls.texts.labelDefenseSkills.text = GC.GetTranslatedStringUI(22) + ":";
		}
		if ((bool)menuControls.texts.labelCamera)
		{
			menuControls.texts.labelCamera.text = GC.GetTranslatedStringUI(23) + ":";
		}
		if ((bool)menuControls.texts.labelInternalView)
		{
			menuControls.texts.labelInternalView.text = GC.GetTranslatedStringUI(96) + ":";
		}
		if ((bool)menuControls.texts.labelInteract)
		{
			menuControls.texts.labelInteract.text = GC.GetTranslatedStringUI(24) + ":";
		}
		if ((bool)menuControls.texts.labelPauseMenu)
		{
			menuControls.texts.labelPauseMenu.text = GC.GetTranslatedStringUI(25) + ":";
		}
		if ((bool)menuControls.texts.labelUp)
		{
			menuControls.texts.labelUp.text = GC.GetTranslatedStringUI(26) + ":";
		}
		if ((bool)menuControls.texts.labelRight)
		{
			menuControls.texts.labelRight.text = GC.GetTranslatedStringUI(27) + ":";
		}
		if ((bool)menuControls.texts.labelDown)
		{
			menuControls.texts.labelDown.text = GC.GetTranslatedStringUI(28) + ":";
		}
		if ((bool)menuControls.texts.labelLeft)
		{
			menuControls.texts.labelLeft.text = GC.GetTranslatedStringUI(29) + ":";
		}
		if ((bool)menuControls.txtGPWarning)
		{
			menuControls.txtGPWarning.text = GC.GetTranslatedStringUI(141);
		}
		if ((bool)menuControls.texts.lblConfirmUI)
		{
			menuControls.texts.lblConfirmUI.text = GC.GetTranslatedStringUI(137) + ":";
		}
		if ((bool)menuControls.texts.lblCancelUI)
		{
			menuControls.texts.lblCancelUI.text = GC.GetTranslatedStringUI(138) + ":";
		}
		if ((bool)menuControls.texts.lblUseItemActive)
		{
			menuControls.texts.lblUseItemActive.text = GC.GetTranslatedStringUI(47) + ":";
		}
		if ((bool)menuControls.texts.lblNextItem)
		{
			menuControls.texts.lblNextItem.text = GC.GetTranslatedStringUI(48) + ":";
		}
		if ((bool)menuControls.texts.lblPreviousItem)
		{
			menuControls.texts.lblPreviousItem.text = GC.GetTranslatedStringUI(49) + ":";
		}
		if ((bool)menuControls.buttonResetDefault)
		{
			menuControls.buttonResetDefault.text = GC.GetTranslatedStringUI(30);
		}
		if ((bool)menuControls.titleControlConfig)
		{
			menuControls.titleControlConfig.text = GC.GetTranslatedStringUI(33);
		}
		if ((bool)menuControls.buttonTextReturn)
		{
			menuControls.buttonTextReturn.text = GC.GetTranslatedStringUI(19);
		}
		if ((bool)menuControls.texts.lbl_DbgHealLife)
		{
			menuControls.texts.lbl_DbgHealLife.text = GC.GetTranslatedStringUI(128);
		}
		if ((bool)menuControls.texts.lbl_DbgHealSpirit)
		{
			menuControls.texts.lbl_DbgHealSpirit.text = GC.GetTranslatedStringUI(129);
		}
		if ((bool)menuControls.texts.lbl_DbgRestoreDurability)
		{
			menuControls.texts.lbl_DbgRestoreDurability.text = GC.GetTranslatedStringUI(130);
		}
		if ((bool)menuControls.texts.lbl_DbgResetArmor)
		{
			menuControls.texts.lbl_DbgResetArmor.text = GC.GetTranslatedStringUI(131);
		}
		if ((bool)menuControls.texts.lbl_DbgTeleportList)
		{
			menuControls.texts.lbl_DbgTeleportList.text = GC.GetTranslatedStringUI(132);
		}
		if ((bool)menuControls.texts.lbl_DbgTPDeviceBoss)
		{
			menuControls.texts.lbl_DbgTPDeviceBoss.text = GC.GetTranslatedStringUI(133);
		}
		if ((bool)menuControls.texts.lbl_DbgDestroyArmor)
		{
			menuControls.texts.lbl_DbgDestroyArmor.text = GC.GetTranslatedStringUI(134);
		}
		if ((bool)menuControls.texts.lbl_DbgInstantFainted)
		{
			menuControls.texts.lbl_DbgInstantFainted.text = GC.GetTranslatedStringUI(135);
		}
		if ((bool)menuInventory.labelTitleInv)
		{
			menuInventory.labelTitleInv.text = GC.GetTranslatedStringUI(37);
		}
		if ((bool)menuInventory.uiMenuBelt.labelSelectSlot)
		{
			menuInventory.uiMenuBelt.labelSelectSlot.text = GC.GetTranslatedStringUI(155);
		}
		if ((bool)menuInventory.btnTextReturnFromInv)
		{
			menuInventory.btnTextReturnFromInv.text = GC.GetTranslatedStringUI(19);
		}
		if ((bool)menuAdvanced.titleAdvanced)
		{
			menuAdvanced.titleAdvanced.text = GC.GetTranslatedStringUI(35);
		}
		if ((bool)menuAdvanced.labelCheck1)
		{
			menuAdvanced.labelCheck1.text = GC.GetTranslatedStringUI(36);
		}
		if ((bool)menuAdvanced.labelCheck2)
		{
			menuAdvanced.labelCheck2.text = GC.GetTranslatedStringUI(40);
		}
		if ((bool)menuAdvanced.labelCheck3)
		{
			menuAdvanced.labelCheck3.text = GC.GetTranslatedStringUI(41);
		}
		if ((bool)menuAdvanced.labelCheck4)
		{
			menuAdvanced.labelCheck4.text = GC.GetTranslatedStringUI(44);
		}
		if ((bool)menuAdvanced.labelCheck5)
		{
			menuAdvanced.labelCheck5.text = GC.GetTranslatedStringUI(45);
		}
		if ((bool)menuAdvanced.labelCheck6)
		{
			menuAdvanced.labelCheck6.text = GC.GetTranslatedStringUI(51);
		}
		if ((bool)menuAdvanced.labelCheck7)
		{
			menuAdvanced.labelCheck7.text = GC.GetTranslatedStringUI(83);
		}
		if ((bool)menuAdvanced.labelCheck8)
		{
			menuAdvanced.labelCheck8.text = GC.GetTranslatedStringUI(97);
		}
		if ((bool)menuAdvanced.labelCheck9)
		{
			menuAdvanced.labelCheck9.text = GC.GetTranslatedStringUI(98);
		}
		if ((bool)menuAdvanced.labelCheck10)
		{
			menuAdvanced.labelCheck10.text = GC.GetTranslatedStringUI(116);
		}
		if ((bool)menuAdvanced.labelCheck11)
		{
			menuAdvanced.labelCheck11.text = GC.GetTranslatedStringUI(126);
		}
		if ((bool)menuAdvanced.labelCheck12)
		{
			menuAdvanced.labelCheck12.text = GC.GetTranslatedStringUI(123);
		}
		if ((bool)menuAdvanced.labelCheck13)
		{
			menuAdvanced.labelCheck13.text = GC.GetTranslatedStringUI(144);
		}
		if ((bool)menuAdvanced.labelCheck14)
		{
			menuAdvanced.labelCheck14.text = GC.GetTranslatedStringUI(157);
		}
		if ((bool)menuAdvanced.labelCheck15)
		{
			menuAdvanced.labelCheck15.text = GC.GetTranslatedStringUI(162);
		}
		if ((bool)menuAdvanced.labelCheck16)
		{
			menuAdvanced.labelCheck16.text = GC.GetTranslatedStringUI(176);
		}
		if ((bool)menuAdvanced.labelCheck17)
		{
			menuAdvanced.labelCheck17.text = GC.GetTranslatedStringUI(186);
		}
		if ((bool)menuAdvanced.btnTextReturnFromAdv)
		{
			menuAdvanced.btnTextReturnFromAdv.text = GC.GetTranslatedStringUI(19);
		}
		if ((bool)menuStats.txtTitleStats)
		{
			menuStats.txtTitleStats.text = GC.GetTranslatedStringUI(38);
		}
		if ((bool)menuStats.labelLust)
		{
			menuStats.labelLust.text = GC.GetTranslatedStringUI(69) + ":";
		}
		if ((bool)menuStats.labelQuartz)
		{
			menuStats.labelQuartz.text = GC.GetTranslatedStringUI(70) + ":";
		}
		if ((bool)menuStats.labelPlayTime)
		{
			menuStats.labelPlayTime.text = GC.GetTranslatedStringUI(71) + ":";
		}
		if ((bool)menuFetish.titleFetish)
		{
			menuFetish.titleFetish.text = GC.GetTranslatedStringUI(18);
		}
		if ((bool)menuFetish.labelCheckVanilla)
		{
			menuFetish.labelCheckVanilla.text = GC.GetTranslatedStringUI(73);
		}
		if ((bool)menuFetish.labelCheckBlowjob)
		{
			menuFetish.labelCheckBlowjob.text = GC.GetTranslatedStringUI(74);
		}
		if ((bool)menuFetish.labelCheckFeet)
		{
			menuFetish.labelCheckFeet.text = GC.GetTranslatedStringUI(75);
		}
		if ((bool)menuFetish.labelCheckLesbian)
		{
			menuFetish.labelCheckLesbian.text = GC.GetTranslatedStringUI(76);
		}
		if ((bool)menuFetish.labelCheckFutanari)
		{
			menuFetish.labelCheckFutanari.text = GC.GetTranslatedStringUI(77);
		}
		if ((bool)menuFetish.labelCheckTentacle)
		{
			menuFetish.labelCheckTentacle.text = GC.GetTranslatedStringUI(78);
		}
		if ((bool)menuFetish.labelCheckBeast)
		{
			menuFetish.labelCheckBeast.text = GC.GetTranslatedStringUI(79);
		}
		if ((bool)menuFetish.labelCheckInsect)
		{
			menuFetish.labelCheckInsect.text = GC.GetTranslatedStringUI(80);
		}
		if ((bool)menuFetish.labelCheckRyona)
		{
			menuFetish.labelCheckRyona.text = GC.GetTranslatedStringUI(81);
		}
		if ((bool)menuFetish.labelCheckVore)
		{
			menuFetish.labelCheckVore.text = GC.GetTranslatedStringUI(82);
		}
		if ((bool)menuFetish.btnTextReturnFromFetish)
		{
			menuFetish.btnTextReturnFromFetish.text = GC.GetTranslatedStringUI(19);
		}
		menu_NPC_Farina.txtTalk.text = GC.GetTranslatedStringUI(163);
		menu_NPC_Farina.txtHeal.text = GC.GetTranslatedStringUI(164);
		menu_NPC_Farina.txtRepair.text = GC.GetTranslatedStringUI(165);
		menu_NPC_Farina.txtHExit.text = GC.GetTranslatedStringUI(166);
		menu_NPC_Farina.txtReturn.text = "← " + GC.GetTranslatedStringUI(19) + " ...";
		menu_NPC_Nene.txtShop.text = GC.GetTranslatedStringUI(169);
		menu_NPC_Nene.txtTalk.text = GC.GetTranslatedStringUI(163);
		menu_NPC_Nene.txtExit.text = GC.GetTranslatedStringUI(166);
		menu_NPC_Nene.txtReturnFromTalk.text = "← " + GC.GetTranslatedStringUI(19) + " ...";
		menu_NPC_Nene.txtReturnFromShop.text = "← " + GC.GetTranslatedStringUI(19) + " ...";
		GC.UpdateLanguageOfItemsInPlayerInventory();
	}

	private void CloseIndividualWindowOnCancelKeyPress()
	{
		if (closeMenuWindows.windowMainMenu.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseMainMenu.onClick.Invoke();
		}
		if (closeMenuWindows.windowOptions.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseOptions.onClick.Invoke();
		}
		if (closeMenuWindows.windowControls.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseControls.onClick.Invoke();
		}
		if (closeMenuWindows.windowInventory.activeInHierarchy)
		{
			if (menuInventory.objEquipRoot.activeInHierarchy)
			{
				CloseBeltEquipSlotScreen();
			}
			else
			{
				closeMenuWindows.buttonCloseInventory.onClick.Invoke();
			}
		}
		if (closeMenuWindows.windowAdvanced.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseAdvanced.onClick.Invoke();
		}
		if (closeMenuWindows.windowSaveLoad.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseSaveLoad.onClick.Invoke();
		}
		if (closeMenuWindows.windowFetish.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseFetish.onClick.Invoke();
		}
		if (closeMenuWindows.windowTeleport.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseTeleport.onClick.Invoke();
		}
		if (closeMenuWindows.windowGallery.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseGallery.onClick.Invoke();
		}
		if (menu_NPC_Farina.objTalkNode.activeInHierarchy && !GC.isDisplayingNPCMenuTalkDialogue && !GC.isGamePaused)
		{
			menu_NPC_Farina.btnBackFromTalk.onClick.Invoke();
		}
		if (menu_NPC_Nene.objTalkNode.activeInHierarchy && !GC.isDisplayingNPCMenuTalkDialogue && !GC.isGamePaused)
		{
			menu_NPC_Nene.btnBackFromTalk.onClick.Invoke();
		}
		if (menu_NPC_Nene.objShopNode.activeInHierarchy && !GC.isDisplayingNPCMenuTalkDialogue && !GC.isGamePaused)
		{
			menu_NPC_Nene.btnBackFromShop.onClick.Invoke();
		}
	}

	public void SpecialCloseAllWindows()
	{
		if (closeMenuWindows.windowOptions.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseOptionsSpecial.onClick.Invoke();
		}
		if (closeMenuWindows.windowControls.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseControlsSpecial.onClick.Invoke();
		}
		if (closeMenuWindows.windowInventory.activeInHierarchy)
		{
			CloseBeltEquipSlotScreen();
			closeMenuWindows.buttonCloseInventorySpecial.onClick.Invoke();
		}
		if (closeMenuWindows.windowAdvanced.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseAdvancedSpecial.onClick.Invoke();
		}
		if (closeMenuWindows.windowSaveLoad.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseSaveLoadSpecial.onClick.Invoke();
		}
		if (closeMenuWindows.windowFetish.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseFetishSpecial.onClick.Invoke();
		}
		if (closeMenuWindows.windowTeleport.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseTeleportSpecial.onClick.Invoke();
		}
		if (closeMenuWindows.windowGallery.activeInHierarchy)
		{
			closeMenuWindows.buttonCloseGallerySpecial.onClick.Invoke();
		}
	}

	private void UpdateStatsScreen()
	{
		EvaluateLastSeenValuesReferences();
		if ((bool)menuStats.txtHP)
		{
			menuStats.txtHP.text = "<color=red>" + GC.GetTranslatedStringUI(65) + "</color>: " + GC.playerMaxHealth + strHP;
		}
		if ((bool)menuStats.txtSP)
		{
			menuStats.txtSP.text = "<color=cyan>" + GC.GetTranslatedStringUI(66) + "</color>: " + GC.playerMaxSpirit + strSP;
		}
		if ((bool)menuStats.txtATK)
		{
			menuStats.txtATK.text = "<color=yellow>" + GC.GetTranslatedStringUI(67) + "</color>: " + GC.playerCurrentBasicAttackPower + strATK;
		}
		if ((bool)menuStats.txtDEF)
		{
			menuStats.txtDEF.text = "<color=silver>" + GC.GetTranslatedStringUI(68) + "</color>: " + GC.playerCurrentBasicDefensePower + strDEF;
		}
		if ((bool)menuStats.txtLust)
		{
			menuStats.txtLust.text = "<color=#FF80FF>" + GC.playerLust + "</color>%";
		}
		if ((bool)menuStats.txtQuartz)
		{
			menuStats.txtQuartz.text = GC.playerCurrentEssenceAmount + strESSE;
		}
		timeSpan = TimeSpan.FromSeconds(GC.timeTotalGamePlayedSeconds);
		timeText = $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
		if ((bool)menuStats.txtPlayTime)
		{
			menuStats.txtPlayTime.text = timeText;
		}
		SaveStoreLastSeenValues();
	}

	private void EvaluateLastSeenValuesReferences()
	{
		WorkOnIndividualLastSeenReferences(ref GC.clsLastSeenStats.Health, GC.playerMaxHealth, ref strHP);
		WorkOnIndividualLastSeenReferences(ref GC.clsLastSeenStats.Spirit, GC.playerMaxSpirit, ref strSP);
		WorkOnIndividualLastSeenReferences(ref GC.clsLastSeenStats.Attack, GC.playerCurrentBasicAttackPower, ref strATK);
		WorkOnIndividualLastSeenReferences(ref GC.clsLastSeenStats.Defense, GC.playerCurrentBasicDefensePower, ref strDEF);
		WorkOnIndividualLastSeenReferences(ref GC.clsLastSeenStats.Quartz, GC.playerCurrentEssenceAmount, ref strESSE);
	}

	private void SaveStoreLastSeenValues()
	{
		if (GC.clsLastSeenStats.Health != GC.playerMaxHealth)
		{
			GC.clsLastSeenStats.Health = GC.playerMaxHealth;
		}
		if (GC.clsLastSeenStats.Spirit != GC.playerMaxSpirit)
		{
			GC.clsLastSeenStats.Spirit = GC.playerMaxSpirit;
		}
		if (GC.clsLastSeenStats.Attack != GC.playerCurrentBasicAttackPower)
		{
			GC.clsLastSeenStats.Attack = GC.playerCurrentBasicAttackPower;
		}
		if (GC.clsLastSeenStats.Defense != GC.playerCurrentBasicDefensePower)
		{
			GC.clsLastSeenStats.Defense = GC.playerCurrentBasicDefensePower;
		}
		if (GC.clsLastSeenStats.Quartz != GC.playerCurrentEssenceAmount)
		{
			GC.clsLastSeenStats.Quartz = GC.playerCurrentEssenceAmount;
		}
	}

	private void WorkOnIndividualLastSeenReferences(ref float valueLastSeen, float valueCurrentGC, ref string stringOutput)
	{
		if (valueLastSeen != 0f && valueLastSeen != valueCurrentGC)
		{
			stringOutput = FormatLastSeenString(valueCurrentGC - valueLastSeen);
			valueLastSeen = valueCurrentGC;
		}
	}

	private void WorkOnIndividualLastSeenReferences(ref int valueLastSeen, int valueCurrentGC, ref string stringOutput)
	{
		if (valueLastSeen != 0 && valueLastSeen != valueCurrentGC)
		{
			stringOutput = FormatLastSeenString(valueCurrentGC - valueLastSeen);
			valueLastSeen = valueCurrentGC;
		}
	}

	private string FormatLastSeenString(float number)
	{
		string text = "";
		string text2 = "<color=#00FF15>";
		if (number > 0f)
		{
			text = "+";
		}
		else
		{
			text2 = "<color=red>";
		}
		text += number;
		return "<size=20>" + text2 + " (" + text + ")</color></size>";
	}

	public void ClearlastSeenStrings()
	{
		strHP = "";
		strSP = "";
		strATK = "";
		strDEF = "";
		strESSE = "";
	}

	private void ApplyTextDressState()
	{
		menuDressState.txt1Full.text = GC.GetTranslatedStringUI(148);
		menuDressState.txt1Full.color = Color.white;
		menuDressState.imgIcon1Full.sprite = menuDressState.spriteImageFull;
		menuDressState.txt2Half.text = GC.GetTranslatedStringUI(149);
		menuDressState.txt2Half.color = Color.white;
		menuDressState.imgIcon2Half.sprite = menuDressState.spriteImageHalf;
		menuDressState.txt3naked.text = GC.GetTranslatedStringUI(150);
		menuDressState.txt3naked.color = Color.white;
		menuDressState.imgIcon3Naked.sprite = menuDressState.spriteImageNaked;
		if (GC.playerCurrentDressStateValidUnbroken != 0)
		{
			menuDressState.txt1Full.text = GC.GetTranslatedStringUI(151);
			menuDressState.txt1Full.color = Color.red;
			menuDressState.imgIcon1Full.sprite = menuDressState.spriteImageBroken;
			if (GC.playerCurrentDressStateValidUnbroken != 1)
			{
				menuDressState.txt2Half.text = GC.GetTranslatedStringUI(151);
				menuDressState.txt2Half.color = Color.red;
				menuDressState.imgIcon2Half.sprite = menuDressState.spriteImageBroken;
			}
		}
		LightUpBackgroundUIDressState();
	}

	private void LightUpBackgroundUIDressState()
	{
		menuDressState.imgBG1Full.color = clrBGUnselected;
		menuDressState.imgBG2Half.color = clrBGUnselected;
		menuDressState.imgBG3Naked.color = clrBGUnselected;
		switch (GC.playerCurrentDressState)
		{
		case 0:
			menuDressState.imgBG1Full.color = clrBGselected;
			break;
		case 1:
			menuDressState.imgBG2Half.color = clrBGselected;
			break;
		case 2:
			menuDressState.imgBG3Naked.color = clrBGselected;
			break;
		}
		ChangeDressLayerTextDescription();
	}

	public void ButtonPressDressState(int buttonID)
	{
		if (GC.CheckPlayerDressStateValidation(ignorePaused: true))
		{
			if (ValidateOtherLayerConditions(buttonID))
			{
				GC.ForceChangeCurrentPlayerDressState(buttonID, isIntentionalPlayerRemoval: true);
				ApplyTextDressState();
				CloseIndividualWindowOnCancelKeyPress();
			}
		}
		else
		{
			CloseIndividualWindowOnCancelKeyPress();
			GC.DisplaySystemMessageUI(0, 152);
		}
	}

	private bool CheckRefreshUIDressState()
	{
		switch (GC.playerCurrentDressState)
		{
		case 0:
			if (menuDressState.imgBG1Full.color != clrBGselected)
			{
				return true;
			}
			break;
		case 1:
			if (menuDressState.imgBG2Half.color != clrBGselected)
			{
				return true;
			}
			break;
		case 2:
			if (menuDressState.imgBG3Naked.color != clrBGselected)
			{
				return true;
			}
			break;
		}
		return false;
	}

	private bool ValidateOtherLayerConditions(int layerID)
	{
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Exhibitionist))
		{
			CloseIndividualWindowOnCancelKeyPress();
			GC.DisplaySystemMessageUI(0, 181);
			return false;
		}
		if ((layerID == 0 || layerID == 1) && GC.playerCurrentDressStateValidUnbroken > layerID)
		{
			CloseIndividualWindowOnCancelKeyPress();
			GC.DisplaySystemMessageUI(0, 153);
			return false;
		}
		if (layerID == GC.playerCurrentDressState)
		{
			CloseIndividualWindowOnCancelKeyPress();
			GC.DisplaySystemMessageUI(0, 154);
			return false;
		}
		return true;
	}

	private void ChangeDressLayerTextDescription(int forcedValue = -1)
	{
		if (forcedValue <= -1)
		{
			forcedValue = GC.playerCurrentDressState;
		}
		switch (forcedValue)
		{
		case 0:
			if (GC.playerCurrentDressStateValidUnbroken != 0)
			{
				menuDressState.txtLayerDescription.text = GC.GetTranslatedStringUI(151);
			}
			else
			{
				menuDressState.txtLayerDescription.text = GC.GetTranslatedStringUI(1003);
			}
			menuDressState.txtLayerDescription.color = clrLayer0;
			break;
		case 1:
			if (GC.playerCurrentDressStateValidUnbroken != 0 && GC.playerCurrentDressStateValidUnbroken != 1)
			{
				menuDressState.txtLayerDescription.text = GC.GetTranslatedStringUI(151);
			}
			else
			{
				menuDressState.txtLayerDescription.text = GC.GetTranslatedStringUI(1004);
			}
			menuDressState.txtLayerDescription.color = clrLayer1;
			break;
		case 2:
			menuDressState.txtLayerDescription.text = GC.GetTranslatedStringUI(1005);
			menuDressState.txtLayerDescription.color = clrLayer2;
			break;
		}
	}

	private void WatchDressLayerButtonSelectionToChangeTextDescription()
	{
		if (GC.currentSelectedUISelectable == menuDressState.btn1)
		{
			ChangeDressLayerTextDescription(0);
		}
		else if (GC.currentSelectedUISelectable == menuDressState.btn2)
		{
			ChangeDressLayerTextDescription(1);
		}
		else if (GC.currentSelectedUISelectable == menuDressState.btn3)
		{
			ChangeDressLayerTextDescription(2);
		}
		else
		{
			ChangeDressLayerTextDescription();
		}
	}

	public void CallRetry()
	{
		TryToLoadTheGame(0, isRetry: true);
	}

	public void CallGalleryMenu()
	{
		if (galleryScreenEnabled)
		{
			GC.TogglePauseForGameMenu();
			menuInGame.btnTextInGameGallery.transform.parent.GetComponent<Button>().onClick.Invoke();
		}
		else
		{
			GC.DisplaySystemMessageUI(0, 184, 5f);
		}
	}

	private void InitiateTextFlashing(Text thisText, float howLong, float intervalFlash, Color colorA, Color colorB)
	{
		if (crtFlashText != null)
		{
			thisText.color = DefaultColorOfTexts;
			StopCoroutine(crtFlashText);
		}
		crtFlashText = StartCoroutine(waitFlashingText(thisText, howLong, intervalFlash, colorA, colorB));
	}

	private void SetArtOfToggleOption(Button thisButton, bool state)
	{
		Sprite sprite = null;
		sprite = ((!state) ? menuOptions.toggleOFF : menuOptions.toggleON);
		thisButton.GetComponent<Image>().sprite = sprite;
	}

	private IEnumerator waitFlashingText(Text thisText, float howLong, float intervalFlash, Color colorA, Color colorB)
	{
		float timeCount = 0f;
		if (intervalFlash <= 0f)
		{
			intervalFlash = 0.1f;
		}
		if (!thisText.gameObject.activeInHierarchy)
		{
			thisText.gameObject.SetActive(value: true);
		}
		for (; timeCount < howLong; timeCount += intervalFlash)
		{
			yield return new WaitForSecondsRealtime(intervalFlash);
			if (thisText.color == colorA)
			{
				thisText.color = colorB;
			}
			else
			{
				thisText.color = colorA;
			}
		}
		thisText.color = DefaultColorOfTexts;
		thisText.gameObject.SetActive(value: false);
		crtFlashText = null;
	}
}
