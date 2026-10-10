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
		localPos = transform.localPosition;
		myWorldRotation = transform.rotation;
		myWorldRotation.x = 0f;
		myWorldRotation.y = 0f;
		myWorldRotation.z = 0f;
	}

	private void LateUpdate()
	{
		if (offsetLocalRightSide)
		{
			transform.localPosition = offsetPosRight;
		}
		else if (offsetLocalLeftSide)
		{
			transform.localPosition = offsetPosLeft;
		}
		else
		{
			transform.localPosition = localPos;
		}
		if (checkRotationLateUpdate && transform.rotation != myWorldRotation)
		{
			transform.rotation = myWorldRotation;
		}
	}
}
