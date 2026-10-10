using System.Collections;
using UnityEngine;
using UnityEngine.U2D;

public class ControllerCamera : MonoBehaviour
{
	public enum camDirection
	{
		Up,
		Right,
		Down,
		Left,
		Custom,
		Null
	}

	public enum ZoomType
	{
		DressRipped,
		EroGrabAuto,
		Dialogue,
		PlayerKeyPress
	}

	private camDirection currentDirection;

	private int currentEroZoomLevel;

	private float startTime;

	private float moveDistance;

	private float moveDuration;

	private float timerFraction;

	private float limitMovementHorizontal;

	private float limitMovementVertical;

	private float camJumpDelay;

	private float camTimerFalling;

	private float originalOrthoSize;

	private float deadZoneUpLimit;

	private float deadZoneDownLimit;

	private float deadZoneRightPoint;

	private float deadZoneLeftPoint;

	private float deadZoneUpPoint;

	private float deadZoneDownPoint;

	private float smoothCameraSpeed;

	private float staticCameraSpeed;

	private float setAspect;

	private float shakeDuration;

	private float shakeMagnitude;

	private float shakeDamping;

	private bool isMovingCameraOffset;

	private bool isRestoringCameraOffset;

	private bool isForbidMoveRight;

	private bool isForbidMoveLeft;

	private bool isForbidMoveDown;

	private bool isForbidMoveUp;

	private bool setOnOffMoveDelay;

	private bool isApplyShake;

	private GameObject objPlayer;

	private Vector2 currentCustomMoveCamera;

	private Vector3 offsetCamera;

	private Vector3 movStartPosition;

	private Vector3 movTargetPosition;

	private Vector3 vecOrigOffset;

	private Vector3 vecRestoreOffsetStartingPos;

	private Vector3 vecTempCameraCalculation;

	private Vector3 vecShake;

	private Rigidbody2D objPlayerRB;

	private Controller_Player objPlayerSCRPT;

	private Coroutine crtSlowDownAfterTeleport;

	private Coroutine crtZoomEffect;

