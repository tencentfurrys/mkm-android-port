using System.Collections;
using UnityEngine;

public class EffectLogic : MonoBehaviour
{
	public bool isDisableOnFinalFrame;

	public bool isChangeParentOnDisable;

	public bool isRamdomizeSpawnPos;

	public bool isRamdomSpawnRotation;

	public bool facePlayerFlipX;

	public float minRandomSpawnPosition;

	public float maxRandomSpawnPosition;

	public float timeDurationLoop;

	public Vector2 targetPosMove;

	public float moveSpeed;

	public bool pointTowardsTargetPos;

	public bool fadeOutAtEndOfDuration;

	public float fadeOutTime;

	public bool alterSpriteScale;

	public Vector2 targetScale;

	public float alterScaleSpeed;

	private bool doMovement;

	private float fixedInvokedFrequency;

	private float repeatingFadeOutRate;

	private Color colorAlphaFadeOut;

	private SpriteRenderer objRE;

	private Coroutine coroutLoopingEffect;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		objRE = GetComponent<SpriteRenderer>();
		repeatingFadeOutRate = 0f;
		fixedInvokedFrequency = 0.05f;
		CheckRandomSpawnPosition();
		CheckRandomSpawnRotation();
		CheckMovement();
		if (!isDisableOnFinalFrame && timeDurationLoop > 0f)
		{
			if (coroutLoopingEffect != null)
			{
				StopCoroutine(coroutLoopingEffect);
			}
			coroutLoopingEffect = StartCoroutine(WaitForEffectTimer(timeDurationLoop));
		}
		if (facePlayerFlipX && GC.playerCurrentWorldXPosition < base.transform.position.x)
		{
			objRE.flipX = true;
		}
	}

	private void FixedUpdate()
	{
		if (doMovement)
		{
			base.transform.position = new Vector2(Mathf.SmoothStep(base.transform.position.x, targetPosMove.x, moveSpeed * Time.deltaTime), Mathf.SmoothStep(base.transform.position.y, targetPosMove.y, moveSpeed * Time.deltaTime));
		}
		if (alterSpriteScale)
		{
			base.transform.localScale = new Vector3(Mathf.SmoothStep(base.transform.localScale.x, targetScale.x, alterScaleSpeed * Time.deltaTime), Mathf.SmoothStep(base.transform.localScale.y, targetScale.y, alterScaleSpeed * Time.deltaTime), 0f);
		}
	}

	private void CheckRandomSpawnPosition()
	{
		if (isRamdomizeSpawnPos)
		{
			Vector3 position = base.transform.position;
			position.x += Random.Range(minRandomSpawnPosition, maxRandomSpawnPosition);
			position.y += Random.Range(minRandomSpawnPosition, maxRandomSpawnPosition);
			base.transform.position = position;
		}
	}

	private void CheckRandomSpawnRotation()
	{
		if (isRamdomSpawnRotation)
		{
			base.transform.Rotate(new Vector3(0f, 0f, 1f), Random.Range(-90, 90));
		}
	}

	private void CheckMovement()
	{
		if (moveSpeed > 0f && targetPosMove != Vector2.zero)
		{
			if (pointTowardsTargetPos)
			{
				Vector3 vector = new Vector3(targetPosMove.x, targetPosMove.y, 0f) - base.transform.position;
				float z = Mathf.Atan2(vector.y, vector.x) * 57.29578f;
				base.transform.rotation = Quaternion.Euler(0f, 0f, z);
			}
			doMovement = true;
		}
	}

	private IEnumerator WaitForEffectTimer(float waittime)
	{
		yield return new WaitForSeconds(waittime);
		if (isChangeParentOnDisable && (bool)GC.refOBJEffectsParent)
		{
			base.transform.parent = GC.refOBJEffectsParent.transform;
		}
		if (fadeOutAtEndOfDuration && fadeOutTime > 0f)
		{
			FadeOutEffect();
		}
		else
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void FinalFrameDisableEffect(int forcedDisable = 0)
	{
		if (isChangeParentOnDisable && (bool)GC.refOBJEffectsParent)
		{
			base.transform.parent = GC.refOBJEffectsParent.transform;
		}
		if (isDisableOnFinalFrame || forcedDisable != 0)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void VFXBehaviorFromAnim(string action)
	{
		if (action == "BlueFoxTP" && !doMovement)
		{
			targetPosMove = base.transform.position;
			targetPosMove.y += 20f;
			moveSpeed = 5f;
			doMovement = true;
		}
	}

	private void FadeOutEffect()
	{
		repeatingFadeOutRate = 1f / fadeOutTime * fixedInvokedFrequency;
		InvokeRepeating("InvokedFadeOutEffect", 0f, fixedInvokedFrequency);
	}

	private void InvokedFadeOutEffect()
	{
		colorAlphaFadeOut = objRE.color;
		colorAlphaFadeOut.a -= repeatingFadeOutRate;
		objRE.color = colorAlphaFadeOut;
		if (objRE.color.a <= 0f)
		{
			CancelInvoke("InvokedFadeOutEffect");
			base.gameObject.SetActive(value: false);
		}
	}
}
