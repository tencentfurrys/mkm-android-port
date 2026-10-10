using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_Spawner : MonoBehaviour
{
	public enum MonsterSpawnerType
	{
		Beach = 0,
		Forest = 1,
		HillFairy = 2,
		HillFly = 3,
		GhostFuta = 4,
		SwampFairy = 5,
		CentGirl = 6,
		Crystal_Player = 7,
		Crystal_Mush = 8,
		Crystal_Bat = 9,
		Crystal_Lamia = 10,
		Crystal_Dwarf = 11,
		HighTechBot = 12,
		CaveGoblin = 13,
		Gauntlet = 14
	}

	[Tooltip("[ENUM] The type of this spawner. This is used only to set the correct sprite animation of this spawner to the current stage theme.")]
	public MonsterSpawnerType currentSpawnerType;

	[Tooltip("[LIST] A list of monster prefabs to be spawned. Add or remove different types of monsters, and the quantity of each type.")]
	public List<SpawnMonster> listMonstersSpawn = new List<SpawnMonster>();

	[Tooltip("[INT] The minimum and maximum amount in seconds of delay between spawns.")]
	public int minDelayBetweenSpawns;

	[Tooltip("[INT] The minimum and maximum amount in seconds of delay between spawns.")]
	public int maxDelayBetweenSpawns;

	[Tooltip("[FLOAT] How close the player has to be to the position of this Spawner, in order for it's logic to run")]
	public float rangePlayerProximityHor;

	[Tooltip("[FLOAT] How close the player has to be to the position of this Spawner, in order for it's logic to run")]
	public float rangePlayerProximityVer;

	[Tooltip("[BOOL] Inverts the logic of the spawner; now it only spawns enemies when the player is OUTSIDE the range of proximity.")]
	public bool isInvertProximityLogic;

	[Tooltip("[BOOL] Checks the player's proximity to the transforms of the child spawn points, instead of the parent spawner object position.")]
	public bool isCheckChildProximity;

	[Tooltip("[FLOAT] Represents fractions of a second. Dictates the frequency in which the main method/logic of spawn is repeated.")]
	public float freqOfMainLogicBehaviour;

	[Tooltip("[BOOL] Wheter or not the spawn process should select randomly from the list of monsters.")]
	public bool isRandomSpawnList;

	[Tooltip("[PREFAB] The prefab for the effects when the spawner is spawning a monster in a spot, and when the monster de facto appears on the screen.")]
	public GameObject objEffectCasting;

	[Tooltip("[PREFAB] The prefab for the effects when the spawner is spawning a monster in a spot, and when the monster de facto appears on the screen.")]
	public GameObject objEffectAppear;

	[Tooltip("[STRING] The name of the state/animation inside the ANIMATOR that should be played for this spawner when it is spawning a new enemy.")]
	public string animNameOfSpawning;

	public float delaySpawnCasting = 2f;

	public bool noSpawnWhilePlayerGrabbed;

	public bool isGauntletSpawner;

	public bool applyGauntletMiniRage;

	private Animator objAN;

	private GameObject[] arraySpawnPoint;

	private GameObject[] arraySpawnedMonsters;

	private GameObject specificOrderedSpawn;

	private Coroutine coroutWaitForSpawn;

	private float countMainTimer;

	private int amountTotalSpawns;

	private int rndTimeToSpawn;

	private string defaultIdleAnimName;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		objAN = GetComponent<Animator>();
		Initialization();
	}

	private void Initialization()
	{
		PlayCorrectIdleAnimation();
		specificOrderedSpawn = null;
		coroutWaitForSpawn = null;
		countMainTimer = 0f;
		amountTotalSpawns = 0;
		if (delaySpawnCasting <= 0f)
		{
			delaySpawnCasting = 2f;
		}
		if (ValidateVariables())
		{
			SetupDynamicArrays();
			InvokeRepeating("MainSpawnBehaviour", 0f, freqOfMainLogicBehaviour);
		}
	}

	private void PlayCorrectIdleAnimation()
	{
		defaultIdleAnimName = "";
		switch (currentSpawnerType)
		{
		case MonsterSpawnerType.Forest:
			defaultIdleAnimName = "SpawnerForest_Idle";
			break;
		case MonsterSpawnerType.Beach:
			defaultIdleAnimName = "SpawnerBeach_Idle";
			break;
		case MonsterSpawnerType.HillFairy:
			defaultIdleAnimName = "Hill_Fairy_Idle";
			break;
		case MonsterSpawnerType.HillFly:
			defaultIdleAnimName = "Hill_Fly_Idle";
			break;
		case MonsterSpawnerType.GhostFuta:
			defaultIdleAnimName = "Ghost Guta Idle";
			break;
		case MonsterSpawnerType.SwampFairy:
			defaultIdleAnimName = "SPW_SwampFairy_Idle";
			break;
		case MonsterSpawnerType.CentGirl:
			defaultIdleAnimName = "CentGirl_Idle";
			break;
		case MonsterSpawnerType.Crystal_Player:
			defaultIdleAnimName = "Crystal_Player_Idle";
			break;
		case MonsterSpawnerType.Crystal_Lamia:
			defaultIdleAnimName = "Crystal_Lamia_Idle";
			break;
		case MonsterSpawnerType.Crystal_Bat:
			defaultIdleAnimName = "Crystal_Bat_Idle";
			break;
		case MonsterSpawnerType.Crystal_Dwarf:
			defaultIdleAnimName = "Crystal_Dwarf_Idle";
			break;
		case MonsterSpawnerType.Crystal_Mush:
			defaultIdleAnimName = "Crystal_Mush_Idle";
			break;
		case MonsterSpawnerType.HighTechBot:
			defaultIdleAnimName = "HighTechBot_Idle";
			break;
		case MonsterSpawnerType.CaveGoblin:
			defaultIdleAnimName = "CaveGoblin";
			break;
		case MonsterSpawnerType.Gauntlet:
			defaultIdleAnimName = "Gauntlet_Idle";
			break;
		}
		if (defaultIdleAnimName != "" && (bool)objAN)
		{
			objAN.Play(defaultIdleAnimName);
		}
	}

	private bool ValidateVariables()
	{
		if (!GC || minDelayBetweenSpawns < 0 || maxDelayBetweenSpawns <= 0 || listMonstersSpawn.Count <= 0 || freqOfMainLogicBehaviour <= 0f || maxDelayBetweenSpawns < minDelayBetweenSpawns)
		{
			Debug.Log("Invalid parameters values.");
			return false;
		}
		foreach (SpawnMonster item in listMonstersSpawn)
		{
			amountTotalSpawns += item.spawnQuantity;
			if (!item.monsterPrefab || item.spawnQuantity <= 0)
			{
				Debug.Log("Prefab entry and/or quantity are invalid.");
				return false;
			}
		}
		if (base.transform.childCount <= 0)
		{
			Debug.Log("Spawner needs at least 1 child object, to act as a spawn point.");
			return false;
		}
		rndTimeToSpawn = GC.MATH_RNG_INT(minDelayBetweenSpawns, maxDelayBetweenSpawns);
		return true;
	}

	private void SetupDynamicArrays()
	{
		int num = 0;
		arraySpawnPoint = new GameObject[base.transform.childCount];
		arraySpawnedMonsters = new GameObject[amountTotalSpawns];
		foreach (Transform item in base.transform)
		{
			arraySpawnPoint[num] = item.gameObject;
			num++;
		}
	}

	private void MainSpawnBehaviour()
	{
		if (base.gameObject.activeInHierarchy && CheckPlayerProximity(base.transform.position, isParentObject: true) && CanSpawnMoreMonsters() && coroutWaitForSpawn == null && ValidateMiscConditions())
		{
			countMainTimer += freqOfMainLogicBehaviour;
			if (countMainTimer >= (float)rndTimeToSpawn)
			{
				PrepareAndExecuteSpawn();
				rndTimeToSpawn = GC.MATH_RNG_INT(minDelayBetweenSpawns, maxDelayBetweenSpawns);
				countMainTimer = 0f;
			}
		}
	}

	private bool ValidateMiscConditions()
	{
		if (noSpawnWhilePlayerGrabbed && GC.playerIsBeingGrabed)
		{
			return false;
		}
		return true;
	}

	private void PrepareAndExecuteSpawn()
	{
		if (coroutWaitForSpawn != null)
		{
			StopCoroutine("WaitForSpawn");
		}
		if (isRandomSpawnList)
		{
			coroutWaitForSpawn = StartCoroutine(WaitForSpawn(delaySpawnCasting, ReturnRandomMonsterToSpawn()));
		}
		else
		{
			coroutWaitForSpawn = StartCoroutine(WaitForSpawn(delaySpawnCasting, specificOrderedSpawn));
		}
	}

	private IEnumerator WaitForSpawn(float waittime, GameObject objToSpawn)
	{
		Vector2 posSpawnPoint = GetRandomSpawnPoint();
		if (isCheckChildProximity && !CheckPlayerProximity(posSpawnPoint, isParentObject: false))
		{
			specificOrderedSpawn = null;
			coroutWaitForSpawn = null;
			yield return false;
		}
		if ((bool)GC.refOBJEffectsParent && (bool)objEffectCasting)
		{
			Object.Instantiate(objEffectCasting, posSpawnPoint, Quaternion.identity, GC.refOBJEffectsParent.transform);
		}
		if (animNameOfSpawning != "")
		{
			objAN.Play(animNameOfSpawning);
		}
		yield return new WaitForSeconds(waittime);
		if ((bool)GC.refOBJEffectsParent && (bool)objEffectAppear)
		{
			Object.Instantiate(objEffectAppear, posSpawnPoint, Quaternion.identity, GC.refOBJEffectsParent.transform);
		}
		if (defaultIdleAnimName != "")
		{
			objAN.Play(defaultIdleAnimName);
		}
		for (int i = 0; i < arraySpawnedMonsters.Length; i++)
		{
			if (arraySpawnedMonsters[i] == null && (bool)GC.refOBJEnemiesParent && (bool)objToSpawn)
			{
				GC.PlaySFX(GameCore.SFXBasic1.SpawnerNoise1, 0, checkForDuplicity: false, base.transform.position, 0.8f, 1.2f);
				arraySpawnedMonsters[i] = Object.Instantiate(objToSpawn, posSpawnPoint, Quaternion.identity, GC.refOBJEnemiesParent.transform);
				if (isGauntletSpawner)
				{
					arraySpawnedMonsters[i].GetComponent<EnemyGeneralLogic>().isGauntletSpecial = true;
					arraySpawnedMonsters[i].GetComponent<EnemyGeneralLogic>().shouldApplyMiniRage = applyGauntletMiniRage;
				}
				break;
			}
		}
		specificOrderedSpawn = null;
		coroutWaitForSpawn = null;
	}

	private bool CheckPlayerProximity(Vector3 thisPos, bool isParentObject)
	{
		float num = GC.playerCurrentWorldXPosition - thisPos.x;
		float num2 = GC.playerCurrentWorldYPosition - thisPos.y;
		if (num < 0f)
		{
			num *= -1f;
		}
		if (num2 < 0f)
		{
			num2 *= -1f;
		}
		if (isParentObject && isCheckChildProximity)
		{
			return true;
		}
		if (!isInvertProximityLogic && num <= rangePlayerProximityHor && num2 <= rangePlayerProximityVer)
		{
			return true;
		}
		if (isInvertProximityLogic && num >= rangePlayerProximityHor && num2 >= rangePlayerProximityVer)
		{
			return true;
		}
		return false;
	}

	private bool CanSpawnMoreMonsters()
	{
		int num = 0;
		if (isRandomSpawnList)
		{
			for (int i = 0; i < arraySpawnedMonsters.Length; i++)
			{
				if (arraySpawnedMonsters[i] != null)
				{
					num++;
				}
			}
			if (num < amountTotalSpawns)
			{
				return true;
			}
		}
		else
		{
			int num2 = 0;
			List<SpawnMonster> list = new List<SpawnMonster>();
			foreach (SpawnMonster item in listMonstersSpawn)
			{
				for (int j = 0; j < arraySpawnedMonsters.Length; j++)
				{
					if (arraySpawnedMonsters[j] != null && arraySpawnedMonsters[j].name.Contains(item.monsterPrefab.name))
					{
						num++;
					}
				}
				if (num < item.spawnQuantity)
				{
					list.Add(new SpawnMonster
					{
						monsterPrefab = item.monsterPrefab,
						spawnQuantity = item.spawnQuantity
					});
				}
				num = 0;
			}
			if (list.Count > 0)
			{
				int num3 = 1;
				num2 = GC.MATH_RNG_INT(1, list.Count);
				foreach (SpawnMonster item2 in list)
				{
					if (num3 == num2)
					{
						specificOrderedSpawn = item2.monsterPrefab;
						return true;
					}
					num3++;
				}
			}
		}
		return false;
	}

	private GameObject ReturnRandomMonsterToSpawn()
	{
		int num = GC.MATH_RNG_INT(1, listMonstersSpawn.Count);
		int num2 = 1;
		foreach (SpawnMonster item in listMonstersSpawn)
		{
			if (num2 == num)
			{
				return item.monsterPrefab;
			}
			num2++;
		}
		return null;
	}

	private Vector2 GetRandomSpawnPoint()
	{
		int num = GC.MATH_RNG_INT(0, arraySpawnPoint.Length - 1);
		for (int i = 0; i < arraySpawnPoint.Length; i++)
		{
			if (i == num)
			{
				return arraySpawnPoint[i].transform.position;
			}
		}
		return base.transform.position;
	}
}
