using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryTabChange : MonoBehaviour, ISelectHandler, IEventSystemHandler, IDeselectHandler
{
	public enum InventorySelectable
	{
		ItemTab,
		ReturnButton
	}

	public GameCore.InventoryLogicType thisInventoryType;

	public InventorySelectable thisTypeOfSelectable;

	public GameObject selectButtonChildFromThis;

	public GameObject hackyMaintainSelection;

	private GameCore GC;

	private Button selfButton;

	private Navigation originalButtonNavigation;

	private void Start()
	{
		GC = GameCore.Instance;
		selfButton = GetComponent<Button>();
		originalButtonNavigation = selfButton.navigation;
		if (gameObject.name.Contains("Consumable"))
		{
			SelectLogic();
		}
	}

	private void Update()
	{
		if ((bool)GC && gameObject.activeInHierarchy && GC.lastSelectedInventoryTab == selfButton && thisTypeOfSelectable == InventorySelectable.ItemTab && !hackyMaintainSelection.activeInHierarchy)
		{
			Button[] componentsInChildren = selectButtonChildFromThis.GetComponentsInChildren<Button>();
			foreach (Button button in componentsInChildren)
			{
				if (EventSystem.current.currentSelectedGameObject == button.gameObject)
				{
					hackyMaintainSelection.SetActive(value: true);
				}
			}
		}
		else if (hackyMaintainSelection != null && hackyMaintainSelection.activeInHierarchy && GC.lastSelectedInventoryTab != selfButton)
		{
			hackyMaintainSelection.SetActive(value: false);
		}
	}

	private bool UPDT_CheckChildItemSelection()
	{
		Button[] componentsInChildren = selectButtonChildFromThis.GetComponentsInChildren<Button>();
		foreach (Button button in componentsInChildren)
		{
			if (EventSystem.current.currentSelectedGameObject == button.gameObject)
			{
				return true;
			}
		}
		return false;
	}

	public void OnSelect(BaseEventData eventData)
	{
		SelectLogic();
	}

	private void SelectLogic()
	{
		if (!selfButton)
		{
			return;
		}
		switch (thisTypeOfSelectable)
		{
		case InventorySelectable.ItemTab:
			selfButton.onClick.Invoke();
			ChangeNavigationItemTab();
			ChangeSizeOfInventoryTab(doGrow: true);
			if ((bool)GC)
			{
				GC.lastSelectedInventoryTab = selfButton;
			}
			break;
		case InventorySelectable.ReturnButton:
			ChangeNavigationReturnButton();
			break;
		}
	}

	public void OnDeselect(BaseEventData eventData)
	{
		ChangeSizeOfInventoryTab(doGrow: false);
	}

	private void ChangeNavigationItemTab()
	{
		if (selectButtonChildFromThis != null)
		{
			Navigation navigation = originalButtonNavigation;
			Button componentInChildren = selectButtonChildFromThis.GetComponentInChildren<Button>();
			if (componentInChildren != null)
			{
				navigation.selectOnDown = componentInChildren;
			}
			selfButton.navigation = navigation;
		}
	}

	private void ChangeNavigationReturnButton()
	{
		if (!(selectButtonChildFromThis != null))
		{
			return;
		}
		Navigation navigation = originalButtonNavigation;
		Button[] componentsInChildren = selectButtonChildFromThis.GetComponentsInChildren<Button>();
		if (componentsInChildren != null && componentsInChildren.Length != 0)
		{
			if (componentsInChildren.Length == 1)
			{
				navigation.selectOnUp = componentsInChildren[0];
			}
			else
			{
				navigation.selectOnUp = componentsInChildren[componentsInChildren.Length / 2];
			}
			navigation.selectOnDown = GC.lastSelectedInventoryTab;
		}
		else if ((bool)GC && GC.lastSelectedInventoryTab != null)
		{
			navigation.selectOnUp = GC.lastSelectedInventoryTab;
			navigation.selectOnDown = GC.lastSelectedInventoryTab;
		}
		selfButton.navigation = navigation;
	}

	private void ChangeSizeOfInventoryTab(bool doGrow)
	{
		if (doGrow)
		{
			transform.localScale = new Vector3(1.25f, 1.25f, 1f);
		}
		else
		{
			transform.localScale = new Vector3(1f, 1f, 1f);
		}
	}
}
