using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class IntroStory : MonoBehaviour
{
	private enum FadeType
	{
		SingleFadeIn = 0,
		SingleFadeOut = 1,
		FadeOutThenIn = 2
	}

	public Text txtStoryLineDisplay;

	public Image imgParchment;

	public Image imgTextPanelBG;

	public Image imgStory01;

	public Image imgStory02;

	public Image imgStory03;

	public Image imgStory04;

	public Image imgStory05;

	public Image imgStory06;

	public Image imgStory07;

	public Text txtLabelNext;

	public Text txtLabelSkip;

	public Text txtInputNext;

	public Text txtInputSkip;

	private int indexListStory;

	private int intImgCurrent;

	private float timeInputDelay;

	private float freqRepeatFade;

	private bool isInputAllowed;

	private bool isApplyingFade;

	private IntroStoryList listOfStory;

	private Image imgToFadeIn;

	private Image imgToFadeOut;

	private Color colorCurrent;

	private Coroutine coroutInputLock;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		if (GC.isSceneTransitioning)
		{
			GC.SceneFadeHandler(actionFadeToBlack: false, 777f);
		}
		InitializeValues();
		LoadIntroFileAndText();
		TranslateAndApplyControlsText();
		if ((bool)GC)
		{
			GC.PlayMusicTheme(GameCore.MusicTheme.IntroStory, doFadeIn: true, 1f, doLoop: true);
		}
	}

	private void InitializeValues()
	{
		isInputAllowed = false;
		isApplyingFade = false;
		timeInputDelay = 0.5f;
		freqRepeatFade = 0.05f;
		indexListStory = 0;
		intImgCurrent = 0;
		if (coroutInputLock != null)
		{
			StopCoroutine(coroutInputLock);
		}
		coroutInputLock = StartCoroutine(waitForInputLockDelay(timeInputDelay));
	}

	private void LoadIntroFileAndText()
	{
		listOfStory = GC.LoadIntroStoryFile();
		DisplayNextSentence(isFirstLine: true);
	}

	private void Update()
	{
		if (!isInputAllowed || isApplyingFade)
		{
			return;
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.ConfirmUI, GameCore.InputPressType.KeyPressedOnce) || Input.GetKeyDown(GC.IKey(GameCore.InputLib.Interact)) || Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || MKM_Tap())
		{
			if (txtStoryLineDisplay.color.a >= 1f)
			{
				DisplayNextSentence(isFirstLine: false);
			}
		}
		else if (GC.UnifiedInputHandler(GameCore.InputLib.CancelUI, GameCore.InputPressType.KeyPressedOnce) || Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.LeftControl) || MKM_TwoFingerTap())
		{
			FinishIntro();
		}
	}

	// Android (P2a): no keyboard, so any tap/click advances the story and a
	// two-finger tap (or right-click) skips it. Touch counts as mouse input,
	// but check real touches too in case mouse simulation is ever disabled.
	private bool MKM_Tap()
	{
		if (Input.GetMouseButtonDown(0))
		{
			return true;
		}
		for (int i = 0; i < Input.touchCount; i++)
		{
			if (Input.GetTouch(i).phase == TouchPhase.Began)
			{
				return true;
			}
		}
		return false;
	}

	private bool MKM_TwoFingerTap()
	{
		if (Input.GetMouseButtonDown(1))
		{
			return true;
		}
		if (Input.touchCount >= 2)
		{
			for (int i = 0; i < Input.touchCount; i++)
			{
				if (Input.GetTouch(i).phase == TouchPhase.Began)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void DisplayNextSentence(bool isFirstLine)
	{
		if (!isFirstLine && indexListStory < listOfStory.listOfIntroStory.Count)
		{
			StartTextFadeEffect(FadeType.FadeOutThenIn, isSpecial: false);
			return;
		}
		ApplyDisplayTextAndImage();
		if (isFirstLine)
		{
			StartTextFadeEffect(FadeType.SingleFadeIn, isSpecial: false);
		}
	}

	private void ApplyDisplayTextAndImage()
	{
		if (indexListStory == listOfStory.listOfIntroStory.Count)
		{
			FinishIntro();
			return;
		}
		CheckImageChange(listOfStory.listOfIntroStory[indexListStory].imageID);
		txtStoryLineDisplay.text = ParseTextLineForBreaks(listOfStory.listOfIntroStory[indexListStory].text);
		imgTextPanelBG.rectTransform.anchoredPosition = listOfStory.listOfIntroStory[indexListStory].posTextUI;
		indexListStory++;
	}

	private string ParseTextLineForBreaks(string line)
	{
		return line.Replace("<@:LINE_BREAK>", "\r\n");
	}

	private void CheckImageChange(int intImgToShow)
	{
		if (intImgToShow != intImgCurrent)
		{
			switch (intImgCurrent)
			{
			case 1:
				imgToFadeOut = imgStory01;
				break;
			case 2:
				imgToFadeOut = imgStory02;
				break;
			case 3:
				imgToFadeOut = imgStory03;
				break;
			case 4:
				imgToFadeOut = imgStory04;
				break;
			case 5:
				imgToFadeOut = imgStory05;
				break;
			case 6:
				imgToFadeOut = imgStory06;
				break;
			case 7:
				imgToFadeOut = imgStory07;
				break;
			}
			switch (intImgToShow)
			{
			case 1:
				imgToFadeIn = imgStory01;
				StartImageFadeEffect(FadeType.SingleFadeIn);
				break;
			case 2:
				imgToFadeIn = imgStory02;
				StartImageFadeEffect(FadeType.FadeOutThenIn);
				break;
			case 3:
				imgToFadeIn = imgStory03;
				StartImageFadeEffect(FadeType.FadeOutThenIn);
				break;
			case 4:
				imgToFadeIn = imgStory04;
				StartImageFadeEffect(FadeType.FadeOutThenIn);
				break;
			case 5:
				imgToFadeIn = imgStory05;
				StartImageFadeEffect(FadeType.FadeOutThenIn);
				break;
			case 6:
				imgToFadeIn = imgStory06;
				StartImageFadeEffect(FadeType.FadeOutThenIn);
				break;
			case 7:
				imgToFadeIn = imgStory07;
				StartImageFadeEffect(FadeType.FadeOutThenIn);
				break;
			}
		}
		intImgCurrent = intImgToShow;
	}

	private void FinishIntro()
	{
		isInputAllowed = false;
		GC.LoadNextScene("Stage2-Beach", Vector2.zero, Vector2.zero, forceMovePlayer: false, applyFade: true, Color.black, GC.globalDefaultSceneFadeTimer, lockPlayerInput: true);
	}

	private void StartImageFadeEffect(FadeType action)
	{
		isApplyingFade = true;
		switch (action)
		{
		default:
			InvokeRepeating("ExecuteImageFadeIn", 0f, freqRepeatFade);
			break;
		case FadeType.SingleFadeOut:
			InvokeRepeating("ExecuteImageFadeOut", 0f, freqRepeatFade);
			break;
		case FadeType.FadeOutThenIn:
			InvokeRepeating("ExecuteImageFadeOutThenIn", 0f, freqRepeatFade);
			break;
		}
	}

	private void ExecuteImageFadeIn()
	{
		colorCurrent = imgToFadeIn.color;
		if (colorCurrent.a < 1f)
		{
			colorCurrent.a += freqRepeatFade;
			imgToFadeIn.color = colorCurrent;
		}
		else
		{
			isApplyingFade = false;
			CancelInvoke("ExecuteImageFadeIn");
		}
	}

	private void ExecuteImageFadeOut()
	{
		colorCurrent = imgToFadeOut.color;
		if (colorCurrent.a > 0f)
		{
			colorCurrent.a -= freqRepeatFade;
			imgToFadeOut.color = colorCurrent;
		}
		else
		{
			isApplyingFade = false;
			CancelInvoke("ExecuteImageFadeOut");
		}
	}

	private void ExecuteImageFadeOutThenIn()
	{
		colorCurrent = imgToFadeOut.color;
		if (colorCurrent.a > 0f)
		{
			colorCurrent.a -= freqRepeatFade;
			imgToFadeOut.color = colorCurrent;
		}
		else
		{
			CancelInvoke("ExecuteImageFadeOutThenIn");
			StartImageFadeEffect(FadeType.SingleFadeIn);
		}
	}

	private void StartTextFadeEffect(FadeType action, bool isSpecial)
	{
		isApplyingFade = true;
		switch (action)
		{
		case FadeType.SingleFadeOut:
			InvokeRepeating("ExecuteTextFadeOut", 0f, freqRepeatFade);
			return;
		case FadeType.FadeOutThenIn:
			InvokeRepeating("ExecuteTextFadeOutThenIn", 0f, freqRepeatFade);
			return;
		}
		if (isSpecial)
		{
			ApplyDisplayTextAndImage();
		}
		InvokeRepeating("ExecuteTextFadeIn", 0f, freqRepeatFade);
	}

	private void ExecuteTextFadeIn()
	{
		colorCurrent = txtStoryLineDisplay.color;
		if (colorCurrent.a < 1f)
		{
			colorCurrent.a += freqRepeatFade;
			txtStoryLineDisplay.color = colorCurrent;
			imgTextPanelBG.color = colorCurrent;
		}
		else
		{
			isApplyingFade = false;
			CancelInvoke("ExecuteTextFadeIn");
		}
	}

	private void ExecuteTextFadeOut()
	{
		colorCurrent = txtStoryLineDisplay.color;
		if (colorCurrent.a > 0f)
		{
			colorCurrent.a -= freqRepeatFade;
			txtStoryLineDisplay.color = colorCurrent;
			imgTextPanelBG.color = colorCurrent;
		}
		else
		{
			isApplyingFade = false;
			CancelInvoke("ExecuteTextFadeOut");
		}
	}

	private void ExecuteTextFadeOutThenIn()
	{
		colorCurrent = txtStoryLineDisplay.color;
		if (colorCurrent.a > 0f)
		{
			colorCurrent.a -= freqRepeatFade;
			txtStoryLineDisplay.color = colorCurrent;
			imgTextPanelBG.color = colorCurrent;
		}
		else
		{
			CancelInvoke("ExecuteTextFadeOutThenIn");
			StartTextFadeEffect(FadeType.SingleFadeIn, isSpecial: true);
		}
	}

	private void TranslateAndApplyControlsText()
	{
		txtLabelNext.text = GC.GetTranslatedStringUI(146) + ":";
		txtLabelSkip.text = GC.GetTranslatedStringUI(147) + ":";
		txtInputNext.text = GC.IKey(GameCore.InputLib.ConfirmUI).ToString();
		txtInputSkip.text = GC.IKey(GameCore.InputLib.CancelUI).ToString();
		if (Application.platform == RuntimePlatform.Android)
		{
			txtInputNext.text = "Tap";
			txtInputSkip.text = "2-Finger Tap";
		}
	}

	private IEnumerator waitForInputLockDelay(float wait)
	{
		isInputAllowed = false;
		yield return new WaitForSeconds(wait);
		isInputAllowed = true;
		coroutInputLock = null;
	}
}
