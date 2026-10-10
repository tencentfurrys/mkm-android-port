using System;

[Serializable]
public class GamePlayerSkillFlags
{
	public bool hasDash;

	public bool hasSlidingTackle;

	public bool hasShield;

	public bool hasFlameSword;

	public bool hasForwardSlash;

	public bool hasGroundPound;

	public bool hasDrillDash;

	public GamePlayerSkillFlags()
	{
		hasDash = false;
		hasSlidingTackle = false;
		hasShield = false;
		hasFlameSword = false;
		hasForwardSlash = false;
		hasGroundPound = false;
		hasDrillDash = false;
	}

	public GamePlayerSkillFlags(GamePlayerSkillFlags copyValues)
	{
		hasDash = copyValues.hasDash;
		hasSlidingTackle = copyValues.hasSlidingTackle;
		hasShield = copyValues.hasShield;
		hasFlameSword = copyValues.hasFlameSword;
		hasForwardSlash = copyValues.hasForwardSlash;
		hasGroundPound = copyValues.hasGroundPound;
		hasDrillDash = copyValues.hasDrillDash;
	}
}
