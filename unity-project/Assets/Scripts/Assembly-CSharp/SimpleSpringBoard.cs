using System.Collections;
using UnityEngine;

public class SimpleSpringBoard : MonoBehaviour
{
	public string strIdleAnim;

	public string strBounceAnim;

	[HideInInspector]
	public bool canPushUp;

	private bool canPlayBounceAnim;

	private float timeToReset;

	private Animator objAN;

	private Coroutine crtSpringBoardReset;

	private Coroutine crtKeepEffect;

	private GameCore GC;

	private void OnEnable()
	{
		if ((bool)objAN)
		{
			objAN.Play(strIdleAnim);
		}
	}

	private void Start()
	{
		GC = GameCore.Instance;
		objAN = GetComponent<Animator>();
		canPushUp = true;
		canPlayBounceAnim = true;
		timeToReset = 0.5f;
		crtKeepEffect = null;
		crtSpringBoardReset = null;
		if ((bool)objAN)
		{
			objAN.Play(strIdleAnim);
		}
	}

	public void SpringBoardWasActivated()
	{
		if (crtKeepEffect == null && crtSpringBoardReset == null)
		{
			if ((bool)GC)
			{
				GC.PlaySFX(GameCore.SFXBasic1.Bounce, 0, checkForDuplicity: true, base.transform.position, 0.9f, 1.1f);
			}
			if (canPlayBounceAnim && (bool)objAN)
			{
				objAN.Play(strBounceAnim);
				canPlayBounceAnim = false;
			}
			crtKeepEffect = StartCoroutine(waitForLingeringEffect(timeToReset / 2f));
		}
	}

	private IEnumerator waitForLingeringEffect(float wait)
	{
		yield return new WaitForSeconds(wait);
		canPushUp = false;
		crtKeepEffect = null;
		if (crtSpringBoardReset != null)
		{
			StopCoroutine(crtSpringBoardReset);
		}
		crtSpringBoardReset = StartCoroutine(waitForSpringBoardReset(timeToReset));
	}

	private IEnumerator waitForSpringBoardReset(float wait)
	{
		yield return new WaitForSeconds(wait);
		if ((bool)objAN)
		{
			objAN.Play(strIdleAnim);
		}
		canPlayBounceAnim = true;
		canPushUp = true;
		crtSpringBoardReset = null;
	}
}
