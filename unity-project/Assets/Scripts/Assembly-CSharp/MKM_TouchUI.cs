// MKM Android touch controls UI (P2). Runtime-built (no scene edits):
// left diamond d-pad (Up/Down/Left/Right), right action cluster
// (ATK/JMP/INT/SKL/DEF/USE), Pause top-right. Parchment-and-bronze styling
// to match the game. Multi-touch safe (per-button pointer tracking).
// Visible on Android in gameplay scenes only; hidden in splash/menus/intro
// and while paused (those are tapped directly). Feeds MKM_Touch state.
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MKM_TouchUI : MonoBehaviour
{
	private class Btn : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		public GameCore.InputLib lib;

		public Image img;

		public Sprite normal;

		public Sprite pressed;

		private int activePointer = -10;

		public int ActivePointer => activePointer;

		public void OnPointerDown(PointerEventData e)
		{
			if (activePointer == -10)
			{
				activePointer = e.pointerId;
				MKM_Touch.Press(lib);
				if (img != null && pressed != null)
				{
					img.sprite = pressed;
				}
			}
		}

		public void OnPointerUp(PointerEventData e)
		{
			if (e.pointerId == activePointer)
			{
				activePointer = -10;
				MKM_Touch.Release(lib);
				if (img != null && normal != null)
				{
					img.sprite = normal;
				}
			}
		}

		public void ForceRelease()
		{
			if (activePointer != -10)
			{
				activePointer = -10;
				MKM_Touch.Release(lib);
				if (img != null && normal != null)
				{
					img.sprite = normal;
				}
			}
		}

		private void OnDisable()
		{
			ForceRelease();
		}
	}

	private static MKM_TouchUI _inst;

	private GameObject _root;

	private System.Collections.Generic.List<Btn> _btns = new System.Collections.Generic.List<Btn>();

	private static Sprite _circle;

	private static Sprite _circleHot;

	private static Font _font;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
	private static void Boot()
	{
		try
		{
			if (Application.platform != RuntimePlatform.Android)
			{
				return;
			}
			if (_inst != null)
			{
				return;
			}
			GameObject go = new GameObject("[MKM_TOUCHUI]");
			Object.DontDestroyOnLoad(go);
			_inst = go.AddComponent<MKM_TouchUI>();
			SceneManager.sceneLoaded += delegate(Scene sc, LoadSceneMode m)
			{
				if (_inst != null)
				{
					_inst.Refresh();
					_inst.ResetButtons();
				}
				MKM_Touch.ResetAll();
			};
			_inst.Build();
			_inst.Refresh();
		}
		catch
		{
		}
	}

	private void LateUpdate()
	{
		MKM_Touch.ClearFrame();
	}

	private void OnApplicationFocus(bool focus)
	{
		if (!focus)
		{
			ResetButtons();
			MKM_Touch.ResetAll();
		}
	}

	private void OnApplicationPause(bool paused)
	{
		if (paused)
		{
			ResetButtons();
			MKM_Touch.ResetAll();
		}
	}

	private void ResetButtons()
	{
		try
		{
			if (_btns != null)
			{
				for (int i = 0; i < _btns.Count; i++)
				{
					if (_btns[i] != null)
					{
						_btns[i].ForceRelease();
					}
				}
			}
		}
		catch
		{
		}
	}

	// A pointer stolen by another control (or a canvas hidden mid-press) never
	// fires OnPointerUp, latching held[Right]=true forever -> Maya auto-runs.
	// Each frame, verify every claimed pointer is still an active touch.
	private void ReapStalePointers()
	{
		try
		{
			if (_btns == null || _btns.Count == 0)
			{
				return;
			}
			bool hasTouches = Input.touchCount > 0;
			for (int i = 0; i < _btns.Count; i++)
			{
				Btn b = _btns[i];
				if (b == null || b.ActivePointer == -10)
				{
					continue;
				}
				int pid = b.ActivePointer;
				if (pid >= 0)
				{
					bool alive = false;
					if (hasTouches)
					{
						for (int t = 0; t < Input.touches.Length; t++)
						{
							Touch th = Input.touches[t];
							if (th.fingerId == pid && (th.phase == TouchPhase.Began || th.phase == TouchPhase.Moved || th.phase == TouchPhase.Stationary))
							{
								alive = true;
								break;
							}
						}
					}
					if (!alive)
					{
						b.ForceRelease();
					}
				}
				else
				{
					// Mouse / pen pointer (editor testing): release when button is up.
					if (!Input.GetMouseButton(0))
					{
						b.ForceRelease();
					}
				}
			}
		}
		catch
		{
		}
	}

	private float _nextVisCheck;

	private void Update()
	{
		ReapStalePointers();
		if (Time.time >= _nextVisCheck)
		{
			_nextVisCheck = Time.time + 0.25f;
			Refresh();
		}
	}

	private void Refresh()
	{
		try
		{
			if (_root == null)
			{
				return;
			}
			bool show = ShouldShow();
			if (_root.activeSelf != show)
			{
				_root.SetActive(show);
				if (!show)
				{
					MKM_Touch.ResetAll();
				}
			}
		}
		catch
		{
		}
	}

	private bool ShouldShow()
	{
		try
		{
			string sc = SceneManager.GetActiveScene().name;
			if (sc == "_InitialScreen" || sc == "MainMenu" || sc == "Intro-Story")
			{
				return false;
			}
			GameCore gc = GameCore.Instance;
			if (gc != null && gc.isGamePaused)
			{
				return false;
			}
			return true;
		}
		catch
		{
			return false;
		}
	}

	private void Build()
	{
		try
		{
			if (Object.FindObjectOfType<EventSystem>() == null)
			{
				GameObject es = new GameObject("MKM_EventSystem");
				Object.DontDestroyOnLoad(es);
				es.AddComponent<EventSystem>();
				es.AddComponent<StandaloneInputModule>();
			}
			_root = new GameObject("TouchCanvas");
			_root.transform.SetParent(base.transform, worldPositionStays: false);
			Canvas cv = _root.AddComponent<Canvas>();
			cv.renderMode = RenderMode.ScreenSpaceOverlay;
			cv.sortingOrder = 100;
			_root.AddComponent<CanvasScaler>();
			_root.AddComponent<GraphicRaycaster>();
			_circle = Circle(new Color(0.16f, 0.11f, 0.07f, 0.55f), new Color(0.85f, 0.72f, 0.47f, 0.9f));
			_circleHot = Circle(new Color(0.55f, 0.42f, 0.22f, 0.75f), new Color(1f, 0.9f, 0.65f, 1f));
			_font = Resources.GetBuiltinResource<Font>("Arial.ttf");
			float s = Mathf.Min((float)Screen.width / 1920f, (float)Screen.height / 1080f);
			s = Mathf.Clamp(s, 0.6f, 1.6f);
			float w = Screen.width;
			float h = Screen.height;
			float R = 92f * s;
			float cx = 250f * s;
			float cy = 250f * s;
			Add(GameCore.InputLib.Up, "▲", cx, cy + R * 1.15f, R);
			Add(GameCore.InputLib.Down, "▼", cx, cy - R * 1.15f, R);
			Add(GameCore.InputLib.Left, "◀", cx - R * 1.15f, cy, R);
			Add(GameCore.InputLib.Right, "▶", cx + R * 1.15f, cy, R);
			// Right cluster: 2 cols x 3 rows grid anchored bottom-right.
			// Old packing put INT and USE on near-identical centers and pushed
			// SKL off the right edge at 2340x1080. Grid spacing (180h x 170v)
			// exceeds max diameter sum (~160) so neighbours can't overlap,
			// and every center is clamped inside the screen.
			float col1 = w - 330f * s;
			float col2 = w - 150f * s;
			float row1 = 190f * s;
			float row2 = 360f * s;
			float row3 = 530f * s;
			float rBig = 88f * s;
			float rMed = 76f * s;
			float rSml = 68f * s;
			Add(GameCore.InputLib.Jump, "JMP", col1, row1, rMed);
			Add(GameCore.InputLib.BasicAttack, "ATK", col2, row1, rBig);
			Add(GameCore.InputLib.UseItemActive, "USE", col1, row2, rSml);
			Add(GameCore.InputLib.DefenseSkills, "DEF", col2, row2, rSml);
			Add(GameCore.InputLib.Interact, "INT", col1, row3, rSml);
			Add(GameCore.InputLib.AttackSkills, "SKL", col2, row3, rSml);
			// Pause was top-right under a game icon; move to top-left.
			GameObject pause = Add(GameCore.InputLib.Pause, "II", 90f * s, h - 90f * s, 62f * s);
			pause.name = "TouchPause";
			MKMDiag.Step("TOUCHUI built, scale=" + s + " w=" + w + " h=" + h);
		}
		catch
		{
		}
	}

	private GameObject Add(GameCore.InputLib lib, string label, float x, float y, float rad)
	{
		// Clamp every center inside the screen so no button can end up half
		// off-screen on wide/tall aspects (SKL was cut off at 2340x1080).
		float cx = Mathf.Clamp(x, rad, Mathf.Max(rad, (float)Screen.width - rad));
		float cy = Mathf.Clamp(y, rad, Mathf.Max(rad, (float)Screen.height - rad));
		GameObject go = new GameObject("Touch_" + lib);
		go.transform.SetParent(_root.transform, worldPositionStays: false);
		RectTransform rt = go.AddComponent<RectTransform>();
		rt.anchorMin = Vector2.zero;
		rt.anchorMax = Vector2.zero;
		rt.pivot = new Vector2(0.5f, 0.5f);
		rt.sizeDelta = new Vector2(rad * 2f, rad * 2f);
		rt.anchoredPosition = new Vector2(cx, cy);
		Image img = go.AddComponent<Image>();
		img.sprite = _circle;
		img.type = Image.Type.Simple;
		Btn b = go.AddComponent<Btn>();
		b.lib = lib;
		b.img = img;
		b.normal = _circle;
		b.pressed = _circleHot;
		if (_btns == null)
		{
			_btns = new System.Collections.Generic.List<Btn>();
		}
		_btns.Add(b);
		GameObject tx = new GameObject("Label");
		tx.transform.SetParent(go.transform, worldPositionStays: false);
		RectTransform rtt = tx.AddComponent<RectTransform>();
		rtt.anchorMin = Vector2.zero;
		rtt.anchorMax = Vector2.one;
		rtt.offsetMin = Vector2.zero;
		rtt.offsetMax = Vector2.zero;
		Text t = tx.AddComponent<Text>();
		t.text = label;
		t.font = _font;
		t.fontSize = Mathf.RoundToInt(rad * 0.42f);
		t.alignment = TextAnchor.MiddleCenter;
		t.color = new Color(0.96f, 0.89f, 0.74f, 0.95f);
		return go;
	}

	private static Sprite Circle(Color fill, Color rim)
	{
		int S = 128;
		Texture2D tex = new Texture2D(S, S, TextureFormat.ARGB32, false);
		tex.filterMode = FilterMode.Bilinear;
		Color clear = new Color(0f, 0f, 0f, 0f);
		for (int py = 0; py < S; py++)
		{
			for (int px = 0; px < S; px++)
			{
				float dx = (px - S / 2f + 0.5f) / (S / 2f);
				float dy = (py - S / 2f + 0.5f) / (S / 2f);
				float d = Mathf.Sqrt(dx * dx + dy * dy);
				if (d > 1f)
				{
					tex.SetPixel(px, py, clear);
				}
				else if (d > 0.86f)
				{
					tex.SetPixel(px, py, rim);
				}
				else
				{
					tex.SetPixel(px, py, fill);
				}
			}
		}
		tex.Apply();
		return Sprite.Create(tex, new Rect(0f, 0f, S, S), new Vector2(0.5f, 0.5f), 100f);
	}
}
