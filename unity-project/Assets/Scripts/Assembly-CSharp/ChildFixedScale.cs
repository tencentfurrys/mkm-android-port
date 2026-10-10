using UnityEngine;

public class ChildFixedScale : MonoBehaviour
{
	public bool resetLocalPos;

	private void Update()
	{
		if (resetLocalPos && base.transform.localPosition != Vector3.zero)
		{
			base.transform.localPosition = Vector3.zero;
			resetLocalPos = false;
		}
		if (!(base.transform.parent == null))
		{
			if ((float)Mathf.RoundToInt(base.transform.parent.transform.localScale.x) == 1f && base.transform.localScale.x != 1f)
			{
				Vector3 localScale = base.transform.localScale;
				localScale.x = 1f;
				base.transform.localScale = localScale;
			}
			else if ((float)Mathf.RoundToInt(base.transform.parent.transform.localScale.x) == -1f && base.transform.localScale.x != -1f)
			{
				Vector3 localScale2 = base.transform.localScale;
				localScale2.x = -1f;
				base.transform.localScale = localScale2;
			}
		}
	}
}
