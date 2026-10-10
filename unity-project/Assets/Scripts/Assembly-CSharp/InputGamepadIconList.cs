using UnityEngine;

[CreateAssetMenu(fileName = "New InputGamepadIcon List", menuName = "InputGamepadIcon List")]
public class InputGamepadIconList : ScriptableObject
{
	public Sprite iconKeyboard;

	public Sprite iconPSX;

	public Sprite iconXBOX;

	public UIGamepadIconList[] typePSX;

	public UIGamepadIconList[] typeXBOX;

	public int GetButtonTLID(string sentKeyCode, GameCore.InputType sentType)
	{
		UIGamepadIconList[] array = ReturnIconArray(sentType);
		int result = 0;
		if (array != null)
		{
			UIGamepadIconList[] array2 = array;
			foreach (UIGamepadIconList uIGamepadIconList in array2)
			{
				if (uIGamepadIconList.RawKeyCode == sentKeyCode)
				{
					result = uIGamepadIconList.TranslationID;
					break;
				}
			}
		}
		return result;
	}

	public Sprite GetIcon(string sentKeyCode, GameCore.InputType sentType)
	{
		UIGamepadIconList[] array = ReturnIconArray(sentType);
		Sprite result = null;
		if (array != null)
		{
			UIGamepadIconList[] array2 = array;
			foreach (UIGamepadIconList uIGamepadIconList in array2)
			{
				if (uIGamepadIconList.RawKeyCode == sentKeyCode)
				{
					result = uIGamepadIconList.ButtonIcon;
					break;
				}
			}
		}
		return result;
	}

	private UIGamepadIconList[] ReturnIconArray(GameCore.InputType type)
	{
		switch (type)
		{
		case GameCore.InputType.GPTypePSX:
			return typePSX;
		case GameCore.InputType.GPTypeXBOX:
			return typeXBOX;
		default:
			return null;
		}
	}
}
