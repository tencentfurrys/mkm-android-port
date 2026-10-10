using System;
using UnityEngine;

[Serializable]
public class clsGalleryEnemyList
{
	public GameCore.EnemyType logicType;

	public int idNameJSON;

	public Sprite displaySprite;

	public int customScaleSprite;

	public Vector2 OffsetTPPos;

	public GameObject enemyPrefab;

	public GameObject enemyCombo;

	public int EroVariantID;

	public int arenaSpawnType;
}
