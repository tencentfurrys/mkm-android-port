using UnityEngine;

public class IconsBubble : MonoBehaviour
{
	public string PlayAnimationName;

	private void OnEnable()
	{
		GetComponent<Animator>().Play(PlayAnimationName);
	}
}
