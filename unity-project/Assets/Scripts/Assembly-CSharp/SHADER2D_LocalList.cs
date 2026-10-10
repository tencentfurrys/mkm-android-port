using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SHADER2D_LocalList
{
	public Material main2DShaderMaterial;

	public string mainVariableName;

	public AnimationCurve mainAnimationCurve;

	public bool clampCurve;

	public float mainSpeed;

	public float finalMultiplier;

	public List<SHADER2D_AdditionalFloats> subFloats;

	public List<SHADER2D_AdditionalColors> subColors;

	public SHADER2D_LocalList()
	{
		main2DShaderMaterial = null;
		mainVariableName = "";
		mainAnimationCurve = null;
		clampCurve = false;
		mainSpeed = 1f;
		finalMultiplier = 1f;
		subFloats = new List<SHADER2D_AdditionalFloats>();
		subColors = new List<SHADER2D_AdditionalColors>();
	}

	public SHADER2D_LocalList(SHADER2D_LocalList Copy)
	{
		main2DShaderMaterial = Copy.main2DShaderMaterial;
		mainVariableName = Copy.mainVariableName;
		mainAnimationCurve = Copy.mainAnimationCurve;
		clampCurve = Copy.clampCurve;
		mainSpeed = Copy.mainSpeed;
		finalMultiplier = Copy.finalMultiplier;
		subFloats = new List<SHADER2D_AdditionalFloats>(Copy.subFloats);
		subColors = new List<SHADER2D_AdditionalColors>(Copy.subColors);
	}
}
