using UnityEngine;

public class PlatMoveDots : MonoBehaviour
{
	public void UpdateChildIndicatorVisuals(bool receivedSignal)
	{
		foreach (Transform item in base.transform)
		{
			item.GetComponent<Animator>().SetBool("signal", receivedSignal);
		}
	}
}
