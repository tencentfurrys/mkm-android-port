using UnityEngine;

public class ChildFixedScale : MonoBehaviour
{
	public bool resetLocalPos;

	private void Update()
	{
		if (resetLocalPos && transform.localPosition != Vector3.zero)
		{
			transform.localPosition = Vector3.zero;
			resetLocalPos = false;
		}
		if (!(transform.parent == null))
		{
			if ((float)Mathf.RoundToInt(transform.parent.transform.localScale.x) == 1f && transform.localScale.x != 1f)
			{
				Vector3 localScale = transform.localScale;
				localScale.x = 1f;
				transform.localScale = localScale;
			}
			else if ((float)Mathf.RoundToInt(transform.parent.transform.localScale.x) == -1f && transform.localScale.x != -1f)
			{
				Vector3 localScale2 = transform.localScale;
				localScale2.x = -1f;
				transform.localScale = localScale2;
			}
		}
	}
}
