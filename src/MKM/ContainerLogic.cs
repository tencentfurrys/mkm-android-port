using System.Collections;
using UnityEngine;

public class ContainerLogic : MonoBehaviour
{
	public GameCore.ContainerType currentContainerType;

	public int hitsToDestroy;

	public GameObject objIconDisplay;

	public float delayItemSpawn;

	public Vector2 spawnOffset;

	public bool spawnPositionRandomly;

	public ContainerSpawnItems[] arraySpawnItems;

	[HideInInspector]
	public bool checkCrateDisabledList = true;

	private bool _isThisChestOpen;

	private int currentHitsToDestroy;

	private Vector2 posOrigin;

	private Vector3 posHitCollisionEffect;

	private GameObject mainCollectibleParentObj;

	private Collider2D selfCollider2D;

	private Rigidbody2D objRB;

	private Animator objAN;

	private GameCore GC;

	[HideInInspector]
	public bool isThisChestOpen => _isThisChestOpen;

	private void Start()
	{
		InitialSettings();
		CheckForUniquePickupState();
	}

	private void InitialSettings()
	{
		GC = GameCore.Instance;
		objRB = GetComponent<Rigidbody2D>();
		objAN = GetComponent<Animator>();
		selfCollider2D = GetComponent<Collider2D>();
		_isThisChestOpen = false;
		currentHitsToDestroy = hitsToDestroy;
		posOrigin = transform.position;
	}

	private void CheckForUniquePickupState()
	{
		GameCore.ContainerType containerType = currentContainerType;
		if (containerType == GameCore.ContainerType.ChestBigGold)
		{
			int itemID = arraySpawnItems[0].spawnObject.GetComponent<CollectibleLogic>().itemID;
			if (GC.CheckPlayerHasItem(itemID))
			{
				CHEST_OpenLogic(stateChestColl: false, setAnimatorChestOpen: true, doItemSpawnLogic: false);
			}
		}
	}

