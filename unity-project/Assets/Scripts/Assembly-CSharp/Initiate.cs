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
		GameObject obj = new GameObject
		{
			name = "Fader"
		};
		Canvas canvas = obj.AddComponent<Canvas>();
		canvas.renderMode = RenderMode.ScreenSpaceOverlay;
		canvas.sortingOrder = 1;
		obj.AddComponent<Fader>();
		obj.AddComponent<CanvasGroup>();
		obj.AddComponent<Image>();
		Fader component = obj.GetComponent<Fader>();
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
