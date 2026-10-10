using System.Collections;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
	public bool isLocalPortal;

	public string targetScene;

	public Vector2 targetPosition;

	public Vector2 offsetCameraAfterTP;

	public bool forceMovePlayerAfter;

	public bool lockPlayerInput;

	public bool doFade;

	public Color fadeColor;

	public float floatFadeSpeed;

	public bool delayDuringSceneLoad;

	public bool delayDuringLocalTP;

	public float timeDelay;

	public bool allowVerticalMovementOnTouch;

	public bool playMayaAnimEntering;

	private GameCore GC;

	private Coroutine crtDelayDuringSceneLoad;

	private Coroutine crtDelayDuringLocalTP;

	private void Start()
	{
		GC = GameCore.Instance;
		if (delayDuringSceneLoad)
		{
			if (timeDelay <= 0f)
			{
				timeDelay = 1f;
			}
			crtDelayDuringSceneLoad = StartCoroutine(waitDelayDuringLoad(timeDelay));
		}
		floatFadeSpeed = GC.globalDefaultSceneFadeTimer;
	}

	public void DoPlayerPortal(Controller_Player refPlayer = null)
	{
		if (GC.playerCanUseTeleport && !GC.isSceneTransitioning && !delayDuringSceneLoad)
		{
			if (allowVerticalMovementOnTouch)
			{
				GC.isAllowVerticalMovementDuringSceneLoad = true;
			}
			if ((bool)refPlayer)
			{
				refPlayer.SetInvincibleFromPortalScript();
			}
			GC.LoadNextScene(targetScene, targetPosition, offsetCameraAfterTP, forceMovePlayerAfter, doFade, fadeColor, floatFadeSpeed, lockPlayerInput, persistPlayerGameSessionValues: true);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (!(other.gameObject.name == "[PLAYER]"))
		{
			return;
		}
		if (!isLocalPortal)
		{
			DoPlayerPortal(other.gameObject.GetComponent<Controller_Player>());
		}
		else if (!delayDuringLocalTP)
		{
			other.gameObject.GetComponent<Controller_Player>().TeleportSameLevelPortal(GetComponent<PortalSameLevel>());
			delayDuringLocalTP = true;
			if (crtDelayDuringLocalTP != null)
			{
				StopCoroutine(crtDelayDuringLocalTP);
			}
			crtDelayDuringLocalTP = StartCoroutine(waitDelayDuringLocalTP(1f));
		}
	}

	private IEnumerator waitDelayDuringLoad(float wait)
	{
		yield return new WaitForSeconds(wait);
		delayDuringSceneLoad = false;
		crtDelayDuringSceneLoad = null;
	}

	private IEnumerator waitDelayDuringLocalTP(float wait)
	{
		yield return new WaitForSeconds(wait);
		delayDuringLocalTP = false;
		crtDelayDuringSceneLoad = null;
	}
}