	private void Update()
	{
		if (isThisChestOpen || currentContainerType != GameCore.ContainerType.ChestBigGold)
		{
			return;
		}
		if (GC.playerIsHurt || GC.isGamePaused || GC.playerIsBeingGrabed || GC.isBossFight || GC.isBossRematch || GC.isPlayerFainted || GC.eventIsPlaying)
		{
			if (selfCollider2D.enabled)
			{
				selfCollider2D.enabled = false;
			}
		}
		else if (!selfCollider2D.enabled)
		{
			selfCollider2D.enabled = true;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.gameObject.tag)
		{
		case "Lala-Atk-Hitbox":
			posHitCollisionEffect = other.bounds.ClosestPoint(transform.position);
			ThisContainerWasHitByPlayer(1, other.gameObject.tag, other.gameObject.name);
			break;
		case "Projectile-Basic":
		{
			ProjectileLogic component2 = other.gameObject.GetComponent<ProjectileLogic>();
			if (component2.Originator == ProjectileLogic.OwnerOfProjectile.Player)
			{
				posHitCollisionEffect = other.bounds.ClosestPoint(transform.position);
				Vector2 zero = Vector2.zero;
				int dmgInstances = 1;
				if (other.name.Contains("Explosion"))
				{
					dmgInstances = 100;
				}
				if (component2.playerDmgType == GameCore.PlayerDamageType.ForwardSlash)
				{
					dmgInstances = 1000;
					posHitCollisionEffect = component2.transform.position;
				}
				if (component2.playerDmgType == GameCore.PlayerDamageType.GroundPound)
				{
					dmgInstances = 1000;
				}
				if (component2.playerDmgType == GameCore.PlayerDamageType.entropicExplosion)
				{
					dmgInstances = 10000;
				}
				ThisContainerWasHitByPlayer(dmgInstances, other.gameObject.tag, other.gameObject.name);
				zero = posHitCollisionEffect;
				zero.x += GC.MATH_RNG_FLOAT(-0.5f, 0.5f);
				zero.y += GC.MATH_RNG_FLOAT(-0.5f, 0.5f);
				GC.InstantiateVFX(component2.VisualEffectOnHit, zero, "ContainerLogic");
			}
			break;
		}
		case "Player":
			if (objIconDisplay != null && !objIconDisplay.activeInHierarchy && !GC.isPlayerFainted)
			{
				objIconDisplay.SetActive(value: true);
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
		string text = other.gameObject.tag;
		if (!(text == "SpringBoard"))
		{
			return;
		}
		Rigidbody2D rigidbody2D = objRB;
		if ((object)rigidbody2D != null && rigidbody2D.velocity.y <= 0.5f)
		{
			SimpleSpringBoard component = other.gameObject.GetComponent<SimpleSpringBoard>();
			if ((object)component != null && component.canPushUp)
			{
				objRB.velocity = new Vector2(objRB.velocity.x, 35f);
				other.gameObject.GetComponent<SimpleSpringBoard>()?.SpringBoardWasActivated();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player" && objIconDisplay != null && objIconDisplay.activeInHierarchy)
		{
			objIconDisplay.SetActive(value: false);
		}
	}

	private void ThisContainerWasHitByPlayer(int dmgInstances, string oTag, string oName)
	{
		bool flag = false;
		switch (currentContainerType)
		{
		case GameCore.ContainerType.Crate:
			CRATE_OpenLogic(dmgInstances);
			flag = true;
			break;
		case GameCore.ContainerType.ChestSmall:
			CHEST_OpenLogic(stateChestColl: false, setAnimatorChestOpen: true, doItemSpawnLogic: true);
			flag = true;
			break;
		}
		if (flag)
		{
			GC.CheckHit_VFX_SFX(oTag, oName, gameObject.tag, gameObject.name, posHitCollisionEffect);
		}
	}

	private void HitBySpecialSkillLogic(GameCore.PlayerSkillAtk thisSkill, Collider2D skillColl)
	{
		if (thisSkill != GameCore.PlayerSkillAtk.SlidingTackle && thisSkill == GameCore.PlayerSkillAtk.FlameSword)
		{
			ThisContainerWasHitByPlayer(3, "FlameSword", "FlameSword");
		}
	}

	private void StartDestroyedLogicBehaviour()
	{
		objRB.isKinematic = true;
		selfCollider2D.enabled = false;
		objAN.SetBool("isDestroy", value: true);
		BeginSpawnMainLogic();
		if (currentContainerType == GameCore.ContainerType.Crate && checkCrateDisabledList)
		{
			GC.CRATE_INSERT_DISABLED_LIST(GC.strCurrentActiveSceneName, gameObject.name, posOrigin);
		}
	}

	private void FinishDestroyedLogic()
	{
		gameObject.SetActive(value: false);
	}

	public void CHEST_OpenLogic(bool stateChestColl, bool setAnimatorChestOpen, bool doItemSpawnLogic)
	{
		selfCollider2D.enabled = stateChestColl;
		_isThisChestOpen = true;
		objAN.SetBool("isChestOpen", setAnimatorChestOpen);
		if (doItemSpawnLogic)
		{
			if (currentContainerType == GameCore.ContainerType.ChestBigGold)
			{
				GC.PlaySFX(GameCore.SFXBasic2.ChestOpen1, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			}
			BeginSpawnMainLogic();
		}
	}

	private void CRATE_OpenLogic(int dmgInstances)
	{
		currentHitsToDestroy -= dmgInstances;
		if (currentHitsToDestroy <= 0)
		{
			GC.PlaySFX(GameCore.SFXBasic1.BarrelDestroyed1, 0, checkForDuplicity: true, transform.position, 0.75f, 1.25f);
			StartDestroyedLogicBehaviour();
		}
		else
		{
			GC.PlaySFX(GameCore.SFXBasic1.BarrelHit1, 0, checkForDuplicity: true, transform.position, 0.8f, 1.2f);
		}
	}

	private void BeginSpawnMainLogic()
	{
		if ((currentContainerType == GameCore.ContainerType.Crate && checkCrateDisabledList && GC.CRATE_EXISTS_DISABLED_LIST(GC.strCurrentActiveSceneName, gameObject.name, posOrigin)) || arraySpawnItems == null || arraySpawnItems.Length == 0)
		{
			return;
		}
		for (int i = 0; i < arraySpawnItems.Length; i++)
		{
			if (arraySpawnItems[i].spawnObject != null && arraySpawnItems[i].spawnChance > 0f && arraySpawnItems[i].spawnAmount > 0)
			{
				if (delayItemSpawn > 0f)
				{
					StartCoroutine(waitDelayItemSpawn(delayItemSpawn, arraySpawnItems[i]));
				}
				else
				{
					ExecuteSpawn(arraySpawnItems[i].spawnObject, arraySpawnItems[i].spawnChance, arraySpawnItems[i].spawnAmount);
				}
			}
		}
	}

	private void ExecuteSpawn(GameObject spawnObject, float spawnChance, int spawnMaxAmount)
	{
		for (int i = 0; i < spawnMaxAmount; i++)
		{
			if ((float)GC.MATH_RNG_INT(1, 100) <= spawnChance && CheckParentCollectibleReference())
			{
				Vector2 vector = new Vector2(transform.position.x, transform.position.y) + spawnOffset;
				GameObject gameObject = Object.Instantiate(spawnObject, vector, Quaternion.identity, mainCollectibleParentObj.transform);
				if (spawnPositionRandomly)
				{
					gameObject.GetComponent<CollectibleLogic>().isMoveSpawnRandom = true;
				}
				gameObject = null;
			}
		}
	}

	private IEnumerator waitDelayItemSpawn(float wait, ContainerSpawnItems thisItem)
	{
		yield return new WaitForSeconds(wait);
		ExecuteSpawn(thisItem.spawnObject, thisItem.spawnChance, thisItem.spawnAmount);
	}

	private bool CheckParentCollectibleReference()
	{
		if (!mainCollectibleParentObj)
		{
			mainCollectibleParentObj = GameObject.Find("[COLLECTIBLE]");
		}
		if ((bool)mainCollectibleParentObj)
		{
			return true;
		}
		return false;
	}
}
