using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EroComboReceptor : MonoBehaviour
{
	public enum ComboType
	{
		Null = 0,
		Lizards1on1 = 1,
		GoblinsUpTo4 = 2,
		HillFairyUpTo3 = 3
	}

	private EnemyGeneralLogic scriptSelfEnemy;

	private EnemyGeneralLogic scriptOtherEnemy;

	private EroAnimation scriptSelfEroAnim;

	private GameCore GC;

	private Coroutine crtDelayCollisionDetectionOnEnable;

	public int maxEroPhaseToGrabForCombo;

	public ComboType comboType;

	private static string defaultEnemyTag = "Enemy-Basic";

	private bool canPerformCheck;

	private int intPlayThisID;

	private float delayCollissionSafety = 1.75f;

	private void Start()
	{
		GC = GameCore.Instance;
		crtDelayCollisionDetectionOnEnable = null;
	}

	private void OnEnable()
	{
		BeginCourotineDelayCollisionCheck(delayCollissionSafety);
	}

	private void CheckThisComboCandidate(GameObject otherOBJ)
	{
		canPerformCheck = false;
		switch (comboType)
		{
		default:
			Debug.Log("Other enemy [" + otherOBJ.name + "] collided with a combo receptor, but it is set as NULL! Open the EroCombo script to add its own logic.");
			break;
		case ComboType.Lizards1on1:
			ValidateComboType1(otherOBJ);
			break;
		case ComboType.GoblinsUpTo4:
		case ComboType.HillFairyUpTo3:
			ValidateComboType2(otherOBJ);
			break;
		}
		BeginCourotineDelayCollisionCheck(delayCollissionSafety);
	}

	private void ValidateComboType1(GameObject otherOBJ)
	{
		if (GetSelfScript() && !scriptSelfEroAnim.isPlayingComboEroScene && GetOtherScript(otherOBJ) && scriptSelfEroAnim._eroPhaseAnimator <= maxEroPhaseToGrabForCombo && ValidateFetishComboScene(scriptOtherEnemy.currentEnemyType, out intPlayThisID) && intPlayThisID != 0)
		{
			scriptSelfEroAnim.InterruptSoloSceneAndStartComboScene(intPlayThisID, otherOBJ);
		}
	}

	private void ValidateComboType2(GameObject otherOBJ)
	{
		int num = 4;
		if (comboType == ComboType.HillFairyUpTo3)
		{
			num = 3;
		}
		if (GetSelfScript() && GetOtherScript(otherOBJ) && scriptSelfEroAnim._eroPhaseAnimator <= maxEroPhaseToGrabForCombo && ValidateFetishComboScene(scriptOtherEnemy.currentEnemyType, out intPlayThisID) && intPlayThisID != 0 && scriptSelfEroAnim._eroIDVariant < num)
		{
			intPlayThisID = scriptSelfEroAnim._eroIDVariant + 1;
			scriptSelfEroAnim.InterruptSoloSceneAndStartComboScene(intPlayThisID, otherOBJ);
		}
	}

	private void OnTriggerStay2D(Collider2D other)
	{
		if (ValidateComboCollission(other.tag, other.gameObject.GetInstanceID()))
		{
			CheckThisComboCandidate(other.gameObject);
		}
	}

	private void OnCollisionStay2D(Collision2D other)
	{
		if (ValidateComboCollission(other.gameObject.tag, other.gameObject.GetInstanceID()))
		{
			CheckThisComboCandidate(other.gameObject);
		}
	}

	private bool ValidateComboCollission(string otherTag, int InstanceID)
	{
		if (otherTag == defaultEnemyTag && canPerformCheck && InstanceID != base.transform.parent.gameObject.GetInstanceID())
		{
			return true;
		}
		return false;
	}

	private bool ValidateFetishComboScene(GameCore.EnemyType otherEnemyType, out int playThisComboID)
	{
		if (scriptSelfEnemy.eroCore.eroComboScenes != null && scriptSelfEnemy.eroCore.eroComboScenes.Length != 0)
		{
			List<ENM_EroComboScenes> list = new List<ENM_EroComboScenes>();
			for (int i = 0; i < scriptSelfEnemy.eroCore.eroComboScenes.Length; i++)
			{
				if (scriptSelfEnemy.eroCore.eroComboScenes[i].PairedWith == otherEnemyType)
				{
					list.Add(scriptSelfEnemy.eroCore.eroComboScenes[i]);
				}
			}
			for (int num = list.Count - 1; num >= 0; num--)
			{
				if (!GC.CompareFetishToggle(list[num].SceneTag) && !GC.VerifyOverrideSpecialCasesForCombos(scriptSelfEnemy.currentEnemyType))
				{
					list.RemoveAt(num);
				}
			}
			if (list.Count > 0)
			{
				List<int> list2 = new List<int>();
				foreach (ENM_EroComboScenes item in list)
				{
					list2.Add(item.GenericID);
				}
				playThisComboID = GC.MATH_RNG_INT_ARRAY(list2.ToArray());
				return true;
			}
		}
		playThisComboID = 0;
		return false;
	}

	private bool GetSelfScript()
	{
		if (scriptSelfEnemy == null)
		{
			scriptSelfEnemy = GetComponentInParent<EnemyGeneralLogic>();
		}
		if (scriptSelfEroAnim == null)
		{
			scriptSelfEroAnim = GetComponentInParent<EroAnimation>();
		}
		if ((bool)scriptSelfEnemy && (bool)scriptSelfEroAnim)
		{
			return true;
		}
		return false;
	}

	private bool GetOtherScript(GameObject otherOBJ)
	{
		scriptOtherEnemy = otherOBJ.GetComponent<EnemyGeneralLogic>();
		if ((bool)scriptOtherEnemy)
		{
			return true;
		}
		return false;
	}

	private void ResetVariables()
	{
		crtDelayCollisionDetectionOnEnable = null;
		scriptSelfEnemy = null;
		scriptOtherEnemy = null;
		scriptSelfEroAnim = null;
		canPerformCheck = false;
		intPlayThisID = 0;
	}

	private void OnDisable()
	{
		ResetVariables();
	}

	private void BeginCourotineDelayCollisionCheck(float waitTime)
	{
		if (crtDelayCollisionDetectionOnEnable != null)
		{
			StopCoroutine(crtDelayCollisionDetectionOnEnable);
		}
		crtDelayCollisionDetectionOnEnable = StartCoroutine(waitToActivateCollisionDetection(waitTime));
	}

	private IEnumerator waitToActivateCollisionDetection(float wait)
	{
		yield return new WaitForSeconds(wait);
		canPerformCheck = true;
		crtDelayCollisionDetectionOnEnable = null;
	}
}
