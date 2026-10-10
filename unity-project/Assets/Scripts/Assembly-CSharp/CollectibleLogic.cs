using System.Collections;
using UnityEngine;

public class CollectibleLogic : MonoBehaviour
{
	public GameCore.CollectibleType currentItemType;

	public int itemID;

	public int forcedQuantity;

	public int quartzValue;

	public float healValuePercent;

	public float selfDisableTimer;

	public bool isUniquePickup;

	public bool isMoveSpawnRandom;

	public Vector3 vecMoveSpawnMaxRandom;

	public bool smoothMoveSpawnRNG;

	public bool isPingPongUpDown;

	public bool isApplyShaderEffect;

	public bool isDelayColliderActivation;

	public float delayCollTime;

	public float smoothStepSpeed;

	public float rangePingPong;

	public float speedPingPong;

	public bool NoPlaySkillSpawnSFX;

	[HideInInspector]
	public Vector3 originalPos;

	[HideInInspector]
	public bool orbEssenceChasePlayer;

	private Coroutine crtDelayColliderActivation;

	private Coroutine crtDelayObjectDeactivation;

	private Coroutine crtSelfDisable;

	private bool isMovingToPoint;

	private float rngEssenceOffsetX;

	private float rngEssenceOffsetY;

	private BoxCollider2D objBox2D;

	private SpriteRenderer objRE;

	private Swap2DShader ref2DShader;

	private GameCore GC;

	private void Start()
	{
		InitializeVariables();
		if (!CheckIfThisUniqueItemWasAlreadyPickedUp())
		{
			InitialMovement();
			if (isDelayColliderActivation)
			{
				DelayColliderActivation();
			}
			else
			{
				objBox2D.enabled = true;
			}
			if (isApplyShaderEffect && (bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: true, 0);
			}
			if (selfDisableTimer > 0f)
			{
				if (base.gameObject.tag == "Item-Essence" && (bool)GC)
				{
					selfDisableTimer = GC.MATH_RNG_FLOAT(selfDisableTimer - 2.5f, selfDisableTimer + 2.5f);
				}
				crtSelfDisable = StartCoroutine(waitSelfDisable(selfDisableTimer));
			}
		}
		else
		{
			ItemDisableBehaviour();
		}
	}

	private void InitializeVariables()
	{
		GC = GameCore.Instance;
		objRE = GetComponent<SpriteRenderer>();
		ref2DShader = GetComponent<Swap2DShader>();
		objBox2D = GetComponent<BoxCollider2D>();
		isMovingToPoint = false;
		originalPos.x = Mathf.Round(base.transform.position.x * 100f) / 100f;
		originalPos.y = Mathf.Round(base.transform.position.y * 100f) / 100f;
		if (isDelayColliderActivation && delayCollTime <= 0f)
		{
			delayCollTime = 1f;
		}
		rngEssenceOffsetX = GC.MATH_RNG_FLOAT(-0.15f, 0.15f);
		rngEssenceOffsetY = rngEssenceOffsetX;
	}

