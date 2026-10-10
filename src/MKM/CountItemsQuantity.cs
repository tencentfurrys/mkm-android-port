using System;

[Serializable]
public class CountItemsQuantity
{
	public int id;

	public int quantity;

	public CountItemsQuantity()
	{
		id = 0;
		quantity = 0;
	}

	public CountItemsQuantity(CountItemsQuantity copyValues)
	{
		id = copyValues.id;
		quantity = copyValues.quantity;
	}
}
