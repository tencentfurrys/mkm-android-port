using System.Collections;
using UnityEngine;

public class PortalSameLevel : MonoBehaviour
{
	public enum TransitionVFX
	{
		None = 0,
		Basic = 1,
		Mirror = 2,
		InstantIN = 3
	}

	public Transform targetPos;

	public Vector3 offsetTargetPos;

	public Vector2 offsetCamera;

	public float delayTeleport;

	public TransitionVFX vfxTeleport;

	public int forceMovePlayerAfterTP;

	public bool playMayaAnimEntering;

	public bool storeRetryBeforeTP;

	private GameObject objPlayer;

	private GameObject objCamera;

	private Controller_Player scriptPlayer;

	private ControllerCamera scriptCamera;

	private GameCore GC;

	private Coroutine crtTeleportToPosition;

	private void Start()
	{
		GC = GameCore.Instance;
	}

	public void TeleportPlayerToTargetPosition()
	{
		Vector2 tmpTargetWithOffset = targetPos.position + offsetTargetPos;
		if (GetObjectReferences())
		{
			DisplayTransitionVFX();
			if (crtTeleportToPosition != null)
			{
				StopCoroutine(crtTeleportToPosition);
			}
			crtTeleportToPosition = StartCoroutine(waitTeleportToPosition(delayTeleport, tmpTargetWithOffset));
		}
	}

	private void DisplayTransitionVFX()
	{
		switch (vfxTeleport)
		{
		case TransitionVFX.Basic:
			GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 0.5f);
			GC.isPlayerInteractingLocked = true;
			break;
		case TransitionVFX.Mirror:
			if ((bool)GC)
			{
				GC.DoMirrorEffectImageUI(1f);
				GC.PlaySFX(GameCore.SFXBasic1.EnterMirror, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			}
			break;
		case TransitionVFX.InstantIN:
			GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 0.0001f);
			GC.isPlayerInteractingLocked = true;
			break;
		}
	}

	private bool GetObjectReferences()
	{
		if (objPlayer == null)
		{
			objPlayer = GameObject.FindGameObjectWithTag("Player");
		}
		if (objCamera == null)
		{
			objCamera = GameObject.FindGameObjectWithTag("MainCamera");
		}
		if (!objPlayer || !objCamera)
		{
			Debug.Log("Could not find the gameobjects of the PLAYER and/or CAMERA!!! Aborting...");
			return false;
		}
		if (scriptPlayer == null)
		{
			scriptPlayer = objPlayer.GetComponent<Controller_Player>();
		}
		if (scriptCamera == null)
		{
			scriptCamera = objCamera.GetComponent<ControllerCamera>();
		}
		if (!scriptPlayer || !scriptCamera)
		{
			Debug.Log("Could not get reference to script PLAYER and/or CAMERA!!! Aborting...");
			return false;
		}
		return true;
	}

	private IEnumerator waitTeleportToPosition(float wait, Vector2 tmpTargetWithOffset)
	{
		if ((bool)GC && storeRetryBeforeTP)
		{
			GC.StoreRetryGameCore(objPlayer.transform.position.x, objPlayer.transform.position.y, "PORTAL_SAME_LEVEL-waitTeleportPortal", ignoreCameraOffset: true);
		}
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: true);
		yield return new WaitForSeconds(wait);
		objPlayer.transform.position = tmpTargetWithOffset;
		if (offsetCamera != Vector2.zero)
		{
			tmpTargetWithOffset += offsetCamera;
		}
		scriptCamera.InstantlyMoveTheCameraToPositionAfterTeleportToNewSceneBorder(tmpTargetWithOffset);
		if (forceMovePlayerAfterTP != 0)
		{
			scriptPlayer.ForceMovePlayerLeftRight(forceMovePlayerAfterTP, 0.1f);
		}
		if ((bool)GC && !storeRetryBeforeTP)
		{
			GC.StoreRetryGameCore(tmpTargetWithOffset.x, tmpTargetWithOffset.y, "PORTAL_SAME_LEVEL-waitTeleportPortal", ignoreCameraOffset: true);
		}
		GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 0.5f);
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: false);
		GC.isPlayerInteractingLocked = false;
		crtTeleportToPosition = null;
	}
}
