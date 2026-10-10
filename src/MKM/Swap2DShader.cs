using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Swap2DShader : MonoBehaviour
{
	public Material defaultMaterial;

	public List<SHADER2D_LocalList> localListOf2DShader;

	public bool activateEditorDebug;

	public int indexShaderDebug;

	private SHADER2D_LocalList _Ongoing;

	private SpriteRenderer _selfSpriteRenderer;

	private Material _currentSpecialMaterial;

	private bool _isApplyEffect;

	private float _timeSingle;

	public bool isCurrentlyApplyingShadeEffect => _isApplyEffect;

	private void Awake()
	{
		SetupBasic();
	}

	private void SetupBasic()
	{
		_selfSpriteRenderer = GetComponent<SpriteRenderer>();
		_timeSingle = 0f;
	}

	private bool CheckValidity()
	{
		if (_selfSpriteRenderer != null && localListOf2DShader != null && localListOf2DShader.Count > 0)
		{
			return true;
		}
		Debug.Log("Tried to start 'Swap2DShader' of " + gameObject.name + " but was unable to validate! Shader swaping aborted!");
		return false;
	}

	public void Toggle2DShader(bool stateON, int index)
	{
		if (!CheckValidity())
		{
			return;
		}
		if (stateON)
		{
			_Ongoing = new SHADER2D_LocalList(localListOf2DShader[index]);
			_selfSpriteRenderer.material = _Ongoing.main2DShaderMaterial;
			_currentSpecialMaterial = _selfSpriteRenderer.material;
			_timeSingle = 0f;
			if (_Ongoing.mainSpeed <= 0f)
			{
				_Ongoing.mainSpeed = 1f;
			}
			if (_Ongoing.finalMultiplier <= 0f)
			{
				_Ongoing.finalMultiplier = 1f;
			}
			ConfigureAdditionalValues();
			_isApplyEffect = true;
		}
		else
		{
			_isApplyEffect = false;
			_selfSpriteRenderer.material = defaultMaterial;
			_currentSpecialMaterial = null;
			_Ongoing = null;
		}
	}

	private void ConfigureAdditionalValues()
	{
		for (int i = 0; i < _Ongoing.subFloats.Count; i++)
		{
			_currentSpecialMaterial.SetFloat(_Ongoing.subFloats[i].name, _Ongoing.subFloats[i].value);
		}
		for (int j = 0; j < _Ongoing.subColors.Count; j++)
		{
			_currentSpecialMaterial.SetColor(_Ongoing.subColors[j].name, _Ongoing.subColors[j].value);
		}
	}

	private void Update()
	{
		if (!Application.isPlaying)
		{
			CheckEditorDebugMode();
		}
		if (_isApplyEffect && _currentSpecialMaterial != null && _Ongoing.mainVariableName != "" && _Ongoing.mainAnimationCurve != null)
		{
			UPDATE_ApplyLineLerpToShaderEffect();
		}
	}

	private void UPDATE_ApplyLineLerpToShaderEffect()
	{
		if (_Ongoing.clampCurve)
		{
			_currentSpecialMaterial.SetFloat(_Ongoing.mainVariableName, _Ongoing.mainAnimationCurve.Evaluate((_timeSingle += Time.deltaTime) * _Ongoing.mainSpeed) * _Ongoing.finalMultiplier);
		}
		else
		{
			_currentSpecialMaterial.SetFloat(_Ongoing.mainVariableName, _Ongoing.mainAnimationCurve.Evaluate(Time.time * _Ongoing.mainSpeed) * _Ongoing.finalMultiplier);
		}
	}

	private void CheckEditorDebugMode()
	{
		if (activateEditorDebug)
		{
			if (!_isApplyEffect)
			{
				SetupBasic();
				Toggle2DShader(stateON: true, indexShaderDebug);
			}
		}
		else if (_isApplyEffect)
		{
			Toggle2DShader(stateON: false, 0);
		}
	}
}
