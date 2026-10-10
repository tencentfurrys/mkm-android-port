using UnityEngine;
using UnityEngine.UI;

public static class Initiate
{
	private static bool areWeFading;

	public static void Fade(string scene, Color col, float multiplier)
	{
		if (areWeFading)
		{
			Debug.Log("Already Fading");
			return;
		}
		GameObject gameObject = new GameObject
		{
			name = "Fader"
		};
		Canvas canvas = gameObject.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 1;
		gameObject.AddComponent<Fader>();
		gameObject.AddComponent<CanvasGroup>();
		gameObject.AddComponent<Image>();
		Fader component = gameObject.GetComponent<Fader>();
		component.fadeDamp = multiplier;
		component.fadeScene = scene;
		component.fadeColor = col;
		component.start = true;
		areWeFading = true;
		component.InitiateFader();
	}

	public static void DoneFading()
	{
		areWeFading = false;
	}
}
