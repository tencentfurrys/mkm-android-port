using System.Collections;
using UnityEngine;

public class LeverSignal : MonoBehaviour
{
	public enum LeverSprite
	{
		Stick = 0,
		Crystal = 1,
		Switch = 2
	}

	public enum LeverType
	{
		Toggle = 0,
		Timer = 1,
		Pressure = 2
	}

	public enum LeverSounds
	{
		None = 0,
		LeverStart = 1,
		LeverEnd = 2,
		LeverRepeat = 3,
		CrystalOn = 4,
		CrystalOff = 5,
		CrystalEngaged = 6,
		CrystalDisengaged = 7,
		SwitchStart = 8,
		SwitchEnd = 9
	}

	[Tooltip("[ENUM] This sets up what will be the sprite and animations of this lever.")]
	public LeverSprite currentLeverSprite;

	[Tooltip("[ENUM] TOGGLE: Switches ON/OFF and the signal stays in the state. TIMER: A signal is sent when lever is activated, then a timer runs down until it resets the state/signal back.")]
	public LeverType currentLeverType;

	[Tooltip("[FLOAT] If the type of lever is TIMER, then the amount in seconds is set here.")]
	public float timerAmount;

	[Tooltip("[OBJ] The ICON that will appear above this object when the player is near it (touching the collider).")]
	public GameObject objIconToDisplay;

	[Tooltip("[BOOL] Adds a delay (of the same existing private value that delays instant reactivation) before sending the signal.")]
	public bool delaySendingSignal;

	[Tooltip("[OBJ] The list of mechanisms attached to receive signals from this lever.")]
	public GameObject[] arrayOfMechanisms;

	public LeverSounds sfxBeginTowardsOn;

	public LeverSounds sfxFinishAtOn;

	public LeverSounds sfxBeginTowardsOff;

	public LeverSounds sfxFinishAtOff;

	public LeverSounds sfxRepeatTimer;

	[Tooltip("[OBJ] A visual effect that will indicate the direction that the ON signal goes to.")]
	public GameObject objVisualSignalON;

	[Tooltip("[OBJ] A visual effect that will indicate the direction that the OFF signal goes to.")]
	public GameObject objVisualSignalOFF;

	[Tooltip("[OBJ] A visual line that connects to all mechanisms.")]
	public GameObject objVisualLine;

	public bool blockVisualLine;

	public bool allowSaveSignalState;

	public bool alwaysSendTrue;

	[HideInInspector]
	public bool signal_ON;

	private string strInvokedLines = "InvokedRefreshVisualLines";

	private bool canUseLever;

	private bool isTimerReseting;

	private float delayAmountAfterSwitch;

	private float delaySignalAfterSwitch;

	private int orderLeverID;

	private GameObject[] arrayVisualLines;

	private Coroutine crtDelayToggleSignal;

	private Coroutine crtDelaySendingSignal;

