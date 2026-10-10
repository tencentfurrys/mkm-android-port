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
		return thisType switch
		{
			GameCore.MusicTheme.TitleScreen => listOfMusic.TitleScreenTheme, 
			GameCore.MusicTheme.IntroStory => listOfMusic.IntroStoryTheme, 
			GameCore.MusicTheme.Beach1 => listOfMusic.StageBeach1, 
			GameCore.MusicTheme.Beach2 => listOfMusic.StageBeach2, 
			GameCore.MusicTheme.HubCastle1 => listOfMusic.StageHubCastle1, 
			GameCore.MusicTheme.CastleUpperFLoor => listOfMusic.CastleUpperFLoor, 
			GameCore.MusicTheme.Forest1 => listOfMusic.StageForest1, 
			GameCore.MusicTheme.Fainted => listOfMusic.defeatedFainted, 
			GameCore.MusicTheme.BossFight1 => listOfMusic.BossFight1, 
			GameCore.MusicTheme.VictoryFanfare1 => listOfMusic.VictoryFanfare1, 
			GameCore.MusicTheme.Valley1 => listOfMusic.StageValley1, 
			GameCore.MusicTheme.City1 => listOfMusic.StageCity1, 
			GameCore.MusicTheme.EroGrabbed => listOfMusic.EroGrabbed, 
			GameCore.MusicTheme.UniqueItemFanfare1 => listOfMusic.UniqueItemFanfare1, 
			GameCore.MusicTheme.Cave1 => listOfMusic.Cave1, 
			GameCore.MusicTheme.Cave2 => listOfMusic.Cave2, 
			GameCore.MusicTheme.Cave3 => listOfMusic.Cave3, 
			GameCore.MusicTheme.DarkAmbience1 => listOfMusic.DarkAmbience1, 
			GameCore.MusicTheme.Swamp1 => listOfMusic.Swamp1, 
			GameCore.MusicTheme.Dungeon => listOfMusic.Dungeon, 
			GameCore.MusicTheme.HighTech1 => listOfMusic.HighTech1, 
			GameCore.MusicTheme.BossFight2 => listOfMusic.BossFight2, 
			GameCore.MusicTheme.HighTech2 => listOfMusic.HighTech2, 
			GameCore.MusicTheme.BadEndTheme => listOfMusic.BadEndTheme, 
			GameCore.MusicTheme.BossFight3 => listOfMusic.BossFight3, 
			_ => null, 
		};
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
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.SwingTK1, 
				2 => listOfSFXBasic1.SwingTK2, 
				_ => listOfSFXBasic1.SwingTK3, 
			};
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
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_1a, 
				2 => listOfSFXBasic1.Diag_1b, 
				_ => listOfSFXBasic1.Diag_1c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup2:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_2a, 
				2 => listOfSFXBasic1.Diag_2b, 
				_ => listOfSFXBasic1.Diag_2c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup3:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_3a, 
				2 => listOfSFXBasic1.Diag_3b, 
				_ => listOfSFXBasic1.Diag_3c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup4:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_4a, 
				2 => listOfSFXBasic1.Diag_4b, 
				_ => listOfSFXBasic1.Diag_4c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup5:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_5a, 
				2 => listOfSFXBasic1.Diag_5b, 
				_ => listOfSFXBasic1.Diag_5c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup6:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_6a, 
				2 => listOfSFXBasic1.Diag_6b, 
				_ => listOfSFXBasic1.Diag_6c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup7:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_7a, 
				2 => listOfSFXBasic1.Diag_7b, 
				_ => listOfSFXBasic1.Diag_7c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup8:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_8a, 
				2 => listOfSFXBasic1.Diag_8b, 
				_ => listOfSFXBasic1.Diag_8c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup9:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_9a, 
				2 => listOfSFXBasic1.Diag_9b, 
				_ => listOfSFXBasic1.Diag_9c, 
			};
		case GameCore.SFXBasic1.DiagToneGroup10:
			return GetMATH_RNG_INT(1, 8, "DIAGwrite") switch
			{
				1 => listOfSFXBasic1.Diag_10a, 
				2 => listOfSFXBasic1.Diag_10b, 
				3 => listOfSFXBasic1.Diag_10c, 
				4 => listOfSFXBasic1.Diag_10d, 
				5 => listOfSFXBasic1.Diag_10e, 
				6 => listOfSFXBasic1.Diag_10f, 
				7 => listOfSFXBasic1.Diag_10g, 
				_ => listOfSFXBasic1.Diag_10h, 
			};
		case GameCore.SFXBasic1.DiagToneGroup11:
			return GetMATH_RNG_INT(1, 3) switch
			{
				1 => listOfSFXBasic1.Diag_11a, 
				2 => listOfSFXBasic1.Diag_11b, 
				_ => listOfSFXBasic1.Diag_11c, 
			};
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
			return GetMATH_RNG_INT(1, 4) switch
			{
				1 => listOfSFXBasic2.Boing1, 
				2 => listOfSFXBasic2.Boing2, 
				3 => listOfSFXBasic2.Boing3, 
				_ => listOfSFXBasic2.Boing4, 
			};
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
		return thisType switch
		{
			GameCore.MonsterSounds.IsoboraSkill => listOfMonsterSound.IsoboraSkill, 
			GameCore.MonsterSounds.IsoboraGrab => listOfMonsterSound.IsoboraGrab, 
			GameCore.MonsterSounds.IsoboraDead => listOfMonsterSound.IsoboraDead, 
			GameCore.MonsterSounds.SpitBudDead => listOfMonsterSound.SpitBudDead, 
			GameCore.MonsterSounds.StarfishGrab => listOfMonsterSound.StarfishGrab, 
			GameCore.MonsterSounds.StarfishDead => listOfMonsterSound.StarfishDead, 
			GameCore.MonsterSounds.BarnacleDead => listOfMonsterSound.BarnacleDead, 
			GameCore.MonsterSounds.KitsuneGiggle => listOfMonsterSound.KitsuneGiggle, 
			GameCore.MonsterSounds.KitsuneDead => listOfMonsterSound.KitsuneDead, 
			GameCore.MonsterSounds.DFLYDead => listOfMonsterSound.DFLYDead, 
			GameCore.MonsterSounds.DFLYSkill => listOfMonsterSound.DFLYSkill, 
			GameCore.MonsterSounds.FairyDead => listOfMonsterSound.FairyDead, 
			GameCore.MonsterSounds.FairyLaugh => listOfMonsterSound.FairyLaugh, 
			GameCore.MonsterSounds.FairySkill => listOfMonsterSound.FairySkill, 
			GameCore.MonsterSounds.SalaTongue => listOfMonsterSound.SalaTongue, 
			GameCore.MonsterSounds.LizardDead => listOfMonsterSound.LizardDead, 
			GameCore.MonsterSounds.MimicHowl => listOfMonsterSound.MimicHowl, 
			GameCore.MonsterSounds.MimicJump => listOfMonsterSound.MimicJump, 
			GameCore.MonsterSounds.MimicLand => listOfMonsterSound.MimicLand, 
			GameCore.MonsterSounds.ArachneLaugh => listOfMonsterSound.ArachneLaugh, 
			GameCore.MonsterSounds.ArachneClimax => listOfMonsterSound.ArachneClimax, 
			GameCore.MonsterSounds.LamiaCast1 => listOfMonsterSound.LamiaCast1, 
			GameCore.MonsterSounds.LamiaCast2 => listOfMonsterSound.LamiaCast2, 
			GameCore.MonsterSounds.LamiaAttack1 => listOfMonsterSound.LamiaAttack1, 
			GameCore.MonsterSounds.LamiaAttack2 => listOfMonsterSound.LamiaAttack2, 
			GameCore.MonsterSounds.LamiaDead => listOfMonsterSound.LamiaDead, 
			GameCore.MonsterSounds.LamiaOpenMouth => listOfMonsterSound.LamiaOpenMouth, 
			GameCore.MonsterSounds.MushroomGirlLaugh => listOfMonsterSound.MushroomGirlLaugh, 
			GameCore.MonsterSounds.MushroomGirlDeath => listOfMonsterSound.MushroomGirlDeath, 
			GameCore.MonsterSounds.MushroomGirlSkillA => listOfMonsterSound.MushroomGirlSkillA, 
			GameCore.MonsterSounds.MushroomGirlSkillB => listOfMonsterSound.MushroomGirlSkillB, 
			GameCore.MonsterSounds.BatCall => listOfMonsterSound.BatCall, 
			GameCore.MonsterSounds.BatSwoop => listOfMonsterSound.BatSwoop, 
			GameCore.MonsterSounds.BatWings => listOfMonsterSound.BatWings, 
			GameCore.MonsterSounds.BatDead => listOfMonsterSound.BatDead, 
			GameCore.MonsterSounds.BatAhh => listOfMonsterSound.BatAhh, 
			GameCore.MonsterSounds.FutaGhostRing => listOfMonsterSoundPart2.FutaGhostRing, 
			GameCore.MonsterSounds.FutaGhostRing2 => listOfMonsterSoundPart2.FutaGhostRing2, 
			GameCore.MonsterSounds.FTG_Cast1 => listOfMonsterSoundPart2.FTG_Cast1, 
			GameCore.MonsterSounds.FTG_Cast2 => listOfMonsterSoundPart2.FTG_Cast2, 
			GameCore.MonsterSounds.FTG_Cast3 => listOfMonsterSoundPart2.FTG_Cast3, 
			GameCore.MonsterSounds.FTG_Dead => listOfMonsterSoundPart2.FTG_Dead, 
			GameCore.MonsterSounds.FTG_Hurt => listOfMonsterSoundPart2.FTG_Hurt, 
			GameCore.MonsterSounds.FTG_Cast_Random => Random.Range(0, 3) switch
			{
				1 => listOfMonsterSoundPart2.FTG_Cast2, 
				2 => listOfMonsterSoundPart2.FTG_Cast3, 
				_ => listOfMonsterSoundPart2.FTG_Cast1, 
			}, 
			GameCore.MonsterSounds.SwampFairy_Cast => listOfMonsterSoundPart2.SwampFairy_Cast, 
			GameCore.MonsterSounds.SwampFairy_Caw => listOfMonsterSoundPart2.SwampFairy_Caw, 
			GameCore.MonsterSounds.SwampFairy_Laugh => listOfMonsterSoundPart2.SwampFairy_Laugh, 
			GameCore.MonsterSounds.SwampFairy_Dead => listOfMonsterSoundPart2.SwampFairy_Dead, 
			GameCore.MonsterSounds.SwampFairyThunder => listOfMonsterSoundPart2.SwampFairy_Thunder, 
			GameCore.MonsterSounds.MothSkillCast => listOfMonsterSoundPart2.MothSkillCast, 
			GameCore.MonsterSounds.MothSkillExplode => listOfMonsterSoundPart2.MothSkillExplode, 
			GameCore.MonsterSounds.MothDead => listOfMonsterSoundPart2.MothDead, 
			GameCore.MonsterSounds.MothCasting => listOfMonsterSoundPart2.MothCasting, 
			GameCore.MonsterSounds.DwarfGrunt => listOfMonsterSoundPart2.DwarfGrunt, 
			GameCore.MonsterSounds.DwarfDeath => listOfMonsterSoundPart2.DwarfDeath, 
			GameCore.MonsterSounds.CTG_Disdain => listOfMonsterSoundPart2.CTG_Disdain, 
			GameCore.MonsterSounds.CTG_Snort => listOfMonsterSoundPart2.CTG_Snort, 
			GameCore.MonsterSounds.CTG_PreCHarge => listOfMonsterSoundPart2.CTG_PreCHarge, 
			GameCore.MonsterSounds.CTG_Gallop => listOfMonsterSoundPart2.CTG_Gallop, 
			GameCore.MonsterSounds.CTG_Dead => listOfMonsterSoundPart2.CTG_Dead, 
			GameCore.MonsterSounds.GSB_CastRandom => listOfMonsterSoundPart2.GSB_Cast2, 
			GameCore.MonsterSounds.GSB_Grunt1 => listOfMonsterSoundPart2.GSB_Grunt1, 
			GameCore.MonsterSounds.GSB_Grunt2 => listOfMonsterSoundPart2.GSB_Grunt2, 
			GameCore.MonsterSounds.Crystal_Cast => listOfMonsterSoundPart2.Crystal_Cast, 
			GameCore.MonsterSounds.Crystal_Shoot => listOfMonsterSoundPart2.Crystal_Shoot, 
			GameCore.MonsterSounds.Crystal_Chase => listOfMonsterSoundPart2.Crystal_Chase, 
			GameCore.MonsterSounds.Crystal_Dead => listOfMonsterSoundPart2.Crystal_Dead, 
			GameCore.MonsterSounds.Brute1 => listOfMonsterSoundPart2.Brute1, 
			GameCore.MonsterSounds.Brute2 => listOfMonsterSoundPart2.Brute2, 
			GameCore.MonsterSounds.PaintingEvil_Appear => listOfMonsterSoundPart3.PaintingEvil_Appear, 
			GameCore.MonsterSounds.PaintingEvil_Splat => listOfMonsterSoundPart3.PaintingEvil_Splat, 
			GameCore.MonsterSounds.LatexMaiden_Open => listOfMonsterSoundPart3.LatexMaiden_Open, 
			GameCore.MonsterSounds.LatexMaiden_Close => listOfMonsterSoundPart3.LatexMaiden_Close, 
			GameCore.MonsterSounds.LatexMaiden_Dead => listOfMonsterSoundPart3.LatexMaiden_Dead, 
			GameCore.MonsterSounds.Domme_Skill => listOfMonsterSoundPart3.Domme_Skill, 
			GameCore.MonsterSounds.Domme_Crack => listOfMonsterSoundPart3.Domme_Crack, 
			GameCore.MonsterSounds.Domme_Dead => listOfMonsterSoundPart3.Domme_Dead, 
			GameCore.MonsterSounds.Domme_Dodge => listOfMonsterSoundPart3.Domme_Dodge, 
			GameCore.MonsterSounds.Domme_Swing => listOfMonsterSoundPart3.Domme_Swing, 
			GameCore.MonsterSounds.CityDogSkill => listOfMonsterSoundPart3.CityDogSkill, 
			GameCore.MonsterSounds.CityDogBark => listOfMonsterSoundPart3.CityDogBark, 
			GameCore.MonsterSounds.CityDogPanting => listOfMonsterSoundPart3.CityDogPanting, 
			GameCore.MonsterSounds.CityDogGroaning => listOfMonsterSoundPart3.CityDogGroaning, 
			GameCore.MonsterSounds.CityDogBabyBark => listOfMonsterSoundPart3.CityDogBabyBark, 
			GameCore.MonsterSounds.CityDogBabyWhine => listOfMonsterSoundPart3.CityDogBabyWhine, 
			GameCore.MonsterSounds.BunnyCast => listOfMonsterSoundPart3.BunnyCast, 
			GameCore.MonsterSounds.BunnyDead => listOfMonsterSoundPart3.BunnyDead, 
			GameCore.MonsterSounds.PirateCast => listOfMonsterSoundPart3.PirateCast, 
			GameCore.MonsterSounds.PirateSpin => listOfMonsterSoundPart3.PirateSpin, 
			GameCore.MonsterSounds.PirateDead => listOfMonsterSoundPart3.PirateDead, 
			GameCore.MonsterSounds.WormCast => listOfMonsterSoundPart3.WormCast, 
			GameCore.MonsterSounds.WormDead => listOfMonsterSoundPart3.WormDead, 
			GameCore.MonsterSounds.WormDigDown => listOfMonsterSoundPart3.WormDigDown, 
			GameCore.MonsterSounds.WormDigUp => listOfMonsterSoundPart3.WormDigUp, 
			GameCore.MonsterSounds.WormMelee => listOfMonsterSoundPart3.WormMelee, 
			GameCore.MonsterSounds.RobotAlarm1 => listOfMonsterSoundPart3.Robot_Alarm_1, 
			GameCore.MonsterSounds.RobotAlarm2 => listOfMonsterSoundPart3.Robot_Alarm_2, 
			GameCore.MonsterSounds.RobotAlarm3 => listOfMonsterSoundPart3.Robot_Alarm_3, 
			GameCore.MonsterSounds.RobotSkill1 => listOfMonsterSoundPart3.Robot_Skill_1, 
			GameCore.MonsterSounds.RobotSkill2 => listOfMonsterSoundPart3.Robot_Skill_2, 
			GameCore.MonsterSounds.RobotSkill3 => listOfMonsterSoundPart3.Robot_Skill_3, 
			GameCore.MonsterSounds.RobotExplode => listOfMonsterSoundPart3.Robot_Explode, 
			GameCore.MonsterSounds.WyvernCast => listOfMonsterSoundPart3.WyvernCast, 
			GameCore.MonsterSounds.WyvernSkill => listOfMonsterSoundPart3.WyvernSkill, 
			GameCore.MonsterSounds.WyvernDead => listOfMonsterSoundPart3.WyvernDead, 
			GameCore.MonsterSounds.WyvernBirth => listOfMonsterSoundPart3.WyvernBirth, 
			GameCore.MonsterSounds.BeeGirlCast => listOfMonsterSoundPart3.BeeGirlCast, 
			GameCore.MonsterSounds.BeeGirlChase => listOfMonsterSoundPart3.BeeGirlChase, 
			GameCore.MonsterSounds.BeeGirlDead => listOfMonsterSoundPart3.BeeGirlDead, 
			GameCore.MonsterSounds.FutaCentaurHeal => listOfMonsterSoundPart3.FutaCentaurHeal, 
			GameCore.MonsterSounds.FutaCentaurAttack => listOfMonsterSoundPart3.FutaCentaurAttack, 
			GameCore.MonsterSounds.FutaCentaurDead => listOfMonsterSoundPart3.FutaCentaurDead, 
			GameCore.MonsterSounds.BIS_Cast => listOfMonsterSoundPart3.BIS_Cast, 
			GameCore.MonsterSounds.BIS_Skill => listOfMonsterSoundPart3.BIS_Skill, 
			GameCore.MonsterSounds.BIS_Dead => listOfMonsterSoundPart3.BIS_Dead, 
			GameCore.MonsterSounds.FLR_Cast => listOfMonsterSoundPart3.FLR_Cast, 
			GameCore.MonsterSounds.FLR_Pound => listOfMonsterSoundPart3.FLR_Pound, 
			GameCore.MonsterSounds.FLR_Dead => listOfMonsterSoundPart3.FLR_Dead, 
			GameCore.MonsterSounds.FHB_Tentacles => listOfMonsterSoundPart3.FHB_Tentacles, 
			GameCore.MonsterSounds.FHB_Dead => listOfMonsterSoundPart3.FHB_Dead, 
			GameCore.MonsterSounds.CRB_Cast => listOfMonsterSoundPart3.CRB_Cast, 
			GameCore.MonsterSounds.CRB_Skill => listOfMonsterSoundPart3.CRB_Skill, 
			GameCore.MonsterSounds.CRB_Dead => listOfMonsterSoundPart3.CRB_Dead, 
			GameCore.MonsterSounds.SCB_Cast => listOfMonsterSoundPart4.Sinccubus_Cast, 
			GameCore.MonsterSounds.SCB_Skill => listOfMonsterSoundPart4.Sinccubus_Skill, 
			GameCore.MonsterSounds.SCB_Chase => listOfMonsterSoundPart4.Sinccubus_Chase, 
			GameCore.MonsterSounds.SCB_Dead => listOfMonsterSoundPart4.Sinccubus_Dead, 
			GameCore.MonsterSounds.DKD_Cast => listOfMonsterSoundPart4.DarkDamsel_Cast, 
			GameCore.MonsterSounds.DKD_Skill => listOfMonsterSoundPart4.DarkDamsel_Skill, 
			GameCore.MonsterSounds.DKD_Dead => listOfMonsterSoundPart4.DarkDamsel_Dead, 
			GameCore.MonsterSounds.MDS_Cast => listOfMonsterSoundPart4.Medusa_Casting, 
			GameCore.MonsterSounds.MDS_Petr => listOfMonsterSoundPart4.Medusa_PetrifyingDebuff, 
			GameCore.MonsterSounds.MDS_Stiff => listOfMonsterSoundPart4.Medusa_StiffDebuff, 
			GameCore.MonsterSounds.MDS_Dead => listOfMonsterSoundPart4.Medusa_Dead, 
			GameCore.MonsterSounds.FairySorrowDead => listOfMonsterSoundPart4.FairySorrowDead, 
			GameCore.MonsterSounds.GoblinLaugh => listOfMonsterSoundPart4.GoblinLaugh, 
			GameCore.MonsterSounds.CatHiss => listOfMonsterSoundPart4.CatHiss, 
			GameCore.MonsterSounds.CatNyaOther => listOfMonsterSoundPart4.CatNyaOther, 
			GameCore.MonsterSounds.CatNya1 => listOfMonsterSoundPart4.CatNya1, 
			GameCore.MonsterSounds.CatNya2 => listOfMonsterSoundPart4.CatNya2, 
			GameCore.MonsterSounds.CatNya3 => listOfMonsterSoundPart4.CatNya3, 
			GameCore.MonsterSounds.CatNya4 => listOfMonsterSoundPart4.CatNya4, 
			GameCore.MonsterSounds.CatNya5 => listOfMonsterSoundPart4.CatNya5, 
			_ => null, 
		};
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
			return rndNumber switch
			{
				1 => listOfEroSound.Rubber1, 
				2 => listOfEroSound.Rubber2, 
				3 => listOfEroSound.Rubber3, 
				4 => listOfEroSound.Rubber4, 
				5 => listOfEroSound.Rubber5, 
				6 => listOfEroSound.Rubber6, 
				_ => listOfEroSound.Rubber7, 
			};
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
		return text2 switch
		{
			"BJLight" => RETURN_MV_BJLIGHT(thisType), 
			"BJMedium" => RETURN_MV_BJMEDIUM(thisType), 
			"BJHeavy" => RETURN_MV_BJHEAVY(thisType), 
			"BJClimax" => RETURN_MV_BJCLIMAX(thisType), 
			"BJLick" => RETURN_MV_BJLICK(thisType), 
			"BJMisc" => RETURN_MV_BJMISC(thisType), 
			"ClosedMouth" => RETURN_MV_CLOSEDMOUTH(thisType), 
			"MoanLight" => RETURN_MV_MOANLIGHT(thisType), 
			"MoanMedium" => RETURN_MV_MOANMEDIUM(thisType), 
			"MoanHeavy" => RETURN_MV_MOANHEAVY(thisType), 
			"MoanRough" => RETURN_MV_MOANROUGH(thisType), 
			"Climax" => RETURN_MV_CLIMAX(thisType), 
			"Attack" => RETURN_MV_ATTACK(thisType), 
			"HurtPain" => RETURN_MV_HURTPAIN(thisType), 
			"Gasp" => RETURN_MV_GASP(thisType), 
			"Scream" => RETURN_MV_SCREAM(thisType), 
			"Swallow" => RETURN_MV_SWALLOW(thisType), 
			"MBBLogo" => RETURN_MV_MBBLOGO(thisType), 
			_ => null, 
		};
	}

	private AudioClip RETURN_MV_BJLIGHT(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.BJLight2 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight2, 
			GameCore.MayaVoice.BJLight3 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight3, 
			GameCore.MayaVoice.BJLight4 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight4, 
			GameCore.MayaVoice.BJLight5 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight5, 
			GameCore.MayaVoice.BJLight6 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight6, 
			GameCore.MayaVoice.BJLight7 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight7, 
			GameCore.MayaVoice.BJLight8 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight8, 
			GameCore.MayaVoice.BJLight9 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight9, 
			GameCore.MayaVoice.BJLight10 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight10, 
			GameCore.MayaVoice.BJLight11 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight11, 
			GameCore.MayaVoice.BJLight12 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight12, 
			GameCore.MayaVoice.BJLight13 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight13, 
			GameCore.MayaVoice.BJLight14 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight14, 
			GameCore.MayaVoice.BJLight15 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight15, 
			GameCore.MayaVoice.BJLight16 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight16, 
			GameCore.MayaVoice.BJLight17 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight17, 
			GameCore.MayaVoice.BJLight18 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight18, 
			GameCore.MayaVoice.BJLight19 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight19, 
			GameCore.MayaVoice.BJLight20 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight20, 
			GameCore.MayaVoice.BJLight21 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight21, 
			GameCore.MayaVoice.BJLight22 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight22, 
			GameCore.MayaVoice.BJLight23 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight23, 
			GameCore.MayaVoice.BJLight24 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight24, 
			GameCore.MayaVoice.BJLight25 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight25, 
			GameCore.MayaVoice.BJLight26 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight26, 
			GameCore.MayaVoice.BJLight27 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight27, 
			GameCore.MayaVoice.BJLight28 => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight28, 
			_ => listOfMayaVoice.BlowjobMoanLight.BlowjobMoanLight1, 
		};
	}

	private AudioClip RETURN_MV_BJMEDIUM(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.BJMedium2 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium2, 
			GameCore.MayaVoice.BJMedium3 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium3, 
			GameCore.MayaVoice.BJMedium4 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium4, 
			GameCore.MayaVoice.BJMedium5 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium5, 
			GameCore.MayaVoice.BJMedium6 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium6, 
			GameCore.MayaVoice.BJMedium7 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium7, 
			GameCore.MayaVoice.BJMedium8 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium8, 
			GameCore.MayaVoice.BJMedium9 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium9, 
			GameCore.MayaVoice.BJMedium10 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium10, 
			GameCore.MayaVoice.BJMedium11 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium11, 
			GameCore.MayaVoice.BJMedium12 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium12, 
			GameCore.MayaVoice.BJMedium13 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium13, 
			GameCore.MayaVoice.BJMedium14 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium14, 
			GameCore.MayaVoice.BJMedium15 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium15, 
			GameCore.MayaVoice.BJMedium16 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium16, 
			GameCore.MayaVoice.BJMedium17 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium17, 
			GameCore.MayaVoice.BJMedium18 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium18, 
			GameCore.MayaVoice.BJMedium19 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium19, 
			GameCore.MayaVoice.BJMedium20 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium20, 
			GameCore.MayaVoice.BJMedium21 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium21, 
			GameCore.MayaVoice.BJMedium22 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium22, 
			GameCore.MayaVoice.BJMedium23 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium23, 
			GameCore.MayaVoice.BJMedium24 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium24, 
			GameCore.MayaVoice.BJMedium25 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium25, 
			GameCore.MayaVoice.BJMedium26 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium26, 
			GameCore.MayaVoice.BJMedium27 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium27, 
			GameCore.MayaVoice.BJMedium28 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium28, 
			GameCore.MayaVoice.BJMedium29 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium29, 
			GameCore.MayaVoice.BJMedium30 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium30, 
			GameCore.MayaVoice.BJMedium31 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium31, 
			GameCore.MayaVoice.BJMedium32 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium32, 
			GameCore.MayaVoice.BJMedium33 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium33, 
			GameCore.MayaVoice.BJMedium34 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium34, 
			GameCore.MayaVoice.BJMedium35 => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium35, 
			_ => listOfMayaVoice.BlowjobMoanMedium.BlowjobMoanMedium1, 
		};
	}

	private AudioClip RETURN_MV_BJHEAVY(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.BJHeavy2 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy2, 
			GameCore.MayaVoice.BJHeavy3 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy3, 
			GameCore.MayaVoice.BJHeavy4 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy4, 
			GameCore.MayaVoice.BJHeavy5 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy5, 
			GameCore.MayaVoice.BJHeavy6 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy6, 
			GameCore.MayaVoice.BJHeavy7 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy7, 
			GameCore.MayaVoice.BJHeavy8 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy8, 
			GameCore.MayaVoice.BJHeavy9 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy9, 
			GameCore.MayaVoice.BJHeavy10 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy10, 
			GameCore.MayaVoice.BJHeavy11 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy11, 
			GameCore.MayaVoice.BJHeavy12 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy12, 
			GameCore.MayaVoice.BJHeavy13 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy13, 
			GameCore.MayaVoice.BJHeavy14 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy14, 
			GameCore.MayaVoice.BJHeavy15 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy15, 
			GameCore.MayaVoice.BJHeavy16 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy16, 
			GameCore.MayaVoice.BJHeavy17 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy17, 
			GameCore.MayaVoice.BJHeavy18 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy18, 
			GameCore.MayaVoice.BJHeavy19 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy19, 
			GameCore.MayaVoice.BJHeavy20 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy20, 
			GameCore.MayaVoice.BJHeavy21 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy21, 
			GameCore.MayaVoice.BJHeavy22 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy22, 
			GameCore.MayaVoice.BJHeavy23 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy23, 
			GameCore.MayaVoice.BJHeavy24 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy24, 
			GameCore.MayaVoice.BJHeavy25 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy25, 
			GameCore.MayaVoice.BJHeavy26 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy26, 
			GameCore.MayaVoice.BJHeavy27 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy27, 
			GameCore.MayaVoice.BJHeavy28 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy28, 
			GameCore.MayaVoice.BJHeavy29 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy29, 
			GameCore.MayaVoice.BJHeavy30 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy30, 
			GameCore.MayaVoice.BJHeavy31 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy31, 
			GameCore.MayaVoice.BJHeavy32 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy32, 
			GameCore.MayaVoice.BJHeavy33 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy33, 
			GameCore.MayaVoice.BJHeavy34 => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy34, 
			_ => listOfMayaVoice.BlowjobMoanHeavy.BlowjobMoanHeavy1, 
		};
	}

	private AudioClip RETURN_MV_BJCLIMAX(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.BJClimax2 => listOfMayaVoice.BlowjobClimax.BlowjobClimax2, 
			GameCore.MayaVoice.BJClimax3 => listOfMayaVoice.BlowjobClimax.BlowjobClimax3, 
			GameCore.MayaVoice.BJClimax4 => listOfMayaVoice.BlowjobClimax.BlowjobClimax4, 
			GameCore.MayaVoice.BJClimax5 => listOfMayaVoice.BlowjobClimax.BlowjobClimax5, 
			GameCore.MayaVoice.BJClimax6 => listOfMayaVoice.BlowjobClimax.BlowjobClimax6, 
			GameCore.MayaVoice.BJClimax7 => listOfMayaVoice.BlowjobClimax.BlowjobClimax7, 
			GameCore.MayaVoice.BJClimax8 => listOfMayaVoice.BlowjobClimax.BlowjobClimax8, 
			GameCore.MayaVoice.BJClimax9 => listOfMayaVoice.BlowjobClimax.BlowjobClimax9, 
			GameCore.MayaVoice.BJClimax10 => listOfMayaVoice.BlowjobClimax.BlowjobClimax10, 
			GameCore.MayaVoice.BJClimax11 => listOfMayaVoice.BlowjobClimax.BlowjobClimax11, 
			GameCore.MayaVoice.BJClimax12 => listOfMayaVoice.BlowjobClimax.BlowjobClimax12, 
			GameCore.MayaVoice.BJClimax13 => listOfMayaVoice.BlowjobClimax.BlowjobClimax13, 
			GameCore.MayaVoice.BJClimax14 => listOfMayaVoice.BlowjobClimax.BlowjobClimax14, 
			GameCore.MayaVoice.BJClimax15 => listOfMayaVoice.BlowjobClimax.BlowjobClimax15, 
			GameCore.MayaVoice.BJClimax16 => listOfMayaVoice.BlowjobClimax.BlowjobClimax16, 
			GameCore.MayaVoice.BJClimax17 => listOfMayaVoice.BlowjobClimax.BlowjobClimax17, 
			GameCore.MayaVoice.BJClimax18 => listOfMayaVoice.BlowjobClimax.BlowjobClimax18, 
			GameCore.MayaVoice.BJClimax19 => listOfMayaVoice.BlowjobClimax.BlowjobClimax19, 
			GameCore.MayaVoice.BJClimax20 => listOfMayaVoice.BlowjobClimax.BlowjobClimax20, 
			GameCore.MayaVoice.BJClimax21 => listOfMayaVoice.BlowjobClimax.BlowjobClimax21, 
			GameCore.MayaVoice.BJClimax22 => listOfMayaVoice.BlowjobClimax.BlowjobClimax22, 
			GameCore.MayaVoice.BJClimax23 => listOfMayaVoice.BlowjobClimax.BlowjobClimax23, 
			GameCore.MayaVoice.BJClimax24 => listOfMayaVoice.BlowjobClimax.BlowjobClimax24, 
			GameCore.MayaVoice.BJClimax25 => listOfMayaVoice.BlowjobClimax.BlowjobClimax25, 
			GameCore.MayaVoice.BJClimax26 => listOfMayaVoice.BlowjobClimax.BlowjobClimax26, 
			GameCore.MayaVoice.BJClimax27 => listOfMayaVoice.BlowjobClimax.BlowjobClimax27, 
			GameCore.MayaVoice.BJClimax28 => listOfMayaVoice.BlowjobClimax.BlowjobClimax28, 
			GameCore.MayaVoice.BJClimax29 => listOfMayaVoice.BlowjobClimax.BlowjobClimax29, 
			GameCore.MayaVoice.BJClimax30 => listOfMayaVoice.BlowjobClimax.BlowjobClimax30, 
			GameCore.MayaVoice.BJClimax31 => listOfMayaVoice.BlowjobClimax.BlowjobClimax31, 
			GameCore.MayaVoice.BJClimax32 => listOfMayaVoice.BlowjobClimax.BlowjobClimax32, 
			GameCore.MayaVoice.BJClimax33 => listOfMayaVoice.BlowjobClimax.BlowjobClimax33, 
			GameCore.MayaVoice.BJClimax34 => listOfMayaVoice.BlowjobClimax.BlowjobClimax34, 
			GameCore.MayaVoice.BJClimax35 => listOfMayaVoice.BlowjobClimax.BlowjobClimax35, 
			_ => listOfMayaVoice.BlowjobClimax.BlowjobClimax1, 
		};
	}

	private AudioClip RETURN_MV_BJLICK(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.BJLick2 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick2, 
			GameCore.MayaVoice.BJLick3 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick3, 
			GameCore.MayaVoice.BJLick4 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick4, 
			GameCore.MayaVoice.BJLick5 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick5, 
			GameCore.MayaVoice.BJLick6 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick6, 
			GameCore.MayaVoice.BJLick7 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick7, 
			GameCore.MayaVoice.BJLick8 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick8, 
			GameCore.MayaVoice.BJLick9 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick9, 
			GameCore.MayaVoice.BJLick10 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick10, 
			GameCore.MayaVoice.BJLick11 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick11, 
			GameCore.MayaVoice.BJLick12 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick12, 
			GameCore.MayaVoice.BJLick13 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick13, 
			GameCore.MayaVoice.BJLick14 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick14, 
			GameCore.MayaVoice.BJLick15 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick15, 
			GameCore.MayaVoice.BJLick16 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick16, 
			GameCore.MayaVoice.BJLick17 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick17, 
			GameCore.MayaVoice.BJLick18 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick18, 
			GameCore.MayaVoice.BJLick19 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick19, 
			GameCore.MayaVoice.BJLick20 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick20, 
			GameCore.MayaVoice.BJLick21 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick21, 
			GameCore.MayaVoice.BJLick22 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick22, 
			GameCore.MayaVoice.BJLick23 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick23, 
			GameCore.MayaVoice.BJLick24 => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick24, 
			_ => listOfMayaVoice.BlowjobOpenLick.BlowjobOpenLick1, 
		};
	}

	private AudioClip RETURN_MV_BJMISC(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.BJMisc2 => listOfMayaVoice.BlowjobMisc.BlowjobMisc2, 
			GameCore.MayaVoice.BJMisc3 => listOfMayaVoice.BlowjobMisc.BlowjobMisc3, 
			GameCore.MayaVoice.BJMisc4 => listOfMayaVoice.BlowjobMisc.BlowjobMisc4, 
			GameCore.MayaVoice.BJMisc5 => listOfMayaVoice.BlowjobMisc.BlowjobMisc5, 
			GameCore.MayaVoice.BJMisc6 => listOfMayaVoice.BlowjobMisc.BlowjobMisc6, 
			GameCore.MayaVoice.BJMisc7 => listOfMayaVoice.BlowjobMisc.BlowjobMisc7, 
			GameCore.MayaVoice.BJMisc8 => listOfMayaVoice.BlowjobMisc.BlowjobMisc8, 
			GameCore.MayaVoice.BJMisc9 => listOfMayaVoice.BlowjobMisc.BlowjobMisc9, 
			GameCore.MayaVoice.BJMisc10 => listOfMayaVoice.BlowjobMisc.BlowjobMisc10, 
			GameCore.MayaVoice.BJMisc11 => listOfMayaVoice.BlowjobMisc.BlowjobMisc11, 
			GameCore.MayaVoice.BJMisc12 => listOfMayaVoice.BlowjobMisc.BlowjobMisc12, 
			GameCore.MayaVoice.BJMisc13 => listOfMayaVoice.BlowjobMisc.BlowjobMisc13, 
			GameCore.MayaVoice.BJMisc14 => listOfMayaVoice.BlowjobMisc.BlowjobMisc14, 
			GameCore.MayaVoice.BJMisc15 => listOfMayaVoice.BlowjobMisc.BlowjobMisc15, 
			GameCore.MayaVoice.BJMisc16 => listOfMayaVoice.BlowjobMisc.BlowjobMisc16, 
			GameCore.MayaVoice.BJMisc17 => listOfMayaVoice.BlowjobMisc.BlowjobMisc17, 
			GameCore.MayaVoice.BJMisc18 => listOfMayaVoice.BlowjobMisc.BlowjobMisc18, 
			GameCore.MayaVoice.BJMisc19 => listOfMayaVoice.BlowjobMisc.BlowjobMisc19, 
			GameCore.MayaVoice.BJMisc20 => listOfMayaVoice.BlowjobMisc.BlowjobMisc20, 
			GameCore.MayaVoice.BJMisc21 => listOfMayaVoice.BlowjobMisc.BlowjobMisc21, 
			GameCore.MayaVoice.BJMisc22 => listOfMayaVoice.BlowjobMisc.BlowjobMisc22, 
			GameCore.MayaVoice.BJMisc23 => listOfMayaVoice.BlowjobMisc.BlowjobMisc23, 
			GameCore.MayaVoice.BJMisc24 => listOfMayaVoice.BlowjobMisc.BlowjobMisc24, 
			GameCore.MayaVoice.BJMisc25 => listOfMayaVoice.BlowjobMisc.BlowjobMisc25, 
			GameCore.MayaVoice.BJMisc26 => listOfMayaVoice.BlowjobMisc.BlowjobMisc26, 
			_ => listOfMayaVoice.BlowjobMisc.BlowjobMisc1, 
		};
	}

	private AudioClip RETURN_MV_CLOSEDMOUTH(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.ClosedMouth2 => listOfMayaVoice.ClosedMouth.ClosedMouth2, 
			GameCore.MayaVoice.ClosedMouth3 => listOfMayaVoice.ClosedMouth.ClosedMouth3, 
			GameCore.MayaVoice.ClosedMouth4 => listOfMayaVoice.ClosedMouth.ClosedMouth4, 
			GameCore.MayaVoice.ClosedMouth5 => listOfMayaVoice.ClosedMouth.ClosedMouth5, 
			GameCore.MayaVoice.ClosedMouth6 => listOfMayaVoice.ClosedMouth.ClosedMouth6, 
			GameCore.MayaVoice.ClosedMouth7 => listOfMayaVoice.ClosedMouth.ClosedMouth7, 
			GameCore.MayaVoice.ClosedMouth8 => listOfMayaVoice.ClosedMouth.ClosedMouth8, 
			GameCore.MayaVoice.ClosedMouth9 => listOfMayaVoice.ClosedMouth.ClosedMouth9, 
			GameCore.MayaVoice.ClosedMouth10 => listOfMayaVoice.ClosedMouth.ClosedMouth10, 
			GameCore.MayaVoice.ClosedMouth11 => listOfMayaVoice.ClosedMouth.ClosedMouth11, 
			GameCore.MayaVoice.ClosedMouth12 => listOfMayaVoice.ClosedMouth.ClosedMouth12, 
			GameCore.MayaVoice.ClosedMouth13 => listOfMayaVoice.ClosedMouth.ClosedMouth13, 
			GameCore.MayaVoice.ClosedMouth14 => listOfMayaVoice.ClosedMouth.ClosedMouth14, 
			GameCore.MayaVoice.ClosedMouth15 => listOfMayaVoice.ClosedMouth.ClosedMouth15, 
			GameCore.MayaVoice.ClosedMouth16 => listOfMayaVoice.ClosedMouth.ClosedMouth16, 
			GameCore.MayaVoice.ClosedMouth17 => listOfMayaVoice.ClosedMouth.ClosedMouth17, 
			_ => listOfMayaVoice.ClosedMouth.ClosedMouth1, 
		};
	}

	private AudioClip RETURN_MV_MOANLIGHT(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.MoanLight2 => listOfMayaVoice.MoanLight.MoanLight2, 
			GameCore.MayaVoice.MoanLight3 => listOfMayaVoice.MoanLight.MoanLight3, 
			GameCore.MayaVoice.MoanLight4 => listOfMayaVoice.MoanLight.MoanLight4, 
			GameCore.MayaVoice.MoanLight5 => listOfMayaVoice.MoanLight.MoanLight5, 
			GameCore.MayaVoice.MoanLight6 => listOfMayaVoice.MoanLight.MoanLight6, 
			GameCore.MayaVoice.MoanLight7 => listOfMayaVoice.MoanLight.MoanLight7, 
			GameCore.MayaVoice.MoanLight8 => listOfMayaVoice.MoanLight.MoanLight8, 
			GameCore.MayaVoice.MoanLight9 => listOfMayaVoice.MoanLight.MoanLight9, 
			GameCore.MayaVoice.MoanLight10 => listOfMayaVoice.MoanLight.MoanLight10, 
			GameCore.MayaVoice.MoanLight11 => listOfMayaVoice.MoanLight.MoanLight11, 
			GameCore.MayaVoice.MoanLight12 => listOfMayaVoice.MoanLight.MoanLight12, 
			GameCore.MayaVoice.MoanLight13 => listOfMayaVoice.MoanLight.MoanLight13, 
			GameCore.MayaVoice.MoanLight14 => listOfMayaVoice.MoanLight.MoanLight14, 
			GameCore.MayaVoice.MoanLight15 => listOfMayaVoice.MoanLight.MoanLight15, 
			GameCore.MayaVoice.MoanLight16 => listOfMayaVoice.MoanLight.MoanLight16, 
			GameCore.MayaVoice.MoanLight17 => listOfMayaVoice.MoanLight.MoanLight17, 
			GameCore.MayaVoice.MoanLight18 => listOfMayaVoice.MoanLight.MoanLight18, 
			GameCore.MayaVoice.MoanLight19 => listOfMayaVoice.MoanLight.MoanLight19, 
			_ => listOfMayaVoice.MoanLight.MoanLight1, 
		};
	}

	private AudioClip RETURN_MV_MOANMEDIUM(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.MoanMedium2 => listOfMayaVoice.MoanMedium.MoanMedium2, 
			GameCore.MayaVoice.MoanMedium3 => listOfMayaVoice.MoanMedium.MoanMedium3, 
			GameCore.MayaVoice.MoanMedium4 => listOfMayaVoice.MoanMedium.MoanMedium4, 
			GameCore.MayaVoice.MoanMedium5 => listOfMayaVoice.MoanMedium.MoanMedium5, 
			GameCore.MayaVoice.MoanMedium6 => listOfMayaVoice.MoanMedium.MoanMedium6, 
			GameCore.MayaVoice.MoanMedium7 => listOfMayaVoice.MoanMedium.MoanMedium7, 
			GameCore.MayaVoice.MoanMedium8 => listOfMayaVoice.MoanMedium.MoanMedium8, 
			GameCore.MayaVoice.MoanMedium9 => listOfMayaVoice.MoanMedium.MoanMedium9, 
			GameCore.MayaVoice.MoanMedium10 => listOfMayaVoice.MoanMedium.MoanMedium10, 
			GameCore.MayaVoice.MoanMedium11 => listOfMayaVoice.MoanMedium.MoanMedium11, 
			_ => listOfMayaVoice.MoanMedium.MoanMedium1, 
		};
	}

	private AudioClip RETURN_MV_MOANHEAVY(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.MoanHeavy2 => listOfMayaVoice.MoanHeavy.MoanHeavy2, 
			GameCore.MayaVoice.MoanHeavy3 => listOfMayaVoice.MoanHeavy.MoanHeavy3, 
			GameCore.MayaVoice.MoanHeavy4 => listOfMayaVoice.MoanHeavy.MoanHeavy4, 
			GameCore.MayaVoice.MoanHeavy5 => listOfMayaVoice.MoanHeavy.MoanHeavy5, 
			GameCore.MayaVoice.MoanHeavy6 => listOfMayaVoice.MoanHeavy.MoanHeavy6, 
			GameCore.MayaVoice.MoanHeavy7 => listOfMayaVoice.MoanHeavy.MoanHeavy7, 
			GameCore.MayaVoice.MoanHeavy8 => listOfMayaVoice.MoanHeavy.MoanHeavy8, 
			GameCore.MayaVoice.MoanHeavy9 => listOfMayaVoice.MoanHeavy.MoanHeavy9, 
			GameCore.MayaVoice.MoanHeavy10 => listOfMayaVoice.MoanHeavy.MoanHeavy10, 
			GameCore.MayaVoice.MoanHeavy11 => listOfMayaVoice.MoanHeavy.MoanHeavy11, 
			_ => listOfMayaVoice.MoanHeavy.MoanHeavy1, 
		};
	}

	private AudioClip RETURN_MV_MOANROUGH(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.MoanRough2 => listOfMayaVoice.MoanRough.MoanRough2, 
			GameCore.MayaVoice.MoanRough3 => listOfMayaVoice.MoanRough.MoanRough3, 
			GameCore.MayaVoice.MoanRough4 => listOfMayaVoice.MoanRough.MoanRough4, 
			GameCore.MayaVoice.MoanRough5 => listOfMayaVoice.MoanRough.MoanRough5, 
			GameCore.MayaVoice.MoanRough6 => listOfMayaVoice.MoanRough.MoanRough6, 
			GameCore.MayaVoice.MoanRough7 => listOfMayaVoice.MoanRough.MoanRough7, 
			GameCore.MayaVoice.MoanRough8 => listOfMayaVoice.MoanRough.MoanRough8, 
			GameCore.MayaVoice.MoanRough9 => listOfMayaVoice.MoanRough.MoanRough9, 
			GameCore.MayaVoice.MoanRough10 => listOfMayaVoice.MoanRough.MoanRough10, 
			GameCore.MayaVoice.MoanRough11 => listOfMayaVoice.MoanRough.MoanRough11, 
			GameCore.MayaVoice.MoanRough12 => listOfMayaVoice.MoanRough.MoanRough12, 
			_ => listOfMayaVoice.MoanRough.MoanRough1, 
		};
	}

	private AudioClip RETURN_MV_CLIMAX(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.Climax2 => listOfMayaVoice.Climax.Climax2, 
			GameCore.MayaVoice.Climax3 => listOfMayaVoice.Climax.Climax3, 
			GameCore.MayaVoice.Climax4 => listOfMayaVoice.Climax.Climax4, 
			GameCore.MayaVoice.Climax5 => listOfMayaVoice.Climax.Climax5, 
			GameCore.MayaVoice.Climax6 => listOfMayaVoice.Climax.Climax6, 
			GameCore.MayaVoice.Climax7 => listOfMayaVoice.Climax.Climax7, 
			GameCore.MayaVoice.Climax8 => listOfMayaVoice.Climax.Climax8, 
			GameCore.MayaVoice.Climax9 => listOfMayaVoice.Climax.Climax9, 
			GameCore.MayaVoice.Climax10 => listOfMayaVoice.Climax.Climax10, 
			GameCore.MayaVoice.Climax11 => listOfMayaVoice.Climax.Climax11, 
			_ => listOfMayaVoice.Climax.Climax1, 
		};
	}

	private AudioClip RETURN_MV_ATTACK(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.Attack2 => listOfMayaVoice.Attack.Attack2, 
			GameCore.MayaVoice.Attack3 => listOfMayaVoice.Attack.Attack3, 
			GameCore.MayaVoice.Attack4 => listOfMayaVoice.Attack.Attack4, 
			GameCore.MayaVoice.Attack5 => listOfMayaVoice.Attack.Attack5, 
			GameCore.MayaVoice.Attack6 => listOfMayaVoice.Attack.Attack6, 
			GameCore.MayaVoice.Attack7 => listOfMayaVoice.Attack.Attack7, 
			GameCore.MayaVoice.Attack8 => listOfMayaVoice.Attack.Attack8, 
			GameCore.MayaVoice.Attack9 => listOfMayaVoice.Attack.Attack9, 
			GameCore.MayaVoice.Attack10 => listOfMayaVoice.Attack.Attack10, 
			GameCore.MayaVoice.Attack11 => listOfMayaVoice.Attack.Attack11, 
			GameCore.MayaVoice.Attack12 => listOfMayaVoice.Attack.Attack12, 
			GameCore.MayaVoice.Attack13 => listOfMayaVoice.Attack.Attack13, 
			GameCore.MayaVoice.Attack14 => listOfMayaVoice.Attack.Attack14, 
			GameCore.MayaVoice.Attack15 => listOfMayaVoice.Attack.Attack15, 
			GameCore.MayaVoice.Attack16 => listOfMayaVoice.Attack.Attack16, 
			GameCore.MayaVoice.Attack17 => listOfMayaVoice.Attack.Attack17, 
			_ => listOfMayaVoice.Attack.Attack1, 
		};
	}

	private AudioClip RETURN_MV_HURTPAIN(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.HurtPain2 => listOfMayaVoice.HurtPain.HurtPain2, 
			GameCore.MayaVoice.HurtPain3 => listOfMayaVoice.HurtPain.HurtPain3, 
			GameCore.MayaVoice.HurtPain4 => listOfMayaVoice.HurtPain.HurtPain4, 
			GameCore.MayaVoice.HurtPain5 => listOfMayaVoice.HurtPain.HurtPain5, 
			GameCore.MayaVoice.HurtPain6 => listOfMayaVoice.HurtPain.HurtPain6, 
			GameCore.MayaVoice.HurtPain7 => listOfMayaVoice.HurtPain.HurtPain7, 
			GameCore.MayaVoice.HurtPain8 => listOfMayaVoice.HurtPain.HurtPain8, 
			GameCore.MayaVoice.HurtPain9 => listOfMayaVoice.HurtPain.HurtPain9, 
			GameCore.MayaVoice.HurtPain10 => listOfMayaVoice.HurtPain.HurtPain10, 
			GameCore.MayaVoice.HurtPain11 => listOfMayaVoice.HurtPain.HurtPain11, 
			GameCore.MayaVoice.HurtPain12 => listOfMayaVoice.HurtPain.HurtPain12, 
			GameCore.MayaVoice.HurtPain13 => listOfMayaVoice.HurtPain.HurtPain13, 
			GameCore.MayaVoice.HurtPain14 => listOfMayaVoice.HurtPain.HurtPain14, 
			GameCore.MayaVoice.HurtPain15 => listOfMayaVoice.HurtPain.HurtPain15, 
			GameCore.MayaVoice.HurtPain16 => listOfMayaVoice.HurtPain.HurtPain16, 
			_ => listOfMayaVoice.HurtPain.HurtPain1, 
		};
	}

	private AudioClip RETURN_MV_GASP(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.Gasp2 => listOfMayaVoice.Gasp.Gasp2, 
			GameCore.MayaVoice.Gasp3 => listOfMayaVoice.Gasp.Gasp3, 
			GameCore.MayaVoice.Gasp4 => listOfMayaVoice.Gasp.Gasp4, 
			GameCore.MayaVoice.Gasp5 => listOfMayaVoice.Gasp.Gasp5, 
			GameCore.MayaVoice.Gasp6 => listOfMayaVoice.Gasp.Gasp6, 
			GameCore.MayaVoice.Gasp7 => listOfMayaVoice.Gasp.Gasp7, 
			GameCore.MayaVoice.Gasp8 => listOfMayaVoice.Gasp.Gasp8, 
			GameCore.MayaVoice.Gasp9 => listOfMayaVoice.Gasp.Gasp9, 
			GameCore.MayaVoice.Gasp10 => listOfMayaVoice.Gasp.Gasp10, 
			_ => listOfMayaVoice.Gasp.Gasp1, 
		};
	}

	private AudioClip RETURN_MV_SCREAM(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.Scream2 => listOfMayaVoice.Scream.Scream2, 
			GameCore.MayaVoice.Scream3 => listOfMayaVoice.Scream.Scream3, 
			GameCore.MayaVoice.Scream4 => listOfMayaVoice.Scream.Scream4, 
			GameCore.MayaVoice.Scream5 => listOfMayaVoice.Scream.Scream5, 
			GameCore.MayaVoice.Scream6 => listOfMayaVoice.Scream.Scream6, 
			GameCore.MayaVoice.Scream7 => listOfMayaVoice.Scream.Scream7, 
			GameCore.MayaVoice.Scream8 => listOfMayaVoice.Scream.Scream8, 
			GameCore.MayaVoice.Scream9 => listOfMayaVoice.Scream.Scream9, 
			GameCore.MayaVoice.Scream10 => listOfMayaVoice.Scream.Scream10, 
			GameCore.MayaVoice.Scream11 => listOfMayaVoice.Scream.Scream11, 
			GameCore.MayaVoice.Scream12 => listOfMayaVoice.Scream.Scream12, 
			GameCore.MayaVoice.Scream13 => listOfMayaVoice.Scream.Scream13, 
			GameCore.MayaVoice.Scream14 => listOfMayaVoice.Scream.Scream14, 
			GameCore.MayaVoice.Scream15 => listOfMayaVoice.Scream.Scream15, 
			_ => listOfMayaVoice.Scream.Scream1, 
		};
	}

	private AudioClip RETURN_MV_SWALLOW(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.Swallow2 => listOfMayaVoice.Swallow.Swallow2, 
			GameCore.MayaVoice.Swallow3 => listOfMayaVoice.Swallow.Swallow3, 
			GameCore.MayaVoice.Swallow4 => listOfMayaVoice.Swallow.Swallow4, 
			GameCore.MayaVoice.Swallow5 => listOfMayaVoice.Swallow.Swallow5, 
			GameCore.MayaVoice.Swallow6 => listOfMayaVoice.Swallow.Swallow6, 
			_ => listOfMayaVoice.Swallow.Swallow1, 
		};
	}

	private AudioClip RETURN_MV_MBBLOGO(GameCore.MayaVoice thisType)
	{
		return thisType switch
		{
			GameCore.MayaVoice.MBBLogo2 => listOfMayaVoice.MBBLogo.MBBLogo2, 
			GameCore.MayaVoice.MBBLogo3 => listOfMayaVoice.MBBLogo.MBBLogo3, 
			GameCore.MayaVoice.MBBLogo4 => listOfMayaVoice.MBBLogo.MBBLogo4, 
			GameCore.MayaVoice.MBBLogo5 => listOfMayaVoice.MBBLogo.MBBLogo5, 
			_ => listOfMayaVoice.MBBLogo.MBBLogo1, 
		};
	}

	public AudioClip ReturnForestBossSounds(int soundID)
	{
		return soundID switch
		{
			1 => listOfForestBossSound.ForestBoss1, 
			2 => listOfForestBossSound.ForestBoss2, 
			3 => listOfForestBossSound.ForestBoss3, 
			4 => listOfForestBossSound.ForestBoss4, 
			5 => listOfForestBossSound.ForestBoss5, 
			6 => listOfForestBossSound.ForestBoss6, 
			7 => listOfForestBossSound.ForestBoss7, 
			8 => listOfForestBossSound.ForestBoss8, 
			9 => listOfForestBossSound.ForestBoss9, 
			10 => listOfForestBossSound.ForestBoss10, 
			11 => listOfForestBossSound.ForestBoss11, 
			12 => listOfForestBossSound.ForestBoss12, 
			13 => listOfForestBossSound.ForestBoss13, 
			14 => listOfForestBossSound.ForestBoss14, 
			15 => listOfForestBossSound.ForestBoss15, 
			_ => null, 
		};
	}

	public AudioClip ReturnHillBossSounds(int soundID)
	{
		return soundID switch
		{
			1 => listOfHillBossSound.HillBoss1, 
			2 => listOfHillBossSound.HillBoss2, 
			3 => listOfHillBossSound.HillBoss3, 
			4 => listOfHillBossSound.HillBoss4, 
			5 => listOfHillBossSound.HillBoss5, 
			6 => listOfHillBossSound.HillBoss6, 
			_ => null, 
		};
	}

	public AudioClip ReturnWerewolfBossSounds(int soundID)
	{
		return soundID switch
		{
			1 => listOfSFXBasic2.Transformation1, 
			2 => listOfWerewolfBossSound.Howl, 
			3 => listOfWerewolfBossSound.Grrr, 
			4 => listOfWerewolfBossSound.SnarlBite, 
			5 => listOfWerewolfBossSound.LeapBack, 
			6 => listOfWerewolfBossSound.PounceForwards, 
			7 => listOfWerewolfBossSound.Swing1, 
			8 => listOfWerewolfBossSound.Swing2, 
			9 => listOfWerewolfBossSound.Grrr2, 
			10 => listOfWerewolfBossSound.RunAway, 
			11 => listOfWerewolfBossSound.Step1, 
			12 => listOfWerewolfBossSound.Step2, 
			13 => listOfWerewolfBossSound.MoonExplosion, 
			14 => listOfWerewolfBossSound.SlashBarrage, 
			_ => null, 
		};
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
			return Random.Range(0, 3) switch
			{
				1 => listOfMinotaurBossSound.Bull2, 
				2 => listOfMinotaurBossSound.Bull3, 
				_ => listOfMinotaurBossSound.Bull1, 
			};
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
		return soundID switch
		{
			1 => listOfPrincessBossSound.Attack1, 
			2 => listOfPrincessBossSound.Attack2, 
			3 => listOfPrincessBossSound.Attack3, 
			4 => listOfPrincessBossSound.Woah, 
			5 => Random.Range(0, 3) switch
			{
				1 => listOfPrincessBossSound.Attack2, 
				2 => listOfPrincessBossSound.Attack3, 
				_ => listOfPrincessBossSound.Attack1, 
			}, 
			10 => listOfPrincessBossSound.Boulder, 
			11 => listOfPrincessBossSound.Ice, 
			12 => listOfPrincessBossSound.Break, 
			13 => listOfPrincessBossSound.IceGen1, 
			14 => listOfPrincessBossSound.IceGen2, 
			15 => listOfPrincessBossSound.Laugh, 
			_ => null, 
		};
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
			return Random.Range(1, 6) switch
			{
				2 => listOfWitchBossSound.laugh_vol04_02, 
				3 => listOfWitchBossSound.laugh_vol04_03, 
				4 => listOfWitchBossSound.laugh_vol04_04, 
				5 => listOfWitchBossSound.laugh_vol04_05, 
				_ => listOfWitchBossSound.laugh_vol04_01, 
			};
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
			return Random.Range(1, 5) switch
			{
				2 => listOfWitchBossSound.moan02, 
				3 => listOfWitchBossSound.moan03, 
				4 => listOfWitchBossSound.moan04, 
				_ => listOfWitchBossSound.moan01, 
			};
		case 31:
			return Random.Range(1, 5) switch
			{
				2 => listOfWitchBossSound.moan06, 
				3 => listOfWitchBossSound.moan07, 
				4 => listOfWitchBossSound.moan08, 
				_ => listOfWitchBossSound.moan05, 
			};
		case 32:
			return Random.Range(1, 5) switch
			{
				2 => listOfWitchBossSound.moan10, 
				3 => listOfWitchBossSound.moan11, 
				4 => listOfWitchBossSound.moan12, 
				_ => listOfWitchBossSound.moan09, 
			};
		case 33:
			return Random.Range(1, 5) switch
			{
				2 => listOfWitchBossSound.moan04, 
				3 => listOfWitchBossSound.moan07, 
				4 => listOfWitchBossSound.moan09, 
				_ => listOfWitchBossSound.moan01, 
			};
		default:
			return null;
		}
	}

	public AudioClip ReturnBlueFoxBossSounds(int soundID)
	{
		return soundID switch
		{
			0 => listOfBlueFoxBossSound.ShootProjectiles, 
			1 => listOfBlueFoxBossSound.PreMelee, 
			2 => listOfBlueFoxBossSound.MeleeSpin, 
			3 => listOfBlueFoxBossSound.Jump, 
			4 => listOfBlueFoxBossSound.Dash, 
			5 => listOfBlueFoxBossSound.PreBeam, 
			6 => listOfBlueFoxBossSound.Beam, 
			7 => listOfBlueFoxBossSound.PounceExplode, 
			8 => listOfBlueFoxBossSound.DropMine, 
			9 => listOfBlueFoxBossSound.MineActivate1, 
			10 => listOfBlueFoxBossSound.MineActivate2, 
			11 => listOfBlueFoxBossSound.MineActivate3, 
			12 => listOfBlueFoxBossSound.MindBlast1, 
			13 => listOfBlueFoxBossSound.MindBlast2, 
			14 => listOfBlueFoxBossSound.Electric1, 
			15 => listOfBlueFoxBossSound.Electric2, 
			16 => listOfBlueFoxBossSound.Electric3, 
			17 => listOfBlueFoxBossSound.Electric4, 
			18 => listOfBlueFoxBossSound.Electric5, 
			_ => null, 
		};
	}

	public AudioClip ReturnHarunaVoiceMoans(int soundID)
	{
		return soundID switch
		{
			0 => listOfHarunaVoiceSound.Moan0, 
			1 => listOfHarunaVoiceSound.Moan1, 
			2 => listOfHarunaVoiceSound.Moan2, 
			3 => listOfHarunaVoiceSound.Moan3, 
			4 => listOfHarunaVoiceSound.Moan4, 
			5 => listOfHarunaVoiceSound.Moan5, 
			6 => listOfHarunaVoiceSound.Moan6, 
			7 => listOfHarunaVoiceSound.Moan7, 
			8 => listOfHarunaVoiceSound.Moan8, 
			9 => listOfHarunaVoiceSound.Moan9, 
			10 => listOfHarunaVoiceSound.Moan10, 
			11 => listOfHarunaVoiceSound.Moan11, 
			12 => listOfHarunaVoiceSound.Moan12, 
			13 => listOfHarunaVoiceSound.Moan13, 
			14 => listOfHarunaVoiceSound.Moan14, 
			_ => null, 
		};
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
