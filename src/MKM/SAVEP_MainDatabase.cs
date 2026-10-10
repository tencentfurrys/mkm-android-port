using System;
using UnityEngine;

[Serializable]
public class SAVEP_MainDatabase
{
	public int sceneGroup;

	public string sceneName;

	public int idSavePoint;

	public GameCore.SavePieceType typeOfPiece;

	public Vector2 positionOfPiece;
}
