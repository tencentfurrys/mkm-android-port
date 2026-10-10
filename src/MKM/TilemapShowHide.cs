using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapShowHide : MonoBehaviour
{
	public GameObject objTilemap;

	public SpriteRenderer[] arraySpriteRenderer;

	[Range(0f, 1f)]
	public float targetAlphaLevel;

	private bool stateShow;

	private bool isPlayerInside;

	private float floatDelayTime;

	private Coroutine crtDelayVisibilityChange;

	private Color colorOriginal;

	private Color colorAlphaTransparency;

	private Color colorTemp;

	private Color colorMinimapClone;

	private Tilemap thisTilemap;

	private Tilemap thisTilemapCloneMinimap;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		InitializeValues();
	}

	private void InitializeValues()
	{
		thisTilemap = objTilemap.GetComponent<Tilemap>();
		crtDelayVisibilityChange = null;
		floatDelayTime = 0.1f;
		colorOriginal = Color.white;
		colorTemp = colorOriginal;
		colorAlphaTransparency = colorOriginal;
		colorAlphaTransparency.a = targetAlphaLevel;
		isPlayerInside = false;
		stateShow = true;
	}

	private void FixedUpdate()
	{
		if (stateShow && thisTilemap.color != colorOriginal)
		{
			colorTemp.a = Mathf.SmoothStep(thisTilemap.color.a, colorOriginal.a, 10f * Time.deltaTime);
			thisTilemap.color = colorTemp;
			FXU_ApplyChangeToAllSprites();
			UpdateMinimapCloneIfApplicable();
		}
		else if (!stateShow && thisTilemap.color != colorAlphaTransparency)
		{
			colorTemp.a = Mathf.SmoothStep(thisTilemap.color.a, colorAlphaTransparency.a, 10f * Time.deltaTime);
			thisTilemap.color = colorTemp;
			FXU_ApplyChangeToAllSprites();
			UpdateMinimapCloneIfApplicable();
		}
	}

	private void FXU_ApplyChangeToAllSprites()
	{
		if (arraySpriteRenderer != null && arraySpriteRenderer.Length != 0)
		{
			for (int i = 0; i < arraySpriteRenderer.Length; i++)
			{
				arraySpriteRenderer[i].color = colorTemp;
			}
		}
	}

	private void UpdateMinimapCloneIfApplicable()
	{
		if (GetTilemapCloneForMinimap())
		{
			colorMinimapClone = thisTilemapCloneMinimap.color;
			colorMinimapClone.a = thisTilemap.color.a;
			thisTilemapCloneMinimap.color = colorMinimapClone;
		}
	}

	private bool GetTilemapCloneForMinimap()
	{
		if (thisTilemapCloneMinimap == null)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("MinimapClone");
			if (array == null)
			{
				return false;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name.Contains("(Clone)") && array[i].name.Contains(objTilemap.name))
				{
					thisTilemapCloneMinimap = array[i].GetComponent<Tilemap>();
					return true;
				}
			}
			return false;
		}
		return true;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player")
		{
			isPlayerInside = true;
			VerifyCoroutineStart();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && !GC.playerIsBeingGrabed)
		{
			isPlayerInside = false;
			VerifyCoroutineStart();
		}
	}

	private void VerifyCoroutineStart()
	{
		if (crtDelayVisibilityChange == null)
		{
			crtDelayVisibilityChange = StartCoroutine(waitDelayVisibilityChange(floatDelayTime));
		}
	}

	private IEnumerator waitDelayVisibilityChange(float wait)
	{
		yield return new WaitForSeconds(wait);
		if (isPlayerInside)
		{
			stateShow = false;
		}
		else
		{
			stateShow = true;
		}
		crtDelayVisibilityChange = null;
	}
}
