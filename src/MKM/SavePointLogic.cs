using System.Collections;
using UnityEngine;

public class SavePointLogic : MonoBehaviour
{
	public GameCore.SavePieceType currentPieceType;

	public int idScreenshot;

	public GameObject objSaveEffect;

	public float SavingEffectDuration;

	public bool debugEffect;

	private string currentCollision;

	private bool isCollidingWithPlayer;

	private bool isThisPointAllowedToAutoSave;

	private Coroutine crtSavingEffect;

	private Coroutine crtDelayLeaveCollision;

	private Coroutine crtDelayAutoSave;

	private Coroutine crtDelaySaveAfterSceneLoad;

	private Swap2DShader ref2DShader;

	private SpriteRenderer objRE;

	private Animator objAN;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		objAN = GetComponent<Animator>();
		objRE = GetComponent<SpriteRenderer>();
		ref2DShader = GetComponent<Swap2DShader>();
		crtDelayLeaveCollision = null;
		currentCollision = "";
		crtSavingEffect = null;
		isCollidingWithPlayer = false;
		isThisPointAllowedToAutoSave = false;
		PlayContextAnimation();
		if (debugEffect && (bool)ref2DShader)
		{
			ref2DShader.Toggle2DShader(stateON: true, 0);
		}
		if (crtDelaySaveAfterSceneLoad != null)
		{
			StopCoroutine(crtDelaySaveAfterSceneLoad);
		}
		crtDelaySaveAfterSceneLoad = StartCoroutine(waitToAllowAutoSaveInThisPoint(1f));
	}

	private void PlayContextAnimation()
	{
		string stateName = "";
		switch (currentPieceType)
		{
		case GameCore.SavePieceType.Pawn:
			stateName = "Save-IdlePawn";
			break;
		case GameCore.SavePieceType.Queen:
			stateName = "Save-IdleQueen";
			break;
		case GameCore.SavePieceType.Rook:
			stateName = "Save-IdleRook";
			break;
		case GameCore.SavePieceType.Knight:
			stateName = "Save-IdleKnight";
			break;
		case GameCore.SavePieceType.Bishop:
			stateName = "Save-IdleBishop";
			break;
		case GameCore.SavePieceType.Crystal:
			stateName = "Save-IdleCrystal";
			break;
		}
		objAN.Play(stateName);
	}

	private void Update()
	{
		if (isCollidingWithPlayer && GC.savingPlayerData && crtSavingEffect == null)
		{
			crtSavingEffect = StartCoroutine(waitForSavingEffect(SavingEffectDuration));
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (!(other.gameObject.tag == "Player"))
		{
			return;
		}
		currentCollision = other.ToString();
		if (!isCollidingWithPlayer)
		{
			GC.idCurrentSavePointScreenshot = idScreenshot;
			GC.AddNewVisitedSavePoint(idScreenshot, isDebug: false);
			isCollidingWithPlayer = true;
			ref2DShader.Toggle2DShader(stateON: true, 0);
			if (GC.doAutoSaveFeature && crtDelayAutoSave == null && !GC.playerIsHurt && !GC.isGamePaused && !GC.isPlayerFainted && !GC.playerIsBeingGrabed)
			{
				crtDelayAutoSave = StartCoroutine(waitDelayedAutoSave(0.15f));
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			currentCollision = "";
			if (crtDelayLeaveCollision != null)
			{
				StopCoroutine(crtDelayLeaveCollision);
			}
			crtDelayLeaveCollision = StartCoroutine(waitForDelayLeaveCollision(0.33f));
		}
	}

	private IEnumerator waitForDelayLeaveCollision(float wait)
	{
		yield return new WaitForSeconds(wait);
		if (currentCollision == "" && isCollidingWithPlayer)
		{
			isCollidingWithPlayer = false;
			if ((bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: false, 0);
			}
		}
		crtDelayLeaveCollision = null;
	}

	private IEnumerator waitForSavingEffect(float wait)
	{
		if ((bool)objSaveEffect)
		{
			objSaveEffect.SetActive(value: true);
		}
		yield return new WaitForSeconds(wait);
		if ((bool)objSaveEffect)
		{
			objSaveEffect.SetActive(value: false);
		}
		GC.savingPlayerData = false;
		crtSavingEffect = null;
	}

	private IEnumerator waitDelayedAutoSave(float wait)
	{
		yield return new WaitForSeconds(wait);
		if (!GC.savingPlayerData && isThisPointAllowedToAutoSave)
		{
			GC.SaveGameData(0);
			GC.DisplaySystemMessageUI(0, 59);
		}
		crtDelayAutoSave = null;
	}

	private IEnumerator waitToAllowAutoSaveInThisPoint(float wait)
	{
		yield return new WaitForSeconds(wait);
		isThisPointAllowedToAutoSave = true;
		crtDelaySaveAfterSceneLoad = null;
	}
}
