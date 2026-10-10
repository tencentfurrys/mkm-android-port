using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemScrollManager : MonoBehaviour
{
	public GameCore.InventoryLogicType selfInventoryLogic;

	public Transform contentPanel;

	public SimpleObjectPool buttonObjectPool;

	public IconInventoryList listInventoryIcon;

	public Text txtItemDescription;

	public Text txtTabName;

	public Text txtItemQuantity;

	public Text txtDiscount;

	private GameCore GC;

	private GameCore.InventoryTabType previousListType;

	private GameCore.InventoryTabType currentListType;

	private Coroutine crtDelayAfterListLoad;

	private int _lastItemIDCount;

	private float timeDelayAfterListLoad;

	private bool isFirstLoad;

	private bool isDelayAfterListLoad;

	private void OnEnable()
	{
		timeDelayAfterListLoad = 0.1f;
		crtDelayAfterListLoad = null;
		isFirstLoad = true;
		isDelayAfterListLoad = false;
		currentListType = GameCore.InventoryTabType.Consumables;
		previousListType = currentListType;
		SetupTabType("Default");
	}

	public void SetupTabType(string tabName)
	{
		if (!isDelayAfterListLoad)
		{
			switch (tabName)
			{
			default:
				currentListType = GameCore.InventoryTabType.Consumables;
				break;
			case "Skills":
				currentListType = GameCore.InventoryTabType.Skills;
				break;
			case "KeyItems":
				currentListType = GameCore.InventoryTabType.KeyItems;
				break;
			case "Gear":
				currentListType = GameCore.InventoryTabType.Gear;
				break;
			case "Misc":
				currentListType = GameCore.InventoryTabType.Misc;
				break;
			case "Equip":
				currentListType = GameCore.InventoryTabType.Equip;
				break;
			case "Acc":
				currentListType = GameCore.InventoryTabType.Accessories;
				break;
			}
			LoadList();
		}
	}

	public void TurnOffAllListBehaviour()
	{
		StopAllCoroutines();
		UnloadList();
	}

	private void LoadList()
	{
		CheckGameCoreInstance();
		if (currentListType != previousListType || isFirstLoad)
		{
			UnloadList();
			AddItemButtons();
			previousListType = currentListType;
			isFirstLoad = false;
			BeginTheDelayAfterLoad();
		}
		else if (previousListType == currentListType && contentPanel != null && contentPanel.childCount > 0)
		{
			Button component = contentPanel.transform.GetChild(0).gameObject.GetComponent<Button>();
			if (component != null)
			{
				component.Select();
			}
		}
	}

	private void UnloadList()
	{
		if (contentPanel.childCount > 0 && CheckGameCoreInstance())
		{
			RemoveItemButton();
		}
	}

	private void Update()
	{
		UpdateCurrentItemDescription();
		UpdateDiscountText();
	}

	private void UpdateCurrentItemDescription()
	{
		GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;
		if (currentSelectedGameObject != null && currentSelectedGameObject.name.Contains("btnItemDynamic"))
		{
			DynamicButtonItem component = currentSelectedGameObject.GetComponent<DynamicButtonItem>();
			if (component.itemDescription != "")
			{
				txtItemDescription.text = GC.ParseDialogueLineTags(component.itemDescription);
			}
			txtTabName.text = "";
			if ((selfInventoryLogic == GameCore.InventoryLogicType.NeneShop && txtItemQuantity != null && _lastItemIDCount != component.itemID) || GC.updateShopListAfterPurchase)
			{
				_lastItemIDCount = component.itemID;
				txtItemQuantity.text = GC.GetTranslatedStringUI(172) + ": " + GC.ItemQuantityInPlayerInventory(component.itemID);
			}
		}
		else if (currentSelectedGameObject != null && currentSelectedGameObject.name.Contains("btnBX"))
		{
			int num = int.Parse(currentSelectedGameObject.name.Substring(5, 1));
			ItemInfo itemInfo = null;
			if (GC.arraySlotsOfBelt[num] != null)
			{
				itemInfo = GC.GetItemInfoFromDatabase(GC.arraySlotsOfBelt[num].id);
			}
			if (itemInfo != null && itemInfo.description != "")
			{
				txtItemDescription.text = "[" + itemInfo.name + "]\r\n\r\n" + GC.ParseDialogueLineTags(itemInfo.description);
				txtTabName.text = "";
			}
			else
			{
				txtItemDescription.text = "";
				txtTabName.text = "[" + GetListTypeName() + "]";
			}
		}
		else
		{
			txtItemDescription.text = "";
			txtTabName.text = "[" + GetListTypeName() + "]";
			if (selfInventoryLogic == GameCore.InventoryLogicType.NeneShop && txtItemQuantity != null)
			{
				_lastItemIDCount = 0;
				txtItemQuantity.text = GC.GetTranslatedStringUI(172) + ": 0";
			}
		}
	}

	private void UpdateDiscountText()
	{
		if (!GC || !txtDiscount)
		{
			return;
		}
		if (GC.discountShopTotal > 0)
		{
			if (txtDiscount.color.a != 1f)
			{
				txtDiscount.color = new Color(txtDiscount.color.r, txtDiscount.color.g, txtDiscount.color.b, 1f);
			}
			txtDiscount.text = GC.GetTranslatedStringUI(177) + ": " + GC.discountShopTotal + "%";
		}
		else if (txtDiscount.color.a == 1f)
		{
			txtDiscount.color = new Color(txtDiscount.color.r, txtDiscount.color.g, txtDiscount.color.b, 0f);
		}
	}

	private string GetListTypeName()
	{
		switch (currentListType)
		{
		case GameCore.InventoryTabType.Consumables:
			return GC.GetTranslatedStringUI(86);
		case GameCore.InventoryTabType.Gear:
			return GC.GetTranslatedStringUI(87);
		case GameCore.InventoryTabType.Skills:
			return GC.GetTranslatedStringUI(88);
		case GameCore.InventoryTabType.KeyItems:
			return GC.GetTranslatedStringUI(89);
		case GameCore.InventoryTabType.Equip:
			return GC.GetTranslatedStringUI(143);
		case GameCore.InventoryTabType.Misc:
			return GC.GetTranslatedStringUI(90);
		case GameCore.InventoryTabType.Accessories:
			return GC.GetTranslatedStringUI(173);
		default:
			return "";
		}
	}

	private void AddItemButtons()
	{
		GameCore.InventoryLogicType inventoryLogicType = selfInventoryLogic;
		if (inventoryLogicType == GameCore.InventoryLogicType.PlayerInventory || inventoryLogicType != GameCore.InventoryLogicType.NeneShop)
		{
			AddButtonPlayerInventory();
		}
		else
		{
			AddButtonHarunaShop();
		}
	}

	private void AddButtonPlayerInventory()
	{
		List<CountItemsQuantity> list = new List<CountItemsQuantity>();
		GC.listPlayerInventory.Sort((ItemInfo x, ItemInfo y) => x.id.CompareTo(y.id));
		foreach (ItemInfo item in GC.listPlayerInventory)
		{
			if (!CheckItemTypeGroup(item.type))
			{
				continue;
			}
			if (list.Exists((CountItemsQuantity x) => x.id == item.id))
			{
				list[list.FindIndex((CountItemsQuantity x) => x.id == item.id)].quantity++;
			}
			else
			{
				list.Add(new CountItemsQuantity
				{
					id = item.id,
					quantity = 1
				});
			}
		}
		foreach (CountItemsQuantity item2 in list)
		{
			ItemInfo itemInfo = GC.listPlayerInventory.Find((ItemInfo x) => x.id == item2.id);
			if (itemInfo == null)
			{
				Debug.Log("[ItemScrollManager]AddItemButtons ERROR: Could not find ITEM_ID in the LIST! (ID:" + item2.id + ")");
				break;
			}
			buttonObjectPool.GetObject(contentPanel).GetComponent<DynamicButtonItem>().SetupButtonInformation(itemInfo, listInventoryIcon.GetIconFromID(item2.id), item2.quantity, selfInventoryLogic);
		}
		list.Clear();
	}

	private void AddButtonHarunaShop()
	{
		ItemInfo itemInfo = new ItemInfo();
		GC.listShopState.Sort((ItemInfo x, ItemInfo y) => x.id.CompareTo(y.id));
		foreach (ItemInfo item in GC.listShopState)
		{
			if (item.type == 0)
			{
				itemInfo = GC.GetItemInfoFromDatabase(item.id);
				item.name = itemInfo.name;
				item.description = itemInfo.description;
				item.type = itemInfo.type;
			}
		}
		foreach (ItemInfo item2 in GC.listShopState)
		{
			if (CheckItemTypeGroup(item2.type))
			{
				buttonObjectPool.GetObject(contentPanel).GetComponent<DynamicButtonItem>().SetupButtonInformation(item2, listInventoryIcon.GetIconFromID(item2.id), item2.shopStock, selfInventoryLogic);
			}
		}
		int num = 0;
		int num2 = 4;
		ItemInfo itemInfo2 = new ItemInfo();
		itemInfo2.id = GC.constEmptyShopSlotID;
		while (!GC.MATH_IsDivisibleBy(contentPanel.childCount, num2) && num <= num2)
		{
			num++;
			buttonObjectPool.GetObject(contentPanel).GetComponent<DynamicButtonItem>().SetupButtonInformation(itemInfo2, listInventoryIcon.GetIconFromID(itemInfo2.id), itemInfo2.shopStock, selfInventoryLogic);
		}
	}

	private void RemoveItemButton()
	{
		while (contentPanel.childCount > 0)
		{
			GameObject gameObject = contentPanel.transform.GetChild(0).gameObject;
			DynamicButtonItem component = gameObject.GetComponent<DynamicButtonItem>();
			if ((bool)component && (bool)component.imgOutOfStock)
			{
				component.imgOutOfStock.gameObject.SetActive(value: false);
			}
			buttonObjectPool.ReturnObject(gameObject);
		}
	}

	private bool CheckItemTypeGroup(int typeCheck)
	{
		switch ((GameCore.CollectibleType)typeCheck)
		{
		case GameCore.CollectibleType.HealthPotion:
		case GameCore.CollectibleType.ManaPotion:
		case GameCore.CollectibleType.SubPotions:
		case GameCore.CollectibleType.SubConsumables:
			if (currentListType == GameCore.InventoryTabType.Consumables)
			{
				return true;
			}
			break;
		case GameCore.CollectibleType.ThrowDagger:
		case GameCore.CollectibleType.SmallRuneBomb:
		case GameCore.CollectibleType.MetalDust:
		case GameCore.CollectibleType.FreshClothing:
		case GameCore.CollectibleType.Throwable:
			if (currentListType == GameCore.InventoryTabType.Gear)
			{
				return true;
			}
			break;
		case GameCore.CollectibleType.SkillPickup:
			if (currentListType == GameCore.InventoryTabType.Skills)
			{
				return true;
			}
			break;
		case GameCore.CollectibleType.KeyItem:
		case GameCore.CollectibleType.GalleryGlobe:
			if (currentListType == GameCore.InventoryTabType.KeyItems)
			{
				return true;
			}
			break;
		case GameCore.CollectibleType.CoreEquip:
			if (currentListType == GameCore.InventoryTabType.Equip)
			{
				return true;
			}
			break;
		case GameCore.CollectibleType.Quartz:
		case GameCore.CollectibleType.AttackUpgrade:
		case GameCore.CollectibleType.DefenseUpgrade:
		case GameCore.CollectibleType.HealthUpgrade:
		case GameCore.CollectibleType.SpiritUpgrade:
		case GameCore.CollectibleType.GeneralUpgrades:
			if (currentListType == GameCore.InventoryTabType.Misc)
			{
				return true;
			}
			break;
		case GameCore.CollectibleType.Ring:
		case GameCore.CollectibleType.Bracelet:
		case GameCore.CollectibleType.Necklace:
		case GameCore.CollectibleType.Earring:
		case GameCore.CollectibleType.Hairpin:
			if (currentListType == GameCore.InventoryTabType.Accessories)
			{
				return true;
			}
			break;
		}
		return false;
	}

	private bool CheckGameCoreInstance()
	{
		if ((bool)GC)
		{
			return true;
		}
		GC = GameCore.Instance;
		if ((bool)GC)
		{
			return true;
		}
		return false;
	}

	private void BeginTheDelayAfterLoad()
	{
		if (crtDelayAfterListLoad != null)
		{
			StopCoroutine(crtDelayAfterListLoad);
		}
		crtDelayAfterListLoad = StartCoroutine(waitDelayAfterListLoad(timeDelayAfterListLoad));
	}

	private IEnumerator waitDelayAfterListLoad(float wait)
	{
		isDelayAfterListLoad = true;
		yield return new WaitForSecondsRealtime(wait);
		isDelayAfterListLoad = false;
		crtDelayAfterListLoad = null;
	}
}
