using UnityEngine;

public class ShieldObjectMisc : MonoBehaviour
{
	public enum ShieldSound
	{
		Null = 0,
		Basic = 1,
		MinotaurBoss = 2
	}

	public int flatDamageReduce;

	public int dividedDamageReduce;

	public float doShakeIntensity;

	public float doShakeDecayRate;

	public ShieldSound playThisSFXOnDefense;

	private GameCore GC;

	private float tmpX;

	private float tmpY;

	private void Start()
	{
		GC = GameCore.Instance;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (CheckPlayerAttack(other.gameObject))
		{
			if (playThisSFXOnDefense != ShieldSound.Null)
			{
				PlayShieldHitSound();
			}
			if (doShakeIntensity > 0f && doShakeDecayRate > 0f)
			{
				base.transform.parent.GetComponent<EnemyGeneralLogic>().StartRotationShakeEffect(doShakeIntensity, doShakeDecayRate);
			}
		}
	}

	private void PlayShieldHitSound()
	{
		ShieldSound shieldSound = playThisSFXOnDefense;
		GameCore.SFXBasic2 whichSFX = ((shieldSound != ShieldSound.Basic && shieldSound == ShieldSound.MinotaurBoss) ? GameCore.SFXBasic2.MinotaurShield : GameCore.SFXBasic2.ShieldHit1);
		GC.PlaySFX(whichSFX, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f);
	}

	private bool CheckPlayerAttack(GameObject otherOBJ)
	{
		if (otherOBJ.tag == "Lala-Atk-Hitbox" || CheckPlayerProjectile(otherOBJ))
		{
			return true;
		}
		return false;
	}

	private bool CheckPlayerProjectile(GameObject otherOBJ)
	{
		if (otherOBJ.tag == "Projectile-Basic")
		{
			ProjectileLogic component = otherOBJ.GetComponent<ProjectileLogic>();
			if (component.Originator == ProjectileLogic.OwnerOfProjectile.Player && component.specialTag != "noHitDmg")
			{
				return true;
			}
		}
		return false;
	}

	private void OnDisable()
	{
		StopAllCoroutines();
	}
}
