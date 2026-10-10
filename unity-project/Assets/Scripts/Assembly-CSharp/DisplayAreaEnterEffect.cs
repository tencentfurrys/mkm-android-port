using System.Collections;
using UnityEngine;

public class DisplayAreaEnterEffect : MonoBehaviour
{
	public enum AreaEffectType
	{
		BeachWater = 0,
		SewerWater = 1,
		SwampWater = 2,
		DungeonWater = 3
	}

	public AreaEffectType thisArea;

	private GameCore.VisualEffect enterVFXtype;

	private GameCore.VisualEffect exitVFXtype;

	private float amountPlayerEffectDelay;

	private bool canPlayPlayerEffect;

	private Vector2 posEffect;

	private BoxCollider2D objBox2D;

	private GameObject objEffectEnter;

	private GameObject objeffectExit;

	private Coroutine crtDelayPlayerEffect;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		canPlayPlayerEffect = true;
		crtDelayPlayerEffect = null;
		amountPlayerEffectDelay = 0.25f;
		posEffect = base.transform.position;
		InitializeAreaType();
	}

	private void InitializeAreaType()
	{
		objBox2D = GetComponent<BoxCollider2D>();
		switch (thisArea)
		{
		default:
			enterVFXtype = GameCore.VisualEffect.SplashWaterEnter;
			exitVFXtype = GameCore.VisualEffect.SplashWaterLeave;
			break;
		case AreaEffectType.SewerWater:
			enterVFXtype = GameCore.VisualEffect.SplashSewerEnter;
			exitVFXtype = GameCore.VisualEffect.SplashSewerEnter;
			break;
		case AreaEffectType.SwampWater:
			enterVFXtype = GameCore.VisualEffect.SplashSwampEnter;
			exitVFXtype = GameCore.VisualEffect.SplashSwampEnter;
			break;
		case AreaEffectType.DungeonWater:
			enterVFXtype = GameCore.VisualEffect.SplashWaterEnterDungeon;
			exitVFXtype = GameCore.VisualEffect.SplashWaterEnterDungeon;
			break;
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		switch (other.tag)
		{
		case "Player":
			if (CheckReferenceObjectEnter() && (bool)GC.refOBJEffectsParent && canPlayPlayerEffect && !GC.isPlayerGoneForGood)
			{
				StartPlayerEffectDelay();
				posEffect = objBox2D.bounds.ClosestPoint(other.gameObject.transform.position);
				posEffect.y = base.transform.position.y + objBox2D.size.y / 2f;
				Object.Instantiate(objEffectEnter, posEffect, Quaternion.identity, GC.refOBJEffectsParent.transform);
				PlaySoundEnter();
			}
			break;
		case "Lala-Atk-Hitbox":
			PlaySoundExit();
			break;
		case "Enemy-Basic":
		case "moveable-collider":
		case "Projectile-Basic":
			if (CheckReferenceObjectEnter() && (bool)GC.refOBJEffectsParent)
			{
				posEffect = objBox2D.bounds.ClosestPoint(other.bounds.ClosestPoint(base.transform.position));
				posEffect.y = base.transform.position.y + objBox2D.size.y / 2f;
				Object.Instantiate(objEffectEnter, posEffect, Quaternion.identity, GC.refOBJEffectsParent.transform);
				PlaySoundEnter();
			}
			break;
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		switch (other.tag)
		{
		case "Player":
			if (CheckReferenceObjectExit() && (bool)GC.refOBJEffectsParent && canPlayPlayerEffect && !GC.isPlayerInteractingLocked && !GC.isPlayerGoneForGood)
			{
				StartPlayerEffectDelay();
				posEffect = objBox2D.bounds.ClosestPoint(other.bounds.ClosestPoint(base.transform.position));
				posEffect.y = base.transform.position.y + objBox2D.size.y / 2f;
				Object.Instantiate(objeffectExit, posEffect, Quaternion.identity, GC.refOBJEffectsParent.transform);
				PlaySoundExit();
			}
			break;
		case "Enemy-Basic":
		case "moveable-collider":
			if (CheckReferenceObjectExit() && (bool)GC.refOBJEffectsParent)
			{
				posEffect = objBox2D.bounds.ClosestPoint(other.bounds.ClosestPoint(base.transform.position));
				posEffect.y = base.transform.position.y + objBox2D.size.y / 2f;
				Object.Instantiate(objeffectExit, posEffect, Quaternion.identity, GC.refOBJEffectsParent.transform);
				PlaySoundExit();
			}
			break;
		}
	}

	private bool CheckReferenceObjectEnter()
	{
		if ((bool)objEffectEnter)
		{
			return true;
		}
		objEffectEnter = GC.ReturnVisualEffectPrefab(enterVFXtype);
		if ((bool)objEffectEnter)
		{
			return true;
		}
		return false;
	}

	private bool CheckReferenceObjectExit()
	{
		if ((bool)objeffectExit)
		{
			return true;
		}
		objeffectExit = GC.ReturnVisualEffectPrefab(exitVFXtype);
		if ((bool)objeffectExit)
		{
			return true;
		}
		return false;
	}

	private void StartPlayerEffectDelay()
	{
		canPlayPlayerEffect = false;
		if (crtDelayPlayerEffect != null)
		{
			StopCoroutine(crtDelayPlayerEffect);
		}
		crtDelayPlayerEffect = StartCoroutine(waitForPlayerEffectDelay(amountPlayerEffectDelay));
	}

	private void PlaySoundEnter()
	{
		if (!GC.isPlayerFainted && !GC.playerIsBeingGrabed)
		{
			GC.PlaySFX(GameCore.SFXBasic1.WaterSplashEnter, 0, checkForDuplicity: true, posEffect, 0.75f, 1.25f);
		}
	}

	private void PlaySoundExit()
	{
		if (!GC.isPlayerFainted && !GC.playerIsBeingGrabed)
		{
			GC.PlaySFX(GameCore.SFXBasic1.WaterSplashExit, 0, checkForDuplicity: true, posEffect, 0.75f, 1.25f);
		}
	}

	private IEnumerator waitForPlayerEffectDelay(float wait)
	{
		yield return new WaitForSeconds(wait);
		canPlayPlayerEffect = true;
		crtDelayPlayerEffect = null;
	}
}
