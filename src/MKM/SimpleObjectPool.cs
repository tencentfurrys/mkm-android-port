using System.Collections.Generic;
using UnityEngine;

public class SimpleObjectPool : MonoBehaviour
{
	public GameObject prefab;

	private Stack<GameObject> inactiveInstances = new Stack<GameObject>();

	public GameObject GetObject(Transform desiredParent)
	{
		GameObject gameObject;
		if (inactiveInstances.Count > 0)
		{
			gameObject = inactiveInstances.Pop();
		}
		else
		{
			gameObject = Object.Instantiate(prefab, desiredParent);
			gameObject.AddComponent<PooledObject>().pool = this;
		}
		gameObject.transform.SetParent(desiredParent);
		gameObject.SetActive(value: true);
		return gameObject;
	}

	public void ReturnObject(GameObject toReturn)
	{
		PooledObject component = toReturn.GetComponent<PooledObject>();
		if (component != null && component.pool == this)
		{
			toReturn.transform.SetParent(transform);
			toReturn.SetActive(value: false);
			inactiveInstances.Push(toReturn);
		}
		else
		{
			Debug.LogWarning(toReturn.name + " was returned to a pool it wasn't spawned from! Destroying.");
			Object.Destroy(toReturn);
		}
	}
}
