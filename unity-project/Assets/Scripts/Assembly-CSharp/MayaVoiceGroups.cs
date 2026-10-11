using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New MayaVoiceGroup List", menuName = "MayaVoiceGroup")]
public class MayaVoiceGroups : ScriptableObject
{
	public List<PresetMayaVoiceGroups> listOfMayaVoiceGroups;

	public AudioClip[] GetVoiceArrayOfGroupType(GameCore.MARange thisGroup)
	{
		return listOfMayaVoiceGroups.Find((PresetMayaVoiceGroups x) => x.groupType == thisGroup)?.voices;
	}
}
