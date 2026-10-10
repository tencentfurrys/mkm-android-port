using UnityEngine;

[CreateAssetMenu(fileName = "New AudioClips List", menuName = "AudioClips List")]
public class AudioClipsList : ScriptableObject
{
	public ListOfMusicThemesAC listOfMusic;

	public ListOfSFXBasic1AC listOfSFXBasic1;

	public ListOfSFXBasic2AC listOfSFXBasic2;

	public ListOfEroSoundsAC listOfEroSound;

	public ListOfMayaVoiceAC listOfMayaVoice;

	public ListOfMonsterSoundsAC listOfMonsterSound;

	public ListOfMonsterSoundsACPart2 listOfMonsterSoundPart2;

	public ListOfMonsterSoundsACPart3 listOfMonsterSoundPart3;

	public ListOfMonsterSoundsACPart4 listOfMonsterSoundPart4;

	public ListOfForestBossSoundsAC listOfForestBossSound;

	public ListOfHillBossSoundsAC listOfHillBossSound;

	public ListOfWerewolfBossSoundsAC listOfWerewolfBossSound;

	public ListOfMinotaurBossSoundsAC listOfMinotaurBossSound;

	public ListOfPrincessBossSoundsAC listOfPrincessBossSound;

	public ListOfWitchBossSoundsAC listOfWitchBossSound;

	public ListOfBlueFoxBossSoundsAC listOfBlueFoxBossSound;

	public ListOfHarunaVoice listOfHarunaVoiceSound;

	private int rndNumber;

	public AudioClip ReturnMusicAudioClipByType(GameCore.MusicTheme thisType)
	{
		switch (thisType)
		{
		case GameCore.MusicTheme.TitleScreen:
			return listOfMusic.TitleScreenTheme;
		case GameCore.MusicTheme.IntroStory:
			return listOfMusic.IntroStoryTheme;
		case GameCore.MusicTheme.Beach1:
			return listOfMusic.StageBeach1;
		case GameCore.MusicTheme.Beach2:
			return listOfMusic.StageBeach2;
		case GameCore.MusicTheme.HubCastle1:
			return listOfMusic.StageHubCastle1;
		case GameCore.MusicTheme.CastleUpperFLoor:
			return listOfMusic.CastleUpperFLoor;
		case GameCore.MusicTheme.Forest1:
			return listOfMusic.StageForest1;
		case GameCore.MusicTheme.Fainted:
			return listOfMusic.defeatedFainted;
		case GameCore.MusicTheme.BossFight1:
			return listOfMusic.BossFight1;
		case GameCore.MusicTheme.VictoryFanfare1:
			return listOfMusic.VictoryFanfare1;
		case GameCore.MusicTheme.Valley1:
			return listOfMusic.StageValley1;
		case GameCore.MusicTheme.City1:
			return listOfMusic.StageCity1;
		case GameCore.MusicTheme.EroGrabbed:
			return listOfMusic.EroGrabbed;
		case GameCore.MusicTheme.UniqueItemFanfare1:
			return listOfMusic.UniqueItemFanfare1;
		case GameCore.MusicTheme.Cave1:
			return listOfMusic.Cave1;
		case GameCore.MusicTheme.Cave2:
			return listOfMusic.Cave2;
		case GameCore.MusicTheme.Cave3:
			return listOfMusic.Cave3;
		case GameCore.MusicTheme.DarkAmbience1:
			return listOfMusic.DarkAmbience1;
		case GameCore.MusicTheme.Swamp1:
			return listOfMusic.Swamp1;
		case GameCore.MusicTheme.Dungeon:
			return listOfMusic.Dungeon;
		case GameCore.MusicTheme.HighTech1:
			return listOfMusic.HighTech1;
		case GameCore.MusicTheme.BossFight2:
			return listOfMusic.BossFight2;
		case GameCore.MusicTheme.HighTech2:
			return listOfMusic.HighTech2;
		case GameCore.MusicTheme.BadEndTheme:
			return listOfMusic.BadEndTheme;
		case GameCore.MusicTheme.BossFight3:
			return listOfMusic.BossFight3;
		default:
			return null;
		}
	}

