using UnityEngine;

public class ColliderPerFrame : MonoBehaviour
{
	public BoxCollider2D thisCollider;

	public AttackColliderPerFrame[] listOfChanges;

	private int frameIndexCounter;

	public void ChangeCollider(string start)
	{
		if (start != "")
		{
			frameIndexCounter = 0;
		}
		if (listOfChanges != null && listOfChanges.Length != 0 && frameIndexCounter < listOfChanges.Length && isActiveAndEnabled)
		{
			AlterColliderPerFrame();
		}
	}

	private void AlterColliderPerFrame()
	{
		thisCollider.enabled = listOfChanges[frameIndexCounter].setActive;
		thisCollider.offset = listOfChanges[frameIndexCounter].setOffset;
		thisCollider.size = listOfChanges[frameIndexCounter].setSize;
		frameIndexCounter++;
	}
}
