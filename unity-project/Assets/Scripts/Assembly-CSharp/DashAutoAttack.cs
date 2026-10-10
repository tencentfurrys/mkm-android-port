using UnityEngine;

public class DashAutoAttack : MonoBehaviour
{
	public GameObject projectileAutoAttack;

	private GameCore GC;

	private int quantityAttacks = 2;

	private void Start()
	{
		GC = GameCore.Instance;
	}

	private void OnCollisionEnter2D(Collision2D other)
	{
		if (other.gameObject.CompareTag("Enemy-Boss") || other.gameObject.CompareTag("Enemy-Basic"))
		{
			SpawnSlashAutoAttack(other.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Enemy-Boss") || other.gameObject.CompareTag("Enemy-Basic"))
		{
			SpawnSlashAutoAttack(other.gameObject);
		}
	}

	private void SpawnSlashAutoAttack(GameObject obj)
	{
		Vector3 vector = base.transform.position;
		Vector3 vector2 = vector;
		EnemyGeneralLogic component = obj.GetComponent<EnemyGeneralLogic>();
		if (component != null)
		{
			vector = component.GetEnemyCenterPos();
		}
		for (int i = 0; i < quantityAttacks; i++)
		{
			vector2.x = vector.x + GC.MATH_RNG_FLOAT(-0.2f, 0.2f);
			vector2.y = vector.y + GC.MATH_RNG_FLOAT(-0.2f, 0.2f);
			Object.Instantiate(projectileAutoAttack, vector2, Quaternion.identity, GC.refOBJEffectsParent.transform);
			GC.InstantiateVFX(GameCore.VisualEffect.DashATKSlash, vector2, "DashAutoAttack");
		}
	}
}
