using System;
using UnityEngine;

[Serializable]
public class ENM_CommonSkill
{
	public int baseSkillDamage;

	public int minSkillFrequency;

	public int maxSkillFrequency;

	public int skillPercentChance;

	public float minSkillProximityX;

	public float minSkillProximityY;

	public float SkillAnimDuration;

	public float SkillTimeToSpawn;

	public bool ignoreRoundTime;

	public bool ignoreCollisionOnExecution;

	public bool haltSkillOnPlayerFaint;

	public GameObject spawnOffsetEmptyObj;
}
