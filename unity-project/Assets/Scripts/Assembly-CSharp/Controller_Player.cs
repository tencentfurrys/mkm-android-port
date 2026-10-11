using System.Collections;
using UnityEngine;

public class Controller_Player : MonoBehaviour
{
	private enum playerState
	{
		Standing = 0,
		Running = 1,
		Jumping = 2,
		Falling = 3,
		Crouching = 4,
		Fainted = 5,
		OnLadder = 6,
		OnLadderMoving = 7
	}

	private enum playerBasicAttackState
	{
		Null = 0,
		Atk1 = 1,
		Atk2 = 2,
		Atk3 = 3,
		JumpAtk = 4,
		CrouchAtk = 5,
		UpwardsType1 = 6,
		UpwardsType2 = 7,
		LadderAttack = 8,
		LadderAttackUpwards = 9
	}

	private enum playerSkillState
	{
		Null = 0,
		Dash = 1,
		SlidingTackle = 2,
		Shield = 3,
		FlameSword = 4,
		ForwardSlash = 5,
		GroundPound = 6,
		DrillDash = 7,
		IntentionalDressChange = 8,
		RepairAllClothingLayer = 9
	}

	private enum playerSkillAffect
	{
		Duration = 0,
		Speed = 1
	}

	private enum PlayerColliderType
	{
		Box2D_Normal = 0,
		Box2D_Small = 1,
		Box2D_Fainted = 2,
		Polygon2D_Normal = 3,
		Capsule2D_Slide = 4,
		Box2D_Ladder = 5
	}

	private enum LocalMayaVoiceType
	{
		atkSimple = 0,
		atkHeavy = 1,
		hurtSoft = 2,
		hurtHard = 3,
		hurtScream = 4,
		eroReleased = 5,
		skillSlashCast = 6
	}

	public GameObject MainCamera;

	public GameObject EroFaintedObject;

	public Transform StartPointGroundCheck;

	public Transform StartPointCeilingCheck;

	public Transform GroundCheck1;

	public Transform GroundCheck2;

	public Transform GroundCheck3;

	public Transform CeilingCheck1;

	public Transform CeilingCheck2;

	public Transform CeilingCheck3;

	public Transform FrontCheckTop_main;

	public Transform FrontCheckTop_sub;

	public Transform FrontCheckBot_main;

	public Transform FrontCheckBot_sub;

	public Transform positionFlameSwordTrail;

	public Transform pointStairsCheck;

	public GameObject objAtk1HitBox;

	public GameObject objAtk2HitBox;

	public GameObject objAtk3HitBox;

	public GameObject objAtkJumpHitBox;

	public GameObject objAtkCrouchHitBox;

	public GameObject objAtkUpwards_type1;

	public GameObject objAtkUpwards_type2;

	public GameObject objSkillSlideTackle;

	public GameObject objTextDamageUI;

	public Transform centerOfMass;

	public Transform centerOfMassSliding;

	public Transform slideTopStart;

	public Transform slideTopEnd;

	public Transform slideBotStart;

	public Transform slideBotEnd;

	public float freqUpdateGlobalStats;

	public float freqDoSpiritRegen;

	public float delayToMoveLookCamera;

	public float durationHurtAnim;

	public GameObject essenceAttractor;

	public PlayerMiscObjects miscOBJ;

	public PlayerHackUnstuck[] hackUnstuck;

	[HideInInspector]
	public string currStatHack;

	private playerState currentPlayerState;

	private playerBasicAttackState currentAtkState;

	private playerSkillState currentSkillState;

	private GameCore.PlayerDressState currentDressState;

	private static float playerBaseHorMoveSpeed = 7f;

	private static float playerBaseVertMoveSpeed = 5.5f;

	private static float playerLadderVerticalSpeedMulti = 0.85f;

	private static float valueLadderDelay = 0.25f;

	private float playerHorizontalAccelSpeed;

	private float playerCurrentXMove;

	private float playerCurrentYMove;

	private float valueTimeOfJumpAssist;

	private float valueGravityCap;

	private float valueGravityIncrement;

	private float countCameraLookUpTimer;

	private float valueLadderX;

	private float valueLadderYTop;

	private float valueLadderYBottom;

	private float countCameraLookDownTimer;

	private float currentSpiritRegenMultiplier;

	private float currentStruggle;

	private float maxStruggle;

	private float playerMaxJumpForce;

	private float originalAnimatorSpeed;

	private float origCeilingCheck_Ypos;

	private float currentHorMoveSpeed;

	private float flameSwordDuration;

	private float flameSwordStartTime;

	private float _ptrFrequencyInvoke;

	private float _ptrTimer;

	private float _ptrGoal;

	private float _ptrCounter;

	private float _ptrMoveAffectMultiplier;

	private float _hackySlidingSpeed;

	private float potLifePercent;

	private float potDuration;

	private float defRegenDuration;

	private float defRegenPercent;

	private float skillRepairStartTime;

	private float currentLIFERegenMultiplier;

	private float charmedDuration;

	private int cumulativeDamageTaken;

	private int originalSortingOrder;

	private int valueLadderSO;

	private int specialAnimCounter;

	private int specialAnimMax;

	private int JumpAttackCounter;

	private int JumpAttackLimiter;

	private int intAnimLoopCounter;

	private int intStairsFacing;

	private string strCurrentPitfallCheckpoint;

	private string strCurrentEventWayPointTarget;

	private string _ptrStringinvoked;

	private string _ptrStringinvoked2;

	private string _ptrStringStiffAnim;

	private bool isJumpAssit;

	private bool isDoingBasicAttackAnimation;

	private bool canCombo;

	private bool isTakingCumulativeDamage;

	private bool isHurt;

	private bool isInvincible;

	private bool isPassTroughPlatform;

	private bool isPressingUpwards;

	private bool isPressingDownwards;

	private bool canInteractWithNPC;

	private bool canSaveGame;

	private bool isBlockMoveInput;

	private bool isBlockActionInput;

	private bool canCameraLook;

	private bool isPlayerAllowedToStruggle;

	private bool endDash;

	private bool hasJumpedDuringDash;

	private bool doDressChangeAfterHurt;

	private bool canPlayerFlipNextAttack;

	private bool forceMoveRight;

	private bool forceMoveLeft;

	private bool canUseTeleport;

	private bool canFastSkipDialogue;

	private bool isLoadDelay;

	private bool isSaveDelayAfterLoad;

	private bool canInteractWithLever;

	private bool delayFrontCheck;

	private bool isSkillShieldEffectActive;

	private bool isForwardSlashReset;

	private bool runTimePlayedCoroutine;

	private bool hasUpdatedBeltSlotsOnce;

	private bool seeIfStuck;

	private bool canInteractWithChest;

	private bool canGrabLadder;

	private bool wasJumpButton;

	private bool isCustomSort;

	private bool isDelayFastSkipValidated;

	private bool isFlameSwordReset;

	private bool isSpecialFlameSwordMove;

	private bool isDelaySameLevelPortal;

	private bool isDrillDashReset;

	private bool boolMinimapWasON;

	private bool isStairsCollTouch;

	private bool hasDoubleJumped;

	private Vector3 playerSetScale;

	private Vector2 vecPlayerMovement;

	private Vector2 vecOriginalColliderSize;

	private Vector2 vecOriginalColliderOffset;

	private Vector2 vecCrouchColliderSize;

	private Vector2 vecCrouchColliderOffset;

	private Vector2 vecFaintedColliderSize;

	private Vector2 vecFaintedColliderOffset;

	private Vector2 vecGeneric;

	private Vector2 vecLadderCollSize;

	private Vector2 vecLadderCollOffset;

	private Vector2 vecTargetMovementVelocity;

	private Vector2 vecMasterOverridePlayerMovement;

	private Vector2 lastUnstuckPosition;

	private Color colorOriginal;

	private Color colorFlashing;

	private Coroutine coroutPlayerDamaged;

	private Coroutine coroutPlayerDamagedCumulative;

	private Coroutine coroutPlayerInvincible;

	private Coroutine coroutPlayerPassTroughPlatform;

	private Coroutine crtDelayJump;

	private Coroutine crtSpecialSkill;

	private Coroutine crtSpecialSkillForceEnd;

	private Coroutine crtPitfallRespawn;

	private Coroutine crtCountSpecialAnim;

	private Coroutine crtColorFlashing;

	private Coroutine crtForceMovePlayer;

	private Coroutine crtDelayTeleportUse;

	private Coroutine crtWaitForSkipDialogue;

	private Coroutine crtWaitForLoadDelay;

	private Coroutine crtPitRespawnControlDelay;

	private Coroutine crtDelayEroZoomKeyPress;

	private Coroutine crtWaitSaveDelay;

	private Coroutine crtJumpPressed;

	private Coroutine crtFrontCheck;

	private Coroutine crtEventDialogueWait;

	private Coroutine crtDelayPlayerCanBeGrabbed;

	private Coroutine crtCountTimePlayed;

	private Coroutine crtSeeStuck;

	private Coroutine crtDelayRetry;

	private Coroutine crtWaitRestorePassPlatColliders;

	private Coroutine crtShader2D;

	private Coroutine crtLadderDelay;

	private Coroutine crtJumpAssist;

	private Coroutine crtDislodgeSlide;

	private Coroutine crtSoundSpecial;

	private Coroutine crtDelayPressFastSkip;

	private Coroutine crtSkillShieldTimer;

	private Coroutine crtNotEnoughSpirit;

	private Coroutine crtResetCameraLeverOffset;

	private Coroutine crtInputBlock;

	private Coroutine crtMinimapToggleHold;

	private Coroutine crtIgnoreColl;

	private Coroutine crtPTRPetrifying;

	private Coroutine crtPotionAnim;

	private Coroutine crtTeleportArrive;

	private Coroutine crtWaitDelayAfterFainted;

	private Coroutine crtDelayFadeOut;

	private Coroutine crtDelayNPCTalk;

	private Coroutine crtInputActDelay;

	private Coroutine crtStairsFallThrough;

	private Coroutine crtItemThrow;

	private Coroutine crtAfterItemThrow;

	private BoxCollider2D objBox2D;

	private PolygonCollider2D objPolygon2d;

	private CapsuleCollider2D objCapsule2D;

	private Rigidbody2D objRB;

	private Rigidbody2D objMovPlatRB;

	private Collider2D _currStairColl;

	private SpriteRenderer objRE;

	private Animator objAN;

	private LeverSignal currentLeverRef;

	private GameObject objGrabReleaseEffect;

	private GameObject objLastEnemyCollided;

	private GameObject objLastChestCollided;

	private GameObject objTouchedNPC;

	private GameObject objPetrifierEnemy;

	private GameObject refSelfFaintedEroOBJ;

	private GameCore GC;

	private GameUI coreUI;

	private ControllerCamera coreCAM;

	private Swap2DShader refShader2D;

	private EventTriggerLogic lastTouchedEventTrigger;

	private ENM_EroPhaseControl[] arraySpecialAnimControl;

	private void Awake()
	{
		CheckRefCoreUI();
	}

	private void Start()
	{
		GC = GameCore.Instance;
		if (GC.isSceneTransitioning)
		{
			if (!GC.flagsMain.seenEventBeach01 && !GC.LogicCheckGameState(GameCore.LogicCheckType.AnyBossDefeated) && GC.strCurrentActiveSceneName == "Stage2-Beach")
			{
				GC.DoFadeInOutBlackScreen(FadeIntoBlack: false, 999999f);
				GC.EquipmentAddStarting();
			}
			else
			{
				float wait = 0.2f;
				if (GC.enemyViewGallery != null && GC.enemyViewGallery.isRedirectToGalleryView)
				{
					wait = 1f;
				}
				crtDelayFadeOut = StartCoroutine(waitDelayFadeOut(wait));
			}
			if ((bool)GC && GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.MealHero))
			{
				GameObject obj = GC.InstantiateVFX(GameCore.VisualEffect.ShieldMealInvincible, base.transform.position, "playerStart()", base.transform, returnOBJref: true);
				BuffEffect buffEffect = GC.playerEffectList.Find((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.MealHero);
				obj.GetComponent<EffectLogic>().timeDurationLoop = buffEffect.maxTimer - buffEffect.countTimer;
			}
		}
		InitializeReferences();
		InitializeVariables();
		AlterColliderState(box: false, poly: false, caps: false);
		ChangeTheActiveCollider(PlayerColliderType.Box2D_Normal);
		StartInvokedFunctions();
		GC.StartStopAllCleanUpRountines(setON: true);
		SetupPlayerPreLoad();
		GC.CheckStageMusic();
		if ((bool)GC && CheckRefCoreUI())
		{
			coreUI.UIMisc.StageName.text = GC.strCurrentActiveStageName;
			coreUI.ShowStageNameDelayed();
		}
		GC.ValidatePlayerInventoryItemTypes();
		GC.ValidatePowerupsAfterLoad();
		GC.ACC_RefreshListActiveAccessories();
		GC.UIMainToggle(setState: true);
		if (GC.enemyViewGallery != null && GC.enemyViewGallery.isRedirectToGalleryView)
		{
			GC.ApplyGalleryViewLogicAfterLoad();
		}
		else
		{
			if (GC.playerIsBeingGrabed)
			{
				PlayerWasEroReleased(isMayaGoneForever: false);
				GC.playerIsBeingGrabed = false;
			}
			GC.StoreRetryGameCore(base.transform.position.x, base.transform.position.y, "[CONTROLLER_PLAYER-START()]");
		}
		crtCountTimePlayed = StartCoroutine(timerThatCountsPlayedTime());
		if ((bool)GC && GC.playerDidTeleport)
		{
			GC.playerDidTeleport = false;
			TeleportationVisualbehavior("Pre-Arrive-Setup");
		}
		if (GC.lock60fps)
		{
			Application.targetFrameRate = 60;
		}
		else
		{
			Application.targetFrameRate = -1;
		}
		ToggleEssenceAttractor(state: true);
	}

	private void StartInvokedFunctions()
	{
		InvokeRepeating("UpdatePlayerGlobalStats", 0f, freqUpdateGlobalStats);
		InvokeRepeating("DoSpiritConstantRegeneration", 0f, freqDoSpiritRegen);
		InvokeRepeating("InvokedHack_AvoidStuckOnGroundAndGates", 0f, 0.1f);
		InvokeRepeating("INVOKED_EVERY_SECOND", 0f, 1f);
	}

	private void InitializeReferences()
	{
		objRB = GetComponent<Rigidbody2D>();
		objRE = GetComponent<SpriteRenderer>();
		objAN = GetComponent<Animator>();
		objBox2D = GetComponent<BoxCollider2D>();
		objPolygon2d = GetComponent<PolygonCollider2D>();
		objCapsule2D = GetComponent<CapsuleCollider2D>();
		if (MainCamera != null)
		{
			coreCAM = MainCamera.GetComponent<ControllerCamera>();
			coreCAM.SetTheCameraStartPositionToPlayer(base.gameObject);
		}
		else
		{
			Debug.LogWarning("Must insert a valid camera on [PLAYER] object!", base.transform);
		}
		refShader2D = GetComponent<Swap2DShader>();
		CheckRefCoreUI();
	}

