// MKM Android crash diagnostic (P1f). Self-contained: creates its own
// DontDestroyOnLoad logger before any scene loads, records step markers +
// every Unity log (incl. exceptions) to persistentDataPath/MKMDiag.log and
// shows the last error on screen. All I/O is try/catch-guarded so the
// logger itself can never crash the game. Remove for release builds.
// P2k-probe: every 4s log live sprite renderer values (player + nearest 3
// enemies) so ghost-transparency can be root-caused from device instead of
// guessing in editor.
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MKMDiag : MonoBehaviour
{
	private static MKMDiag _inst;

	private static readonly object _lock = new object();

	private static string _path = "";

	private static List<string> _recent = new List<string>();

	private string _screenStep = "";

	private string _screenError = "";

	private float _nextProbe;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	private static void Boot()
	{
		try
		{
			GameObject go = new GameObject("[MKMDIAG]");
			UnityEngine.Object.DontDestroyOnLoad(go);
			_inst = go.AddComponent<MKMDiag>();
			_path = Path.Combine(Application.persistentDataPath, "MKMDiag.log");
			try { File.WriteAllText(_path, ""); } catch { }
			Step("BOOT platform=" + Application.platform + " unity=" + Application.unityVersion);
			Step("BOOT device=" + SystemInfo.deviceModel + " os=" + SystemInfo.operatingSystem + " ramMB=" + SystemInfo.systemMemorySize + " cpu=" + SystemInfo.processorType);
			Step("PROBE build=P2k-diag interval=4s (player+3 nearest enemies, live renderer values)");
			Application.logMessageReceived += OnLog;
			AppDomain.CurrentDomain.UnhandledException += delegate(object s, UnhandledExceptionEventArgs e)
			{
				Step("FATAL-CLR: " + ((e.ExceptionObject != null) ? e.ExceptionObject.ToString() : "(null)"));
			};
			SceneManager.sceneLoaded += delegate(Scene sc, LoadSceneMode m)
			{
				Step("SCENE-LOADED: " + sc.name + " mode=" + m);
			};
			SceneManager.activeSceneChanged += delegate(Scene from, Scene to)
			{
				Step("SCENE-ACTIVE: " + from.name + " -> " + to.name);
			};
		}
		catch
		{
		}
	}

	private static void OnLog(string msg, string trace, LogType type)
	{
		try
		{
			if (type == LogType.Exception || type == LogType.Error || type == LogType.Assert)
			{
				Step("LOG-" + type + ": " + msg + "\n" + trace);
				if (_inst != null)
				{
					_inst._screenError = msg.Length > 220 ? msg.Substring(0, 220) : msg;
				}
			}
		}
		catch
		{
		}
	}

	public static void Step(string s)
	{
		try
		{
			string line = DateTime.Now.ToString("HH:mm:ss.fff") + " " + s;
			lock (_lock)
			{
				_recent.Add(line);
				if (_recent.Count > 60)
				{
					_recent.RemoveAt(0);
				}
			}
			if (_path != "")
			{
				try { File.AppendAllText(_path, line + "\n"); } catch { }
			}
			if (_inst != null)
			{
				_inst._screenStep = s.Length > 160 ? s.Substring(0, 160) : s;
			}
		}
		catch
		{
		}
	}

	private void Update()
	{
		try
		{
			if (Time.time >= _nextProbe)
			{
				_nextProbe = Time.time + 4f;
				ProbeSprites();
			}
		}
		catch
		{
		}
	}

	private void ProbeSprites()
	{
		try
		{
			string scene = SceneManager.GetActiveScene().name;
			GameObject player = null;
			try { player = GameObject.FindGameObjectWithTag("Player"); } catch { }
			Vector3 pp = Vector3.zero;
			if (player != null) pp = player.transform.position;
			if (player != null)
				Step("PROBE scene=" + scene + " " + DescribeRenderer("PLAYER", player, pp));
			else
				Step("PROBE scene=" + scene + " PLAYER=(none)");
			EnemyGeneralLogic[] foes = null;
			try { foes = (EnemyGeneralLogic[])GameObject.FindObjectsOfType(typeof(EnemyGeneralLogic)); } catch { }
			if (foes == null || foes.Length == 0)
			{
				Step("PROBE scene=" + scene + " enemies=0");
				return;
			}
			// insertion-sort nearest 3 to player (few enemies on screen, no alloc concerns for diag)
			int n = Math.Min(3, foes.Length);
			for (int i = 0; i < foes.Length && i < 12; i++)
			{
				for (int j = i + 1; j < foes.Length && j < 12; j++)
				{
					if (foes[j] == null || foes[i] == null) continue;
					float di = (foes[i].transform.position - pp).sqrMagnitude;
					float dj = (foes[j].transform.position - pp).sqrMagnitude;
					if (dj < di) { EnemyGeneralLogic t = foes[i]; foes[i] = foes[j]; foes[j] = t; }
				}
			}
			Step("PROBE scene=" + scene + " enemies=" + foes.Length + " (nearest " + n + "):");
			for (int k = 0; k < n; k++)
			{
				if (foes[k] == null) continue;
				Step("PROBE " + DescribeRenderer("ENEMY", foes[k].gameObject, pp) + " etype=" + foes[k].currentEnemyType);
			}
		}
		catch (Exception e)
		{
			Step("PROBE-ERR " + e.GetType().Name + ": " + e.Message);
		}
	}

	private string DescribeRenderer(string tag, GameObject go, Vector3 refPos)
	{
		try
		{
			if (go == null) return tag + "=(null)";
			SpriteRenderer sr = go.GetComponent<SpriteRenderer>();
			if (sr == null) return tag + " " + go.name + " NO-SpriteRenderer";
			string s = tag + " name=" + go.name;
			s += " pos=" + go.transform.position.ToString("F2");
			try { s += " dist=" + Vector3.Distance(go.transform.position, refPos).ToString("F1"); } catch { }
			s += " en=" + sr.enabled + " srCol=" + Col(sr.color);
			s += " sort=" + sr.sortingOrder + " flipX=" + sr.flipX + " flipY=" + sr.flipY;
			s += " sprite=" + ((sr.sprite != null) ? sr.sprite.name : "(null)");
			Material m = null;
			try { m = sr.sharedMaterial; } catch { }
			if (m == null)
			{
				try { m = sr.material; } catch { }
				s += " matSrc=instance";
			}
			else s += " matSrc=shared";
			if (m == null) { s += " mat=(null)"; return s; }
			s += " mat=" + m.name;
			try { s += " shader=" + ((m.shader != null) ? m.shader.name : "(null)"); } catch { s += " shader=(err)"; }
			try
			{
				string[] kw = m.shaderKeywords;
				s += " kw=[" + ((kw != null && kw.Length > 0) ? string.Join(",", kw) : "-") + "]";
			}
			catch { }
			try
			{
				if (m.HasProperty("_Color")) s += " _Color=" + Col(m.GetColor("_Color"));
				if (m.HasProperty("_SpriteFade")) s += " _SpriteFade=" + m.GetFloat("_SpriteFade").ToString("F3");
				if (m.HasProperty("Pixel_Fade")) s += " Pixel_Fade=" + m.GetFloat("Pixel_Fade").ToString("F3");
				if (m.HasProperty("_FlashAmount")) s += " _Flash=" + m.GetFloat("_FlashAmount").ToString("F3");
				if (m.HasProperty("_Outline_Color_1")) s += " OutlineCol=" + Col(m.GetColor("_Outline_Color_1"));
				if (m.HasProperty("_Outline_Size_1")) s += " OutlineSz=" + m.GetFloat("_Outline_Size_1").ToString("F2");
			}
			catch (Exception e) { s += " matReadErr=" + e.GetType().Name; }
			return s;
		}
		catch (Exception e)
		{
			return tag + "-ERR " + e.GetType().Name;
		}
	}

	private string Col(Color c)
	{
		return "(" + c.r.ToString("F2") + "," + c.g.ToString("F2") + "," + c.b.ToString("F2") + "," + c.a.ToString("F2") + ")";
	}

	private void OnGUI()
	{
		try
		{
			GUI.color = Color.white;
			GUI.Label(new Rect(10, 10, Screen.width - 20, 40), "DIAG: " + _screenStep);
			if (_screenError != "")
			{
				GUI.color = Color.red;
				GUI.Label(new Rect(10, 52, Screen.width - 20, 120), "ERR: " + _screenError);
			}
		}
		catch
		{
		}
	}
}
