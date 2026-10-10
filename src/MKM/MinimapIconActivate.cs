using UnityEngine;

public class MinimapIconActivate : MonoBehaviour
{
	public GameObject minimapIcon;

	private void Start()
	{
		if (minimapIcon != null)
		{
			minimapIcon.SetActive(value: true);
			minimapIcon.GetComponent<SpriteRenderer>().sortingOrder = GetComponent<SpriteRenderer>().sortingOrder;
		}
	}
}
