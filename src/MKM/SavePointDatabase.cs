using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New SavePoint Database", menuName = "SavePoint Database")]
public class SavePointDatabase : ScriptableObject
{
	public List<SAVEP_MainDatabase> databaseOfSavePoint;

	public SAVEP_Screenshots[] databaseOfScreenshots;

	public Sprite ReturnScreenshotByID(int ID)
	{
		for (int i = 0; i < databaseOfScreenshots.Length; i++)
		{
			if (databaseOfScreenshots[i].idSavePoint == ID)
			{
				return databaseOfScreenshots[i].imageScreenshot;
			}
		}
		return databaseOfScreenshots[0].imageScreenshot;
	}
}
