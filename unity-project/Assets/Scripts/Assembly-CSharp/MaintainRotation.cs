using UnityEngine;

public class MaintainRotation : MonoBehaviour
{
	public bool checkRotationLateUpdate;

	public bool offsetLocalRightSide;

	public bool offsetLocalLeftSide;

	public Vector3 offsetPosLeft;

	public Vector3 offsetPosRight;

	private Quaternion myWorldRotation;

	private Vector3 localPos;

	private void Awake()
	{
		localPos = base.transform.localPosition;
		myWorldRotation = base.transform.rotation;
		myWorldRotation.x = 0f;
		myWorldRotation.y = 0f;
		myWorldRotation.z = 0f;
	}

	private void LateUpdate()
	{
		if (offsetLocalRightSide)
		{
			base.transform.localPosition = offsetPosRight;
		}
		else if (offsetLocalLeftSide)
		{
			base.transform.localPosition = offsetPosLeft;
		}
		else
		{
			base.transform.localPosition = localPos;
		}
		if (checkRotationLateUpdate && base.transform.rotation != myWorldRotation)
		{
			base.transform.rotation = myWorldRotation;
		}
	}
}