	private void InitialMovement()
	{
		if (currentItemType == GameCore.CollectibleType.KeyItem)
		{
			MoveUpwardsFromSpawn();
			GC.PlaySFX(GameCore.SFXBasic2.UniqueItemAppear1, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		}
		else if (isMoveSpawnRandom && (vecMoveSpawnMaxRandom.x > 0f || vecMoveSpawnMaxRandom.y > 0f))
		{
			MoveToRandomPointFromSpawn();
		}
	}

	private void Update()
	{
		if (orbEssenceChasePlayer)
		{
			UPDT_ORBCHASEPLAYER();
		}
		else if (isMovingToPoint)
		{
			if (smoothMoveSpawnRNG)
			{
				base.transform.position = new Vector3(Mathf.Lerp(base.transform.position.x, vecMoveSpawnMaxRandom.x, smoothStepSpeed * Time.deltaTime), Mathf.Lerp(base.transform.position.y, vecMoveSpawnMaxRandom.y, smoothStepSpeed * Time.deltaTime), originalPos.z);
				return;
			}
			float maxDistanceDelta = 1f * Time.deltaTime;
			base.transform.position = Vector3.MoveTowards(base.transform.position, vecMoveSpawnMaxRandom, maxDistanceDelta);
			if (base.transform.position == vecMoveSpawnMaxRandom)
			{
				isMovingToPoint = false;
				if (currentItemType == GameCore.CollectibleType.KeyItem)
				{
					base.gameObject.GetComponent<BoxCollider2D>().enabled = true;
				}
			}
		}
		else if (isPingPongUpDown)
		{
			base.transform.position = new Vector3(originalPos.x, Mathf.SmoothStep(base.transform.position.y, originalPos.y + Mathf.PingPong(Time.time / speedPingPong, rangePingPong) - rangePingPong / 2f, smoothStepSpeed * Time.deltaTime), originalPos.z);
		}
	}

	private void UPDT_ORBCHASEPLAYER()
	{
		if (!GC.isPlayerFainted && !GC.isSceneTransitioning)
		{
			Vector3 vector = new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition - 0.25f, base.transform.position.z);
			vector.x += rngEssenceOffsetX;
			vector.y += rngEssenceOffsetY;
			base.transform.position = new Vector3(Mathf.LerpAngle(base.transform.position.x, vector.x, smoothStepSpeed * Time.deltaTime), Mathf.LerpAngle(base.transform.position.y, vector.y, smoothStepSpeed * Time.deltaTime), vector.z);
			smoothStepSpeed += 0.75f;
			if (GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 0.5f, 0.5f))
			{
				GC.TouchedEssenceCollectible(this);
				ItemDisableBehaviour();
			}
		}
	}

	private bool CheckIfThisUniqueItemWasAlreadyPickedUp()
	{
		if (isUniquePickup && (bool)GC && GC.CheckPlayerHasItem(itemID, GC.strCurrentActiveSceneName, originalPos, base.gameObject.name))
		{
			return true;
		}
		return false;
	}

	public void ItemDisableBehaviour(bool delayDeactivation = false, float delayTime = 0f)
	{
		StopAllCoroutines();
		CancelInvoke();
		if (currentItemType == GameCore.CollectibleType.SkillPickup)
		{
			GC.StopSFXBasic2(GameCore.SFXBasic2.SkillAppear1);
		}
		if (currentItemType == GameCore.CollectibleType.KeyItem)
		{
			GC.StopSFXBasic2(GameCore.SFXBasic2.UniqueItemAppear1);
		}
		if (delayDeactivation)
		{
			if (crtDelayObjectDeactivation != null)
			{
				StopCoroutine(crtDelayObjectDeactivation);
			}
			crtDelayObjectDeactivation = StartCoroutine(waitDelayDeactivation(delayTime));
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	private IEnumerator waitDelayDeactivation(float wait)
	{
		objBox2D.enabled = false;
		objRE.sortingOrder = 20;
		yield return new WaitForSeconds(wait);
		base.gameObject.SetActive(value: false);
		crtDelayObjectDeactivation = null;
	}

	private void MoveToRandomPointFromSpawn()
	{
		float num = Random.Range(0f - vecMoveSpawnMaxRandom.x, vecMoveSpawnMaxRandom.x);
		float num2 = Random.Range(0f - vecMoveSpawnMaxRandom.y, vecMoveSpawnMaxRandom.y);
		vecMoveSpawnMaxRandom = base.transform.position;
		vecMoveSpawnMaxRandom.x += num;
		vecMoveSpawnMaxRandom.y += num2;
		isMovingToPoint = true;
	}

	private void MoveUpwardsFromSpawn()
	{
		vecMoveSpawnMaxRandom.x = base.transform.position.x;
		vecMoveSpawnMaxRandom.y += base.transform.position.y + 1f;
		isMovingToPoint = true;
	}

	private void DelayColliderActivation()
	{
		if (currentItemType == GameCore.CollectibleType.Quartz && GC.playerIsBeingGrabed)
		{
			delayCollTime *= 2f;
		}
		if (crtDelayColliderActivation != null)
		{
			StopCoroutine(crtDelayColliderActivation);
		}
		crtDelayColliderActivation = StartCoroutine(waitColliderActivation(delayCollTime));
	}

	public void SignalThisEssenceOrbAttraction()
	{
		orbEssenceChasePlayer = true;
	}

	private IEnumerator waitColliderActivation(float wait)
	{
		yield return new WaitForSeconds(wait);
		objBox2D.enabled = true;
		crtDelayColliderActivation = null;
		if (currentItemType == GameCore.CollectibleType.Quartz && GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 5f, 5f))
		{
			orbEssenceChasePlayer = true;
		}
	}

	private IEnumerator waitSelfDisable(float wait)
	{
		yield return new WaitForSeconds(wait);
		if ((bool)GC)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.ExplosionType2, base.transform.position, "ItemLogic");
		}
		base.gameObject.SetActive(value: false);
	}
}
