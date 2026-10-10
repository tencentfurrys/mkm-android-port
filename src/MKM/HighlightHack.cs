using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HighlightHack : EventTrigger
{
	public override void OnPointerEnter(PointerEventData data)
	{
		Selectable component = GetComponent<Selectable>();
		if (EventSystem.current.currentSelectedGameObject != gameObject && component.interactable)
		{
			component.Select();
		}
	}
}
