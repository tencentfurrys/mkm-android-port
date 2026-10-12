// MKM Android touch state (P2). Frame-latched virtual input per InputLib:
// held persists while a touch button is down; down/up are true for exactly
// one frame (cleared in LateUpdate, which runs after every Update, so all
// gameplay polling sees a consistent picture regardless of script order).
// All methods are null/total safe so game code can call them unconditionally.
using UnityEngine;

public static class MKM_Touch
{
	private static readonly bool[] _held = new bool[32];

	private static readonly bool[] _down = new bool[32];

	private static readonly bool[] _up = new bool[32];

	private static int Idx(GameCore.InputLib lib)
	{
		int i = (int)lib;
		if (i < 0 || i >= 32)
		{
			return (int)GameCore.InputLib.Null;
		}
		return i;
	}

	public static void Press(GameCore.InputLib lib)
	{
		try
		{
			int i = Idx(lib);
			if (!_held[i])
			{
				_held[i] = true;
				_down[i] = true;
				_up[i] = false;
			}
		}
		catch
		{
		}
	}

	public static void Release(GameCore.InputLib lib)
	{
		try
		{
			int i = Idx(lib);
			if (_held[i])
			{
				_held[i] = false;
				_up[i] = true;
			}
			_down[i] = false;
		}
		catch
		{
		}
	}

	public static bool Held(GameCore.InputLib lib)
	{
		try
		{
			return _held[Idx(lib)];
		}
		catch
		{
			return false;
		}
	}

	public static bool Down(GameCore.InputLib lib)
	{
		try
		{
			return _down[Idx(lib)];
		}
		catch
		{
			return false;
		}
	}

	public static bool Up(GameCore.InputLib lib)
	{
		try
		{
			return _up[Idx(lib)];
		}
		catch
		{
			return false;
		}
	}

	public static bool Query(GameCore.InputLib lib, GameCore.InputPressType press)
	{
		try
		{
			switch (press)
			{
			case GameCore.InputPressType.KeyPressedOnce:
				return Down(lib);
			case GameCore.InputPressType.KeyHolding:
				return Held(lib);
			case GameCore.InputPressType.KeyReleased:
				return Up(lib);
			}
		}
		catch
		{
		}
		return false;
	}

	public static void ClearFrame()
	{
		try
		{
			for (int i = 0; i < 32; i++)
			{
				_down[i] = false;
				_up[i] = false;
			}
		}
		catch
		{
		}
	}

	public static void ResetAll()
	{
		try
		{
			for (int i = 0; i < 32; i++)
			{
				_held[i] = false;
				_down[i] = false;
				_up[i] = false;
			}
		}
		catch
		{
		}
	}
}
