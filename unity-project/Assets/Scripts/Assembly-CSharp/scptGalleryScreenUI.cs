using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class scptGalleryScreenUI : MonoBehaviour
{
	private enum LocalNavigation
	{
		Left = 0,
		Right = 1
	}

	private enum LocalSelection
	{
		Theme = 0,
		Enemy = 1
	}

	private enum LocalFOTState
	{
		Neutral = 0,
		ForceAllow = 1,
		ForceBlock = 2
	}

	public Button btnChooseStageTheme;

	public Text txtChooseStageTheme;

	public Button btnMainEnemySprite;

	public Image imgMainEnemySprite;

	public Text txtEnemyName;

	public Text txtEnemyFetish;

	public Text txtReturnFromGallery;

	public Image imgEnemyPanel;

	public Image imgThemeArrowRight;

	public Image imgThemeArrowLeft;

	public Image imgEnemyArrowRight;

	public Image imgEnemyArrowLeft;

	public Image imgThemeBKG;

	public Color colorDefaultEnemyPanel;

	public Color colorDefaultArrows;

	public Color colorSelectedAny;

	public sblGalleryEnemyList sbjBeachList;

	public sblGalleryEnemyList sbjForestList;

	public sblGalleryEnemyList sbjHillList;

	public sblGalleryEnemyList sbjCity;

	public sblGalleryEnemyList sbjCave;

	public sblGalleryEnemyList sbjSwamp;

	public sblGalleryEnemyList sbjCastle;

	public sblGalleryEnemyList sbjHighTech;

	public GameObject hackWormEnemySpawn;

	public Color colorThemeBeach;

	public Color colorThemeForest;

	public Color colorThemeHill;

	public Color colorThemeCity;

	public Color colorThemeCave;

	public Color colorThemeSwamp;

	public Color colorThemeCastle;

	public Color colorThemeHighTech;

	public Sprite spriteBeach;

	public Sprite spriteForest;

	public Sprite spriteHill;

	public Sprite spriteCity;

	public Sprite spriteCave;

	public Sprite spriteSwamp;

	public Sprite spriteCastle;

	public Sprite spriteHighTech;

	public Sprite EnemyFetishOFF;

	public Image imgOverrideState;

	public Text txtOverrideDisclaimer;

	public Text txtOverrideState;

	public Sprite sprOverrideNeutral;

	public Sprite sprOverrideAllow;

	public Sprite sprOverrideBlock;

	public GameObject containerHasBadEnd;

	private GameCore GC;

	private List<GameCore.GalleryThemeType> _listOfUnlockedThemes;

	private GameCore.GalleryThemeType _selectedTheme;

	private clsGalleryEnemyList[] _currentEnemyArray;

	private GameObject _currentPrefabToSpawn;

	private Vector3 _defaultSpriteDisplayScaling = new Vector3(6f, 6f, 1f);

	private Vector3 _arenaAreaSpawn = new Vector3(-119f, -1.5f, 0f);

	private Vector3 _arenaGroundSpawn = new Vector3(-119f, -4f, 0f);

	private Vector3 _arenaDigSpawn = new Vector3(-119f, -6.5f, 0f);

	private Vector3 _currentOffsetSpawn;

	private LocalFOTState _localFOT;

	private int _currentThemeIndex;

	private int _currentEnemyIndex;

	private int _currentEroVariantID;

	private float speedArrowAnim;

	private bool checkPreviousThemeIndex;

	private bool checkPreviousEnemyIndex;

	private bool switchArrowAnim;

	private void OnEnable()
	{
		GC = GameCore.Instance;
		InitialResetLogicState();
	}

	private void InitialResetLogicState()
	{
		_currentThemeIndex = 0;
		_currentEnemyIndex = 0;
		speedArrowAnim = 4f;
		checkPreviousThemeIndex = true;
		checkPreviousEnemyIndex = true;
		PopulateThemeList();
		SetSelectedTheme();
		RefreshEnemyArray();
		SetSelectedEnemy();
		containerHasBadEnd.SetActive(value: false);
	}

	private void PopulateThemeList()
	{
		if (_listOfUnlockedThemes == null || _listOfUnlockedThemes.Count <= 0)
		{
			_listOfUnlockedThemes = new List<GameCore.GalleryThemeType>();
		}
		else
		{
			_listOfUnlockedThemes.Clear();
		}
		if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Beach))
		{
			_listOfUnlockedThemes.Add(GameCore.GalleryThemeType.Beach);
		}
		if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Forest))
		{
			_listOfUnlockedThemes.Add(GameCore.GalleryThemeType.Forest);
		}
		if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Hill))
		{
			_listOfUnlockedThemes.Add(GameCore.GalleryThemeType.Hill);
		}
		if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.City))
		{
			_listOfUnlockedThemes.Add(GameCore.GalleryThemeType.City);
		}
		if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Cave))
		{
			_listOfUnlockedThemes.Add(GameCore.GalleryThemeType.Cave);
		}
		if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Castle))
		{
			_listOfUnlockedThemes.Add(GameCore.GalleryThemeType.Castle);
		}
		if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.Swamp))
		{
			_listOfUnlockedThemes.Add(GameCore.GalleryThemeType.Swamp);
		}
		if (GC.GalleryValidateRoomAndView(GameCore.GalleryThemeType.HighTech))
		{
			_listOfUnlockedThemes.Add(GameCore.GalleryThemeType.HighTech);
		}
	}

	private void RefreshEnemyArray()
	{
		clsGalleryEnemyList[] array = null;
		switch (_selectedTheme)
		{
		default:
			array = sbjForestList.listOfEnemyForGallery;
			break;
		case GameCore.GalleryThemeType.Beach:
			array = sbjBeachList.listOfEnemyForGallery;
			break;
		case GameCore.GalleryThemeType.Hill:
			array = sbjHillList.listOfEnemyForGallery;
			break;
		case GameCore.GalleryThemeType.City:
			array = sbjCity.listOfEnemyForGallery;
			break;
		case GameCore.GalleryThemeType.Cave:
			array = sbjCave.listOfEnemyForGallery;
			break;
		case GameCore.GalleryThemeType.Castle:
			array = sbjCastle.listOfEnemyForGallery;
			break;
		case GameCore.GalleryThemeType.Swamp:
			array = sbjSwamp.listOfEnemyForGallery;
			break;
		case GameCore.GalleryThemeType.HighTech:
			array = sbjHighTech.listOfEnemyForGallery;
			break;
		}
		_currentEnemyArray = new clsGalleryEnemyList[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			_currentEnemyArray[i] = array[i];
		}
		SortEnemyArray();
	}

	private void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == btnMainEnemySprite.gameObject)
		{
			if (Input.GetKeyDown(GC.IKey(GameCore.InputLib.Left)) || GC.UnifiedInputHandler(GameCore.InputLib.Left, GameCore.InputPressType.KeyPressedOnce))
			{
				NavigateEnemyList(LocalNavigation.Left);
			}
			if (Input.GetKeyDown(GC.IKey(GameCore.InputLib.Right)) || GC.UnifiedInputHandler(GameCore.InputLib.Right, GameCore.InputPressType.KeyPressedOnce))
			{
				NavigateEnemyList(LocalNavigation.Right);
			}
			SetSelectionColor(LocalSelection.Enemy, isDefault: false);
		}
		else
		{
			SetSelectionColor(LocalSelection.Enemy, isDefault: true);
		}
		if (EventSystem.current.currentSelectedGameObject == btnChooseStageTheme.gameObject)
		{
			if (Input.GetKeyDown(GC.IKey(GameCore.InputLib.Left)) || GC.UnifiedInputHandler(GameCore.InputLib.Left, GameCore.InputPressType.KeyPressedOnce))
			{
				NavigateThemeList(LocalNavigation.Left);
			}
			if (Input.GetKeyDown(GC.IKey(GameCore.InputLib.Right)) || GC.UnifiedInputHandler(GameCore.InputLib.Right, GameCore.InputPressType.KeyPressedOnce))
			{
				NavigateThemeList(LocalNavigation.Right);
			}
			SetSelectionColor(LocalSelection.Theme, isDefault: false);
		}
		else
		{
			SetSelectionColor(LocalSelection.Theme, isDefault: true);
		}
		if (base.isActiveAndEnabled && GC.UnifiedInputHandler(GameCore.InputLib.Interact, GameCore.InputPressType.KeyPressedOnce))
		{
			ToggleOverrideFetishAnim();
		}
		if ((bool)GC)
		{
			GC.UI_SELECTION_StoreLastSelected();
			GC.UI_SELECTION_HighlightElementIfNoneAreCurrentlySelected();
		}
	}

	public void ButtonClicked(string action)
	{
		switch (action)
		{
		case "THEME":
		case "RIGHT-THEME":
			NavigateThemeList(LocalNavigation.Right);
			break;
		case "LEFT-THEME":
			NavigateThemeList(LocalNavigation.Left);
			break;
		case "RIGHT-ENEMY":
			NavigateEnemyList(LocalNavigation.Right);
			break;
		case "LEFT-ENEMY":
			NavigateEnemyList(LocalNavigation.Left);
			break;
		case "ENEMY":
			if (Check_GA_LogicPosition())
			{
				StartGallerySpecialViewLogic();
			}
			else
			{
				StartArenaEnemySpawnLogic();
			}
			break;
		}
	}

	private void StartGallerySpecialViewLogic()
	{
		if (!(imgMainEnemySprite.sprite == EnemyFetishOFF))
		{
			GC.SetupGalleryViewBeforeLoad(_currentEnemyArray[_currentEnemyIndex].logicType, _currentPrefabToSpawn, _currentEnemyArray[_currentEnemyIndex].enemyCombo, _currentEroVariantID, _currentOffsetSpawn);
			GC.galleryScreenLastThemeIndex = _currentThemeIndex;
			GC.galleryScreenLastEnemyIndex = _currentEnemyIndex;
		}
	}

	private void NavigateThemeList(LocalNavigation direction)
	{
		if (direction == LocalNavigation.Right)
		{
			if (_currentThemeIndex + 1 >= _listOfUnlockedThemes.Count)
			{
				_currentThemeIndex = 0;
			}
			else
			{
				_currentThemeIndex++;
			}
		}
		else if (_currentThemeIndex - 1 < 0)
		{
			_currentThemeIndex = _listOfUnlockedThemes.Count - 1;
		}
		else
		{
			_currentThemeIndex--;
		}
		GC.PlaySFX(GameCore.SFXBasic1.DialogueNext2, 0, checkForDuplicity: true, Vector2.zero, 1.2f, 1.2f);
		SetSelectedTheme(wasThemeNavigation: true);
	}

	private void SetSelectedTheme(bool wasThemeNavigation = false)
	{
		if (checkPreviousThemeIndex)
		{
			checkPreviousThemeIndex = false;
			_currentThemeIndex = AcquireLastSelectedThemeIndex();
		}
		_selectedTheme = _listOfUnlockedThemes[_currentThemeIndex];
		txtChooseStageTheme.text = TranslateCurrentThemeName();
		imgThemeBKG.color = GetThemeBackgroundColor();
		imgThemeBKG.sprite = GetThemeBackgroundSprite();
		RefreshEnemyArray();
		SetSelectedEnemy(wasThemeNavigation);
	}

	private int AcquireLastSelectedThemeIndex()
	{
		if (GC.galleryScreenLastThemeIndex != 0 && GC.galleryScreenLastThemeIndex <= _listOfUnlockedThemes.Count - 1)
		{
			return GC.galleryScreenLastThemeIndex;
		}
		GC.galleryScreenLastThemeIndex = 0;
		return 0;
	}

	private void NavigateEnemyList(LocalNavigation direction)
	{
		if (direction == LocalNavigation.Right)
		{
			if (_currentEnemyIndex + 1 >= _currentEnemyArray.Length)
			{
				_currentEnemyIndex = 0;
			}
			else
			{
				_currentEnemyIndex++;
			}
		}
		else if (_currentEnemyIndex - 1 < 0)
		{
			_currentEnemyIndex = _currentEnemyArray.Length - 1;
		}
		else
		{
			_currentEnemyIndex--;
		}
		GC.PlaySFX(GameCore.SFXBasic1.DialogueNext1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		SetSelectedEnemy();
	}

	private void SetSelectedEnemy(bool wasThemeNavigation = false)
	{
		if (wasThemeNavigation)
		{
			_currentEnemyIndex = 0;
		}
		if (checkPreviousEnemyIndex)
		{
			checkPreviousEnemyIndex = false;
			_currentEnemyIndex = AcquireLastSelectedEnemyIndex();
		}
		_currentPrefabToSpawn = _currentEnemyArray[_currentEnemyIndex].enemyPrefab;
		_currentEroVariantID = _currentEnemyArray[_currentEnemyIndex].EroVariantID;
		_currentOffsetSpawn = _currentEnemyArray[_currentEnemyIndex].OffsetTPPos;
		UpdateFOTStateOfCurrentAnim();
		imgMainEnemySprite.sprite = GetEnemySpriteBasedOnFetishToggle();
		txtEnemyName.text = TranslateCurrentEnemyName(_currentEnemyArray[_currentEnemyIndex].idNameJSON);
		txtEnemyFetish.text = GC.GetFetishNameByType(GC.GetFetishTypeByEroSceneVariantID(_currentEroVariantID, _currentPrefabToSpawn.GetComponent<EnemyGeneralLogic>().eroCore));
		GetBadEndInfo();
		if (_currentEroVariantID != 0)
		{
			Text text = txtEnemyName;
			text.text = text.text + " " + _currentEroVariantID;
		}
		if (_currentEnemyArray[_currentEnemyIndex].enemyCombo != null)
		{
			Text text2 = txtEnemyName;
			text2.text = text2.text + " " + GC.GetTranslatedStringUI(161);
		}
		if (_currentEnemyArray[_currentEnemyIndex].customScaleSprite <= 0 || imgMainEnemySprite.sprite == EnemyFetishOFF)
		{
			btnMainEnemySprite.transform.localScale = _defaultSpriteDisplayScaling;
		}
		else
		{
			btnMainEnemySprite.transform.localScale = new Vector3(_currentEnemyArray[_currentEnemyIndex].customScaleSprite, _currentEnemyArray[_currentEnemyIndex].customScaleSprite, 1f);
		}
	}

	private Sprite GetEnemySpriteBasedOnFetishToggle()
	{
		RefreshOverrideSideUI();
		if (_localFOT == LocalFOTState.ForceAllow)
		{
			return _currentEnemyArray[_currentEnemyIndex].displaySprite;
		}
		if (_localFOT == LocalFOTState.ForceBlock)
		{
			return EnemyFetishOFF;
		}
		if (_currentEnemyArray[_currentEnemyIndex].enemyCombo != null)
		{
			if (GC.VerifyComboImageDisplayOnGalleryScreen(_currentPrefabToSpawn.GetComponent<EnemyGeneralLogic>().currentEnemyType))
			{
				return _currentEnemyArray[_currentEnemyIndex].displaySprite;
			}
			return EnemyFetishOFF;
		}
		if (GC.doGalleryHideBlockedScenes && !ValidateFetishSoloSceneHACKCOPYFROMENEMYSCRIPT(_currentEroVariantID, _currentPrefabToSpawn.GetComponent<EnemyGeneralLogic>().eroCore) && _currentEnemyArray[_currentEnemyIndex].enemyCombo == null)
		{
			return EnemyFetishOFF;
		}
		return _currentEnemyArray[_currentEnemyIndex].displaySprite;
	}

	private bool ValidateFetishSoloSceneHACKCOPYFROMENEMYSCRIPT(int galleryCheckID, ENM_EroBaseConfig eroCore)
	{
		List<int> list = new List<int>();
		bool result = false;
		SpecialGalleryIDReplacement(ref galleryCheckID);
		if (eroCore.eroSoloAlts != null && eroCore.eroSoloAlts.Length != 0)
		{
			for (int i = 0; i < eroCore.eroSoloAlts.Length; i++)
			{
				if (GC.CompareFetishToggle(eroCore.eroSoloAlts[i].SceneTag) && galleryCheckID == eroCore.eroSoloAlts[i].GenericID)
				{
					list.Add(eroCore.eroSoloAlts[i].GenericID);
				}
			}
			if (list.Count > 0)
			{
				result = true;
			}
		}
		if (eroCore.FetishTags != null && eroCore.FetishTags.Length != 0 && (list == null || list.Count <= 0))
		{
			result = true;
			for (int j = 0; j < eroCore.FetishTags.Length; j++)
			{
				if (!GC.CompareFetishToggle(eroCore.FetishTags[j]))
				{
					result = false;
					break;
				}
			}
		}
		return result;
	}

	private void SpecialGalleryIDReplacement(ref int galleryID)
	{
		if (_currentPrefabToSpawn.GetComponent<EnemyGeneralLogic>().currentEnemyType == GameCore.EnemyType.PrisonBrute)
		{
			if (galleryID == 9)
			{
				galleryID = 1;
			}
			else if (galleryID == 10)
			{
				galleryID = 2;
			}
		}
	}

	private int AcquireLastSelectedEnemyIndex()
	{
		if (GC.galleryScreenLastEnemyIndex != 0 && GC.galleryScreenLastEnemyIndex <= _currentEnemyArray.Length - 1)
		{
			return GC.galleryScreenLastEnemyIndex;
		}
		GC.galleryScreenLastEnemyIndex = 0;
		return 0;
	}

	private void StartArenaEnemySpawnLogic()
	{
		if (ValidateArenaSpawn())
		{
			GameObject gameObject = Object.Instantiate(_currentPrefabToSpawn, GetArenaSpawnTypePosition(), Quaternion.identity, GC.refOBJEnemiesParent.transform);
			if (gameObject != null)
			{
				gameObject.GetComponent<EnemyGeneralLogic>().isArenaGallerySpecial = true;
				gameObject.GetComponent<EnemyGeneralLogic>().arenaPreferedEroID = _currentEroVariantID;
			}
			GC.PlaySFX(GameCore.SFXBasic1.SpawnerNoise1, 0, checkForDuplicity: true, Vector2.zero, 0.85f, 1.15f);
		}
	}

	private Vector2 GetArenaSpawnTypePosition()
	{
		Vector2 zero = Vector2.zero;
		int num = 0;
		int num2 = 10;
		do
		{
			num++;
			switch (_currentEnemyArray[_currentEnemyIndex].arenaSpawnType)
			{
			default:
				zero = _arenaAreaSpawn;
				zero += _currentEnemyArray[_currentEnemyIndex].OffsetTPPos;
				zero.x += GC.MATH_RNG_FLOAT(-13f, 13f);
				zero.y += GC.MATH_RNG_FLOAT(-2f, 2f);
				break;
			case 1:
				zero = _arenaGroundSpawn;
				zero += _currentEnemyArray[_currentEnemyIndex].OffsetTPPos;
				zero.x += GC.MATH_RNG_FLOAT(-13f, 13f);
				break;
			case 2:
				zero = _arenaDigSpawn;
				zero += _currentEnemyArray[_currentEnemyIndex].OffsetTPPos;
				zero.x += GC.MATH_RNG_FLOAT(-10f, 10f);
				zero.y += GC.MATH_RNG_FLOAT(-1f, 1.5f);
				break;
			}
		}
		while (!IsSpawnSafely(zero) || num < num2);
		return zero;
	}

	private bool IsSpawnSafely(Vector2 tentativePos)
	{
		if (GC.CheckPlayerProximityToAnyObject(tentativePos, GameCore.PlayerProximityType.Custom, 1.5f, 2f))
		{
			return false;
		}
		return true;
	}

	private bool ValidateArenaSpawn()
	{
		if (_currentEnemyArray[_currentEnemyIndex].logicType == GameCore.EnemyType.WormDig)
		{
			_currentPrefabToSpawn = hackWormEnemySpawn;
			return true;
		}
		if (_currentEnemyArray[_currentEnemyIndex].logicType.ToString().Contains("Boss"))
		{
			return false;
		}
		return true;
	}

	private void ToggleOverrideFetishAnim()
	{
		AdvanceLocalFOTStateOnButtonPress();
		switch (_localFOT)
		{
		default:
			GC.FetishOverrideTryAlter(_currentPrefabToSpawn.GetComponent<EnemyGeneralLogic>().currentEnemyType, _currentEroVariantID, overrideForcedStatus: false, isJustEntryRemoval: true);
			break;
		case LocalFOTState.ForceAllow:
			GC.FetishOverrideTryAlter(_currentPrefabToSpawn.GetComponent<EnemyGeneralLogic>().currentEnemyType, _currentEroVariantID, overrideForcedStatus: true);
			break;
		case LocalFOTState.ForceBlock:
			GC.FetishOverrideTryAlter(_currentPrefabToSpawn.GetComponent<EnemyGeneralLogic>().currentEnemyType, _currentEroVariantID, overrideForcedStatus: false);
			break;
		}
		imgMainEnemySprite.sprite = GetEnemySpriteBasedOnFetishToggle();
		GC.SaveGameConfiguration();
	}

	private LocalFOTState UpdateFOTStateOfCurrentAnim()
	{
		CFG_OverrideFetishToggle cFG_OverrideFetishToggle = GC.FetishOverrideCheckExistance(_currentPrefabToSpawn.GetComponent<EnemyGeneralLogic>().currentEnemyType, _currentEroVariantID);
		if (cFG_OverrideFetishToggle != null)
		{
			if (cFG_OverrideFetishToggle.switcherOverride)
			{
				_localFOT = LocalFOTState.ForceAllow;
			}
			else
			{
				_localFOT = LocalFOTState.ForceBlock;
			}
		}
		else
		{
			_localFOT = LocalFOTState.Neutral;
		}
		return _localFOT;
	}

	private void RefreshOverrideSideUI()
	{
		txtOverrideDisclaimer.text = GC.GetTranslatedStringUI(1006);
		switch (_localFOT)
		{
		default:
			imgOverrideState.sprite = sprOverrideNeutral;
			txtOverrideState.text = GC.GetTranslatedStringUI(158);
			break;
		case LocalFOTState.ForceAllow:
			imgOverrideState.sprite = sprOverrideAllow;
			txtOverrideState.text = GC.GetTranslatedStringUI(159);
			break;
		case LocalFOTState.ForceBlock:
			imgOverrideState.sprite = sprOverrideBlock;
			txtOverrideState.text = GC.GetTranslatedStringUI(160);
			break;
		}
	}

	private void AdvanceLocalFOTStateOnButtonPress()
	{
		switch (_localFOT)
		{
		default:
			_localFOT = LocalFOTState.ForceAllow;
			break;
		case LocalFOTState.ForceAllow:
			_localFOT = LocalFOTState.ForceBlock;
			break;
		case LocalFOTState.ForceBlock:
			_localFOT = LocalFOTState.Neutral;
			break;
		}
	}

	private string TranslateCurrentThemeName()
	{
		return GC.TranslateBiomeName(_selectedTheme);
	}

	private string TranslateCurrentEnemyName(int idInLanguageFile)
	{
		return GC.GetTranslatedStringUI(idInLanguageFile);
	}

	private void GetBadEndInfo()
	{
		bool active = false;
		ENM_BadEndScene eNM_BadEndScene = _currentPrefabToSpawn?.GetComponent<EroAnimation>()?.BadEndConfiguration;
		if (eNM_BadEndScene != null && eNM_BadEndScene.hasBadEnd && eNM_BadEndScene.eroVariantID == _currentEroVariantID)
		{
			active = true;
		}
		if (_currentEnemyArray[_currentEnemyIndex].logicType == GameCore.EnemyType.BossMinotaur)
		{
			active = true;
		}
		if (_currentEnemyArray[_currentEnemyIndex].logicType == GameCore.EnemyType.BossWerewolf && _currentEroVariantID == 2)
		{
			active = true;
		}
		containerHasBadEnd.SetActive(active);
	}

	private void SetSelectionColor(LocalSelection type, bool isDefault)
	{
		if (isDefault)
		{
			if (type == LocalSelection.Theme && imgThemeArrowRight.color != colorDefaultArrows)
			{
				imgThemeArrowRight.color = colorDefaultArrows;
				imgThemeArrowLeft.color = colorDefaultArrows;
			}
			if (type == LocalSelection.Enemy && imgEnemyPanel.color != colorDefaultEnemyPanel)
			{
				imgEnemyPanel.color = colorDefaultEnemyPanel;
				imgEnemyArrowRight.color = colorDefaultArrows;
				imgEnemyArrowLeft.color = colorDefaultArrows;
			}
		}
		else
		{
			if (type == LocalSelection.Theme && imgThemeArrowRight.color != colorSelectedAny)
			{
				imgThemeArrowRight.color = colorSelectedAny;
				imgThemeArrowLeft.color = colorSelectedAny;
			}
			if (type == LocalSelection.Enemy && imgEnemyPanel.color != colorSelectedAny)
			{
				imgEnemyPanel.color = colorSelectedAny;
				imgEnemyArrowRight.color = colorSelectedAny;
				imgEnemyArrowLeft.color = colorSelectedAny;
			}
		}
		AnimateArrowButtons(type, isDefault);
	}

	private void AnimateArrowButtons(LocalSelection type, bool stopAnim)
	{
		Vector3 vector = default;
		vector = ((type != LocalSelection.Enemy) ? new Vector3(1f, 1f, 1f) : new Vector3(2f, 2f, 1f));
		if (stopAnim)
		{
			if (type == LocalSelection.Enemy && imgEnemyArrowRight.transform.localScale != vector)
			{
				imgEnemyArrowRight.transform.localScale = vector;
				imgEnemyArrowLeft.transform.localScale = vector;
			}
			if (type == LocalSelection.Theme && imgThemeArrowRight.transform.localScale != vector)
			{
				imgThemeArrowRight.transform.localScale = vector;
				imgThemeArrowLeft.transform.localScale = vector;
			}
			return;
		}
		if (type == LocalSelection.Enemy)
		{
			vector = imgEnemyArrowRight.transform.localScale;
			if (!switchArrowAnim)
			{
				vector.x = Mathf.MoveTowards(vector.x, 2.5f, speedArrowAnim * Time.unscaledDeltaTime);
				vector.y = Mathf.MoveTowards(vector.y, 2.5f, speedArrowAnim * Time.unscaledDeltaTime);
				if (vector.x >= 2.5f)
				{
					switchArrowAnim = true;
				}
			}
			else
			{
				vector.x = Mathf.MoveTowards(vector.x, 1.5f, speedArrowAnim * Time.unscaledDeltaTime);
				vector.y = Mathf.MoveTowards(vector.y, 1.5f, speedArrowAnim * Time.unscaledDeltaTime);
				if (vector.x <= 1.5f)
				{
					switchArrowAnim = false;
				}
			}
			imgEnemyArrowRight.transform.localScale = vector;
			imgEnemyArrowLeft.transform.localScale = vector;
		}
		if (type != LocalSelection.Theme)
		{
			return;
		}
		vector = imgThemeArrowRight.transform.localScale;
		if (!switchArrowAnim)
		{
			vector.x = Mathf.MoveTowards(vector.x, 1.5f, speedArrowAnim * Time.unscaledDeltaTime);
			vector.y = Mathf.MoveTowards(vector.y, 1.5f, speedArrowAnim * Time.unscaledDeltaTime);
			if (vector.x >= 1.5f)
			{
				switchArrowAnim = true;
			}
		}
		else
		{
			vector.x = Mathf.MoveTowards(vector.x, 0.5f, speedArrowAnim * Time.unscaledDeltaTime);
			vector.y = Mathf.MoveTowards(vector.y, 0.5f, speedArrowAnim * Time.unscaledDeltaTime);
			if (vector.x <= 0.5f)
			{
				switchArrowAnim = false;
			}
		}
		imgThemeArrowRight.transform.localScale = vector;
		imgThemeArrowLeft.transform.localScale = vector;
	}

	private void SortEnemyArray()
	{
		List<int> list = new List<int>();
		List<int> list2 = new List<int>();
		clsGalleryEnemyList[] array = new clsGalleryEnemyList[_currentEnemyArray.Length];
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = _currentEnemyArray[i];
			list.Add(array[i].idNameJSON);
		}
		list.Sort();
		foreach (int item in list)
		{
			int j;
			for (j = 0; j < array.Length; j++)
			{
				if (item == array[j].idNameJSON && !list2.Exists((int x) => x == j))
				{
					_currentEnemyArray[num] = array[j];
					list2.Add(j);
					break;
				}
			}
			num++;
		}
	}

	private bool Check_GA_LogicPosition()
	{
		if ((bool)GC && GC.playerCurrentWorldXPosition > GC.galleryPlayerX_ArenaLimit)
		{
			return true;
		}
		return false;
	}

	private Color GetThemeBackgroundColor()
	{
		switch (_selectedTheme)
		{
		default:
			return colorThemeBeach;
		case GameCore.GalleryThemeType.Forest:
			return colorThemeForest;
		case GameCore.GalleryThemeType.Hill:
			return colorThemeHill;
		case GameCore.GalleryThemeType.City:
			return colorThemeCity;
		case GameCore.GalleryThemeType.Cave:
			return colorThemeCave;
		case GameCore.GalleryThemeType.Castle:
			return colorThemeCastle;
		case GameCore.GalleryThemeType.Swamp:
			return colorThemeSwamp;
		case GameCore.GalleryThemeType.HighTech:
			return colorThemeHighTech;
		}
	}

	private Sprite GetThemeBackgroundSprite()
	{
		switch (_selectedTheme)
		{
		default:
			return spriteBeach;
		case GameCore.GalleryThemeType.Forest:
			return spriteForest;
		case GameCore.GalleryThemeType.Hill:
			return spriteHill;
		case GameCore.GalleryThemeType.City:
			return spriteCity;
		case GameCore.GalleryThemeType.Cave:
			return spriteCave;
		case GameCore.GalleryThemeType.Castle:
			return spriteCastle;
		case GameCore.GalleryThemeType.Swamp:
			return spriteSwamp;
		case GameCore.GalleryThemeType.HighTech:
			return spriteHighTech;
		}
	}
}
