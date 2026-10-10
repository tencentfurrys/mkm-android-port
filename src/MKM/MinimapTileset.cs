using UnityEngine;
using UnityEngine.Tilemaps;

public class MinimapTileset : MonoBehaviour
{
	public GameObject[] arrayOfCollisionTileset;

	public Color colorCOLL = Color.white;

	public Material materialCOLL;

	public GameObject[] arrayOfRemovableTileset;

	public Color colorRMV = Color.white;

	public Material materialRMV;

	private void Start()
	{
		CloneTilesetsForMinimap();
	}

	private void CloneTilesetsForMinimap()
	{
		for (int i = 0; i < arrayOfCollisionTileset.Length; i++)
		{
			GameObject cloneOBJ = Object.Instantiate(arrayOfCollisionTileset[i], transform);
			ReplaceCloneSettings(cloneOBJ, colorCOLL, materialCOLL, disableCollider: true);
		}
		for (int j = 0; j < arrayOfRemovableTileset.Length; j++)
		{
			GameObject cloneOBJ2 = Object.Instantiate(arrayOfRemovableTileset[j], transform);
			ReplaceCloneSettings(cloneOBJ2, colorRMV, materialRMV, disableCollider: false);
		}
	}

	private void ReplaceCloneSettings(GameObject CloneOBJ, Color setColor, Material setMaterial, bool disableCollider)
	{
		Tilemap component = CloneOBJ.GetComponent<Tilemap>();
		TilemapRenderer component2 = CloneOBJ.GetComponent<TilemapRenderer>();
		TilemapCollider2D tilemapCollider2D = null;
		if (disableCollider)
		{
			tilemapCollider2D = CloneOBJ.GetComponent<TilemapCollider2D>();
		}
		CloneOBJ.layer = LayerMask.NameToLayer("Minimap");
		CloneOBJ.tag = "MinimapClone";
		component.color = setColor;
		component2.material = setMaterial;
		if (disableCollider && tilemapCollider2D != null)
		{
			tilemapCollider2D.enabled = false;
		}
	}
}