	private PixelPerfectCamera refScriptPPC;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		currentEroZoomLevel = 0;
		vecShake = Vector3.zero;
	}

	public void SetTheCameraStartPositionToPlayer(GameObject parPlayer)
	{
		if (parPlayer != null)
		{
			objPlayer = parPlayer;
			Vector3 position = transform.position;
			position.x = objPlayer.transform.position.x;
			position.y = objPlayer.transform.position.y;
			transform.position = position;
			offsetCamera = transform.position - objPlayer.transform.position;
			offsetCamera.y += 0.5f;
			vecOrigOffset = offsetCamera;
			refScriptPPC = GetComponent<PixelPerfectCamera>();
			float num = 2f * GetComponent<Camera>().orthographicSize;
			float num2 = num * GetComponent<Camera>().aspect;
			GetComponent<BoxCollider2D>().size = new Vector2(num2 - 3f, num);
			objPlayerRB = objPlayer.GetComponent<Rigidbody2D>();
			objPlayerSCRPT = objPlayer.GetComponent<Controller_Player>();
			camTimerFalling = 0f;
			SetupCameraMoveSpeed(primary: true);
			deadZoneUpLimit = 3.95f;
			deadZoneDownLimit = 0.1f;
			SetupCurrentDeadZonePointsInTheWorld(forceSetup: true);
		}
	}

	private void SetupCameraMoveSpeed(bool primary)
	{
		if (primary)
		{
			smoothCameraSpeed = 9f;
			staticCameraSpeed = 18f;
		}
		else
		{
			smoothCameraSpeed = 5f;
			staticCameraSpeed = 5f;
		}
	}

	public void MoveCameraFixedDirectionOffset(camDirection direction, float distance, float duration, Vector2 customMove, bool isDialogueChange = false)
	{
		if (!isMovingCameraOffset)
		{
			if (isRestoringCameraOffset)
			{
				isRestoringCameraOffset = false;
				offsetCamera = vecOrigOffset;
			}
			startTime = Time.time;
			currentDirection = direction;
			moveDistance = distance;
			moveDuration = duration;
			if (direction == camDirection.Custom && customMove != Vector2.zero)
			{
				currentCustomMoveCamera = customMove;
			}
			isMovingCameraOffset = true;
		}
		else if (isDialogueChange && direction == camDirection.Custom && customMove != Vector2.zero)
		{
			currentCustomMoveCamera = customMove;
		}
	}

	public void StopCameraFixedDirectionOffset(camDirection direction, bool instantReset = false)
	{
		if (instantReset)
		{
			isMovingCameraOffset = false;
			isRestoringCameraOffset = false;
			offsetCamera = vecOrigOffset;
			transform.position = objPlayer.transform.position + offsetCamera;
		}
		else if (isMovingCameraOffset && direction == currentDirection)
		{
			isMovingCameraOffset = false;
			isRestoringCameraOffset = true;
			vecRestoreOffsetStartingPos = offsetCamera;
			moveDuration = 0.25f;
			startTime = Time.time;
		}
	}

	public void InstantlyMoveTheCameraToPositionAfterTeleportToNewSceneBorder(Vector2 position)
	{
		SetupCameraMoveSpeed(primary: false);
		position.y += 0.5f;
		transform.position = position;
		if (crtSlowDownAfterTeleport != null)
		{
			StopCoroutine(crtSlowDownAfterTeleport);
		}
		crtSlowDownAfterTeleport = StartCoroutine(waitForSlowDownAfterEdgeTeleport(0.25f));
	}

	private IEnumerator waitForSlowDownAfterEdgeTeleport(float wait)
	{
		yield return new WaitForSeconds(wait);
		SetupCameraMoveSpeed(primary: true);
		crtSlowDownAfterTeleport = null;
	}

	private void Update()
	{
		if (isMovingCameraOffset)
		{
			timerFraction = (Time.time - startTime) / moveDuration;
			switch (currentDirection)
			{
			case camDirection.Up:
				offsetCamera = new Vector3(vecOrigOffset.x, Mathf.SmoothStep(vecOrigOffset.y, vecOrigOffset.y + moveDistance, timerFraction), vecOrigOffset.z);
				break;
			case camDirection.Right:
				offsetCamera = new Vector3(Mathf.SmoothStep(vecOrigOffset.x, vecOrigOffset.x + moveDistance, timerFraction), vecOrigOffset.y, vecOrigOffset.z);
				break;
			case camDirection.Down:
				offsetCamera = new Vector3(vecOrigOffset.x, Mathf.SmoothStep(vecOrigOffset.y, vecOrigOffset.y - moveDistance, timerFraction), vecOrigOffset.z);
				break;
			case camDirection.Left:
				offsetCamera = new Vector3(Mathf.SmoothStep(vecOrigOffset.x, vecOrigOffset.x - moveDistance, timerFraction), vecOrigOffset.y, vecOrigOffset.z);
				break;
			case camDirection.Custom:
				offsetCamera = new Vector3(Mathf.SmoothStep(vecOrigOffset.x, vecOrigOffset.x + currentCustomMoveCamera.x, timerFraction), Mathf.SmoothStep(vecOrigOffset.y, vecOrigOffset.y + currentCustomMoveCamera.y, timerFraction), vecOrigOffset.z);
				break;
			}
		}
		else if (isRestoringCameraOffset)
		{
			timerFraction = (Time.time - startTime) / moveDuration;
			offsetCamera = new Vector3(Mathf.SmoothStep(vecRestoreOffsetStartingPos.x, vecOrigOffset.x, timerFraction), Mathf.SmoothStep(vecRestoreOffsetStartingPos.y, vecOrigOffset.y, timerFraction), vecOrigOffset.z);
			if (offsetCamera == vecOrigOffset)
			{
				isRestoringCameraOffset = false;
			}
		}
	}

	private void FixedUpdate()
	{
		FXU_DoShakeCamera();
		if (objPlayer != null)
		{
			vecTempCameraCalculation = objPlayer.transform.position + offsetCamera + vecShake;
			SetupCurrentDeadZonePointsInTheWorld();
			if (GC.configCameraType == GameCore.CameraType.Smooth && !isApplyShake)
			{
				vecTempCameraCalculation = CheckIsOutsideCameraDeadZone(vecTempCameraCalculation);
			}
			if (isForbidMoveRight && limitMovementHorizontal != 0f && vecTempCameraCalculation.x > limitMovementHorizontal)
			{
				vecTempCameraCalculation.x = limitMovementHorizontal;
			}
			if (isForbidMoveLeft && limitMovementHorizontal != 0f && vecTempCameraCalculation.x < limitMovementHorizontal)
			{
				vecTempCameraCalculation.x = limitMovementHorizontal;
			}
			if (isForbidMoveUp && limitMovementVertical != 0f && vecTempCameraCalculation.y > limitMovementVertical)
			{
				vecTempCameraCalculation.y = limitMovementVertical;
			}
			if (isForbidMoveDown && limitMovementVertical != 0f && vecTempCameraCalculation.y < limitMovementVertical)
			{
				vecTempCameraCalculation.y = limitMovementVertical;
			}
			if (GC.configCameraType == GameCore.CameraType.Smooth)
			{
				transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, vecTempCameraCalculation.x, smoothCameraSpeed * Time.deltaTime), Mathf.SmoothStep(transform.position.y, vecTempCameraCalculation.y, smoothCameraSpeed * Time.deltaTime), vecTempCameraCalculation.z);
			}
			else
			{
				transform.position = new Vector3(Mathf.SmoothStep(transform.position.x, vecTempCameraCalculation.x, staticCameraSpeed * Time.deltaTime), Mathf.SmoothStep(transform.position.y, vecTempCameraCalculation.y, staticCameraSpeed * Time.deltaTime), vecTempCameraCalculation.z);
			}
		}
	}

	private void FXU_DoShakeCamera()
	{
		if (isApplyShake)
		{
			if (shakeDuration <= 0f)
			{
				isApplyShake = false;
			}
			if (isApplyShake)
			{
				vecShake = Random.insideUnitSphere * shakeMagnitude;
				shakeDuration -= Time.deltaTime * shakeDamping;
				return;
			}
			shakeDuration = 0f;
			shakeMagnitude = 0f;
			shakeDamping = 0f;
			vecShake = Vector3.zero;
		}
	}

	private void SetupCurrentDeadZonePointsInTheWorld(bool forceSetup = false)
	{
		if (CheckValidPlayerStatesForDeadZone() | forceSetup)
		{
			deadZoneUpPoint = objPlayer.transform.position.y + deadZoneUpLimit;
			deadZoneDownPoint = objPlayer.transform.position.y + deadZoneDownLimit;
			setOnOffMoveDelay = true;
		}
	}

	private bool CheckValidPlayerStatesForDeadZone()
	{
		switch (objPlayerSCRPT.currStatHack)
		{
		case "Standing":
		case "Running":
		case "Crouching":
		case "Fainted":
		case "OnLadder":
		case "OnLadderMoving":
			return true;
		default:
			return false;
		}
	}

	private Vector3 CheckIsOutsideCameraDeadZone(Vector3 tmpVec)
	{
		if (objPlayer.transform.position.y < deadZoneUpPoint && objPlayer.transform.position.y > deadZoneDownPoint)
		{
			if ((objPlayerSCRPT.currStatHack == "Jumping" && vecTempCameraCalculation.y - transform.position.y >= deadZoneUpLimit + 1f) || GC.playerCurrentSkill == "FlameSword" || GC.playerCurrentSkill == "DrillDash")
			{
				tmpVec.y = objPlayer.transform.position.y;
				setOnOffMoveDelay = false;
			}
			else if (setOnOffMoveDelay)
			{
				tmpVec.y = transform.position.y;
			}
		}
		if (objPlayerRB.velocity.x > 0.1f)
		{
			tmpVec.x += 2.5f;
		}
		else if (objPlayerRB.velocity.x < -0.1f)
		{
			tmpVec.x -= 2.5f;
		}
		if (CHECK_FallingConditionsDZ())
		{
			camTimerFalling += Time.unscaledDeltaTime;
			if (GC.playerCurrentSkill == "GroundPound" && camTimerFalling >= 0.2f)
			{
				tmpVec.y -= 5f;
			}
			else if (camTimerFalling >= 0.4f)
			{
				tmpVec.y -= 2f;
			}
		}
		else
		{
			camTimerFalling = 0f;
		}
		return tmpVec;
	}

	private bool CHECK_FallingConditionsDZ()
	{
		if (objPlayerSCRPT.currStatHack == "Falling" && GC.playerCurrentAttackState != "JumpAtk" && GC.playerCurrentAttackState != "UpwardsType2" && GC.playerCurrentSkill != "Shield" && GC.playerCurrentSkill != "FlameSword" && GC.playerCurrentSkill != "ForwardSlash" && GC.playerCurrentSkill != "DrillDash" && GC.playerCurrentSkill != "RepairAllClothingLayer" && GC.playerCurrentAnimState != "Player-ThrowA_Jump")
		{
			return true;
		}
		return false;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (ReturnID_OfBorder(other.name))
		{
		case 1:
			isForbidMoveRight = true;
			limitMovementHorizontal = transform.position.x;
			break;
		case 2:
			isForbidMoveLeft = true;
			limitMovementHorizontal = transform.position.x;
			break;
		case 3:
			isForbidMoveUp = true;
			limitMovementVertical = transform.position.y;
			break;
		case 4:
			isForbidMoveDown = true;
			limitMovementVertical = transform.position.y;
			break;
		default:
			Debug.Log("[CAMERA]OnTriggerEnter2D: Could not find border ID! Camera may become behave weirdly!");
			break;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		switch (ReturnID_OfBorder(other.name))
		{
		case 1:
			isForbidMoveRight = false;
			limitMovementHorizontal = 0f;
			break;
		case 2:
			isForbidMoveLeft = false;
			limitMovementHorizontal = 0f;
			break;
		case 3:
			isForbidMoveUp = false;
			limitMovementVertical = 0f;
			break;
		case 4:
			isForbidMoveDown = false;
			limitMovementVertical = 0f;
			break;
		default:
			Debug.Log("[CAMERA]OnTriggerExit2D: Could not find border ID! Camera may become stuck!");
			break;
		}
	}

	private int ReturnID_OfBorder(string name)
	{
		if (name.Contains("BorderRight"))
		{
			return 1;
		}
		if (name.Contains("BorderLeft"))
		{
			return 2;
		}
		if (name.Contains("BorderUp"))
		{
			return 3;
		}
		if (name.Contains("BorderDown"))
		{
			return 4;
		}
		return 0;
	}

	public void InitiateZoom(ZoomType type, bool hasTimeLimit, float timeLimit)
	{
		bool flag = false;
		switch (type)
		{
		case ZoomType.DressRipped:
			if (!GC.canApplySlowZoomEffectOnDressBreak)
			{
				ManualEndZoomEffect();
				return;
			}
			refScriptPPC.valueZoomSizeMultiply = 0.7f;
			flag = true;
			break;
		case ZoomType.EroGrabAuto:
			if (!CheckCurrentEroZoomLevel(checkForFaintedToCancelAutoZoomChangeOnGrab: true, GC.doRememberLastZoomAmount))
			{
				ManualEndZoomEffect();
				return;
			}
			break;
		case ZoomType.PlayerKeyPress:
			if (!CheckCurrentEroZoomLevel())
			{
				ManualEndZoomEffect();
				return;
			}
			break;
		case ZoomType.Dialogue:
			refScriptPPC.valueZoomSizeMultiply = 0.8f;
			flag = true;
			break;
		}
		refScriptPPC.valueZoomSpeed = 2.5f;
		if ((currentEroZoomLevel != 0) | flag)
		{
			refScriptPPC.isZoomEffect = true;
		}
		if (hasTimeLimit)
		{
			if (crtZoomEffect != null)
			{
				StopCoroutine(crtZoomEffect);
			}
			crtZoomEffect = StartCoroutine(waitForZoomAndReset(timeLimit));
		}
	}

	public void ManualEndZoomEffect(bool checkForFaintedToCancelAutoZoomChangeOnGrab = false)
	{
		if ((!checkForFaintedToCancelAutoZoomChangeOnGrab || !GC.isPlayerFainted) && (!GC.isRunningBadEndLogic || !GC.isDisplayingDialogue || !GC.isPlayerFainted))
		{
			refScriptPPC.isZoomEffect = false;
			refScriptPPC.valueZoomSizeMultiply = 1f;
			currentEroZoomLevel = 0;
			GC.currentEroZoomLevel = currentEroZoomLevel;
		}
	}

	private bool CheckCurrentEroZoomLevel(bool checkForFaintedToCancelAutoZoomChangeOnGrab = false, bool rememberLastZoomLevel = false)
	{
		if (checkForFaintedToCancelAutoZoomChangeOnGrab && GC.isPlayerFainted)
		{
			return true;
		}
		if (rememberLastZoomLevel && GC.intLastZoomLevel != 0)
		{
			ApplyEffectiveZoomLevel(GC.intLastZoomLevel);
		}
		else
		{
			CycleZoomLevelToNext();
		}
		GC.currentEroZoomLevel = currentEroZoomLevel;
		if (currentEroZoomLevel != 0)
		{
			GC.intLastZoomLevel = currentEroZoomLevel;
			return true;
		}
		return false;
	}

	private void CycleZoomLevelToNext()
	{
		switch (currentEroZoomLevel)
		{
		case 0:
			ApplyEffectiveZoomLevel(1);
			break;
		case 1:
			ApplyEffectiveZoomLevel(2);
			break;
		case 2:
			ApplyEffectiveZoomLevel(3);
			break;
		case 3:
			ApplyEffectiveZoomLevel(4);
			break;
		default:
			ApplyEffectiveZoomLevel(0);
			break;
		}
	}

	private void ApplyEffectiveZoomLevel(int levelToApply)
	{
		switch (levelToApply)
		{
		default:
			currentEroZoomLevel = 0;
			refScriptPPC.valueZoomSizeMultiply = 1f;
			GC.DisplaySystemMessageUI(2, 53);
			break;
		case 1:
			currentEroZoomLevel = 1;
			refScriptPPC.valueZoomSizeMultiply = 0.8f;
			GC.DisplaySystemMessageUI(2, 56);
			break;
		case 2:
			currentEroZoomLevel = 2;
			refScriptPPC.valueZoomSizeMultiply = 0.65f;
			GC.DisplaySystemMessageUI(2, 57);
			break;
		case 3:
			currentEroZoomLevel = 3;
			refScriptPPC.valueZoomSizeMultiply = 0.5f;
			GC.DisplaySystemMessageUI(2, 58);
			break;
		case 4:
			currentEroZoomLevel = 4;
			refScriptPPC.valueZoomSizeMultiply = 0.4f;
			GC.DisplaySystemMessageUI(2, 156);
			break;
		}
	}

	private IEnumerator waitForZoomAndReset(float wait)
	{
		yield return new WaitForSeconds(wait);
		ManualEndZoomEffect();
		crtZoomEffect = null;
	}

	public void ShakeTheCamera(float duration, float magnitude, float damping)
	{
		if (!isApplyShake)
		{
			if (GC.configCameraType == GameCore.CameraType.Smooth)
			{
				magnitude *= 2f;
			}
			shakeDuration = duration;
			shakeMagnitude = magnitude;
			shakeDamping = damping;
			isApplyShake = true;
		}
	}
}
