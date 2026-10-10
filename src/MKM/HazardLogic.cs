using System.Collections;
using UnityEngine;

public class HazardLogic : MonoBehaviour
{
	public enum HazardType
	{
		Spike,
		BossVines
	}

	public HazardType currentType;

	public int damagePercentage;

	public float moveSpeed;

	private bool doMovement;

	private bool isDoingSpecialBehaviour;

	private int currentSpecialPhase;

	private int intSpecialDigit;

	private float originalMoveSpeed;

	private string strSpecialTag;

	private string strSpecialType;

	private Coroutine crtWaitForNextMovePhase;

	private Vector3 origPosition;

	private Vector3 targetPosition;

	private SpriteRenderer objRE;

	private void Start()
	{
		objRE = GetComponent<SpriteRenderer>();
		originalMoveSpeed = moveSpeed;
		Initialize();
	}

	private void Initialize()
	{
		strSpecialTag = "";
		crtWaitForNextMovePhase = null;
		isDoingSpecialBehaviour = false;
		doMovement = false;
		origPosition = transform.position;
		currentSpecialPhase = 0;
		if (damagePercentage <= 0)
		{
			damagePercentage = 10;
		}
	}

	private void Update()
	{
		if (doMovement && transform.position != targetPosition)
		{
			transform.position = Vector3.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
			if (transform.position == targetPosition)
			{
				doMovement = false;
				CheckForTheNextStep();
			}
		}
	}

	public void InitiateSpecialBehaviour(string specialTag, int specialDigit, string specialType)
	{
		if (isDoingSpecialBehaviour)
		{
			Debug.Log("Object " + gameObject.name + " is already performing a special behaviour!");
			return;
		}
		strSpecialTag = specialTag;
		intSpecialDigit = specialDigit;
		strSpecialType = specialType;
		HazardType hazardType = currentType;
		if (hazardType == HazardType.BossVines)
		{
			isDoingSpecialBehaviour = true;
			BeginBossVinesLogicPhase_1();
		}
	}

	private void CheckForTheNextStep()
	{
		HazardType hazardType = currentType;
		if (hazardType != HazardType.BossVines)
		{
			return;
		}
		switch (currentSpecialPhase)
		{
		case 0:
		{
			float wait = 2f;
			if (strSpecialType == "Chase")
			{
				wait = 0.5f;
			}
			currentSpecialPhase = 1;
			if (crtWaitForNextMovePhase != null)
			{
				StopCoroutine(crtWaitForNextMovePhase);
			}
			crtWaitForNextMovePhase = StartCoroutine(waitDelayNextMovePhase(wait));
			break;
		}
		case 1:
			currentSpecialPhase = 2;
			BeginBossVinesLogicPhase_2();
			break;
		case 2:
			if (strSpecialType != "Chase")
			{
				EndBossVineLogic();
				break;
			}
			currentSpecialPhase = 3;
			BeginBossVinesLogicPhase_3();
			break;
		default:
			EndBossVineLogic();
			break;
		}
	}

	private void BeginBossVinesLogicPhase_1()
	{
		if (intSpecialDigit == 1 && moveSpeed == originalMoveSpeed)
		{
			moveSpeed = originalMoveSpeed * 1.5f;
		}
		objRE.enabled = true;
		if (strSpecialType == "Chase")
		{
			moveSpeed = originalMoveSpeed * 1.75f;
			targetPosition = origPosition;
			if (strSpecialTag == "right")
			{
				targetPosition.x += 9.5f;
			}
			else
			{
				targetPosition.x -= 9.5f;
			}
			transform.position = targetPosition;
		}
		else
		{
			targetPosition = origPosition;
		}
		targetPosition.y += 4f;
		doMovement = true;
	}

	private void BeginBossVinesLogicPhase_2()
	{
		targetPosition = origPosition;
		if (strSpecialType == "Chase")
		{
			moveSpeed = originalMoveSpeed * 1.75f;
			targetPosition.y += 4f;
			if (strSpecialTag == "right")
			{
				targetPosition.x++;
			}
			else
			{
				targetPosition.x--;
			}
		}
		else if (strSpecialTag == "right")
		{
			targetPosition.x += 8f;
		}
		else
		{
			targetPosition.x -= 8f;
		}
		doMovement = true;
	}

	private void BeginBossVinesLogicPhase_3()
	{
		if (strSpecialType == "Chase")
		{
			moveSpeed = originalMoveSpeed * 1.75f;
		}
		targetPosition = origPosition;
		doMovement = true;
	}

	private void EndBossVineLogic()
	{
		objRE.enabled = false;
		transform.position = origPosition;
		Initialize();
	}

	private IEnumerator waitDelayNextMovePhase(float wait)
	{
		yield return new WaitForSeconds(wait);
		CheckForTheNextStep();
		crtWaitForNextMovePhase = null;
	}
}
