using System;

[Serializable]
public class LogPurchase
{
	public int itemID;

	public int qtyPurchased;

	public LogPurchase()
	{
		itemID = -1;
		qtyPurchased = -1;
	}

	public LogPurchase(LogPurchase copyValues)
	{
		itemID = copyValues.itemID;
		qtyPurchased = copyValues.qtyPurchased;
	}
}
