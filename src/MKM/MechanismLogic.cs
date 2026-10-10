using System.Collections;
using UnityEngine;

public class MechanismLogic : MonoBehaviour
{
	public enum MechanismType
	{
		MovingGate,
		DisableColl,
		Spawner,
		ToggleObjectActive
	}

	public enum MechSoundType
	{
		Null,
		OpenDoor,
		CloseDoor,
		OpenHatch
	}

	[Tooltip("[ENUM] MOVINGGATE: Moves the object from the start position to a destination position (and back). DISABLECOLL: Toggles the collider ON/OFF;")]
	public MechanismType currentMechType;

	[Tooltip("[VEC2] How much in the X and Y axis (can be negative) the object needs to move. This effectivelly moves the OBJ to a destination position, based on this offset from the starting pos.")]
	public Vector3 offsetFromStartPos;

	public bool isStartingHidden;

	public bool endRemainHidden;

	public bool isStartingInvis;

	public bool DoorStartOpenInvisible;

	public GameObject spawnThis;

	public int limitSpawnQuantity;

	public MechSoundType SFXOpen;

	public MechSoundType SFXClose;

	public bool isSpecialActivation;

	public int OverrideGateHide;

	public MECH_SpecialTriggers[] listSpecial;

	private GameObject[] arrayRefSpawn;

	private bool signal_ON;

	private bool checkMovingGatePos;

	private int hiddenLayerOrder;

	private int originalSortingOrder;

	private float moveSpeed;

	private Color colorOriginal;

	private Color colorTransparent;

	private Coroutine crtDelaySound;

	private SpriteRenderer objRE;

	private BoxCollider2D objBox2D;

