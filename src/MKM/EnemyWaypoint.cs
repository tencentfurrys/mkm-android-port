using System.Collections;
using UnityEngine;

public class EnemyWaypoint : MonoBehaviour
{
	public GameObject myParentOBJ;

	public float timerAutoDisable;

	private void Start()
	{
		if (timerAutoDisable > 0f)
		{
			StartCoroutine(waitSelfDisable(timerAutoDisable));
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (myParentOBJ != null && other.gameObject == myParentOBJ)
		{
			myParentOBJ.GetComponent<EnemyGeneralLogic>().GENERIC_TouchedWayPoint();
		}
	}

	private IEnumerator waitSelfDisable(float time)
	{
		yield return new WaitForSeconds(time);
		gameObject.SetActive(value: false);
	}
}
