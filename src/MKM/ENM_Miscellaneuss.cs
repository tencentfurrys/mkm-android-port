using System;
using UnityEngine;

[Serializable]
public class ENM_Miscellaneuss
{
	public enum FlyerType
	{
		Null,
		Agressive,
		Defensive,
		LanderTypeB,
		HorizontalOnly,
		AgressiveTypeB,
		AgressiveTypeC,
		WormDig1,
		Patroller,
		AgressiveGallery
	}

	public bool isFlyer;

	public FlyerType typeFlyerBehaviour;

	public bool flyerIgnoreGDistance;

	public float flyerZRotation;

	public float LandAgainDelay;

	public bool maintainCollisionOnDeath;

	public bool canBeAffectedBySpringBoard;

	public bool staticDeathLocation;

	public float rateHorizontalFriction;

	public float rateVerticalFriction;

	public GameObject[] Objects;

	public bool FlyerEroApplyGravity;

	public float flyerChaseMultiplier = 1f;

	public float flyerRetreatMultiplier = 1f;

	public bool isPlayerTransformed;

	public bool CanWalkWater;

	public bool ignoreSpriteFliping;

	public bool selfCollEnemyAttack;
}
