using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueLine
{
	public int id;

	public List<AnimAction> actionList = new List<AnimAction>();

	public List<SoundAction> soundList = new List<SoundAction>();

	public GameCore.SpeakerID letterTone;

	public Vector2 cameraMovement;

	public bool isInstantText;

	public Color speakerColorRGB;

	public string speakerName;

	public string speakerDialogue;

	public string miscText;
}
