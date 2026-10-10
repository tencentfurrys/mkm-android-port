using System;
using UnityEngine;

[Serializable]
public class ENM_Miscellaneuss
{
	public enum FlyerType
	{
		Null = 0,
		Agressive = 1,
		Defensive = 2,
		LanderTypeB = 3,
		HorizontalOnly = 4,
		AgressiveTypeB = 5,
		AgressiveTypeC = 6,
		WormDig1 = 7,
		Patroller = 8,
		AgressiveGallery = 9
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
