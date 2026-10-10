using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_BeltEquipMain
{
	public GameObject objContainerMaster;

	public Text labelSelectSlot;

	public Text labelItemName;

	public Image imgCurrentItem;

	public IconInventoryList listInvIcons;

	public UI_BeltInventorySlots[] arraySlots;
}
