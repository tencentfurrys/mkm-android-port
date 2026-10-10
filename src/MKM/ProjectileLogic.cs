using System.Collections;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
	public enum OwnerOfProjectile
	{
		Enemy,
		Player
	}

	public enum LocalSoundType
	{
		Null,
		Basic1,
		Basic2,
		Kitsune1,
		ThrowDagger,
		SmallRuneBomb,
		ExplosionRune,
		RockCrash,
		LamiaCast,
		LamiaSnakeAppear,
		Chirp1,
		WWExplode,
		RavenCaw,
		TreeNut,
		PowerSlash,
		MothSpore,
		MothCast,
		MothBurst,
		DwarfPickaxe,
		CrystalLaser,
		IceBig,
		IceSmall,
		IceBreak,
		IceGen1,
		IceGen2,
		Boing3,
		Tornado,
		Cloud,
		Thunder,
		Weird,
		TornadoHighPitch,
		LaserBig,
		CircularSaw,
		LowShoot,
		WyvernFleshCord,
		ThrowDaggerHigh,
		Pound,
		MindBlast1,
		MindBlast2,
		ElectricMine,
		Spawn
	}

	public OwnerOfProjectile Originator;

	public GameCore.PlayerDamageType playerDmgType;

	public GameCore.ProjectileType currentProjectileType;

	public LocalSoundType PlayThisSpawnSound;

	public LocalSoundType PlayThisHitSound;

	public LocalSoundType PlayThisDisableSound;

	public GameCore.VisualEffect VisualEffectOnHit;

	public GameCore.VisualEffect VisualEffectOnTimeOut;

	public bool isStatic;

	public bool isRotateOwnZAxis;

	public bool isFacePlayerAtSpawn;

	public bool isFacePlayerConstant;

	public bool PointZAxisOnSpawn;

	public bool isPointZAxisConstant;

	public bool isFacePointReferenceX;

	public float pointReferenceXFacing;

	public int touchDamage;

	public float moveSpeed;

	public float timerDuration;

	public float timerFrequency;

	public float specialGravityRate;

	public float specialHorizDragRate;

	public float specialVertDragRate;

	public float ZAxisRotationSpeed;

	public string specialTag;

	public Vector2 specialForce;

	public bool canCollideFainted;

	public bool spawnSpecialOnDisable;

	public GameObject specialObj1;

	public GameObject specialObj2;

	public GameObject objTrailVFX;

	public GameObject specialProjectileInherit;

	public bool changeCollPerAnim;

	public AttackColliderPerFrame[] listCollChange;

	public float trailFreq;

	public bool IgnoreFloorCollision;

	public bool OnDisableSendToCleanupParent;

	public bool InstaDisableOnGrabbed;

	public bool InstaDisableOnFainted;

	public bool canSpringBoard;

	public bool playerException;

	public bool HackCameraChase;

	public bool shaderOnSpawn;

	public bool isPingPongUpDown;

	public float smoothStepSpeed;

	public float rangePingPong;

	public float speedPingPong;

	public bool checkPlayerAttackThis;

	public bool selfCollEnemyAttack;

	public bool disableRotationOnColl;

	public PROJECTILE_OrbiterParameters coreOrbit;

	public ENM_EroPhaseControl[] arrayAnimPhase;

	[HideInInspector]
	public GameObject objOriginator;

	private Coroutine crtDelayedShot;

	private Vector2 originPosition;

	private Vector2 targetPosition;

	private Vector2 moveHeading;

	private Vector2 moveDirection;

	private Quaternion quatRotation;

	private float countFrequency;

	private float moveDistance;

	private float countGeneric;

	private float spHorIncrease;

	private float spHorMoveLimit;

	private float playerXOffset;

	private float playerYOffset;

	private bool allowedToPlayHitSound;

	private bool doShotAfterDelay;

	private int ogTouchDamage;

	private int intCountGeneric;

	private int animCount;

	private int animLocalPhase;

	private GameObject objPlayerRef;

	private GameObject[] arrayGenericObjects;

	private Animator objAN;

	private Collider2D coll2D;

	private BoxCollider2D box2D;

	private Rigidbody2D objRB;

	private SpriteRenderer objRE;

	private GameCore GC;

	private void Start()
	{
		playerXOffset = 0f;
		playerYOffset = 0f;
		InitializeReferences();
		if (Originator == OwnerOfProjectile.Enemy)
		{
			InitializeEnemyProjectileBehaviour();
		}
		else if (Originator == OwnerOfProjectile.Player)
		{
			InitializePlayerProjectileBehaviour(Vector2.zero);
		}
		if (isFacePlayerAtSpawn)
		{
			FlipThisSpriteToFaceThePlayerPosition(PointZAxisOnSpawn);
		}
		if (isFacePointReferenceX && transform.position.x > pointReferenceXFacing)
		{
			objRE.flipX = true;
		}
		if (shaderOnSpawn)
		{
			ToggleShaderEfectOn();
		}
		if (playerDmgType == GameCore.PlayerDamageType.entropicExplosion)
		{
			GC.DoMirrorEffectImageUI(1f, clrRed: true);
		}
		PlayInitialShootSFX();
	}

	private void InitializeReferences()
	{
		GC = GameCore.Instance;
		objRB = GetComponent<Rigidbody2D>();
		objRE = GetComponent<SpriteRenderer>();
		coll2D = GetComponent<Collider2D>();
		if (coll2D.GetType() == typeof(BoxCollider2D))
		{
			box2D = GetComponent<BoxCollider2D>();
		}
		intCountGeneric = 0;
		allowedToPlayHitSound = true;
		if (specialGravityRate == 0f)
		{
			specialGravityRate = 0.1f;
		}
		if (ZAxisRotationSpeed == 0f)
		{
			ZAxisRotationSpeed = 150f;
		}
		if (GC.isBossRematch && Originator == OwnerOfProjectile.Enemy)
		{
			touchDamage *= 2;
		}
		if (HackCameraChase)
		{
			objPlayerRef = GameObject.FindGameObjectWithTag("Player");
		}
		ogTouchDamage = touchDamage;
	}

	private void InitializeEnemyProjectileBehaviour()
	{
		bool flag = true;
		originPosition = transform.position;
		countFrequency = 0f;
		switch (currentProjectileType)
		{
		case GameCore.ProjectileType.Static:
			if (!isStatic)
			{
				isStatic = true;
				moveDirection = Vector2.zero;
			}
			if (specialTag == "WitchSpawnCircle")
			{
				InvokeRepeating("Invoked_WitchSpawnCircle", 1f, 0.5f);
			}
			if (specialTag == "BlueFoxGroundShock")
			{
				StartCoroutine(waitToDoGenericStuff(0f, "shockWalls"));
			}
			break;
		case GameCore.ProjectileType.StraightLine:
			if (specialTag == "DelayedShot")
			{
				flag = CheckDelayedShotStatus(specialForce.y);
				moveDirection.x = specialForce.x;
			}
			if (gameObject.name.Contains("BLF-SideShockB"))
			{
				if (transform.position.x > 0f)
				{
					specialTag = "left";
				}
				else
				{
					specialTag = "right";
				}
				StartCoroutine(waitToDoGenericStuff(1f, "BLF-SideShockB"));
			}
			if (specialTag == "left")
			{
				moveDirection.x = -1f;
				moveDirection.y = 0f;
			}
			else if (specialTag == "right")
			{
				moveDirection.x = 1f;
				moveDirection.y = 0f;
			}
			else if (specialTag == "up")
			{
				moveDirection.x = 0f;
				moveDirection.y = 1f;
			}
			else if (specialTag == "down")
			{
				moveDirection.x = 0f;
				moveDirection.y = -1f;
			}
			else if (GC.playerCurrentWorldXPosition >= transform.position.x)
			{
				moveDirection.x = 1f;
			}
			else
			{
				moveDirection.x = -1f;
			}
			break;
		case GameCore.ProjectileType.TargetPosition:
			if (specialTag == "DelayedShot")
			{
				if (!CheckDelayedShotStatus(specialForce.y))
				{
					return;
				}
				GC.PlaySFX(GameCore.SFXBasic1.Shoot1, 0, checkForDuplicity: true, transform.position, 0f, 0f);
			}
			flag = true;
			targetPosition.x = GC.playerCurrentWorldXPosition;
			targetPosition.y = GC.playerCurrentWorldYPosition;
			switch (specialTag)
			{
			case "Offset":
				targetPosition.x += specialForce.x;
				targetPosition.y += specialForce.y;
				break;
			case "RandomOffset":
				targetPosition.x += GC.MATH_RNG_FLOAT(0f - specialForce.x, specialForce.x);
				targetPosition.y += GC.MATH_RNG_FLOAT(0f - specialForce.y, specialForce.y);
				break;
			case "DirectPoint":
				targetPosition.x = specialForce.x;
				targetPosition.y = specialForce.y;
				break;
			}
			moveHeading = targetPosition - originPosition;
			moveDistance = moveHeading.magnitude;
			moveDirection = moveHeading / moveDistance;
			break;
		case GameCore.ProjectileType.Ballistic:
		case GameCore.ProjectileType.BallisticBossSlime:
		{
			Vector2 vector6 = transform.position;
			Vector2 specialCheck = specialForce;
			Vector2 vector7 = new Vector2(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition);
			bool increaseHeightSpecial = false;
			if (currentProjectileType == GameCore.ProjectileType.BallisticBossSlime)
			{
				float num = 5f;
				vector7.y += num;
				if (!GC.CheckPlayerProximityToAnyObject(transform.position, GameCore.PlayerProximityType.CustomXOnly, num))
				{
					num--;
					if (transform.position.x > GC.playerCurrentWorldXPosition)
					{
						vector7.x += num;
					}
					else
					{
						vector7.x -= num;
					}
				}
			}
			if (specialTag == "HarunaHat")
			{
				increaseHeightSpecial = true;
				vector7.x += 4f;
			}
			if (specialTag == "Condom")
			{
				increaseHeightSpecial = true;
				vector7.x += specialForce.x;
			}
			if (specialTag == "Toy")
			{
				increaseHeightSpecial = true;
				vector7.x = specialForce.x;
				vector7.y = specialForce.y;
			}
			if (specialTag == "Height")
			{
				increaseHeightSpecial = true;
			}
			if (specialTag == "RandomOffset")
			{
				vector7.x += GC.MATH_RNG_FLOAT(0f - specialForce.x, specialForce.x);
				vector7.y += GC.MATH_RNG_FLOAT(0f - specialForce.y, specialForce.y);
			}
			if (specialTag == "RandomOffsetSelfRef")
			{
				vector7 = transform.position;
				vector7.x += GC.MATH_RNG_FLOAT(0f - specialForce.x, specialForce.x);
				vector7.y += GC.MATH_RNG_FLOAT(0f - specialForce.y, specialForce.y);
			}
			if (specialTag == "Carrot")
			{
				vector7.y += 5f;
				increaseHeightSpecial = true;
				if (vector7.x > transform.position.x)
				{
					vector7.x--;
				}
				else
				{
					vector7.x++;
				}
			}
			if (specialTag == "WitchBasic")
			{
				rangePingPong += GC.MATH_RNG_FLOAT(0.1f, 0.75f);
				increaseHeightSpecial = true;
				vector7.y += GC.MATH_RNG_FLOAT(-0.5f, 0.5f);
				vector7.x += GC.MATH_RNG_FLOAT(-1f, 1f);
			}
			objRB.velocity = GC.MATH_CALC_BALLISTIC_TRAJECTORY_TO_POINT(vector6, vector7, increaseHeightSpecial, specialCheck);
			flag = false;
			break;
		}
		case GameCore.ProjectileType.VerticalDown:
			if ((bool)specialObj1)
			{
				Object.Instantiate(specialObj1, transform.position, Quaternion.identity, transform.parent);
			}
			moveDirection.x = 0f;
			moveDirection.y = -1f;
			break;
		case GameCore.ProjectileType.ArchRandom:
			if (specialForce.x == 0f && specialForce.y == 0f)
			{
				if (GC.MATH_RNG_INT(0, 1) == 0)
				{
					specialForce.x = GC.MATH_RNG_FLOAT(-2f, -0.5f);
				}
				else
				{
					specialForce.x = GC.MATH_RNG_FLOAT(0.5f, 2f);
				}
				specialForce.y = GC.MATH_RNG_FLOAT(1.5f, 3f);
			}
			moveDirection.x = specialForce.x;
			moveDirection.y = specialForce.y;
			break;
		case GameCore.ProjectileType.ChasingSlow:
		{
			Vector3 vector4 = new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, transform.position.z);
			spHorIncrease = 0.2f;
			spHorMoveLimit = 1f;
			moveDirection = (vector4 - transform.position).normalized;
			flag = true;
			InvokeRepeating("Special_InvokedChase", 0.2f, 0.2f);
			break;
		}
		case GameCore.ProjectileType.ChasingMedium:
		{
			Vector3 vector5 = new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, transform.position.z);
			spHorIncrease = 0.2f;
			spHorMoveLimit = 2f;
			moveDirection = (vector5 - transform.position).normalized;
			flag = true;
			InvokeRepeating("Special_InvokedChase", 0.1f, 0.1f);
			break;
		}
		case GameCore.ProjectileType.ChasingSlowHorizontal:
		{
			Vector3 vector = new Vector3(GC.playerCurrentWorldXPosition, transform.position.y, transform.position.z);
			spHorIncrease = 0.6f;
			spHorMoveLimit = 2f;
			moveDirection = (vector - transform.position).normalized;
			flag = true;
			InvokeRepeating("Special_ChaseSlowLamiaVersion", 0.1f, 0.1f);
			break;
		}
		case GameCore.ProjectileType.SpecialForestBossSpore:
			Special_ForestBossSpore();
			break;
		case GameCore.ProjectileType.SpecialForestBossSeed:
			flag = false;
			Special_ForestBossSeed();
			break;
		case GameCore.ProjectileType.SpecialBeachBossWaves:
			Special_BeachBossWaves();
			break;
		case GameCore.ProjectileType.RollingBall:
			if (GC.playerCurrentWorldXPosition < transform.position.x)
			{
				ZAxisRotationSpeed *= -1f;
			}
			moveDirection.y = -0.1f;
			if (specialTag == "BallisticSpawn")
			{
				Vector2 vector3 = transform.position;
				vector3.y += GC.MATH_RNG_FLOAT(0.75f, 1.5f);
				if (GC.MATH_RNG_INT(0, 1) == 1)
				{
					vector3.x += GC.MATH_RNG_FLOAT(-1.5f, -0.5f);
				}
				else
				{
					vector3.x += GC.MATH_RNG_FLOAT(0.5f, 1.5f);
				}
				objRB.velocity = GC.MATH_CALC_BALLISTIC_TRAJECTORY_TO_POINT(transform.position, vector3, increaseHeightSpecial: false, Vector2.zero);
				flag = false;
			}
			if (specialTag == "BlueFoxMine")
			{
				flag = false;
			}
			break;
		case GameCore.ProjectileType.BoomerangLine:
			moveDirection.x = specialForce.x;
			moveDirection.y = 0f;
			break;
		case GameCore.ProjectileType.BoomerangAnyDir:
			if (specialTag == "PlayerPos")
			{
				targetPosition.x = GC.playerCurrentWorldXPosition;
				targetPosition.y = GC.playerCurrentWorldYPosition;
			}
			moveHeading = targetPosition - originPosition;
			moveDistance = moveHeading.magnitude;
			moveDirection = moveHeading / moveDistance;
			moveDirection.x *= 4f;
			moveDirection.y *= 4f;
			specialForce = moveDirection;
			if (specialHorizDragRate <= 0f)
			{
				specialHorizDragRate = specialForce.x / 100f;
			}
			if (specialVertDragRate <= 0f)
			{
				specialVertDragRate = specialForce.y / 100f;
			}
			if (specialForce.x < 0f && specialHorizDragRate < 0f)
			{
				specialHorizDragRate *= -1f;
			}
			if (specialForce.y < 0f && specialVertDragRate < 0f)
			{
				specialVertDragRate *= -1f;
			}
			break;
		case GameCore.ProjectileType.SpecialPrincessBoss:
			if (specialTag == "spawn")
			{
				arrayGenericObjects = new GameObject[3];
				for (int i = 0; i < arrayGenericObjects.Length; i++)
				{
					arrayGenericObjects[i] = specialObj1;
				}
			}
			break;
		case GameCore.ProjectileType.WitchCloud:
		{
			Vector3 vector2 = new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, transform.position.z);
			spHorIncrease = 0.75f;
			spHorMoveLimit = 2.5f;
			moveDirection = (vector2 - transform.position).normalized;
			playerYOffset = 1.5f;
			flag = true;
			InvokeRepeating("Special_InvokedChase", 0.1f, 0.1f);
			InvokeRepeating("Invoked_WitchCloud", 2f, 2f);
			break;
		}
		case GameCore.ProjectileType.OrbitAround:
			if (coreOrbit.radiusBase == 0f)
			{
				coreOrbit.radiusBase = 0.5f;
			}
			if (coreOrbit.radiusSpeed == 0f)
			{
				coreOrbit.radiusSpeed = 0.5f;
			}
			if (coreOrbit.rotationSpeed == 0f)
			{
				coreOrbit.rotationSpeed = 100f;
			}
			if (coreOrbit.axisRotation == Vector3.zero)
			{
				coreOrbit.axisRotation = new Vector3(0f, 0f, 1f);
			}
			InvokeRepeating("OrbitAroundIncrementalValues", 0f, timerFrequency);
			break;
		}
		if (objTrailVFX != null)
		{
			InvokeRepeating("Invoked_SpawnTrailEffect", trailFreq, trailFreq);
		}
		if (!isStatic & flag)
		{
			objRB.velocity = moveDirection * moveSpeed;
		}
		InvokeRepeating("ProjectileTimer", 0f, timerFrequency);
	}

	private bool MATH_CheckBallisticHeightSpecial(float horDis, float verDis)
	{
		if (specialForce != Vector2.zero && horDis <= specialForce.x && horDis >= 0f - specialForce.x && verDis <= specialForce.y && verDis >= 0f - specialForce.y)
		{
			return true;
		}
		return false;
	}

	private void Update()
	{
		CheckForSpecialBehavioursOnUpdate();
		if (GC.configDifficultyType == GameCore.DifficultyType.Easy && touchDamage == ogTouchDamage)
		{
			touchDamage = Mathf.RoundToInt((float)touchDamage * 0.5f);
		}
		else if (GC.configDifficultyType == GameCore.DifficultyType.Normal && touchDamage != ogTouchDamage)
		{
			touchDamage = ogTouchDamage;
		}
		if (isPingPongUpDown)
		{
			transform.position = new Vector3(transform.position.x, Mathf.SmoothStep(transform.position.y, transform.position.y + Mathf.PingPong(Time.time / speedPingPong, rangePingPong) - rangePingPong / 2f, smoothStepSpeed * Time.deltaTime));
		}
	}

	private void FixedUpdate()
	{
		if (isRotateOwnZAxis)
		{
			FXU_AXIS_ROTATE();
		}
		switch (currentProjectileType)
		{
		case GameCore.ProjectileType.ArchRandom:
		case GameCore.ProjectileType.SmallRuneBomb:
			moveDirection.y -= specialGravityRate;
			objRB.velocity = moveDirection * moveSpeed;
			break;
		case GameCore.ProjectileType.BoomerangLine:
		case GameCore.ProjectileType.PlayerBoomerang:
			if (specialForce.x > 0f)
			{
				moveDirection.x -= specialHorizDragRate;
			}
			else
			{
				moveDirection.x += specialHorizDragRate;
			}
			if (moveDirection.x < -1f)
			{
				moveDirection.x = -1f;
			}
			else if (moveDirection.x > 1f)
			{
				moveDirection.x = 1f;
			}
			objRB.velocity = moveDirection * moveSpeed;
			break;
		case GameCore.ProjectileType.BoomerangAnyDir:
			if (specialForce.x > 0f)
			{
				moveDirection.x -= specialHorizDragRate;
			}
			else
			{
				moveDirection.x += specialHorizDragRate;
			}
			if (specialForce.y > 0f)
			{
				moveDirection.y -= specialVertDragRate;
			}
			else
			{
				moveDirection.y += specialVertDragRate;
			}
			objRB.velocity = moveDirection * moveSpeed;
			break;
		case GameCore.ProjectileType.OrbitAround:
			transform.RotateAround(transform.parent.position, coreOrbit.axisRotation, coreOrbit.rotationSpeed * Time.deltaTime);
			targetPosition = (transform.position - transform.parent.position).normalized * coreOrbit.radiusBase + transform.parent.position;
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, Time.deltaTime * coreOrbit.radiusSpeed);
			if (!isRotateOwnZAxis)
			{
				quatRotation = transform.rotation;
				quatRotation.z = 0f;
				transform.rotation = quatRotation;
			}
			break;
		}
		if (isPointZAxisConstant)
		{
			FlipThisSpriteToFaceThePlayerPosition(pointInTheZAxis: true);
		}
		else if (isFacePlayerConstant)
		{
			FlipThisSpriteToFaceThePlayerPosition(pointInTheZAxis: false);
		}
		if ((InstaDisableOnFainted && GC.isPlayerFainted) || (InstaDisableOnGrabbed && GC.playerIsBeingGrabed))
		{
			ProjectileDisableBehaviour(spawnTimeOutEffect: false);
		}
		if (objPlayerRef != null && gameObject.activeInHierarchy)
		{
			objPlayerRef.transform.position = transform.position;
		}
	}

	private void FXU_AXIS_ROTATE()
	{
		if (gameObject.transform.parent != null && gameObject.transform.parent.tag == "Projectile-Basic")
		{
			if (objRE.flipX)
			{
				transform.localRotation = Quaternion.Euler(0f, 0f, objRB.rotation + ZAxisRotationSpeed * Time.fixedDeltaTime);
			}
			else
			{
				transform.localRotation = Quaternion.Euler(0f, 0f, objRB.rotation - ZAxisRotationSpeed * Time.fixedDeltaTime);
			}
		}
		else if (objRE.flipX)
		{
			objRB.MoveRotation(objRB.rotation + ZAxisRotationSpeed * Time.fixedDeltaTime);
		}
		else
		{
			objRB.MoveRotation(objRB.rotation - ZAxisRotationSpeed * Time.fixedDeltaTime);
		}
	}

	private void CheckForSpecialBehavioursOnUpdate()
	{
		switch (currentProjectileType)
		{
		case GameCore.ProjectileType.SpecialForestBossSpore:
			if (moveDirection.y > 0f && transform.position.y >= targetPosition.y)
			{
				float num = 0f;
				num = ((GC.MATH_RNG_INT(0, 1) != 0) ? GC.MATH_RNG_FLOAT(0.5f, 10f) : GC.MATH_RNG_FLOAT(-10f, -0.5f));
				Vector3 position = transform.position;
				position.x += num;
				transform.position = position;
				moveDirection.x = 0f;
				moveDirection.y = -1f;
				moveSpeed = GC.MATH_RNG_FLOAT(3f, 4.5f);
				objRB.velocity = moveDirection * moveSpeed;
			}
			break;
		case GameCore.ProjectileType.SpecialBeachBossWaves:
			if (transform.position.y > 0.8f && moveDirection.y > 0f)
			{
				moveDirection.y = 0f;
				objRB.velocity = moveDirection * moveSpeed;
			}
			break;
		case GameCore.ProjectileType.RollingBall:
			if (objRB.velocity == Vector2.zero)
			{
				ZAxisRotationSpeed *= -1f;
			}
			break;
		}
	}

	private void InitializePlayerProjectileBehaviour(Vector2 specialPosition)
	{
		bool flag = true;
		bool flag2 = false;
		float x = GC.playerCurrentRBVelocity.x;
		_ = GC.playerCurrentRBVelocity;
		switch (currentProjectileType)
		{
		case GameCore.ProjectileType.ThrowDagger:
			moveDirection.x = specialForce.x;
			if (moveDirection.x < 0f)
			{
				flag2 = true;
			}
			if (playerDmgType == GameCore.PlayerDamageType.ForwardSlash)
			{
				if (GC.listAccBehavior.Contains(GameCore.AccessoryBehavior.SKILLEnhancePowerSlash))
				{
					timerDuration = 1f;
					break;
				}
				timerDuration = 1.5f;
				moveSpeed /= 2f;
			}
			break;
		case GameCore.ProjectileType.PlayerLance:
			moveDirection.x = specialForce.x;
			moveDirection.y = specialForce.y;
			if (moveDirection.x < 0f && moveDirection.y == 0f)
			{
				flag2 = true;
			}
			if (moveDirection.y > 0f)
			{
				transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
			}
			else if (moveDirection.y < 0f)
			{
				transform.localRotation = Quaternion.Euler(0f, 0f, -90f);
			}
			break;
		case GameCore.ProjectileType.PlayerBoomerang:
			moveDirection.x = specialForce.x;
			moveDirection.y = 0f;
			break;
		case GameCore.ProjectileType.SmallRuneBomb:
			moveDirection.x = specialForce.x * 1.25f + x / 4f;
			moveDirection.y = 1.1f;
			break;
		case GameCore.ProjectileType.PlayerTP:
			moveDirection.x = 0f;
			moveDirection.y = specialForce.y;
			break;
		case GameCore.ProjectileType.TargetPosition:
			flag = true;
			targetPosition.x = specialPosition.x;
			targetPosition.y = specialPosition.y;
			moveHeading = targetPosition - originPosition;
			moveDistance = moveHeading.magnitude;
			moveDirection = moveHeading / moveDistance;
			break;
		case GameCore.ProjectileType.SmallSwordSlash:
			moveDirection.x = specialForce.x;
			moveDirection.y = specialForce.y;
			if (moveDirection.x < 0f)
			{
				flag2 = true;
			}
			if (moveDirection.y > 0f)
			{
				transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
			}
			break;
		case GameCore.ProjectileType.PlayerHomingSphere:
			InvokeRepeating("IVK_AcquireClosestTarget", 0f, GC.MATH_RNG_FLOAT(0.2f, 0.5f));
			InvokeRepeating("IVK_PlayerHomingSphere", timerFrequency, timerFrequency);
			break;
		}
		if (flag2)
		{
			objRE.flipX = true;
		}
		if (objTrailVFX != null)
		{
			InvokeRepeating("Invoked_SpawnTrailEffect", trailFreq, trailFreq);
		}
		if (!isStatic & flag)
		{
			objRB.velocity = moveDirection * moveSpeed;
		}
		InvokeRepeating("ProjectileTimer", 0f, timerFrequency);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		UnifiedCollisionMethodCheck(other.gameObject);
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (canSpringBoard && other.gameObject.tag == "SpringBoard")
		{
			SimpleSpringBoard component = other.gameObject.GetComponent<SimpleSpringBoard>();
			if ((object)component != null && component.canPushUp && objRB.velocity.y <= 0.5f)
			{
				Vector2 velocity = objRB.velocity;
				velocity.y += moveSpeed;
				objRB.velocity = velocity;
				other.gameObject.GetComponent<SimpleSpringBoard>()?.SpringBoardWasActivated();
			}
		}
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		UnifiedCollisionMethodCheck(other.gameObject);
	}

	private void UnifiedCollisionMethodCheck(GameObject touchedOBJ)
	{
		bool flag = false;
		if (Originator == OwnerOfProjectile.Enemy && touchedOBJ.tag == "Player" && gameObject.name.Contains("BOT1M"))
		{
			flag = true;
		}
		if (Originator == OwnerOfProjectile.Enemy && touchedOBJ.tag == "Player" && ContinueAfterSpecialChecks(touchedOBJ))
		{
			flag = true;
		}
		if (Originator == OwnerOfProjectile.Enemy && touchedOBJ.CompareTag("Lala-Atk-Hitbox") && GC.ExistsEffectOnPlayer(GameCore.TempPlayerEffect.FruitSlice))
		{
			ProjectileDisableBehaviour(spawnTimeOutEffect: false, specialFruitSliceEffect: true);
			return;
		}
		if (Originator == OwnerOfProjectile.Player && (touchedOBJ.tag == "Enemy-Basic" || touchedOBJ.tag == "Enemy-Boss" || touchedOBJ.tag == "ShieldObject"))
		{
			if (ContinueAfterSpecialChecks(touchedOBJ))
			{
				flag = true;
			}
			if (playerDmgType == GameCore.PlayerDamageType.ForwardSlash || playerDmgType == GameCore.PlayerDamageType.WeaponWhip)
			{
				GC.InstantiateVFX(VisualEffectOnHit, touchedOBJ.transform.position, "PROJECTILELOGIC:HasTouchedValidTargetSoEndProjectileLogic");
				if (allowedToPlayHitSound && PlayThisHitSound != LocalSoundType.Null)
				{
					PlayHitCollisionSFX("LOL_IAM_DUMB_LOL");
				}
			}
			if (playerDmgType == GameCore.PlayerDamageType.ForwardSlash)
			{
				intCountGeneric++;
				if (GC.listAccBehavior.Contains(GameCore.AccessoryBehavior.SKILLEnhancePowerSlash))
				{
					if (intCountGeneric >= 3)
					{
						ProjectileDisableBehaviour(spawnTimeOutEffect: false);
					}
				}
				else if (intCountGeneric >= 1)
				{
					ProjectileDisableBehaviour(spawnTimeOutEffect: false);
				}
			}
		}
		if (touchedOBJ.tag == "AreaEnterEffect")
		{
			allowedToPlayHitSound = false;
		}
		if (touchedOBJ.layer == LayerMask.NameToLayer("LayerPlatforms") || touchedOBJ.layer == LayerMask.NameToLayer("LayerPlatMoving"))
		{
			if (disableRotationOnColl)
			{
				ZAxisRotationSpeed = 0f;
			}
			if (ContinueAfterSpecialChecks(touchedOBJ, isSurface: true))
			{
				flag = true;
			}
			if (currentProjectileType == GameCore.ProjectileType.ThrowDagger)
			{
				allowedToPlayHitSound = false;
			}
		}
		if (flag)
		{
			HasTouchedValidTargetSoEndProjectileLogic(touchedOBJ.tag, touchedOBJ.name);
		}
		if (checkPlayerAttackThis && ValidateIfPlayerHasattackedThisProjectile(touchedOBJ) && specialTag == "WitchIllusion")
		{
			TriggerWitchIllusionsExplode();
		}
	}

	private bool ValidateIfPlayerHasattackedThisProjectile(GameObject potentialPlayerAttack)
	{
		switch (potentialPlayerAttack.gameObject.tag)
		{
		case "Lala-Atk-Hitbox":
			return true;
		case "Projectile-Basic":
		{
			ProjectileLogic component = potentialPlayerAttack.gameObject.GetComponent<ProjectileLogic>();
			if (component != null && component.Originator == OwnerOfProjectile.Player)
			{
				return true;
			}
			break;
		}
		case "PlayerSkillAttack":
			if (potentialPlayerAttack.gameObject.GetComponent<PlayerSkillAttack>() != null)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void ShootCollPerFrame(int frame)
	{
		if (changeCollPerAnim && box2D != null)
		{
			SetColliderDimensionsBox(listCollChange[frame].setActive, listCollChange[frame].setOffset, listCollChange[frame].setSize);
		}
	}

	private void SetColliderDimensionsBox(bool active, Vector2 offset, Vector2 size)
	{
		if (active && !gameObject.activeInHierarchy)
		{
			gameObject.SetActive(value: true);
		}
		box2D.offset = offset;
		box2D.size = size;
		box2D.enabled = active;
		if (!active && gameObject.activeInHierarchy)
		{
			gameObject.SetActive(value: false);
		}
	}

	private bool ContinueAfterSpecialChecks(GameObject objOther, bool isSurface = false)
	{
		if (IgnoreFloorCollision)
		{
			return false;
		}
		if (currentProjectileType == GameCore.ProjectileType.SpecialForestBossSeed)
		{
			if (isSurface)
			{
				Special_ForestBossSpawnSeed();
				return true;
			}
			return false;
		}
		if (currentProjectileType == GameCore.ProjectileType.StraightLine && objOther.tag == "Hazard-Basic" && objOther.name.Contains("Vines"))
		{
			return false;
		}
		if (PlayThisSpawnSound == LocalSoundType.ExplosionRune)
		{
			return false;
		}
		if (currentProjectileType == GameCore.ProjectileType.SpecialBeachBossWaves)
		{
			return false;
		}
		if (Originator == OwnerOfProjectile.Player && objOther.name.Contains("Wolf-Girl-Mimic"))
		{
			Animator component = objOther.GetComponent<Animator>();
			if (component != null)
			{
				int integer = component.GetInteger("genericPhase");
				if (integer == 0 || integer == 1)
				{
					return false;
				}
			}
		}
		return true;
	}

	public void ProjectileWasReflectedByPlayer(GameObject trailOBJ)
	{
		StopAllCoroutines();
		CancelInvoke();
		Originator = OwnerOfProjectile.Player;
		playerDmgType = GameCore.PlayerDamageType.ReflectProjectile;
		currentProjectileType = GameCore.ProjectileType.TargetPosition;
		objRB.bodyType = RigidbodyType2D.Kinematic;
		originPosition = transform.position;
		moveSpeed = 10f;
		countFrequency = 0f;
		touchDamage = 0;
		ogTouchDamage = 0;
		timerDuration = 5f;
		isFacePlayerConstant = false;
		IgnoreFloorCollision = true;
		objTrailVFX = trailOBJ;
		objRE.color = new Color(1f, 0.5f, 0.5f);
		GC.InstantiateVFX(GameCore.VisualEffect.SlashSkillHitPurple, transform.position, "ProjectileLogic");
		InvokeRepeating("Invoked_SpawnTrailEffect", 0.15f, 0.15f);
		ReflectFindClosestEnemyTarget(resetProjectile: true, baseReferenceSelf: false, ignoreFacing: false, safeNetZero: true);
	}

	private void ReflectFindClosestEnemyTarget(bool resetProjectile, bool baseReferenceSelf, bool ignoreFacing, bool safeNetZero)
	{
		Vector2 basePosition = new Vector2(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition);
		int playerFacing = GC.currentPlayerFacing;
		if (baseReferenceSelf)
		{
			basePosition = transform.position;
		}
		if (ignoreFacing)
		{
			playerFacing = 0;
		}
		Vector2 vector = GC.MATH_GetPositionOfNearestEnemy(basePosition, 20f, 3, playerFacing, 50);
		if ((vector == Vector2.zero) & safeNetZero)
		{
			vector.x = GC.currentPlayerFacing;
			vector.y = originPosition.y;
		}
		targetPosition = vector;
		if (resetProjectile)
		{
			InitializePlayerProjectileBehaviour(targetPosition);
		}
	}

	private void OrbitAroundIncrementalValues()
	{
		if (coreOrbit.incrementalRadiusBase > 0f)
		{
			coreOrbit.radiusBase += coreOrbit.incrementalRadiusBase;
		}
		if (coreOrbit.incrementalRadiusSpeed > 0f)
		{
			coreOrbit.radiusSpeed += coreOrbit.incrementalRadiusSpeed;
		}
		if (coreOrbit.incrementalRotationSpeed > 0f)
		{
			coreOrbit.rotationSpeed += coreOrbit.incrementalRotationSpeed;
		}
	}

	private void Special_ForestBossSpore()
	{
		float num = 0f;
		num = ((GC.MATH_RNG_INT(0, 1) != 0) ? GC.MATH_RNG_FLOAT(0.5f, 3f) : GC.MATH_RNG_FLOAT(-3f, -0.5f));
		targetPosition = originPosition;
		targetPosition.x += num;
		targetPosition.y += 10f;
		moveHeading = targetPosition - originPosition;
		moveDistance = moveHeading.magnitude;
		moveDirection = moveHeading / moveDistance;
	}

	private void Special_ForestBossSeed()
	{
		float num = 0f;
		if (specialTag == "right")
		{
			num = GC.MATH_RNG_FLOAT(6f, 8.5f);
		}
		else if (specialTag == "left")
		{
			num = GC.MATH_RNG_FLOAT(-8.5f, -6f);
		}
		targetPosition = transform.position;
		targetPosition.x += num;
		targetPosition.y = 1.5f;
		float num2 = targetPosition.x - transform.position.x;
		float f = Mathf.Atan((targetPosition.y - transform.position.y + 4.905f) / num2);
		float num3 = num2 / Mathf.Cos(f);
		float x = num3 * Mathf.Cos(f);
		float y = num3 * Mathf.Sin(f);
		objRB.velocity = new Vector2(x, y);
	}

	private void Special_ForestBossSpawnSeed()
	{
		if ((bool)specialObj1 && (bool)specialObj2 && (bool)GC.refOBJEffectsParent && (bool)GC.refOBJEnemiesParent)
		{
			Vector3 position = transform.position;
			position.y += 1.25f;
			position.x = GC.MATH_RoundFloatDecimals(position.x, 2);
			position.y = GC.MATH_RoundFloatDecimals(position.y, 2);
			Object.Instantiate(specialObj1, position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
			position.y--;
			Object.Instantiate(specialObj2, position, Quaternion.identity, GC.refOBJEffectsParent.transform);
		}
	}

	private void Special_BeachBossWaves()
	{
		if (GC.playerCurrentWorldXPosition >= transform.position.x)
		{
			objRE.flipX = false;
			moveDirection.x = 1f;
		}
		else
		{
			objRE.flipX = true;
			moveDirection.x = -1f;
		}
		moveDirection.y = 0.5f;
		InvokeRepeating("Special_EndBeachBossWaves", specialForce.x, 10f);
	}

	private void Special_EndBeachBossWaves()
	{
		moveDirection.y = -0.6f;
		objRB.velocity = moveDirection * moveSpeed;
		CancelInvoke("Special_EndBeachBossWaves");
	}

	private void Special_InvokedChase()
	{
		if (GC.playerCurrentWorldXPosition + playerXOffset < transform.position.x)
		{
			if (moveDirection.x > 0f - spHorMoveLimit)
			{
				moveDirection.x -= spHorIncrease;
			}
			Special_ChaseSlowVerticalCalc(spHorIncrease, spHorMoveLimit);
		}
		else
		{
			if (moveDirection.x < spHorMoveLimit)
			{
				moveDirection.x += spHorIncrease;
			}
			Special_ChaseSlowVerticalCalc(spHorIncrease, spHorMoveLimit);
		}
		objRB.velocity = moveDirection * moveSpeed;
	}

	private void Special_ChaseSlowVerticalCalc(float vInc, float hMoveLimit)
	{
		if (GC.playerCurrentWorldYPosition + playerYOffset < transform.position.y)
		{
			if (moveDirection.y > 0f - hMoveLimit)
			{
				moveDirection.y -= vInc;
			}
		}
		else if (moveDirection.y < hMoveLimit)
		{
			moveDirection.y += vInc;
		}
	}

	private void Special_ChaseSlowLamiaVersion()
	{
		if (GC.playerCurrentWorldXPosition < transform.position.x)
		{
			if (moveDirection.x > 0f - spHorMoveLimit)
			{
				moveDirection.x -= spHorIncrease;
			}
		}
		else if (moveDirection.x < spHorMoveLimit)
		{
			moveDirection.x += spHorIncrease;
		}
		objRB.velocity = moveDirection * moveSpeed;
	}

	private void IVK_PlayerHomingSphere()
	{
		moveDirection = (targetPosition - new Vector2(transform.position.x, transform.position.y)).normalized;
		objRB.velocity = moveDirection * moveSpeed;
	}

	private void IVK_AcquireClosestTarget()
	{
		ReflectFindClosestEnemyTarget(resetProjectile: false, baseReferenceSelf: true, ignoreFacing: true, safeNetZero: false);
		if (targetPosition == Vector2.zero)
		{
			targetPosition.x = transform.position.x + (float)GC.currentPlayerFacing;
			targetPosition.y = transform.position.y;
		}
	}

	private void Invoked_WitchCloud()
	{
		intCountGeneric++;
		Vector2 vector = transform.position;
		vector.y -= 1.25f;
		Object.Instantiate(specialObj1, vector, Quaternion.identity, transform.parent);
		if (intCountGeneric >= 4)
		{
			StopAllCoroutines();
			CancelInvoke();
			gameObject.SetActive(value: false);
		}
	}

	private void Invoked_WitchSpawnCircle()
	{
		intCountGeneric++;
		Object.Instantiate(specialObj1, transform.position, Quaternion.identity, transform.parent);
		if (intCountGeneric >= 20)
		{
			StopAllCoroutines();
			CancelInvoke();
			gameObject.SetActive(value: false);
		}
	}

	private void TriggerWitchIllusionsExplode()
	{
		if (intCountGeneric == 0)
		{
			intCountGeneric = 1;
			Vector2 zero = Vector2.zero;
			for (int i = 0; i < 3; i++)
			{
				zero = transform.position;
				zero.x += GC.MATH_RNG_FLOAT(-0.75f, 0.75f);
				zero.y += GC.MATH_RNG_FLOAT(-0.75f, 0.75f);
				Object.Instantiate(specialObj1, zero, Quaternion.identity, GC.refOBJEffectsParent.transform);
			}
			Object.Instantiate(specialObj2, transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
			ProjectileDisableBehaviour(spawnTimeOutEffect: false);
		}
	}

	public void ProjectileCountLoopAnim()
	{
		if (!CheckAnimatorReference() || arrayAnimPhase == null || arrayAnimPhase.Length == 0)
		{
			Debug.Log("Tried to play projectile anim phase control, but the phases weren't set for " + gameObject.name);
			return;
		}
		animLocalPhase = objAN.GetInteger("animPhase");
		animCount++;
		if (animCount > arrayAnimPhase[animLocalPhase].runLoops)
		{
			objAN.SetInteger("animPhase", animLocalPhase + 1);
			animCount = 0;
		}
	}

	private bool CheckAnimatorReference()
	{
		if (!objAN)
		{
			objAN = GetComponent<Animator>();
		}
		if ((bool)objAN)
		{
			return true;
		}
		return false;
	}

	private void Invoked_SpawnTrailEffect()
	{
		if ((bool)GC.refOBJEffectsParent && objTrailVFX != null)
		{
			Object.Instantiate(objTrailVFX, transform.position, Quaternion.identity, GC.refOBJEffectsParent.transform);
		}
	}

	public void DisableOnAnimationEnd()
	{
		ProjectileDisableBehaviour(spawnTimeOutEffect: false);
	}

	private void HasTouchedValidTargetSoEndProjectileLogic(string collidedTag, string collidedName)
	{
		GC.InstantiateVFX(VisualEffectOnHit, transform.position, "PROJECTILELOGIC:HasTouchedValidTargetSoEndProjectileLogic");
		if (allowedToPlayHitSound && PlayThisHitSound != LocalSoundType.Null)
		{
			PlayHitCollisionSFX(collidedTag);
		}
		ProjectileDisableBehaviour(spawnTimeOutEffect: false);
	}

	private void ProjectileDisableBehaviour(bool spawnTimeOutEffect, bool specialFruitSliceEffect = false)
	{
		if (spawnTimeOutEffect && VisualEffectOnTimeOut != GameCore.VisualEffect.Null)
		{
			GC.InstantiateVFX(VisualEffectOnTimeOut, transform.position, "PROJECTILELOGIC:ProjectileDisableBehaviour");
		}
		if (specialFruitSliceEffect)
		{
			GC.InstantiateVFX(GameCore.VisualEffect.DashATKSlash, transform.position, "PROJECTILELOGIC:ProjectileDisableBehaviour");
		}
		if (spawnSpecialOnDisable)
		{
			SpawnSpecialObjectsOnDisable();
		}
		if (OnDisableSendToCleanupParent)
		{
			transform.parent = GC.refOBJEffectsParent.transform;
		}
		if (PlayThisDisableSound != LocalSoundType.Null)
		{
			DoPlayThisDisableSound();
		}
		StopAllCoroutines();
		CancelInvoke();
		gameObject.SetActive(value: false);
	}

	private void SpawnSpecialObjectsOnDisable()
	{
		if (SpecialSpawnCornerCase())
		{
			return;
		}
		if (specialObj1 != null)
		{
			Object.Instantiate(specialObj1, transform.position, Quaternion.identity, transform.parent);
		}
		if (specialObj2 != null)
		{
			Object.Instantiate(specialObj2, transform.position, Quaternion.identity, transform.parent);
		}
		if (specialProjectileInherit != null && objOriginator != null)
		{
			ProjectileLogic component = Object.Instantiate(specialProjectileInherit, transform.position, Quaternion.identity, transform.parent).GetComponent<ProjectileLogic>();
			if (component != null)
			{
				component.objOriginator = objOriginator;
			}
		}
	}

	private bool SpecialSpawnCornerCase()
	{
		if (gameObject.name.Contains("ARC-Seedling"))
		{
			Special_ForestBossSpawnSeed();
			return true;
		}
		if (currentProjectileType == GameCore.ProjectileType.SpecialPrincessBoss && specialTag == "spawn" && arrayGenericObjects != null && arrayGenericObjects.Length != 0)
		{
			Vector3 position = transform.position;
			position.y -= 0.25f;
			for (int i = 0; i < arrayGenericObjects.Length; i++)
			{
				ProjectileLogic component = Object.Instantiate(arrayGenericObjects[i], position, Quaternion.identity, transform.parent).GetComponent<ProjectileLogic>();
				switch (i)
				{
				case 0:
					component.specialForce.x = transform.position.x - 1f;
					component.specialForce.y = transform.position.y - 1f;
					break;
				case 1:
					component.specialForce.x = transform.position.x;
					component.specialForce.y = transform.position.y - 1f;
					break;
				case 2:
					component.specialForce.x = transform.position.x + 1f;
					component.specialForce.y = transform.position.y - 1f;
					break;
				}
			}
			Object.Instantiate(specialObj2, transform.position, Quaternion.identity, transform.parent);
			return true;
		}
		if (gameObject.name.Contains("PSlime"))
		{
			bool flag = Physics2D.Linecast(transform.position, transform.position, 1 << LayerMask.NameToLayer("LayerPlatforms"));
			if (GC.MATH_RNG_INT(1, 100) <= 40 && GC.quantityOfSpawnedPurpleSlimes < 5 && !flag)
			{
				Object.Instantiate(specialObj1, transform.position, Quaternion.identity, GC.refOBJEnemiesParent.transform);
				GC.quantityOfSpawnedPurpleSlimes++;
				if (GC.quantityOfSpawnedPurpleSlimes > 5)
				{
					GC.quantityOfSpawnedPurpleSlimes = 5;
				}
				GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: true, transform.position, 1.5f, 1.7f);
			}
			return true;
		}
		if (specialTag == "WitchIllusion")
		{
			TriggerWitchIllusionsExplode();
			return true;
		}
		return false;
	}

	private void DoPlayThisDisableSound()
	{
		switch (PlayThisDisableSound)
		{
		case LocalSoundType.MothBurst:
			GC.PlaySFX(GameCore.SFXBasic2.BossDeathExplode, 0, checkForDuplicity: true, transform.position, 0f, 0f);
			break;
		case LocalSoundType.IceSmall:
			GC.PlaySFXGeneric(GameCore.SpeakerID.PrincessBoss.ToString(), 11, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f, "sfx", "IceSmall");
			break;
		case LocalSoundType.IceBreak:
			GC.PlaySFXGeneric(GameCore.SpeakerID.PrincessBoss.ToString(), 12, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f, "sfx", "IceBreak");
			break;
		case LocalSoundType.Weird:
			GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: true, transform.position, 0.8f, 1.2f);
			break;
		case LocalSoundType.Boing3:
			GC.PlaySFX(GameCore.SFXBasic2.Boing3, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.LamiaCast:
			GC.PlaySFX(GameCore.SFXBasic2.LamiaSkillCast, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.MothCast:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MothSkillCast, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.Spawn:
			GC.PlaySFX(GameCore.SFXBasic1.SpawnerNoise1, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		}
	}

	private bool CheckDelayedShotStatus(float timeToDelayAndWait)
	{
		if (!doShotAfterDelay)
		{
			StartDelayedShot(timeToDelayAndWait);
			return false;
		}
		return true;
	}

	private void StartDelayedShot(float timeToDelay)
	{
		if (!doShotAfterDelay)
		{
			if (crtDelayedShot != null)
			{
				StopCoroutine(crtDelayedShot);
			}
			crtDelayedShot = StartCoroutine(waitDelayedShot(timeToDelay));
		}
	}

	private IEnumerator waitDelayedShot(float wait)
	{
		yield return new WaitForSeconds(wait);
		doShotAfterDelay = true;
		crtDelayedShot = null;
		InitializeEnemyProjectileBehaviour();
	}

	private IEnumerator waitToDoGenericStuff(float wait, string action)
	{
		yield return new WaitForSeconds(wait);
		if (!(action == "BLF-SideShockB"))
		{
			if (action == "shockWalls")
			{
				Vector2 vector = transform.position;
				vector.y += 2.5f;
				Object.Instantiate(specialObj1, vector, Quaternion.identity, transform.parent);
				Object.Instantiate(specialObj2, vector, Quaternion.identity, transform.parent);
			}
		}
		else
		{
			IgnoreFloorCollision = false;
		}
	}

	private void FlipThisSpriteToFaceThePlayerPosition(bool pointInTheZAxis)
	{
		if (pointInTheZAxis)
		{
			Vector3 vector = new Vector3(GC.playerCurrentWorldXPosition, GC.playerCurrentWorldYPosition, 0f);
			string text = specialTag;
			if (!(text == "Offset"))
			{
				if (text == "DirectPoint")
				{
					vector = specialForce;
				}
			}
			else
			{
				vector.x += specialForce.x;
				vector.y += specialForce.y;
			}
			Vector3 vector2 = vector - transform.position;
			float z = Mathf.Atan2(vector2.y, vector2.x) * 57.29578f;
			transform.rotation = Quaternion.Euler(0f, 0f, z);
		}
		else if (GC.playerCurrentWorldXPosition < transform.position.x && !objRE.flipX)
		{
			objRE.flipX = true;
		}
		else if (GC.playerCurrentWorldXPosition >= transform.position.x && objRE.flipX)
		{
			objRE.flipX = false;
		}
	}

	private void PlayInitialShootSFX()
	{
		switch (PlayThisSpawnSound)
		{
		case LocalSoundType.Basic1:
			GC.PlaySFX(GameCore.SFXBasic1.LaserShoot1, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.Basic2:
			GC.PlaySFX(GameCore.SFXBasic1.SwingTK3, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.Kitsune1:
			GC.PlaySFX(GameCore.SFXBasic1.Falling1, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.ThrowDagger:
			GC.PlaySFX(GameCore.SFXBasic2.Throw1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case LocalSoundType.ThrowDaggerHigh:
			GC.PlaySFX(GameCore.SFXBasic2.Throw1, 0, checkForDuplicity: true, transform.position, 1.9f, 2.1f);
			break;
		case LocalSoundType.SmallRuneBomb:
			GC.PlaySFX(GameCore.SFXBasic2.Throw3, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case LocalSoundType.ExplosionRune:
			GC.PlaySFX(GameCore.SFXBasic2.Explosion1, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case LocalSoundType.LamiaCast:
			GC.PlaySFX(GameCore.SFXBasic2.LamiaSkillCast, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.LamiaSnakeAppear:
			GC.PlaySFX(GameCore.SFXBasic2.LamiaSkillAppear, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.WWExplode:
			GC.PlaySFXGeneric(GameCore.SpeakerID.WerewolfBoss.ToString(), 13, 0, checkForDuplicity: true, transform.position, 0f, 0f, "sfx", "WWExplode");
			break;
		case LocalSoundType.RavenCaw:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.SwampFairy_Caw, 0, checkForDuplicity: true, transform.position, 0.85f, 1.15f, "cawLOL");
			break;
		case LocalSoundType.TreeNut:
			GC.PlaySFX(GameCore.SFXBasic2.Wooden2, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.MothSpore:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MothSkillExplode, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.MothCast:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MothSkillCast, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.DwarfPickaxe:
			GC.PlaySFX(GameCore.SFXBasic2.Swing4, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.CrystalLaser:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.Crystal_Shoot, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.IceBig:
			GC.PlaySFXGeneric(GameCore.SpeakerID.PrincessBoss.ToString(), 10, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f, "sfx", "IceBig");
			break;
		case LocalSoundType.IceSmall:
			GC.PlaySFXGeneric(GameCore.SpeakerID.PrincessBoss.ToString(), 11, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f, "sfx", "IceSmall");
			break;
		case LocalSoundType.IceGen1:
			GC.PlaySFXGeneric(GameCore.SpeakerID.PrincessBoss.ToString(), 13, 0, checkForDuplicity: true, transform.position, 0.85f, 1.15f, "sfx", "IceSmall");
			break;
		case LocalSoundType.IceGen2:
			GC.PlaySFXGeneric(GameCore.SpeakerID.PrincessBoss.ToString(), 14, 0, checkForDuplicity: true, transform.position, 0.85f, 1.15f, "sfx", "IceSmall");
			break;
		case LocalSoundType.Boing3:
			GC.PlaySFX(GameCore.SFXBasic2.Boing3, 0, checkForDuplicity: true, transform.position, 1.25f, 1.45f);
			break;
		case LocalSoundType.Tornado:
			GC.PlaySFX(GameCore.SFXBasic1.Tornado, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.TornadoHighPitch:
			GC.PlaySFX(GameCore.SFXBasic1.Tornado, 0, checkForDuplicity: true, transform.position, 1.5f, 1.6f);
			break;
		case LocalSoundType.Cloud:
			GC.PlaySFX(GameCore.SFXBasic1.Cloud, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.Thunder:
			GC.PlaySFX(GameCore.SFXBasic1.Thunder, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.Weird:
			GC.PlaySFX(GameCore.SFXBasic2.Weird1, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.LowShoot:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.RobotSkill1, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.WyvernFleshCord:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.WyvernSkill, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.CircularSaw:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.RobotSkill2, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.LaserBig:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.RobotSkill3, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.Pound:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FLR_Pound, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.MindBlast1:
			GC.PlaySFXGeneric("BlueFoxBoss", 12, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f, "sfx");
			break;
		case LocalSoundType.MindBlast2:
			GC.PlaySFXGeneric("BlueFoxBoss", 13, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f, "sfx");
			break;
		case LocalSoundType.ElectricMine:
			GC.PlaySFXGeneric("BlueFoxBoss", 14, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f, "sfx");
			break;
		case LocalSoundType.RockCrash:
		case LocalSoundType.Chirp1:
		case LocalSoundType.PowerSlash:
		case LocalSoundType.MothBurst:
		case LocalSoundType.IceBreak:
			break;
		}
	}

	private void PlayHitCollisionSFX(string collTag)
	{
		switch (PlayThisHitSound)
		{
		case LocalSoundType.Basic1:
			GC.PlaySFX(GameCore.SFXBasic1.HitHurtTouch1, 0, checkForDuplicity: false, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.Kitsune1:
			GC.PlaySFX(GameCore.SFXBasic1.Coconut1, 0, checkForDuplicity: false, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.ThrowDagger:
			GC.PlaySFX(GameCore.SFXBasic2.Hit1, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.RockCrash:
			GC.PlaySFX(GameCore.SFXBasic2.RockSmash, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
			break;
		case LocalSoundType.Chirp1:
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.MushroomGirlSkillB, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f);
			break;
		case LocalSoundType.PowerSlash:
			GC.PlaySFX(GameCore.SFXBasic2.SkillSlashHit, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f, "PowerSlashHit");
			break;
		case LocalSoundType.IceBreak:
			GC.PlaySFXGeneric(GameCore.SpeakerID.PrincessBoss.ToString(), 12, 0, checkForDuplicity: true, transform.position, 0.9f, 1.1f, "sfx", "IceBreak");
			break;
		}
		if (Originator == OwnerOfProjectile.Enemy && collTag == "Player")
		{
			PlayPlayerTouchSFX();
		}
	}

	private void PlayPlayerTouchSFX()
	{
		GC.PlaySFX(GameCore.SFXBasic1.HitHurtTouch3, 0, checkForDuplicity: false, Vector2.zero, 0.9f, 1.1f);
	}

	public void PlayProjectileDirectSFX(string sfxName)
	{
		switch (sfxName)
		{
		case "mothPupaBoing":
			GC.PlaySFX(GameCore.SFXBasic2.BoingRandom, 0, checkForDuplicity: true, transform.position, 0.85f, 1.15f, "PupaBoing");
			break;
		case "beeEggBoing":
			GC.PlaySFX(GameCore.SFXBasic2.BoingRandom2, 0, checkForDuplicity: true, transform.position, 0.6f, 0.8f, "eggBoing");
			break;
		case "Odd4":
			GC.PlaySFX(GameCore.SFXBasic2.Odd4, 0, checkForDuplicity: true, transform.position, 0f, 0f);
			break;
		case "Odd8":
			GC.PlaySFX(GameCore.SFXBasic2.Odd8, 0, checkForDuplicity: true, transform.position, 0f, 0f);
			break;
		}
	}

	public void ToggleProjectileCollider(string setState)
	{
		if (setState == "enable")
		{
			coll2D.enabled = true;
		}
		else
		{
			coll2D.enabled = false;
		}
	}

	public void ChangeProjectileSortingOrderByAnim(int newLayer)
	{
		objRE.sortingOrder = newLayer;
	}

	private void ProjectileTimer()
	{
		countFrequency += timerFrequency;
		if (countFrequency >= timerDuration)
		{
			ProjectileDisableBehaviour(spawnTimeOutEffect: true);
		}
		if (ZAxisRotationSpeed != 0f && disableRotationOnColl && countFrequency > 5f)
		{
			ZAxisRotationSpeed = 0f;
		}
	}

	private void ToggleShaderEfectOn()
	{
		Swap2DShader component = GetComponent<Swap2DShader>();
		if (component != null)
		{
			component.Toggle2DShader(stateON: true, 0);
		}
		else
		{
			Debug.Log("This projectile had the BOOL for 'toggle shader effect' activaded, but the reference to the shader script/component was NULL!");
		}
	}
}