	private void InitializeVariables()
	{
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: false);
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerProjectile"), ignore: false);
		playerSetScale = base.transform.localScale;
		GC.isForbidAllPlayerInput = false;
		currentPlayerState = playerState.Falling;
		currentAtkState = playerBasicAttackState.Null;
		currentSkillState = playerSkillState.Null;
		currentHorMoveSpeed = playerBaseHorMoveSpeed;
		playerCurrentXMove = 0f;
		playerCurrentYMove = 0.1f;
		playerHorizontalAccelSpeed = 0f;
		vecPlayerMovement = Vector2.zero;
		vecMasterOverridePlayerMovement = Vector2.zero;
		vecOriginalColliderSize = objBox2D.size;
		vecOriginalColliderOffset = objBox2D.offset;
		vecCrouchColliderSize = new Vector2(0.595f, 0.95f);
		vecCrouchColliderOffset = new Vector2(0f, -1.01f);
		vecFaintedColliderSize = new Vector2(0.8f, 0.3f);
		vecFaintedColliderOffset = new Vector2(0f, -1.335f);
		vecLadderCollSize = new Vector2(0.5f, 1.35f);
		vecLadderCollOffset = new Vector2(0f, -0.45f);
		originalSortingOrder = objRE.sortingOrder;
		colorOriginal = objRE.color;
		colorFlashing = colorOriginal;
		GC.playerIsAllowedToBeGrabbed = true;
		GC.isPlayerFainted = false;
		GC.savingPlayerData = false;
		origCeilingCheck_Ypos = CeilingCheck1.transform.localPosition.y;
		currentSpiritRegenMultiplier = 1f;
		currentLIFERegenMultiplier = 1f;
		currentStruggle = 0f;
		maxStruggle = 100f;
		JumpAttackCounter = 0;
		JumpAttackLimiter = 3;
		cumulativeDamageTaken = 0;
		countCameraLookUpTimer = 0f;
		countCameraLookDownTimer = 0f;
		originalAnimatorSpeed = objAN.speed;
		currentLeverRef = null;
		GC.canPlayerCanUpdateGlobalVariables = true;
		canPlayerFlipNextAttack = false;
		isTakingCumulativeDamage = false;
		IsInvincibleToggleSelf(setInvnicibleState: false);
		isJumpAssit = false;
		isDoingBasicAttackAnimation = false;
		canCombo = false;
		isPassTroughPlatform = false;
		canInteractWithNPC = false;
		canInteractWithChest = false;
		isBlockMoveInput = false;
		isBlockActionInput = false;
		canSaveGame = false;
		canCameraLook = false;
		isPlayerAllowedToStruggle = false;
		endDash = false;
		hasJumpedDuringDash = false;
		seeIfStuck = false;
		forceMoveLeft = false;
		forceMoveRight = false;
		canUseTeleport = false;
		isPressingUpwards = false;
		canFastSkipDialogue = true;
		doDressChangeAfterHurt = false;
		isLoadDelay = true;
		isSaveDelayAfterLoad = true;
		canInteractWithLever = false;
		runTimePlayedCoroutine = true;
		isDelayFastSkipValidated = false;
		objGrabReleaseEffect = null;
		lastTouchedEventTrigger = null;
		isSkillShieldEffectActive = false;
		isFlameSwordReset = true;
		isDrillDashReset = true;
		isForwardSlashReset = true;
		boolMinimapWasON = false;
		isStairsCollTouch = false;
		valueTimeOfJumpAssist = GC.valueTimeOfJumpAssist;
		valueGravityCap = GC.worldBaseGravityCap;
		valueGravityIncrement = GC.worldBaseGravityIncrements;
		playerMaxJumpForce = GC.playerMaxJumpForce;
		if (!GC.isPersistGameSessionValues)
		{
			ApplyDressStateChange(resetToFullDress: true);
			GC.clsLastSeenStats.SaveValues(0, 0f, 0, 0, 0);
		}
		else
		{
			currentDressState = (GameCore.PlayerDressState)GC.playerCurrentDressState;
			ChangeAnimatorLayer(GameCore.PlayerDressID.Default, currentDressState);
			GC.clsLastSeenStats.SaveValues(GC.playerMaxHealth, GC.playerMaxSpirit, GC.playerCurrentBasicAttackPower, GC.playerCurrentBasicDefensePower, GC.playerCurrentEssenceAmount);
		}
		GC.isPlayerInteractingLocked = false;
		GC.isRunningBadEndLogic = false;
		GC.playerIsIntentionalDressStateChange = false;
		if (GC.configToggleMinimapState)
		{
			GC.MINIMAP_Toggle(toggleCurrentState: false, setThisSpecificState: true);
		}
		RefreshUI();
	}

	private void SetupPlayerPreLoad()
	{
		if (GC.isSceneTransitioning)
		{
			GC.isSceneTransitioning = false;
		}
		if (crtWaitForLoadDelay != null)
		{
			StopCoroutine(crtWaitForLoadDelay);
		}
		crtWaitForLoadDelay = StartCoroutine(waitForCrouchDelayAfterLoad(0.6f));
		if (crtWaitSaveDelay != null)
		{
			StopCoroutine(crtWaitSaveDelay);
		}
		crtWaitSaveDelay = StartCoroutine(waitForSaveDelay(3f));
		if (GC.teleportTargetPosition != Vector2.zero)
		{
			if (!SpecialCheckStuckLoadPosOnFloor())
			{
				base.transform.position = GC.teleportTargetPosition;
			}
			Vector2 position = base.transform.position;
			position.x += GC.offsetCameraAfterTP.x;
			position.y += GC.offsetCameraAfterTP.y;
			coreCAM.InstantlyMoveTheCameraToPositionAfterTeleportToNewSceneBorder(position);
		}
		if (isInvincible)
		{
			IsInvincibleToggleSelf(setInvnicibleState: false);
		}
		if (GC.forceMovePlayerAfterTP)
		{
			float num = GC.offsetCameraAfterTP.x;
			if (num > 0f)
			{
				num = 1f;
			}
			else if (num < 0f)
			{
				num = -1f;
			}
			ForceMovePlayerLeftRight((int)num, 0.2f);
		}
		BeginDelayToEnterTeleport(0.75f);
		if (GC.strCurrentActiveSceneName == "Stage2-Beach-PartBoss" && GC.strPreviousSceneName != "Stage2-Beach-Part2")
		{
			GC.DisplaySystemMessageUI(123456, 0, 0f, "DEBUG: Defeat this boss to acquire the teleporter!");
		}
		checkEndCameraOffsetFromLever();
		if ((bool)GC && !GC.isGamePaused && Time.timeScale != 1f)
		{
			Time.timeScale = 1f;
		}
		if (GC.strCurrentActiveSceneName == "AnimGallery" && !GC.flagsMain.HasVisitedTheGallery)
		{
			GC.flagsMain.HasVisitedTheGallery = true;
			GC.configMasterIsGalleryEnabled = true;
			GC.HACK_CHECK_REDUNDANCE_GALLERY_ENABLE_ROOMS();
		}
		if (GC.arraySlotsOfBelt[GC.indexCurrentBeltSlotActive] == null || GC.arraySlotsOfBelt[GC.indexCurrentBeltSlotActive].quantity <= 0)
		{
			GC.CycleActiveItem(isForwards: true);
		}
	}

	private bool SpecialCheckStuckLoadPosOnFloor()
	{
		bool flag = false;
		RaycastHit2D[] array = Physics2D.BoxCastAll(GC.teleportTargetPosition, new Vector2(0.5f, 0.5f), 0f, Vector2.zero);
		if (array != null)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].transform.gameObject.tag == "platform-floor")
				{
					flag = true;
					break;
				}
			}
		}
		if (flag)
		{
			float num = 999999f;
			int num2 = -1;
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("SavePoint");
			if (array2 != null)
			{
				for (int j = 0; j < array2.Length; j++)
				{
					float num3 = GC.teleportTargetPosition.x - array2[j].transform.position.x;
					float num4 = GC.teleportTargetPosition.y - array2[j].transform.position.y;
					if (num3 < 0f)
					{
						num3 *= -1f;
					}
					if (num4 < 0f)
					{
						num4 *= -1f;
					}
					if (num3 + num4 < num)
					{
						num = num3 + num4;
						num2 = j;
					}
				}
			}
			if (num2 >= 0)
			{
				base.transform.position = array2[num2].transform.position;
				return true;
			}
		}
		return false;
	}

	private void FixedUpdate()
	{
		FXU_FINISH_SPECIAL_SKILLS_SITUATIONS();
		if (GC.playerIsBeingGrabed)
		{
			return;
		}
		FXU_STAIRS_MAIN_SWITCHER();
		STATE_MACHINE_LOGIC();
		STATE_MACHINE_ANIMATION();
		switch (currentPlayerState)
		{
		case playerState.Standing:
			playerHorizontalAccelSpeed = currentHorMoveSpeed / 1.75f;
			break;
		case playerState.Running:
			if (currentSkillState == playerSkillState.SlidingTackle)
			{
				playerHorizontalAccelSpeed = currentHorMoveSpeed;
			}
			if (playerHorizontalAccelSpeed < currentHorMoveSpeed)
			{
				playerHorizontalAccelSpeed += 0.25f;
			}
			if (playerHorizontalAccelSpeed > currentHorMoveSpeed)
			{
				playerHorizontalAccelSpeed = currentHorMoveSpeed;
			}
			break;
		case playerState.Jumping:
		case playerState.Falling:
			if (currentSkillState == playerSkillState.DrillDash)
			{
				playerHorizontalAccelSpeed = currentHorMoveSpeed;
			}
			break;
		}
		if (!FXU_GRAVITY_EXCEPTIONS() && (currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling || currentPlayerState == playerState.Fainted))
		{
			if (playerCurrentYMove > valueGravityCap)
			{
				playerCurrentYMove += valueGravityIncrement;
			}
			playerHorizontalAccelSpeed = currentHorMoveSpeed;
		}
		else if (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running)
		{
			playerCurrentYMove = -1f;
		}
		if (playerCurrentYMove > 0.5f && currentPlayerState == playerState.Jumping && CheckCollisionCeiling() && !FXU_HACKY_CEILING_LENIENCE())
		{
			playerCurrentYMove = 0.5f;
		}
		if (FXU_JUMPHOVER())
		{
			playerCurrentYMove = -0.3f;
			playerCurrentXMove /= 3f;
		}
		FXU_LADDER_SPECIFICS();
		FXU_CONTROL_PLAYER_COLLIDER();
		FXU_STOP_MOVE_ON_BASIC_ATK();
		FXU_CHECK_SPECIAL_DISABLE_PLAT_EFFECTOR();
		vecPlayerMovement.x = playerCurrentXMove * playerHorizontalAccelSpeed;
		vecPlayerMovement.y = playerCurrentYMove * playerBaseVertMoveSpeed;
		if ((bool)GC)
		{
			vecPlayerMovement.x *= GC.GetDressStatValue(GameCore.DressStatValue.Movement);
			vecPlayerMovement.y *= GC.GetDressStatValue(GameCore.DressStatValue.Movement);
			if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.DrinkSpeed))
			{
				vecPlayerMovement.x *= 1.25f;
			}
		}
		if (isPassTroughPlatform)
		{
			vecPlayerMovement.x = 0f;
		}
		FXU_CHECK_CRUSHED_DMG();
		FXU_APPLY_SPECIAL_MOVEMENT_VELOCITY(vecPlayerMovement);
		FXU_HACKY_FIX_MICRO_DISPLACEMENT_BUG();
		FXU_HACKY_FIX_CINEMATIC_STUCK_BUG();
		FXU_PLATMOVE_HORIZONTAL();
		FXU_PLATMOVE_VERTICAL();
		FXU_STAIRS_BEHAVIOR();
		if (GC.isSceneTransitioning && !GC.isAllowVerticalMovementDuringSceneLoad)
		{
			vecPlayerMovement.y = 0f;
		}
		if (vecMasterOverridePlayerMovement != Vector2.zero)
		{
			if (isSpecialFlameSwordMove)
			{
				vecPlayerMovement = FXU_MOVE_FLAME_SWORD();
			}
			if (currentSkillState == playerSkillState.RepairAllClothingLayer)
			{
				vecPlayerMovement = FXU_MOVE_CLOTH_REPAIR();
			}
		}
		if (objRB.bodyType != RigidbodyType2D.Static)
		{
			objRB.velocity = vecPlayerMovement;
		}
	}

	private void FXU_APPLY_SPECIAL_MOVEMENT_VELOCITY(Vector2 alteredMovement)
	{
		switch (currentSkillState)
		{
		case playerSkillState.Dash:
		{
			float x = playerCurrentXMove;
			if (!hasJumpedDuringDash && (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running || currentPlayerState == playerState.Crouching))
			{
				x = base.transform.localScale.x;
			}
			alteredMovement.x = x * (currentHorMoveSpeed * 2f * ITEM_SkillAffect(currentSkillState, playerSkillAffect.Speed));
			break;
		}
		case playerSkillState.Null:
		{
			bool flag3 = false;
			if (playerCurrentXMove == 1f && vecTargetMovementVelocity.x > vecPlayerMovement.x)
			{
				vecTargetMovementVelocity.x -= 0.4f;
				flag3 = true;
			}
			else if (playerCurrentXMove == -1f && vecTargetMovementVelocity.x < vecPlayerMovement.x)
			{
				vecTargetMovementVelocity.x += 0.4f;
				flag3 = true;
			}
			else if (playerCurrentXMove == 0f)
			{
				vecTargetMovementVelocity.x = vecPlayerMovement.x;
			}
			if (flag3)
			{
				alteredMovement.x = vecTargetMovementVelocity.x;
			}
			break;
		}
		case playerSkillState.GroundPound:
			alteredMovement.x = 0f;
			alteredMovement.y = -15f;
			break;
		case playerSkillState.DrillDash:
			if (Mathf.RoundToInt(objRB.velocity.x) == 0)
			{
				bool flag = !COLL_RayCastFrontOfDrillDashOpenPath(slideTopStart.transform.position, slideTopEnd.transform.position);
				bool flag2 = !COLL_RayCastFrontOfDrillDashOpenPath(slideBotStart.transform.position, slideBotEnd.transform.position);
				if (!flag && !flag2)
				{
					alteredMovement.y = 0f;
				}
				else if (flag)
				{
					alteredMovement.y = 5f;
				}
				else if (flag2)
				{
					alteredMovement.y = -5f;
				}
			}
			else
			{
				alteredMovement.y = 0f;
			}
			break;
		}
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrifying) && _ptrCounter > 0f)
		{
			if (alteredMovement.x > 0.1f)
			{
				alteredMovement.x -= _ptrCounter * _ptrMoveAffectMultiplier;
			}
			else if (alteredMovement.x < -0.1f)
			{
				alteredMovement.x += _ptrCounter * _ptrMoveAffectMultiplier;
			}
			else
			{
				alteredMovement.x = 0f;
			}
			alteredMovement.y -= _ptrCounter * _ptrMoveAffectMultiplier;
		}
		if (GC.playerCurrentAnimState == "Player-ThrowA_Stand" || GC.playerCurrentAnimState == "Player-ThrowA_Jump")
		{
			alteredMovement.x /= 2.5f;
		}
		if (alteredMovement != vecPlayerMovement)
		{
			vecTargetMovementVelocity = alteredMovement;
			vecPlayerMovement = vecTargetMovementVelocity;
		}
	}

	private void FXU_FINISH_SPECIAL_SKILLS_SITUATIONS()
	{
		switch (currentSkillState)
		{
		case playerSkillState.Dash:
			if (endDash)
			{
				EndSpecialSkill();
			}
			if (currentPlayerState == playerState.Jumping)
			{
				hasJumpedDuringDash = true;
			}
			break;
		case playerSkillState.SlidingTackle:
		case playerSkillState.DrillDash:
			if (crtSpecialSkill == null && !CheckCollisionCeiling())
			{
				EndSpecialSkill();
			}
			break;
		case playerSkillState.GroundPound:
			if (crtSpecialSkill == null || CheckCollisionGround() || CheckCollisionMovingPlatforms())
			{
				EndSpecialSkill();
			}
			break;
		case playerSkillState.Shield:
		case playerSkillState.FlameSword:
		case playerSkillState.ForwardSlash:
			break;
		}
	}

	private void FXU_CONTROL_PLAYER_COLLIDER()
	{
		if (GC.isPlayerFainted || GC.playerIsBeingGrabed || isHurt)
		{
			return;
		}
		playerSkillState playerSkillState2 = currentSkillState;
		if (playerSkillState2 == playerSkillState.SlidingTackle || playerSkillState2 == playerSkillState.DrillDash)
		{
			ChangeTheActiveCollider(PlayerColliderType.Capsule2D_Slide);
			return;
		}
		switch (currentPlayerState)
		{
		case playerState.Standing:
		case playerState.Running:
			ChangeTheActiveCollider(PlayerColliderType.Polygon2D_Normal);
			break;
		case playerState.Crouching:
			playerCurrentXMove = 0f;
			ChangeTheActiveCollider(PlayerColliderType.Box2D_Small);
			break;
		case playerState.OnLadder:
		case playerState.OnLadderMoving:
			ChangeTheActiveCollider(PlayerColliderType.Box2D_Ladder);
			break;
		default:
			ChangeTheActiveCollider(PlayerColliderType.Box2D_Normal);
			break;
		}
	}

	private void FXU_STOP_MOVE_ON_BASIC_ATK()
	{
		if (isDoingBasicAttackAnimation && currentAtkState != playerBasicAttackState.JumpAtk && currentAtkState != playerBasicAttackState.UpwardsType2)
		{
			playerState playerState2 = currentPlayerState;
			if ((uint)playerState2 <= 1u || playerState2 == playerState.Crouching)
			{
				playerCurrentXMove = 0f;
			}
		}
	}

	private void FXU_CHECK_CRUSHED_DMG()
	{
		if (CheckCollisionCeilingMovingExclusive() && CheckCollisionGround() && !isHurt && !isInvincible && !GC.isPlayerFainted && !GC.playerDidTeleport)
		{
			ApplyDamageToPlayer((int)GC.MATH_ResultValueOfTotalPercentile(40f, GC.playerMaxHealth), Vector3.zero, afterEroEscaped: false, bypassDefense: true);
			GC.PlaySFX(GameCore.SFXBasic1.HitHurtCrush1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
		}
		else if (CheckCollisionCeilingMovingExclusive() && !CheckCollisionGround() && !isHurt && !isInvincible && !GC.isPlayerFainted && !GC.playerDidTeleport)
		{
			ApplyDamageToPlayer((int)GC.MATH_ResultValueOfTotalPercentile(35f, GC.playerMaxHealth), Vector3.zero, afterEroEscaped: false, bypassDefense: true);
			GC.PlaySFX(GameCore.SFXBasic1.HitHurtSpike1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			GC.InstantiateVFX(GameCore.VisualEffect.ClawRed, base.transform.position, "CONTROLLER_PLAYER:FXU_CHECK_CRUSHED_DMG");
		}
	}

	private void FXU_HACKY_FIX_MICRO_DISPLACEMENT_BUG()
	{
		if ((currentPlayerState == playerState.Crouching || currentPlayerState == playerState.Standing) && vecPlayerMovement.y < 0f && CheckCollisionGround() && !CheckCollisionEnemies())
		{
			vecPlayerMovement.y = 0f;
		}
	}

	private void FXU_HACKY_FIX_CINEMATIC_STUCK_BUG()
	{
		if (seeIfStuck && crtForceMovePlayer != null && (forceMoveLeft || forceMoveRight) && Mathf.Round(objRB.velocity.x) == 0f && CheckCollisionGround())
		{
			playerCurrentYMove = playerMaxJumpForce / 2f;
		}
	}

	private bool FXU_GRAVITY_EXCEPTIONS()
	{
		if (MainRecurringEffect(GameCore.TempPlayerEffect.PullGrab))
		{
			return true;
		}
		if (currentSkillState == playerSkillState.Shield || currentSkillState == playerSkillState.FlameSword || currentSkillState == playerSkillState.GroundPound || currentSkillState == playerSkillState.DrillDash || currentSkillState == playerSkillState.RepairAllClothingLayer)
		{
			return true;
		}
		if (isStairsCollTouch && currentPlayerState != playerState.Jumping)
		{
			playerCurrentYMove = 0f;
			return true;
		}
		return false;
	}

	private void FXU_LADDER_SPECIFICS()
	{
		if (currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving)
		{
			playerCurrentXMove = 0f;
			if (currentSkillState == playerSkillState.Dash)
			{
				EndSpecialSkill(forceEnd: true);
			}
			if (GC.playerIsHurt || (playerCurrentYMove < 0f && CheckCollisionGround()))
			{
				EndPlayerGrabLadder();
			}
		}
	}

	private void FXU_CHECK_SPECIAL_DISABLE_PLAT_EFFECTOR()
	{
		if (((bool)objMovPlatRB && objMovPlatRB.velocity != Vector2.zero) || currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving)
		{
			SetCollisionLayerPlatEffector(setCollisionON: false);
		}
		else
		{
			SetCollisionLayerPlatEffector(setCollisionON: true);
		}
	}

	private bool FXU_HACKY_CEILING_LENIENCE()
	{
		switch (CheckCollisionCeilingHackyJump())
		{
		case 1:
			base.transform.position = base.transform.position + new Vector3(0.2f, 0.05f, 0f);
			return true;
		case 3:
			base.transform.position = base.transform.position + new Vector3(-0.2f, 0.05f, 0f);
			return true;
		default:
			return false;
		}
	}

	private bool FXU_JUMPHOVER()
	{
		if ((currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling) && (currentAtkState == playerBasicAttackState.JumpAtk || currentAtkState == playerBasicAttackState.UpwardsType2 || currentSkillState == playerSkillState.ForwardSlash || GC.playerCurrentAnimState == "Player-ThrowA_Jump"))
		{
			return true;
		}
		return false;
	}

	private void FXU_HACKY_SIDE_COLL_ENTER_LENIENCY()
	{
		if (!delayFrontCheck && (currentPlayerState == playerState.Falling || currentPlayerState == playerState.Jumping) && playerCurrentXMove != 0f && CheckSideCollision())
		{
			delayFrontCheck = true;
			if (base.transform.localScale.x == 1f)
			{
				base.transform.position = base.transform.position + new Vector3(0.1f, 0f, 0f);
			}
			else if (base.transform.localScale.x == -1f)
			{
				base.transform.position = base.transform.position + new Vector3(-0.1f, 0f, 0f);
			}
			if (crtFrontCheck != null)
			{
				StopCoroutine(crtFrontCheck);
			}
			crtFrontCheck = StartCoroutine(waitDelayFronCheck(0.5f));
		}
	}

	private void FXU_PLATMOVE_VERTICAL()
	{
		if (objMovPlatRB != null && objMovPlatRB.velocity.y < 0.1f && objRB.velocity.y < 0f && vecPlayerMovement.y < 0f)
		{
			vecPlayerMovement.y = objMovPlatRB.velocity.y;
		}
	}

	private void FXU_PLATMOVE_HORIZONTAL()
	{
		if (objMovPlatRB != null)
		{
			if (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Crouching)
			{
				vecPlayerMovement.x += objMovPlatRB.velocity.x;
			}
			else if (currentPlayerState == playerState.Running && ((objMovPlatRB.velocity.x > 0.1f && vecPlayerMovement.x > 0f) || (objMovPlatRB.velocity.x < -0.1f && vecPlayerMovement.x < 0f)))
			{
				vecPlayerMovement.x += objMovPlatRB.velocity.x;
			}
		}
	}

	private Vector2 FXU_MOVE_FLAME_SWORD()
	{
		float t = (Time.time - flameSwordStartTime) / flameSwordDuration;
		if (vecMasterOverridePlayerMovement.x > 0f)
		{
			vecMasterOverridePlayerMovement.x = Mathf.SmoothStep(5f, 1f, t);
		}
		else
		{
			vecMasterOverridePlayerMovement.x = Mathf.SmoothStep(-5f, -1f, t);
		}
		vecMasterOverridePlayerMovement.y = Mathf.SmoothStep(12.5f, 4f, t);
		return vecMasterOverridePlayerMovement;
	}

	private Vector2 FXU_MOVE_CLOTH_REPAIR()
	{
		float t = (Time.time - skillRepairStartTime) / 3f;
		vecMasterOverridePlayerMovement.x = 0f;
		vecMasterOverridePlayerMovement.y = Mathf.SmoothStep(0.5f, 0.25f, t);
		return vecMasterOverridePlayerMovement;
	}

	private IEnumerator waitDelayFronCheck(float wait)
	{
		yield return new WaitForSeconds(wait);
		delayFrontCheck = false;
		crtFrontCheck = null;
	}

	private void Update()
	{
		if (!hasUpdatedBeltSlotsOnce && (bool)GC && CheckRefCoreUI())
		{
			hasUpdatedBeltSlotsOnce = true;
			coreUI.UpdateItemSlotsDisplay();
		}
		if (GC.isForbidAllPlayerInput)
		{
			return;
		}
		MainRecurringEffect(GameCore.TempPlayerEffect.Charmed);
		MainRecurringEffect(GameCore.TempPlayerEffect.Petrified);
		if ((bool)GC && GC.isPlayerFainted && !objRE.enabled && IsMayaAnimationState("SlimeMelt-Loop1"))
		{
			objRE.enabled = true;
		}
		UPD_BLOCK_ENABLE_INPUTS();
		if ((movHack_Right() && !isBlockMoveInput && !GC.isPlayerFainted) || forceMoveRight)
		{
			if (!isHurt)
			{
				playerCurrentXMove = 1f;
			}
			checkEndCameraOffsetFromLever();
		}
		else if ((movHack_Left() && !isBlockMoveInput && !GC.isPlayerFainted) || forceMoveLeft)
		{
			if (!isHurt)
			{
				playerCurrentXMove = -1f;
			}
			checkEndCameraOffsetFromLever();
		}
		else if (movHack_Right() && currentSkillState == playerSkillState.Dash && (currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling || currentPlayerState == playerState.Running))
		{
			if (!isHurt)
			{
				playerCurrentXMove = 1f;
			}
			checkEndCameraOffsetFromLever();
		}
		else if (movHack_Left() && currentSkillState == playerSkillState.Dash && (currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling || currentPlayerState == playerState.Running))
		{
			if (!isHurt)
			{
				playerCurrentXMove = -1f;
			}
			checkEndCameraOffsetFromLever();
		}
		else
		{
			playerCurrentXMove = 0f;
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Up, GameCore.InputPressType.KeyHolding) && (currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving) && !canCameraLook && !GC.isGamePaused)
		{
			if (base.transform.position.y < valueLadderYTop)
			{
				if (!isDoingBasicAttackAnimation)
				{
					playerCurrentYMove = playerLadderVerticalSpeedMulti;
				}
				else
				{
					playerCurrentYMove = 0f;
				}
			}
			else
			{
				playerCurrentYMove = 0f;
			}
		}
		else if (GC.UnifiedInputHandler(GameCore.InputLib.Down, GameCore.InputPressType.KeyHolding) && (currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving) && !canCameraLook && !GC.isGamePaused)
		{
			if (base.transform.position.y > valueLadderYBottom)
			{
				if (!isDoingBasicAttackAnimation)
				{
					playerCurrentYMove = 0f - playerLadderVerticalSpeedMulti;
				}
				else
				{
					playerCurrentYMove = 0f;
				}
			}
			else
			{
				EndPlayerGrabLadder();
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Jump, GameCore.InputPressType.KeyPressedOnce) && (!isBlockActionInput || currentSkillState == playerSkillState.Dash) && !GC.isPlayerFainted)
		{
			if (crtJumpPressed != null)
			{
				StopCoroutine(crtJumpPressed);
			}
			crtJumpPressed = StartCoroutine(waitJumpPressed(0.1f));
			PerformJumpFallLogic();
			checkEndCameraOffsetFromLever();
		}
		if (currentPlayerState == playerState.Jumping && !GC.isPlayerFainted && !isHurt && (GC.UnifiedInputHandler(GameCore.InputLib.Jump, GameCore.InputPressType.KeyReleased) || !GC.UnifiedInputHandler(GameCore.InputLib.Jump, GameCore.InputPressType.KeyHolding)) && playerCurrentYMove > 1f && wasJumpButton)
		{
			playerCurrentYMove = 1f;
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Up, GameCore.InputPressType.KeyHolding) && !GC.isPlayerFainted && !isHurt)
		{
			if (!isBlockActionInput || UPD_EXCEPTIONS_SPECIAL_SKILL_CANCELS())
			{
				isPressingUpwards = true;
				checkEndCameraOffsetFromLever();
			}
			if (!isBlockActionInput || (isBlockActionInput && currentSkillState == playerSkillState.Dash))
			{
				CheckIfCanGrabLadder("UP");
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Up, GameCore.InputPressType.KeyReleased))
		{
			isPressingUpwards = false;
			if (currentPlayerState == playerState.OnLadderMoving)
			{
				playerCurrentYMove = 0f;
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Down, GameCore.InputPressType.KeyHolding) && !GC.isPlayerFainted && !isHurt)
		{
			if ((!isBlockActionInput && currentSkillState != playerSkillState.SlidingTackle) || UPD_EXCEPTIONS_SPECIAL_SKILL_CANCELS())
			{
				isPressingDownwards = true;
				checkEndCameraOffsetFromLever();
			}
			else
			{
				isPressingDownwards = false;
			}
			if (!isBlockActionInput || (isBlockActionInput && currentSkillState == playerSkillState.Dash))
			{
				CheckIfCanGrabLadder("DOWN");
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Down, GameCore.InputPressType.KeyReleased))
		{
			isPressingDownwards = false;
			if (currentPlayerState == playerState.OnLadderMoving)
			{
				playerCurrentYMove = 0f;
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.BasicAttack, GameCore.InputPressType.KeyPressedOnce) && !GC.isPlayerFainted && !isHurt)
		{
			if (!isBlockActionInput || (isBlockActionInput && currentSkillState == playerSkillState.Dash))
			{
				BeginBasicAttackLogic();
			}
			checkEndCameraOffsetFromLever();
		}
		if ((GC.UnifiedInputHandler(GameCore.InputLib.Interact, GameCore.InputPressType.KeyPressedOnce) || (GC.UnifiedInputHandler(GameCore.InputLib.ConfirmUI, GameCore.InputPressType.KeyPressedOnce) && GC.isDisplayingDialogue)) && CheckInteractionAndDialoguePermissions() && !IsUsingAnySkill())
		{
			if (canInteractWithNPC)
			{
				if (GC.isDisplayingSpecialMenuNPC)
				{
					if (GC.isDisplayingNPCMenuTalkDialogue && crtDelayNPCTalk == null)
					{
						coreUI.DisplayNextDialogue();
					}
					return;
				}
				if (GC.isDisplayingDialogue)
				{
					coreUI.DisplayNextDialogue();
				}
				else
				{
					PlayerHasInteractedWithNPC();
				}
			}
			if (canInteractWithChest)
			{
				TryToOpenChest();
			}
			HACK_MimicChestCheck();
		}
		if ((GC.UnifiedInputHandler(GameCore.InputLib.Interact, GameCore.InputPressType.KeyPressedOnce) || (GC.UnifiedInputHandler(GameCore.InputLib.ConfirmUI, GameCore.InputPressType.KeyPressedOnce) && GC.isDisplayingDialogue)) && GC.isRunningBadEndLogic && !GC.isGamePaused && GC.doAllowDialogueDuringBadEnd && GC.isDisplayingDialogue)
		{
			coreUI.DisplayNextDialogue();
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Interact, GameCore.InputPressType.KeyPressedOnce) && !isHurt && !GC.isPlayerFainted && !GC.isGamePaused && currentSkillState == playerSkillState.Null)
		{
			checkEndCameraOffsetFromLever();
			if (canInteractWithLever && (bool)currentLeverRef && currentLeverRef.CanThisLeverBeActivated() && (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running || currentPlayerState == playerState.Crouching))
			{
				currentLeverRef.ExecuteThisLeverActivation();
				Vector2 vector = base.transform.position;
				vector.x = currentLeverRef.transform.position.x;
				base.transform.position = vector;
				BlockPlayerInputs(0.55f);
				objAN.Play("Player-UseLever");
				startCameraOffsetFromLever();
			}
			if ((bool)lastTouchedEventTrigger)
			{
				InteractedWithEvent();
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Interact, GameCore.InputPressType.KeyPressedOnce) && GC.isPlayerFainted && crtDelayRetry == null)
		{
			GC.CallMenuRetry();
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.CancelUI, GameCore.InputPressType.KeyPressedOnce) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftShift))
		{
			if ((canInteractWithNPC && GC.isDisplayingDialogue && !GC.isPlayerFainted && !GC.isGamePaused && currentSkillState == playerSkillState.Null) || (GC.isDisplayingDialogue && GC.isRunningBadEndLogic && !GC.isGamePaused))
			{
				coreUI.DisplayNextDialogue();
				if (crtDelayPressFastSkip == null)
				{
					isDelayFastSkipValidated = false;
					crtDelayPressFastSkip = StartCoroutine(waitDelayFastSkipValidate(0.5f));
				}
			}
			if (crtCountSpecialAnim != null && GC.isPlayerFainted && !GC.isGamePaused)
			{
				SKIP_SPECIAL_ANIM_INCREASE_PHASE();
			}
		}
		if ((GC.UnifiedInputHandler(GameCore.InputLib.CancelUI, GameCore.InputPressType.KeyHolding) || Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.LeftShift)) && isDelayFastSkipValidated && !GC.isGamePaused)
		{
			if (GC.isDisplayingDialogue && canFastSkipDialogue)
			{
				if (canInteractWithNPC && GC.isDisplayingDialogue)
				{
					coreUI.DisplayNextDialogue();
				}
				if (crtWaitForSkipDialogue != null)
				{
					StopCoroutine(crtWaitForSkipDialogue);
				}
				crtWaitForSkipDialogue = StartCoroutine(waitForSkipDialogueText(0.05f));
			}
			if (!GC.eventAnimationIsFinished)
			{
				GC.eventAnimationIsFinished = true;
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.CancelUI, GameCore.InputPressType.KeyReleased) || Input.GetKeyUp(KeyCode.LeftControl) || Input.GetKeyUp(KeyCode.LeftShift))
		{
			if (crtDelayPressFastSkip != null)
			{
				StopCoroutine(crtDelayPressFastSkip);
			}
			crtDelayPressFastSkip = null;
			isDelayFastSkipValidated = false;
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.AttackSkills, GameCore.InputPressType.KeyPressedOnce))
		{
			if (isPressingUpwards)
			{
				TryToExecuteSpecialSkill(playerSkillState.FlameSword);
			}
			else if (isPressingDownwards)
			{
				TryToExecuteSpecialSkill(playerSkillState.GroundPound);
			}
			else
			{
				TryToExecuteSpecialSkill(playerSkillState.ForwardSlash);
				checkEndCameraOffsetFromLever();
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DefenseSkills, GameCore.InputPressType.KeyPressedOnce))
		{
			if (isPressingDownwards)
			{
				TryToExecuteSpecialSkill(playerSkillState.SlidingTackle);
			}
			else if (isPressingUpwards)
			{
				TryToExecuteSpecialSkill(playerSkillState.Shield);
			}
			if (currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling)
			{
				TryToExecuteSpecialSkill(playerSkillState.DrillDash);
			}
			else
			{
				TryToExecuteSpecialSkill(playerSkillState.Dash);
				checkEndCameraOffsetFromLever();
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Camera, GameCore.InputPressType.KeyHolding) && !isBlockActionInput && !GC.isPlayerFainted && !GC.playerIsBeingGrabed)
		{
			canCameraLook = true;
			checkEndCameraOffsetFromLever();
		}
		else if (GC.UnifiedInputHandler(GameCore.InputLib.Camera, GameCore.InputPressType.KeyPressedOnce) && (GC.isPlayerFainted || GC.playerIsBeingGrabed))
		{
			ToggleEroGrabZoom();
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Camera, GameCore.InputPressType.KeyReleased))
		{
			canCameraLook = false;
			checkEndCameraOffsetFromLever();
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Pause, GameCore.InputPressType.KeyPressedOnce) && !GC.delayAfterkeyBound && !isLoadDelay && !GC.playerIsIntentionalDressStateChange)
		{
			GC.TogglePauseForGameMenu();
		}
		else if (Input.GetKeyDown(KeyCode.Escape) && Time.timeScale == 1f && !isLoadDelay && !GC.playerIsIntentionalDressStateChange)
		{
			GC.TogglePauseForGameMenu();
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.Interact, GameCore.InputPressType.KeyPressedOnce) && !isHurt && !GC.isGamePaused && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && canSaveGame && (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running))
		{
			PlayerQuickSaveGameData();
		}
		if (ValidStruggleSituation())
		{
			if (GC.UnifiedInputHandler(GameCore.InputLib.Up, GameCore.InputPressType.KeyPressedOnce))
			{
				TryMoveMayaStruggleBoard(GameCore.InputLib.Up);
				GC.PlayerPressedUPDuringStruggleCheckHeartIcon();
			}
			else if (GC.UnifiedInputHandler(GameCore.InputLib.Right, GameCore.InputPressType.KeyPressedOnce))
			{
				TryMoveMayaStruggleBoard(GameCore.InputLib.Right);
			}
			else if (GC.UnifiedInputHandler(GameCore.InputLib.Down, GameCore.InputPressType.KeyPressedOnce))
			{
				TryMoveMayaStruggleBoard(GameCore.InputLib.Down);
			}
			else if (GC.UnifiedInputHandler(GameCore.InputLib.Left, GameCore.InputPressType.KeyPressedOnce))
			{
				TryMoveMayaStruggleBoard(GameCore.InputLib.Left);
			}
			else if (GC.UnifiedInputHandler(GameCore.InputLib.Up, GameCore.InputPressType.KeyHolding))
			{
				TryMoveMayaStruggleBoard(GameCore.InputLib.Up, isHold: true);
			}
			else if (GC.UnifiedInputHandler(GameCore.InputLib.Right, GameCore.InputPressType.KeyHolding))
			{
				TryMoveMayaStruggleBoard(GameCore.InputLib.Right, isHold: true);
			}
			else if (GC.UnifiedInputHandler(GameCore.InputLib.Down, GameCore.InputPressType.KeyHolding))
			{
				TryMoveMayaStruggleBoard(GameCore.InputLib.Down, isHold: true);
			}
			else if (GC.UnifiedInputHandler(GameCore.InputLib.Left, GameCore.InputPressType.KeyHolding))
			{
				TryMoveMayaStruggleBoard(GameCore.InputLib.Left, isHold: true);
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DefenseSkills, GameCore.InputPressType.KeyPressedOnce) && GC.isPlayerFainted && GC.playerIsBeingGrabed)
		{
			GC.trySkipToBadEndStart = true;
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.ToggleInternalView, GameCore.InputPressType.KeyPressedOnce))
		{
			if (crtMinimapToggleHold != null)
			{
				StopCoroutine(crtMinimapToggleHold);
			}
			crtMinimapToggleHold = StartCoroutine(waitMinimapToggleHold(0.5f));
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.ToggleInternalView, GameCore.InputPressType.KeyReleased) && crtMinimapToggleHold != null)
		{
			StopCoroutine(crtMinimapToggleHold);
			GC.showInternalShots = !GC.showInternalShots;
			GC.DisplaySystemMessageUI(1);
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.UseItemActive, GameCore.InputPressType.KeyPressedOnce))
		{
			TryToUseItemInBeltSlot();
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.NextItem, GameCore.InputPressType.KeyPressedOnce) && !GC.isPlayerFainted && !GC.isGamePaused && !GC.isSceneTransitioning)
		{
			GC.CycleActiveItem(isForwards: true, doSound: true);
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.PreviousItem, GameCore.InputPressType.KeyPressedOnce) && !GC.isPlayerFainted && !GC.isGamePaused && !GC.isSceneTransitioning)
		{
			GC.CycleActiveItem(isForwards: false, doSound: true);
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DebugF5, GameCore.InputPressType.KeyPressedOnce) && GC.doAllowDebug && Time.timeScale > 0f && !isHurt && !isInvincible && !isTakingCumulativeDamage && !GC.isPlayerFainted)
		{
			GC.HealThePlayer(100f);
			GC.DisplaySystemMessageUI(123456, 0, 0f, "DEBUG: Maya's health was fully restored!");
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DebugF6, GameCore.InputPressType.KeyPressedOnce) && GC.doAllowDebug && Time.timeScale > 0f && !isHurt && !isInvincible && !isTakingCumulativeDamage && !GC.isPlayerFainted)
		{
			GC.HealTheSpirit(100f);
			GC.DisplaySystemMessageUI(123456, 0, 0f, "DEBUG: Maya's spirit was fully restored!");
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DebugF7, GameCore.InputPressType.KeyPressedOnce) && GC.doAllowDebug && Time.timeScale > 0f && !isHurt && !isInvincible && !isTakingCumulativeDamage && !GC.isPlayerFainted)
		{
			GC.ResetPlayerDressDurability();
			DEBUG_AddPower();
			GC.DisplaySystemMessageUI(123456, 0, 0f, "DEBUG: Armor durability was reset back to 100%!");
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DebugF8, GameCore.InputPressType.KeyPressedOnce) && GC.doAllowDebug && Time.timeScale > 0f && !isHurt && !isInvincible && !isTakingCumulativeDamage && !GC.isPlayerFainted)
		{
			RestoreAllArmorLayers();
			GC.DisplaySystemMessageUI(123456, 0, 0f, "DEBUG: Maya's armor was fixed!");
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DebugF9, GameCore.InputPressType.KeyPressedOnce) && GC.doAllowDebug && Time.timeScale > 0f)
		{
			DEBUG_DisplayBasicInfo();
			GC.DEBUG_AddAllSkills();
			GC.AddNewVisitedSavePoint(0, isDebug: true);
			GC.DEBUG_UNLOCKGALLERY();
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DebugF10, GameCore.InputPressType.KeyPressedOnce) && GC.doAllowDebug && Time.timeScale > 0f && (bool)GC && !GC.isSceneTransitioning && GC.strCurrentActiveSceneName != "Stage2-Beach-PartBoss")
		{
			if (GC.CheckPlayerHasItem(100))
			{
				GC.DEBUG_UNLOCKGALLERY();
				GC.DisplaySystemMessageUI(123456, 0, 5f, "Gallery full unlock!");
				GC.LoadNextScene("AnimGallery", Vector2.zero, Vector2.zero, forceMovePlayer: false, applyFade: true, Color.black, GC.globalDefaultSceneFadeTimer, lockPlayerInput: true, persistPlayerGameSessionValues: true);
			}
			else
			{
				GC.LoadNextScene("Stage2-Beach-PartBoss", Vector2.zero, Vector2.zero, forceMovePlayer: false, applyFade: true, Color.black, GC.globalDefaultSceneFadeTimer, lockPlayerInput: true, persistPlayerGameSessionValues: true);
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DebugF11, GameCore.InputPressType.KeyPressedOnce) && GC.doAllowDebug && Time.timeScale > 0f)
		{
			if (!isHurt && !isInvincible && !isTakingCumulativeDamage && !GC.isPlayerFainted)
			{
				ApplyDamageToPlayer(200, Vector3.zero, afterEroEscaped: false, bypassDefense: true, onlyArmorDmg: true);
				GC.DisplaySystemMessageUI(123456, 0, 0f, "DEBUG: One dress layer has been broken!");
			}
			if ((bool)GC && GC.playerIsBeingGrabed)
			{
				GC.debugForceReleaseEroGrab = true;
				GC.playerCancelStopEro = true;
			}
		}
		if (GC.UnifiedInputHandler(GameCore.InputLib.DebugF12, GameCore.InputPressType.KeyPressedOnce) && GC.doAllowDebug && Time.timeScale > 0f && !isHurt && !isInvincible && !isTakingCumulativeDamage && !GC.isPlayerFainted)
		{
			ApplyDamageToPlayer(GC.playerMaxHealth + 1, Vector3.zero, afterEroEscaped: false, bypassDefense: true);
		}
	}

	private void UPD_BLOCK_ENABLE_INPUTS()
	{
		if (GC.isGamePaused || GC.isPlayerInteractingLocked || EffectsThatBlocksInput() || GC.playerIsBeingGrabed || crtPotionAnim != null || crtInputActDelay != null || currentSkillState == playerSkillState.Dash || currentSkillState == playerSkillState.SlidingTackle || currentSkillState == playerSkillState.Shield || currentSkillState == playerSkillState.FlameSword || currentSkillState == playerSkillState.GroundPound || currentSkillState == playerSkillState.DrillDash || currentSkillState == playerSkillState.IntentionalDressChange || currentSkillState == playerSkillState.RepairAllClothingLayer)
		{
			isBlockActionInput = true;
			isBlockMoveInput = true;
		}
		else if (isBlockActionInput || isBlockMoveInput)
		{
			isBlockActionInput = false;
			isBlockMoveInput = false;
		}
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.PullGrab) && !Physics2D.GetIgnoreLayerCollision(16, 8))
		{
			Physics2D.IgnoreLayerCollision(16, 8, ignore: true);
			Physics2D.IgnoreLayerCollision(16, 10, ignore: true);
			Physics2D.IgnoreLayerCollision(16, 11, ignore: true);
		}
		else if (!GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.PullGrab) && Physics2D.GetIgnoreLayerCollision(16, 8))
		{
			Physics2D.IgnoreLayerCollision(16, 8, ignore: false);
			Physics2D.IgnoreLayerCollision(16, 10, ignore: false);
			Physics2D.IgnoreLayerCollision(16, 11, ignore: false);
		}
	}

	private bool UPD_EXCEPTIONS_SPECIAL_SKILL_CANCELS()
	{
		if (Input.GetKeyDown(GC.IKey(GameCore.InputLib.AttackSkills)) || Input.GetKeyDown(GC.IKey(GameCore.InputLib.DefenseSkills)))
		{
			return true;
		}
		return false;
	}

	private IEnumerator waitDelayFastSkipValidate(float wait)
	{
		yield return new WaitForSeconds(wait);
		isDelayFastSkipValidated = true;
		crtDelayPressFastSkip = null;
	}

	public void StartDelayOfInputAction(float time)
	{
		if (crtInputActDelay != null)
		{
			StopCoroutine(crtInputActDelay);
		}
		crtInputActDelay = StartCoroutine(waitDelayActInput(time));
	}

	private IEnumerator waitDelayActInput(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtInputActDelay = null;
	}

	private void STATE_MACHINE_LOGIC()
	{
		switch (currentSkillState)
		{
		case playerSkillState.SlidingTackle:
			if (Mathf.RoundToInt(objRB.velocity.x) == 0 && crtDislodgeSlide == null)
			{
				crtDislodgeSlide = StartCoroutine(waitToDislodgeStuckSliding(0.5f));
			}
			if (!CheckCollisionGround())
			{
				currentPlayerState = playerState.Falling;
			}
			break;
		case playerSkillState.DrillDash:
			if (Mathf.RoundToInt(objRB.velocity.x) == 0 && crtDislodgeSlide == null)
			{
				crtDislodgeSlide = StartCoroutine(waitToDislodgeStuckSliding(0.5f));
			}
			break;
		}
		switch (currentPlayerState)
		{
		case playerState.Standing:
		case playerState.Running:
		case playerState.Crouching:
		case playerState.OnLadder:
		case playerState.OnLadderMoving:
			if (wasJumpButton && crtJumpPressed == null)
			{
				wasJumpButton = false;
			}
			if (!isFlameSwordReset)
			{
				isFlameSwordReset = true;
			}
			if (!isDrillDashReset)
			{
				isDrillDashReset = true;
			}
			if (!isForwardSlashReset)
			{
				isForwardSlashReset = true;
			}
			if (JumpAttackCounter > 0)
			{
				JumpAttackCounter = 0;
			}
			if (hasDoubleJumped)
			{
				hasDoubleJumped = false;
			}
			break;
		}
		switch (currentPlayerState)
		{
		case playerState.Jumping:
			if (playerCurrentYMove <= 0f || currentSkillState == playerSkillState.SlidingTackle)
			{
				currentPlayerState = playerState.Falling;
			}
			break;
		case playerState.Falling:
			if (Mathf.RoundToInt(objRB.velocity.y) == 0 && CheckCollisionGround() && !isPassTroughPlatform)
			{
				if (playerCurrentXMove == 0f)
				{
					currentPlayerState = playerState.Standing;
				}
				else
				{
					currentPlayerState = playerState.Running;
				}
				if (!isLoadDelay && !isHurt && (bool)GC && !GC.isPlayerInteractingLocked)
				{
					PlaySimpleLocalPlayerSounds("stepGround");
				}
			}
			else if ((CheckCollisionMovingPlatforms() || CheckCollisionEnemies()) && !isPassTroughPlatform)
			{
				if (playerCurrentXMove == 0f)
				{
					currentPlayerState = playerState.Standing;
				}
				else
				{
					currentPlayerState = playerState.Running;
				}
				if (!isLoadDelay && !isHurt && (bool)GC && !GC.isPlayerInteractingLocked)
				{
					PlaySimpleLocalPlayerSounds("stepGround");
				}
			}
			else if (Mathf.RoundToInt(objRB.velocity.y) == 0 && (float)Mathf.RoundToInt(playerCurrentYMove) == -3f && !CheckCollisionGround() && !isPassTroughPlatform)
			{
				playerCurrentYMove = -0.1f;
				base.transform.position = new Vector2(base.transform.position.x, base.transform.position.y - 0.25f);
			}
			if (isPressingDownwards && (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running))
			{
				currentPlayerState = playerState.Crouching;
			}
			break;
		case playerState.Standing:
			if (isPressingDownwards)
			{
				currentPlayerState = playerState.Crouching;
			}
			else if (!STATE_MACHINE_JUMP_OR_FALL() && playerCurrentXMove != 0f)
			{
				currentPlayerState = playerState.Running;
			}
			else if (currentSkillState == playerSkillState.Dash)
			{
				currentPlayerState = playerState.Running;
			}
			break;
		case playerState.Running:
			if (isPressingDownwards)
			{
				if (!isStairsCollTouch)
				{
					currentPlayerState = playerState.Crouching;
				}
			}
			else if (!STATE_MACHINE_JUMP_OR_FALL() && playerCurrentXMove == 0f && currentSkillState != playerSkillState.Dash)
			{
				currentPlayerState = playerState.Standing;
			}
			break;
		case playerState.Crouching:
			if (STATE_MACHINE_JUMP_OR_FALL())
			{
				if (playerCurrentYMove <= 0f)
				{
					currentPlayerState = playerState.Falling;
				}
				else
				{
					currentPlayerState = playerState.Jumping;
				}
				break;
			}
			if (currentSkillState == playerSkillState.SlidingTackle)
			{
				currentPlayerState = playerState.Running;
				break;
			}
			if (!isPressingDownwards && !STATE_MACHINE_JUMP_OR_FALL() && !CheckCollisionCeiling())
			{
				if (playerCurrentXMove == 0f)
				{
					currentPlayerState = playerState.Standing;
				}
				else
				{
					currentPlayerState = playerState.Running;
				}
			}
			if (isStairsCollTouch && (GC.movHackLeft || GC.movHackRight))
			{
				currentPlayerState = playerState.Running;
			}
			break;
		case playerState.OnLadder:
			if (playerCurrentYMove != 0f)
			{
				currentPlayerState = playerState.OnLadderMoving;
			}
			break;
		case playerState.OnLadderMoving:
			if (playerCurrentYMove == 0f)
			{
				currentPlayerState = playerState.OnLadder;
			}
			break;
		case playerState.Fainted:
			break;
		}
	}

	private void STATE_MACHINE_ANIMATION()
	{
		if (currentPlayerState == playerState.Crouching)
		{
			objAN.SetBool("isCrouching", value: true);
		}
		else
		{
			objAN.SetBool("isCrouching", value: false);
		}
		if (currentPlayerState == playerState.Standing)
		{
			objAN.SetBool("isStanding", value: true);
		}
		else
		{
			objAN.SetBool("isStanding", value: false);
		}
		if (currentPlayerState == playerState.Running)
		{
			objAN.SetBool("isRunning", value: true);
		}
		else
		{
			objAN.SetBool("isRunning", value: false);
		}
		if (currentPlayerState == playerState.Jumping)
		{
			objAN.SetBool("isJumping", value: true);
		}
		else
		{
			objAN.SetBool("isJumping", value: false);
		}
		if (currentPlayerState == playerState.Falling)
		{
			objAN.SetBool("isFalling", value: true);
		}
		else
		{
			objAN.SetBool("isFalling", value: false);
		}
		if (currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving)
		{
			objAN.SetBool("isOnLadder", value: true);
		}
		else
		{
			objAN.SetBool("isOnLadder", value: false);
		}
		if (currentPlayerState == playerState.OnLadderMoving)
		{
			int value = 1;
			if (playerCurrentYMove < 0f)
			{
				value = -1;
			}
			objAN.SetInteger("OnLadderMove", value);
		}
		else
		{
			objAN.SetInteger("OnLadderMove", 0);
		}
		if (isPressingUpwards)
		{
			objAN.SetBool("isPressingUpwards", value: true);
		}
		else
		{
			objAN.SetBool("isPressingUpwards", value: false);
		}
		objAN.SetInteger("skillStateID", (int)currentSkillState);
		if (!isDoingBasicAttackAnimation || (isDoingBasicAttackAnimation && canPlayerFlipNextAttack))
		{
			STATE_MACHINE_FLIP_CHARACTER();
			canPlayerFlipNextAttack = false;
		}
		if (currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving)
		{
			if (objRE.sortingOrder != valueLadderSO + 1)
			{
				objRE.sortingOrder = valueLadderSO + 1;
			}
		}
		else if (objRE.sortingOrder != originalSortingOrder && !isCustomSort)
		{
			objRE.sortingOrder = originalSortingOrder;
		}
	}

	private void STATE_MACHINE_FLIP_CHARACTER(bool forceDirection = false, float direction = 1f)
	{
		if (!isHurt)
		{
			if (playerCurrentXMove > 0f)
			{
				playerSetScale.x = 1f;
			}
			else if (playerCurrentXMove < 0f)
			{
				playerSetScale.x = -1f;
			}
			if (forceDirection)
			{
				playerSetScale.x = direction;
			}
			if (playerSetScale != base.transform.localScale)
			{
				base.transform.localScale = playerSetScale;
			}
		}
	}

	private bool STATE_MACHINE_JUMP_OR_FALL()
	{
		if (CheckCollisionMovingPlatforms())
		{
			if (crtJumpAssist != null)
			{
				StopCoroutine(crtJumpAssist);
			}
			crtJumpAssist = StartCoroutine(waitForJumpAssist(valueTimeOfJumpAssist));
			return false;
		}
		if (playerCurrentYMove > 0f && currentPlayerState != playerState.OnLadderMoving)
		{
			currentPlayerState = playerState.Jumping;
			if (isDoingBasicAttackAnimation && currentAtkState != playerBasicAttackState.JumpAtk && currentAtkState != playerBasicAttackState.UpwardsType2)
			{
				EndBasicAttackGeneric();
			}
			return true;
		}
		if (playerCurrentYMove <= 0f && !CheckCollisionGround() && currentPlayerState != playerState.OnLadder && currentPlayerState != playerState.OnLadderMoving)
		{
			if (currentPlayerState == playerState.Running && crtJumpAssist == null)
			{
				crtJumpAssist = StartCoroutine(waitForJumpAssist(valueTimeOfJumpAssist));
			}
			currentPlayerState = playerState.Falling;
			playerCurrentYMove = 0f;
			return true;
		}
		return false;
	}

	private void PerformJumpFallLogic()
	{
		if (isJumpAssit && !isHurt && currentPlayerState != playerState.Crouching)
		{
			ExecuteBasicJump();
			return;
		}
		switch (currentPlayerState)
		{
		case playerState.Standing:
		case playerState.Running:
			if (!isHurt)
			{
				ExecuteBasicJump();
			}
			break;
		case playerState.Crouching:
			CheckStairsFallDownThrough();
			if (CheckCollisionPassablePlatformGround())
			{
				switch (CheckCollisionGroundSpecialPlatPassHack())
				{
				case 1:
					base.transform.position = base.transform.position + new Vector3(0.2f, -0.1f, 0f);
					break;
				case 2:
					return;
				case 3:
					base.transform.position = base.transform.position + new Vector3(-0.2f, -0.1f, 0f);
					break;
				}
				isPassTroughPlatform = true;
				DisableColliderPlatEffector(0.1f, 0.2f);
				playerCurrentYMove = -0.05f;
				currentPlayerState = playerState.Falling;
			}
			break;
		case playerState.OnLadder:
		case playerState.OnLadderMoving:
			EndPlayerGrabLadder();
			currentPlayerState = playerState.Jumping;
			if (!isPressingDownwards)
			{
				ExecuteBasicJump();
			}
			else
			{
				playerCurrentYMove = 0.01f;
			}
			break;
		case playerState.Jumping:
		case playerState.Falling:
			if (ITEM_Behavior(GameCore.AccessoryBehavior.DoubleJump) && !hasDoubleJumped && !isHurt)
			{
				hasDoubleJumped = true;
				currentPlayerState = playerState.Jumping;
				ExecuteBasicJump();
				if ((bool)GC)
				{
					GC.InstantiateVFX(GameCore.VisualEffect.JumpSmoke, GroundCheck2.transform.position, "PlayerScript");
				}
			}
			break;
		case playerState.Fainted:
			break;
		}
	}

	private void ExecuteBasicJump()
	{
		playerCurrentYMove = playerMaxJumpForce;
		if (isJumpAssit)
		{
			JumpAssistHack();
		}
		GC.PlaySFX(GameCore.SFXBasic1.JumpStartRNG, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
	}

	private void CheckIfCanGrabLadder(string keyInpuy)
	{
		if (canGrabLadder && crtLadderDelay == null && currentPlayerState != playerState.OnLadder && currentPlayerState != playerState.OnLadderMoving && currentSkillState != playerSkillState.GroundPound)
		{
			if (keyInpuy == "UP" && base.transform.position.y <= valueLadderYTop)
			{
				StartPlayerGrabLadder();
			}
			else if (keyInpuy == "DOWN" && CheckCollisionPassablePlatformGround())
			{
				StartPlayerGrabLadder();
			}
		}
	}

	private void StartPlayerGrabLadder()
	{
		playerCurrentXMove = 0f;
		playerCurrentYMove = 0f;
		Vector2 vector = new Vector2(valueLadderX, base.transform.position.y);
		base.transform.position = vector;
		currentPlayerState = playerState.OnLadder;
		SetCollisionLayerPlatEffector(setCollisionON: false);
		EndBasicAttackGeneric();
		InvokeRepeating("InvokedCheckLadderPlayerGrabbedOnState", 0.05f, 0.05f);
	}

	private void EndPlayerGrabLadder()
	{
		CancelInvoke("InvokedCheckLadderPlayerGrabbedOnState");
		currentPlayerState = playerState.Falling;
		if (crtLadderDelay != null)
		{
			StopCoroutine(crtLadderDelay);
		}
		crtLadderDelay = StartCoroutine(waitForLadderDelay(valueLadderDelay));
	}

	private void InvokedCheckLadderPlayerGrabbedOnState()
	{
		Vector2 posOfObject = new Vector2(valueLadderX, 0f);
		CheckCollisionMovingPlatforms();
		if (currentPlayerState != playerState.OnLadder && currentPlayerState != playerState.OnLadderMoving)
		{
			CancelInvoke("InvokedCheckLadderPlayerGrabbedOnState");
		}
		else if (GC.CheckPlayerProximityToAnyObject(posOfObject, GameCore.PlayerProximityType.CustomXOnlyInverted, 0.25f))
		{
			EndPlayerGrabLadder();
		}
	}

	private IEnumerator waitForLadderDelay(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtLadderDelay = null;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		switch (other.gameObject.tag)
		{
		case "Enemy-Boss":
		case "Enemy-Basic":
			if (CheckPlayerValidState(other.gameObject))
			{
				PlayerHasTouchedBasicEnemy(other.gameObject);
			}
			break;
		case "Hazard-Basic":
			if (CheckPlayerValidState())
			{
				PlayerHasTouchedHazard(other.gameObject, other.collider.bounds.ClosestPoint(base.transform.position));
			}
			break;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.gameObject.tag)
		{
		case "Hazard-Basic":
			if (CheckPlayerValidState())
			{
				PlayerHasTouchedHazard(other.gameObject, other.bounds.ClosestPoint(base.transform.position));
			}
			break;
		case "Projectile-Basic":
			if (CheckPlayerValidState(null, other.gameObject) && other.gameObject.GetComponent<ProjectileLogic>().Originator == ProjectileLogic.OwnerOfProjectile.Enemy)
			{
				PlayerHasTouchedProjectile(other.gameObject);
			}
			break;
		case "Item-Basic":
		case "Item-Key":
		case "Item-Essence":
			PlayerHasTouchedItem(other.gameObject);
			break;
		case "NPC":
		{
			NpcLogic npcLogic = other?.GetComponent<NpcLogic>();
			if (npcLogic != null && npcLogic.currentNPCType != GameCore.NPCType.OBJ_Tutorial)
			{
				objTouchedNPC = other.gameObject;
				canInteractWithNPC = true;
			}
			break;
		}
		case "SavePoint":
			canSaveGame = true;
			if (!isLoadDelay && GC.playerCurrentHealth < (float)GC.playerMaxHealth && !GC.isPlayerFainted && !GC.playerIsBeingGrabed)
			{
				GC.HealThePlayer(100f);
			}
			break;
		case "Pitfall":
			if (GC.playerCurrentHealth > 0f && crtPitfallRespawn == null)
			{
				BeginPitfallLogic(other.gameObject);
			}
			break;
		case "Pitfall-Respaw":
			strCurrentPitfallCheckpoint = other.gameObject.name;
			break;
		case "Mech-Lever":
		{
			LeverSignal component = other.gameObject.GetComponent<LeverSignal>();
			currentLeverRef = component;
			canInteractWithLever = true;
			break;
		}
		case "EventTrigger":
			PlayerHasTouchedEventTriggerArea(other.gameObject.GetComponent<EventTriggerLogic>());
			break;
		case "CinematicMoveTarget":
			if (other.gameObject.name == strCurrentEventWayPointTarget)
			{
				PlayerHasReachedTargetPoint();
			}
			break;
		case "Enemy-Atk-Hitbox":
			if (CheckPlayerValidState(other.gameObject) && currentSkillState != playerSkillState.Dash)
			{
				PlayerHasTouchedEnemyAttackCollider(other.gameObject, other.bounds.ClosestPoint(base.transform.position), other.transform.parent.transform.position);
			}
			break;
		case "Enemy-Basic":
			objLastEnemyCollided = other.gameObject;
			break;
		case "Chest":
			canInteractWithChest = true;
			objLastChestCollided = other.gameObject;
			break;
		}
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		switch (other.gameObject.tag)
		{
		case "Enemy-Boss":
		case "Enemy-Basic":
			if (CheckPlayerValidState(other.gameObject))
			{
				PlayerHasTouchedBasicEnemy(other.gameObject);
			}
			break;
		case "Hazard-Basic":
			if (CheckPlayerValidState())
			{
				PlayerHasTouchedHazard(other.gameObject, other.collider.bounds.ClosestPoint(base.transform.position));
			}
			break;
		case "Projectile-Basic":
			if (CheckPlayerValidState() && other.gameObject.GetComponent<ProjectileLogic>().Originator == ProjectileLogic.OwnerOfProjectile.Enemy)
			{
				PlayerHasTouchedProjectile(other.gameObject);
			}
			break;
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		switch (other.gameObject.tag)
		{
		case "Enemy-Basic":
			if (CheckPlayerValidState(other.gameObject))
			{
				PlayerHasTouchedBasicEnemy(other.gameObject);
			}
			break;
		case "Hazard-Basic":
			if (CheckPlayerValidState())
			{
				PlayerHasTouchedHazard(other.gameObject, other.bounds.ClosestPoint(base.transform.position));
			}
			break;
		case "SpringBoard":
			if (playerCurrentYMove <= 0.5f)
			{
				SimpleSpringBoard component = other.gameObject.GetComponent<SimpleSpringBoard>();
				if ((object)component != null && component.canPushUp && !GC.isPlayerFainted && !GC.eventIsPlaying)
				{
					playerCurrentYMove = playerMaxJumpForce * 1.6f;
					other.gameObject.GetComponent<SimpleSpringBoard>()?.SpringBoardWasActivated();
				}
			}
			break;
		case "grabLadder":
			if (!canGrabLadder)
			{
				Collider2D component2 = other.GetComponent<Collider2D>();
				valueLadderSO = other.GetComponentInChildren<SpriteRenderer>().sortingOrder;
				valueLadderX = other.transform.position.x;
				valueLadderYTop = component2.bounds.center.y + component2.bounds.extents.y;
				valueLadderYBottom = component2.bounds.center.y - component2.bounds.extents.y;
				canGrabLadder = true;
			}
			break;
		case "Enemy-Atk-Hitbox":
			if (CheckPlayerValidState(other.gameObject) && other.gameObject.name.Contains("Petrify"))
			{
				PlayerHasTouchedEnemyAttackCollider(other.gameObject, other.bounds.ClosestPoint(base.transform.position), other.transform.parent.transform.position);
			}
			break;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		switch (other.gameObject.tag)
		{
		case "NPC":
			if (other.gameObject.activeInHierarchy)
			{
				InterruptDialogue(hasExitedColliderWithNPC: true);
			}
			break;
		case "SavePoint":
			canSaveGame = false;
			break;
		case "Mech-Lever":
			canInteractWithLever = false;
			currentLeverRef = null;
			break;
		case "Chest":
			canInteractWithChest = false;
			objLastChestCollided = null;
			break;
		case "grabLadder":
			canGrabLadder = false;
			break;
		case "EventTrigger":
			lastTouchedEventTrigger = null;
			break;
		}
	}

	private bool CheckPlayerValidState(GameObject enemyOBJ = null, GameObject projectileOBJ = null)
	{
		EnemyGeneralLogic enemyGeneralLogic = null;
		ProjectileLogic projectileLogic = null;
		if (enemyOBJ != null)
		{
			enemyGeneralLogic = enemyOBJ.GetComponent<EnemyGeneralLogic>();
		}
		if (projectileOBJ != null)
		{
			projectileLogic = projectileOBJ.GetComponent<ProjectileLogic>();
		}
		if (enemyGeneralLogic != null && enemyGeneralLogic.isDeath)
		{
			return false;
		}
		if (enemyGeneralLogic != null && DoIgnoreCollisionbetweenEnemyPlayer(enemyGeneralLogic))
		{
			return false;
		}
		if (enemyOBJ != null && CheckValidStateExceptions(enemyOBJ.name))
		{
			return true;
		}
		if (!isHurt && !isInvincible && !GC.playerIsInvincible && !isTakingCumulativeDamage && !GC.playerIsBeingGrabed && !GC.isPlayerFainted)
		{
			return true;
		}
		if (projectileLogic != null && GC.isPlayerFainted && projectileLogic.canCollideFainted)
		{
			return true;
		}
		return false;
	}

	private bool CheckValidStateExceptions(string objName)
	{
		if ((objName == "TNL-SideAttackRIGHT" || objName == "TNL-SideAttackLEFT") && !isHurt && !isInvincible && !GC.playerIsInvincible && !GC.playerIsBeingGrabed)
		{
			return true;
		}
		return false;
	}

	private bool DoIgnoreCollisionbetweenEnemyPlayer(EnemyGeneralLogic enemyScript)
	{
		if (enemyScript.isIgnoreCollPlayer)
		{
			float wait = 1f;
			ColliderToggleIgnoreBetweenPlayerEnemy(enemyScript.GetComponent<Collider2D>(), isIgnore: true);
			if (enemyScript.ignoreCollTimePeriod > 0f)
			{
				wait = enemyScript.ignoreCollTimePeriod;
			}
			if (crtIgnoreColl != null)
			{
				StopCoroutine(crtIgnoreColl);
			}
			crtIgnoreColl = StartCoroutine(waitForColliderIgnore(wait, enemyScript.GetComponent<Collider2D>()));
			return true;
		}
		return false;
	}

	private bool CheckCollisionGround(bool ignoreEnemyCheck = false, bool ignorePlatPassCheck = false)
	{
		bool flag = false;
		bool flag2 = false;
		if (isStairsCollTouch)
		{
			return true;
		}
		bool num = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck1.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck2.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag2 = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck3.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		if (num | flag | flag2)
		{
			return true;
		}
		if (!ignoreEnemyCheck && CheckCollisionEnemies())
		{
			return true;
		}
		if (!ignorePlatPassCheck && CheckCollisionPassablePlatformGround())
		{
			return true;
		}
		if (checkCollisionSolidProjectile())
		{
			return true;
		}
		return false;
	}

	private int CheckCollisionGroundSpecialPlatPassHack()
	{
		bool flag = false;
		bool flag2 = false;
		flag = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck1.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		bool num = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck2.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag2 = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck3.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		if (num && (flag | flag2))
		{
			return 2;
		}
		if (flag)
		{
			if (base.transform.localScale.x == -1f)
			{
				return 3;
			}
			return 1;
		}
		if (flag2)
		{
			if (base.transform.localScale.x == -1f)
			{
				return 1;
			}
			return 3;
		}
		return 0;
	}

	private bool CheckCollisionEnemies()
	{
		bool flag = false;
		bool flag2 = false;
		bool num = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck1.position), 1 << LayerMask.NameToLayer("LayerEnemies"));
		flag = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck2.position), 1 << LayerMask.NameToLayer("LayerEnemies"));
		flag2 = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck3.position), 1 << LayerMask.NameToLayer("LayerEnemies"));
		if (num | flag | flag2)
		{
			return true;
		}
		return false;
	}

	private bool CheckCollisionPassablePlatformGround()
	{
		if (IgnoreCollisionWithPassablePlatforms())
		{
			return false;
		}
		bool flag = false;
		bool flag2 = false;
		bool num = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck1.position), 1 << LayerMask.NameToLayer("LayerPlatEffector"));
		flag = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck2.position), 1 << LayerMask.NameToLayer("LayerPlatEffector"));
		flag2 = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck3.position), 1 << LayerMask.NameToLayer("LayerPlatEffector"));
		if (num | flag | flag2)
		{
			return true;
		}
		return false;
	}

	private bool checkCollisionSolidProjectile()
	{
		bool flag = false;
		bool flag2 = false;
		bool num = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck1.position), 1 << LayerMask.NameToLayer("LayerProjectile"));
		flag = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck2.position), 1 << LayerMask.NameToLayer("LayerProjectile"));
		flag2 = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck3.position), 1 << LayerMask.NameToLayer("LayerProjectile"));
		if (num | flag | flag2)
		{
			return true;
		}
		return false;
	}

	private bool IgnoreCollisionWithPassablePlatforms()
	{
		if (isPassTroughPlatform || Physics2D.GetIgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerPlatEffector")))
		{
			return true;
		}
		return false;
	}

	private bool CheckCollisionMovingPlatforms()
	{
		RaycastHit2D raycastHit2D = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck1.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		RaycastHit2D raycastHit2D2 = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck2.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		RaycastHit2D raycastHit2D3 = Physics2D.Linecast(COLL_GetGroundStartPointReference(), COLL_GetGroundEndPoint(GroundCheck3.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		if ((bool)raycastHit2D)
		{
			objMovPlatRB = raycastHit2D.rigidbody;
			return true;
		}
		if ((bool)raycastHit2D2)
		{
			objMovPlatRB = raycastHit2D2.rigidbody;
			return true;
		}
		if ((bool)raycastHit2D3)
		{
			objMovPlatRB = raycastHit2D3.rigidbody;
			return true;
		}
		objMovPlatRB = null;
		return false;
	}

	private bool CheckCollisionCeiling()
	{
		bool flag = false;
		bool flag2 = false;
		bool num = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck1.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck2.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag2 = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck3.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		if (num | flag | flag2)
		{
			return true;
		}
		bool num2 = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck1.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		flag = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck2.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		flag2 = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck3.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		if (num2 | flag | flag2)
		{
			return true;
		}
		return false;
	}

	private int CheckCollisionCeilingHackyJump()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		flag = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck1.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag2 = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck2.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		flag3 = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck3.position), 1 << LayerMask.NameToLayer("LayerPlatforms"));
		if (flag2 && (flag | flag3))
		{
			return 2;
		}
		if ((!flag3 && !flag2) & flag)
		{
			if (base.transform.localScale.x == -1f)
			{
				return 3;
			}
			return 1;
		}
		if ((!flag && !flag2) & flag3)
		{
			if (base.transform.localScale.x == -1f)
			{
				return 1;
			}
			return 3;
		}
		return 0;
	}

	private bool CheckCollisionCeilingMovingExclusive()
	{
		RaycastHit2D checkThis = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck1.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		RaycastHit2D checkThis2 = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck2.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		RaycastHit2D checkThis3 = Physics2D.Linecast(COLL_GetCeilingStartPointReference(), COLL_GetCeilingEndPoint(CeilingCheck3.position), 1 << LayerMask.NameToLayer("LayerPlatMoving"));
		if ((bool)checkThis)
		{
			if (ValidateAndExcludeCeilingDamageCases(ref checkThis))
			{
				return true;
			}
			return false;
		}
		if ((bool)checkThis2)
		{
			if (ValidateAndExcludeCeilingDamageCases(ref checkThis2))
			{
				return true;
			}
			return false;
		}
		if ((bool)checkThis3)
		{
			if (ValidateAndExcludeCeilingDamageCases(ref checkThis3))
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private bool ValidateAndExcludeCeilingDamageCases(ref RaycastHit2D checkThis)
	{
		if (checkThis.transform.gameObject.name.Contains("Mech") || checkThis.transform.gameObject.name.Contains("VinesWall"))
		{
			return false;
		}
		if (checkThis.transform.gameObject.tag == "moveable-collider")
		{
			return false;
		}
		if (checkThis.transform.gameObject.tag == "Hazard-Basic")
		{
			return false;
		}
		return true;
	}

	private bool COLL_RayCastFrontOfDrillDashOpenPath(Vector2 startPos, Vector2 endPos)
	{
		return Physics2D.Linecast(startPos, endPos, 1 << LayerMask.NameToLayer("LayerPlatforms"));
	}

	private void DisableColliderPlatEffector(float disableTimePlayer, float disableTimePassPlatColls)
	{
		Vector2 start = GroundCheck2.position;
		start.y += 0.25f;
		RaycastHit2D raycastHit2D = Physics2D.Linecast(start, GroundCheck1.position, 1 << LayerMask.NameToLayer("LayerPlatEffector"));
		RaycastHit2D raycastHit2D2 = Physics2D.Linecast(start, GroundCheck2.position, 1 << LayerMask.NameToLayer("LayerPlatEffector"));
		RaycastHit2D raycastHit2D3 = Physics2D.Linecast(start, GroundCheck3.position, 1 << LayerMask.NameToLayer("LayerPlatEffector"));
		if ((bool)raycastHit2D.collider)
		{
			Physics2D.IgnoreCollision(objBox2D, raycastHit2D.collider, ignore: true);
			Physics2D.IgnoreCollision(objPolygon2d, raycastHit2D.collider, ignore: true);
		}
		if ((bool)raycastHit2D2.collider)
		{
			Physics2D.IgnoreCollision(objBox2D, raycastHit2D2.collider, ignore: true);
			Physics2D.IgnoreCollision(objPolygon2d, raycastHit2D2.collider, ignore: true);
		}
		if ((bool)raycastHit2D3.collider)
		{
			Physics2D.IgnoreCollision(objBox2D, raycastHit2D3.collider, ignore: true);
			Physics2D.IgnoreCollision(objPolygon2d, raycastHit2D3.collider, ignore: true);
		}
		if (coroutPlayerPassTroughPlatform != null)
		{
			StopCoroutine(coroutPlayerPassTroughPlatform);
		}
		coroutPlayerPassTroughPlatform = StartCoroutine(waitPlayerFallTroughPassPlat(disableTimePlayer));
		if (crtWaitRestorePassPlatColliders != null)
		{
			StopCoroutine(crtWaitRestorePassPlatColliders);
		}
		crtWaitRestorePassPlatColliders = StartCoroutine(waitRestorePassPlatColliders(disableTimePassPlatColls, raycastHit2D.collider, raycastHit2D2.collider, raycastHit2D3.collider));
	}

	private void SetCollisionLayerPlatEffector(bool setCollisionON)
	{
		int num = LayerMask.NameToLayer("LayerPlayer");
		int num2 = LayerMask.NameToLayer("LayerPlatEffector");
		if (num < 0 || num > 31 || num2 < 0 || num2 > 31)
		{
			Debug.Log("layer out of range!!!");
		}
		else if (setCollisionON && Physics2D.GetIgnoreLayerCollision(num, num2))
		{
			Physics2D.IgnoreLayerCollision(num, num2, ignore: false);
		}
		else if (!setCollisionON && !Physics2D.GetIgnoreLayerCollision(num, num2))
		{
			Physics2D.IgnoreLayerCollision(num, num2, ignore: true);
		}
	}

	private bool CheckSideCollision()
	{
		bool flag = Physics2D.Linecast(base.transform.position, FrontCheckTop_main.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		bool flag2 = Physics2D.Linecast(base.transform.position, FrontCheckTop_sub.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		bool flag3 = Physics2D.Linecast(base.transform.position, FrontCheckBot_main.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		bool flag4 = Physics2D.Linecast(base.transform.position, FrontCheckBot_sub.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
		if ((flag && !flag2 && !flag4 && !flag3) || (((!flag && !flag2) & flag3) && !flag4))
		{
			return true;
		}
		return false;
	}

	private Vector2 COLL_GetGroundStartPointReference()
	{
		if (currentSkillState == playerSkillState.SlidingTackle || currentSkillState == playerSkillState.DrillDash)
		{
			return centerOfMassSliding.position;
		}
		return StartPointGroundCheck.position;
	}

	private Vector2 COLL_GetCeilingStartPointReference()
	{
		if (currentSkillState == playerSkillState.SlidingTackle || currentSkillState == playerSkillState.DrillDash)
		{
			return centerOfMassSliding.position;
		}
		return StartPointCeilingCheck.position;
	}

	private Vector2 COLL_GetGroundEndPoint(Vector2 refCheckThisPoint)
	{
		if (currentSkillState == playerSkillState.SlidingTackle || currentSkillState == playerSkillState.DrillDash)
		{
			refCheckThisPoint.y -= 0.25f;
		}
		return refCheckThisPoint;
	}

	private Vector2 COLL_GetCeilingEndPoint(Vector2 refCheckThisPoint)
	{
		if (currentSkillState == playerSkillState.SlidingTackle || currentSkillState == playerSkillState.DrillDash)
		{
			refCheckThisPoint.y -= 0.5f;
		}
		return refCheckThisPoint;
	}

	private void FXU_STAIRS_MAIN_SWITCHER()
	{
		if (CheckDiagonalStairsCollision())
		{
			isStairsCollTouch = true;
		}
		else
		{
			isStairsCollTouch = false;
		}
	}

	private bool CheckDiagonalStairsCollision()
	{
		if (!ValidateStairsCollision())
		{
			return false;
		}
		RaycastHit2D[] array = Physics2D.BoxCastAll(size: new Vector2(0.4f, 0.2f), origin: pointStairsCheck.position, angle: 0f, direction: Vector2.zero, distance: 1f, layerMask: 1 << LayerMask.NameToLayer("Staircase"));
		for (int i = 0; i < array.Length; i++)
		{
			int num = Mathf.RoundToInt(array[i].collider.transform.localScale.x);
			if (array.Length == 1)
			{
				_currStairColl = array[i].collider;
				intStairsFacing = num;
				return true;
			}
			if (num != intStairsFacing && isPressingUpwards && array[i].collider.transform.position.y > pointStairsCheck.position.y && GC.currentPlayerFacing == num)
			{
				_currStairColl = array[i].collider;
				intStairsFacing = num;
				return true;
			}
			if (num != intStairsFacing && isPressingDownwards && array[i].collider.transform.position.y < pointStairsCheck.position.y)
			{
				_currStairColl = array[i].collider;
				intStairsFacing = num;
				return true;
			}
			if (i == array.Length - 1)
			{
				if (intStairsFacing != 1 && intStairsFacing != -1)
				{
					intStairsFacing = num;
					_currStairColl = array[i].collider;
				}
				return true;
			}
		}
		return false;
	}

	private bool ValidateStairsCollision()
	{
		if (crtStairsFallThrough != null)
		{
			return false;
		}
		if (currentSkillState != playerSkillState.Null && currentSkillState != playerSkillState.Dash && currentSkillState != playerSkillState.SlidingTackle)
		{
			return false;
		}
		if (!isPressingUpwards && !isPressingDownwards)
		{
			if (currentPlayerState == playerState.Falling || isStairsCollTouch)
			{
				return true;
			}
			return false;
		}
		return true;
	}

	private bool ValidateStairsBehavior()
	{
		if (!isStairsCollTouch)
		{
			return false;
		}
		if (currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling || currentPlayerState == playerState.Crouching)
		{
			return false;
		}
		return true;
	}

	private void FXU_STAIRS_BEHAVIOR()
	{
		if (!ValidateStairsBehavior())
		{
			return;
		}
		float num = 5f;
		if (CheckOverlapOfBasePointDuringClimb())
		{
			base.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 0.05f, base.transform.position.z);
		}
		if (currentSkillState == playerSkillState.Dash || currentSkillState == playerSkillState.SlidingTackle)
		{
			num = 8.5f;
		}
		switch (intStairsFacing)
		{
		case 1:
			if (GC.movHackRight)
			{
				if (isPressingDownwards)
				{
					break;
				}
				vecPlayerMovement.x = num;
				vecPlayerMovement.y = num;
			}
			if (GC.movHackLeft && !isPressingUpwards)
			{
				CheckStairsPassOnDescent();
				vecPlayerMovement.x = 0f - num;
				vecPlayerMovement.y = 0f - num;
			}
			break;
		case -1:
			if (GC.movHackLeft)
			{
				if (isPressingDownwards)
				{
					break;
				}
				vecPlayerMovement.x = 0f - num;
				vecPlayerMovement.y = num;
			}
			if (GC.movHackRight && !isPressingUpwards)
			{
				CheckStairsPassOnDescent();
				vecPlayerMovement.x = num;
				vecPlayerMovement.y = 0f - num;
			}
			break;
		}
	}

	private bool CheckOverlapOfBasePointDuringClimb()
	{
		if (currentPlayerState == playerState.Running)
		{
			Vector2 point = pointStairsCheck.position;
			if (_currStairColl.OverlapPoint(point))
			{
				return true;
			}
			point.y += 0.2f;
			if (_currStairColl.OverlapPoint(point))
			{
				return true;
			}
		}
		return false;
	}

	private void CheckStairsFallDownThrough()
	{
		if (isStairsCollTouch && crtStairsFallThrough == null)
		{
			isStairsCollTouch = false;
			crtStairsFallThrough = StartCoroutine(waitStairsFallThrough(0.25f));
		}
	}

	private void CheckStairsPassOnDescent()
	{
		if (isPressingDownwards && CheckCollisionPassablePlatformGround())
		{
			isPassTroughPlatform = true;
			DisableColliderPlatEffector(0f, 0.2f);
		}
	}

	private IEnumerator waitStairsFallThrough(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtStairsFallThrough = null;
	}

	private void BeginBasicAttackLogic()
	{
		if (currentSkillState == playerSkillState.Dash)
		{
			EndSpecialSkill();
		}
		if (ITEM_Behavior(GameCore.AccessoryBehavior.WeaponWhip) && CheckSpecialItemUsageCases(550))
		{
			SetupItemThrow(GameCore.CollectibleType.WeaponWhip, 0.1f, 0.75f, new Vector2(1.75f, 0.5f), 0.75f, asChildOfPlayer: true, doVFXThrow: false, 0.4f);
			SPECIAL_ITEM_ATTACK_SLASH(20f);
			return;
		}
		if (isDoingBasicAttackAnimation)
		{
			if (canCombo && (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running))
			{
				if (isPressingUpwards)
				{
					currentAtkState = playerBasicAttackState.UpwardsType1;
				}
				switch (currentAtkState)
				{
				case playerBasicAttackState.Atk1:
					currentAtkState = playerBasicAttackState.Atk2;
					break;
				case playerBasicAttackState.Atk2:
					currentAtkState = playerBasicAttackState.Atk3;
					break;
				}
				SPECIAL_ITEM_ATTACK_SLASH();
				PlayCorrectAtackAnimation();
				objAN.SetInteger("BasicAttackCombo", (int)currentAtkState);
			}
			return;
		}
		switch (currentPlayerState)
		{
		case playerState.Standing:
		case playerState.Running:
			if (isPressingUpwards)
			{
				currentAtkState = playerBasicAttackState.UpwardsType1;
			}
			else
			{
				currentAtkState = playerBasicAttackState.Atk1;
			}
			break;
		case playerState.Jumping:
		case playerState.Falling:
			if (JumpAttackCounter < JumpAttackLimiter)
			{
				JumpAttackCounter++;
				if (isPressingUpwards)
				{
					currentAtkState = playerBasicAttackState.UpwardsType2;
				}
				else
				{
					currentAtkState = playerBasicAttackState.JumpAtk;
				}
				break;
			}
			return;
		case playerState.Crouching:
			if (ITEM_Behavior(GameCore.AccessoryBehavior.CrouchShield))
			{
				return;
			}
			currentAtkState = playerBasicAttackState.CrouchAtk;
			break;
		case playerState.OnLadder:
		case playerState.OnLadderMoving:
			if (isPressingUpwards)
			{
				currentAtkState = playerBasicAttackState.LadderAttackUpwards;
			}
			else
			{
				currentAtkState = playerBasicAttackState.LadderAttack;
			}
			break;
		}
		SPECIAL_ITEM_ATTACK_SLASH();
		PlayCorrectAtackAnimation();
		objAN.SetInteger("BasicAttackCombo", (int)currentAtkState);
		objAN.SetBool("isBasicAttack", value: true);
		canCombo = false;
		canPlayerFlipNextAttack = false;
		isDoingBasicAttackAnimation = true;
	}

	private void SPECIAL_ITEM_ATTACK_SLASH(float baseSpiriCost = 10f)
	{
		if (ITEM_Behavior(GameCore.AccessoryBehavior.SwordSlashProjectile) && GC.playerCurrentSpirit >= baseSpiriCost)
		{
			Vector2 vector = base.transform.position;
			bool flag = true;
			float num = 1.25f;
			float num2 = -0.25f;
			if (currentAtkState == playerBasicAttackState.CrouchAtk)
			{
				num -= 0.75f;
				num2 -= 0.75f;
			}
			if (currentAtkState == playerBasicAttackState.JumpAtk)
			{
				num2 -= 0.25f;
			}
			if (currentAtkState == playerBasicAttackState.LadderAttackUpwards || currentAtkState == playerBasicAttackState.UpwardsType1 || currentAtkState == playerBasicAttackState.UpwardsType2)
			{
				flag = false;
				num = 0f;
				num2 = 0.5f;
			}
			if (GC.currentPlayerFacing < 0)
			{
				num *= -1f;
			}
			vector.x += num;
			vector.y += num2;
			ProjectileLogic component = Object.Instantiate(miscOBJ.SmallSwordSlash, vector, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>();
			if (flag)
			{
				component.specialForce.x = GC.currentPlayerFacing;
			}
			else
			{
				component.specialForce.y = 1f;
			}
			SubtractSpirit(baseSpiriCost, displayPopupNumber: true, Vector3.zero);
		}
	}

	private void InitialComboFrameAtk()
	{
		canCombo = false;
		canPlayerFlipNextAttack = false;
		GC.SetNewBasicAttackVariation();
	}

	private void ToggleAttackCollider(string state)
	{
		if (state == "on")
		{
			ActivateAttackHitBox(activate: false, playerBasicAttackState.Null);
			ActivateAttackHitBox(activate: true, currentAtkState);
		}
		else
		{
			ActivateAttackHitBox(activate: false, currentAtkState);
		}
	}

	private void ListenForCombo()
	{
		canCombo = true;
	}

	private void CanFlipAttack()
	{
		canPlayerFlipNextAttack = true;
	}

	private void EndBasicAttack1()
	{
		if (currentAtkState == playerBasicAttackState.Atk2)
		{
			ActivateAttackHitBox(activate: false, playerBasicAttackState.Atk1);
		}
		else
		{
			EndBasicAttackGeneric();
		}
	}

	private void EndBasicAttack2()
	{
		if (currentAtkState == playerBasicAttackState.Atk3)
		{
			ActivateAttackHitBox(activate: false, playerBasicAttackState.Atk2);
		}
		else
		{
			EndBasicAttackGeneric();
		}
	}

	private void EndBasicAttackGeneric()
	{
		canCombo = false;
		canPlayerFlipNextAttack = false;
		currentAtkState = playerBasicAttackState.Null;
		ActivateAttackHitBox(activate: false, currentAtkState);
		objAN.SetInteger("BasicAttackCombo", (int)currentAtkState);
		objAN.SetBool("isBasicAttack", value: false);
		isDoingBasicAttackAnimation = false;
	}

	private void ActivateAttackHitBox(bool activate, playerBasicAttackState atkType)
	{
		switch (atkType)
		{
		case playerBasicAttackState.Atk1:
		case playerBasicAttackState.LadderAttack:
			objAtk1HitBox.SetActive(activate);
			return;
		case playerBasicAttackState.Atk2:
			objAtk2HitBox.SetActive(activate);
			return;
		case playerBasicAttackState.Atk3:
			objAtk3HitBox.SetActive(activate);
			return;
		case playerBasicAttackState.JumpAtk:
			objAtkJumpHitBox.SetActive(activate);
			return;
		case playerBasicAttackState.CrouchAtk:
			objAtkCrouchHitBox.SetActive(activate);
			return;
		case playerBasicAttackState.UpwardsType1:
		case playerBasicAttackState.LadderAttackUpwards:
			objAtkUpwards_type1.SetActive(activate);
			return;
		case playerBasicAttackState.UpwardsType2:
			objAtkUpwards_type2.SetActive(activate);
			return;
		}
		objAtk1HitBox.SetActive(value: false);
		objAtk2HitBox.SetActive(value: false);
		objAtk3HitBox.SetActive(value: false);
		objAtkJumpHitBox.SetActive(value: false);
		objAtkCrouchHitBox.SetActive(value: false);
		objAtkUpwards_type1.SetActive(value: false);
		objAtkUpwards_type2.SetActive(value: false);
	}

	private void PlayCorrectAtackAnimation()
	{
		switch (currentAtkState)
		{
		case playerBasicAttackState.Atk1:
			objAN.Play("Player-Attack1");
			break;
		case playerBasicAttackState.Atk2:
			objAN.Play("Player-Attack2");
			break;
		case playerBasicAttackState.Atk3:
			objAN.Play("Player-Attack3");
			break;
		case playerBasicAttackState.CrouchAtk:
			objAN.Play("Player-AttackCrouch");
			break;
		case playerBasicAttackState.JumpAtk:
			objAN.Play("Player-AttackJump");
			if ((bool)GC)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.JumpSmoke, GroundCheck2.transform.position, "PlayerScript");
			}
			break;
		case playerBasicAttackState.UpwardsType1:
			objAN.Play("Player-AttackUpwards_type1");
			break;
		case playerBasicAttackState.UpwardsType2:
			objAN.Play("Player-AttackUpwards_type2");
			if ((bool)GC)
			{
				GC.InstantiateVFX(GameCore.VisualEffect.JumpSmoke, GroundCheck2.transform.position, "PlayerScript");
			}
			break;
		case playerBasicAttackState.LadderAttack:
			objAN.Play("Player-LadderAttack");
			break;
		case playerBasicAttackState.LadderAttackUpwards:
			objAN.Play("Player-LadderAttackUpwards");
			break;
		}
		GC.PlaySFX(GameCore.SFXBasic1.Swing1, 0, checkForDuplicity: true, Vector2.zero, 0.75f, 1.25f);
	}

	private void TryToExecuteSpecialSkill(playerSkillState doSkill)
	{
		float baseCost = 0f;
		float duration = 0f;
		SKILL_OutBaseValues(doSkill, out baseCost, out duration);
		if (GC.configDifficultyType == GameCore.DifficultyType.Easy)
		{
			baseCost *= 0.5f;
		}
		if (doSkill == playerSkillState.IntentionalDressChange || doSkill == playerSkillState.RepairAllClothingLayer)
		{
			if (!CheckCanExecuteSkill(doSkill, baseCost) || IsUsingAnySkill(doSkill))
			{
				return;
			}
		}
		else if (!CheckBasicLogicPermissions() || !CheckCanExecuteSkill(doSkill, baseCost) || IsUsingAnySkill(doSkill))
		{
			return;
		}
		if (baseCost > 0f)
		{
			SubtractSpirit(baseCost, displayPopupNumber: true, Vector3.zero);
		}
		switch (doSkill)
		{
		case playerSkillState.Dash:
		{
			endDash = false;
			currentSkillState = doSkill;
			GC.PlaySFX(GameCore.SFXBasic1.SkillDash, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			if (!ITEM_Behavior(GameCore.AccessoryBehavior.masterDash))
			{
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: true);
			}
			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerProjectile"), ignore: true);
			objAN.speed *= 1.5f;
			ToggleShader2DEffect(state: true, 3);
			if (crtSpecialSkillForceEnd != null)
			{
				StopCoroutine(crtSpecialSkillForceEnd);
			}
			crtSpecialSkillForceEnd = StartCoroutine(waitForSkillExecution(1f, forceEnd: true));
			InvokeRepeating("Spawn2DSpriteTrail", 0.05f, 0.05f);
			Vector2 targetPosition = base.transform.position;
			targetPosition.y--;
			GC.InstantiateVFX(GameCore.VisualEffect.SkillDashCast, targetPosition, "ControllerPlayer::ExecuteSpecialSkills");
			if (ITEM_Behavior(GameCore.AccessoryBehavior.DashPhaseAttack))
			{
				miscOBJ.dashAutoAttack.SetActive(value: true);
			}
			break;
		}
		case playerSkillState.SlidingTackle:
		{
			IsInvincibleToggleSelf(setInvnicibleState: true);
			EndBasicAttackGeneric();
			currentSkillState = doSkill;
			ToggleShader2DEffect(state: true, 3);
			InvokeRepeating("Spawn2DSpriteTrail", 0.05f, 0.05f);
			ChangeTheActiveCollider(PlayerColliderType.Capsule2D_Slide);
			Vector2 targetPoint = HACK_ReturnSlidingTackleDirection(invertDirection: false);
			objSkillSlideTackle.SetActive(value: true);
			_hackySlidingSpeed = 9f;
			if (ITEM_Behavior(GameCore.AccessoryBehavior.SKILLEnhancedSlidingTackle))
			{
				_hackySlidingSpeed = 12f;
			}
			ForceMovePlayerToTargetPoint("", targetPoint, _hackySlidingSpeed, 10f, checkForStuck: true);
			GC.PlaySFX(GameCore.SFXBasic2.SkillSlidingTackle, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		}
		case playerSkillState.Shield:
			objAN.Play("SkillCastShield");
			playerCurrentXMove = 0f;
			playerCurrentYMove = 0f;
			IsInvincibleToggleSelf(setInvnicibleState: true);
			EndBasicAttackGeneric();
			currentSkillState = doSkill;
			GC.PlaySFX(GameCore.SFXBasic2.SkillShieldCast, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case playerSkillState.FlameSword:
		{
			float num = 1f;
			EndBasicAttackGeneric();
			objAN.Play("Player-SkillFlameSwordStart");
			currentSkillState = doSkill;
			if (base.transform.localScale.x != 1f)
			{
				num *= -1f;
			}
			flameSwordDuration = duration;
			flameSwordStartTime = Time.time;
			isSpecialFlameSwordMove = true;
			vecMasterOverridePlayerMovement.x = num;
			vecMasterOverridePlayerMovement.y = 1f;
			PlayLocalMayaVoicesLogic(LocalMayaVoiceType.atkSimple);
			GC.PlaySFX(GameCore.SFXBasic2.SkillFlameCast, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			if (currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving)
			{
				EndPlayerGrabLadder();
			}
			IsInvincibleToggleSelf(setInvnicibleState: true);
			break;
		}
		case playerSkillState.ForwardSlash:
			EndBasicAttackGeneric();
			isDoingBasicAttackAnimation = true;
			if (currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling)
			{
				JumpAttackCounter++;
				objAN.Play("Player-SkillForwardSlashAIR");
				if ((bool)GC)
				{
					GC.InstantiateVFX(GameCore.VisualEffect.JumpSmoke, GroundCheck2.transform.position, "PlayerScript");
				}
			}
			else
			{
				objAN.Play("Player-SkillForwardSlash");
			}
			PlayLocalMayaVoicesLogic(LocalMayaVoiceType.atkHeavy);
			GC.PlaySFX(GameCore.SFXBasic2.SkillSlashSpawn, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			currentSkillState = doSkill;
			isForwardSlashReset = false;
			break;
		case playerSkillState.GroundPound:
			EndBasicAttackGeneric();
			currentSkillState = doSkill;
			PlayLocalMayaVoicesLogic(LocalMayaVoiceType.atkHeavy);
			if (currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving)
			{
				EndPlayerGrabLadder();
			}
			currentPlayerState = playerState.Falling;
			objAN.Play("Player-SkillGroundPound");
			IsInvincibleToggleSelf(setInvnicibleState: true);
			GC.PlaySFX(GameCore.SFXBasic1.FallSword, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		case playerSkillState.DrillDash:
		{
			IsInvincibleToggleSelf(setInvnicibleState: true);
			EndBasicAttackGeneric();
			currentSkillState = doSkill;
			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: true);
			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerProjectile"), ignore: true);
			ToggleShader2DEffect(state: true, 3);
			InvokeRepeating("Spawn2DSpriteTrail", 0.05f, 0.05f);
			ChangeTheActiveCollider(PlayerColliderType.Capsule2D_Slide);
			Vector2 targetPoint2 = HACK_ReturnSlidingTackleDirection(invertDirection: false);
			targetPoint2.y = 0f;
			_hackySlidingSpeed = 14f;
			ForceMovePlayerToTargetPoint("", targetPoint2, _hackySlidingSpeed, 10f, checkForStuck: true);
			base.transform.position += new Vector3(0f, 0.5f, 0f);
			GC.PlaySFX(GameCore.SFXBasic1.DrillDash, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		}
		case playerSkillState.IntentionalDressChange:
			EndBasicAttackGeneric();
			currentSkillState = doSkill;
			GC.playerIsIntentionalDressStateChange = true;
			break;
		case playerSkillState.RepairAllClothingLayer:
			skillRepairStartTime = Time.time;
			objAN.Play("SkillRestoreClothArmor");
			playerCurrentXMove = 0f;
			playerCurrentYMove = 0f;
			vecMasterOverridePlayerMovement.x = 0f;
			vecMasterOverridePlayerMovement.y = 1f;
			IsInvincibleToggleSelf(setInvnicibleState: true);
			if (currentPlayerState == playerState.OnLadder || currentPlayerState == playerState.OnLadderMoving)
			{
				EndPlayerGrabLadder();
			}
			EndBasicAttackGeneric();
			currentSkillState = doSkill;
			GC.ToggleMusicMute(isMute: true, isFade: false, 0f);
			coreCAM.InitiateZoom(ControllerCamera.ZoomType.DressRipped, hasTimeLimit: true, 2.75f);
			GC.PlaySFX(GameCore.SFXBasic2.MayaClothTransform, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			break;
		}
		if (crtSpecialSkill != null)
		{
			StopCoroutine(crtSpecialSkill);
		}
		if (duration > 0f)
		{
			crtSpecialSkill = StartCoroutine(waitForSkillExecution(duration));
		}
	}

	private void EndSpecialSkill(bool forceEnd = false)
	{
		switch (currentSkillState)
		{
		case playerSkillState.Dash:
			if ((currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running || currentPlayerState == playerState.Crouching) | forceEnd)
			{
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: false);
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerProjectile"), ignore: false);
				ToggleShader2DEffect(state: false, 3);
				objAN.speed = originalAnimatorSpeed;
				CancelInvoke("Spawn2DSpriteTrail");
				endDash = false;
				hasJumpedDuringDash = false;
				currentSkillState = playerSkillState.Null;
				if (crtSpecialSkillForceEnd != null)
				{
					StopCoroutine(crtSpecialSkillForceEnd);
					crtSpecialSkillForceEnd = null;
				}
				if (miscOBJ.dashAutoAttack.activeInHierarchy)
				{
					miscOBJ.dashAutoAttack.SetActive(value: false);
				}
			}
			break;
		case playerSkillState.SlidingTackle:
			if ((currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling || !CheckCollisionCeiling()) | forceEnd)
			{
				ToggleShader2DEffect(state: false, 3);
				CancelInvoke("Spawn2DSpriteTrail");
				objSkillSlideTackle.SetActive(value: false);
				FinalizeForceMove();
				currentSkillState = playerSkillState.Null;
				IsInvincibleToggleSelf(setInvnicibleState: false);
				if (crtDislodgeSlide != null)
				{
					StopCoroutine(crtDislodgeSlide);
					crtDislodgeSlide = null;
				}
				if (currentPlayerState != playerState.Jumping && currentPlayerState != playerState.Falling)
				{
					playerCurrentXMove = 0f;
					playerCurrentYMove = 0f;
					currentPlayerState = playerState.Crouching;
				}
				STATE_MACHINE_ANIMATION();
			}
			break;
		case playerSkillState.Shield:
		{
			playerCurrentYMove = -0.05f;
			GameObject vfxShield = GC.InstantiateVFX(GameCore.VisualEffect.MayaSkillShield, centerOfMass.position, "Player:SkillShieldSpawnEffect", base.transform, returnOBJref: true);
			if (crtSkillShieldTimer != null)
			{
				StopCoroutine(crtSkillShieldTimer);
			}
			IsInvincibleToggleSelf(setInvnicibleState: false);
			isSkillShieldEffectActive = true;
			crtSkillShieldTimer = StartCoroutine(waitForShieldToEnd(10f, vfxShield, 1));
			currentSkillState = playerSkillState.Null;
			break;
		}
		case playerSkillState.FlameSword:
			if (IsInvoking("INVOKED_SpawnFlameSwordTrail"))
			{
				CancelInvoke("INVOKED_SpawnFlameSwordTrail");
			}
			isFlameSwordReset = false;
			isSpecialFlameSwordMove = false;
			vecMasterOverridePlayerMovement = Vector2.zero;
			playerCurrentYMove = 1f;
			currentSkillState = playerSkillState.Null;
			IsInvincibleToggleSelf(setInvnicibleState: false);
			break;
		case playerSkillState.ForwardSlash:
			isDoingBasicAttackAnimation = false;
			currentSkillState = playerSkillState.Null;
			break;
		case playerSkillState.GroundPound:
		{
			Vector2 vector = GroundCheck2.transform.position;
			vector.y += 0.25f;
			GC.CallShakeTheCamera(0.5f, 0.5f, 1f);
			Object.Instantiate(miscOBJ.projectileGroundPound, vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
			GC.InstantiateVFX(GameCore.VisualEffect.playerGroundPound, vector, "hehe");
			currentSkillState = playerSkillState.Null;
			MakePlayerInvincibleForSeconds(1f, doSpriteFlashing: false);
			break;
		}
		case playerSkillState.DrillDash:
			if ((!CheckCollisionCeiling() || !CheckCollisionGround()) | forceEnd)
			{
				isDrillDashReset = false;
				FinalizeForceMove();
				currentSkillState = playerSkillState.Null;
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: false);
				Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerProjectile"), ignore: false);
				ToggleShader2DEffect(state: false, 3);
				CancelInvoke("Spawn2DSpriteTrail");
				IsInvincibleToggleSelf(setInvnicibleState: false);
				if (crtDislodgeSlide != null)
				{
					StopCoroutine(crtDislodgeSlide);
					crtDislodgeSlide = null;
				}
				playerCurrentYMove = 0f;
				STATE_MACHINE_ANIMATION();
			}
			break;
		case playerSkillState.IntentionalDressChange:
			GC.playerIsIntentionalDressStateChange = false;
			currentSkillState = playerSkillState.Null;
			break;
		case playerSkillState.RepairAllClothingLayer:
			GC.ToggleMusicMute(isMute: false, isFade: true, 1f);
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, centerOfMass.position, "Player:RepairDress");
			GC.playerCurrentDressStateValidUnbroken = 0;
			ApplyDressStateChange(resetToFullDress: true);
			objAN.Play("Player-Idle");
			playerCurrentYMove = -0.05f;
			currentSkillState = playerSkillState.Null;
			skillRepairStartTime = 0f;
			IsInvincibleToggleSelf(setInvnicibleState: false);
			break;
		}
		if (crtSpecialSkill != null)
		{
			StopCoroutine(crtSpecialSkill);
		}
	}

	private IEnumerator waitForSkillExecution(float time, bool forceEnd = false)
	{
		yield return new WaitForSeconds(time);
		EndSpecialSkill(forceEnd);
		crtSpecialSkill = null;
		endDash = true;
	}

	public void EndSpecialSkillFromAnim(int doForceEnd = 0)
	{
		bool forceEnd = false;
		if (doForceEnd > 0)
		{
			forceEnd = true;
		}
		EndSpecialSkill(forceEnd);
	}

	public void ToggleFlameSwordTrail()
	{
		float repeatRate = 0.14f;
		if (GC.ACC_Check(GameCore.AccessoryBehavior.SKILLEnhancedFlameSword))
		{
			repeatRate = 0.09f;
		}
		if (!IsInvoking("INVOKED_SpawnFlameSwordTrail") && currentSkillState == playerSkillState.FlameSword)
		{
			InvokeRepeating("INVOKED_SpawnFlameSwordTrail", 0f, repeatRate);
		}
	}

	private void INVOKED_SpawnFlameSwordTrail()
	{
		GC.InstantiateVFX(GameCore.VisualEffect.FlameSwordTrail, positionFlameSwordTrail.position, "ControllerPlayer");
	}

	public void SpawnForwardSlashSkill()
	{
		float x = 1f;
		Vector2 vector = centerOfMass.position;
		if (base.transform.localScale.x == 1f)
		{
			vector.x += 0.5f;
		}
		else
		{
			vector.x -= 0.5f;
			x = -1f;
		}
		vector.y += 0.3f;
		GameObject obj = Object.Instantiate(miscOBJ.projectileSlash, vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
		vector.y = 0f;
		vector.x = x;
		obj.GetComponent<ProjectileLogic>().specialForce = vector;
	}

	private IEnumerator waitForShieldToEnd(float wait, GameObject vfxShield, int skillPhase)
	{
		yield return new WaitForSeconds(wait);
		GC.InstantiateVFX(GameCore.VisualEffect.ShieldBreak, vfxShield.transform.position, "PlayerController");
		GC.PlaySFX(GameCore.SFXBasic2.SkillShieldDissipates, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		switch (skillPhase)
		{
		case 1:
			vfxShield.GetComponent<Transform>().localScale = new Vector3(0.8f, 0.8f, 1f);
			crtSkillShieldTimer = StartCoroutine(waitForShieldToEnd(1f, vfxShield, 2));
			yield break;
		case 2:
			vfxShield.GetComponent<Transform>().localScale = new Vector3(0.5f, 0.5f, 1f);
			crtSkillShieldTimer = StartCoroutine(waitForShieldToEnd(1f, vfxShield, 3));
			yield break;
		}
		isSkillShieldEffectActive = false;
		if (vfxShield != null)
		{
			vfxShield.GetComponent<EffectLogic>().FinalFrameDisableEffect();
		}
		crtSkillShieldTimer = null;
	}

	private IEnumerator waitForLowSpiritEffect(float wait)
	{
		Vector2 targetPosition = centerOfMass.position;
		targetPosition.y--;
		GC.InstantiateVFX(GameCore.VisualEffect.NotEnoughSPToUseSkill, targetPosition, "ControllerPlayer:waitForLowSpiritEffect");
		yield return new WaitForSeconds(wait);
		crtNotEnoughSpirit = null;
	}

	private void SKILL_OutBaseValues(playerSkillState skill, out float baseCost, out float duration)
	{
		baseCost = 0f;
		duration = 1f;
		switch (skill)
		{
		case playerSkillState.Dash:
			baseCost = 20f;
			duration = 0.6f;
			baseCost = Mathf.RoundToInt(GC.ACC_DoMathItemAccStats(baseCost, GC.AccModifiers.DASH_cost_flat, GC.AccModifiers.DASH_cost_percent));
			if (ITEM_Behavior(GameCore.AccessoryBehavior.masterDash))
			{
				baseCost *= 0.25f;
			}
			break;
		case playerSkillState.DrillDash:
			baseCost = 30f;
			duration = 0.8f;
			baseCost = Mathf.RoundToInt(GC.ACC_DoMathItemAccStats(baseCost, GC.AccModifiers.DRILL_cost_flat, GC.AccModifiers.DRILL_cost_percent));
			break;
		case playerSkillState.SlidingTackle:
			baseCost = 30f;
			duration = 0.8f;
			baseCost = Mathf.RoundToInt(GC.ACC_DoMathItemAccStats(baseCost, GC.AccModifiers.TACKLE_cost_flat, GC.AccModifiers.TACKLE_cost_percent));
			if (ITEM_Behavior(GameCore.AccessoryBehavior.SKILLEnhancedSlidingTackle))
			{
				duration = 1f;
			}
			break;
		case playerSkillState.Shield:
			baseCost = 50f;
			duration = 3f;
			baseCost = Mathf.RoundToInt(GC.ACC_DoMathItemAccStats(baseCost, GC.AccModifiers.SHIELD_cost_flat, GC.AccModifiers.SHIELD_cost_percent));
			break;
		case playerSkillState.FlameSword:
			baseCost = 35f;
			duration = 0.5f;
			baseCost = Mathf.RoundToInt(GC.ACC_DoMathItemAccStats(baseCost, GC.AccModifiers.FLAME_cost_flat, GC.AccModifiers.FLAME_cost_percent));
			break;
		case playerSkillState.GroundPound:
			baseCost = 50f;
			duration = 10f;
			baseCost = Mathf.RoundToInt(GC.ACC_DoMathItemAccStats(baseCost, GC.AccModifiers.POUND_cost_flat, GC.AccModifiers.POUND_cost_percent));
			break;
		case playerSkillState.ForwardSlash:
			baseCost = 40f;
			duration = 1f;
			if (ITEM_Behavior(GameCore.AccessoryBehavior.SKILLEnhancePowerSlash))
			{
				baseCost *= 1.5f;
			}
			baseCost = Mathf.RoundToInt(GC.ACC_DoMathItemAccStats(baseCost, GC.AccModifiers.SLASH_cost_flat, GC.AccModifiers.SLASH_cost_percent));
			break;
		case playerSkillState.IntentionalDressChange:
			baseCost = 0f;
			duration = 2f;
			break;
		case playerSkillState.RepairAllClothingLayer:
			baseCost = 0f;
			duration = 3f;
			break;
		}
		if (ITEM_Behavior(GameCore.AccessoryBehavior.cursedHorned))
		{
			baseCost *= 0.8f;
		}
	}

	public bool CheckBasicLogicPermissions(bool ignorePaused = false)
	{
		if (GC.isGamePaused && !ignorePaused)
		{
			return false;
		}
		if (isHurt || GC.isPlayerFainted || GC.playerIsBeingGrabed || GC.isPlayerInteractingLocked || GC.isSceneTransitioning || EffectsThatBlocksInput() || crtPotionAnim != null)
		{
			return false;
		}
		return true;
	}

	private bool CheckInteractionAndDialoguePermissions()
	{
		if (isHurt || GC.isPlayerFainted || GC.isGamePaused || GC.isSceneTransitioning || EffectsThatBlocksInput() || crtPotionAnim != null)
		{
			return false;
		}
		return true;
	}

	private bool IsUsingAnySkill()
	{
		if (currentSkillState != playerSkillState.Null)
		{
			return true;
		}
		return false;
	}

	private bool IsUsingAnySkill(playerSkillState tryingToUseDifferentSkill)
	{
		if (currentSkillState != playerSkillState.Null)
		{
			if (tryingToUseDifferentSkill != playerSkillState.Null && CheckSkillCancelingExceptions(tryingToUseDifferentSkill))
			{
				return false;
			}
			return true;
		}
		return false;
	}

	private bool CheckCanExecuteSkill(playerSkillState thisSkill, float spiritCost)
	{
		if (GC.playerCurrentSpirit < spiritCost)
		{
			if (thisSkill == playerSkillState.IntentionalDressChange || thisSkill == playerSkillState.RepairAllClothingLayer)
			{
				return true;
			}
			if (crtNotEnoughSpirit == null)
			{
				crtNotEnoughSpirit = StartCoroutine(waitForLowSpiritEffect(0.5f));
				GC.PlaySFX(GameCore.SFXBasic2.SkillNotEnoughSP, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
			}
			return false;
		}
		if ((objAN.GetBool("isCrouching") || currentSkillState == playerSkillState.SlidingTackle) && CheckCollisionCeiling() && CheckCollisionGround(ignoreEnemyCheck: true) && thisSkill != playerSkillState.SlidingTackle)
		{
			return false;
		}
		switch (thisSkill)
		{
		case playerSkillState.Dash:
			if (GC.skillFlags.hasDash && (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running))
			{
				return true;
			}
			break;
		case playerSkillState.SlidingTackle:
			if (GC.skillFlags.hasSlidingTackle && (currentPlayerState == playerState.Standing || currentPlayerState == playerState.Running || currentPlayerState == playerState.Crouching))
			{
				return true;
			}
			break;
		case playerSkillState.Shield:
			if (ITEM_Behavior(GameCore.AccessoryBehavior.glassEarrings))
			{
				return false;
			}
			if (GC.skillFlags.hasShield && !isInvincible && currentPlayerState != playerState.OnLadder && currentPlayerState != playerState.OnLadderMoving)
			{
				return true;
			}
			break;
		case playerSkillState.FlameSword:
			if (GC.skillFlags.hasFlameSword && isFlameSwordReset)
			{
				return true;
			}
			break;
		case playerSkillState.ForwardSlash:
			if (GC.skillFlags.hasForwardSlash && isForwardSlashReset && currentPlayerState != playerState.OnLadder && currentPlayerState != playerState.OnLadderMoving)
			{
				return true;
			}
			break;
		case playerSkillState.GroundPound:
			if (GC.skillFlags.hasGroundPound && !CheckCollisionGround() && (currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling))
			{
				return true;
			}
			break;
		case playerSkillState.DrillDash:
			if (GC.skillFlags.hasDrillDash && !CheckCollisionGround() && isDrillDashReset && (currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling))
			{
				return true;
			}
			break;
		case playerSkillState.IntentionalDressChange:
		case playerSkillState.RepairAllClothingLayer:
			return true;
		}
		return false;
	}

	private bool CheckSkillCancelingExceptions(playerSkillState tryingToUseSkill)
	{
		switch (tryingToUseSkill)
		{
		case playerSkillState.FlameSword:
		case playerSkillState.GroundPound:
			if (currentSkillState == playerSkillState.Dash || currentSkillState == playerSkillState.DrillDash || currentSkillState == playerSkillState.SlidingTackle)
			{
				EndSpecialSkill(forceEnd: true);
				return true;
			}
			break;
		case playerSkillState.ForwardSlash:
			if (currentSkillState == playerSkillState.Dash || currentSkillState == playerSkillState.DrillDash)
			{
				EndSpecialSkill(forceEnd: true);
				return true;
			}
			break;
		case playerSkillState.DrillDash:
			if (currentSkillState == playerSkillState.Dash)
			{
				EndSpecialSkill(forceEnd: true);
				return true;
			}
			break;
		}
		return false;
	}

	private void PlayerHasTouchedBasicEnemy(GameObject objEnemy)
	{
		EnemyGeneralLogic component = objEnemy.GetComponent<EnemyGeneralLogic>();
		if ((!GC || GC.enemyViewGallery == null || !GC.enemyViewGallery.isRedirectToGalleryView) && (currentSkillState != playerSkillState.Dash || !ITEM_Behavior(GameCore.AccessoryBehavior.masterDash)) && !component.isStagger)
		{
			if (component.enmMisc.selfCollEnemyAttack && objEnemy.GetComponent<EnemyAttack>() != null)
			{
				PlayerHasTouchedEnemyAttackCollider(objEnemy, component.enmObjRefs.StartPointCenterMass.position, component.enmObjRefs.StartPointCenterMass.position);
			}
			else if (component != null && component.canDoTouchDamage)
			{
				GC.PlaySFX(GameCore.SFXBasic1.HitHurtTouch4, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
				ApplyDamageToPlayer(component.enmBaseStats.baseTouchDamage, Vector3.zero);
			}
		}
	}

	private void PlayerHasTouchedHazard(GameObject objHazard, Vector2 targetPos)
	{
		HazardLogic component = objHazard.GetComponent<HazardLogic>();
		if (component != null)
		{
			int dmg = Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(component.damagePercentage, GC.playerMaxHealth));
			ApplyDamageToPlayer(dmg, Vector3.zero);
			GC.PlaySFX(GameCore.SFXBasic1.HitHurtSpike1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			GC.InstantiateVFX(GameCore.VisualEffect.ClawRed, targetPos, "CONTROLLER_PLAYER:PlayerHasTouchedHazard");
		}
	}

	private void PlayerHasTouchedProjectile(GameObject objProjectile)
	{
		ProjectileLogic component = objProjectile.GetComponent<ProjectileLogic>();
		if (!(component == null))
		{
			if (component.selfCollEnemyAttack && objProjectile.GetComponent<EnemyAttack>() != null && component.objOriginator != null)
			{
				PlayerHasTouchedEnemyAttackCollider(objProjectile, component.transform.position, component.objOriginator.transform.position);
			}
			else
			{
				ApplyDamageToPlayer(component.touchDamage, Vector3.zero, afterEroEscaped: false, bypassDefense: false, onlyArmorDmg: false, component.specialTag, component.canCollideFainted);
			}
		}
	}

	private void PlayerHasTouchedEnemyAttackCollider(GameObject objectThis, Vector2 targetPos, Vector2 parentPosition)
	{
		EnemyAttack component = objectThis.GetComponent<EnemyAttack>();
		if (component == null || ((bool)GC && GC.enemyViewGallery != null && GC.enemyViewGallery.isRedirectToGalleryView))
		{
			return;
		}
		switch (component.attackType)
		{
		default:
			ApplyDamageToPlayer(component.dmgAmount, Vector3.zero);
			break;
		case GameCore.EnemyAttackType.SalamanderTongue:
			if (currentDressState != GameCore.PlayerDressState.None)
			{
				ApplyDamageToPlayer(Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(50f, GC.playerMaxDressDurability)), Vector3.zero, afterEroEscaped: false, bypassDefense: true, onlyArmorDmg: true);
			}
			else
			{
				vecGeneric = parentPosition;
				GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.PullGrab, 0.75f, "playerScriptCollision:SalamanderTongue");
			}
			GC.InstantiateVFX(GameCore.VisualEffect.Spit, targetPos, "CONTROLLER_PLAYER:PlayerHasTouchedEnemyAttackCollider");
			GC.PlaySFXEro(GameCore.EroSound.Piston4, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case GameCore.EnemyAttackType.GhostReachHand:
			if (currentDressState != GameCore.PlayerDressState.None)
			{
				ApplyDamageToPlayer(Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(100f, GC.playerMaxDressDurability)), Vector3.zero, afterEroEscaped: false, bypassDefense: true, onlyArmorDmg: true);
			}
			else if (GC.enemyGhostFutaSkillCasting != null)
			{
				GC.enemyGhostFutaSkillCasting.transform.position = base.transform.position;
				GC.enemyGhostFutaSkillCasting.GetComponent<EnemyGeneralLogic>().EnemyConcludeSkill();
			}
			break;
		case GameCore.EnemyAttackType.TentacleTreeGrab:
			if (currentDressState != GameCore.PlayerDressState.None)
			{
				ApplyDamageToPlayer(component.dmgAmount, Vector3.zero);
				break;
			}
			vecGeneric = parentPosition;
			GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.PullGrab, 0.75f, "playerScriptCollision:TentacleGrab");
			break;
		case GameCore.EnemyAttackType.PaintingTendrils:
			if (currentDressState != GameCore.PlayerDressState.None)
			{
				ApplyDamageToPlayer(Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(100f, GC.playerMaxDressDurability)), Vector3.zero, afterEroEscaped: false, bypassDefense: true, onlyArmorDmg: true);
				break;
			}
			vecGeneric = parentPosition;
			GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.PullGrab, 0.5f, "playerScriptCollision:PaintingTendrils");
			break;
		case GameCore.EnemyAttackType.WyvernFleshHook:
			if (currentDressState != GameCore.PlayerDressState.None)
			{
				ApplyDamageToPlayer(Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(100f, GC.playerMaxDressDurability)), Vector3.zero, afterEroEscaped: false, bypassDefense: true, onlyArmorDmg: true);
				break;
			}
			vecGeneric = objectThis.transform.position;
			GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.PullGrab, 1f, "playerScriptCollision:WyvernFleshHook");
			break;
		case GameCore.EnemyAttackType.BasicCharm:
			MayaHasBeenHitByCharmAttack(3f, parentPosition, "playerScriptCollision:EnemyAttackBasicCharm");
			break;
		case GameCore.EnemyAttackType.DogBoyCharm:
			MayaHasBeenHitByCharmAttack(5f, parentPosition, "playerScriptCollision:DogBoy");
			break;
		case GameCore.EnemyAttackType.BlueFoxAttractorBeam:
			ApplyDamageToPlayer(Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(100f, GC.playerMaxDressDurability)), Vector3.zero, afterEroEscaped: false, bypassDefense: true, onlyArmorDmg: true);
			vecGeneric = objectThis.transform.position;
			vecGeneric.y++;
			GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.PullGrab, 2f, "playerScriptCollision:BlueFoxAttractorBeam");
			break;
		case GameCore.EnemyAttackType.Petrify:
			if (!GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrifying) && !GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified))
			{
				objPetrifierEnemy = component.transform.parent.gameObject;
				if (objPetrifierEnemy != null && ((objPetrifierEnemy.transform.localScale.x == 1f && base.transform.localScale.x == -1f) || (objPetrifierEnemy.transform.localScale.x == -1f && base.transform.localScale.x == 1f)))
				{
					GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.Petrifying, _ptrTimer, "Controller_Player:PlayerHasTouchedAttackCollider");
				}
			}
			break;
		}
	}

	private void MayaHasBeenHitByCharmAttack(float debuffDuration, Vector2 positionOfEnemy, string debuffOriginator)
	{
		if (currentDressState != GameCore.PlayerDressState.None)
		{
			ApplyDamageToPlayer(Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(100f, GC.playerMaxDressDurability)), Vector3.zero, afterEroEscaped: false, bypassDefense: true, onlyArmorDmg: true);
			return;
		}
		vecGeneric = positionOfEnemy;
		GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.Charmed, debuffDuration, debuffOriginator);
	}

	public void ApplyDamageToPlayer(int dmg, Vector3 offsetDamageDisplay, bool afterEroEscaped = false, bool bypassDefense = false, bool onlyArmorDmg = false, string specialTag = "", bool faintedException = false)
	{
		if (GC.isPlayerFainted && !faintedException)
		{
			isHurt = false;
			objAN.SetBool("isHurt", value: false);
			return;
		}
		if (CheckRefCoreUI() && GC.isDisplayingDialogue)
		{
			InterruptDialogue(hasExitedColliderWithNPC: false);
		}
		checkEndCameraOffsetFromLever();
		CheckSpecialTagDuringDamage(specialTag);
		if (faintedException)
		{
			isHurt = false;
			objAN.SetBool("isHurt", value: false);
			return;
		}
		int num = dmg;
		int num2 = 0;
		if (isSkillShieldEffectActive)
		{
			float num3 = 0.7f;
			if (GC.ACC_Check(GameCore.AccessoryBehavior.SKILLEnhanceDefenseShield))
			{
				num3 = 0.5f;
			}
			dmg = (int)((float)dmg * num3);
		}
		if (!bypassDefense)
		{
			num = dmg - Mathf.RoundToInt(GC.GetDressStatValue(GameCore.DressStatValue.Defense, GC.playerCurrentBasicDefensePower));
			if (num <= 0)
			{
				num = 1;
			}
			num2 = dmg - num;
			if (num2 <= 0)
			{
				num2 = 1;
			}
		}
		GC.playerCurrentDressDurability -= Mathf.RoundToInt(GC.MATH_ResultPercentileOfTotal(num, GC.playerMaxHealth) * 1.5f);
		if (GC.playerCurrentDressDurability < 0)
		{
			GC.playerCurrentDressDurability = 0;
		}
		if (!onlyArmorDmg)
		{
			GC.playerCurrentHealth -= num;
		}
		else
		{
			num = 0;
		}
		DisplayPopupNumbersUI(0.75f, num, num2, offsetDamageDisplay, Color.red, Color.black, "-", "", GC.canShowBlockedDamage);
		ToggleSpriteColorFlashing(stateON: true, new Color(0.8f, 0.25f, 0.25f, 1f), 0.2f, 0.2f);
		ResolvePlayerDamage(num, afterEroEscaped);
	}

	private void ResolvePlayerDamage(int damage, bool isAfterEroEscaped)
	{
		if (GC.playerCurrentHealth > 0f)
		{
			bool flag = false;
			bool flag2 = false;
			cumulativeDamageTaken += damage;
			if (ITEM_Behavior(GameCore.AccessoryBehavior.WeaponWhip))
			{
				cumulativeDamageTaken = 9999999;
			}
			if (GC.playerIsBeingGrabed)
			{
				cumulativeDamageTaken = 0;
				return;
			}
			if (crtPotionAnim != null)
			{
				InterruptPotionDrinkAnimation();
				isHurt = true;
				flag = true;
			}
			EndDefenseDurabilityRegen();
			if ((GC.MATH_ResultPercentileOfTotal(damage, GC.playerMaxHealth) >= 20f || GC.MATH_ResultPercentileOfTotal(cumulativeDamageTaken, GC.playerMaxHealth) >= 20f) | isAfterEroEscaped)
			{
				cumulativeDamageTaken = 0;
				isTakingCumulativeDamage = false;
				isHurt = true;
				CheckDressDurability();
				flag = true;
			}
			else if (CheckDressDurability())
			{
				isHurt = true;
				flag = true;
			}
			else
			{
				flag2 = true;
			}
			if (flag2)
			{
				isTakingCumulativeDamage = true;
				if (coroutPlayerDamagedCumulative != null)
				{
					StopCoroutine(coroutPlayerDamagedCumulative);
				}
				coroutPlayerDamagedCumulative = StartCoroutine(waitForPlayerCumulativeDmg(0.75f));
			}
			if (flag)
			{
				float waittime = durationHurtAnim;
				if (!GC.playerIsBeingGrabed)
				{
					PlayLocalMayaVoicesLogic(LocalMayaVoiceType.hurtHard);
				}
				if (doDressChangeAfterHurt)
				{
					waittime = 1.15f;
				}
				if (coroutPlayerDamaged != null)
				{
					StopCoroutine(coroutPlayerDamaged);
				}
				coroutPlayerDamaged = StartCoroutine(waitForPlayerHurtAnimation(waittime));
			}
			else if (!GC.playerIsBeingGrabed)
			{
				PlayLocalMayaVoicesLogic(LocalMayaVoiceType.hurtSoft);
			}
			TRAIT_PAIN_BEHAVIOR(damage);
		}
		else
		{
			FaintedLogic();
		}
	}

	private void IsInvincibleToggleSelf(bool setInvnicibleState)
	{
		if (setInvnicibleState)
		{
			isInvincible = true;
		}
		else if (currentSkillState != playerSkillState.RepairAllClothingLayer)
		{
			isInvincible = false;
		}
	}

	public void MakePlayerInvincibleForSeconds(float seconds, bool doSpriteFlashing, bool doVicoryAnimAfterBoss = false)
	{
		IsInvincibleToggleSelf(setInvnicibleState: true);
		if (coroutPlayerInvincible != null)
		{
			StopCoroutine(coroutPlayerInvincible);
		}
		coroutPlayerInvincible = StartCoroutine(waitForPlayerInvincibility(seconds, doSpriteFlashing));
		if (doVicoryAnimAfterBoss)
		{
			objAN.Play("Player-WinFight");
			BlockPlayerInputs(4f);
			coreCAM.InitiateZoom(ControllerCamera.ZoomType.DressRipped, hasTimeLimit: true, 3.5f);
		}
	}

	private void PlayHurtAnimationAndDressBreakLogic()
	{
		if (doDressChangeAfterHurt)
		{
			ApplyDressStateChange(resetToFullDress: false, doSpriteChange: true);
			PlayLocalMayaVoicesLogic(LocalMayaVoiceType.hurtScream);
		}
		else
		{
			objAN.Play("Player-Hurt");
		}
	}

	private void CheckSpecialTagDuringDamage(string tag)
	{
		if (!(tag == "WitchCharm"))
		{
			if (tag == "ForestBossCharm")
			{
				GameObject gameObject = GameObject.FindGameObjectWithTag("Enemy-Boss");
				if (gameObject != null)
				{
					Vector2 positionOfEnemy = gameObject.transform.position;
					MayaHasBeenHitByCharmAttack(6f, positionOfEnemy, "ForestBossFight");
				}
			}
		}
		else
		{
			GameObject gameObject2 = GameObject.FindGameObjectWithTag("Enemy-Boss");
			if (gameObject2 != null)
			{
				Vector2 positionOfEnemy2 = gameObject2.transform.position;
				MayaHasBeenHitByCharmAttack(6f, positionOfEnemy2, "WitchBossFight");
			}
		}
	}

	private void FaintedLogic()
	{
		if (!GC.playerIsBeingGrabed && !GC.isPlayerGoneForGood)
		{
			GC.playerIsAllowedToBeGrabbed = false;
			crtWaitDelayAfterFainted = StartCoroutine(waitDelayAfterFainted(1.5f));
		}
		coreUI.ToggleUIElementsOnFainted();
		DelayRetry();
		CleanCertainEffectsOnFainted();
		EndBasicAttackGeneric();
		EndSpecialSkill(forceEnd: true);
		canUseTeleport = false;
		GC.isPlayerFainted = true;
		objAN.SetBool("isFainted", value: true);
		if (crtPotionAnim != null)
		{
			InterruptPotionDrinkAnimation();
		}
		if (!AreThereSpecialAnimationsOnDeath())
		{
			switch (currentDressState)
			{
			case GameCore.PlayerDressState.Full:
				objAN.Play("DressBreakHurt");
				GC.PlaySFX(GameCore.SFXBasic1.Armorbreak1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case GameCore.PlayerDressState.Half:
				ChangeAnimatorLayer(GameCore.PlayerDressID.Default, GameCore.PlayerDressState.None);
				objAN.Play("DressBreakHurt");
				GC.PlaySFX(GameCore.SFXBasic1.ClothRip1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				break;
			case GameCore.PlayerDressState.None:
				objAN.Play("Player-Hurt");
				break;
			}
		}
		currentPlayerState = playerState.Fainted;
		ChangeTheActiveCollider(PlayerColliderType.Box2D_Fainted);
		GC.playerCurrentSpirit = 0f;
		GC.playerCurrentDressDurability = 0;
		playerCurrentYMove = 0.5f;
		valueGravityCap /= 2.5f;
		valueGravityIncrement /= 2f;
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: true);
		if ((bool)EroFaintedObject)
		{
			EroFaintedObject.GetComponent<BoxCollider2D>().size = vecFaintedColliderSize;
			EroFaintedObject.GetComponent<BoxCollider2D>().offset = vecFaintedColliderOffset;
			refSelfFaintedEroOBJ = Object.Instantiate(EroFaintedObject, base.transform.position, Quaternion.identity, base.transform);
		}
		currentDressState = GameCore.PlayerDressState.None;
		GC.playerCurrentDressState = (int)currentDressState;
		GC.ToggleEroGrabbedTheme(toggleON: false);
		GC.PlayMusicTheme(GameCore.MusicTheme.Fainted, doFadeIn: false, 0f, doLoop: true);
		GC.DisplaySystemMessageUI(5, 0, 4f);
		checkEndCameraOffsetFromLever();
		GC.MINIMAP_Toggle(toggleCurrentState: false, setThisSpecificState: false);
		ToggleEssenceAttractor(state: false);
	}

	private IEnumerator waitDelayAfterFainted(float wait)
	{
		GC.playerIsAllowedToBeGrabbed = false;
		if ((bool)refSelfFaintedEroOBJ)
		{
			refSelfFaintedEroOBJ.GetComponent<BoxCollider2D>().enabled = false;
		}
		yield return new WaitForSeconds(wait);
		GC.playerIsAllowedToBeGrabbed = true;
		if ((bool)refSelfFaintedEroOBJ)
		{
			refSelfFaintedEroOBJ.GetComponent<BoxCollider2D>().enabled = true;
		}
	}

	public void PlayerWasEroGrabbed(Vector2 offsetStruggleBarPosition)
	{
		if (!GC.isPlayerFainted)
		{
			GC.ToggleEroGrabbedTheme(toggleON: true);
			ToggleEssenceAttractor(state: false);
		}
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.FruitForbidden))
		{
			GC.playerCurrentSpirit = 1f;
		}
		isPlayerAllowedToStruggle = true;
		GC.playerIsAllowedToBeGrabbed = false;
		canUseTeleport = false;
		objRE.enabled = false;
		objBox2D.enabled = false;
		objPolygon2d.enabled = false;
		objCapsule2D.enabled = false;
		if (crtPotionAnim != null)
		{
			InterruptPotionDrinkAnimation();
		}
		ResetThePlayer();
		objAN.Play("Player-Idle");
		objAN.SetBool("isCrouching", value: false);
		if (GC.canApplyAutoZoomOnEroGrab)
		{
			coreCAM.InitiateZoom(ControllerCamera.ZoomType.EroGrabAuto, hasTimeLimit: false, 0f);
		}
		ShowStruggleUI();
		float num = GC.MATH_ResultValueOfTotalPercentile(20f, GC.playerMaxSpirit);
		GC.playerCurrentSpirit -= num;
		DisplayPopupNumbersUI(1f, Mathf.RoundToInt(num), 0, Vector2.zero, new Color(0.15f, 0.9f, 0.9f), Color.black, "-");
		if (GC.isDisplayingMinimap)
		{
			GC.MINIMAP_Toggle(toggleCurrentState: false, setThisSpecificState: false);
			boolMinimapWasON = true;
		}
		InvokeRepeating("DoStruggleConstantDrain", 0f, freqDoSpiritRegen);
		checkEndCameraOffsetFromLever();
		CleanCertainEffectsOnEroGrab();
	}

	public void PlayerWasEroReleased(bool isMayaGoneForever, bool isGalleryEnemyRelease = false)
	{
		if (!GC.isPlayerFainted)
		{
			GC.ToggleEroGrabbedTheme(toggleON: false);
			ToggleEssenceAttractor(state: true);
		}
		GC.trySkipToBadEndStart = false;
		PlayerForbidStruggle();
		coreCAM.ManualEndZoomEffect(checkForFaintedToCancelAutoZoomChangeOnGrab: true);
		if (boolMinimapWasON)
		{
			GC.MINIMAP_Toggle(toggleCurrentState: false, setThisSpecificState: true);
			boolMinimapWasON = false;
		}
		if (GC.playerCurrentHealth > 0f && !isGalleryEnemyRelease)
		{
			isHurt = true;
			canUseTeleport = true;
			if (coroutPlayerDamaged != null)
			{
				StopCoroutine(coroutPlayerDamaged);
			}
			coroutPlayerDamaged = StartCoroutine(waitForPlayerHurtAnimation(durationHurtAnim));
			if (!objGrabReleaseEffect)
			{
				objGrabReleaseEffect = GC.ReturnVisualEffectPrefab(GameCore.VisualEffect.PunchGrabRelease);
			}
			if ((bool)GC.refOBJEffectsParent && (bool)objGrabReleaseEffect)
			{
				Object.Instantiate(objGrabReleaseEffect, base.transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
				GC.PlaySFX(GameCore.SFXBasic1.PunchGrabRelease, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 1.2f);
				PlayLocalMayaVoicesLogic(LocalMayaVoiceType.eroReleased);
			}
		}
		else
		{
			if (GC.isPlayerFainted && !AreThereSpecialAnimationsOnDeath())
			{
				objAN.Play("Player-Fainted");
			}
			if (GC.isPlayerFainted)
			{
				EroFaintedReleaseSpecialMovement();
			}
		}
		if (!isMayaGoneForever)
		{
			objRE.enabled = true;
			objBox2D.enabled = true;
		}
		else
		{
			if (arraySpecialAnimControl != null)
			{
				ENM_EroPhaseControl[] array = arraySpecialAnimControl;
				if (array == null || array.Length != 0)
				{
					goto IL_0206;
				}
			}
			objRE.enabled = false;
			objBox2D.enabled = false;
			Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerProjectile"), ignore: true);
			GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Petrified, "PLAYER.PlayerWasEroReleased().MayaIsGoneForeverPetrified", cleanAllEffects: true);
		}
		goto IL_0206;
		IL_0206:
		if (!isGalleryEnemyRelease)
		{
			if (crtDelayPlayerCanBeGrabbed != null)
			{
				StopCoroutine(crtDelayPlayerCanBeGrabbed);
			}
			crtDelayPlayerCanBeGrabbed = StartCoroutine(waitDelaySoPlayerCanBeGrabbed(1f));
		}
	}

	public void PlayerForbidStruggle()
	{
		isPlayerAllowedToStruggle = false;
		CancelInvoke("DoStruggleConstantDrain");
		HideStruggleUI();
	}

	private void EndFaintedEroWalk()
	{
		GC.playerIsAllowedToBeGrabbed = true;
		objAN.SetBool("isEroWalk", value: false);
	}

	private void EroFaintedReleaseSpecialMovement()
	{
		if (GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.LatexBodySuit) && GC.enemyGrabbingPlayer == GameCore.EnemyType.LatexMaiden)
		{
			int direction = 1;
			if (GC.MATH_RNG_INT(0, 1) == 0)
			{
				direction = -1;
			}
			playerCurrentYMove = 1.7f;
			ForceMovePlayerLeftRight(direction, 0.6f);
			GC.PlaySFX(GameCore.SFXBasic2.Bump1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			GC.PlaySFX(GameCore.SFXBasic1.Falling1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			GC.CallReferenceMayaVoiceGC("climax1", "Controller_Player:EroFaintedReleaseSpecial");
		}
	}

	private void TRAIT_PAIN_BEHAVIOR(int dmg)
	{
		if (GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.Masochist))
		{
			if (GC.MATH_RNG_ROLL(20))
			{
				GC.AddRemoveLustToPlayer(dmg / 20, doVFX: true);
			}
			return;
		}
		GC.permaTraitCountMasoch += dmg;
		if (GC.MATH_RNG_ROLL(10) && GC.permaTraitCountMasoch >= 250)
		{
			GC.TRAIT_CHECK_ADD(GameCore.MayaTraits.Masochist);
			GC.DisplaySystemMessageUI(7, 920, 7f, "", isFlavorCentralMessage: true);
		}
	}

	private void TRAIT_EXIB_BEHAVIOR()
	{
		if (!CheckBasicLogicPermissions())
		{
			return;
		}
		if (GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.Exibitionist))
		{
			if (currentDressState == GameCore.PlayerDressState.None && GC.MATH_RNG_ROLL(25))
			{
				TRAIT_HORNY_TICK();
			}
			if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Exhibitionist))
			{
				TRAIT_HORNY_TICK();
			}
			else if (GC.MATH_RNG_INT(1, 1000) <= 2 && GC.playerLust >= 50 && currentDressState == GameCore.PlayerDressState.Full)
			{
				TRAIT_EXHIB_REMOVE(1);
			}
			else if (GC.MATH_RNG_INT(1, 1000) <= 3 && GC.playerLust >= 75 && currentDressState == GameCore.PlayerDressState.Half)
			{
				TRAIT_EXHIB_REMOVE(2);
			}
		}
		else
		{
			if (currentDressState == GameCore.PlayerDressState.Half && GC.playerLust >= 30)
			{
				GC.permaTraitCountExib++;
			}
			if (currentDressState == GameCore.PlayerDressState.None && GC.playerLust >= 15)
			{
				GC.permaTraitCountExib += 2;
			}
			if ((GC.permaTraitCountExib > 90 && GC.MATH_RNG_ROLL(3) && currentDressState == GameCore.PlayerDressState.Half) || (GC.permaTraitCountExib > 90 && GC.MATH_RNG_ROLL(5) && currentDressState == GameCore.PlayerDressState.None))
			{
				GC.TRAIT_CHECK_ADD(GameCore.MayaTraits.Exibitionist);
				GC.DisplaySystemMessageUI(7, 921, 7f, "", isFlavorCentralMessage: true);
			}
		}
	}

	private void TRAIT_EXHIB_REMOVE(int dressState)
	{
		ForceOverrideDressState(dressState, isPlayerSelfRemoval: true);
		GC.DisplaySystemMessageUI(0, 181, 5f);
		GC.CallReferenceMayaVoiceGC("rng-Climax2", "ContrlPlayerTrait");
		GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.Exhibitionist, 60f, "PLAYER.TRAIT_EXIB");
	}

	private void TRAIT_HORNY_BEHAVIOR()
	{
		if (GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.Horny))
		{
			TRAIT_HORNY_TICK();
		}
		else if (GC.playerIsBeingGrabed && GC.enemyGrabbingEroPhase > 0 && !GC.isPlayerFainted)
		{
			GC.permaTraitCountHorny++;
			if (GC.MATH_RNG_ROLL(5) && GC.permaTraitCountHorny >= 30)
			{
				GC.TRAIT_CHECK_ADD(GameCore.MayaTraits.Horny);
				GC.DisplaySystemMessageUI(7, 922, 7f, "", isFlavorCentralMessage: true);
			}
		}
	}

	private void TRAIT_HORNY_TICK()
	{
		if (CheckBasicLogicPermissions() && GC.MATH_RNG_ROLL(5) && GC.playerLust < 100)
		{
			GC.AddRemoveLustToPlayer(1, doVFX: true);
		}
	}

	private void TRAIT_NYMPHO_BEHAVIOR()
	{
		if (GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.Nymphomaniac))
		{
			if (GC.MATH_RNG_INT(1, 1000) <= 5 && GC.playerLust > 50 && !GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Charmed) && currentDressState == GameCore.PlayerDressState.None && CheckBasicLogicPermissions())
			{
				vecGeneric = GC.MATH_GetPositionOfNearestEnemy(base.transform.position, 5f, 4);
				if (vecGeneric != Vector2.zero)
				{
					GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.Charmed, 5f, "ContrlPlayerTrait");
				}
			}
		}
		else if (GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.Masochist) && GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.Exibitionist) && GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.Horny) && GC.playerIsBeingGrabed && GC.enemyGrabbingEroPhase > 0 && !GC.isPlayerFainted)
		{
			GC.permaTraitCountNympho++;
			if (GC.MATH_RNG_ROLL(10) && GC.permaTraitCountNympho >= 30)
			{
				GC.TRAIT_CHECK_ADD(GameCore.MayaTraits.Nymphomaniac);
				GC.DisplaySystemMessageUI(7, 923, 7f, "", isFlavorCentralMessage: true);
			}
		}
	}

	private void TRAIT_VOREWHORE_BEHAVIOR()
	{
		if (!GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.VoreWhore) && GC.playerIsBeingGrabed && GC.enemyGrabbingEroPhase > 0 && !GC.isPlayerFainted && GC.enemyGrabbingEroFetishType == GameCore.FetishType.Vore && GC.MATH_RNG_ROLL(5))
		{
			GC.TRAIT_CHECK_ADD(GameCore.MayaTraits.VoreWhore);
			GC.DisplaySystemMessageUI(7, 924, 7f, "", isFlavorCentralMessage: true);
		}
	}

	private void DelayRetry()
	{
		if (crtDelayRetry == null)
		{
			crtDelayRetry = StartCoroutine(waitDelayRetry(0.2f));
		}
	}

	private IEnumerator waitForPlayerHurtAnimation(float waittime)
	{
		if (isHurt)
		{
			objAN.SetBool("isHurt", value: true);
			PlayHurtAnimationAndDressBreakLogic();
		}
		if (playerCurrentXMove > 0f)
		{
			playerCurrentXMove = -0.25f;
		}
		else if (playerCurrentXMove < 0f)
		{
			playerCurrentXMove = 0.25f;
		}
		else
		{
			playerCurrentXMove = 0f;
		}
		playerCurrentYMove = 1f;
		if (ITEM_Behavior(GameCore.AccessoryBehavior.WeaponWhip))
		{
			playerCurrentXMove = GC.MATH_RNG_INT_ARRAY(new int[2] { -1, 1 });
			playerCurrentYMove = 2f;
		}
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: true);
		yield return new WaitForSeconds(waittime);
		if (!GC.isGamePaused && Time.timeScale != 1f)
		{
			GC.ToggleSlowMotionOnDressBreak(actvate: false);
		}
		doDressChangeAfterHurt = false;
		objAN.SetBool("isHurt", value: false);
		isHurt = false;
		playerCurrentXMove = 0f;
		playerCurrentYMove = -0.1f;
		coroutPlayerDamaged = null;
		MakePlayerInvincibleForSeconds(1.25f, doSpriteFlashing: true);
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: false);
	}

	private IEnumerator waitForPlayerCumulativeDmg(float waittime)
	{
		yield return new WaitForSeconds(waittime);
		isTakingCumulativeDamage = false;
		coroutPlayerDamagedCumulative = null;
	}

	private IEnumerator waitForPlayerInvincibility(float waittime, bool doSpriteFlashing)
	{
		if (doSpriteFlashing)
		{
			ToggleSpriteColorFlashing(stateON: true, new Color(1f, 1f, 1f, 0.25f), 0.1f, waittime);
		}
		yield return new WaitForSeconds(waittime);
		IsInvincibleToggleSelf(setInvnicibleState: false);
		GC.playerIsAllowedToBeGrabbed = true;
		coroutPlayerInvincible = null;
	}

	private IEnumerator waitDelaySoPlayerCanBeGrabbed(float wait)
	{
		yield return new WaitForSeconds(wait);
		GC.playerIsAllowedToBeGrabbed = true;
		crtDelayPlayerCanBeGrabbed = null;
	}

	private IEnumerator waitDelayRetry(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtDelayRetry = null;
	}

	private bool CheckDressDurability()
	{
		if (GC.playerCurrentDressDurability <= 1 && currentDressState != GameCore.PlayerDressState.None)
		{
			doDressChangeAfterHurt = true;
			return true;
		}
		doDressChangeAfterHurt = false;
		return false;
	}

	public void ApplyDressStateChange(bool resetToFullDress = false, bool doSpriteChange = false)
	{
		bool flag = false;
		bool flag2 = false;
		string stateName = "";
		if (resetToFullDress)
		{
			currentDressState = GameCore.PlayerDressState.Full;
			stateName = "Player-Idle";
			flag2 = true;
		}
		else
		{
			switch (currentDressState)
			{
			case GameCore.PlayerDressState.Full:
				GC.PlaySFX(GameCore.SFXBasic1.Armorbreak1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				currentDressState = GameCore.PlayerDressState.Half;
				if (doSpriteChange)
				{
					stateName = "DressBreakHurt";
					flag2 = true;
					flag = true;
				}
				break;
			case GameCore.PlayerDressState.Half:
				GC.PlaySFX(GameCore.SFXBasic1.ClothRip1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
				GC.playerIsAllowedToBeGrabbed = false;
				currentDressState = GameCore.PlayerDressState.None;
				if (doSpriteChange)
				{
					stateName = "DressBreakHurt";
					flag = true;
				}
				break;
			}
		}
		GC.playerCurrentDressState = (int)currentDressState;
		GC.playerCurrentDressStateValidUnbroken = (int)currentDressState;
		ChangeAnimatorLayer(GameCore.PlayerDressID.Default, currentDressState);
		if (doSpriteChange)
		{
			objAN.Play(stateName);
		}
		if (flag2)
		{
			GC.ResetPlayerDressDurability();
		}
		if (flag)
		{
			GC.ToggleSlowMotionOnDressBreak(actvate: true);
			coreCAM.InitiateZoom(ControllerCamera.ZoomType.DressRipped, hasTimeLimit: true, 1f);
		}
		if (ITEM_Behavior(GameCore.AccessoryBehavior.HairFertility))
		{
			GC.ACC_RefreshListActiveAccessories();
		}
	}

	public void ForceOverrideDressState(int desiredState, bool isPlayerSelfRemoval = false)
	{
		if (isPlayerSelfRemoval)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, centerOfMass.position, "[PLAYER]:ForceDressState", base.transform);
			GC.InstantiateVFX(GameCore.VisualEffect.DressStateChange, centerOfMass.position, "[PLAYER]:ForceDressState", base.transform);
			TryToExecuteSpecialSkill(playerSkillState.IntentionalDressChange);
			ToggleSpriteColorFlashing(stateON: true, Color.clear, 1.4f, 1.5f);
			GC.PlaySFX(GameCore.SFXBasic2.EquipCloth, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		}
		switch (desiredState)
		{
		case 0:
			currentDressState = GameCore.PlayerDressState.Full;
			break;
		case 1:
			currentDressState = GameCore.PlayerDressState.Half;
			break;
		case 2:
			currentDressState = GameCore.PlayerDressState.None;
			break;
		}
		GC.playerCurrentDressState = (int)currentDressState;
		ChangeAnimatorLayer(GameCore.PlayerDressID.Default, currentDressState);
		objAN.Play("Player-Idle");
		if (ITEM_Behavior(GameCore.AccessoryBehavior.HairFertility))
		{
			GC.ACC_RefreshListActiveAccessories();
		}
	}

	private void ChangeAnimatorLayer(GameCore.PlayerDressID targetDress, GameCore.PlayerDressState targetState)
	{
		int num = 0;
		if (targetDress == GameCore.PlayerDressID.Default)
		{
			switch (targetState)
			{
			case GameCore.PlayerDressState.Full:
				num = 0;
				break;
			case GameCore.PlayerDressState.Half:
				num = 1;
				break;
			case GameCore.PlayerDressState.None:
				num = 2;
				break;
			}
		}
		for (int i = 0; i < objAN.layerCount; i++)
		{
			if (num == i)
			{
				objAN.SetLayerWeight(i, 1f);
			}
			else
			{
				objAN.SetLayerWeight(i, 0f);
			}
		}
	}

	public void RestoreAllArmorLayers()
	{
		TryToExecuteSpecialSkill(playerSkillState.RepairAllClothingLayer);
	}

	private void ShowStruggleUI()
	{
		if (GC.playerCurrentHealth > 0f)
		{
			currentStruggle = GC.MATH_RNG_INT(15, 30);
			coreUI.UpdateStatusBars(GameUI.BarType.Struggle, currentStruggle, maxStruggle);
			coreUI.STG_ToggleStruggleUI(setState: true, GC.currentEnemyGrabbingIsEroFatal);
		}
	}

	private void HideStruggleUI()
	{
		coreUI.STG_ToggleStruggleUI(setState: false);
	}

	private void TryMoveMayaStruggleBoard(GameCore.InputLib moveDirection, bool isHold = false)
	{
		if (GC.revertToOldStruggleBar)
		{
			if (isHold)
			{
				GlobalChangeStruggle(0.3f);
			}
			else
			{
				GlobalChangeStruggle(2.25f);
			}
		}
		else if (coreUI.STG_TryPerformMayaMove(moveDirection))
		{
			GlobalChangeStruggle(5f);
			SubtractSpirit(GC.MATH_ResultValueOfTotalPercentile(1f, GC.playerMaxSpirit), displayPopupNumber: true, new Vector3(0f, 1f, 0f));
		}
	}

	private void DoStruggleConstantDrain()
	{
		if (currentStruggle > 0f && currentStruggle < maxStruggle)
		{
			float num = -3f;
			if (GC.revertToOldStruggleBar)
			{
				num = -12f;
			}
			currentStruggle += num * freqDoSpiritRegen;
			if (currentStruggle < 0f)
			{
				currentStruggle = 0f;
			}
		}
		else if (currentStruggle >= maxStruggle)
		{
			currentStruggle = 0f;
			if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified))
			{
				PTR_StruggledFreeFromPetrified();
			}
			else
			{
				GC.playerCancelStopEro = true;
			}
		}
	}

	private bool ValidStruggleSituation()
	{
		if ((GC.playerIsBeingGrabed || GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified)) && isPlayerAllowedToStruggle && !GC.isGamePaused)
		{
			return true;
		}
		return false;
	}

	public void GlobalChangeStruggle(float amount)
	{
		if (amount > 0f && GC.TRAIT_CHECK_EXISTS(GameCore.MayaTraits.VoreWhore) && GC.enemyGrabbingEroFetishType == GameCore.FetishType.Vore)
		{
			amount /= 1.4f;
		}
		currentStruggle += amount;
		if (currentStruggle < 0f)
		{
			currentStruggle = 0f;
		}
	}

	private void RefreshUI()
	{
		coreUI.UpdateStatusBars(GameUI.BarType.Life, GC.playerCurrentHealth, GC.playerMaxHealth);
		coreUI.UpdateStatusBars(GameUI.BarType.Spirit, GC.playerCurrentSpirit, GC.playerMaxSpirit);
		coreUI.UpdateStatusBars(GameUI.BarType.Durability, GC.playerCurrentDressDurability, GC.playerMaxDressDurability);
		coreUI.UpdateStatusBars(GameUI.BarType.Struggle, currentStruggle, maxStruggle, MainCamera.GetComponent<Camera>());
	}

	private void DisplayPopupNumbersUI(float timeDurationDisplay, int primaryValue, int optionalValue, Vector3 offsetStartPos, Color mainColor, Color shadowColor, string addPrefix = "", string addSufix = "", bool localShowBlockedDMG = false)
	{
		offsetStartPos = ((!(offsetStartPos == Vector3.zero)) ? (base.transform.position + offsetStartPos) : base.transform.position);
		if ((bool)GC.refOBJEffectsParent)
		{
			DisplayDamage component = Object.Instantiate(objTextDamageUI, offsetStartPos, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<DisplayDamage>();
			component.GetComponent<DisplayDamage>().dmgPrimary = primaryValue;
			component.GetComponent<DisplayDamage>().dmgOptional = optionalValue;
			component.GetComponent<DisplayDamage>().showBlockedDamage = localShowBlockedDMG;
			component.GetComponent<DisplayDamage>().colorToDisplay = mainColor;
			component.GetComponent<DisplayDamage>().colorToDisplayShadow = shadowColor;
			component.GetComponent<DisplayDamage>().durationOfDisplay = timeDurationDisplay;
			component.GetComponent<DisplayDamage>().strPrefix = addPrefix;
			component.GetComponent<DisplayDamage>().strSufix = addSufix;
		}
	}

	public void ToggleShader2DEffect(bool state, int indexEffect, float setDuration = 0f)
	{
		if (!refShader2D)
		{
			return;
		}
		refShader2D.Toggle2DShader(state, indexEffect);
		if (!state && crtShader2D != null)
		{
			StopCoroutine(crtShader2D);
		}
		if (setDuration > 0f)
		{
			if (crtShader2D != null)
			{
				StopCoroutine(crtShader2D);
			}
			crtShader2D = StartCoroutine(waitForDeactivateShader(setDuration, indexEffect));
		}
	}

	private void ToggleSpriteColorFlashing(bool stateON, Color colorToFlash, float repeatFrequency = 0f, float flashTotalDuration = 0f)
	{
		string methodName = "INVOKED_SpriteColorFlashingRepeating";
		if (stateON && repeatFrequency > 0f)
		{
			colorFlashing = colorToFlash;
			InvokeRepeating(methodName, 0f, repeatFrequency);
			if (flashTotalDuration > 0f)
			{
				if (crtColorFlashing != null)
				{
					StopCoroutine(crtColorFlashing);
				}
				crtColorFlashing = StartCoroutine(waitForDeactivateFlashing(flashTotalDuration));
			}
		}
		else
		{
			CancelInvoke(methodName);
			objRE.color = colorOriginal;
		}
	}

	private void INVOKED_SpriteColorFlashingRepeating()
	{
		if (objRE.color != colorFlashing)
		{
			objRE.color = colorFlashing;
		}
		else
		{
			objRE.color = colorOriginal;
		}
	}

	private IEnumerator waitForDeactivateShader(float wait, int index)
	{
		yield return new WaitForSeconds(wait);
		ToggleShader2DEffect(state: false, index);
		crtShader2D = null;
	}

	private IEnumerator waitForDeactivateFlashing(float wait)
	{
		yield return new WaitForSeconds(wait);
		ToggleSpriteColorFlashing(stateON: false, Color.white);
		crtColorFlashing = null;
	}

	private void PlayerHasTouchedItem(GameObject objItem)
	{
		bool flag = false;
		bool delayDeactivation = false;
		float delayTime = 1f;
		int num = 1;
		CollectibleLogic classItem = objItem.GetComponent<CollectibleLogic>();
		if (classItem == null)
		{
			Debug.Log("Could not reference the script 'CollectibleLogic'!");
			return;
		}
		if (classItem.itemID == 0 && classItem.currentItemType != GameCore.CollectibleType.Quartz)
		{
			Debug.Log("This item type can't have an id == 0! Canceling pickup...");
			return;
		}
		if (classItem.forcedQuantity > 1)
		{
			num = classItem.forcedQuantity;
		}
		switch (classItem.currentItemType)
		{
		case GameCore.CollectibleType.Quartz:
			GC.TouchedEssenceCollectible(classItem);
			break;
		case GameCore.CollectibleType.HealthPotion:
		case GameCore.CollectibleType.SubPotions:
		case GameCore.CollectibleType.SubConsumables:
			if (classItem.itemID == 1 || classItem.itemID == 2)
			{
				classItem.itemID = 3;
			}
			flag = true;
			GC.PlaySFX(GameCore.SFXBasic1.PotionGet, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case GameCore.CollectibleType.ManaPotion:
			if (classItem.itemID == 4 || classItem.itemID == 5)
			{
				classItem.itemID = 6;
			}
			flag = true;
			GC.PlaySFX(GameCore.SFXBasic1.PotionGet, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case GameCore.CollectibleType.MetalDust:
		case GameCore.CollectibleType.FreshClothing:
			flag = true;
			break;
		case GameCore.CollectibleType.AttackUpgrade:
		case GameCore.CollectibleType.DefenseUpgrade:
		case GameCore.CollectibleType.HealthUpgrade:
		case GameCore.CollectibleType.SpiritUpgrade:
			if (GC.AddThisPowerUp(classItem.currentItemType))
			{
				string description = GC.clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == classItem.itemID)?.description;
				flag = true;
				GC.PlaySFX(GameCore.SFXBasic1.PowerupCollected, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
				GC.DisplaySystemMessageUI(123456, 0, 0f, description);
			}
			break;
		case GameCore.CollectibleType.SkillPickup:
			if (GC.CanAddThisSkill(classItem.itemID))
			{
				flag = true;
				GC.DisplaySystemMessageUI(4, classItem.itemID, 5f);
				GC.StartWaitingThenDoSomethingGeneric(GameCore.WaitGenerics.UniqueItemFanfare, 3f);
			}
			else
			{
				Debug.Log(objItem.name + " returned false on the GC.CanAddThisSkill!");
			}
			break;
		case GameCore.CollectibleType.ThrowDagger:
		case GameCore.CollectibleType.SmallRuneBomb:
		case GameCore.CollectibleType.Throwable:
			GC.PlaySFX(GameCore.SFXBasic1.Pickup1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			flag = true;
			break;
		case GameCore.CollectibleType.GeneralUpgrades:
			flag = true;
			break;
		case GameCore.CollectibleType.KeyItem:
		{
			string customString = GC.clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == classItem.itemID)?.name;
			flag = true;
			GC.DisplaySystemMessageUI(100, 0, 4f, customString);
			GC.StartWaitingThenDoSomethingGeneric(GameCore.WaitGenerics.UniqueItemFanfare, 3f);
			delayDeactivation = true;
			delayTime = 3.75f;
			Vector2 vector = base.transform.position;
			vector.x = objItem.transform.position.x;
			base.transform.position = vector;
			vector.y++;
			objItem.transform.position = vector;
			playerCurrentYMove = -10f;
			objAN.Play("Player-GetItem");
			BlockPlayerInputs(4f);
			coreCAM.InitiateZoom(ControllerCamera.ZoomType.DressRipped, hasTimeLimit: true, 3.5f);
			break;
		}
		default:
			Debug.Log("[CONTROLLER_PLAYER](PlayerHasTouchedItem): This collectible type was not identified inside this method! Add this new type as a CASE statement inside the switch!");
			break;
		}
		if (flag)
		{
			for (int num2 = 0; num2 < num; num2++)
			{
				GC.AddItemToPlayerInventory(classItem.itemID, GC.strCurrentActiveSceneName, objItem.GetComponent<CollectibleLogic>().originalPos);
			}
		}
		classItem.ItemDisableBehaviour(delayDeactivation, delayTime);
	}

	private void TryToUseItemInBeltSlot()
	{
		int indexCurrentBeltSlotActive = GC.indexCurrentBeltSlotActive;
		if ((CheckBasicLogicPermissions() && !IsUsingAnySkill() && GC.CheckBeltSlotValidity(indexCurrentBeltSlotActive) && CheckSpecialItemUsageCases(GC.arraySlotsOfBelt[indexCurrentBeltSlotActive].id)) || (!GC.isPlayerFainted && GC.playerIsBeingGrabed && GC.CheckBeltSlotValidity(indexCurrentBeltSlotActive) && GC.arraySlotsOfBelt[indexCurrentBeltSlotActive].id == 27))
		{
			UseItemEquipedOnSlot(GC.arraySlotsOfBelt[indexCurrentBeltSlotActive].id);
		}
		else if (!GC.isGamePaused)
		{
			GC.PlaySFX(GameCore.SFXBasic1.uiWrongError, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
	}

	private void UseItemEquipedOnSlot(int itemID)
	{
		bool flag = false;
		switch (itemID)
		{
		case 1:
		case 2:
		case 3:
			if ((CheckCollisionGround() || CheckCollisionMovingPlatforms()) && crtPotionAnim == null && !isHurt)
			{
				PlayerDrinkPotionAnimation(GameCore.PotionType.Health);
				flag = true;
			}
			break;
		case 4:
		case 5:
		case 6:
			if ((CheckCollisionGround() || CheckCollisionMovingPlatforms()) && crtPotionAnim == null && !isHurt)
			{
				PlayerDrinkPotionAnimation(GameCore.PotionType.Spirit);
				flag = true;
			}
			break;
		case 20:
		case 21:
		case 22:
		case 23:
		case 24:
			if ((CheckCollisionGround() || CheckCollisionMovingPlatforms()) && crtPotionAnim == null && !isHurt)
			{
				PlayerDrinkPotionAnimation(GameCore.PotionType.Brew, itemID, 1f, applyInvincibleDuringAnim: true);
				flag = true;
			}
			break;
		case 25:
		case 26:
		case 27:
		case 28:
		case 29:
		case 33:
			if ((CheckCollisionGround() || CheckCollisionMovingPlatforms()) && crtPotionAnim == null && !isHurt)
			{
				PlayerDrinkPotionAnimation(GameCore.PotionType.Eat, itemID, 1f, applyInvincibleDuringAnim: true);
				flag = true;
			}
			break;
		case 8:
			if (crtItemThrow == null)
			{
				SetupItemThrow(GameCore.CollectibleType.ThrowDagger, 0.15f, 1.1f, Vector2.zero);
				flag = true;
			}
			break;
		case 9:
			if (crtItemThrow == null)
			{
				SetupItemThrow(GameCore.CollectibleType.SmallRuneBomb, 0.25f, 0.75f, Vector2.zero);
				flag = true;
			}
			break;
		case 17:
			if (crtItemThrow == null)
			{
				SetupItemThrow(GameCore.CollectibleType.Throwable, 0.25f, 0.75f, Vector2.zero, 1f, asChildOfPlayer: false, doVFXThrow: true, 0.2f, miscOBJ.throwLance);
				flag = true;
			}
			break;
		case 18:
			if (crtItemThrow == null)
			{
				SetupItemThrow(GameCore.CollectibleType.Throwable, 0.35f, 0.55f, Vector2.zero, 1f, asChildOfPlayer: false, doVFXThrow: true, 0.5f, miscOBJ.throwBoomerang);
				flag = true;
			}
			break;
		case 19:
			if (crtItemThrow == null)
			{
				SetupItemThrow(GameCore.CollectibleType.Throwable, 0.15f, 1.1f, Vector2.zero, 1f, asChildOfPlayer: false, doVFXThrow: true, 0f, miscOBJ.throwHomingSphere);
				flag = true;
			}
			break;
		case 30:
			if (crtItemThrow == null)
			{
				SetupItemThrow(GameCore.CollectibleType.Throwable, 0.25f, 0.75f, Vector2.zero, 1f, asChildOfPlayer: false, doVFXThrow: true, 0f, miscOBJ.throwPoisonPowder);
				flag = true;
			}
			break;
		case 31:
			if (crtItemThrow == null)
			{
				SetupItemThrow(GameCore.CollectibleType.Throwable, 0.2f, 0.85f, Vector2.zero, 1f, asChildOfPlayer: false, doVFXThrow: true, 0f, miscOBJ.throwRagePepper);
				flag = true;
			}
			break;
		case 32:
			if (crtItemThrow == null)
			{
				SetupItemThrow(GameCore.CollectibleType.Throwable, 0.4f, 0.65f, Vector2.zero, 1f, asChildOfPlayer: false, doVFXThrow: true, 0f, miscOBJ.throwEntropicBomb);
				flag = true;
			}
			break;
		case 15:
			if (GC.playerCurrentDressDurability < GC.playerMaxDressDurability && !IsInvoking("INVOKED_DurabilityRegen"))
			{
				BeginDefenseDurabilityRegen(10f, 200f);
				flag = true;
			}
			break;
		case 16:
			if (CheckValidToRepairAllClothingLayer())
			{
				RestoreAllArmorLayers();
				flag = true;
			}
			break;
		default:
			Debug.Log("[PLAYER]UseItemEquipedOnSlot: This itemID: (" + itemID + ") was not expected on the switch! Need to insert the item use logic for this!");
			return;
		}
		if (flag)
		{
			GC.RemoveItemFromPlayerInventory(itemID);
		}
		else if (crtItemThrow == null)
		{
			GC.PlaySFX(GameCore.SFXBasic1.uiWrongError, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		}
	}

	public void SetupItemThrow(GameCore.CollectibleType thisItem, float spawnDelay, float animSpeed, Vector2 offsetSpawn, float crouchLowerY = 1f, bool asChildOfPlayer = false, bool doVFXThrow = true, float waitAfterThrow = 0f, GameObject hackOBJ = null)
	{
		GameObject thisOBJ = null;
		if (waitAfterThrow > 0f && crtAfterItemThrow != null)
		{
			return;
		}
		switch (thisItem)
		{
		case GameCore.CollectibleType.ThrowDagger:
			thisOBJ = miscOBJ.throwDagger;
			break;
		case GameCore.CollectibleType.SmallRuneBomb:
			thisOBJ = miscOBJ.throwSmallRuneBomb;
			break;
		case GameCore.CollectibleType.WeaponWhip:
			thisOBJ = miscOBJ.WeaponWhip;
			break;
		case GameCore.CollectibleType.Throwable:
			thisOBJ = hackOBJ;
			break;
		}
		if (crtItemThrow != null || !GC.refOBJEffectsParent)
		{
			return;
		}
		switch (currentPlayerState)
		{
		case playerState.Crouching:
			objAN.Play("Player-ThrowItem_Crouch", -1, 0f);
			break;
		case playerState.Jumping:
		case playerState.Falling:
			if (JumpAttackCounter < JumpAttackLimiter)
			{
				JumpAttackCounter++;
				objAN.Play("Player-ThrowItem_Jump", -1, 0f);
				GC.InstantiateVFX(GameCore.VisualEffect.JumpSmoke, GroundCheck2.transform.position, "PlayerScript");
				break;
			}
			return;
		case playerState.OnLadder:
		case playerState.OnLadderMoving:
			objAN.Play("Player-ThrowItem_Jump", -1, 0f);
			break;
		default:
			objAN.Play("Player-ThrowItem_Stand", -1, 0f);
			break;
		}
		objAN.SetFloat("speedAnimMisc", animSpeed);
		crtItemThrow = StartCoroutine(waitItemThrow(spawnDelay, thisOBJ, offsetSpawn, crouchLowerY, asChildOfPlayer, doVFXThrow));
		if (waitAfterThrow > 0f)
		{
			crtAfterItemThrow = StartCoroutine(waitItemThrowAfter(waitAfterThrow));
		}
	}

	private bool CheckSpecialItemUsageCases(int itemID)
	{
		if (((uint)(itemID - 8) <= 1u || (uint)(itemID - 17) <= 2u || itemID == 550) && currentPlayerState == playerState.Crouching && ITEM_Behavior(GameCore.AccessoryBehavior.CrouchShield))
		{
			return false;
		}
		return true;
	}

	public bool CheckValidToRepairAllClothingLayer()
	{
		if (currentSkillState != playerSkillState.RepairAllClothingLayer && GC.playerCurrentDressStateValidUnbroken > 0)
		{
			return true;
		}
		return false;
	}

	private IEnumerator waitItemThrow(float wait, GameObject thisOBJ, Vector2 offsetSpawnPos, float crouchYoffset = 1f, bool childUnderPlayer = false, bool vfxThrow = true)
	{
		yield return new WaitForSeconds(wait);
		Transform parent = GC.refOBJEffectsParent.transform;
		Vector3 position = base.transform.position;
		Vector2 zero = Vector2.zero;
		bool flag = false;
		if (childUnderPlayer)
		{
			parent = base.transform;
		}
		if (currentPlayerState == playerState.Crouching)
		{
			position.y -= crouchYoffset;
		}
		else
		{
			position.y -= 0.2f;
		}
		if (base.transform.localScale.x == 1f)
		{
			zero.x = 1f;
			position.x += 0.3f;
		}
		else
		{
			zero.x = -1f;
			position.x -= 0.3f;
			offsetSpawnPos.x *= -1f;
		}
		if (thisOBJ == miscOBJ.throwLance)
		{
			flag = true;
		}
		if (flag)
		{
			if (isPressingUpwards)
			{
				zero.y = 1f;
				zero.x = 0f;
				position.x = base.transform.position.x;
			}
			else if (isPressingDownwards && currentPlayerState != playerState.Crouching)
			{
				zero.y = -1f;
				zero.x = 0f;
				position.x = base.transform.position.x;
				position.y -= 0.5f;
			}
		}
		position.x += offsetSpawnPos.x;
		position.y += offsetSpawnPos.y;
		if (thisOBJ != null)
		{
			thisOBJ = Object.Instantiate(thisOBJ, position, Quaternion.identity, parent);
			thisOBJ.GetComponent<ProjectileLogic>().specialForce = zero;
		}
		if (vfxThrow)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.ThrowItem, position, "PlayerController");
		}
		crtItemThrow = null;
	}

	private IEnumerator waitItemThrowAfter(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtAfterItemThrow = null;
	}

	private void TryToOpenChest()
	{
		if (objLastChestCollided != null && objLastChestCollided.activeInHierarchy)
		{
			ContainerLogic component = objLastChestCollided.GetComponent<ContainerLogic>();
			if (!component.isThisChestOpen && component.gameObject.name == objLastChestCollided.name)
			{
				component.CHEST_OpenLogic(stateChestColl: false, setAnimatorChestOpen: true, doItemSpawnLogic: true);
			}
		}
	}

	private bool ITEM_Behavior(GameCore.AccessoryBehavior thisBehavior)
	{
		if (GC.listAccBehavior.Exists((GameCore.AccessoryBehavior x) => x == thisBehavior))
		{
			return true;
		}
		return false;
	}

	private float ITEM_SkillAffect(playerSkillState thisSkill, playerSkillAffect affect)
	{
		switch (thisSkill)
		{
		case playerSkillState.Dash:
			if (affect == playerSkillAffect.Speed)
			{
				return GC.MATH_NormalizeFloatPercent(GC.AccModifiers.DASH_speed_mult, zeroReturnOne: true);
			}
			break;
		}
		return 0f;
	}

	private void ITEM_RecurringEffectChecks()
	{
		if (GC.playerIsCrouching)
		{
			if (ITEM_Behavior(GameCore.AccessoryBehavior.CrouchShield) && !miscOBJ.crouchShielding.activeInHierarchy)
			{
				miscOBJ.crouchShielding.SetActive(value: true);
			}
		}
		else if (miscOBJ.crouchShielding.activeInHierarchy)
		{
			miscOBJ.crouchShielding.SetActive(value: false);
		}
	}

	private void BeginPitfallLogic(GameObject pitfall)
	{
		GC.CallReferenceMayaVoiceGC("rng-Scream2", "[Controller_Player:BeginPitfallLogic()]");
		if (!ApplyDamagePitfall(20f))
		{
			FaintedLogic();
			GC.isPlayerGoneForGood = true;
			return;
		}
		Vector2 respawnPosition = GetRespawnPosition(pitfall.transform);
		GC.isPlayerInteractingLocked = true;
		coreUI.FadeInOutBlackScreen(FadeIntoBlack: true, 0.5f);
		if (crtPitfallRespawn != null)
		{
			StopCoroutine(crtPitfallRespawn);
		}
		crtPitfallRespawn = StartCoroutine(waitForPitfallRespawn(0.6f, respawnPosition));
		if (crtPitRespawnControlDelay != null)
		{
			StopCoroutine(crtPitRespawnControlDelay);
		}
		crtPitRespawnControlDelay = StartCoroutine(waitForDelayOnControlsAfterPitfall(1f));
	}

	private bool ApplyDamagePitfall(float percentDamage)
	{
		IsInvincibleToggleSelf(setInvnicibleState: false);
		GC.playerCurrentHealth -= (int)GC.MATH_ResultValueOfTotalPercentile(percentDamage, GC.playerMaxHealth);
		if (GC.playerCurrentHealth > 0f)
		{
			return true;
		}
		return false;
	}

	private Vector2 GetRespawnPosition(Transform pitfall)
	{
		Vector2 result = Vector2.zero;
		int num = 0;
		foreach (Transform item in pitfall)
		{
			if (num == 0 || strCurrentPitfallCheckpoint == item.name)
			{
				result = item.position;
			}
			num++;
		}
		if (num == 0)
		{
			result = pitfall.position;
			result.y += 10f;
		}
		return result;
	}

	private IEnumerator waitForPitfallRespawn(float waittime, Vector2 respawnPoint)
	{
		yield return new WaitForSeconds(waittime);
		GC.playerIsAllowedToBeGrabbed = true;
		MakePlayerInvincibleForSeconds(2f, doSpriteFlashing: true);
		coreUI.FadeInOutBlackScreen(FadeIntoBlack: false, 0.5f);
		objRE.color = colorOriginal;
		base.transform.position = respawnPoint;
		crtPitfallRespawn = null;
	}

	private IEnumerator waitForDelayOnControlsAfterPitfall(float wait)
	{
		yield return new WaitForSeconds(wait);
		GC.isPlayerInteractingLocked = false;
		crtPitRespawnControlDelay = null;
	}

	private void PlayerHasTouchedEventTriggerArea(EventTriggerLogic refEventScript)
	{
		if (!refEventScript)
		{
			Debug.Log("PLAYER object could not get a reference to the EventTriggerLogic script from this event area!");
			lastTouchedEventTrigger = null;
		}
		else if (!GC.isPlayerFainted)
		{
			if (!refEventScript.needToPressInteract && refEventScript.thisEvent != EventTriggerLogic.EventList.Checkpoint)
			{
				GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Null, "PLAYER.PlayerHasTouchedEventTriggerArea()", cleanAllEffects: true);
			}
			if (refEventScript.thisEvent == EventTriggerLogic.EventList.Checkpoint)
			{
				refEventScript.StartEventMainLogic();
			}
			else if (!refEventScript.needToPressInteract)
			{
				lastTouchedEventTrigger = null;
				SetPlayerStateDuringEvents();
				refEventScript.StartEventMainLogic();
			}
			else
			{
				lastTouchedEventTrigger = refEventScript;
			}
		}
	}

	private void InteractedWithEvent()
	{
		if (lastTouchedEventTrigger != null && lastTouchedEventTrigger.needToPressInteract)
		{
			lastTouchedEventTrigger.StartEventMainLogic();
		}
	}

	private void SetPlayerStateDuringEvents()
	{
		if (CheckRefCoreUI())
		{
			coreUI.ToggleEventFrame(setThisState: true);
			GC.MINIMAP_Toggle(toggleCurrentState: false, setThisSpecificState: false);
		}
		GC.eventIsPlaying = true;
		GC.isPlayerInteractingLocked = true;
		IsInvincibleToggleSelf(setInvnicibleState: true);
		EndSpecialSkill(forceEnd: true);
		vecPlayerMovement.x = 0f;
		vecPlayerMovement.y = 0f;
	}

	public void PlayerEventHasBeenFinished()
	{
		if (CheckRefCoreUI())
		{
			coreUI.ToggleEventFrame(setThisState: false);
		}
		GC.eventIsPlaying = false;
		GC.isPlayerInteractingLocked = false;
		IsInvincibleToggleSelf(setInvnicibleState: false);
		currentHorMoveSpeed = playerBaseHorMoveSpeed;
	}

	public void StartEventDialogue(GameCore.NPCType currentNPCType, int idThisDialogue, bool zoomDialogue)
	{
		canInteractWithNPC = true;
		GC.BeginDialogueLogic(currentNPCType, idThisDialogue, zoomDialogue);
		if (crtEventDialogueWait != null)
		{
			StopCoroutine(crtEventDialogueWait);
		}
		crtEventDialogueWait = StartCoroutine(waitForEventDialogue());
	}

	public void EndEventDialogue()
	{
		canInteractWithNPC = false;
		GC.eventPlayerFinishedDialogue = true;
	}

	private void InterruptDialogue(bool hasExitedColliderWithNPC)
	{
		if (hasExitedColliderWithNPC)
		{
			canInteractWithNPC = false;
			objTouchedNPC = null;
		}
		if (GC.isDisplayingDialogue)
		{
			coreUI.FinishDialogue();
		}
	}

	private void PlayerHasInteractedWithNPC()
	{
		GameObject gameObject = objTouchedNPC;
		NpcLogic npcLogic = null;
		if (gameObject != null)
		{
			npcLogic = gameObject.GetComponent<NpcLogic>();
			GC.lastNPCTypeInteracted = npcLogic.currentNPCType;
			switch (npcLogic.currentNPCType)
			{
			case GameCore.NPCType.OBJ_Portal:
			{
				PortalLogic component = npcLogic.specialObject.GetComponent<PortalLogic>();
				if (GC.playerCanUseTeleport && !GC.isSceneTransitioning && canUseTeleport)
				{
					isBlockMoveInput = true;
					isBlockActionInput = true;
					playerCurrentXMove = 0f;
					playerCurrentYMove = 0f;
					PortalAnimAndSoundEnteringVFX(component.playMayaAnimEntering, component.transform.position);
					Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerEnemies"), ignore: true);
				}
				component.DoPlayerPortal();
				break;
			}
			case GameCore.NPCType.OBJ_PortalSameLevel:
				if (canUseTeleport)
				{
					canUseTeleport = false;
					TeleportSameLevelPortal(npcLogic.specialObject.GetComponent<PortalSameLevel>());
					if (crtDelayTeleportUse != null)
					{
						StopCoroutine(crtDelayTeleportUse);
					}
					crtDelayTeleportUse = StartCoroutine(waitDelayTeleportUsage(0.6f));
					MakePlayerInvincibleForSeconds(1f, doSpriteFlashing: false);
				}
				break;
			case GameCore.NPCType.NeneShop:
			case GameCore.NPCType.FarinaSupport:
				GC.ToggleSpecialMenuNPC(npcLogic.currentNPCType, state: true);
				GC.BeginDialogueLogic(npcLogic.currentNPCType, npcLogic.idSpecificDialogue, npcLogic.isZoomInOnDialogue);
				break;
			case GameCore.NPCType.GalleryBook:
				GC.CallGalleryViewMenuFromPillar();
				break;
			default:
				GC.BeginDialogueLogic(npcLogic.currentNPCType, npcLogic.idSpecificDialogue, npcLogic.isZoomInOnDialogue);
				break;
			case GameCore.NPCType.OBJ_Tutorial:
				break;
			}
		}
		else
		{
			Debug.Log("Error getting NPC_SCRIPT reference from touched object! Halting logic...");
		}
	}

	public void TeleportSameLevelPortal(PortalSameLevel tmpSameLevel)
	{
		PortalAnimAndSoundEnteringVFX(tmpSameLevel.playMayaAnimEntering, tmpSameLevel.transform.position);
		isBlockMoveInput = true;
		isBlockActionInput = true;
		playerCurrentXMove = 0f;
		playerCurrentYMove = 0f;
		tmpSameLevel.TeleportPlayerToTargetPosition();
	}

	public void TeleportSameLevelGallery(PortalSameLevel refComponent)
	{
		isLoadDelay = true;
		if (crtWaitForLoadDelay != null)
		{
			StopCoroutine(crtWaitForLoadDelay);
		}
		crtWaitForLoadDelay = StartCoroutine(waitForCrouchDelayAfterLoad(2f));
		canUseTeleport = false;
		TeleportSameLevelPortal(refComponent);
		if (crtDelayTeleportUse != null)
		{
			StopCoroutine(crtDelayTeleportUse);
		}
		crtDelayTeleportUse = StartCoroutine(waitDelayTeleportUsage(1f));
	}

	private void PortalAnimAndSoundEnteringVFX(bool playAnim, Vector2 enterPos)
	{
		if (playAnim)
		{
			enterPos.y = base.transform.position.y;
			base.transform.position = enterPos;
			objAN.Play("Player-EnterDoor");
		}
	}

	private IEnumerator waitForEventDialogue()
	{
		yield return new WaitUntil(() => !GC.isDisplayingDialogue);
		EndEventDialogue();
	}

	public void ForceMovePlayerLeftRight(int direction, float duration)
	{
		if (crtForceMovePlayer != null)
		{
			StopCoroutine(crtForceMovePlayer);
		}
		crtForceMovePlayer = StartCoroutine(waitForceMovePlayer(direction, duration));
	}

	public void ForceMovePlayerToTargetPoint(string targetName, Vector2 targetPoint, float moveSpeed, float runTimer, bool checkForStuck)
	{
		int num = 0;
		strCurrentEventWayPointTarget = targetName;
		num = ((targetPoint.x >= base.transform.position.x) ? 1 : (-1));
		currentHorMoveSpeed = moveSpeed;
		if (crtForceMovePlayer != null)
		{
			StopCoroutine(crtForceMovePlayer);
		}
		crtForceMovePlayer = StartCoroutine(waitForceMovePlayer(num, runTimer));
		if (checkForStuck)
		{
			if (crtSeeStuck != null)
			{
				StopCoroutine(crtSeeStuck);
			}
			crtSeeStuck = StartCoroutine(waitToSeeStuck(0.2f));
		}
	}

	private void PlayerHasReachedTargetPoint()
	{
		FinalizeForceMove();
		strCurrentEventWayPointTarget = "";
		GC.eventPlayerReachedPoint = true;
	}

	private void FinalizeForceMove()
	{
		currentHorMoveSpeed = playerBaseHorMoveSpeed;
		forceMoveRight = false;
		forceMoveLeft = false;
		seeIfStuck = false;
		if (GC.isPlayerFainted)
		{
			EndFaintedEroWalk();
		}
		if (crtForceMovePlayer != null)
		{
			StopCoroutine(crtForceMovePlayer);
		}
		if (crtSeeStuck != null)
		{
			StopCoroutine(crtSeeStuck);
		}
		crtForceMovePlayer = null;
		crtSeeStuck = null;
	}

	private IEnumerator waitForceMovePlayer(int dir, float waittime)
	{
		switch (dir)
		{
		case 1:
			forceMoveRight = true;
			forceMoveLeft = false;
			break;
		case -1:
			forceMoveLeft = true;
			forceMoveRight = false;
			break;
		}
		yield return new WaitForSeconds(waittime);
		FinalizeForceMove();
	}

	private IEnumerator waitToSeeStuck(float wait)
	{
		yield return new WaitForSeconds(wait);
		seeIfStuck = true;
	}

	private void ChangeTheActiveCollider(PlayerColliderType collType)
	{
		switch (collType)
		{
		case PlayerColliderType.Box2D_Normal:
			if (!objBox2D.enabled)
			{
				AlterColliderState(box: true, poly: false, caps: false);
			}
			if (objBox2D.size != vecOriginalColliderSize && !GC.isPlayerFainted)
			{
				objBox2D.size = vecOriginalColliderSize;
				objBox2D.offset = vecOriginalColliderOffset;
				AlterPositionOfTheCeilingChecks(collType);
			}
			break;
		case PlayerColliderType.Box2D_Small:
			if (!objBox2D.enabled)
			{
				AlterColliderState(box: true, poly: false, caps: false);
			}
			if (objBox2D.size != vecCrouchColliderSize)
			{
				objBox2D.size = vecCrouchColliderSize;
				objBox2D.offset = vecCrouchColliderOffset;
				if (isDoingBasicAttackAnimation && canPlayerFlipNextAttack)
				{
					EndBasicAttackGeneric();
				}
				AlterPositionOfTheCeilingChecks(collType);
			}
			break;
		case PlayerColliderType.Box2D_Ladder:
			if (!objBox2D.enabled)
			{
				AlterColliderState(box: true, poly: false, caps: false);
			}
			if (objBox2D.size != vecLadderCollSize)
			{
				objBox2D.size = vecLadderCollSize;
				objBox2D.offset = vecLadderCollOffset;
				AlterPositionOfTheCeilingChecks(collType);
			}
			break;
		case PlayerColliderType.Box2D_Fainted:
			if (!objBox2D.enabled)
			{
				AlterColliderState(box: true, poly: false, caps: false);
			}
			if (objBox2D.size != vecFaintedColliderSize)
			{
				objBox2D.size = vecFaintedColliderSize;
				objBox2D.offset = vecFaintedColliderOffset;
			}
			break;
		case PlayerColliderType.Polygon2D_Normal:
			if (!objPolygon2d.enabled)
			{
				AlterColliderState(box: false, poly: true, caps: false);
				objBox2D.size = vecOriginalColliderSize;
				objBox2D.offset = vecOriginalColliderOffset;
				AlterPositionOfTheCeilingChecks(collType);
			}
			break;
		case PlayerColliderType.Capsule2D_Slide:
			if (!objCapsule2D.enabled)
			{
				AlterColliderState(box: false, poly: false, caps: true);
				AlterPositionOfTheCeilingChecks(collType);
			}
			break;
		}
	}

	private void AlterColliderState(bool box, bool poly, bool caps)
	{
		objBox2D.enabled = box;
		objPolygon2d.enabled = poly;
		objCapsule2D.enabled = caps;
	}

	private void AlterPositionOfTheCeilingChecks(PlayerColliderType colliderType)
	{
		if (colliderType != PlayerColliderType.Box2D_Small && colliderType != PlayerColliderType.Capsule2D_Slide)
		{
			Vector2 vector = CeilingCheck1.transform.localPosition;
			vector.y = origCeilingCheck_Ypos;
			CeilingCheck1.transform.localPosition = vector;
			vector = CeilingCheck2.transform.localPosition;
			vector.y = origCeilingCheck_Ypos;
			CeilingCheck2.transform.localPosition = vector;
			vector = CeilingCheck3.transform.localPosition;
			vector.y = origCeilingCheck_Ypos;
			CeilingCheck3.transform.localPosition = vector;
		}
		else
		{
			Vector2 vector = CeilingCheck1.transform.localPosition;
			vector.y = origCeilingCheck_Ypos - 0.25f;
			CeilingCheck1.transform.localPosition = vector;
			vector = CeilingCheck2.transform.localPosition;
			vector.y = origCeilingCheck_Ypos - 0.25f;
			CeilingCheck2.transform.localPosition = vector;
			vector = CeilingCheck3.transform.localPosition;
			vector.y = origCeilingCheck_Ypos - 0.25f;
			CeilingCheck3.transform.localPosition = vector;
		}
	}

	private void ColliderToggleIgnoreBetweenPlayerEnemy(Collider2D otherColl, bool isIgnore)
	{
		Physics2D.IgnoreCollision(otherColl, objBox2D, isIgnore);
		Physics2D.IgnoreCollision(otherColl, objPolygon2d, isIgnore);
		Physics2D.IgnoreCollision(otherColl, objCapsule2D, isIgnore);
	}

	private IEnumerator waitForColliderIgnore(float wait, Collider2D otherColl)
	{
		yield return new WaitForSeconds(wait);
		ColliderToggleIgnoreBetweenPlayerEnemy(otherColl, isIgnore: false);
		crtIgnoreColl = null;
	}

	public void PlaySimpleLocalPlayerSounds(string strAudio)
	{
		switch (strAudio)
		{
		case "stepGround":
			if (currentSkillState != playerSkillState.SlidingTackle)
			{
				GC.PlaySFX(GameCore.SFXBasic1.JumpEndRNG, 0, checkForDuplicity: true, Vector2.zero, 0.8f, 1.2f);
			}
			break;
		case "crystal":
			GC.PlaySFX(GameCore.SFXBasic1.CrystalDisengaged, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "weird1":
			GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "odd1":
			GC.PlaySFX(GameCore.SFXBasic2.Odd1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "lick1":
			GC.PlaySFXEro(GameCore.EroSound.Lick1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "lick2":
			GC.PlaySFXEro(GameCore.EroSound.Lick2, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "slap1":
			GC.PlaySFXEro(GameCore.EroSound.Slap1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "boing1":
			GC.PlaySFX(GameCore.SFXBasic2.Boing1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "boing2":
			GC.PlaySFX(GameCore.SFXBasic2.Boing2, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "boing2alt":
			GC.PlaySFX(GameCore.SFXBasic2.Boing2, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "boing2low":
			GC.PlaySFX(GameCore.SFXBasic2.Boing2, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 0.9f);
			break;
		case "boing3low":
			GC.PlaySFX(GameCore.SFXBasic2.Boing3, 0, checkForDuplicity: false, Vector2.zero, 0.8f, 0.9f);
			break;
		case "fast1":
			GC.PlaySFX(GameCore.SFXBasic1.Fast1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case "wet-rng":
			GC.PlaySFXEro(GameCore.EroSound.WetRandom, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
			break;
		case "rng-GaspGentle":
		case "attack16":
		case "rng-MoanLight":
		case "rng-MoanMedium1":
		case "rng-GaspGentle2":
		case "rng-MiscOpen2":
		case "rng-MoanHeavy":
		case "climax8":
			GC.CallReferenceMayaVoiceGC(strAudio, "ControllerPlayer:PlaySimpleLocalPlayerSounds from ANIM");
			break;
		}
	}

	private void PlayLocalMayaVoicesLogic(LocalMayaVoiceType voiceType)
	{
		bool flag = false;
		float wait = 0f;
		string text = "";
		switch (voiceType)
		{
		case LocalMayaVoiceType.atkSimple:
			text = ((GC.playerCurrentDressState != 2) ? "rng-AttackSimple" : "rng-AttackSimpleNaked");
			break;
		case LocalMayaVoiceType.atkHeavy:
			text = ((GC.playerCurrentDressState != 2) ? "rng-AttackHeavy" : "rng-AttackHeavyNaked");
			break;
		case LocalMayaVoiceType.hurtSoft:
			text = "rng-HurtPain1";
			if (currentDressState == GameCore.PlayerDressState.None)
			{
				flag = true;
				wait = 0.05f;
			}
			break;
		case LocalMayaVoiceType.hurtHard:
			text = "rng-HurtPain3";
			if (currentDressState == GameCore.PlayerDressState.None)
			{
				flag = true;
				wait = 0.05f;
			}
			break;
		case LocalMayaVoiceType.hurtScream:
			text = "rng-Scream";
			break;
		case LocalMayaVoiceType.eroReleased:
			text = "rng-EroReleased";
			break;
		case LocalMayaVoiceType.skillSlashCast:
			text = "rng-AttackHeavy";
			break;
		}
		if (flag)
		{
			if (crtSoundSpecial != null)
			{
				StopCoroutine(crtSoundSpecial);
			}
			crtSoundSpecial = StartCoroutine(waitDelayPlayLocalSounds(wait, text));
		}
		else
		{
			GC.CallReferenceMayaVoiceGC(text, "Controller_Player.PlayLocalMayaVoicesLogic()");
		}
	}

	private IEnumerator waitDelayPlayLocalSounds(float wait, string tagToPlay)
	{
		yield return new WaitForSeconds(wait);
		if ((tagToPlay == "rng-HurtPain1" || tagToPlay == "rng-HurtPain3") && !GC.playerIsBeingGrabed)
		{
			GC.CallReferenceMayaVoiceGC(tagToPlay, "Controller_Player.waitDelayPlayLocalSounds()");
		}
		crtSoundSpecial = null;
	}

	private void startCameraOffsetFromLever()
	{
		if (!GC || GC.cameraOffsetMovingForLever)
		{
			return;
		}
		Vector2 vector = currentLeverRef.arrayOfMechanisms[0].transform.position;
		Vector2 cameraOffset = new Vector2(base.transform.position.x - vector.x, base.transform.position.y - vector.y);
		if ((base.transform.position.x < vector.x && cameraOffset.x < 0f) || (base.transform.position.x > vector.x && cameraOffset.x > 0f))
		{
			cameraOffset.x *= -1f;
		}
		if ((base.transform.position.y < vector.y && cameraOffset.y < 0f) || (base.transform.position.y > vector.y && cameraOffset.y > 0f))
		{
			cameraOffset.y *= -1f;
		}
		if (!(Mathf.Abs(cameraOffset.x) <= 9f) || !(Mathf.Abs(cameraOffset.y) <= 6f))
		{
			GC.OverrideCameraMove(cameraOffset);
			GC.cameraOffsetMovingForLever = true;
			if (crtResetCameraLeverOffset != null)
			{
				StopCoroutine(crtResetCameraLeverOffset);
			}
			crtResetCameraLeverOffset = StartCoroutine(waitResetCameraAfterLeverOffset(3f));
		}
	}

	private void checkEndCameraOffsetFromLever()
	{
		if ((bool)GC && GC.cameraOffsetMovingForLever)
		{
			GC.cameraOffsetMovingForLever = false;
		}
	}

	private IEnumerator waitResetCameraAfterLeverOffset(float wait)
	{
		yield return new WaitForSeconds(wait);
		checkEndCameraOffsetFromLever();
		crtResetCameraLeverOffset = null;
	}

	private bool movHack_Right()
	{
		if (GC != null && GC.movHackRight)
		{
			return true;
		}
		return false;
	}

	private bool movHack_Left()
	{
		if (GC != null && GC.movHackLeft)
		{
			return true;
		}
		return false;
	}

	private void JumpAssistHack()
	{
		currentPlayerState = playerState.Jumping;
		STATE_MACHINE_ANIMATION();
		objAN.Play("Player-JumpingStart");
		isJumpAssit = false;
	}

	private void DoDelayedJump(float delay)
	{
		if (crtDelayJump != null)
		{
			StopCoroutine(crtDelayJump);
		}
		crtDelayJump = StartCoroutine(DelayedJump(delay));
	}

	private void InvokedHack_AvoidStuckOnGroundAndGates()
	{
		if (!GC.isPlayerFainted && !GC.playerIsBeingGrabed && !isHurt && currentSkillState == playerSkillState.Null)
		{
			Vector2 start = new Vector2(StartPointCeilingCheck.position.x - 0.15f, StartPointCeilingCheck.position.y - 0.15f);
			Vector2 end = new Vector2(StartPointCeilingCheck.position.x + 0.15f, StartPointCeilingCheck.position.y - 0.15f);
			Vector2 start2 = new Vector2(StartPointCeilingCheck.position.x - 0.15f, StartPointCeilingCheck.position.y - 0.75f);
			Vector2 end2 = new Vector2(StartPointCeilingCheck.position.x + 0.15f, StartPointCeilingCheck.position.y - 0.75f);
			Vector2 start3 = new Vector2(StartPointGroundCheck.position.x - 0.15f, StartPointGroundCheck.position.y + 0.15f);
			Vector2 end3 = new Vector2(StartPointGroundCheck.position.x + 0.15f, StartPointGroundCheck.position.y + 0.15f);
			Vector2 start4 = new Vector2(StartPointGroundCheck.position.x - 0.15f, StartPointGroundCheck.position.y + 0.25f);
			Vector2 end4 = new Vector2(StartPointGroundCheck.position.x + 0.15f, StartPointGroundCheck.position.y + 0.25f);
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			bool flag6 = false;
			bool flag7 = false;
			bool flag8 = false;
			if (!objAN.GetBool("isCrouching"))
			{
				flag = Physics2D.Linecast(start, end, 1 << LayerMask.NameToLayer("LayerPlatforms"));
				flag5 = Physics2D.Linecast(start, end, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
				flag2 = Physics2D.Linecast(start2, end2, 1 << LayerMask.NameToLayer("LayerPlatforms"));
				flag6 = Physics2D.Linecast(start2, end2, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
			}
			else
			{
				flag3 = Physics2D.Linecast(start3, end3, 1 << LayerMask.NameToLayer("LayerPlatforms"));
				flag7 = Physics2D.Linecast(start3, end3, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
				flag4 = Physics2D.Linecast(start4, end4, 1 << LayerMask.NameToLayer("LayerPlatforms"));
				flag8 = Physics2D.Linecast(start4, end4, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
			}
			if (flag | flag2 | flag3 | flag4 | flag5 | flag6 | flag7 | flag8)
			{
				HACK_UNSTUCK_MOVE();
			}
			else
			{
				lastUnstuckPosition = base.transform.position;
			}
		}
	}

	private void HACK_UNSTUCK_MOVE()
	{
		bool flag = false;
		bool flag2 = false;
		_ = Vector2.zero;
		Vector2 zero = Vector2.zero;
		int[] array = GC.MATH_RNG_TYPE_ARRAY(hackUnstuck, hackUnstuck.Length - 1);
		for (int i = 0; i < hackUnstuck.Length - 1; i++)
		{
			Transform tryPOS = hackUnstuck[array[i]].tryPOS;
			Vector2 start = new Vector2(tryPOS.position.x - 0.15f, tryPOS.position.y);
			zero = new Vector2(tryPOS.position.x + 0.15f, tryPOS.position.y);
			flag = Physics2D.Linecast(start, zero, 1 << LayerMask.NameToLayer("LayerPlatforms"));
			flag2 = Physics2D.Linecast(start, zero, 1 << LayerMask.NameToLayer("LayerPlatMoving"));
			if (!flag && !flag2)
			{
				base.transform.position = new Vector2(base.transform.position.x + tryPOS.localPosition.x / 2f, base.transform.position.y + tryPOS.localPosition.y / 2f);
				return;
			}
		}
		if (lastUnstuckPosition != Vector2.zero)
		{
			base.transform.position = lastUnstuckPosition;
		}
	}

	private IEnumerator DelayedJump(float time)
	{
		yield return new WaitForSeconds(time);
		playerCurrentYMove = playerMaxJumpForce;
		crtDelayJump = null;
	}

	private IEnumerator waitForJumpAssist(float time)
	{
		isJumpAssit = true;
		yield return new WaitForSeconds(time);
		isJumpAssit = false;
		crtJumpAssist = null;
	}

	private IEnumerator waitToDislodgeStuckSliding(float time)
	{
		float stuckDirection = base.transform.localScale.x;
		yield return new WaitForSeconds(time);
		if ((currentSkillState == playerSkillState.SlidingTackle || currentSkillState == playerSkillState.DrillDash) && Mathf.RoundToInt(objRB.velocity.x) == 0 && stuckDirection == base.transform.localScale.x && CheckCollisionCeiling())
		{
			Vector2 targetPoint = HACK_ReturnSlidingTackleDirection(invertDirection: true);
			ForceMovePlayerToTargetPoint("", targetPoint, _hackySlidingSpeed, 10f, checkForStuck: true);
		}
		crtDislodgeSlide = null;
	}

	private IEnumerator waitJumpPressed(float wait)
	{
		wasJumpButton = true;
		yield return new WaitForSeconds(wait);
		crtJumpPressed = null;
	}

	private void HACK_MimicChestCheck()
	{
		if (objLastEnemyCollided != null && objLastEnemyCollided.activeInHierarchy)
		{
			EnemyGeneralLogic component = objLastEnemyCollided.GetComponent<EnemyGeneralLogic>();
			if (component != null && component.currentEnemyType == GameCore.EnemyType.WolfGirlMimic && component.GetMiscGameObject(0).activeInHierarchy)
			{
				ApplyDamageToPlayer(GC.playerMaxHealth, Vector3.zero, afterEroEscaped: false, bypassDefense: true);
				component.CallEroLogic(base.gameObject);
			}
		}
	}

	private Vector2 HACK_ReturnSlidingTackleDirection(bool invertDirection)
	{
		Vector2 result = base.transform.position;
		float num = 0f;
		num = base.transform.localScale.x;
		if (invertDirection)
		{
			num *= -1f;
		}
		result.x += num;
		return result;
	}

	public void ApplyInitialEffects(GameCore.TempPlayerEffect thisEffect)
	{
		switch (thisEffect)
		{
		case GameCore.TempPlayerEffect.PullGrab:
			playerCurrentXMove = 0f;
			playerCurrentYMove = 0f;
			PlayHurtAnimationAndDressBreakLogic();
			if (currentSkillState == playerSkillState.Dash)
			{
				EndSpecialSkill(forceEnd: true);
			}
			break;
		case GameCore.TempPlayerEffect.Petrifying:
			PTR_StartPetrifyLogic();
			break;
		case GameCore.TempPlayerEffect.Petrified:
			PTR_StartPetrifiedLogic();
			break;
		case GameCore.TempPlayerEffect.Stuned:
			objAN.Play("Maya-Stun");
			break;
		}
	}

	private bool MainRecurringEffect(GameCore.TempPlayerEffect thisEffect)
	{
		switch (thisEffect)
		{
		case GameCore.TempPlayerEffect.PullGrab:
			if (!GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.PullGrab))
			{
				return false;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, vecGeneric, 11f * Time.deltaTime);
			return true;
		case GameCore.TempPlayerEffect.Charmed:
			if (!GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Charmed))
			{
				if (objAN.GetBool("isEroWalk"))
				{
					objAN.SetBool("isEroWalk", value: false);
					objAN.Play("Player-Idle");
					PlayerHasReachedTargetPoint();
				}
				return false;
			}
			if (objAN.GetBool("isEroWalk"))
			{
				break;
			}
			if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Charmed))
			{
				charmedDuration = GC.playerEffectList.Find((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.Charmed)?.maxTimer ?? 5f;
			}
			else
			{
				charmedDuration = 5f;
			}
			EndSpecialSkill(forceEnd: true);
			objAN.SetBool("isEroWalk", value: true);
			objAN.Play("Naked-WalkTowardsEnemy");
			ForceMovePlayerToTargetPoint("", vecGeneric, 2f, charmedDuration, checkForStuck: true);
			return true;
		case GameCore.TempPlayerEffect.Petrified:
			if (!GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified))
			{
				return false;
			}
			PTR_PetrifiedRecurring();
			break;
		}
		return false;
	}

	public void ApplyFinishedEffects(GameCore.TempPlayerEffect thisEffect)
	{
		switch (thisEffect)
		{
		case GameCore.TempPlayerEffect.Petrified:
			PTR_ConcludePetrified();
			break;
		case GameCore.TempPlayerEffect.Stuned:
			objAN.Play("Player-Idle");
			break;
		}
	}

	private bool EffectsThatBlocksInput()
	{
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.PullGrab) || GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Charmed) || GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified) || GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Stuned))
		{
			return true;
		}
		return false;
	}

	private void CleanCertainEffectsOnEroGrab(string logInfo = "PLAYER.CleanCertainEffectsOnEroGrab()")
	{
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Charmed, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Petrifying, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Petrified, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Stuned, logInfo);
		if (GC.playerSpecialAnimCount == 5 && GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.WerewolfBossCurse))
		{
			GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.WerewolfBossCurse, "PLAYER.PLAYER.CleanCertainEffectsOnEroGrab().WerewolfCurse");
		}
	}

	private void CleanCertainEffectsOnFainted(string logInfo = "PLAYER.CleanCertainEffectsOnFainted()")
	{
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Charmed, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Petrifying, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Petrified, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Stuned, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.DrinkATK, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.DrinkDEF, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.DrinkSpeed, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.DrinkLifeRegen, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.DrinkManaRegen, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.FruitSlice, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.FruitForbidden, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.FruitDistract, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.FruitWarrior, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.MealHero, logInfo);
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Exhibitionist, logInfo);
	}

	private void ForbiddenFruitFindNearestEnemy()
	{
		Vector2 vector = new Vector2(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition);
		float num = 0f;
		int num2 = 1;
		do
		{
			num += 50f;
			num2--;
			RaycastHit2D[] arrayHits = Physics2D.BoxCastAll(base.transform.position, new Vector2(num, num), 0f, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("LayerEnemies"));
			vecGeneric = GC.MATH_GetNearestPosition(vector, arrayHits, 0, 10, 0, getEnemyCenterPos: true);
		}
		while (num2 > 0 && vecGeneric == Vector2.zero);
	}

	private void PTR_ResetPetrifyBaseValues()
	{
		_ptrFrequencyInvoke = 0.2f;
		_ptrTimer = 6f;
		_ptrGoal = 3f;
		_ptrMoveAffectMultiplier = 2f;
		_ptrCounter = 0f;
		_ptrStringinvoked = "PTR_Invoked_PetrifyingEffect";
		_ptrStringinvoked2 = "PTR_InvokedPetrifiedDMG";
		_ptrStringStiffAnim = "Debuff-PetrifiedStiff";
	}

	private void PTR_StartPetrifyLogic()
	{
		PTR_ResetPetrifyBaseValues();
		if (!IsInvoking(_ptrStringinvoked))
		{
			InvokeRepeating(_ptrStringinvoked, 0f, _ptrFrequencyInvoke);
		}
		if (crtPTRPetrifying != null)
		{
			StopCoroutine(crtPTRPetrifying);
		}
		crtPTRPetrifying = StartCoroutine(waitPTR_Petrifying(_ptrTimer));
	}

	private void PTR_Invoked_PetrifyingEffect()
	{
		if (PTR_CheckStopCancelPetrifying())
		{
			PTR_ConcludeCleanPetrifying();
			CancelInvoke(_ptrStringinvoked);
		}
		else if ((objPetrifierEnemy != null && objPetrifierEnemy.transform.localScale.x == 1f && base.transform.localScale.x == -1f) || (objPetrifierEnemy.transform.localScale.x == -1f && base.transform.localScale.x == 1f))
		{
			_ptrCounter += _ptrFrequencyInvoke;
			if (!miscOBJ.vfxPetrifying.activeInHierarchy)
			{
				miscOBJ.vfxPetrifying.SetActive(value: true);
				miscOBJ.vfxPetrifying.GetComponent<Animator>().Play("Medusa-Petrify1");
				GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MDS_Petr, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			if (_ptrCounter >= _ptrGoal)
			{
				PTR_ConcludeCleanPetrifying();
				GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.Petrified, 99f, "Invoked_PetrifyingEffect");
			}
		}
	}

	private bool PTR_CheckStopCancelPetrifying()
	{
		if (!GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrifying) || objPetrifierEnemy == null || !objPetrifierEnemy.GetComponent<Animator>().GetBool("isSkill"))
		{
			return true;
		}
		return false;
	}

	private void PTR_ConcludeCleanPetrifying()
	{
		if (crtPTRPetrifying != null)
		{
			StopCoroutine(crtPTRPetrifying);
			crtPTRPetrifying = null;
		}
		if (miscOBJ.vfxPetrifying.activeInHierarchy)
		{
			miscOBJ.vfxPetrifying.SetActive(value: false);
		}
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Petrifying, "PLAYER.PTR_ConcludeCleanPetrifying()");
		PTR_ResetPetrifyBaseValues();
	}

	private IEnumerator waitPTR_Petrifying(float wait)
	{
		yield return new WaitForSeconds(wait);
		PTR_ConcludeCleanPetrifying();
	}

	private void PTR_StartPetrifiedLogic()
	{
		string senderScriptMethod = "Controller_Player:StartPetrifiedLogic()";
		GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, centerOfMass.position, senderScriptMethod);
		if (currentDressState == GameCore.PlayerDressState.Full)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.ArmorBreakC, centerOfMass.position, senderScriptMethod);
			GC.PlaySFX(GameCore.SFXBasic1.ClothRip1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
		else if (currentDressState == GameCore.PlayerDressState.Half)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.ArmorBreakB, centerOfMass.position, senderScriptMethod);
			GC.PlaySFX(GameCore.SFXBasic1.ClothRip1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
		ForceOverrideDressState(2);
		GC.DisplaySystemMessageUI(0, 113, 5f);
		objAN.Play(_ptrStringStiffAnim);
		isPlayerAllowedToStruggle = true;
		ShowStruggleUI();
		InvokeRepeating("DoStruggleConstantDrain", 0f, freqDoSpiritRegen);
		if (!IsInvoking(_ptrStringinvoked2))
		{
			InvokeRepeating(_ptrStringinvoked2, 0f, 1f);
		}
	}

	private void PTR_PetrifiedRecurring()
	{
		if (!isHurt && !GC.isPlayerFainted && !GC.playerIsBeingGrabed && !IsMayaAnimationState(_ptrStringStiffAnim))
		{
			objAN.Play(_ptrStringStiffAnim);
		}
	}

	private void PTR_StruggledFreeFromPetrified()
	{
		GC.RemoveEffectFromPlayer(GameCore.TempPlayerEffect.Petrified, "PLAYER.PTR_StruggledFreeFromPetrified()");
	}

	private void PTR_ConcludePetrified()
	{
		PTR_ResetPetrifyBaseValues();
		if (!GC.isPlayerFainted)
		{
			objAN.Play("Player-Idle");
			if (!GC.playerIsBeingGrabed)
			{
				PlayerForbidStruggle();
			}
		}
		if (IsInvoking(_ptrStringinvoked2))
		{
			CancelInvoke(_ptrStringinvoked2);
		}
	}

	private void PTR_InvokedPetrifiedDMG()
	{
		ApplyDamageToPlayer(PTR_PetrifiedDMG(), new Vector3(0f, 1f, 0f));
		GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MDS_Stiff, 0, checkForDuplicity: true, base.transform.position, 0.95f, 1.05f);
	}

	private int PTR_PetrifiedDMG()
	{
		if (GC.playerCurrentSpirit > 0f)
		{
			return Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(2f, GC.playerMaxHealth));
		}
		return Mathf.RoundToInt(GC.MATH_ResultValueOfTotalPercentile(10f, GC.playerMaxHealth));
	}

	private void StartSpecialMayaAnimationControl(string animToPlay, int arrayConfigType)
	{
		arraySpecialAnimControl = null;
		objAN.SetInteger("countSpecial", 0);
		intAnimLoopCounter = 0;
		if (arrayConfigType == 0 || arrayConfigType != 1)
		{
			arraySpecialAnimControl = GC.defaultEroPhases;
		}
		else
		{
			arraySpecialAnimControl = new ENM_EroPhaseControl[5];
			arraySpecialAnimControl[0] = new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.6f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			arraySpecialAnimControl[1] = new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.8f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			arraySpecialAnimControl[2] = new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 1f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			arraySpecialAnimControl[3] = new ENM_EroPhaseControl
			{
				runLoops = 6,
				speedAnimation = 0.75f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
			arraySpecialAnimControl[4] = new ENM_EroPhaseControl
			{
				runLoops = 1,
				speedAnimation = 0.75f,
				frequencyOfDmg = 0,
				divisionOfDmg = 0
			};
		}
		objAN.Play(animToPlay);
	}

	public void LoopMayaAnimationCounter()
	{
		if (arraySpecialAnimControl == null)
		{
			Debug.Log("Tried to play a special animation on Maya, but the array which controls the animation phases is NULL!");
			return;
		}
		if (objAN.GetInteger("countSpecial") >= arraySpecialAnimControl.Length)
		{
			EndSpecialMayaAnimation();
			return;
		}
		if (intAnimLoopCounter >= arraySpecialAnimControl[objAN.GetInteger("countSpecial")].runLoops)
		{
			objAN.SetInteger("countSpecial", objAN.GetInteger("countSpecial") + 1);
			intAnimLoopCounter = 0;
			if (objAN.GetInteger("countSpecial") >= arraySpecialAnimControl.Length)
			{
				EndSpecialMayaAnimation();
				return;
			}
		}
		intAnimLoopCounter++;
	}

	private void EndSpecialMayaAnimation()
	{
		if (GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.SlimeMelt))
		{
			SpecialPlayerGone();
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, base.transform.position, "ControllerPlayer");
			Object.Instantiate(miscOBJ.slimeGirlMeltTransform, base.transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
		}
	}

	public void SpecialPlayerGone()
	{
		GC.isPlayerGoneForGood = true;
		objRE.enabled = false;
		objAN.enabled = false;
		objBox2D.enabled = false;
		Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("LayerPlayer"), LayerMask.NameToLayer("LayerProjectile"), ignore: true);
	}

	private void CleanVFXunderMaya(string specificTag = "")
	{
		EffectLogic[] componentsInChildren = GetComponentsInChildren<EffectLogic>();
		if (componentsInChildren == null)
		{
			return;
		}
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (specificTag == "" || (specificTag != "" && componentsInChildren[i].tag == specificTag))
			{
				componentsInChildren[i].FinalFrameDisableEffect(1);
			}
		}
	}

	public void OnAnimationFrameMayaDo(string action)
	{
		if (!(action == "Anim-TP-Depart"))
		{
			if (action == "Anim-TP-Arrive-Final")
			{
				TeleportationVisualbehavior("Arrive-Final");
			}
		}
		else
		{
			TeleportationVisualbehavior("Depart");
		}
	}

	private bool IsMayaAnimationState(string animName, int layerIndex = 0)
	{
		if (objAN.GetCurrentAnimatorStateInfo(layerIndex).IsName(animName))
		{
			return true;
		}
		return false;
	}

	private bool IsMayaAnimationClip(string animName, int layerIndex = 0, int clipArrayIndex = 0)
	{
		if (objAN.GetCurrentAnimatorClipInfo(layerIndex)[clipArrayIndex].clip.name == animName)
		{
			return true;
		}
		return false;
	}

	public void TeleportAnimation(bool isDepart)
	{
		if (isDepart)
		{
			objAN.Play("TP-Start", -1, 0f);
			return;
		}
		objAN.Play("TP-Arrive", -1, 0f);
		GC.PlaySFX(GameCore.SFXBasic2.Boing4, 0, checkForDuplicity: true, base.transform.position, 0.6f, 0.6f);
	}

	private void TeleportationVisualbehavior(string action)
	{
		Vector2 vector = base.transform.position;
		switch (action)
		{
		case "Depart":
		{
			objRE.color = new Color(1f, 1f, 1f, 0f);
			objRB.bodyType = RigidbodyType2D.Static;
			objBox2D.enabled = false;
			ProjectileLogic component = Object.Instantiate(miscOBJ.projectileTPUP, vector, Quaternion.identity, GC.refOBJEffectsParent.transform).GetComponent<ProjectileLogic>();
			component.specialForce.y = 1f;
			GC.InstantiateVFX(GameCore.VisualEffect.playerTeleportStars, centerOfMass.position, "playerController");
			break;
		}
		case "Pre-Arrive-Setup":
		{
			objRE.color = new Color(1f, 1f, 1f, 0f);
			objRB.bodyType = RigidbodyType2D.Static;
			objBox2D.enabled = false;
			GC.playerIsInvincible = true;
			GC.isPlayerInteractingLocked = true;
			GC.isForbidAllPlayerInput = true;
			GC.playerIsAllowedToBeGrabbed = false;
			vector.y += 8f;
			GameObject obj = Object.Instantiate(miscOBJ.projectileTPUP, vector, Quaternion.identity, GC.refOBJEffectsParent.transform);
			ProjectileLogic component = obj.GetComponent<ProjectileLogic>();
			obj.GetComponent<SpriteRenderer>().flipY = true;
			component.moveSpeed += 2f;
			component.specialForce.y = -1f;
			component.timerDuration = 0.8f;
			crtTeleportArrive = StartCoroutine(waitTeleportArrive(0.5f));
			break;
		}
		case "Arrive-Final":
			GC.InstantiateVFX(GameCore.VisualEffect.playerTeleportStars, centerOfMass.position, "playerController");
			objRB.bodyType = RigidbodyType2D.Dynamic;
			objBox2D.enabled = true;
			GC.playerIsInvincible = false;
			GC.isPlayerInteractingLocked = false;
			GC.isForbidAllPlayerInput = false;
			GC.playerIsAllowedToBeGrabbed = true;
			ChangeTheActiveCollider(PlayerColliderType.Capsule2D_Slide);
			ChangeTheActiveCollider(PlayerColliderType.Polygon2D_Normal);
			if (GC.playerIsBeingGrabed)
			{
				PlayerWasEroReleased(isMayaGoneForever: false);
				GC.playerIsBeingGrabed = false;
			}
			break;
		}
	}

	private IEnumerator waitTeleportArrive(float wait)
	{
		yield return new WaitForSeconds(wait);
		objRE.color = colorOriginal;
		TeleportAnimation(isDepart: false);
		crtTeleportArrive = null;
	}

	private bool AreThereSpecialAnimationsOnDeath()
	{
		if (SPECIAL_DEATH_ANIM_WW_CURSE())
		{
			return true;
		}
		if (SPECIAL_LATEX_SUIT() || SPECIAL_SLIMEGIRL_MELT())
		{
			return true;
		}
		return false;
	}

	private bool SPECIAL_DEATH_ANIM_WW_CURSE()
	{
		if (!GC.playerIsBeingGrabed && GC.playerSpecialAnimCount == 0 && GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.WerewolfBossCurse))
		{
			objAN.Play("WWCurse_A");
			objAN.SetInteger("countSpecial", 0);
			GC.InstantiateVFX(GameCore.VisualEffect.WerewolfCurse, centerOfMass.position, "ControllerPlayer:AreThereSpacialAnimationOnDeath");
			STATE_MACHINE_FLIP_CHARACTER(forceDirection: true, SPECIAL_DEATH_ANIM_WW_CURSE_FLIP());
			StartCountSpecialAnim(6f, 5, spawnEffect: true, centerOfMass.position, GameCore.VisualEffect.WerewolfCurse);
			return true;
		}
		if (GC.playerSpecialAnimCount == 5)
		{
			objAN.Play("WWCurse_E_Idle");
			STATE_MACHINE_FLIP_CHARACTER(forceDirection: true, SPECIAL_DEATH_ANIM_WW_CURSE_FLIP());
			return true;
		}
		return false;
	}

	private float SPECIAL_DEATH_ANIM_WW_CURSE_FLIP()
	{
		float result = 1f;
		GameObject gameObject = GameObject.FindGameObjectWithTag("Enemy-Boss");
		if (gameObject != null && gameObject.transform.position.x <= base.transform.position.x)
		{
			result = -1f;
		}
		return result;
	}

	public void HACKY_FACE_FLIP_TO_WW()
	{
		STATE_MACHINE_FLIP_CHARACTER(forceDirection: true, SPECIAL_DEATH_ANIM_WW_CURSE_FLIP());
	}

	public void HACKY_INVERT_FLIP_FOR_DIALOGUE()
	{
		STATE_MACHINE_FLIP_CHARACTER(forceDirection: true, base.transform.localScale.x * -1f);
	}

	private bool SPECIAL_LATEX_SUIT()
	{
		if (!GC.playerIsBeingGrabed && GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.LatexBodySuit))
		{
			Vector2 targetPosition = base.transform.position;
			targetPosition.y -= 0.5f;
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, targetPosition, "Controller_Player");
			objAN.Play("Maya-FaintedLatex");
			return true;
		}
		return false;
	}

	private bool SPECIAL_SLIMEGIRL_MELT()
	{
		if (GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.SlimeMelt) && !IsMayaAnimationState("SlimeMelt-Loop1"))
		{
			if (GC.playerIsBeingGrabed)
			{
				GC.playerCancelStopEro = true;
			}
			Vector2 targetPosition = base.transform.position;
			targetPosition.y -= 0.5f;
			GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, targetPosition, "Controller_Player");
			StartSpecialMayaAnimationControl("SlimeMelt-Loop1", 1);
			return true;
		}
		return false;
	}

	private void PlayerDrinkPotionAnimation(GameCore.PotionType thisPotion, int idItem = 0, float drinkTime = 1f, bool applyInvincibleDuringAnim = false)
	{
		bool flag = true;
		objAN.SetBool("isPotionAnim", value: true);
		switch (thisPotion)
		{
		case GameCore.PotionType.Health:
			drinkTime = 1.5f;
			objAN.Play("Player-DrinkPotionRed");
			BeginPotionRegen(drinkTime, 100f);
			GC.HealThePlayer(5f, drinkTime);
			break;
		case GameCore.PotionType.Spirit:
			drinkTime = 1f;
			objAN.Play("Player-DrinkPotionBlue");
			GC.HealTheSpirit(50f, drinkTime);
			break;
		case GameCore.PotionType.Brew:
		case GameCore.PotionType.Eat:
			flag = OtherConsumablesBeginLogic(idItem, drinkTime);
			break;
		}
		if (flag)
		{
			if (applyInvincibleDuringAnim && idItem != 29)
			{
				MakePlayerInvincibleForSeconds(drinkTime, doSpriteFlashing: false);
			}
			GC.CallReferenceMayaVoiceGC("Swallow6", "PlayerControl");
			if (crtPotionAnim != null)
			{
				StopCoroutine(crtPotionAnim);
			}
			crtPotionAnim = StartCoroutine(waitForPotionAnimation(drinkTime));
		}
	}

	private bool OtherConsumablesBeginLogic(int itemID, float consumptionTime = 1f)
	{
		bool flag = true;
		float num = 30f;
		string text = "";
		Color colorToFlash = Color.white;
		GameCore.VisualEffect visualEffect = GameCore.VisualEffect.Null;
		GameCore.TempPlayerEffect tempPlayerEffect = GameCore.TempPlayerEffect.Null;
		switch (itemID)
		{
		case 20:
			tempPlayerEffect = GameCore.TempPlayerEffect.DrinkLifeRegen;
			visualEffect = GameCore.VisualEffect.particleDrinkREGENLIFE;
			colorToFlash = Color.green;
			text = "Player-DrinkPotionBrew";
			GC.PlaySFX(GameCore.SFXBasic2.itemDrinkPotion1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case 21:
			tempPlayerEffect = GameCore.TempPlayerEffect.DrinkManaRegen;
			visualEffect = GameCore.VisualEffect.particleDrinkREGENSP;
			colorToFlash = Color.blue;
			text = "Player-DrinkPotionBrew";
			GC.PlaySFX(GameCore.SFXBasic2.itemDrinkPotion1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case 22:
			tempPlayerEffect = GameCore.TempPlayerEffect.DrinkATK;
			visualEffect = GameCore.VisualEffect.particleDrinkATK;
			colorToFlash = Color.yellow;
			text = "Player-DrinkPotionBrew";
			GC.PlaySFX(GameCore.SFXBasic2.itemDrinkPotion1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case 23:
			tempPlayerEffect = GameCore.TempPlayerEffect.DrinkDEF;
			visualEffect = GameCore.VisualEffect.particleDrinkDEF;
			colorToFlash = new Color(0.8f, 0.6f, 0.4f, 1f);
			text = "Player-DrinkPotionBrew";
			GC.PlaySFX(GameCore.SFXBasic2.itemDrinkPotion1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case 24:
			tempPlayerEffect = GameCore.TempPlayerEffect.DrinkSpeed;
			visualEffect = GameCore.VisualEffect.particleDrinkSPEED;
			colorToFlash = Color.cyan;
			text = "Player-DrinkPotionBrew";
			GC.PlaySFX(GameCore.SFXBasic2.itemDrinkPotion1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case 25:
			tempPlayerEffect = GameCore.TempPlayerEffect.FruitSlice;
			visualEffect = GameCore.VisualEffect.particleFruitSlice;
			colorToFlash = Color.white;
			text = "Player-EatThing";
			GC.PlaySFXEro(GameCore.EroSound.SwallowRandom, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case 26:
			tempPlayerEffect = GameCore.TempPlayerEffect.FruitForbidden;
			visualEffect = GameCore.VisualEffect.particleFruitForbidden;
			colorToFlash = Color.red;
			text = "Player-EatThing";
			GC.PlaySFXEro(GameCore.EroSound.SwallowRandom, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			num = 10f;
			GC.InstantiateVFX(GameCore.VisualEffect.SmokesPink, base.transform.position, "ControlerPlayer:OtherConsumables");
			ForceOverrideDressState(2);
			GC.AddRemoveLustToPlayer(50, doVFX: true);
			ForbiddenFruitFindNearestEnemy();
			GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.Charmed, num, "ControlerPlayer:OtherConsumables");
			break;
		case 27:
			if (GC.playerIsBeingGrabed)
			{
				GC.playerCancelStopEro = true;
				break;
			}
			tempPlayerEffect = GameCore.TempPlayerEffect.FruitDistract;
			visualEffect = GameCore.VisualEffect.particleFruitDistract;
			colorToFlash = Color.gray;
			text = "Player-EatThing";
			GC.PlaySFXEro(GameCore.EroSound.SwallowRandom, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			num = 10f;
			break;
		case 28:
			tempPlayerEffect = GameCore.TempPlayerEffect.FruitWarrior;
			visualEffect = GameCore.VisualEffect.particleFruitWarrior;
			colorToFlash = Color.green;
			text = "Player-EatThing";
			GC.PlaySFXEro(GameCore.EroSound.SwallowRandom, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			break;
		case 29:
			tempPlayerEffect = GameCore.TempPlayerEffect.MealHero;
			visualEffect = GameCore.VisualEffect.particleHeroMeal;
			colorToFlash = Color.yellow;
			text = "Player-EatThing";
			GC.PlaySFXEro(GameCore.EroSound.SwallowRandom, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			num = 10f;
			MakePlayerInvincibleForSeconds(num, doSpriteFlashing: false);
			GC.InstantiateVFX(GameCore.VisualEffect.ShieldMealInvincible, base.transform.position, "self", base.transform);
			break;
		case 33:
			colorToFlash = Color.white;
			text = "Player-EatThing";
			visualEffect = GameCore.VisualEffect.GrabSmokes;
			GC.PlaySFXEro(GameCore.EroSound.SwallowRandom, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			GC.AddRemoveLustToPlayer(-10, doVFX: false);
			objAN.Play(text);
			break;
		}
		if (tempPlayerEffect != GameCore.TempPlayerEffect.Null)
		{
			if (GC.ExistsEffectOnPlayer(tempPlayerEffect))
			{
				GC.AddItemToPlayerInventory(itemID, "HackLOL", Vector2.zero);
				return false;
			}
			GC.ApplyEffectToPlayer(tempPlayerEffect, num, "Controller_Player");
			if (text != "")
			{
				objAN.Play(text);
			}
		}
		if (flag && visualEffect != GameCore.VisualEffect.Null && consumptionTime > 0f)
		{
			ToggleSpriteColorFlashing(stateON: true, colorToFlash, 0.05f, consumptionTime);
			GC.InstantiateVFX(visualEffect, base.transform.position, "CONTROLPLAYER:OtherConsumablesBeginLogic", base.transform, returnOBJref: true).GetComponent<EffectLogic>().timeDurationLoop = consumptionTime;
		}
		return true;
	}

	private void InterruptPotionDrinkAnimation()
	{
		GC.InstantiateVFX(GameCore.VisualEffect.GlassShatter, base.transform.position, "PlayerControl");
		GC.StopSFXBasic2(GameCore.SFXBasic2.itemDrinkPotion1);
		GC.PlaySFXGeneric(GameCore.SpeakerID.PrincessBoss.ToString(), 12, 0, checkForDuplicity: true, base.transform.position, 0.8f, 0.9f, "sfx", "IceBreak");
		EndPotionDrinkAnimation();
	}

	private void EndPotionDrinkAnimation()
	{
		if (crtPotionAnim != null)
		{
			StopCoroutine(crtPotionAnim);
		}
		EndPotionRegen();
		ToggleShader2DEffect(state: false, 0);
		objAN.SetBool("isPotionAnim", value: false);
		crtPotionAnim = null;
		CleanVFXunderMaya("VFXpotion");
	}

	private void SubtractSpirit(float spiritCost, bool displayPopupNumber, Vector3 offsetPos)
	{
		GC.playerCurrentSpirit -= spiritCost;
		if (displayPopupNumber)
		{
			DisplayPopupNumbersUI(1f, Mathf.RoundToInt(spiritCost), 0, offsetPos, new Color(0.15f, 0.9f, 0.9f), Color.black, "-");
		}
	}

	private void DoSpiritConstantRegeneration()
	{
		CheckCurrentSpiritMultiplier();
		CheckCurrentLifeRegenMultiplier();
		if (GC.playerCurrentSpirit < GC.playerMaxSpirit || GC.playerCurrentHealth < (float)GC.playerMaxHealth || GC.playerIsBeingGrabed || GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified))
		{
			float num = GC.playerBaseSpiritRegeneration * freqDoSpiritRegen * currentSpiritRegenMultiplier;
			float num2 = GC.playerBaseSpiritRegeneration * freqDoSpiritRegen * currentLIFERegenMultiplier;
			if (currentSkillState != playerSkillState.Null || GC.playerCurrentHealth <= 0f)
			{
				num = 0f;
				num2 = 0f;
			}
			if (GC.configDifficultyType == GameCore.DifficultyType.Easy)
			{
				num *= 2f;
				num2 *= 2f;
			}
			if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.DrinkManaRegen))
			{
				num *= 2f;
			}
			if (GC.playerIsBeingGrabed && ITEM_Behavior(GameCore.AccessoryBehavior.EroticDancer))
			{
				num /= 2f;
			}
			GC.playerCurrentSpirit += num;
			if (CheckLifeRegenFromItems())
			{
				GC.playerCurrentHealth += num2;
			}
			if (GC.playerCurrentSpirit < 0f)
			{
				GC.playerCurrentSpirit = 0f;
			}
			if (GC.playerCurrentHealth < 0f)
			{
				GC.playerCurrentHealth = 0f;
			}
			if (GC.playerCurrentSpirit > GC.playerMaxSpirit && GC.playerMaxSpirit > 0f)
			{
				GC.playerCurrentSpirit = GC.playerMaxSpirit;
			}
			if (GC.playerCurrentHealth > (float)GC.playerMaxHealth && GC.playerMaxHealth > 0)
			{
				GC.playerCurrentHealth = GC.playerMaxHealth;
			}
		}
		else
		{
			if (GC.playerCurrentSpirit != GC.playerMaxSpirit && GC.playerMaxSpirit > 0f)
			{
				GC.playerCurrentSpirit = GC.playerMaxSpirit;
			}
			if (GC.playerCurrentHealth != (float)GC.playerMaxHealth && GC.playerMaxHealth > 0)
			{
				GC.playerCurrentHealth = GC.playerMaxHealth;
			}
		}
		if (GC.playerCurrentSpirit <= 0f && ValidStruggleSituation())
		{
			PlayerForbidStruggle();
		}
		if (ITEM_Behavior(GameCore.AccessoryBehavior.SPLow25Stun) && GC.playerCurrentSpirit <= GC.MATH_ResultValueOfTotalPercentile(10f, GC.playerMaxSpirit) && CheckBasicLogicPermissions())
		{
			GC.ApplyEffectToPlayer(GameCore.TempPlayerEffect.Stuned, 5f, "PLAYERSCRIPT:DoSpirirtConstantRegen");
			GC.HealTheSpirit(5f, 0f);
		}
	}

	private void BeginPotionRegen(float timeDuration, float healAmount)
	{
		potDuration = timeDuration;
		potLifePercent = healAmount;
		InvokeRepeating("INVOKED_HealthRegen", 0f, freqDoSpiritRegen);
	}

	private void INVOKED_HealthRegen()
	{
		float f = GC.MATH_ResultValueOfTotalPercentile(potLifePercent, GC.playerMaxHealth) / (potDuration / freqDoSpiritRegen);
		GC.playerCurrentHealth += Mathf.RoundToInt(f);
		if (GC.playerCurrentHealth > (float)GC.playerMaxHealth)
		{
			GC.playerCurrentHealth = GC.playerMaxHealth;
		}
	}

	private void EndPotionRegen()
	{
		potDuration = 0f;
		potLifePercent = 0f;
		if (IsInvoking("INVOKED_HealthRegen"))
		{
			CancelInvoke("INVOKED_HealthRegen");
		}
	}

	private void CheckCurrentSpiritMultiplier()
	{
		if (isSkillShieldEffectActive)
		{
			currentSpiritRegenMultiplier = 0f;
			return;
		}
		if (GC.playerIsBeingGrabed)
		{
			if (GC.revertToOldStruggleBar)
			{
				currentSpiritRegenMultiplier = -0.2f;
			}
			else
			{
				currentSpiritRegenMultiplier = -0.1f;
			}
			return;
		}
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.Petrified))
		{
			if (GC.revertToOldStruggleBar)
			{
				currentSpiritRegenMultiplier = -0.75f;
			}
			else
			{
				currentSpiritRegenMultiplier = -0.2f;
			}
			return;
		}
		if (currentAtkState != playerBasicAttackState.Null)
		{
			currentSpiritRegenMultiplier = 0.25f;
			return;
		}
		if (currentPlayerState == playerState.Running || currentPlayerState == playerState.Jumping || currentPlayerState == playerState.Falling)
		{
			currentSpiritRegenMultiplier = 0.5f;
			return;
		}
		if (currentPlayerState != playerState.Standing)
		{
			_ = currentPlayerState;
			_ = 4;
		}
		currentSpiritRegenMultiplier = 1f;
	}

	private bool CheckLifeRegenFromItems()
	{
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.DrinkLifeRegen))
		{
			return true;
		}
		return false;
	}

	private void CheckCurrentLifeRegenMultiplier()
	{
		if (GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.DrinkLifeRegen))
		{
			currentLIFERegenMultiplier = 1f;
		}
	}

	private IEnumerator waitForPotionAnimation(float wait)
	{
		yield return new WaitForSeconds(wait);
		EndPotionDrinkAnimation();
	}

	private void BeginDefenseDurabilityRegen(float timeDuration, float healAmount)
	{
		defRegenDuration = timeDuration;
		defRegenPercent = healAmount;
		InvokeRepeating("INVOKED_DurabilityRegen", 0f, freqDoSpiritRegen);
		GC.InstantiateVFX(GameCore.VisualEffect.GrabSmokes, base.transform.position, "PLAYERCONTROL:SpawnDefRegen");
		GC.InstantiateVFX(GameCore.VisualEffect.PVFXRegenDefenseDurability, base.transform.position, "PLAYERCONTROL:RegenDurability", base.transform, returnOBJref: true).GetComponent<EffectLogic>().timeDurationLoop = timeDuration;
		GC.PlaySFX(GameCore.SFXBasic2.ShieldHit1, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		GC.PlaySFX(GameCore.SFXBasic2.Odd6, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
	}

	private void INVOKED_DurabilityRegen()
	{
		float f = GC.MATH_ResultValueOfTotalPercentile(defRegenPercent, GC.playerMaxDressDurability) / (defRegenDuration / freqDoSpiritRegen);
		GC.playerCurrentDressDurability += Mathf.RoundToInt(f);
		if (GC.playerCurrentDressDurability > GC.playerMaxDressDurability)
		{
			GC.playerCurrentDressDurability = GC.playerMaxDressDurability;
			EndDefenseDurabilityRegen();
		}
	}

	private void EndDefenseDurabilityRegen()
	{
		defRegenDuration = 0f;
		defRegenPercent = 0f;
		CleanVFXunderMaya("VFXDefRegen");
		if (IsInvoking("INVOKED_DurabilityRegen"))
		{
			CancelInvoke("INVOKED_DurabilityRegen");
		}
	}

	private void UpdatePlayerGlobalStats()
	{
		if (GC.canPlayerCanUpdateGlobalVariables)
		{
			if (GC.isPlayerFainted)
			{
				GC.playerCurrentWorldXPosition = base.transform.position.x;
				GC.playerCurrentWorldYPosition = base.transform.position.y - 1f;
				GC.canPlayerSaveGame = false;
			}
			else
			{
				GC.playerCurrentWorldXPosition = base.transform.position.x;
				GC.playerCurrentWorldYPosition = base.transform.position.y;
				GC.canPlayerSaveGame = canSaveGame;
			}
			GC.currentPlayerFacing = Mathf.RoundToInt(base.transform.localScale.x);
			GC.playerCurrentState = currentPlayerState.ToString();
			GC.playerCurrentSkill = currentSkillState.ToString();
			GC.playerCurrentAttackState = currentAtkState.ToString();
			GC.playerCurrentMoveX = playerCurrentXMove;
			GC.playerCurrentRBVelocity = objRB.velocity;
			GC.playerIsHurt = isHurt;
			GC.playerIsInvincible = isInvincible;
			GC.playerCanUseTeleport = canUseTeleport;
			GC.playerSpecialAnimCount = objAN.GetInteger("countSpecial");
			GC.playerIsCrouching = objAN.GetBool("isCrouching");
			GC.playerLastUnstuckPosition = lastUnstuckPosition;
			GC.playerIsOnGround = CheckCollisionGround(ignoreEnemyCheck: true);
			Animator animator = objAN;
			if ((object)animator != null && animator.GetCurrentAnimatorClipInfo(0)?.Length > 0)
			{
				GC.playerCurrentAnimState = objAN.GetCurrentAnimatorClipInfo(0)[0].clip.name;
			}
			currStatHack = currentPlayerState.ToString();
		}
		ITEM_RecurringEffectChecks();
		FixedLookCameraBehaviour();
		RefreshUI();
	}

	private void INVOKED_EVERY_SECOND()
	{
		if ((bool)GC && GC.ACC_Check(GameCore.AccessoryBehavior.JoyDevice))
		{
			GC.AddRemoveLustToPlayer(1, doVFX: true);
		}
		TRAIT_EXIB_BEHAVIOR();
		TRAIT_HORNY_BEHAVIOR();
		TRAIT_NYMPHO_BEHAVIOR();
		TRAIT_VOREWHORE_BEHAVIOR();
	}

	private void BlockPlayerInputs(float duration)
	{
		if (crtInputBlock != null)
		{
			StopCoroutine(crtInputBlock);
		}
		crtInputBlock = StartCoroutine(waitBlockPlayerInput(duration));
	}

	private IEnumerator waitBlockPlayerInput(float wait)
	{
		GC.isPlayerInteractingLocked = true;
		yield return new WaitForSeconds(wait);
		GC.isPlayerInteractingLocked = false;
		crtInputBlock = null;
	}

	public void ChangePlayerSpriteRendererSortingOrder(int newLayer, bool resetToOriginal = false)
	{
		if (resetToOriginal)
		{
			isCustomSort = false;
			objRE.sortingOrder = originalSortingOrder;
		}
		else
		{
			isCustomSort = true;
			objRE.sortingOrder = newLayer;
		}
	}

	public void SetInvincibleFromPortalScript()
	{
		IsInvincibleToggleSelf(setInvnicibleState: true);
	}

	private void ToggleEroGrabZoom()
	{
		if (crtDelayEroZoomKeyPress == null)
		{
			crtDelayEroZoomKeyPress = StartCoroutine(waitForDelayEroZoomKeyPress(0.25f));
			coreCAM.InitiateZoom(ControllerCamera.ZoomType.PlayerKeyPress, hasTimeLimit: false, 0f);
		}
	}

	private void ResetThePlayer()
	{
		currentPlayerState = playerState.Standing;
		objRB.velocity = Vector2.zero;
		EndSpecialSkill(forceEnd: true);
	}

	private void FixedLookCameraBehaviour()
	{
		if (isPressingDownwards && canCameraLook)
		{
			countCameraLookDownTimer += freqUpdateGlobalStats;
			if (countCameraLookDownTimer >= delayToMoveLookCamera)
			{
				coreCAM.MoveCameraFixedDirectionOffset(ControllerCamera.camDirection.Down, 5f, 0.75f, Vector2.zero);
			}
			if (countCameraLookUpTimer != 0f)
			{
				countCameraLookUpTimer = 0f;
			}
		}
		else
		{
			if (countCameraLookDownTimer != 0f)
			{
				countCameraLookDownTimer = 0f;
			}
			coreCAM.StopCameraFixedDirectionOffset(ControllerCamera.camDirection.Down);
		}
		if (isPressingUpwards && canCameraLook)
		{
			countCameraLookUpTimer += freqUpdateGlobalStats;
			if (countCameraLookUpTimer >= delayToMoveLookCamera)
			{
				coreCAM.MoveCameraFixedDirectionOffset(ControllerCamera.camDirection.Up, 3f, 0.75f, Vector2.zero);
			}
			if (countCameraLookDownTimer != 0f)
			{
				countCameraLookDownTimer = 0f;
			}
		}
		else
		{
			if (countCameraLookUpTimer != 0f)
			{
				countCameraLookUpTimer = 0f;
			}
			coreCAM.StopCameraFixedDirectionOffset(ControllerCamera.camDirection.Up);
		}
	}

	private bool CheckRefCoreUI()
	{
		if (coreUI != null)
		{
			return true;
		}
		GameObject gameObject = GameObject.Find("[CANVAS]");
		if (gameObject != null)
		{
			coreUI = gameObject.GetComponent<GameUI>();
		}
		if (coreUI != null)
		{
			return true;
		}
		return false;
	}

	private void Spawn2DSpriteTrail()
	{
		GameObject obj = new GameObject();
		SpriteRenderer spriteRenderer = obj.AddComponent<SpriteRenderer>();
		Color color = objRE.color;
		color.a = 0.5f;
		spriteRenderer.material = objRE.material;
		spriteRenderer.color = color;
		spriteRenderer.sprite = objRE.sprite;
		spriteRenderer.sortingOrder = originalSortingOrder - 1;
		obj.transform.position = base.transform.position;
		obj.transform.localScale = base.transform.localScale;
		Object.Destroy(obj, 0.25f);
	}

	private void PlayerQuickSaveGameData()
	{
		if (GC.doQuickSaveFeature && !GC.savingPlayerData && !isSaveDelayAfterLoad)
		{
			GC.SaveGameData(0);
			GC.DisplaySystemMessageUI(0, 60);
		}
	}

	private void BeginDelayToEnterTeleport(float duration)
	{
		if (crtDelayTeleportUse != null)
		{
			StopCoroutine(crtDelayTeleportUse);
		}
		crtDelayTeleportUse = StartCoroutine(waitDelayTeleportUsage(duration));
	}

	private void HackyDevelopmentScreenshot()
	{
	}

	private void DEBUG_DisplayBasicInfo()
	{
		Debug.Log("|=================================|");
		Debug.Log("!!! [START OF DEBUG INFO] !!!");
		Debug.Log("|=================================|");
		Debug.Log("LAtex Effect exists?: " + GC.playerEffectList.Exists((BuffEffect x) => x.effect == GameCore.TempPlayerEffect.LatexBodySuit));
		Debug.Log(">|---END OF DEBUG---|<");
	}

	private void DEBUG_AddPower()
	{
		Vector2 itemPosition = new Vector2(9999f, 9999f);
		string text = "description";
		int[] array = new int[4] { 4, 6, 7, 5 };
		int[] arrayDBid = new int[4] { 7, 10, 11, 12 };
		int i;
		for (i = 0; i < array.Length; i++)
		{
			if (GC.AddThisPowerUp((GameCore.CollectibleType)array[i]))
			{
				text = GC.clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == arrayDBid[i])?.description;
				GC.DisplaySystemMessageUI(123456, 0, 0f, text);
				GC.AddItemToPlayerInventory(arrayDBid[i], GC.strCurrentActiveSceneName, itemPosition);
			}
		}
	}

	private void StartCountSpecialAnim(float frequencyCount, int MaxCount, bool spawnEffect, Vector2 spawnPos, GameCore.VisualEffect spawnVFX)
	{
		if (crtCountSpecialAnim != null)
		{
			StopCoroutine(crtCountSpecialAnim);
		}
		crtCountSpecialAnim = StartCoroutine(countSpecialAnimation(frequencyCount, MaxCount, spawnEffect, spawnPos, spawnVFX));
	}

	public void AddDelayNPCTalk(float delay)
	{
		if (crtDelayNPCTalk != null)
		{
			StopCoroutine(crtDelayNPCTalk);
		}
		crtDelayNPCTalk = StartCoroutine(waitDelayNPCTalk(delay));
	}

	private IEnumerator waitDelayNPCTalk(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtDelayNPCTalk = null;
	}

	private IEnumerator waitPlayerFallTroughPassPlat(float waittime)
	{
		yield return new WaitForSeconds(waittime);
		isPassTroughPlatform = false;
		coroutPlayerPassTroughPlatform = null;
	}

	private IEnumerator waitRestorePassPlatColliders(float waittime, Collider2D tmp1, Collider2D tmp2, Collider2D tmp3)
	{
		yield return new WaitForSeconds(waittime);
		if ((bool)tmp1)
		{
			Physics2D.IgnoreCollision(objBox2D, tmp1, ignore: false);
			Physics2D.IgnoreCollision(objPolygon2d, tmp1, ignore: false);
		}
		if ((bool)tmp2)
		{
			Physics2D.IgnoreCollision(objBox2D, tmp2, ignore: false);
			Physics2D.IgnoreCollision(objPolygon2d, tmp2, ignore: false);
		}
		if ((bool)tmp3)
		{
			Physics2D.IgnoreCollision(objBox2D, tmp3, ignore: false);
			Physics2D.IgnoreCollision(objPolygon2d, tmp3, ignore: false);
		}
		crtWaitRestorePassPlatColliders = null;
	}

	private IEnumerator waitDelayTeleportUsage(float waittime)
	{
		canUseTeleport = false;
		yield return new WaitForSeconds(waittime);
		canUseTeleport = true;
		crtDelayTeleportUse = null;
	}

	private IEnumerator waitForSkipDialogueText(float wait)
	{
		canFastSkipDialogue = false;
		yield return new WaitForSeconds(wait);
		crtWaitForSkipDialogue = null;
		canFastSkipDialogue = true;
	}

	private IEnumerator waitForCrouchDelayAfterLoad(float wait)
	{
		yield return new WaitForSeconds(wait);
		isLoadDelay = false;
		crtWaitForLoadDelay = null;
	}

	private IEnumerator waitForSaveDelay(float wait)
	{
		yield return new WaitForSeconds(wait);
		isSaveDelayAfterLoad = false;
		crtWaitSaveDelay = null;
	}

	private IEnumerator waitForDelayEroZoomKeyPress(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtDelayEroZoomKeyPress = null;
	}

	private IEnumerator timerThatCountsPlayedTime()
	{
		while (runTimePlayedCoroutine)
		{
			yield return new WaitForSecondsRealtime(1f);
			if ((bool)GC)
			{
				GC.timeTotalGamePlayedSeconds++;
			}
		}
	}

	private IEnumerator waitMinimapToggleHold(float wait)
	{
		yield return new WaitForSeconds(wait);
		if (!GC.isPlayerFainted && !GC.playerIsBeingGrabed)
		{
			GC.MINIMAP_Toggle(toggleCurrentState: true, setThisSpecificState: false);
		}
		crtMinimapToggleHold = null;
	}

	private IEnumerator countSpecialAnimation(float frequencyIncrease, int limitMax, bool onIncreaseSpawnVFX, Vector2 thisPos, GameCore.VisualEffect thisVFX = GameCore.VisualEffect.Null)
	{
		specialAnimCounter = objAN.GetInteger("countSpecial");
		specialAnimMax = limitMax;
		while (specialAnimCounter < limitMax)
		{
			yield return new WaitForSeconds(frequencyIncrease);
			if (GC.playerIsBeingGrabed)
			{
				specialAnimCounter = 0;
				objAN.SetInteger("countSpecial", 0);
				break;
			}
			specialAnimCounter = objAN.GetInteger("countSpecial");
			specialAnimCounter++;
			objAN.SetInteger("countSpecial", specialAnimCounter);
			if (onIncreaseSpawnVFX && thisVFX != GameCore.VisualEffect.Null)
			{
				GC.InstantiateVFX(thisVFX, thisPos, "Controller_Player:countSpecialAnim");
				GC.PlaySFX(GameCore.SFXBasic2.Odd2, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			}
		}
		if (specialAnimCounter > limitMax)
		{
			specialAnimCounter = limitMax;
			objAN.SetInteger("countSpecial", specialAnimCounter);
		}
		crtCountSpecialAnim = null;
	}

	private void SKIP_SPECIAL_ANIM_INCREASE_PHASE()
	{
		if (specialAnimCounter < specialAnimMax)
		{
			specialAnimCounter++;
			objAN.SetInteger("countSpecial", specialAnimCounter);
			return;
		}
		StopCoroutine(crtCountSpecialAnim);
		specialAnimCounter = specialAnimMax;
		objAN.SetInteger("countSpecial", specialAnimCounter);
		crtCountSpecialAnim = null;
	}

	private void ToggleEssenceAttractor(bool state)
	{
		essenceAttractor.SetActive(state);
	}

	private bool IsBuild(int build)
	{
		switch (build)
		{
		case 0:
			if (GC.GameBuildCurrent == GameCore.GameBuildType.Win64 || GC.GameBuildCurrent == GameCore.GameBuildType.Mac || GC.GameBuildCurrent == GameCore.GameBuildType.Linux)
			{
				return true;
			}
			break;
		case 3:
			if (GC.GameBuildCurrent == GameCore.GameBuildType.WebGL)
			{
				return true;
			}
			break;
		}
		return false;
	}

	private IEnumerator waitDelayFadeOut(float wait)
	{
		GC.SceneFadeHandler(actionFadeToBlack: true, 0f);
		yield return new WaitForSeconds(wait);
		GC.SceneFadeHandler(actionFadeToBlack: false, 777f);
		crtDelayFadeOut = null;
	}
}
