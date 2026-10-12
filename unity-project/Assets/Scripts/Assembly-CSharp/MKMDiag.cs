// MKM Android crash diagnostic (P1f). Self-contained: creates its own
// DontDestroyOnLoad logger before any scene loads, records step markers +
// every Unity log (incl. exceptions) to persistentDataPath/MKMDiag.log and
// shows the last error on screen. All I/O is try/catch-guarded so the
// logger itself can never crash the game. Remove for release builds.
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
