// MKM Android UI fitter (P2a). The ripped UI was designed for exactly 1920x1080
// with CanvasScaler set to Match-Width (match=0): on wider phone screens
// (e.g. 2340x1080) everything scales up and the top/bottom get cropped -
// buttons and labels end up half off-screen. Forcing Expand mode shows the
// whole design space on every aspect (extra room on the sides, nothing cut).
// Runtime hook so it covers all 33 scenes with no scene-file edits.
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MKMCanvasFit : MonoBehaviour
{
	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Boot()
	{
		try
		{
			SceneManager.sceneLoaded += OnScene;
			FitAll();
		}
		catch
		{
		}
	}

	private static void OnScene(Scene sc, LoadSceneMode mode)
	{
		FitAll();
	}

	public static void FitAll()
	{
		try
		{
			CanvasScaler[] scalers = Object.FindObjectsOfType<CanvasScaler>();
			for (int i = 0; i < scalers.Length; i++)
			{
				if (scalers[i] != null && scalers[i].uiScaleMode == CanvasScaler.ScaleMode.ScaleWithScreenSize && scalers[i].screenMatchMode != CanvasScaler.ScreenMatchMode.Expand)
				{
					scalers[i].screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
				}
			}
		}
		catch
		{
		}
	}
}
