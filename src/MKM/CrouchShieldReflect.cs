using UnityEngine;

public class CrouchShieldReflect : MonoBehaviour
{
	public Transform parentRefScale;

	public GameObject objReflectedTrail;

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Projectile-Basic"))
		{
			ReflectEnemyProjectile(other.gameObject.GetComponent<ProjectileLogic>());
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Projectile-Basic"))
		{
			ReflectEnemyProjectile(other.gameObject.GetComponent<ProjectileLogic>());
		}
	}

	private void ReflectEnemyProjectile(ProjectileLogic refScript)
	{
		if (refScript.Originator == ProjectileLogic.OwnerOfProjectile.Enemy && (!(parentRefScale.localScale.x > 0f) || !(refScript.transform.position.x < parentRefScale.position.x)) && (!(parentRefScale.localScale.x < 0f) || !(refScript.transform.position.x > parentRefScale.position.x)))
		{
			refScript.ProjectileWasReflectedByPlayer(objReflectedTrail);
		}
	}
}
