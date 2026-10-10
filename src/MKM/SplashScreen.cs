using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
	public Text txtMainWarnings;

	public Text txtPushAnyKey;

	public Text txtAlphaDisclaimer;

	public Image imgFateToBlack;

	public Image imgPainel1;

	public Image imgPainel2;

	public Image imgPainel3;

	public Image imgName;

	public SVGImage svgLogo;

	public Image imgLoading;

	public TextAsset jsonFileUI;

	public TextAsset jsonFileItemList;

	public TextAsset jsonFileIntroStory;

	public TextAsset jsonFileDialogueBoard;

	public TextAsset jsonFileDialogueHaruna;

	public TextAsset jsonFileDialogueFarina;

	public TextAsset jsonFileDialogueVarious;

	private int idScreen;

	private float freqInvokeFade;

	private float rateFadeInOut;

	private float timeDelayKeyPress;

	private float timeFading;

	private bool canPressKeys;

	private Color colorBlackScreen;

	private Coroutine crtDelayKeyPressOnStart;

	private GameCore GC;

	private void Start()
	{
		InitializeVariables();
		RunKeyPressDelay(timeDelayKeyPress);
		InitialScreenSetup();
	}

	private void InitializeVariables()
	{
		GC = GameCore.Instance;
		idScreen = 0;
		timeFading = 0.5f;
		timeDelayKeyPress = timeFading;
		canPressKeys = false;
		freqInvokeFade = 0.05f;
		colorBlackScreen = Color.black;
	}

	private void InitialScreenSetup()
	{
		GC.MainGameDataInitialize(RenewAll: true);
		GC.LoadGameConfiguration();
		if (GC.GameBuildCurrent == GameCore.GameBuildType.WebGL || GC.GameBuildCurrent == GameCore.GameBuildType.Mac)
		{
			GC.WEBGL_FeedLocalVariablesToString(jsonFileUI, jsonFileItemList, jsonFileIntroStory, jsonFileDialogueBoard, jsonFileDialogueHaruna, jsonFileDialogueVarious, jsonFileDialogueFarina);
		}
		GC.GetRootLanguageList();
		GC.ConcatenateSelectedLanguageFolderPath();
		if (GC.LoadUIFile())
		{
			ApplyLanguageOfWarnings();
		}
	}

	private void Update()
	{
		if (Input.anyKeyDown && canPressKeys)
		{
			canPressKeys = false;
			RunKeyPressDelay(timeDelayKeyPress * 2f);
			switch (idScreen)
			{
			default:
				idScreen = 1;
				break;
			case 1:
				idScreen = 2;
				break;
			case 2:
				idScreen = 3;
				StopAllCoroutines();
				canPressKeys = false;
				break;
			}
			BeginFadeLogic(doFadeIn: true, timeFading);
		}
	}

	private void BeginFadeLogic(bool doFadeIn, float duration)
	{
		CancelInvoke();
		if (doFadeIn)
		{
			colorBlackScreen.a = 0f;
			imgFateToBlack.color = colorBlackScreen;
			imgFateToBlack.gameObject.SetActive(value: true);
			rateFadeInOut = 1f / (duration / freqInvokeFade);
			InvokeRepeating("InvokedFadeIn", 0f, freqInvokeFade);
		}
		else
		{
			colorBlackScreen.a = 1f;
			imgFateToBlack.color = colorBlackScreen;
			imgFateToBlack.gameObject.SetActive(value: true);
			rateFadeInOut = 1f / (duration / freqInvokeFade);
			InvokeRepeating("InvokedFadeOut", 0f, freqInvokeFade);
		}
	}

	private void InvokedFadeIn()
	{
		colorBlackScreen.a += rateFadeInOut;
		imgFateToBlack.color = colorBlackScreen;
		if (imgFateToBlack.color.a >= 1f)
		{
			CancelInvoke("InvokedFadeIn");
			switch (idScreen)
			{
			case 1:
				ShowHideElements(p1: false, p2: false, p3: true, svg1: false, img1: false, img2: false, txt1: false, txt2: false, txt3: true);
				break;
			case 2:
				ShowHideElements(p1: false, p2: false, p3: false, svg1: true, img1: true, img2: false, txt1: false, txt2: false, txt3: false);
				break;
			case 3:
				ShowHideElements(p1: false, p2: false, p3: false, svg1: false, img1: false, img2: true, txt1: false, txt2: false, txt3: false);
				LoadMainMenuScene();
				break;
			}
		}
	}

	private void InvokedFadeOut()
	{
		colorBlackScreen.a -= rateFadeInOut;
		imgFateToBlack.color = colorBlackScreen;
		if (imgFateToBlack.color.a <= 0f)
		{
			imgFateToBlack.gameObject.SetActive(value: false);
			CancelInvoke("InvokedFadeOut");
		}
	}

	private void ShowHideElements(bool p1, bool p2, bool p3, bool svg1, bool img1, bool img2, bool txt1, bool txt2, bool txt3)
	{
		imgPainel1.gameObject.SetActive(p1);
		imgPainel2.gameObject.SetActive(p2);
		imgPainel3.gameObject.SetActive(p3);
		svgLogo.gameObject.SetActive(svg1);
		imgName.gameObject.SetActive(img1);
		imgLoading.gameObject.SetActive(img2);
		txtMainWarnings.gameObject.SetActive(txt1);
		txtPushAnyKey.gameObject.SetActive(txt2);
		txtAlphaDisclaimer.gameObject.SetActive(txt3);
		BeginFadeLogic(doFadeIn: false, timeFading);
		if (svg1 && (bool)GC && !GC.disableVoiceLogo)
		{
			GC.CallReferenceMayaVoiceGC("MBBLogo", "SplashScreen:ShowHideElements()");
		}
	}

	private void ApplyLanguageOfWarnings()
	{
		if ((bool)txtMainWarnings)
		{
			txtMainWarnings.text = GC.clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 1000).text;
		}
		if ((bool)txtPushAnyKey)
		{
			txtPushAnyKey.text = GC.clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 1001).text;
		}
		if ((bool)txtAlphaDisclaimer)
		{
			txtAlphaDisclaimer.text = GC.clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 1002).text;
		}
	}

	private void LoadMainMenuScene()
	{
		StartCoroutine(GC.LoadYourAsyncScene("MainMenu", lockPlayerInput: true));
	}

	private void RunKeyPressDelay(float time)
	{
		if (crtDelayKeyPressOnStart != null)
		{
			StopCoroutine(crtDelayKeyPressOnStart);
		}
		crtDelayKeyPressOnStart = StartCoroutine(waitForDelayKeyPress(time));
	}

	private IEnumerator waitForDelayKeyPress(float wait)
	{
		yield return new WaitForSeconds(wait);
		canPressKeys = true;
	}
}
