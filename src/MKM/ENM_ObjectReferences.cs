using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class ENM_ObjectReferences
{
	public Transform StartPointCenterMass;

	public Transform GroundCheck1;

	public Transform GroundCheck2;

	public Transform GroundCheck3;

	public GameObject objStaggerVFX;

	public GameObject objProjectile;

	public GameObject objTextDamageUI;

	public GameObject localHPCanvas;

	public Image imgHealthFill;

	public Image imgHealthFrame;

	public Image imgHealthBG;

	public Sprite specialSprite;

	public GameObject deathSpawnEffect;

	public GameObject rageSpawnEffect;

	public Transform CeilingCheck1;

	public Transform CeilingCheck2;

	public Transform CeilingCheck3;

	public AnimatorOverrideController AnimOverrideCtrl1;
}
