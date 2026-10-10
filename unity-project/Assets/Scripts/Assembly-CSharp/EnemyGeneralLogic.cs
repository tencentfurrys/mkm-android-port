using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneralLogic : MonoBehaviour
{
	public enum faintedChaseType
	{
		Basic = 0,
		Ranged = 1,
		Flying = 2
	}

	private enum enemyState
	{
		Standing = 0,
		Patrolling = 1,
		Jumping = 2,
		Falling = 3,
		Crouching = 4,
		Chasing = 5
	}

	private enum enemyFlyingState
	{
		Idle = 0,
		Patrolling = 1,
		Wandering = 2,
		Chasing = 3,
		Retreating = 4,
		Landed = 5,
		SpecialAction = 6
	}

	private enum flashType
	{
		Null = 0,
		Transparent = 1,
		HalfWhite = 2
	}

	private enum BossState
	{
		Idle = 0,
		Battle = 1
	}

	private enum BoxCollAction
	{
		Reset = 0,
		ChangeSize = 1,
		ChangeOffset = 2,
		ChangeBoth = 3
	}

	private enum BDEDiagBehavior
	{
		StayOnAnimationPhase = 0,
		StallCoroutine = 1
	}

	private enum EmojiType
	{
		Null = 0,
		Love = 1,
		Happy = 2,
		Blushing = 3,
		Angry = 4,
		Exclamation = 5,
		Sweat = 6,
		Laugh = 7,
		LickTongue = 8
	}

	public GameCore.EnemyType currentEnemyType;

	public bool isStatic;

	public bool isImmuneToStagger;

	public bool canSpriteFlash;

	public bool canDoTouchDamage;

	public ENM_BaseStats enmBaseStats;

	public ENM_CommonBehaviour enmCmnBehaviour;

	public ENM_CommonSkill enmCmnSkill;

	public ENM_ObjectReferences enmObjRefs;

	public ENM_Miscellaneuss enmMisc;

	public ENM_EroBaseConfig eroCore;

	public faintedChaseType typeOfFaintedChase;

	[HideInInspector]
	public bool isDeath;

	[HideInInspector]
	public bool isIgnoreCollPlayer;

	public float ignoreCollTimePeriod;

	[HideInInspector]
	public bool isGalleryView;

	[HideInInspector]
	public bool isAddComboGalleryView;

	[HideInInspector]
	public int intGalleryVariantEroID;

	[HideInInspector]
	public bool isArenaGallerySpecial;

	[HideInInspector]
	public bool arenaHasPlayedPreferedEroID;

	[HideInInspector]
	public bool isGauntletSpecial;

	[HideInInspector]
	public bool shouldApplyMiniRage;

	[HideInInspector]
	public bool isStagger;

	[HideInInspector]
	public int arenaPreferedEroID;

	private Material matSpriteFlash;

	private GameCore.DifficultyType localDifficulty;

	private enemyState currentEnemyState;

	private enemyFlyingState currentFlyingState;

	private flashType currentFlashType;

	private BossState currentBossState;

	private static float defaultEnemyStaggerTime = 0.5f;

	private bool isUsingSkill;

	private bool isGrabbing;

	private bool isChasingFainted;

	private bool canChaseFainted;

	private bool isEnraged;

	private bool canBossUseEroAttractSkill;

	private bool isUsingEroAttractSkill;

	private bool localBadEndFlag;

	private bool eroCount10;

	private bool _isRagePepper;

	private bool isSkillDelay;

	private bool isJumpDelay;

	private bool isMoveToPoint;

	private bool invertSpriteFacing;

	private bool hasPlayedIdleSound;

	private bool flySpecialRotation;

	private bool boolGeneric;

	private bool wasVoreEroToggledToFatal;

	private bool hasSkippedToBadEnd;

	private bool boolFinal_BE_Message;

	private bool _isRageGauntlet;

	private string strInvokeFaintedChaseName;

	private string strIVKEroTotalTimeCount;

	private float enemyCurrentXMove;

	private float enemyCurrentYMove;

	private float countTimeToUseSkill;

	private float countFrequency;

	private float minChaseDelay;

	private float maxChaseDelay;

	private float eroGracePeriod;

	private float prevSkillTime;

	private float rotationZ;

	private float floatLandAgainDelay;

	private float localSpeedX;

	private float localSpeedY;

	private float floatGeneric;

	private float floatDotCountPoison;

	private int limitIdle;

	private int limitRun;

	private int originalTouchDamage;

	private int amountLife;

	private int currentDefense;

	private int countFlipChanged;

	private int dmgOfEro;

	private int countGenericStuff;

	private int prevAttackID;

	private int prevEroSceneVariantID;

	private int _eroPhase;

	private int intGeneric;

	private int intBadEndLocalLogicID;

	private int countBEStuff;

	private int countEroTotalTime;

	private List<int> listOfValidEroSceneVariantsID;

	private List<GameObject> listOfExternalAttacks;

	private GameObject[] arrayEroVoreFatalTempHoldPreviousReferences;

	private float timeToTriggerNextSkill;

	private int sequenceOfBossSkill;

	private int phaseOfBossFight;

	private Vector3 preShakePos;

	private Quaternion preShakeRot;

	private float shakeDecayRate;

	private float shakeIntensity;

	private EroAnimation localEroScript;

	private Vector2 offsetStruggleBarPosit;

	private GameObject tmpObjPlayerReference;

	private GameObject objGeneric;

	private Color colorOriginalMaterial;

	private Color colorFlashingMaterial;

	private Color colorFadeOutLifeBar;

	private Color colorGeneric;

	private Coroutine coroutExecuteSkill;

	private Coroutine coroutStagger;

	private Coroutine coroutEroGracePeriod;

	private Coroutine crtChaseFainted;

	private Coroutine crtChaseFaintedDelay;

	private Coroutine crtEnemyWasHitByPlayerAttack;

	private Coroutine crtDamageFlashing;

	private Coroutine crtDisplayHealthBar;

	private Coroutine crtResetBossSkilLAnim;

	private Coroutine crtResetEnragedFlag;

	private Coroutine crtFinishBossDeathAnimation;

	private Coroutine crtWaitBossCastEroAttractSkill;

	private Coroutine crtGeneric;

	private Coroutine crtJumpDelay;

	private Coroutine crtWaitBossSequence;

	private Coroutine crtStartLogicDelay;

	private Coroutine crtBadEnd;

	private Vector2 vecEnemyMovement;

	private Vector2 vecOriginalColliderOffset;

	private Vector2 vecOriginalColliderSize;

	private Vector2 vecLastStoredPos;

	private Vector2 chaseTarget;

	private Vector3 enemySetScale;

	private Vector3 posGenericVector;

	private Vector3 vecStartingPosition;

	private ENM_BaseStats defaultBaseStats;

	private ENM_CommonSkill defaultSkillStats;

	private GameCore.VisualEffect trailEffectVFX;

	private Swap2DShader ref2DShader;

	private Controller_Player playerScript;

	private SpriteRenderer playerRenderer;

	private BoxCollider2D playerBoxCollider;

	private PolygonCollider2D playerPolyCollider;

	private BoxCollider2D objBox2D;

	private Rigidbody2D objRB;

	private Rigidbody2D objMovePlatRB;

	private SpriteRenderer objRE;

	private Animator objAN;

	private Animator subAN;

	private GameCore GC;

	private MaintainRotation MROT;

	private void Start()
	{
		GC = GameCore.Instance;
		crtStartLogicDelay = StartCoroutine(waitStartLogicDelay(1f));
		InitializeVariables();
		ReplaceDefaultBehavioralValues();
		InitializeEnemyMainBehaviour();
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerEnemies"), LayerMask.NameToLayer("LayerEnemies"));
		CheckIfThisEnemyCanRemainActiveOnStart();
	}

	private void InitializeVariables()
	{
		objRB = GetComponent<Rigidbody2D>();
		objRE = GetComponent<SpriteRenderer>();
		objAN = GetComponent<Animator>();
		localEroScript = GetComponent<EroAnimation>();
		ref2DShader = GetComponent<Swap2DShader>();
		matSpriteFlash = objRE.material;
		objBox2D = base.gameObject.GetComponent<BoxCollider2D>();
		if (isStatic)
		{
			currentEnemyState = enemyState.Standing;
		}
		else
		{
			currentEnemyState = enemyState.Falling;
		}
		if (enmMisc.isFlyer && !enmBaseStats.isBoss)
		{
			MROT = enmObjRefs.localHPCanvas.GetComponent<MaintainRotation>();
		}
		EASYMODE_StoreDefaults();
		EASYMODE_ApplyChanges(forceInitialConfig: true);
		currentBossState = BossState.Idle;
		vecEnemyMovement = Vector2.zero;
		posGenericVector = Vector3.zero;
		vecStartingPosition = base.transform.position;
		vecLastStoredPos = vecStartingPosition;
		vecOriginalColliderOffset = objBox2D.offset;
		vecOriginalColliderSize = objBox2D.size;
		offsetStruggleBarPosit = Vector2.zero;
		currentFlashType = flashType.Null;
		colorOriginalMaterial = new Color(1f, 1f, 1f, 0f);
		colorFlashingMaterial = new Color(1f, 1f, 1f, 0.5f);
		enemySetScale = base.transform.localScale;
		amountLife = enmBaseStats.maxLife;
		currentDefense = enmBaseStats.baseDefense;
		strInvokeFaintedChaseName = "CHASE_InvokedCheckFaintedChaseLogic";
		strIVKEroTotalTimeCount = "ERO_IVK_CountEroTotalTime";
		if (enmMisc.rateHorizontalFriction <= 0f)
		{
			enmMisc.rateHorizontalFriction = 0.1f;
		}
		if (enmMisc.rateVerticalFriction <= 0f)
		{
			enmMisc.rateVerticalFriction = 0.05f;
		}
		preShakeRot = base.transform.rotation;
		rotationZ = 0f;
		enemyCurrentXMove = 0f;
		enemyCurrentYMove = 0f;
		isUsingSkill = false;
		isStagger = false;
		isDeath = false;
		isGrabbing = false;
		isEnraged = false;
		isChasingFainted = false;
		canBossUseEroAttractSkill = false;
		isUsingEroAttractSkill = false;
		isSkillDelay = false;
		isJumpDelay = false;
		isMoveToPoint = false;
		invertSpriteFacing = false;
		flySpecialRotation = false;
		countTimeToUseSkill = 0f;
		prevSkillTime = 0f;
		countFlipChanged = 0;
		dmgOfEro = 0;
		sequenceOfBossSkill = 0;
		phaseOfBossFight = 0;
		countGenericStuff = 0;
		minChaseDelay = 3f;
		maxChaseDelay = 6f;
		prevAttackID = 0;
		shakeDecayRate = 1f;
		shakeIntensity = 0f;
		floatLandAgainDelay = 0f;
		prevEroSceneVariantID = 0;
		countBEStuff = 0;
		if ((bool)localEroScript)
		{
			intBadEndLocalLogicID = localEroScript.BadEndConfiguration.eroVariantID;
		}
		crtEnemyWasHitByPlayerAttack = null;
		crtBadEnd = null;
		localBadEndFlag = false;
		hasSkippedToBadEnd = false;
		if (eroCore.eroSoloAlts != null && eroCore.eroSoloAlts.Length != 0)
		{
			listOfValidEroSceneVariantsID = new List<int>();
		}
		ignoreCollTimePeriod = 1f;
		if (eroCore.offsetPlayerRelease.y <= 0f)
		{
			eroCore.offsetPlayerRelease.y = 0.25f;
		}
	}

	private void ReplaceDefaultBehavioralValues()
	{
		if (!enmBaseStats.isBoss && !isStatic)
		{
			enmCmnBehaviour.behaviourRangeX = 30f;
			enmCmnBehaviour.behaviourRangeY = 15f;
			enmCmnBehaviour.rangeFaintedChaseX = 20f;
			enmCmnBehaviour.rangeFaintedChaseY = 10f;
			enmCmnBehaviour.durationFaintedChase = 7;
		}
	}

	private void OnEnable()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.WormOverGround)
		{
			objAN = GetComponent<Animator>();
			if (vecOriginalColliderSize == Vector2.zero)
			{
				vecOriginalColliderSize = GetComponent<BoxCollider2D>().size;
			}
			if (vecOriginalColliderOffset == Vector2.zero)
			{
				vecOriginalColliderOffset = GetComponent<BoxCollider2D>().offset;
			}
			if ((bool)objAN)
			{
				objAN.Play("WRM-Rise");
				objAN.SetInteger("genericPhase", 0);
			}
			if (!IsInvoking("BasicStatic_MainBehaviour"))
			{
				InvokeRepeating("BasicStatic_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			}
		}
	}

	private void OnDisable()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.WormOverGround)
		{
			EnemyConcludeSkill();
			StopAllCoroutines();
			crtGeneric = null;
			if (IsInvoking("BasicStatic_MainBehaviour"))
			{
				CancelInvoke("BasicStatic_MainBehaviour");
			}
		}
	}

	private void InitializeEnemyMainBehaviour()
	{
		if (isGauntletSpecial && shouldApplyMiniRage && !_isRageGauntlet)
		{
			RAGE_GAUNTLET_APPLY();
		}
		if (!enmMisc.isFlyer)
		{
			SetNewLimitIdleTime();
			SetNewLimitRunningTime();
		}
		if (enmBaseStats.isBoss && GC.isBossRematch)
		{
			enmBaseStats.maxLife *= 2;
			amountLife = enmBaseStats.maxLife;
			enmBaseStats.baseTouchDamage *= 2;
			enmCmnSkill.baseSkillDamage *= 2;
			enmBaseStats.baseDefense *= 2;
		}
		offsetStruggleBarPosit = eroCore.offsetStruggleBarPos;
		eroGracePeriod = eroCore.eroGracePeriod;
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Isobora:
		case GameCore.EnemyType.BeachKitsune:
		case GameCore.EnemyType.Arachne:
		case GameCore.EnemyType.WaterJelly:
		case GameCore.EnemyType.Lamia:
		case GameCore.EnemyType.MushroomGirl:
		case GameCore.EnemyType.DwarfGirl:
		case GameCore.EnemyType.PurpleSlime:
		case GameCore.EnemyType.FutaBunny:
		case GameCore.EnemyType.SailorBurly:
		case GameCore.EnemyType.WyvernGirl:
		case GameCore.EnemyType.Failed:
		case GameCore.EnemyType.CorruptBeast:
		case GameCore.EnemyType.DarkDamsel:
		case GameCore.EnemyType.Medusa:
		case GameCore.EnemyType.SlimeGirl:
		case GameCore.EnemyType.SlimeMayaFat:
		case GameCore.EnemyType.Goblin:
		case GameCore.EnemyType.Catgirl:
		case GameCore.EnemyType.GeckNewt:
		case GameCore.EnemyType.Tentapod:
			InvokeRepeating("GenericWalker_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			if (enmMisc.isPlayerTransformed)
			{
				tmpObjPlayerReference = GameObject.FindGameObjectWithTag("Player");
				if (tmpObjPlayerReference != null)
				{
					tmpObjPlayerReference.transform.parent = base.transform;
					tmpObjPlayerReference.transform.position = base.transform.position;
				}
			}
			break;
		case GameCore.EnemyType.LazyStarfish:
			InvokeRepeating("GenericWalker_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			if (base.gameObject.name.Contains("Spiderling"))
			{
				crtGeneric = StartCoroutine(waitGENERIC(10f, "layerOrder", 8));
			}
			break;
		case GameCore.EnemyType.LizardShield:
		case GameCore.EnemyType.LizardAxe:
		case GameCore.EnemyType.Kobold:
		case GameCore.EnemyType.SwampGolem:
		case GameCore.EnemyType.PrisonBrute:
		case GameCore.EnemyType.Dominatrix:
		case GameCore.EnemyType.Goblina:
		case GameCore.EnemyType.SewerBug:
		case GameCore.EnemyType.HillTroll:
		case GameCore.EnemyType.VagrantMonk:
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(0));
			InvokeRepeating("GenericWalker_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.CentaurGirl:
			InvokeRepeating("GenericWalker_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(0));
			listOfExternalAttacks.Add(GetMiscGameObject(1));
			break;
		case GameCore.EnemyType.SalamanderGirl:
		case GameCore.EnemyType.Dogboy:
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(0));
			floatGeneric = GC.MATH_RNG_FLOAT(1.3f, 1.6f);
			if (currentEnemyType == GameCore.EnemyType.Dogboy)
			{
				floatGeneric = GC.MATH_RNG_FLOAT(1.1f, 1.4f);
			}
			InvokeRepeating("GenericWalker_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BlazePlant:
		case GameCore.EnemyType.NaughtyCoral:
		case GameCore.EnemyType.FlowerBlossom:
		case GameCore.EnemyType.FleshBlossom:
			InvokeRepeating("BasicStatic_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.TentacleTree:
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(0));
			listOfExternalAttacks.Add(GetMiscGameObject(1));
			InvokeRepeating("BasicStatic_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.PaintingEvil:
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(0));
			colorGeneric = objRE.color;
			colorGeneric.a = 0f;
			objRE.color = colorGeneric;
			InvokeRepeating("BasicStatic_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.LatexMaiden:
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(0));
			InvokeRepeating("BasicStatic_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.WolfGirlMimic:
			posGenericVector.y = -1f;
			InvokeRepeating("WFG_RepeatingBehavior", 0f, enmBaseStats.freqRepeatBehaviour);
			InvokeRepeating("WFG_RepeatingPeek", 0f, 1f);
			break;
		case GameCore.EnemyType.BossForest:
			timeToTriggerNextSkill = 5f;
			currentBossState = BossState.Idle;
			objAN.Play("ForestBoss-Sleep");
			InvokeRepeating("InvokedForestBossBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BossBeach:
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(6));
			timeToTriggerNextSkill = 3f;
			currentBossState = BossState.Idle;
			InvokeRepeating("InvokedBeachBossBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.Dragonfly:
		case GameCore.EnemyType.HillFairy:
		case GameCore.EnemyType.CrystalChunk:
		case GameCore.EnemyType.Bot1:
		case GameCore.EnemyType.Bot2:
		case GameCore.EnemyType.Bot3:
		case GameCore.EnemyType.BeeGirl:
		case GameCore.EnemyType.Sinccubus:
			currentFlyingState = enemyFlyingState.Wandering;
			InvokeRepeating("BasicFlyer_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			BasicFlying_CheckLanded(enforceLanding: true);
			if (enmMisc.isPlayerTransformed)
			{
				currentFlyingState = enemyFlyingState.Idle;
				tmpObjPlayerReference = GameObject.FindGameObjectWithTag("Player");
				if (tmpObjPlayerReference != null)
				{
					tmpObjPlayerReference.transform.parent = base.transform;
					tmpObjPlayerReference.transform.position = base.transform.position;
				}
			}
			break;
		case GameCore.EnemyType.MothGirl:
			currentFlyingState = enemyFlyingState.Idle;
			InvokeRepeating("BasicFlyer_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			BasicFlying_CheckLanded(enforceLanding: true);
			if (enmMisc.isPlayerTransformed)
			{
				tmpObjPlayerReference = GameObject.FindGameObjectWithTag("Player");
				if (tmpObjPlayerReference != null)
				{
					tmpObjPlayerReference.transform.parent = base.transform;
					tmpObjPlayerReference.transform.position = base.transform.position;
				}
			}
			break;
		case GameCore.EnemyType.SwampFairy:
		case GameCore.EnemyType.WormDig:
		case GameCore.EnemyType.FairySorrow:
			currentFlyingState = enemyFlyingState.Wandering;
			InvokeRepeating("BasicFlyer_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			BasicFlying_CheckLanded(enforceLanding: true);
			break;
		case GameCore.EnemyType.Batgirl:
		case GameCore.EnemyType.BigInsect:
			currentFlyingState = enemyFlyingState.Wandering;
			InvokeRepeating("BasicFlyer_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			BasicFlying_CheckLanded(enforceLanding: true);
			flySpecialRotation = true;
			boolGeneric = false;
			break;
		case GameCore.EnemyType.FutaGhost:
		case GameCore.EnemyType.GhostButler:
			currentFlyingState = enemyFlyingState.Idle;
			InvokeRepeating("BasicFlyer_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			BasicFlying_CheckLanded(enforceLanding: true);
			UnityEngine.Object.Instantiate(GetMiscGameObject(0), enmObjRefs.StartPointCenterMass.position, Quaternion.identity).SetActive(value: true);
			break;
		case GameCore.EnemyType.Manequin:
			InvokeRepeating("GenericWalker_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			InvokeRepeating("Invoked_Manequin_PrincessBossHack", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.Manequin_EvilMaya:
		case GameCore.EnemyType.Manequin_MayaStuck:
		case GameCore.EnemyType.PSMMaya:
			InvokeRepeating("GenericWalker_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			if (enmMisc.isPlayerTransformed)
			{
				tmpObjPlayerReference = GameObject.FindGameObjectWithTag("Player");
				if (tmpObjPlayerReference != null)
				{
					tmpObjPlayerReference.transform.parent = base.transform;
					tmpObjPlayerReference.transform.position = base.transform.position;
				}
			}
			break;
		case GameCore.EnemyType.FutaDruidCentauress:
			if ((bool)GC && GC.configFetishes.showFutanari && enmObjRefs.AnimOverrideCtrl1 != null)
			{
				objAN.runtimeAnimatorController = enmObjRefs.AnimOverrideCtrl1;
			}
			InvokeRepeating("GenericWalker_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BossHill:
			timeToTriggerNextSkill = 3f;
			currentBossState = BossState.Idle;
			InvokeRepeating("InvokedHillBossBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BossWerewolf:
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(2));
			timeToTriggerNextSkill = 3f;
			currentBossState = BossState.Idle;
			InvokeRepeating("InvokedWerewolfBossBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BossMinotaur:
			listOfExternalAttacks = new List<GameObject>();
			listOfExternalAttacks.Add(GetMiscGameObject(1));
			listOfExternalAttacks.Add(GetMiscGameObject(2));
			listOfExternalAttacks.Add(GetMiscGameObject(3));
			listOfExternalAttacks.Add(GetMiscGameObject(6));
			timeToTriggerNextSkill = 3f;
			currentBossState = BossState.Idle;
			InvokeRepeating("InvokedMinotaurBossBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BossPrincess:
			timeToTriggerNextSkill = 3f;
			currentBossState = BossState.Idle;
			InvokeRepeating("InvokedPrincessBossBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BossWitch:
			timeToTriggerNextSkill = 3f;
			currentBossState = BossState.Idle;
			InvokeRepeating("InvokedWitchBossBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BossBlueFox:
			timeToTriggerNextSkill = 3f;
			currentBossState = BossState.Idle;
			InvokeRepeating("InvokedBlueFoxBossBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.BossSwampSlime:
			timeToTriggerNextSkill = 3f;
			currentBossState = BossState.Idle;
			InvokeRepeating("InvokedBossSwampBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			break;
		case GameCore.EnemyType.TargetDummy:
		case GameCore.EnemyType.WormOverGround:
			break;
		}
	}

	private bool CheckIfThisEnemyCanRemainActiveOnStart()
	{
		if (enmBaseStats.isBoss)
		{
			if (isGalleryView)
			{
				return true;
			}
			bool flag = false;
			switch (currentEnemyType)
			{
			case GameCore.EnemyType.BossForest:
				if ((bool)GC && GC.flagsMain.isForestBossDefeatd)
				{
					flag = true;
				}
				break;
			case GameCore.EnemyType.BossBeach:
				if ((bool)GC && GC.flagsMain.isBeachBossDefeated)
				{
					flag = true;
				}
				break;
			case GameCore.EnemyType.BossHill:
				if ((bool)GC && GC.flagsMain.isHillBossDefeated)
				{
					flag = true;
				}
				break;
			case GameCore.EnemyType.BossWerewolf:
				if ((bool)GC && GC.flagsMain.isWerewolfBossDefeated)
				{
					flag = true;
				}
				break;
			case GameCore.EnemyType.BossMinotaur:
				if ((bool)GC && GC.flagsMain.isMinotaurBossDefeated)
				{
					flag = true;
				}
				break;
			case GameCore.EnemyType.BossPrincess:
				if ((bool)GC && GC.flagsMain.isPrincessBossDefeated)
				{
					flag = true;
				}
				break;
			case GameCore.EnemyType.BossWitch:
				if ((bool)GC && GC.flagsMain.isWitchBossDefeated)
				{
					flag = true;
				}
				break;
			case GameCore.EnemyType.BossBlueFox:
				if ((bool)GC && GC.flagsMain.isBlueFoxDefeated)
				{
					flag = true;
				}
				break;
			case GameCore.EnemyType.BossSwampSlime:
				if ((bool)GC && GC.flagsMain.isBossSwampDefeated)
				{
					flag = true;
				}
				break;
			default:
				Debug.Log("This boss object should be cheking on start() if they should stay active based on their defeated flag");
				break;
			}
			if (flag)
			{
				GC.DoStuffAfterBossDeath(currentEnemyType, doGenericToggles: false);
				FinalDeathFrame();
				return false;
			}
		}
		return true;
	}

	private void FixedUpdate()
	{
		localSpeedX = enmBaseStats.speedMoveX;
		localSpeedY = enmBaseStats.speedMoveY;
		if (!enmMisc.isFlyer)
		{
			SetEnemyStateMachine();
			SetEnemyAnimation();
		}
		else
		{
			SetEnemyAnimationFlying();
		}
		if (enemyCurrentYMove > GC.worldBaseGravityCap && !isDeath && !isStatic && !enmMisc.isFlyer)
		{
			enemyCurrentYMove += GC.worldBaseGravityIncrements;
		}
		else if ((enemyCurrentYMove > GC.worldBaseGravityCap && enmMisc.isFlyer && (isDeath || (isGrabbing && enmMisc.FlyerEroApplyGravity))) || (isDeath && enmMisc.maintainCollisionOnDeath))
		{
			enemyCurrentYMove += GC.worldBaseGravityIncrements;
		}
		if (!enmMisc.isFlyer && !isDeath)
		{
			FXU_CheckWalkerMovement();
		}
		if (enmMisc.isFlyer && !isDeath)
		{
			FXU_CheckFlyerMovement();
		}
		if (enemyCurrentXMove > 1f)
		{
			enemyCurrentXMove -= enmMisc.rateHorizontalFriction;
		}
		else if (enemyCurrentXMove < -1f)
		{
			enemyCurrentXMove += enmMisc.rateHorizontalFriction;
		}
		if (enmMisc.isFlyer && enemyCurrentYMove > 1f && !isGrabbing)
		{
			enemyCurrentYMove -= enmMisc.rateVerticalFriction;
		}
		else if (enmMisc.isFlyer && enemyCurrentYMove < -1f && !isGrabbing)
		{
			enemyCurrentYMove += enmMisc.rateVerticalFriction;
		}
		vecEnemyMovement.x = enemyCurrentXMove * localSpeedX;
		vecEnemyMovement.y = enemyCurrentYMove * localSpeedY;
		if (enmBaseStats.isBoss && !isDeath && isMoveToPoint && posGenericVector != Vector3.zero)
		{
			FXU_CheckFixedBossMovement();
		}
		FXU_FIX_SLIDING_BUG();
		FXU_HACK_PLAYER_BECAME_ENEMY_CAMERA_FOLLOW();
		if ((bool)objRB && !FXU_MOVE_EXCEPTIONS() && objRB.bodyType != RigidbodyType2D.Static)
		{
			objRB.velocity = vecEnemyMovement;
		}
		EASYMODE_ApplyChanges(forceInitialConfig: false);
	}

	private void Update()
	{
		if (isGrabbing && GC.playerCancelStopEro)
		{
			GC.playerCancelStopEro = false;
			objAN.SetBool("isGrabbing", value: false);
			objAN.SetBool("isEro", value: false);
			localEroScript.EndEroLogic();
		}
		if (isGrabbing && eroCore.forceDefaultFacing && GetEnemyFacing() != 1f)
		{
			STATE_MACHINE_FLIP_SELF(forcedLeft: false, forcedRight: true);
		}
		if (shakeIntensity > 0f)
		{
			base.transform.position = preShakePos + UnityEngine.Random.insideUnitSphere * shakeIntensity;
			base.transform.rotation = new Quaternion(preShakeRot.x + UnityEngine.Random.Range(0f - shakeIntensity, shakeIntensity) * 0.2f, preShakeRot.y + UnityEngine.Random.Range(0f - shakeIntensity, shakeIntensity) * 0.2f, preShakeRot.z + UnityEngine.Random.Range(0f - shakeIntensity, shakeIntensity) * 0.2f, preShakeRot.w + UnityEngine.Random.Range(0f - shakeIntensity, shakeIntensity) * 0.2f);
			shakeIntensity -= shakeDecayRate;
			if (shakeIntensity <= 0f)
			{
				base.transform.position = preShakePos;
				base.transform.rotation = preShakeRot;
			}
		}
		if (isDeath && enmBaseStats.isBoss && base.transform.position != posGenericVector)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, posGenericVector, 0.1f * Time.deltaTime);
		}
		if (isStagger && isUsingSkill)
		{
			EnemyConcludeSkill();
		}
		UPDATE_DisableExternalAttacksSafetyMeasure();
		BE_UPDATE_MAINCHECK();
	}

	private void FXU_CheckFlyerMovement()
	{
		if (currentFlyingState == enemyFlyingState.Chasing && limitRun > 0 && !isStagger)
		{
			float num = GC.playerCurrentWorldYPosition;
			if (currentEnemyType == GameCore.EnemyType.WormDig && (GC.playerIsOnGround || GC.isPlayerFainted))
			{
				num -= 2.25f;
			}
			num += GC.MATH_RNG_FLOAT(-0.2f, 0.2f);
			posGenericVector = (new Vector3(GC.playerCurrentWorldXPosition, num, base.transform.position.z) - base.transform.position).normalized;
			enemyCurrentXMove = posGenericVector.x;
			enemyCurrentYMove = posGenericVector.y;
			if (localSpeedX <= enmBaseStats.speedMoveX)
			{
				localSpeedX *= enmMisc.flyerChaseMultiplier;
				localSpeedY *= enmMisc.flyerChaseMultiplier;
			}
		}
		else if (currentFlyingState == enemyFlyingState.Retreating && limitRun > 0 && !isStagger)
		{
			posGenericVector = (new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, base.transform.position.z) - base.transform.position).normalized;
			posGenericVector *= -1f;
			enemyCurrentXMove = posGenericVector.x;
			enemyCurrentYMove = posGenericVector.y;
			if (localSpeedX <= enmBaseStats.speedMoveX)
			{
				localSpeedX *= enmMisc.flyerRetreatMultiplier;
				localSpeedY *= enmMisc.flyerRetreatMultiplier;
			}
		}
		if (!enmMisc.flyerIgnoreGDistance)
		{
			CheckFlyerGroundDistance();
		}
	}

	private void FXU_CheckWalkerMovement()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.SalamanderGirl:
			if (currentEnemyState == enemyState.Chasing)
			{
				localSpeedX *= floatGeneric;
			}
			break;
		case GameCore.EnemyType.Dogboy:
			if (currentEnemyState == enemyState.Chasing)
			{
				localSpeedX *= floatGeneric;
			}
			if (isUsingSkill)
			{
				enemyCurrentXMove = 0f;
			}
			break;
		case GameCore.EnemyType.WolfGirlMimic:
			if (enemyCurrentYMove > GC.worldBaseGravityCap && !objBox2D.isTrigger)
			{
				enemyCurrentYMove += GC.worldBaseGravityIncrements;
			}
			break;
		case GameCore.EnemyType.SailorBurly:
			if (currentEnemyState == enemyState.Chasing && objAN.GetInteger("genericPhase") == 1)
			{
				localSpeedX *= 3f;
			}
			break;
		}
	}

	private bool FXU_MOVE_EXCEPTIONS()
	{
		if (isDeath)
		{
			return false;
		}
		if (CheckEroMoveException())
		{
			return false;
		}
		if (enmMisc.typeFlyerBehaviour == ENM_Miscellaneuss.FlyerType.Defensive && !isGrabbing)
		{
			float num = 1f;
			if (objRB.velocity.x <= num && objRB.velocity.x >= 0f - num && objRB.velocity.y <= num && objRB.velocity.y >= 0f - num)
			{
				objRB.velocity = vecEnemyMovement;
			}
			return true;
		}
		if (enmMisc.isFlyer && isGrabbing && !enmMisc.FlyerEroApplyGravity)
		{
			objRB.velocity = Vector2.zero;
			return true;
		}
		if (enmMisc.CanWalkWater && CheckCollisionGround_WaterEnterEffect() && vecEnemyMovement.y < 0f)
		{
			vecEnemyMovement.y = 0f;
		}
		return false;
	}

	private void FXU_FIX_SLIDING_BUG()
	{
		if (base.gameObject.activeInHierarchy && !enmMisc.isFlyer && !isDeath && (currentEnemyState == enemyState.Standing || currentEnemyState == enemyState.Crouching) && vecEnemyMovement.y < 0f && CheckCollisionGround())
		{
			vecEnemyMovement.y = 0f;
		}
	}

	private void FXU_MOVE_PLAT_VERTICAL()
	{
		if (objMovePlatRB != null && objMovePlatRB.velocity.y < 0.1f && ((enmMisc.isFlyer && isGrabbing) || (!enmMisc.isFlyer && objRB.velocity.y < 0f && vecEnemyMovement.y < 0f)))
		{
			vecEnemyMovement.y = objMovePlatRB.velocity.y;
		}
	}

	private void FXU_HACK_PLAYER_BECAME_ENEMY_CAMERA_FOLLOW()
	{
		if (enmMisc.isPlayerTransformed && GC.isPlayerFainted && GC.isPlayerGoneForGood && tmpObjPlayerReference != null)
		{
			tmpObjPlayerReference.transform.position = base.transform.position;
		}
	}

	private void SetEnemyStateMachine()
	{
		if (isChasingFainted)
		{
			currentEnemyState = enemyState.Chasing;
			return;
		}
		switch (currentEnemyState)
		{
		case enemyState.Standing:
			if (!CheckStateMachineJumpFall() && enemyCurrentXMove != 0f)
			{
				currentEnemyState = enemyState.Patrolling;
			}
			break;
		case enemyState.Patrolling:
			if (!CheckStateMachineJumpFall() && enemyCurrentXMove == 0f)
			{
				currentEnemyState = enemyState.Standing;
			}
			break;
		case enemyState.Jumping:
			if (enemyCurrentYMove <= 0f)
			{
				currentEnemyState = enemyState.Falling;
			}
			break;
		case enemyState.Falling:
			if (Mathf.RoundToInt(objRB.velocity.y) == 0 && CheckCollisionGround())
			{
				if (enemyCurrentXMove == 0f)
				{
					currentEnemyState = enemyState.Standing;
				}
				else
				{
					currentEnemyState = enemyState.Patrolling;
				}
			}
			else if (CheckCollisionMovingPlatforms())
			{
				if (enemyCurrentXMove == 0f)
				{
					currentEnemyState = enemyState.Standing;
				}
				else
				{
					currentEnemyState = enemyState.Patrolling;
				}
			}
			break;
		case enemyState.Chasing:
			if (!isChasingFainted)
			{
				if (!CheckStateMachineJumpFall() && enemyCurrentXMove == 0f)
				{
					currentEnemyState = enemyState.Standing;
				}
				else if (!CheckStateMachineJumpFall() && enemyCurrentXMove != 0f)
				{
					currentEnemyState = enemyState.Patrolling;
				}
			}
			break;
		case enemyState.Crouching:
			break;
		}
	}

	private void SetEnemyAnimation()
	{
		if (currentEnemyState == enemyState.Standing && (bool)objAN)
		{
			objAN.SetBool("isStanding", value: true);
		}
		else
		{
			objAN.SetBool("isStanding", value: false);
		}
		if ((currentEnemyState == enemyState.Patrolling || currentEnemyState == enemyState.Chasing) && (bool)objAN)
		{
			objAN.SetBool("isRunning", value: true);
		}
		else
		{
			objAN.SetBool("isRunning", value: false);
		}
		if (currentEnemyState == enemyState.Jumping || currentEnemyState == enemyState.Falling || STATE_MACHINE_SPECIAL_JUMP_CHECK())
		{
			objAN.SetBool("isJumping", value: true);
		}
		else
		{
			objAN.SetBool("isJumping", value: false);
		}
		STATE_MACHINE_FLIP_SELF();
	}

	private void SetEnemyAnimationFlying()
	{
		enemyFlyingState enemyFlyingState2 = currentFlyingState;
		if (enemyFlyingState2 == enemyFlyingState.Idle)
		{
			objAN.SetBool("isStanding", value: true);
		}
		else
		{
			objAN.SetBool("isStanding", value: false);
		}
		if (enemyFlyingState2 == enemyFlyingState.Landed)
		{
			objAN.SetBool("isFlyerLanded", value: true);
		}
		else
		{
			objAN.SetBool("isFlyerLanded", value: false);
		}
		if (enemyFlyingState2 == enemyFlyingState.Chasing || enemyFlyingState2 == enemyFlyingState.Patrolling || enemyFlyingState2 == enemyFlyingState.Retreating || enemyFlyingState2 == enemyFlyingState.Wandering)
		{
			objAN.SetBool("isRunning", value: true);
		}
		else
		{
			objAN.SetBool("isRunning", value: false);
		}
		STATE_MACHINE_FLIP_SELF();
		if (!flySpecialRotation && ((base.transform.localScale.x == -1f && Mathf.RoundToInt(rotationZ) < 0) || (base.transform.localScale.x == 1f && Mathf.RoundToInt(rotationZ) > 0)))
		{
			rotationZ *= -1f;
		}
		if (Mathf.RoundToInt(rotationZ) != 0 && !flySpecialRotation)
		{
			base.transform.rotation = Quaternion.AngleAxis(rotationZ, new Vector3(0f, 0f, 1f));
		}
		else if (Mathf.RoundToInt(rotationZ) != 0 && flySpecialRotation)
		{
			base.transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
		}
		else if (base.transform.rotation != Quaternion.identity)
		{
			base.transform.rotation = Quaternion.identity;
		}
		if ((bool)MROT)
		{
			HackyCheckSubCanvasRotation();
		}
		FlyingAnimSpecifics();
	}

	private void FlyingAnimSpecifics()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Dragonfly:
			if (currentFlyingState == enemyFlyingState.Chasing && GC.CheckPlayerProximityToAnyObject(enmObjRefs.StartPointCenterMass.position, GameCore.PlayerProximityType.Adjacent))
			{
				objAN.SetBool("isSkill", value: true);
				rotationZ = 0f;
				if (!isGrabbing)
				{
					AlterSelfBoxCollider(BoxCollAction.ChangeSize, new Vector2(0.3f, 0.3f), Vector2.zero);
				}
			}
			else if (objAN.GetBool("isSkill"))
			{
				objAN.SetBool("isSkill", value: false);
				rotationZ = enmMisc.flyerZRotation;
				if (!isGrabbing)
				{
					AlterSelfBoxCollider(BoxCollAction.Reset, Vector2.zero, Vector2.zero);
				}
			}
			break;
		case GameCore.EnemyType.HillFairy:
		case GameCore.EnemyType.SwampFairy:
		case GameCore.EnemyType.FairySorrow:
			if (currentFlyingState == enemyFlyingState.Retreating || currentFlyingState == enemyFlyingState.Chasing)
			{
				if (!objAN.GetBool("isRunning"))
				{
					objAN.SetBool("isRunning", value: true);
					objAN.SetBool("isStanding", value: false);
				}
			}
			else if (objAN.GetBool("isRunning"))
			{
				objAN.SetBool("isRunning", value: false);
				objAN.SetBool("isStanding", value: true);
			}
			break;
		case GameCore.EnemyType.Batgirl:
			if (rotationZ != 0f && currentFlyingState != enemyFlyingState.SpecialAction)
			{
				rotationZ = 0f;
			}
			break;
		case GameCore.EnemyType.BigInsect:
			if (rotationZ != 0f && objAN.GetInteger("genericPhase") != 1)
			{
				rotationZ = 0f;
			}
			if (isUsingSkill && objAN.GetInteger("genericPhase") == 1 && (double)objRB.velocity.x < -0.1 && !objRE.flipY)
			{
				objRE.flipY = true;
			}
			if (objRE.flipY && objAN.GetInteger("genericPhase") != 1)
			{
				objRE.flipY = false;
			}
			break;
		}
	}

	private void HackyCheckSubCanvasRotation()
	{
		if (rotationZ < 0f)
		{
			MROT.offsetLocalRightSide = false;
			MROT.offsetLocalLeftSide = true;
		}
		else if (rotationZ > 0f)
		{
			MROT.offsetLocalLeftSide = false;
			MROT.offsetLocalRightSide = true;
		}
		else
		{
			MROT.offsetLocalLeftSide = false;
			MROT.offsetLocalRightSide = false;
		}
	}

	private bool STATE_MACHINE_SPECIAL_JUMP_CHECK()
	{
		if (isChasingFainted && enemyCurrentYMove != 0f && !CheckCollisionGround() && !CheckCollisionMovingPlatforms())
		{
			return true;
		}
		return false;
	}

	private void STATE_MACHINE_FLIP_SELF(bool forcedLeft = false, bool forcedRight = false)
	{
		if (enmMisc.ignoreSpriteFliping)
		{
			return;
		}
		STATE_FLIP_EXCEPTIONS(ref forcedLeft, ref forcedRight);
		enemySetScale = base.transform.localScale;
		if (invertSpriteFacing)
		{
			if (enemyCurrentXMove < 0f && Mathf.RoundToInt(base.transform.localScale.x) != 1)
			{
				enemySetScale.x = 1f;
			}
			else if (enemyCurrentXMove > 0f && Mathf.RoundToInt(base.transform.localScale.x) != -1)
			{
				enemySetScale.x = -1f;
			}
		}
		else if (enemyCurrentXMove > 0f && Mathf.RoundToInt(base.transform.localScale.x) != 1)
		{
			enemySetScale.x = 1f;
		}
		else if (enemyCurrentXMove < 0f && Mathf.RoundToInt(base.transform.localScale.x) != -1)
		{
			enemySetScale.x = -1f;
		}
		if (forcedLeft)
		{
			enemySetScale.x = -1f;
		}
		else if (forcedRight)
		{
			enemySetScale.x = 1f;
		}
		if (base.transform.localScale != enemySetScale)
		{
			base.transform.localScale = enemySetScale;
		}
		HACK_FIX_CHILD_CANVAS_SCALE_FLIPPING();
	}

	private void STATE_MACHINE_FLIP_FACE_PLAYER(bool lookAwayFromPlayer = false)
	{
		if (enmMisc.ignoreSpriteFliping)
		{
			return;
		}
		enemySetScale = base.transform.localScale;
		if (lookAwayFromPlayer)
		{
			if (GC.playerCurrentWorldXPosition > base.transform.position.x && Mathf.RoundToInt(base.transform.localScale.x) != -1)
			{
				enemySetScale.x = -1f;
			}
			else if (GC.playerCurrentWorldXPosition <= base.transform.position.x && Mathf.RoundToInt(base.transform.localScale.x) != 1)
			{
				enemySetScale.x = 1f;
			}
		}
		else if (GC.playerCurrentWorldXPosition < base.transform.position.x && Mathf.RoundToInt(base.transform.localScale.x) != -1)
		{
			enemySetScale.x = -1f;
		}
		else if (GC.playerCurrentWorldXPosition >= base.transform.position.x && Mathf.RoundToInt(base.transform.localScale.x) != 1)
		{
			enemySetScale.x = 1f;
		}
		if (base.transform.localScale != enemySetScale)
		{
			base.transform.localScale = enemySetScale;
		}
		HACK_FIX_CHILD_CANVAS_SCALE_FLIPPING();
	}

	private void STATE_FLIP_EXCEPTIONS(ref bool fLeft, ref bool fRight)
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Batgirl:
			if (currentFlyingState == enemyFlyingState.SpecialAction)
			{
				fRight = true;
			}
			break;
		case GameCore.EnemyType.BigInsect:
			if (isUsingSkill && objAN.GetInteger("genericPhase") == 1)
			{
				fRight = true;
			}
			break;
		}
	}

	private void ThisEnemyWasHit(int dmgValue, int objInstanceID, Vector3 hitEffectPos, string hitOriginTag, string hitOriginName, GameCore.PlayerDamageType thisDmgType, bool forceStagger = false)
	{
		if (isDeath || enmObjRefs.objTextDamageUI == null)
		{
			return;
		}
		float durationOfStagger = 0f;
		if (!NewInstanceIDCurrentAttack(objInstanceID))
		{
			return;
		}
		prevAttackID = objInstanceID;
		if (crtEnemyWasHitByPlayerAttack != null)
		{
			StopCoroutine(crtEnemyWasHitByPlayerAttack);
		}
		crtEnemyWasHitByPlayerAttack = StartCoroutine(waitForDifferentPlayerAttack(0.1f));
		if (dmgValue <= 0)
		{
			dmgValue = 1;
		}
		HitEnemySpecialHealthAlterations(ref dmgValue);
		amountLife -= dmgValue;
		DisplayDamageUI(dmgValue, currentDefense, Color.white, hitOriginName);
		UpdateHealthBarDisplay();
		GC.CheckHit_VFX_SFX(hitOriginTag, hitOriginName, base.gameObject.tag, base.gameObject.name, hitEffectPos);
		if (crtDamageFlashing != null)
		{
			StopCoroutine(crtDamageFlashing);
		}
		if (!enmBaseStats.isBoss)
		{
			crtDamageFlashing = StartCoroutine(waitForDamageFlashing(0.5f));
		}
		if (!isImmuneToStagger && (STAGGER_CHECK(thisDmgType, dmgValue, out durationOfStagger) | forceStagger))
		{
			isStagger = true;
		}
		if (amountLife <= 0)
		{
			if (enmBaseStats.isBoss)
			{
				StartDeathLogicBoss();
			}
			else
			{
				StartDeathLogicBehaviour();
			}
		}
		else
		{
			if (isStagger && !isGrabbing)
			{
				STAGGER_APPLY(durationOfStagger);
			}
			if (!isGrabbing)
			{
				CheckEnemySpecialBehaviourOnHit();
			}
		}
		if (isGrabbing && (bool)localEroScript)
		{
			GC.GlobalApplyStruggleAmountToPlayer(GC.MATH_ResultPercentileOfTotal(dmgValue, enmBaseStats.maxLife) / 2f);
		}
	}

	private bool NewInstanceIDCurrentAttack(int idInstance)
	{
		if (idInstance != prevAttackID)
		{
			return true;
		}
		if (crtEnemyWasHitByPlayerAttack == null)
		{
			return true;
		}
		return false;
	}

	private void CheckEnemySpecialBehaviourOnHit()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.TargetDummy:
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (objAN.GetCurrentAnimatorStateInfo(0).IsName("TGT-Hit"))
			{
				objAN.SetBool("isHit", value: false);
				objAN.Play("Basic-Idle");
			}
			objAN.SetBool("isHit", value: true);
			objAN.Play("TGT-Hit");
			break;
		case GameCore.EnemyType.FlowerBlossom:
		{
			for (int i = 0; i < 3; i++)
			{
				BasicProjectileInstantiate(GetMiscGameObject(1));
			}
			break;
		}
		case GameCore.EnemyType.WolfGirlMimic:
			if (objAN.GetInteger("genericPhase") == 0 || objAN.GetInteger("genericPhase") == 1)
			{
				WFG_ToggleChaseState(state: true);
			}
			break;
		case GameCore.EnemyType.Batgirl:
			if (currentFlyingState == enemyFlyingState.Landed)
			{
				countFrequency = 0f;
				limitRun = 0;
				countFrequency = 0f;
				limitIdle = 0;
				currentFlyingState = enemyFlyingState.Wandering;
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BatWings, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			}
			break;
		case GameCore.EnemyType.BossWitch:
			if (objAN.GetInteger("genericPhase") == 41 && !objAN.GetCurrentAnimatorStateInfo(0).IsName("Illusion-Angry"))
			{
				WITCH_HURTCORRECTLYDURINGILLUSION();
			}
			break;
		case GameCore.EnemyType.GeckNewt:
			isChasingFainted = true;
			break;
		}
	}

	private void HitEnemySpecialHealthAlterations(ref int damageAltered)
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType != GameCore.EnemyType.FutaGhost && enemyType != GameCore.EnemyType.GhostButler)
		{
			return;
		}
		damageAltered = 1;
		boolGeneric = true;
		if (isUsingSkill)
		{
			EnemyConcludeSkill();
		}
		Color color = objRE.color;
		color.a = 0.3f;
		objRE.color = color;
		objBox2D.enabled = false;
		limitRun = 0;
		currentFlyingState = enemyFlyingState.Wandering;
		if (amountLife > 1)
		{
			switch (currentEnemyType)
			{
			case GameCore.EnemyType.FutaGhost:
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FTG_Hurt, 0, checkForDuplicity: true, base.transform.position, 1.1f, 1.2f, "FutaGhost");
				break;
			case GameCore.EnemyType.GhostButler:
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.GSB_Grunt1, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
				break;
			}
		}
		if (crtGeneric != null)
		{
			StopCoroutine(crtGeneric);
		}
		crtGeneric = StartCoroutine(waitGENERIC(5f, ""));
	}

	public void EndEnemyDamageCornerCases()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.TargetDummy)
		{
			objAN.SetBool("isHit", value: false);
		}
	}

	public void ThisEnemyWasHealed(int healValue)
	{
		amountLife += healValue;
		if (amountLife > enmBaseStats.maxLife)
		{
			amountLife = enmBaseStats.maxLife;
		}
		DisplayDamageUI(healValue, 0, Color.green);
		UpdateHealthBarDisplay();
	}

	private IEnumerator waitForDifferentPlayerAttack(float waittime)
	{
		yield return new WaitForSeconds(waittime);
		crtEnemyWasHitByPlayerAttack = null;
	}

	private IEnumerator waitForDamageFlashing(float waittime)
	{
		StartStopSpriteFlashing(start: true, flashType.HalfWhite, 0.275f);
		yield return new WaitForSeconds(waittime);
		StartStopSpriteFlashing(start: false, flashType.HalfWhite, 0f);
		crtDamageFlashing = null;
	}

	private void HitBySpecialSkillLogic(GameCore.PlayerSkillAtk thisSkill, Collider2D skillColl)
	{
		switch (thisSkill)
		{
		case GameCore.PlayerSkillAtk.SlidingTackle:
			if (!enmBaseStats.isBoss && !isStatic)
			{
				float num = 9f / enmBaseStats.speedMoveX;
				float num2 = 1.5f;
				if (GC.ACC_Check(GameCore.AccessoryBehavior.SKILLEnhancedSlidingTackle))
				{
					num *= 1.5f;
					num2 *= 2f;
				}
				ThisEnemyWasHit(GC.GetPlayerDamageForEnemies(GameCore.PlayerDamageType.slidingTackle), 0, base.transform.position, thisSkill.ToString(), thisSkill.ToString(), GameCore.PlayerDamageType.slidingTackle, forceStagger: true);
				SetNewLimitIdleTime();
				currentEnemyState = enemyState.Standing;
				if (GC.currentPlayerFacing == 1)
				{
					enemyCurrentXMove = num;
				}
				else
				{
					enemyCurrentXMove = 0f - num;
				}
				enemyCurrentYMove = num2;
				GC.PlaySFX(GameCore.SFXBasic2.Bump1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			}
			break;
		case GameCore.PlayerSkillAtk.FlameSword:
			ThisEnemyWasHit(GC.GetPlayerDamageForEnemies(GameCore.PlayerDamageType.FlameSword, currentDefense), skillColl.gameObject.GetInstanceID(), objBox2D.bounds.ClosestPoint(skillColl.bounds.ClosestPoint(base.transform.position)), thisSkill.ToString(), thisSkill.ToString(), GameCore.PlayerDamageType.FlameSword);
			break;
		case GameCore.PlayerSkillAtk.GroundPound:
			if (!enmBaseStats.isBoss && !isStatic)
			{
				float num3 = 9f / enmBaseStats.speedMoveX;
				float num4 = 2.5f;
				SetNewLimitIdleTime();
				currentEnemyState = enemyState.Standing;
				if (GC.playerCurrentWorldXPosition <= base.transform.position.x)
				{
					enemyCurrentXMove = num3;
				}
				else
				{
					enemyCurrentXMove = 0f - num3;
				}
				enemyCurrentYMove = num4;
				GC.InstantiateVFX(GameCore.VisualEffect.ShieldBreak, base.transform.position, "EnemyLogic.HitByGroundPound");
				GC.PlaySFX(GameCore.SFXBasic2.Bump1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			}
			break;
		case GameCore.PlayerSkillAtk.PoisonEffect:
			DOT_TOGGLE(thisSkill, setActive: true);
			break;
		case GameCore.PlayerSkillAtk.RagePepper:
			if (!_isRagePepper && !enmBaseStats.isBoss && !isStatic)
			{
				RAGE_APPLY();
				UpdateHealthBarDisplay();
			}
			break;
		case GameCore.PlayerSkillAtk.EntropicExplosion:
			if (enmBaseStats.isBoss)
			{
				int dmgValue = Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(20f, enmBaseStats.maxLife));
				ThisEnemyWasHit(dmgValue, 0, GetEnemyCenterPos(), "entropExpld", "entropExpld", GameCore.PlayerDamageType.entropicExplosion);
			}
			else
			{
				StartDeathLogicBehaviour();
				GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, GetEnemyCenterPos(), "entropExpld");
				GC.InstantiateVFX(GameCore.VisualEffect.HackProjectileFlame, GetEnemyCenterPos(), "entropExpld");
			}
			break;
		}
	}

	private bool STAGGER_CHECK(GameCore.PlayerDamageType dmgType, int dmg, out float durationOfStagger)
	{
		durationOfStagger = defaultEnemyStaggerTime;
		switch (dmgType)
		{
		case GameCore.PlayerDamageType.BasicAttack:
			if (GC.ACC_Check(GameCore.AccessoryBehavior.BladeStun) && GC.MATH_RNG_ROLL(10))
			{
				durationOfStagger = 0.1f;
				return true;
			}
			break;
		case GameCore.PlayerDamageType.ThrowDagger:
			if (GC.ACC_Check(GameCore.AccessoryBehavior.DaggerSpecialist) && GC.MATH_RNG_ROLL(20))
			{
				durationOfStagger = 0.1f;
				return true;
			}
			break;
		case GameCore.PlayerDamageType.FlameSword:
			if (GC.ACC_Check(GameCore.AccessoryBehavior.SKILLEnhancedFlameSword) && enmMisc.isFlyer)
			{
				durationOfStagger = 0.1f;
				return true;
			}
			break;
		case GameCore.PlayerDamageType.slidingTackle:
			durationOfStagger = 2f;
			return true;
		case GameCore.PlayerDamageType.GroundPound:
			durationOfStagger = 3f;
			return true;
		case GameCore.PlayerDamageType.throwLance:
			durationOfStagger = 1f;
			return true;
		case GameCore.PlayerDamageType.throwStar:
			durationOfStagger = 2f;
			return true;
		}
		return false;
	}

	private void STAGGER_APPLY(float duration)
	{
		objAN.SetBool("isStagger", value: true);
		if (coroutStagger != null)
		{
			StopCoroutine(coroutStagger);
		}
		coroutStagger = StartCoroutine(waitForStagger(duration));
		if (isUsingSkill)
		{
			EnemyConcludeSkill();
		}
		if (crtGeneric != null)
		{
			StopCoroutine(crtGeneric);
		}
		if (isChasingFainted || currentEnemyState == enemyState.Chasing || currentFlyingState == enemyFlyingState.Chasing)
		{
			CHASE_END();
			if (enmMisc.isFlyer)
			{
				DecideNextFlyerState(4);
			}
		}
		currentEnemyState = enemyState.Standing;
		currentFlyingState = enemyFlyingState.Idle;
		limitRun = 0;
		countFrequency = 0f;
		countFlipChanged = 0;
		enemyCurrentXMove = 0f;
		enemyCurrentYMove = 0f;
		SetNewLimitRunningTime();
		if ((bool)enmObjRefs.objStaggerVFX)
		{
			enmObjRefs.objStaggerVFX.SetActive(value: true);
			return;
		}
		GameObject objStaggerVFX = UnityEngine.Object.Instantiate(GC.objStaggerEnemyVFX, enmObjRefs.localHPCanvas.transform.position, Quaternion.identity, base.transform);
		enmObjRefs.objStaggerVFX = objStaggerVFX;
		enmObjRefs.objStaggerVFX.SetActive(value: true);
	}

	private IEnumerator waitForStagger(float waittime)
	{
		yield return new WaitForSeconds(waittime);
		if ((bool)enmObjRefs.objStaggerVFX)
		{
			enmObjRefs.objStaggerVFX.SetActive(value: false);
		}
		if (enemyCurrentXMove > 0f)
		{
			enemyCurrentXMove = 1f;
		}
		else if (enemyCurrentXMove < 0f)
		{
			enemyCurrentXMove = -1f;
		}
		isStagger = false;
		objAN.SetBool("isStagger", value: false);
		coroutStagger = null;
	}

	private void DOT_TOGGLE(GameCore.PlayerSkillAtk thisType, bool setActive)
	{
		string text = "";
		if (thisType == GameCore.PlayerSkillAtk.PoisonEffect)
		{
			text = "DOT_INVOKED_POISON";
			floatDotCountPoison = 0f;
		}
		if (IsInvoking(text))
		{
			CancelInvoke(text);
		}
		if ((text != "") & setActive)
		{
			InvokeRepeating(text, 0f, 1f);
		}
	}

	private void DOT_INVOKED_POISON()
	{
		int num = Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(5f, enmBaseStats.maxLife));
		if (num > 50)
		{
			num = 50;
		}
		ThisEnemyWasHit(num, 0, base.transform.position, "dot_poison", "dot_poison", GameCore.PlayerDamageType.poisonPowder);
		GC.InstantiateVFX(GameCore.VisualEffect.PoisonTick, GetEnemyCenterPos(), "EnemyPoisoned");
		floatDotCountPoison++;
		if (floatDotCountPoison >= 10f)
		{
			DOT_TOGGLE(GameCore.PlayerSkillAtk.PoisonEffect, setActive: false);
		}
	}

	private void RAGE_APPLY()
	{
		_isRagePepper = true;
		GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, GetEnemyCenterPos(), "RAGE_APPLY_ENEMY");
		enmBaseStats.speedMoveX *= 1.5f;
		enmBaseStats.speedMoveY *= 1.5f;
		enmBaseStats.baseTouchDamage *= 2;
		enmBaseStats.baseDefense *= 2;
		enmBaseStats.specialDefense *= 2;
		enmBaseStats.freqRepeatBehaviour = 0.1f;
		enmBaseStats.maxLife *= 2;
		amountLife = enmBaseStats.maxLife;
		enmCmnSkill.baseSkillDamage *= 2;
		enmCmnSkill.minSkillFrequency = 2;
		enmCmnSkill.maxSkillFrequency = 6;
		enmCmnSkill.skillPercentChance = 40;
		enmCmnSkill.SkillAnimDuration *= 0.75f;
		enmCmnSkill.SkillTimeToSpawn *= 0.75f;
		enmCmnBehaviour.behaviourRangeX = 999f;
		enmCmnBehaviour.behaviourRangeY = 999f;
		enmCmnBehaviour.rangeFaintedChaseX = 999999f;
		enmCmnBehaviour.rangeFaintedChaseY = 999999f;
		enmCmnBehaviour.durationFaintedChase = 999999;
		enmCmnBehaviour.canJumpToMove = true;
		eroCore.canChasePlayerAliveGrabbed = true;
		isImmuneToStagger = true;
	}

	private void RAGE_GAUNTLET_APPLY()
	{
		_isRageGauntlet = true;
		enmCmnBehaviour.timerIdleMin -= 2;
		enmCmnBehaviour.timerIdleMax -= 2;
		enmCmnBehaviour.timerRunMin += 2;
		enmCmnBehaviour.timerRunMax += 2;
		enmBaseStats.speedMoveX *= 1.25f;
		enmBaseStats.speedMoveY *= 1.25f;
		enmBaseStats.baseTouchDamage = Mathf.RoundToInt((float)enmBaseStats.baseTouchDamage * 1.5f);
		enmBaseStats.baseDefense = Mathf.RoundToInt((float)enmBaseStats.baseDefense * 1.5f);
		enmBaseStats.specialDefense = Mathf.RoundToInt((float)enmBaseStats.specialDefense * 1.5f);
		enmBaseStats.freqRepeatBehaviour = 0.1f;
		enmBaseStats.maxLife = Mathf.RoundToInt((float)enmBaseStats.maxLife * 1.5f);
		amountLife = enmBaseStats.maxLife;
		enmCmnSkill.baseSkillDamage = Mathf.RoundToInt((float)enmCmnSkill.baseSkillDamage * 1.5f);
		enmCmnSkill.minSkillFrequency--;
		enmCmnSkill.maxSkillFrequency -= 2;
		enmCmnSkill.skillPercentChance = Mathf.RoundToInt((float)enmCmnSkill.skillPercentChance * 1.5f);
		enmCmnBehaviour.behaviourRangeX = 77f;
		enmCmnBehaviour.behaviourRangeY = 77f;
		enmCmnBehaviour.rangeFaintedChaseX = 77f;
		enmCmnBehaviour.rangeFaintedChaseY = 77f;
		enmCmnBehaviour.durationFaintedChase = 5;
		enmCmnBehaviour.canJumpToMove = true;
		eroCore.canChasePlayerAliveGrabbed = true;
	}

	private void RAGE_CursedItemCheck()
	{
		if (!_isRagePepper && !enmBaseStats.isBoss && !isStatic && GC.ACC_Check(GameCore.AccessoryBehavior.cursedHorned))
		{
			RAGE_APPLY();
			UpdateHealthBarDisplay();
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		int num = 0;
		switch (other.gameObject.tag)
		{
		case "Lala-Atk-Hitbox":
			num = GC.GetPlayerDamageForEnemies(GameCore.PlayerDamageType.BasicAttack, currentDefense);
			Check_SHIELD_DEFENSE(other, num, out num);
			if (!isGrabbing)
			{
				ThisEnemyWasHit(num, other.gameObject.GetInstanceID(), objBox2D.bounds.ClosestPoint(other.bounds.ClosestPoint(base.transform.position)), other.gameObject.tag, other.gameObject.name, GameCore.PlayerDamageType.BasicAttack);
			}
			break;
		case "Player":
			CollPlayerEnterLeaveTriggerThenDoThis(isEnter: true);
			if (CanGrabPlayerForEro(other.gameObject.name))
			{
				if ((bool)other.transform.parent)
				{
					CallEroLogic(other.transform.parent.gameObject);
				}
				else
				{
					CallEroLogic(other.gameObject);
				}
			}
			break;
		case "Pitfall":
			if (!enmBaseStats.isBoss)
			{
				StartDeathLogicBehaviour();
			}
			break;
		case "Projectile-Basic":
		{
			ProjectileLogic component2 = other.gameObject.GetComponent<ProjectileLogic>();
			if (component2.Originator == ProjectileLogic.OwnerOfProjectile.Player && !IsPlayerProjectileExpections(component2.playerException))
			{
				num = ((component2.touchDamage <= 0) ? GC.GetPlayerDamageForEnemies(component2.playerDmgType) : component2.touchDamage);
				Check_SHIELD_DEFENSE(other, num, out num);
				ThisEnemyWasHit(num, other.gameObject.GetInstanceID(), objBox2D.bounds.ClosestPoint(other.bounds.ClosestPoint(base.transform.position)), other.gameObject.tag, other.gameObject.name, component2.playerDmgType);
				if (component2.playerDmgType == GameCore.PlayerDamageType.GroundPound)
				{
					HitBySpecialSkillLogic(GameCore.PlayerSkillAtk.GroundPound, other);
				}
				if (component2.playerDmgType == GameCore.PlayerDamageType.poisonPowder)
				{
					HitBySpecialSkillLogic(GameCore.PlayerSkillAtk.PoisonEffect, other);
				}
				if (component2.playerDmgType == GameCore.PlayerDamageType.ragePepper)
				{
					HitBySpecialSkillLogic(GameCore.PlayerSkillAtk.RagePepper, other);
				}
				if (component2.playerDmgType == GameCore.PlayerDamageType.entropicExplosion)
				{
					HitBySpecialSkillLogic(GameCore.PlayerSkillAtk.EntropicExplosion, other);
				}
			}
			component2 = null;
			break;
		}
		case "CinematicMoveTarget":
			if (enmBaseStats.isBoss)
			{
				BossHasTouchedWaypoint();
			}
			break;
		case "PlayerSkillAttack":
		{
			PlayerSkillAttack component = other.gameObject.GetComponent<PlayerSkillAttack>();
			if (component != null)
			{
				HitBySpecialSkillLogic(component.thisSkillAttack, other);
			}
			break;
		}
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if ((isStatic || isGalleryView) && other.gameObject.tag == "Player" && CanGrabPlayerForEro(other.gameObject.name))
		{
			if ((bool)other.transform.parent)
			{
				CallEroLogic(other.transform.parent.gameObject);
			}
			else
			{
				CallEroLogic(other.gameObject);
			}
		}
		if (currentEnemyType == GameCore.EnemyType.BossBeach && !isGrabbing && other.gameObject.tag == "Player" && CanGrabPlayerForEro(other.gameObject.name))
		{
			if ((bool)other.transform.parent)
			{
				CallEroLogic(other.transform.parent.gameObject);
			}
			else
			{
				CallEroLogic(other.gameObject);
			}
		}
		if (currentEnemyType == GameCore.EnemyType.BossMinotaur && !isGrabbing && other.gameObject.tag == "Player" && CanGrabPlayerForEro(other.gameObject.name))
		{
			if ((bool)other.transform.parent)
			{
				CallEroLogic(other.transform.parent.gameObject);
			}
			else
			{
				CallEroLogic(other.gameObject);
			}
		}
		if (currentEnemyType == GameCore.EnemyType.BossPrincess && !isGrabbing && other.gameObject.tag == "Player" && CanGrabPlayerForEro(other.gameObject.name))
		{
			if ((bool)other.transform.parent)
			{
				CallEroLogic(other.transform.parent.gameObject);
			}
			else
			{
				CallEroLogic(other.gameObject);
			}
		}
		if (currentEnemyType == GameCore.EnemyType.BossBlueFox && !isGrabbing && other.gameObject.tag == "Player" && CanGrabPlayerForEro(other.gameObject.name))
		{
			if ((bool)other.transform.parent)
			{
				CallEroLogic(other.transform.parent.gameObject);
			}
			else
			{
				CallEroLogic(other.gameObject);
			}
		}
		if (currentEnemyType == GameCore.EnemyType.BossSwampSlime && !isGrabbing && other.gameObject.tag == "Player" && CanGrabPlayerForEro(other.gameObject.name))
		{
			if ((bool)other.transform.parent)
			{
				CallEroLogic(other.transform.parent.gameObject);
			}
			else
			{
				CallEroLogic(other.gameObject);
			}
		}
		if (enmMisc.canBeAffectedBySpringBoard && enemyCurrentYMove <= 0.5f && other.gameObject.tag == "SpringBoard")
		{
			SimpleSpringBoard component = other.gameObject.GetComponent<SimpleSpringBoard>();
			if ((object)component != null && component.canPushUp && !GC.eventIsPlaying)
			{
				enemyCurrentYMove = 5f;
				other.gameObject.GetComponent<SimpleSpringBoard>()?.SpringBoardWasActivated();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		string text = other.gameObject.tag;
		if (text == "Player")
		{
			CollPlayerEnterLeaveTriggerThenDoThis(isEnter: false);
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.tag == "Player" && CanGrabPlayerForEro(other.gameObject.name))
		{
			CallEroLogic(other.gameObject);
		}
		if (enmMisc.isFlyer && other.gameObject.tag == "platform-effector")
		{
			Physics2D.IgnoreCollision(objBox2D, other.collider);
		}
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		string text = other.gameObject.tag;
		if (text == "platform-moving" && enmMisc.isFlyer)
		{
			objMovePlatRB = other.rigidbody;
		}
	}

	private void OnCollisionExit2D(Collision2D other)
	{
		string text = other.gameObject.tag;
		if (text == "platform-moving" && enmMisc.isFlyer)
		{
			objMovePlatRB = null;
		}
	}

	private bool CheckCollisionGround(bool allCheckMustTrue = false, bool ignorePlatPass = false, bool ignorePlayerColl = false, bool ignoreProjectile = false)
	{
		if (enmMisc.CanWalkWater && CheckCollisionGround_WaterEnterEffect())
		{
			return true;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag2 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck2.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag3 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		if (((!allCheckMustTrue & flag) | flag2 | flag3) || (allCheckMustTrue & flag & flag2 & flag3))
		{
			return true;
		}
		if (!ignorePlatPass)
		{
			flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatEffector"));
			flag2 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck2.position, 1 << LayerMask.NameToLayer("LayerPlatEffector"));
			flag3 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatEffector"));
			if (((!allCheckMustTrue & flag) | flag2 | flag3) || (allCheckMustTrue & flag & flag2 & flag3))
			{
				return true;
			}
		}
		if (!ignorePlayerColl)
		{
			flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerPlayer"));
			flag2 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck2.position, 1 << LayerMask.NameToLayer("LayerPlayer"));
			flag3 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerPlayer"));
			if (((!allCheckMustTrue & flag) | flag2 | flag3) || (allCheckMustTrue & flag & flag2 & flag3))
			{
				return true;
			}
		}
		if (!ignoreProjectile)
		{
			flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerProjectile"));
			flag2 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck2.position, 1 << LayerMask.NameToLayer("LayerProjectile"));
			flag3 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerProjectile"));
			if (((!allCheckMustTrue & flag) | flag2 | flag3) || (allCheckMustTrue & flag & flag2 & flag3))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckCollisionGround_WaterEnterEffect()
	{
		RaycastHit2D[] array = Physics2D.LinecastAll(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position);
		if (array != null && array.Length != 0)
		{
			RaycastHit2D[] array2 = array;
			foreach (RaycastHit2D raycastHit2D in array2)
			{
				if (raycastHit2D.transform.tag == "AreaEnterEffect")
				{
					if (enemyCurrentYMove < 0f)
					{
						enemyCurrentYMove = 0f;
					}
					return true;
				}
			}
		}
		RaycastHit2D[] array3 = Physics2D.LinecastAll(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck2.position);
		if (array3 != null && array3.Length != 0)
		{
			RaycastHit2D[] array2 = array3;
			foreach (RaycastHit2D raycastHit2D2 in array2)
			{
				if (raycastHit2D2.transform.tag == "AreaEnterEffect")
				{
					if (enemyCurrentYMove < 0f)
					{
						enemyCurrentYMove = 0f;
					}
					return true;
				}
			}
		}
		RaycastHit2D[] array4 = Physics2D.LinecastAll(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position);
		if (array4 != null && array4.Length != 0)
		{
			RaycastHit2D[] array2 = array4;
			foreach (RaycastHit2D raycastHit2D3 in array2)
			{
				if (raycastHit2D3.transform.tag == "AreaEnterEffect")
				{
					if (enemyCurrentYMove < 0f)
					{
						enemyCurrentYMove = 0f;
					}
					return true;
				}
			}
		}
		return false;
	}

	private bool CheckCollisionCeiling(bool allCheckMustTrue = false, bool ignorePlatMov = false)
	{
		if (!enmObjRefs.CeilingCheck1 || !enmObjRefs.CeilingCheck2 || !enmObjRefs.CeilingCheck3)
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.CeilingCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag2 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.CeilingCheck2.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag3 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.CeilingCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		if (((!allCheckMustTrue & flag) | flag2 | flag3) || (allCheckMustTrue & flag & flag2 & flag3))
		{
			return true;
		}
		if (!ignorePlatMov)
		{
			flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.CeilingCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
			flag2 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.CeilingCheck2.position, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
			flag3 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.CeilingCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
			if (((!allCheckMustTrue & flag) | flag2 | flag3) || (allCheckMustTrue & flag & flag2 & flag3))
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckIfThereIsGroundInFront()
	{
		if (enmMisc.CanWalkWater && CheckCollisionGround_WaterEnterEffect())
		{
			return true;
		}
		bool flag = false;
		if (isUsingSkill && enmCmnSkill.ignoreCollisionOnExecution)
		{
			return true;
		}
		if (enemyCurrentXMove < 0f)
		{
			flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
			if (!flag)
			{
				flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatEffector"));
			}
			if (!flag)
			{
				flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
			}
			if (!flag && GC.MATH_RNG_INT(1, 1000) <= 2 && GC.CheckPlayerProximityToAnyObject(enmObjRefs.StartPointCenterMass.position, GameCore.PlayerProximityType.PlayerToTheLeftCustom, 10f))
			{
				return true;
			}
		}
		else if (enemyCurrentXMove > 0f)
		{
			flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
			if (!flag)
			{
				flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatEffector"));
			}
			if (!flag)
			{
				flag = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
			}
			if (!flag && GC.MATH_RNG_INT(1, 1000) <= 2 && GC.CheckPlayerProximityToAnyObject(enmObjRefs.StartPointCenterMass.position, GameCore.PlayerProximityType.PlayerToTheRightCustom, 10f))
			{
				return true;
			}
		}
		return flag;
	}

	private bool CheckCollisionMovingPlatforms()
	{
		RaycastHit2D raycastHit2D = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		RaycastHit2D raycastHit2D2 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck2.position, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		RaycastHit2D raycastHit2D3 = Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		if ((bool)raycastHit2D)
		{
			objMovePlatRB = raycastHit2D.rigidbody;
			return true;
		}
		if ((bool)raycastHit2D2)
		{
			objMovePlatRB = raycastHit2D2.rigidbody;
			return true;
		}
		if ((bool)raycastHit2D3)
		{
			objMovePlatRB = raycastHit2D3.rigidbody;
			return true;
		}
		objMovePlatRB = null;
		return false;
	}

	private void CheckCollisionCenterOfMass(out int typeColl)
	{
		typeColl = 0;
		if ((bool)Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.StartPointCenterMass.position, 1 << LayerMask.NameToLayer("LayerPlatforms")))
		{
			if (CheckCollisionGround(allCheckMustTrue: true, ignorePlatPass: true, ignorePlayerColl: true))
			{
				typeColl = 1;
			}
			else if (CheckCollisionCeiling(allCheckMustTrue: true, ignorePlatMov: true))
			{
				typeColl = 2;
			}
		}
	}

	private bool CheckStateMachineJumpFall()
	{
		if (isStatic)
		{
			return false;
		}
		if (CheckCollisionMovingPlatforms())
		{
			return false;
		}
		if (enemyCurrentYMove > 0f)
		{
			currentEnemyState = enemyState.Jumping;
			return true;
		}
		if (enemyCurrentYMove < 0f && !CheckCollisionGround())
		{
			currentEnemyState = enemyState.Falling;
			enemyCurrentYMove = -0.1f;
			return true;
		}
		return false;
	}

	private void AlterSelfBoxCollider(BoxCollAction action, Vector2 size, Vector2 offset)
	{
		bool flag = false;
		bool flag2 = false;
		switch (action)
		{
		default:
			if (objBox2D.size != vecOriginalColliderSize)
			{
				objBox2D.size = vecOriginalColliderSize;
			}
			if (objBox2D.offset != vecOriginalColliderOffset)
			{
				objBox2D.offset = vecOriginalColliderOffset;
			}
			return;
		case BoxCollAction.ChangeSize:
			flag = true;
			break;
		case BoxCollAction.ChangeOffset:
			flag2 = true;
			break;
		case BoxCollAction.ChangeBoth:
			flag = true;
			flag2 = true;
			break;
		}
		if (flag && objBox2D.size != size)
		{
			objBox2D.size = size;
		}
		if (flag2 && objBox2D.offset != offset)
		{
			objBox2D.offset = offset;
		}
	}

	private void CollPlayerEnterLeaveTriggerThenDoThis(bool isEnter)
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType != GameCore.EnemyType.WolfGirlMimic)
		{
			return;
		}
		int integer = objAN.GetInteger("genericPhase");
		if (isEnter && !GC.isPlayerFainted && (integer == 0 || integer == 1))
		{
			if (!GetMiscGameObject(0).activeInHierarchy)
			{
				GetMiscGameObject(0).SetActive(value: true);
			}
		}
		else if (GetMiscGameObject(0).activeInHierarchy)
		{
			GetMiscGameObject(0).SetActive(value: false);
		}
	}

	private void Check_SHIELD_DEFENSE(Collider2D otherColl, int inputDmg, out int dmgChanged)
	{
		int shieldObjectID = 0;
		if (SHIELD_CheckIfThisEnemyHasShieldObject(out shieldObjectID) && SHIELD_CheckIfThePlayerAttackCollidedWithTheShield(otherColl, GetMiscGameObject(shieldObjectID).GetComponent<Collider2D>()))
		{
			ShieldObjectMisc component = GetMiscGameObject(shieldObjectID).GetComponent<ShieldObjectMisc>();
			inputDmg = ((component.flatDamageReduce > 0) ? (inputDmg - component.flatDamageReduce) : ((component.dividedDamageReduce <= 0) ? (inputDmg / 2) : (inputDmg / component.dividedDamageReduce)));
		}
		dmgChanged = inputDmg;
	}

	private bool SHIELD_CheckIfThisEnemyHasShieldObject(out int shieldObjectID)
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.LizardShield:
			shieldObjectID = 1;
			return true;
		case GameCore.EnemyType.BossMinotaur:
			shieldObjectID = 7;
			return true;
		default:
			shieldObjectID = 0;
			return false;
		}
	}

	private bool SHIELD_CheckIfThePlayerAttackCollidedWithTheShield(Collider2D playerAtackColl, Collider2D enemyShieldColl)
	{
		if (playerAtackColl.IsTouching(enemyShieldColl))
		{
			return true;
		}
		return false;
	}

	public void PlayMonsterSoundsFromAnim(string sfxName)
	{
		switch (sfxName)
		{
		case "isoboraGrab":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.IsoboraGrab, 60, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "isoboraSkill":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.IsoboraSkill, 0, checkForDuplicity: true, base.transform.position, 0.85f, 1.15f);
			break;
		case "starfishGrab":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.StarfishGrab, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "starfishGrabChance":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.StarfishGrab, 50, checkForDuplicity: false, Vector2.zero, 0.8f, 0.9f);
			break;
		case "kitsuneCast":
			GC.PlaySFX(GameCore.SFXBasic1.KitsuneCast1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "kitsuneGiggle":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.KitsuneGiggle, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "fairyGiggle":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FairyLaugh, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "fairySkill":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FairySkill, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "salamanderTongue":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.SalaTongue, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "genericSwing1":
			GC.PlaySFX(GameCore.SFXBasic1.SwingTK1, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "genericSwing2":
			GC.PlaySFX(GameCore.SFXBasic1.SwingTK2, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "genericDefend1":
			GC.PlaySFX(GameCore.SFXBasic1.Defend1, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "dizzy1":
			GC.PlaySFX(GameCore.SFXBasic2.Dizzy1, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "steps1":
			GC.PlaySFX(GameCore.SFXBasic2.Steps1, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "steps2":
			GC.PlaySFX(GameCore.SFXBasic2.Steps1, 0, checkForDuplicity: false, base.transform.position, 1.95f, 2.05f);
			break;
		case "steps3":
			GC.PlaySFX(GameCore.SFXBasic2.Steps1, 50, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "grow1":
			GC.PlaySFX(GameCore.SFXBasic2.Grow1, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			break;
		case "weird1":
			GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			break;
		case "odd1":
			GC.PlaySFX(GameCore.SFXBasic2.Odd1, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "odd2":
			GC.PlaySFX(GameCore.SFXBasic2.Odd2, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "odd4":
			GC.PlaySFX(GameCore.SFXBasic2.Odd4, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "odd5":
			GC.PlaySFX(GameCore.SFXBasic2.Odd5, 0, checkForDuplicity: false, base.transform.position, 1.2f, 1.3f);
			break;
		case "odd6":
			GC.PlaySFX(GameCore.SFXBasic2.Odd6, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "howl1":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MimicHowl, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "arach-Giggle":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.ArachneLaugh, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "arach-Climax":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.ArachneClimax, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
			break;
		case "bounce":
			GC.PlaySFX(GameCore.SFXBasic1.Bounce, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.1f);
			break;
		case "boing1":
			GC.PlaySFX(GameCore.SFXBasic2.Boing1, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.1f);
			break;
		case "boing2":
			GC.PlaySFX(GameCore.SFXBasic2.Boing2, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			break;
		case "boing2L":
			GC.PlaySFX(GameCore.SFXBasic2.Boing2, 0, checkForDuplicity: false, base.transform.position, 0.8f, 0.85f);
			break;
		case "boing3":
			GC.PlaySFX(GameCore.SFXBasic2.Boing3, 0, checkForDuplicity: false, base.transform.position, 0.85f, 1.1f);
			break;
		case "boing4":
			GC.PlaySFX(GameCore.SFXBasic2.Boing4, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			break;
		case "boing4high":
			GC.PlaySFX(GameCore.SFXBasic2.Boing4, 0, checkForDuplicity: false, base.transform.position, 1.2f, 1.4f);
			break;
		case "boing-rng":
			GC.PlaySFX(GameCore.SFXBasic2.BoingRandom, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.1f);
			break;
		case "swing-rng":
			GC.PlaySFX(GameCore.SFXBasic1.Swing2, 0, checkForDuplicity: false, base.transform.position, 1.5f, 2f);
			break;
		case "bounce1":
			GC.PlaySFX(GameCore.SFXBasic1.Bounce, 0, checkForDuplicity: false, base.transform.position, 0.85f, 1.15f);
			break;
		case "lamiaClaw":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.LamiaAttack2, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.1f);
			break;
		case "lamiaMouth":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.LamiaOpenMouth, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.1f);
			break;
		case "lamiaSnake":
			GC.PlaySFX(GameCore.SFXBasic2.LamiaSkillAppear, 0, checkForDuplicity: false, Vector2.zero, 1.2f, 1.3f);
			break;
		case "transformation1":
			GC.PlaySFX(GameCore.SFXBasic2.Transformation1, 0, checkForDuplicity: false, Vector2.zero, 1.1f, 1.2f);
			break;
		case "swing-high":
			GC.PlaySFX(GameCore.SFXBasic2.Swing2, 0, checkForDuplicity: false, Vector2.zero, 1f, 1.25f);
			break;
		case "bat-aah":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BatAhh, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "bat-bite":
			GC.PlaySFX(GameCore.SFXBasic1.Bite1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "plat-power-on":
			GC.PlaySFX(GameCore.SFXBasic2.PlatPowerOn, 0, checkForDuplicity: false, Vector2.zero, 1.1f, 1.3f);
			break;
		case "futaGhost-ring":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FutaGhostRing, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case "futaGhost-ring2":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FutaGhostRing2, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case "swampFairy-thunder":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.SwampFairyThunder, 0, checkForDuplicity: true, base.transform.position, 0.85f, 1.15f);
			break;
		case "fast1":
			GC.PlaySFX(GameCore.SFXBasic1.Fast1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swing1":
			GC.PlaySFX(GameCore.SFXBasic2.Swing1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swing3":
			GC.PlaySFX(GameCore.SFXBasic2.Swing3, 0, checkForDuplicity: false, Vector2.zero, 1.9f, 2.1f);
			break;
		case "swing4":
			GC.PlaySFX(GameCore.SFXBasic2.Swing4, 0, checkForDuplicity: false, Vector2.zero, 1f, 1.1f);
			break;
		case "swing5":
			GC.PlaySFX(GameCore.SFXBasic2.Swing5, 0, checkForDuplicity: false, Vector2.zero, 1.1f, 1.2f);
			break;
		case "swingTK3":
			GC.PlaySFX(GameCore.SFXBasic1.SwingTK3, 0, checkForDuplicity: false, Vector2.zero, 0.95f, 1.05f);
			break;
		case "wooden1":
			GC.PlaySFX(GameCore.SFXBasic2.Wooden1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "wooden2":
			GC.PlaySFX(GameCore.SFXBasic2.Wooden2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "swingHeavy":
			GC.PlaySFX(GameCore.SFXBasic2.Swing5, 0, checkForDuplicity: true, base.transform.position, 1f, 1f);
			break;
		case "hooves2":
			PlaySFXGeneric(10, 1.1f, 1.1f);
			break;
		case "jumpEnd2":
			GC.PlaySFX(GameCore.SFXBasic1.JumpEnd2, 0, checkForDuplicity: true, base.transform.position, 0.85f, 1.15f);
			break;
		case "hitTouch2":
			GC.PlaySFX(GameCore.SFXBasic1.HitHurtTouch2, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "hitTouch3":
			GC.PlaySFX(GameCore.SFXBasic1.HitHurtTouch3, 0, checkForDuplicity: true, base.transform.position, 1f, 1f);
			break;
		case "gallop":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CTG_Gallop, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "princessLaugh":
			GC.PlaySFXGeneric("PrincessBoss", 15, 0, checkForDuplicity: false, base.transform.position, 0f, 0f, "voice");
			break;
		case "princessIce":
			GC.PlaySFXGeneric("PrincessBoss", 11, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.1f, "sfx");
			break;
		case "princessIce2":
			GC.PlaySFXGeneric("PrincessBoss", 13, 0, checkForDuplicity: false, base.transform.position, 0f, 0f, "sfx");
			break;
		case "LatexMaidenSwing":
			GC.PlaySFX(GameCore.SFXBasic2.Swing5, 0, checkForDuplicity: true, base.transform.position, 0.75f, 0.85f, "LatexMaiden");
			break;
		case "LatexMaidenOpen":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.LatexMaiden_Open, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f, "LatexMaiden");
			break;
		case "LatexMaidenClose":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.LatexMaiden_Close, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f, "LatexMaiden");
			break;
		case "dommeSwing":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.Domme_Swing, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f, "Dominatrix");
			break;
		case "dommeWhip":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.Domme_Crack, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f, "Dominatrix");
			break;
		case "paintSplat":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.PaintingEvil_Splat, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case "dogPanting1":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CityDogPanting, 25, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case "pirateSpin":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.PirateSpin, 0, checkForDuplicity: true, base.transform.position, 0.85f, 1.15f);
			break;
		case "wormMelee":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WormMelee, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "witchLaugh01":
			GC.PlaySFXGeneric("WitchBoss", 4, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "voice");
			break;
		case "witchLaugh02":
			GC.PlaySFXGeneric("WitchBoss", 5, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "voice");
			break;
		case "witchLaugh03":
			GC.PlaySFXGeneric("WitchBoss", 6, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "voice");
			break;
		case "witchLaugh04":
			GC.PlaySFXGeneric("WitchBoss", 7, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "voice");
			break;
		case "witchLaugh05":
			GC.PlaySFXGeneric("WitchBoss", 8, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "voice");
			break;
		case "witchBoing":
			GC.PlaySFX(GameCore.SFXBasic2.Boing2, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.15f);
			break;
		case "witchBoingHeavy":
			GC.PlaySFX(GameCore.SFXBasic2.Boing2, 0, checkForDuplicity: false, base.transform.position, 0.8f, 0.9f);
			break;
		case "witchMagic":
			GC.PlaySFXGeneric("WitchBoss", 27, 50, checkForDuplicity: true, base.transform.position, 1.3f, 1.5f, "sfx");
			break;
		case "witchMoan01":
			GC.PlaySFXGeneric("WitchBoss", 30, 70, checkForDuplicity: true, base.transform.position, 1f, 1.15f, "voice", "WitchMoan");
			break;
		case "witchMoan02":
			GC.PlaySFXGeneric("WitchBoss", 31, 80, checkForDuplicity: true, base.transform.position, 1f, 1.15f, "voice", "WitchMoan");
			break;
		case "witchMoan03":
			GC.PlaySFXGeneric("WitchBoss", 32, 90, checkForDuplicity: true, base.transform.position, 1f, 1.15f, "voice", "WitchMoan");
			break;
		case "witchMoanClimax":
			GC.PlaySFXGeneric("WitchBoss", 33, 0, checkForDuplicity: true, base.transform.position, 1f, 1.15f, "voice", "WitchMoan");
			break;
		case "WyvRoar":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WyvernBirth, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "BeeBuzz":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BeeGirlCast, 0, checkForDuplicity: true, base.transform.position, 1.25f, 1.35f);
			break;
		case "FLR-Roar":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FLR_Cast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "FHB-Tentacle":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FHB_Tentacles, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "BFox-Electric":
			GC.PlaySFXGeneric("BlueFoxBoss", 15, 0, checkForDuplicity: true, base.transform.position, 0.75f, 0.95f, "sfx");
			break;
		case "Medusa-Petrify":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MDS_Petr, 0, checkForDuplicity: true, base.transform.position, 1.1f, 1.2f);
			break;
		case "BullScream":
			PlaySFXGeneric(4);
			break;
		case "nyaClimax":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CatNya2, 0, checkForDuplicity: true, base.transform.position, 1f, 1.2f);
			break;
		case "nyaOther":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CatNyaOther, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "nyaOther%50":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CatNyaOther, 60, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "nya3":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CatNya3, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "nya4%50":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CatNya4, 60, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case "nya5%50":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CatNya5, 60, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		}
	}

	private void DoEnemyDefeatedSound()
	{
		GameCore.MonsterSounds monsterSounds = GameCore.MonsterSounds.Null;
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Isobora:
			monsterSounds = GameCore.MonsterSounds.IsoboraDead;
			break;
		case GameCore.EnemyType.BlazePlant:
			monsterSounds = GameCore.MonsterSounds.SpitBudDead;
			break;
		case GameCore.EnemyType.NaughtyCoral:
			monsterSounds = GameCore.MonsterSounds.BarnacleDead;
			break;
		case GameCore.EnemyType.LazyStarfish:
			monsterSounds = GameCore.MonsterSounds.StarfishDead;
			break;
		case GameCore.EnemyType.BeachKitsune:
			monsterSounds = GameCore.MonsterSounds.KitsuneDead;
			break;
		case GameCore.EnemyType.Dragonfly:
			monsterSounds = GameCore.MonsterSounds.DFLYDead;
			break;
		case GameCore.EnemyType.CrystalChunk:
			monsterSounds = GameCore.MonsterSounds.Crystal_Dead;
			break;
		case GameCore.EnemyType.HillFairy:
			monsterSounds = GameCore.MonsterSounds.FairyDead;
			break;
		case GameCore.EnemyType.SwampFairy:
			monsterSounds = GameCore.MonsterSounds.SwampFairy_Dead;
			break;
		case GameCore.EnemyType.LizardShield:
		case GameCore.EnemyType.LizardAxe:
			monsterSounds = GameCore.MonsterSounds.LizardDead;
			break;
		case GameCore.EnemyType.WolfGirlMimic:
			GC.PlaySFX(GameCore.SFXBasic2.Broken1, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.WaterJelly:
			PlayMonsterSoundsFromAnim("boing4");
			break;
		case GameCore.EnemyType.Lamia:
			monsterSounds = GameCore.MonsterSounds.LamiaDead;
			break;
		case GameCore.EnemyType.MushroomGirl:
			monsterSounds = GameCore.MonsterSounds.MushroomGirlDeath;
			break;
		case GameCore.EnemyType.Batgirl:
			monsterSounds = GameCore.MonsterSounds.BatDead;
			break;
		case GameCore.EnemyType.FutaGhost:
			monsterSounds = GameCore.MonsterSounds.FTG_Dead;
			break;
		case GameCore.EnemyType.GhostButler:
			monsterSounds = GameCore.MonsterSounds.GSB_Grunt2;
			break;
		case GameCore.EnemyType.TentacleTree:
			GC.PlaySFX(GameCore.SFXBasic2.LowExplosion1, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.MothGirl:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MothDead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.DwarfGirl:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.DwarfDeath, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.SwampGolem:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MothSkillExplode, 0, checkForDuplicity: true, base.transform.position, 0.7f, 0.71f);
			break;
		case GameCore.EnemyType.CentaurGirl:
			monsterSounds = GameCore.MonsterSounds.CTG_Dead;
			break;
		case GameCore.EnemyType.PurpleSlime:
			GC.PlaySFX(GameCore.SFXBasic2.Boing1, 0, checkForDuplicity: true, base.transform.position, 0.5f, 0.6f);
			break;
		case GameCore.EnemyType.PrisonBrute:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.GSB_Grunt1, 0, checkForDuplicity: true, base.transform.position, 0.5f, 0.6f);
			break;
		case GameCore.EnemyType.PaintingEvil:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.PaintingEvil_Splat, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case GameCore.EnemyType.LatexMaiden:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.LatexMaiden_Dead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.Dominatrix:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.Domme_Dead, 0, checkForDuplicity: true, base.transform.position, 1f, 1.05f);
			break;
		case GameCore.EnemyType.Dogboy:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CityDogBabyBark, 0, checkForDuplicity: true, base.transform.position, 0.7f, 0.8f);
			break;
		case GameCore.EnemyType.FutaBunny:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BunnyDead, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case GameCore.EnemyType.SailorBurly:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.PirateDead, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case GameCore.EnemyType.WormOverGround:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WormDead, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case GameCore.EnemyType.Bot1:
		case GameCore.EnemyType.Bot2:
		case GameCore.EnemyType.Bot3:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.RobotExplode, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.WyvernGirl:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WyvernDead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.BeeGirl:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BeeGirlDead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.FutaDruidCentauress:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FutaCentaurDead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.BigInsect:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BIS_Dead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.Failed:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FLR_Dead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.FleshBlossom:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FHB_Dead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.CorruptBeast:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CRB_Dead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.Sinccubus:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.SCB_Dead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.DarkDamsel:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.DKD_Dead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.Medusa:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MDS_Dead, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.FairySorrow:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FairySorrowDead, 0, checkForDuplicity: true, base.transform.position, 1.2f, 1.3f);
			break;
		case GameCore.EnemyType.SlimeGirl:
			PlayMonsterSoundsFromAnim("boing4high");
			break;
		case GameCore.EnemyType.Catgirl:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CatNya1, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.Goblin:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.GSB_Grunt2, 0, checkForDuplicity: true, base.transform.position, 1.7f, 1.8f);
			break;
		}
		if (monsterSounds != GameCore.MonsterSounds.Null)
		{
			GC.PlaySFXMonsterSounds(monsterSounds, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
		}
	}

	private void PlayBossDeathSound()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.BossForest)
		{
			PlaySFXGeneric(13);
		}
	}

	public void PlaySFXGeneric(int sfxID)
	{
		PlaySFXGeneric(sfxID, 0f, 0f);
	}

	public void PlaySFXGeneric(int sfxID, float pitchMin = 0f, float pitchMax = 0f)
	{
		string text = "";
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.BossForest:
			text = GameCore.SpeakerID.ForestBoss.ToString();
			break;
		case GameCore.EnemyType.BossHill:
			text = GameCore.SpeakerID.HillBoss.ToString();
			break;
		case GameCore.EnemyType.BossWerewolf:
			text = GameCore.SpeakerID.WerewolfBoss.ToString();
			break;
		case GameCore.EnemyType.BossMinotaur:
			text = GameCore.SpeakerID.MinotaurBoss.ToString();
			break;
		case GameCore.EnemyType.BossPrincess:
			text = GameCore.SpeakerID.PrincessBoss.ToString();
			break;
		case GameCore.EnemyType.BossWitch:
			text = GameCore.SpeakerID.WitchBoss.ToString();
			break;
		case GameCore.EnemyType.BossBlueFox:
			text = GameCore.SpeakerID.BlueFoxBoss.ToString();
			break;
		case GameCore.EnemyType.BossSwampSlime:
			text = GameCore.SpeakerID.SwampBoss.ToString();
			break;
		}
		if (text != "")
		{
			GC.PlaySFXGeneric(text, sfxID, 0, checkForDuplicity: false, Vector2.zero, pitchMin, pitchMax, "voice");
		}
		else
		{
			Debug.Log("ENEMY_LOGIC:PlaySFXGeneric() -> Tried to play a generic sound but the enemy type was not expected! Current Enemy: " + currentEnemyType);
		}
	}

	public void PlayChaseStartSound()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.Dragonfly)
		{
			InvokeRepeating("RepeatingSound", 0f, 2f);
		}
	}

	private void PlaySoundJump()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.WaterJelly)
		{
			PlayMonsterSoundsFromAnim("boing2");
		}
	}

	private void RepeatingSound()
	{
		if (currentFlyingState != enemyFlyingState.Chasing)
		{
			CancelInvoke("RepeatingSound");
			return;
		}
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.Dragonfly)
		{
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.DFLYSkill, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
		}
	}

	public void StartDeathLogicBehaviour()
	{
		if (isDeath)
		{
			return;
		}
		if (eroCore.IsAllowedToGrab)
		{
			eroCore.IsAllowedToGrab = false;
		}
		if (isGrabbing)
		{
			localEroScript.EndEroLogic();
		}
		DisableSkillsAndMiscObjects();
		StopAllCoroutines();
		CancelInvoke();
		DoSpecificStuffOnMonsterDeath();
		InvokeRepeating("FadeOutLifeBar", 0f, 0.1f);
		isStagger = true;
		isDeath = true;
		if (rotationZ != 0f)
		{
			rotationZ = 0f;
		}
		if (enmMisc.maintainCollisionOnDeath)
		{
			base.gameObject.layer = LayerMask.NameToLayer("DeadEnemies");
			if (enemyCurrentYMove < 1f)
			{
				enemyCurrentYMove = 1f;
			}
		}
		else
		{
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = 0f;
			if ((bool)objRB)
			{
				objRB.isKinematic = true;
			}
			objBox2D.enabled = false;
		}
		if ((bool)objRB && enmMisc.staticDeathLocation)
		{
			objRB.bodyType = RigidbodyType2D.Static;
		}
		objAN.SetBool("isStagger", value: true);
		objAN.SetBool("isDeath", value: true);
		DoEnemyDefeatedSound();
		StartStopSpriteFlashing(start: false, flashType.HalfWhite, 0f);
		CheckValidEssenceSpawnSituation();
		if (GC.ACC_Check(GameCore.AccessoryBehavior.cheapDiscount))
		{
			GC.discountShopAcumulation += enmBaseStats.maxLife;
		}
		if (isGauntletSpecial)
		{
			GC.GAUNTLET_AddKillCount(1);
		}
	}

	private void DisableSkillsAndMiscObjects()
	{
		_ = currentEnemyType;
		if ((bool)objAN)
		{
			objAN.SetInteger("genericPhase", 0);
		}
		for (int i = 0; i < enmMisc.Objects.Length; i++)
		{
			if (enmMisc.Objects[i] != null && enmMisc.Objects[i].activeInHierarchy)
			{
				enmMisc.Objects[i].SetActive(value: false);
			}
		}
	}

	private void DoSpecificStuffOnMonsterDeath()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.FlowerBlossom:
			GetMiscGameObject(3).SetActive(value: false);
			break;
		case GameCore.EnemyType.FutaGhost:
			if ((isUsingSkill || objAN.GetBool("isSkill")) && (bool)GC.enemyGhostFutaSkillCasting)
			{
				GC.enemyGhostFutaSkillCasting = null;
			}
			break;
		case GameCore.EnemyType.PurpleSlime:
			GC.quantityOfSpawnedPurpleSlimes--;
			if (GC.quantityOfSpawnedPurpleSlimes < 0)
			{
				GC.quantityOfSpawnedPurpleSlimes = 0;
			}
			break;
		case GameCore.EnemyType.PaintingEvil:
			colorGeneric.a = 1f;
			objRE.color = colorGeneric;
			break;
		}
	}

	private void CheckValidEssenceSpawnSituation()
	{
		bool flag = false;
		GameCore.EssenceSpawnType typeToSpawn = GameCore.EssenceSpawnType.EnemyDefeated;
		if (currentEnemyType != GameCore.EnemyType.TargetDummy && !(GC.strCurrentActiveSceneName == "AnimGallery"))
		{
			if (eroCount10 && GC.ACC_Check(GameCore.AccessoryBehavior.HeadSuccubus))
			{
				flag = true;
				typeToSpawn = GameCore.EssenceSpawnType.SuccubusHeadress;
			}
			if (GC.MATH_RNG_ROLL(GC.chanceEnemySpawnEssence) || (GC.MATH_RNG_ROLL(GC.chanceEnemySpawnEssence * 2) && GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.FruitWarrior)))
			{
				flag = true;
			}
			if (_isRagePepper)
			{
				typeToSpawn = GameCore.EssenceSpawnType.ragePepper;
				flag = true;
			}
			if (flag)
			{
				GC.ESSENCE_Spawn(typeToSpawn, GetEnemyCenterPos(), enmBaseStats.maxLife);
			}
		}
	}

	private void StartDeathLogicBoss()
	{
		if (!GC.isBossRematch)
		{
			GC.ESSENCE_Spawn(GameCore.EssenceSpawnType.BossDefeated, GetEnemyCenterPos(), enmBaseStats.maxLife);
		}
		if (eroCore.IsAllowedToGrab)
		{
			eroCore.IsAllowedToGrab = false;
		}
		if (isGrabbing)
		{
			localEroScript.EndEroLogic();
		}
		StopAllCoroutines();
		CancelInvoke();
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(0f, enmBaseStats.maxLife);
		}
		PlayBossDeathSound();
		enemyCurrentXMove = 0f;
		if (!enmMisc.isFlyer)
		{
			enemyCurrentYMove = 0f;
		}
		else
		{
			enemyCurrentYMove = -1f;
		}
		posGenericVector = base.transform.position;
		posGenericVector.y -= 0.5f;
		isDeath = true;
		objBox2D.enabled = false;
		GC.DisplaySystemMessageUI(3, enmBaseStats.idEnemyNameInFile, 5f);
		GC.DoStuffAfterBossDeath(currentEnemyType, doGenericToggles: true);
		if (GC.isBossRematch)
		{
			GC.isBossRematch = false;
		}
		foreach (Transform item in base.gameObject.transform)
		{
			if (item.gameObject.activeSelf)
			{
				item.gameObject.SetActive(value: false);
			}
		}
		BossDefaultDeathAnimationExplosions();
	}

	private void BossDefaultDeathAnimationExplosions()
	{
		if (CheckIfDefaultBossDeathAnim())
		{
			objAN.enabled = false;
			objRE.sprite = enmObjRefs.specialSprite;
			if ((bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: true, 0);
			}
			else
			{
				Debug.Log("This boss was defeated but it has no 2D shader script attached! Death effect can't be displayed.");
			}
			if ((bool)enmObjRefs.deathSpawnEffect && (bool)GC.refOBJEffectsParent)
			{
				UnityEngine.Object.Instantiate(enmObjRefs.deathSpawnEffect, base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
			if (crtFinishBossDeathAnimation != null)
			{
				StopCoroutine(crtFinishBossDeathAnimation);
			}
			crtFinishBossDeathAnimation = StartCoroutine(waitToFinishBossDeathAnimation(4f));
			GC.PlaySFX(GameCore.SFXBasic2.BossDeathExplode, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			StartStopSpriteFlashing(start: false, flashType.HalfWhite, 0f);
		}
	}

	private bool CheckIfDefaultBossDeathAnim()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.BossBlueFox)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.BlueFoxTPAway, base.transform.position, "EnemyLogic:BlueFox:DefeatedAnim");
			if (crtFinishBossDeathAnimation != null)
			{
				StopCoroutine(crtFinishBossDeathAnimation);
			}
			crtFinishBossDeathAnimation = StartCoroutine(waitToFinishBossDeathAnimation(0f));
			return false;
		}
		return true;
	}

	private void FadeOutLifeBar()
	{
		if ((bool)enmObjRefs.imgHealthFrame && (bool)enmObjRefs.imgHealthBG)
		{
			colorFadeOutLifeBar = enmObjRefs.imgHealthFrame.color;
			colorFadeOutLifeBar.a -= 0.2f;
			enmObjRefs.imgHealthFrame.color = colorFadeOutLifeBar;
			enmObjRefs.imgHealthBG.color = colorFadeOutLifeBar;
		}
		if (colorFadeOutLifeBar.a <= 0f)
		{
			CancelInvoke("FadeOutLifeBar");
		}
	}

	public void FinalDeathFrame()
	{
		CancelInvoke();
		base.gameObject.SetActive(value: false);
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.WormOverGround)
		{
			base.transform.parent.gameObject.SetActive(value: false);
		}
	}

	private void DisplayDamageUI(int amountDMG, int amountBlockedDMG, Color mainColor, string hitName = "")
	{
		if (hitName == "dot_poison")
		{
			amountBlockedDMG = 0;
		}
		if ((bool)GC.refOBJEffectsParent)
		{
			DisplayDamage component = UnityEngine.Object.Instantiate(enmObjRefs.objTextDamageUI, base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<DisplayDamage>();
			component.GetComponent<DisplayDamage>().dmgPrimary = amountDMG;
			component.GetComponent<DisplayDamage>().dmgOptional = amountBlockedDMG;
			component.GetComponent<DisplayDamage>().showBlockedDamage = GC.canShowBlockedDamage;
			component.GetComponent<DisplayDamage>().colorToDisplay = mainColor;
			component.GetComponent<DisplayDamage>().colorToDisplayShadow = Color.black;
		}
	}

	private void UpdateHealthBarDisplay()
	{
		ToggleLocalHPCanvas(stateON: true);
		if ((bool)enmObjRefs.imgHealthFill)
		{
			enmObjRefs.imgHealthFill.fillAmount = (float)amountLife / (float)enmBaseStats.maxLife;
		}
		if (crtDisplayHealthBar != null)
		{
			StopCoroutine(crtDisplayHealthBar);
		}
		crtDisplayHealthBar = StartCoroutine(waitForHealthDisplay(0.7f));
	}

	private void ToggleLocalHPCanvas(bool stateON)
	{
		if (!enmObjRefs.localHPCanvas)
		{
			return;
		}
		if (stateON)
		{
			if (!enmObjRefs.localHPCanvas.activeInHierarchy)
			{
				enmObjRefs.localHPCanvas.SetActive(value: true);
			}
		}
		else
		{
			enmObjRefs.localHPCanvas.SetActive(value: false);
		}
	}

	private void HACK_FIX_CHILD_CANVAS_SCALE_FLIPPING()
	{
		if (!(enmObjRefs.localHPCanvas == null) && (((float)Mathf.RoundToInt(base.transform.localScale.x) == 1f && enmObjRefs.localHPCanvas.transform.localScale.x != 0.01f) || ((float)Mathf.RoundToInt(base.transform.localScale.x) == -1f && enmObjRefs.localHPCanvas.transform.localScale.x != -0.01f)))
		{
			Vector3 localScale = enmObjRefs.localHPCanvas.transform.localScale;
			localScale.x *= -1f;
			enmObjRefs.localHPCanvas.transform.localScale = localScale;
		}
	}

	private IEnumerator waitForHealthDisplay(float wait)
	{
		yield return new WaitForSeconds(wait);
		ToggleLocalHPCanvas(stateON: false);
		crtDisplayHealthBar = null;
	}

	public void PlayStopChildAnimation(int booleanINT)
	{
		bool value = false;
		if (booleanINT == 1)
		{
			value = true;
		}
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.FlowerBlossom:
		{
			float num = objAN.GetFloat("speedAnimation");
			if (num <= 0f)
			{
				num = 1f;
			}
			GetMiscGameObject(3).GetComponent<Animator>().SetBool("playAnimation", value);
			GetMiscGameObject(3).GetComponent<Animator>().SetFloat("speedAnimation", num);
			break;
		}
		case GameCore.EnemyType.CrystalChunk:
			UnityEngine.Object.Instantiate(GetMiscGameObject(0), base.transform.position, Quaternion.identity).transform.localScale = base.transform.localScale;
			GC.isPlayerGoneForGood = true;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitGENERIC(0.1f, "CrystalPlayerSpawner"));
			break;
		case GameCore.EnemyType.Manequin:
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "EnemyLogic");
			GC.isPlayerGoneForGood = true;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitGENERIC(0.1f, "ManequinPossession"));
			break;
		case GameCore.EnemyType.PurpleSlime:
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "EnemyLogic");
			GC.PlaySFX(GameCore.SFXBasic1.SpawnerNoise1, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			GC.isPlayerGoneForGood = true;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitGENERIC(0.1f, "PurpleSlimeAbsorb"));
			break;
		case GameCore.EnemyType.PaintingEvil:
			GC.isPlayerGoneForGood = true;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitGENERIC(0.1f, "EvilPaintingPortrait"));
			break;
		case GameCore.EnemyType.WormOverGround:
			if (isGalleryView)
			{
				base.gameObject.SetActive(value: false);
				break;
			}
			boolGeneric = false;
			base.transform.parent.GetComponent<EnemyGeneralLogic>().enabled = true;
			base.transform.parent.GetComponent<EnemyGeneralLogic>().WORMDIG_ToggleSelfState(stateON: true);
			break;
		case GameCore.EnemyType.SlimeGirl:
			GC.PlaySFX(GameCore.SFXBasic1.SpawnerNoise1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f, "slimeGirl");
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, base.transform.position, "EnemyLogic");
			GC.isPlayerGoneForGood = true;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitGENERIC(0.1f, "SlimeGirlMayaFat"));
			GC.DisplaySystemMessageUI(0, 118, 5f, "", isFlavorCentralMessage: true);
			break;
		case GameCore.EnemyType.BossMinotaur:
			if (countBEStuff == 1)
			{
				countBEStuff++;
				Vector2 vector = base.transform.position;
				vector.x += 0.75f;
				vector.y -= 0.3f;
				UnityEngine.Object.Instantiate(GetMiscGameObject(9), vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
			break;
		case GameCore.EnemyType.DarkDamsel:
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "EnemyLogic");
			GC.PlaySFX(GameCore.SFXBasic1.SpawnerNoise1, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			GC.isPlayerGoneForGood = true;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitGENERIC(0.1f, "DarkDamselAbsorb"));
			break;
		case GameCore.EnemyType.SwampFairy:
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "EnemyLogic");
			GC.PlaySFX(GameCore.SFXBasic1.SpawnerNoise1, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			GC.isPlayerGoneForGood = true;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitGENERIC(0.1f, "SwampFairyAbsorb"));
			break;
		}
	}

	private bool CheckSpecialEnemyBehaviourOnEroStart()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.BossBeach:
			StopAllCoroutines();
			SpecialBeachBossStopSlash();
			if (!isGalleryView)
			{
				GC.GlobalApplyDamageToPlayer((int)GC.MATH_ResultValueOfTotalPercentile(20f, GC.playerMaxHealth), Vector3.zero);
			}
			isMoveToPoint = false;
			break;
		case GameCore.EnemyType.FutaGhost:
			if (GC.isPlayerFainted)
			{
				base.transform.position = new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition + 0.75f, base.transform.position.z);
			}
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FTG_Cast_Random, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.15f, "FutaGhost");
			break;
		case GameCore.EnemyType.GhostButler:
			if (GC.playerLastUnstuckPosition != Vector2.zero && !isGalleryView)
			{
				base.transform.position = GC.playerLastUnstuckPosition;
			}
			break;
		case GameCore.EnemyType.LatexMaiden:
			if (!GC.isPlayerFainted && GC.playerCurrentDressState == 2 && !isGalleryView)
			{
				GC.GlobalApplyDamageToPlayer(GC.playerMaxHealth / 2, Vector3.zero);
			}
			break;
		case GameCore.EnemyType.BossSwampSlime:
			if (objAN.GetInteger("genericPhase") != 0)
			{
				objAN.SetInteger("genericPhase", 0);
			}
			break;
		}
		return true;
	}

	public void StartRotationShakeEffect(float intensity, float decayFreq)
	{
		preShakePos = base.transform.position;
		preShakeRot = base.transform.rotation;
		shakeDecayRate = decayFreq;
		shakeIntensity = intensity;
	}

	private void Invoked_Manequin_PrincessBossHack()
	{
		if (GC.strCurrentActiveSceneName == "Stage0-BossChamber")
		{
			if (objGeneric == null)
			{
				objGeneric = GameObject.FindGameObjectWithTag("Enemy-Boss");
				return;
			}
			float num = base.transform.position.x - objGeneric.transform.position.x;
			if (num < 0f)
			{
				num *= -1f;
			}
			float num2 = base.transform.position.y - objGeneric.transform.position.y;
			if (num2 < 0f)
			{
				num2 *= -1f;
			}
			if (isGrabbing && num <= 2f && num2 <= 2f)
			{
				base.gameObject.GetComponent<EroAnimation>().EndEroLogic();
			}
		}
		else
		{
			CancelInvoke("Invoked_Manequin_PrincessBossHack");
		}
	}

	private void CHASE_InvokeFaintedChaseLogic()
	{
		if (!IsInvoking(strInvokeFaintedChaseName))
		{
			float repeatRate = GC.MATH_RNG_FLOAT(1f, 2f);
			InvokeRepeating(strInvokeFaintedChaseName, 1f, repeatRate);
		}
	}

	private void CHASE_InvokedCheckFaintedChaseLogic()
	{
		CHASE_ValidateFaintedChaseFetishes();
		if (CHASE_ValidateFaintedChaseValues() && (!GC.isPlayerFainted || isGrabbing || !GC.playerIsBeingGrabed || !GC.isRunningBadEndLogic))
		{
			int chaseChance = 15;
			AlterBaseChaseChance(ref chaseChance);
			if (enmCmnBehaviour.durationFaintedChase > 0 && GC.MATH_RNG_INT(1, 100) <= chaseChance)
			{
				ExecuteFaintedChaseLogic(typeOfFaintedChase, enmCmnBehaviour.durationFaintedChase);
			}
		}
	}

	private void ExecuteFaintedChaseLogic(faintedChaseType typeOfChase, float totalTimeOfChase)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		switch (typeOfChase)
		{
		case faintedChaseType.Basic:
			if (crtChaseFainted != null)
			{
				StopCoroutine(crtChaseFainted);
			}
			crtChaseFainted = StartCoroutine(waitForChasingFainted(totalTimeOfChase));
			break;
		case faintedChaseType.Flying:
			isChasingFainted = true;
			currentFlyingState = enemyFlyingState.Chasing;
			break;
		}
	}

	private void CHASE_ValidateFaintedChaseFetishes()
	{
		if (ValidateFetishSoloScene() || ValidateFetishComboScene())
		{
			canChaseFainted = true;
		}
		else
		{
			canChaseFainted = false;
		}
	}

	private void CHASE_StandToWatch()
	{
		CHASE_END();
		enemyCurrentXMove = 0f;
		countFrequency = 0f;
		SetNewLimitIdleTime(doMax: true);
		currentEnemyState = enemyState.Standing;
		STATE_MACHINE_FLIP_FACE_PLAYER();
	}

	private bool CHASE_ValidateFaintedChaseValues()
	{
		if (isStagger)
		{
			return false;
		}
		if (CHASE_ValidateSpecialChases())
		{
			return true;
		}
		if ((GC.isPlayerFainted || CHASE_ValidateChaseAliveGrabbed() || GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified)) && canChaseFainted && !isChasingFainted && !GC.isPlayerGoneForGood && !isGrabbing && eroCore.IsAllowedToGrab && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.FaintedChase, enmCmnBehaviour.rangeFaintedChaseX, enmCmnBehaviour.rangeFaintedChaseY) && GC.EnemyChaseBellowLimit())
		{
			return true;
		}
		return false;
	}

	private bool CHASE_ValidateSpecialChases()
	{
		if (_isRagePepper || _isRageGauntlet)
		{
			return true;
		}
		if ((isGalleryView || isAddComboGalleryView) && !isGrabbing)
		{
			return true;
		}
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Charmed) && canChaseFainted && !isChasingFainted)
		{
			return true;
		}
		if (currentEnemyType == GameCore.EnemyType.WyvernGirl && enmMisc.isPlayerTransformed && objGeneric != null && canChaseFainted && !isChasingFainted)
		{
			return true;
		}
		return false;
	}

	private bool CHASE_ValidateChaseAliveGrabbed()
	{
		if (!GC.isPlayerFainted && GC.playerIsBeingGrabed && eroCore.canChasePlayerAliveGrabbed)
		{
			return true;
		}
		return false;
	}

	private void CHASE_BEGIN()
	{
		if (!isChasingFainted)
		{
			GC.EnemyChaseAmountAdd();
			CHASE_PlaySoundAtChaseStart();
			if (!enmBaseStats.isBoss)
			{
				posGenericVector = Vector3.zero;
			}
			isJumpDelay = true;
			crtJumpDelay = StartCoroutine(waitForJumpDelay(0.5f));
			isChasingFainted = true;
		}
	}

	private void CHASE_END()
	{
		if (isChasingFainted)
		{
			GC.EnemyChaseAmountSubtract();
			isChasingFainted = false;
			crtChaseFainted = null;
			if (crtChaseFaintedDelay != null)
			{
				StopCoroutine(crtChaseFaintedDelay);
			}
			crtChaseFaintedDelay = StartCoroutine(waitForChasingFaintedDelay());
		}
	}

	private void AlterBaseChaseChance(ref int chaseChance)
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType != GameCore.EnemyType.Batgirl)
		{
			if (isGauntletSpecial)
			{
				chaseChance = 10;
			}
			if (isGalleryView || isAddComboGalleryView)
			{
				chaseChance = 50;
			}
			if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Charmed) || _isRagePepper)
			{
				chaseChance = 100;
			}
		}
		else if (GC.playerCurrentDressState == 2 || GC.isPlayerFainted)
		{
			chaseChance *= 2;
		}
	}

	private void CHASE_PlaySoundAtChaseStart()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.BeeGirl:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BeeGirlChase, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.Sinccubus:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.SCB_Chase, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		}
	}

	private IEnumerator waitForChasingFainted(float waitTime)
	{
		CHASE_BEGIN();
		yield return new WaitForSeconds(waitTime);
		CHASE_END();
	}

	private IEnumerator waitForChasingFaintedDelay()
	{
		float seconds = UnityEngine.Random.Range(minChaseDelay, maxChaseDelay);
		canChaseFainted = false;
		yield return new WaitForSeconds(seconds);
		canChaseFainted = true;
		crtChaseFaintedDelay = null;
	}

	public void CallEroLogic(GameObject objPlayer)
	{
		if (CheckSpecialEnemyBehaviourOnEroStart())
		{
			Vector3 zero = Vector3.zero;
			zero = base.gameObject.transform.position + (objPlayer.transform.position - base.gameObject.transform.position) / 2f;
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, zero, "ENEMYGENERALLOGIC:CallEroLogic");
			localEroScript.BeginEroLogic(objPlayer, offsetStruggleBarPosit, this, objAN, enmBaseStats.baseTouchDamage, eroCore.offsetDamageDisplay, eroCore.offsetPlayerHidden, eroCore.offsetPlayerRelease);
			base.transform.rotation = preShakeRot;
			if (isGalleryView)
			{
				GC.RemoveSpirit(100f);
			}
		}
	}

	private bool CanGrabPlayerForEro(string playerObjName)
	{
		if (isGalleryView && !isGrabbing && (bool)GC && !GC.isPlayerGoneForGood)
		{
			if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.LatexBodySuit) && currentEnemyType == GameCore.EnemyType.LatexMaiden)
			{
				return false;
			}
			return true;
		}
		if (!ValidateFetishSoloScene() || !ValidateEroGrabSpecialCases())
		{
			return false;
		}
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.FruitDistract))
		{
			return false;
		}
		if (playerObjName == "[PLAYER]" && GC.playerCurrentDressState == 2 && eroCore.IsAllowedToGrab && GC.playerIsAllowedToBeGrabbed && !GC.playerIsHurt && !GC.playerIsInvincible && !GC.playerIsBeingGrabed && eroCore.IsAllowedToGrab && !isGrabbing && !isStagger && !isIgnoreCollPlayer)
		{
			if (currentEnemyType == GameCore.EnemyType.BossBeach && sequenceOfBossSkill != 0)
			{
				return false;
			}
			return true;
		}
		if (playerObjName.Contains("[PLAYER-ERO]"))
		{
			if (GC.isPlayerFainted && !GC.playerIsBeingGrabed && eroCore.IsAllowedToGrab && !isGrabbing && GC.playerIsAllowedToBeGrabbed && !isStagger)
			{
				return true;
			}
			if (currentEnemyType == GameCore.EnemyType.BossForest && GC.isPlayerFainted && !GC.playerIsBeingGrabed && eroCore.IsAllowedToGrab && !isGrabbing)
			{
				return true;
			}
		}
		return false;
	}

	private bool ValidateEroGrabSpecialCases()
	{
		if (currentEnemyType == GameCore.EnemyType.LatexMaiden && !GetMiscGameObject(0).activeInHierarchy)
		{
			return false;
		}
		if (GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.LatexBodySuit) && currentEnemyType != GameCore.EnemyType.PrisonBrute && currentEnemyType != GameCore.EnemyType.Dominatrix && currentEnemyType != GameCore.EnemyType.PurpleSlime)
		{
			return false;
		}
		if (GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.SlimeMelt))
		{
			return false;
		}
		return true;
	}

	public void ERO_ToggleCountEroTotalTime(bool turnON)
	{
		if (turnON)
		{
			countEroTotalTime = 0;
			InvokeRepeating(strIVKEroTotalTimeCount, 0f, 1f);
		}
		else if (IsInvoking(strIVKEroTotalTimeCount))
		{
			CancelInvoke(strIVKEroTotalTimeCount);
		}
	}

	private void ERO_IVK_CountEroTotalTime()
	{
		countEroTotalTime++;
		if (GC.ACC_Check(GameCore.AccessoryBehavior.HeadSuccubus) && !eroCount10 && countEroTotalTime >= 10)
		{
			eroCount10 = true;
		}
	}

	private bool ValidateFetishSoloScene()
	{
		bool result = false;
		if (eroCore.eroSoloAlts != null && eroCore.eroSoloAlts.Length != 0)
		{
			listOfValidEroSceneVariantsID.Clear();
			for (int i = 0; i < eroCore.eroSoloAlts.Length; i++)
			{
				CFG_OverrideFetishToggle cFG_OverrideFetishToggle = GC.FetishOverrideCheckExistance(currentEnemyType, eroCore.eroSoloAlts[i].GenericID);
				if (cFG_OverrideFetishToggle != null)
				{
					if (cFG_OverrideFetishToggle.switcherOverride)
					{
						listOfValidEroSceneVariantsID.Add(eroCore.eroSoloAlts[i].GenericID);
					}
				}
				else if (GC.CompareFetishToggle(eroCore.eroSoloAlts[i].SceneTag))
				{
					listOfValidEroSceneVariantsID.Add(eroCore.eroSoloAlts[i].GenericID);
				}
			}
			if (listOfValidEroSceneVariantsID.Count > 0)
			{
				result = true;
			}
		}
		if (eroCore.FetishTags != null && eroCore.FetishTags.Length != 0 && (listOfValidEroSceneVariantsID == null || listOfValidEroSceneVariantsID.Count <= 0))
		{
			result = true;
			CFG_OverrideFetishToggle cFG_OverrideFetishToggle2 = GC.FetishOverrideCheckExistance(currentEnemyType, 0);
			if (cFG_OverrideFetishToggle2 != null)
			{
				result = cFG_OverrideFetishToggle2.switcherOverride;
			}
			else
			{
				for (int j = 0; j < eroCore.FetishTags.Length; j++)
				{
					if (!GC.CompareFetishToggle(eroCore.FetishTags[j]))
					{
						result = false;
						break;
					}
				}
			}
		}
		return result;
	}

	private bool ValidateFetishComboScene()
	{
		if (CHASE_ValidateChaseAliveGrabbed() && eroCore.eroComboScenes != null && eroCore.eroComboScenes.Length != 0)
		{
			List<ENM_EroComboScenes> list = new List<ENM_EroComboScenes>();
			for (int i = 0; i < eroCore.eroComboScenes.Length; i++)
			{
				if (eroCore.eroComboScenes[i].PairedWith == GC.enemyGrabbingPlayer)
				{
					list.Add(eroCore.eroComboScenes[i]);
				}
			}
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (!GC.CompareFetishToggle(list[num].SceneTag))
				{
					list.RemoveAt(num);
				}
			}
			if (list.Count > 0)
			{
				return true;
			}
		}
		return false;
	}

	public void CheckSpecialVoreFatal(int variantID = 0)
	{
		bool flag = false;
		GameCore.FetishType[] fetishTags = eroCore.FetishTags;
		for (int i = 0; i < fetishTags.Length; i++)
		{
			if (fetishTags[i] == GameCore.FetishType.Vore)
			{
				if (GC.doAllVoreScenesAreFatal && !eroCore.eroIsFatal)
				{
					eroCore.eroIsFatal = true;
					wasVoreEroToggledToFatal = true;
					ReplaceInternalViewsVoreFatal();
				}
				else if (!GC.doAllVoreScenesAreFatal && wasVoreEroToggledToFatal)
				{
					eroCore.eroIsFatal = false;
					wasVoreEroToggledToFatal = false;
					ReplaceInternalViewsVoreFatal(returnToOriginal: true);
				}
				flag = true;
			}
		}
		if (!flag)
		{
			ENM_EroSoloSceneVariations[] eroSoloAlts = eroCore.eroSoloAlts;
			foreach (ENM_EroSoloSceneVariations eNM_EroSoloSceneVariations in eroSoloAlts)
			{
				if (eNM_EroSoloSceneVariations.SceneTag == GameCore.FetishType.Vore && eNM_EroSoloSceneVariations.GenericID == variantID)
				{
					if (GC.doAllVoreScenesAreFatal && !eroCore.eroIsFatal)
					{
						eroCore.eroIsFatal = true;
						wasVoreEroToggledToFatal = true;
						ReplaceInternalViewsVoreFatal();
					}
					else if (!GC.doAllVoreScenesAreFatal && wasVoreEroToggledToFatal)
					{
						eroCore.eroIsFatal = false;
						wasVoreEroToggledToFatal = false;
						ReplaceInternalViewsVoreFatal(returnToOriginal: true);
					}
				}
			}
		}
		GC.currentEnemyGrabbingIsEroFatal = eroCore.eroIsFatal;
	}

	private void ReplaceInternalViewsVoreFatal(bool returnToOriginal = false)
	{
		EroAnimation component = GetComponent<EroAnimation>();
		if (component == null)
		{
			Debug.Log("[EnemyGeneralLogic:ReplaceInternalView -> could not get reference to eroAnimation script on the object!]");
		}
		else if (component.arrayFatalVoreReplaceInternal == null || component.arrayFatalVoreReplaceInternal.Length == 0)
		{
			Debug.Log("Array with replacements is null or zero! Ignore this message if this character doesn't replace fatal vore internals.");
		}
		else if (eroCore.eroSoloAlts.Length != 0)
		{
			for (int i = 0; i < eroCore.eroSoloAlts.Length; i++)
			{
				if (!(component.arrayFatalVoreReplaceInternal[i] != null))
				{
					continue;
				}
				if (returnToOriginal && arrayEroVoreFatalTempHoldPreviousReferences != null)
				{
					eroCore.eroSoloAlts[i].InternalsOBJ = arrayEroVoreFatalTempHoldPreviousReferences[i];
					continue;
				}
				if (arrayEroVoreFatalTempHoldPreviousReferences == null)
				{
					arrayEroVoreFatalTempHoldPreviousReferences = new GameObject[eroCore.eroSoloAlts.Length];
				}
				arrayEroVoreFatalTempHoldPreviousReferences[i] = eroCore.eroSoloAlts[i].InternalsOBJ;
				eroCore.eroSoloAlts[i].InternalsOBJ = component.arrayFatalVoreReplaceInternal[i];
			}
		}
		else if (returnToOriginal && arrayEroVoreFatalTempHoldPreviousReferences != null)
		{
			eroCore.basicEroInternals = arrayEroVoreFatalTempHoldPreviousReferences[0];
		}
		else
		{
			if (arrayEroVoreFatalTempHoldPreviousReferences == null)
			{
				arrayEroVoreFatalTempHoldPreviousReferences = new GameObject[1];
			}
			arrayEroVoreFatalTempHoldPreviousReferences[0] = eroCore.basicEroInternals;
			eroCore.basicEroInternals = component.arrayFatalVoreReplaceInternal[0];
		}
	}

	public void RunTheCoroutinesAfterEroEnd(float eroGraceTime)
	{
		DoSpecialBehaviorOnEroEnd();
		if ((bool)GC && !GC.isPlayerFainted && GC.configDifficultyType != GameCore.DifficultyType.Easy)
		{
			eroGraceTime = 1f;
		}
		if (isUsingSkill)
		{
			EnemyConcludeSkill();
		}
		if (isChasingFainted)
		{
			isChasingFainted = false;
			crtChaseFainted = null;
		}
		if (coroutEroGracePeriod != null)
		{
			StopCoroutine(coroutEroGracePeriod);
		}
		coroutEroGracePeriod = StartCoroutine(waitForEroGracePeriod(eroGraceTime));
		if (crtChaseFaintedDelay != null)
		{
			StopCoroutine(crtChaseFaintedDelay);
		}
		crtChaseFaintedDelay = StartCoroutine(waitForChasingFaintedDelay());
	}

	private void DoSpecialBehaviorOnEroEnd()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.MothGirl:
		case GameCore.EnemyType.BeeGirl:
			if (GC.isPlayerFainted && GC.isPlayerGoneForGood)
			{
				UnityEngine.Object.Instantiate(GetMiscGameObject(0), base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform).transform.localScale = base.transform.localScale;
			}
			break;
		case GameCore.EnemyType.Dogboy:
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
				crtGeneric = null;
				objAN.SetInteger("genericPhase", 0);
			}
			break;
		case GameCore.EnemyType.WyvernGirl:
			if (GC.isPlayerFainted && GC.isPlayerGoneForGood)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "BehaviorOnEroEndWyvern");
				UnityEngine.Object.Instantiate(GetMiscGameObject(0), base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform).GetComponent<EnemyGeneralLogic>().objGeneric = base.gameObject;
				GC.PlaySFXEro(GameCore.EroSound.Cum5, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			}
			break;
		case GameCore.EnemyType.Catgirl:
			if (GC.isPlayerFainted && GC.isPlayerGoneForGood)
			{
				GENERIC_StartTheWait(3f, "cat_reveal");
			}
			break;
		case GameCore.EnemyType.BlazePlant:
			if (GC.isPlayerFainted && GC.isPlayerGoneForGood)
			{
				ProjectileLogic component = UnityEngine.Object.Instantiate(GetMiscGameObject(0), GetEnemyCenterPos(), Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>();
				if (GC.MATH_RNG_ROLL(50))
				{
					component.specialForce.x = GetEnemyCenterPos().x + GC.MATH_RNG_FLOAT(1f, 2f);
				}
				else
				{
					component.specialForce.x = GetEnemyCenterPos().x + GC.MATH_RNG_FLOAT(-2f, -1f);
				}
				component.specialForce.y = GetEnemyCenterPos().y + GC.MATH_RNG_FLOAT(0.5f, 1f);
			}
			break;
		}
	}

	private IEnumerator waitForEroGracePeriod(float waittime)
	{
		eroCore.IsAllowedToGrab = false;
		yield return new WaitForSeconds(waittime);
		eroCore.IsAllowedToGrab = true;
		coroutEroGracePeriod = null;
	}

	private void BE_UPDATE_MAINCHECK()
	{
		if ((bool)localEroScript && localEroScript.BadEndConfiguration.hasBadEnd && (bool)GC && GC.doAllowBadEnd)
		{
			if (isGrabbing && !localBadEndFlag)
			{
				localBadEndFlag = true;
			}
			else if (!isGrabbing && localBadEndFlag)
			{
				localBadEndFlag = false;
				BAD_ResetBadEndState();
			}
			if (localBadEndFlag && (intBadEndLocalLogicID == 0 || intBadEndLocalLogicID == objAN.GetInteger("eroVariantID")))
			{
				BadEndEroBehaviorControl();
			}
			if (localBadEndFlag && GC.trySkipToBadEndStart)
			{
				BADEND_TryToSkipToBadEndStart();
			}
		}
	}

	public void BadEndEroBehaviorControl()
	{
		int integer = objAN.GetInteger("intBEPhase");
		int integer2 = objAN.GetInteger("eroPhase");
		if (localBadEndFlag && !GC.isRunningBadEndLogic)
		{
			GC.isRunningBadEndLogic = true;
		}
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Dogboy:
			BAD01(integer, integer2);
			break;
		case GameCore.EnemyType.BigInsect:
			BAD02(integer, integer2);
			break;
		case GameCore.EnemyType.FutaDruidCentauress:
			BAD03(integer, integer2);
			break;
		case GameCore.EnemyType.BossSwampSlime:
			BAD04(integer, integer2);
			break;
		case GameCore.EnemyType.BossMinotaur:
			BAD05(integer, integer2);
			break;
		case GameCore.EnemyType.Kobold:
			BAD06(integer, integer2);
			break;
		case GameCore.EnemyType.Sinccubus:
			BAD07(integer, integer2);
			break;
		case GameCore.EnemyType.Arachne:
			BAD08(integer, integer2);
			break;
		case GameCore.EnemyType.BossWerewolf:
			BAD09(integer, integer2);
			break;
		}
	}

	private void BADEND_TryToSkipToBadEndStart()
	{
		int integer = objAN.GetInteger("intBEPhase");
		int num = 0;
		if (!GC.trySkipToBadEndStart)
		{
			return;
		}
		GC.trySkipToBadEndStart = false;
		if (hasSkippedToBadEnd || !localBadEndFlag || !GC.isPlayerFainted || GC.isPlayerGoneForGood)
		{
			return;
		}
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Dogboy:
			num = 101;
			break;
		case GameCore.EnemyType.BigInsect:
			num = 50;
			break;
		case GameCore.EnemyType.FutaDruidCentauress:
			num = 40;
			break;
		case GameCore.EnemyType.BossSwampSlime:
			num = 50;
			break;
		case GameCore.EnemyType.BossMinotaur:
			num = 10;
			break;
		case GameCore.EnemyType.Kobold:
			num = 50;
			break;
		case GameCore.EnemyType.Sinccubus:
			num = 50;
			break;
		case GameCore.EnemyType.Arachne:
			num = 50;
			break;
		case GameCore.EnemyType.BossWerewolf:
			num = 50;
			break;
		}
		if (integer < num)
		{
			hasSkippedToBadEnd = true;
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, base.transform.position, "skipToBadEnd");
			switch (currentEnemyType)
			{
			case GameCore.EnemyType.BossSwampSlime:
				objAN.SetInteger("eroVariantID", 2);
				break;
			case GameCore.EnemyType.BossMinotaur:
				objAN.SetInteger("eroVariantID", 11);
				break;
			}
			BAD_BadEndStartCoroutine(0f, num);
		}
	}

	public void BAD_UpCount()
	{
		countBEStuff++;
	}

	private void BAD_BadEndStartCoroutine(float waitTime, int nextPhase, bool setTempVoidState = false, string actionDescription = "")
	{
		if (crtBadEnd != null)
		{
			StopCoroutine(crtBadEnd);
		}
		crtBadEnd = StartCoroutine(waitBadEndPhases(waitTime, nextPhase, setTempVoidState, actionDescription));
	}

	public void BAD_ResetBadEndState()
	{
		objAN.SetInteger("intBEPhase", 0);
		countBEStuff = 0;
		if (crtBadEnd != null)
		{
			StopCoroutine(crtBadEnd);
			crtBadEnd = null;
		}
		GC.isRunningBadEndLogic = false;
		localBadEndFlag = false;
	}

	public void BAD_InitiateDialogue(int idDialogue, bool isZoomDiag = false)
	{
		if (GC.doAllowDialogueDuringBadEnd && idDialogue > 0)
		{
			GC.SignalPlayerToStartDialogue(idDialogue, isZoomDiag);
		}
	}

	private bool BAD_WhileDialogueDoBehavior(BDEDiagBehavior behavior, int intParam = 0, string strParam = "", float fltParam = 0f)
	{
		if (!GC.isDisplayingDialogue)
		{
			return false;
		}
		switch (behavior)
		{
		case BDEDiagBehavior.StayOnAnimationPhase:
			localEroScript.MidEroManipulateValues(resetLoopCounters: true, intParam, strParam);
			break;
		case BDEDiagBehavior.StallCoroutine:
			if (crtBadEnd != null)
			{
				BAD_BadEndStartCoroutine(fltParam, intParam);
				return true;
			}
			break;
		}
		return false;
	}

	private IEnumerator waitBadEndPhases(float time, int nextPhase, bool isTempVoidState, string action)
	{
		if (isTempVoidState)
		{
			objAN.SetInteger("intBEPhase", -1);
		}
		yield return new WaitForSeconds(time);
		objAN.SetInteger("intBEPhase", nextPhase);
		crtBadEnd = null;
	}

	private void BAD01(int currentBEPhase, int currentEroPhase)
	{
		int idDialogue = 0;
		int num = 2;
		int num2 = 6;
		string text = "DBY-Ero-03Loop1";
		string playThisEroAnim = "DBY-Preggo_Normal";
		if (countBEStuff >= num)
		{
			text = "DBY-Ero-03Loop1_SheDog";
		}
		if (countBEStuff >= num)
		{
			playThisEroAnim = "DBY-Preggo_SheDog";
		}
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				BAD_BadEndStartCoroutine(10f, 1);
				break;
			case 1:
				GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "EROANIMATION:DoUniqueBehaviorDuringGrabLoops");
				GC.PlaySFX(GameCore.SFXBasic1.HitHurtTouch1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
				objAN.Play("DBY-Ero-02Walk");
				BAD_BadEndStartCoroutine(0.1f, 100);
				break;
			case 100:
				GC.DisplaySystemMessageUI(0, 100, 5f, "", isFlavorCentralMessage: true);
				BAD_BadEndStartCoroutine(12f, 101);
				break;
			case 101:
				countBEStuff = 0;
				localEroScript.KillPlayer();
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 102);
				break;
			case 102:
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CityDogGroaning, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
				BAD_BadEndStartCoroutine(1f, 200);
				break;
			case 200:
				GC.SpecialEroTeleport(base.gameObject, "DogBoy");
				switch (countBEStuff)
				{
				case 0:
					idDialogue = 9001;
					break;
				case 1:
					idDialogue = 9002;
					break;
				case 2:
					idDialogue = 9003;
					break;
				}
				BAD_InitiateDialogue(idDialogue);
				if (countBEStuff == 0)
				{
					GC.DisplaySystemMessageUI(0, 101, 7f, "", isFlavorCentralMessage: true);
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 1, text);
				BAD_BadEndStartCoroutine(1f, 201);
				break;
			case 201:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(999999f, 202);
				break;
			case 202:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 203);
				break;
			case 203:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 1, "Basic-Idle");
				GC.StopAllSFXCurrentylPlaying();
				BAD_BadEndStartCoroutine(0.5f, 300);
				break;
			case 300:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 1, playThisEroAnim);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				countBEStuff++;
				BAD_BadEndStartCoroutine(9999999f, 301);
				break;
			case 301:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 302);
				break;
			case 302:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 1, "Basic-Idle");
				GC.StopAllSFXCurrentylPlaying();
				GC.CallReferenceMayaVoiceGC("climax8", "EnemyGeneralLogic:waitGeneric(DogBoy)");
				BAD_BadEndStartCoroutine(1.5f, 303);
				break;
			case 303:
				if (countBEStuff <= num2)
				{
					int num3 = GC.MATH_RNG_INT(1, 3);
					for (int i = 0; i < num3; i++)
					{
						Vector2 vector = base.transform.position;
						vector.x += GC.MATH_RNG_FLOAT(-2f, 2f);
						UnityEngine.Object.Instantiate(GetMiscGameObject(1), vector, Quaternion.identity, GC.refOBJEnemiesParent.transform);
					}
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CityDogBabyBark, 0, checkForDuplicity: true, base.transform.position, 1.1f, 1.2f);
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CityDogBabyWhine, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.1f);
				}
				objRE.flipX = !objRE.flipX;
				BAD_BadEndStartCoroutine(2f, 200);
				break;
			}
		}
		switch (currentBEPhase)
		{
		case 0:
			if ((GC.playerCurrentHealth <= (float)GC.playerMaxHealth / 2f && !GC.isPlayerFainted) || GC.isPlayerFainted)
			{
				BAD_BadEndStartCoroutine(0.1f, 1, setTempVoidState: true);
			}
			break;
		case 100:
			if (GC.isPlayerFainted)
			{
				BAD_BadEndStartCoroutine(12f, 101, setTempVoidState: true);
			}
			break;
		case 201:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 1, text);
			if (currentEroPhase == 5)
			{
				BAD_BadEndStartCoroutine(0.1f, 202, setTempVoidState: true);
			}
			break;
		case 300:
			if (currentEroPhase == 2)
			{
				BAD_BadEndStartCoroutine(0.1f, 301, setTempVoidState: true);
			}
			break;
		}
		GC.BEpartInfo(countBEStuff, 4);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 4)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 700, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void BAD02(int currentBEPhase, int currentEroPhase)
	{
		int idDialogue = 0;
		string text = "BIS-BE01-LoopGrabbed";
		string text2 = "BIS-BE02-Loop1Silent";
		string text3 = "BIS-HoldAnim";
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				if (currentEroPhase == 7 && GC.isPlayerFainted)
				{
					BAD_BadEndStartCoroutine(10f, 50);
					localEroScript.MidEroManipulateValues(resetLoopCounters: true, 7, "", 999999);
					GC.DisplaySystemMessageUI(0, 124, 10f, "", isFlavorCentralMessage: true);
				}
				break;
			case 50:
				countBEStuff = 0;
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 51);
				break;
			case 51:
			{
				Vector2 changeSize = new Vector2(0.5f, 1.75f);
				Vector2 changeOffset = new Vector2(0f, -0.625f);
				ERO_ToggleSpecialEroCollider(changeSize, changeOffset, isTrigger: false);
				enmMisc.FlyerEroApplyGravity = true;
				GC.SpecialEroTeleport(base.gameObject, "GiantFly");
				BAD_BadEndStartCoroutine(1f, 52);
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 10, text3);
				break;
			}
			case 52:
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				GC.CallReferenceMayaVoiceGC("scream14", "...");
				BAD_BadEndStartCoroutine(1.5f, 100);
				break;
			case 100:
				if (countBEStuff > 0)
				{
					UnityEngine.Object.Instantiate(GetMiscGameObject(1), base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
				}
				GC.StopAllSFXCurrentylPlaying();
				countBEStuff++;
				if (countBEStuff == 1)
				{
					idDialogue = 9004;
				}
				BAD_InitiateDialogue(idDialogue);
				intGeneric = 0;
				floatGeneric = 0f;
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 10, text3);
				BAD_BadEndStartCoroutine(0.5f, 101);
				break;
			case 101:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 10, text);
				if (countBEStuff == 3)
				{
					idDialogue = 9006;
				}
				BAD_InitiateDialogue(idDialogue);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(9999999f, 200);
				break;
			case 200:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				countBEStuff++;
				BAD_BadEndStartCoroutine(4f, 201);
				break;
			case 201:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 11, text2);
				intGeneric = GC.MATH_RNG_INT(2, 3);
				BAD_BadEndStartCoroutine(1f, 202);
				break;
			case 202:
				switch (countBEStuff)
				{
				case 2:
					idDialogue = 9005;
					break;
				case 4:
					idDialogue = 9007;
					break;
				}
				BAD_InitiateDialogue(idDialogue);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(999999f, 203);
				break;
			case 203:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 100);
				break;
			}
		}
		switch (currentBEPhase)
		{
		case 100:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 101, "", 0.5f);
			break;
		case 101:
			if (countBEStuff >= 3)
			{
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 10, text);
			}
			else
			{
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 10, text3);
			}
			if (currentEroPhase >= 11)
			{
				BAD_BadEndStartCoroutine(0.1f, 200, setTempVoidState: true);
			}
			break;
		case 202:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 11, text2);
			switch (currentEroPhase)
			{
			case 15:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 11, text2);
				if (floatGeneric < (float)intGeneric)
				{
					Vector2 vector = enmObjRefs.StartPointCenterMass.position;
					vector.y -= 0.75f;
					floatGeneric++;
					GC.InstantiateVFX(GameCore.VisualEffect.Spit, vector, "GiantInsect:BadEnd");
					UnityEngine.Object.Instantiate(GetMiscGameObject(1), vector, Quaternion.identity, GC.refOBJEnemiesParent.transform);
					GC.CallReferenceMayaVoiceGC("rng-Climax", "InsectBadEnd");
					GC.PlaySFXEro(GameCore.EroSound.Cum5, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
				}
				break;
			case 11:
				if (floatGeneric >= (float)intGeneric)
				{
					BAD_BadEndStartCoroutine(0.1f, 203);
				}
				break;
			}
			break;
		}
		GC.BEpartInfo(countBEStuff, 5);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 5)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 701, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void BAD03(int currentBEPhase, int currentEroPhase)
	{
		string text = "FDC-BE-01Idle";
		string playThisEroAnim = "";
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				if (currentEroPhase == 4 && GC.isPlayerFainted)
				{
					BAD_BadEndStartCoroutine(6f, 40);
				}
				break;
			case 40:
				if (hasSkippedToBadEnd)
				{
					playThisEroAnim = "FDC-Ero-07RingNeck";
				}
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				BAD_InitiateDialogue(9008);
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 4, playThisEroAnim, 9999999);
				BAD_BadEndStartCoroutine(1f, 41);
				break;
			case 41:
				GC.DisplaySystemMessageUI(0, 125, 10f, "", isFlavorCentralMessage: true);
				BAD_BadEndStartCoroutine(12f, 50);
				break;
			case 50:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				GC.ToggleMuteAudio(GameCore.AudioType.pairSFXVOI, applyMute: true);
				BAD_BadEndStartCoroutine(4f, 51);
				break;
			case 51:
				GC.SpecialEroTeleport(base.gameObject, "FutaDruid");
				BAD_BadEndStartCoroutine(1f, 52);
				break;
			case 52:
				GC.CallReferenceMayaVoiceGC("BJClimax8", "...");
				BAD_InitiateDialogue(9009);
				BAD_BadEndStartCoroutine(1f, 100);
				break;
			case 100:
				BAD_BadEndStartCoroutine(0.1f, 101);
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 6, text);
				break;
			case 101:
				GC.ToggleMuteAudio(GameCore.AudioType.pairSFXVOI, applyMute: false);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				if (countBEStuff == 0)
				{
					BAD_BadEndStartCoroutine(6f, 301);
				}
				else
				{
					BAD_BadEndStartCoroutine(9999999f, 102);
				}
				break;
			case 102:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				countBEStuff++;
				BAD_BadEndStartCoroutine(4f, 100);
				break;
			case 301:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 6, text);
				BAD_InitiateDialogue(9010);
				BAD_BadEndStartCoroutine(9999999f, 102);
				break;
			}
		}
		switch (currentBEPhase)
		{
		case 40:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 41, "", 1f);
			break;
		case 52:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 100, "", 1f);
			break;
		case 101:
		case 301:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 6, text);
			if (currentEroPhase == 10)
			{
				BAD_BadEndStartCoroutine(10f, 102, setTempVoidState: true);
			}
			break;
		}
		GC.BEpartInfo(countBEStuff, 2);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 2)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 702, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void BAD04(int currentBEPhase, int currentEroPhase)
	{
		string playThisEroAnim = "SBE-01Grab_A";
		string text = "SBE-02MainLoop";
		if (countBEStuff > 0)
		{
			playThisEroAnim = "SBE-01Grab_B";
		}
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				if (currentEroPhase == 3 && GC.isPlayerFainted)
				{
					BAD_BadEndStartCoroutine(12f, 50);
				}
				break;
			case 50:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				GC.ToggleMuteAudio(GameCore.AudioType.pairSFXVOI, applyMute: true);
				BAD_BadEndStartCoroutine(4f, 51);
				break;
			case 51:
				GC.SpecialEroTeleport(base.gameObject, "SwampBoss");
				BAD_BadEndStartCoroutine(1f, 52);
				break;
			case 52:
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 5, playThisEroAnim);
				BAD_InitiateDialogue(9011);
				BAD_BadEndStartCoroutine(1f, 100);
				break;
			case 100:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				GC.ToggleMuteAudio(GameCore.AudioType.pairSFXVOI, applyMute: false);
				if (countBEStuff == 0)
				{
					BAD_InitiateDialogue(9012);
				}
				else if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9014);
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 5, playThisEroAnim);
				if (countBEStuff < 2)
				{
					BAD_BadEndStartCoroutine(1f, 101);
				}
				else
				{
					BAD_BadEndStartCoroutine(16f, 101);
				}
				break;
			case 101:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 102);
				break;
			case 102:
				BAD_BadEndStartCoroutine(1f, 200);
				break;
			case 200:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 6, text);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				if (countBEStuff == 0)
				{
					BAD_InitiateDialogue(9013);
				}
				else if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9015);
				}
				BAD_BadEndStartCoroutine(999999f, 201);
				break;
			case 201:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 202);
				break;
			case 202:
				countBEStuff++;
				BAD_BadEndStartCoroutine(1f, 100);
				break;
			}
		}
		switch (currentBEPhase)
		{
		case 52:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 100, "", 1f);
			break;
		case 100:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 101, "", 1f);
			break;
		case 200:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 6, text);
			if (currentEroPhase == 9)
			{
				BAD_BadEndStartCoroutine(12f, 201, setTempVoidState: true);
			}
			break;
		}
		GC.BEpartInfo(countBEStuff, 3);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 3)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 703, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void BAD05(int currentBEPhase, int currentEroPhase)
	{
		string strParam = "MNT-Ero2_PullLoop";
		string playThisEroAnim = "PullLoop_STARTBADEND";
		string text = "BadEnd1-Start-A";
		string text2 = "BadEnd1-Start-B";
		string playThisEroAnim2 = "BadEnd2-Start-Rest";
		string playThisEroAnim3 = "BadEnd3-Start-Rest";
		string playThisEroAnim4 = "BD1insertLoop";
		string playThisEroAnim5 = "BD2insertLoop";
		string playThisEroAnim6 = "BD3insertLoop";
		string playThisEroAnim7 = "MNT-BD3-05loop1";
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				if (GC.isPlayerFainted && objAN.GetCurrentAnimatorStateInfo(0).IsName(strParam))
				{
					BAD_BadEndStartCoroutine(0.5f, 10);
				}
				break;
			case 10:
				BAD_InitiateDialogue(9016);
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 2, playThisEroAnim);
				STATE_MACHINE_FLIP_SELF(forcedLeft: false, forcedRight: true);
				BAD_BadEndStartCoroutine(0.25f, 11);
				break;
			case 11:
				BAD_BadEndStartCoroutine(9999999f, 50);
				break;
			case 50:
				objAN.SetInteger("eroVariantID", 99);
				intBadEndLocalLogicID = 99;
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 51);
				GC.ToggleMuteAudio(GameCore.AudioType.pairSFXVOI, applyMute: true);
				break;
			case 51:
				GC.SpecialEroTeleport(base.gameObject, "CityBoss");
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				BAD_BadEndStartCoroutine(1f, 100);
				break;
			case 100:
			{
				string playThisEroAnim8 = text2;
				if (countBEStuff == 0)
				{
					GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
					BAD_InitiateDialogue(9017);
					playThisEroAnim8 = text;
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 0, playThisEroAnim8);
				GC.ToggleMuteAudio(GameCore.AudioType.pairSFXVOI, applyMute: false);
				BAD_BadEndStartCoroutine(0.1f, 101);
				break;
			}
			case 101:
				BAD_BadEndStartCoroutine(9999999f, 102);
				break;
			case 102:
				BAD_BadEndStartCoroutine(1f, 200);
				break;
			case 200:
				if (countBEStuff == 3)
				{
					BAD_InitiateDialogue(9019);
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 0, playThisEroAnim2);
				BAD_BadEndStartCoroutine(0.1f, 201);
				break;
			case 201:
				BAD_BadEndStartCoroutine(9999999f, 202);
				break;
			case 202:
				BAD_BadEndStartCoroutine(1f, 300);
				break;
			case 300:
				if (countBEStuff == 5)
				{
					BAD_InitiateDialogue(9021);
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 0, playThisEroAnim3);
				BAD_BadEndStartCoroutine(0.1f, 301);
				break;
			case 301:
				BAD_BadEndStartCoroutine(9999999f, 302);
				break;
			case 302:
				countBEStuff++;
				BAD_BadEndStartCoroutine(1f, 100);
				break;
			}
		}
		switch (currentBEPhase)
		{
		case 11:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 2, strParam);
			if (!GC.isDisplayingDialogue)
			{
				BAD_BadEndStartCoroutine(0.5f, 50, setTempVoidState: true);
			}
			break;
		case 101:
			switch (currentEroPhase)
			{
			case 0:
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase);
				if (!GC.isDisplayingDialogue && countBEStuff == 0)
				{
					countBEStuff++;
					localEroScript.MidEroManipulateValues(resetLoopCounters: false, 1);
				}
				break;
			case 2:
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 2);
				if (!GC.isDisplayingDialogue && countBEStuff == 2)
				{
					countBEStuff++;
					localEroScript.MidEroManipulateValues(resetLoopCounters: false, 2, playThisEroAnim4);
					BAD_InitiateDialogue(9018);
				}
				break;
			case 7:
				BAD_BadEndStartCoroutine(0f, 102, setTempVoidState: true);
				break;
			}
			break;
		case 201:
			switch (currentEroPhase)
			{
			case 0:
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase);
				if (!GC.isDisplayingDialogue && countBEStuff == 3)
				{
					countBEStuff++;
					localEroScript.MidEroManipulateValues(resetLoopCounters: false, 1);
				}
				break;
			case 2:
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 2);
				if (!GC.isDisplayingDialogue && countBEStuff == 4)
				{
					countBEStuff++;
					localEroScript.MidEroManipulateValues(resetLoopCounters: false, 2, playThisEroAnim5);
					BAD_InitiateDialogue(9020);
				}
				break;
			case 7:
				BAD_BadEndStartCoroutine(0f, 202, setTempVoidState: true);
				break;
			}
			break;
		case 301:
			switch (currentEroPhase)
			{
			case 0:
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase);
				if (!GC.isDisplayingDialogue && countBEStuff == 5)
				{
					countBEStuff++;
					localEroScript.MidEroManipulateValues(resetLoopCounters: false, 1);
				}
				break;
			case 2:
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 2);
				if (!GC.isDisplayingDialogue && countBEStuff == 6)
				{
					countBEStuff++;
					localEroScript.MidEroManipulateValues(resetLoopCounters: false, 2, playThisEroAnim6);
					BAD_InitiateDialogue(9022);
				}
				break;
			case 3:
				BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 3);
				if (!GC.isDisplayingDialogue && countBEStuff == 7)
				{
					countBEStuff++;
					localEroScript.MidEroManipulateValues(resetLoopCounters: false, 3, playThisEroAnim7);
					BAD_InitiateDialogue(9023);
				}
				break;
			case 7:
				BAD_BadEndStartCoroutine(0f, 302, setTempVoidState: true);
				break;
			}
			break;
		}
		GC.BEpartInfo(countBEStuff, 10);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 10)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 704, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void BAD06(int currentBEPhase, int currentEroPhase)
	{
		string text = "KBD-BE-Loop1";
		string text2 = "KBD-BE-TF-Loop1";
		string text3 = "KBD-BE-SX-Loop1";
		string text4 = "KBD-BE-EG-LoopRest";
		string playThisEroAnim = "KBD-BE-Silent";
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				if (currentEroPhase == 4 && GC.isPlayerFainted)
				{
					BAD_BadEndStartCoroutine(8f, 50);
					localEroScript.MidEroManipulateValues(resetLoopCounters: true, 4, "", 999999);
					GC.DisplaySystemMessageUI(0, 140, 8f, "", isFlavorCentralMessage: true);
				}
				break;
			case 50:
				countBEStuff = 0;
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 51);
				break;
			case 51:
				GC.SpecialEroTeleport(base.gameObject, "Kobold");
				BAD_BadEndStartCoroutine(1f, 52);
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 10, playThisEroAnim);
				break;
			case 52:
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				GC.CallReferenceMayaVoiceGC("scream14", "...");
				BAD_BadEndStartCoroutine(1.5f, 100);
				break;
			case 100:
				GC.StopAllSFXCurrentylPlaying();
				BAD_InitiateDialogue(9024);
				BAD_BadEndStartCoroutine(0.5f, 101);
				break;
			case 101:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 6, text);
				BAD_InitiateDialogue(9025);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(9999999f, 102);
				break;
			case 102:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 200);
				break;
			case 200:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 12, text2);
				BAD_BadEndStartCoroutine(1f, 201);
				break;
			case 201:
				BAD_InitiateDialogue(9026);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(999999f, 202);
				break;
			case 202:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 300);
				break;
			case 300:
				countBEStuff++;
				GC.StopAllSFXCurrentylPlaying();
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 6, playThisEroAnim);
				if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9027);
				}
				intGeneric = 0;
				floatGeneric = 0f;
				BAD_BadEndStartCoroutine(0.5f, 301);
				break;
			case 301:
				if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9028);
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 6, text3);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(999999f, 302);
				break;
			case 302:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 400);
				break;
			case 400:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, text4);
				intGeneric = GC.MATH_RNG_INT(2, 3);
				BAD_BadEndStartCoroutine(1f, 401);
				break;
			case 401:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, text4);
				if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9029);
				}
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(999999f, 402);
				break;
			case 402:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 300);
				break;
			}
		}
		switch (currentBEPhase)
		{
		case 100:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 101, "", 0.5f);
			break;
		case 101:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 6, text);
			if (currentEroPhase >= 11)
			{
				BAD_BadEndStartCoroutine(0.1f, 102, setTempVoidState: true);
			}
			break;
		case 201:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 12, text2);
			if (currentEroPhase >= 19)
			{
				BAD_BadEndStartCoroutine(0.1f, 202, setTempVoidState: true);
			}
			break;
		case 300:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 301, "", 0.5f);
			break;
		case 301:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 6, text3);
			if (currentEroPhase >= 11)
			{
				BAD_BadEndStartCoroutine(0.1f, 302, setTempVoidState: true);
			}
			break;
		case 401:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 20, text4);
			switch (currentEroPhase)
			{
			case 24:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, text4);
				if (floatGeneric < (float)intGeneric)
				{
					Vector2 vector = enmObjRefs.StartPointCenterMass.position;
					if (GetEnemyFacing() > 0f)
					{
						vector.x -= 0.5f;
					}
					else
					{
						vector.x += 0.5f;
					}
					floatGeneric++;
					GC.InstantiateVFX(GameCore.VisualEffect.Spit, vector, "Kobold:BadEnd");
					GameObject obj = UnityEngine.Object.Instantiate(GetMiscGameObject(1), vector, Quaternion.identity, GC.refOBJEnemiesParent.transform);
					ProjectileLogic component = obj.GetComponent<ProjectileLogic>();
					Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), objBox2D);
					if (GetEnemyFacing() > 0f)
					{
						component.specialForce.x = -0.75f;
					}
					else
					{
						component.specialForce.x = 0.75f;
					}
					component.specialForce.y = 1f;
					GC.CallReferenceMayaVoiceGC("rng-Climax", "KoboldBadEnd");
					GC.PlaySFXEro(GameCore.EroSound.Cum5, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
				}
				break;
			case 20:
				if (floatGeneric >= (float)intGeneric)
				{
					BAD_BadEndStartCoroutine(0.1f, 402);
				}
				break;
			}
			break;
		}
		GC.BEpartInfo(countBEStuff, 3);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 3)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 705, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void BAD07(int currentBEPhase, int currentEroPhase)
	{
		Vector2 zero = Vector2.zero;
		string text = "SCB-BE-01VoreLoop";
		string playThisEroAnim = "SCB-BE-01VoreLoopSilent";
		string strParam = "SCB-BE-04ChurnLoop1";
		string playThisEroAnim2 = "SCB-BE-04ChurnLoopSilent";
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				if (currentEroPhase == 7 && GC.isPlayerFainted)
				{
					BAD_BadEndStartCoroutine(7f, 50);
					localEroScript.MidEroManipulateValues(resetLoopCounters: true, 7, "", 999999);
					GC.DisplaySystemMessageUI(0, 142, 5f, "", isFlavorCentralMessage: true);
				}
				break;
			case 50:
				countBEStuff = 0;
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 51);
				break;
			case 51:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 9, playThisEroAnim);
				BAD_BadEndStartCoroutine(1f, 52);
				break;
			case 52:
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				GC.CallReferenceMayaVoiceGC("scream14", "...");
				zero = base.transform.position;
				zero.y++;
				base.transform.position = zero;
				BAD_BadEndStartCoroutine(1.5f, 100);
				break;
			case 100:
				GC.StopAllSFXCurrentylPlaying();
				BAD_InitiateDialogue(9030);
				BAD_BadEndStartCoroutine(0.5f, 101);
				break;
			case 101:
				countBEStuff++;
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 9, text);
				BAD_InitiateDialogue(9031);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(9999999f, 102);
				break;
			case 102:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 7f);
				BAD_BadEndStartCoroutine(8f, 200);
				break;
			case 200:
			{
				countBEStuff++;
				GC.SpecialEroTeleport(base.gameObject, "Sinccubus");
				Vector2 changeSize = new Vector2(0.8f, 1.5f);
				Vector2 changeOffset = new Vector2(0f, -0.75f);
				ERO_ToggleSpecialEroCollider(changeSize, changeOffset, isTrigger: false);
				enmMisc.FlyerEroApplyGravity = true;
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 15, playThisEroAnim2);
				BAD_BadEndStartCoroutine(1f, 201);
				break;
			}
			case 201:
			{
				countBEStuff++;
				GC.PlaySFXEro(GameCore.EroSound.Rubber7, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
				zero = base.transform.position;
				zero.y++;
				GameObject gameObject = null;
				GameObject obj = UnityEngine.Object.Instantiate(GetMiscGameObject(0), zero, Quaternion.identity, GC.refOBJEffectsParent.transform);
				Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<Collider2D>(), ignore: true);
				gameObject = UnityEngine.Object.Instantiate(GetMiscGameObject(1), zero, Quaternion.identity, GC.refOBJEffectsParent.transform);
				Physics2D.IgnoreCollision(gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>(), ignore: true);
				Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), gameObject.GetComponent<Collider2D>(), ignore: true);
				BAD_InitiateDialogue(9032);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(999999f, 100);
				break;
			}
			}
		}
		switch (currentBEPhase)
		{
		case 100:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 101, "", 0.5f);
			break;
		case 101:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 9, text);
			if (currentEroPhase >= 14)
			{
				BAD_BadEndStartCoroutine(0.1f, 102, setTempVoidState: true);
			}
			break;
		case 201:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 15, strParam);
			break;
		}
		GC.BEpartInfo(countBEStuff, 5);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 5)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 706, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void BAD08(int currentBEPhase, int currentEroPhase)
	{
		_ = Vector2.zero;
		string text = "ARC-BE-01PenetrateA";
		string playThisEroAnim = "ARC-BE-SexSilent";
		string text2 = "ARC-BE-05Resting";
		string playThisEroAnim2 = "ARC-BE-05RestingSilent";
		string playThisEroAnim3 = "ARC-BE-silentcopy";
		string text3 = "ARC-BE-03PenetrateB";
		if (countBEStuff <= 2)
		{
			text3 = text;
		}
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				if (currentEroPhase == 14 && GC.isPlayerFainted)
				{
					BAD_BadEndStartCoroutine(6f, 50);
					localEroScript.MidEroManipulateValues(resetLoopCounters: true, 14, "", 999999);
					GC.DisplaySystemMessageUI(0, 145, 5f, "", isFlavorCentralMessage: true);
				}
				break;
			case 50:
				countBEStuff = 0;
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 51);
				break;
			case 51:
				GC.SpecialEroTeleport(base.gameObject, "Arachne");
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 16, playThisEroAnim);
				BAD_BadEndStartCoroutine(1f, 52);
				break;
			case 52:
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				GC.CallReferenceMayaVoiceGC("scream14", "...");
				BAD_BadEndStartCoroutine(1.5f, 100);
				foreach (Transform item in base.gameObject.transform)
				{
					if (item.gameObject.name.Contains("Internal"))
					{
						Quaternion localRotation = item.localRotation;
						localRotation.z = 0f;
						item.localRotation = localRotation;
					}
				}
				break;
			case 100:
				GC.StopAllSFXCurrentylPlaying();
				if (countBEStuff == 0)
				{
					BAD_InitiateDialogue(9033);
					countBEStuff++;
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 16, playThisEroAnim);
				BAD_BadEndStartCoroutine(0.5f, 101);
				break;
			case 101:
				if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9034);
					countBEStuff++;
				}
				if (countBEStuff == 3)
				{
					BAD_InitiateDialogue(9036);
					countBEStuff++;
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 16, text3);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(9999999f, 102);
				break;
			case 102:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 5f);
				intGeneric = 2;
				floatGeneric = 0f;
				BAD_BadEndStartCoroutine(6f, 200);
				break;
			case 200:
				if (countBEStuff >= 4)
				{
					countBEStuff++;
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 15, playThisEroAnim2);
				BAD_BadEndStartCoroutine(1f, 201);
				break;
			case 201:
				if (countBEStuff == 2)
				{
					BAD_InitiateDialogue(9035);
					countBEStuff++;
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, text2);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				BAD_BadEndStartCoroutine(999999f, 202);
				break;
			case 202:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(3f, 100);
				break;
			}
		}
		switch (currentBEPhase)
		{
		case 100:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 101, "", 0.5f);
			break;
		case 101:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 16, text3);
			if (currentEroPhase >= 19)
			{
				BAD_BadEndStartCoroutine(0.1f, 102, setTempVoidState: true);
			}
			break;
		case 201:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 20, text2);
			switch (currentEroPhase)
			{
			case 25:
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, playThisEroAnim3);
				if (floatGeneric < (float)intGeneric)
				{
					Vector2 vector = enmObjRefs.StartPointCenterMass.position;
					vector.x += 0.8f;
					vector.y -= 0.5f;
					floatGeneric++;
					GC.InstantiateVFX(GameCore.VisualEffect.Spit, vector, "Arachne:BadEnd");
					GameObject gameObject = null;
					gameObject = ((!GC.MATH_RNG_ROLL(50)) ? GetMiscGameObject(1) : GetMiscGameObject(0));
					UnityEngine.Object.Instantiate(gameObject, vector, Quaternion.identity, GC.refOBJEnemiesParent.transform);
					GC.CallReferenceMayaVoiceGC("rng-Climax", "ArachnedBadEnd");
					GC.PlaySFXEro(GameCore.EroSound.Cum5, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
					vector = enmObjRefs.StartPointCenterMass.position;
					vector.y += 2f;
					GameObject obj = UnityEngine.Object.Instantiate(GetMiscGameObject(GC.MATH_RNG_INT(2, 7)), vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
					Physics2D.IgnoreCollision(obj.GetComponent<Collider2D>(), GetComponent<CompositeCollider2D>(), ignore: true);
					vector.y--;
					if (GC.MATH_RNG_ROLL(50))
					{
						vector.x += GC.MATH_RNG_FLOAT(1.5f, 4f);
					}
					else
					{
						vector.x += GC.MATH_RNG_FLOAT(-4f, -1.5f);
					}
					obj.GetComponent<ProjectileLogic>().specialForce = vector;
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.ArachneClimax, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
				}
				break;
			case 20:
				if (floatGeneric >= (float)intGeneric)
				{
					BAD_BadEndStartCoroutine(0.1f, 202);
				}
				break;
			}
			break;
		}
		GC.BEpartInfo(countBEStuff, 6);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 6)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 707, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void BAD09(int currentBEPhase, int currentEroPhase)
	{
		string text = "CWW-BE-01Tease1";
		string playThisEroAnim = "CWW-BE-01Tease1Silent";
		string text2 = "CWW-BE-08Blowjob1";
		string text3 = "CWW-BE-03Loop1";
		if (countBEStuff > 0)
		{
			text3 = "CWW-BE-03Loop1Belly";
		}
		if (crtBadEnd == null)
		{
			switch (currentBEPhase)
			{
			case 0:
				if (currentEroPhase == 8 && GC.isPlayerFainted && GC.playerSpecialAnimCount >= 5)
				{
					BAD_BadEndStartCoroutine(12f, 50);
				}
				break;
			case 50:
				objAN.SetInteger("eroVariantID", 99);
				countBEStuff = 0;
				BAD_BadEndStartCoroutine(4f, 51);
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				break;
			case 51:
				boolGeneric = true;
				eroCore.forceDefaultFacing = true;
				GC.SpecialEroTeleport(base.gameObject, "WerewolfBoss");
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, playThisEroAnim);
				BAD_BadEndStartCoroutine(1f, 52);
				break;
			case 52:
				GC.PlayMusicTheme(GameCore.MusicTheme.BadEndTheme, doFadeIn: false, 0f, doLoop: true);
				BAD_InitiateDialogue(9037);
				BAD_BadEndStartCoroutine(1f, 100);
				break;
			case 100:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				if (countBEStuff == 0)
				{
					BAD_InitiateDialogue(9038);
				}
				else if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9042);
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, text);
				BAD_BadEndStartCoroutine(999999f, 101);
				break;
			case 101:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 102);
				break;
			case 102:
				BAD_BadEndStartCoroutine(1f, 200);
				break;
			case 200:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 24, text3);
				if (countBEStuff == 0)
				{
					BAD_InitiateDialogue(9039);
				}
				else if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9043);
				}
				BAD_BadEndStartCoroutine(999999f, 201);
				break;
			case 201:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				BAD_BadEndStartCoroutine(4f, 202);
				break;
			case 202:
				BAD_BadEndStartCoroutine(1f, 300);
				break;
			case 300:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f);
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 30, text2);
				if (countBEStuff == 0)
				{
					BAD_InitiateDialogue(9040);
				}
				else if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9044);
				}
				BAD_BadEndStartCoroutine(999999f, 301);
				break;
			case 301:
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: true, 3f);
				if (countBEStuff == 0 || countBEStuff == 1)
				{
					BAD_BadEndStartCoroutine(3.5f, 302);
				}
				else
				{
					BAD_BadEndStartCoroutine(3.5f, 303);
				}
				break;
			case 302:
				if (countBEStuff == 0)
				{
					BAD_InitiateDialogue(9041);
				}
				else if (countBEStuff == 1)
				{
					BAD_InitiateDialogue(9045);
				}
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, playThisEroAnim);
				BAD_BadEndStartCoroutine(999999f, 303);
				break;
			case 303:
				if (countBEStuff <= 15)
				{
					int num = GC.MATH_RNG_INT(2, 3);
					for (int i = 0; i < num; i++)
					{
						Vector2 vector = base.transform.position;
						vector.x += GC.MATH_RNG_FLOAT(-2f, 2f);
						UnityEngine.Object.Instantiate(GetMiscGameObject(GC.MATH_RNG_INT(5, 6)), vector, Quaternion.identity, GC.refOBJEnemiesParent.transform);
					}
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CityDogBabyWhine, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.1f);
				}
				countBEStuff++;
				localEroScript.MidEroManipulateValues(resetLoopCounters: true, 20, playThisEroAnim);
				BAD_BadEndStartCoroutine(3f, 100);
				break;
			}
		}
		switch (currentBEPhase)
		{
		case 52:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 100, "", 1f);
			break;
		case 100:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 20, text);
			if (currentEroPhase >= 23)
			{
				BAD_BadEndStartCoroutine(0.1f, 101, setTempVoidState: true);
			}
			break;
		case 200:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 24, text3);
			if (currentEroPhase == 29)
			{
				BAD_BadEndStartCoroutine(3f, 201, setTempVoidState: true);
			}
			break;
		case 300:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StayOnAnimationPhase, 30, text2);
			if (currentEroPhase == 35)
			{
				BAD_BadEndStartCoroutine(3f, 301, setTempVoidState: true);
			}
			break;
		case 302:
			BAD_WhileDialogueDoBehavior(BDEDiagBehavior.StallCoroutine, 303, "", 2f);
			break;
		}
		GC.BEpartInfo(countBEStuff, 3);
		if (!boolFinal_BE_Message && countBEStuff + 1 == 3)
		{
			boolFinal_BE_Message = true;
			GC.DisplaySystemMessageUI(0, 708, 10f, "", isFlavorCentralMessage: true);
		}
	}

	private void EASYMODE_ApplyChanges(bool forceInitialConfig)
	{
		if (forceInitialConfig || localDifficulty != GC.configDifficultyType)
		{
			if (GC.configDifficultyType == GameCore.DifficultyType.Easy)
			{
				enmBaseStats.maxLife = Mathf.RoundToInt((float)enmBaseStats.maxLife * 0.5f);
				enmBaseStats.baseDefense = Mathf.RoundToInt((float)enmBaseStats.baseDefense * 0.66f);
				enmBaseStats.baseTouchDamage = Mathf.RoundToInt((float)enmBaseStats.baseTouchDamage * 0.5f);
				enmCmnSkill.skillPercentChance = Mathf.RoundToInt((float)enmCmnSkill.skillPercentChance * 0.5f);
				enmCmnSkill.baseSkillDamage = Mathf.RoundToInt((float)enmCmnSkill.baseSkillDamage * 0.5f);
				enmCmnSkill.minSkillFrequency = Mathf.RoundToInt((float)enmCmnSkill.minSkillFrequency * 1.5f);
				enmCmnSkill.maxSkillFrequency = Mathf.RoundToInt((float)enmCmnSkill.maxSkillFrequency * 1.5f);
				amountLife = enmBaseStats.maxLife;
				UpdateHealthBarDisplay();
			}
			else
			{
				enmBaseStats = defaultBaseStats;
				enmCmnSkill = defaultSkillStats;
				amountLife = enmBaseStats.maxLife;
				UpdateHealthBarDisplay();
			}
			localDifficulty = GC.configDifficultyType;
		}
	}

	private void EASYMODE_StoreDefaults()
	{
		defaultBaseStats = enmBaseStats;
		defaultSkillStats = enmCmnSkill;
	}

	public GameObject GetMiscGameObject(int index)
	{
		try
		{
			if (enmMisc.Objects.Length > index && enmMisc.Objects[index] != null)
			{
				return enmMisc.Objects[index];
			}
			Debug.Log("ENEMY_LOGIC[GetMiscGameObject] Error: enmMisc.Objects is empty or index " + index + " is null!");
		}
		catch (Exception ex)
		{
			Debug.Log("ENEMY_LOGIC[GetMiscGameObject] Error: " + ex.Message);
			throw;
		}
		return null;
	}

	private void SetNewLimitIdleTime(bool doMax = false)
	{
		if (doMax)
		{
			limitIdle = enmCmnBehaviour.timerIdleMax;
		}
		else
		{
			limitIdle = GC.MATH_RNG_INT(enmCmnBehaviour.timerIdleMin, enmCmnBehaviour.timerIdleMax);
		}
	}

	private void SetNewLimitRunningTime(int customMin = 0, int customMax = 0)
	{
		if (customMin > 0 && customMax > 0)
		{
			limitRun = GC.MATH_RNG_INT(customMin, customMax);
		}
		else
		{
			limitRun = GC.MATH_RNG_INT(enmCmnBehaviour.timerRunMin, enmCmnBehaviour.timerRunMax);
		}
	}

	private bool IsPlayerProjectileExpections(bool isPlayerProjectileException)
	{
		if (isPlayerProjectileException)
		{
			return true;
		}
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.WolfGirlMimic && (objAN.GetInteger("genericPhase") == 0 || objAN.GetInteger("genericPhase") == 1))
		{
			return true;
		}
		return false;
	}

	private void StartStopSpriteFlashing(bool start, flashType type, float frequency)
	{
		if (!canSpriteFlash)
		{
			return;
		}
		if (start)
		{
			currentFlashType = type;
			if (IsInvoking("DoSpriteFlashingRepeating"))
			{
				CancelInvoke("DoSpriteFlashingRepeating");
			}
			InvokeRepeating("DoSpriteFlashingRepeating", 0f, frequency);
			return;
		}
		CancelInvoke("DoSpriteFlashingRepeating");
		switch (type)
		{
		case flashType.Transparent:
			objRE.enabled = true;
			break;
		case flashType.HalfWhite:
			matSpriteFlash.SetColor("_Color", colorOriginalMaterial);
			break;
		}
		currentFlashType = flashType.Null;
	}

	private void DoSpriteFlashingRepeating()
	{
		switch (currentFlashType)
		{
		case flashType.Transparent:
			objRE.enabled = !objRE.enabled;
			break;
		case flashType.HalfWhite:
			if (matSpriteFlash.GetColor("_Color") != colorFlashingMaterial)
			{
				matSpriteFlash.SetColor("_Color", colorFlashingMaterial);
			}
			else
			{
				matSpriteFlash.SetColor("_Color", colorOriginalMaterial);
			}
			break;
		}
	}

	private void SetEnemyTransparency(float alphaValue)
	{
		Color color = objRE.color;
		color.a = alphaValue;
		objRE.color = color;
	}

	private void Spawn2DSpriteTrail()
	{
		GameObject obj = new GameObject();
		SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
		Color color = objRE.color;
		color.a = 0.6f;
		spriteRenderer.color = color;
		spriteRenderer.sprite = objRE.sprite;
		spriteRenderer.sortingOrder = objRE.sortingOrder;
		obj.transform.position = base.transform.position;
		obj.transform.localScale = base.transform.localScale;
		UnityEngine.Object.Destroy(obj, 0.2f);
	}

	private void Spawn2DSpriteTrailVFX()
	{
		if (trailEffectVFX != GameCore.VisualEffect.Null)
		{
			GC.InstantiateVFX(trailEffectVFX, base.transform.position, "EnemyGeneralLogic:Spawn@DSpriteTrailVFX");
		}
	}

	private void StartStopSpawnSpriteTrail(bool stateON, float repeatFrequency, GameCore.VisualEffect vfxTrail = GameCore.VisualEffect.Null)
	{
		if (vfxTrail == GameCore.VisualEffect.Null)
		{
			if (stateON && !IsInvoking("Spawn2DSpriteTrail"))
			{
				InvokeRepeating("Spawn2DSpriteTrail", 0f, repeatFrequency);
			}
			else if (!stateON && IsInvoking("Spawn2DSpriteTrail"))
			{
				CancelInvoke("Spawn2DSpriteTrail");
			}
			return;
		}
		trailEffectVFX = vfxTrail;
		if (stateON && !IsInvoking("Spawn2DSpriteTrailVFX"))
		{
			InvokeRepeating("Spawn2DSpriteTrailVFX", 0f, repeatFrequency);
		}
		else if (!stateON && IsInvoking("Spawn2DSpriteTrailVFX"))
		{
			CancelInvoke("Spawn2DSpriteTrailVFX");
		}
	}

	private void HACK_COMBO_ENEMIES_SPAWN_GALLERY()
	{
	}

	private IEnumerator waitStartLogicDelay(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtStartLogicDelay = null;
	}

	public bool GetIsGalleryView()
	{
		return isGalleryView;
	}

	public Vector2 GetDmgDisplayOffset()
	{
		return eroCore.offsetDamageDisplay;
	}

	public Vector3 GetEnemyCenterPos()
	{
		if (enmObjRefs.StartPointCenterMass != null)
		{
			return enmObjRefs.StartPointCenterMass.position;
		}
		return base.transform.position;
	}

	public float GetEroGracePeriod()
	{
		return eroGracePeriod;
	}

	public float GetEnemyFacing()
	{
		return base.transform.localScale.x;
	}

	public int GetDmgEro()
	{
		return dmgOfEro;
	}

	public void SetGrabbing(bool state)
	{
		isGrabbing = state;
		if (state)
		{
			GC.enemyGrabbingPlayer = currentEnemyType;
			GC.objEnemyGrabbingPlayer = base.gameObject;
		}
	}

	public bool GetIsCurrentlyGrabing()
	{
		return isGrabbing;
	}

	public void SetAddToRendererLayerOrder(int value)
	{
		objRE.sortingOrder += value;
	}

	public void SetEnemyFacing(float direction)
	{
		enemySetScale.x = direction;
		base.transform.localScale = enemySetScale;
	}

	public void SetAnimSpeed(float value)
	{
		objAN.SetFloat("speedAnimation", value);
	}

	public void SetParentRendererAlpha(float alphaValue)
	{
		Color color = objRE.color;
		color.a = alphaValue;
		objRE.color = color;
	}

	public void SetIsUsingSkill(int state)
	{
		if (state != 0)
		{
			isUsingSkill = true;
		}
		else
		{
			isUsingSkill = false;
		}
	}

	public void SetPairedObject(GameObject sentOBJ)
	{
		objGeneric = sentOBJ;
	}

	public void SetEroPhase(int eroPhase)
	{
		_eroPhase = eroPhase;
	}

	public int GetRandomEroSceneVariantID()
	{
		int num = 0;
		int num2 = 0;
		if (isGalleryView && intGalleryVariantEroID > 0)
		{
			return intGalleryVariantEroID;
		}
		if (isArenaGallerySpecial && !arenaHasPlayedPreferedEroID)
		{
			arenaHasPlayedPreferedEroID = true;
			return arenaPreferedEroID;
		}
		if (eroCore.NonRepeatDoSequential)
		{
			return prevEroSceneVariantID = ((prevEroSceneVariantID == 0) ? listOfValidEroSceneVariantsID[0] : ((prevEroSceneVariantID != listOfValidEroSceneVariantsID[listOfValidEroSceneVariantsID.Count - 1]) ? listOfValidEroSceneVariantsID[listOfValidEroSceneVariantsID.FindIndex((int x) => x == prevEroSceneVariantID) + 1] : listOfValidEroSceneVariantsID[0]));
		}
		if (listOfValidEroSceneVariantsID != null && listOfValidEroSceneVariantsID.Count > 1)
		{
			if (eroCore.cantRepeatLastAlt)
			{
				do
				{
					num = GC.MATH_RNG_INT_ARRAY(listOfValidEroSceneVariantsID.ToArray());
					num2++;
				}
				while (num == prevEroSceneVariantID || num2 < 10);
			}
			else
			{
				num = GC.MATH_RNG_INT_ARRAY(listOfValidEroSceneVariantsID.ToArray());
			}
			prevEroSceneVariantID = num;
		}
		if (listOfValidEroSceneVariantsID != null && listOfValidEroSceneVariantsID.Count == 1)
		{
			num = (prevEroSceneVariantID = listOfValidEroSceneVariantsID[0]);
		}
		return num;
	}

	public GameObject GetInternalsObjVariantByID(int thisID)
	{
		for (int i = 0; i < eroCore.eroSoloAlts.Length; i++)
		{
			if (eroCore.eroSoloAlts[i].GenericID == thisID)
			{
				return eroCore.eroSoloAlts[i].InternalsOBJ;
			}
		}
		return null;
	}

	public GameObject GetInternalsObjComboByID(int thisID)
	{
		for (int i = 0; i < eroCore.eroComboScenes.Length; i++)
		{
			if (eroCore.eroComboScenes[i].GenericID == thisID)
			{
				return eroCore.eroComboScenes[i].InternalsOBJ;
			}
		}
		return null;
	}

	public void ERO_ToggleSpecialEroCollider(bool activate)
	{
		if (activate)
		{
			if (eroCore.eroCollDimensions != Vector2.zero)
			{
				objBox2D.size = eroCore.eroCollDimensions;
			}
			if (eroCore.eroCollOffset != Vector2.zero)
			{
				objBox2D.offset = eroCore.eroCollOffset;
			}
			if (eroCore.eroCollSwitchTrigger)
			{
				objBox2D.isTrigger = !objBox2D.isTrigger;
			}
			return;
		}
		if (objBox2D.size != vecOriginalColliderSize)
		{
			objBox2D.size = vecOriginalColliderSize;
		}
		if (objBox2D.offset != vecOriginalColliderOffset)
		{
			objBox2D.offset = vecOriginalColliderOffset;
		}
		if (eroCore.eroCollSwitchTrigger)
		{
			objBox2D.isTrigger = !objBox2D.isTrigger;
		}
		if (enmMisc.isFlyer)
		{
			base.transform.position += new Vector3(0f, 1f, 0f);
			objRB.velocity = Vector2.zero;
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = 0f;
		}
	}

	public void ERO_ToggleSpecialEroCollider(Vector2 changeSize, Vector2 changeOffset, bool isTrigger)
	{
		objBox2D.size = changeSize;
		objBox2D.offset = changeOffset;
		objBox2D.isTrigger = isTrigger;
	}

	public void ATKSpecial_CollPlayerDetected()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Batgirl:
			currentFlyingState = enemyFlyingState.SpecialAction;
			break;
		case GameCore.EnemyType.WormOverGround:
			if (!isGrabbing && !GC.playerIsBeingGrabed && !GC.isPlayerFainted && CanGrabPlayerForEro("[PLAYER]"))
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
				if ((bool)gameObject)
				{
					CallEroLogic(gameObject);
				}
			}
			break;
		case GameCore.EnemyType.SewerBug:
			if (!GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Stuned))
			{
				GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.Stuned, 1.5f, "ENMY_LOGIC:ATKSpecial");
			}
			break;
		}
	}

	public void BossRematchInitializeBehavior()
	{
		InitializeEnemyMainBehaviour();
	}

	public void EVENT_ANIM_END()
	{
		objAN.SetBool("isDiagAction", value: false);
		GC.EventSpecificAnimationDialoguehasCompleted();
	}

	public void DebugForceBossEnragePhase()
	{
		CheckBossEnragedPhaseChangeLogic("", 0f, playSFX: false, 0, forceEnrage: true);
	}

	public bool HasAnyEroVariants()
	{
		if (eroCore.eroSoloAlts != null && eroCore.eroSoloAlts.Length != 0)
		{
			return true;
		}
		return false;
	}

	public bool HasAnyComboscenes()
	{
		if (eroCore.eroComboScenes != null && eroCore.eroComboScenes.Length != 0)
		{
			return true;
		}
		return false;
	}

	private void GenericWalker_MainBehaviour()
	{
		if (!GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.MainBehaviour, enmCmnBehaviour.behaviourRangeX, enmCmnBehaviour.behaviourRangeY))
		{
			enemyCurrentXMove = 0f;
			return;
		}
		if (isGrabbing)
		{
			if (isUsingSkill)
			{
				EnemyConcludeSkill();
			}
			invertSpriteFacing = false;
			StartStopSpawnSpriteTrail(stateON: false, 0f);
			if (!CheckEroSpecialMovement())
			{
				enemyCurrentXMove = 0f;
				enemyCurrentYMove = 0f;
			}
			return;
		}
		if (isStagger && enemyCurrentYMove <= 0f && CheckCollisionGround())
		{
			if (enemyCurrentXMove > 1f)
			{
				enemyCurrentXMove = 1f;
			}
			else if (enemyCurrentXMove < -1f)
			{
				enemyCurrentXMove = -1f;
			}
		}
		countFrequency += enmBaseStats.freqRepeatBehaviour;
		GenericWalker_SpecificBehaviors();
		CHASE_InvokeFaintedChaseLogic();
		WalkerCycleStates();
		WALKER_CHECK_IDLE_SOUND();
		if (!IsInvoking("WALKER_StuckPosition") && enmCmnBehaviour.canJumpToMove)
		{
			InvokeRepeating("WALKER_StuckPosition", 0f, 1f);
		}
		if (isUsingSkill)
		{
			CheckDuringSkillUsage(enmCmnSkill.haltSkillOnPlayerFaint);
		}
		else if (!GC.playerIsBeingGrabed && !GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified))
		{
			EnemyCheckSkillExecution();
		}
		if (isGauntletSpecial && !GC.isGauntletFight && !isDeath)
		{
			StartDeathLogicBehaviour();
		}
		RAGE_CursedItemCheck();
	}

	private void WalkerCycleStates()
	{
		if (!isStagger)
		{
			switch (currentEnemyState)
			{
			case enemyState.Standing:
				WALKER_STANDING_IDLE();
				break;
			case enemyState.Patrolling:
				WALKER_PATROLLING();
				break;
			case enemyState.Chasing:
				WALKER_CHASING();
				break;
			case enemyState.Jumping:
			case enemyState.Falling:
				WALKER_JUMPFALL();
				break;
			case enemyState.Crouching:
				break;
			}
		}
	}

	private void WALKER_STANDING_IDLE()
	{
		if (countFrequency >= (float)limitIdle)
		{
			if (GC.MATH_RNG_INT(0, 1) == 1)
			{
				enemyCurrentXMove = 1f;
			}
			else
			{
				enemyCurrentXMove = -1f;
			}
			if (!CheckIfThereIsGroundInFront())
			{
				enemyCurrentXMove *= -1f;
			}
			countFrequency = 0f;
			SetNewLimitIdleTime();
		}
		if (!CheckCollisionGround() && !CheckCollisionMovingPlatforms())
		{
			objRB.velocity = new Vector2(0f, -0.5f);
			currentEnemyState = enemyState.Falling;
		}
		if (GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 6f, 3f) && ((base.transform.position.x > GC.playerCurrentWorldXPosition && base.transform.localScale.x == 1f) || (base.transform.position.x < GC.playerCurrentWorldXPosition && base.transform.localScale.x == -1f)))
		{
			STATE_MACHINE_FLIP_FACE_PLAYER();
		}
	}

	private void WALKER_PATROLLING()
	{
		_ = currentEnemyType;
		if (Mathf.RoundToInt(objRB.velocity.x) == 0)
		{
			GenericWhatToDoWhenHittingObstacle();
		}
		if (!CheckIfThereIsGroundInFront())
		{
			if (GC.MATH_RNG_INT(1, 100) <= 35 && enmCmnBehaviour.canJumpToMove)
			{
				GenericExecuteJump(1f, playSound: true);
				return;
			}
			enemyCurrentXMove *= -1f;
			countFlipChanged++;
		}
		if (countFrequency >= (float)limitRun || (countFlipChanged >= 4 && countFrequency <= 2f))
		{
			enemyCurrentXMove = 0f;
			countFrequency = 0f;
			countFlipChanged = 0;
			SetNewLimitRunningTime();
		}
		if (!CheckCollisionGround() && !CheckCollisionMovingPlatforms())
		{
			objRB.velocity = new Vector2(0f, -0.5f);
			currentEnemyState = enemyState.Falling;
		}
	}

	private void WALKER_JUMPFALL()
	{
		if (Mathf.Round(objRB.velocity.x) == 0f && Mathf.Round(objRB.velocity.y) == 0f && !CheckCollisionGround() && currentEnemyState == enemyState.Falling)
		{
			float x = GC.MATH_RNG_FLOAT(-0.1f, 0.1f);
			base.transform.position += new Vector3(x, 0f, 0f);
		}
	}

	private void WALKER_CHASING()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.WyvernGirl)
		{
			if (enmMisc.isPlayerTransformed && objGeneric != null)
			{
				chaseTarget.x = objGeneric.transform.position.x;
				chaseTarget.y = objGeneric.transform.position.y;
			}
			else
			{
				chaseTarget.x = GC.playerCurrentWorldXPosition;
				chaseTarget.y = GC.playerCurrentWorldYPosition;
			}
		}
		else
		{
			chaseTarget.x = GC.playerCurrentWorldXPosition;
			chaseTarget.y = GC.playerCurrentWorldYPosition;
		}
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.SalamanderGirl:
		case GameCore.EnemyType.Manequin:
		case GameCore.EnemyType.Dogboy:
		case GameCore.EnemyType.GeckNewt:
			if ((!GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 12f, 6f) && !isUsingSkill) || GC.playerIsBeingGrabed || GC.isPlayerGoneForGood)
			{
				isChasingFainted = false;
				countFrequency = 0f;
				SetNewLimitIdleTime();
				currentEnemyState = enemyState.Standing;
				return;
			}
			if (isUsingSkill)
			{
				return;
			}
			break;
		}
		if (base.transform.position.x > chaseTarget.x + GC.MATH_RNG_FLOAT(1f, 3f) && enemyCurrentXMove == 1f)
		{
			if (GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 5f, 3f))
			{
				CHASE_StandToWatch();
				return;
			}
			enemyCurrentXMove = -1f;
		}
		else if (base.transform.position.x < chaseTarget.x - GC.MATH_RNG_FLOAT(1f, 3f) && enemyCurrentXMove == -1f)
		{
			if (GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 5f, 3f))
			{
				CHASE_StandToWatch();
				return;
			}
			enemyCurrentXMove = 1f;
		}
		if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomXOnly, 1f) && base.transform.position.y < chaseTarget.y)
		{
			GenericWhatToDoWhenHittingObstacle();
		}
		if (Mathf.RoundToInt(objRB.velocity.x) == 0)
		{
			GenericWhatToDoWhenHittingObstacle(checkRepeatedJumpInPlace: true);
		}
		if (enemyCurrentXMove == 0f)
		{
			if (base.transform.position.x > chaseTarget.x)
			{
				enemyCurrentXMove = -1f;
			}
			else
			{
				enemyCurrentXMove = 1f;
			}
		}
	}

	private void WALKER_CHECK_IDLE_SOUND()
	{
		if (currentEnemyState == enemyState.Standing)
		{
			if (hasPlayedIdleSound || crtStartLogicDelay != null)
			{
				return;
			}
			switch (currentEnemyType)
			{
			case GameCore.EnemyType.MushroomGirl:
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MushroomGirlLaugh, 35, checkForDuplicity: false, base.transform.position, 0.95f, 1.05f);
				break;
			case GameCore.EnemyType.WyvernGirl:
				if (enmMisc.isPlayerTransformed)
				{
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WyvernCast, 70, checkForDuplicity: true, base.transform.position, 1.9f, 2.1f);
				}
				break;
			}
			hasPlayedIdleSound = true;
		}
		else
		{
			hasPlayedIdleSound = false;
		}
	}

	private void WALKER_StuckPosition()
	{
		if (GC.MATH_RoundFloatDecimals(base.transform.position.x, 0) == GC.MATH_RoundFloatDecimals(vecLastStoredPos.x, 0) && GC.MATH_RoundFloatDecimals(base.transform.position.y, 0) == GC.MATH_RoundFloatDecimals(vecLastStoredPos.y, 0) && (currentEnemyState == enemyState.Patrolling || currentEnemyState == enemyState.Chasing))
		{
			GenericExecuteJump();
		}
		vecLastStoredPos = base.transform.position;
	}

	private void GenericWalker_SpecificBehaviors()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.SalamanderGirl:
			if (currentEnemyState == enemyState.Standing && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 8f, 3f) && GC.MATH_RNG_INT(1, 100) <= 15)
			{
				isChasingFainted = true;
			}
			break;
		case GameCore.EnemyType.Manequin:
			if (currentEnemyState == enemyState.Standing && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 4f, 3f) && GC.MATH_RNG_INT(1, 100) <= 20)
			{
				isChasingFainted = true;
			}
			break;
		case GameCore.EnemyType.GeckNewt:
			if (currentEnemyState == enemyState.Standing && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 2f) && GC.MATH_RNG_INT(1, 100) <= 20)
			{
				isChasingFainted = true;
			}
			break;
		case GameCore.EnemyType.Dogboy:
			if (objAN.GetInteger("genericPhase") == 1 && !listOfExternalAttacks[0].activeInHierarchy)
			{
				listOfExternalAttacks[0].SetActive(value: true);
			}
			else if (objAN.GetInteger("genericPhase") != 1 && listOfExternalAttacks[0].activeInHierarchy)
			{
				listOfExternalAttacks[0].SetActive(value: false);
			}
			if (currentEnemyState == enemyState.Standing && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 8f, 3f) && GC.MATH_RNG_INT(1, 100) <= 10)
			{
				isChasingFainted = true;
			}
			break;
		case GameCore.EnemyType.LizardShield:
			if (!isUsingSkill && objAN.GetInteger("genericPhase") != 0)
			{
				ChangeAnimGenericPhaseID(0, isInstant: true, 0f);
			}
			break;
		case GameCore.EnemyType.LizardAxe:
			if (!isUsingSkill && objAN.GetInteger("genericPhase") != 0)
			{
				ChangeAnimGenericPhaseID(0, isInstant: true, 0f);
			}
			if (!isUsingSkill && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 10f, 6f) && GC.MATH_RNG_INT(1, 1000) <= 10)
			{
				isUsingSkill = true;
				ChangeAnimGenericPhaseID(11, isInstant: true, 0f);
			}
			break;
		case GameCore.EnemyType.Arachne:
			if (GC.MATH_RNG_INT(1, 1000) <= 50 && !isUsingSkill && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && objAN.GetInteger("genericPhase") == 0)
			{
				if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 4f, 3f))
				{
					GENERIC_ArachneDoJump(1.5f, 1.1f);
				}
				else if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 8f, 5f))
				{
					GENERIC_ArachneDoJump(2.1f, 1.3f);
				}
			}
			if (objAN.GetInteger("genericPhase") >= 2 && (CheckCollisionGround() || CheckCollisionMovingPlatforms()))
			{
				enemyCurrentXMove = 0f;
				ChangeAnimGenericPhaseID(0, isInstant: false, 0f);
			}
			break;
		case GameCore.EnemyType.Lamia:
			if (!isUsingSkill && GC.MATH_RNG_INT(1, 1000) <= 50 && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 4f, 1f))
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
				SetNewLimitIdleTime();
				currentEnemyState = enemyState.Standing;
				enemyCurrentXMove = 0f;
				isUsingSkill = true;
				objAN.SetBool("isSkill", value: true);
				objAN.SetInteger("genericPhase", 2);
			}
			break;
		case GameCore.EnemyType.Kobold:
		case GameCore.EnemyType.SewerBug:
			if (!isUsingSkill && GC.MATH_RNG_INT(1, 1000) <= 75 && CheckCollisionGround() && !isStagger && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f))
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
				SetNewLimitIdleTime();
				if (base.transform.localScale.x == -1f)
				{
					enemyCurrentXMove = -2.5f;
				}
				else
				{
					enemyCurrentXMove = 2.5f;
				}
				GenericExecuteJump();
				isUsingSkill = true;
				objAN.SetBool("isSkill", value: true);
				objAN.SetInteger("genericPhase", 1);
				if (coroutExecuteSkill != null)
				{
					StopCoroutine(coroutExecuteSkill);
				}
				coroutExecuteSkill = StartCoroutine(EnemyWaitForSkillExecution(0.2f));
				listOfExternalAttacks[0].SetActive(value: true);
			}
			else if ((isUsingSkill && CheckCollisionGround() && objAN.GetInteger("genericPhase") == 0) || (objAN.GetInteger("genericPhase") == 0 && listOfExternalAttacks[0].activeInHierarchy && CheckCollisionGround()))
			{
				listOfExternalAttacks[0].SetActive(value: false);
			}
			break;
		case GameCore.EnemyType.CentaurGirl:
			if (!isUsingSkill && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 1f) && GC.MATH_RNG_INT(1, 1000) <= 125)
			{
				countGenericStuff = 1;
				EnemyExecuteSkill();
			}
			break;
		case GameCore.EnemyType.Dominatrix:
			if (!isGrabbing)
			{
				GENERIC_Dominatrix_Dodge();
				break;
			}
			objAN.SetInteger("genericPhase", 0);
			invertSpriteFacing = false;
			StartStopSpawnSpriteTrail(stateON: false, 0f);
			break;
		case GameCore.EnemyType.FutaBunny:
			if (isUsingSkill)
			{
				if (enemyCurrentYMove >= 1f)
				{
					StartCoroutine(waitGENERIC(0.3f, "BunnyCarrot"));
					StartCoroutine(waitGENERIC(0.6f, "BunnyCarrot"));
					isUsingSkill = false;
				}
				if (CheckCollisionGround())
				{
					isUsingSkill = false;
				}
			}
			if (!isUsingSkill && GC.MATH_RNG_INT(1, 1000) <= 50 && CheckCollisionGround() && !isStagger && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.SkillUsage, enmCmnSkill.minSkillProximityX, enmCmnSkill.minSkillProximityY))
			{
				isUsingSkill = true;
				GenericExecuteJump();
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BunnyCast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			break;
		case GameCore.EnemyType.SailorBurly:
			if (objAN.GetInteger("genericPhase") == 1)
			{
				if (!isChasingFainted && isUsingSkill && enemyCurrentXMove == 0f)
				{
					ExecuteFaintedChaseLogic(faintedChaseType.Basic, enmCmnSkill.SkillAnimDuration);
				}
				if (!GetMiscGameObject(0).activeInHierarchy)
				{
					GetMiscGameObject(0).SetActive(value: true);
				}
			}
			else if (GetMiscGameObject(0).activeInHierarchy)
			{
				GetMiscGameObject(0).SetActive(value: false);
			}
			break;
		case GameCore.EnemyType.Failed:
			if (isUsingSkill && enemyCurrentYMove <= 0f && (CheckCollisionGround() || CheckCollisionMovingPlatforms()) && objAN.GetInteger("genericPhase") == 1)
			{
				Vector2 vector = base.transform.position;
				vector.y -= 1.4f;
				BasicProjectileInstantiate(null, overrideSpawnManual: true, vector.x, vector.y);
				EnemyConcludeSkill();
			}
			break;
		case GameCore.EnemyType.Medusa:
			if (isUsingSkill && !isGrabbing && !isChasingFainted && !isStagger)
			{
				if (!GetMiscGameObject(0).activeInHierarchy)
				{
					GetMiscGameObject(0).SetActive(value: true);
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MDS_Cast, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.05f);
				}
				if (currentEnemyState != enemyState.Standing)
				{
					SetNewLimitIdleTime();
					currentEnemyState = enemyState.Standing;
				}
				enemyCurrentXMove = 0f;
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			else if (GetMiscGameObject(0).activeInHierarchy)
			{
				GetMiscGameObject(0).SetActive(value: false);
			}
			if (isStagger && isUsingSkill)
			{
				EnemyConcludeSkill();
			}
			break;
		}
	}

	private void GENERIC_ArachneDoJump(float xSpeed, float jumpMulti)
	{
		ChangeAnimGenericPhaseID(1, isInstant: true, 0f);
		ChangeAnimGenericPhaseID(2, isInstant: false, 0.2f);
		STATE_MACHINE_FLIP_FACE_PLAYER();
		if (GC.playerCurrentWorldXPosition >= base.transform.position.x)
		{
			enemyCurrentXMove = xSpeed;
		}
		else
		{
			enemyCurrentXMove = 0f - xSpeed;
		}
		GenericExecuteJump(jumpMulti);
	}

	public void GENERIC_Lamia_Slide_Attack()
	{
		float num = 4.5f;
		STATE_MACHINE_FLIP_FACE_PLAYER();
		if (GC.playerCurrentWorldXPosition > base.transform.position.x)
		{
			enemyCurrentXMove = num;
		}
		else if (GC.playerCurrentWorldXPosition < base.transform.position.x)
		{
			enemyCurrentXMove = 0f - num;
		}
		if (coroutExecuteSkill != null)
		{
			StopCoroutine(coroutExecuteSkill);
		}
		coroutExecuteSkill = StartCoroutine(EnemyWaitForSkillExecution(0.5f));
	}

	private void GENERIC_Dominatrix_Dodge()
	{
		if (objAN.GetInteger("genericPhase") == 2)
		{
			if (CheckCollisionGround() || currentEnemyState == enemyState.Standing || currentEnemyState == enemyState.Patrolling)
			{
				objAN.SetInteger("genericPhase", 0);
				invertSpriteFacing = false;
				STATE_MACHINE_FLIP_FACE_PLAYER();
				countTimeToUseSkill++;
				return;
			}
			StartStopSpawnSpriteTrail(stateON: true, 0.1f);
		}
		else
		{
			StartStopSpawnSpriteTrail(stateON: false, 0f);
			invertSpriteFacing = false;
		}
		if ((currentEnemyState == enemyState.Standing || currentEnemyState == enemyState.Patrolling) && objAN.GetInteger("genericPhase") != 2 && !isUsingSkill && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && !isGrabbing && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 4f, 2f) && GC.MATH_RNG_INT(1, 1000) <= 100)
		{
			float num = 2f;
			if (GC.playerCurrentWorldXPosition > base.transform.position.x)
			{
				num *= -1f;
			}
			invertSpriteFacing = true;
			GenericExecuteJump(1.25f);
			enemyCurrentXMove = num;
			objAN.SetInteger("genericPhase", 2);
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.Domme_Dodge, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
		}
	}

	private IEnumerator waitForJumpDelay(float wait)
	{
		yield return new WaitForSeconds(wait);
		isJumpDelay = false;
		crtJumpDelay = null;
	}

	private void BasicStatic_MainBehaviour()
	{
		if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.MainBehaviour, enmCmnBehaviour.behaviourRangeX, enmCmnBehaviour.behaviourRangeY))
		{
			countFrequency += enmBaseStats.freqRepeatBehaviour;
			StaticEnemy_SpecialBehavior();
			if (!isUsingSkill && !GC.playerIsBeingGrabed)
			{
				EnemyCheckSkillExecution();
			}
		}
	}

	private void StaticEnemy_SpecialBehavior()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.TentacleTree:
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.PlayerToTheRightCustom, 3.5f, 2.5f) && !GetMiscGameObject(0).activeInHierarchy && !GC.playerIsBeingGrabed)
			{
				GetMiscGameObject(0).SetActive(value: true);
				GetMiscGameObject(0).GetComponent<Animator>().Play("StartAnim");
			}
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.PlayerToTheLeftCustom, 3.5f, 2.5f) && !GetMiscGameObject(1).activeInHierarchy && !GC.playerIsBeingGrabed)
			{
				GetMiscGameObject(1).SetActive(value: true);
				GetMiscGameObject(1).GetComponent<Animator>().Play("StartAnim");
			}
			break;
		case GameCore.EnemyType.PaintingEvil:
			if (!isUsingSkill && GetMiscGameObject(0).activeInHierarchy)
			{
				GetMiscGameObject(0).SetActive(value: false);
			}
			if (isGrabbing && isUsingSkill)
			{
				EnemyConcludeSkill();
			}
			if (isGrabbing || isUsingSkill || isArenaGallerySpecial)
			{
				colorGeneric.a = 1f;
			}
			else
			{
				colorGeneric.a = 0f;
			}
			objRE.color = colorGeneric;
			break;
		case GameCore.EnemyType.LatexMaiden:
			if (!isUsingSkill && GetMiscGameObject(0).activeInHierarchy)
			{
				GetMiscGameObject(0).SetActive(value: false);
			}
			if (isGrabbing && isUsingSkill)
			{
				EnemyConcludeSkill();
			}
			break;
		case GameCore.EnemyType.WormOverGround:
			if (listOfExternalAttacks == null)
			{
				listOfExternalAttacks = new List<GameObject>();
				listOfExternalAttacks.Add(GetMiscGameObject(0));
			}
			STATIC_WORMSPECIALBEHAVIOR();
			break;
		case GameCore.EnemyType.FleshBlossom:
			if (GC.isPlayerFainted && !isUsingSkill && !isGrabbing && !GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.SkillUsage, enmCmnSkill.minSkillProximityX, enmCmnSkill.minSkillProximityY))
			{
				EnemyExecuteSkill();
			}
			break;
		}
	}

	private void STATIC_WORMSPECIALBEHAVIOR()
	{
		STATE_MACHINE_FLIP_FACE_PLAYER();
		if (crtGeneric == null && base.gameObject.activeInHierarchy && !isDeath && !boolGeneric && !objAN.GetCurrentAnimatorStateInfo(0).IsName("WRM-Retract"))
		{
			crtGeneric = StartCoroutine(waitGENERIC(GC.MATH_RNG_INT(5, 10), "setupForRetracting"));
		}
		if ((boolGeneric || countGenericStuff >= 3) && !isDeath && !isGrabbing && !isUsingSkill && !objAN.GetCurrentAnimatorStateInfo(0).IsName("WRM-Retract"))
		{
			countGenericStuff = 0;
			objAN.SetInteger("genericPhase", 999);
			objAN.Play("WRM-Retract");
			GC.InstantiateVFX(GameCore.VisualEffect.RocksDigging, base.transform.position, "EnemyGeneralLogic:WORMDIG_RiseBehavior");
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WormDigDown, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
		}
		if (STATIC_WORMCANDOSKILL() && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f) && GC.MATH_RNG_INT(1, 1000) <= 100)
		{
			objAN.SetInteger("genericPhase", 10);
			countGenericStuff++;
			EnemyExecuteSkill();
		}
		if (STATIC_WORMCANDOSKILL() && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 8f, 4f) && GC.MATH_RNG_INT(1, 1000) <= 50)
		{
			objAN.SetInteger("genericPhase", 20);
			countGenericStuff++;
			EnemyExecuteSkill();
		}
		if (!objAN.GetCurrentAnimatorStateInfo(0).IsName("WRM-Rise") && !objAN.GetCurrentAnimatorStateInfo(0).IsName("WRM-Retract"))
		{
			WORMCOLL(0);
		}
		if (GetMiscGameObject(0).activeInHierarchy && !objAN.GetCurrentAnimatorStateInfo(0).IsName("WRM-SkillLunge"))
		{
			GetMiscGameObject(0).SetActive(value: false);
		}
	}

	private bool STATIC_WORMCANDOSKILL()
	{
		if (!isUsingSkill && countGenericStuff < 3 && objAN.GetInteger("genericPhase") != 999 && !isDeath && !isGrabbing && !objAN.GetCurrentAnimatorStateInfo(0).IsName("WRM-Retract") && !objAN.GetCurrentAnimatorStateInfo(0).IsName("WRM-Rise"))
		{
			return true;
		}
		return false;
	}

	private void WFG_RepeatingBehavior()
	{
		int integer = objAN.GetInteger("genericPhase");
		switch (integer)
		{
		case 0:
		case 1:
			if (eroCore.IsAllowedToGrab && !GC.isPlayerFainted)
			{
				eroCore.IsAllowedToGrab = false;
			}
			if (integer == 1 && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 7f, 4f))
			{
				ChangeAnimGenericPhaseID(0, isInstant: true, 0f);
			}
			if ((CheckCollisionGround() || CheckCollisionMovingPlatforms()) && !objBox2D.isTrigger && floatGeneric < 900f)
			{
				floatGeneric = 999f;
				if (crtGeneric != null)
				{
					StopCoroutine(crtGeneric);
					crtGeneric = null;
				}
				enemyCurrentXMove = 0f;
				enemyCurrentYMove = 0f;
				crtGeneric = StartCoroutine(WFG_waitToActSpecial(0.25f, 2));
			}
			if (!CheckCollisionGround() && !CheckCollisionMovingPlatforms() && objBox2D.isTrigger && crtGeneric == null)
			{
				WFG_ToggleChaseState(state: true);
			}
			break;
		case 10:
		case 11:
			if (GetMiscGameObject(0).activeInHierarchy)
			{
				GetMiscGameObject(0).SetActive(value: false);
			}
			if (integer == 11 && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomInverted, 12f, 9f))
			{
				WFG_ToggleChaseState(state: false);
			}
			if (isGrabbing)
			{
				WFG_ToggleChaseState(state: false);
			}
			if (CheckCollisionGround() || CheckCollisionMovingPlatforms())
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
				enemyCurrentXMove = 0f;
				if (enemyCurrentYMove <= 0f && crtGeneric == null)
				{
					BasicProjectileInstantiate();
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MimicLand, 0, checkForDuplicity: true, base.transform.position, 0.8f, 1.2f);
					crtGeneric = StartCoroutine(WFG_waitToActSpecial(0.5f, 1));
				}
			}
			else if (!CheckCollisionGround() && !CheckCollisionMovingPlatforms())
			{
				WFG_SpawnTrailEffect();
			}
			if (CheckCollisionCeiling() && enemyCurrentYMove > 0.5f)
			{
				enemyCurrentYMove = 0.5f;
			}
			break;
		}
	}

	private void WFG_RepeatingPeek()
	{
		if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomInverted, 9f, 6f) && GC.MATH_RNG_INT(1, 100) <= 20 && objAN.GetInteger("genericPhase") == 0)
		{
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.SetInteger("genericPhase", 1);
			ChangeAnimGenericPhaseID(0, isInstant: false, 2f);
		}
	}

	private void WFG_ToggleChaseState(bool state)
	{
		if (state)
		{
			STATE_MACHINE_FLIP_FACE_PLAYER();
			AlterSelfBoxCollider(BoxCollAction.ChangeBoth, new Vector2(1f, 1.75f), new Vector2(0f, -0.625f));
			objBox2D.isTrigger = false;
			eroCore.IsAllowedToGrab = true;
			base.gameObject.layer = LayerMask.NameToLayer("LayerEnemies");
			ChangeAnimGenericPhaseID(10, isInstant: true, 0f);
			ChangeAnimGenericPhaseID(11, isInstant: false, 1f);
			objAN.Play("WFG-Rise");
			canDoTouchDamage = true;
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MimicHowl, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		}
		else
		{
			ChangeAnimGenericPhaseID(0, isInstant: true, 0f);
		}
	}

	private void WFG_SpawnTrailEffect()
	{
		GC.InstantiateVFX(GameCore.VisualEffect.MimicChestTrail, base.transform.position + posGenericVector, "ENEMYGENERALOGIC:WFG_SpawnTrailEffect");
	}

	private IEnumerator WFG_waitToActSpecial(float wait, int action)
	{
		yield return new WaitForSeconds(wait);
		switch (action)
		{
		case 1:
		{
			float num = GC.MATH_RNG_FLOAT(0.9f, 1.2f);
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 4f, 3f))
			{
				num = GC.MATH_RNG_FLOAT(0.5f, 0.9f);
			}
			num = enmBaseStats.speedMoveX * num;
			if (GC.playerCurrentWorldXPosition >= base.transform.position.x)
			{
				enemyCurrentXMove = 1f * num;
			}
			else
			{
				enemyCurrentXMove = -1f * num;
			}
			GenericExecuteJump();
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MimicJump, 0, checkForDuplicity: true, base.transform.position, 0.8f, 1.2f);
			break;
		}
		case 2:
			canDoTouchDamage = false;
			enemyCurrentYMove = 0f;
			objRB.velocity = Vector2.zero;
			objBox2D.isTrigger = true;
			AlterSelfBoxCollider(BoxCollAction.Reset, Vector2.zero, Vector2.zero);
			base.gameObject.layer = LayerMask.NameToLayer("Default");
			floatGeneric = 0f;
			break;
		}
		crtGeneric = null;
	}

	private void MUSHROOM_Spawn_Spores()
	{
		int num = 8;
		Vector2[] array = new Vector2[num];
		array[0] = new Vector2(0.1f, 0.75f);
		array[1] = new Vector2(-0.1f, 0.75f);
		array[2] = new Vector2(0.2f, 0.35f);
		array[3] = new Vector2(-0.2f, 0.35f);
		array[4] = new Vector2(0.1f, 0.1f);
		array[5] = new Vector2(-0.1f, 0.1f);
		array[6] = new Vector2(0.15f, 0.55f);
		array[7] = new Vector2(-0.15f, 0.55f);
		for (int i = 0; i < num; i++)
		{
			UnityEngine.Object.Instantiate(enmObjRefs.objProjectile, enmObjRefs.StartPointCenterMass.position, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>().specialForce = array[i];
		}
	}

	private bool CheckEroSpecialMovement()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.Dogboy && isGrabbing && (objAN.GetInteger("intBEPhase") == 100 || objAN.GetInteger("intBEPhase") == 101 || objAN.GetInteger("intBEPhase") == -1))
		{
			if (currentEnemyState == enemyState.Standing)
			{
				currentEnemyState = enemyState.Patrolling;
			}
			countFrequency = 0f;
			countFlipChanged = 0;
			limitRun = 999999;
			if (enemyCurrentXMove == 0f)
			{
				if (GC.MATH_RNG_INT(0, 1) == 1)
				{
					enemyCurrentXMove = 1f;
				}
				else
				{
					enemyCurrentXMove = -1f;
				}
			}
			WalkerCycleStates();
			return true;
		}
		return false;
	}

	private bool CheckEroMoveException()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.BigInsect:
			if (isGrabbing && _eroPhase >= 2 && _eroPhase <= 6 && objAN.GetInteger("genericPhase") < 50)
			{
				return true;
			}
			break;
		case GameCore.EnemyType.Sinccubus:
			if (isGrabbing && _eroPhase == 13)
			{
				if (enmBaseStats.speedMoveX > 1f)
				{
					enmBaseStats.speedMoveX = 1f;
					enmBaseStats.speedMoveY = 1f;
					enmCmnBehaviour.timerIdleMin = 4;
					enmCmnBehaviour.timerIdleMax = 6;
					enmCmnBehaviour.timerRunMin = 6;
					enmCmnBehaviour.timerIdleMax = 8;
					countFrequency = 0f;
					countFlipChanged = 0;
					limitRun = 999999;
					BasicFlying_CycleStates();
				}
				return true;
			}
			break;
		}
		return false;
	}

	private void BasicFlyer_MainBehaviour()
	{
		if (!GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.MainBehaviour, enmCmnBehaviour.behaviourRangeX, enmCmnBehaviour.behaviourRangeY))
		{
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = 0f;
			return;
		}
		if ((isGalleryView || isAddComboGalleryView) && enmMisc.typeFlyerBehaviour != ENM_Miscellaneuss.FlyerType.AgressiveGallery)
		{
			enmMisc.typeFlyerBehaviour = ENM_Miscellaneuss.FlyerType.AgressiveGallery;
		}
		if (isGrabbing && !CheckEroMoveException())
		{
			if (isUsingSkill)
			{
				EnemyConcludeSkill();
			}
			if (countFrequency > 0f || currentFlyingState != enemyFlyingState.Idle || Mathf.RoundToInt(rotationZ) != 0)
			{
				countFrequency = 0f;
				objRB.velocity = Vector2.zero;
				enemyCurrentXMove = 0f;
				enemyCurrentYMove = -0.5f;
				rotationZ = 0f;
				invertSpriteFacing = false;
				currentFlyingState = enemyFlyingState.Idle;
			}
		}
		else
		{
			countFrequency += enmBaseStats.freqRepeatBehaviour;
			FLY_Genericbehaviors();
			BasicFlying_CheckLanded();
			CHASE_InvokeFaintedChaseLogic();
			BasicFlying_CycleStates();
		}
		if (isStagger)
		{
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = 0f;
			return;
		}
		if (isUsingSkill)
		{
			CheckDuringSkillUsage(enmCmnSkill.haltSkillOnPlayerFaint);
		}
		else if (!GC.playerIsBeingGrabed)
		{
			EnemyCheckSkillExecution();
		}
		if (_isRageGauntlet && !GC.isGauntletFight && !isDeath)
		{
			StartDeathLogicBehaviour();
		}
		RAGE_CursedItemCheck();
	}

	private void BasicFlying_CycleStates()
	{
		if (isStagger)
		{
			return;
		}
		switch (currentFlyingState)
		{
		case enemyFlyingState.Idle:
			if (limitIdle <= 0)
			{
				SetNewLimitIdleTime();
				enemyCurrentXMove = 0f;
				enemyCurrentYMove = 0f;
				rotationZ = 0f;
				invertSpriteFacing = false;
			}
			if ((isGrabbing || !FLY_DecideActionDuringIdleWander()) && countFrequency > (float)limitIdle)
			{
				countFrequency = 0f;
				limitIdle = 0;
				DecideNextFlyerState(1);
			}
			break;
		case enemyFlyingState.Patrolling:
			if (limitRun <= 0)
			{
				SetNewLimitRunningTime(enmCmnBehaviour.timerRunMin / 2, enmCmnBehaviour.timerRunMax / 2);
				RollRandomFlyerMov(rollRamdomX: true, rollRamdomY: false);
				countFlipChanged = 0;
				enemyCurrentYMove = 0f;
				countGenericStuff = 0;
				rotationZ = enmMisc.flyerZRotation;
				invertSpriteFacing = false;
			}
			if (Mathf.RoundToInt(objRB.velocity.x) == 0 || FLY_CheckActionBasedOnSecondsFrequency(3))
			{
				GenericWhatToDoWhenHittingObstacle();
			}
			FLY_DecideActionDuringPatrolling();
			if (countFrequency > (float)limitRun || countFlipChanged >= 6)
			{
				countFrequency = 0f;
				limitRun = 0;
				DecideNextFlyerState(2);
			}
			break;
		case enemyFlyingState.Wandering:
			if (limitRun <= 0)
			{
				SetNewLimitRunningTime();
				RollRandomFlyerMov(rollRamdomX: true, rollRamdomY: true);
				countGenericStuff = 0;
				rotationZ = enmMisc.flyerZRotation;
				invertSpriteFacing = false;
			}
			if (!FLY_DecideActionDuringIdleWander())
			{
				if (FLY_CheckActionBasedOnSecondsFrequency(2, 50))
				{
					RollRandomFlyerMov(rollRamdomX: true, rollRamdomY: true);
				}
				if (Mathf.RoundToInt(objRB.velocity.x) == 0)
				{
					GenericWhatToDoWhenHittingObstacle();
				}
				if (countFrequency > (float)limitRun)
				{
					countFrequency = 0f;
					limitRun = 0;
					DecideNextFlyerState(3);
				}
			}
			break;
		case enemyFlyingState.Chasing:
			if (limitRun <= 0 && FLY_CheckChaseConditions())
			{
				SetNewLimitRunningTime();
				rotationZ = enmMisc.flyerZRotation;
				invertSpriteFacing = false;
				PlayChaseStartSound();
			}
			if (!FLY_DecideActionDuringChase())
			{
				if (!FLY_CHASE_SPECIAL_ALLOW() && ((!GC.isPlayerFainted && GC.playerIsBeingGrabed) || (GC.isPlayerFainted && GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Adjacent))))
				{
					limitRun = 0;
				}
				if (countFrequency > (float)limitRun)
				{
					countFrequency = 0f;
					limitRun = 0;
					DecideNextFlyerState(4);
				}
			}
			break;
		case enemyFlyingState.Retreating:
			if (limitRun <= 0 && !GC.playerIsBeingGrabed && !GC.isPlayerFainted && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.FlyingChase))
			{
				SetNewLimitRunningTime();
				rotationZ = 0f;
				invertSpriteFacing = true;
			}
			if (countFrequency > (float)limitRun || GC.playerIsBeingGrabed || !GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.FlyingChase))
			{
				countFrequency = 0f;
				limitRun = 0;
				DecideNextFlyerState(5);
			}
			break;
		case enemyFlyingState.Landed:
		{
			if (limitIdle <= 0)
			{
				SetNewLimitIdleTime();
				invertSpriteFacing = false;
				if (floatLandAgainDelay <= 0f)
				{
					floatLandAgainDelay = enmMisc.LandAgainDelay;
				}
			}
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			if (enemyCurrentYMove != 0f)
			{
				enemyCurrentYMove = 0f;
			}
			if (rotationZ != 0f)
			{
				rotationZ = 0f;
			}
			ENM_Miscellaneuss.FlyerType typeFlyerBehaviour = enmMisc.typeFlyerBehaviour;
			if (typeFlyerBehaviour == ENM_Miscellaneuss.FlyerType.LanderTypeB)
			{
				if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 10f, 5f) && ((base.transform.localScale.x == 1f && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.PlayerToTheRightCustom, 5f)) || (base.transform.localScale.x == -1f && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.PlayerToTheLeftCustom, 5f))))
				{
					countFrequency = 0f;
					limitIdle = 0;
					DecideNextFlyerState(1);
				}
			}
			else if (countFrequency > (float)(limitIdle * 2))
			{
				countFrequency = 0f;
				limitIdle = 0;
				DecideNextFlyerState(1);
			}
			break;
		}
		case enemyFlyingState.SpecialAction:
			FLY_ExecuteSpecialAction();
			break;
		}
	}

	private void DecideNextFlyerState(int previousState)
	{
		int num = previousState;
		switch (enmMisc.typeFlyerBehaviour)
		{
		case ENM_Miscellaneuss.FlyerType.Agressive:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[4] { 1, 2, 3, 4 });
			}
			break;
		case ENM_Miscellaneuss.FlyerType.Defensive:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[4] { 1, 3, 3, 5 });
			}
			break;
		case ENM_Miscellaneuss.FlyerType.LanderTypeB:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[5] { 1, 2, 2, 3, 3 });
			}
			break;
		case ENM_Miscellaneuss.FlyerType.HorizontalOnly:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[2] { 1, 2 });
			}
			break;
		case ENM_Miscellaneuss.FlyerType.AgressiveTypeB:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[6] { 1, 2, 3, 3, 4, 5 });
			}
			break;
		case ENM_Miscellaneuss.FlyerType.AgressiveTypeC:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[6] { 1, 1, 2, 3, 3, 4 });
			}
			break;
		case ENM_Miscellaneuss.FlyerType.WormDig1:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[5] { 1, 3, 4, 4, 4 });
			}
			break;
		case ENM_Miscellaneuss.FlyerType.Patroller:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[5] { 1, 2, 2, 3, 4 });
			}
			break;
		case ENM_Miscellaneuss.FlyerType.AgressiveGallery:
			while (num == previousState)
			{
				num = GC.MATH_RNG_INT_ARRAY(new int[6] { 1, 2, 3, 7, 7, 7 });
			}
			break;
		}
		switch (num)
		{
		default:
			currentFlyingState = enemyFlyingState.Idle;
			break;
		case 2:
			currentFlyingState = enemyFlyingState.Patrolling;
			break;
		case 3:
			currentFlyingState = enemyFlyingState.Wandering;
			break;
		case 4:
			if (FLY_CheckChaseConditions() && !GC.isPlayerGoneForGood && !GC.playerIsBeingGrabed)
			{
				if ((!GC.isPlayerFainted && GC.playerIsBeingGrabed) || (GC.isPlayerFainted && GC.playerIsBeingGrabed && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Adjacent)))
				{
					DecideNextFlyerState(4);
				}
				currentFlyingState = enemyFlyingState.Chasing;
				CHASE_PlaySoundAtChaseStart();
			}
			else
			{
				DecideNextFlyerState(4);
			}
			break;
		case 5:
			currentFlyingState = enemyFlyingState.Retreating;
			break;
		case 7:
			if (FLY_CheckChaseConditions() && !GC.isPlayerGoneForGood && !isGrabbing)
			{
				currentFlyingState = enemyFlyingState.Chasing;
			}
			else
			{
				DecideNextFlyerState(7);
			}
			break;
		}
		if (isChasingFainted)
		{
			isChasingFainted = false;
		}
		FLY_ActionAfterStateChange();
	}

	private void FLY_ActionAfterStateChange()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.Batgirl)
		{
			boolGeneric = false;
		}
	}

	private void FLY_DecideActionDuringPatrolling()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Dragonfly:
			if (FLY_CheckActionBasedOnSecondsFrequency(1, 25) && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.FlyingChase))
			{
				countFrequency = 0f;
				limitRun = 0;
				currentFlyingState = enemyFlyingState.Chasing;
			}
			break;
		case GameCore.EnemyType.CrystalChunk:
			if (FLY_CheckActionBasedOnSecondsFrequency(1, 10) && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.FlyingChase))
			{
				countFrequency = 0f;
				limitRun = 0;
				currentFlyingState = enemyFlyingState.Chasing;
				objAN.SetInteger("genericPhase", 1);
			}
			break;
		case GameCore.EnemyType.Batgirl:
			if (!boolGeneric && !GC.playerIsBeingGrabed)
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BatWings, 50, checkForDuplicity: true, base.transform.position, 0f, 0f);
				boolGeneric = true;
			}
			break;
		case GameCore.EnemyType.FutaGhost:
			if (isUsingSkill)
			{
				countFrequency = limitRun + 1;
			}
			break;
		case GameCore.EnemyType.GhostButler:
			if ((float)GC.MATH_RNG_INT(1, 1000) <= Mathf.Round(100f / enmBaseStats.freqRepeatBehaviour))
			{
				enemyCurrentYMove += GC.MATH_RNG_FLOAT(-0.5f, 0.5f);
			}
			break;
		}
	}

	private bool FLY_DecideActionDuringIdleWander()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.HillFairy:
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 8f, 8f) && FLY_CheckActionBasedOnSecondsFrequency(2, 33) && !GC.playerIsBeingGrabed)
			{
				countFrequency = 0f;
				limitRun = 0;
				if (GC.playerCurrentDressState == 2)
				{
					currentFlyingState = enemyFlyingState.Chasing;
				}
				else
				{
					currentFlyingState = enemyFlyingState.Retreating;
				}
				return true;
			}
			break;
		case GameCore.EnemyType.SwampFairy:
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 6f, 6f) && FLY_CheckActionBasedOnSecondsFrequency(2, 20) && !GC.playerIsBeingGrabed)
			{
				countFrequency = 0f;
				limitRun = 0;
				if (GC.playerCurrentDressState == 2)
				{
					currentFlyingState = enemyFlyingState.Chasing;
				}
				else
				{
					currentFlyingState = enemyFlyingState.Retreating;
				}
				return true;
			}
			break;
		case GameCore.EnemyType.Batgirl:
			if (!boolGeneric && !GC.playerIsBeingGrabed)
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BatWings, 50, checkForDuplicity: true, base.transform.position, 0f, 0f);
				boolGeneric = true;
			}
			if (GC.playerCurrentDressState == 2 && enmCmnBehaviour.rangeFaintedChaseX < 40f)
			{
				enmCmnSkill.skillPercentChance *= 2;
				enmCmnBehaviour.rangeFaintedChaseX = 40f;
				enmCmnBehaviour.rangeFaintedChaseY = 40f;
			}
			else if (GC.playerCurrentDressState != 2 && enmCmnBehaviour.rangeFaintedChaseX >= 40f)
			{
				enmCmnSkill.skillPercentChance /= 2;
				enmCmnBehaviour.rangeFaintedChaseX = 20f;
				enmCmnBehaviour.rangeFaintedChaseY = 20f;
			}
			break;
		case GameCore.EnemyType.FutaGhost:
		case GameCore.EnemyType.GhostButler:
			if (FLY_IsCenterMassOnGround() && currentFlyingState == enemyFlyingState.Idle && !boolGeneric && !GC.isPlayerFainted && !isChasingFainted && CheckSelfProximityInsideRangeOfPoint(enmObjRefs.StartPointCenterMass.position, vecStartingPosition, 1f, 1f, invertToOutiseOfRange: true))
			{
				if (isUsingSkill)
				{
					EnemyConcludeSkill();
				}
				currentFlyingState = enemyFlyingState.SpecialAction;
			}
			break;
		case GameCore.EnemyType.CrystalChunk:
			if (objAN.GetInteger("genericPhase") != 0)
			{
				objAN.SetInteger("genericPhase", 0);
			}
			break;
		case GameCore.EnemyType.WormDig:
			if (countFlipChanged >= 3)
			{
				countFlipChanged = 0;
				RollRandomFlyerMov(rollRamdomX: false, rollRamdomY: true);
			}
			break;
		}
		return false;
	}

	private bool FLY_DecideActionDuringChase()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.SwampFairy:
			if (!GC.isPlayerFainted && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f))
			{
				limitRun = 0;
				currentFlyingState = enemyFlyingState.Wandering;
				return true;
			}
			break;
		case GameCore.EnemyType.CrystalChunk:
			if (objAN.GetInteger("genericPhase") == 0)
			{
				objAN.SetInteger("genericPhase", 1);
			}
			Spawn2DSpriteTrail();
			break;
		}
		return false;
	}

	private bool FLY_CheckActionBasedOnSecondsFrequency(int eachHowManySeconds, int percentChanceEachTime = 0)
	{
		if (Mathf.RoundToInt(countFrequency) != countGenericStuff)
		{
			countGenericStuff = Mathf.RoundToInt(countFrequency);
			if (GC.MATH_IsDivisibleBy(countGenericStuff, eachHowManySeconds))
			{
				if (percentChanceEachTime > 0)
				{
					if (GC.MATH_RNG_INT(1, 100) <= percentChanceEachTime)
					{
						return true;
					}
					return false;
				}
				return true;
			}
		}
		return false;
	}

	private void CheckFlyerGroundDistance()
	{
		Vector2 end = base.transform.position;
		enemyFlyingState enemyFlyingState2 = currentFlyingState;
		end.y -= enmCmnBehaviour.minGroundFlyRange;
		if ((bool)Physics2D.Linecast(base.transform.position, end, 1 << LayerMask.NameToLayer("LayerPlatforms")) && enemyCurrentYMove < 0f && (enemyFlyingState2 == enemyFlyingState.Patrolling || enemyFlyingState2 == enemyFlyingState.Wandering || enemyFlyingState2 == enemyFlyingState.Retreating))
		{
			RollRandomFlyerMov(rollRamdomX: false, rollRamdomY: true, 0.1f, 1f, maintainTheX: true);
			return;
		}
		end.y -= enmCmnBehaviour.maxGroundFlyRange - enmCmnBehaviour.minGroundFlyRange;
		if (!Physics2D.Linecast(base.transform.position, end, 1 << LayerMask.NameToLayer("LayerPlatforms")))
		{
			if (objRB.velocity.y >= 0.5f)
			{
				RollRandomFlyerMov(rollRamdomX: false, rollRamdomY: true, -0.25f, -0.1f, maintainTheX: true);
			}
			end.y *= 2.5f;
			if (!Physics2D.Linecast(base.transform.position, end, 1 << LayerMask.NameToLayer("LayerPlatforms")) && objRB.velocity.y >= 0.5f)
			{
				objRB.velocity /= 2f;
			}
		}
	}

	private void RollRandomFlyerMov(bool rollRamdomX, bool rollRamdomY, float customMin = 0f, float customMax = 0f, bool maintainTheX = false, bool maintainTheY = false)
	{
		if (rollRamdomX)
		{
			if (customMin == 0f && customMax == 0f)
			{
				if (GC.MATH_RNG_INT(0, 1) == 1)
				{
					enemyCurrentXMove = 1f;
				}
				else
				{
					enemyCurrentXMove = -1f;
				}
			}
			else
			{
				enemyCurrentXMove = GC.MATH_RNG_FLOAT(customMin, customMax);
			}
		}
		else if (!maintainTheX)
		{
			enemyCurrentXMove = 0f;
		}
		if (rollRamdomY)
		{
			if (customMin == 0f && customMax == 0f)
			{
				if (GC.MATH_RNG_INT(0, 1) == 1)
				{
					enemyCurrentYMove = GC.MATH_RNG_FLOAT(0.25f, 1.5f);
				}
				else
				{
					enemyCurrentYMove = GC.MATH_RNG_FLOAT(-1.5f, -0.25f);
				}
			}
			else
			{
				enemyCurrentYMove = GC.MATH_RNG_FLOAT(customMin, customMax);
			}
		}
		else if (!maintainTheY)
		{
			enemyCurrentYMove = 0f;
		}
	}

	private void BasicFlying_CheckLanded(bool enforceLanding = false)
	{
		if ((bool)enmObjRefs.CeilingCheck1 && (bool)enmObjRefs.CeilingCheck2 && (bool)enmObjRefs.CeilingCheck3 && (FLY_CheckToLandConditions() | enforceLanding) && CheckCollisionCeiling(allCheckMustTrue: true, ignorePlatMov: true) && floatLandAgainDelay <= 0f)
		{
			objAN.SetInteger("genericPhase", 1);
			currentFlyingState = enemyFlyingState.Landed;
		}
		if ((bool)enmObjRefs.GroundCheck1 && (bool)enmObjRefs.GroundCheck2 && (bool)enmObjRefs.GroundCheck3 && (FLY_CheckToLandConditions() | enforceLanding) && CheckCollisionGround(allCheckMustTrue: true, ignorePlatPass: true, ignorePlayerColl: true) && floatLandAgainDelay <= 0f)
		{
			currentFlyingState = enemyFlyingState.Landed;
		}
	}

	private bool FLY_CheckToLandConditions()
	{
		if ((currentFlyingState != enemyFlyingState.Landed && currentFlyingState != enemyFlyingState.Chasing && (float)GC.MATH_RNG_INT(1, 100) <= 30f * enmBaseStats.freqRepeatBehaviour) || currentFlyingState == enemyFlyingState.Retreating)
		{
			return true;
		}
		return false;
	}

	private bool FLY_CheckChaseConditions()
	{
		if ((!GC.isPlayerFainted && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.FlyingChase)) || (GC.isPlayerFainted && isChasingFainted))
		{
			return true;
		}
		if (GC.playerIsBeingGrabed && FLY_CHASE_SPECIAL_ALLOW())
		{
			return true;
		}
		return false;
	}

	private bool FLY_CHASE_SPECIAL_ALLOW()
	{
		if (isGalleryView || isAddComboGalleryView || eroCore.canChasePlayerAliveGrabbed || enmMisc.typeFlyerBehaviour == ENM_Miscellaneuss.FlyerType.AgressiveGallery)
		{
			return true;
		}
		return false;
	}

	private void FLY_Genericbehaviors()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Batgirl:
		{
			int typeColl = 0;
			if (currentFlyingState != enemyFlyingState.Landed && objAN.GetInteger("genericPhase") == 1)
			{
				objAN.SetInteger("genericPhase", 0);
			}
			CheckCollisionCenterOfMass(out typeColl);
			switch (typeColl)
			{
			case 1:
				base.transform.position += new Vector3(0f, 0.5f, 0f);
				break;
			case 2:
				base.transform.position += new Vector3(0f, -0.5f, 0f);
				break;
			}
			break;
		}
		case GameCore.EnemyType.MothGirl:
			FLY_ManageSpecialChasingAnimation("MTG-Chasing");
			break;
		case GameCore.EnemyType.BeeGirl:
			FLY_ManageSpecialChasingAnimation("BEG-Chase");
			break;
		case GameCore.EnemyType.BigInsect:
			FLY_ManageSpecialChasingAnimation("BIS-Chase");
			break;
		case GameCore.EnemyType.Sinccubus:
			FLY_ManageSpecialChasingAnimation("SCB-Chase");
			break;
		case GameCore.EnemyType.CrystalChunk:
			if (currentEnemyState == enemyState.Chasing)
			{
				StartStopSpawnSpriteTrail(stateON: true, 0.2f);
			}
			else
			{
				StartStopSpawnSpriteTrail(stateON: false, 0f);
			}
			break;
		case GameCore.EnemyType.WormDig:
			if (currentFlyingState != enemyFlyingState.Chasing)
			{
				floatGeneric = GC.MATH_RNG_FLOAT(0.25f, 2f);
			}
			if (!GetMiscGameObject(0).activeInHierarchy && (currentFlyingState == enemyFlyingState.Chasing || currentFlyingState == enemyFlyingState.Wandering))
			{
				StartStopSpawnSpriteTrail(stateON: true, 0.2f, GameCore.VisualEffect.SmokeDigWorm);
			}
			else
			{
				StartStopSpawnSpriteTrail(stateON: false, 0.2f, GameCore.VisualEffect.SmokeDigWorm);
			}
			WORMDIG_SpecialRiseBehavior();
			break;
		}
		if (currentFlyingState != enemyFlyingState.Landed && floatLandAgainDelay >= 0f)
		{
			floatLandAgainDelay -= enmBaseStats.freqRepeatBehaviour;
		}
	}

	private void FLY_ManageSpecialChasingAnimation(string animChase)
	{
		if (currentFlyingState == enemyFlyingState.Chasing && !objAN.GetCurrentAnimatorStateInfo(0).IsName(animChase) && !isUsingSkill)
		{
			objAN.Play(animChase);
		}
		else if (currentFlyingState != enemyFlyingState.Chasing && objAN.GetCurrentAnimatorStateInfo(0).IsName(animChase))
		{
			objAN.Play("Basic-Idle");
		}
	}

	private void FLY_ExecuteSpecialAction()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Batgirl:
			if (!isUsingSkill && crtGeneric == null)
			{
				FLY_DiveSkillGeneric(new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, 0f), 2f, 1.5f, 1);
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BatSwoop, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			}
			break;
		case GameCore.EnemyType.FutaGhost:
		case GameCore.EnemyType.GhostButler:
			if (!isUsingSkill)
			{
				FLY_MoveToPoint(enmObjRefs.StartPointCenterMass.position, vecStartingPosition);
				break;
			}
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = 0f;
			break;
		}
	}

	private void FLY_DiveSkillGeneric(Vector3 tmpTarget, float timeWaitSkill, float speedModifier, int objMiscIndex)
	{
		tmpTarget.y--;
		isUsingSkill = true;
		objAN.SetBool("isSkill", value: true);
		if (coroutExecuteSkill != null)
		{
			StopCoroutine(coroutExecuteSkill);
		}
		coroutExecuteSkill = StartCoroutine(EnemyWaitForSkillExecution(timeWaitSkill));
		GENERIC_InstantiateWayPoint(objMiscIndex, tmpTarget);
		FLY_MoveToPoint(enmObjRefs.StartPointCenterMass.position, tmpTarget, enmBaseStats.speedMoveX * speedModifier, applyRotationAtan2: true);
	}

	private void FLY_MoveToPoint(Vector3 startPoint, Vector3 targetPoint, float speedMultiplier = 1f, bool applyRotationAtan2 = false)
	{
		Vector3 vector = targetPoint - startPoint;
		float magnitude = vector.magnitude;
		Vector3 vector2 = vector / magnitude * speedMultiplier;
		enemyCurrentXMove = vector2.x;
		enemyCurrentYMove = vector2.y;
		if (applyRotationAtan2)
		{
			rotationZ = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
		}
	}

	private bool FLY_IsCenterMassOnGround()
	{
		if (enmObjRefs.StartPointCenterMass == null)
		{
			Debug.Log("EnemyGeneralLogic:FLY_IsCenterMassOnGround -> Could not verify logic, because the OBJ CenterOfMass is NULL!");
			return false;
		}
		if ((bool)Physics2D.Linecast(enmObjRefs.StartPointCenterMass.position, enmObjRefs.StartPointCenterMass.position, 1 << LayerMask.NameToLayer("LayerPlatforms")))
		{
			return true;
		}
		return false;
	}

	private void WORMDIG_SpecialRiseBehavior()
	{
		if (currentEnemyType == GameCore.EnemyType.WormDig && crtGeneric == null && !GetMiscGameObject(0).activeInHierarchy && !GC.isPlayerGoneForGood && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, floatGeneric, 4f) && WORMDIG_ValidOpenSpaceAbove() && (!GC.isPlayerFainted || GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 0.5f, 2f)))
		{
			Vector2 zero = Vector2.zero;
			CHASE_END();
			zero = new Vector2(GC.MATH_RoundFloatToHalf(base.transform.position.x), GC.MATH_RoundFloatToHalf(base.transform.position.y));
			StartStopSpawnSpriteTrail(stateON: false, 0.2f, GameCore.VisualEffect.SmokeDigWorm);
			WORMDIG_ToggleSelfState(stateON: false, zero.x, zero.y);
			zero.y += 0.5f;
			GC.InstantiateVFX(GameCore.VisualEffect.RocksDigging, zero, "EnemyGeneralLogic:WORMDIG_RiseBehavior");
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WormDigUp, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
		}
	}

	public void WORMDIG_ToggleSelfState(bool stateON, float xStopPos = 0f, float yStopPos = 0f)
	{
		if (stateON)
		{
			InvokeRepeating("BasicFlyer_MainBehaviour", 0f, enmBaseStats.freqRepeatBehaviour);
			crtGeneric = StartCoroutine(waitGENERIC(5f, "wormDelayNextRise"));
			isStatic = false;
			colorGeneric = objRE.color;
			colorGeneric.a = 1f;
			objRE.color = colorGeneric;
			GetMiscGameObject(0).SetActive(value: false);
			objRB.bodyType = RigidbodyType2D.Dynamic;
			objRB.velocity = Vector2.zero;
		}
		else
		{
			CancelInvoke("BasicFlyer_MainBehaviour");
			isStatic = true;
			currentFlyingState = enemyFlyingState.Idle;
			countFrequency = 0f;
			limitIdle = 0;
			SetNewLimitIdleTime(doMax: true);
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = 0f;
			objRB.velocity = Vector2.zero;
			objRB.bodyType = RigidbodyType2D.Static;
			colorGeneric = objRE.color;
			colorGeneric.a = 0f;
			objRE.color = colorGeneric;
			base.transform.position = new Vector2(xStopPos, yStopPos);
			crtGeneric = StartCoroutine(waitGENERIC(1f, "wormRise1"));
		}
	}

	private bool WORMDIG_ValidOpenSpaceAbove()
	{
		Vector2 vector = new Vector2(base.transform.position.x, base.transform.position.y + 1.25f);
		Vector2 end = vector;
		vector.x -= 0.3f;
		vector.y += 0.3f;
		end.x += 0.3f;
		end.y -= 0.3f;
		RaycastHit2D[] array = Physics2D.LinecastAll(vector, end);
		if (array == null || array.Length == 0)
		{
			return true;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].transform.tag == "platform-floor" || array[i].transform.tag == "moveable-collider")
			{
				return false;
			}
		}
		return true;
	}

	public void WORMCOLL(int phase)
	{
		switch (phase)
		{
		default:
			AlterSelfBoxCollider(BoxCollAction.Reset, Vector2.zero, Vector2.zero);
			break;
		case 1:
			AlterSelfBoxCollider(BoxCollAction.ChangeBoth, new Vector2(0.5f, 0.1f), new Vector2(0f, -10f));
			break;
		case 2:
			AlterSelfBoxCollider(BoxCollAction.ChangeBoth, new Vector2(0.5f, 0.75f), new Vector2(0f, 0.7f));
			break;
		case 3:
			AlterSelfBoxCollider(BoxCollAction.ChangeBoth, new Vector2(0.6f, 1.5f), new Vector2(0f, 1f));
			break;
		case 4:
			AlterSelfBoxCollider(BoxCollAction.ChangeBoth, new Vector2(0.6f, 2f), new Vector2(0f, 1.2f));
			break;
		}
	}

	public void PerFrame(string attackID_and_Frame)
	{
		int index = int.Parse(attackID_and_Frame.Substring(0, 2));
		int frame = int.Parse(attackID_and_Frame.Substring(3, 2));
		if (listOfExternalAttacks == null || listOfExternalAttacks.Count <= 0)
		{
			Debug.Log("[ENEMY](PerFrame): Method was called, but the list of attacks was empty! Should add the attack objects to the list during enemy initialization!");
		}
		else
		{
			listOfExternalAttacks[index].GetComponent<EnemyAttack>().CollPerFrame(frame);
		}
	}

	private void GenericWhatToDoWhenHittingObstacle(bool checkRepeatedJumpInPlace = false)
	{
		if (enmCmnBehaviour.canJumpToMove || currentEnemyState == enemyState.Chasing)
		{
			GameCore.EnemyType enemyType = currentEnemyType;
			if (enemyType == GameCore.EnemyType.Isobora)
			{
				if (!isUsingSkill && !GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Adjacent) && !GC.playerIsBeingGrabed)
				{
					EnemyExecuteSkill();
					return;
				}
			}
			else
			{
				if (currentEnemyState == enemyState.Patrolling && GC.MATH_RNG_INT(1, 100) <= 35)
				{
					GenericExecuteJump(1f, playSound: true);
					return;
				}
				if (currentEnemyState == enemyState.Chasing && !isJumpDelay && (CheckCollisionGround() || CheckCollisionMovingPlatforms()))
				{
					if (checkRepeatedJumpInPlace)
					{
						if (GC.MATH_ROUND_VECTOR3(base.transform.position, 1) != posGenericVector)
						{
							GenericExecuteJump(1f, playSound: false, applyJumpDelay: true, 0.5f);
							posGenericVector = GC.MATH_ROUND_VECTOR3(base.transform.position, 1);
						}
						else
						{
							CHASE_END();
						}
						return;
					}
					GenericExecuteJump(1f, playSound: false, applyJumpDelay: true, 0.5f);
				}
			}
		}
		if ((!isUsingSkill || !enmCmnSkill.ignoreCollisionOnExecution) && !isChasingFainted && currentEnemyState != enemyState.Chasing)
		{
			enemyCurrentXMove *= -1f;
			countFlipChanged++;
		}
	}

	private void GenericExecuteJump(float jumpForceMultiplier = 1f, bool playSound = false, bool applyJumpDelay = false, float delayTime = 0f)
	{
		if (enmBaseStats.maxJumpForce <= 0f || ForbidJumpExceptions() || (isGrabbing && !DuringEroGrabJumpAllowance()))
		{
			return;
		}
		enemyCurrentYMove = enmBaseStats.maxJumpForce * jumpForceMultiplier;
		if (playSound)
		{
			PlaySoundJump();
		}
		if (applyJumpDelay && delayTime > 0f)
		{
			isJumpDelay = true;
			if (base.gameObject.activeInHierarchy)
			{
				crtJumpDelay = StartCoroutine(waitForJumpDelay(delayTime));
			}
		}
	}

	private bool ForbidJumpExceptions()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.Dogboy && isUsingSkill && objAN.GetInteger("genericPhase") != 1)
		{
			return true;
		}
		return false;
	}

	private bool DuringEroGrabJumpAllowance()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.Dogboy && objAN.GetInteger("genericPhase") == 101)
		{
			return true;
		}
		return false;
	}

	private void ChangeAnimGenericPhaseID(int setID, bool isInstant, float timerWaitToChange, bool exitAfterStop = false)
	{
		if (crtGeneric != null)
		{
			StopCoroutine(crtGeneric);
			crtGeneric = null;
			if (exitAfterStop)
			{
				return;
			}
		}
		if (isInstant)
		{
			objAN.SetInteger("genericPhase", setID);
		}
		else
		{
			crtGeneric = StartCoroutine(waitChangeGenericPhase(timerWaitToChange, setID));
		}
	}

	private void UPDATE_DisableExternalAttacksSafetyMeasure()
	{
		if (!isDeath && !isGrabbing)
		{
			return;
		}
		if (isUsingSkill)
		{
			EnemyConcludeSkill();
		}
		if (listOfExternalAttacks == null || listOfExternalAttacks.Count <= 0)
		{
			return;
		}
		foreach (GameObject listOfExternalAttack in listOfExternalAttacks)
		{
			listOfExternalAttack.SetActive(value: false);
		}
	}

	private bool CheckSelfProximityInsideRangeOfPoint(Vector3 selfPointOfReference, Vector3 targetPointToCheck, float checkDistanceX = 0f, float checkDistanceY = 0f, bool invertToOutiseOfRange = false)
	{
		bool flag = false;
		bool flag2 = false;
		float num = selfPointOfReference.x - targetPointToCheck.x;
		float num2 = selfPointOfReference.y - targetPointToCheck.y;
		if (num < 0f)
		{
			num *= -1f;
		}
		if (num2 < 0f)
		{
			num2 *= -1f;
		}
		if (checkDistanceX > 0f && (checkDistanceX < num || (invertToOutiseOfRange && checkDistanceX > num)))
		{
			flag = true;
		}
		if (checkDistanceY > 0f && (checkDistanceY < num2 || (invertToOutiseOfRange && checkDistanceY > num2)))
		{
			flag2 = true;
		}
		if (((checkDistanceX > 0f && checkDistanceY > 0f) & flag & flag2) || ((checkDistanceX > 0f) & flag) || ((checkDistanceY > 0f) & flag2))
		{
			return true;
		}
		return false;
	}

	private void GENERIC_InstantiateWayPoint(int miscObjectID, Vector3 spawnPosition)
	{
		if (GetMiscGameObject(miscObjectID) != null)
		{
			UnityEngine.Object.Instantiate(GetMiscGameObject(miscObjectID), spawnPosition, Quaternion.identity, GC.refOBJEffectsParent.transform).SetActive(value: true);
		}
		else
		{
			Debug.Log(base.gameObject.name + " tried to instantiate a waypoint object, but it returned null on GETMISCOBJECT at ID: " + miscObjectID);
		}
	}

	public void GENERIC_TouchedWayPoint()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Batgirl:
		case GameCore.EnemyType.BigInsect:
			if (isUsingSkill)
			{
				GENERIC_StartTheWait(0.5f, "delayedReset");
				isUsingSkill = false;
				objAN.SetBool("isSkill", value: false);
			}
			break;
		case GameCore.EnemyType.FutaGhost:
		case GameCore.EnemyType.GhostButler:
			if (currentFlyingState == enemyFlyingState.SpecialAction)
			{
				enemyCurrentXMove = 0f;
				enemyCurrentYMove = 0f;
				limitIdle = 0;
				currentFlyingState = enemyFlyingState.Idle;
			}
			break;
		case GameCore.EnemyType.Catgirl:
			GENERIC_StartTheWait(4f, "cat_birth");
			break;
		}
	}

	private void GENERIC_StartTheWait(float timer, string genAction)
	{
		if (crtGeneric != null)
		{
			StopCoroutine(crtGeneric);
		}
		crtGeneric = StartCoroutine(waitGENERIC(timer, genAction));
	}

	private IEnumerator waitChangeGenericPhase(float wait, int genericPhaseID)
	{
		yield return new WaitForSeconds(wait);
		objAN.SetInteger("genericPhase", genericPhaseID);
		crtGeneric = null;
	}

	private IEnumerator waitGENERIC(float time, string genericAction, int genericInteger = 0)
	{
		yield return new WaitForSeconds(time);
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.LazyStarfish:
			if (genericAction == "layerOrder")
			{
				objRE.sortingOrder = genericInteger;
			}
			break;
		case GameCore.EnemyType.Batgirl:
		case GameCore.EnemyType.BigInsect:
			if (genericAction == "delayedReset")
			{
				DecideNextFlyerState(4);
			}
			break;
		case GameCore.EnemyType.FutaGhost:
		case GameCore.EnemyType.GhostButler:
		{
			boolGeneric = false;
			Color color = objRE.color;
			color.a = 1f;
			objRE.color = color;
			objBox2D.enabled = true;
			limitIdle = 0;
			currentFlyingState = enemyFlyingState.Idle;
			break;
		}
		case GameCore.EnemyType.CrystalChunk:
			objRB.isKinematic = true;
			base.gameObject.SetActive(value: false);
			break;
		case GameCore.EnemyType.Manequin:
			UnityEngine.Object.Instantiate(GetMiscGameObject(0), base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
			UnityEngine.Object.Instantiate(GetMiscGameObject(1), base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
			objRB.isKinematic = true;
			base.gameObject.SetActive(value: false);
			break;
		case GameCore.EnemyType.PurpleSlime:
			UnityEngine.Object.Instantiate(GetMiscGameObject(0), base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
			objRB.isKinematic = true;
			base.gameObject.SetActive(value: false);
			break;
		case GameCore.EnemyType.PaintingEvil:
			base.gameObject.SetActive(value: false);
			break;
		case GameCore.EnemyType.FutaBunny:
			BasicProjectileInstantiate();
			break;
		case GameCore.EnemyType.WormDig:
			if (genericAction == "wormRise1")
			{
				GetMiscGameObject(0).SetActive(value: true);
				GetComponent<EnemyGeneralLogic>().enabled = false;
			}
			break;
		case GameCore.EnemyType.WormOverGround:
			boolGeneric = true;
			break;
		case GameCore.EnemyType.SlimeGirl:
			if (genericAction == "SlimeGirlMayaFat")
			{
				objRB.isKinematic = true;
				base.gameObject.SetActive(value: false);
				if ((bool)GC)
				{
					GC.SpecialPlayerGone();
				}
				UnityEngine.Object.Instantiate(GetMiscGameObject(0), base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
			}
			break;
		case GameCore.EnemyType.Catgirl:
			if (genericAction == "cat_birth")
			{
				GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "waitGenericCatGirl");
				UnityEngine.Object.Instantiate(GetMiscGameObject(0), base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
				GC.PlaySFXEro(GameCore.EroSound.Cum5, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
				GC.playerCancelStopEro = true;
			}
			if (genericAction == "cat_reveal")
			{
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 3f, 3f);
				PlayMonsterSoundsFromAnim("nyaOther");
			}
			break;
		case GameCore.EnemyType.DarkDamsel:
			UnityEngine.Object.Instantiate(GetMiscGameObject(0), base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
			objRB.isKinematic = true;
			base.gameObject.SetActive(value: false);
			break;
		case GameCore.EnemyType.SwampFairy:
		{
			for (int i = 0; i < 2; i++)
			{
				posGenericVector = base.transform.position;
				posGenericVector.x += GC.MATH_RNG_FLOAT(-0.25f, 0.25f);
				posGenericVector.y += GC.MATH_RNG_FLOAT(-0.25f, 0.25f);
				GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, posGenericVector, "spawnSwampFairy");
				UnityEngine.Object.Instantiate(GetMiscGameObject(0), posGenericVector, Quaternion.identity, GC.refOBJEnemiesParent.transform);
			}
			break;
		}
		}
		crtGeneric = null;
	}

	private void EnemyCheckSkillExecution()
	{
		float num = 0f;
		countTimeToUseSkill += enmBaseStats.freqRepeatBehaviour;
		num = Mathf.Round(countTimeToUseSkill);
		if (((num != prevSkillTime && GC.MATH_IsDivisibleBy(num, 1)) || enmCmnSkill.ignoreRoundTime) && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.SkillUsage, enmCmnSkill.minSkillProximityX, enmCmnSkill.minSkillProximityY) && (!GC.isPlayerFainted || SkillUseFaintedException()) && ((num >= (float)enmCmnSkill.minSkillFrequency && GC.MATH_RNG_INT(1, 100) <= enmCmnSkill.skillPercentChance) || (enmCmnSkill.maxSkillFrequency > 0 && num >= (float)enmCmnSkill.maxSkillFrequency)))
		{
			countTimeToUseSkill = 0f;
			num = 0f;
			EnemyExecuteSkill();
		}
		prevSkillTime = num;
	}

	private void EnemyExecuteSkill()
	{
		bool flag = false;
		if (isSkillDelay)
		{
			return;
		}
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Isobora:
			currentDefense = enmBaseStats.specialDefense;
			originalTouchDamage = enmBaseStats.baseTouchDamage;
			enmBaseStats.baseTouchDamage = enmCmnSkill.baseSkillDamage;
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			GetComponent<BoxCollider2D>().offset = new Vector2(0f, 0f);
			GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 0.8f);
			flag = true;
			break;
		case GameCore.EnemyType.Catgirl:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			objAN.Play("CAT-PreAttack");
			flag = true;
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CatHiss, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.BlazePlant:
		case GameCore.EnemyType.NaughtyCoral:
		case GameCore.EnemyType.FlowerBlossom:
		case GameCore.EnemyType.TentacleTree:
		case GameCore.EnemyType.CrystalChunk:
		case GameCore.EnemyType.PurpleSlime:
		case GameCore.EnemyType.FleshBlossom:
		case GameCore.EnemyType.Medusa:
			SKILL_BasicSkillUsage();
			flag = true;
			break;
		case GameCore.EnemyType.Goblin:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.GoblinLaugh, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			SKILL_BasicSkillUsage();
			flag = true;
			break;
		case GameCore.EnemyType.BeeGirl:
			SKILL_BasicSkillUsage();
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BeeGirlCast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			flag = true;
			break;
		case GameCore.EnemyType.BeachKitsune:
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			enemyCurrentXMove = 0f;
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			flag = true;
			if (crtGeneric == null)
			{
				crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			}
			STATE_MACHINE_FLIP_FACE_PLAYER();
			GC.PlaySFX(GameCore.SFXBasic1.KitsuneCast1, 0, checkForDuplicity: false, base.transform.position, 0.8f, 1.2f);
			break;
		case GameCore.EnemyType.HillFairy:
		case GameCore.EnemyType.SwampFairy:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			rotationZ = 0f;
			flag = true;
			if (crtGeneric == null)
			{
				crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			}
			if (!invertSpriteFacing)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			if (currentEnemyType == GameCore.EnemyType.HillFairy)
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FairySkill, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.1f);
			}
			else if (currentEnemyType == GameCore.EnemyType.SwampFairy)
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.SwampFairy_Cast, 0, checkForDuplicity: false, base.transform.position, 0.9f, 1.1f);
			}
			break;
		case GameCore.EnemyType.SalamanderGirl:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			enemyCurrentXMove = 0f;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			break;
		case GameCore.EnemyType.Dogboy:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			enemyCurrentXMove = 0f;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CityDogPanting, 0, checkForDuplicity: true, base.transform.position, 1f, 1.1f);
			break;
		case GameCore.EnemyType.LizardShield:
			isUsingSkill = true;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			ChangeAnimGenericPhaseID(1, isInstant: true, 0f);
			GC.PlaySFX(GameCore.SFXBasic1.Defend1, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.LizardAxe:
			isUsingSkill = true;
			ChangeAnimGenericPhaseID(1, isInstant: true, 0f);
			ChangeAnimGenericPhaseID(2, isInstant: false, 1f);
			break;
		case GameCore.EnemyType.Arachne:
			if (objAN.GetInteger("genericPhase") != 0)
			{
				return;
			}
			enemyCurrentXMove = 0f;
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn * 2f));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn * 3f));
			STATE_MACHINE_FLIP_FACE_PLAYER(lookAwayFromPlayer: true);
			flag = true;
			break;
		case GameCore.EnemyType.Lamia:
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			enemyCurrentXMove = 0f;
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			objAN.SetInteger("genericPhase", 1);
			BasicProjectileInstantiate(null, overrideSpawnManual: true, GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition - 0.5f);
			flag = true;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (GC.MATH_RNG_INT(0, 1) == 1)
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.LamiaCast1, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.1f);
			}
			else
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.LamiaCast2, 0, checkForDuplicity: false, base.transform.position, 0.95f, 1.1f);
			}
			break;
		case GameCore.EnemyType.MushroomGirl:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			enemyCurrentXMove = 0f;
			flag = true;
			MUSHROOM_Spawn_Spores();
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillAnimDuration / 2f));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MushroomGirlSkillA, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.05f);
			break;
		case GameCore.EnemyType.Batgirl:
		{
			GameObject obj = UnityEngine.Object.Instantiate(GetMiscGameObject(0), enmObjRefs.StartPointCenterMass.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BatCall, 0, checkForDuplicity: false, base.transform.position, 0f, 0f);
			obj.SetActive(value: true);
			break;
		}
		case GameCore.EnemyType.FutaGhost:
			if (GC.enemyGhostFutaSkillCasting != null || boolGeneric || currentFlyingState == enemyFlyingState.SpecialAction)
			{
				return;
			}
			GC.enemyGhostFutaSkillCasting = base.gameObject;
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			GC.InstantiateVFX(GameCore.VisualEffect.GhostCast, enmObjRefs.StartPointCenterMass.position, "EnemyGeneralLogic:FutaGhostSkillCast");
			PlayMonsterSoundsFromAnim("futaGhost-ring2");
			enemyCurrentXMove = 0f;
			limitIdle = 0;
			currentFlyingState = enemyFlyingState.Idle;
			flag = true;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			BasicProjectileInstantiate(null, overrideSpawnManual: true, GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, GC.playerTransformReference);
			break;
		case GameCore.EnemyType.GhostButler:
		{
			if (boolGeneric || currentFlyingState == enemyFlyingState.SpecialAction)
			{
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.GSB_CastRandom, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			enemyCurrentXMove = 0f;
			limitIdle = 0;
			currentFlyingState = enemyFlyingState.Idle;
			flag = true;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			GameObject miscGameObject = GetMiscGameObject(GC.MATH_RNG_INT(1, 3));
			BasicProjectileInstantiate(miscGameObject);
			break;
		}
		case GameCore.EnemyType.MothGirl:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MothCasting, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			if (currentFlyingState == enemyFlyingState.Chasing)
			{
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			flag = true;
			break;
		case GameCore.EnemyType.DwarfGirl:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			flag = true;
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.DwarfGrunt, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.SwampGolem:
			if (isUsingSkill || GC.isPlayerFainted || GC.playerIsBeingGrabed || !GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 4f, 3f))
			{
				return;
			}
			countGenericStuff = GC.MATH_RNG_INT(1, 2);
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			if (countGenericStuff == 1)
			{
				objAN.Play("SWG-Skill1_Pre");
				enmCmnSkill.SkillTimeToSpawn = 0.85f;
			}
			else
			{
				objAN.Play("SWG-Skill2_Pre");
				enemyCurrentYMove = 3f;
				enmCmnSkill.SkillTimeToSpawn = 0.65f;
			}
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			break;
		case GameCore.EnemyType.CentaurGirl:
		case GameCore.EnemyType.Goblina:
		case GameCore.EnemyType.HillTroll:
		{
			float num = enmCmnSkill.SkillTimeToSpawn;
			if (isUsingSkill || GC.isPlayerFainted || GC.playerIsBeingGrabed)
			{
				EnemyConcludeSkill();
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			if (currentEnemyType == GameCore.EnemyType.CentaurGirl)
			{
				if (countGenericStuff == 1)
				{
					STATE_MACHINE_FLIP_FACE_PLAYER(lookAwayFromPlayer: true);
					objAN.Play("CTG-PreKick");
					num *= 0.7f;
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CTG_Disdain, 0, checkForDuplicity: true, base.transform.position, 0.85f, 1.15f);
				}
				else
				{
					STATE_MACHINE_FLIP_FACE_PLAYER();
					objAN.Play("CTG-PreCharge");
					flag = true;
					GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CTG_PreCHarge, 0, checkForDuplicity: true, base.transform.position, 0.85f, 1.15f);
				}
			}
			else if (currentEnemyType == GameCore.EnemyType.Goblina)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
				objAN.Play("GOBA-PreAttack");
				flag = true;
			}
			else if (currentEnemyType == GameCore.EnemyType.HillTroll)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
				objAN.Play("Pre-ATK");
				flag = true;
			}
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(num));
			break;
		}
		case GameCore.EnemyType.PrisonBrute:
		{
			if (isUsingSkill || GC.isPlayerFainted || GC.playerIsBeingGrabed)
			{
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			objAN.Play("BRT-PrePunch");
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			GameCore.MonsterSounds whichSFX = GameCore.MonsterSounds.Brute1;
			if (GC.MATH_RNG_INT(0, 1) == 1)
			{
				whichSFX = GameCore.MonsterSounds.Brute2;
			}
			GC.PlaySFXMonsterSounds(whichSFX, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		}
		case GameCore.EnemyType.Dominatrix:
			if (isUsingSkill || GC.isPlayerFainted || isGrabbing || GC.playerIsBeingGrabed || objAN.GetInteger("genericPhase") == 2)
			{
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			objAN.Play("DOM-PreSkill");
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.Domme_Skill, 0, checkForDuplicity: true, base.transform.position, 1f, 1.1f);
			break;
		case GameCore.EnemyType.PaintingEvil:
			if (isUsingSkill)
			{
				return;
			}
			if (GC.isPlayerFainted || GC.playerIsBeingGrabed || GC.isPlayerGoneForGood || GC.playerCurrentWorldYPosition > base.transform.position.y)
			{
				EnemyConcludeSkill();
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.PaintingEvil_Appear, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.LatexMaiden:
			if (isUsingSkill)
			{
				return;
			}
			if (GC.isPlayerFainted || GC.playerIsBeingGrabed || GC.isPlayerGoneForGood)
			{
				EnemyConcludeSkill();
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.LatexMaiden_Open, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f, "LatexMaiden");
			break;
		case GameCore.EnemyType.SailorBurly:
			if (isUsingSkill || GC.isPlayerFainted || GC.playerIsBeingGrabed)
			{
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			objAN.Play("SLR-PreAttack");
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.PirateCast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			flag = true;
			break;
		case GameCore.EnemyType.WormOverGround:
		{
			if (isUsingSkill || GC.isPlayerFainted || GC.playerIsBeingGrabed || !base.gameObject.activeInHierarchy)
			{
				return;
			}
			float wait = 1f;
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WormCast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			int integer = objAN.GetInteger("genericPhase");
			if (integer == 10 || integer != 20)
			{
				objAN.Play("WRM-SkillPreTackle");
			}
			else
			{
				wait = 1.5f;
				objAN.Play("WRM-SkillSpit");
			}
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(wait));
			break;
		}
		case GameCore.EnemyType.Bot1:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn + 1f));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.RobotAlarm1, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			flag = true;
			break;
		case GameCore.EnemyType.Bot2:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn + 0.5f));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn + 1f));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.RobotAlarm2, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			flag = true;
			break;
		case GameCore.EnemyType.Bot3:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn + 0.33f));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn + 0.66f));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn + 0.99f));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.RobotAlarm3, 0, checkForDuplicity: true, base.transform.position, 1.1f, 1.1f);
			flag = true;
			break;
		case GameCore.EnemyType.WyvernGirl:
		case GameCore.EnemyType.Tentapod:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			objAN.Play("Cast-TentacleFleshHook");
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			if (currentEnemyType == GameCore.EnemyType.WyvernGirl)
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WyvernCast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			flag = true;
			break;
		case GameCore.EnemyType.FutaDruidCentauress:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			objAN.Play("FDC-Skill");
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			if (HEAL_CheckAbleToHeal(200, 50, new Vector2(20f, 16f), 75, GetMiscGameObject(0)))
			{
				objAN.SetInteger("genericPhase", 2);
				GC.PlaySFX(GameCore.SFXBasic2.Heal1, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FutaCentaurHeal, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			else
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FutaCentaurAttack, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			flag = true;
			break;
		case GameCore.EnemyType.BigInsect:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			objAN.Play("BIS-SkillPre");
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentFlyingState = enemyFlyingState.Idle;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BIS_Cast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.Failed:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FLR_Cast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			flag = true;
			break;
		case GameCore.EnemyType.CorruptBeast:
		case GameCore.EnemyType.VagrantMonk:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			if (currentEnemyType == GameCore.EnemyType.CorruptBeast)
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CRB_Cast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			flag = true;
			break;
		case GameCore.EnemyType.Sinccubus:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn + 1f));
			StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn + 2f));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.SCB_Cast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			flag = true;
			break;
		case GameCore.EnemyType.DarkDamsel:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			objAN.SetInteger("genericPhase", 0);
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.DKD_Cast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			flag = true;
			break;
		case GameCore.EnemyType.FairySorrow:
			if (objGeneric != null)
			{
				return;
			}
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentFlyingState = enemyFlyingState.Idle;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			flag = true;
			break;
		case GameCore.EnemyType.SlimeGirl:
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
			GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: true, base.transform.position, 1.2f, 1.3f);
			flag = true;
			break;
		}
		if (flag)
		{
			if (coroutExecuteSkill != null)
			{
				StopCoroutine(coroutExecuteSkill);
			}
			coroutExecuteSkill = StartCoroutine(EnemyWaitForSkillExecution(enmCmnSkill.SkillAnimDuration));
		}
	}

	private void SKILL_BasicSkillUsage()
	{
		if (isStagger)
		{
			EnemyConcludeSkill();
			return;
		}
		isUsingSkill = true;
		objAN.SetBool("isSkill", value: true);
		if (crtGeneric != null)
		{
			StopCoroutine(crtGeneric);
		}
		crtGeneric = StartCoroutine(waitForSkillCasting(enmCmnSkill.SkillTimeToSpawn));
	}

	private IEnumerator EnemyWaitForSkillExecution(float waittime)
	{
		yield return new WaitForSeconds(waittime);
		EnemyConcludeSkill();
	}

	public void EnemyConcludeSkill()
	{
		if (coroutExecuteSkill != null)
		{
			StopCoroutine(coroutExecuteSkill);
			coroutExecuteSkill = null;
		}
		bool flag = false;
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Isobora:
			if (!isChasingFainted)
			{
				enemyCurrentXMove = 0f;
			}
			currentDefense = enmBaseStats.baseDefense;
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			GetComponent<BoxCollider2D>().offset = vecOriginalColliderOffset;
			GetComponent<BoxCollider2D>().size = vecOriginalColliderSize;
			enmBaseStats.baseTouchDamage = originalTouchDamage;
			flag = true;
			break;
		case GameCore.EnemyType.BlazePlant:
		case GameCore.EnemyType.NaughtyCoral:
		case GameCore.EnemyType.BeachKitsune:
		case GameCore.EnemyType.FlowerBlossom:
		case GameCore.EnemyType.HillFairy:
		case GameCore.EnemyType.SalamanderGirl:
		case GameCore.EnemyType.Arachne:
		case GameCore.EnemyType.MushroomGirl:
		case GameCore.EnemyType.SwampFairy:
		case GameCore.EnemyType.TentacleTree:
		case GameCore.EnemyType.MothGirl:
		case GameCore.EnemyType.DwarfGirl:
		case GameCore.EnemyType.GhostButler:
		case GameCore.EnemyType.CrystalChunk:
		case GameCore.EnemyType.PurpleSlime:
		case GameCore.EnemyType.PaintingEvil:
		case GameCore.EnemyType.LatexMaiden:
		case GameCore.EnemyType.Bot1:
		case GameCore.EnemyType.Bot2:
		case GameCore.EnemyType.Bot3:
		case GameCore.EnemyType.FleshBlossom:
		case GameCore.EnemyType.Sinccubus:
		case GameCore.EnemyType.Medusa:
		case GameCore.EnemyType.Goblin:
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			break;
		case GameCore.EnemyType.LizardShield:
			GetMiscGameObject(1).SetActive(value: false);
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			ChangeAnimGenericPhaseID(0, isInstant: false, 0f, exitAfterStop: true);
			objAN.SetInteger("genericPhase", 0);
			break;
		case GameCore.EnemyType.LizardAxe:
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			ChangeAnimGenericPhaseID(0, isInstant: false, 0f, exitAfterStop: true);
			objAN.SetInteger("genericPhase", 0);
			break;
		case GameCore.EnemyType.Lamia:
		case GameCore.EnemyType.Kobold:
		case GameCore.EnemyType.SwampGolem:
		case GameCore.EnemyType.PrisonBrute:
		case GameCore.EnemyType.Dominatrix:
		case GameCore.EnemyType.Dogboy:
		case GameCore.EnemyType.SailorBurly:
		case GameCore.EnemyType.WormOverGround:
		case GameCore.EnemyType.WyvernGirl:
		case GameCore.EnemyType.BeeGirl:
		case GameCore.EnemyType.FutaDruidCentauress:
		case GameCore.EnemyType.Failed:
		case GameCore.EnemyType.CorruptBeast:
		case GameCore.EnemyType.DarkDamsel:
		case GameCore.EnemyType.SlimeGirl:
		case GameCore.EnemyType.SewerBug:
		case GameCore.EnemyType.Tentapod:
		case GameCore.EnemyType.VagrantMonk:
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			objAN.SetInteger("genericPhase", 0);
			if (currentEnemyType == GameCore.EnemyType.VagrantMonk)
			{
				listOfExternalAttacks[0].SetActive(value: false);
			}
			break;
		case GameCore.EnemyType.CentaurGirl:
		case GameCore.EnemyType.Goblina:
		case GameCore.EnemyType.HillTroll:
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			objAN.SetInteger("genericPhase", 0);
			countFrequency = 0f;
			enemyCurrentXMove = 0f;
			SetNewLimitIdleTime();
			currentEnemyState = enemyState.Standing;
			break;
		case GameCore.EnemyType.Batgirl:
		case GameCore.EnemyType.BigInsect:
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			objAN.SetInteger("genericPhase", 0);
			DecideNextFlyerState(4);
			break;
		case GameCore.EnemyType.FutaGhost:
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			if ((bool)GC.enemyGhostFutaSkillCasting)
			{
				GC.enemyGhostFutaSkillCasting = null;
			}
			GC.RemoveChildObjectsFromPlayer(alsoDisableObject: true, "FTG-Projectile_A", "", GC.refOBJEffectsParent.transform);
			GC.InstantiateVFX(GameCore.VisualEffect.GhostCast, enmObjRefs.StartPointCenterMass.position, "EnemyGeneralLogic:FutaGhostSkillConclude");
			break;
		case GameCore.EnemyType.FairySorrow:
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			if (!isGrabbing && !isStagger && !isDeath)
			{
				int index = GC.MATH_RNG_INT(0, 9);
				Color color = new Color(0f, 0f, 0f, 1f);
				objGeneric = UnityEngine.Object.Instantiate(GetMiscGameObject(index), posGenericVector, Quaternion.identity, GC.refOBJEnemiesParent.transform);
				objGeneric.GetComponent<EnemyGeneralLogic>().eroCore.IsAllowedToGrab = false;
				objGeneric.GetComponent<SpriteRenderer>().color = color;
			}
			break;
		case GameCore.EnemyType.Catgirl:
			isUsingSkill = false;
			objAN.SetBool("isSkill", value: false);
			objAN.SetInteger("genericPhase", 0);
			AlterSelfBoxCollider(BoxCollAction.Reset, Vector2.zero, Vector2.zero);
			break;
		}
		if (flag)
		{
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitSkillDelay(3f));
		}
	}

	private void CheckDuringSkillUsage(bool isPlayerFaintedStopSkill)
	{
		if (isPlayerFaintedStopSkill && GC.isPlayerFainted)
		{
			EnemyConcludeSkill();
			return;
		}
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.Isobora:
			ISOBORA_DuringSkill();
			break;
		case GameCore.EnemyType.BeachKitsune:
		case GameCore.EnemyType.Arachne:
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = -0.5f;
			break;
		case GameCore.EnemyType.LizardShield:
			LIZARDSHIELD_DuringSkill();
			break;
		case GameCore.EnemyType.LizardAxe:
			LIZARDAXE_DuringSkill();
			break;
		case GameCore.EnemyType.CentaurGirl:
		case GameCore.EnemyType.Goblina:
		case GameCore.EnemyType.HillTroll:
			CENTAURGIRL_DuringSkill();
			break;
		case GameCore.EnemyType.Catgirl:
			CATGIRL_DuringSkill();
			break;
		}
	}

	private void ISOBORA_DuringSkill()
	{
		if (currentEnemyState == enemyState.Standing || currentEnemyState == enemyState.Patrolling || currentEnemyState == enemyState.Chasing)
		{
			if (GC.playerCurrentWorldXPosition > base.transform.position.x)
			{
				enemyCurrentXMove = 1f;
			}
			else
			{
				enemyCurrentXMove = -1f;
			}
			if (enemyCurrentYMove < -3f && (CheckCollisionGround() || CheckCollisionMovingPlatforms()))
			{
				GenericExecuteJump();
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.IsoboraSkill, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
		}
	}

	private void LIZARDSHIELD_DuringSkill()
	{
		if (isGrabbing && objAN.GetInteger("genericPhase") != 0)
		{
			EnemyConcludeSkill();
			return;
		}
		if (objAN.GetInteger("genericPhase") != 3)
		{
			enemyCurrentXMove = 0f;
		}
		switch (objAN.GetInteger("genericPhase"))
		{
		case 0:
			EnemyConcludeSkill();
			return;
		case 1:
			STATE_MACHINE_FLIP_FACE_PLAYER();
			SetNewLimitIdleTime();
			GetMiscGameObject(1).SetActive(value: true);
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, enmCmnSkill.minSkillProximityX * 0.75f, enmCmnSkill.minSkillProximityY))
			{
				ChangeAnimGenericPhaseID(2, isInstant: true, 0f);
			}
			else if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomInverted, enmCmnSkill.minSkillProximityX, enmCmnSkill.minSkillProximityY) && crtGeneric == null)
			{
				ChangeAnimGenericPhaseID(0, isInstant: false, 1f);
			}
			return;
		case 2:
			STATE_MACHINE_FLIP_FACE_PLAYER();
			SetNewLimitIdleTime();
			GetMiscGameObject(1).SetActive(value: true);
			if (crtGeneric == null)
			{
				ChangeAnimGenericPhaseID(3, isInstant: false, 0.75f);
			}
			return;
		}
		GetMiscGameObject(1).SetActive(value: false);
		if (enemyCurrentXMove == 0f)
		{
			if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = 4f;
			}
			else
			{
				enemyCurrentXMove = -4f;
			}
		}
		if (crtGeneric == null)
		{
			ChangeAnimGenericPhaseID(0, isInstant: false, 0.5f);
		}
	}

	private void LIZARDAXE_DuringSkill()
	{
		if (isGrabbing && objAN.GetInteger("genericPhase") != 0)
		{
			EnemyConcludeSkill();
			return;
		}
		if (objAN.GetInteger("genericPhase") != 2)
		{
			enemyCurrentXMove = 0f;
		}
		switch (objAN.GetInteger("genericPhase"))
		{
		case 0:
			EnemyConcludeSkill();
			break;
		case 1:
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomInverted, enmCmnSkill.minSkillProximityX, enmCmnSkill.minSkillProximityY) && crtGeneric == null)
			{
				ChangeAnimGenericPhaseID(0, isInstant: false, 1f);
			}
			break;
		case 2:
			if (enemyCurrentXMove == 0f)
			{
				if (base.transform.localScale.x == 1f)
				{
					enemyCurrentXMove = 4.5f;
				}
				else
				{
					enemyCurrentXMove = -4.5f;
				}
			}
			if (crtGeneric == null)
			{
				ChangeAnimGenericPhaseID(0, isInstant: false, 0.5f);
			}
			break;
		case 11:
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (crtGeneric == null)
			{
				ChangeAnimGenericPhaseID(12, isInstant: false, 1f);
			}
			break;
		default:
			if (crtGeneric == null)
			{
				ChangeAnimGenericPhaseID(0, isInstant: false, 0.5f);
			}
			break;
		}
	}

	private void CENTAURGIRL_DuringSkill()
	{
		if (isGrabbing)
		{
			EnemyConcludeSkill();
			return;
		}
		if ((currentEnemyType == GameCore.EnemyType.Goblina || currentEnemyType == GameCore.EnemyType.HillTroll) && ((base.transform.localScale.x == 1f && base.transform.position.x > GC.playerCurrentWorldXPosition + 2f) || (base.transform.localScale.x != 1f && base.transform.position.x < GC.playerCurrentWorldXPosition - 2f)))
		{
			STATE_MACHINE_FLIP_FACE_PLAYER();
		}
		if (!(enemyCurrentXMove > 0f) && !(enemyCurrentXMove < 0f))
		{
			return;
		}
		if (base.transform.localScale.x == 1f)
		{
			enemyCurrentXMove = enmBaseStats.speedMoveX * 1.2f;
		}
		else
		{
			enemyCurrentXMove = 0f - enmBaseStats.speedMoveX * 1.2f;
		}
		if (currentEnemyType == GameCore.EnemyType.HillTroll)
		{
			if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = enmBaseStats.speedMoveX * 2.5f;
			}
			else
			{
				enemyCurrentXMove = 0f - enmBaseStats.speedMoveX * 2.5f;
			}
		}
		if (Mathf.RoundToInt(objRB.velocity.x) == 0 && CheckCollisionGround())
		{
			GenericExecuteJump(0.75f);
		}
	}

	private void CATGIRL_DuringSkill()
	{
		if (objAN.GetInteger("genericPhase") == 0)
		{
			STATE_MACHINE_FLIP_FACE_PLAYER();
		}
		else if ((currentEnemyState == enemyState.Standing || currentEnemyState == enemyState.Patrolling || currentEnemyState == enemyState.Chasing) && (CheckCollisionGround() || CheckCollisionMovingPlatforms()))
		{
			if (GC.playerCurrentWorldXPosition > base.transform.position.x)
			{
				enemyCurrentXMove = enmBaseStats.speedMoveX * 1.25f;
			}
			else
			{
				enemyCurrentXMove = 0f - enmBaseStats.speedMoveX * 1.25f;
			}
			GenericExecuteJump();
			GC.PlaySFX(GameCore.SFXBasic2.Boing4, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
		}
	}

	private bool HEAL_CheckAbleToHeal(int healingValue, int selfHP_pct, Vector2 boxCastSize, int upperLimitOtherHP_pct, GameObject objVFXHeal = null)
	{
		if (GC.MATH_ResultPercentileOfTotal(amountLife, enmBaseStats.maxLife) < (float)selfHP_pct && GC.MATH_RNG_INT(1, 100) <= 75)
		{
			HEAL_ApplyHealing(healingValue, selfHeal: true, objVFXHeal);
			return true;
		}
		if (GC.MATH_RNG_INT(1, 100) <= 20)
		{
			return false;
		}
		RaycastHit2D[] array = Physics2D.BoxCastAll(enmObjRefs.StartPointCenterMass.position, boxCastSize, 0f, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("LayerEnemies"));
		if (array == null || array.Length <= 1)
		{
			return false;
		}
		EnemyGeneralLogic enemyGeneralLogic = null;
		int num = 0;
		float num2 = 999f;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].transform.GetInstanceID() != base.transform.GetInstanceID())
			{
				enemyGeneralLogic = array[i].transform.gameObject.GetComponent<EnemyGeneralLogic>();
				if (enemyGeneralLogic != null && GC.MATH_ResultPercentileOfTotal(enemyGeneralLogic.amountLife, enemyGeneralLogic.enmBaseStats.maxLife) < num2)
				{
					num2 = GC.MATH_ResultPercentileOfTotal(enemyGeneralLogic.amountLife, enemyGeneralLogic.enmBaseStats.maxLife);
					num = i;
				}
			}
			if (i >= 10)
			{
				break;
			}
		}
		if (num2 >= (float)upperLimitOtherHP_pct)
		{
			return false;
		}
		HEAL_ApplyHealing(healingValue, selfHeal: false, objVFXHeal, array[num].transform.gameObject.GetComponent<EnemyGeneralLogic>());
		return true;
	}

	private void HEAL_ApplyHealing(int healAmount, bool selfHeal, GameObject objVFX_Healing, EnemyGeneralLogic otherScript = null)
	{
		Vector2 zero = Vector2.zero;
		Transform customParent = base.transform;
		if (selfHeal)
		{
			ThisEnemyWasHealed(healAmount);
			zero = base.transform.position;
		}
		else
		{
			if (otherScript == null)
			{
				return;
			}
			customParent = otherScript.transform;
			zero = otherScript.transform.position;
			otherScript.ThisEnemyWasHealed(healAmount);
		}
		if (objVFX_Healing != null)
		{
			BasicProjectileInstantiate(objVFX_Healing, overrideSpawnManual: true, zero.x, zero.y, customParent);
		}
	}

	private void BasicProjectileInstantiate(GameObject overrideOBJ = null, bool overrideSpawnManual = false, float overrideSpawnPosX = 0f, float overrideSpawnPosY = 0f, Transform customParent = null, bool ignoreCollOwnShot = false)
	{
		Vector2 vector = base.transform.position;
		Transform transform = null;
		if (enmCmnSkill.spawnOffsetEmptyObj != null)
		{
			Transform component = enmCmnSkill.spawnOffsetEmptyObj.GetComponent<Transform>();
			float num = component.position.x - vector.x;
			float num2 = component.position.y - vector.y;
			vector.x += num;
			vector.y += num2;
			if (enmObjRefs.StartPointCenterMass != null && enmCmnSkill.spawnOffsetEmptyObj.transform.position == enmObjRefs.StartPointCenterMass.position)
			{
				vector = enmObjRefs.StartPointCenterMass.position;
			}
		}
		else if (overrideSpawnManual)
		{
			vector.x = overrideSpawnPosX;
			vector.y = overrideSpawnPosY;
		}
		if ((bool)customParent)
		{
			transform = customParent;
		}
		else if ((bool)GC.refOBJEffectsParent)
		{
			transform = GC.refOBJEffectsParent.transform;
		}
		if (!(transform != null))
		{
			return;
		}
		if ((bool)overrideOBJ)
		{
			UnityEngine.Object.Instantiate(overrideOBJ, vector, Quaternion.identity, transform);
			return;
		}
		GameObject gameObject = null;
		if ((bool)enmObjRefs.objProjectile)
		{
			gameObject = UnityEngine.Object.Instantiate(enmObjRefs.objProjectile, vector, Quaternion.identity, transform);
		}
		if (ignoreCollOwnShot && gameObject != null)
		{
			Physics2D.IgnoreCollision(objBox2D, gameObject.GetComponent<Collider2D>(), ignore: true);
		}
	}

	public void ANIM_SPAWN_PROJECTILE()
	{
		BasicProjectileInstantiate();
	}

	private bool SkillUseFaintedException()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.FlowerBlossom)
		{
			if (enmCmnSkill.minSkillProximityX < 8f)
			{
				enmCmnSkill.minSkillProximityX = 8f;
			}
			return true;
		}
		return false;
	}

	private void SKILL_OnStartCastGeneric()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.CrystalChunk)
		{
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.Crystal_Cast, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
		}
	}

	private IEnumerator waitForSkillCasting(float wait)
	{
		SKILL_OnStartCastGeneric();
		yield return new WaitForSeconds(wait);
		if (!isUsingSkill)
		{
			EnemyConcludeSkill();
			yield break;
		}
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.BlazePlant:
		case GameCore.EnemyType.NaughtyCoral:
		case GameCore.EnemyType.FlowerBlossom:
		case GameCore.EnemyType.HillFairy:
		case GameCore.EnemyType.Arachne:
		case GameCore.EnemyType.SwampFairy:
		case GameCore.EnemyType.TentacleTree:
		case GameCore.EnemyType.CrystalChunk:
		case GameCore.EnemyType.PurpleSlime:
		case GameCore.EnemyType.Bot1:
		case GameCore.EnemyType.Bot3:
		case GameCore.EnemyType.Goblin:
			BasicProjectileInstantiate();
			break;
		case GameCore.EnemyType.Bot2:
			BasicProjectileInstantiate(null, overrideSpawnManual: false, 0f, 0f, null, ignoreCollOwnShot: true);
			break;
		case GameCore.EnemyType.BeachKitsune:
		{
			float num = GC.MATH_RNG_FLOAT(3f, 4f);
			float num2 = GC.MATH_RNG_FLOAT(-0.6f, 0.6f);
			UnityEngine.Object.Instantiate(position: new Vector2(GC.playerCurrentWorldXPosition + num2, GC.playerCurrentWorldYPosition + num), original: enmObjRefs.objProjectile, rotation: Quaternion.identity, parent: GC.refOBJEffectsParent.transform);
			break;
		}
		case GameCore.EnemyType.MushroomGirl:
			MUSHROOM_Spawn_Spores();
			break;
		case GameCore.EnemyType.MothGirl:
		{
			float overrideSpawnPosX = GC.playerCurrentWorldXPosition + GC.MATH_RNG_FLOAT(-1.5f, 1.5f);
			float overrideSpawnPosY = GC.playerCurrentWorldYPosition + GC.MATH_RNG_FLOAT(-1.5f, 1.5f);
			BasicProjectileInstantiate(null, overrideSpawnManual: true, overrideSpawnPosX, overrideSpawnPosY);
			break;
		}
		case GameCore.EnemyType.DwarfGirl:
		{
			Vector2 vector5 = enmObjRefs.StartPointCenterMass.position;
			vector5.y += 0.4f;
			ProjectileLogic component = UnityEngine.Object.Instantiate(enmObjRefs.objProjectile, vector5, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>();
			if (base.transform.localScale.x == -1f)
			{
				component.specialForce.x = -1f;
			}
			else
			{
				component.specialForce.x = 1f;
			}
			break;
		}
		case GameCore.EnemyType.SwampGolem:
		{
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
				break;
			}
			objAN.SetInteger("genericPhase", 1);
			if (countGenericStuff == 1)
			{
				enemyCurrentYMove = 3f;
				if (base.transform.localScale.x == 1f)
				{
					enemyCurrentXMove = 4.5f;
				}
				else
				{
					enemyCurrentXMove = -4.5f;
				}
				PlayMonsterSoundsFromAnim("swingHeavy");
				break;
			}
			GC.PlaySFX(GameCore.SFXBasic2.GroundPoundLight, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			Vector2 vector4 = enmObjRefs.StartPointCenterMass.position;
			vector4.y -= 1.25f;
			for (int i = 0; i < 6; i++)
			{
				BasicProjectileInstantiate(GetMiscGameObject(1), overrideSpawnManual: true, GC.MATH_RNG_FLOAT(vector4.x - 0.25f, vector4.x + 0.25f), vector4.y);
			}
			break;
		}
		case GameCore.EnemyType.CentaurGirl:
		case GameCore.EnemyType.Goblina:
		case GameCore.EnemyType.HillTroll:
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
				break;
			}
			objAN.SetInteger("genericPhase", 1);
			if (countGenericStuff != 1)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
				if (base.transform.localScale.x == 1f)
				{
					enemyCurrentXMove = 1f;
				}
				else
				{
					enemyCurrentXMove = -1f;
				}
			}
			countGenericStuff = 0;
			break;
		case GameCore.EnemyType.PrisonBrute:
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
				break;
			}
			objAN.SetInteger("genericPhase", 1);
			GenericExecuteJump();
			if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = 7.5f;
			}
			else
			{
				enemyCurrentXMove = -7.5f;
			}
			PlayMonsterSoundsFromAnim("swingHeavy");
			break;
		case GameCore.EnemyType.Dominatrix:
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
				break;
			}
			objAN.SetInteger("genericPhase", 1);
			if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = 1.5f;
			}
			else
			{
				enemyCurrentXMove = -1.5f;
			}
			break;
		case GameCore.EnemyType.Dogboy:
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
			}
			else if (objAN.GetInteger("genericPhase") != 1)
			{
				objAN.SetInteger("genericPhase", 1);
				listOfExternalAttacks[0].SetActive(value: true);
				StartCoroutine(waitForSkillCasting(2f));
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CityDogSkill, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			else
			{
				objAN.SetInteger("genericPhase", 0);
				listOfExternalAttacks[0].SetActive(value: false);
				EnemyConcludeSkill();
			}
			break;
		case GameCore.EnemyType.SailorBurly:
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
			}
			else
			{
				objAN.SetInteger("genericPhase", 1);
			}
			break;
		case GameCore.EnemyType.WormOverGround:
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
				break;
			}
			if (objAN.GetInteger("genericPhase") == 10)
			{
				objAN.SetInteger("genericPhase", 11);
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WormMelee, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			if (objAN.GetInteger("genericPhase") == 20)
			{
				objAN.SetInteger("genericPhase", 21);
				BasicProjectileInstantiate();
				BasicProjectileInstantiate();
			}
			break;
		case GameCore.EnemyType.WyvernGirl:
		{
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
				break;
			}
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.SetInteger("genericPhase", 1);
			Vector2 vector3 = new Vector2(base.transform.position.x, base.transform.position.y);
			if (base.transform.localScale.x == 1f)
			{
				vector3.x += 0.25f;
			}
			else
			{
				vector3.x -= 0.25f;
			}
			vector3.y -= 0.4f;
			BasicProjectileInstantiate(null, overrideSpawnManual: true, vector3.x, vector3.y);
			break;
		}
		case GameCore.EnemyType.Tentapod:
		{
			if (isGrabbing || !isUsingSkill)
			{
				EnemyConcludeSkill();
				break;
			}
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.SetInteger("genericPhase", 1);
			Vector2 vector7 = GetEnemyCenterPos();
			BasicProjectileInstantiate(null, overrideSpawnManual: true, vector7.x, vector7.y);
			break;
		}
		case GameCore.EnemyType.BeeGirl:
		{
			STATE_MACHINE_FLIP_FACE_PLAYER();
			Vector2 vector6 = enmObjRefs.StartPointCenterMass.position;
			vector6.y -= 0.25f;
			Vector2 specialForce = new Vector2(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition);
			for (int j = 0; j < 3; j++)
			{
				UnityEngine.Object.Instantiate(enmObjRefs.objProjectile, vector6, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>().specialForce = specialForce;
				if (j == 0)
				{
					specialForce.y += 2f;
				}
				if (j == 1)
				{
					specialForce.y -= 4f;
				}
			}
			break;
		}
		case GameCore.EnemyType.FutaDruidCentauress:
		{
			int integer = objAN.GetInteger("genericPhase");
			if (integer != 2)
			{
				BasicProjectileInstantiate();
			}
			else
			{
				EnemyConcludeSkill();
			}
			break;
		}
		case GameCore.EnemyType.BigInsect:
			objAN.SetInteger("genericPhase", 1);
			currentFlyingState = enemyFlyingState.SpecialAction;
			FLY_DiveSkillGeneric(new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, 0f), 2f, 3f, 0);
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.BIS_Skill, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.Failed:
		case GameCore.EnemyType.CorruptBeast:
		case GameCore.EnemyType.SlimeGirl:
		case GameCore.EnemyType.VagrantMonk:
		{
			Vector2 targetPosition = default;
			targetPosition.x = GC.playerCurrentWorldXPosition;
			targetPosition.y = GC.playerCurrentWorldYPosition;
			Vector2 vector2 = GC.MATH_CALC_BALLISTIC_TRAJECTORY_TO_POINT(base.transform.position, targetPosition, increaseHeightSpecial: true, Vector2.zero);
			if (currentEnemyType == GameCore.EnemyType.SlimeGirl)
			{
				vector2.x *= 1.25f;
			}
			if (currentEnemyType == GameCore.EnemyType.VagrantMonk)
			{
				vector2.x *= 0.75f;
				listOfExternalAttacks[0].SetActive(value: true);
			}
			enemyCurrentXMove = vector2.x;
			enemyCurrentYMove = vector2.y;
			GenericExecuteJump();
			objAN.SetInteger("genericPhase", 1);
			if (currentEnemyType == GameCore.EnemyType.Failed)
			{
				GC.PlaySFX(GameCore.SFXBasic2.Swing5, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			if (currentEnemyType == GameCore.EnemyType.CorruptBeast)
			{
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CRB_Skill, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			if (currentEnemyType == GameCore.EnemyType.SlimeGirl)
			{
				GC.PlaySFX(GameCore.SFXBasic2.Boing1, 0, checkForDuplicity: true, base.transform.position, 1.2f, 1.3f);
			}
			break;
		}
		case GameCore.EnemyType.Sinccubus:
		{
			Vector2 vector = new Vector2(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition);
			vector.x += GC.MATH_RNG_FLOAT(-3f, 3f);
			vector.y += GC.MATH_RNG_FLOAT(-3f, 3f);
			UnityEngine.Object.Instantiate(enmObjRefs.objProjectile, vector, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>().objOriginator = base.gameObject;
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, vector, "EnemyGeneralLogic:SinccubusProjectileSkill");
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.SCB_Skill, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		}
		case GameCore.EnemyType.DarkDamsel:
			objAN.SetInteger("genericPhase", 1);
			UnityEngine.Object.Instantiate(enmObjRefs.objProjectile, enmObjRefs.StartPointCenterMass.position, Quaternion.identity, base.transform);
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.DKD_Skill, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			break;
		case GameCore.EnemyType.FairySorrow:
			posGenericVector = new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, 0f);
			posGenericVector.x += GC.MATH_RNG_FLOAT(-0.5f, 0.5f);
			posGenericVector.y += GC.MATH_RNG_FLOAT(-0.5f, 0.5f);
			UnityEngine.Object.Instantiate(enmObjRefs.objProjectile, posGenericVector, Quaternion.identity, GC.refOBJEffectsParent.transform);
			break;
		case GameCore.EnemyType.Catgirl:
			objAN.SetInteger("genericPhase", 1);
			currentDefense = enmBaseStats.specialDefense;
			originalTouchDamage = enmBaseStats.baseTouchDamage;
			enmBaseStats.baseTouchDamage = enmCmnSkill.baseSkillDamage;
			isUsingSkill = true;
			objAN.SetBool("isSkill", value: true);
			AlterSelfBoxCollider(BoxCollAction.ChangeBoth, new Vector2(1.1f, 1.1f), new Vector2(0f, -0.2f));
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.CRB_Skill, 0, checkForDuplicity: true, base.transform.position, 0.7f, 0.8f);
			break;
		}
		crtGeneric = null;
	}

	private IEnumerator waitSkillDelay(float wait)
	{
		isSkillDelay = true;
		yield return new WaitForSeconds(wait);
		isSkillDelay = false;
	}

	private void InvokedForestBossBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		STATE_MACHINE_FLIP_FACE_PLAYER();
		if (currentBossState != BossState.Battle)
		{
			if (!GC.isBossFight)
			{
				return;
			}
			objAN.SetBool("isBossBattleMode", value: true);
			currentBossState = BossState.Battle;
			timeToTriggerNextSkill = 1f;
			GC.UpdateBossName(enmBaseStats.idEnemyNameInFile);
		}
		if (sequenceOfBossSkill == 1 && currentDefense == enmBaseStats.baseDefense)
		{
			currentDefense *= 99;
		}
		else if (sequenceOfBossSkill != 1 && currentDefense != enmBaseStats.baseDefense)
		{
			currentDefense = enmBaseStats.baseDefense;
		}
		CheckBossEnragedPhaseChangeLogic("ForestBoss-Enraged", 2.5f, playSFX: true, 3);
		if (!isGrabbing && !GC.isPlayerFainted)
		{
			BossSkillCheck();
		}
		if (GC.isPlayerFainted)
		{
			ForestBoss_EroSporeCast();
		}
	}

	private void ExecuteForestBossSkill()
	{
		switch (sequenceOfBossSkill)
		{
		case 0:
		{
			if (!GetMiscGameObject(0) || !GetMiscGameObject(1))
			{
				break;
			}
			DoBossSkillAnimForSeconds("ForestBoss-UseSkillVines", 3f);
			PlaySFXGeneric(2);
			HazardLogic component2 = GetMiscGameObject(0).GetComponent<HazardLogic>();
			if (component2 == null)
			{
				break;
			}
			HazardLogic component3 = GetMiscGameObject(1).GetComponent<HazardLogic>();
			if (!(component3 == null))
			{
				component2.InitiateSpecialBehaviour("left", phaseOfBossFight, "");
				component3.InitiateSpecialBehaviour("right", phaseOfBossFight, "");
				sequenceOfBossSkill = 1;
				if (phaseOfBossFight <= 0)
				{
					timeToTriggerNextSkill = 4.5f;
				}
				else
				{
					timeToTriggerNextSkill = 4f;
				}
			}
			break;
		}
		case 1:
		{
			if (!GetMiscGameObject(2) || !GC.refOBJEffectsParent)
			{
				break;
			}
			DoBossSkillAnimForSeconds("ForestBoss-UseSkillSpore", 2.25f);
			PlaySFXGeneric(5);
			countGenericStuff = 0;
			posGenericVector = base.transform.position;
			posGenericVector.y += 1.5f;
			if (phaseOfBossFight <= 0)
			{
				InvokeRepeating("InvokedForestBossSkill2", 0f, 0.2f);
			}
			else
			{
				InvokeRepeating("InvokedForestBossSkill2", 0f, 0.1f);
			}
			HazardLogic component = GetMiscGameObject(5).GetComponent<HazardLogic>();
			if (!(component == null))
			{
				string text = "";
				text = ((!(GC.playerCurrentWorldXPosition <= base.transform.position.x)) ? "right" : "left");
				component.InitiateSpecialBehaviour(text, phaseOfBossFight, "Chase");
				sequenceOfBossSkill = 2;
				if (phaseOfBossFight <= 0)
				{
					timeToTriggerNextSkill = 6f;
				}
				else
				{
					timeToTriggerNextSkill = 5f;
				}
			}
			break;
		}
		case 2:
			if (!GetMiscGameObject(3) || !GetMiscGameObject(4) || !GC.refOBJEffectsParent)
			{
				break;
			}
			DoBossSkillAnimForSeconds("ForestBoss-UseSeedSkill", 0.25f);
			PlaySFXGeneric(1);
			if (phaseOfBossFight <= 0)
			{
				if (GC.MATH_RNG_INT(0, 1) == 0)
				{
					SpawnForestBossSeed_A();
				}
				else
				{
					SpawnForestBossSeed_B();
				}
			}
			else
			{
				SpawnForestBossSeed_A();
				SpawnForestBossSeed_B();
			}
			sequenceOfBossSkill = 0;
			if (phaseOfBossFight <= 0)
			{
				timeToTriggerNextSkill = 5f;
			}
			else
			{
				timeToTriggerNextSkill = 4f;
			}
			break;
		}
	}

	private void InvokedForestBossSkill2()
	{
		UnityEngine.Object.Instantiate(GetMiscGameObject(2), posGenericVector, Quaternion.identity, GC.refOBJEffectsParent.transform);
		countGenericStuff++;
		if (phaseOfBossFight <= 0 && countGenericStuff >= 10)
		{
			CancelInvoke("InvokedForestBossSkill2");
		}
		else if (phaseOfBossFight == 1 && countGenericStuff >= 20)
		{
			CancelInvoke("InvokedForestBossSkill2");
		}
	}

	private void SpawnForestBossSeed_A()
	{
		UnityEngine.Object.Instantiate(GetMiscGameObject(3), base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
	}

	private void SpawnForestBossSeed_B()
	{
		UnityEngine.Object.Instantiate(GetMiscGameObject(4), base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
	}

	private void ForestBoss_EroSporeCast()
	{
		if (!canBossUseEroAttractSkill && crtWaitBossCastEroAttractSkill == null)
		{
			isUsingEroAttractSkill = false;
			crtWaitBossCastEroAttractSkill = StartCoroutine(waitDelayBossCastEroAttractSkill(2f));
		}
		if (!GC.playerIsBeingGrabed && GC.playerIsAllowedToBeGrabbed && canBossUseEroAttractSkill && !isUsingEroAttractSkill && !isGrabbing)
		{
			isUsingEroAttractSkill = true;
			DoBossSkillAnimForSeconds("ForestBoss-UseSkillSpore", 1f);
			PlaySFXGeneric(1);
			posGenericVector = base.transform.position;
			posGenericVector.y += 1.5f;
			if ((bool)GC.refOBJEffectsParent)
			{
				UnityEngine.Object.Instantiate(GetMiscGameObject(6), posGenericVector, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
			if (crtWaitBossCastEroAttractSkill != null)
			{
				StopCoroutine(crtWaitBossCastEroAttractSkill);
			}
			crtWaitBossCastEroAttractSkill = StartCoroutine(waitDelayBossCastEroAttractSkill(4f));
		}
	}

	private IEnumerator waitDelayBossCastEroAttractSkill(float wait)
	{
		yield return new WaitForSeconds(wait);
		isUsingEroAttractSkill = false;
		canBossUseEroAttractSkill = true;
		crtWaitBossCastEroAttractSkill = null;
	}

	private void InvokedBeachBossBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		if (currentBossState != BossState.Battle)
		{
			if (!GC.isBossFight)
			{
				return;
			}
			objAN.SetBool("isBossBattleMode", value: true);
			currentBossState = BossState.Battle;
			timeToTriggerNextSkill = 1f;
			GC.UpdateBossName(enmBaseStats.idEnemyNameInFile);
		}
		CheckBossEnragedPhaseChangeLogic("", 2f, playSFX: false, 0);
		if (!CheckCollisionGround() && sequenceOfBossSkill == 0)
		{
			Special_BeachBossCallDiveSpecial();
		}
		if (!isGrabbing && !GC.isPlayerFainted)
		{
			BossSkillCheck();
		}
	}

	private void ExecuteBeachBossSkill()
	{
		switch (sequenceOfBossSkill)
		{
		case 0:
			isMoveToPoint = false;
			countGenericStuff++;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			posGenericVector = base.transform.position;
			if (base.transform.localScale.x == 1f)
			{
				posGenericVector.x += 2f;
			}
			else
			{
				posGenericVector.x -= 2f;
			}
			enmBaseStats.speedMoveX = 3f;
			crtGeneric = StartCoroutine(waitBossBeachGeneric(1f, "doSlash"));
			objAN.Play("Slash-Pre");
			timeToTriggerNextSkill = 4f;
			break;
		case 1:
			if (IsLeftPointClosest())
			{
				posGenericVector = enmMisc.Objects[0].transform.position;
				STATE_MACHINE_FLIP_SELF(forcedLeft: false, forcedRight: true);
			}
			else
			{
				posGenericVector = enmMisc.Objects[1].transform.position;
				STATE_MACHINE_FLIP_SELF(forcedLeft: true);
			}
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
				crtGeneric = null;
			}
			GC.PlaySFX(GameCore.SFXBasic2.Odd4, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			objBox2D.isTrigger = true;
			enmBaseStats.speedMoveX = 1f;
			objAN.Play("Jump");
			isMoveToPoint = true;
			InvokeRepeating("Spawn2DSpriteTrail", 0f, 0.1f);
			timeToTriggerNextSkill = 999f;
			break;
		case 2:
			Special_BeachBossSplash(0.25f);
			posGenericVector = base.transform.position;
			posGenericVector.y -= 2f;
			base.transform.position = posGenericVector;
			objBox2D.isTrigger = true;
			crtGeneric = StartCoroutine(waitBossBeachGeneric(0.25f, "doChase"));
			timeToTriggerNextSkill = 999f;
			GetMiscGameObject(4).SetActive(value: true);
			break;
		case 3:
		{
			float wait = 3f;
			enmBaseStats.speedMoveX = 3f;
			isMoveToPoint = true;
			if (phaseOfBossFight > 0)
			{
				wait = 2f;
			}
			crtGeneric = StartCoroutine(waitBossBeachGeneric(wait, "stopChase"));
			timeToTriggerNextSkill = 999f;
			break;
		}
		case 4:
			isJumpDelay = false;
			GetMiscGameObject(4).SetActive(value: false);
			Special_BeachBossSplash(2.25f);
			posGenericVector = base.transform.position;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			enmBaseStats.speedMoveX = 1f;
			enmBaseStats.speedMoveY = 4f;
			enemyCurrentYMove = 6f;
			if (IsLeftPointClosest())
			{
				enemyCurrentXMove = 4f;
			}
			else
			{
				enemyCurrentXMove = -4f;
			}
			currentEnemyState = enemyState.Jumping;
			isMoveToPoint = true;
			timeToTriggerNextSkill = 999f;
			break;
		}
	}

	private bool IsLeftPointClosest()
	{
		float x = GetMiscGameObject(0).transform.position.x;
		float x2 = GetMiscGameObject(1).transform.position.x;
		if (GC.MATH_POINTDISTX_FLOAT(base.transform.position.x, x) <= GC.MATH_POINTDISTX_FLOAT(base.transform.position.x, x2))
		{
			return true;
		}
		return false;
	}

	private void Special_BeachBossSplash(float offsetY)
	{
		posGenericVector = base.transform.position;
		posGenericVector.y += offsetY;
		UnityEngine.Object.Instantiate(GetMiscGameObject(3), posGenericVector, Quaternion.identity, GC.refOBJEffectsParent.transform).transform.localScale = new Vector3(2f, 2f, 2f);
		GC.PlaySFX(GameCore.SFXBasic2.Splash1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
	}

	private void Invoked_BeachBossSkill1()
	{
		bool flag = false;
		float num = 2f;
		GameObject gameObject = null;
		if (isJumpDelay)
		{
			isJumpDelay = false;
			flag = true;
			num = 0.5f;
		}
		else
		{
			objAN.Play("Slash2");
			countGenericStuff++;
			posGenericVector = base.transform.position;
			if (base.transform.localScale.x == -1f)
			{
				posGenericVector.x--;
			}
			else
			{
				posGenericVector.x++;
			}
			gameObject = ((phaseOfBossFight > 0) ? GetMiscGameObject(5) : GetMiscGameObject(2));
			if ((bool)GC.refOBJEffectsParent)
			{
				UnityEngine.Object.Instantiate(gameObject, posGenericVector, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
			if (phaseOfBossFight <= 0)
			{
				if (countGenericStuff >= 3)
				{
					flag = true;
				}
			}
			else if (countGenericStuff >= 5)
			{
				flag = true;
			}
		}
		if (flag)
		{
			countGenericStuff = 0;
			countTimeToUseSkill = 0f;
			sequenceOfBossSkill = 2;
			timeToTriggerNextSkill = num;
			CancelInvoke("Invoked_BeachBossSkill1");
		}
	}

	private void Special_BeachBossCallDiveSpecial()
	{
		SpecialBeachBossStopSlash();
		enemyCurrentYMove = 0f;
		enmBaseStats.speedMoveX = 0f;
		enmBaseStats.speedMoveY = 0f;
		countTimeToUseSkill = 0f;
		sequenceOfBossSkill = 1;
		timeToTriggerNextSkill = 0f;
		isJumpDelay = true;
	}

	private IEnumerator waitBossBeachGeneric(float wait, string action)
	{
		yield return new WaitForSeconds(wait);
		switch (action)
		{
		case "doSlash":
			objAN.Play("Slash");
			GC.PlaySFX(GameCore.SFXBasic2.Swing1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			isMoveToPoint = true;
			break;
		case "doChase":
			countTimeToUseSkill = 0f;
			sequenceOfBossSkill = 3;
			timeToTriggerNextSkill = 0.5f;
			break;
		case "stopChase":
		{
			isMoveToPoint = false;
			float num = 1f;
			if (phaseOfBossFight > 0)
			{
				num = 0.25f;
			}
			countTimeToUseSkill = 0f;
			sequenceOfBossSkill = 4;
			timeToTriggerNextSkill = num;
			break;
		}
		case "resetSkills":
			countTimeToUseSkill = 0f;
			sequenceOfBossSkill = 0;
			timeToTriggerNextSkill = 1f;
			break;
		}
	}

	public void SpecialBeachBossStopSlash()
	{
		objAN.Play("Basic-Idle");
		isMoveToPoint = false;
		if (phaseOfBossFight <= 0)
		{
			if (countGenericStuff >= 2)
			{
				countTimeToUseSkill = 0f;
				sequenceOfBossSkill = 1;
				timeToTriggerNextSkill = 3f;
			}
			else
			{
				timeToTriggerNextSkill = 4f;
			}
		}
		else if (countGenericStuff >= 3)
		{
			countTimeToUseSkill = 0f;
			sequenceOfBossSkill = 1;
			timeToTriggerNextSkill = 2f;
		}
		else
		{
			timeToTriggerNextSkill = 3f;
		}
	}

	private void InvokedHillBossBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		if (currentBossState != BossState.Battle)
		{
			if (!GC.isBossFight)
			{
				return;
			}
			objAN.SetBool("isBossBattleMode", value: true);
			currentBossState = BossState.Battle;
			timeToTriggerNextSkill = 1f;
			GC.UpdateBossName(enmBaseStats.idEnemyNameInFile);
		}
		CheckBossEnragedPhaseChangeLogic("", 2f, playSFX: true, 2);
		CHASE_ValidateFaintedChaseFetishes();
		if (isGrabbing)
		{
			sequenceOfBossSkill = 0;
			invertSpriteFacing = false;
			isUsingSkill = false;
		}
		else if (!isGrabbing && GC.isPlayerFainted && canChaseFainted && sequenceOfBossSkill != 1 && CheckCollisionGround())
		{
			isUsingSkill = false;
			objAN.Play("Basic-Idle");
			invertSpriteFacing = false;
			sequenceOfBossSkill = 1;
		}
		else if (!isGrabbing && !GC.isPlayerFainted)
		{
			BossSkillCheck();
		}
		else if (GC.isPlayerFainted && !canChaseFainted && sequenceOfBossSkill != 0)
		{
			isUsingSkill = false;
			objAN.Play("Basic-Idle");
			invertSpriteFacing = false;
			sequenceOfBossSkill = 0;
		}
		switch (sequenceOfBossSkill)
		{
		case 0:
		case 4:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			break;
		case 1:
			if (base.transform.position.x < GC.playerCurrentWorldXPosition)
			{
				enemyCurrentXMove = 1f;
			}
			else
			{
				enemyCurrentXMove = -1f;
			}
			if (phaseOfBossFight == 1 && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f))
			{
				sequenceOfBossSkill = 2;
				timeToTriggerNextSkill = 0f;
			}
			break;
		case 2:
			if ((GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomXOnly, 1f) && phaseOfBossFight != 1 && enemyCurrentXMove != 0f) || (enemyCurrentYMove < 0.5f && CheckCollisionGround()))
			{
				enemyCurrentXMove = 0f;
			}
			if (enemyCurrentYMove < -1f && !CheckCollisionGround() && !objAN.GetCurrentAnimatorStateInfo(0).IsName("Pre-Stomp1"))
			{
				objAN.Play("Pre-Stomp1");
				PlaySFXGeneric(1);
			}
			if (objAN.GetCurrentAnimatorStateInfo(0).IsName("Pre-Stomp1") && CheckCollisionGround())
			{
				isUsingSkill = true;
			}
			break;
		case 3:
			if (enemyCurrentXMove != 0f && enemyCurrentYMove != 0f && Mathf.RoundToInt(objRB.velocity.x) == 0 && !CheckCollisionGround())
			{
				enemyCurrentXMove = 0f;
			}
			break;
		}
		objAN.SetBool("isSkill", isUsingSkill);
	}

	private void ExecuteHillBossSkill()
	{
		switch (sequenceOfBossSkill)
		{
		case 0:
			isUsingSkill = false;
			SetWaitToChangeSequence(1, 0.2f);
			countGenericStuff = 0;
			break;
		case 1:
			SetWaitToChangeSequence(2, 3f);
			break;
		case 2:
			countGenericStuff++;
			if (base.transform.position.x < GC.playerCurrentWorldXPosition)
			{
				enemyCurrentXMove = 1f;
			}
			else
			{
				enemyCurrentXMove = -1f;
			}
			GenericExecuteJump();
			if (phaseOfBossFight == 1)
			{
				enemyCurrentYMove /= 1.75f;
			}
			PlaySFXGeneric(2);
			if (phaseOfBossFight != 1)
			{
				SetWaitToChangeSequence(3, 3f);
			}
			else if (countGenericStuff < 2)
			{
				SetWaitToChangeSequence(2, 1.5f);
			}
			else
			{
				SetWaitToChangeSequence(3, 2f);
			}
			break;
		case 3:
			countGenericStuff = 0;
			isUsingSkill = false;
			if (base.transform.position.x > GC.playerCurrentWorldXPosition)
			{
				enemyCurrentXMove = 1.5f;
			}
			else
			{
				enemyCurrentXMove = -1.5f;
			}
			GenericExecuteJump(0.75f);
			invertSpriteFacing = true;
			PlaySFXGeneric(4);
			SetWaitToChangeSequence(4, 1.5f);
			break;
		case 4:
		{
			float wait = 1.5f;
			if (phaseOfBossFight == 1)
			{
				wait = 1f;
			}
			countGenericStuff++;
			invertSpriteFacing = false;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.Play("Pre-Stomp2");
			PlaySFXGeneric(3);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitBossHillGeneric(wait));
			if (phaseOfBossFight != 1)
			{
				SetWaitToChangeSequence(0, 3.5f);
			}
			else if (countGenericStuff < 2)
			{
				SetWaitToChangeSequence(4, 1.5f);
			}
			else
			{
				SetWaitToChangeSequence(0, 3f);
			}
			break;
		}
		}
	}

	private void HillBossSpawnSkill(int skill)
	{
		if (!isUsingSkill)
		{
			return;
		}
		if (skill == 1)
		{
			Vector2 vector = base.transform.position;
			vector.y -= 0.2f;
			Vector2 vector2 = vector;
			vector.x -= 0.2f;
			vector2.x += 0.2f;
			UnityEngine.Object.Instantiate(GetMiscGameObject(0), vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
			UnityEngine.Object.Instantiate(GetMiscGameObject(1), vector2, Quaternion.identity, GC.refOBJEffectsParent.transform);
			GC.CallShakeTheCamera(0.75f, 0.75f, 1f);
			GC.PlaySFX(GameCore.SFXBasic2.GroundPoundLonger, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 1.2f);
		}
		else
		{
			Vector2 vector3 = base.transform.position;
			float num = 0.35f;
			float num2 = 0.5f;
			vector3.y--;
			if (base.transform.localScale.x == 1f)
			{
				vector3.x += 2f;
			}
			else
			{
				vector3.x -= 2f;
				num *= -1f;
			}
			float num3 = num;
			float num4 = num2;
			GC.CallShakeTheCamera(0.75f, 0.75f, 1f);
			GC.PlaySFX(GameCore.SFXBasic2.GroundPoundLonger, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 1.2f);
			for (int i = 0; i < 7; i++)
			{
				UnityEngine.Object.Instantiate(GetMiscGameObject(2), vector3, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>().specialForce = new Vector2(num3, num4);
				num3 += num;
				num4 += num2;
			}
		}
		isUsingSkill = false;
	}

	private IEnumerator waitBossHillGeneric(float wait)
	{
		yield return new WaitForSeconds(wait);
		isUsingSkill = true;
	}

	private void InvokedWerewolfBossBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		if (currentBossState != BossState.Battle && !BOSS_IS_PRE_FIGHT_MODE_COMPLETED())
		{
			return;
		}
		if (isGrabbing && boolGeneric)
		{
			enemyCurrentXMove = 0f;
			return;
		}
		CheckBossEnragedPhaseChangeLogic("", 2f, playSFX: true, 2);
		CHASE_ValidateFaintedChaseFetishes();
		if (!isStatic)
		{
			BOSS_Walker_Ero_Chase_Logic_Check(0, 1);
		}
		if (!isGrabbing && !GC.isPlayerFainted)
		{
			BossSkillCheck();
		}
		WEREWOLF_MovementPatterns();
		objAN.SetBool("isSkill", isUsingSkill);
	}

	private void WEREWOLF_MovementPatterns()
	{
		if (sequenceOfBossSkill != 3 && IsInvoking("Spawn2DSpriteTrail"))
		{
			CancelInvoke("Spawn2DSpriteTrail");
		}
		if (sequenceOfBossSkill != 3 && GetMiscGameObject(2).activeInHierarchy)
		{
			GetMiscGameObject(2).SetActive(value: false);
		}
		if (sequenceOfBossSkill != 7 && GetMiscGameObject(4).activeInHierarchy)
		{
			GetMiscGameObject(4).SetActive(value: false);
		}
		switch (sequenceOfBossSkill)
		{
		case 0:
		case 4:
		case 6:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			break;
		case 1:
			floatGeneric = 1.5f;
			if (GC.MATH_RNG_INT(1, 1000) <= 5)
			{
				SetWaitToChangeSequence(6, 0.1f);
			}
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 1.75f;
			}
			if (base.transform.position.x < GC.playerCurrentWorldXPosition && CheckCollisionGround())
			{
				enemyCurrentXMove = floatGeneric;
			}
			else
			{
				enemyCurrentXMove = 0f - floatGeneric;
			}
			if (isUsingSkill && enemyCurrentXMove != 0f)
			{
				isUsingSkill = false;
			}
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f) && !isUsingSkill && CheckCollisionGround())
			{
				sequenceOfBossSkill = 2;
				timeToTriggerNextSkill = 0f;
			}
			WEREWOLF_LeapBackwardsWhenHit();
			if (GC.MATH_RNG_INT(1, 1000) <= 15)
			{
				WEREWOLF_PrepareToPounce();
			}
			break;
		case 2:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			WEREWOLF_LeapBackwardsWhenHit();
			break;
		case 3:
			floatGeneric = 2f;
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 2.5f;
			}
			if (base.transform.position.x > GC.playerCurrentWorldXPosition)
			{
				enemyCurrentXMove = floatGeneric;
			}
			else
			{
				enemyCurrentXMove = 0f - floatGeneric;
			}
			break;
		case 5:
			floatGeneric = 1f;
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 1.5f;
			}
			if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = 0f - floatGeneric;
			}
			else
			{
				enemyCurrentXMove = floatGeneric;
			}
			break;
		case 7:
			floatGeneric = 2.5f;
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 2.75f;
			}
			if (isUsingSkill)
			{
				bool flag2 = true;
				if (base.transform.localScale.x != 1f)
				{
					flag2 = ((GC.playerCurrentWorldXPosition > base.transform.position.x && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomXOnlyInverted, 6f)) ? true : false);
				}
				else if (GC.playerCurrentWorldXPosition < base.transform.position.x && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomXOnlyInverted, 6f))
				{
					flag2 = false;
				}
				if (flag2)
				{
					enemyCurrentXMove = floatGeneric;
				}
				else
				{
					enemyCurrentXMove = 0f - floatGeneric;
				}
				if (enemyCurrentXMove != 0f && !GetMiscGameObject(4).activeInHierarchy)
				{
					GetMiscGameObject(4).SetActive(value: true);
				}
			}
			break;
		case 8:
			if (isUsingSkill)
			{
				floatGeneric = 1.55f;
				if (phaseOfBossFight == 1)
				{
					floatGeneric = 1.75f;
				}
				if (base.transform.localScale.x == 1f)
				{
					enemyCurrentXMove = floatGeneric;
				}
				else
				{
					enemyCurrentXMove = 0f - floatGeneric;
				}
				if (CheckCollisionGround() && enemyCurrentYMove <= 0f)
				{
					isUsingSkill = false;
					enemyCurrentXMove = 0f;
				}
			}
			break;
		case 99:
			floatGeneric = 1.25f;
			if (GC.playerSpecialAnimCount == 5 && !boolGeneric)
			{
				boolGeneric = true;
				RunTheCoroutinesAfterEroEnd(eroCore.eroGracePeriod * 2f);
				break;
			}
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomXOnly, 4.5f) && !isChasingFainted)
			{
				if (coroutEroGracePeriod != null)
				{
					bool flag = false;
					float num = 0f;
					num = base.transform.position.x - vecStartingPosition.x;
					if (enemyCurrentXMove == 0f)
					{
						if (GC.MATH_RNG_INT(0, 1) == 1)
						{
							flag = true;
						}
						if (num <= -7f)
						{
							flag = true;
						}
						else if (num >= 7f)
						{
							flag = false;
						}
						if (flag)
						{
							enemyCurrentXMove = floatGeneric;
						}
						else
						{
							enemyCurrentXMove = 0f - floatGeneric;
						}
					}
				}
				else if (enemyCurrentXMove != 0f)
				{
					enemyCurrentXMove = 0f;
					if (!isGrabbing)
					{
						STATE_MACHINE_FLIP_FACE_PLAYER();
					}
				}
			}
			else if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomXOnlyInverted, 5.5f) || isChasingFainted)
			{
				if (coroutEroGracePeriod != null)
				{
					enemyCurrentXMove = 0f;
					STATE_MACHINE_FLIP_FACE_PLAYER();
					break;
				}
				if (base.transform.position.x <= GC.playerCurrentWorldXPosition)
				{
					enemyCurrentXMove = floatGeneric;
				}
				else
				{
					enemyCurrentXMove = 0f - floatGeneric;
				}
			}
			else if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
				if (!isGrabbing)
				{
					STATE_MACHINE_FLIP_FACE_PLAYER();
				}
			}
			if (!isGrabbing && crtGeneric == null && coroutEroGracePeriod == null && GC.playerSpecialAnimCount == 5 && !isChasingFainted)
			{
				crtGeneric = StartCoroutine(waitWerewolfLovePhase(4f));
			}
			if (isGrabbing && isChasingFainted)
			{
				isChasingFainted = false;
			}
			break;
		}
	}

	private void ExecuteWerewolfBossSkill()
	{
		float num = 0f;
		switch (sequenceOfBossSkill)
		{
		case 0:
			isUsingSkill = false;
			invertSpriteFacing = false;
			countGenericStuff = 0;
			SetWaitToChangeSequence(1, 1f);
			break;
		case 1:
			floatGeneric = 8f;
			invertSpriteFacing = false;
			if (isUsingSkill)
			{
				isUsingSkill = false;
			}
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 5f;
			}
			SetWaitToChangeSequence(3, floatGeneric);
			break;
		case 2:
			if (phaseOfBossFight == 1 && GC.MATH_RNG_INT(1, 100) <= 10)
			{
				SetWaitToChangeSequence(7, 1f);
				break;
			}
			countGenericStuff++;
			invertSpriteFacing = false;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.SetInteger("genericPhase", 0);
			objAN.Play("CWW-PreClaw");
			num = 1f;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitWerewolfBossSkill(num));
			if (countGenericStuff < 3)
			{
				SetWaitToChangeSequence(1, 2f);
				break;
			}
			countGenericStuff = 0;
			SetWaitToChangeSequence(3, 2f);
			break;
		case 3:
			if (GC.MATH_RNG_INT(1, 100) <= 35)
			{
				SetWaitToChangeSequence(6, 0.1f);
				break;
			}
			countGenericStuff = 0;
			isUsingSkill = false;
			invertSpriteFacing = false;
			PlaySFXGeneric(10);
			InvokeRepeating("Spawn2DSpriteTrail", 0f, 0.1f);
			SetWaitToChangeSequence(4, 5f);
			break;
		case 4:
			floatGeneric = 1f;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			isUsingSkill = false;
			invertSpriteFacing = false;
			countGenericStuff++;
			if (base.transform.position.x > vecStartingPosition.x)
			{
				STATE_MACHINE_FLIP_SELF(forcedLeft: true);
			}
			else
			{
				STATE_MACHINE_FLIP_SELF(forcedLeft: false, forcedRight: true);
			}
			objAN.SetInteger("genericPhase", 1);
			objAN.Play("CWW-PreClaw");
			if (phaseOfBossFight == 1)
			{
				floatGeneric -= 0.5f;
			}
			num = floatGeneric;
			GC.InstantiateVFX(GameCore.VisualEffect.ChargePower_White, enmObjRefs.GroundCheck2.position, "EnemyGeneralLogic_WerewolfBossSkill");
			crtGeneric = StartCoroutine(waitWerewolfBossSkill(num));
			if (phaseOfBossFight == 0)
			{
				if (countGenericStuff < 2)
				{
					SetWaitToChangeSequence(4, floatGeneric + 0.5f);
				}
				else
				{
					SetWaitToChangeSequence(0, 3f);
				}
			}
			else if (countGenericStuff < 3)
			{
				SetWaitToChangeSequence(4, floatGeneric + 0.5f);
			}
			else
			{
				SetWaitToChangeSequence(0, 2f);
			}
			break;
		case 5:
			isUsingSkill = false;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			invertSpriteFacing = true;
			countGenericStuff++;
			objAN.Play("Basic-Idle");
			PlaySFXGeneric(5);
			GenericExecuteJump(0.5f);
			if (countGenericStuff < 3)
			{
				floatGeneric = 20f;
				if (phaseOfBossFight == 1)
				{
					floatGeneric = 50f;
				}
				if ((float)GC.MATH_RNG_INT(1, 100) <= floatGeneric)
				{
					SetWaitToChangeSequence(7, 1f);
				}
				else
				{
					SetWaitToChangeSequence(1, 0.75f);
				}
			}
			else
			{
				countGenericStuff = 0;
				SetWaitToChangeSequence(3, 0.75f);
			}
			break;
		case 6:
			GC.InstantiateVFX(GameCore.VisualEffect.ChargePower_Yellow, enmObjRefs.GroundCheck2.position, "EnemyGeneralLogic_WerewolfBossSkill");
			isUsingSkill = true;
			invertSpriteFacing = false;
			countGenericStuff = 0;
			objAN.Play("CWW-Howl");
			PlaySFXGeneric(2);
			WEREWOLF_SpawnMoonExplosion();
			SetWaitToChangeSequence(1, 2.75f);
			break;
		case 7:
			floatGeneric = 3.5f;
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			GC.InstantiateVFX(GameCore.VisualEffect.ChargePower_Red, enmObjRefs.GroundCheck2.position, "EnemyGeneralLogic_WerewolfBossSkill");
			num = 1.5f;
			isUsingSkill = false;
			invertSpriteFacing = false;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			PlaySFXGeneric(3);
			objAN.Play("CWW-PreBarrage");
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitWerewolfBossSkill(num));
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 4f;
			}
			SetWaitToChangeSequence(0, floatGeneric);
			break;
		case 8:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			num = 0.75f;
			isUsingSkill = false;
			invertSpriteFacing = false;
			countGenericStuff++;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.Play("CWW-PrePounce");
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitWerewolfBossSkill(num));
			if (countGenericStuff < 3)
			{
				SetWaitToChangeSequence(1, 2f);
				break;
			}
			countGenericStuff = 0;
			SetWaitToChangeSequence(3, 2f);
			break;
		}
	}

	private void WEREWOLF_LeapBackwardsWhenHit()
	{
		if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f) && crtEnemyWasHitByPlayerAttack != null && CheckCollisionGround() && GC.MATH_RNG_INT(1, 100) <= 10)
		{
			sequenceOfBossSkill = 5;
			timeToTriggerNextSkill = 0f;
		}
	}

	private void WEREWOLF_PrepareToPounce()
	{
		if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomInverted, 6f, 3f) && !isUsingSkill && CheckCollisionGround())
		{
			sequenceOfBossSkill = 8;
			timeToTriggerNextSkill = 0f;
		}
	}

	private void WEREWOLF_TouchedWaypoint()
	{
		if (GC.isPlayerFainted || isGrabbing)
		{
			return;
		}
		if (sequenceOfBossSkill == 3)
		{
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			countGenericStuff = 0;
			sequenceOfBossSkill = 4;
			timeToTriggerNextSkill = 0f;
			return;
		}
		switch (GC.MATH_RNG_INT(1, 3))
		{
		default:
			SetWaitToChangeSequence(6, 0.1f);
			break;
		case 1:
			SetWaitToChangeSequence(7, 0.1f);
			break;
		case 2:
			SetWaitToChangeSequence(8, 0.1f);
			break;
		}
	}

	private void WEREWOLF_SpawnSlashProjectiles()
	{
		string specialTag = "right";
		float num = 1f;
		float num2 = 0.5f;
		Vector3 localScale = new Vector3(1f, 1f, 1f);
		Vector3 position = enmObjRefs.StartPointCenterMass.position;
		if (base.transform.localScale.x == -1f)
		{
			num *= -1f;
			localScale.x = -1f;
			specialTag = "left";
		}
		switch (countGenericStuff)
		{
		case 1:
		case 3:
			position.y += 0.5f;
			break;
		case 2:
			position.y -= 0.75f;
			break;
		}
		position.x += num;
		position.y += num2;
		GameObject gameObject = UnityEngine.Object.Instantiate(GetMiscGameObject(3), position, Quaternion.identity, GC.refOBJEffectsParent.transform);
		gameObject.GetComponent<ProjectileLogic>().specialTag = specialTag;
		if (phaseOfBossFight == 1)
		{
			gameObject.GetComponent<ProjectileLogic>().moveSpeed = 6f;
		}
		gameObject.transform.localScale = localScale;
	}

	private void WEREWOLF_SpawnMoonExplosion()
	{
		int num = 0;
		int num2 = 16;
		if (phaseOfBossFight == 1)
		{
			num2 = 26;
		}
		Transform[] array = new Transform[GetMiscGameObject(0).transform.childCount];
		foreach (Transform item in GetMiscGameObject(0).transform)
		{
			array[num] = item;
			num++;
		}
		int[] array2 = GC.MATH_RNG_TYPE_ARRAY(array, num2);
		if (array2 != null && array2.Length == num2)
		{
			int[] array3 = array2;
			foreach (int num3 in array3)
			{
				UnityEngine.Object.Instantiate(GetMiscGameObject(1), array[num3].transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
		}
		else
		{
			Debug.Log("GAMECORE:WEREWOLF_SpawnMoonExplosion -> The return array is NULL or invalid!");
		}
	}

	public void WEREWOLF_DisableAtkColl()
	{
		if (GetMiscGameObject(2).activeInHierarchy)
		{
			GetMiscGameObject(2).SetActive(value: false);
		}
	}

	private IEnumerator waitWerewolfBossSkill(float wait)
	{
		yield return new WaitForSeconds(wait);
		isUsingSkill = true;
		switch (sequenceOfBossSkill)
		{
		case 4:
			WEREWOLF_SpawnSlashProjectiles();
			PlaySFXGeneric(8);
			break;
		case 8:
			GenericExecuteJump(0.75f);
			PlaySFXGeneric(6);
			break;
		}
		crtGeneric = null;
	}

	private IEnumerator waitWerewolfLovePhase(float wait)
	{
		isUsingSkill = true;
		objAN.Play("CWW-Howl");
		PlaySFXGeneric(2);
		yield return new WaitForSeconds(wait);
		ExecuteFaintedChaseLogic(faintedChaseType.Basic, 3f);
		isUsingSkill = false;
		crtGeneric = null;
	}

	private void InvokedMinotaurBossBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		if (currentBossState != BossState.Battle && !BOSS_IS_PRE_FIGHT_MODE_COMPLETED())
		{
			return;
		}
		CheckBossEnragedPhaseChangeLogic("", 2f, playSFX: true, 2);
		CHASE_ValidateFaintedChaseFetishes();
		if (!isStatic)
		{
			BOSS_Walker_Ero_Chase_Logic_Check(0, 1);
		}
		if (!isGrabbing && !GC.isPlayerFainted)
		{
			BossSkillCheck();
		}
		if (GC.isPlayerFainted)
		{
			isUsingSkill = false;
			invertSpriteFacing = false;
			floatGeneric = 1.5f;
			if (isGrabbing)
			{
				sequenceOfBossSkill = 0;
			}
			else
			{
				sequenceOfBossSkill = 1;
			}
		}
		MINOTAUR_MovementPatterns();
		objAN.SetBool("isSkill", isUsingSkill);
	}

	private void MINOTAUR_MovementPatterns()
	{
		if ((isGrabbing || (sequenceOfBossSkill != 4 && sequenceOfBossSkill != 5 && sequenceOfBossSkill != 31 && sequenceOfBossSkill != 34)) && IsInvoking("Spawn2DSpriteTrail"))
		{
			CancelInvoke("Spawn2DSpriteTrail");
		}
		if ((isGrabbing || sequenceOfBossSkill != 34) && GetMiscGameObject(4).activeInHierarchy)
		{
			GetMiscGameObject(4).SetActive(value: false);
		}
		if ((isGrabbing || (sequenceOfBossSkill != 3 && sequenceOfBossSkill != 8 && sequenceOfBossSkill != 10 && sequenceOfBossSkill != 2)) && GetMiscGameObject(7).activeInHierarchy)
		{
			GetMiscGameObject(7).SetActive(value: false);
		}
		if (isGrabbing && (bool)ref2DShader)
		{
			ref2DShader.Toggle2DShader(stateON: false, 0);
		}
		switch (sequenceOfBossSkill)
		{
		case 0:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			if (!isGrabbing)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			if (!GC.isPlayerFainted)
			{
				MINOTAUR_CheckUltraSkills();
				MINOTAUR_CheckDefense();
			}
			break;
		case 1:
			if (CheckCollisionGround())
			{
				if (base.transform.position.x < GC.playerCurrentWorldXPosition)
				{
					enemyCurrentXMove = floatGeneric;
				}
				else
				{
					enemyCurrentXMove = 0f - floatGeneric;
				}
				if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 4f, 4f) && !isUsingSkill && !isGrabbing && !GC.isPlayerFainted && CheckCollisionGround())
				{
					sequenceOfBossSkill = 2;
					timeToTriggerNextSkill = 0f;
				}
				if (!GC.isPlayerFainted)
				{
					MINOTAUR_CheckDefense();
				}
			}
			break;
		case 3:
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			break;
		case 4:
		case 5:
			if (floatGeneric != 2.25f && sequenceOfBossSkill == 5)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			floatGeneric = 2.25f;
			if (sequenceOfBossSkill == 4)
			{
				if (base.transform.localScale.x != 1f)
				{
					enemyCurrentXMove = floatGeneric;
				}
				else
				{
					enemyCurrentXMove = 0f - floatGeneric;
				}
			}
			else if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = floatGeneric;
			}
			else
			{
				enemyCurrentXMove = 0f - floatGeneric;
			}
			if (!isUsingSkill)
			{
				enemyCurrentXMove = 0f;
				SetWaitToChangeSequence(0, 0f);
			}
			break;
		case 6:
		case 7:
			if (!isUsingSkill)
			{
				break;
			}
			if (sequenceOfBossSkill == 6)
			{
				floatGeneric = -0.75f;
			}
			else
			{
				floatGeneric = 1.75f;
			}
			if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = floatGeneric;
			}
			else
			{
				enemyCurrentXMove = 0f - floatGeneric;
			}
			if (CheckCollisionGround() && enemyCurrentYMove <= 0f)
			{
				isUsingSkill = false;
				enemyCurrentXMove = 0f;
				if (sequenceOfBossSkill == 6)
				{
					SetWaitToChangeSequence(5, 0f);
				}
				else if (GC.MATH_RNG_INT(0, 1) == 1)
				{
					SetWaitToChangeSequence(0, 0f);
				}
				else
				{
					SetWaitToChangeSequence(5, 0f);
				}
				GC.CallShakeTheCamera(0.5f, 0.5f, 1f);
				GC.PlaySFX(GameCore.SFXBasic2.GroundPoundLight, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 1.2f);
				PlaySFXGeneric(12);
			}
			break;
		case 10:
			if (isUsingSkill && enemyCurrentYMove <= 0f && CheckCollisionGround() && objAN.GetInteger("genericPhase") == 1)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.RedExplode, enmObjRefs.GroundCheck2.position, "MinotaurMovementPatterns()");
				GetMiscGameObject(5).SetActive(value: true);
				GC.CallShakeTheCamera(1f, 1.25f, 1f);
				GC.PlaySFX(GameCore.SFXBasic2.GroundPoundLonger, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 1.2f);
				SetWaitToChangeSequence(0, 0f);
				PlaySFXGeneric(8);
			}
			break;
		case 31:
			if (base.transform.position.x > 13f)
			{
				enemyCurrentXMove = 1.5f;
			}
			else
			{
				enemyCurrentXMove = -1.5f;
			}
			if (CheckCollisionGround() && enemyCurrentYMove <= 0f)
			{
				GenericExecuteJump(0.7f);
				PlaySFXGeneric(12);
			}
			break;
		case 32:
			if (enemyCurrentYMove >= 0f)
			{
				enemyCurrentYMove = -1f;
			}
			if (CheckCollisionGround() && enemyCurrentYMove <= 0f)
			{
				SetWaitToChangeSequence(30, 0f);
			}
			break;
		case 34:
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 4f;
			}
			else
			{
				floatGeneric = 3.5f;
			}
			if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = floatGeneric;
			}
			else
			{
				enemyCurrentXMove = 0f - floatGeneric;
			}
			if (!GetMiscGameObject(4).activeInHierarchy)
			{
				GetMiscGameObject(4).SetActive(value: true);
			}
			break;
		}
	}

	private void ExecuteMinotaurBossSkill()
	{
		switch (sequenceOfBossSkill)
		{
		case 0:
			isUsingSkill = false;
			invertSpriteFacing = false;
			objAN.SetInteger("genericPhase", 0);
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 1.75f;
			}
			else
			{
				floatGeneric = 1.5f;
			}
			SetWaitToChangeSequence(1, 1f);
			break;
		case 1:
			if (GC.MATH_RNG_INT(1, 100) <= 10)
			{
				SetWaitToChangeSequence(7, 0f);
				break;
			}
			isUsingSkill = false;
			invertSpriteFacing = false;
			objAN.SetInteger("genericPhase", 0);
			if (phaseOfBossFight == 1)
			{
				floatGeneric = 1.75f;
			}
			else
			{
				floatGeneric = 1.5f;
			}
			SetWaitToChangeSequence(7, 3f);
			InvokeRepeating("MINOTAUR_InvokedStepSounds", 0f, 0.6f);
			break;
		case 2:
		{
			if (GC.MATH_RNG_INT(1, 100) <= 20)
			{
				SetWaitToChangeSequence(6, 0f);
				break;
			}
			if (GC.playerIsCrouching && GC.MATH_RNG_INT(1, 100) <= 50)
			{
				SetWaitToChangeSequence(8, 0f);
				break;
			}
			float wait4 = 1.25f;
			if (phaseOfBossFight == 1)
			{
				wait4 = 0.75f;
			}
			isUsingSkill = true;
			invertSpriteFacing = false;
			objAN.SetInteger("genericPhase", 0);
			enemyCurrentXMove = 0f;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			if (GC.MATH_RNG_INT(0, 1) == 1)
			{
				objAN.Play("SetupAttack1");
			}
			else
			{
				objAN.Play("SetupAttack2");
				GetMiscGameObject(7).SetActive(value: true);
			}
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitMinotaurBossSkill(wait4));
			SetWaitToChangeSequence(0, 2f);
			countGenericStuff++;
			break;
		}
		case 3:
		{
			float wait6 = 2f;
			if (phaseOfBossFight == 1)
			{
				wait6 = 1f;
			}
			if (GC.playerIsCrouching && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f) && GC.MATH_RNG_INT(1, 100) <= 50)
			{
				SetWaitToChangeSequence(8, 0f);
				break;
			}
			isUsingSkill = true;
			invertSpriteFacing = false;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.Play("MNT-Defend");
			GetMiscGameObject(7).SetActive(value: true);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitMinotaurBossSkill(wait6));
			SetWaitToChangeSequence(0, 3f);
			countGenericStuff++;
			break;
		}
		case 4:
		case 5:
			isUsingSkill = true;
			if (sequenceOfBossSkill == 4)
			{
				invertSpriteFacing = true;
			}
			else
			{
				invertSpriteFacing = false;
			}
			objAN.Play("MNT-Attack3Shield");
			PlaySFXGeneric(13);
			PlaySFXGeneric(2);
			InvokeRepeating("Spawn2DSpriteTrail", 0f, 0.1f);
			SetWaitToChangeSequence(0, 1f);
			countGenericStuff++;
			break;
		case 6:
		case 7:
			isUsingSkill = true;
			invertSpriteFacing = false;
			if (sequenceOfBossSkill == 6)
			{
				invertSpriteFacing = true;
				GenericExecuteJump(0.5f);
			}
			else
			{
				GenericExecuteJump(0.85f);
			}
			SetWaitToChangeSequence(0, 1.5f);
			countGenericStuff++;
			PlaySFXGeneric(7);
			break;
		case 8:
		{
			float wait5 = 1.25f;
			if (phaseOfBossFight == 1)
			{
				float wait4 = 0.75f;
			}
			isUsingSkill = true;
			invertSpriteFacing = false;
			objAN.SetInteger("genericPhase", 0);
			enemyCurrentXMove = 0f;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.Play("SetupAttack4");
			GetMiscGameObject(7).SetActive(value: true);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitMinotaurBossSkill(wait5));
			SetWaitToChangeSequence(0, 2f);
			countGenericStuff++;
			break;
		}
		case 10:
		{
			float wait2 = 1f;
			if (phaseOfBossFight == 1)
			{
				wait2 = 0.5f;
			}
			isUsingSkill = true;
			invertSpriteFacing = false;
			enemyCurrentXMove = 0f;
			objAN.Play("SetupJumpExplode");
			GetMiscGameObject(7).SetActive(value: true);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitMinotaurBossSkill(wait2));
			SetWaitToChangeSequence(0, 4f);
			GC.InstantiateVFX(GameCore.VisualEffect.ChargePower_Yellow, enmObjRefs.GroundCheck2.position, "EnemyGeneralLogic_MinotaurBossSkill");
			if ((bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: true, 1);
			}
			PlaySFXGeneric(4);
			break;
		}
		case 20:
		{
			float wait = 2f;
			if (phaseOfBossFight == 1)
			{
				wait = 1f;
			}
			isUsingSkill = true;
			invertSpriteFacing = false;
			enemyCurrentXMove = 0f;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.Play("SetupEtheralSword");
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitMinotaurBossSkill(wait));
			SetWaitToChangeSequence(0, 2f);
			GC.InstantiateVFX(GameCore.VisualEffect.ChargePower_Yellow, enmObjRefs.GroundCheck2.position, "EnemyGeneralLogic_MinotaurBossSkill");
			if ((bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: true, 1);
			}
			PlaySFXGeneric(4);
			break;
		}
		case 30:
		{
			float wait3 = 2f;
			if (phaseOfBossFight == 1)
			{
				wait3 = 1f;
			}
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = -1f;
			isUsingSkill = true;
			invertSpriteFacing = false;
			STATE_MACHINE_FLIP_FACE_PLAYER();
			objAN.Play("SetupCharge");
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitMinotaurBossSkill(wait3));
			if ((bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: true, 1);
			}
			SetWaitToChangeSequence(0, 3f);
			PlaySFXGeneric(4);
			break;
		}
		case 31:
			isUsingSkill = true;
			invertSpriteFacing = false;
			InvokeRepeating("Spawn2DSpriteTrail", 0f, 0.1f);
			SetWaitToChangeSequence(30, 3f);
			break;
		case 32:
			isUsingSkill = true;
			invertSpriteFacing = false;
			SetWaitToChangeSequence(30, 2f);
			break;
		case 33:
		{
			float waitTime = 1f;
			if (sequenceOfBossSkill == 1)
			{
				waitTime = 0.25f;
			}
			isUsingSkill = true;
			invertSpriteFacing = false;
			SetWaitToChangeSequence(31, waitTime);
			GC.InstantiateVFX(GameCore.VisualEffect.ChargePower_Red, enmObjRefs.GroundCheck2.position, "EnemyGeneralLogic_MinotaurBossSkill");
			break;
		}
		case 34:
			isUsingSkill = true;
			InvokeRepeating("Spawn2DSpriteTrail", 0f, 0.1f);
			InvokeRepeating("MINOTAUR_InvokedTrail", 0f, 0.2f);
			InvokeRepeating("MINOTAUR_InvokedStepSounds", 0f, 0.1f);
			if ((bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: false, 1);
			}
			SetWaitToChangeSequence(0, 2.5f);
			break;
		}
	}

	private void MINOTAUR_CheckDefense()
	{
		if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 10f, 4f) && crtEnemyWasHitByPlayerAttack != null && CheckCollisionGround() && GC.MATH_RNG_FLOAT(1f, 100f) < 10f)
		{
			if (GC.MATH_RNG_INT(1, 100) < 66)
			{
				SetWaitToChangeSequence(3, 0f);
			}
			else
			{
				SetWaitToChangeSequence(4, 0f);
			}
		}
	}

	private int MINOTAUR_ActionAfterDefense()
	{
		if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 4f))
		{
			switch (GC.MATH_RNG_INT(0, 2))
			{
			default:
				return 8;
			case 1:
				return 2;
			case 2:
				return 4;
			}
		}
		if (GC.MATH_RNG_INT(0, 1) == 1)
		{
			return 6;
		}
		return 7;
	}

	private void MINOTAUR_CheckUltraSkills()
	{
		if (((countGenericStuff != 3 || phaseOfBossFight != 1) && countGenericStuff < 4) || isUsingSkill)
		{
			return;
		}
		invertSpriteFacing = false;
		countGenericStuff = 0;
		enemyCurrentXMove = 0f;
		objAN.SetInteger("genericPhase", 0);
		int num = 0;
		int num2 = 10;
		do
		{
			switch (GC.MATH_RNG_INT(0, 2))
			{
			default:
				num2 = 10;
				break;
			case 1:
				num2 = 20;
				break;
			case 2:
				num2 = 33;
				break;
			}
			num++;
		}
		while (num < 10 && num2 == intGeneric);
		intGeneric = num2;
		SetWaitToChangeSequence(num2, 0f);
	}

	private void MINOTAUR_TouchedWaypoint()
	{
		if (!GC.isPlayerFainted && !isGrabbing)
		{
			switch (sequenceOfBossSkill)
			{
			case 31:
				SetWaitToChangeSequence(32, 0f);
				break;
			case 34:
				GC.CallShakeTheCamera(1f, 1f, 1f);
				SetWaitToChangeSequence(0, 0f);
				PlaySFXGeneric(8);
				break;
			}
		}
	}

	private void MINOTAUR_InvokedTrail()
	{
		if (sequenceOfBossSkill != 34)
		{
			CancelInvoke("MINOTAUR_InvokedTrail");
		}
		UnityEngine.Object.Instantiate(GetMiscGameObject(8), enmObjRefs.GroundCheck2.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
	}

	private void MINOTAUR_InvokedStepSounds()
	{
		if (sequenceOfBossSkill != 1 && sequenceOfBossSkill != 34)
		{
			CancelInvoke("MINOTAUR_InvokedStepSounds");
		}
		PlaySFXGeneric(100, 0.85f, 1.15f);
	}

	private IEnumerator waitMinotaurBossSkill(float wait)
	{
		yield return new WaitForSeconds(wait);
		switch (sequenceOfBossSkill)
		{
		case 2:
		case 8:
		{
			float num = 2.5f;
			if (phaseOfBossFight == 1)
			{
				num = 3.25f;
			}
			objAN.SetInteger("genericPhase", 1);
			if (base.transform.localScale.x == 1f)
			{
				enemyCurrentXMove = num;
			}
			else
			{
				enemyCurrentXMove = 0f - num;
			}
			PlaySFXGeneric(1);
			break;
		}
		case 3:
			isUsingSkill = false;
			invertSpriteFacing = false;
			SetWaitToChangeSequence(MINOTAUR_ActionAfterDefense(), 0f);
			break;
		case 10:
		{
			float jumpForceMultiplier = 1f;
			if (phaseOfBossFight == 1)
			{
				jumpForceMultiplier = 0.75f;
			}
			GenericExecuteJump(jumpForceMultiplier);
			objAN.SetInteger("genericPhase", 1);
			if ((bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: false, 1);
			}
			break;
		}
		case 20:
		{
			objAN.SetInteger("genericPhase", 1);
			Vector2 vector = enmObjRefs.StartPointCenterMass.position;
			if (phaseOfBossFight == 1)
			{
				vector.y -= 0.25f;
			}
			else
			{
				vector.y += 0.25f;
			}
			ProjectileLogic component = UnityEngine.Object.Instantiate(GetMiscGameObject(0), vector, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>();
			if (base.transform.localScale.x == -1f)
			{
				component.specialForce.x = -1f;
			}
			else
			{
				component.specialForce.x = 1f;
			}
			if ((bool)ref2DShader)
			{
				ref2DShader.Toggle2DShader(stateON: false, 1);
			}
			PlaySFXGeneric(2);
			break;
		}
		case 30:
			objAN.SetInteger("genericPhase", 1);
			SetWaitToChangeSequence(34, 0f);
			break;
		}
		crtGeneric = null;
	}

	private void InvokedPrincessBossBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		if (currentBossState == BossState.Battle || BOSS_IS_PRE_FIGHT_MODE_COMPLETED())
		{
			CheckBossEnragedPhaseChangeLogic("", 2f, playSFX: true, 2);
			if (isUsingSkill && GC.isPlayerFainted)
			{
				SetWaitToChangeSequence(0, 0f);
				BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
				sequenceOfBossSkill = 0;
			}
			if (!isGrabbing && !GC.isPlayerFainted)
			{
				BossSkillCheck();
			}
			if (BOSS_FLYER_NOT_CHASING())
			{
				PRINCESS_MovementPatterns();
			}
			objAN.SetBool("isSkill", isUsingSkill);
		}
	}

	private void PRINCESS_MovementPatterns()
	{
		switch (sequenceOfBossSkill)
		{
		case 0:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			if (!isGrabbing)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			if (base.transform.position.y < 5.5f)
			{
				enemyCurrentYMove = enmBaseStats.speedMoveY;
			}
			else
			{
				enemyCurrentYMove = 0f;
			}
			break;
		case 1:
		case 2:
		case 5:
			floatGeneric = enmBaseStats.speedMoveX;
			if (boolGeneric)
			{
				enemyCurrentXMove = floatGeneric;
			}
			else
			{
				enemyCurrentXMove = 0f - floatGeneric;
			}
			floatGeneric = enmBaseStats.speedMoveY / 20f;
			if (intGeneric == 1)
			{
				enemyCurrentYMove += floatGeneric;
				if (enemyCurrentYMove >= 0.5f)
				{
					intGeneric = 0;
				}
			}
			else
			{
				enemyCurrentYMove -= floatGeneric;
				if (enemyCurrentYMove <= -0.5f)
				{
					intGeneric = 1;
				}
			}
			if (enemyCurrentXMove > 0f && base.transform.position.x > GC.playerCurrentWorldXPosition)
			{
				invertSpriteFacing = true;
			}
			else if (enemyCurrentXMove < 0f && base.transform.position.x < GC.playerCurrentWorldXPosition)
			{
				invertSpriteFacing = true;
			}
			else
			{
				invertSpriteFacing = false;
			}
			break;
		case 3:
			if (base.transform.position.x > 14.5f)
			{
				enemyCurrentXMove = -1f;
			}
			else if (base.transform.position.x < 12.5f)
			{
				enemyCurrentXMove = 1f;
			}
			else
			{
				enemyCurrentXMove = 0f;
			}
			if (base.transform.position.y > 4.5f)
			{
				enemyCurrentYMove = -1f;
			}
			else if (base.transform.position.y < 2.5f)
			{
				enemyCurrentYMove = 1f;
			}
			else
			{
				enemyCurrentYMove = 0f;
			}
			if (enemyCurrentXMove == 0f && enemyCurrentYMove == 0f)
			{
				SetWaitToChangeSequence(4, 0f);
			}
			break;
		case 4:
		case 6:
		case 8:
		case 10:
			enemyCurrentXMove = 0f;
			enemyCurrentYMove = 0f;
			if (sequenceOfBossSkill == 6)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			break;
		case 7:
			if (base.transform.position.y < 7f)
			{
				enemyCurrentYMove = enmBaseStats.speedMoveY;
			}
			else
			{
				enemyCurrentYMove = 0f;
			}
			break;
		case 9:
			if (base.transform.position.x > 14.5f)
			{
				enemyCurrentXMove = -1f;
			}
			else if (base.transform.position.x < 12.5f)
			{
				enemyCurrentXMove = 1f;
			}
			else
			{
				enemyCurrentXMove = 0f;
			}
			if (base.transform.position.y > 1f)
			{
				enemyCurrentYMove = 0f - enmBaseStats.speedMoveY;
			}
			else
			{
				enemyCurrentYMove = 0f;
			}
			break;
		case 99:
			posGenericVector = (new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, base.transform.position.z) - base.transform.position).normalized;
			enemyCurrentXMove = posGenericVector.x;
			enemyCurrentYMove = posGenericVector.y;
			if (localSpeedX <= enmBaseStats.speedMoveX)
			{
				localSpeedX *= enmMisc.flyerChaseMultiplier;
				localSpeedY *= enmMisc.flyerChaseMultiplier;
			}
			if (isGrabbing)
			{
				SetWaitToChangeSequence(100, 0f);
				sequenceOfBossSkill = 100;
			}
			break;
		}
	}

	private void ExecutePrincessBossSkill()
	{
		if (sequenceOfBossSkill != 4 && sequenceOfBossSkill != 8 && currentDefense != enmBaseStats.baseDefense)
		{
			currentDefense = enmBaseStats.baseDefense;
		}
		switch (sequenceOfBossSkill)
		{
		case 0:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(1, 1f);
			intGeneric = GC.MATH_RNG_INT(0, 1);
			if (GC.MATH_RNG_INT(1, 100) > 50)
			{
				boolGeneric = true;
			}
			else
			{
				boolGeneric = false;
			}
			break;
		case 1:
		{
			int num2 = 3;
			float waitTime2 = 3f;
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			if (phaseOfBossFight == 1)
			{
				num2 = 6;
				waitTime2 = 1.5f;
			}
			if (countGenericStuff < num2)
			{
				SetWaitToChangeSequence(2, waitTime2);
			}
			else
			{
				SetWaitToChangeSequence(3, 0.5f);
			}
			intGeneric = GC.MATH_RNG_INT(0, 1);
			break;
		}
		case 2:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 1);
			countGenericStuff++;
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(PRINCESS_WaitDoThings(1, 0.5f));
			SetWaitToChangeSequence(1, 1f);
			GC.PlaySFXGeneric("PrincessBoss", 5, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.05f, "voice");
			break;
		case 3:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(4, 3f);
			break;
		case 4:
		{
			float repeatRate2 = 1f;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 2, doResetCountGenericStuff: true);
			if (phaseOfBossFight == 1)
			{
				repeatRate2 = 0.5f;
			}
			InvokeRepeating("PRINCESS_InvokedSpawnIceBoulders", 0f, repeatRate2);
			if (currentDefense != enmBaseStats.specialDefense)
			{
				currentDefense = enmBaseStats.specialDefense;
			}
			SetWaitToChangeSequence(5, 6f);
			intGeneric = GC.MATH_RNG_INT(0, 1);
			if (GC.MATH_RNG_INT(1, 100) > 50)
			{
				boolGeneric = true;
			}
			else
			{
				boolGeneric = false;
			}
			GC.PlaySFXGeneric("PrincessBoss", 5, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.05f, "voice");
			break;
		}
		case 5:
		{
			float waitTime = 4f;
			int num = 2;
			if (phaseOfBossFight == 1)
			{
				num = 4;
				waitTime = 2f;
			}
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			if (countGenericStuff < num)
			{
				SetWaitToChangeSequence(6, waitTime);
			}
			else
			{
				SetWaitToChangeSequence(7, 0f);
			}
			break;
		}
		case 6:
		{
			float repeatRate = 0.33f;
			float num3 = 1.75f;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 1);
			countGenericStuff++;
			InvokeRepeating("PRINCESS_InvokedSpawnIceShoots", 1f, repeatRate);
			if (phaseOfBossFight == 1)
			{
				num3 *= 2f;
			}
			SetWaitToChangeSequence(5, num3);
			GC.PlaySFXGeneric("PrincessBoss", 5, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.05f, "voice");
			break;
		}
		case 7:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(8, 2f);
			break;
		case 8:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 2, doResetCountGenericStuff: true);
			if (phaseOfBossFight == 1)
			{
				InvokeRepeating("PRINCESS_InvokedSpawnOwl", 0f, 0.5f);
			}
			else
			{
				InvokeRepeating("PRINCESS_InvokedSpawnIceRain", 0f, 3.5f);
			}
			if (currentDefense != enmBaseStats.specialDefense)
			{
				currentDefense = enmBaseStats.specialDefense;
			}
			SetWaitToChangeSequence(9, 11f);
			STATE_MACHINE_FLIP_FACE_PLAYER();
			GC.PlaySFXGeneric("PrincessBoss", 5, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.05f, "voice");
			break;
		case 9:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(10, 3f);
			break;
		case 10:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 3, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(0, 6f);
			break;
		}
	}

	private void PRINCESS_TouchedWaypoint()
	{
		if (!GC.isPlayerFainted && !isGrabbing)
		{
			switch (sequenceOfBossSkill)
			{
			case 1:
			case 2:
			case 6:
				boolGeneric = !boolGeneric;
				break;
			case 5:
				boolGeneric = !boolGeneric;
				SetWaitToChangeSequence(6, 0f);
				break;
			case 3:
			case 4:
				break;
			}
		}
	}

	private void PRINCESS_CheckFartestCorner()
	{
		float num = base.transform.position.x - GetMiscGameObject(0).transform.position.x;
		float num2 = base.transform.position.x - GetMiscGameObject(1).transform.position.x;
		if (num < 0f)
		{
			num *= -1f;
		}
		if (num2 < 0f)
		{
			num2 *= -1f;
		}
		if (num > num2)
		{
			boolGeneric = false;
		}
		else
		{
			boolGeneric = true;
		}
	}

	private void PRINCESS_InvokedSpawnIceBoulders()
	{
		if (sequenceOfBossSkill != 4)
		{
			CancelInvoke("PRINCESS_InvokedSpawnIceBoulders");
			return;
		}
		Vector3 zero = Vector3.zero;
		int num = 0;
		int num2 = 0;
		int num3 = 1;
		num = GetMiscGameObject(5).transform.childCount + GetMiscGameObject(6).transform.childCount;
		Transform[] array = new Transform[num];
		foreach (Transform item in GetMiscGameObject(5).transform)
		{
			array[num2] = item;
			num2++;
		}
		foreach (Transform item2 in GetMiscGameObject(6).transform)
		{
			array[num2] = item2;
			num2++;
		}
		zero = array[GC.MATH_RNG_INT(0, num - 1)].position;
		GameObject obj = UnityEngine.Object.Instantiate(GetMiscGameObject(3), zero, Quaternion.identity, GC.refOBJEffectsParent.transform);
		if (zero.x > base.transform.position.x)
		{
			num3 = -1;
		}
		obj.GetComponent<ProjectileLogic>().specialForce.x = num3;
		GC.InstantiateVFX(GameCore.VisualEffect.IceSmoke1, zero, "EnemyGeneralLogic:InvokedPrincessSpawnBoudler");
	}

	private void PRINCESS_InvokedSpawnIceShoots()
	{
		if (sequenceOfBossSkill != 6)
		{
			CancelInvoke("PRINCESS_InvokedSpawnIceShoots");
			return;
		}
		Vector2 vector = base.transform.position;
		if (base.transform.localScale.x == 1f)
		{
			vector.x += 0.75f;
		}
		else
		{
			vector.x -= 0.75f;
		}
		BasicProjectileInstantiate(GetMiscGameObject(8), overrideSpawnManual: true, vector.x, vector.y);
	}

	private void PRINCESS_InvokedSpawnIceRain()
	{
		if (sequenceOfBossSkill != 8)
		{
			CancelInvoke("PRINCESS_InvokedSpawnIceRain");
			return;
		}
		Vector2 zero = Vector2.zero;
		int num = 0;
		Transform[] array = new Transform[GetMiscGameObject(4).transform.childCount];
		foreach (Transform item in GetMiscGameObject(4).transform)
		{
			array[num] = item;
			num++;
		}
		int[] array2 = GC.MATH_RNG_TYPE_ARRAY(array, array.Length - 1);
		if (array2 != null)
		{
			for (int i = 0; i < array2.Length; i++)
			{
				zero = array[array2[i]].position;
				BasicProjectileInstantiate(GetMiscGameObject(9), overrideSpawnManual: true, zero.x, zero.y);
			}
		}
	}

	private void PRINCESS_InvokedSpawnOwl()
	{
		if (sequenceOfBossSkill != 8)
		{
			CancelInvoke("PRINCESS_InvokedSpawnOwl");
			return;
		}
		Vector3 zero = Vector3.zero;
		int num = 0;
		int num2 = 0;
		num = GetMiscGameObject(4).transform.childCount;
		Transform[] array = new Transform[num];
		foreach (Transform item in GetMiscGameObject(4).transform)
		{
			array[num2] = item;
			num2++;
		}
		zero = array[GC.MATH_RNG_INT(0, num - 1)].position;
		UnityEngine.Object.Instantiate(GetMiscGameObject(7), zero, Quaternion.identity, GC.refOBJEffectsParent.transform);
		GC.InstantiateVFX(GameCore.VisualEffect.IceSmoke1, zero, "EnemyGeneralLogic:PRINCESS_InvokedSpawnOwl");
	}

	private IEnumerator PRINCESS_WaitDoThings(int idThing, float wait)
	{
		yield return new WaitForSeconds(wait);
		if (idThing == 1)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.IceSmoke1, base.transform.position, "EnemyGeneralLogic");
			BasicProjectileInstantiate(GetMiscGameObject(2));
		}
	}

	private void InvokedWitchBossBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		if (currentBossState != BossState.Battle && !BOSS_IS_PRE_FIGHT_MODE_COMPLETED())
		{
			return;
		}
		CheckBossEnragedPhaseChangeLogic("", 2f, playSFX: true, 2);
		if (isUsingSkill && GC.isPlayerFainted && objAN.GetInteger("genericPhase") != 1 && objAN.GetInteger("genericPhase") != 2)
		{
			SetWaitToChangeSequence(0, 0f);
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			sequenceOfBossSkill = 0;
		}
		if (!WITCH_CHARMCHECK())
		{
			if (!isGrabbing && !GC.isPlayerFainted)
			{
				BossSkillCheck();
			}
			if (!isGrabbing && GC.isPlayerFainted && !GC.playerIsBeingGrabed)
			{
				WITCH_FAINTED_CHASE_LOGIC();
			}
			WITCH_MovementPatterns();
			objAN.SetBool("isSkill", isUsingSkill);
		}
	}

	private void WITCH_MovementPatterns()
	{
		if (!WITCH_SPECIALDEFENSE() && currentDefense != enmBaseStats.baseDefense)
		{
			currentDefense = enmBaseStats.baseDefense;
		}
		_ = sequenceOfBossSkill;
		if (!isGrabbing && sequenceOfBossSkill != 41)
		{
			STATE_MACHINE_FLIP_FACE_PLAYER();
		}
	}

	private void WITCH_ExecuteSkills()
	{
		float waitTime = 3f;
		if (phaseOfBossFight == 1)
		{
			waitTime = 2f;
		}
		switch (sequenceOfBossSkill)
		{
		case 0:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(WITCH_GETPHASEBYCONTEXT(), waitTime);
			break;
		case 1:
		case 2:
		case 3:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 1, doResetCountGenericStuff: true);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(WITCH_WaitToDoThing("TeleportStart", 1.5f));
			SetWaitToChangeSequence(0, 10f);
			if (!GC.isPlayerFainted)
			{
				PlaySFXGeneric(27, 0f, 0f);
			}
			break;
		case 10:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			SetWaitToChangeSequence(WITCH_GETPHASEBYCONTEXT(), waitTime);
			break;
		case 11:
			countGenericStuff++;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 3);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(WITCH_WaitToDoThing("SpawnCloud", 2f));
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch1, enmObjRefs.GroundCheck2.position, "EnemyLogicWitch");
			SetWaitToChangeSequence(10, 2f);
			PlaySFXGeneric(26, 0f, 0f);
			if (phaseOfBossFight == 1)
			{
				WITCH_ENRAGED_SPELLS(0);
				WITCH_ENRAGED_SPELLS(0);
				WITCH_ENRAGED_SPELLS(0);
			}
			break;
		case 12:
		{
			float num = 2f;
			countGenericStuff++;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 4);
			for (int i = 0; i < 6; i++)
			{
				StartCoroutine(WITCH_WaitToDoThing("SpawnProjectilesErupt", num));
				num += 0.25f;
			}
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch2, enmObjRefs.GroundCheck2.position, "EnemyLogicWitch");
			SetWaitToChangeSequence(10, 3.5f);
			PlaySFXGeneric(26, 0f, 0f);
			break;
		}
		case 20:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			SetWaitToChangeSequence(WITCH_GETPHASEBYCONTEXT(), waitTime);
			break;
		case 21:
			countGenericStuff++;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 5);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(WITCH_WaitToDoThing("SpawnCircleSummon", 2f));
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch3, enmObjRefs.GroundCheck2.position, "EnemyLogicWitch");
			SetWaitToChangeSequence(20, 3f);
			PlaySFXGeneric(26, 0f, 0f);
			if (phaseOfBossFight == 1)
			{
				WITCH_ENRAGED_SPELLS(1);
			}
			break;
		case 22:
			countGenericStuff++;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 4);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(WITCH_WaitToDoThing("SpawnHorizontalTornado", 2f));
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch4, enmObjRefs.GroundCheck2.position, "EnemyLogicWitch");
			SetWaitToChangeSequence(20, 3f);
			PlaySFXGeneric(26, 0f, 0f);
			if (phaseOfBossFight == 1)
			{
				WITCH_ENRAGED_SPELLS(0);
			}
			break;
		case 30:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			SetWaitToChangeSequence(WITCH_GETPHASEBYCONTEXT(), waitTime);
			break;
		case 31:
			countGenericStuff++;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 5);
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(WITCH_WaitToDoThing("initiateWallBlocks", 2f));
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch5, enmObjRefs.GroundCheck2.position, "EnemyLogicWitch");
			SetWaitToChangeSequence(30, 15f);
			PlaySFXGeneric(26, 0f, 0f);
			if (phaseOfBossFight == 1)
			{
				WITCH_ENRAGED_SPELLS(2);
			}
			break;
		case 32:
			countGenericStuff++;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 3);
			StartCoroutine(WITCH_WaitToDoThing("SpawnEnemy", 0f));
			StartCoroutine(WITCH_WaitToDoThing("SpawnEnemy", 1f));
			StartCoroutine(WITCH_WaitToDoThing("SpawnEnemy", 2f));
			StartCoroutine(WITCH_WaitToDoThing("SpawnEnemy", 3f));
			StartCoroutine(WITCH_WaitToDoThing("SpawnEnemy", 4f));
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch6, enmObjRefs.GroundCheck2.position, "EnemyLogicWitch");
			SetWaitToChangeSequence(30, 6f);
			PlaySFXGeneric(26, 0f, 0f);
			if (phaseOfBossFight == 1)
			{
				WITCH_ENRAGED_SPELLS(0);
			}
			break;
		case 41:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 41);
			WITCH_EXECUTECUSTOM("StartIllusion");
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(WITCH_WaitToDoThing("ApplyIllusion", 3f));
			PlaySFXGeneric(10, 0f, 0f);
			SetWaitToChangeSequence(WITCH_GETPHASEBYCONTEXT(), 10f);
			break;
		case 51:
		{
			Vector2 targetPosition = WITCH_GETPOSITIONARRAY()[5].transform.position;
			targetPosition.y -= 1.5f;
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch6, targetPosition, "EnemyLogicWitch");
			if (base.transform.position.x > 0f || base.transform.position.x < -1f || base.transform.position.y > 0f)
			{
				BossBasicSkillToggles(usingSkill: true, invertFacing: false, 1);
				if (crtGeneric != null)
				{
					StopCoroutine(crtGeneric);
				}
				crtGeneric = StartCoroutine(WITCH_WaitToDoThing("TeleportStart", 0.5f));
				SetWaitToChangeSequence(0, 10f);
			}
			else
			{
				SetWaitToChangeSequence(52, 0.5f);
			}
			break;
		}
		case 52:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 52, doResetCountGenericStuff: true);
			StartCoroutine(WITCH_WaitToDoThing("BoobTeaseHearts", 2f));
			StartCoroutine(WITCH_WaitToDoThing("BoobTeaseHearts", 5f));
			StartCoroutine(WITCH_WaitToDoThing("BoobTeaseHearts", 8f));
			StartCoroutine(WITCH_WaitToDoThing("BoobTeaseHearts", 11f));
			SetWaitToChangeSequence(0, 12f);
			PlaySFXGeneric(10, 0f, 0f);
			break;
		}
	}

	private void WITCH_ENRAGED_SPELLS(int spellID)
	{
		Vector2 vector = base.transform.position;
		switch (spellID)
		{
		default:
			vector.x += GC.MATH_RNG_FLOAT(-2f, 2f);
			vector.y += GC.MATH_RNG_FLOAT(-2f, 2f);
			BasicProjectileInstantiate(GetMiscGameObject(1), overrideSpawnManual: true, vector.x, vector.y);
			break;
		case 1:
			vector.x += GC.MATH_RNG_FLOAT(-0.5f, 0.5f);
			vector.y += GC.MATH_RNG_FLOAT(-0.5f, 0.5f);
			BasicProjectileInstantiate(GetMiscGameObject(2), overrideSpawnManual: true, vector.x, vector.y);
			BasicProjectileInstantiate(GetMiscGameObject(2), overrideSpawnManual: true, vector.x, vector.y);
			BasicProjectileInstantiate(GetMiscGameObject(2), overrideSpawnManual: true, vector.x, vector.y);
			break;
		case 2:
			vector = new Vector2(-0.5f, 4f);
			vector.y += GC.MATH_RNG_FLOAT(-2f, 2f);
			BasicProjectileInstantiate(GetMiscGameObject(3), overrideSpawnManual: true, vector.x, vector.y);
			break;
		}
	}

	public void WITCH_EXECUTECUSTOM(string action)
	{
		switch (action)
		{
		case "TeleportStart":
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 2);
			GC.InstantiateVFX(GameCore.VisualEffect.StarsINWARD, base.transform.position, "EnemyGeneralLogic:WITCHBOSS");
			PlaySFXGeneric(24, 0f, 0f);
			break;
		case "TeleportApply":
			WITCH_TeleportApplyLogic();
			break;
		case "TeleportWait":
		{
			base.transform.position = posGenericVector;
			objBox2D.enabled = true;
			objRB.bodyType = RigidbodyType2D.Dynamic;
			objAN.Play("Witch-Skill2-C");
			Color color2 = objRE.color;
			color2.a = 1f;
			objRE.color = color2;
			GC.InstantiateVFX(GameCore.VisualEffect.StarsOUTWARD, base.transform.position, "EnemyGeneralLogic:WITCHBOSS");
			PlaySFXGeneric(25, 0f, 0f);
			break;
		}
		case "TeleportEnd":
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			SetWaitToChangeSequence(WITCH_GETPHASEBYCONTEXT(), 0f);
			break;
		case "SpawnCloud":
		{
			Vector2 vector4 = base.transform.position;
			vector4.y++;
			BasicProjectileInstantiate(GetMiscGameObject(1), overrideSpawnManual: true, vector4.x, vector4.y);
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			break;
		}
		case "SpawnProjectilesErupt":
			BasicProjectileInstantiate(GetMiscGameObject(2), overrideSpawnManual: true, enmObjRefs.StartPointCenterMass.position.x, enmObjRefs.StartPointCenterMass.position.y);
			break;
		case "SpawnCircleSummon":
		{
			Vector2 vector3 = new Vector2(-0.5f, 4f);
			vector3.y += GC.MATH_RNG_FLOAT(-2f, 2f);
			BasicProjectileInstantiate(GetMiscGameObject(3), overrideSpawnManual: true, vector3.x, vector3.y);
			break;
		}
		case "SpawnHorizontalTornado":
			BasicProjectileInstantiate(GetMiscGameObject(4), overrideSpawnManual: true, base.transform.position.x, base.transform.position.y);
			break;
		case "initiateWallBlocks":
		{
			float num = 0f;
			for (int i = 0; i < 6; i++)
			{
				StartCoroutine(WITCH_WaitToDoThing("SpawnWallBlocks", num));
				num += 2f;
			}
			break;
		}
		case "SpawnWallBlocks":
		{
			int num2 = 6;
			int num3 = 0;
			if (base.transform.position.x < 0f)
			{
				num2 = 5;
			}
			Transform[] array7 = new Transform[GetMiscGameObject(num2).transform.childCount];
			foreach (Transform item in GetMiscGameObject(num2).transform)
			{
				array7[num3] = item;
				num3++;
			}
			int[] array8 = GC.MATH_RNG_TYPE_ARRAY(array7, 1);
			Vector2 vector2 = array7[array8[0]].position;
			GameObject gameObject = UnityEngine.Object.Instantiate(GetMiscGameObject(7), vector2, Quaternion.identity, GC.refOBJEffectsParent.transform);
			if (num2 == 5)
			{
				gameObject.GetComponent<ProjectileLogic>().specialTag = "right";
			}
			else
			{
				gameObject.GetComponent<ProjectileLogic>().specialTag = "left";
			}
			Physics2D.IgnoreCollision(objBox2D, gameObject.GetComponent<CapsuleCollider2D>());
			break;
		}
		case "SpawnEnemy":
		{
			Transform[] array5 = WITCH_GETPOSITIONARRAY();
			int[] array6 = GC.MATH_RNG_TYPE_ARRAY(array5, 1);
			Vector2 vector = array5[array6[0]].position;
			vector.y -= 1.5f;
			UnityEngine.Object.Instantiate(GetMiscGameObject(8), vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
			break;
		}
		case "StartIllusion":
		{
			Transform[] array3 = WITCH_GETPOSITIONARRAY();
			Vector2[] array4 = new Vector2[4];
			for (int j = 0; j < array4.Length; j++)
			{
				array4[j] = array3[j].position;
				array4[j].y -= 1.5f;
			}
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch0, array4[0], "EnemyGeneralLogic:WitchBoss");
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch0, array4[1], "EnemyGeneralLogic:WitchBoss");
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch0, array4[2], "EnemyGeneralLogic:WitchBoss");
			GC.InstantiateVFX(GameCore.VisualEffect.CastingWitch0, array4[3], "EnemyGeneralLogic:WitchBoss");
			break;
		}
		case "ApplyIllusion":
		{
			Transform[] array = WITCH_GETPOSITIONARRAY();
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, array[0].position, "EnemyGeneralLogic:WitchBoss");
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, array[1].position, "EnemyGeneralLogic:WitchBoss");
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, array[2].position, "EnemyGeneralLogic:WitchBoss");
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, array[3].position, "EnemyGeneralLogic:WitchBoss");
			int[] array2 = GC.MATH_RNG_INT_ARRAY_INDEX_SCRAMBLE(4);
			objAN.Play("Basic-Idle-Illusion");
			base.transform.position = array[array2[0]].position;
			UnityEngine.Object.Instantiate(GetMiscGameObject(9), array[array2[1]].position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			UnityEngine.Object.Instantiate(GetMiscGameObject(9), array[array2[2]].position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			UnityEngine.Object.Instantiate(GetMiscGameObject(9), array[array2[3]].position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			objBox2D.enabled = true;
			objRB.bodyType = RigidbodyType2D.Dynamic;
			Color color = objRE.color;
			color.a = 1f;
			objRE.color = color;
			if (base.transform.position.x > 0f)
			{
				STATE_MACHINE_FLIP_SELF(forcedLeft: true);
			}
			else
			{
				STATE_MACHINE_FLIP_SELF(forcedLeft: false, forcedRight: true);
			}
			break;
		}
		case "BoobTeaseHearts":
			UnityEngine.Object.Instantiate(GetMiscGameObject(10), base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			PlaySFXGeneric(29, 0f, 0f);
			break;
		}
	}

	private int WITCH_GETPHASEBYCONTEXT()
	{
		switch (sequenceOfBossSkill)
		{
		case 0:
			return 1;
		case 1:
			return 10;
		case 2:
			return 20;
		case 3:
			return 30;
		case 10:
			if (countGenericStuff < 2)
			{
				if (phaseOfBossFight == 1)
				{
					return WITCH_RANDOMALLSPELLS();
				}
				if (GC.MATH_RNG_FLOAT(1f, 100f) <= 50f)
				{
					return 11;
				}
				return 12;
			}
			return 2;
		case 20:
			if (countGenericStuff < 2)
			{
				if (phaseOfBossFight == 1)
				{
					return WITCH_RANDOMALLSPELLS();
				}
				if (GC.MATH_RNG_FLOAT(1f, 100f) <= 50f)
				{
					return 21;
				}
				return 22;
			}
			if (phaseOfBossFight == 1)
			{
				return WITCH_RANDOMALLSPELLS();
			}
			return 3;
		case 30:
			if (countGenericStuff < 2)
			{
				if (phaseOfBossFight == 1)
				{
					return WITCH_RANDOMALLSPELLS();
				}
				if (GC.MATH_RNG_FLOAT(1f, 100f) <= 50f)
				{
					return 31;
				}
				return 32;
			}
			return 51;
		case 41:
			return countGenericStuff;
		case 51:
			return 52;
		default:
			return 0;
		}
	}

	private void WITCH_FAINTED_CHASE_LOGIC()
	{
		if (!isUsingSkill && coroutEroGracePeriod == null && crtGeneric == null)
		{
			StopAllCoroutines();
			SetWaitToChangeSequence(999999, 0f);
			sequenceOfBossSkill = 999999;
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 1, doResetCountGenericStuff: true);
			WITCH_EXECUTECUSTOM("TeleportStart");
		}
	}

	private bool WITCH_CHARMCHECK()
	{
		STATE_MACHINE_FLIP_FACE_PLAYER();
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Charmed) && !isUsingSkill && !GC.isPlayerFainted && !isGrabbing)
		{
			if (objAN.GetInteger("genericPhase") != 99)
			{
				objAN.SetInteger("genericPhase", 99);
				return true;
			}
			return true;
		}
		if (!GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Charmed) && objAN.GetInteger("genericPhase") == 99)
		{
			objAN.SetInteger("genericPhase", 0);
		}
		return false;
	}

	private Transform[] WITCH_GETPOSITIONARRAY()
	{
		int num = 0;
		Transform[] array = new Transform[GetMiscGameObject(0).transform.childCount];
		foreach (Transform item in GetMiscGameObject(0).transform)
		{
			array[num] = item;
			num++;
		}
		return array;
	}

	private void WITCH_TeleportApplyLogic()
	{
		objAN.Play("Skill2Wait");
		Color color = objRE.color;
		color.a = 0f;
		objRE.color = color;
		objBox2D.enabled = false;
		objRB.bodyType = RigidbodyType2D.Static;
		if (!GC.isPlayerFainted && floatGeneric >= 30f && GC.MATH_RNG_FLOAT(1f, 100f) <= floatGeneric && sequenceOfBossSkill != 51 && sequenceOfBossSkill != 52)
		{
			countGenericStuff = sequenceOfBossSkill;
			floatGeneric = 0f;
			SetWaitToChangeSequence(41, 0.1f);
			return;
		}
		floatGeneric += 15f;
		int num = 0;
		Transform[] array = new Transform[GetMiscGameObject(0).transform.childCount];
		foreach (Transform item in GetMiscGameObject(0).transform)
		{
			array[num] = item;
			num++;
		}
		Transform transform2 = array[0];
		num = 0;
		if (!GC.isPlayerFainted || (GC.isPlayerFainted && WITCH_NEARPLAYER(base.transform.position)))
		{
			do
			{
				num++;
				int[] array2 = GC.MATH_RNG_TYPE_ARRAY(array, 1);
				transform2 = array[array2[0]];
			}
			while (WITCH_NEARPLAYER(transform2.position) || GC.CheckProximityBetweenTwoPositions(transform2.position, base.transform.position, 1f, 1f) || num < 10);
		}
		if (crtGeneric != null)
		{
			StopCoroutine(crtGeneric);
		}
		crtGeneric = StartCoroutine(WITCH_WaitToDoThing("TeleportWait", 1f));
		if (sequenceOfBossSkill == 51)
		{
			transform2 = array[5];
		}
		posGenericVector = transform2.position;
		if (GC.isPlayerFainted && !WITCH_NEARPLAYER(base.transform.position))
		{
			posGenericVector.x = GC.playerCurrentWorldXPosition;
			posGenericVector.y = GC.playerCurrentWorldYPosition;
		}
	}

	private bool WITCH_SPECIALDEFENSE()
	{
		if (objAN.GetCurrentAnimatorStateInfo(0).IsName("Witch-Skill2-B") || objAN.GetCurrentAnimatorStateInfo(0).IsName("Witch-Skill2-C"))
		{
			currentDefense = enmBaseStats.specialDefense;
			return true;
		}
		return false;
	}

	private void WITCH_HURTCORRECTLYDURINGILLUSION()
	{
		PlaySFXGeneric(28, 0f, 0f);
		objAN.Play("Illusion-Angry");
		GameObject[] array = GameObject.FindGameObjectsWithTag("Projectile-Basic");
		if (array == null || array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Contains("WitchIllusion"))
			{
				GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, array[i].transform.position, "");
				array[i].SetActive(value: false);
			}
		}
	}

	private bool WITCH_NEARPLAYER(Vector3 positionOfReference)
	{
		if (GC.CheckPlayerProximityToAnyObject(positionOfReference, GameCore.PlayerProximityType.Custom, 4f, 4f))
		{
			return true;
		}
		return false;
	}

	private int WITCH_RANDOMALLSPELLS()
	{
		int num = GC.MATH_RNG_INT(1, 120);
		if (num >= 1 && num <= 20)
		{
			return 11;
		}
		if (num >= 21 && num <= 40)
		{
			return 12;
		}
		if (num >= 41 && num <= 60)
		{
			return 21;
		}
		if (num >= 61 && num <= 80)
		{
			return 22;
		}
		if (num >= 81 && num <= 100)
		{
			return 31;
		}
		if (num >= 101 && num <= 120)
		{
			return 32;
		}
		return 12;
	}

	private IEnumerator WITCH_WaitToDoThing(string thingToDo, float wait)
	{
		yield return new WaitForSeconds(wait);
		WITCH_EXECUTECUSTOM(thingToDo);
		crtGeneric = null;
	}

	private void InvokedBlueFoxBossBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		if (currentBossState == BossState.Battle || BOSS_IS_PRE_FIGHT_MODE_COMPLETED())
		{
			CheckBossEnragedPhaseChangeLogic("", 2f, playSFX: true, 2);
			CHASE_ValidateFaintedChaseFetishes();
			if (!isStatic)
			{
				BOSS_Walker_Ero_Chase_Logic_Check(0, 10);
			}
			if (!isGrabbing && !GC.isPlayerFainted)
			{
				BossSkillCheck();
			}
			if (isUsingSkill && GC.isPlayerFainted && objAN.GetInteger("genericPhase") != 0 && objAN.GetInteger("genericPhase") != 10)
			{
				SetWaitToChangeSequence(0, 0f);
				BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
				sequenceOfBossSkill = 0;
			}
			BLUEFOX_MovementPatterns();
			objAN.SetBool("isSkill", isUsingSkill);
		}
	}

	private void BLUEFOX_MovementPatterns()
	{
		if (IsInvoking("Spawn2DSpriteTrail") && ((sequenceOfBossSkill != 1 && sequenceOfBossSkill != 21) || isGrabbing))
		{
			CancelInvoke("Spawn2DSpriteTrail");
		}
		if (isGrabbing)
		{
			SetEnemyTransparency(1f);
		}
		BLUEFOX_MELEESPINTOGGLE();
		switch (sequenceOfBossSkill)
		{
		case 0:
		case 3:
		case 4:
		case 7:
		case 9:
		case 13:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			if (!isGrabbing)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			if (sequenceOfBossSkill == 1 && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f) && GC.MATH_RNG_ROLL(1))
			{
				SetWaitToChangeSequence(40, 0f);
			}
			break;
		case 2:
		case 11:
		case 20:
		case 30:
		case 52:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			if (!isGrabbing)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			break;
		case 5:
		case 6:
		case 51:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			break;
		case 10:
		case 50:
		{
			if (!CheckCollisionGround())
			{
				break;
			}
			float num = 1.75f;
			if (phaseOfBossFight == 1)
			{
				num = 2f;
			}
			if (GC.isPlayerFainted && coroutEroGracePeriod != null)
			{
				if (GC.playerCurrentWorldXPosition <= 0f)
				{
					enemyCurrentXMove = num;
				}
				else
				{
					enemyCurrentXMove = 0f - num;
				}
				break;
			}
			if (base.transform.position.x < GC.playerCurrentWorldXPosition)
			{
				enemyCurrentXMove = num;
			}
			else
			{
				enemyCurrentXMove = 0f - num;
			}
			if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.Custom, 3f, 3f) && !isUsingSkill && !isGrabbing && !GC.isPlayerFainted && CheckCollisionGround())
			{
				if (sequenceOfBossSkill == 10)
				{
					SetWaitToChangeSequence(11, 0f);
				}
				else
				{
					SetWaitToChangeSequence(51, 0f);
				}
			}
			break;
		}
		case 12:
		{
			bool flag = true;
			float num2 = 2.5f;
			if (phaseOfBossFight == 1)
			{
				num2 = 2.75f;
			}
			if (base.transform.localScale.x != 1f)
			{
				flag = ((GC.playerCurrentWorldXPosition > base.transform.position.x && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomXOnlyInverted, 2f)) ? true : false);
			}
			else if (GC.playerCurrentWorldXPosition < base.transform.position.x && GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.CustomXOnlyInverted, 2f))
			{
				flag = false;
			}
			if (flag)
			{
				enemyCurrentXMove = num2;
			}
			else
			{
				enemyCurrentXMove = 0f - num2;
			}
			break;
		}
		case 22:
			if (CheckCollisionGround() && enemyCurrentYMove <= 0.5f)
			{
				BLUEFOX_POUNCEGROUNDHIT();
			}
			break;
		}
	}

	private void BLUEFOX_ExecuteSkills()
	{
		float waitTime = 4f;
		float num = 3f;
		int num2 = 0;
		if (phaseOfBossFight == 1)
		{
			waitTime = 3f;
		}
		switch (sequenceOfBossSkill)
		{
		case 0:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			if (GC.MATH_RNG_ROLL(15) || (phaseOfBossFight == 1 && GC.MATH_RNG_ROLL(40)))
			{
				SetWaitToChangeSequence(20, 0f);
			}
			else
			{
				SetWaitToChangeSequence(1, waitTime);
			}
			break;
		case 1:
			isIgnoreCollPlayer = true;
			BLUEFOX_DASHLOGIC();
			BossBasicSkillToggles(usingSkill: true, invertFacing: true, 1);
			SetWaitToChangeSequence(2, 1f);
			break;
		case 2:
			SetEnemyTransparency(1f);
			isIgnoreCollPlayer = false;
			if (GC.MATH_RNG_ROLL(15))
			{
				SetWaitToChangeSequence(5, 0f);
				break;
			}
			if (phaseOfBossFight == 1 && GC.MATH_RNG_ROLL(15))
			{
				SetWaitToChangeSequence(20, 0f);
				break;
			}
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 2);
			if (phaseOfBossFight == 1)
			{
				StartCoroutine(wait_BLUEFOX_Execute(2, 0f));
				StartCoroutine(wait_BLUEFOX_Execute(2, 0.75f));
				StartCoroutine(wait_BLUEFOX_Execute(2, 1.5f));
				StartCoroutine(wait_BLUEFOX_Execute(2, 2.25f));
			}
			else
			{
				StartCoroutine(wait_BLUEFOX_Execute(2, 0f));
				StartCoroutine(wait_BLUEFOX_Execute(2, 1f));
				StartCoroutine(wait_BLUEFOX_Execute(2, 2f));
			}
			SetWaitToChangeSequence(3, 3f);
			break;
		case 3:
			SetEnemyTransparency(1f);
			isIgnoreCollPlayer = false;
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			if (GC.MATH_RNG_ROLL(15))
			{
				SetWaitToChangeSequence(30, 2f);
				break;
			}
			countGenericStuff++;
			num2 = 1;
			if (countGenericStuff >= 2)
			{
				num2 = 4;
			}
			SetWaitToChangeSequence(num2, 2f);
			break;
		case 4:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(5, 1f);
			break;
		case 5:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 5);
			StartCoroutine(wait_BLUEFOX_Execute(5, 0f));
			num = 2f;
			if (phaseOfBossFight == 1)
			{
				num = 2f;
			}
			SetWaitToChangeSequence(6, num);
			STATE_MACHINE_FLIP_FACE_PLAYER();
			GC.PlaySFXGeneric("BlueFoxBoss", 5, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
			break;
		case 6:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 6);
			StartCoroutine(wait_BLUEFOX_Execute(6, 0f));
			SetWaitToChangeSequence(7, 2.5f);
			GC.PlaySFXGeneric("BlueFoxBoss", 6, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx", "BLF-Beam");
			break;
		case 7:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			waitTime = 2f;
			if (phaseOfBossFight == 1)
			{
				waitTime = 2.5f;
			}
			SetWaitToChangeSequence(20, waitTime);
			break;
		case 9:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			waitTime = 3f;
			if (phaseOfBossFight == 1)
			{
				waitTime = 2.5f;
			}
			SetWaitToChangeSequence(10, waitTime);
			break;
		case 10:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			if (GC.MATH_RNG_ROLL(15))
			{
				SetWaitToChangeSequence(30, 0f);
			}
			else
			{
				SetWaitToChangeSequence(11, 4f);
			}
			break;
		case 11:
			if (GC.MATH_RNG_ROLL(20))
			{
				SetWaitToChangeSequence(1, 0f);
				break;
			}
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 11);
			num = 1.5f;
			if (phaseOfBossFight == 1)
			{
				num = 1.5f;
			}
			SetWaitToChangeSequence(12, num);
			GC.PlaySFXGeneric("BlueFoxBoss", 1, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
			break;
		case 12:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 12);
			num = 1f;
			if (phaseOfBossFight == 1)
			{
				num = 2f;
			}
			SetWaitToChangeSequence(13, num);
			GC.PlaySFXGeneric("BlueFoxBoss", 2, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
			break;
		case 13:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0);
			countGenericStuff++;
			num2 = 10;
			if (GC.MATH_RNG_ROLL(10))
			{
				SetWaitToChangeSequence(5, 2f);
				break;
			}
			if (countGenericStuff >= 2)
			{
				num2 = 30;
			}
			SetWaitToChangeSequence(num2, 1f);
			break;
		case 20:
			if (GC.MATH_RNG_ROLL(20))
			{
				SetWaitToChangeSequence(5, 0f);
				break;
			}
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 20);
			STATE_MACHINE_FLIP_FACE_PLAYER();
			num = 2f;
			if (phaseOfBossFight == 1)
			{
				num = 0.5f;
			}
			SetWaitToChangeSequence(21, num);
			break;
		case 21:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 21);
			BLUEFOX_MINE();
			BLUEFOX_POUNCELOGIC();
			SetWaitToChangeSequence(22, 0.25f);
			break;
		case 22:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 21);
			SetWaitToChangeSequence(9, 1.25f);
			break;
		case 30:
		{
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 30);
			float num4 = 2f;
			int num5 = 6;
			if (phaseOfBossFight == 1)
			{
				num4 = 1f;
				num5 = 8;
			}
			for (int i = 0; i < num5; i++)
			{
				StartCoroutine(wait_BLUEFOX_Execute(30, num4));
				num4 = ((phaseOfBossFight != 1) ? (num4 + 1f) : (num4 + 0.7f));
			}
			SetWaitToChangeSequence(50, 8f);
			break;
		}
		case 40:
		{
			int num3 = 0;
			isIgnoreCollPlayer = true;
			BLUEFOX_DASHLOGIC();
			BossBasicSkillToggles(usingSkill: true, invertFacing: true, 40);
			switch (GC.MATH_RNG_INT(0, 4))
			{
			default:
				SetWaitToChangeSequence(0, 1f);
				break;
			case 1:
				SetWaitToChangeSequence(5, 1f);
				break;
			case 2:
				SetWaitToChangeSequence(11, 1f);
				break;
			case 3:
				SetWaitToChangeSequence(20, 1f);
				break;
			case 4:
				SetWaitToChangeSequence(30, 1f);
				break;
			}
			break;
		}
		case 50:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(51, 4f);
			break;
		case 51:
			if (objGeneric != null && objGeneric.activeInHierarchy && objGeneric.name.Contains("BLF-GroudShock"))
			{
				SetWaitToChangeSequence(20, 0f);
				break;
			}
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 51);
			SetWaitToChangeSequence(52, 2f);
			GC.PlaySFXGeneric("BlueFoxBoss", 2, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
			break;
		case 52:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 52);
			BLUEFOX_GROUNDSHOCK();
			SetWaitToChangeSequence(0, 3f);
			break;
		}
	}

	private void BLUEFOX_DASHLOGIC()
	{
		float num = 3.5f;
		BLUEFOX_MINE();
		if (GC.CheckPlayerProximityToAnyObject(base.transform.position, GameCore.PlayerProximityType.PlayerToTheRightCustom, 99f, 10f))
		{
			num *= -1f;
		}
		if ((base.transform.position.x >= 7f && num > 0f) || (base.transform.position.x <= -7f && num <= 0f))
		{
			num *= -1f;
			GenericExecuteJump();
			GC.PlaySFXGeneric("BlueFoxBoss", 3, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
		}
		else
		{
			GC.PlaySFXGeneric("BlueFoxBoss", 4, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
		}
		enemyCurrentXMove = num;
		SetEnemyTransparency(0.4f);
		InvokeRepeating("Spawn2DSpriteTrail", 0f, 0.1f);
	}

	private void BLUEFOX_POUNCELOGIC()
	{
		float num = 1f;
		float num2 = base.transform.position.x - GC.playerCurrentWorldXPosition;
		if (num2 < 0f)
		{
			num2 *= -1f;
		}
		Vector2 targetPosition = default;
		targetPosition.x = GC.playerCurrentWorldXPosition;
		targetPosition.y = GC.playerCurrentWorldYPosition;
		Vector2 vector = GC.MATH_CALC_BALLISTIC_TRAJECTORY_TO_POINT(base.transform.position, targetPosition, increaseHeightSpecial: false, Vector2.zero);
		num = ((num2 <= 4f) ? 4f : ((num2 <= 8f) ? 2.25f : ((!(num2 <= 12f)) ? 1.4f : 1.8f)));
		enemyCurrentXMove = vector.x / num;
		enemyCurrentYMove = vector.y;
		GenericExecuteJump();
		InvokeRepeating("Spawn2DSpriteTrail", 0f, 0.05f);
		GC.PlaySFXGeneric("BlueFoxBoss", 3, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
	}

	private void BLUEFOX_POUNCEGROUNDHIT()
	{
		SetWaitToChangeSequence(9, 0f);
		BasicProjectileInstantiate(GetMiscGameObject(3));
		BasicProjectileInstantiate(GetMiscGameObject(4));
		GC.PlaySFX(GameCore.SFXBasic2.Explosion1, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		GC.PlaySFXGeneric("BlueFoxBoss", 7, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
	}

	private void BLUEFOX_PROJECTILES()
	{
		Vector2 vector = enmObjRefs.StartPointCenterMass.position;
		if (base.transform.localScale.x > 0f)
		{
			vector.x += 0.75f;
		}
		else
		{
			vector.x -= 0.75f;
		}
		vector.y -= 0.75f;
		GameObject obj = UnityEngine.Object.Instantiate(GetMiscGameObject(5), vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
		if (base.transform.localScale.x > 0f)
		{
			vector.x += 5f;
		}
		else
		{
			vector.x -= 5f;
		}
		vector.y += GC.MATH_RNG_FLOAT(-0.1f, 1f);
		obj.GetComponent<ProjectileLogic>().specialForce = vector;
		GC.PlaySFXGeneric("BlueFoxBoss", 0, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
	}

	private void BLUEFOX_MELEESPINTOGGLE()
	{
		if (sequenceOfBossSkill == 12 && !GetMiscGameObject(6).activeInHierarchy)
		{
			GetMiscGameObject(6).SetActive(value: true);
		}
		else if (sequenceOfBossSkill != 12 && GetMiscGameObject(6).activeInHierarchy)
		{
			GetMiscGameObject(6).SetActive(value: false);
		}
	}

	private void BLUEFOX_GROUNDSHOCK()
	{
		new Vector3(0f, 0f, 90f);
		Vector2 vector = enmObjRefs.GroundCheck2.position;
		objGeneric = UnityEngine.Object.Instantiate(GetMiscGameObject(7), vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
		vector.y += 0.5f;
		GC.InstantiateVFX(GameCore.VisualEffect.BlueFoxCastShock, vector, "EnemyGeneralLogic:BLUEFOX_GROUNDSHOCK()");
		GC.PlaySFX(GameCore.SFXBasic2.GroundPoundLonger, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		GC.PlaySFXGeneric("BlueFoxBoss", 18, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
		GC.PlaySFXGeneric("BlueFoxBoss", 16, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
		GC.PlaySFXGeneric("BlueFoxBoss", 15, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
	}

	private void BLUEFOX_MINE()
	{
		Physics2D.IgnoreCollision(UnityEngine.Object.Instantiate(GetMiscGameObject(8), enmObjRefs.StartPointCenterMass.position, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<Collider2D>(), objBox2D);
		GC.PlaySFXGeneric("BlueFoxBoss", 8, 0, checkForDuplicity: true, base.transform.position, 0f, 0f, "sfx");
	}

	private IEnumerator wait_BLUEFOX_Execute(int sequence, float wait)
	{
		yield return new WaitForSeconds(wait);
		Vector3 zero = Vector3.zero;
		switch (sequence)
		{
		case 2:
			zero.x = GC.playerCurrentWorldXPosition;
			zero.y = GC.playerCurrentWorldYPosition;
			BasicProjectileInstantiate(GetMiscGameObject(0), overrideSpawnManual: true, zero.x, zero.y);
			break;
		case 5:
		case 6:
		{
			zero.x = enmObjRefs.StartPointCenterMass.position.x;
			zero.y = enmObjRefs.StartPointCenterMass.position.y;
			GameObject gameObject = ((sequence != 5) ? UnityEngine.Object.Instantiate(GetMiscGameObject(2), zero, Quaternion.identity, GC.refOBJEffectsParent.transform) : UnityEngine.Object.Instantiate(GetMiscGameObject(1), zero, Quaternion.identity, GC.refOBJEffectsParent.transform));
			if (base.transform.localScale.x < 0f)
			{
				zero = new Vector3(-1f, 1f, 1f);
				gameObject.transform.localScale = zero;
			}
			break;
		}
		case 30:
			BLUEFOX_PROJECTILES();
			break;
		}
	}

	private void InvokedBossSwampBehaviour()
	{
		if ((bool)GC)
		{
			GC.UpdateBossLifeBarValues(amountLife, enmBaseStats.maxLife);
		}
		if (currentBossState == BossState.Battle || BOSS_IS_PRE_FIGHT_MODE_COMPLETED())
		{
			CheckBossEnragedPhaseChangeLogic("", 2f, playSFX: true, 2);
			CHASE_ValidateFaintedChaseFetishes();
			if (!isStatic)
			{
				BOSS_Walker_Ero_Chase_Logic_Check(0, 800);
			}
			if (!isGrabbing && !GC.isPlayerFainted)
			{
				BossSkillCheck();
			}
			if (isUsingSkill && GC.isPlayerFainted && objAN.GetInteger("genericPhase") != 0 && objAN.GetInteger("genericPhase") != 10)
			{
				SetWaitToChangeSequence(0, 0f);
				BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
				sequenceOfBossSkill = 0;
			}
			SWAMPBOSS_MovementPatterns();
			objAN.SetBool("isSkill", isUsingSkill);
		}
	}

	private void SWAMPBOSS_MovementPatterns()
	{
		switch (sequenceOfBossSkill)
		{
		case 0:
		case 1:
		case 3:
		case 100:
		case 901:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			if (!isGrabbing)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
			break;
		case 2:
		case 900:
			if (enemyCurrentXMove != 0f)
			{
				enemyCurrentXMove = 0f;
			}
			break;
		case 200:
		case 800:
		{
			float num = enmBaseStats.speedMoveX;
			if (!CheckCollisionGround())
			{
				return;
			}
			if (phaseOfBossFight == 1)
			{
				num *= 1.25f;
			}
			if (GC.isPlayerFainted && coroutEroGracePeriod != null)
			{
				if (GC.playerCurrentWorldXPosition <= 0f)
				{
					enemyCurrentXMove = num;
				}
				else
				{
					enemyCurrentXMove = 0f - num;
				}
				return;
			}
			SWAMPBOSS_MoveWithOffsetBeyondPlayerPos(num);
			if (sequenceOfBossSkill == 200)
			{
				SWAMPBOSS_CheckJumpSpecialSkills();
			}
			break;
		}
		}
		if (GC.isPlayerFainted && !GetMiscGameObject(3).activeInHierarchy && !isGrabbing)
		{
			SWAMPBOSS_ToggleSpawnJumpPads(setState: true);
		}
	}

	private void SWAMPBOSS_MoveWithOffsetBeyondPlayerPos(float movSpeed, float offset = 2f)
	{
		if (GC.isPlayerFainted)
		{
			offset = 0.5f;
		}
		if (enemyCurrentXMove > 0f && base.transform.position.x > GC.playerCurrentWorldXPosition + offset)
		{
			enemyCurrentXMove = 0f - movSpeed;
		}
		else if (enemyCurrentXMove < 0f && base.transform.position.x < GC.playerCurrentWorldXPosition - offset)
		{
			enemyCurrentXMove = movSpeed;
		}
		else if (enemyCurrentXMove == 0f)
		{
			if (GC.playerCurrentWorldXPosition < base.transform.position.x)
			{
				enemyCurrentXMove = 0f - movSpeed;
			}
			else
			{
				enemyCurrentXMove = movSpeed;
			}
		}
	}

	private void SWAMPBOSS_AttackSkills()
	{
		float num = 3f;
		float num2 = 0f;
		int num3 = 0;
		if (phaseOfBossFight == 1)
		{
			num = 2f;
		}
		switch (sequenceOfBossSkill)
		{
		case 0:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(1, num);
			break;
		case 1:
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 1);
			SetWaitToChangeSequence(2, num - 1f);
			GC.PlaySFX(GameCore.SFXBasic2.Odd2, 0, checkForDuplicity: true, base.transform.position, 0.8f, 1f);
			break;
		case 2:
		{
			countGenericStuff++;
			if (countGenericStuff > 2)
			{
				SetWaitToChangeSequence(900, 0f);
				break;
			}
			int[] elements = new int[3] { 100, 200, 800 };
			int num5 = 0;
			do
			{
				num5++;
				num3 = GC.MATH_RNG_INT_ARRAY(elements);
			}
			while (num3 == intGeneric || num5 < 10);
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, num3);
			SetWaitToChangeSequence(num3, 2f);
			intGeneric = num3;
			if (num3 == 800)
			{
				SWAMPBOSS_ToggleSpawnJumpPads(setState: true);
			}
			break;
		}
		case 3:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 3);
			SetWaitToChangeSequence(1, num);
			break;
		case 100:
		{
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, sequenceOfBossSkill);
			num2 = 1f;
			if (phaseOfBossFight == 1)
			{
				num2 = 0.5f;
			}
			for (float num4 = 0f; num4 < 5f; num4 += num2)
			{
				StartCoroutine(waitBossSwampActions(num4, "ShootBasic"));
			}
			GC.PlaySFX(GameCore.SFXBasic2.Boing1, 0, checkForDuplicity: true, base.transform.position, 0.7f, 0.9f);
			SetWaitToChangeSequence(3, 5f);
			break;
		}
		case 200:
			num2 = 4f;
			if (phaseOfBossFight == 1)
			{
				num2 = 6f;
			}
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, sequenceOfBossSkill);
			SetWaitToChangeSequence(3, num2);
			GC.PlaySFX(GameCore.SFXBasic2.Odd2, 0, checkForDuplicity: true, base.transform.position, 0.7f, 0.9f);
			break;
		case 800:
			num2 = 5f;
			if (phaseOfBossFight == 1)
			{
				num2 = 6f;
			}
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, sequenceOfBossSkill);
			InvokeRepeating("Invoked_SWAMPBOSSWalkingSkill", 0f, 0.25f);
			SetWaitToChangeSequence(3, num2);
			GC.PlaySFX(GameCore.SFXBasic2.Odd8, 0, checkForDuplicity: true, base.transform.position, 0.7f, 0.9f);
			break;
		case 900:
		{
			int quantityOfSpawn = 1;
			num2 = 3f;
			if (phaseOfBossFight == 1)
			{
				num2 = 2f;
				quantityOfSpawn = 2;
			}
			BossBasicSkillToggles(usingSkill: true, invertFacing: false, 900);
			SWAMPBOSS_SetupEnemySpawn(quantityOfSpawn);
			SetWaitToChangeSequence(901, num2);
			break;
		}
		case 901:
			BossBasicSkillToggles(usingSkill: false, invertFacing: false, 0, doResetCountGenericStuff: true);
			SetWaitToChangeSequence(0, 1f);
			break;
		}
	}

	private void Invoked_SWAMPBOSSWalkingSkill()
	{
		if (!isUsingSkill || objAN.GetInteger("genericPhase") != 800)
		{
			CancelInvoke("Invoked_SWAMPBOSSWalkingSkill");
			return;
		}
		Vector2 vector = enmObjRefs.StartPointCenterMass.position;
		vector.y -= 0.5f;
		vector.x += GC.MATH_RNG_FLOAT(-0.3f, 0.3f);
		vector.y += GC.MATH_RNG_FLOAT(-0.3f, 0.3f);
		UnityEngine.Object.Instantiate(GetMiscGameObject(1), vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
	}

	private void SWAMPBOSS_CheckJumpSpecialSkills()
	{
		if (CheckCollisionGround() && enemyCurrentYMove <= 0.5f)
		{
			GenericExecuteJump();
			boolGeneric = false;
		}
		if (!boolGeneric)
		{
			boolGeneric = true;
			UnityEngine.Object.Instantiate(GetMiscGameObject(2), enmObjRefs.StartPointCenterMass.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, enmObjRefs.StartPointCenterMass.position, "EnemyGeneralLogic:SlimeQueenBoss");
			GC.PlaySFX(GameCore.SFXBasic2.Odd4, 0, checkForDuplicity: true, base.transform.position, 0.75f, 0.9f);
		}
	}

	private void SWAMPBOSS_ToggleSpawnJumpPads(bool setState)
	{
		if (setState)
		{
			float wait = 20f;
			if (phaseOfBossFight == 1)
			{
				wait = 40f;
			}
			GetMiscGameObject(3).SetActive(value: true);
			SWAMPBOSS_SpawnJumpPadsVFX();
			if (crtGeneric != null)
			{
				StopCoroutine(crtGeneric);
			}
			crtGeneric = StartCoroutine(waitBossSwampActions(wait, "DisableSlimePads"));
		}
		else
		{
			SWAMPBOSS_SpawnJumpPadsVFX();
			GetMiscGameObject(3).SetActive(value: false);
		}
	}

	private void SWAMPBOSS_SpawnJumpPadsVFX()
	{
		foreach (Transform item in GetMiscGameObject(3).transform)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, item.position, "EnemyLogic:SwampBoss");
		}
	}

	private void SWAMPBOSS_SetupEnemySpawn(int QuantityOfSpawn)
	{
		List<Transform> list = new List<Transform>();
		List<Transform> list2 = new List<Transform>();
		int num = 50;
		int num2 = 0;
		int num3 = 0;
		foreach (Transform item in GetMiscGameObject(9).transform)
		{
			list.Add(item);
		}
		if (QuantityOfSpawn > list.Count)
		{
			Debug.Log("Error! Spawn quantity must be fewer than unique position quantity!");
			return;
		}
		do
		{
			num2++;
			num3 = GC.MATH_RNG_INT(0, list.Count - 1);
			if (!list2.Contains(list[num3]))
			{
				list2.Add(list[num3]);
			}
		}
		while (list2.Count < QuantityOfSpawn && num2 < num);
		if (list2.Count > 0)
		{
			StartCoroutine(waitBossSwampActions(3f, "SkillSummon", list2));
			foreach (Transform item2 in list2)
			{
				UnityEngine.Object.Instantiate(GetMiscGameObject(8), item2.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
			for (int i = 0; i < QuantityOfSpawn; i++)
			{
				UnityEngine.Object.Instantiate(GetMiscGameObject(10), base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
		}
		else
		{
			Debug.Log("Error! For some reason, list of valid SpawnPoints was zero!");
		}
	}

	private IEnumerator waitBossSwampActions(float wait, string action, List<Transform> listSpawnPoint = null)
	{
		yield return new WaitForSeconds(wait);
		switch (action)
		{
		case "ShootBasic":
		{
			for (int i = 0; i < 5; i++)
			{
				UnityEngine.Object.Instantiate(GetMiscGameObject(0), enmObjRefs.StartPointCenterMass.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
			break;
		}
		case "DisableSlimePads":
			SWAMPBOSS_ToggleSpawnJumpPads(setState: false);
			crtGeneric = null;
			break;
		case "SkillSummon":
		{
			Color color = new Color(1f, 0.5f, 0.8f, 1f);
			{
				foreach (Transform item in listSpawnPoint)
				{
					GameObject gameObject = UnityEngine.Object.Instantiate(GetMiscGameObject(GC.MATH_RNG_INT(4, 7)), item.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
					if (gameObject != null)
					{
						gameObject.GetComponent<EnemyGeneralLogic>().eroCore.IsAllowedToGrab = false;
						gameObject.GetComponent<SpriteRenderer>().color = color;
					}
				}
				break;
			}
		}
		}
	}

	public void BossSkillEnd()
	{
		isUsingSkill = false;
		objAN.SetInteger("genericPhase", 0);
	}

	private void BossSkillCheck()
	{
		countTimeToUseSkill += enmBaseStats.freqRepeatBehaviour;
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.BossForest:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				ExecuteForestBossSkill();
			}
			break;
		case GameCore.EnemyType.BossBeach:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				ExecuteBeachBossSkill();
			}
			break;
		case GameCore.EnemyType.BossHill:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				ExecuteHillBossSkill();
			}
			break;
		case GameCore.EnemyType.BossWerewolf:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				ExecuteWerewolfBossSkill();
			}
			break;
		case GameCore.EnemyType.BossMinotaur:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				ExecuteMinotaurBossSkill();
			}
			break;
		case GameCore.EnemyType.BossPrincess:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				ExecutePrincessBossSkill();
			}
			break;
		case GameCore.EnemyType.BossWitch:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				WITCH_ExecuteSkills();
			}
			break;
		case GameCore.EnemyType.BossBlueFox:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				BLUEFOX_ExecuteSkills();
			}
			break;
		case GameCore.EnemyType.BossSwampSlime:
			if (countTimeToUseSkill >= timeToTriggerNextSkill)
			{
				countTimeToUseSkill = 0f;
				SWAMPBOSS_AttackSkills();
			}
			break;
		}
	}

	private void BossBasicSkillToggles(bool usingSkill, bool invertFacing, int gnrPhase, bool doResetCountGenericStuff = false)
	{
		isUsingSkill = usingSkill;
		invertSpriteFacing = invertFacing;
		objAN.SetInteger("genericPhase", gnrPhase);
		if (doResetCountGenericStuff)
		{
			countGenericStuff = 0;
		}
	}

	private void BossHasTouchedWaypoint()
	{
		switch (currentEnemyType)
		{
		case GameCore.EnemyType.BossBeach:
			if (sequenceOfBossSkill == 1)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
				float repeatRate = 1.5f;
				isMoveToPoint = false;
				objBox2D.isTrigger = false;
				objRB.velocity = Vector2.zero;
				countGenericStuff = 0;
				CancelInvoke("Spawn2DSpriteTrail");
				if (phaseOfBossFight == 1)
				{
					repeatRate = 0.75f;
				}
				InvokeRepeating("Invoked_BeachBossSkill1", 0.5f, repeatRate);
			}
			else if (sequenceOfBossSkill == 4)
			{
				Special_BeachBossCallDiveSpecial();
			}
			break;
		case GameCore.EnemyType.BossWerewolf:
			WEREWOLF_TouchedWaypoint();
			break;
		case GameCore.EnemyType.BossMinotaur:
			MINOTAUR_TouchedWaypoint();
			break;
		case GameCore.EnemyType.BossPrincess:
			PRINCESS_TouchedWaypoint();
			break;
		}
	}

	private void BOSS_Walker_Ero_Chase_Logic_Check(int idOfStateIdle, int idOfStateRunningTowardsPlayer)
	{
		if (!BOSS_Walker_Ero_Chase_SpecialExceptions())
		{
			if (isGrabbing)
			{
				sequenceOfBossSkill = idOfStateIdle;
				invertSpriteFacing = false;
				isUsingSkill = false;
			}
			else if (GC.isPlayerFainted && canChaseFainted && sequenceOfBossSkill != idOfStateRunningTowardsPlayer && CheckCollisionGround())
			{
				sequenceOfBossSkill = idOfStateRunningTowardsPlayer;
				isUsingSkill = false;
				invertSpriteFacing = false;
				objAN.Play("Basic-Idle");
			}
			else if (GC.isPlayerFainted && !canChaseFainted && sequenceOfBossSkill != idOfStateIdle)
			{
				sequenceOfBossSkill = idOfStateIdle;
				isUsingSkill = false;
				invertSpriteFacing = false;
				objAN.Play("Basic-Idle");
			}
		}
	}

	private bool BOSS_Walker_Ero_Chase_SpecialExceptions()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.BossWerewolf)
		{
			if (GC.isPlayerFainted && !isGrabbing && sequenceOfBossSkill != 99)
			{
				isUsingSkill = false;
				invertSpriteFacing = false;
				crtGeneric = null;
				countGenericStuff = 0;
				objAN.Play("Basic-Idle");
				SetWaitToChangeSequence(99, 0f);
				return true;
			}
			if (sequenceOfBossSkill == 99)
			{
				return true;
			}
			if (GC.isPlayerFainted && GC.playerSpecialAnimCount == 0)
			{
				STATE_MACHINE_FLIP_FACE_PLAYER();
			}
		}
		return false;
	}

	private void FXU_CheckFixedBossMovement()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType != GameCore.EnemyType.BossBeach)
		{
			return;
		}
		switch (sequenceOfBossSkill)
		{
		case 0:
			base.transform.position = new Vector3(Mathf.Lerp(base.transform.position.x, posGenericVector.x, enmBaseStats.speedMoveX * Time.deltaTime), base.transform.position.y, base.transform.position.z);
			break;
		case 1:
			base.transform.position = new Vector3(Mathf.Lerp(base.transform.position.x, posGenericVector.x, enmBaseStats.speedMoveX * Time.deltaTime), Mathf.Lerp(base.transform.position.y, posGenericVector.y, enmBaseStats.speedMoveX * Time.deltaTime), base.transform.position.z);
			break;
		case 3:
			posGenericVector.x = GC.playerCurrentWorldXPosition;
			if (GC.playerCurrentMoveX < -0.1f)
			{
				posGenericVector.x -= 3f;
			}
			else if (GC.playerCurrentMoveX > 0.1f)
			{
				posGenericVector.x += 3f;
			}
			base.transform.position = new Vector3(Mathf.Lerp(base.transform.position.x, posGenericVector.x, enmBaseStats.speedMoveX * Time.deltaTime), base.transform.position.y, base.transform.position.z);
			break;
		case 4:
			if (currentEnemyState == enemyState.Falling && objBox2D.isTrigger)
			{
				objBox2D.isTrigger = false;
				enemyCurrentXMove = 0f;
			}
			else if (currentEnemyState == enemyState.Standing && enmBaseStats.speedMoveY != 0f)
			{
				enemyCurrentYMove = 0f;
				enmBaseStats.speedMoveX = 0f;
				enmBaseStats.speedMoveY = 0f;
				objAN.Play("Basic-Idle");
				STATE_MACHINE_FLIP_FACE_PLAYER();
				crtGeneric = StartCoroutine(waitBossBeachGeneric(2f, "resetSkills"));
			}
			break;
		case 2:
			break;
		}
	}

	private void CheckBossEnragedPhaseChangeLogic(string animName, float timeHowLong, bool playSFX, int sfxID, bool forceEnrage = false)
	{
		if (!((phaseOfBossFight == 0 && GC.MATH_ResultPercentileOfTotal(amountLife, enmBaseStats.maxLife) <= 50f) | forceEnrage))
		{
			return;
		}
		phaseOfBossFight = 1;
		if (!isEnraged)
		{
			if (playSFX)
			{
				PlaySFXGeneric(sfxID);
			}
			GC.DisplaySystemMessageUI(0, 72);
			isEnraged = true;
			objAN.SetBool("isEnrageStart", value: true);
			if (animName != "")
			{
				objAN.Play(animName);
			}
			if (crtResetEnragedFlag != null)
			{
				StopCoroutine(crtResetEnragedFlag);
			}
			crtResetEnragedFlag = StartCoroutine(waitToResetBossEnragedFlag(timeHowLong));
		}
	}

	private void DoBossSkillAnimForSeconds(string animName, float secondsHowLong)
	{
		isUsingSkill = true;
		objAN.SetBool("isSkill", isUsingSkill);
		if (!isEnraged)
		{
			objAN.Play(animName);
		}
		if (crtResetBossSkilLAnim != null)
		{
			StopCoroutine(crtResetBossSkilLAnim);
		}
		crtResetBossSkilLAnim = StartCoroutine(waitToResetBossSkillFlag(secondsHowLong));
	}

	private void SetWaitToChangeSequence(int nextSequence, float waitTime)
	{
		if (crtWaitBossSequence != null)
		{
			StopCoroutine(crtWaitBossSequence);
		}
		crtWaitBossSequence = StartCoroutine(waitToChangeBossSequence(waitTime, nextSequence));
	}

	private bool BOSS_IS_PRE_FIGHT_MODE_COMPLETED()
	{
		if (GC.isBossFight)
		{
			objAN.SetBool("isBossBattleMode", value: true);
			currentBossState = BossState.Battle;
			timeToTriggerNextSkill = 1f;
			GC.UpdateBossName(enmBaseStats.idEnemyNameInFile);
			BOSS_DoSpecialThingsAtBattleStart();
			return true;
		}
		return false;
	}

	private void BOSS_DoSpecialThingsAtBattleStart()
	{
		GameCore.EnemyType enemyType = currentEnemyType;
		if (enemyType == GameCore.EnemyType.BossWerewolf && !GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.WerewolfBossCurse))
		{
			GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.WerewolfBossCurse, 0f, "EnemyScript:WerewolfBoss");
		}
	}

	private IEnumerator waitToChangeBossSequence(float wait, int next)
	{
		timeToTriggerNextSkill = 999f;
		yield return new WaitForSeconds(wait);
		timeToTriggerNextSkill = 0f;
		sequenceOfBossSkill = next;
		crtWaitBossSequence = null;
	}

	private IEnumerator waitToResetBossSkillFlag(float wait)
	{
		yield return new WaitForSeconds(wait);
		isUsingSkill = false;
		objAN.SetBool("isSkill", isUsingSkill);
		crtResetBossSkilLAnim = null;
	}

	private IEnumerator waitToResetBossEnragedFlag(float wait)
	{
		yield return new WaitForSeconds(wait);
		isEnraged = false;
		objAN.SetBool("isEnrageStart", value: false);
		crtResetEnragedFlag = null;
	}

	private IEnumerator waitToFinishBossDeathAnimation(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtFinishBossDeathAnimation = null;
		FinalDeathFrame();
	}

	private bool BOSS_FLYER_NOT_CHASING()
	{
		BOSS_FLYER_CHASE_LOGIC();
		if (isChasingFainted || isGrabbing || currentFlyingState == enemyFlyingState.Retreating)
		{
			return false;
		}
		return true;
	}

	private void BOSS_FLYER_CHASE_LOGIC()
	{
		CHASE_ValidateFaintedChaseFetishes();
		if (coroutEroGracePeriod != null)
		{
			if (currentFlyingState != enemyFlyingState.Retreating)
			{
				currentFlyingState = enemyFlyingState.Retreating;
				limitRun = 5;
			}
		}
		else if (currentFlyingState == enemyFlyingState.Retreating)
		{
			currentFlyingState = enemyFlyingState.Idle;
			limitRun = 0;
		}
		if (GC.isPlayerFainted && !isChasingFainted && canChaseFainted && !isGrabbing && !GC.isPlayerGoneForGood && currentFlyingState != enemyFlyingState.Retreating)
		{
			isChasingFainted = true;
			invertSpriteFacing = false;
			currentFlyingState = enemyFlyingState.Chasing;
			limitRun = 10;
		}
		if (isGrabbing)
		{
			isChasingFainted = false;
			currentFlyingState = enemyFlyingState.Idle;
			enemyCurrentXMove = 0f;
			limitRun = 0;
		}
	}
}
