using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Fader : MonoBehaviour
{
	[HideInInspector]
	public bool start;

	[HideInInspector]
	public float fadeDamp;

	[HideInInspector]
	public string fadeScene;

	[HideInInspector]
	public float alpha;

	[HideInInspector]
	public Color fadeColor;

	[HideInInspector]
	public bool isFadeIn;

	private CanvasGroup myCanvas;

	private Image bg;

	private float lastTime;

	private bool startedLoading;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoading;
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoading;
	}

	public void InitiateFader()
	{
		Object.DontDestroyOnLoad(gameObject);
		if ((bool)transform.GetComponent<CanvasGroup>())
		{
			myCanvas = transform.GetComponent<CanvasGroup>();
		}
		if ((bool)transform.GetComponentInChildren<Image>())
		{
			bg = transform.GetComponent<Image>();
			bg.color = fadeColor;
		}
		if ((bool)myCanvas && (bool)bg)
		{
			myCanvas.alpha = 0f;
			StartCoroutine(FadeIt());
		}
		else
		{
			Debug.LogWarning("Something is missing please reimport the package.");
		}
	}

	private IEnumerator FadeIt()
	{
		while (!start)
		{
			yield return null;
		}
		lastTime = Time.time;
		_ = lastTime;
		bool hasFadedIn = false;
		while (!hasFadedIn)
		{
			float delta = Time.time - lastTime;
			if (!isFadeIn)
			{
				alpha = newAlpha(delta, 1, alpha);
				if (alpha == 1f && !startedLoading)
				{
					startedLoading = true;
					SceneManager.LoadScene(fadeScene);
				}
			}
			else
			{
				alpha = newAlpha(delta, 0, alpha);
				if (alpha == 0f)
				{
					hasFadedIn = true;
				}
			}
			lastTime = Time.time;
			myCanvas.alpha = alpha;
			yield return null;
		}
		Initiate.DoneFading();
		Object.Destroy(gameObject);
		yield return null;
	}

	private float newAlpha(float delta, int to, float currAlpha)
	{
		switch (to)
		{
		case 0:
			currAlpha -= fadeDamp * delta;
			if (currAlpha <= 0f)
			{
				currAlpha = 0f;
			}
			break;
		case 1:
			currAlpha += fadeDamp * delta;
			if (currAlpha >= 1f)
			{
				currAlpha = 1f;
			}
			break;
		}
		return currAlpha;
	}

	private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
	{
		StartCoroutine(FadeIt());
		isFadeIn = true;
	}
}
