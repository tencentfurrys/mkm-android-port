using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions;

[AddComponentMenu("UI/Extensions/UI ScrollTo Selection XY")]
[RequireComponent(typeof(ScrollRect))]
public class UIScrollToSelection : MonoBehaviour
{
	public float scrollSpeed = 5f;

	public float hackVertDeltaOffset = 3.5f;

	[SerializeField]
	private RectTransform layoutListGroup;

	private RectTransform targetScrollObject;

	private bool scrollToSelection = true;

	private RectTransform scrollWindow;

	private RectTransform currentCanvas;

	private ScrollRect targetScrollRect;

	private void Start()
	{
		targetScrollRect = GetComponent<ScrollRect>();
		scrollWindow = targetScrollRect.GetComponent<RectTransform>();
	}

	private void Update()
	{
		ScrollRectToLevelSelection();
	}

	private void ScrollRectToLevelSelection()
	{
		EventSystem current = EventSystem.current;
		if (targetScrollRect == null || layoutListGroup == null || scrollWindow == null)
		{
			return;
		}
		RectTransform rectTransform = null;
		if (current.currentSelectedGameObject != null)
		{
			rectTransform = current.currentSelectedGameObject.GetComponent<RectTransform>();
			if (rectTransform.parent != null && rectTransform.parent.CompareTag("CFG-Bind"))
			{
				rectTransform = rectTransform.parent.GetComponent<RectTransform>();
			}
		}
		if (rectTransform != targetScrollObject)
		{
			scrollToSelection = true;
		}
		if (!(rectTransform == null) && scrollToSelection && !(rectTransform.transform.parent != layoutListGroup.transform))
		{
			bool flag = false;
			bool flag2 = false;
			if (targetScrollRect.vertical)
			{
				float num = 0f - rectTransform.anchoredPosition.y;
				float num2 = layoutListGroup.anchoredPosition.y + layoutListGroup.sizeDelta.y / hackVertDeltaOffset;
				float num3 = 0f;
				num3 = num2 - num;
				targetScrollRect.verticalNormalizedPosition += num3 / layoutListGroup.sizeDelta.y * Time.unscaledDeltaTime * scrollSpeed;
				flag2 = Mathf.Abs(num3) < 2f;
			}
			if (targetScrollRect.horizontal)
			{
				float num4 = 0f - rectTransform.anchoredPosition.x;
				float x = layoutListGroup.anchoredPosition.x;
				float num5 = 0f;
				num5 = x - num4;
				targetScrollRect.horizontalNormalizedPosition += num5 / layoutListGroup.sizeDelta.x * Time.deltaTime * scrollSpeed;
				flag = Mathf.Abs(num5) < 2f;
			}
			if (flag & flag2)
			{
				scrollToSelection = false;
			}
			targetScrollObject = rectTransform;
		}
	}
}