	private Animator objAN;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		Initialization();
		AcquireSelfOrderIDUnderParent();
		LoadSignalStateIfApplicable();
	}

	private void Initialization()
	{
		objAN = GetComponent<Animator>();
		canUseLever = true;
		signal_ON = false;
		isTimerReseting = false;
		delaySignalAfterSwitch = 0.2f;
		delayAmountAfterSwitch = 1f;
		if (currentLeverType == LeverType.Timer && timerAmount <= 0f)
		{
			timerAmount = 1f;
		}
		SetupLeverSprite();
	}

	private void SetupLeverSprite()
	{
		int num = 0;
		switch (currentLeverSprite)
		{
		default:
			num = 0;
			break;
		case LeverSprite.Crystal:
			num = 1;
			break;
		case LeverSprite.Switch:
			num = 2;
			break;
		}
		objAN.SetLayerWeight(num, 1f);
	}

	private void AcquireSelfOrderIDUnderParent()
	{
		orderLeverID = 0;
		foreach (Transform item in base.transform.parent)
		{
			if (base.transform == item)
			{
				break;
			}
			orderLeverID++;
		}
	}

	public void ExecuteThisLeverActivation(bool instantEffect = false)
	{
		if (instantEffect)
		{
			LastFrameOfLeverSwitch(1);
			return;
		}
		if (IsInvoking("InvokePlayTimerSound"))
		{
			CancelInvoke("InvokePlayTimerSound");
		}
		if (!signal_ON)
		{
			PlayLocalSFX(sfxBeginTowardsOn);
		}
		else
		{
			PlayLocalSFX(sfxBeginTowardsOff);
		}
		canUseLever = false;
		switch (currentLeverType)
		{
		case LeverType.Toggle:
		case LeverType.Timer:
			if ((bool)objAN)
			{
				if (signal_ON)
				{
					objAN.Play("Mech-LeverBtoA");
				}
				else
				{
					objAN.Play("Mech-LeverAtoB");
				}
			}
			break;
		}
		for (int i = 0; i < arrayOfMechanisms.Length; i++)
		{
			InstantiateSignalVisualEffect(arrayOfMechanisms[i].transform.position, !signal_ON);
		}
	}

	public void LastFrameOfLeverSwitch(int instantEffect = 0)
	{
		signal_ON = !signal_ON;
		if (instantEffect >= 1)
		{
			SendCurrentSignalToAllMechanisms(instantEffect: true);
			if (signal_ON)
			{
				objAN.Play("Mech-LeverIdleB");
			}
			return;
		}
		if (delaySendingSignal)
		{
			if (crtDelaySendingSignal != null)
			{
				StopCoroutine(crtDelaySendingSignal);
			}
			crtDelaySendingSignal = StartCoroutine(waitDelaySendSignal(delaySignalAfterSwitch));
		}
		else
		{
			if (crtDelaySendingSignal != null)
			{
				StopCoroutine(crtDelaySendingSignal);
			}
			SendCurrentSignalToAllMechanisms();
		}
		switch (currentLeverType)
		{
		case LeverType.Toggle:
			if (crtDelayToggleSignal != null)
			{
				StopCoroutine(crtDelayToggleSignal);
			}
			crtDelayToggleSignal = StartCoroutine(waitDelayToggleSignal(delayAmountAfterSwitch));
			break;
		case LeverType.Timer:
			if (!isTimerReseting)
			{
				isTimerReseting = true;
				if (crtDelayToggleSignal != null)
				{
					StopCoroutine(crtDelayToggleSignal);
				}
				crtDelayToggleSignal = StartCoroutine(waitDelayTimerSignal(timerAmount));
				InvokeRepeating("InvokePlayTimerSound", 0.5f, 1f);
				GC.InstantiateVFX(GameCore.VisualEffect.Hourglass, base.transform.position, "[LeverSignal]LastFrameOfLeverSwitch()", null, returnOBJref: true).GetComponent<EffectLogic>().timeDurationLoop = timerAmount;
			}
			else
			{
				isTimerReseting = false;
				canUseLever = true;
			}
			break;
		}
		if (signal_ON)
		{
			PlayLocalSFX(sfxFinishAtOn);
		}
		else
		{
			PlayLocalSFX(sfxFinishAtOff);
		}
		SaveCurrentSignalState();
	}

	private void SendCurrentSignalToAllMechanisms(bool instantEffect = false)
	{
		bool sentSignal = signal_ON;
		if (alwaysSendTrue)
		{
			sentSignal = true;
		}
		if (arrayOfMechanisms.Length != 0)
		{
			for (int i = 0; i < arrayOfMechanisms.Length; i++)
			{
				if (arrayOfMechanisms != null)
				{
					MechanismLogic component = arrayOfMechanisms[i].GetComponent<MechanismLogic>();
					MovingPlatform component2 = arrayOfMechanisms[i].GetComponent<MovingPlatform>();
					if ((bool)component)
					{
						component.ReceivedSignal(sentSignal, instantEffect);
						component = null;
					}
					else if ((bool)component2)
					{
						component2.ReceivedSignal(sentSignal, instantEffect);
						component2 = null;
					}
					else
					{
						Debug.Log("Could not get a reference to the MECHANISM script in the OBJ in the array of the lever " + base.gameObject.name + "!");
					}
				}
				else
				{
					Debug.Log("Null mechanism OBJ at index " + i + " in the lever " + base.gameObject.name + ". Ignoring this index.");
				}
			}
		}
		else
		{
			Debug.Log("This lever " + base.gameObject.name + " has no mechanisms attached in the array!");
		}
	}

	private void InstantiateSignalVisualEffect(Vector3 vecPosition, bool stateOfSignal)
	{
		if (alwaysSendTrue)
		{
			stateOfSignal = true;
		}
		if (stateOfSignal && (bool)objVisualSignalON)
		{
			Object.Instantiate(objVisualSignalON, base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<EffectLogic>().targetPosMove = vecPosition;
		}
		else if (!stateOfSignal && (bool)objVisualSignalOFF)
		{
			Object.Instantiate(objVisualSignalOFF, base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<EffectLogic>().targetPosMove = vecPosition;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && !GC.isPlayerFainted && objIconToDisplay != null)
		{
			objIconToDisplay.SetActive(value: true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && objIconToDisplay != null)
		{
			objIconToDisplay.SetActive(value: false);
		}
	}

	private void ToggleVisualConnectionLine(bool active)
	{
		if (GC.doDisplayLinesMechanisms && !blockVisualLine)
		{
			if (active && !IsInvoking(strInvokedLines))
			{
				InvokeRepeating(strInvokedLines, 0f, 0.025f);
			}
		}
		else
		{
			if (IsInvoking(strInvokedLines))
			{
				CancelInvoke(strInvokedLines);
			}
			active = false;
		}
		if (arrayVisualLines != null && arrayVisualLines.Length != 0)
		{
			for (int i = 0; i < arrayVisualLines.Length; i++)
			{
				arrayVisualLines[i].SetActive(active);
			}
		}
	}

	private void InvokedRefreshVisualLines()
	{
		if (arrayVisualLines == null || arrayVisualLines.Length < 0)
		{
			arrayVisualLines = new GameObject[arrayOfMechanisms.Length];
			for (int i = 0; i < arrayOfMechanisms.Length; i++)
			{
				arrayVisualLines[i] = Object.Instantiate(objVisualLine, base.transform.position, Quaternion.identity, base.transform);
			}
			return;
		}
		Color color = Color.green;
		if (signal_ON)
		{
			color = Color.red;
		}
		if (objIconToDisplay.activeInHierarchy)
		{
			for (int j = 0; j < arrayVisualLines.Length; j++)
			{
				LineRenderer component = arrayVisualLines[j].GetComponent<LineRenderer>();
				component.SetPosition(0, base.transform.position);
				component.SetPosition(1, arrayOfMechanisms[j].transform.position);
				Gradient gradient = new Gradient();
				GradientColorKey[] colorKeys = component.colorGradient.colorKeys;
				GradientAlphaKey[] alphaKeys = component.colorGradient.alphaKeys;
				colorKeys[0].color = color;
				gradient.SetKeys(colorKeys, alphaKeys);
				component.colorGradient = gradient;
			}
		}
	}

	private void PlayLocalSFX(LeverSounds thisLocalSFX)
	{
		if ((bool)GC)
		{
			switch (thisLocalSFX)
			{
			case LeverSounds.LeverStart:
				GC.PlaySFX(GameCore.SFXBasic1.LeverStart, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case LeverSounds.LeverEnd:
				GC.PlaySFX(GameCore.SFXBasic1.LeverEnd, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case LeverSounds.LeverRepeat:
				GC.PlaySFX(GameCore.SFXBasic1.LeverEnd, 0, checkForDuplicity: false, Vector2.zero, 1.2f, 1.3f);
				break;
			case LeverSounds.CrystalOn:
				GC.PlaySFX(GameCore.SFXBasic1.CrystalOn, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case LeverSounds.CrystalOff:
				GC.PlaySFX(GameCore.SFXBasic1.CrystalOff, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case LeverSounds.CrystalEngaged:
				GC.PlaySFX(GameCore.SFXBasic1.CrystalEngaged, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case LeverSounds.CrystalDisengaged:
				GC.PlaySFX(GameCore.SFXBasic1.CrystalDisengaged, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case LeverSounds.SwitchStart:
				GC.PlaySFX(GameCore.SFXBasic2.SwitchStart, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case LeverSounds.SwitchEnd:
				GC.PlaySFX(GameCore.SFXBasic2.SwitchEnd, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			}
		}
	}

	private void InvokePlayTimerSound()
	{
		PlayLocalSFX(sfxRepeatTimer);
	}

	public bool CanThisLeverBeActivated()
	{
		return canUseLever;
	}

	private IEnumerator waitDelayToggleSignal(float wait)
	{
		yield return new WaitForSeconds(wait);
		canUseLever = true;
		crtDelayToggleSignal = null;
	}

	private IEnumerator waitDelaySendSignal(float wait, bool instantEffect = false)
	{
		yield return new WaitForSeconds(wait);
		if (instantEffect)
		{
			ExecuteThisLeverActivation(instantEffect: true);
			crtDelaySendingSignal = null;
		}
		else
		{
			SendCurrentSignalToAllMechanisms();
			crtDelaySendingSignal = null;
		}
	}

	private IEnumerator waitDelayTimerSignal(float wait)
	{
		yield return new WaitForSeconds(wait);
		ExecuteThisLeverActivation();
		crtDelayToggleSignal = null;
	}

	private void SaveCurrentSignalState()
	{
		if (allowSaveSignalState)
		{
			GameLeverSignalState gameLeverSignalState = GC.listOfLeverSignalState.Find((GameLeverSignalState x) => x.sceneName == GC.strCurrentActiveSceneName && x.orderLeverID == orderLeverID);
			if (gameLeverSignalState == null)
			{
				gameLeverSignalState = new GameLeverSignalState
				{
					sceneName = GC.strCurrentActiveSceneName,
					orderLeverID = orderLeverID,
					signalState = signal_ON
				};
				GC.listOfLeverSignalState.Add(gameLeverSignalState);
			}
			else
			{
				GC.listOfLeverSignalState.Remove(gameLeverSignalState);
				gameLeverSignalState.signalState = signal_ON;
				GC.listOfLeverSignalState.Add(gameLeverSignalState);
			}
		}
	}

	private void LoadSignalStateIfApplicable()
	{
		if (!allowSaveSignalState)
		{
			return;
		}
		GameLeverSignalState gameLeverSignalState = GC.listOfLeverSignalState.Find((GameLeverSignalState x) => x.sceneName == GC.strCurrentActiveSceneName && x.orderLeverID == orderLeverID);
		if (gameLeverSignalState != null && gameLeverSignalState.signalState != signal_ON)
		{
			if (crtDelaySendingSignal != null)
			{
				StopCoroutine(crtDelaySendingSignal);
			}
			crtDelaySendingSignal = StartCoroutine(waitDelaySendSignal(0.1f, instantEffect: true));
		}
	}
}
