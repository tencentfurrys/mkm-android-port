using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TeleportScreenLogic : MonoBehaviour
{
	private enum LocalNav
	{
		Null = 0,
		Up = 1,
		Down = 2,
		Left = 3,
		Right = 4
	}

	private enum LocalScroll
	{
		Null = 0,
		Downward = 1,
		Upward = 2,
		Leftward = 3,
		Rightward = 4
	}

	public Button btnCenterMain;

	public Button btnReturn;

	public GameObject arrowUP;

	public GameObject arrowDOWN;

	public GameObject arrowLEFT;

	public GameObject arrowRIGHT;

	public Text txtSceneName;

	public Text txtPieceName;

	public SavePointDatabase dbSavePointSource;

	public GameObject objMagicCircle;

	public GameObject objMagicInner;

	public Image imgTransitionA;

	public Image imgTransitionB;

	public Vector2 posTargetHorVer;

	public float speedTransition;

	public float targetCenterDetection;

	public float angleCircleRotation;

	public float factorMultiplier;

	private List<SAVEP_MainDatabase> listCurrentSVP = new List<SAVEP_MainDatabase>();

	private List<SAVEP_MainDatabase> listVerticalShortSVP = new List<SAVEP_MainDatabase>();

	private Vector2 targetPositionA;

	private Vector2 targetPositionB;

	private Color colorOriginalButton;

	private GameCore GC;

	private bool isValidatedTwoClickTeleport;

	private LocalScroll currentTransition;

	private int previousSelectedIndex;

	private int currentSelectedIndex;

	private float currentMulti;

	private void Start()
	{
		MKMDiag.Step("TELEPORT.Start");
		GC = GameCore.Instance;
		currentTransition = LocalScroll.Null;
		colorOriginalButton = btnCenterMain.image.color;
		isValidatedTwoClickTeleport = false;
		currentMulti = 1f;
		InitialLogic();
	}

	private void InitialLogic()
	{
		if ((bool)GC)
		{
			previousSelectedIndex = -1;
			currentSelectedIndex = 0;
			PopulateListOfPlayerCurrentValidVisitedSavePoints();
			StartupCheckStageName();
			SetupUpdateUI(LocalNav.Null);
		}
	}

	private void StartupCheckStageName()
	{
		if (listCurrentSVP.Exists((SAVEP_MainDatabase x) => x.sceneName == GC.strCurrentActiveSceneName))
		{
			currentSelectedIndex = listCurrentSVP.FindIndex((SAVEP_MainDatabase x) => x.sceneName == GC.strCurrentActiveSceneName);
			PopulateVerticalShortList(listCurrentSVP[currentSelectedIndex].sceneGroup);
		}
	}

	private void OnEnable()
	{
		InitialLogic();
	}

	private void Update()
	{
		if (currentTransition == LocalScroll.Null && EventSystem.current.currentSelectedGameObject == btnCenterMain.gameObject)
		{
			if (Input.GetKey(GC.IKey(GameCore.InputLib.Up)) || GC.UnifiedInputHandler(GameCore.InputLib.Up, GameCore.InputPressType.KeyHolding))
			{
				HandleListNavigationBasedOnInput(LocalNav.Up);
			}
			if (Input.GetKey(GC.IKey(GameCore.InputLib.Down)) || GC.UnifiedInputHandler(GameCore.InputLib.Down, GameCore.InputPressType.KeyHolding))
			{
				HandleListNavigationBasedOnInput(LocalNav.Down);
			}
			if (Input.GetKey(GC.IKey(GameCore.InputLib.Left)) || GC.UnifiedInputHandler(GameCore.InputLib.Left, GameCore.InputPressType.KeyHolding))
			{
				HandleListNavigationBasedOnInput(LocalNav.Left);
			}
			if (Input.GetKey(GC.IKey(GameCore.InputLib.Right)) || GC.UnifiedInputHandler(GameCore.InputLib.Right, GameCore.InputPressType.KeyHolding))
			{
				HandleListNavigationBasedOnInput(LocalNav.Right);
			}
		}
		if (currentTransition != LocalScroll.Null)
		{
			UPDATE_CleanUI();
			imgTransitionA.transform.localPosition = new Vector2(Mathf.Lerp(imgTransitionA.transform.localPosition.x, targetPositionA.x, speedTransition * Time.fixedDeltaTime), Mathf.Lerp(imgTransitionA.transform.localPosition.y, targetPositionA.y, speedTransition * Time.fixedDeltaTime));
			imgTransitionB.transform.localPosition = new Vector2(Mathf.Lerp(imgTransitionB.transform.localPosition.x, targetPositionB.x, speedTransition * Time.fixedDeltaTime), Mathf.Lerp(imgTransitionB.transform.localPosition.y, targetPositionB.y, speedTransition * Time.fixedDeltaTime));
			if (UPDATE_CheckImageReachedCenterTarget())
			{
				EndImageTransitionProcess();
			}
		}
		UPDATE_MagicCircleBehavior();
		UPDATE_ToggleArrowButtons();
	}

	private void UPDATE_CleanUI()
	{
		if (txtSceneName.text != "")
		{
			txtSceneName.text = "";
			txtPieceName.text = "";
		}
	}

	private bool UPDATE_CheckImageReachedCenterTarget()
	{
		if (imgTransitionA.transform.localPosition.x > 0f - targetCenterDetection && imgTransitionA.transform.localPosition.x < targetCenterDetection && imgTransitionA.transform.localPosition.y > 0f - targetCenterDetection && imgTransitionA.transform.localPosition.y < targetCenterDetection)
		{
			return true;
		}
		return false;
	}

	private void UPDATE_MagicCircleBehavior()
	{
		if (EventSystem.current.currentSelectedGameObject == btnCenterMain.gameObject)
		{
			if (!objMagicCircle.activeInHierarchy)
			{
				objMagicCircle.SetActive(value: true);
			}
			if (isValidatedTwoClickTeleport)
			{
				currentMulti = factorMultiplier;
				if (!objMagicInner.activeInHierarchy)
				{
					objMagicInner.SetActive(value: true);
				}
			}
			else
			{
				currentMulti = 1f;
				if (objMagicInner.activeInHierarchy)
				{
					objMagicInner.SetActive(value: false);
				}
			}
			objMagicCircle.transform.Rotate(0f, 0f, (0f - angleCircleRotation * currentMulti) * Time.fixedDeltaTime, Space.Self);
			objMagicInner.transform.Rotate(0f, 0f, angleCircleRotation * currentMulti * Time.fixedDeltaTime, Space.Self);
		}
		else
		{
			if (objMagicCircle.activeInHierarchy)
			{
				objMagicCircle.SetActive(value: false);
			}
			if (objMagicInner.activeInHierarchy)
			{
				objMagicInner.SetActive(value: false);
			}
		}
	}

	private void UPDATE_ToggleArrowButtons()
	{
		if (currentSelectedIndex == 0)
		{
			if (arrowLEFT.activeInHierarchy)
			{
				arrowLEFT.SetActive(value: false);
			}
		}
		else if (!arrowLEFT.activeInHierarchy)
		{
			arrowLEFT.SetActive(value: true);
		}
		if (!listCurrentSVP.Exists((SAVEP_MainDatabase x) => x.sceneGroup > listCurrentSVP[currentSelectedIndex].sceneGroup))
		{
			if (arrowRIGHT.activeInHierarchy)
			{
				arrowRIGHT.SetActive(value: false);
			}
		}
		else if (!arrowRIGHT.activeInHierarchy)
		{
			arrowRIGHT.SetActive(value: true);
		}
		if (listVerticalShortSVP.Count <= 1)
		{
			if (arrowUP.activeInHierarchy)
			{
				arrowUP.SetActive(value: false);
			}
			if (arrowDOWN.activeInHierarchy)
			{
				arrowDOWN.SetActive(value: false);
			}
		}
		else
		{
			if (!arrowUP.activeInHierarchy)
			{
				arrowUP.SetActive(value: true);
			}
			if (!arrowDOWN.activeInHierarchy)
			{
				arrowDOWN.SetActive(value: true);
			}
		}
	}

	private void BeginImageTransitionProcess(LocalNav keyInput)
	{
		Vector2 vector = new Vector2(0f, 0f);
		Vector2 vector2 = new Vector2(0f, 0f);
		Color color = colorOriginalButton;
		color.a = 0f;
		targetPositionA = vector;
		targetPositionB = vector2;
		imgTransitionA.sprite = dbSavePointSource.ReturnScreenshotByID(listCurrentSVP[currentSelectedIndex].idSavePoint);
		imgTransitionB.sprite = dbSavePointSource.ReturnScreenshotByID(listCurrentSVP[previousSelectedIndex].idSavePoint);
		btnCenterMain.image.color = color;
		switch (keyInput)
		{
		case LocalNav.Up:
			vector.y = posTargetHorVer.y;
			targetPositionB.y = 0f - posTargetHorVer.y;
			currentTransition = LocalScroll.Downward;
			break;
		case LocalNav.Down:
			vector.y = 0f - posTargetHorVer.y;
			targetPositionB.y = posTargetHorVer.y;
			currentTransition = LocalScroll.Upward;
			break;
		case LocalNav.Left:
			vector.x = 0f - posTargetHorVer.x;
			targetPositionB.x = posTargetHorVer.x;
			currentTransition = LocalScroll.Rightward;
			break;
		case LocalNav.Right:
			vector.x = posTargetHorVer.x;
			targetPositionB.x = 0f - posTargetHorVer.x;
			currentTransition = LocalScroll.Leftward;
			break;
		}
		imgTransitionA.transform.localPosition = vector;
		imgTransitionB.transform.localPosition = vector2;
	}

	private void EndImageTransitionProcess()
	{
		currentTransition = LocalScroll.Null;
		btnCenterMain.image.color = colorOriginalButton;
		imgTransitionA.transform.localPosition = posTargetHorVer;
		imgTransitionB.transform.localPosition = posTargetHorVer;
		imgTransitionA.sprite = null;
		imgTransitionB.sprite = null;
		FillUpdateUIElementsInformation(currentSelectedIndex);
	}

	private void SetupUpdateUI(LocalNav thisInput)
	{
		if (currentSelectedIndex != previousSelectedIndex)
		{
			if (thisInput != LocalNav.Null)
			{
				BeginImageTransitionProcess(thisInput);
			}
			else
			{
				FillUpdateUIElementsInformation(currentSelectedIndex);
			}
		}
	}

	private void FillUpdateUIElementsInformation(int thisIndex)
	{
		btnCenterMain.image.sprite = dbSavePointSource.ReturnScreenshotByID(listCurrentSVP[thisIndex].idSavePoint);
		txtSceneName.text = GC.TranslateSceneNameToStageName(isCurrentlyActive: false, listCurrentSVP[thisIndex].sceneName);
		txtPieceName.text = HACK_AddBiomeSuffix(listCurrentSVP[thisIndex].sceneGroup) + " - " + GC.getTranslatedStringChessPiece(listCurrentSVP[thisIndex].typeOfPiece);
	}

	private void HandleListNavigationBasedOnInput(LocalNav basicInput)
	{
		isValidatedTwoClickTeleport = false;
		previousSelectedIndex = currentSelectedIndex;
		switch (basicInput)
		{
		case LocalNav.Up:
		case LocalNav.Down:
			currentSelectedIndex = ReturnNextIndexOfDifferentVerticalSaveID(basicInput);
			break;
		case LocalNav.Left:
		case LocalNav.Right:
			currentSelectedIndex = ReturnNextIndexOfDifferentHorizontalGroup(basicInput);
			PopulateVerticalShortList(listCurrentSVP[currentSelectedIndex].sceneGroup);
			if (listVerticalShortSVP.Count > 1 && CheckIfStageGroupChanged())
			{
				currentSelectedIndex = listCurrentSVP.FindIndex((SAVEP_MainDatabase x) => x.idSavePoint == listVerticalShortSVP[0].idSavePoint);
			}
			break;
		}
		SetupUpdateUI(basicInput);
	}

	private int ReturnNextIndexOfDifferentHorizontalGroup(LocalNav horizontalInput)
	{
		int sceneGroup = listCurrentSVP[currentSelectedIndex].sceneGroup;
		switch (horizontalInput)
		{
		case LocalNav.Left:
		{
			int i2;
			for (i2 = currentSelectedIndex; i2 >= 0; i2--)
			{
				if (i2 < currentSelectedIndex && listCurrentSVP[i2].sceneGroup < sceneGroup)
				{
					return listCurrentSVP.FindIndex((SAVEP_MainDatabase x) => x.sceneGroup == listCurrentSVP[i2].sceneGroup);
				}
			}
			break;
		}
		case LocalNav.Right:
		{
			int i;
			for (i = currentSelectedIndex; i < listCurrentSVP.Count; i++)
			{
				if (i > currentSelectedIndex && listCurrentSVP[i].sceneGroup > sceneGroup)
				{
					return listCurrentSVP.FindIndex((SAVEP_MainDatabase x) => x.sceneGroup == listCurrentSVP[i].sceneGroup);
				}
			}
			return currentSelectedIndex;
		}
		}
		return 0;
	}

	private int ReturnNextIndexOfDifferentVerticalSaveID(LocalNav verticalInput)
	{
		int tempSaveID = listCurrentSVP[currentSelectedIndex].idSavePoint;
		int num = 0;
		if (listVerticalShortSVP.Count > 0)
		{
			num = listVerticalShortSVP.FindIndex((SAVEP_MainDatabase x) => x.idSavePoint == tempSaveID);
		}
		switch (verticalInput)
		{
		case LocalNav.Up:
		{
			int i2;
			for (i2 = num; i2 < listVerticalShortSVP.Count; i2++)
			{
				if (listVerticalShortSVP.Count > 0 && listVerticalShortSVP[i2].idSavePoint > tempSaveID)
				{
					return listCurrentSVP.FindIndex((SAVEP_MainDatabase x) => x.idSavePoint == listVerticalShortSVP[i2].idSavePoint);
				}
			}
			if (num == listVerticalShortSVP.Count - 1 && listVerticalShortSVP.Count > 1)
			{
				return listCurrentSVP.FindIndex((SAVEP_MainDatabase x) => x.idSavePoint == listVerticalShortSVP[0].idSavePoint);
			}
			break;
		}
		case LocalNav.Down:
		{
			int i;
			for (i = num; i >= 0; i--)
			{
				if (listVerticalShortSVP.Count > 0 && listVerticalShortSVP[i].idSavePoint < tempSaveID)
				{
					return listCurrentSVP.FindIndex((SAVEP_MainDatabase x) => x.idSavePoint == listVerticalShortSVP[i].idSavePoint);
				}
			}
			if (num == 0 && listVerticalShortSVP.Count > 1)
			{
				return listCurrentSVP.FindIndex((SAVEP_MainDatabase x) => x.idSavePoint == listVerticalShortSVP[listVerticalShortSVP.Count - 1].idSavePoint);
			}
			break;
		}
		}
		return currentSelectedIndex;
	}

	private bool CheckIfStageGroupChanged()
	{
		if (listCurrentSVP[currentSelectedIndex].sceneGroup != listCurrentSVP[previousSelectedIndex].sceneGroup)
		{
			return true;
		}
		return false;
	}

	private string HACK_AddBiomeSuffix(int groupID)
	{
		GameCore.GalleryThemeType biomeType;
		switch (groupID)
		{
		default:
			biomeType = GameCore.GalleryThemeType.Castle;
			break;
		case 1:
			biomeType = GameCore.GalleryThemeType.Forest;
			break;
		case 2:
			biomeType = GameCore.GalleryThemeType.Beach;
			break;
		case 3:
			biomeType = GameCore.GalleryThemeType.Hill;
			break;
		case 4:
			biomeType = GameCore.GalleryThemeType.City;
			break;
		case 5:
			biomeType = GameCore.GalleryThemeType.Cave;
			break;
		case 6:
			biomeType = GameCore.GalleryThemeType.Swamp;
			break;
		case 7:
			biomeType = GameCore.GalleryThemeType.HighTech;
			break;
		}
		return GC.TranslateBiomeName(biomeType);
	}

	private void PopulateListOfPlayerCurrentValidVisitedSavePoints()
	{
		listCurrentSVP.Clear();
		foreach (int idSVP in GC.playerListOfVisitedSavePoints)
		{
			SAVEP_MainDatabase sAVEP_MainDatabase = null;
			sAVEP_MainDatabase = dbSavePointSource.databaseOfSavePoint.Find((SAVEP_MainDatabase x) => x.idSavePoint == idSVP);
			if (sAVEP_MainDatabase != null)
			{
				listCurrentSVP.Add(sAVEP_MainDatabase);
			}
		}
		listCurrentSVP.Sort((SAVEP_MainDatabase x, SAVEP_MainDatabase y) => x.sceneGroup.CompareTo(y.sceneGroup));
	}

	private void PopulateVerticalShortList(int stageGroup)
	{
		listVerticalShortSVP.Clear();
		listVerticalShortSVP = listCurrentSVP.FindAll((SAVEP_MainDatabase x) => x.sceneGroup == stageGroup);
		listVerticalShortSVP.Sort((SAVEP_MainDatabase x, SAVEP_MainDatabase y) => x.idSavePoint.CompareTo(y.idSavePoint));
	}

	public void ButtonClickedPerformTeleport()
	{
		if (isValidatedTwoClickTeleport)
		{
			isValidatedTwoClickTeleport = false;
			GC.DoPlayerTeleport(listCurrentSVP[currentSelectedIndex].sceneName, listCurrentSVP[currentSelectedIndex].positionOfPiece);
		}
		else
		{
			isValidatedTwoClickTeleport = true;
		}
	}

	public void ButtonArrowClicked(string direction)
	{
		btnCenterMain.Select();
		if (currentTransition == LocalScroll.Null)
		{
			switch (direction)
			{
			case "UP":
				HandleListNavigationBasedOnInput(LocalNav.Up);
				break;
			case "DOWN":
				HandleListNavigationBasedOnInput(LocalNav.Down);
				break;
			case "LEFT":
				HandleListNavigationBasedOnInput(LocalNav.Left);
				break;
			case "RIGHT":
				HandleListNavigationBasedOnInput(LocalNav.Right);
				break;
			}
		}
	}
}