	public AudioClip ReturnSFXBasic1AudioClipByType(GameCore.SFXBasic1 thisType)
	{
		switch (thisType)
		{
		case GameCore.SFXBasic1.NormalSwordHit1:
		case GameCore.SFXBasic1.NormalSwordHit2:
		case GameCore.SFXBasic1.NormalSwordHit3:
			rndNumber = GetMATH_RNG_INT(1, 3);
			if (rndNumber == 2)
			{
				return listOfSFXBasic1.NormalSwordHit2;
			}
			if (rndNumber == 3)
			{
				return listOfSFXBasic1.NormalSwordHit3;
			}
			return listOfSFXBasic1.NormalSwordHit1;
		case GameCore.SFXBasic1.SpawnerNoise1:
			return listOfSFXBasic1.SpawnerNoise1;
		case GameCore.SFXBasic1.Swing1:
		case GameCore.SFXBasic1.Swing2:
			rndNumber = GetMATH_RNG_INT(1, 2);
			if (rndNumber == 2)
			{
				return listOfSFXBasic1.Swing2;
			}
			return listOfSFXBasic1.Swing1;
		case GameCore.SFXBasic1.SwingTK1:
			return listOfSFXBasic1.SwingTK1;
		case GameCore.SFXBasic1.SwingTK2:
			return listOfSFXBasic1.SwingTK2;
		case GameCore.SFXBasic1.SwingTK3:
			return listOfSFXBasic1.SwingTK3;
		case GameCore.SFXBasic1.SwingTKRandom:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.SwingTK1;
			case 2:
				return listOfSFXBasic1.SwingTK2;
			default:
				return listOfSFXBasic1.SwingTK3;
			}
		case GameCore.SFXBasic1.DialogueNext:
			return listOfSFXBasic1.DialogueNext;
		case GameCore.SFXBasic1.DialogueNext1:
			return listOfSFXBasic1.DialogueNext1;
		case GameCore.SFXBasic1.DialogueNext2:
			return listOfSFXBasic1.DialogueNext2;
		case GameCore.SFXBasic1.DialogueEnd:
			return listOfSFXBasic1.DialogueEnd;
		case GameCore.SFXBasic1.BarrelDestroyed1:
			return listOfSFXBasic1.BarrelDestroyed1;
		case GameCore.SFXBasic1.BarrelHit1:
			return listOfSFXBasic1.BarrelHit1;
		case GameCore.SFXBasic1.SkillDash:
			return listOfSFXBasic1.SkillDash;
		case GameCore.SFXBasic1.QuartzGet:
			return listOfSFXBasic1.QuartzGet;
		case GameCore.SFXBasic1.PotionGet:
			return listOfSFXBasic1.PotionGet;
		case GameCore.SFXBasic1.WaterSplashEnter:
			return listOfSFXBasic1.WaterSplashEnter;
		case GameCore.SFXBasic1.WaterSplashExit:
			return listOfSFXBasic1.WaterSplashExit;
		case GameCore.SFXBasic1.PunchGrabRelease:
			return listOfSFXBasic1.PunchGrabRelease;
		case GameCore.SFXBasic1.JumpStart:
			return listOfSFXBasic1.JumpStart;
		case GameCore.SFXBasic1.JumpStartRNG:
			rndNumber = GetMATH_RNG_INT(1, 2);
			if (rndNumber == 2)
			{
				return listOfSFXBasic1.JumpStart2;
			}
			return listOfSFXBasic1.JumpStart;
		case GameCore.SFXBasic1.JumpEnd:
			return listOfSFXBasic1.JumpEnd;
		case GameCore.SFXBasic1.JumpEnd2:
			return listOfSFXBasic1.JumpEnd2;
		case GameCore.SFXBasic1.JumpEndRNG:
			rndNumber = GetMATH_RNG_INT(1, 2);
			if (rndNumber == 2)
			{
				return listOfSFXBasic1.JumpEnd2;
			}
			return listOfSFXBasic1.JumpEnd;
		case GameCore.SFXBasic1.HitHurtSpike1:
			return listOfSFXBasic1.HitHurtSpike1;
		case GameCore.SFXBasic1.HitHurtSpike2:
			return listOfSFXBasic1.HitHurtSpike2;
		case GameCore.SFXBasic1.HitHurtCrush1:
			return listOfSFXBasic1.HitHurtCrush1;
		case GameCore.SFXBasic1.HitHurtTouch1:
			return listOfSFXBasic1.HitHurtTouch1;
		case GameCore.SFXBasic1.HitHurtTouch2:
			return listOfSFXBasic1.HitHurtTouch2;
		case GameCore.SFXBasic1.HitHurtTouch3:
			return listOfSFXBasic1.HitHurtTouch3;
		case GameCore.SFXBasic1.HitHurtTouch4:
			return listOfSFXBasic1.HitHurtTouch4;
		case GameCore.SFXBasic1.Armorbreak1:
			return listOfSFXBasic1.ArmorBreak1;
		case GameCore.SFXBasic1.ClothRip1:
			return listOfSFXBasic1.ClothRip1;
		case GameCore.SFXBasic1.LaserShoot1:
			return listOfSFXBasic1.LaserShoot1;
		case GameCore.SFXBasic1.LeverStart:
			return listOfSFXBasic1.LeverStart;
		case GameCore.SFXBasic1.LeverEnd:
			return listOfSFXBasic1.LeverEnd;
		case GameCore.SFXBasic1.GateOpening:
			return listOfSFXBasic1.GateOpening;
		case GameCore.SFXBasic1.GateClosed:
			return listOfSFXBasic1.GateClosed;
		case GameCore.SFXBasic1.uiEquipItem:
			return listOfSFXBasic1.uiEquipItem;
		case GameCore.SFXBasic1.uiUnequipItem:
			return listOfSFXBasic1.uiUnequipItem;
		case GameCore.SFXBasic1.uiWrongError:
			return listOfSFXBasic1.uiWrongError;
		case GameCore.SFXBasic1.Falling1:
			return listOfSFXBasic1.Falling1;
		case GameCore.SFXBasic1.Coconut1:
			return listOfSFXBasic1.Coconut1;
		case GameCore.SFXBasic1.KitsuneCast1:
			return listOfSFXBasic1.KitsuneCast1;
		case GameCore.SFXBasic1.Fast1:
			return listOfSFXBasic1.Fast1;
		case GameCore.SFXBasic1.Pickup1:
			return listOfSFXBasic1.Pickup1;
		case GameCore.SFXBasic1.CrystalOn:
			return listOfSFXBasic1.CrystalOn;
		case GameCore.SFXBasic1.CrystalOff:
			return listOfSFXBasic1.CrystalOff;
		case GameCore.SFXBasic1.CrystalEngaged:
			return listOfSFXBasic1.CrystalEngaged;
		case GameCore.SFXBasic1.Bounce:
			return listOfSFXBasic1.Bounce;
		case GameCore.SFXBasic1.CrystalDisengaged:
			return listOfSFXBasic1.CrystalDisengaged;
		case GameCore.SFXBasic1.PowerupCollected:
			return listOfSFXBasic1.PowerupCollected;
		case GameCore.SFXBasic1.Defend1:
			return listOfSFXBasic1.Defend1;
		case GameCore.SFXBasic1.Bite1:
			return listOfSFXBasic1.Bite1;
		case GameCore.SFXBasic1.UnlockGate:
			return listOfSFXBasic1.UnlockGate;
		case GameCore.SFXBasic1.EnterMirror:
			return listOfSFXBasic1.EnterMirror;
		case GameCore.SFXBasic1.Shoot1:
			return listOfSFXBasic1.Shoot1;
		case GameCore.SFXBasic1.Tornado:
			return listOfSFXBasic1.Tornado;
		case GameCore.SFXBasic1.Cloud:
			return listOfSFXBasic1.Cloud;
		case GameCore.SFXBasic1.Thunder:
			return listOfSFXBasic1.Thunder;
		case GameCore.SFXBasic1.DrillDash:
			return listOfSFXBasic1.DrillDash;
		case GameCore.SFXBasic1.FallSword:
			return listOfSFXBasic1.FallSword;
		case GameCore.SFXBasic1.playerTP_UP:
			return listOfSFXBasic1.playerTP_UP;
		case GameCore.SFXBasic1.playerTP_DOWN:
			return listOfSFXBasic1.playerTP_DOWN;
		case GameCore.SFXBasic1.CycleItemNext:
			return listOfSFXBasic1.CycleItemNext;
		case GameCore.SFXBasic1.CycleItemPrevious:
			return listOfSFXBasic1.CycleItemPrevious;
		case GameCore.SFXBasic1.HeartIconSpawn:
			return listOfSFXBasic1.HeartIconSpawn;
		case GameCore.SFXBasic1.DiagToneGroup1:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_1a;
			case 2:
				return listOfSFXBasic1.Diag_1b;
			default:
				return listOfSFXBasic1.Diag_1c;
			}
		case GameCore.SFXBasic1.DiagToneGroup2:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_2a;
			case 2:
				return listOfSFXBasic1.Diag_2b;
			default:
				return listOfSFXBasic1.Diag_2c;
			}
		case GameCore.SFXBasic1.DiagToneGroup3:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_3a;
			case 2:
				return listOfSFXBasic1.Diag_3b;
			default:
				return listOfSFXBasic1.Diag_3c;
			}
		case GameCore.SFXBasic1.DiagToneGroup4:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_4a;
			case 2:
				return listOfSFXBasic1.Diag_4b;
			default:
				return listOfSFXBasic1.Diag_4c;
			}
		case GameCore.SFXBasic1.DiagToneGroup5:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_5a;
			case 2:
				return listOfSFXBasic1.Diag_5b;
			default:
				return listOfSFXBasic1.Diag_5c;
			}
		case GameCore.SFXBasic1.DiagToneGroup6:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_6a;
			case 2:
				return listOfSFXBasic1.Diag_6b;
			default:
				return listOfSFXBasic1.Diag_6c;
			}
		case GameCore.SFXBasic1.DiagToneGroup7:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_7a;
			case 2:
				return listOfSFXBasic1.Diag_7b;
			default:
				return listOfSFXBasic1.Diag_7c;
			}
		case GameCore.SFXBasic1.DiagToneGroup8:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_8a;
			case 2:
				return listOfSFXBasic1.Diag_8b;
			default:
				return listOfSFXBasic1.Diag_8c;
			}
		case GameCore.SFXBasic1.DiagToneGroup9:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_9a;
			case 2:
				return listOfSFXBasic1.Diag_9b;
			default:
				return listOfSFXBasic1.Diag_9c;
			}
		case GameCore.SFXBasic1.DiagToneGroup10:
			switch (GetMATH_RNG_INT(1, 8, "DIAGwrite"))
			{
			case 1:
				return listOfSFXBasic1.Diag_10a;
			case 2:
				return listOfSFXBasic1.Diag_10b;
			case 3:
				return listOfSFXBasic1.Diag_10c;
			case 4:
				return listOfSFXBasic1.Diag_10d;
			case 5:
				return listOfSFXBasic1.Diag_10e;
			case 6:
				return listOfSFXBasic1.Diag_10f;
			case 7:
				return listOfSFXBasic1.Diag_10g;
			default:
				return listOfSFXBasic1.Diag_10h;
			}
		case GameCore.SFXBasic1.DiagToneGroup11:
			switch (GetMATH_RNG_INT(1, 3))
			{
			case 1:
				return listOfSFXBasic1.Diag_11a;
			case 2:
				return listOfSFXBasic1.Diag_11b;
			default:
				return listOfSFXBasic1.Diag_11c;
			}
		default:
			return null;
		}
	}

	public AudioClip ReturnSFXBasic2AudioClipByType(GameCore.SFXBasic2 thisType)
	{
		switch (thisType)
		{
		case GameCore.SFXBasic2.BossFightStart:
			return listOfSFXBasic2.BossBattleStart;
		case GameCore.SFXBasic2.BossDeathExplode:
			return listOfSFXBasic2.BossDeathExplode;
		case GameCore.SFXBasic2.Weird1:
			return listOfSFXBasic2.Weird1;
		case GameCore.SFXBasic2.itemDrinkPotion1:
			return listOfSFXBasic2.itemDrinkPotion1;
		case GameCore.SFXBasic2.Odd1:
			return listOfSFXBasic2.Odd1;
		case GameCore.SFXBasic2.Odd2:
			return listOfSFXBasic2.Odd2;
		case GameCore.SFXBasic2.Odd3:
			return listOfSFXBasic2.Odd3;
		case GameCore.SFXBasic2.Odd4:
			return listOfSFXBasic2.Odd4;
		case GameCore.SFXBasic2.Odd5:
			return listOfSFXBasic2.Odd5;
		case GameCore.SFXBasic2.Odd6:
			return listOfSFXBasic2.Odd6;
		case GameCore.SFXBasic2.Odd7:
			return listOfSFXBasic2.Odd7;
		case GameCore.SFXBasic2.Odd8:
			return listOfSFXBasic2.Odd8;
		case GameCore.SFXBasic2.Throw1:
			return listOfSFXBasic2.Throw1;
		case GameCore.SFXBasic2.Throw2:
			return listOfSFXBasic2.Throw2;
		case GameCore.SFXBasic2.Throw3:
			return listOfSFXBasic2.Throw3;
		case GameCore.SFXBasic2.Hit1:
			return listOfSFXBasic2.Hit1;
		case GameCore.SFXBasic2.Explosion1:
			return listOfSFXBasic2.Explosion1;
		case GameCore.SFXBasic2.Swing1:
			return listOfSFXBasic2.Swing1;
		case GameCore.SFXBasic2.Splash1:
			return listOfSFXBasic2.Splash1;
		case GameCore.SFXBasic2.RockSmash:
			return listOfSFXBasic2.RockSmash;
		case GameCore.SFXBasic2.GroundPoundLonger:
			return listOfSFXBasic2.GroundPoundLonger;
		case GameCore.SFXBasic2.SwitchStart:
			return listOfSFXBasic2.SwitchStart;
		case GameCore.SFXBasic2.SwitchEnd:
			return listOfSFXBasic2.SwitchEnd;
		case GameCore.SFXBasic2.DoorOpen:
			return listOfSFXBasic2.DoorOpen;
		case GameCore.SFXBasic2.DoorClose:
			return listOfSFXBasic2.DoorClose;
		case GameCore.SFXBasic2.HatchOpen:
			return listOfSFXBasic2.HatchOpen;
		case GameCore.SFXBasic2.ShieldHit1:
			return listOfSFXBasic2.ShieldHit1;
		case GameCore.SFXBasic2.Broken1:
			return listOfSFXBasic2.Broken1;
		case GameCore.SFXBasic2.Dizzy1:
			return listOfSFXBasic2.Dizzy1;
		case GameCore.SFXBasic2.Steps1:
			return listOfSFXBasic2.Steps1;
		case GameCore.SFXBasic2.Boing1:
			return listOfSFXBasic2.Boing1;
		case GameCore.SFXBasic2.Boing2:
			return listOfSFXBasic2.Boing2;
		case GameCore.SFXBasic2.Boing3:
			return listOfSFXBasic2.Boing3;
		case GameCore.SFXBasic2.Boing4:
			return listOfSFXBasic2.Boing4;
		case GameCore.SFXBasic2.BoingRandom:
			switch (GetMATH_RNG_INT(1, 4))
			{
			case 1:
				return listOfSFXBasic2.Boing1;
			case 2:
				return listOfSFXBasic2.Boing2;
			case 3:
				return listOfSFXBasic2.Boing3;
			default:
				return listOfSFXBasic2.Boing4;
			}
		case GameCore.SFXBasic2.BoingRandom2:
		{
			int mATH_RNG_INT = GetMATH_RNG_INT(2, 3);
			if (mATH_RNG_INT == 2)
			{
				return listOfSFXBasic2.Boing2;
			}
			return listOfSFXBasic2.Boing3;
		}
		case GameCore.SFXBasic2.UniqueItemAppear1:
			return listOfSFXBasic2.UniqueItemAppear1;
		case GameCore.SFXBasic2.SkillAppear1:
			return listOfSFXBasic2.SkillAppear1;
		case GameCore.SFXBasic2.ChestOpen1:
			return listOfSFXBasic2.ChestOpen1;
		case GameCore.SFXBasic2.PlatPowerOn:
			return listOfSFXBasic2.PlatPowerOn;
		case GameCore.SFXBasic2.PlatPowerOff:
			return listOfSFXBasic2.PlatPowerOff;
		case GameCore.SFXBasic2.SkillSlidingTackle:
			return listOfSFXBasic2.SkillSlidingTackle;
		case GameCore.SFXBasic2.Bump1:
			return listOfSFXBasic2.Bump1;
		case GameCore.SFXBasic2.LamiaSkillCast:
			return listOfSFXBasic2.LamiaSkillCast;
		case GameCore.SFXBasic2.LamiaSkillAppear:
			return listOfSFXBasic2.LamiaSkillAppear;
		case GameCore.SFXBasic2.Swing2:
			return listOfSFXBasic2.Swing2;
		case GameCore.SFXBasic2.ElectricSurge:
			return listOfSFXBasic2.ElectricSurge;
		case GameCore.SFXBasic2.Transformation1:
			return listOfSFXBasic2.Transformation1;
		case GameCore.SFXBasic2.LowExplosion1:
			return listOfSFXBasic2.LowExplosion1;
		case GameCore.SFXBasic2.Wooden1:
			return listOfSFXBasic2.Wooden1;
		case GameCore.SFXBasic2.Wooden2:
			return listOfSFXBasic2.Wooden2;
		case GameCore.SFXBasic2.Swing3:
			return listOfSFXBasic2.Swing3;
		case GameCore.SFXBasic2.SkillNotEnoughSP:
			return listOfSFXBasic2.SkillNotEnoughSP;
		case GameCore.SFXBasic2.SkillSlashSpawn:
			return listOfSFXBasic2.SkillSlashSpawn;
		case GameCore.SFXBasic2.SkillSlashHit:
			return listOfSFXBasic2.SkillSlashHit;
		case GameCore.SFXBasic2.SkillFlameCast:
			return listOfSFXBasic2.SkillFlameCast;
		case GameCore.SFXBasic2.SkillShieldCast:
			return listOfSFXBasic2.SkillShieldCast;
		case GameCore.SFXBasic2.SkillShieldDissipates:
			return listOfSFXBasic2.SkillShieldDissipates;
		case GameCore.SFXBasic2.Swing4:
			return listOfSFXBasic2.Swing4;
		case GameCore.SFXBasic2.Grow1:
			return listOfSFXBasic2.Grow1;
		case GameCore.SFXBasic2.Ripping1:
			return listOfSFXBasic2.Ripping1;
		case GameCore.SFXBasic2.Tearing1:
			return listOfSFXBasic2.Tearing1;
		case GameCore.SFXBasic2.Swing5:
			return listOfSFXBasic2.Swing5;
		case GameCore.SFXBasic2.GroundPoundLight:
			return listOfSFXBasic2.GroundPoundLight;
		case GameCore.SFXBasic2.MinotaurShield:
			return listOfSFXBasic2.MinotaurShield;
		case GameCore.SFXBasic2.Crystalize:
			return listOfSFXBasic2.Crystalize;
		case GameCore.SFXBasic2.Heal1:
			return listOfSFXBasic2.Heal1;
		case GameCore.SFXBasic2.GateClose:
			return listOfSFXBasic2.GateClose;
		case GameCore.SFXBasic2.Cursor1:
			return listOfSFXBasic2.Cursor1;
		case GameCore.SFXBasic2.MayaClothTransform:
			return listOfSFXBasic2.MayaClothTransform;
		case GameCore.SFXBasic2.EquipCloth:
			return listOfSFXBasic2.EquipCloth;
		case GameCore.SFXBasic2.EssenceSmall:
			return listOfSFXBasic2.EssenceSmall;
		case GameCore.SFXBasic2.EssenceMedium:
			return listOfSFXBasic2.EssenceMedium;
		case GameCore.SFXBasic2.EssenceBig:
			return listOfSFXBasic2.EssenceBig;
		case GameCore.SFXBasic2.GauntletFanfare:
			return listOfSFXBasic2.GauntletFanfare;
		default:
			return null;
		}
	}

	public AudioClip ReturnMonsterAudioClipByType(GameCore.MonsterSounds thisType)
	{
		switch (thisType)
		{
		case GameCore.MonsterSounds.IsoboraSkill:
			return listOfMonsterSound.IsoboraSkill;
		case GameCore.MonsterSounds.IsoboraGrab:
			return listOfMonsterSound.IsoboraGrab;
		case GameCore.MonsterSounds.IsoboraDead:
			return listOfMonsterSound.IsoboraDead;
		case GameCore.MonsterSounds.SpitBudDead:
			return listOfMonsterSound.SpitBudDead;
		case GameCore.MonsterSounds.StarfishGrab:
			return listOfMonsterSound.StarfishGrab;
		case GameCore.MonsterSounds.StarfishDead:
			return listOfMonsterSound.StarfishDead;
		case GameCore.MonsterSounds.BarnacleDead:
			return listOfMonsterSound.BarnacleDead;
		case GameCore.MonsterSounds.KitsuneGiggle:
			return listOfMonsterSound.KitsuneGiggle;
		case GameCore.MonsterSounds.KitsuneDead:
			return listOfMonsterSound.KitsuneDead;
		case GameCore.MonsterSounds.DFLYDead:
			return listOfMonsterSound.DFLYDead;
		case GameCore.MonsterSounds.DFLYSkill:
			return listOfMonsterSound.DFLYSkill;
		case GameCore.MonsterSounds.FairyDead:
			return listOfMonsterSound.FairyDead;
		case GameCore.MonsterSounds.FairyLaugh:
			return listOfMonsterSound.FairyLaugh;
		case GameCore.MonsterSounds.FairySkill:
			return listOfMonsterSound.FairySkill;
		case GameCore.MonsterSounds.SalaTongue:
			return listOfMonsterSound.SalaTongue;
		case GameCore.MonsterSounds.LizardDead:
			return listOfMonsterSound.LizardDead;
		case GameCore.MonsterSounds.MimicHowl:
			return listOfMonsterSound.MimicHowl;
		case GameCore.MonsterSounds.MimicJump:
			return listOfMonsterSound.MimicJump;
		case GameCore.MonsterSounds.MimicLand:
			return listOfMonsterSound.MimicLand;
		case GameCore.MonsterSounds.ArachneLaugh:
			return listOfMonsterSound.ArachneLaugh;
		case GameCore.MonsterSounds.ArachneClimax:
			return listOfMonsterSound.ArachneClimax;
		case GameCore.MonsterSounds.LamiaCast1:
			return listOfMonsterSound.LamiaCast1;
		case GameCore.MonsterSounds.LamiaCast2:
			return listOfMonsterSound.LamiaCast2;
		case GameCore.MonsterSounds.LamiaAttack1:
			return listOfMonsterSound.LamiaAttack1;
		case GameCore.MonsterSounds.LamiaAttack2:
			return listOfMonsterSound.LamiaAttack2;
		case GameCore.MonsterSounds.LamiaDead:
			return listOfMonsterSound.LamiaDead;
		case GameCore.MonsterSounds.LamiaOpenMouth:
			return listOfMonsterSound.LamiaOpenMouth;
		case GameCore.MonsterSounds.MushroomGirlLaugh:
			return listOfMonsterSound.MushroomGirlLaugh;
		case GameCore.MonsterSounds.MushroomGirlDeath:
			return listOfMonsterSound.MushroomGirlDeath;
		case GameCore.MonsterSounds.MushroomGirlSkillA:
			return listOfMonsterSound.MushroomGirlSkillA;
		case GameCore.MonsterSounds.MushroomGirlSkillB:
			return listOfMonsterSound.MushroomGirlSkillB;
		case GameCore.MonsterSounds.BatCall:
			return listOfMonsterSound.BatCall;
		case GameCore.MonsterSounds.BatSwoop:
			return listOfMonsterSound.BatSwoop;
		case GameCore.MonsterSounds.BatWings:
			return listOfMonsterSound.BatWings;
		case GameCore.MonsterSounds.BatDead:
			return listOfMonsterSound.BatDead;
		case GameCore.MonsterSounds.BatAhh:
			return listOfMonsterSound.BatAhh;
		case GameCore.MonsterSounds.FutaGhostRing:
			return listOfMonsterSoundPart2.FutaGhostRing;
		case GameCore.MonsterSounds.FutaGhostRing2:
			return listOfMonsterSoundPart2.FutaGhostRing2;
		case GameCore.MonsterSounds.FTG_Cast1:
			return listOfMonsterSoundPart2.FTG_Cast1;
		case GameCore.MonsterSounds.FTG_Cast2:
			return listOfMonsterSoundPart2.FTG_Cast2;
		case GameCore.MonsterSounds.FTG_Cast3:
			return listOfMonsterSoundPart2.FTG_Cast3;
		case GameCore.MonsterSounds.FTG_Dead:
			return listOfMonsterSoundPart2.FTG_Dead;
		case GameCore.MonsterSounds.FTG_Hurt:
			return listOfMonsterSoundPart2.FTG_Hurt;
		case GameCore.MonsterSounds.FTG_Cast_Random:
			switch (Random.Range(0, 3))
			{
			default:
				return listOfMonsterSoundPart2.FTG_Cast1;
			case 1:
				return listOfMonsterSoundPart2.FTG_Cast2;
			case 2:
				return listOfMonsterSoundPart2.FTG_Cast3;
			}
		case GameCore.MonsterSounds.SwampFairy_Cast:
			return listOfMonsterSoundPart2.SwampFairy_Cast;
		case GameCore.MonsterSounds.SwampFairy_Caw:
			return listOfMonsterSoundPart2.SwampFairy_Caw;
		case GameCore.MonsterSounds.SwampFairy_Laugh:
			return listOfMonsterSoundPart2.SwampFairy_Laugh;
		case GameCore.MonsterSounds.SwampFairy_Dead:
			return listOfMonsterSoundPart2.SwampFairy_Dead;
		case GameCore.MonsterSounds.SwampFairyThunder:
			return listOfMonsterSoundPart2.SwampFairy_Thunder;
		case GameCore.MonsterSounds.MothSkillCast:
			return listOfMonsterSoundPart2.MothSkillCast;
		case GameCore.MonsterSounds.MothSkillExplode:
			return listOfMonsterSoundPart2.MothSkillExplode;
		case GameCore.MonsterSounds.MothDead:
			return listOfMonsterSoundPart2.MothDead;
		case GameCore.MonsterSounds.MothCasting:
			return listOfMonsterSoundPart2.MothCasting;
		case GameCore.MonsterSounds.DwarfGrunt:
			return listOfMonsterSoundPart2.DwarfGrunt;
		case GameCore.MonsterSounds.DwarfDeath:
			return listOfMonsterSoundPart2.DwarfDeath;
		case GameCore.MonsterSounds.CTG_Disdain:
			return listOfMonsterSoundPart2.CTG_Disdain;
		case GameCore.MonsterSounds.CTG_Snort:
			return listOfMonsterSoundPart2.CTG_Snort;
		case GameCore.MonsterSounds.CTG_PreCHarge:
			return listOfMonsterSoundPart2.CTG_PreCHarge;
		case GameCore.MonsterSounds.CTG_Gallop:
			return listOfMonsterSoundPart2.CTG_Gallop;
		case GameCore.MonsterSounds.CTG_Dead:
			return listOfMonsterSoundPart2.CTG_Dead;
		case GameCore.MonsterSounds.GSB_CastRandom:
			return listOfMonsterSoundPart2.GSB_Cast2;
		case GameCore.MonsterSounds.GSB_Grunt1:
			return listOfMonsterSoundPart2.GSB_Grunt1;
		case GameCore.MonsterSounds.GSB_Grunt2:
			return listOfMonsterSoundPart2.GSB_Grunt2;
		case GameCore.MonsterSounds.Crystal_Cast:
			return listOfMonsterSoundPart2.Crystal_Cast;
		case GameCore.MonsterSounds.Crystal_Shoot:
			return listOfMonsterSoundPart2.Crystal_Shoot;
		case GameCore.MonsterSounds.Crystal_Chase:
			return listOfMonsterSoundPart2.Crystal_Chase;
		case GameCore.MonsterSounds.Crystal_Dead:
			return listOfMonsterSoundPart2.Crystal_Dead;
		case GameCore.MonsterSounds.Brute1:
			return listOfMonsterSoundPart2.Brute1;
		case GameCore.MonsterSounds.Brute2:
			return listOfMonsterSoundPart2.Brute2;
		case GameCore.MonsterSounds.PaintingEvil_Appear:
			return listOfMonsterSoundPart3.PaintingEvil_Appear;
		case GameCore.MonsterSounds.PaintingEvil_Splat:
			return listOfMonsterSoundPart3.PaintingEvil_Splat;
		case GameCore.MonsterSounds.LatexMaiden_Open:
			return listOfMonsterSoundPart3.LatexMaiden_Open;
		case GameCore.MonsterSounds.LatexMaiden_Close:
			return listOfMonsterSoundPart3.LatexMaiden_Close;
		case GameCore.MonsterSounds.LatexMaiden_Dead:
			return listOfMonsterSoundPart3.LatexMaiden_Dead;
		case GameCore.MonsterSounds.Domme_Skill:
			return listOfMonsterSoundPart3.Domme_Skill;
		case GameCore.MonsterSounds.Domme_Crack:
			return listOfMonsterSoundPart3.Domme_Crack;
		case GameCore.MonsterSounds.Domme_Dead:
			return listOfMonsterSoundPart3.Domme_Dead;
		case GameCore.MonsterSounds.Domme_Dodge:
			return listOfMonsterSoundPart3.Domme_Dodge;
		case GameCore.MonsterSounds.Domme_Swing:
			return listOfMonsterSoundPart3.Domme_Swing;
		case GameCore.MonsterSounds.CityDogSkill:
			return listOfMonsterSoundPart3.CityDogSkill;
		case GameCore.MonsterSounds.CityDogBark:
			return listOfMonsterSoundPart3.CityDogBark;
		case GameCore.MonsterSounds.CityDogPanting:
			return listOfMonsterSoundPart3.CityDogPanting;
		case GameCore.MonsterSounds.CityDogGroaning:
			return listOfMonsterSoundPart3.CityDogGroaning;
		case GameCore.MonsterSounds.CityDogBabyBark:
			return listOfMonsterSoundPart3.CityDogBabyBark;
		case GameCore.MonsterSounds.CityDogBabyWhine:
			return listOfMonsterSoundPart3.CityDogBabyWhine;
		case GameCore.MonsterSounds.BunnyCast:
			return listOfMonsterSoundPart3.BunnyCast;
		case GameCore.MonsterSounds.BunnyDead:
			return listOfMonsterSoundPart3.BunnyDead;
		case GameCore.MonsterSounds.PirateCast:
			return listOfMonsterSoundPart3.PirateCast;
		case GameCore.MonsterSounds.PirateSpin:
			return listOfMonsterSoundPart3.PirateSpin;
		case GameCore.MonsterSounds.PirateDead:
			return listOfMonsterSoundPart3.PirateDead;
		case GameCore.MonsterSounds.WormCast:
			return listOfMonsterSoundPart3.WormCast;
		case GameCore.MonsterSounds.WormDead:
			return listOfMonsterSoundPart3.WormDead;
		case GameCore.MonsterSounds.WormDigDown:
			return listOfMonsterSoundPart3.WormDigDown;
		case GameCore.MonsterSounds.WormDigUp:
			return listOfMonsterSoundPart3.WormDigUp;
		case GameCore.MonsterSounds.WormMelee:
			return listOfMonsterSoundPart3.WormMelee;
		case GameCore.MonsterSounds.RobotAlarm1:
			return listOfMonsterSoundPart3.Robot_Alarm_1;
		case GameCore.MonsterSounds.RobotAlarm2:
			return listOfMonsterSoundPart3.Robot_Alarm_2;
		case GameCore.MonsterSounds.RobotAlarm3:
			return listOfMonsterSoundPart3.Robot_Alarm_3;
		case GameCore.MonsterSounds.RobotSkill1:
			return listOfMonsterSoundPart3.Robot_Skill_1;
		case GameCore.MonsterSounds.RobotSkill2:
			return listOfMonsterSoundPart3.Robot_Skill_2;
		case GameCore.MonsterSounds.RobotSkill3:
			return listOfMonsterSoundPart3.Robot_Skill_3;
		case GameCore.MonsterSounds.RobotExplode:
			return listOfMonsterSoundPart3.Robot_Explode;
		case GameCore.MonsterSounds.WyvernCast:
			return listOfMonsterSoundPart3.WyvernCast;
		case GameCore.MonsterSounds.WyvernSkill:
			return listOfMonsterSoundPart3.WyvernSkill;
		case GameCore.MonsterSounds.WyvernDead:
			return listOfMonsterSoundPart3.WyvernDead;
		case GameCore.MonsterSounds.WyvernBirth:
			return listOfMonsterSoundPart3.WyvernBirth;
		case GameCore.MonsterSounds.BeeGirlCast:
			return listOfMonsterSoundPart3.BeeGirlCast;
		case GameCore.MonsterSounds.BeeGirlChase:
			return listOfMonsterSoundPart3.BeeGirlChase;
		case GameCore.MonsterSounds.BeeGirlDead:
			return listOfMonsterSoundPart3.BeeGirlDead;
		case GameCore.MonsterSounds.FutaCentaurHeal:
			return listOfMonsterSoundPart3.FutaCentaurHeal;
		case GameCore.MonsterSounds.FutaCentaurAttack:
			return listOfMonsterSoundPart3.FutaCentaurAttack;
		case GameCore.MonsterSounds.FutaCentaurDead:
			return listOfMonsterSoundPart3.FutaCentaurDead;
		case GameCore.MonsterSounds.BIS_Cast:
			return listOfMonsterSoundPart3.BIS_Cast;
		case GameCore.MonsterSounds.BIS_Skill:
			return listOfMonsterSoundPart3.BIS_Skill;
		case GameCore.MonsterSounds.BIS_Dead:
			return listOfMonsterSoundPart3.BIS_Dead;
		case GameCore.MonsterSounds.FLR_Cast:
			return listOfMonsterSoundPart3.FLR_Cast;
		case GameCore.MonsterSounds.FLR_Pound:
			return listOfMonsterSoundPart3.FLR_Pound;
		case GameCore.MonsterSounds.FLR_Dead:
			return listOfMonsterSoundPart3.FLR_Dead;
		case GameCore.MonsterSounds.FHB_Tentacles:
			return listOfMonsterSoundPart3.FHB_Tentacles;
		case GameCore.MonsterSounds.FHB_Dead:
			return listOfMonsterSoundPart3.FHB_Dead;
		case GameCore.MonsterSounds.CRB_Cast:
			return listOfMonsterSoundPart3.CRB_Cast;
		case GameCore.MonsterSounds.CRB_Skill:
			return listOfMonsterSoundPart3.CRB_Skill;
		case GameCore.MonsterSounds.CRB_Dead:
			return listOfMonsterSoundPart3.CRB_Dead;
		case GameCore.MonsterSounds.SCB_Cast:
			return listOfMonsterSoundPart4.Sinccubus_Cast;
		case GameCore.MonsterSounds.SCB_Skill:
			return listOfMonsterSoundPart4.Sinccubus_Skill;
		case GameCore.MonsterSounds.SCB_Chase:
			return listOfMonsterSoundPart4.Sinccubus_Chase;
		case GameCore.MonsterSounds.SCB_Dead:
			return listOfMonsterSoundPart4.Sinccubus_Dead;
		case GameCore.MonsterSounds.DKD_Cast:
			return listOfMonsterSoundPart4.DarkDamsel_Cast;
		case GameCore.MonsterSounds.DKD_Skill:
			return listOfMonsterSoundPart4.DarkDamsel_Skill;
		case GameCore.MonsterSounds.DKD_Dead:
			return listOfMonsterSoundPart4.DarkDamsel_Dead;
		case GameCore.MonsterSounds.MDS_Cast:
			return listOfMonsterSoundPart4.Medusa_Casting;
		case GameCore.MonsterSounds.MDS_Petr:
			return listOfMonsterSoundPart4.Medusa_PetrifyingDebuff;
		case GameCore.MonsterSounds.MDS_Stiff:
			return listOfMonsterSoundPart4.Medusa_StiffDebuff;
		case GameCore.MonsterSounds.MDS_Dead:
			return listOfMonsterSoundPart4.Medusa_Dead;
		case GameCore.MonsterSounds.FairySorrowDead:
			return listOfMonsterSoundPart4.FairySorrowDead;
		case GameCore.MonsterSounds.GoblinLaugh:
			return listOfMonsterSoundPart4.GoblinLaugh;
		case GameCore.MonsterSounds.CatHiss:
			return listOfMonsterSoundPart4.CatHiss;
		case GameCore.MonsterSounds.CatNyaOther:
			return listOfMonsterSoundPart4.CatNyaOther;
		case GameCore.MonsterSounds.CatNya1:
			return listOfMonsterSoundPart4.CatNya1;
		case GameCore.MonsterSounds.CatNya2:
			return listOfMonsterSoundPart4.CatNya2;
		case GameCore.MonsterSounds.CatNya3:
			return listOfMonsterSoundPart4.CatNya3;
		case GameCore.MonsterSounds.CatNya4:
			return listOfMonsterSoundPart4.CatNya4;
		case GameCore.MonsterSounds.CatNya5:
			return listOfMonsterSoundPart4.CatNya5;
		default:
			return null;
		}
	}

	public AudioClip ReturnEroAudioClipByType(GameCore.EroSound thisType)
	{
		switch (thisType)
		{
		case GameCore.EroSound.Piston1:
			return listOfEroSound.Piston1;
		case GameCore.EroSound.Piston2:
			return listOfEroSound.Piston2;
		case GameCore.EroSound.Piston3:
			return listOfEroSound.Piston3;
		case GameCore.EroSound.Piston4:
			return listOfEroSound.Piston4;
		case GameCore.EroSound.Suck1:
			return listOfEroSound.Suck1;
		case GameCore.EroSound.Suck2:
			return listOfEroSound.Suck2;
		case GameCore.EroSound.Suck3:
			return listOfEroSound.Suck3;
		case GameCore.EroSound.Suck4:
			return listOfEroSound.Suck4;
		case GameCore.EroSound.Suck5:
			return listOfEroSound.Suck5;
		case GameCore.EroSound.SuckRandom:
			rndNumber = GetMATH_RNG_INT(1, 4);
			if (rndNumber == 1)
			{
				return listOfEroSound.Suck1;
			}
			if (rndNumber == 2)
			{
				return listOfEroSound.Suck2;
			}
			if (rndNumber == 3)
			{
				return listOfEroSound.Suck3;
			}
			return listOfEroSound.Suck5;
		case GameCore.EroSound.Lick1:
			return listOfEroSound.Lick1;
		case GameCore.EroSound.Lick2:
			return listOfEroSound.Lick2;
		case GameCore.EroSound.Lick3:
			return listOfEroSound.Lick3;
		case GameCore.EroSound.LickRandom:
			rndNumber = GetMATH_RNG_INT(2, 3);
			if (rndNumber == 2)
			{
				return listOfEroSound.Lick2;
			}
			return listOfEroSound.Lick3;
		case GameCore.EroSound.Swallow1:
			return listOfEroSound.Swallow1;
		case GameCore.EroSound.Swallow2:
			return listOfEroSound.Swallow2;
		case GameCore.EroSound.Swallow3:
			return listOfEroSound.Swallow3;
		case GameCore.EroSound.Swallow4:
			return listOfEroSound.Swallow4;
		case GameCore.EroSound.Swallow5:
			return listOfEroSound.Swallow5;
		case GameCore.EroSound.SwallowRandom:
			rndNumber = GetMATH_RNG_INT(1, 3);
			if (rndNumber == 1)
			{
				return listOfEroSound.Swallow1;
			}
			if (rndNumber == 2)
			{
				return listOfEroSound.Swallow2;
			}
			return listOfEroSound.Swallow3;
		case GameCore.EroSound.SwallowRandom2:
			rndNumber = GetMATH_RNG_INT(1, 3);
			if (rndNumber == 1)
			{
				return listOfEroSound.Swallow4;
			}
			if (rndNumber == 2)
			{
				return listOfEroSound.Swallow5;
			}
			return listOfEroSound.Vore1;
		case GameCore.EroSound.Cum1:
			return listOfEroSound.Cum1;
		case GameCore.EroSound.Cum2:
			return listOfEroSound.Cum2;
		case GameCore.EroSound.Cum3:
			return listOfEroSound.Cum3;
		case GameCore.EroSound.Cum4:
			return listOfEroSound.Cum4;
		case GameCore.EroSound.Cum5:
			return listOfEroSound.Cum5;
		case GameCore.EroSound.Cum6:
			return listOfEroSound.Cum6;
		case GameCore.EroSound.Pull1:
			return listOfEroSound.Pull1;
		case GameCore.EroSound.Vore1:
			return listOfEroSound.Vore1;
		case GameCore.EroSound.Vore2:
			return listOfEroSound.Vore2;
		case GameCore.EroSound.Vore3:
			return listOfEroSound.Vore3;
		case GameCore.EroSound.Vore4:
			return listOfEroSound.Vore4;
		case GameCore.EroSound.VoreRandomA:
			rndNumber = GetMATH_RNG_INT(1, 3);
			if (rndNumber == 1)
			{
				return listOfEroSound.Cum2;
			}
			if (rndNumber == 2)
			{
				return listOfEroSound.Cum3;
			}
			return listOfEroSound.Vore1;
		case GameCore.EroSound.VoreRandomB:
			rndNumber = GetMATH_RNG_INT(1, 3);
			if (rndNumber == 1)
			{
				return listOfEroSound.Vore2;
			}
			if (rndNumber == 2)
			{
				return listOfEroSound.Vore3;
			}
			return listOfEroSound.Vore4;
		case GameCore.EroSound.Touch1:
			return listOfEroSound.Touch1;
		case GameCore.EroSound.Touch2:
			return listOfEroSound.Touch2;
		case GameCore.EroSound.Touch3:
			return listOfEroSound.Touch3;
		case GameCore.EroSound.Slap1:
			return listOfEroSound.Slap1;
		case GameCore.EroSound.Slap2:
			return listOfEroSound.Slap2;
		case GameCore.EroSound.Contact1:
			return listOfEroSound.Contact1;
		case GameCore.EroSound.Kiss1:
			return listOfEroSound.Kiss1;
		case GameCore.EroSound.Kiss2:
			return listOfEroSound.Kiss2;
		case GameCore.EroSound.Kiss3:
			return listOfEroSound.Kiss3;
		case GameCore.EroSound.Kiss4:
			return listOfEroSound.Kiss4;
		case GameCore.EroSound.KissRandom:
			rndNumber = GetMATH_RNG_INT(1, 4);
			if (rndNumber == 1)
			{
				return listOfEroSound.Kiss1;
			}
			if (rndNumber == 2)
			{
				return listOfEroSound.Kiss2;
			}
			if (rndNumber == 3)
			{
				return listOfEroSound.Kiss3;
			}
			return listOfEroSound.Kiss4;
		case GameCore.EroSound.Wet1:
			return listOfEroSound.Wet1;
		case GameCore.EroSound.Wet2:
			return listOfEroSound.Wet2;
		case GameCore.EroSound.Wet3:
			return listOfEroSound.Wet3;
		case GameCore.EroSound.Wet4:
			return listOfEroSound.Wet4;
		case GameCore.EroSound.Wet5:
			return listOfEroSound.Wet5;
		case GameCore.EroSound.Wet6:
			return listOfEroSound.Wet6;
		case GameCore.EroSound.WetRandom:
			rndNumber = GetMATH_RNG_INT(1, 3);
			if (rndNumber == 1)
			{
				return listOfEroSound.Wet1;
			}
			if (rndNumber == 2)
			{
				return listOfEroSound.Wet2;
			}
			return listOfEroSound.Wet3;
		case GameCore.EroSound.Bubble1:
			return listOfEroSound.Bubble1;
		case GameCore.EroSound.Bubble2:
			return listOfEroSound.Bubble2;
		case GameCore.EroSound.Bubble3:
			return listOfEroSound.Bubble3;
		case GameCore.EroSound.Bubble4:
			return listOfEroSound.Bubble4;
		case GameCore.EroSound.Bubble5:
			return listOfEroSound.Bubble5;
		case GameCore.EroSound.BubbleRandom:
			rndNumber = GetMATH_RNG_INT(1, 3);
			if (rndNumber == 1)
			{
				return listOfEroSound.Bubble1;
			}
			if (rndNumber == 2)
			{
				return listOfEroSound.Bubble2;
			}
			return listOfEroSound.Bubble3;
		case GameCore.EroSound.Rubber1:
			return listOfEroSound.Rubber1;
		case GameCore.EroSound.Rubber2:
			return listOfEroSound.Rubber2;
		case GameCore.EroSound.Rubber3:
			return listOfEroSound.Rubber3;
		case GameCore.EroSound.Rubber4:
			return listOfEroSound.Rubber4;
		case GameCore.EroSound.Rubber5:
			return listOfEroSound.Rubber5;
		case GameCore.EroSound.Rubber6:
			return listOfEroSound.Rubber6;
		case GameCore.EroSound.Rubber7:
			return listOfEroSound.Rubber7;
		case GameCore.EroSound.RubberRandom:
			rndNumber = GetMATH_RNG_INT(1, 7);
			switch (rndNumber)
			{
			case 1:
				return listOfEroSound.Rubber1;
			case 2:
				return listOfEroSound.Rubber2;
			case 3:
				return listOfEroSound.Rubber3;
			case 4:
				return listOfEroSound.Rubber4;
			case 5:
				return listOfEroSound.Rubber5;
			case 6:
				return listOfEroSound.Rubber6;
			default:
				return listOfEroSound.Rubber7;
			}
		case GameCore.EroSound.HeartBeat1:
			return listOfEroSound.HeartBeat1;
		default:
			return null;
		}
	}

	public AudioClip ReturnMayaVoiceAudioClipByType(GameCore.MayaVoice thisType)
	{
		string text = thisType.ToString();
		string text2 = "";
		for (int i = 0; i < text.Length; i++)
		{
			if (!char.IsNumber(text, i))
			{
				text2 += text.Substring(i, 1);
			}
		}
		switch (text2)
		{
		case "BJLight":
			return RETURN_MV_BJLIGHT(thisType);
		case "BJMedium":
			return RETURN_MV_BJMEDIUM(thisType);
		case "BJHeavy":
			return RETURN_MV_BJHEAVY(thisType);
		case "BJClimax":
			return RETURN_MV_BJCLIMAX(thisType);
		case "BJLick":
			return RETURN_MV_BJLICK(thisType);
		case "BJMisc":
			return RETURN_MV_BJMISC(thisType);
		case "ClosedMouth":
			return RETURN_MV_CLOSEDMOUTH(thisType);
		case "MoanLight":
			return RETURN_MV_MOANLIGHT(thisType);
		case "MoanMedium":
			return RETURN_MV_MOANMEDIUM(thisType);
		case "MoanHeavy":
			return RETURN_MV_MOANHEAVY(thisType);
		case "MoanRough":
			return RETURN_MV_MOANROUGH(thisType);
		case "Climax":
			return RETURN_MV_CLIMAX(thisType);
		case "Attack":
			return RETURN_MV_ATTACK(thisType);
		case "HurtPain":
			return RETURN_MV_HURTPAIN(thisType);
		case "Gasp":
			return RETURN_MV_GASP(thisType);
		case "Scream":
			return RETURN_MV_SCREAM(thisType);
		case "Swallow":
			return RETURN_MV_SWALLOW(thisType);
		case "MBBLogo":
			return RETURN_MV_MBBLOGO(thisType);
		default:
			return null;
		}
	}

	private AudioClip RETURN_MV_BJLIGHT(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight1;
		case GameCore.MayaVoice.BJLight2:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight2;
		case GameCore.MayaVoice.BJLight3:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight3;
		case GameCore.MayaVoice.BJLight4:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight4;
		case GameCore.MayaVoice.BJLight5:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight5;
		case GameCore.MayaVoice.BJLight6:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight6;
		case GameCore.MayaVoice.BJLight7:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight7;
		case GameCore.MayaVoice.BJLight8:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight8;
		case GameCore.MayaVoice.BJLight9:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight9;
		case GameCore.MayaVoice.BJLight10:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight10;
		case GameCore.MayaVoice.BJLight11:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight11;
		case GameCore.MayaVoice.BJLight12:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight12;
		case GameCore.MayaVoice.BJLight13:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight13;
		case GameCore.MayaVoice.BJLight14:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight14;
		case GameCore.MayaVoice.BJLight15:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight15;
		case GameCore.MayaVoice.BJLight16:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight16;
		case GameCore.MayaVoice.BJLight17:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight17;
		case GameCore.MayaVoice.BJLight18:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight18;
		case GameCore.MayaVoice.BJLight19:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight19;
		case GameCore.MayaVoice.BJLight20:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight20;
		case GameCore.MayaVoice.BJLight21:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight21;
		case GameCore.MayaVoice.BJLight22:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight22;
		case GameCore.MayaVoice.BJLight23:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight23;
		case GameCore.MayaVoice.BJLight24:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight24;
		case GameCore.MayaVoice.BJLight25:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight25;
		case GameCore.MayaVoice.BJLight26:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight26;
		case GameCore.MayaVoice.BJLight27:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight27;
		case GameCore.MayaVoice.BJLight28:
			return listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight28;
		}
	}

	private AudioClip RETURN_MV_BJMEDIUM(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium1;
		case GameCore.MayaVoice.BJMedium2:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium2;
		case GameCore.MayaVoice.BJMedium3:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium3;
		case GameCore.MayaVoice.BJMedium4:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium4;
		case GameCore.MayaVoice.BJMedium5:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium5;
		case GameCore.MayaVoice.BJMedium6:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium6;
		case GameCore.MayaVoice.BJMedium7:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium7;
		case GameCore.MayaVoice.BJMedium8:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium8;
		case GameCore.MayaVoice.BJMedium9:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium9;
		case GameCore.MayaVoice.BJMedium10:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium10;
		case GameCore.MayaVoice.BJMedium11:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium11;
		case GameCore.MayaVoice.BJMedium12:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium12;
		case GameCore.MayaVoice.BJMedium13:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium13;
		case GameCore.MayaVoice.BJMedium14:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium14;
		case GameCore.MayaVoice.BJMedium15:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium15;
		case GameCore.MayaVoice.BJMedium16:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium16;
		case GameCore.MayaVoice.BJMedium17:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium17;
		case GameCore.MayaVoice.BJMedium18:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium18;
		case GameCore.MayaVoice.BJMedium19:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium19;
		case GameCore.MayaVoice.BJMedium20:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium20;
		case GameCore.MayaVoice.BJMedium21:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium21;
		case GameCore.MayaVoice.BJMedium22:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium22;
		case GameCore.MayaVoice.BJMedium23:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium23;
		case GameCore.MayaVoice.BJMedium24:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium24;
		case GameCore.MayaVoice.BJMedium25:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium25;
		case GameCore.MayaVoice.BJMedium26:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium26;
		case GameCore.MayaVoice.BJMedium27:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium27;
		case GameCore.MayaVoice.BJMedium28:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium28;
		case GameCore.MayaVoice.BJMedium29:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium29;
		case GameCore.MayaVoice.BJMedium30:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium30;
		case GameCore.MayaVoice.BJMedium31:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium31;
		case GameCore.MayaVoice.BJMedium32:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium32;
		case GameCore.MayaVoice.BJMedium33:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium33;
		case GameCore.MayaVoice.BJMedium34:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium34;
		case GameCore.MayaVoice.BJMedium35:
			return listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium35;
		}
	}

	private AudioClip RETURN_MV_BJHEAVY(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy1;
		case GameCore.MayaVoice.BJHeavy2:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy2;
		case GameCore.MayaVoice.BJHeavy3:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy3;
		case GameCore.MayaVoice.BJHeavy4:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy4;
		case GameCore.MayaVoice.BJHeavy5:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy5;
		case GameCore.MayaVoice.BJHeavy6:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy6;
		case GameCore.MayaVoice.BJHeavy7:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy7;
		case GameCore.MayaVoice.BJHeavy8:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy8;
		case GameCore.MayaVoice.BJHeavy9:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy9;
		case GameCore.MayaVoice.BJHeavy10:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy10;
		case GameCore.MayaVoice.BJHeavy11:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy11;
		case GameCore.MayaVoice.BJHeavy12:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy12;
		case GameCore.MayaVoice.BJHeavy13:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy13;
		case GameCore.MayaVoice.BJHeavy14:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy14;
		case GameCore.MayaVoice.BJHeavy15:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy15;
		case GameCore.MayaVoice.BJHeavy16:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy16;
		case GameCore.MayaVoice.BJHeavy17:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy17;
		case GameCore.MayaVoice.BJHeavy18:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy18;
		case GameCore.MayaVoice.BJHeavy19:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy19;
		case GameCore.MayaVoice.BJHeavy20:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy20;
		case GameCore.MayaVoice.BJHeavy21:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy21;
		case GameCore.MayaVoice.BJHeavy22:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy22;
		case GameCore.MayaVoice.BJHeavy23:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy23;
		case GameCore.MayaVoice.BJHeavy24:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy24;
		case GameCore.MayaVoice.BJHeavy25:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy25;
		case GameCore.MayaVoice.BJHeavy26:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy26;
		case GameCore.MayaVoice.BJHeavy27:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy27;
		case GameCore.MayaVoice.BJHeavy28:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy28;
		case GameCore.MayaVoice.BJHeavy29:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy29;
		case GameCore.MayaVoice.BJHeavy30:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy30;
		case GameCore.MayaVoice.BJHeavy31:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy31;
		case GameCore.MayaVoice.BJHeavy32:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy32;
		case GameCore.MayaVoice.BJHeavy33:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy33;
		case GameCore.MayaVoice.BJHeavy34:
			return listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy34;
		}
	}

	private AudioClip RETURN_MV_BJCLIMAX(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax1;
		case GameCore.MayaVoice.BJClimax2:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax2;
		case GameCore.MayaVoice.BJClimax3:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax3;
		case GameCore.MayaVoice.BJClimax4:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax4;
		case GameCore.MayaVoice.BJClimax5:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax5;
		case GameCore.MayaVoice.BJClimax6:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax6;
		case GameCore.MayaVoice.BJClimax7:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax7;
		case GameCore.MayaVoice.BJClimax8:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax8;
		case GameCore.MayaVoice.BJClimax9:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax9;
		case GameCore.MayaVoice.BJClimax10:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax10;
		case GameCore.MayaVoice.BJClimax11:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax11;
		case GameCore.MayaVoice.BJClimax12:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax12;
		case GameCore.MayaVoice.BJClimax13:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax13;
		case GameCore.MayaVoice.BJClimax14:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax14;
		case GameCore.MayaVoice.BJClimax15:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax15;
		case GameCore.MayaVoice.BJClimax16:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax16;
		case GameCore.MayaVoice.BJClimax17:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax17;
		case GameCore.MayaVoice.BJClimax18:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax18;
		case GameCore.MayaVoice.BJClimax19:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax19;
		case GameCore.MayaVoice.BJClimax20:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax20;
		case GameCore.MayaVoice.BJClimax21:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax21;
		case GameCore.MayaVoice.BJClimax22:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax22;
		case GameCore.MayaVoice.BJClimax23:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax23;
		case GameCore.MayaVoice.BJClimax24:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax24;
		case GameCore.MayaVoice.BJClimax25:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax25;
		case GameCore.MayaVoice.BJClimax26:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax26;
		case GameCore.MayaVoice.BJClimax27:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax27;
		case GameCore.MayaVoice.BJClimax28:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax28;
		case GameCore.MayaVoice.BJClimax29:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax29;
		case GameCore.MayaVoice.BJClimax30:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax30;
		case GameCore.MayaVoice.BJClimax31:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax31;
		case GameCore.MayaVoice.BJClimax32:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax32;
		case GameCore.MayaVoice.BJClimax33:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax33;
		case GameCore.MayaVoice.BJClimax34:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax34;
		case GameCore.MayaVoice.BJClimax35:
			return listOfMayaVoice.BlowjobClimax.BlowjobClimax35;
		}
	}

	private AudioClip RETURN_MV_BJLICK(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick1;
		case GameCore.MayaVoice.BJLick2:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick2;
		case GameCore.MayaVoice.BJLick3:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick3;
		case GameCore.MayaVoice.BJLick4:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick4;
		case GameCore.MayaVoice.BJLick5:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick5;
		case GameCore.MayaVoice.BJLick6:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick6;
		case GameCore.MayaVoice.BJLick7:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick7;
		case GameCore.MayaVoice.BJLick8:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick8;
		case GameCore.MayaVoice.BJLick9:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick9;
		case GameCore.MayaVoice.BJLick10:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick10;
		case GameCore.MayaVoice.BJLick11:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick11;
		case GameCore.MayaVoice.BJLick12:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick12;
		case GameCore.MayaVoice.BJLick13:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick13;
		case GameCore.MayaVoice.BJLick14:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick14;
		case GameCore.MayaVoice.BJLick15:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick15;
		case GameCore.MayaVoice.BJLick16:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick16;
		case GameCore.MayaVoice.BJLick17:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick17;
		case GameCore.MayaVoice.BJLick18:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick18;
		case GameCore.MayaVoice.BJLick19:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick19;
		case GameCore.MayaVoice.BJLick20:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick20;
		case GameCore.MayaVoice.BJLick21:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick21;
		case GameCore.MayaVoice.BJLick22:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick22;
		case GameCore.MayaVoice.BJLick23:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick23;
		case GameCore.MayaVoice.BJLick24:
			return listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick24;
		}
	}

	private AudioClip RETURN_MV_BJMISC(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc1;
		case GameCore.MayaVoice.BJMisc2:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc2;
		case GameCore.MayaVoice.BJMisc3:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc3;
		case GameCore.MayaVoice.BJMisc4:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc4;
		case GameCore.MayaVoice.BJMisc5:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc5;
		case GameCore.MayaVoice.BJMisc6:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc6;
		case GameCore.MayaVoice.BJMisc7:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc7;
		case GameCore.MayaVoice.BJMisc8:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc8;
		case GameCore.MayaVoice.BJMisc9:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc9;
		case GameCore.MayaVoice.BJMisc10:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc10;
		case GameCore.MayaVoice.BJMisc11:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc11;
		case GameCore.MayaVoice.BJMisc12:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc12;
		case GameCore.MayaVoice.BJMisc13:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc13;
		case GameCore.MayaVoice.BJMisc14:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc14;
		case GameCore.MayaVoice.BJMisc15:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc15;
		case GameCore.MayaVoice.BJMisc16:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc16;
		case GameCore.MayaVoice.BJMisc17:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc17;
		case GameCore.MayaVoice.BJMisc18:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc18;
		case GameCore.MayaVoice.BJMisc19:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc19;
		case GameCore.MayaVoice.BJMisc20:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc20;
		case GameCore.MayaVoice.BJMisc21:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc21;
		case GameCore.MayaVoice.BJMisc22:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc22;
		case GameCore.MayaVoice.BJMisc23:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc23;
		case GameCore.MayaVoice.BJMisc24:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc24;
		case GameCore.MayaVoice.BJMisc25:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc25;
		case GameCore.MayaVoice.BJMisc26:
			return listOfMayaVoice.BlowjobMisc.BlowjobMisc26;
		}
	}

	private AudioClip RETURN_MV_CLOSEDMOUTH(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.ClosedMouth.ClosedMouth1;
		case GameCore.MayaVoice.ClosedMouth2:
			return listOfMayaVoice.ClosedMouth.ClosedMouth2;
		case GameCore.MayaVoice.ClosedMouth3:
			return listOfMayaVoice.ClosedMouth.ClosedMouth3;
		case GameCore.MayaVoice.ClosedMouth4:
			return listOfMayaVoice.ClosedMouth.ClosedMouth4;
		case GameCore.MayaVoice.ClosedMouth5:
			return listOfMayaVoice.ClosedMouth.ClosedMouth5;
		case GameCore.MayaVoice.ClosedMouth6:
			return listOfMayaVoice.ClosedMouth.ClosedMouth6;
		case GameCore.MayaVoice.ClosedMouth7:
			return listOfMayaVoice.ClosedMouth.ClosedMouth7;
		case GameCore.MayaVoice.ClosedMouth8:
			return listOfMayaVoice.ClosedMouth.ClosedMouth8;
		case GameCore.MayaVoice.ClosedMouth9:
			return listOfMayaVoice.ClosedMouth.ClosedMouth9;
		case GameCore.MayaVoice.ClosedMouth10:
			return listOfMayaVoice.ClosedMouth.ClosedMouth10;
		case GameCore.MayaVoice.ClosedMouth11:
			return listOfMayaVoice.ClosedMouth.ClosedMouth11;
		case GameCore.MayaVoice.ClosedMouth12:
			return listOfMayaVoice.ClosedMouth.ClosedMouth12;
		case GameCore.MayaVoice.ClosedMouth13:
			return listOfMayaVoice.ClosedMouth.ClosedMouth13;
		case GameCore.MayaVoice.ClosedMouth14:
			return listOfMayaVoice.ClosedMouth.ClosedMouth14;
		case GameCore.MayaVoice.ClosedMouth15:
			return listOfMayaVoice.ClosedMouth.ClosedMouth15;
		case GameCore.MayaVoice.ClosedMouth16:
			return listOfMayaVoice.ClosedMouth.ClosedMouth16;
		case GameCore.MayaVoice.ClosedMouth17:
			return listOfMayaVoice.ClosedMouth.ClosedMouth17;
		}
	}

	private AudioClip RETURN_MV_MOANLIGHT(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.MoanLight.MoanLight1;
		case GameCore.MayaVoice.MoanLight2:
			return listOfMayaVoice.MoanLight.MoanLight2;
		case GameCore.MayaVoice.MoanLight3:
			return listOfMayaVoice.MoanLight.MoanLight3;
		case GameCore.MayaVoice.MoanLight4:
			return listOfMayaVoice.MoanLight.MoanLight4;
		case GameCore.MayaVoice.MoanLight5:
			return listOfMayaVoice.MoanLight.MoanLight5;
		case GameCore.MayaVoice.MoanLight6:
			return listOfMayaVoice.MoanLight.MoanLight6;
		case GameCore.MayaVoice.MoanLight7:
			return listOfMayaVoice.MoanLight.MoanLight7;
		case GameCore.MayaVoice.MoanLight8:
			return listOfMayaVoice.MoanLight.MoanLight8;
		case GameCore.MayaVoice.MoanLight9:
			return listOfMayaVoice.MoanLight.MoanLight9;
		case GameCore.MayaVoice.MoanLight10:
			return listOfMayaVoice.MoanLight.MoanLight10;
		case GameCore.MayaVoice.MoanLight11:
			return listOfMayaVoice.MoanLight.MoanLight11;
		case GameCore.MayaVoice.MoanLight12:
			return listOfMayaVoice.MoanLight.MoanLight12;
		case GameCore.MayaVoice.MoanLight13:
			return listOfMayaVoice.MoanLight.MoanLight13;
		case GameCore.MayaVoice.MoanLight14:
			return listOfMayaVoice.MoanLight.MoanLight14;
		case GameCore.MayaVoice.MoanLight15:
			return listOfMayaVoice.MoanLight.MoanLight15;
		case GameCore.MayaVoice.MoanLight16:
			return listOfMayaVoice.MoanLight.MoanLight16;
		case GameCore.MayaVoice.MoanLight17:
			return listOfMayaVoice.MoanLight.MoanLight17;
		case GameCore.MayaVoice.MoanLight18:
			return listOfMayaVoice.MoanLight.MoanLight18;
		case GameCore.MayaVoice.MoanLight19:
			return listOfMayaVoice.MoanLight.MoanLight19;
		}
	}

	private AudioClip RETURN_MV_MOANMEDIUM(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.MoanMedium.MoanMedium1;
		case GameCore.MayaVoice.MoanMedium2:
			return listOfMayaVoice.MoanMedium.MoanMedium2;
		case GameCore.MayaVoice.MoanMedium3:
			return listOfMayaVoice.MoanMedium.MoanMedium3;
		case GameCore.MayaVoice.MoanMedium4:
			return listOfMayaVoice.MoanMedium.MoanMedium4;
		case GameCore.MayaVoice.MoanMedium5:
			return listOfMayaVoice.MoanMedium.MoanMedium5;
		case GameCore.MayaVoice.MoanMedium6:
			return listOfMayaVoice.MoanMedium.MoanMedium6;
		case GameCore.MayaVoice.MoanMedium7:
			return listOfMayaVoice.MoanMedium.MoanMedium7;
		case GameCore.MayaVoice.MoanMedium8:
			return listOfMayaVoice.MoanMedium.MoanMedium8;
		case GameCore.MayaVoice.MoanMedium9:
			return listOfMayaVoice.MoanMedium.MoanMedium9;
		case GameCore.MayaVoice.MoanMedium10:
			return listOfMayaVoice.MoanMedium.MoanMedium10;
		case GameCore.MayaVoice.MoanMedium11:
			return listOfMayaVoice.MoanMedium.MoanMedium11;
		}
	}

	private AudioClip RETURN_MV_MOANHEAVY(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.MoanHeavy.MoanHeavy1;
		case GameCore.MayaVoice.MoanHeavy2:
			return listOfMayaVoice.MoanHeavy.MoanHeavy2;
		case GameCore.MayaVoice.MoanHeavy3:
			return listOfMayaVoice.MoanHeavy.MoanHeavy3;
		case GameCore.MayaVoice.MoanHeavy4:
			return listOfMayaVoice.MoanHeavy.MoanHeavy4;
		case GameCore.MayaVoice.MoanHeavy5:
			return listOfMayaVoice.MoanHeavy.MoanHeavy5;
		case GameCore.MayaVoice.MoanHeavy6:
			return listOfMayaVoice.MoanHeavy.MoanHeavy6;
		case GameCore.MayaVoice.MoanHeavy7:
			return listOfMayaVoice.MoanHeavy.MoanHeavy7;
		case GameCore.MayaVoice.MoanHeavy8:
			return listOfMayaVoice.MoanHeavy.MoanHeavy8;
		case GameCore.MayaVoice.MoanHeavy9:
			return listOfMayaVoice.MoanHeavy.MoanHeavy9;
		case GameCore.MayaVoice.MoanHeavy10:
			return listOfMayaVoice.MoanHeavy.MoanHeavy10;
		case GameCore.MayaVoice.MoanHeavy11:
			return listOfMayaVoice.MoanHeavy.MoanHeavy11;
		}
	}

	private AudioClip RETURN_MV_MOANROUGH(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.MoanRough.MoanRough1;
		case GameCore.MayaVoice.MoanRough2:
			return listOfMayaVoice.MoanRough.MoanRough2;
		case GameCore.MayaVoice.MoanRough3:
			return listOfMayaVoice.MoanRough.MoanRough3;
		case GameCore.MayaVoice.MoanRough4:
			return listOfMayaVoice.MoanRough.MoanRough4;
		case GameCore.MayaVoice.MoanRough5:
			return listOfMayaVoice.MoanRough.MoanRough5;
		case GameCore.MayaVoice.MoanRough6:
			return listOfMayaVoice.MoanRough.MoanRough6;
		case GameCore.MayaVoice.MoanRough7:
			return listOfMayaVoice.MoanRough.MoanRough7;
		case GameCore.MayaVoice.MoanRough8:
			return listOfMayaVoice.MoanRough.MoanRough8;
		case GameCore.MayaVoice.MoanRough9:
			return listOfMayaVoice.MoanRough.MoanRough9;
		case GameCore.MayaVoice.MoanRough10:
			return listOfMayaVoice.MoanRough.MoanRough10;
		case GameCore.MayaVoice.MoanRough11:
			return listOfMayaVoice.MoanRough.MoanRough11;
		case GameCore.MayaVoice.MoanRough12:
			return listOfMayaVoice.MoanRough.MoanRough12;
		}
	}

	private AudioClip RETURN_MV_CLIMAX(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.Climax.Climax1;
		case GameCore.MayaVoice.Climax2:
			return listOfMayaVoice.Climax.Climax2;
		case GameCore.MayaVoice.Climax3:
			return listOfMayaVoice.Climax.Climax3;
		case GameCore.MayaVoice.Climax4:
			return listOfMayaVoice.Climax.Climax4;
		case GameCore.MayaVoice.Climax5:
			return listOfMayaVoice.Climax.Climax5;
		case GameCore.MayaVoice.Climax6:
			return listOfMayaVoice.Climax.Climax6;
		case GameCore.MayaVoice.Climax7:
			return listOfMayaVoice.Climax.Climax7;
		case GameCore.MayaVoice.Climax8:
			return listOfMayaVoice.Climax.Climax8;
		case GameCore.MayaVoice.Climax9:
			return listOfMayaVoice.Climax.Climax9;
		case GameCore.MayaVoice.Climax10:
			return listOfMayaVoice.Climax.Climax10;
		case GameCore.MayaVoice.Climax11:
			return listOfMayaVoice.Climax.Climax11;
		}
	}

	private AudioClip RETURN_MV_ATTACK(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.Attack.Attack1;
		case GameCore.MayaVoice.Attack2:
			return listOfMayaVoice.Attack.Attack2;
		case GameCore.MayaVoice.Attack3:
			return listOfMayaVoice.Attack.Attack3;
		case GameCore.MayaVoice.Attack4:
			return listOfMayaVoice.Attack.Attack4;
		case GameCore.MayaVoice.Attack5:
			return listOfMayaVoice.Attack.Attack5;
		case GameCore.MayaVoice.Attack6:
			return listOfMayaVoice.Attack.Attack6;
		case GameCore.MayaVoice.Attack7:
			return listOfMayaVoice.Attack.Attack7;
		case GameCore.MayaVoice.Attack8:
			return listOfMayaVoice.Attack.Attack8;
		case GameCore.MayaVoice.Attack9:
			return listOfMayaVoice.Attack.Attack9;
		case GameCore.MayaVoice.Attack10:
			return listOfMayaVoice.Attack.Attack10;
		case GameCore.MayaVoice.Attack11:
			return listOfMayaVoice.Attack.Attack11;
		case GameCore.MayaVoice.Attack12:
			return listOfMayaVoice.Attack.Attack12;
		case GameCore.MayaVoice.Attack13:
			return listOfMayaVoice.Attack.Attack13;
		case GameCore.MayaVoice.Attack14:
			return listOfMayaVoice.Attack.Attack14;
		case GameCore.MayaVoice.Attack15:
			return listOfMayaVoice.Attack.Attack15;
		case GameCore.MayaVoice.Attack16:
			return listOfMayaVoice.Attack.Attack16;
		case GameCore.MayaVoice.Attack17:
			return listOfMayaVoice.Attack.Attack17;
		}
	}

	private AudioClip RETURN_MV_HURTPAIN(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.HurtPain.HurtPain1;
		case GameCore.MayaVoice.HurtPain2:
			return listOfMayaVoice.HurtPain.HurtPain2;
		case GameCore.MayaVoice.HurtPain3:
			return listOfMayaVoice.HurtPain.HurtPain3;
		case GameCore.MayaVoice.HurtPain4:
			return listOfMayaVoice.HurtPain.HurtPain4;
		case GameCore.MayaVoice.HurtPain5:
			return listOfMayaVoice.HurtPain.HurtPain5;
		case GameCore.MayaVoice.HurtPain6:
			return listOfMayaVoice.HurtPain.HurtPain6;
		case GameCore.MayaVoice.HurtPain7:
			return listOfMayaVoice.HurtPain.HurtPain7;
		case GameCore.MayaVoice.HurtPain8:
			return listOfMayaVoice.HurtPain.HurtPain8;
		case GameCore.MayaVoice.HurtPain9:
			return listOfMayaVoice.HurtPain.HurtPain9;
		case GameCore.MayaVoice.HurtPain10:
			return listOfMayaVoice.HurtPain.HurtPain10;
		case GameCore.MayaVoice.HurtPain11:
			return listOfMayaVoice.HurtPain.HurtPain11;
		case GameCore.MayaVoice.HurtPain12:
			return listOfMayaVoice.HurtPain.HurtPain12;
		case GameCore.MayaVoice.HurtPain13:
			return listOfMayaVoice.HurtPain.HurtPain13;
		case GameCore.MayaVoice.HurtPain14:
			return listOfMayaVoice.HurtPain.HurtPain14;
		case GameCore.MayaVoice.HurtPain15:
			return listOfMayaVoice.HurtPain.HurtPain15;
		case GameCore.MayaVoice.HurtPain16:
			return listOfMayaVoice.HurtPain.HurtPain16;
		}
	}

	private AudioClip RETURN_MV_GASP(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.Gasp.Gasp1;
		case GameCore.MayaVoice.Gasp2:
			return listOfMayaVoice.Gasp.Gasp2;
		case GameCore.MayaVoice.Gasp3:
			return listOfMayaVoice.Gasp.Gasp3;
		case GameCore.MayaVoice.Gasp4:
			return listOfMayaVoice.Gasp.Gasp4;
		case GameCore.MayaVoice.Gasp5:
			return listOfMayaVoice.Gasp.Gasp5;
		case GameCore.MayaVoice.Gasp6:
			return listOfMayaVoice.Gasp.Gasp6;
		case GameCore.MayaVoice.Gasp7:
			return listOfMayaVoice.Gasp.Gasp7;
		case GameCore.MayaVoice.Gasp8:
			return listOfMayaVoice.Gasp.Gasp8;
		case GameCore.MayaVoice.Gasp9:
			return listOfMayaVoice.Gasp.Gasp9;
		case GameCore.MayaVoice.Gasp10:
			return listOfMayaVoice.Gasp.Gasp10;
		}
	}

	private AudioClip RETURN_MV_SCREAM(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.Scream.Scream1;
		case GameCore.MayaVoice.Scream2:
			return listOfMayaVoice.Scream.Scream2;
		case GameCore.MayaVoice.Scream3:
			return listOfMayaVoice.Scream.Scream3;
		case GameCore.MayaVoice.Scream4:
			return listOfMayaVoice.Scream.Scream4;
		case GameCore.MayaVoice.Scream5:
			return listOfMayaVoice.Scream.Scream5;
		case GameCore.MayaVoice.Scream6:
			return listOfMayaVoice.Scream.Scream6;
		case GameCore.MayaVoice.Scream7:
			return listOfMayaVoice.Scream.Scream7;
		case GameCore.MayaVoice.Scream8:
			return listOfMayaVoice.Scream.Scream8;
		case GameCore.MayaVoice.Scream9:
			return listOfMayaVoice.Scream.Scream9;
		case GameCore.MayaVoice.Scream10:
			return listOfMayaVoice.Scream.Scream10;
		case GameCore.MayaVoice.Scream11:
			return listOfMayaVoice.Scream.Scream11;
		case GameCore.MayaVoice.Scream12:
			return listOfMayaVoice.Scream.Scream12;
		case GameCore.MayaVoice.Scream13:
			return listOfMayaVoice.Scream.Scream13;
		case GameCore.MayaVoice.Scream14:
			return listOfMayaVoice.Scream.Scream14;
		case GameCore.MayaVoice.Scream15:
			return listOfMayaVoice.Scream.Scream15;
		}
	}

	private AudioClip RETURN_MV_SWALLOW(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.Swallow.Swallow1;
		case GameCore.MayaVoice.Swallow2:
			return listOfMayaVoice.Swallow.Swallow2;
		case GameCore.MayaVoice.Swallow3:
			return listOfMayaVoice.Swallow.Swallow3;
		case GameCore.MayaVoice.Swallow4:
			return listOfMayaVoice.Swallow.Swallow4;
		case GameCore.MayaVoice.Swallow5:
			return listOfMayaVoice.Swallow.Swallow5;
		case GameCore.MayaVoice.Swallow6:
			return listOfMayaVoice.Swallow.Swallow6;
		}
	}

	private AudioClip RETURN_MV_MBBLOGO(GameCore.MayaVoice thisType)
	{
		switch (thisType)
		{
		default:
			return listOfMayaVoice.MBBLogo.MBBLogo1;
		case GameCore.MayaVoice.MBBLogo2:
			return listOfMayaVoice.MBBLogo.MBBLogo2;
		case GameCore.MayaVoice.MBBLogo3:
			return listOfMayaVoice.MBBLogo.MBBLogo3;
		case GameCore.MayaVoice.MBBLogo4:
			return listOfMayaVoice.MBBLogo.MBBLogo4;
		case GameCore.MayaVoice.MBBLogo5:
			return listOfMayaVoice.MBBLogo.MBBLogo5;
		}
	}

	public AudioClip ReturnForestBossSounds(int soundID)
	{
		switch (soundID)
		{
		case 1:
			return listOfForestBossSound.ForestBoss1;
		case 2:
			return listOfForestBossSound.ForestBoss2;
		case 3:
			return listOfForestBossSound.ForestBoss3;
		case 4:
			return listOfForestBossSound.ForestBoss4;
		case 5:
			return listOfForestBossSound.ForestBoss5;
		case 6:
			return listOfForestBossSound.ForestBoss6;
		case 7:
			return listOfForestBossSound.ForestBoss7;
		case 8:
			return listOfForestBossSound.ForestBoss8;
		case 9:
			return listOfForestBossSound.ForestBoss9;
		case 10:
			return listOfForestBossSound.ForestBoss10;
		case 11:
			return listOfForestBossSound.ForestBoss11;
		case 12:
			return listOfForestBossSound.ForestBoss12;
		case 13:
			return listOfForestBossSound.ForestBoss13;
		case 14:
			return listOfForestBossSound.ForestBoss14;
		case 15:
			return listOfForestBossSound.ForestBoss15;
		default:
			return null;
		}
	}

	public AudioClip ReturnHillBossSounds(int soundID)
	{
		switch (soundID)
		{
		case 1:
			return listOfHillBossSound.HillBoss1;
		case 2:
			return listOfHillBossSound.HillBoss2;
		case 3:
			return listOfHillBossSound.HillBoss3;
		case 4:
			return listOfHillBossSound.HillBoss4;
		case 5:
			return listOfHillBossSound.HillBoss5;
		case 6:
			return listOfHillBossSound.HillBoss6;
		default:
			return null;
		}
	}

	public AudioClip ReturnWerewolfBossSounds(int soundID)
	{
		switch (soundID)
		{
		case 1:
			return listOfSFXBasic2.Transformation1;
		case 2:
			return listOfWerewolfBossSound.Howl;
		case 3:
			return listOfWerewolfBossSound.Grrr;
		case 4:
			return listOfWerewolfBossSound.SnarlBite;
		case 5:
			return listOfWerewolfBossSound.LeapBack;
		case 6:
			return listOfWerewolfBossSound.PounceForwards;
		case 7:
			return listOfWerewolfBossSound.Swing1;
		case 8:
			return listOfWerewolfBossSound.Swing2;
		case 9:
			return listOfWerewolfBossSound.Grrr2;
		case 10:
			return listOfWerewolfBossSound.RunAway;
		case 11:
			return listOfWerewolfBossSound.Step1;
		case 12:
			return listOfWerewolfBossSound.Step2;
		case 13:
			return listOfWerewolfBossSound.MoonExplosion;
		case 14:
			return listOfWerewolfBossSound.SlashBarrage;
		default:
			return null;
		}
	}

	public AudioClip ReturnMinotaurBossSounds(int soundID)
	{
		switch (soundID)
		{
		case 1:
			return listOfSFXBasic2.Swing1;
		case 2:
			return listOfSFXBasic2.Swing5;
		case 3:
			return listOfMinotaurBossSound.Bull1;
		case 4:
			return listOfMinotaurBossSound.Bull2;
		case 5:
			return listOfMinotaurBossSound.Bull3;
		case 6:
			return listOfMinotaurBossSound.DefenseShieldBlock;
		case 7:
			return listOfMinotaurBossSound.Jump;
		case 8:
			return listOfMinotaurBossSound.GroundExplode;
		case 9:
			return listOfMinotaurBossSound.HoovesWalk1;
		case 10:
			return listOfMinotaurBossSound.HoovesWalk2;
		case 11:
			return listOfMinotaurBossSound.HoovesWalk3;
		case 12:
			return listOfMinotaurBossSound.HoovesLand;
		case 13:
			return listOfMinotaurBossSound.Dodge;
		case 99:
			switch (Random.Range(0, 3))
			{
			default:
				return listOfMinotaurBossSound.Bull1;
			case 1:
				return listOfMinotaurBossSound.Bull2;
			case 2:
				return listOfMinotaurBossSound.Bull3;
			}
		case 100:
		{
			int num = Random.Range(0, 2);
			if (num == 0 || num != 1)
			{
				return listOfMinotaurBossSound.HoovesWalk1;
			}
			return listOfMinotaurBossSound.HoovesWalk2;
		}
		default:
			return null;
		}
	}

	public AudioClip ReturnPrincessBossSounds(int soundID)
	{
		switch (soundID)
		{
		case 1:
			return listOfPrincessBossSound.Attack1;
		case 2:
			return listOfPrincessBossSound.Attack2;
		case 3:
			return listOfPrincessBossSound.Attack3;
		case 4:
			return listOfPrincessBossSound.Woah;
		case 5:
			switch (Random.Range(0, 3))
			{
			default:
				return listOfPrincessBossSound.Attack1;
			case 1:
				return listOfPrincessBossSound.Attack2;
			case 2:
				return listOfPrincessBossSound.Attack3;
			}
		case 10:
			return listOfPrincessBossSound.Boulder;
		case 11:
			return listOfPrincessBossSound.Ice;
		case 12:
			return listOfPrincessBossSound.Break;
		case 13:
			return listOfPrincessBossSound.IceGen1;
		case 14:
			return listOfPrincessBossSound.IceGen2;
		case 15:
			return listOfPrincessBossSound.Laugh;
		default:
			return null;
		}
	}

	public AudioClip ReturnWitchBossSounds(int soundID)
	{
		switch (soundID)
		{
		case 1:
			return listOfWitchBossSound.laugh1;
		case 2:
			return listOfWitchBossSound.laugh2;
		case 3:
			return listOfWitchBossSound.laugh3;
		case 4:
			return listOfWitchBossSound.laugh_vol04_01;
		case 5:
			return listOfWitchBossSound.laugh_vol04_02;
		case 6:
			return listOfWitchBossSound.laugh_vol04_03;
		case 7:
			return listOfWitchBossSound.laugh_vol04_04;
		case 8:
			return listOfWitchBossSound.laugh_vol04_05;
		case 9:
		{
			int num = Random.Range(0, 2);
			if (num == 0 || num != 1)
			{
				return listOfWitchBossSound.laugh1;
			}
			return listOfWitchBossSound.laugh3;
		}
		case 10:
			switch (Random.Range(1, 6))
			{
			default:
				return listOfWitchBossSound.laugh_vol04_01;
			case 2:
				return listOfWitchBossSound.laugh_vol04_02;
			case 3:
				return listOfWitchBossSound.laugh_vol04_03;
			case 4:
				return listOfWitchBossSound.laugh_vol04_04;
			case 5:
				return listOfWitchBossSound.laugh_vol04_05;
			}
		case 11:
			return listOfWitchBossSound.moan01;
		case 12:
			return listOfWitchBossSound.moan02;
		case 13:
			return listOfWitchBossSound.moan03;
		case 14:
			return listOfWitchBossSound.moan04;
		case 15:
			return listOfWitchBossSound.moan05;
		case 16:
			return listOfWitchBossSound.moan06;
		case 17:
			return listOfWitchBossSound.moan07;
		case 18:
			return listOfWitchBossSound.moan08;
		case 19:
			return listOfWitchBossSound.moan09;
		case 20:
			return listOfWitchBossSound.moan10;
		case 21:
			return listOfWitchBossSound.moan11;
		case 22:
			return listOfWitchBossSound.moan12;
		case 23:
			return listOfWitchBossSound.oh_my;
		case 24:
			return listOfWitchBossSound.TP_start;
		case 25:
			return listOfWitchBossSound.TP_end;
		case 26:
			return listOfWitchBossSound.Casting;
		case 27:
			return listOfWitchBossSound.Spawn;
		case 28:
			return listOfWitchBossSound.AngryHurt;
		case 29:
			return listOfWitchBossSound.KissHearts;
		case 30:
			switch (Random.Range(1, 5))
			{
			default:
				return listOfWitchBossSound.moan01;
			case 2:
				return listOfWitchBossSound.moan02;
			case 3:
				return listOfWitchBossSound.moan03;
			case 4:
				return listOfWitchBossSound.moan04;
			}
		case 31:
			switch (Random.Range(1, 5))
			{
			default:
				return listOfWitchBossSound.moan05;
			case 2:
				return listOfWitchBossSound.moan06;
			case 3:
				return listOfWitchBossSound.moan07;
			case 4:
				return listOfWitchBossSound.moan08;
			}
		case 32:
			switch (Random.Range(1, 5))
			{
			default:
				return listOfWitchBossSound.moan09;
			case 2:
				return listOfWitchBossSound.moan10;
			case 3:
				return listOfWitchBossSound.moan11;
			case 4:
				return listOfWitchBossSound.moan12;
			}
		case 33:
			switch (Random.Range(1, 5))
			{
			default:
				return listOfWitchBossSound.moan01;
			case 2:
				return listOfWitchBossSound.moan04;
			case 3:
				return listOfWitchBossSound.moan07;
			case 4:
				return listOfWitchBossSound.moan09;
			}
		default:
			return null;
		}
	}

	public AudioClip ReturnBlueFoxBossSounds(int soundID)
	{
		switch (soundID)
		{
		case 0:
			return listOfBlueFoxBossSound.ShootProjectiles;
		case 1:
			return listOfBlueFoxBossSound.PreMelee;
		case 2:
			return listOfBlueFoxBossSound.MeleeSpin;
		case 3:
			return listOfBlueFoxBossSound.Jump;
		case 4:
			return listOfBlueFoxBossSound.Dash;
		case 5:
			return listOfBlueFoxBossSound.PreBeam;
		case 6:
			return listOfBlueFoxBossSound.Beam;
		case 7:
			return listOfBlueFoxBossSound.PounceExplode;
		case 8:
			return listOfBlueFoxBossSound.DropMine;
		case 9:
			return listOfBlueFoxBossSound.MineActivate1;
		case 10:
			return listOfBlueFoxBossSound.MineActivate2;
		case 11:
			return listOfBlueFoxBossSound.MineActivate3;
		case 12:
			return listOfBlueFoxBossSound.MindBlast1;
		case 13:
			return listOfBlueFoxBossSound.MindBlast2;
		case 14:
			return listOfBlueFoxBossSound.Electric1;
		case 15:
			return listOfBlueFoxBossSound.Electric2;
		case 16:
			return listOfBlueFoxBossSound.Electric3;
		case 17:
			return listOfBlueFoxBossSound.Electric4;
		case 18:
			return listOfBlueFoxBossSound.Electric5;
		default:
			return null;
		}
	}

	public AudioClip ReturnHarunaVoiceMoans(int soundID)
	{
		switch (soundID)
		{
		case 0:
			return listOfHarunaVoiceSound.Moan0;
		case 1:
			return listOfHarunaVoiceSound.Moan1;
		case 2:
			return listOfHarunaVoiceSound.Moan2;
		case 3:
			return listOfHarunaVoiceSound.Moan3;
		case 4:
			return listOfHarunaVoiceSound.Moan4;
		case 5:
			return listOfHarunaVoiceSound.Moan5;
		case 6:
			return listOfHarunaVoiceSound.Moan6;
		case 7:
			return listOfHarunaVoiceSound.Moan7;
		case 8:
			return listOfHarunaVoiceSound.Moan8;
		case 9:
			return listOfHarunaVoiceSound.Moan9;
		case 10:
			return listOfHarunaVoiceSound.Moan10;
		case 11:
			return listOfHarunaVoiceSound.Moan11;
		case 12:
			return listOfHarunaVoiceSound.Moan12;
		case 13:
			return listOfHarunaVoiceSound.Moan13;
		case 14:
			return listOfHarunaVoiceSound.Moan14;
		default:
			return null;
		}
	}

	private int GetMATH_RNG_INT(int a, int b, string noRepeat = "")
	{
		GameCore instance = GameCore.Instance;
		if ((bool)instance)
		{
			return instance.MATH_RNG_INT(a, b, noRepeat);
		}
		return 0;
	}
}
