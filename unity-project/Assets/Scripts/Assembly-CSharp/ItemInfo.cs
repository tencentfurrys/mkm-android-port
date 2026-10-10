using System;
using UnityEngine;

[Serializable]
public class ItemInfo
{
	public int id;

	public int type;

	public int usageCost;

	public int quantityHeld;

	public bool equipable;

	public string name;

	public string description;

	public string scene;

	public Vector2 position;

	public Sprite icon;

	public int shopPrice;

	public int shopStock;

	public ItemInfo()
	{
		id = 0;
		type = 0;
		usageCost = 0;
		quantityHeld = 0;
		equipable = false;
		name = "";
		description = "";
		scene = "";
		position = new Vector2(0f, 0f);
		icon = null;
		shopPrice = 0;
		shopStock = 0;
	}

	public ItemInfo(ItemInfo copyValues)
	{
		id = copyValues.id;
		type = copyValues.type;
		usageCost = copyValues.usageCost;
		quantityHeld = copyValues.quantityHeld;
		equipable = copyValues.equipable;
		name = copyValues.name;
		description = copyValues.description;
		scene = copyValues.scene;
		position = copyValues.position;
		icon = copyValues.icon;
		shopPrice = copyValues.shopPrice;
		shopStock = copyValues.shopStock;
	}
}
