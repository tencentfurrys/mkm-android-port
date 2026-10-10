using System;
using UnityEngine;

[Serializable]
public class GamepadDirectionalStateControl
{
	public string axisNameVer;

	public string axisNameHor;

	public KeyCode reservedKey;

	public GameCore.GamepadDirection direction;

	public GameCore.InputPressType state;

	public GamepadDirectionalStateControl()
	{
		axisNameVer = "";
		axisNameHor = "";
		reservedKey = KeyCode.Asterisk;
		direction = GameCore.GamepadDirection.Neutral;
		state = GameCore.InputPressType.Null;
	}
}