	private Vector3 startingPos;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		hiddenLayerOrder = -5;
		if (OverrideGateHide != 0)
		{
			hiddenLayerOrder = OverrideGateHide;
		}
		Initialization();
	}

	private void Initialization()
	{
		objRE = GetComponent<SpriteRenderer>();
		objBox2D = GetComponent<BoxCollider2D>();
		if (objRE != null)
		{
			colorOriginal = objRE.color;
		}
		colorTransparent = colorOriginal;
		colorTransparent.a = 0f;
		startingPos = transform.position;
		if (objRE != null)
		{
			originalSortingOrder = objRE.sortingOrder;
		}
		offsetFromStartPos.x = transform.position.x + offsetFromStartPos.x;
		offsetFromStartPos.y = transform.position.y + offsetFromStartPos.y;
		signal_ON = false;
		moveSpeed = 1f;
		checkMovingGatePos = false;
		if (isStartingHidden && objRE != null)
		{
			objRE.sortingOrder = hiddenLayerOrder;
		}
		if (isStartingInvis && objRE != null)
		{
			objRE.color = colorTransparent;
		}
		if (limitSpawnQuantity > 0)
		{
			arrayRefSpawn = new GameObject[limitSpawnQuantity];
		}
		if (isSpecialActivation)
		{
			InvokeRepeating("InvokedSpecialActivation", 1f, 1f);
		}
		if (currentMechType == MechanismType.DisableColl && DoorStartOpenInvisible)
		{
			ValidateSignalChange(isIgnoreSoundOnStart: true);
		}
	}

	private void Update()
	{
		if (!checkMovingGatePos)
		{
			return;
		}
		SwitchOrderInLayer(1);
		if (signal_ON && transform.position != offsetFromStartPos)
		{
			transform.position = Vector3.MoveTowards(transform.position, offsetFromStartPos, moveSpeed * Time.deltaTime);
			if (transform.position == offsetFromStartPos)
			{
				checkMovingGatePos = false;
				PlayClosedSound();
				SwitchOrderInLayer(2);
			}
		}
		else if (!signal_ON && transform.position != startingPos)
		{
			transform.position = Vector3.MoveTowards(transform.position, startingPos, moveSpeed * Time.deltaTime);
			if (transform.position == startingPos)
			{
				checkMovingGatePos = false;
				PlayClosedSound();
				SwitchOrderInLayer(2);
			}
		}
	}

	private void ValidateSignalChange(bool isIgnoreSoundOnStart = false, bool instantEffect = false)
	{
		if (IsInvoking("InvokePlayOpenSound"))
		{
			CancelInvoke("InvokePlayOpenSound");
		}
		switch (currentMechType)
		{
		case MechanismType.MovingGate:
			if (instantEffect)
			{
				if (signal_ON)
				{
					transform.position = offsetFromStartPos;
					SwitchOrderInLayer(2);
				}
			}
			else
			{
				checkMovingGatePos = true;
				InvokeRepeating("InvokePlayOpenSound", 0f, 1f);
			}
			break;
		case MechanismType.DisableColl:
			if ((bool)objBox2D)
			{
				objBox2D.enabled = !objBox2D.enabled;
				if (objBox2D.enabled)
				{
					PlayOpenCloseSound(actionOpen: false, isIgnoreSoundOnStart);
				}
				else
				{
					PlayOpenCloseSound(actionOpen: true, isIgnoreSoundOnStart);
				}
			}
			if ((bool)objRE)
			{
				if (objRE.color == colorOriginal)
				{
					objRE.color = colorTransparent;
					DoChildrenLogic(0);
				}
				else
				{
					objRE.color = colorOriginal;
					DoChildrenLogic(1);
				}
			}
			break;
		case MechanismType.Spawner:
		{
			if (arrayRefSpawn == null || !(spawnThis != null))
			{
				break;
			}
			for (int i = 0; i < arrayRefSpawn.Length; i++)
			{
				if (arrayRefSpawn[i] == null || !arrayRefSpawn[i].activeInHierarchy)
				{
					arrayRefSpawn[i] = Object.Instantiate(spawnThis, transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
					GC.InstantiateVFX(GameCore.VisualEffect.HatchLeft, transform.position, "MECHANISMLOGIC:ValidateSignalChange");
					PlayOpenCloseSound(actionOpen: true, isIgnoreSoundOnStart);
				}
			}
			break;
		}
		case MechanismType.ToggleObjectActive:
			if (!gameObject.activeInHierarchy)
			{
				gameObject.SetActive(value: true);
				StartStopDelaySoundCoroutine(startCoroutine: true, 0.1f, setSoundState: true, isIgnoreSoundOnStart);
			}
			else
			{
				gameObject.SetActive(value: false);
				PlayOpenCloseSound(actionOpen: false, isIgnoreSoundOnStart);
			}
			break;
		}
	}

	public void ReceivedSignal(bool sentSignal, bool instantEffect = false)
	{
		signal_ON = sentSignal;
		ValidateSignalChange(instantEffect, instantEffect);
	}

	private void DoChildrenLogic(int action)
	{
		if (gameObject.transform.childCount <= 0)
		{
			return;
		}
		MechanismType mechanismType = currentMechType;
		if (mechanismType != MechanismType.DisableColl)
		{
			return;
		}
		Color white = Color.white;
		white = ((action != 0) ? colorOriginal : colorTransparent);
		foreach (Transform item in transform)
		{
			item.gameObject.GetComponent<SpriteRenderer>().color = white;
		}
	}

	private void InvokedSpecialActivation()
	{
		if (listSpecial == null || listSpecial.Length == 0)
		{
			return;
		}
		int num = 0;
		for (int i = 0; i < listSpecial.Length; i++)
		{
			if (listSpecial[i].scriptLever.signal_ON == listSpecial[i].stateRequired)
			{
				num++;
			}
		}
		if (num == listSpecial.Length && !signal_ON)
		{
			ReceivedSignal(sentSignal: true);
		}
		else if (num < listSpecial.Length && signal_ON)
		{
			ReceivedSignal(sentSignal: false);
		}
	}

	private void InvokePlayOpenSound()
	{
		if ((bool)GC)
		{
			GC.PlaySFX(GameCore.SFXBasic1.GateOpening, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
	}

	private void PlayClosedSound()
	{
		if (IsInvoking("InvokePlayOpenSound"))
		{
			CancelInvoke("InvokePlayOpenSound");
		}
		if ((bool)GC)
		{
			GC.PlaySFX(GameCore.SFXBasic1.GateClosed, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
		}
	}

	private void PlayOpenCloseSound(bool actionOpen, bool ignoreSoundOnStart)
	{
		if (!ignoreSoundOnStart && (bool)GC)
		{
			if (actionOpen)
			{
				GC.PlaySFX(ReturnSFX(SFXOpen), 0, checkForDuplicity: false, Vector2.zero, 1f, 1f);
			}
			else
			{
				GC.PlaySFX(ReturnSFX(SFXClose), 0, checkForDuplicity: false, Vector2.zero, 1f, 1f);
			}
		}
	}

	private GameCore.SFXBasic2 ReturnSFX(MechSoundType thisType)
	{
		return thisType switch
		{
			MechSoundType.OpenDoor => GameCore.SFXBasic2.DoorOpen, 
			MechSoundType.CloseDoor => GameCore.SFXBasic2.DoorClose, 
			MechSoundType.OpenHatch => GameCore.SFXBasic2.HatchOpen, 
			_ => GameCore.SFXBasic2.Null, 
		};
	}

	private void SwitchOrderInLayer(int type)
	{
		switch (type)
		{
		case 1:
			if (objRE.sortingOrder != hiddenLayerOrder)
			{
				objRE.sortingOrder = hiddenLayerOrder;
			}
			break;
		case 2:
			if (endRemainHidden)
			{
				objRE.sortingOrder = hiddenLayerOrder;
			}
			else if (signal_ON && isStartingHidden && objRE.sortingOrder != originalSortingOrder)
			{
				objRE.sortingOrder = originalSortingOrder;
			}
			else if (signal_ON && !isStartingHidden && objRE.sortingOrder != hiddenLayerOrder)
			{
				objRE.sortingOrder = hiddenLayerOrder;
			}
			else if (!signal_ON && !isStartingHidden && objRE.sortingOrder != originalSortingOrder)
			{
				objRE.sortingOrder = originalSortingOrder;
			}
			break;
		}
	}

	private void StartStopDelaySoundCoroutine(bool startCoroutine, float waitTime, bool setSoundState, bool ignoreSound)
	{
		if (startCoroutine)
		{
			if (crtDelaySound != null)
			{
				StopCoroutine(crtDelaySound);
			}
			crtDelaySound = StartCoroutine(waitDelaySoundPlay(waitTime, setSoundState, ignoreSound));
		}
		else
		{
			if (crtDelaySound != null)
			{
				StopCoroutine(crtDelaySound);
			}
			crtDelaySound = null;
		}
	}

	private IEnumerator waitDelaySoundPlay(float wait, bool state, bool ignoreSound)
	{
		yield return new WaitForSeconds(wait);
		PlayOpenCloseSound(state, ignoreSound);
		crtDelaySound = null;
	}
}
