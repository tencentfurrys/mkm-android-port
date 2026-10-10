using UnityEngine;

public class DemoScript : MonoBehaviour
{
	public string scene;

	public Color loadToColor = Color.white;

	public void GoFade()
	{
		Initiate.Fade(scene, loadToColor, 1f);
	}
}
