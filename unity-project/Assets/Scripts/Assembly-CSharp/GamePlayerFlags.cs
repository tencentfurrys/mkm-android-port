using System;

[Serializable]
public class GamePlayerFlags
{
	public bool isForestBossDefeatd;

	public bool isBeachBossDefeated;

	public bool isHillBossDefeated;

	public bool isWerewolfBossDefeated;

	public bool isMinotaurBossDefeated;

	public bool isPrincessBossDefeated;

	public bool isWitchBossDefeated;

	public bool isBlueFoxDefeated;

	public bool hasUsedCastleSilverKey;

	public bool hasUsedCastleDungeonKey;

	public bool hasUsedHillValleyKey;

	public bool HasVisitedTheGallery;

	public bool isBossSwampDefeated;

	public bool isBossHighTechDefeated;

	public bool seenEventBeach01;

	public bool seenEventForest01;

	public bool seenEV_Farina01_meet1;

	public bool seenEV_Farina02_meet2;

	public bool seenEV_Farina03_giveRuby;

	public bool winGauntlet1;

	public GamePlayerFlags()
	{
		isForestBossDefeatd = false;
		isBeachBossDefeated = false;
		isHillBossDefeated = false;
		isWerewolfBossDefeated = false;
		isMinotaurBossDefeated = false;
		isPrincessBossDefeated = false;
		isWitchBossDefeated = false;
		isBlueFoxDefeated = false;
		hasUsedCastleSilverKey = false;
		hasUsedCastleDungeonKey = false;
		hasUsedHillValleyKey = false;
		HasVisitedTheGallery = false;
		isBossSwampDefeated = false;
		isBossHighTechDefeated = false;
		seenEventBeach01 = false;
		seenEventForest01 = false;
		seenEV_Farina01_meet1 = false;
		seenEV_Farina02_meet2 = false;
		seenEV_Farina03_giveRuby = false;
		winGauntlet1 = false;
	}

	public GamePlayerFlags(GamePlayerFlags copyValues)
	{
		isForestBossDefeatd = copyValues.isForestBossDefeatd;
		isBeachBossDefeated = copyValues.isBeachBossDefeated;
		isHillBossDefeated = copyValues.isHillBossDefeated;
		isWerewolfBossDefeated = copyValues.isWerewolfBossDefeated;
		isMinotaurBossDefeated = copyValues.isMinotaurBossDefeated;
		isPrincessBossDefeated = copyValues.isPrincessBossDefeated;
		isWitchBossDefeated = copyValues.isWitchBossDefeated;
		isBlueFoxDefeated = copyValues.isBlueFoxDefeated;
		hasUsedCastleSilverKey = copyValues.hasUsedCastleSilverKey;
		hasUsedCastleDungeonKey = copyValues.hasUsedCastleDungeonKey;
		hasUsedHillValleyKey = copyValues.hasUsedHillValleyKey;
		HasVisitedTheGallery = copyValues.HasVisitedTheGallery;
		isBossSwampDefeated = copyValues.isBossSwampDefeated;
		isBossHighTechDefeated = copyValues.isBossHighTechDefeated;
		seenEventBeach01 = copyValues.seenEventBeach01;
		seenEventForest01 = copyValues.seenEventForest01;
		seenEV_Farina01_meet1 = copyValues.seenEV_Farina01_meet1;
		seenEV_Farina02_meet2 = copyValues.seenEV_Farina02_meet2;
		seenEV_Farina03_giveRuby = copyValues.seenEV_Farina03_giveRuby;
		winGauntlet1 = copyValues.winGauntlet1;
	}
}
