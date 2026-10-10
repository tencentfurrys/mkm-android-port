using UnityEngine;
using UnityEngine.EventSystems;

public class SimpleCursorLerp : MonoBehaviour
{
	public float upperBounceLimit = 1.45f;

	public float lowerBounceLimit = 1f;

	public float targetMarginInvert = 0.1f;

	public float speedBounce = 6f;

	public bool ignoreSelection;

	private GameCore GC;

	private GameObject objSelection;

	private Vector3 originalLocalScale;

	private bool EffectBounce;

	private void Awake()
	{
		objSelection = gameObject;
		originalLocalScale = gameObject.transform.localScale;
	}

	private void Update()
	{
		if (!objSelection)
		{
			return;
		}
		if (!GC)
		{
			GC = GameCore.Instance;
		}
		if ((bool)GC && GC.disableCursorAnimUI)
		{
			ResetToOriginalScale();
		}
		else if (objSelection.activeInHierarchy && (EventSystem.current.currentSelectedGameObject == gameObject || ignoreSelection))
		{
			if (!EffectBounce)
			{
				objSelection.transform.localScale = new Vector3(Mathf.Lerp(objSelection.transform.localScale.x, lowerBounceLimit, speedBounce * Time.fixedDeltaTime), Mathf.Lerp(objSelection.transform.localScale.y, lowerBounceLimit, speedBounce * Time.fixedDeltaTime), 1f);
				if (objSelection.transform.localScale.x < lowerBounceLimit + targetMarginInvert)
				{
					EffectBounce = true;
				}
			}
			else
			{
				objSelection.transform.localScale = new Vector3(Mathf.Lerp(objSelection.transform.localScale.x, upperBounceLimit, speedBounce * Time.fixedDeltaTime), Mathf.Lerp(objSelection.transform.localScale.y, upperBounceLimit, speedBounce * Time.fixedDeltaTime), 1f);
				if (objSelection.transform.localScale.x > upperBounceLimit - targetMarginInvert)
				{
					EffectBounce = false;
				}
			}
		}
		else
		{
			ResetToOriginalScale();
		}
	}

	private void ResetToOriginalScale()
	{
		if (objSelection.transform.localScale != originalLocalScale)
		{
			objSelection.transform.localScale = originalLocalScale;
		}
	}
}
