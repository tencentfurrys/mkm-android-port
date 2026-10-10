using UnityEngine;

public class PlatMoveDots : MonoBehaviour
{
	public void UpdateChildIndicatorVisuals(bool receivedSignal)
	{
		foreach (Transform item in transform)
		{
			item.GetComponent<Animator>().SetBool("signal", receivedSignal);
		}
	}
}
