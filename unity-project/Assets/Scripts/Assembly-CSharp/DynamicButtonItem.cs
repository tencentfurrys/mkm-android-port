using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DynamicButtonItem : MonoBehaviour
{
	public Image itemIcon;

	public Text itemName;

	public string itemDescription;

	public string itemPrice;

	public Text txtItemQuantity;

	public Text txtItemQuantityShadow1;

	public Text txtItemQuantityShadow2;

	public GameObject objSelection;

	public Image imgOutOfStock;

	public int itemID;

	private const string _constMoneySymbol = "<size=24%><color=yellow>$</color></size>";

	private GameCore GC;

	private Button thisButton;

	private ItemInfo selfCurrentItem;

	private GameCore.InventoryLogicType thisCurrentInventoryType;

	private int thisItemQuantity;

	private void Update()
	{
		if (EventSystem.current.currentSelectedGameObject == base.gameObject)
		{
			if (!objSelection.activeInHierarchy)
			{
				objSelection.SetActive(value: true);
			}
			if (thisCurrentInventoryType == GameCore.InventoryLogicType.NeneShop && GetGameCoreReference() && GC.updateShopListAfterPurchase && GC.lastChildIndexShopSlotAfterPurchase == base.transform.GetSiblingIndex())
			{
				DynamicButtonReduceSelfQuantityAfterPurchase();
			}
		}
		else if (objSelection.activeInHierarchy)
		{
			objSelection.SetActive(value: false);
		}
	}

	public void DynamicButtonReduceSelfQuantityAfterPurchase()
	{
		GC.updateShopListAfterPurchase = false;
		if (thisItemQuantity > 0 && thisItemQuantity != GC.constShopStockInfinite)
		{
			thisItemQuantity--;
		}
		SetupButtonInformation(selfCurrentItem, itemIcon.sprite, thisItemQuantity, thisCurrentInventoryType);
	}

	public void SetupButtonInformation(ItemInfo itemReceived, Sprite iconReceived, int itemQuantity, GameCore.InventoryLogicType sentInvType)
	{
		GetGameCoreReference();
		thisButton = GetComponent<Button>();
		if ((bool)thisButton)
		{
			thisButton.interactable = true;
		}
		itemID = itemReceived.id;
		thisCurrentInventoryType = sentInvType;
		selfCurrentItem = itemReceived;
		itemIcon.sprite = iconReceived;
		thisItemQuantity = itemQuantity;
		itemPrice = selfCurrentItem.shopPrice.ToString();
		GameCore.InventoryLogicType inventoryLogicType = thisCurrentInventoryType;
		if (inventoryLogicType == GameCore.InventoryLogicType.PlayerInventory || inventoryLogicType != GameCore.InventoryLogicType.NeneShop)
		{
			itemDescription = "[" + selfCurrentItem.name + "]\r\n\r\n" + selfCurrentItem.description;
		}
		else if (selfCurrentItem.id == GC.constEmptyShopSlotID)
		{
			itemName.text = " ";
			itemDescription = " ";
		}
		else
		{
			itemName.text = "<size=24%><color=yellow>$</color></size>" + itemPrice;
			itemDescription = "[" + selfCurrentItem.name + "]\r\n\r\n" + selfCurrentItem.description;
			if (itemQuantity == 0)
			{
				itemName.text = "<color=black>-</color>";
			}
		}
		if ((thisItemQuantity > 1 && thisCurrentInventoryType == GameCore.InventoryLogicType.PlayerInventory) || (thisItemQuantity > 0 && thisItemQuantity != GC.constShopStockInfinite && thisCurrentInventoryType == GameCore.InventoryLogicType.NeneShop))
		{
			txtItemQuantity.gameObject.SetActive(value: true);
			txtItemQuantityShadow1.gameObject.SetActive(value: true);
			txtItemQuantityShadow2.gameObject.SetActive(value: true);
			txtItemQuantity.text = thisItemQuantity.ToString();
			txtItemQuantityShadow1.text = thisItemQuantity.ToString();
			txtItemQuantityShadow2.text = thisItemQuantity.ToString();
		}
		else
		{
			txtItemQuantity.gameObject.SetActive(value: false);
			txtItemQuantityShadow1.gameObject.SetActive(value: false);
			txtItemQuantityShadow2.gameObject.SetActive(value: false);
		}
		if (selfCurrentItem != null && selfCurrentItem.id != 9999 && thisItemQuantity <= 0)
		{
			imgOutOfStock.gameObject.SetActive(value: true);
		}
	}

	public void SelfButtonWasPressed()
	{
		if (GetGameCoreReference())
		{
			GameCore.InventoryLogicType inventoryLogicType = thisCurrentInventoryType;
			if (inventoryLogicType == GameCore.InventoryLogicType.PlayerInventory || inventoryLogicType != GameCore.InventoryLogicType.NeneShop)
			{
				if (selfCurrentItem.equipable)
				{
					GC.ItemButtonWasClickedOnInventoryScreenToOpenEquipScreen(selfCurrentItem.id, thisItemQuantity, thisButton);
				}
				else
				{
					Debug.Log("[DynamicButtonItem]SelfButtonWasPressed ATTENTION: This item type is not equipable into a belt action slot!");
				}
			}
			else
			{
				GC.TryToBuyItemFromShop(selfCurrentItem.id, selfCurrentItem.name, selfCurrentItem.type, int.Parse(itemPrice), thisItemQuantity, base.transform.GetSiblingIndex());
			}
		}
		else
		{
			Debug.Log("[DynamicButtonItem]SelfButtonWasPressed ERROR: Could not get reference to GAMECORE script instance!");
		}
	}

	private bool GetGameCoreReference()
	{
		GC = GameCore.Instance;
		if ((bool)GC)
		{
			return true;
		}
		return false;
	}
}
