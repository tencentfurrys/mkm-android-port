using UnityEngine;

public class clsGalleryEnemyToView
{
	public bool isRedirectToGalleryView;

	public GameObject objEnemyPrefab;

	public GameObject objEnemyCombo;

	public Vector3 offsetSpawnPos;

	public int idEroVariant;

	public void clsGalleryEnemyList()
	{
		isRedirectToGalleryView = false;
		objEnemyPrefab = null;
		offsetSpawnPos = Vector3.zero;
		idEroVariant = 0;
	}
}
