using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
	public enum PlatformMovementType
	{
		Up,
		Down,
		Left,
		Right,
		Diagonal_A,
		Diagonal_B
	}

	[Tooltip("[CUSTOM] The type of movement that the platform will perform. Diagonal_A goes top left to bottom right. Diagonal_B goes bottom left to top right.")]
	public PlatformMovementType platMovType;

	[Tooltip("[FLOAT] (min=1;max=6) The speed at which the platform will move.")]
	public float baseMovSpeed;

	[Tooltip("[FLOAT] (min=1;max=100) The max range that the platform will move to from the starting position. For vertical, the starting position is considered the bottom edge, and the range is the top edge. For all other types, the starting position is considered to be the left edge, and the range determines the right edge.")]
	public float rangeOfMovement;

	[Tooltip("[BOOL] If this is selected the platform will not move in the 'ping-pong' pattern as usual. Instead it will keep moving in the direction type selected until the time limit, where then it will disable itself.")]
	public float timerLimit;

	[Tooltip("[FLOAT] (min=.1f;max=1f;default=.2f) The frequency at which the methods that handle the movement are repeated.")]
	public float frequencyOfBehaviour;

	[Tooltip("[BOOL] (default ON) Moving platforms can receive signals to turn it ON or OFF. This box determines the default state that the platform will perform at start.")]
	public bool startPowered;

	public bool startingFromMidPoint;

	public float timerStartPowered;

	[Tooltip("[BOOL] Toggle this option to draw a red box (enable gizmos in editor) showing where the platform will move to (editor only) taking into consideration its starting position, range and move direction.")]
	public bool displayDestinationGizmo;

	[Tooltip("[BOOL] This platform will, at the START() method, spawn a small visual indicator at each unit/point between it's starting position and the end position.")]
	public bool spawnIndicators;

	[Tooltip("[GAMEOBJECT] The specific visual indicator that should be spawned.")]
	public GameObject objIndicator;

	public bool stopAtPingPongEdges;

	public bool swapStartEndSceneName;

	public bool swapStartPoweredSceneName;

	public string previousSceneName;

	private Vector2 vecStartPos;

	private Vector2 vecTargetPos;

	private Vector2 vecMovSpeedDirection;

	private GameObject referenceToMyIndicators;

	private Coroutine crtTimerStartPowered;

	private float countInvokedSpawnIndicators;

	private bool signal_ON;

	private GameCore GC;

	private Rigidbody2D objRB;

	private void Start()
	{
		GC = GameCore.Instance;
		InitializeReferences();
		InitializeVariables();
		SetupPlatformBehaviour();
		if (spawnIndicators && objIndicator != null && (bool)GC)
		{
			InvokeRepeating("InvokedTryToSpawnIndicators", 0f, 0.2f);
		}
	}

	private void InitializeReferences()
	{
		objRB = GetComponent<Rigidbody2D>();
	}

	private void InitializeVariables()
	{
		CheckStartPoweredLogic();
		vecStartPos = transform.position;
		if (baseMovSpeed < 1f)
		{
			baseMovSpeed = 1f;
		}
		if (rangeOfMovement < 1f)
		{
			rangeOfMovement = 1f;
		}
		else if (rangeOfMovement > 100f)
		{
			rangeOfMovement = 100f;
		}
		if (timerLimit < 1f)
		{
			timerLimit = 1f;
		}
		else if (timerLimit > 120f)
		{
			timerLimit = 120f;
		}
		if (frequencyOfBehaviour < 0.1f)
		{
			frequencyOfBehaviour = 0.1f;
		}
		else if (frequencyOfBehaviour > 1f)
		{
			frequencyOfBehaviour = 1f;
		}
	}

	private void SetupPlatformBehaviour()
	{
		switch (platMovType)
		{
		default:
			if (startingFromMidPoint)
			{
				vecStartPos.x -= rangeOfMovement / 2f;
			}
			vecTargetPos.x = vecStartPos.x + rangeOfMovement;
			vecMovSpeedDirection = new Vector2(baseMovSpeed, 0f);
			break;
		case PlatformMovementType.Left:
			if (startingFromMidPoint)
			{
				vecStartPos.x += rangeOfMovement / 2f;
			}
			vecTargetPos.x = vecStartPos.x - rangeOfMovement;
			vecMovSpeedDirection = new Vector2(0f - baseMovSpeed, 0f);
			break;
		case PlatformMovementType.Up:
			if (startingFromMidPoint)
			{
				vecStartPos.y -= rangeOfMovement / 2f;
			}
			vecTargetPos.y = vecStartPos.y + rangeOfMovement;
			vecMovSpeedDirection = new Vector2(0f, baseMovSpeed);
			break;
		case PlatformMovementType.Down:
			if (startingFromMidPoint)
			{
				vecStartPos.y += rangeOfMovement / 2f;
			}
			vecTargetPos.y = vecStartPos.y - rangeOfMovement;
			vecMovSpeedDirection = new Vector2(0f, 0f - baseMovSpeed);
			break;
		case PlatformMovementType.Diagonal_A:
			vecTargetPos.x = vecStartPos.x + rangeOfMovement;
			vecTargetPos.y = vecStartPos.y + rangeOfMovement;
			vecMovSpeedDirection = new Vector2(baseMovSpeed, 0f - baseMovSpeed);
			break;
		case PlatformMovementType.Diagonal_B:
			vecTargetPos.x = vecStartPos.x + rangeOfMovement;
			vecTargetPos.y = vecStartPos.y + rangeOfMovement;
			vecMovSpeedDirection = new Vector2(baseMovSpeed, baseMovSpeed);
			break;
		}
		if (swapStartEndSceneName && ValidatePreviousSceneLogic())
		{
			SwapStartingPosition();
		}
		InvokeRepeating("ExecuteMovement", 0f, frequencyOfBehaviour);
	}

	private void SwapStartingPosition()
	{
		Vector2 vector = transform.position;
		switch (platMovType)
		{
		case PlatformMovementType.Left:
		case PlatformMovementType.Right:
			vector.x = vecTargetPos.x;
			break;
		case PlatformMovementType.Up:
		case PlatformMovementType.Down:
			vector.y = vecTargetPos.y;
			break;
		case PlatformMovementType.Diagonal_A:
		case PlatformMovementType.Diagonal_B:
			vector = vecTargetPos;
			break;
		}
		transform.position = vector;
		RoundStoppedPositionOfPlatformOnStop();
		vecMovSpeedDirection *= -1f;
	}

	private void ExecuteMovement()
	{
		if (!signal_ON)
		{
			objRB.velocity = Vector2.zero;
			return;
		}
		if (objRB.velocity != vecMovSpeedDirection)
		{
			objRB.velocity = vecMovSpeedDirection;
		}
		if (!IsPastPoint())
		{
			return;
		}
		if (stopAtPingPongEdges)
		{
			if (objRB.velocity == vecMovSpeedDirection)
			{
				objRB.velocity = Vector2.zero;
				ReceivedSignal(sentSignal: false);
				RoundStoppedPositionOfPlatformOnStop();
				vecMovSpeedDirection *= -1f;
			}
		}
		else
		{
			vecMovSpeedDirection *= -1f;
			objRB.velocity = vecMovSpeedDirection;
		}
	}

	private bool IsPastPoint()
	{
		float num = GC.MATH_RoundFloatDecimals(transform.position.x, 1);
		float num2 = GC.MATH_RoundFloatDecimals(transform.position.y, 1);
		float num3 = GC.MATH_RoundFloatDecimals(vecStartPos.x, 1);
		float num4 = GC.MATH_RoundFloatDecimals(vecStartPos.y, 1);
		float num5 = GC.MATH_RoundFloatDecimals(vecTargetPos.x, 1);
		float num6 = GC.MATH_RoundFloatDecimals(vecTargetPos.y, 1);
		switch (platMovType)
		{
		case PlatformMovementType.Up:
			if ((objRB.velocity.y > 0.1f && num2 >= num6) || (objRB.velocity.y < 0.1f && num2 <= num4))
			{
				return true;
			}
			break;
		case PlatformMovementType.Down:
			if ((objRB.velocity.y < 0.1f && num2 <= num6) || (objRB.velocity.y > 0.1f && num2 >= num4))
			{
				return true;
			}
			break;
		case PlatformMovementType.Right:
			if ((objRB.velocity.x > 0.1f && num >= num5) || (objRB.velocity.x < 0.1f && num <= num3))
			{
				return true;
			}
			break;
		case PlatformMovementType.Left:
			if ((objRB.velocity.x < 0.1f && num <= num5) || (objRB.velocity.x > 0.1f && num >= num3))
			{
				return true;
			}
			break;
		}
		return false;
	}

	private void RoundStoppedPositionOfPlatformOnStop()
	{
		if (CloseToPoint(vecStartPos, 1f))
		{
			transform.position = vecStartPos;
		}
		else if (CloseToPoint(vecTargetPos, 1f))
		{
			transform.position = vecTargetPos;
		}
		else
		{
			transform.position = GC.MATH_ROUND_VECTOR3(transform.position, 1);
		}
	}

	private bool CloseToPoint(Vector3 pointToCheck, float thisClose)
	{
		float num = 0f;
		float num2 = 0f;
		num = transform.position.x - pointToCheck.x;
		num2 = transform.position.y - pointToCheck.y;
		if (num < 0f)
		{
			num *= -1f;
		}
		if (num2 < 0f)
		{
			num2 *= -1f;
		}
		if (num <= thisClose && num2 <= thisClose)
		{
			return true;
		}
		return false;
	}

	public void ReceivedSignal(bool sentSignal, bool instantEffect = false)
	{
		if (!stopAtPingPongEdges || !(GC.MATH_ROUND_VECTOR3(objRB.velocity, 0) != Vector3.zero))
		{
			signal_ON = sentSignal;
			OnSignalChangeUpdateIndicatorsVisual();
			PlaySignalToggledSounds(instantEffect);
		}
	}

	private void OnDrawGizmosSelected()
	{
		_ = displayDestinationGizmo;
	}

	private void InvokedTryToSpawnIndicators()
	{
		countInvokedSpawnIndicators += 0.2f;
		if (referenceToMyIndicators == null)
		{
			SpawnVisualIndicators();
		}
		if (referenceToMyIndicators != null || countInvokedSpawnIndicators >= 1f)
		{
			CancelInvoke("InvokedTryToSpawnIndicators");
		}
	}

	private void SpawnVisualIndicators()
	{
		referenceToMyIndicators = new GameObject();
		Vector2 vector = vecStartPos;
		vector.y += 0.3f;
		referenceToMyIndicators.transform.parent = GC.refOBJEffectsParent.transform;
		referenceToMyIndicators.name = "PlatMoveDots_" + gameObject.GetInstanceID();
		referenceToMyIndicators.AddComponent<PlatMoveDots>();
		for (int i = 0; (float)i <= rangeOfMovement; i++)
		{
			Object.Instantiate(objIndicator, vector, Quaternion.identity, referenceToMyIndicators.transform);
			switch (platMovType)
			{
			case PlatformMovementType.Up:
				vector.y++;
				break;
			case PlatformMovementType.Down:
				vector.y += -1f;
				break;
			case PlatformMovementType.Left:
				vector.x += -1f;
				break;
			case PlatformMovementType.Right:
				vector.x++;
				break;
			}
		}
		OnSignalChangeUpdateIndicatorsVisual();
	}

	private void OnSignalChangeUpdateIndicatorsVisual()
	{
		if (referenceToMyIndicators != null)
		{
			referenceToMyIndicators.GetComponent<PlatMoveDots>().UpdateChildIndicatorVisuals(signal_ON);
		}
	}

	private void PlaySignalToggledSounds(bool ignoreSound = false)
	{
		if (!ignoreSound)
		{
			if (signal_ON)
			{
				GC.PlaySFX(GameCore.SFXBasic2.PlatPowerOn, 0, checkForDuplicity: false, transform.position, 0f, 0f);
			}
			else
			{
				GC.PlaySFX(GameCore.SFXBasic2.PlatPowerOff, 0, checkForDuplicity: false, transform.position, 0f, 0f);
			}
		}
	}

	private void CheckStartPoweredLogic()
	{
		if (swapStartPoweredSceneName && ValidatePreviousSceneLogic())
		{
			startPowered = !startPowered;
		}
		if (!startPowered)
		{
			return;
		}
		if (timerStartPowered > 0f)
		{
			if (crtTimerStartPowered != null)
			{
				StopCoroutine(crtTimerStartPowered);
			}
			crtTimerStartPowered = StartCoroutine(waitForStartTimer(timerStartPowered));
		}
		else
		{
			signal_ON = true;
			OnSignalChangeUpdateIndicatorsVisual();
		}
	}

	private bool ValidatePreviousSceneLogic()
	{
		if (previousSceneName != "" && previousSceneName == GC.strPreviousSceneName)
		{
			return true;
		}
		return false;
	}

	private IEnumerator waitForStartTimer(float wait)
	{
		yield return new WaitForSeconds(wait);
		signal_ON = true;
		OnSignalChangeUpdateIndicatorsVisual();
		crtTimerStartPowered = null;
	}
}
