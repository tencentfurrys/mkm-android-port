using System.Collections;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
	public GameCore.EnemyAttackType attackType;

	public int dmgAmount;

	public int ogDmgAmount;

	public AttackColliderPerFrame[] listCollChange;

	public GameObject SpecialOwnerReference;

	public bool detectPlayerCollision;

	public bool disableOnlyAtAnimationEnd;

	public float selfDeactivateTimer;

	private Coroutine crtSelfDeactivate;

	private BoxCollider2D box2D;

	private CircleCollider2D circle2D;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		if ((bool)GC && GC.isBossRematch)
		{
			dmgAmount *= 2;
		}
		ogDmgAmount = dmgAmount;
	}

	private void OnEnable()
	{
		if (!(selfDeactivateTimer <= 0f))
		{
			if (crtSelfDeactivate != null)
			{
				StopCoroutine(crtSelfDeactivate);
			}
			crtSelfDeactivate = StartCoroutine(waitSelfDeactivate(selfDeactivateTimer));
		}
	}

	private void Update()
	{
		if (GC.configDifficultyType == GameCore.DifficultyType.Easy && dmgAmount == ogDmgAmount)
		{
			dmgAmount = Mathf.RoundToInt((float)dmgAmount * 0.5f);
		}
		else if (GC.configDifficultyType == GameCore.DifficultyType.Normal && dmgAmount != ogDmgAmount)
		{
			dmgAmount = ogDmgAmount;
		}
	}

	public void CollPerFrame(int frame)
	{
		GetColliderReference();
		if (box2D != null)
		{
			SetColliderDimensionsBox(listCollChange[frame].setActive, listCollChange[frame].setOffset, listCollChange[frame].setSize);
		}
		if (circle2D != null)
		{
			SetColliderRadiusCircle(listCollChange[frame].setActive, listCollChange[frame].setSize.x);
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
		if (!active && gameObject.activeInHierarchy && !disableOnlyAtAnimationEnd)
		{
			gameObject.SetActive(value: false);
		}
	}

	private void SetColliderRadiusCircle(bool active, float radius)
	{
		if (active && !gameObject.activeInHierarchy)
		{
			gameObject.SetActive(value: true);
		}
		circle2D.radius = radius;
		circle2D.enabled = active;
		if (!active && gameObject.activeInHierarchy && !disableOnlyAtAnimationEnd)
		{
			gameObject.SetActive(value: false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player") && detectPlayerCollision && SpecialOwnerReference != null)
		{
			SpecialOwnerReference.GetComponent<EnemyGeneralLogic>().ATKSpecial_CollPlayerDetected();
		}
	}

	public void PlayEnemyAttackSFX(string thisSFX)
	{
		switch (thisSFX)
		{
		case "SwingTK2":
			GC.PlaySFX(GameCore.SFXBasic1.SwingTK2, 0, checkForDuplicity: true, transform.position, 0f, 0f);
			break;
		case "FutaGhostHand":
			GC.PlaySFXMonsterSounds(GameCore.MonsterSounds.FTG_Cast_Random, 0, checkForDuplicity: true, transform.position, 0.95f, 1.15f, "FutaGhost");
			break;
		case "Swing3":
			GC.PlaySFX(GameCore.SFXBasic2.Swing3, 0, checkForDuplicity: false, transform.position, 0.9f, 1.1f);
			break;
		}
	}

	private void GetColliderReference()
	{
		if (box2D == null)
		{
			box2D = GetComponent<BoxCollider2D>();
		}
		if (circle2D == null)
		{
			circle2D = GetComponent<CircleCollider2D>();
		}
	}

	public void ATK_DisableSelf()
	{
		gameObject.SetActive(value: false);
	}

	private IEnumerator waitSelfDeactivate(float wait)
	{
		yield return new WaitForSeconds(wait);
		gameObject.SetActive(value: false);
		crtSelfDeactivate = null;
	}
}
