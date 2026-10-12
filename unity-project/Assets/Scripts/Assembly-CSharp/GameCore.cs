using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameCore : MonoBehaviour
{
	public enum GameExecutable
	{
		Uncensored = 0,
		Censored = 1,
		CensoredPlus = 2,
		CensoredNoB = 3
	}

	public enum GameBuildType
	{
		Win64 = 0,
		Mac = 1,
		Linux = 2,
		WebGL = 3
	}

	public enum GamepadDirection
	{
		Neutral = 0,
		Up = 1,
		Down = 2,
		Left = 3,
		Right = 4
	}

	public enum InputLib
	{
		Up = 0,
		Down = 1,
		Left = 2,
		Right = 3,
		BasicAttack = 4,
		Jump = 5,
		DefenseSkills = 6,
		Camera = 7,
		Pause = 8,
		Interact = 9,
		Null = 10,
		UseItem1 = 11,
		UseItem2 = 12,
		UseItem3 = 13,
		UseItem4 = 14,
		ToggleInternalView = 15,
		AttackSkills = 16,
		DebugF5 = 17,
		DebugF6 = 18,
		DebugF7 = 19,
		DebugF8 = 20,
		DebugF9 = 21,
		DebugF10 = 22,
		DebugF11 = 23,
		DebugF12 = 24,
		UseItemActive = 25,
		PreviousItem = 26,
		NextItem = 27,
		ConfirmUI = 28,
		CancelUI = 29
	}

	public enum InputPressType
	{
		Null = 0,
		KeyPressedOnce = 1,
		KeyHolding = 2,
		KeyReleased = 3
	}

	public enum EnemyType
	{
		Null = 0,
		Isobora = 1,
		TargetDummy = 2,
		BlazePlant = 3,
		NaughtyCoral = 4,
		LazyStarfish = 5,
		BossForest = 6,
		BeachKitsune = 7,
		FlowerBlossom = 8,
		BossBeach = 9,
		Dragonfly = 10,
		HillFairy = 11,
		BossHill = 12,
		SalamanderGirl = 13,
		LizardShield = 14,
		LizardAxe = 15,
		WolfGirlMimic = 16,
		Arachne = 17,
		WaterJelly = 18,
		Lamia = 19,
		Kobold = 20,
		MushroomGirl = 21,
		Batgirl = 22,
		BossWerewolf = 23,
		FutaGhost = 24,
		SwampFairy = 25,
		TentacleTree = 26,
		MothGirl = 27,
		DwarfGirl = 28,
		SwampGolem = 29,
		BossMinotaur = 30,
		CentaurGirl = 31,
		GhostButler = 32,
		CrystalChunk = 33,
		Manequin = 34,
		Manequin_EvilMaya = 35,
		Manequin_MayaStuck = 36,
		BossPrincess = 37,
		PrisonBrute = 38,
		PurpleSlime = 39,
		PSMMaya = 40,
		PaintingEvil = 41,
		LatexMaiden = 42,
		Dominatrix = 43,
		Dogboy = 44,
		FutaBunny = 45,
		SailorBurly = 46,
		WormDig = 47,
		WormOverGround = 48,
		BossWitch = 49,
		Bot1 = 50,
		Bot2 = 51,
		Bot3 = 52,
		WyvernGirl = 53,
		BeeGirl = 54,
		FutaDruidCentauress = 55,
		BigInsect = 56,
		Failed = 57,
		BossBlueFox = 58,
		FleshBlossom = 59,
		CorruptBeast = 60,
		Sinccubus = 61,
		DarkDamsel = 62,
		Medusa = 63,
		FairySorrow = 64,
		BossSwampSlime = 65,
		SlimeGirl = 66,
		SlimeMayaFat = 67,
		Goblin = 68,
		Catgirl = 69,
		Goblina = 70,
		GeckNewt = 71,
		SewerBug = 72,
		HillTroll = 73,
		Tentapod = 74,
		VagrantMonk = 75
	}

	public enum ProjectileType
	{
		StraightLine = 0,
		TargetPosition = 1,
		Ballistic = 2,
		SpecialForestBossSpore = 3,
		SpecialForestBossSeed = 4,
		VerticalDown = 5,
		ArchRandom = 6,
		ThrowDagger = 7,
		SmallRuneBomb = 8,
		SpecialBeachBossWaves = 9,
		ChasingSlow = 10,
		ChasingSlowHorizontal = 11,
		Static = 12,
		RollingBall = 13,
		BoomerangLine = 14,
		SpecialPrincessBoss = 15,
		ChasingMedium = 16,
		WitchCloud = 17,
		BoomerangAnyDir = 18,
		OrbitAround = 19,
		BallisticBossSlime = 20,
		PlayerTP = 21,
		WeaponWhip = 22,
		SmallSwordSlash = 23,
		PlayerLance = 24,
		PlayerBoomerang = 25,
		PlayerHomingSphere = 26
	}

	public enum InventoryTabType
	{
		Consumables = 0,
		KeyItems = 1,
		Skills = 2,
		Gear = 3,
		Misc = 4,
		Equip = 5,
		Accessories = 6
	}

	public enum InventoryLogicType
	{
		PlayerInventory = 0,
		NeneShop = 1
	}

	public enum CollectibleType
	{
		Quartz = 0,
		HealthPotion = 1,
		ManaPotion = 2,
		KeyItem = 3,
		AttackUpgrade = 4,
		DefenseUpgrade = 5,
		HealthUpgrade = 6,
		SpiritUpgrade = 7,
		SkillPickup = 8,
		ThrowDagger = 9,
		SmallRuneBomb = 10,
		EnemyEssence = 11,
		CoreEquip = 12,
		GeneralUpgrades = 13,
		MetalDust = 14,
		FreshClothing = 15,
		EmptySlot = 16,
		Ring = 17,
		Bracelet = 18,
		Necklace = 19,
		Earring = 20,
		Hairpin = 21,
		WeaponWhip = 22,
		SmallSwordSlash = 23,
		GalleryGlobe = 24,
		Throwable = 25,
		SubPotions = 26,
		SubConsumables = 27
	}

	public enum AccessoryBehavior
	{
		DashPhaseAttack = 0,
		WeaponWhip = 1,
		SwordSlashProjectile = 2,
		SPLow25Stun = 3,
		CrouchShield = 4,
		SKILLEnhancePowerSlash = 5,
		glassEarrings = 6,
		DoubleJump = 7,
		EroticDancer = 8,
		HeadSuccubus = 9,
		HairFertility = 10,
		HairFertilityStatsHack = 11,
		SKILLEnhanceDefenseShield = 12,
		masterDash = 13,
		SKILLEnhancedSlidingTackle = 14,
		cheapDiscount = 15,
		cursedHorned = 16,
		Chastity = 17,
		JoyDevice = 18,
		DaggerSpecialist = 19,
		BladeStun = 20,
		SKILLEnhancedFlameSword = 21
	}

	public enum ContainerType
	{
		Crate = 0,
		ChestSmall = 1,
		ChestBigRed = 2,
		ChestBigGold = 3
	}

	public enum PlayerDressID
	{
		Default = 0,
		BunnyGirl = 1,
		Swimsuit = 2,
		MagicGirl = 3
	}

	public enum PlayerDressState
	{
		Full = 0,
		Half = 1,
		None = 2
	}

	public enum NPCType
	{
		BoardText = 0,
		Various = 1,
		NeneShop = 2,
		OBJ_Portal = 3,
		OBJ_PortalSameLevel = 4,
		OBJ_Tutorial = 5,
		FarinaSupport = 6,
		LivithNPC = 7,
		GalleryBook = 8
	}

	public enum LogicCheckType
	{
		AnyBossDefeated = 0,
		AnyGalleryItemAcquired = 1
	}

	public enum DifficultyType
	{
		Easy = 0,
		Normal = 1
	}

	public enum GalleryThemeType
	{
		Beach = 0,
		Forest = 1,
		Hill = 2,
		City = 3,
		Cave = 4,
		Swamp = 5,
		Castle = 6,
		HighTech = 7
	}

	public enum CameraType
	{
		Smooth = 0,
		Static = 1
	}

	public enum InputType
	{
		Keyboard = 0,
		GPTypePSX = 1,
		GPTypeXBOX = 2
	}

	public enum MenuScene
	{
		TitleScreen = 0,
		InGame = 1,
		IntroStory = 2
	}

	public enum SpeakerID
	{
		AnyObject = 0,
		Maya = 1,
		Sophie = 2,
		Nene = 3,
		ForestBoss = 4,
		HillBoss = 5,
		WerewolfBoss = 6,
		MusicPlayer = 7,
		MinotaurBoss = 8,
		PrincessBoss = 9,
		WitchBoss = 10,
		BlueFoxBoss = 11,
		SwampBoss = 12,
		Farina = 13,
		BEDoggoBoy1 = 14,
		BEDoggoBoy2 = 15,
		Chronicles = 16,
		LivithFinal = 17
	}

	public enum SpeakerACT
	{
		Null = 0,
		Stand1 = 1,
		Talk1 = 2,
		Talk2 = 3,
		Read1 = 4,
		Laugh1 = 5,
		Angry1 = 6,
		Angry2 = 7,
		InvertFacing = 8,
		Stand2 = 9,
		Misc1 = 10,
		Hurt1 = 11,
		Enable = 12,
		Disable = 13,
		Visible = 14,
		Invisible = 15,
		Special1 = 16,
		Special2 = 17,
		Special3 = 18,
		Talk3 = 19,
		Talk4 = 20
	}

	public enum MusicTheme
	{
		TitleScreen = 0,
		IntroStory = 1,
		Beach1 = 2,
		HubCastle1 = 3,
		Forest1 = 4,
		Fainted = 5,
		BossFight1 = 6,
		VictoryFanfare1 = 7,
		Beach2 = 8,
		Null = 9,
		Valley1 = 10,
		City1 = 11,
		EroGrabbed = 12,
		UniqueItemFanfare1 = 13,
		Cave1 = 14,
		Cave2 = 15,
		Cave3 = 16,
		DarkAmbience1 = 17,
		Swamp1 = 18,
		CastleUpperFLoor = 19,
		Dungeon = 20,
		HighTech1 = 21,
		BossFight2 = 22,
		HighTech2 = 23,
		BadEndTheme = 24,
		BossFight3 = 25
	}

	public enum VisualEffect
	{
		Null = 0,
		GenericHit = 1,
		BasicSwordHit = 2,
		SplashWaterEnter = 3,
		SplashWaterLeave = 4,
		SkillDashCast = 5,
		PunchGrabRelease = 6,
		ClawRed = 7,
		GrabSmokes = 8,
		SporeTransform = 9,
		SmallRuneExplosion = 10,
		HealLife = 11,
		HealSpirit = 12,
		Spit = 13,
		HatchLeft = 14,
		SplashSewerEnter = 15,
		MimicChestTrail = 16,
		SlidingTackleTrail = 17,
		GenericPortal = 18,
		ChargePower_White = 19,
		ChargePower_Yellow = 20,
		ChargePower_Purple = 21,
		ChargePower_Red = 22,
		WerewolfCurse = 23,
		GhostCast = 24,
		SplashSwampEnter = 25,
		TentacleTreeProjDead = 26,
		MayaSkillShield = 27,
		NotEnoughSPToUseSkill = 28,
		ShieldBreak = 29,
		FlameSwordTrail = 30,
		SlashSkillHit = 31,
		PickaxEnd = 32,
		RedExplode = 33,
		UnlockGate = 34,
		Hourglass = 35,
		SplashWaterEnterDungeon = 36,
		IceBreak = 37,
		IceTrail = 38,
		IceSmoke1 = 39,
		PrincessDress = 40,
		PurpleSlimeBurst = 41,
		PortraitEvil = 42,
		SmokeDigWorm = 43,
		RocksDigging = 44,
		StarsINWARD = 45,
		StarsOUTWARD = 46,
		CastingWitch1 = 47,
		CastingWitch2 = 48,
		CastingWitch3 = 49,
		CastingWitch4 = 50,
		CastingWitch5 = 51,
		CastingWitch6 = 52,
		CastingWitch0 = 53,
		SmokesPink = 54,
		BlueFoxCastShock = 55,
		ChargePower_Blue = 56,
		BlueFoxTPAway = 57,
		ArmorBreakB = 58,
		ArmorBreakC = 59,
		Petrified2 = 60,
		JumpSmoke = 61,
		playerGroundPound = 62,
		GlassShatter = 63,
		ThrowItem = 64,
		playerTeleportStars = 65,
		CreamExplosion = 66,
		CreamExplosionPurple = 67,
		DressStateChange = 68,
		PVFXRegenDefenseDurability = 69,
		ExplosionType2 = 70,
		EssenceAbsorb = 71,
		DashATKSlash = 72,
		SlashSkillHitPurple = 73,
		EssenceAbsorbWhite = 74,
		particleDrinkATK = 75,
		particleDrinkDEF = 76,
		particleDrinkSPEED = 77,
		particleDrinkREGENSP = 78,
		particleDrinkREGENLIFE = 79,
		particleFruitSlice = 80,
		particleFruitForbidden = 81,
		particleFruitDistract = 82,
		particleFruitWarrior = 83,
		particleHeroMeal = 84,
		ShieldMealInvincible = 85,
		SmokesGreen = 86,
		PoisonConstant = 87,
		PoisonTick = 88,
		HackProjectileFlame = 89,
		GainLust = 90
	}

	public enum SFXBasic1
	{
		NormalSwordHit1 = 0,
		NormalSwordHit2 = 1,
		NormalSwordHit3 = 2,
		SpawnerNoise1 = 3,
		Swing1 = 4,
		Swing2 = 5,
		DialogueNext = 6,
		DialogueEnd = 7,
		BarrelDestroyed1 = 8,
		BarrelHit1 = 9,
		SkillDash = 10,
		QuartzGet = 11,
		PotionGet = 12,
		WaterSplashEnter = 13,
		WaterSplashExit = 14,
		PunchGrabRelease = 15,
		JumpStart = 16,
		JumpStartRNG = 17,
		JumpEnd = 18,
		JumpEndRNG = 19,
		HitHurtSpike1 = 20,
		HitHurtSpike2 = 21,
		HitHurtCrush1 = 22,
		HitHurtTouch1 = 23,
		HitHurtTouch2 = 24,
		HitHurtTouch3 = 25,
		HitHurtTouch4 = 26,
		ClothRip1 = 27,
		Armorbreak1 = 28,
		LaserShoot1 = 29,
		LeverStart = 30,
		LeverEnd = 31,
		GateOpening = 32,
		GateClosed = 33,
		uiEquipItem = 34,
		uiUnequipItem = 35,
		uiWrongError = 36,
		Falling1 = 37,
		Coconut1 = 38,
		KitsuneCast1 = 39,
		Fast1 = 40,
		Pickup1 = 41,
		CrystalOn = 42,
		CrystalOff = 43,
		CrystalEngaged = 44,
		Bounce = 45,
		CrystalDisengaged = 46,
		PowerupCollected = 47,
		SwingTK1 = 48,
		SwingTK2 = 49,
		SwingTK3 = 50,
		Defend1 = 51,
		SwingTKRandom = 52,
		Bite1 = 53,
		JumpEnd2 = 54,
		UnlockGate = 55,
		EnterMirror = 56,
		Shoot1 = 57,
		Tornado = 58,
		Cloud = 59,
		Thunder = 60,
		DialogueNext1 = 61,
		DialogueNext2 = 62,
		DrillDash = 63,
		FallSword = 64,
		playerTP_UP = 65,
		playerTP_DOWN = 66,
		CycleItemNext = 67,
		CycleItemPrevious = 68,
		DiagToneGroup1 = 69,
		DiagToneGroup2 = 70,
		DiagToneGroup3 = 71,
		DiagToneGroup4 = 72,
		DiagToneGroup5 = 73,
		DiagToneGroup6 = 74,
		DiagToneGroup7 = 75,
		DiagToneGroup8 = 76,
		DiagToneGroup9 = 77,
		DiagToneGroup10 = 78,
		DiagToneGroup11 = 79,
		HeartIconSpawn = 80
	}

	public enum SFXBasic2
	{
		Null = 0,
		BossFightStart = 1,
		BossDeathExplode = 2,
		Boing1 = 3,
		Boing2 = 4,
		Boing3 = 5,
		Boing4 = 6,
		BoingRandom = 7,
		Weird1 = 8,
		itemDrinkPotion1 = 9,
		Odd1 = 10,
		Odd2 = 11,
		Odd3 = 12,
		Odd4 = 13,
		Odd5 = 14,
		Throw1 = 15,
		Throw2 = 16,
		Throw3 = 17,
		Hit1 = 18,
		Explosion1 = 19,
		Swing1 = 20,
		Splash1 = 21,
		RockSmash = 22,
		GroundPoundLonger = 23,
		SwitchStart = 24,
		SwitchEnd = 25,
		DoorOpen = 26,
		DoorClose = 27,
		HatchOpen = 28,
		ShieldHit1 = 29,
		Broken1 = 30,
		Dizzy1 = 31,
		Steps1 = 32,
		UniqueItemAppear1 = 33,
		SkillAppear1 = 34,
		ChestOpen1 = 35,
		PlatPowerOn = 36,
		PlatPowerOff = 37,
		SkillSlidingTackle = 38,
		Bump1 = 39,
		LamiaSkillCast = 40,
		LamiaSkillAppear = 41,
		Swing2 = 42,
		ElectricSurge = 43,
		Transformation1 = 44,
		LowExplosion1 = 45,
		Wooden1 = 46,
		Wooden2 = 47,
		Swing3 = 48,
		SkillNotEnoughSP = 49,
		SkillSlashSpawn = 50,
		SkillSlashHit = 51,
		SkillFlameCast = 52,
		SkillShieldCast = 53,
		SkillShieldDissipates = 54,
		Swing4 = 55,
		Grow1 = 56,
		Ripping1 = 57,
		Tearing1 = 58,
		Swing5 = 59,
		GroundPoundLight = 60,
		MinotaurShield = 61,
		Crystalize = 62,
		Odd6 = 63,
		Heal1 = 64,
		Odd7 = 65,
		Odd8 = 66,
		BoingRandom2 = 67,
		GateClose = 68,
		Cursor1 = 69,
		MayaClothTransform = 70,
		EquipCloth = 71,
		EssenceSmall = 72,
		EssenceMedium = 73,
		EssenceBig = 74,
		GauntletFanfare = 75
	}

	public enum MonsterSounds
	{
		IsoboraSkill = 0,
		IsoboraGrab = 1,
		IsoboraDead = 2,
		SpitBudDead = 3,
		StarfishGrab = 4,
		StarfishDead = 5,
		BarnacleDead = 6,
		KitsuneGiggle = 7,
		KitsuneDead = 8,
		Null = 9,
		DFLYDead = 10,
		DFLYSkill = 11,
		FairyDead = 12,
		FairySkill = 13,
		FairyLaugh = 14,
		SalaTongue = 15,
		LizardDead = 16,
		MimicHowl = 17,
		MimicJump = 18,
		MimicLand = 19,
		ArachneLaugh = 20,
		ArachneClimax = 21,
		LamiaCast1 = 22,
		LamiaCast2 = 23,
		LamiaAttack1 = 24,
		LamiaAttack2 = 25,
		LamiaDead = 26,
		LamiaOpenMouth = 27,
		MushroomGirlLaugh = 28,
		MushroomGirlDeath = 29,
		MushroomGirlSkillA = 30,
		MushroomGirlSkillB = 31,
		BatCall = 32,
		BatSwoop = 33,
		BatWings = 34,
		BatDead = 35,
		BatAhh = 36,
		FutaGhostRing = 37,
		FutaGhostRing2 = 38,
		FTG_Cast1 = 39,
		FTG_Cast2 = 40,
		FTG_Cast3 = 41,
		FTG_Dead = 42,
		FTG_Hurt = 43,
		FTG_Cast_Random = 44,
		SwampFairy_Cast = 45,
		SwampFairy_Caw = 46,
		SwampFairy_Laugh = 47,
		SwampFairy_Dead = 48,
		SwampFairyThunder = 49,
		MothSkillCast = 50,
		MothSkillExplode = 51,
		MothDead = 52,
		MothCasting = 53,
		DwarfGrunt = 54,
		DwarfDeath = 55,
		CTG_Disdain = 56,
		CTG_Snort = 57,
		CTG_PreCHarge = 58,
		CTG_Gallop = 59,
		CTG_Dead = 60,
		GSB_CastRandom = 61,
		GSB_Grunt1 = 62,
		GSB_Grunt2 = 63,
		Crystal_Cast = 64,
		Crystal_Shoot = 65,
		Crystal_Chase = 66,
		Crystal_Dead = 67,
		Brute1 = 68,
		Brute2 = 69,
		PaintingEvil_Appear = 70,
		PaintingEvil_Splat = 71,
		LatexMaiden_Open = 72,
		LatexMaiden_Close = 73,
		LatexMaiden_Dead = 74,
		Domme_Skill = 75,
		Domme_Crack = 76,
		Domme_Dead = 77,
		Domme_Dodge = 78,
		Domme_Swing = 79,
		CityDogSkill = 80,
		CityDogBark = 81,
		CityDogBabyBark = 82,
		CityDogBabyWhine = 83,
		CityDogPanting = 84,
		CityDogGroaning = 85,
		BunnyCast = 86,
		BunnyDead = 87,
		PirateCast = 88,
		PirateSpin = 89,
		PirateDead = 90,
		WormCast = 91,
		WormDead = 92,
		WormDigDown = 93,
		WormDigUp = 94,
		WormMelee = 95,
		RobotAlarm1 = 96,
		RobotAlarm2 = 97,
		RobotAlarm3 = 98,
		RobotSkill1 = 99,
		RobotSkill2 = 100,
		RobotSkill3 = 101,
		RobotExplode = 102,
		WyvernCast = 103,
		WyvernSkill = 104,
		WyvernDead = 105,
		BeeGirlCast = 106,
		BeeGirlChase = 107,
		BeeGirlDead = 108,
		FutaCentaurHeal = 109,
		FutaCentaurAttack = 110,
		FutaCentaurDead = 111,
		WyvernBirth = 112,
		BIS_Cast = 113,
		BIS_Skill = 114,
		BIS_Dead = 115,
		FLR_Cast = 116,
		FLR_Dead = 117,
		FLR_Pound = 118,
		FHB_Tentacles = 119,
		FHB_Dead = 120,
		CRB_Cast = 121,
		CRB_Skill = 122,
		CRB_Dead = 123,
		SCB_Cast = 124,
		SCB_Skill = 125,
		SCB_Chase = 126,
		SCB_Dead = 127,
		DKD_Cast = 128,
		DKD_Skill = 129,
		DKD_Dead = 130,
		MDS_Cast = 131,
		MDS_Petr = 132,
		MDS_Stiff = 133,
		MDS_Dead = 134,
		FairySorrowDead = 135,
		GoblinLaugh = 136,
		CatHiss = 137,
		CatNyaOther = 138,
		CatNya1 = 139,
		CatNya2 = 140,
		CatNya3 = 141,
		CatNya4 = 142,
		CatNya5 = 143
	}

	public enum EroSound
	{
		Piston1 = 0,
		Piston2 = 1,
		Suck1 = 2,
		Suck2 = 3,
		Suck3 = 4,
		SuckRandom = 5,
		Lick1 = 6,
		Swallow1 = 7,
		Swallow2 = 8,
		Swallow3 = 9,
		Swallow4 = 10,
		Swallow5 = 11,
		SwallowRandom = 12,
		Cum1 = 13,
		Cum2 = 14,
		Cum3 = 15,
		Cum4 = 16,
		Pull1 = 17,
		Vore1 = 18,
		Vore2 = 19,
		Lick2 = 20,
		Piston3 = 21,
		Lick3 = 22,
		Piston4 = 23,
		Cum5 = 24,
		VoreRandomA = 25,
		Vore3 = 26,
		Touch1 = 27,
		Slap1 = 28,
		Contact1 = 29,
		Kiss1 = 30,
		Kiss2 = 31,
		Kiss3 = 32,
		Kiss4 = 33,
		KissRandom = 34,
		Wet1 = 35,
		Wet2 = 36,
		Wet3 = 37,
		Wet4 = 38,
		Wet5 = 39,
		Wet6 = 40,
		WetRandom = 41,
		LickRandom = 42,
		Cum6 = 43,
		Bubble1 = 44,
		Bubble2 = 45,
		Bubble3 = 46,
		Bubble4 = 47,
		Bubble5 = 48,
		Slap2 = 49,
		SwallowRandom2 = 50,
		BubbleRandom = 51,
		Vore4 = 52,
		VoreRandomB = 53,
		Touch2 = 54,
		Suck4 = 55,
		Suck5 = 56,
		Rubber1 = 57,
		Rubber2 = 58,
		Rubber3 = 59,
		Rubber4 = 60,
		Rubber5 = 61,
		Rubber6 = 62,
		Rubber7 = 63,
		RubberRandom = 64,
		Touch3 = 65,
		HeartBeat1 = 66
	}

	public enum MayaVoice
	{
		Null = 0,
		MoanLight1 = 1,
		MoanLight2 = 2,
		MoanLight3 = 3,
		MoanLight4 = 4,
		MoanLight5 = 5,
		MoanLight6 = 6,
		MoanLight7 = 7,
		MoanLight8 = 8,
		MoanLight9 = 9,
		MoanLight10 = 10,
		MoanLight11 = 11,
		MoanLight12 = 12,
		MoanLight13 = 13,
		MoanLight14 = 14,
		MoanLight15 = 15,
		MoanLight16 = 16,
		MoanLight17 = 17,
		MoanLight18 = 18,
		MoanLight19 = 19,
		MoanMedium1 = 20,
		MoanMedium2 = 21,
		MoanMedium3 = 22,
		MoanMedium4 = 23,
		MoanMedium5 = 24,
		MoanMedium6 = 25,
		MoanMedium7 = 26,
		MoanMedium8 = 27,
		MoanMedium9 = 28,
		MoanMedium10 = 29,
		MoanMedium11 = 30,
		MoanHeavy1 = 31,
		MoanHeavy2 = 32,
		MoanHeavy3 = 33,
		MoanHeavy4 = 34,
		MoanHeavy5 = 35,
		MoanHeavy6 = 36,
		MoanHeavy7 = 37,
		MoanHeavy8 = 38,
		MoanHeavy9 = 39,
		MoanHeavy10 = 40,
		MoanHeavy11 = 41,
		MoanRough1 = 42,
		MoanRough2 = 43,
		MoanRough3 = 44,
		MoanRough4 = 45,
		MoanRough5 = 46,
		MoanRough6 = 47,
		MoanRough7 = 48,
		MoanRough8 = 49,
		MoanRough9 = 50,
		MoanRough10 = 51,
		MoanRough11 = 52,
		MoanRough12 = 53,
		Climax1 = 54,
		Climax2 = 55,
		Climax3 = 56,
		Climax4 = 57,
		Climax5 = 58,
		Climax6 = 59,
		Climax7 = 60,
		Climax8 = 61,
		Climax9 = 62,
		Climax10 = 63,
		Climax11 = 64,
		Attack1 = 65,
		Attack2 = 66,
		Attack3 = 67,
		Attack4 = 68,
		Attack5 = 69,
		Attack6 = 70,
		Attack7 = 71,
		Attack8 = 72,
		Attack9 = 73,
		Attack10 = 74,
		Attack11 = 75,
		Attack12 = 76,
		Attack13 = 77,
		Attack14 = 78,
		Attack15 = 79,
		Attack16 = 80,
		Attack17 = 81,
		HurtPain1 = 82,
		HurtPain2 = 83,
		HurtPain3 = 84,
		HurtPain4 = 85,
		HurtPain5 = 86,
		HurtPain6 = 87,
		HurtPain7 = 88,
		HurtPain8 = 89,
		HurtPain9 = 90,
		HurtPain10 = 91,
		HurtPain11 = 92,
		HurtPain12 = 93,
		HurtPain13 = 94,
		HurtPain14 = 95,
		HurtPain15 = 96,
		HurtPain16 = 97,
		Scream1 = 98,
		Scream2 = 99,
		Scream3 = 100,
		Scream4 = 101,
		Scream5 = 102,
		Scream6 = 103,
		Scream7 = 104,
		Scream8 = 105,
		Scream9 = 106,
		Scream10 = 107,
		Scream11 = 108,
		Scream12 = 109,
		Scream13 = 110,
		Scream14 = 111,
		Scream15 = 112,
		Gasp1 = 113,
		Gasp2 = 114,
		Gasp3 = 115,
		Gasp4 = 116,
		Gasp5 = 117,
		Gasp6 = 118,
		Gasp7 = 119,
		Gasp8 = 120,
		Gasp9 = 121,
		Gasp10 = 122,
		BJLight1 = 123,
		BJLight2 = 124,
		BJLight3 = 125,
		BJLight4 = 126,
		BJLight5 = 127,
		BJLight6 = 128,
		BJLight7 = 129,
		BJLight8 = 130,
		BJLight9 = 131,
		BJLight10 = 132,
		BJLight11 = 133,
		BJLight12 = 134,
		BJLight13 = 135,
		BJLight14 = 136,
		BJLight15 = 137,
		BJLight16 = 138,
		BJLight17 = 139,
		BJLight18 = 140,
		BJLight19 = 141,
		BJLight20 = 142,
		BJLight21 = 143,
		BJLight22 = 144,
		BJLight23 = 145,
		BJLight24 = 146,
		BJLight25 = 147,
		BJLight26 = 148,
		BJLight27 = 149,
		BJLight28 = 150,
		BJMedium1 = 151,
		BJMedium2 = 152,
		BJMedium3 = 153,
		BJMedium4 = 154,
		BJMedium5 = 155,
		BJMedium6 = 156,
		BJMedium7 = 157,
		BJMedium8 = 158,
		BJMedium9 = 159,
		BJMedium10 = 160,
		BJMedium11 = 161,
		BJMedium12 = 162,
		BJMedium13 = 163,
		BJMedium14 = 164,
		BJMedium15 = 165,
		BJMedium16 = 166,
		BJMedium17 = 167,
		BJMedium18 = 168,
		BJMedium19 = 169,
		BJMedium20 = 170,
		BJMedium21 = 171,
		BJMedium22 = 172,
		BJMedium23 = 173,
		BJMedium24 = 174,
		BJMedium25 = 175,
		BJMedium26 = 176,
		BJMedium27 = 177,
		BJMedium28 = 178,
		BJMedium29 = 179,
		BJMedium30 = 180,
		BJMedium31 = 181,
		BJMedium32 = 182,
		BJMedium33 = 183,
		BJMedium34 = 184,
		BJMedium35 = 185,
		BJHeavy1 = 186,
		BJHeavy2 = 187,
		BJHeavy3 = 188,
		BJHeavy4 = 189,
		BJHeavy5 = 190,
		BJHeavy6 = 191,
		BJHeavy7 = 192,
		BJHeavy8 = 193,
		BJHeavy9 = 194,
		BJHeavy10 = 195,
		BJHeavy11 = 196,
		BJHeavy12 = 197,
		BJHeavy13 = 198,
		BJHeavy14 = 199,
		BJHeavy15 = 200,
		BJHeavy16 = 201,
		BJHeavy17 = 202,
		BJHeavy18 = 203,
		BJHeavy19 = 204,
		BJHeavy20 = 205,
		BJHeavy21 = 206,
		BJHeavy22 = 207,
		BJHeavy23 = 208,
		BJHeavy24 = 209,
		BJHeavy25 = 210,
		BJHeavy26 = 211,
		BJHeavy27 = 212,
		BJHeavy28 = 213,
		BJHeavy29 = 214,
		BJHeavy30 = 215,
		BJHeavy31 = 216,
		BJHeavy32 = 217,
		BJHeavy33 = 218,
		BJHeavy34 = 219,
		BJClimax1 = 220,
		BJClimax2 = 221,
		BJClimax3 = 222,
		BJClimax4 = 223,
		BJClimax5 = 224,
		BJClimax6 = 225,
		BJClimax7 = 226,
		BJClimax8 = 227,
		BJClimax9 = 228,
		BJClimax10 = 229,
		BJClimax11 = 230,
		BJClimax12 = 231,
		BJClimax13 = 232,
		BJClimax14 = 233,
		BJClimax15 = 234,
		BJClimax16 = 235,
		BJClimax17 = 236,
		BJClimax18 = 237,
		BJClimax19 = 238,
		BJClimax20 = 239,
		BJClimax21 = 240,
		BJClimax22 = 241,
		BJClimax23 = 242,
		BJClimax24 = 243,
		BJClimax25 = 244,
		BJClimax26 = 245,
		BJClimax27 = 246,
		BJClimax28 = 247,
		BJClimax29 = 248,
		BJClimax30 = 249,
		BJClimax31 = 250,
		BJClimax32 = 251,
		BJClimax33 = 252,
		BJClimax34 = 253,
		BJClimax35 = 254,
		BJLick1 = 255,
		BJLick2 = 256,
		BJLick3 = 257,
		BJLick4 = 258,
		BJLick5 = 259,
		BJLick6 = 260,
		BJLick7 = 261,
		BJLick8 = 262,
		BJLick9 = 263,
		BJLick10 = 264,
		BJLick11 = 265,
		BJLick12 = 266,
		BJLick13 = 267,
		BJLick14 = 268,
		BJLick15 = 269,
		BJLick16 = 270,
		BJLick17 = 271,
		BJLick18 = 272,
		BJLick19 = 273,
		BJLick20 = 274,
		BJLick21 = 275,
		BJLick22 = 276,
		BJLick23 = 277,
		BJLick24 = 278,
		BJMisc1 = 279,
		BJMisc2 = 280,
		BJMisc3 = 281,
		BJMisc4 = 282,
		BJMisc5 = 283,
		BJMisc6 = 284,
		BJMisc7 = 285,
		BJMisc8 = 286,
		BJMisc9 = 287,
		BJMisc10 = 288,
		BJMisc11 = 289,
		BJMisc12 = 290,
		BJMisc13 = 291,
		BJMisc14 = 292,
		BJMisc15 = 293,
		BJMisc16 = 294,
		BJMisc17 = 295,
		BJMisc18 = 296,
		BJMisc19 = 297,
		BJMisc20 = 298,
		BJMisc21 = 299,
		BJMisc22 = 300,
		BJMisc23 = 301,
		BJMisc24 = 302,
		BJMisc25 = 303,
		BJMisc26 = 304,
		ClosedMouth1 = 305,
		ClosedMouth2 = 306,
		ClosedMouth3 = 307,
		ClosedMouth4 = 308,
		ClosedMouth5 = 309,
		ClosedMouth6 = 310,
		ClosedMouth7 = 311,
		ClosedMouth8 = 312,
		ClosedMouth9 = 313,
		ClosedMouth10 = 314,
		ClosedMouth11 = 315,
		ClosedMouth12 = 316,
		ClosedMouth13 = 317,
		ClosedMouth14 = 318,
		ClosedMouth15 = 319,
		ClosedMouth16 = 320,
		ClosedMouth17 = 321,
		Swallow1 = 322,
		Swallow2 = 323,
		Swallow3 = 324,
		Swallow4 = 325,
		Swallow5 = 326,
		MBBLogo1 = 327,
		MBBLogo2 = 328,
		MBBLogo3 = 329,
		MBBLogo4 = 330,
		MBBLogo5 = 331,
		Swallow6 = 332
	}

	public enum MARange
	{
		MoanLight = 0,
		MoanMedium1 = 1,
		MoanMedium2 = 2,
		MoanHeavy = 3,
		Climax = 4,
		HurtPain1 = 5,
		HurtPain2 = 6,
		HurtPain3 = 7,
		AttackSimple = 8,
		AttackSimpleNaked = 9,
		AttackHeavy = 10,
		AttackHeavyNaked = 11,
		Scream = 12,
		EroReleased = 13,
		ClimaxAfter1 = 14,
		GaspGentle = 15,
		BJLick = 16,
		BJMLight1 = 17,
		BJMLight2 = 18,
		BJMMedium1 = 19,
		BJMMedium2 = 20,
		BJMMedium3 = 21,
		BJMHeavy1 = 22,
		BJMHeavy2 = 23,
		BJMRough1 = 24,
		MayaSlurp1 = 25,
		BJClimax1 = 26,
		ClosedMouth1 = 27,
		ClosedMouth2 = 28,
		ClosedMouthMoan1 = 29,
		ClosedMouthMoan2 = 30,
		ClosedMouthMoan3 = 31,
		SwallowRange1 = 32,
		MoanRough1 = 33,
		GaspGentle2 = 34,
		BJMiscRNG1 = 35,
		ClimaxAfter2 = 36,
		CLMoanAlt1 = 37,
		BJMisc2 = 38,
		HurtPain4 = 39,
		MoanRough2 = 40,
		MiscOpen2 = 41,
		Scream2 = 42,
		MoanLight2 = 43,
		MoanLight3 = 44,
		MoanHeavy2 = 45,
		MayaSlurp2 = 46,
		BJSuck1 = 47,
		Scream3 = 48,
		BJMRough2 = 49,
		GaspGentle3 = 50,
		BJClimax2 = 51,
		BJLovely1 = 52,
		BJLovely2 = 53,
		BJLovely3 = 54,
		MiscOpen = 55,
		Climax2 = 56,
		GaspGentle4 = 57
	}

	public enum WaitGenerics
	{
		VictoryFanfare = 0,
		UniqueItemFanfare = 1
	}

	public enum EroAnimType
	{
		Basic = 0,
		FlowerBlossom = 1,
		WerewolfBoss = 2,
		MinotaurBoss = 3,
		CentaurGirl = 4,
		GhostButler = 5,
		PrincessBoss = 6,
		PaintingEvil = 7,
		DungeonLatex = 8,
		DogBoy = 9,
		WormVore = 10,
		WitchBoss = 11,
		BlueFoxBoss = 12,
		Medusa = 13,
		SlimeGirl = 14,
		SwampBoss = 15,
		CatGirl = 16,
		BatGirl = 17,
		DwarfGirl = 18,
		MushroomFuta = 19,
		DarkDamsel = 20,
		SwampFairy = 21,
		CentaurFutaDruid = 22,
		SwampGolem = 23
	}

	public enum FetishType
	{
		Vanilla = 0,
		Blowjob = 1,
		Feet = 2,
		Lesbian = 3,
		Futanari = 4,
		Tentacle = 5,
		Beast = 6,
		Insect = 7,
		Ryona = 8,
		Vore = 9
	}

	public enum EnemyAttackType
	{
		BasicDamage = 0,
		SalamanderTongue = 1,
		LamiaSnake = 2,
		WWExplosion = 3,
		GhostReachHand = 4,
		TentacleTreeGrab = 5,
		PaintingTendrils = 6,
		DogBoyCharm = 7,
		WyvernFleshHook = 8,
		BlueFoxAttractorBeam = 9,
		BasicCharm = 10,
		Petrify = 11
	}

	public enum TempPlayerEffect
	{
		PullGrab = 0,
		WerewolfBossCurse = 1,
		LatexBodySuit = 2,
		Charmed = 3,
		Petrifying = 4,
		Petrified = 5,
		SlimeMelt = 6,
		Stuned = 7,
		DrinkATK = 8,
		DrinkDEF = 9,
		DrinkSpeed = 10,
		DrinkLifeRegen = 11,
		DrinkManaRegen = 12,
		FruitSlice = 13,
		FruitForbidden = 14,
		FruitDistract = 15,
		FruitWarrior = 16,
		MealHero = 17,
		Null = 18,
		Exhibitionist = 19
	}

	public enum SavePieceType
	{
		Pawn = 0,
		Rook = 1,
		Knight = 2,
		Bishop = 3,
		Queen = 4,
		Crystal = 5
	}

	public enum PlayerProximityType
	{
		SkillUsage = 0,
		MainBehaviour = 1,
		FaintedChase = 2,
		Adjacent = 3,
		PlayerToTheRightCustom = 4,
		PlayerToTheLeftCustom = 5,
		FlyingChase = 6,
		Custom = 7,
		CustomInverted = 8,
		CustomXOnly = 9,
		CustomXOnlyInverted = 10
	}

	public enum PlayerSkillAtk
	{
		SlidingTackle = 0,
		FlameSword = 1,
		GroundPound = 2,
		PoisonEffect = 3,
		RagePepper = 4,
		EntropicExplosion = 5
	}

	public enum PlayerDamageType
	{
		Null = 0,
		BasicAttack = 1,
		ThrowDagger = 2,
		FlameSword = 3,
		ForwardSlash = 4,
		GroundPound = 5,
		ReflectProjectile = 6,
		WeaponWhip = 7,
		SmallSwordSlash = 8,
		throwLance = 9,
		throwStar = 10,
		throwHoming = 11,
		poisonPowder = 12,
		ragePepper = 13,
		entropicExplosion = 14,
		slidingTackle = 15
	}

	public enum AnimPlayback
	{
		Restart = 0,
		Play = 1,
		LoopLock = 2,
		NextPhase = 3,
		SpeedLess = 4,
		SpeedMore = 5,
		Exit = 6,
		Faint = 7
	}

	public enum AudioType
	{
		All = 0,
		Music = 1,
		SFX = 2,
		Voice = 3,
		pairMUSSFX = 4,
		pairMUSVOI = 5,
		pairSFXVOI = 6
	}

	public enum PotionType
	{
		Health = 0,
		Spirit = 1,
		Brew = 2,
		Eat = 3
	}

	public enum EssenceSpawnType
	{
		EnemyDefeated = 0,
		EroRelease = 1,
		BossDefeated = 2,
		EroGrab = 3,
		SuccubusHeadress = 4,
		ragePepper = 5
	}

	public enum DressStatValue
	{
		Attack = 0,
		Defense = 1,
		Movement = 2,
		EroGather = 3
	}

	public enum MayaTraits
	{
		Masochist = 0,
		Exibitionist = 1,
		Horny = 2,
		Nymphomaniac = 3,
		VoreWhore = 4
	}

	private class GC_NO_REPEAT_VA
	{
		public MARange vGroup;

		public AudioClip vClip;
	}

	private string _strGameVersion = "Version 0.7.5 (bugfix1)";

	private float _fltGameVersion = 0.75f;

	private GameExecutable _gameExec;

	private GameBuildType _gameBuild;

	public GameObject objEssenceSmall;

	public GameObject objEssenceMedium;

	public GameObject objEssenceBig;

	public GameObject objStaggerEnemyVFX;

	private const string _DateTimeUniversalFormatSymbol = "O";

	private const float _worldBaseGravityCap = -3f;

	private const float _worldBaseGravityIncrements = -0.2f;

	private const float _playerMaxJumpForce = 3.75f;

	private const float _valueTimeToJumpAssist = 0.1f;

	private const float _dftSceneLoadFadeTimer = 0.5f;

	private const float _frequencyCountUpStatusEffect = 0.05f;

	private const int _maxSimulChase = 3;

	private const int _refEmptyShopSlotID = 9999;

	private const string _defaultLanguageDirectory = "Languages";

	private const string _defaultWebGLTextDirectory = "WEBGLjsonfiles";

	private const string _defaultLanguageListFileName = "LanguageList.json";

	private const string _defaultSaveDirectory = "UserData";

	private const string _defaultSaveFileName = "MKM_savefile";

	private const string _defaultConfigFileName = "MKM_configfile.json";

	private const string _defaultMenuTranslationFileName = "UserInterface.json";

	private const string _defaultNeneDialogueFileName = "Dialogue_Nene.json";

	private const string _defaultFarinaDialogueFileName = "Dialogue_Farina.json";

	private const string _defaultBoardTextFileName = "Dialogue_BoardSign.json";

	private const string _defaultVariousTextFileName = "Dialogue_Various.json";

	private const string _defaultItemDatabaseFileName = "ItemList.json";

	private const string _defaultIntroStoryFileName = "IntroStory.json";

	private const string _defaultScreenshotFileName = "MKMSVP";

	private const string _constLetterToneGroup = "DiagLetter";

	private const string _dftGPType1_Axis1Hor = "Horizontal-DPad";

	private const string _dftGPType1_Axis1Ver = "Vertical-DPad";

	private const string _dftGPType1_Axis2Hor = "Horizontal-StickLeft";

	private const string _dftGPType1_Axis2Ver = "Vertical-StickLeft";

	private const string _dftGPType1_Axis3Hor = "Horizontal-StickRight";

	private const string _dftGPType1_Axis3Ver = "Vertical-StickRight";

	private const string _dftGPType1_Axis4Spc = "XBOX-Triggers";

	private const string _dftGPType2_Axis1Hor = "XBOX-Horizontal-DPad";

	private const string _dftGPType2_Axis1Ver = "XBOX-Vertical-DPad";

	private const string _dftGPType2_Axis2Hor = "XBOX-Horizontal-StickLeft";

	private const string _dftGPType2_Axis2Ver = "XBOX-Vertical-StickLeft";

	private const string _dftGPType2_Axis3Hor = "XBOX-Horizontal-StickRight";

	private const string _dftGPType2_Axis3Ver = "XBOX-Vertical-StickRight";

	private const string _dftGPType2_Axis4Spc = "XBOX-Triggers";

	[HideInInspector]
	public static GameCore Instance;

	public ListOfLanguages clsLanguage = new ListOfLanguages();

	public ListOfTranslationUI clsTranslationUI = new ListOfTranslationUI();

	public ListOfItemDatabase clsItemDatabase = new ListOfItemDatabase();

	private UI_StatsScreenLastSeenValues _clsLastSeenStats = new UI_StatsScreenLastSeenValues();

	private Controller_Player _refPlayerScript;

	private MenuBehaviour _refMenuScript;

	private GamePlayerData coreGameData;

	private GamePlayerData retryCoreGameData;

	private GameConfigData coreConfigData;

	private GamePlayerFlagsTemporarySession _sessionFlags;

	private SaveLoadSlotEntry[] _arraySlotsOfSaveLoad;

	private List<BuffEffect> _playerEffectList = new List<BuffEffect>();

	private List<AccessoryBehavior> _accBehavior = new List<AccessoryBehavior>();

	private TotalAccessoryEffect _accModifiers = new TotalAccessoryEffect();

	private GameUI coreUI;

	private AudioSystem refAudioSystem;

	private GameObject mainEnemyParent;

	private GameObject _mainEffectParent;

	private GameObject _mainEnemiesParent;

	private GameObject mainCollParent;

	private GameObject mainContainerParent;

	private GameObject _objBossArena;

	private GameObject _previousSelectedOBJ;

	private GameObject _currentSelectedOBJ;

	private Animator objAN_Maya;

	private Animator[] objAN_GenericArray;

	private Coroutine crtBufferAnimControlAction;

	private Coroutine crtToggleMuteAudio;

	private Coroutine crtUINavigationMicroDelay;

	private Coroutine crtUniversalClickDelay;

	private List<ItemInfo> _listNeneShopState;

	private List<GamepadDirectionalStateControl> _listGPInputState = new List<GamepadDirectionalStateControl>();

	private List<KeyBindingItem> _defaultListOfKeyBinding;

	private List<KeyBindingItem> _defaultListOfKeyBindingGamepadType1;

	private List<KeyBindingItem> _defaultListOfKeyBindingGamepadType2;

	private Dictionary<string, int> _dicRNGint = new Dictionary<string, int>();

	private clsGalleryEnemyToView _galleryEnemy;

	private ENM_EroPhaseControl[] _defaultEroPhases;

	private FetishType _enemyGrabbingEroFetishType;

	private DialogueList _diagMenuFarina;

	private DialogueList _diagMenuNene;

	private EnemyType _enemyGrabbingPlayer = EnemyType.Isobora;

	private NPCType _currentNPCType;

	private EroAnimation _animControlScript;

	private Animator _animControlComponent;

	private GameObject _enemyGhostFutaSkillCasting;

	private GameObject _objEnemyGrabbingPlayer;

	private Button _lastSelectedInventoryTab;

	private Vector2 _playerCurrentRBVelocity;

	private Vector2 _teleportTargetPosition;

	private Vector2 _offsetCameraAfterTP;

	private Vector2 _playerLastUnstuckPosition;

	private Coroutine crtDelayAfterPause;

	private KeyCode _ListenGamepadBinding = KeyCode.Asterisk;

	private float _playerCurrentWorldXPosition;

	private float _playerCurrentWorldYPosition;

	private float _playerCurrentMoveX;

	private float _lastFadeTimeUsed;

	private float _rangeOfOrbAttraction;

	private float _discountShopAcumulation;

	private bool _isPersistGameSessionValues;

	private bool _isPlayerInteractingWithNPC;

	private bool _playerIsInvincible;

	private bool _playerIsHurt;

	private bool _playerIsAllowedToBeGrabbed;

	private bool _playerIsBeingGrabed;

	private bool _playerCancelStopEro;

	private bool _isPlayerFainted;

	private bool _playerDidTeleport;

	private bool _isGamePaused;

	private bool _isForbidAllPlayerInput;

	private bool _forceMovePlayerAfterTP;

	private bool _playerCanUseTeleport;

	private bool _isSceneTransitioning;

	private bool _isDelayCancelKeyPress;

	private bool _isDelayAfterExitPause;

	private bool _savingPlayerData;

	private bool _isPlayerGoneForGood;

	private bool _delayAfterkeyBound;

	private bool _canPlayerUpdateGlobals;

	private bool _eventPlayerReachedPoint;

	private bool _eventPlayerFinishedDialogue;

	private bool _eventIsPlaying;

	private bool _eventStartBossFight;

	private bool _canPlayerSaveGame;

	private bool _selectedRetry;

	private bool _playerCrouching;

	private bool _hasPlayedMayaDeathScream;

	private bool _isBossRematch;

	private bool _isAllowYMove;

	private bool _eventAnimationIsFinished;

	private bool _movHackRight;

	private bool _movHackLeft;

	private bool _enemyIsEroFatal;

	private bool _playerIsCrouching;

	private bool _cameraOffsetMovingForLever;

	private bool _playerIsOnGround;

	private bool _storedFullScrnState;

	private bool _currentFullScrnState;

	private bool _bufferAnimControlAction;

	private bool _isBadEnd;

	private bool _coreUI_IsDisplayingDialogue;

	private bool _trySkipToBadEndStart;

	private bool _muteSFX;

	private bool _muteVoice;

	private bool _muteMusic;

	private bool _isListeningGeneralKB;

	private bool _playerIntentDSC;

	private bool _isDisplayingSpecialMenuNPC;

	private bool _isDisplayingNPCMenuTalkDialogue;

	private bool _updateShopListAfterPurchase;

	private bool _eventStartGauntlet;

	private bool _debugForceReleaseEroGrab;

	private int _currentNPCSpeechID;

	private int _currentEroZoomLevel;

	private int _idCurrentSavePointScreenshot;

	private int _currentNumberOfEnemiesChasing;

	private int _currentPlayerFacing;

	private int _enemyEroPhase;

	private int _playerSpecialAnimCount;

	private int _quantityOfSpawnedPurpleSlimes;

	private int _galleryScreenLastThemeIndex;

	private int _galleryScreenLastEnemyIndex;

	private int _chanceEnemySpawnEssence;

	private int _lastChildIndexShopSlotAfterPurchase;

	private int _enemyGrabbingEroVariantID;

	private int _countGauntletGoal;

	private int _countGauntletKills;

	private string _strCurrentLanguageFolderPath;

	private string _playerCurrentState;

	private string _playerCurrentSkill;

	private string _strPreviousSceneName;

	private string _playerCurrentAtk;

	private string _playerCurrentAnimState;

	private string _sceneLoadAfterFade;

	private string _jsonUI;

	private string _jsonIntro;

	private string _jsonItemList;

	private string _jsonD_Nene;

	private string _jsonD_Farina;

	private string _jsonD_Board;

	private string _jsonD_Various;

	private string _axis_1_Ver;

	private string _axis_1_Hor;

	private string _axis_2_Ver;

	private string _axis_2_Hor;

	private string _axis_3_Ver;

	private string _axis_3_Hor;

	private string _axis_4_Spc;

	private List<GC_NO_REPEAT_VA> _listOfNoRepeatVoice = new List<GC_NO_REPEAT_VA>();

	private List<AudioClip> _listOfDistinctVoices = new List<AudioClip>();

	private AudioClip _lastVoicePlayed;

	public string strGameVersion => _strGameVersion;

	public GameExecutable GameExecType => _gameExec;

	public GameBuildType GameBuildCurrent => _gameBuild;

	public Vector2 teleportTargetPosition => _teleportTargetPosition;

	public Vector2 offsetCameraAfterTP => _offsetCameraAfterTP;

	public bool isSceneTransitioning
	{
		get
		{
			return _isSceneTransitioning;
		}
		set
		{
			_isSceneTransitioning = value;
		}
	}

	public bool forceMovePlayerAfterTP => _forceMovePlayerAfterTP;

	public bool isPersistGameSessionValues
	{
		get
		{
			return _isPersistGameSessionValues;
		}
		set
		{
			_isPersistGameSessionValues = value;
		}
	}

	public bool isAllowVerticalMovementDuringSceneLoad
	{
		get
		{
			return _isAllowYMove;
		}
		set
		{
			_isAllowYMove = value;
		}
	}

	public float worldBaseGravityCap => -3f;

	public float worldBaseGravityIncrements => -0.2f;

	public float playerMaxJumpForce => 3.75f;

	public float valueTimeOfJumpAssist => 0.1f;

	public bool isPlayerInteractingLocked
	{
		get
		{
			return _isPlayerInteractingWithNPC;
		}
		set
		{
			_isPlayerInteractingWithNPC = value;
		}
	}

	public bool isPlayerFainted
	{
		get
		{
			return _isPlayerFainted;
		}
		set
		{
			_isPlayerFainted = value;
		}
	}

	public bool isPlayerGoneForGood
	{
		get
		{
			return _isPlayerGoneForGood;
		}
		set
		{
			_isPlayerGoneForGood = value;
		}
	}

	public string strCurrentLanguageFolderPath => _strCurrentLanguageFolderPath;

	public string strCurrentActiveSceneName => SceneManager.GetActiveScene().name;

	public string strPreviousSceneName => _strPreviousSceneName;

	public string strCurrentActiveStageName => TranslateSceneNameToStageName(isCurrentlyActive: true, "");

	public bool isGamePaused => _isGamePaused;

	public bool isForbidAllPlayerInput
	{
		get
		{
			return _isForbidAllPlayerInput;
		}
		set
		{
			_isForbidAllPlayerInput = value;
		}
	}

	public bool isDelayCancelKeyPress
	{
		get
		{
			return _isDelayCancelKeyPress;
		}
		set
		{
			_isDelayCancelKeyPress = value;
		}
	}

	public bool savingPlayerData
	{
		get
		{
			return _savingPlayerData;
		}
		set
		{
			_savingPlayerData = value;
		}
	}

	public bool delayAfterkeyBound
	{
		get
		{
			return _delayAfterkeyBound;
		}
		set
		{
			_delayAfterkeyBound = value;
		}
	}

	public int currentEroZoomLevel
	{
		get
		{
			return _currentEroZoomLevel;
		}
		set
		{
			_currentEroZoomLevel = value;
		}
	}

	public int intLastZoomLevel
	{
		get
		{
			return coreConfigData.intLastZoomLevel;
		}
		set
		{
			coreConfigData.intLastZoomLevel = value;
		}
	}

	public bool canPlayerCanUpdateGlobalVariables
	{
		get
		{
			return _canPlayerUpdateGlobals;
		}
		set
		{
			_canPlayerUpdateGlobals = value;
		}
	}

	public bool selectedRetry
	{
		get
		{
			return _selectedRetry;
		}
		set
		{
			_selectedRetry = value;
		}
	}

	public bool isDisplayingMinimap => MINIMAP_IsDisplaying();

	public int chanceEnemySpawnEssence
	{
		get
		{
			return _chanceEnemySpawnEssence;
		}
		set
		{
			_chanceEnemySpawnEssence = value;
		}
	}

	public float rangeOfOrbAttraction
	{
		get
		{
			return _rangeOfOrbAttraction;
		}
		set
		{
			_rangeOfOrbAttraction = value;
		}
	}

	public GameObject refOBJEffectsParent
	{
		get
		{
			CheckRefEffectsParent();
			return _mainEffectParent;
		}
	}

	public GameObject refOBJEnemiesParent
	{
		get
		{
			CheckRefEnemiesParent();
			return _mainEnemiesParent;
		}
	}

	public bool cameraOffsetMovingForLever
	{
		get
		{
			return _cameraOffsetMovingForLever;
		}
		set
		{
			_cameraOffsetMovingForLever = value;
			if (!value)
			{
				OverrideCameraMove(Vector2.zero, resetCameraToDefault: true, instantReset: true);
			}
		}
	}

	public bool movHackRight => _movHackRight;

	public bool movHackLeft => _movHackLeft;

	public float galleryPlayerX_ArenaLimit => -90f;

	public float globalDefaultSceneFadeTimer => 0.5f;

	public int constShopStockInfinite => coreGameData.defaultInfiniteShopStock;

	public int constEmptyShopSlotID => 9999;

	public bool updateShopListAfterPurchase
	{
		get
		{
			return _updateShopListAfterPurchase;
		}
		set
		{
			_updateShopListAfterPurchase = value;
		}
	}

	public int lastChildIndexShopSlotAfterPurchase => _lastChildIndexShopSlotAfterPurchase;

	public bool debugForceReleaseEroGrab
	{
		get
		{
			return _debugForceReleaseEroGrab;
		}
		set
		{
			_debugForceReleaseEroGrab = value;
		}
	}

	public ENM_EroPhaseControl[] defaultEroPhases => _defaultEroPhases;

	public int idCurrentSavePointScreenshot
	{
		get
		{
			return _idCurrentSavePointScreenshot;
		}
		set
		{
			_idCurrentSavePointScreenshot = value;
		}
	}

	public UI_StatsScreenLastSeenValues clsLastSeenStats
	{
		get
		{
			return _clsLastSeenStats;
		}
		set
		{
			_clsLastSeenStats = value;
		}
	}

	public bool isDisplayingDialogue
	{
		get
		{
			return _coreUI_IsDisplayingDialogue;
		}
		set
		{
			_coreUI_IsDisplayingDialogue = value;
		}
	}

	public bool isDisplayingNPCMenuTalkDialogue
	{
		get
		{
			return _isDisplayingNPCMenuTalkDialogue;
		}
		set
		{
			_isDisplayingNPCMenuTalkDialogue = value;
		}
	}

	public bool isDisplayingSpecialMenuNPC
	{
		get
		{
			return _isDisplayingSpecialMenuNPC;
		}
		set
		{
			_isDisplayingSpecialMenuNPC = value;
		}
	}

	public Button lastSelectedInventoryTab
	{
		get
		{
			return _lastSelectedInventoryTab;
		}
		set
		{
			_lastSelectedInventoryTab = value;
		}
	}

	public int galleryScreenLastThemeIndex
	{
		get
		{
			return _galleryScreenLastThemeIndex;
		}
		set
		{
			_galleryScreenLastThemeIndex = value;
		}
	}

	public int galleryScreenLastEnemyIndex
	{
		get
		{
			return _galleryScreenLastEnemyIndex;
		}
		set
		{
			_galleryScreenLastEnemyIndex = value;
		}
	}

	public float playerCurrentHealth
	{
		get
		{
			return coreGameData.intPlayerCurrentHealth;
		}
		set
		{
			coreGameData.intPlayerCurrentHealth = value;
		}
	}

	public int playerMaxHealth
	{
		get
		{
			return _accModifiers.HP_RESULT;
		}
		set
		{
			coreGameData.intPlayerMaxHealth = value;
		}
	}

	public float playerCurrentSpirit
	{
		get
		{
			return coreGameData.intPlayerCurrentSpirit;
		}
		set
		{
			coreGameData.intPlayerCurrentSpirit = value;
		}
	}

	public float playerMaxSpirit
	{
		get
		{
			return _accModifiers.SP_RESULT;
		}
		set
		{
			coreGameData.intPlayerMaxSpirit = value;
		}
	}

	public float playerBaseSpiritRegeneration
	{
		get
		{
			return coreGameData.intPlayerBaseSpiritRegeneration;
		}
		set
		{
			coreGameData.intPlayerBaseSpiritRegeneration = value;
		}
	}

	public int playerCurrentDressState
	{
		get
		{
			return coreGameData.intPlayerCurrentDressState;
		}
		set
		{
			coreGameData.intPlayerCurrentDressState = value;
		}
	}

	public int playerCurrentDressStateValidUnbroken
	{
		get
		{
			return coreGameData.intPlayerCurrentDressStateValidUnbroken;
		}
		set
		{
			coreGameData.intPlayerCurrentDressStateValidUnbroken = value;
		}
	}

	public int playerMaxDressDurability
	{
		get
		{
			return coreGameData.intPlayerMaxDressDurability;
		}
		set
		{
			coreGameData.intPlayerMaxDressDurability = value;
		}
	}

	public int playerCurrentDressDurability
	{
		get
		{
			return coreGameData.intPlayerCurrentDressDurability;
		}
		set
		{
			coreGameData.intPlayerCurrentDressDurability = value;
		}
	}

	public int playerCurrentBasicAttackPower => _accModifiers.ATK_RESULT;

	public int playerLust => coreGameData.intLustAmount;

	public int playerCurrentBasicDefensePower
	{
		get
		{
			if (ExistsEffectOnPlayer(TempPlayerEffect.DrinkDEF))
			{
				return Mathf.RoundToInt((float)_accModifiers.DEF_RESULT * 1.55f);
			}
			return _accModifiers.DEF_RESULT;
		}
		set
		{
			coreGameData.intPlayerCurrentBasicDefensePower = value;
		}
	}

	public int playerCurrentBasicAttackVariation
	{
		get
		{
			return coreGameData.intPlayerCurrentBasicAtackVariation;
		}
		set
		{
			coreGameData.intPlayerCurrentBasicAtackVariation = value;
		}
	}

	public int playerCurrentEssenceAmount => coreGameData.intPlayerCurrentEssenceAmount;

	public string playerSaveScene
	{
		get
		{
			return coreGameData.strPlayerSaveScene;
		}
		set
		{
			coreGameData.strPlayerSaveScene = value;
		}
	}

	public Vector3 playerSavePosition
	{
		get
		{
			return coreGameData.vecPlayerSavePosition;
		}
		set
		{
			coreGameData.vecPlayerSavePosition = value;
		}
	}

	public float playerCurrentWorldXPosition
	{
		get
		{
			return _playerCurrentWorldXPosition;
		}
		set
		{
			_playerCurrentWorldXPosition = value;
		}
	}

	public float playerCurrentWorldYPosition
	{
		get
		{
			return _playerCurrentWorldYPosition;
		}
		set
		{
			_playerCurrentWorldYPosition = value;
		}
	}

	public float playerCurrentMoveX
	{
		get
		{
			return _playerCurrentMoveX;
		}
		set
		{
			_playerCurrentMoveX = value;
		}
	}

	public Vector2 playerCurrentRBVelocity
	{
		get
		{
			return _playerCurrentRBVelocity;
		}
		set
		{
			_playerCurrentRBVelocity = value;
		}
	}

	public bool playerIsInvincible
	{
		get
		{
			return _playerIsInvincible;
		}
		set
		{
			_playerIsInvincible = value;
		}
	}

	public bool canPlayerSaveGame
	{
		get
		{
			return _canPlayerSaveGame;
		}
		set
		{
			_canPlayerSaveGame = value;
		}
	}

	public bool playerIsHurt
	{
		get
		{
			return _playerIsHurt;
		}
		set
		{
			_playerIsHurt = value;
		}
	}

	public bool playerIsBeingGrabed
	{
		get
		{
			return _playerIsBeingGrabed;
		}
		set
		{
			_playerIsBeingGrabed = value;
		}
	}

	public int currentPlayerFacing
	{
		get
		{
			return _currentPlayerFacing;
		}
		set
		{
			_currentPlayerFacing = value;
		}
	}

	public string playerCurrentState
	{
		get
		{
			return _playerCurrentState;
		}
		set
		{
			_playerCurrentState = value;
		}
	}

	public string playerCurrentSkill
	{
		get
		{
			return _playerCurrentSkill;
		}
		set
		{
			_playerCurrentSkill = value;
		}
	}

	public bool playerIsCrouching
	{
		get
		{
			return _playerIsCrouching;
		}
		set
		{
			_playerIsCrouching = value;
		}
	}

	public bool playerIsOnGround
	{
		get
		{
			return _playerIsOnGround;
		}
		set
		{
			_playerIsOnGround = value;
		}
	}

	public List<ItemInfo> listPlayerInventory => coreGameData.listOfItemInventory;

	public List<ItemInfo> listShopState => _listNeneShopState;

	public Transform playerTransformReference
	{
		get
		{
			CheckRefPlayer();
			return _refPlayerScript.transform;
		}
	}

	public List<BuffEffect> playerEffectList => _playerEffectList;

	public List<AccessoryBehavior> listAccBehavior => _accBehavior;

	public TotalAccessoryEffect AccModifiers => _accModifiers;

	public GamePlayerFlags flagsMain => coreGameData.gameFlags;

	public GamePlayerSkillFlags skillFlags => coreGameData.skillFlags;

	public GamePlayerFlagsTemporarySession sessionFlags
	{
		get
		{
			return _sessionFlags;
		}
		set
		{
			_sessionFlags = value;
		}
	}

	public SaveLoadSlotEntry[] arraySlotsOfSaveLoad => _arraySlotsOfSaveLoad;

	public CountItemsQuantity[] arraySlotsOfBelt => coreGameData.arrayBeltSlots;

	public List<int> playerListOfVisitedSavePoints => coreGameData.listOfVisitedSavePoints;

	public List<GameLeverSignalState> listOfLeverSignalState => coreGameData.listOfSignalState;

	public int indexCurrentBeltSlotActive => coreGameData.intIndexOfSlotCurrentActiveItem;

	public int timeTotalGamePlayedSeconds
	{
		get
		{
			return coreGameData.timeGamePlayed;
		}
		set
		{
			coreGameData.timeGamePlayed = value;
		}
	}

	public bool playerDidTeleport
	{
		get
		{
			return _playerDidTeleport;
		}
		set
		{
			_playerDidTeleport = value;
		}
	}

	public bool playerCanUseTeleport
	{
		get
		{
			return _playerCanUseTeleport;
		}
		set
		{
			_playerCanUseTeleport = value;
		}
	}

	public bool playerIsAllowedToBeGrabbed
	{
		get
		{
			return _playerIsAllowedToBeGrabbed;
		}
		set
		{
			_playerIsAllowedToBeGrabbed = value;
		}
	}

	public bool playerCancelStopEro
	{
		get
		{
			return _playerCancelStopEro;
		}
		set
		{
			_playerCancelStopEro = value;
		}
	}

	public string playerCurrentAnimState
	{
		get
		{
			return _playerCurrentAnimState;
		}
		set
		{
			_playerCurrentAnimState = value;
		}
	}

	public string playerCurrentAttackState
	{
		get
		{
			return _playerCurrentAtk;
		}
		set
		{
			_playerCurrentAtk = value;
		}
	}

	public int playerSpecialAnimCount
	{
		get
		{
			return _playerSpecialAnimCount;
		}
		set
		{
			_playerSpecialAnimCount = value;
		}
	}

	public bool playerIsIntentionalDressStateChange
	{
		get
		{
			return _playerIntentDSC;
		}
		set
		{
			_playerIntentDSC = value;
		}
	}

	public Vector2 playerLastUnstuckPosition
	{
		get
		{
			return _playerLastUnstuckPosition;
		}
		set
		{
			_playerLastUnstuckPosition = value;
		}
	}

	public float discountShopAcumulation
	{
		get
		{
			return _discountShopAcumulation;
		}
		set
		{
			int num = 30;
			int num2 = 400;
			int num3 = 10;
			_discountShopAcumulation = value;
			if (discountShopTotal == num)
			{
				_discountShopAcumulation = 0f;
				return;
			}
			while (_discountShopAcumulation >= (float)num2 && num3 > 0)
			{
				num3--;
				_discountShopAcumulation -= num2;
				discountShopTotal++;
			}
			if (discountShopTotal > num)
			{
				discountShopTotal = num;
			}
		}
	}

	public int discountShopTotal
	{
		get
		{
			return coreGameData.intDiscountShop;
		}
		set
		{
			coreGameData.intDiscountShop = value;
		}
	}

	public int permaTraitCountMasoch
	{
		get
		{
			return coreGameData.permaTraitCountMasoch;
		}
		set
		{
			if (coreGameData.permaTraitCountMasoch < 999)
			{
				coreGameData.permaTraitCountMasoch = value;
			}
		}
	}

	public int permaTraitCountExib
	{
		get
		{
			return coreGameData.permaTraitCountExib;
		}
		set
		{
			if (coreGameData.permaTraitCountExib < 999)
			{
				coreGameData.permaTraitCountExib = value;
			}
		}
	}

	public int permaTraitCountHorny
	{
		get
		{
			return coreGameData.permaTraitCountHorny;
		}
		set
		{
			if (coreGameData.permaTraitCountHorny < 999)
			{
				coreGameData.permaTraitCountHorny = value;
			}
		}
	}

	public int permaTraitCountNympho
	{
		get
		{
			return coreGameData.permaTraitCountNympho;
		}
		set
		{
			if (coreGameData.permaTraitCountNympho < 999)
			{
				coreGameData.permaTraitCountNympho = value;
			}
		}
	}

	public int npcCurrentNeneDialogueID
	{
		get
		{
			return coreGameData.npcFlags.currentNeneStoryDialogueID;
		}
		set
		{
			coreGameData.npcFlags.currentNeneStoryDialogueID = value;
		}
	}

	public int npcCurrentFarinaDialogueID
	{
		get
		{
			return coreGameData.npcFlags.currentFarinaStoryDialogueID;
		}
		set
		{
			coreGameData.npcFlags.currentFarinaStoryDialogueID = value;
		}
	}

	public NPCType lastNPCTypeInteracted
	{
		get
		{
			return _currentNPCType;
		}
		set
		{
			_currentNPCType = value;
		}
	}

	public int currentNumberOfEnemiesChasing => _currentNumberOfEnemiesChasing;

	public EnemyType enemyGrabbingPlayer
	{
		get
		{
			return _enemyGrabbingPlayer;
		}
		set
		{
			_enemyGrabbingPlayer = value;
		}
	}

	public GameObject objEnemyGrabbingPlayer
	{
		get
		{
			return _objEnemyGrabbingPlayer;
		}
		set
		{
			_objEnemyGrabbingPlayer = value;
		}
	}

	public int enemyGrabbingEroPhase
	{
		get
		{
			return _enemyEroPhase;
		}
		set
		{
			_enemyEroPhase = value;
		}
	}

	public int enemyGrabbingEroVariantID
	{
		get
		{
			return _enemyGrabbingEroVariantID;
		}
		set
		{
			_enemyGrabbingEroVariantID = value;
		}
	}

	public FetishType enemyGrabbingEroFetishType
	{
		get
		{
			return _enemyGrabbingEroFetishType;
		}
		set
		{
			_enemyGrabbingEroFetishType = value;
		}
	}

	public bool currentEnemyGrabbingIsEroFatal
	{
		get
		{
			return _enemyIsEroFatal;
		}
		set
		{
			_enemyIsEroFatal = value;
		}
	}

	public bool isBossRematch
	{
		get
		{
			return _isBossRematch;
		}
		set
		{
			_isBossRematch = value;
		}
	}

	public GameObject enemyGhostFutaSkillCasting
	{
		get
		{
			return _enemyGhostFutaSkillCasting;
		}
		set
		{
			_enemyGhostFutaSkillCasting = value;
		}
	}

	public int quantityOfSpawnedPurpleSlimes
	{
		get
		{
			return _quantityOfSpawnedPurpleSlimes;
		}
		set
		{
			_quantityOfSpawnedPurpleSlimes = value;
		}
	}

	public clsGalleryEnemyToView enemyViewGallery => _galleryEnemy;

	public bool isRunningBadEndLogic
	{
		get
		{
			return _isBadEnd;
		}
		set
		{
			_isBadEnd = value;
		}
	}

	public bool trySkipToBadEndStart
	{
		get
		{
			return _trySkipToBadEndStart;
		}
		set
		{
			_trySkipToBadEndStart = value;
		}
	}

	public int countGauntletGoal
	{
		get
		{
			return _countGauntletGoal;
		}
		set
		{
			_countGauntletGoal = value;
		}
	}

	public int countGauntletKills
	{
		get
		{
			return _countGauntletKills;
		}
		set
		{
			_countGauntletKills = value;
		}
	}

	public DifficultyType configDifficultyType
	{
		get
		{
			return (DifficultyType)coreConfigData.intDifficulty;
		}
		set
		{
			coreConfigData.intDifficulty = (int)value;
		}
	}

	public int configLanguageType
	{
		get
		{
			return coreConfigData.intLanguage;
		}
		set
		{
			coreConfigData.intLanguage = value;
		}
	}

	public CameraType configCameraType
	{
		get
		{
			return (CameraType)coreConfigData.intCamera;
		}
		set
		{
			coreConfigData.intCamera = (int)value;
		}
	}

	public int configTextSpeed
	{
		get
		{
			return coreConfigData.intTextSpeed;
		}
		set
		{
			coreConfigData.intTextSpeed = value;
		}
	}

	public float configTextVolume
	{
		get
		{
			if (_muteMusic)
			{
				return 0f;
			}
			return coreConfigData.fltTextVol;
		}
		set
		{
			coreConfigData.fltTextVol = value;
		}
	}

	public float configMusicVolume
	{
		get
		{
			if (_muteMusic)
			{
				return 0f;
			}
			return coreConfigData.fltMusicVol;
		}
		set
		{
			coreConfigData.fltMusicVol = value;
		}
	}

	public float configSoundEffectVolume
	{
		get
		{
			if (_muteSFX)
			{
				return 0f;
			}
			return coreConfigData.fltSoundVol;
		}
		set
		{
			coreConfigData.fltSoundVol = value;
		}
	}

	public float configVoiceVolume
	{
		get
		{
			if (_muteVoice)
			{
				return 0f;
			}
			return coreConfigData.fltVoiceVol;
		}
		set
		{
			coreConfigData.fltVoiceVol = value;
		}
	}

	public float configVoiceRate
	{
		get
		{
			return coreConfigData.fltVoiceRate;
		}
		set
		{
			coreConfigData.fltVoiceRate = value;
		}
	}

	public bool configMasterIsGalleryEnabled
	{
		get
		{
			return coreConfigData.configIsGalleryEnabled;
		}
		set
		{
			coreConfigData.configIsGalleryEnabled = value;
		}
	}

	public bool configGalleryRoomBeach
	{
		get
		{
			return coreConfigData.galleryRoomBeach;
		}
		set
		{
			coreConfigData.galleryRoomBeach = value;
		}
	}

	public bool configGalleryRoomForest
	{
		get
		{
			return coreConfigData.galleryRoomForest;
		}
		set
		{
			coreConfigData.galleryRoomForest = value;
		}
	}

	public bool configGalleryRoomHill
	{
		get
		{
			return coreConfigData.galleryRoomHill;
		}
		set
		{
			coreConfigData.galleryRoomHill = value;
		}
	}

	public bool configGalleryRoomCastle
	{
		get
		{
			return coreConfigData.galleryRoomCastle;
		}
		set
		{
			coreConfigData.galleryRoomCastle = value;
		}
	}

	public bool configGalleryRoomCaves
	{
		get
		{
			return coreConfigData.galleryRoomCaves;
		}
		set
		{
			coreConfigData.galleryRoomCaves = value;
		}
	}

	public bool configGalleryRoomCity
	{
		get
		{
			return coreConfigData.galleryRoomCity;
		}
		set
		{
			coreConfigData.galleryRoomCity = value;
		}
	}

	public bool configGalleryRoomSwamp
	{
		get
		{
			return coreConfigData.galleryRoomSwamp;
		}
		set
		{
			coreConfigData.galleryRoomSwamp = value;
		}
	}

	public bool configGalleryRoomHighTech
	{
		get
		{
			return coreConfigData.galleryRoomHighTech;
		}
		set
		{
			coreConfigData.galleryRoomHighTech = value;
		}
	}

	public bool canApplySlowZoomEffectOnDressBreak
	{
		get
		{
			return coreConfigData.adv.toggleSlowZoomOnDressBreak;
		}
		set
		{
			coreConfigData.adv.toggleSlowZoomOnDressBreak = value;
		}
	}

	public bool canApplyAutoZoomOnEroGrab
	{
		get
		{
			return coreConfigData.adv.toggleAutoZoomOnEroGrab;
		}
		set
		{
			coreConfigData.adv.toggleAutoZoomOnEroGrab = value;
		}
	}

	public bool canShowBlockedDamage
	{
		get
		{
			return coreConfigData.adv.toggleShowBlockedDamage;
		}
		set
		{
			coreConfigData.adv.toggleShowBlockedDamage = value;
		}
	}

	public bool doQuickSaveFeature
	{
		get
		{
			return coreConfigData.adv.toggleQuickSave;
		}
		set
		{
			coreConfigData.adv.toggleQuickSave = value;
		}
	}

	public bool doAutoSaveFeature
	{
		get
		{
			return coreConfigData.adv.toggleAutoSave;
		}
		set
		{
			coreConfigData.adv.toggleAutoSave = value;
		}
	}

	public bool doAutoEquipFeature
	{
		get
		{
			return coreConfigData.adv.toggleAutoEquipOnPickup;
		}
		set
		{
			coreConfigData.adv.toggleAutoEquipOnPickup = value;
		}
	}

	public bool doRememberLastZoomAmount
	{
		get
		{
			return coreConfigData.adv.toggleRememberLastZoom;
		}
		set
		{
			coreConfigData.adv.toggleRememberLastZoom = value;
		}
	}

	public bool doDisplayLinesMechanisms
	{
		get
		{
			return coreConfigData.adv.ToggleDisplayLinesMechanism;
		}
		set
		{
			coreConfigData.adv.ToggleDisplayLinesMechanism = value;
		}
	}

	public bool doAllVoreScenesAreFatal
	{
		get
		{
			return coreConfigData.adv.ToggleAllVoreIsFatal;
		}
		set
		{
			coreConfigData.adv.ToggleAllVoreIsFatal = value;
		}
	}

	public bool doGalleryHideBlockedScenes
	{
		get
		{
			return coreConfigData.adv.ToggleGalleryHideScenes;
		}
		set
		{
			coreConfigData.adv.ToggleGalleryHideScenes = value;
		}
	}

	public bool doAllowBadEnd
	{
		get
		{
			return coreConfigData.adv.ToggleBadEnd;
		}
		set
		{
			coreConfigData.adv.ToggleBadEnd = value;
		}
	}

	public bool doAllowDialogueDuringBadEnd
	{
		get
		{
			return coreConfigData.adv.ToggleDisplayDialogueBadEnd;
		}
		set
		{
			coreConfigData.adv.ToggleDisplayDialogueBadEnd = value;
		}
	}

	public bool lock60fps
	{
		get
		{
			return coreConfigData.adv.ToggleLock60fps;
		}
		set
		{
			coreConfigData.adv.ToggleLock60fps = value;
		}
	}

	public bool revertToOldStruggleBar
	{
		get
		{
			return coreConfigData.adv.ToggleToOldStruggleBar;
		}
		set
		{
			coreConfigData.adv.ToggleToOldStruggleBar = value;
		}
	}

	public bool doAllowDebug
	{
		get
		{
			return coreConfigData.adv.doAllowDebug;
		}
		set
		{
			coreConfigData.adv.doAllowDebug = value;
		}
	}

	public bool disableCursorAnimUI
	{
		get
		{
			return coreConfigData.adv.disableCursorAnimUI;
		}
		set
		{
			coreConfigData.adv.disableCursorAnimUI = value;
		}
	}

	public bool disableVoiceLogo
	{
		get
		{
			return coreConfigData.adv.disableVoiceLogo;
		}
		set
		{
			coreConfigData.adv.disableVoiceLogo = value;
		}
	}

	public bool showInternalShots
	{
		get
		{
			return coreConfigData.adv.toggleInternalShots;
		}
		set
		{
			coreConfigData.adv.toggleInternalShots = value;
			SaveGameConfiguration();
		}
	}

	public CFG_FetishToggle configFetishes
	{
		get
		{
			return coreConfigData.fetishes;
		}
		set
		{
			coreConfigData.fetishes = value;
		}
	}

	public bool configToggleMinimapState
	{
		get
		{
			return coreConfigData.toggleMinimapState;
		}
		set
		{
			coreConfigData.toggleMinimapState = value;
		}
	}

	public bool eventIsPlaying
	{
		get
		{
			return _eventIsPlaying;
		}
		set
		{
			_eventIsPlaying = value;
		}
	}

	public bool eventPlayerReachedPoint
	{
		get
		{
			return _eventPlayerReachedPoint;
		}
		set
		{
			_eventPlayerReachedPoint = value;
		}
	}

	public bool eventPlayerFinishedDialogue
	{
		get
		{
			return _eventPlayerFinishedDialogue;
		}
		set
		{
			_eventPlayerFinishedDialogue = value;
		}
	}

	public bool eventAnimationIsFinished
	{
		get
		{
			return _eventAnimationIsFinished;
		}
		set
		{
			_eventAnimationIsFinished = value;
		}
	}

	public bool isBossFight
	{
		get
		{
			return _eventStartBossFight;
		}
		set
		{
			_eventStartBossFight = value;
		}
	}

	public bool isGauntletFight
	{
		get
		{
			return _eventStartGauntlet;
		}
		set
		{
			_eventStartGauntlet = value;
		}
	}

	public GameObject currentSelectedUISelectable => _currentSelectedOBJ;

	public InputType configInputType
	{
		get
		{
			return (InputType)coreConfigData.intInput;
		}
		set
		{
			coreConfigData.intInput = (int)value;
			INPUT_GPUpdateAxisNames(value);
		}
	}

	public List<KeyBindingItem> inputList
	{
		get
		{
			switch (coreConfigData.intInput)
			{
			default:
				return coreConfigData.listOfKeyBinding;
			case 1:
				return coreConfigData.listOfKeyBindingGamepadType1;
			case 2:
				return coreConfigData.listOfKeyBindingGamepadType2;
			}
		}
		set
		{
			switch (coreConfigData.intInput)
			{
			default:
				coreConfigData.listOfKeyBinding = value;
				break;
			case 1:
				coreConfigData.listOfKeyBindingGamepadType1 = value;
				break;
			case 2:
				coreConfigData.listOfKeyBindingGamepadType2 = value;
				break;
			}
		}
	}

	public KeyCode ListenGamepadBinding
	{
		get
		{
			return _ListenGamepadBinding;
		}
		set
		{
			_ListenGamepadBinding = value;
		}
	}

	public bool isListeningForGeneralKeyBindings
	{
		get
		{
			return _isListeningGeneralKB;
		}
		set
		{
			_isListeningGeneralKB = value;
		}
	}

	public bool isKeyboardCfgInput
	{
		get
		{
			if (configInputType == InputType.Keyboard)
			{
				return true;
			}
			return false;
		}
	}

	public bool isGamepadCfgInput
	{
		get
		{
			InputType inputType = configInputType;
			if ((uint)(inputType - 1) <= 1u)
			{
				return true;
			}
			return false;
		}
	}

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		_storedFullScrnState = Screen.fullScreen;
		_currentFullScrnState = _storedFullScrnState;
		if (coreConfigData != null)
		{
			INPUT_GPUpdateAxisNames(configInputType);
			return;
		}
		_axis_1_Ver = "Vertical-DPad";
		_axis_1_Hor = "Horizontal-DPad";
		_axis_2_Ver = "Vertical-StickLeft";
		_axis_2_Hor = "Horizontal-StickLeft";
		_axis_3_Ver = "Vertical-StickRight";
		_axis_3_Hor = "Horizontal-StickRight";
		_axis_4_Spc = "XBOX-Triggers";
	}

	private void Update()
	{
		UpdateGamepadDirectionalStateControl();
		UPDT_UINavigation();
		_currentFullScrnState = Screen.fullScreen;
		if (_currentFullScrnState != _storedFullScrnState)
		{
			if (_currentFullScrnState)
			{
				ForceWindowScreenResolution();
			}
			_storedFullScrnState = _currentFullScrnState;
		}
	}

	private void UPDT_UINavigation()
	{
		EventSystem current = EventSystem.current;
		if ((object)current == null || current.currentSelectedGameObject?.activeInHierarchy != true || _isListeningGeneralKB || crtUINavigationMicroDelay != null || delayAfterkeyBound)
		{
			return;
		}
		Selectable selectable = EventSystem.current?.currentSelectedGameObject?.GetComponent<Selectable>();
		if (!(selectable == null))
		{
			Selectable selectable2 = null;
			float wait = 0.125f;
			float wait2 = 0.125f;
			if (UPDT_UIMove(InputLib.Up, InputPressType.KeyHolding, KeyCode.UpArrow))
			{
				selectable2 = selectable.FindSelectableOnUp();
				OnGUICheckScrollBars(selectable, directionUp: true);
				crtUINavigationMicroDelay = StartCoroutine(waitUINavigationMicroDelay(wait2));
			}
			else if (UPDT_UIMove(InputLib.Left, InputPressType.KeyHolding, KeyCode.LeftArrow))
			{
				selectable2 = selectable.FindSelectableOnLeft();
				OnGUICheckSliders(selectable, directionRight: false);
				crtUINavigationMicroDelay = StartCoroutine(waitUINavigationMicroDelay(wait));
			}
			else if (UPDT_UIMove(InputLib.Down, InputPressType.KeyHolding, KeyCode.DownArrow))
			{
				selectable2 = selectable.FindSelectableOnDown();
				OnGUICheckScrollBars(selectable, directionUp: false);
				crtUINavigationMicroDelay = StartCoroutine(waitUINavigationMicroDelay(wait2));
			}
			else if (UPDT_UIMove(InputLib.Right, InputPressType.KeyHolding, KeyCode.RightArrow))
			{
				selectable2 = selectable.FindSelectableOnRight();
				OnGUICheckSliders(selectable, directionRight: true);
				crtUINavigationMicroDelay = StartCoroutine(waitUINavigationMicroDelay(wait));
			}
			if ((bool)selectable2)
			{
				selectable2.Select();
			}
		}
	}

	private bool UPDT_UIMove(InputLib dir, InputPressType type, KeyCode arrowUniversal)
	{
		if (UnifiedInputHandlerGamepadDirectional(dir, type) || UnifiedInputHandlerTypeOfPress(dir, type) || Input.GetKeyDown(arrowUniversal))
		{
			return true;
		}
		return false;
	}

	private void ForceWindowScreenResolution()
	{
		Screen.SetResolution(1920, 1080, fullscreen: true);
	}

	private IEnumerator waitUINavigationMicroDelay(float wait)
	{
		yield return new WaitForSecondsRealtime(wait);
		crtUINavigationMicroDelay = null;
	}

	private void OnGUI()
	{
		Event current = Event.current;
		if (isKeyboardCfgInput && current.isKey)
		{
			if (current.keyCode == IKey(InputLib.Right))
			{
				if (current.type == EventType.KeyDown)
				{
					_movHackRight = true;
				}
				else
				{
					_movHackRight = false;
				}
			}
			if (current.keyCode == IKey(InputLib.Left))
			{
				if (current.type == EventType.KeyDown)
				{
					_movHackLeft = true;
				}
				else
				{
					_movHackLeft = false;
				}
			}
		}
		if (isGamepadCfgInput)
		{
			if (UnifiedInputHandlerGamepadDirectional(InputLib.Right, InputPressType.KeyHolding))
			{
				_movHackRight = true;
			}
			else
			{
				_movHackRight = false;
			}
			if (UnifiedInputHandlerGamepadDirectional(InputLib.Left, InputPressType.KeyHolding))
			{
				_movHackLeft = true;
			}
			else
			{
				_movHackLeft = false;
			}
		}
	}

	public KeyCode TranslateGamepadInput()
	{
		foreach (GamepadDirectionalStateControl item in _listGPInputState)
		{
			if (ReadGamepadDirectionalInput(item.direction, item.axisNameHor, item.axisNameVer))
			{
				return item.reservedKey;
			}
		}
		return KeyCode.Asterisk;
	}

	private bool ReadGamepadDirectionalInput(GamepadDirection input, string strAxisHor = "", string strAxisVer = "", bool checkTheState = false, InputPressType withThisState = InputPressType.Null, float deadZone = 0.75f)
	{
		if (!isGamepadCfgInput)
		{
			return false;
		}
		if (_isGamePaused)
		{
			deadZone = 0.5f;
		}
		InputPressType inputPressType = InputPressType.Null;
		bool flag = false;
		switch (input)
		{
		case GamepadDirection.Up:
			if (Input.GetAxis(strAxisVer) >= deadZone)
			{
				flag = true;
			}
			break;
		case GamepadDirection.Left:
			if (Input.GetAxis(strAxisHor) <= 0f - deadZone)
			{
				flag = true;
			}
			break;
		case GamepadDirection.Down:
			if (Input.GetAxis(strAxisVer) <= 0f - deadZone)
			{
				flag = true;
			}
			break;
		case GamepadDirection.Right:
			if (Input.GetAxis(strAxisHor) >= deadZone)
			{
				flag = true;
			}
			break;
		}
		if (!checkTheState)
		{
			return flag;
		}
		GamepadDirectionalStateControl gamepadDirectionalStateControl = _listGPInputState.Find((GamepadDirectionalStateControl x) => x.direction == input && x.axisNameHor == strAxisHor && x.axisNameVer == strAxisVer);
		if (gamepadDirectionalStateControl == null)
		{
			return false;
		}
		switch (gamepadDirectionalStateControl.state)
		{
		case InputPressType.KeyPressedOnce:
		case InputPressType.KeyHolding:
			if (flag)
			{
				return true;
			}
			break;
		case InputPressType.KeyReleased:
			if (!flag)
			{
				return true;
			}
			break;
		}
		return false;
	}

	private void UpdateGamepadDirectionalStateControl()
	{
		if (_listGPInputState.Count <= 0)
		{
			GamepadInitiateStateList();
		}
		foreach (GamepadDirectionalStateControl item in _listGPInputState)
		{
			GamepadStateSwitch(item, ReadGamepadDirectionalInput(item.direction, item.axisNameHor, item.axisNameVer));
		}
	}

	private void GamepadStateSwitch(GamepadDirectionalStateControl currItem, bool readTrue)
	{
		switch (currItem.state)
		{
		case InputPressType.Null:
			if (readTrue)
			{
				currItem.state = InputPressType.KeyPressedOnce;
			}
			break;
		case InputPressType.KeyPressedOnce:
			if (readTrue)
			{
				currItem.state = InputPressType.KeyHolding;
			}
			else
			{
				currItem.state = InputPressType.KeyReleased;
			}
			break;
		case InputPressType.KeyHolding:
			if (!readTrue)
			{
				currItem.state = InputPressType.KeyReleased;
			}
			break;
		case InputPressType.KeyReleased:
			currItem.state = InputPressType.Null;
			break;
		}
	}

	private void GamepadInitiateStateList()
	{
		_listGPInputState.Clear();
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.W,
			axisNameVer = _axis_1_Ver,
			direction = GamepadDirection.Up,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.A,
			axisNameHor = _axis_1_Hor,
			direction = GamepadDirection.Left,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.S,
			axisNameVer = _axis_1_Ver,
			direction = GamepadDirection.Down,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.D,
			axisNameHor = _axis_1_Hor,
			direction = GamepadDirection.Right,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.T,
			axisNameVer = _axis_2_Ver,
			direction = GamepadDirection.Up,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.F,
			axisNameHor = _axis_2_Hor,
			direction = GamepadDirection.Left,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.G,
			axisNameVer = _axis_2_Ver,
			direction = GamepadDirection.Down,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.H,
			axisNameHor = _axis_2_Hor,
			direction = GamepadDirection.Right,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.I,
			axisNameVer = _axis_3_Ver,
			direction = GamepadDirection.Up,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.J,
			axisNameHor = _axis_3_Hor,
			direction = GamepadDirection.Left,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.K,
			axisNameVer = _axis_3_Ver,
			direction = GamepadDirection.Down,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.L,
			axisNameHor = _axis_3_Hor,
			direction = GamepadDirection.Right,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.Y,
			axisNameHor = _axis_4_Spc,
			direction = GamepadDirection.Left,
			state = InputPressType.Null
		});
		_listGPInputState.Add(new GamepadDirectionalStateControl
		{
			reservedKey = KeyCode.U,
			axisNameHor = _axis_4_Spc,
			direction = GamepadDirection.Right,
			state = InputPressType.Null
		});
	}

	private void INPUT_GPUpdateAxisNames(InputType thisGP)
	{
		if (thisGP == InputType.GPTypePSX || thisGP != InputType.GPTypeXBOX)
		{
			_axis_1_Ver = "Vertical-DPad";
			_axis_1_Hor = "Horizontal-DPad";
			_axis_2_Ver = "Vertical-StickLeft";
			_axis_2_Hor = "Horizontal-StickLeft";
			_axis_3_Ver = "Vertical-StickRight";
			_axis_3_Hor = "Horizontal-StickRight";
			_axis_4_Spc = "XBOX-Triggers";
		}
		else
		{
			_axis_1_Ver = "XBOX-Vertical-DPad";
			_axis_1_Hor = "XBOX-Horizontal-DPad";
			_axis_2_Ver = "XBOX-Vertical-StickLeft";
			_axis_2_Hor = "XBOX-Horizontal-StickLeft";
			_axis_3_Ver = "XBOX-Vertical-StickRight";
			_axis_3_Hor = "XBOX-Horizontal-StickRight";
			_axis_4_Spc = "XBOX-Triggers";
		}
		foreach (GamepadDirectionalStateControl item in _listGPInputState)
		{
			switch (item.reservedKey)
			{
			case KeyCode.S:
			case KeyCode.W:
				item.axisNameVer = _axis_1_Ver;
				break;
			case KeyCode.A:
			case KeyCode.D:
				item.axisNameHor = _axis_1_Hor;
				break;
			case KeyCode.G:
			case KeyCode.T:
				item.axisNameVer = _axis_2_Ver;
				break;
			case KeyCode.F:
			case KeyCode.H:
				item.axisNameHor = _axis_2_Hor;
				break;
			case KeyCode.I:
			case KeyCode.K:
				item.axisNameVer = _axis_3_Ver;
				break;
			case KeyCode.J:
			case KeyCode.L:
				item.axisNameHor = _axis_3_Hor;
				break;
			case KeyCode.U:
			case KeyCode.Y:
				item.axisNameHor = _axis_4_Spc;
				break;
			}
		}
	}

	public bool GetSubmitButtonUniversalPress(bool isHold = false)
	{
		if (!isHold && Input.GetKeyDown(IKey(InputLib.ConfirmUI)))
		{
			return true;
		}
		if (isHold && Input.GetKey(IKey(InputLib.ConfirmUI)))
		{
			return true;
		}
		return false;
	}

	public bool GetCancelButtonUniversalPress()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(IKey(InputLib.CancelUI)) || Input.GetMouseButtonDown(1))
		{
			return true;
		}
		return false;
	}

	public void SubmitSelectableClickedUniversal(bool hasHoldDelay = false, float timeHoldDelay = 0f)
	{
		Button button = null;
		button = EventSystem.current?.currentSelectedGameObject?.GetComponent<Button>();
		if (button != null && (!hasHoldDelay || (hasHoldDelay && crtUniversalClickDelay == null && timeHoldDelay > 0f && button.CompareTag("specialButton1"))))
		{
			button.onClick.Invoke();
			if (hasHoldDelay)
			{
				crtUniversalClickDelay = StartCoroutine(waitUniversalClickDelay(timeHoldDelay));
			}
		}
	}

	private void OnGUICheckSliders(Selectable currentSelectable, bool directionRight)
	{
		if ((bool)currentSelectable?.GetComponent<Slider>())
		{
			Slider component = currentSelectable.GetComponent<Slider>();
			float num = 0.1f;
			if (component.wholeNumbers)
			{
				num = 1f;
			}
			if (directionRight)
			{
				component.value += num;
			}
			else
			{
				component.value -= num;
			}
		}
	}

	private void OnGUICheckScrollBars(Selectable currentSelectable, bool directionUp)
	{
		if ((bool)currentSelectable?.GetComponent<Scrollbar>())
		{
			Scrollbar component = currentSelectable.GetComponent<Scrollbar>();
			float num = 0.1f;
			if (directionUp)
			{
				component.value += num;
			}
			else
			{
				component.value -= num;
			}
		}
	}

	private IEnumerator waitUniversalClickDelay(float wait)
	{
		yield return new WaitForSeconds(wait);
		crtUniversalClickDelay = null;
	}

	public void LoadNextScene(string strnextScene, Vector2 targetPos, Vector2 offsetCameraTP, bool forceMovePlayer, bool applyFade, Color fadeColor, float fadeTime, bool lockPlayerInput, bool persistPlayerGameSessionValues = false, bool cleanPreviousStageNameOnLoad = false)
	{
		if (CheckRefAudioSystem())
		{
			StopMusicTheme(doFadeOut: true, 0.5f);
		}
		_isSceneTransitioning = true;
		_sceneLoadAfterFade = strnextScene;
		_isPersistGameSessionValues = persistPlayerGameSessionValues;
		_isForbidAllPlayerInput = lockPlayerInput;
		_offsetCameraAfterTP = offsetCameraTP;
		_teleportTargetPosition = targetPos;
		_forceMovePlayerAfterTP = forceMovePlayer;
		if (cleanPreviousStageNameOnLoad)
		{
			_strPreviousSceneName = "savefile was loaded, previous stage name was cleaned!";
		}
		else
		{
			_strPreviousSceneName = strCurrentActiveSceneName;
		}
		_lastFadeTimeUsed = fadeTime;
		Input.ResetInputAxes();
		if (applyFade)
		{
			SceneFadeHandler(actionFadeToBlack: true, _lastFadeTimeUsed);
		}
		else
		{
			SceneManager.LoadScene(_sceneLoadAfterFade);
		}
	}

	public void SceneFadeHandler(bool actionFadeToBlack, float fTime)
	{
		if (actionFadeToBlack)
		{
			DoFadeInOutBlackScreen(actionFadeToBlack, fTime);
		}
		else
		{
			DoFadeInOutBlackScreen(FadeIntoBlack: false, _lastFadeTimeUsed);
		}
	}

	public IEnumerator LoadYourAsyncScene(string strnextScene, bool lockPlayerInput, bool persistPlayerGameSessionValues = false)
	{
		MKMDiag.Step("GC.LoadScene: " + strnextScene);
		if (!MKM_IsSceneInBuild(strnextScene))
		{
			Debug.LogError("GC_Error[LoadYourAsyncScene]: scene [" + strnextScene + "] is not in the build settings! Aborting load.");
			_isSceneTransitioning = false;
			yield break;
		}
		_isSceneTransitioning = true;
		_isPersistGameSessionValues = persistPlayerGameSessionValues;
		_isForbidAllPlayerInput = lockPlayerInput;
		_strPreviousSceneName = strCurrentActiveSceneName;
		MKMDiag.Step("GC.LoadScene: calling LoadSceneAsync");
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(strnextScene);
		while (!asyncLoad.isDone)
		{
			yield return null;
		}
		MKMDiag.Step("GC.LoadScene: async done, progress=" + asyncLoad.progress);
	}

	// The game addresses scenes by bare name ("MainMenu"); Unity's build settings store
	// full paths ("Assets/Scenes/MainMenu.unity"), so match on the file name too.
	private bool MKM_IsSceneInBuild(string sceneName)
	{
		if (string.IsNullOrEmpty(sceneName))
		{
			return false;
		}
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			string path = SceneUtility.GetScenePathByBuildIndex(i);
			if (string.IsNullOrEmpty(path))
			{
				continue;
			}
			if (path == sceneName || System.IO.Path.GetFileNameWithoutExtension(path) == sceneName)
			{
				return true;
			}
		}
		return false;
	}

	private bool CheckIfSceneToLoadIsValid(string sceneName)
	{
		if (SceneUtility.GetBuildIndexByScenePath(sceneName) >= 0)
		{
			return true;
		}
		return false;
	}

	public void FadeIn_SignalFinished()
	{
		if (_isSceneTransitioning && _sceneLoadAfterFade != "")
		{
			SceneManager.LoadScene(_sceneLoadAfterFade);
		}
	}

	public void StartStopAllCleanUpRountines(bool setON)
	{
		if (setON)
		{
			InvokeRepeating("CleanEffects", 5f, 5f);
			InvokeRepeating("CleanCollectibles", 6f, 5f);
			InvokeRepeating("CleanContainers", 7f, 5f);
			InvokeRepeating("CleanEnemies", 8f, 5f);
		}
		else
		{
			CancelInvoke("CleanEffects");
			CancelInvoke("CleanCollectibles");
			CancelInvoke("CleanContainers");
			CancelInvoke("CleanEnemies");
		}
	}

	private void CleanEnemies()
	{
		if (!mainEnemyParent)
		{
			mainEnemyParent = GameObject.Find("[ENEMY-BASIC]");
		}
		if (!mainEnemyParent)
		{
			return;
		}
		foreach (Transform item in mainEnemyParent.transform)
		{
			if (!item.gameObject.activeSelf)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	private void CleanEffects()
	{
		if (!CheckRefEffectsParent())
		{
			return;
		}
		foreach (Transform item in _mainEffectParent.transform)
		{
			if (!item.gameObject.activeSelf)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	private void CleanCollectibles()
	{
		if (!mainCollParent)
		{
			mainCollParent = GameObject.Find("[COLLECTIBLE]");
		}
		if (!mainCollParent)
		{
			return;
		}
		foreach (Transform item in mainCollParent.transform)
		{
			if (!item.gameObject.activeSelf)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	private void CleanContainers()
	{
		if (!mainContainerParent)
		{
			mainContainerParent = GameObject.Find("[CONTAINER]");
		}
		if (!mainContainerParent)
		{
			return;
		}
		foreach (Transform item in mainContainerParent.transform)
		{
			if (!item.gameObject.activeSelf)
			{
				UnityEngine.Object.Destroy(item.gameObject);
			}
		}
	}

	public void BeginDialogueLogic(NPCType npc, int idSpecificDialogue = 0, bool isZoomDialogue = false)
	{
		DialogueList dialogueList = new DialogueList();
		objAN_GenericArray = new Animator[5];
		_currentNPCType = npc;
		DialogueList dialogueList2 = JsonUtility.FromJson<DialogueList>(LoadJSONDialogueFile(npc));
		if (idSpecificDialogue > 0)
		{
			_currentNPCSpeechID = idSpecificDialogue;
		}
		DIAG_BeforeStart_CheckSpecialDialogueLogic(npc, ref _currentNPCSpeechID);
		foreach (DialogueLine item2 in dialogueList2.listOfDialogue)
		{
			if (item2.id == _currentNPCSpeechID)
			{
				dialogueList.listOfDialogue.Add(item2);
			}
		}
		if (dialogueList == null || dialogueList.listOfDialogue.Count <= 0)
		{
			Debug.Log("Could not load dialogue list, or the number of lines is zero.");
			return;
		}
		if (DIALOG_SpecialRepeatingCases(npc, _currentNPCSpeechID))
		{
			int index = MATH_RNG_INT(0, dialogueList.listOfDialogue.Count - 1);
			DialogueLine item = dialogueList.listOfDialogue[index];
			dialogueList.listOfDialogue.Clear();
			dialogueList.listOfDialogue.Add(item);
		}
		if (CheckRefCoreUI())
		{
			coreUI.StartDialogue(dialogueList, npc, isZoomDialogue);
		}
	}

	private string LoadJSONDialogueFile(NPCType npc)
	{
		string text = _strCurrentLanguageFolderPath;
		switch (npc)
		{
		case NPCType.NeneShop:
			_currentNPCSpeechID = coreGameData.npcFlags.currentNeneStoryDialogueID;
			if (WEBGL_CheckIfIsWebBuild())
			{
				return _jsonD_Nene;
			}
			text = Path.Combine(text, "Dialogue_Nene.json");
			break;
		case NPCType.BoardText:
		case NPCType.OBJ_Tutorial:
			if (WEBGL_CheckIfIsWebBuild())
			{
				return _jsonD_Board;
			}
			text = Path.Combine(text, "Dialogue_BoardSign.json");
			break;
		case NPCType.Various:
			if (WEBGL_CheckIfIsWebBuild())
			{
				return _jsonD_Various;
			}
			text = Path.Combine(text, "Dialogue_Various.json");
			break;
		case NPCType.FarinaSupport:
			_currentNPCSpeechID = coreGameData.npcFlags.currentFarinaStoryDialogueID;
			if (WEBGL_CheckIfIsWebBuild())
			{
				return _jsonD_Farina;
			}
			text = Path.Combine(text, "Dialogue_Farina.json");
			break;
		}
		return File.ReadAllText(text);
	}

	public void ForceFinishDialogue()
	{
		if (CheckRefCoreUI())
		{
			coreUI.FinishDialogue();
		}
	}

	public string DIALOG_GetTitleOfTalk(NPCType thisNPC, int idDiag)
	{
		DialogueList dialogueList = null;
		switch (thisNPC)
		{
		case NPCType.FarinaSupport:
			if (_diagMenuFarina == null)
			{
				_diagMenuFarina = JsonUtility.FromJson<DialogueList>(LoadJSONDialogueFile(NPCType.FarinaSupport));
			}
			dialogueList = _diagMenuFarina;
			break;
		case NPCType.NeneShop:
			if (_diagMenuNene == null)
			{
				_diagMenuNene = JsonUtility.FromJson<DialogueList>(LoadJSONDialogueFile(NPCType.NeneShop));
			}
			dialogueList = _diagMenuNene;
			break;
		}
		string result = "title.null";
		if (dialogueList != null && dialogueList.listOfDialogue.Exists((DialogueLine x) => x.id == idDiag))
		{
			result = dialogueList.listOfDialogue.Find((DialogueLine x) => x.id == idDiag)?.miscText;
		}
		return result;
	}

	public void SetNewPositionDialogueWindow(Vector3 newPosition, bool restoreDefault = false)
	{
		if (CheckRefCoreUI())
		{
			coreUI.SetDialogueWindowPosition(newPosition, restoreDefault);
		}
	}

	public void ManageDialogueAnimationsActions(List<AnimAction> listActions)
	{
		for (int i = 0; i < listActions.Count; i++)
		{
			switch (listActions[i].whoID)
			{
			case SpeakerID.Maya:
				SwitchMayaDiagAct(listActions[i].whichACT);
				break;
			case SpeakerID.Nene:
				SwitchAndPlayGenericDiagAct("Nene", isFindByTag: true, "NPC", listActions[i].whichACT);
				break;
			case SpeakerID.Farina:
				SwitchAndPlayGenericDiagAct("Farina-NPC", isFindByTag: true, "NPC", listActions[i].whichACT);
				break;
			case SpeakerID.ForestBoss:
				SwitchAndPlayGenericDiagAct("Forest-Boss", isFindByTag: true, "Enemy-Boss", listActions[i].whichACT);
				break;
			case SpeakerID.HillBoss:
				SwitchAndPlayGenericDiagAct("Hill-Boss", isFindByTag: true, "Enemy-Boss", listActions[i].whichACT);
				break;
			case SpeakerID.WerewolfBoss:
				SwitchAndPlayGenericDiagAct("BossWerewolf", isFindByTag: true, "Enemy-Boss", listActions[i].whichACT);
				break;
			case SpeakerID.MinotaurBoss:
				SwitchAndPlayGenericDiagAct("BossMINOTAUR", isFindByTag: true, "Enemy-Boss", listActions[i].whichACT);
				break;
			case SpeakerID.PrincessBoss:
				SwitchAndPlayGenericDiagAct("IcePrincess-BOSS", isFindByTag: true, "Enemy-Boss", listActions[i].whichACT);
				break;
			case SpeakerID.WitchBoss:
				SwitchAndPlayGenericDiagAct("Witch-BOSS", isFindByTag: true, "Enemy-Boss", listActions[i].whichACT);
				break;
			case SpeakerID.BlueFoxBoss:
				SwitchAndPlayGenericDiagAct("BossBlueFox", isFindByTag: true, "Enemy-Boss", listActions[i].whichACT);
				break;
			case SpeakerID.SwampBoss:
				SwitchAndPlayGenericDiagAct("BossSwamp", isFindByTag: true, "Enemy-Boss", listActions[i].whichACT);
				break;
			case SpeakerID.LivithFinal:
				SwitchAndPlayGenericDiagAct("NPC-Livith", isFindByTag: true, "NPC", listActions[i].whichACT);
				break;
			}
		}
	}

	public void FinishAllDialogueActionsVariables()
	{
		if (objAN_Maya != null)
		{
			SwitchMayaDiagAct(SpeakerACT.Null);
			objAN_Maya = null;
		}
		for (int i = 0; i < objAN_GenericArray.Length; i++)
		{
			if (objAN_GenericArray[i] != null)
			{
				objAN_GenericArray[i].SetBool("isDiagAction", value: false);
				objAN_GenericArray[i] = null;
			}
		}
		if (CheckRefPlayer())
		{
			_refPlayerScript.StartDelayOfInputAction(0.1f);
		}
	}

	private void SwitchAndPlayGenericDiagAct(string speakerName, bool isFindByTag, string tagToFind, SpeakerACT action)
	{
		Animator animator = ValidateAnimatorReferenceDiagAct(speakerName, isFindByTag, tagToFind);
		if (animator != null)
		{
			animator.SetBool("isDiagAction", value: true);
			switch (action)
			{
			default:
				animator.SetBool("isDiagAction", value: false);
				break;
			case SpeakerACT.Stand1:
				animator.Play("Diag-Stand1");
				break;
			case SpeakerACT.Talk1:
				animator.Play("Diag-Talk1");
				break;
			case SpeakerACT.Talk2:
				animator.Play("Diag-Talk2");
				break;
			case SpeakerACT.Read1:
				animator.Play("Diag-Read1");
				break;
			case SpeakerACT.Laugh1:
				animator.Play("Diag-Laugh1");
				break;
			case SpeakerACT.Angry1:
				animator.Play("Diag-Angry1");
				break;
			case SpeakerACT.Angry2:
				animator.Play("Diag-Angry2");
				break;
			case SpeakerACT.InvertFacing:
			{
				Transform transform = animator.gameObject.transform;
				if (transform != null)
				{
					Vector3 localScale = transform.localScale;
					localScale.x *= -1f;
					transform.localScale = localScale;
				}
				break;
			}
			case SpeakerACT.Stand2:
				animator.Play("Diag-Stand2");
				break;
			case SpeakerACT.Misc1:
				animator.Play("Diag-Misc1");
				break;
			case SpeakerACT.Enable:
				animator.gameObject.SetActive(value: true);
				break;
			case SpeakerACT.Disable:
				animator.gameObject.SetActive(value: false);
				break;
			case SpeakerACT.Visible:
			case SpeakerACT.Invisible:
			{
				SpriteRenderer component = animator.gameObject.GetComponent<SpriteRenderer>();
				Color color = component.color;
				if (action == SpeakerACT.Visible)
				{
					color.a = 1f;
				}
				else
				{
					color.a = 0f;
				}
				component.color = color;
				break;
			}
			}
		}
		else
		{
			Debug.Log("Could not get a reference to the ANIMATOR component, of the speaker named: " + speakerName + ". The requested diag action was: " + action);
		}
	}

	private Animator ValidateAnimatorReferenceDiagAct(string spkName, bool isFindByTag, string thisTag)
	{
		for (int i = 0; i < objAN_GenericArray.Length; i++)
		{
			if (objAN_GenericArray[i] == null)
			{
				if (isFindByTag)
				{
					GameObject[] array = GameObject.FindGameObjectsWithTag(thisTag);
					for (int j = 0; j < array.Length; j++)
					{
						if (array[j].name.Contains(spkName))
						{
							objAN_GenericArray[i] = array[j].gameObject.GetComponent<Animator>();
							break;
						}
					}
				}
				else
				{
					GameObject gameObject = GameObject.Find(spkName);
					if (gameObject != null)
					{
						objAN_GenericArray[i] = gameObject.GetComponent<Animator>();
						break;
					}
				}
			}
			if (objAN_GenericArray[i] != null && objAN_GenericArray[i].gameObject.name.Contains(spkName))
			{
				return objAN_GenericArray[i];
			}
		}
		return null;
	}

	public void SwitchMayaDiagAct(SpeakerACT action)
	{
		if (objAN_Maya == null)
		{
			GameObject gameObject = GameObject.FindGameObjectWithTag("Player");
			if (gameObject != null)
			{
				objAN_Maya = gameObject.GetComponent<Animator>();
			}
		}
		if (!(objAN_Maya != null))
		{
			return;
		}
		objAN_Maya.SetBool("isDiagAction", value: true);
		string text = "";
		switch (action)
		{
		default:
			objAN_Maya.SetBool("isDiagAction", value: false);
			break;
		case SpeakerACT.Talk1:
			text = "Player-DiagTalk1";
			break;
		case SpeakerACT.Read1:
			text = "Player-DiagRead1";
			break;
		case SpeakerACT.Angry1:
			text = "Player-DiagAngry1";
			break;
		case SpeakerACT.Hurt1:
			text = "Player-DiagHurt1";
			break;
		case SpeakerACT.InvertFacing:
			if (CheckRefPlayer())
			{
				_refPlayerScript.HACKY_INVERT_FLIP_FOR_DIALOGUE();
			}
			break;
		case SpeakerACT.Special1:
			text = "ev01-01";
			break;
		case SpeakerACT.Special2:
			text = "ev01-02";
			if (CheckRefPlayer())
			{
				InstantiateVFX(VisualEffect.GrabSmokes, _refPlayerScript.StartPointGroundCheck.position, "MayaDiag");
			}
			break;
		case SpeakerACT.Special3:
			text = "Player-DiagHugFarina";
			break;
		case SpeakerACT.Laugh1:
			text = "Player-WinDiagVers";
			break;
		case SpeakerACT.Talk3:
			text = "Player-DiagRuby1";
			break;
		case SpeakerACT.Talk4:
			text = "Player-DiagRuby2";
			break;
		}
		if (text != "")
		{
			objAN_Maya.Play(text);
		}
	}

	public void PlayEventSpecificAnimationDialogue(string actorName, bool isFindGameObjectByTag, string tagToFind, string animationToPlayName)
	{
		_eventAnimationIsFinished = false;
		objAN_GenericArray = new Animator[5];
		Animator animator = ValidateAnimatorReferenceDiagAct(actorName, isFindGameObjectByTag, tagToFind);
		if (animator != null)
		{
			animator.SetBool("isDiagAction", value: true);
			animator.Play(animationToPlayName);
		}
		else
		{
			Debug.Log("GAMECORE:PlayEventSpecificAnimationDialogue() -> Animator was NULL for actorName: " + actorName);
			_eventAnimationIsFinished = true;
			animator.SetBool("isDiagAction", value: false);
		}
	}

	public void EventSpecificAnimationDialoguehasCompleted()
	{
		_eventAnimationIsFinished = true;
	}

	public void PlayEventSpecificAnimationMaya(SpeakerACT mayaAnimationActionToPlay)
	{
		_eventAnimationIsFinished = false;
		SwitchMayaDiagAct(mayaAnimationActionToPlay);
	}

	public void ManageDialogueSoundsActions(List<SoundAction> listActions)
	{
		for (int i = 0; i < listActions.Count; i++)
		{
			switch (listActions[i].whoID)
			{
			case SpeakerID.Maya:
				ManageMayaDialogueSound(listActions[i].idSound);
				break;
			case SpeakerID.ForestBoss:
			case SpeakerID.WerewolfBoss:
			case SpeakerID.PrincessBoss:
			case SpeakerID.WitchBoss:
				PlaySFXGeneric(listActions[i].whoID.ToString(), listActions[i].idSound, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f, "voice");
				break;
			case SpeakerID.MusicPlayer:
				ManageDialogueMusic(listActions[i].idSound);
				break;
			}
		}
	}

	private void ManageMayaDialogueSound(int idSound)
	{
		switch (idSound)
		{
		case 8:
			PlaySFXMayaVoice(MayaVoice.Gasp8, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		case 9:
			PlaySFXMayaVoice(MayaVoice.MoanRough9, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			break;
		}
	}

	private void ManageDialogueMusic(int idMusicType)
	{
		if (idMusicType == 0 || idMusicType == 9)
		{
			StopMusicTheme(doFadeOut: false, 0f);
		}
		else
		{
			PlayMusicTheme((MusicTheme)idMusicType, doFadeIn: false, 0f, doLoop: true);
		}
	}

	public void PlayLetterSoundTone(SpeakerID thisTone)
	{
		SFXBasic1 whichSFX;
		float pitchRangeLow;
		float pitchRangeHigh;
		switch (thisTone)
		{
		default:
			whichSFX = SFXBasic1.DiagToneGroup10;
			pitchRangeLow = 0.7f;
			pitchRangeHigh = 0.8f;
			break;
		case SpeakerID.Maya:
			whichSFX = SFXBasic1.DiagToneGroup2;
			pitchRangeLow = 3.5f;
			pitchRangeHigh = 4f;
			break;
		case SpeakerID.Farina:
			whichSFX = SFXBasic1.DiagToneGroup3;
			pitchRangeLow = 3.5f;
			pitchRangeHigh = 4f;
			break;
		case SpeakerID.Nene:
			whichSFX = SFXBasic1.DiagToneGroup4;
			pitchRangeLow = 1f;
			pitchRangeHigh = 1.5f;
			break;
		case SpeakerID.ForestBoss:
			whichSFX = SFXBasic1.DiagToneGroup2;
			pitchRangeLow = 1.5f;
			pitchRangeHigh = 2f;
			break;
		case SpeakerID.HillBoss:
			whichSFX = SFXBasic1.DiagToneGroup1;
			pitchRangeLow = 2.75f;
			pitchRangeHigh = 3.25f;
			break;
		case SpeakerID.MinotaurBoss:
			whichSFX = SFXBasic1.DiagToneGroup1;
			pitchRangeLow = 0.5f;
			pitchRangeHigh = 1f;
			break;
		case SpeakerID.WerewolfBoss:
			whichSFX = SFXBasic1.DiagToneGroup5;
			pitchRangeLow = 0.75f;
			pitchRangeHigh = 1.25f;
			break;
		case SpeakerID.PrincessBoss:
			whichSFX = SFXBasic1.DiagToneGroup1;
			pitchRangeLow = 3.5f;
			pitchRangeHigh = 4f;
			break;
		case SpeakerID.WitchBoss:
			whichSFX = SFXBasic1.DiagToneGroup6;
			pitchRangeLow = 0.5f;
			pitchRangeHigh = 1f;
			break;
		case SpeakerID.BlueFoxBoss:
			whichSFX = SFXBasic1.DiagToneGroup6;
			pitchRangeLow = 1.5f;
			pitchRangeHigh = 2f;
			break;
		case SpeakerID.SwampBoss:
			whichSFX = SFXBasic1.DiagToneGroup8;
			pitchRangeLow = 1.25f;
			pitchRangeHigh = 1.75f;
			break;
		case SpeakerID.BEDoggoBoy1:
			whichSFX = SFXBasic1.DiagToneGroup9;
			pitchRangeLow = 0.75f;
			pitchRangeHigh = 1.25f;
			break;
		case SpeakerID.BEDoggoBoy2:
			whichSFX = SFXBasic1.DiagToneGroup9;
			pitchRangeLow = 1.25f;
			pitchRangeHigh = 1.75f;
			break;
		case SpeakerID.Chronicles:
			whichSFX = SFXBasic1.DiagToneGroup10;
			pitchRangeLow = 0.7f;
			pitchRangeHigh = 0.8f;
			break;
		}
		PlayDiagLetters(whichSFX, 50, checkForDuplicity: false, Vector2.zero, pitchRangeLow, pitchRangeHigh, "DiagLetter", applyMuffledEffect: false, 0f, dontPlayIfGroupAlreadyPlaying: true);
	}

	public string ParseDialogueLineTags(string lineRaw)
	{
		if (string.IsNullOrEmpty(lineRaw))
		{
			return "";
		}
		string text = "";
		if (lineRaw.Contains("<"))
		{
			string[] array = lineRaw.Split('<', '>');
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].StartsWith("@:"))
				{
					array[i] = ReturnParsedTag(array[i].Substring(2));
				}
				text += array[i];
			}
		}
		if (text != "")
		{
			lineRaw = text;
		}
		return lineRaw;
	}

	private string ReturnParsedTag(string thisTag)
	{
		switch (thisTag)
		{
		case "KEY_INTERACT":
			thisTag = UniversalParseTagInputs(IKey(InputLib.Interact).ToString());
			break;
		case "KEY_JUMP":
			thisTag = UniversalParseTagInputs(IKey(InputLib.Jump).ToString());
			break;
		case "KEY_ATTACK":
			thisTag = UniversalParseTagInputs(IKey(InputLib.BasicAttack).ToString());
			break;
		case "KEY_UP":
			thisTag = UniversalParseTagInputs(IKey(InputLib.Up).ToString());
			break;
		case "KEY_DOWN":
			thisTag = UniversalParseTagInputs(IKey(InputLib.Down).ToString());
			break;
		case "KEY_LEFT":
			thisTag = UniversalParseTagInputs(IKey(InputLib.Left).ToString());
			break;
		case "KEY_RIGHT":
			thisTag = UniversalParseTagInputs(IKey(InputLib.Right).ToString());
			break;
		case "KEY_CAMERA":
			thisTag = UniversalParseTagInputs(IKey(InputLib.Camera).ToString());
			break;
		case "KEY_TOGGLE":
			thisTag = UniversalParseTagInputs(IKey(InputLib.ToggleInternalView).ToString());
			break;
		case "KEY_DEFSKILL":
			thisTag = UniversalParseTagInputs(IKey(InputLib.DefenseSkills).ToString());
			break;
		case "KEY_ATKSKILL":
			thisTag = UniversalParseTagInputs(IKey(InputLib.AttackSkills).ToString());
			break;
		case "KEY_CONFIRM":
			thisTag = UniversalParseTagInputs(IKey(InputLib.ConfirmUI).ToString());
			break;
		case "KEY_CANCEL":
			thisTag = UniversalParseTagInputs(IKey(InputLib.CancelUI).ToString());
			break;
		case "KEY_PAUSE":
			thisTag = UniversalParseTagInputs(IKey(InputLib.Pause).ToString());
			break;
		case "KEY_PREVITEM":
			thisTag = UniversalParseTagInputs(IKey(InputLib.PreviousItem).ToString());
			break;
		case "KEY_NEXTITEM":
			thisTag = UniversalParseTagInputs(IKey(InputLib.NextItem).ToString());
			break;
		case "KEY_USEITEM":
			thisTag = UniversalParseTagInputs(IKey(InputLib.UseItemActive).ToString());
			break;
		case "LINE_BREAK":
			thisTag = "\r\n";
			break;
		case "LB2":
			thisTag = "\r\n\r\n";
			break;
		case "STAT_ESSENCECHANCE":
			thisTag = _chanceEnemySpawnEssence.ToString();
			break;
		case "STAT_RANGEATTRACT":
			thisTag = MATH_RoundFloatDecimals(_rangeOfOrbAttraction, 1).ToString();
			break;
		}
		if (thisTag.Length > 8 && thisTag.Substring(0, 8) == "ITEM_ID_")
		{
			thisTag = GetItemNameFromList(int.Parse(thisTag.Substring(8, 3)));
		}
		if (thisTag.Length > 14 && thisTag.Substring(0, 14) == "ENEMY_NAME_ID_")
		{
			thisTag = GetTranslatedStringUI(int.Parse(thisTag.Substring(14, 4)));
		}
		return thisTag;
	}

	public string UniversalParseTagInputs(string strKey)
	{
		if (isGamepadCfgInput && CheckRefMenuBehaviour())
		{
			strKey = GetTranslatedStringUI(_refMenuScript.listGPIcon.GetButtonTLID(strKey, configInputType));
		}
		return "[" + strKey + "]";
	}

	private void DIAG_BeforeStart_CheckSpecialDialogueLogic(NPCType thisNPC, ref int thisDiagID)
	{
		if (thisNPC == NPCType.Various)
		{
			DIAG_SpecialLogic_TypeVarious(ref thisDiagID);
		}
	}

	private void DIAG_SpecialLogic_TypeVarious(ref int thisDiagID)
	{
		switch (thisDiagID)
		{
		case 5:
			if (CheckPlayerHasItem(102))
			{
				thisDiagID = 8;
			}
			break;
		case 6:
			if (CheckPlayerHasItem(101))
			{
				thisDiagID = 7;
			}
			break;
		case 98:
			if (DIAG_EVT_UnlockSideGatesLogicSceneName(0))
			{
				thisDiagID = 99;
			}
			break;
		case 9:
			if (flagsMain.isWitchBossDefeated)
			{
				thisDiagID = 10;
			}
			break;
		}
	}

	public void DIAG_DoSpecialActionsAndTriggers(int actionID)
	{
		Vector2 zero = Vector2.zero;
		switch (actionID)
		{
		case 1:
			zero = GameObject.Find("Sprite-Door03").transform.position;
			flagsMain.hasUsedCastleSilverKey = true;
			InstantiateVFX(VisualEffect.UnlockGate, zero, "[GAMECORE:DIAG_DoSpecialActionsAndTriggers()]");
			PlaySFX(SFXBasic1.UnlockGate, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			EVENT_RunLocalEventOnTheSceneWithThisName("EventSilverKey");
			StoreRetryGameCore(0f, 0f, "GameCore-DIAGDoSpecialActionsTriggers", ignoreCameraOffset: false, maintainRetryPlayerPosition: true);
			break;
		case 2:
			zero = GameObject.Find("Sprite-Door02").transform.position;
			flagsMain.hasUsedCastleDungeonKey = true;
			InstantiateVFX(VisualEffect.UnlockGate, zero, "[GAMECORE:DIAG_DoSpecialActionsAndTriggers()]");
			PlaySFX(SFXBasic1.UnlockGate, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			EVENT_RunLocalEventOnTheSceneWithThisName("EventDungeonKey");
			StoreRetryGameCore(0f, 0f, "GameCore-DIAGDoSpecialActionsTriggers", ignoreCameraOffset: false, maintainRetryPlayerPosition: true);
			break;
		case 3:
			zero = GameObject.Find("ShipGateway-Closed").transform.position;
			PlaySFX(SFXBasic1.UnlockGate, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			EVENT_RunLocalEventOnTheSceneWithThisName("EventShipDoor");
			StoreRetryGameCore(0f, 0f, "GameCore-DIAGDoSpecialActionsTriggers", ignoreCameraOffset: false, maintainRetryPlayerPosition: true);
			break;
		case 99:
			zero = GameObject.Find("Mech-LockedSideGates").transform.position;
			DIAG_EVT_UnlockSideGatesLogicSceneName(1);
			InstantiateVFX(VisualEffect.UnlockGate, zero, "[GAMECORE:DIAG_DoSpecialActionsAndTriggers()]");
			PlaySFX(SFXBasic1.UnlockGate, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
			EVENT_RunLocalEventOnTheSceneWithThisName("EventOpenGenericSideGate");
			StoreRetryGameCore(0f, 0f, "GameCore-DIAGDoSpecialActionsTriggers", ignoreCameraOffset: false, maintainRetryPlayerPosition: true);
			break;
		}
	}

	public bool DIAG_EVT_UnlockSideGatesLogicSceneName(int idRequest)
	{
		if (strCurrentActiveSceneName == "Stage1-Forest-Boss")
		{
			return DIAG_EVT_UnlockSideGatesAUX(idRequest, 103, ref flagsMain.hasUsedHillValleyKey);
		}
		return false;
	}

	private bool DIAG_EVT_UnlockSideGatesAUX(int typeRequest, int keyITEMID, ref bool flag)
	{
		switch (typeRequest)
		{
		case 0:
			if (CheckPlayerHasItem(keyITEMID))
			{
				return true;
			}
			break;
		case 1:
			flag = true;
			return true;
		case 2:
			if (flag)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void DisplayTutorialMessage(bool setActive, int idLine)
	{
		string text = "tutorial.placeholder";
		text = JsonUtility.FromJson<DialogueList>(LoadJSONDialogueFile(NPCType.OBJ_Tutorial)).listOfDialogue.Find((DialogueLine x) => x.id == idLine)?.speakerDialogue;
		text = ParseDialogueLineTags(text);
		if (CheckRefCoreUI())
		{
			coreUI.TutorialToggleBox(setActive, text.Trim());
		}
	}

	public void HandleNextDialogueID(NPCType thisNPC)
	{
		switch (thisNPC)
		{
		case NPCType.NeneShop:
			if (npcCurrentNeneDialogueID == 1)
			{
				npcCurrentNeneDialogueID = 90001;
			}
			break;
		case NPCType.FarinaSupport:
			if (npcCurrentFarinaDialogueID == 1)
			{
				npcCurrentFarinaDialogueID = 99999;
			}
			break;
		}
	}

	private bool DIALOG_SpecialRepeatingCases(NPCType thisNPC, int idDialogue = 0)
	{
		switch (thisNPC)
		{
		case NPCType.FarinaSupport:
			if (idDialogue == 900 || idDialogue == 99999)
			{
				return true;
			}
			break;
		case NPCType.NeneShop:
			if ((uint)(idDialogue - 90001) <= 1u)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public bool SaveGameData(int slotID)
	{
		try
		{
			_savingPlayerData = true;
			canPlayerCanUpdateGlobalVariables = false;
			coreGameData.strPlayerSaveScene = strCurrentActiveSceneName;
			coreGameData.vecPlayerSavePosition = new Vector3(_playerCurrentWorldXPosition, _playerCurrentWorldYPosition, 0f);
			coreGameData.strDateTime = DateTime.Now.ToString("O");
			coreGameData.idSavePointImage = _idCurrentSavePointScreenshot;
			coreGameData.versionName = _strGameVersion;
			coreGameData.fltGameVersion = _fltGameVersion;
			if (DataDirectoryCheck() && ValidateDataValues(coreGameData) && slotID >= 0 && slotID <= 9)
			{
				string text = "MKM_savefile";
				text = text + slotID + ".json";
				string strText = JsonUtility.ToJson(coreGameData);
				strText = BasicXorEncryptorDecryptor(strText);
				File.WriteAllText(Path.Combine(MKMSaveDir(), text), strText);
				ValidateSaveFilesIntoSlots();
				StoreRetryGameCore(_playerCurrentWorldXPosition, _playerCurrentWorldYPosition, "GameCore-SaveGameData");
				_offsetCameraAfterTP = Vector2.zero;
				canPlayerCanUpdateGlobalVariables = true;
				return true;
			}
			Debug.Log("GC_Error[SaveGameData]: Returned FALSE on the main IF statement!");
			canPlayerCanUpdateGlobalVariables = true;
			return false;
		}
		catch (Exception ex)
		{
			Debug.Log("GC_Error[SaveGameData]: " + ex.Message);
			_savingPlayerData = false;
			canPlayerCanUpdateGlobalVariables = true;
			return false;
		}
	}

	public bool LoadGameData(int slotID, bool doSceneLoadAfterDataLoad, bool isRetry = false, bool isTitleScreenGallery = false)
	{
		try
		{
			GamePlayerData loadedFile = new GamePlayerData();
			if (DataDirectoryCheck() && coreGameData != null && slotID >= 0 && slotID <= 9 && !isRetry)
			{
				string text = "MKM_savefile";
				text = text + slotID + ".json";
				if (File.Exists(Path.Combine(MKMSaveDir(), text)))
				{
					string strText = File.ReadAllText(Path.Combine(MKMSaveDir(), text));
					strText = BasicXorEncryptorDecryptor(strText);
					loadedFile = JsonUtility.FromJson<GamePlayerData>(strText);
				}
			}
			if (isRetry && retryCoreGameData != null)
			{
				loadedFile = retryCoreGameData;
			}
			if (doSceneLoadAfterDataLoad)
			{
				HACK_TEMP_TO_FIX_STAGE_NAME_CHANGING(ref loadedFile.strPlayerSaveScene);
				HACK_AttemptToFixOutdatedSaveFiles(ref loadedFile);
				if (!CheckIfSceneToLoadIsValid(loadedFile.strPlayerSaveScene))
				{
					Debug.Log("GC_Error[LoadGameData]: Tried to load scene [" + loadedFile.strPlayerSaveScene + "], but it is not valid or was not found in the list!");
					return false;
				}
			}
			coreGameData = loadedFile;
			if (isTitleScreenGallery && !coreGameData.gameFlags.HasVisitedTheGallery && !configMasterIsGalleryEnabled)
			{
				return false;
			}
			return true;
		}
		catch (Exception ex)
		{
			Debug.Log("GC_Error[LoadGameData]: " + ex.Message);
			return false;
		}
	}

	public bool TrySetupGameLoad(bool getLatestSave, int slotID, MenuScene senderScene, bool isRetry = false, float customFadeTime = 0.5f, bool isTitleScreenGallery = false)
	{
		try
		{
			canPlayerCanUpdateGlobalVariables = false;
			if (getLatestSave)
			{
				DateTime t = new DateTime(2000, 1, 1);
				DateTime dateTime = new DateTime(2000, 1, 1);
				int num = -1;
				for (int i = 0; i < _arraySlotsOfSaveLoad.Length; i++)
				{
					if (_arraySlotsOfSaveLoad[i] != null)
					{
						dateTime = ReturnParsedDateTime(_arraySlotsOfSaveLoad[i].textDateTimeSaved);
						if (DateTime.Compare(dateTime, t) > 0)
						{
							t = dateTime;
							num = i;
						}
					}
				}
				if (num >= 0 && num <= 9)
				{
					slotID = num;
				}
				else
				{
					Debug.Log("GC_Error[TrySetupGameLoad]: Tried to get a saveFile and slotID based on most recent dateTime, but it failed!");
				}
			}
			if (LoadGameData(slotID, doSceneLoadAfterDataLoad: true, isRetry, isTitleScreenGallery))
			{
				Vector2 zero = Vector2.zero;
				bool forceMovePlayer = false;
				ResetGameCoreGlobals();
				if (isRetry)
				{
					_selectedRetry = true;
					zero = _offsetCameraAfterTP;
				}
				else
				{
					MainGameDataInitialize(RenewAll: false);
				}
				if (zero != Vector2.zero)
				{
					forceMovePlayer = true;
				}
				playerIsInvincible = true;
				isPlayerInteractingLocked = true;
				isForbidAllPlayerInput = true;
				playerIsAllowedToBeGrabbed = false;
				StopAllSFXCurrentylPlaying();
				if (senderScene == MenuScene.InGame && CheckRefCoreUI())
				{
					if (Time.timeScale == 0f)
					{
						TogglePauseForGameMenu();
					}
					coreUI.DeactivateCanvasDuringLoad();
				}
				if (isTitleScreenGallery)
				{
					playerSaveScene = "AnimGallery";
					playerSavePosition = new Vector3(0f, 13.5f, 0f);
				}
				LoadNextScene(playerSaveScene, playerSavePosition, zero, forceMovePlayer, applyFade: true, Color.black, customFadeTime, lockPlayerInput: true, persistPlayerGameSessionValues: true, cleanPreviousStageNameOnLoad: true);
				return true;
			}
			canPlayerCanUpdateGlobalVariables = true;
			return false;
		}
		catch (Exception ex)
		{
			canPlayerCanUpdateGlobalVariables = true;
			Debug.Log("GC_Error[TrySetupGameLoad]: " + ex.Message);
			return false;
		}
	}

	public void SaveGameConfiguration()
	{
		if (DataDirectoryCheck() && coreConfigData != null)
		{
			string strText = JsonUtility.ToJson(coreConfigData);
			strText = BasicXorEncryptorDecryptor(strText);
			File.WriteAllText(Path.Combine(MKMSaveDir(), "MKM_configfile.json"), strText);
		}
		else
		{
			Debug.Log("Error: Could not save config data!");
		}
	}

	public void LoadGameConfiguration()
	{
		if (DataDirectoryCheck() && coreConfigData != null && File.Exists(Path.Combine(MKMSaveDir(), "MKM_configfile.json")))
		{
			string strText = File.ReadAllText(Path.Combine(MKMSaveDir(), "MKM_configfile.json"));
			strText = BasicXorEncryptorDecryptor(strText);
			coreConfigData = JsonUtility.FromJson<GameConfigData>(strText);
			ValidateInputList();
		}
		else
		{
			Debug.Log("Error: Could not load config data!");
		}
	}

	private void HACK_AttemptToFixOutdatedSaveFiles(ref GamePlayerData loadedFile)
	{
		if (loadedFile.fltGameVersion < 0.9f)
		{
			HACK_SAVEFILE_FOR_OLD_VERSIONS(ref loadedFile);
		}
	}

	private void HACK_TEMP_TO_FIX_STAGE_NAME_CHANGING(ref string savedStageName)
	{
		if (savedStageName == "Stage6-Swamp1")
		{
			savedStageName = "Stage6-1-Swamp";
		}
	}

	private void HACK_SAVEFILE_FOR_OLD_VERSIONS(ref GamePlayerData outdatedFile)
	{
		if (outdatedFile.arrayBeltSlots.Length == 4)
		{
			CountItemsQuantity[] arrayBeltSlots = new CountItemsQuantity[8];
			outdatedFile.arrayBeltSlots = arrayBeltSlots;
		}
		List<ItemInfo> list = new List<ItemInfo>();
		ItemInfo itemInfo = null;
		for (int i = 0; i < outdatedFile.listOfItemInventory.Count; i++)
		{
			itemInfo = outdatedFile.listOfItemInventory[i];
			switch (itemInfo.id)
			{
			case 1:
			case 2:
				itemInfo.id = 3;
				break;
			case 4:
			case 5:
				itemInfo.id = 6;
				break;
			}
			list.Add(itemInfo);
		}
		outdatedFile.listOfItemInventory = list;
		if (!outdatedFile.listOfItemInventory.Exists((ItemInfo x) => x.id == 900))
		{
			Vector2 position = new Vector2(9999f, 9999f);
			ItemInfo itemInfo2 = null;
			int i2;
			for (i2 = 900; i2 < 904; i2++)
			{
				itemInfo2 = clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == i2);
				outdatedFile.listOfItemInventory.Add(new ItemInfo
				{
					id = i2,
					name = itemInfo2.name,
					equipable = itemInfo2.equipable,
					description = itemInfo2.description,
					position = position,
					scene = "Stage2-Beach",
					type = itemInfo2.type
				});
			}
			for (int num = 0; num < 3; num++)
			{
				itemInfo2 = clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == 904);
				outdatedFile.listOfItemInventory.Add(new ItemInfo
				{
					id = 904,
					name = itemInfo2.name,
					equipable = itemInfo2.equipable,
					description = itemInfo2.description,
					position = position,
					scene = "Stage2-Beach",
					type = itemInfo2.type
				});
				itemInfo2 = clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == 905);
				outdatedFile.listOfItemInventory.Add(new ItemInfo
				{
					id = 905,
					name = itemInfo2.name,
					equipable = itemInfo2.equipable,
					description = itemInfo2.description,
					position = position,
					scene = "Stage2-Beach",
					type = itemInfo2.type
				});
			}
			for (int num2 = 0; num2 < 2; num2++)
			{
				itemInfo2 = clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == 906);
				outdatedFile.listOfItemInventory.Add(new ItemInfo
				{
					id = 906,
					name = itemInfo2.name,
					equipable = itemInfo2.equipable,
					description = itemInfo2.description,
					position = position,
					scene = "Stage2-Beach",
					type = itemInfo2.type
				});
			}
			itemInfo2 = clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == 907);
			outdatedFile.listOfItemInventory.Add(new ItemInfo
			{
				id = 907,
				name = itemInfo2.name,
				equipable = itemInfo2.equipable,
				description = itemInfo2.description,
				position = position,
				scene = "Stage2-Beach",
				type = itemInfo2.type
			});
		}
		if (outdatedFile.fltGameVersion < 0.605f)
		{
			outdatedFile.intPlayerCurrentDressStateValidUnbroken = outdatedFile.intPlayerCurrentDressState;
		}
		if (!outdatedFile.listOfItemInventory.Exists((ItemInfo x) => x.id == 908))
		{
			Vector2 position2 = new Vector2(9999f, 9999f);
			ItemInfo itemInfo3 = null;
			int i3 = 908;
			itemInfo3 = clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == i3);
			outdatedFile.listOfItemInventory.Add(new ItemInfo
			{
				id = i3,
				name = itemInfo3.name,
				equipable = itemInfo3.equipable,
				description = itemInfo3.description,
				position = position2,
				scene = "Stage2-Beach",
				type = itemInfo3.type
			});
		}
		outdatedFile.gameFlags.seenEventBeach01 = true;
	}

	public void MainGameDataInitialize(bool RenewAll)
	{
		if (RenewAll)
		{
			coreGameData = new GamePlayerData();
			if (coreConfigData == null)
			{
				coreConfigData = new GameConfigData();
				SetInputDefaultValues(onlyResetCurrentConfigType: false);
				if (!File.Exists(Path.Combine(MKMSaveDir(), "MKM_configfile.json")))
				{
					SaveGameConfiguration();
				}
			}
		}
		retryCoreGameData = new GamePlayerData();
		_sessionFlags = new GamePlayerFlagsTemporarySession();
		_arraySlotsOfSaveLoad = new SaveLoadSlotEntry[10];
		SetupDefaultValuesOfCommonEroPhase();
		ValidateSaveFilesIntoSlots();
	}

	
	private static string MKMSaveDir()
	{
		string dir = System.IO.Path.Combine(Application.persistentDataPath, "UserData");
		if (!System.IO.Directory.Exists(dir))
		{
			System.IO.Directory.CreateDirectory(dir);
		}
		return dir;
	}

	private bool DataDirectoryCheck()
	{
		if (!Directory.Exists(MKMSaveDir()))
		{
			Directory.CreateDirectory(MKMSaveDir());
		}
		if (Directory.Exists(MKMSaveDir()))
		{
			return true;
		}
		return false;
	}

	private bool ValidateDataValues(GamePlayerData data)
	{
		if (_isPlayerFainted || _playerIsBeingGrabed)
		{
			Debug.Log("GameCore tried to make a corrupt save file, while Player is fainted and/or being ero grabbed!!! Ignoring this save attempt...");
			return false;
		}
		if (data == null)
		{
			Debug.Log("GameCore tried to make a corrupt save file, with NULL player data!!! Ignoring this save attempt...");
			return false;
		}
		if (data.intPlayerCurrentHealth <= 0f)
		{
			Debug.Log("GameCore tried to make a corrupt save file (zero health)!!! Ignoring this save attempt...");
			return false;
		}
		return true;
	}

	public void ValidateSaveFilesIntoSlots()
	{
		GamePlayerData gamePlayerData = null;
		string text = "";
		if (DataDirectoryCheck())
		{
			for (int i = 0; i < _arraySlotsOfSaveLoad.Length; i++)
			{
				text = "MKM_savefile";
				text = text + i + ".json";
				if (File.Exists(Path.Combine(MKMSaveDir(), text)))
				{
					string strText = File.ReadAllText(Path.Combine(MKMSaveDir(), text));
					strText = BasicXorEncryptorDecryptor(strText);
					gamePlayerData = JsonUtility.FromJson<GamePlayerData>(strText);
					_arraySlotsOfSaveLoad[i] = new SaveLoadSlotEntry
					{
						slotID = i,
						textSceneNameSaved = gamePlayerData.strPlayerSaveScene,
						textDateTimeSaved = gamePlayerData.strDateTime,
						idSaveScreenshot = gamePlayerData.idSavePointImage
					};
				}
				else
				{
					_arraySlotsOfSaveLoad[i] = null;
				}
			}
		}
		else
		{
			Debug.Log("GAMECORE Error: Could not validate save files into array of slots!");
		}
	}

	public void StoreRetryGameCore(float playerPosX, float playerPosY, string nameScriptRequestFrom, bool ignoreCameraOffset = false, bool maintainRetryPlayerPosition = false)
	{
		if (retryCoreGameData != null && ValidateDataValues(coreGameData))
		{
			Vector3 vecPlayerSavePosition = new Vector3(playerPosX, playerPosY, 0f);
			if (_canPlayerUpdateGlobals)
			{
				_canPlayerUpdateGlobals = false;
			}
			if (maintainRetryPlayerPosition)
			{
				vecPlayerSavePosition = retryCoreGameData.vecPlayerSavePosition;
			}
			retryCoreGameData = coreGameData.DeepCopy();
			retryCoreGameData.strPlayerSaveScene = strCurrentActiveSceneName;
			retryCoreGameData.vecPlayerSavePosition = vecPlayerSavePosition;
			if (ignoreCameraOffset)
			{
				_offsetCameraAfterTP = Vector2.zero;
			}
			if (!isPlayerFainted)
			{
				_canPlayerUpdateGlobals = true;
			}
		}
		else
		{
			Debug.Log("Failed to validate gameData while attempting to save a retry point!");
		}
	}

	public void InjectPlayerPositionOnRetryStored(float posX, float posY)
	{
		if (retryCoreGameData != null)
		{
			retryCoreGameData.vecPlayerSavePosition = new Vector3(posX, posY, 0f);
		}
	}

	public void TestGenerateJSON()
	{
	}

	public void ResetGameCoreGlobals()
	{
		_isPlayerInteractingWithNPC = false;
		_isPlayerFainted = false;
		_isGamePaused = false;
		_isForbidAllPlayerInput = false;
		_isDelayCancelKeyPress = false;
		_savingPlayerData = false;
		_delayAfterkeyBound = false;
		_currentEroZoomLevel = 0;
		_countGauntletKills = 0;
		_countGauntletGoal = 0;
		_playerIsInvincible = false;
		_playerDidTeleport = false;
		_playerCanUseTeleport = true;
		_playerIsAllowedToBeGrabbed = true;
		_playerIsHurt = false;
		_isPlayerGoneForGood = false;
		_playerIsBeingGrabed = false;
		_eventIsPlaying = false;
		_eventStartBossFight = false;
		_canPlayerSaveGame = false;
		_hasPlayedMayaDeathScream = false;
		_playerEffectList.Clear();
		_accBehavior.Clear();
		_accModifiers = new TotalAccessoryEffect();
		_listOfNoRepeatVoice.Clear();
		_dicRNGint.Clear();
		_lastVoicePlayed = null;
		_isBossRematch = false;
		_enemyGhostFutaSkillCasting = null;
		_cameraOffsetMovingForLever = false;
		_coreUI_IsDisplayingDialogue = false;
		_isDisplayingNPCMenuTalkDialogue = false;
		_isDisplayingSpecialMenuNPC = false;
		_updateShopListAfterPurchase = false;
		_isBadEnd = false;
		_muteMusic = false;
		_muteSFX = false;
		_muteVoice = false;
		ESSENCE_SetCurrentChance();
		INPUT_GPUpdateAxisNames(configInputType);
		ReLoadShopStockComparedToPurchaseLog();
	}

	private void SetupDefaultValuesOfCommonEroPhase()
	{
		if (_defaultEroPhases == null)
		{
			_defaultEroPhases = new ENM_EroPhaseControl[5]
			{
				new ENM_EroPhaseControl
				{
					runLoops = 10,
					speedAnimation = 0.65f,
					divisionOfDmg = 14,
					frequencyOfDmg = 3
				},
				new ENM_EroPhaseControl
				{
					runLoops = 16,
					speedAnimation = 0.6f,
					divisionOfDmg = 12,
					frequencyOfDmg = 4
				},
				new ENM_EroPhaseControl
				{
					runLoops = 20,
					speedAnimation = 0.9f,
					divisionOfDmg = 11,
					frequencyOfDmg = 4
				},
				new ENM_EroPhaseControl
				{
					runLoops = 24,
					speedAnimation = 1.2f,
					divisionOfDmg = 10,
					frequencyOfDmg = 4
				},
				new ENM_EroPhaseControl
				{
					runLoops = 12,
					speedAnimation = 0.5f,
					divisionOfDmg = 9,
					frequencyOfDmg = 4
				}
			};
		}
	}

	public float GetDressStatValue(DressStatValue thisElement, float returnValue = 1f)
	{
		switch (thisElement)
		{
		case DressStatValue.Attack:
			switch (playerCurrentDressState)
			{
			case 0:
				returnValue = MATH_ResultValueOfTotalPercentile(100f, returnValue);
				break;
			case 1:
				returnValue = MATH_ResultValueOfTotalPercentile(90f, returnValue);
				break;
			case 2:
				returnValue = MATH_ResultValueOfTotalPercentile(80f, returnValue);
				break;
			}
			break;
		case DressStatValue.Defense:
			switch (playerCurrentDressState)
			{
			case 0:
				returnValue = MATH_ResultValueOfTotalPercentile(100f, returnValue);
				break;
			case 1:
				returnValue = MATH_ResultValueOfTotalPercentile(75f, returnValue);
				break;
			case 2:
				returnValue = MATH_ResultValueOfTotalPercentile(50f, returnValue);
				break;
			}
			break;
		case DressStatValue.Movement:
			switch (playerCurrentDressState)
			{
			case 0:
				returnValue = MATH_ResultValueOfTotalPercentile(100f, returnValue);
				break;
			case 1:
				returnValue = MATH_ResultValueOfTotalPercentile(105f, returnValue);
				break;
			case 2:
				returnValue = MATH_ResultValueOfTotalPercentile(110f, returnValue);
				break;
			}
			break;
		case DressStatValue.EroGather:
			switch (playerCurrentDressState)
			{
			case 0:
				returnValue = MATH_ResultValueOfTotalPercentile(25f, returnValue) + 1f;
				break;
			case 1:
				returnValue = MATH_ResultValueOfTotalPercentile(50f, returnValue) + 1f;
				break;
			case 2:
				returnValue = MATH_ResultValueOfTotalPercentile(100f, returnValue);
				break;
			}
			break;
		}
		return returnValue;
	}

	public void SetInputDefaultValues(bool onlyResetCurrentConfigType)
	{
		INPUTS_DefaultKeyboardList(onlyResetCurrentConfigType);
		INPUTS_DefaultGamepadType1List(onlyResetCurrentConfigType);
		INPUTS_DefaultGamepadType2List(onlyResetCurrentConfigType);
		ResetInputDefaultValues();
	}

	private void ResetInputDefaultValues()
	{
		coreConfigData.listOfKeyBinding.Clear();
		coreConfigData.listOfKeyBindingGamepadType1.Clear();
		coreConfigData.listOfKeyBindingGamepadType2.Clear();
		foreach (KeyBindingItem item in _defaultListOfKeyBinding)
		{
			coreConfigData.listOfKeyBinding.Add(item);
		}
		foreach (KeyBindingItem item2 in _defaultListOfKeyBindingGamepadType1)
		{
			coreConfigData.listOfKeyBindingGamepadType1.Add(item2);
		}
		foreach (KeyBindingItem item3 in _defaultListOfKeyBindingGamepadType2)
		{
			coreConfigData.listOfKeyBindingGamepadType2.Add(item3);
		}
	}

	private void ValidateInputList()
	{
		foreach (KeyBindingItem entry in _defaultListOfKeyBinding)
		{
			if (!coreConfigData.listOfKeyBinding.Exists((KeyBindingItem x) => x.type == entry.type))
			{
				coreConfigData.listOfKeyBinding.Add(entry);
			}
		}
		foreach (KeyBindingItem entry2 in _defaultListOfKeyBindingGamepadType1)
		{
			if (!coreConfigData.listOfKeyBindingGamepadType1.Exists((KeyBindingItem x) => x.type == entry2.type))
			{
				coreConfigData.listOfKeyBindingGamepadType1.Add(entry2);
			}
		}
		foreach (KeyBindingItem entry3 in _defaultListOfKeyBindingGamepadType2)
		{
			if (!coreConfigData.listOfKeyBindingGamepadType2.Exists((KeyBindingItem x) => x.type == entry3.type))
			{
				coreConfigData.listOfKeyBindingGamepadType2.Add(entry3);
			}
		}
	}

	private void INPUTS_DefaultKeyboardList(bool selfMustBeCurrentConfigType)
	{
		if (!selfMustBeCurrentConfigType || configInputType == InputType.Keyboard)
		{
			_defaultListOfKeyBinding = new List<KeyBindingItem>();
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 0,
				key = 273
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 2,
				key = 276
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 1,
				key = 274
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 3,
				key = 275
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 5,
				key = 122
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 4,
				key = 120
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 16,
				key = 97
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 6,
				key = 100
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 7,
				key = 99
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 15,
				key = 116
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 8,
				key = 112
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 9,
				key = 32
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 28,
				key = 122
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 29,
				key = 120
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 11,
				key = 49
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 12,
				key = 50
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 13,
				key = 51
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 14,
				key = 52
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 26,
				key = 113
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 27,
				key = 119
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 25,
				key = 101
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 17,
				key = 286
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 18,
				key = 287
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 19,
				key = 288
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 20,
				key = 289
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 21,
				key = 290
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 22,
				key = 291
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 23,
				key = 292
			});
			_defaultListOfKeyBinding.Add(new KeyBindingItem
			{
				type = 24,
				key = 293
			});
		}
	}

	private void INPUTS_DefaultGamepadType1List(bool selfMustBeCurrentConfigType)
	{
		if (!selfMustBeCurrentConfigType || configInputType == InputType.GPTypePSX)
		{
			_defaultListOfKeyBindingGamepadType1 = new List<KeyBindingItem>();
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 0,
				key = 116
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 2,
				key = 102
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 1,
				key = 103
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 3,
				key = 104
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 5,
				key = 331
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 4,
				key = 330
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 16,
				key = 335
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 6,
				key = 337
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 7,
				key = 332
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 15,
				key = 115
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 8,
				key = 339
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 9,
				key = 333
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 28,
				key = 331
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 29,
				key = 332
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 11,
				key = 49
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 12,
				key = 50
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 13,
				key = 51
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 14,
				key = 52
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 26,
				key = 97
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 27,
				key = 100
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 25,
				key = 334
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 17,
				key = 340
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 18,
				key = 341
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 19,
				key = 105
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 20,
				key = 106
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 21,
				key = 107
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 22,
				key = 108
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 23,
				key = 343
			});
			_defaultListOfKeyBindingGamepadType1.Add(new KeyBindingItem
			{
				type = 24,
				key = 338
			});
		}
	}

	private void INPUTS_DefaultGamepadType2List(bool selfMustBeCurrentConfigType)
	{
		if (!selfMustBeCurrentConfigType || configInputType == InputType.GPTypeXBOX)
		{
			_defaultListOfKeyBindingGamepadType2 = new List<KeyBindingItem>();
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 0,
				key = 116
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 2,
				key = 102
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 1,
				key = 103
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 3,
				key = 104
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 5,
				key = 330
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 4,
				key = 332
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 16,
				key = 335
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 6,
				key = 117
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 7,
				key = 331
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 15,
				key = 115
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 8,
				key = 337
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 9,
				key = 333
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 28,
				key = 330
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 29,
				key = 331
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 11,
				key = 49
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 12,
				key = 50
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 13,
				key = 51
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 14,
				key = 52
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 26,
				key = 97
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 27,
				key = 100
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 25,
				key = 334
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 17,
				key = 338
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 18,
				key = 339
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 19,
				key = 105
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 20,
				key = 106
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 21,
				key = 107
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 22,
				key = 108
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 23,
				key = 121
			});
			_defaultListOfKeyBindingGamepadType2.Add(new KeyBindingItem
			{
				type = 24,
				key = 336
			});
		}
	}

	public void GetRootLanguageList()
	{
		string text = "";
		if (WEBGL_CheckIfIsWebBuild())
		{
			clsLanguage.listOfLanguage.Add(new LineOfLanguage
			{
				id = 0,
				native_name = "English",
				folder_name = "ENG"
			});
		}
		else
		{
			text = Path.Combine(Application.streamingAssetsPath, "Languages");
			text = Path.Combine(text, "LanguageList.json");
			clsLanguage = JsonUtility.FromJson<ListOfLanguages>(File.ReadAllText(text));
		}
	}

	public void ConcatenateSelectedLanguageFolderPath()
	{
		_isDelayAfterExitPause = false;
		crtDelayAfterPause = null;
		if (WEBGL_CheckIfIsWebBuild())
		{
			return;
		}
		foreach (LineOfLanguage item in clsLanguage.listOfLanguage)
		{
			if (configLanguageType == item.id)
			{
				string path = Path.Combine(Application.streamingAssetsPath, "Languages");
				path = Path.Combine(path, item.folder_name);
				_strCurrentLanguageFolderPath = path;
			}
		}
	}

	public bool LoadUIFile()
	{
		if (WEBGL_CheckIfIsWebBuild())
		{
			// Android/WebGL: embedded TextAsset feed. An empty/failed parse would null
			// clsTranslationUI and NRE every Find(...).text in the menu - guard it.
			clsTranslationUI = JsonUtility.FromJson<ListOfTranslationUI>(_jsonUI);
			if (clsTranslationUI != null && clsTranslationUI.listOfUserInterfaceTranslation.Count > 0)
			{
				return true;
			}
			Debug.LogError("GC_Error[LoadUIFile]: embedded UI TextAsset is empty or failed to parse!");
			return false;
		}
		string path = Path.Combine(_strCurrentLanguageFolderPath, "UserInterface.json");
		clsTranslationUI = JsonUtility.FromJson<ListOfTranslationUI>(File.ReadAllText(path));
		if (clsTranslationUI.listOfUserInterfaceTranslation.Count > 0)
		{
			return true;
		}
		return false;
	}

	public IntroStoryList LoadIntroStoryFile()
	{
		if (WEBGL_CheckIfIsWebBuild())
		{
			IntroStoryList story = JsonUtility.FromJson<IntroStoryList>(_jsonIntro);
			if (story == null || story.listOfIntroStory.Count == 0)
			{
				Debug.LogError("GC_Error[LoadIntroStoryFile]: embedded IntroStory TextAsset is empty or failed to parse!");
			}
			return story;
		}
		return JsonUtility.FromJson<IntroStoryList>(File.ReadAllText(Path.Combine(_strCurrentLanguageFolderPath, "IntroStory.json")));
	}

	public void SetNewBasicAttackVariation()
	{
		coreGameData.intPlayerCurrentBasicAtackVariation = UnityEngine.Random.Range(playerCurrentBasicAttackPower - 2, playerCurrentBasicAttackPower + 3);
	}

	public int GetPlayerDamageForEnemies(PlayerDamageType dmgType, int minusEnemyDefense = 0)
	{
		int num = 0;
		int num2 = playerCurrentBasicAttackVariation;
		num2 = Mathf.RoundToInt(GetDressStatValue(DressStatValue.Attack, num2)) + 1;
		switch (dmgType)
		{
		case PlayerDamageType.BasicAttack:
			num = num2;
			break;
		case PlayerDamageType.slidingTackle:
			num = ((!ACC_Check(AccessoryBehavior.SKILLEnhancedSlidingTackle)) ? num2 : Mathf.RoundToInt(num2 * 3));
			break;
		case PlayerDamageType.ThrowDagger:
			num = (int)MATH_ResultValueOfTotalPercentile(30f, playerCurrentBasicAttackPower);
			if (ACC_Check(AccessoryBehavior.DaggerSpecialist))
			{
				num = (int)MATH_ResultValueOfTotalPercentile(50f, playerCurrentBasicAttackPower);
			}
			break;
		case PlayerDamageType.FlameSword:
			num = Mathf.RoundToInt(num2);
			if (ACC_Check(AccessoryBehavior.SKILLEnhancedFlameSword))
			{
				num = Mathf.RoundToInt((float)num2 * 1.5f);
			}
			break;
		case PlayerDamageType.ForwardSlash:
			num = ((!ACC_Check(AccessoryBehavior.SKILLEnhancePowerSlash)) ? Mathf.RoundToInt(num2 * 3) : Mathf.RoundToInt(num2 * 6));
			break;
		case PlayerDamageType.GroundPound:
			num = Mathf.RoundToInt(num2 * 7);
			break;
		case PlayerDamageType.ReflectProjectile:
			num = Mathf.RoundToInt(num2 * 3);
			break;
		case PlayerDamageType.WeaponWhip:
			num = Mathf.RoundToInt(num2);
			break;
		case PlayerDamageType.SmallSwordSlash:
			num = Mathf.RoundToInt(num2);
			break;
		case PlayerDamageType.throwLance:
			num = Mathf.RoundToInt(num2);
			break;
		case PlayerDamageType.throwStar:
			num = Mathf.RoundToInt((float)num2 * 2.5f);
			break;
		case PlayerDamageType.throwHoming:
			num = Mathf.RoundToInt((float)num2 * 0.2f);
			break;
		}
		if (minusEnemyDefense > 0)
		{
			num -= minusEnemyDefense;
		}
		if (ExistsEffectOnPlayer(TempPlayerEffect.DrinkATK))
		{
			num = Mathf.RoundToInt((float)num * 1.55f);
		}
		return num;
	}

	public void TouchedEssenceCollectible(CollectibleLogic thisEssence)
	{
		InstantiateVFX(VisualEffect.EssenceAbsorb, thisEssence.transform.position, "PlayerItemGet");
		AddEssenceToPlayerAmount(thisEssence.quartzValue);
		string text = thisEssence.name.Substring(4, 3);
		if (!(text == "med"))
		{
			if (!(text == "max"))
			{
				PlaySFX(SFXBasic2.EssenceSmall, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f, "essorb");
			}
			else
			{
				PlaySFX(SFXBasic2.EssenceBig, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f, "essorb");
			}
		}
		else
		{
			PlaySFX(SFXBasic2.EssenceMedium, 0, checkForDuplicity: true, Vector2.zero, 0.9f, 1.1f, "essorb");
		}
	}

	public void AddEssenceToPlayerAmount(int amount)
	{
		coreGameData.intPlayerCurrentEssenceAmount += amount;
		if (coreGameData.intPlayerCurrentEssenceAmount < 0)
		{
			coreGameData.intPlayerCurrentEssenceAmount = 0;
		}
	}

	public bool HealThePlayer(float amountPercent, float effectTime = 1f)
	{
		int num = (int)MATH_ResultValueOfTotalPercentile(amountPercent, playerMaxHealth);
		coreGameData.intPlayerCurrentHealth += num;
		if (coreGameData.intPlayerCurrentHealth > (float)playerMaxHealth)
		{
			playerCurrentHealth = playerMaxHealth;
		}
		if (playerIsBeingGrabed)
		{
			return true;
		}
		if (CheckRefPlayer())
		{
			InstantiateVFX(VisualEffect.HealLife, _refPlayerScript.transform.position, "GAMECORE:HealThePlayer", _refPlayerScript.transform, returnOBJref: true).GetComponent<EffectLogic>().timeDurationLoop = effectTime;
			_refPlayerScript.ToggleShader2DEffect(state: true, 0, effectTime);
		}
		PlaySFX(SFXBasic2.itemDrinkPotion1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		return true;
	}

	public void HealTheSpirit(float amountPercent, float effectTime = 1f)
	{
		int num = (int)MATH_ResultValueOfTotalPercentile(amountPercent, playerMaxSpirit);
		coreGameData.intPlayerCurrentSpirit += num;
		if (playerCurrentSpirit > playerMaxSpirit)
		{
			playerCurrentSpirit = playerMaxSpirit;
		}
		if (!playerIsBeingGrabed)
		{
			if (CheckRefPlayer() && effectTime > 0f)
			{
				InstantiateVFX(VisualEffect.HealSpirit, _refPlayerScript.transform.position, "GAMECORE:HealThePlayer", _refPlayerScript.transform, returnOBJref: true).GetComponent<EffectLogic>().timeDurationLoop = effectTime;
				_refPlayerScript.ToggleShader2DEffect(state: true, 1, effectTime);
			}
			PlaySFX(SFXBasic2.itemDrinkPotion1, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
		}
	}

	public void RemoveSpirit(float amountPercent)
	{
		int num = (int)MATH_ResultValueOfTotalPercentile(amountPercent, playerMaxSpirit);
		coreGameData.intPlayerCurrentSpirit -= num;
		if (coreGameData.intPlayerCurrentSpirit < 0f)
		{
			coreGameData.intPlayerCurrentSpirit = 0f;
		}
	}

	public void GlobalApplyDamageToPlayer(int damage, Vector3 offsetDmgDisplay, bool afterEroEscaped = false, bool bypassDefense = false)
	{
		if (CheckRefPlayer())
		{
			_refPlayerScript.ApplyDamageToPlayer(damage, offsetDmgDisplay, afterEroEscaped, bypassDefense);
		}
	}

	public void GlobalApplyStruggleAmountToPlayer(float amount)
	{
		if (CheckRefPlayer())
		{
			_refPlayerScript.GlobalChangeStruggle(amount);
		}
	}

	public void PlayerPressedUPDuringStruggleCheckHeartIcon()
	{
		if (CheckRefCoreUI())
		{
			coreUI.STG_PlayerPressedUpOnStruggle();
		}
	}

	public void AddRemoveLustToPlayer(int amount, bool doVFX)
	{
		if (MATH_RNG_ROLL(90) && ACC_Check(AccessoryBehavior.Chastity) && amount > 0)
		{
			return;
		}
		coreGameData.intLustAmount += amount;
		if (coreGameData.intLustAmount < 0)
		{
			coreGameData.intLustAmount = 0;
		}
		if (coreGameData.intLustAmount > 100)
		{
			coreGameData.intLustAmount = 100;
		}
		if (amount > 0)
		{
			CheckRefPlayer();
			if (doVFX)
			{
				InstantiateVFX(VisualEffect.GainLust, new Vector2(playerCurrentWorldXPosition, playerCurrentWorldYPosition), "GC.AddLust", playerTransformReference);
			}
		}
	}

	public void TRAIT_CHECK_ADD(MayaTraits thisTrait)
	{
		if (!TRAIT_CHECK_EXISTS(thisTrait))
		{
			switch (thisTrait)
			{
			case MayaTraits.Masochist:
				AddItemToPlayerInventory(920, "Trait", Vector2.zero);
				break;
			case MayaTraits.Exibitionist:
				AddItemToPlayerInventory(921, "Trait", Vector2.zero);
				break;
			case MayaTraits.Horny:
				AddItemToPlayerInventory(922, "Trait", Vector2.zero);
				break;
			case MayaTraits.Nymphomaniac:
				AddItemToPlayerInventory(923, "Trait", Vector2.zero);
				break;
			case MayaTraits.VoreWhore:
				AddItemToPlayerInventory(924, "Trait", Vector2.zero);
				break;
			}
		}
	}

	public bool TRAIT_CHECK_EXISTS(MayaTraits thisTrait)
	{
		switch (thisTrait)
		{
		case MayaTraits.Masochist:
			return CheckPlayerHasItem(920);
		case MayaTraits.Exibitionist:
			return CheckPlayerHasItem(921);
		case MayaTraits.Horny:
			return CheckPlayerHasItem(922);
		case MayaTraits.Nymphomaniac:
			return CheckPlayerHasItem(923);
		case MayaTraits.VoreWhore:
			return CheckPlayerHasItem(924);
		default:
			return false;
		}
	}

	public bool ExistsEffectOnPlayer(TempPlayerEffect thisEffect)
	{
		if (playerEffectList.Exists((BuffEffect x) => x.effect == thisEffect))
		{
			return true;
		}
		return false;
	}

	public void ApplyEffectToPlayer(TempPlayerEffect thisEffect, float time, string sender)
	{
		if (!_playerEffectList.Exists((BuffEffect x) => x.effect == thisEffect))
		{
			_playerEffectList.Add(new BuffEffect
			{
				effect = thisEffect,
				countTimer = 0f,
				maxTimer = time
			});
			if (CheckRefPlayer())
			{
				_refPlayerScript.ApplyInitialEffects(thisEffect);
			}
			if (!IsInvoking("INVOKED_StatusEffectCountUpToTimer"))
			{
				InvokeRepeating("INVOKED_StatusEffectCountUpToTimer", 0f, 0.05f);
			}
		}
	}

	public void RemoveEffectFromPlayer(TempPlayerEffect thisEffect, string senderLog, bool cleanAllEffects = false)
	{
		bool flag = false;
		if (cleanAllEffects)
		{
			_playerEffectList.Clear();
		}
		else if (ExistsEffectOnPlayer(thisEffect))
		{
			_playerEffectList.RemoveAll((BuffEffect x) => x.effect == thisEffect);
			flag = true;
		}
		if (CheckRefPlayer() & flag)
		{
			_refPlayerScript.ApplyFinishedEffects(thisEffect);
		}
	}

	private void INVOKED_StatusEffectCountUpToTimer()
	{
		if (_isSceneTransitioning)
		{
			return;
		}
		foreach (BuffEffect playerEffect in _playerEffectList)
		{
			playerEffect.countTimer += 0.05f;
			if (CheckRefCoreUI() && playerEffect.refUIElement == null && playerEffect.countTimer < playerEffect.maxTimer)
			{
				coreUI.ShowDisplayStatusEffectUIBox(playerEffect.effect, playerEffect.countTimer, playerEffect.maxTimer);
			}
		}
		for (int num = _playerEffectList.Count - 1; num >= 0; num--)
		{
			if (_playerEffectList[num].countTimer > _playerEffectList[num].maxTimer)
			{
				RemoveEffectFromPlayer(_playerEffectList[num].effect, "GAMECORE.INVOKED_StatusEffectCountUpToTimer()");
			}
		}
	}

	public void StatusEffectRefenreceToUIElement(GameObject objUI, TempPlayerEffect thisEffect)
	{
		foreach (BuffEffect playerEffect in _playerEffectList)
		{
			if (playerEffect.effect == thisEffect)
			{
				playerEffect.refUIElement = objUI;
			}
		}
	}

	public bool CheckPlayerProximityToAnyObject(Vector2 posOfObject, PlayerProximityType checkType, float customProximityX = 0f, float customProximityY = 0f)
	{
		float num = playerCurrentWorldXPosition - posOfObject.x;
		float num2 = playerCurrentWorldYPosition - posOfObject.y;
		if (num < 0f)
		{
			num *= -1f;
		}
		if (num2 < 0f)
		{
			num2 *= -1f;
		}
		switch (checkType)
		{
		default:
			if (num <= customProximityX && num2 <= customProximityY)
			{
				return true;
			}
			break;
		case PlayerProximityType.MainBehaviour:
			if (customProximityX <= 0f || customProximityY <= 0f)
			{
				return false;
			}
			if (num <= customProximityX && num2 <= customProximityY)
			{
				return true;
			}
			break;
		case PlayerProximityType.FaintedChase:
			if (customProximityX <= 0f || customProximityY <= 0f)
			{
				return false;
			}
			if (num <= customProximityX && num2 <= customProximityY)
			{
				return true;
			}
			break;
		case PlayerProximityType.Adjacent:
			if (num <= 1f && num2 <= 1f)
			{
				return true;
			}
			break;
		case PlayerProximityType.PlayerToTheRightCustom:
			if (playerCurrentWorldXPosition > posOfObject.x && num <= customProximityX)
			{
				if (customProximityY > 0f && num2 > customProximityY)
				{
					return false;
				}
				return true;
			}
			break;
		case PlayerProximityType.PlayerToTheLeftCustom:
			if (playerCurrentWorldXPosition < posOfObject.x && num <= customProximityX)
			{
				if (customProximityY > 0f && num2 > customProximityY)
				{
					return false;
				}
				return true;
			}
			break;
		case PlayerProximityType.FlyingChase:
			if (num <= 12f && num2 <= 12f)
			{
				return true;
			}
			break;
		case PlayerProximityType.Custom:
			if (num <= customProximityX && num2 <= customProximityY)
			{
				return true;
			}
			break;
		case PlayerProximityType.CustomInverted:
			if (num >= customProximityX || num2 >= customProximityY)
			{
				return true;
			}
			break;
		case PlayerProximityType.CustomXOnly:
			if (num <= customProximityX)
			{
				return true;
			}
			break;
		case PlayerProximityType.CustomXOnlyInverted:
			if (num >= customProximityX)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public bool CheckProximityBetweenTwoPositions(Vector2 pos1, Vector2 pos2, float proxX, float proxY)
	{
		float num = pos2.x - pos1.x;
		float num2 = pos2.y - pos1.y;
		if (num < 0f)
		{
			num *= -1f;
		}
		if (num2 < 0f)
		{
			num2 *= -1f;
		}
		if (num <= proxX && num2 <= proxY)
		{
			return true;
		}
		return false;
	}

	public Vector2 MATH_GetNearestPosition(Vector3 referencePos, RaycastHit2D[] arrayHits, int basedOnFacing = 0, int countFailSafe = 100, int doRNG = 0, bool getEnemyCenterPos = false)
	{
		Vector2 vector = Vector2.zero;
		float num = float.PositiveInfinity;
		bool flag = false;
		for (int i = 0; i < arrayHits.Length; i++)
		{
			RaycastHit2D raycastHit2D = arrayHits[i];
			float sqrMagnitude = (raycastHit2D.transform.position - referencePos).sqrMagnitude;
			if (doRNG > 0 && MATH_RNG_ROLL(doRNG) && vector != Vector2.zero)
			{
				flag = true;
			}
			if (((sqrMagnitude < num) | flag) && (basedOnFacing == 0 || (basedOnFacing > 0 && raycastHit2D.transform.position.x > referencePos.x) || (basedOnFacing < 0 && raycastHit2D.transform.position.x < referencePos.x)))
			{
				num = sqrMagnitude;
				vector = raycastHit2D.transform.position;
				if (getEnemyCenterPos && raycastHit2D.transform.gameObject != null && raycastHit2D.transform.gameObject.activeInHierarchy)
				{
					EnemyGeneralLogic component = raycastHit2D.transform.GetComponent<EnemyGeneralLogic>();
					if (component != null)
					{
						vector = component.GetEnemyCenterPos();
					}
				}
			}
			countFailSafe--;
			if ((countFailSafe <= 0) | flag)
			{
				break;
			}
		}
		return vector;
	}

	public void SetPlayerSpriteRendererSortingOrder(int layerToSet, bool resetToOriginal = false)
	{
		if (CheckRefPlayer())
		{
			_refPlayerScript.ChangePlayerSpriteRendererSortingOrder(layerToSet, resetToOriginal);
		}
	}

	public void SpecialPlayerGone()
	{
		if (CheckRefPlayer())
		{
			_refPlayerScript.SpecialPlayerGone();
		}
	}

	public void RemoveChildObjectsFromPlayer(bool alsoDisableObject = false, string thisName = "", string thisTag = "", Transform setNewParent = null)
	{
		List<GameObject> list = new List<GameObject>();
		if (!CheckRefPlayer())
		{
			return;
		}
		if (thisName != "")
		{
			foreach (Transform item in _refPlayerScript.transform)
			{
				if (item.name.Contains(thisName))
				{
					list.Add(item.gameObject);
					break;
				}
			}
		}
		if (thisTag != "")
		{
			foreach (Transform item2 in _refPlayerScript.transform)
			{
				if (item2.tag == thisTag)
				{
					list.Add(item2.gameObject);
					break;
				}
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			if ((bool)setNewParent)
			{
				list[i].transform.parent = setNewParent;
			}
			else
			{
				list[i].transform.parent = null;
			}
			if (alsoDisableObject)
			{
				list[i].SetActive(value: false);
			}
		}
	}

	public void SetupGalleryViewBeforeLoad(EnemyType typeOfEnemy, GameObject objEnemyToSpawn, GameObject objEnemyCombo, int idEroVariantToPlay, Vector3 offsetSpawnPos)
	{
		try
		{
			PlaySFX(SFXBasic2.GateClose, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			_galleryEnemy = new clsGalleryEnemyToView();
			_galleryEnemy.isRedirectToGalleryView = true;
			_galleryEnemy.objEnemyPrefab = objEnemyToSpawn;
			_galleryEnemy.objEnemyCombo = objEnemyCombo;
			_galleryEnemy.idEroVariant = idEroVariantToPlay;
			_galleryEnemy.offsetSpawnPos = offsetSpawnPos;
			if (TrySetupGameLoad(getLatestSave: false, 0, MenuScene.InGame, isRetry: true))
			{
				TogglePauseForGameMenu();
			}
		}
		catch (Exception ex)
		{
			Debug.Log("[GAMECORE]SetupGallery_Error: " + ex.Message);
			throw;
		}
	}

	public void ApplyGalleryViewLogicAfterLoad()
	{
		PortalSameLevel galleryEroTargetPointReference = GetGalleryEroTargetPointReference();
		Vector3 vector = new Vector3(0f, 0f, 0f);
		CallReferenceMayaVoiceGC("rng-Scream3", "scptGalleryScreenUI.StartGallerySpecialViewLogic()");
		vector = galleryEroTargetPointReference.transform.position + _galleryEnemy.offsetSpawnPos;
		vector.x += MATH_RNG_FLOAT(-7f, 7f);
		galleryEroTargetPointReference.transform.position = vector;
		if (CheckRefPlayer())
		{
			_refPlayerScript.TeleportSameLevelGallery(galleryEroTargetPointReference);
		}
		RemoveSpirit(100f);
		DressDurabilityManipulateValue(-100, isPercent: true, isRemoveForGallery: true);
		GameObject obj = UnityEngine.Object.Instantiate(_galleryEnemy.objEnemyPrefab, vector, Quaternion.identity, refOBJEnemiesParent.transform);
		obj.GetComponent<EnemyGeneralLogic>().isGalleryView = true;
		obj.GetComponent<EnemyGeneralLogic>().intGalleryVariantEroID = _galleryEnemy.idEroVariant;
		if (_galleryEnemy.objEnemyCombo != null)
		{
			bool flag = true;
			string text = _galleryEnemy.objEnemyCombo.name;
			if (text.Contains("Goblin") || text.Contains("Hill-Fairy"))
			{
				HACK_ComboGalleryViewCombo(vector, 1.15f);
				flag = false;
			}
			if (flag)
			{
				StartCoroutine(waitGalleryAction(1.15f, "delayComboSpawn", _galleryEnemy.objEnemyCombo, vector));
			}
		}
		_galleryEnemy = new clsGalleryEnemyToView();
		InjectPlayerPositionOnRetryStored(15f, -0.5f);
		if (CheckRefCoreUI())
		{
			coreUI.ToggleUIElementsOnFainted();
		}
		MINIMAP_Toggle(toggleCurrentState: false, setThisSpecificState: false);
	}

	private PortalSameLevel GetGalleryEroTargetPointReference()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("eroSpecialTP");
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Contains("Gallery"))
			{
				return array[i].GetComponent<PortalSameLevel>();
			}
		}
		return null;
	}

	public bool GalleryValidateRoomAndView(GalleryThemeType thisTheme)
	{
		switch (thisTheme)
		{
		case GalleryThemeType.Beach:
			if (CheckPlayerHasItem(909) || configGalleryRoomBeach)
			{
				return true;
			}
			break;
		case GalleryThemeType.Forest:
			if (CheckPlayerHasItem(910) || configGalleryRoomForest)
			{
				return true;
			}
			break;
		case GalleryThemeType.Castle:
			if (CheckPlayerHasItem(911) || configGalleryRoomCastle)
			{
				return true;
			}
			break;
		case GalleryThemeType.Hill:
			if (CheckPlayerHasItem(912) || configGalleryRoomHill)
			{
				return true;
			}
			break;
		case GalleryThemeType.City:
			if (CheckPlayerHasItem(913) || configGalleryRoomCity)
			{
				return true;
			}
			break;
		case GalleryThemeType.Cave:
			if (CheckPlayerHasItem(914) || configGalleryRoomCaves)
			{
				return true;
			}
			break;
		case GalleryThemeType.Swamp:
			if (CheckPlayerHasItem(915) || configGalleryRoomSwamp)
			{
				return true;
			}
			break;
		case GalleryThemeType.HighTech:
			if (CheckPlayerHasItem(916) || configGalleryRoomHighTech)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public bool CompareFetishToggle(FetishType thisType)
	{
		switch (thisType)
		{
		case FetishType.Vanilla:
			if (configFetishes.showVanilla)
			{
				return true;
			}
			break;
		case FetishType.Blowjob:
			if (configFetishes.showBlowjob)
			{
				return true;
			}
			break;
		case FetishType.Feet:
			if (configFetishes.showFeet)
			{
				return true;
			}
			break;
		case FetishType.Lesbian:
			if (configFetishes.showLesbian)
			{
				return true;
			}
			break;
		case FetishType.Futanari:
			if (configFetishes.showFutanari)
			{
				return true;
			}
			break;
		case FetishType.Tentacle:
			if (configFetishes.showTentacle)
			{
				return true;
			}
			break;
		case FetishType.Beast:
			if (configFetishes.showBeast)
			{
				return true;
			}
			break;
		case FetishType.Insect:
			if (configFetishes.showInsect)
			{
				return true;
			}
			break;
		case FetishType.Ryona:
			if (configFetishes.showRyona)
			{
				return true;
			}
			break;
		case FetishType.Vore:
			if (configFetishes.showVore)
			{
				return true;
			}
			break;
		}
		return false;
	}

	private IEnumerator waitGalleryAction(float time, string action, GameObject objToSpawn, Vector3 spawnPos)
	{
		yield return new WaitForSeconds(time);
		if (action == "delayComboSpawn")
		{
			EnemyGeneralLogic component = UnityEngine.Object.Instantiate(objToSpawn, spawnPos, Quaternion.identity, refOBJEnemiesParent.transform).GetComponent<EnemyGeneralLogic>();
			if ((bool)component)
			{
				component.isAddComboGalleryView = true;
			}
		}
	}

	public bool ValidateAnimationControlWindow()
	{
		if (strCurrentActiveSceneName == "AnimGallery" && _playerIsBeingGrabed && !_isGamePaused && !_isSceneTransitioning && _playerCurrentWorldXPosition > galleryPlayerX_ArenaLimit && !isDisplayingDialogue)
		{
			return true;
		}
		return false;
	}

	public void RelayAnimControlAction(int actionID)
	{
		if (actionID == 6 && !_bufferAnimControlAction && !ValidateAnimationControlReferences())
		{
			TrySetupGameLoad(getLatestSave: false, 0, MenuScene.InGame, isRetry: true);
		}
		else if (!_bufferAnimControlAction && ValidateAnimationControlReferences())
		{
			_animControlScript.PerformAnimationControlAction((AnimPlayback)actionID);
			if (crtBufferAnimControlAction != null)
			{
				StopCoroutine(crtBufferAnimControlAction);
			}
			crtBufferAnimControlAction = StartCoroutine(waitBufferAnimControlActions(0.15f));
		}
	}

	private bool ValidateAnimationControlReferences()
	{
		if (_objEnemyGrabbingPlayer == null)
		{
			DisplaySystemMessageUI(6);
			return false;
		}
		if (_animControlComponent == null || _animControlComponent.gameObject != _objEnemyGrabbingPlayer)
		{
			_animControlComponent = _objEnemyGrabbingPlayer.GetComponent<Animator>();
			_animControlScript = _objEnemyGrabbingPlayer.GetComponent<EroAnimation>();
		}
		if (_animControlComponent != null && _animControlComponent.gameObject == _objEnemyGrabbingPlayer)
		{
			return true;
		}
		return false;
	}

	private IEnumerator waitBufferAnimControlActions(float wait)
	{
		_bufferAnimControlAction = true;
		yield return new WaitForSeconds(wait);
		_bufferAnimControlAction = false;
		crtBufferAnimControlAction = null;
	}

	public void DoPlayerTeleport(string sceneName, Vector2 positionOfPiece)
	{
		StopAllSFXCurrentylPlaying();
		PlaySFX(SFXBasic1.playerTP_UP, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		playerIsInvincible = true;
		isPlayerInteractingLocked = true;
		isForbidAllPlayerInput = true;
		playerIsAllowedToBeGrabbed = false;
		playerDidTeleport = true;
		TogglePauseForGameMenu();
		if (CheckRefPlayer())
		{
			_refPlayerScript.TeleportAnimation(isDepart: true);
		}
		LoadNextScene(sceneName, positionOfPiece, Vector2.zero, forceMovePlayer: false, applyFade: true, Color.black, 2f, lockPlayerInput: true, persistPlayerGameSessionValues: true);
	}

	public bool UnifiedInputHandler(InputLib requestedInput, InputPressType typeOfPress)
	{
		switch (configInputType)
		{
		case InputType.Keyboard:
			if (UnifiedInputHandlerTypeOfPress(requestedInput, typeOfPress))
			{
				return true;
			}
			break;
		case InputType.GPTypePSX:
		case InputType.GPTypeXBOX:
			if (UnifiedInputHandlerGamepadDirectional(requestedInput, typeOfPress) || UnifiedInputHandlerTypeOfPress(requestedInput, typeOfPress))
			{
				return true;
			}
			break;
		}
		return false;
	}

	private bool UnifiedInputHandlerGamepadDirectional(InputLib sentInput, InputPressType sentType)
	{
		if (_listGPInputState != null && _listGPInputState.Count > 0 && _listGPInputState.Exists((GamepadDirectionalStateControl x) => x.reservedKey == IKey(sentInput) && x.state == sentType))
		{
			return true;
		}
		return false;
	}

	private bool UnifiedInputHandlerTypeOfPress(InputLib sentInput, InputPressType sentPress)
	{
		switch (sentPress)
		{
		case InputPressType.KeyPressedOnce:
			if (Input.GetKeyDown(IKey(sentInput)))
			{
				return true;
			}
			break;
		case InputPressType.KeyHolding:
			if (Input.GetKey(IKey(sentInput)))
			{
				return true;
			}
			break;
		case InputPressType.KeyReleased:
			if (Input.GetKeyUp(IKey(sentInput)))
			{
				return true;
			}
			break;
		}
		return false;
	}

	public KeyCode IKey(InputLib type)
	{
		return (KeyCode)inputList.Find((KeyBindingItem x) => x.type == (int)type).key;
	}

	public void InsertNewInputKeyCode(InputLib newType, KeyCode newKeyCode)
	{
		inputList[inputList.FindIndex((KeyBindingItem x) => x.type == (int)newType)].key = (int)newKeyCode;
	}

	public void SwapDuplicateKeybindings(InputLib newType, KeyCode newKeyCode, InputLib duplicateType)
	{
		inputList[inputList.FindIndex((KeyBindingItem x) => x.type == (int)duplicateType)].key = inputList[inputList.FindIndex((KeyBindingItem x) => x.type == (int)newType)].key;
		inputList[inputList.FindIndex((KeyBindingItem x) => x.type == (int)newType)].key = (int)newKeyCode;
	}

	public void ResetPlayerDressDurability()
	{
		coreGameData.intPlayerCurrentDressDurability = coreGameData.intPlayerMaxDressDurability;
	}

	public void DressDurabilityManipulateValue(int value, bool isPercent, bool isRemoveForGallery = false)
	{
		if (isPercent)
		{
			coreGameData.intPlayerCurrentDressDurability += (int)MATH_ResultValueOfTotalPercentile(value, coreGameData.intPlayerMaxDressDurability);
		}
		else
		{
			coreGameData.intPlayerCurrentDressDurability += value;
		}
		if (coreGameData.intPlayerCurrentDressDurability > 100)
		{
			coreGameData.intPlayerCurrentDressDurability = 100;
		}
		if (coreGameData.intPlayerCurrentDressDurability < 0)
		{
			coreGameData.intPlayerCurrentDressDurability = 0;
		}
		if (isRemoveForGallery && CheckRefPlayer())
		{
			_refPlayerScript.ForceOverrideDressState(2);
		}
	}

	public void ToggleSlowMotionOnDressBreak(bool actvate)
	{
		if (!coreConfigData.adv.toggleSlowZoomOnDressBreak)
		{
			return;
		}
		if (actvate)
		{
			if (!_isGamePaused)
			{
				Time.timeScale = 0.6f;
			}
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void ForceChangeCurrentPlayerDressState(int stateID, bool isIntentionalPlayerRemoval = false)
	{
		if (!CheckRefPlayer())
		{
			Debug.Log("Error! Could not locate player reference, in order to change the dress state.");
		}
		_refPlayerScript.ForceOverrideDressState(stateID, isIntentionalPlayerRemoval);
	}

	public bool CheckPlayerDressStateValidation(bool ignorePaused = false)
	{
		if (CheckRefPlayer() && _refPlayerScript.CheckBasicLogicPermissions(ignorePaused) && _playerIsOnGround && _playerCurrentSkill == "Null" && !_playerIsInvincible && !isPlayerFainted && !_playerIsBeingGrabed)
		{
			return true;
		}
		return false;
	}

	public bool CallRestorePlayerArmorLayers(bool checkValidity)
	{
		if (!CheckRefPlayer())
		{
			Debug.Log("Error! Could not locate player reference, in order to change the dress state.");
			return false;
		}
		if (checkValidity && !_refPlayerScript.CheckValidToRepairAllClothingLayer())
		{
			return false;
		}
		_refPlayerScript.RestoreAllArmorLayers();
		return true;
	}

	public FetishType GetFetishTypeByEroSceneVariantID(int eroVariantID, ENM_EroBaseConfig eroCore)
	{
		FetishType result = FetishType.Vanilla;
		bool flag = false;
		if (eroVariantID == 0 && eroCore.FetishTags.Length != 0)
		{
			result = eroCore.FetishTags[0];
			flag = true;
		}
		for (int i = 0; i < eroCore.eroSoloAlts.Length; i++)
		{
			if (flag)
			{
				break;
			}
			if (eroCore.eroSoloAlts[i].GenericID == eroVariantID)
			{
				result = eroCore.eroSoloAlts[i].SceneTag;
				flag = true;
			}
		}
		for (int j = 0; j < eroCore.eroComboScenes.Length; j++)
		{
			if (flag)
			{
				break;
			}
			if (eroCore.eroComboScenes[j].GenericID == eroVariantID)
			{
				result = eroCore.eroComboScenes[j].SceneTag;
				flag = true;
			}
		}
		return result;
	}

	public string GetFetishNameByType(FetishType thisType)
	{
		int sentID = 0;
		switch (thisType)
		{
		case FetishType.Vanilla:
			sentID = 73;
			break;
		case FetishType.Blowjob:
			sentID = 74;
			break;
		case FetishType.Feet:
			sentID = 75;
			break;
		case FetishType.Lesbian:
			sentID = 76;
			break;
		case FetishType.Futanari:
			sentID = 77;
			break;
		case FetishType.Tentacle:
			sentID = 78;
			break;
		case FetishType.Beast:
			sentID = 79;
			break;
		case FetishType.Insect:
			sentID = 80;
			break;
		case FetishType.Ryona:
			sentID = 81;
			break;
		case FetishType.Vore:
			sentID = 82;
			break;
		}
		return GetTranslatedStringUI(sentID);
	}

	private void HACK_ComboGalleryViewCombo(Vector3 targetPos, float delaySpawn)
	{
		int num = 0;
		if (_galleryEnemy.offsetSpawnPos.x == 0.2f)
		{
			num = 1;
		}
		else if (_galleryEnemy.offsetSpawnPos.x == 0.3f)
		{
			num = 2;
		}
		else if (_galleryEnemy.offsetSpawnPos.x == 0.4f)
		{
			num = 3;
		}
		for (int i = 0; i < num; i++)
		{
			StartCoroutine(waitGalleryAction(delaySpawn, "delayComboSpawn", _galleryEnemy.objEnemyCombo, targetPos));
		}
	}

	public bool VerifyOverrideSpecialCasesForCombos(EnemyType thisEnemy)
	{
		CFG_OverrideFetishToggle cFG_OverrideFetishToggle = null;
		switch (thisEnemy)
		{
		case EnemyType.LizardShield:
		case EnemyType.LizardAxe:
			cFG_OverrideFetishToggle = FetishOverrideCheckExistance(EnemyType.LizardShield, 0);
			if (cFG_OverrideFetishToggle != null && cFG_OverrideFetishToggle.switcherOverride)
			{
				return true;
			}
			break;
		case EnemyType.Goblin:
			cFG_OverrideFetishToggle = FetishOverrideCheckExistance(EnemyType.Goblin);
			if (cFG_OverrideFetishToggle != null && cFG_OverrideFetishToggle.switcherOverride)
			{
				return true;
			}
			break;
		case EnemyType.Goblina:
			cFG_OverrideFetishToggle = FetishOverrideCheckExistance(EnemyType.Goblina);
			if (cFG_OverrideFetishToggle != null && cFG_OverrideFetishToggle.switcherOverride)
			{
				return true;
			}
			break;
		case EnemyType.HillFairy:
			cFG_OverrideFetishToggle = FetishOverrideCheckExistance(EnemyType.HillFairy);
			if (cFG_OverrideFetishToggle != null && cFG_OverrideFetishToggle.switcherOverride)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public bool VerifyComboImageDisplayOnGalleryScreen(EnemyType thisEnemy)
	{
		switch (thisEnemy)
		{
		case EnemyType.LizardShield:
		case EnemyType.LizardAxe:
		case EnemyType.Goblin:
			if (CompareFetishToggle(FetishType.Vanilla))
			{
				return true;
			}
			return false;
		case EnemyType.HillFairy:
		case EnemyType.Goblina:
			if (CompareFetishToggle(FetishType.Lesbian))
			{
				return true;
			}
			return false;
		default:
			return false;
		}
	}

	public void FetishOverrideTryAlter(EnemyType enemy, int sceneID, bool overrideForcedStatus, bool isJustEntryRemoval = false)
	{
		CFG_OverrideFetishToggle cFG_OverrideFetishToggle = FetishOverrideCheckExistance(enemy, sceneID);
		if (cFG_OverrideFetishToggle != null)
		{
			coreConfigData.listFetishOverride.Remove(cFG_OverrideFetishToggle);
		}
		if (!isJustEntryRemoval)
		{
			coreConfigData.listFetishOverride.Add(new CFG_OverrideFetishToggle
			{
				enemy = enemy,
				sceneID = sceneID,
				switcherOverride = overrideForcedStatus
			});
		}
	}

	public CFG_OverrideFetishToggle FetishOverrideCheckExistance(EnemyType enemy)
	{
		return coreConfigData.listFetishOverride.Find((CFG_OverrideFetishToggle x) => x.enemy == enemy);
	}

	public CFG_OverrideFetishToggle FetishOverrideCheckExistance(EnemyType enemy, int sceneID)
	{
		return coreConfigData.listFetishOverride.Find((CFG_OverrideFetishToggle x) => x.enemy == enemy && x.sceneID == sceneID);
	}

	private void ESSENCE_SetCurrentChance()
	{
		_chanceEnemySpawnEssence = 25;
		_rangeOfOrbAttraction = 3f;
	}

	public void ESSENCE_Spawn(EssenceSpawnType typeToSpawn, Vector3 spawnPos, int maxLife, int eroGrabPercentage = 30)
	{
		int num = maxLife / 10;
		if (typeToSpawn == EssenceSpawnType.EnemyDefeated)
		{
			num = Mathf.RoundToInt(GetDressStatValue(DressStatValue.EroGather, num));
		}
		if (typeToSpawn == EssenceSpawnType.EroGrab)
		{
			num = Mathf.RoundToInt(MATH_ResultValueOfTotalPercentile(eroGrabPercentage, num));
		}
		if (typeToSpawn == EssenceSpawnType.SuccubusHeadress)
		{
			num *= 2;
		}
		if (typeToSpawn == EssenceSpawnType.ragePepper)
		{
			num /= 2;
		}
		foreach (GameObject item in ESSENCE_GenerateListOfEssenceToSpawn(num))
		{
			UnityEngine.Object.Instantiate(item, spawnPos, Quaternion.identity, _mainEffectParent.transform);
		}
	}

	private List<GameObject> ESSENCE_GenerateListOfEssenceToSpawn(int points)
	{
		List<GameObject> list = new List<GameObject>();
		bool flag = false;
		bool flag2 = false;
		do
		{
			if (points >= 100)
			{
				if (flag || !MATH_RNG_ROLL(20))
				{
					points -= 100;
					list.Add(objEssenceBig);
					continue;
				}
				flag = true;
			}
			if (points >= 10)
			{
				if (flag2 || !MATH_RNG_ROLL(20))
				{
					points -= 10;
					list.Add(objEssenceMedium);
					continue;
				}
				flag2 = true;
			}
			if (points >= 1)
			{
				points--;
				list.Add(objEssenceSmall);
			}
		}
		while (points > 0);
		return list;
	}

	public void AddItemToPlayerInventory(int itemID, string itemSceneName, Vector2 itemPosition)
	{
		ItemInfo itemInfoFromDatabase = GetItemInfoFromDatabase(itemID);
		if (itemInfoFromDatabase == null)
		{
			Debug.Log("[GC]AddItemToPlayerInventory: Could not find specified itemID (" + itemID + ") in the database!");
			return;
		}
		coreGameData.listOfItemInventory.Add(new ItemInfo
		{
			id = itemID,
			name = itemInfoFromDatabase.name,
			equipable = itemInfoFromDatabase.equipable,
			description = itemInfoFromDatabase.description,
			position = itemPosition,
			scene = itemSceneName,
			type = itemInfoFromDatabase.type
		});
		CheckBeltItemCountUpdate(itemID, isItemAdded: true);
		if (itemInfoFromDatabase.equipable && doAutoEquipFeature && !ACC_IsItemAccessory(itemID))
		{
			VerifyAutoItemEquipOnPickup(itemInfoFromDatabase.id);
		}
	}

	public void RemoveItemFromPlayerInventory(int itemID)
	{
		ItemInfo itemInfo = coreGameData.listOfItemInventory.Find((ItemInfo x) => x.id == itemID);
		if (itemInfo == null)
		{
			Debug.Log("[GC]RemoveItemFromPlayerInventory: Could not find specified itemID (" + itemID + ") in the players inventory!");
			return;
		}
		coreGameData.listOfItemInventory.Remove(itemInfo);
		CheckBeltItemCountUpdate(itemID, isItemAdded: false);
	}

	public bool CheckPlayerHasItem(int itemID)
	{
		if (coreGameData.listOfItemInventory.Find((ItemInfo x) => x.id == itemID) != null || IsThereFlagActiveForThisUniqueItem(itemID, "notApplicable"))
		{
			return true;
		}
		return false;
	}

	public bool CheckPlayerHasItem(int itemID, string sceneName, Vector2 itemPos, string objName)
	{
		if (coreGameData.listOfItemInventory.Find((ItemInfo x) => x.id == itemID && x.scene == sceneName && x.position == itemPos) != null || IsThereFlagActiveForThisUniqueItem(itemID, objName))
		{
			return true;
		}
		return false;
	}

	public int ItemQuantityInPlayerInventory(int itemID)
	{
		int num = 0;
		foreach (ItemInfo item in coreGameData.listOfItemInventory.FindAll((ItemInfo x) => x.id == itemID))
		{
			_ = item;
			num++;
		}
		return num;
	}

	public ItemInfo GetItemInfoFromDatabase(int thisItemID)
	{
		return clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == thisItemID);
	}

	public string GetItemNameFromList(int itemID)
	{
		return coreGameData.listOfItemInventory.Find((ItemInfo x) => x.id == itemID)?.name;
	}

	private bool IsThereFlagActiveForThisUniqueItem(int itemID, string itemName)
	{
		if (itemID == 50 && coreGameData.skillFlags.hasDash)
		{
			return true;
		}
		return false;
	}

	public void UpdateLanguageOfItemsInPlayerInventory()
	{
		ItemInfo itemInfo = null;
		int i;
		for (i = 0; i < listPlayerInventory.Count; i++)
		{
			itemInfo = clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == listPlayerInventory[i].id);
			if (itemInfo != null)
			{
				listPlayerInventory[i].name = itemInfo.name;
				listPlayerInventory[i].description = itemInfo.description;
				itemInfo = null;
			}
		}
	}

	public void LoadItemDatabaseFile()
	{
		if (WEBGL_CheckIfIsWebBuild())
		{
			clsItemDatabase = JsonUtility.FromJson<ListOfItemDatabase>(_jsonItemList);
			if (clsItemDatabase == null || clsItemDatabase.listOfItemDatabase.Count == 0)
			{
				Debug.LogError("GC_Error[LoadItemDatabaseFile]: embedded ItemList TextAsset is empty or failed to parse!");
			}
			return;
		}
		string path = Path.Combine(_strCurrentLanguageFolderPath, "ItemList.json");
		clsItemDatabase = JsonUtility.FromJson<ListOfItemDatabase>(File.ReadAllText(path));
	}

	public void ItemButtonWasClickedOnInventoryScreenToOpenEquipScreen(int itemID, int itemQuantity, Button thisButton)
	{
		if (CheckRefMenuBehaviour())
		{
			CountItemsQuantity sentItem = new CountItemsQuantity
			{
				id = itemID,
				quantity = itemQuantity
			};
			_refMenuScript.ShowBeltEquipSlotScreen(sentItem, thisButton);
		}
	}

	public void CycleActiveItem(bool isForwards, bool doSound = false)
	{
		if (doSound)
		{
			if (isForwards)
			{
				PlaySFX(SFXBasic1.CycleItemNext, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			}
			else
			{
				PlaySFX(SFXBasic1.CycleItemPrevious, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f);
			}
		}
		coreGameData.intIndexOfSlotCurrentActiveItem = ITEM_CycleBeltSlotReturnNextUseable(isForwards);
	}

	public int ITEM_CycleBeltSlotReturnNextUseable(bool isForwardNext)
	{
		int num = indexCurrentBeltSlotActive;
		int num2 = 0;
		int num3 = arraySlotsOfBelt.Length + 1;
		do
		{
			num2++;
			num = (isForwardNext ? ((num < arraySlotsOfBelt.Length - 1) ? (num + 1) : 0) : ((num != 0) ? (num - 1) : (arraySlotsOfBelt.Length - 1)));
		}
		while ((!CheckBeltSlotValidity(num) || ACC_IsItemAccessory(coreGameData.arrayBeltSlots[num].id)) && num2 < num3);
		return num;
	}

	public void WipePlayerCurrentInventory()
	{
		coreGameData.listOfItemInventory.Clear();
		for (int i = 0; i < arraySlotsOfBelt.Length; i++)
		{
			RemoveItemFromBeltSlot(i);
		}
	}

	public void AddItemToBeltSlot(int slotID, CountItemsQuantity thisItem)
	{
		coreGameData.arrayBeltSlots[slotID] = thisItem;
		if (CheckRefCoreUI())
		{
			coreUI.UpdateItemSlotsDisplay();
		}
		ACC_RefreshListActiveAccessories();
		if ((ACC_IsItemAccessory(thisItem.id) && coreGameData.intIndexOfSlotCurrentActiveItem == slotID) || coreGameData.arrayBeltSlots[coreGameData.intIndexOfSlotCurrentActiveItem] == null || coreGameData.arrayBeltSlots[coreGameData.intIndexOfSlotCurrentActiveItem].quantity <= 0)
		{
			CycleActiveItem(isForwards: true);
		}
	}

	public void RemoveItemFromBeltSlot(int slotID)
	{
		coreGameData.arrayBeltSlots[slotID] = null;
		if (CheckRefCoreUI())
		{
			coreUI.UpdateItemSlotsDisplay();
		}
		ACC_RefreshListActiveAccessories();
		if (coreGameData.intIndexOfSlotCurrentActiveItem == slotID)
		{
			CycleActiveItem(isForwards: true);
		}
	}

	private void CheckBeltItemCountUpdate(int itemID, bool isItemAdded)
	{
		bool flag = false;
		for (int i = 0; i < coreGameData.arrayBeltSlots.Length; i++)
		{
			if (coreGameData.arrayBeltSlots[i] != null && coreGameData.arrayBeltSlots[i].id == itemID)
			{
				if (isItemAdded)
				{
					coreGameData.arrayBeltSlots[i].quantity++;
				}
				else
				{
					coreGameData.arrayBeltSlots[i].quantity--;
				}
				if (coreGameData.arrayBeltSlots[i].quantity <= 0)
				{
					coreGameData.arrayBeltSlots[i] = null;
				}
				flag = true;
				break;
			}
		}
		if (flag && CheckRefCoreUI())
		{
			coreUI.UpdateItemSlotsDisplay();
		}
	}

	public void RefreshBeltItemVisualSlotsDisplay(Image[] imgUI, Text[] txtUI, IconInventoryList refScript)
	{
		try
		{
			string text = "";
			for (int i = 0; i < coreGameData.arrayBeltSlots.Length; i++)
			{
				if (CheckBeltSlotValidity(i))
				{
					int id = coreGameData.arrayBeltSlots[i].id;
					text = ((!ACC_IsItemAccessory(id)) ? coreGameData.arrayBeltSlots[i].quantity.ToString() : "");
					imgUI[i].sprite = refScript.GetIconFromID(id);
					imgUI[i].color = Color.white;
					txtUI[i].text = text;
				}
				else
				{
					imgUI[i].color = new Color(1f, 1f, 1f, 0f);
					imgUI[i].sprite = null;
					txtUI[i].text = "";
				}
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public void RefreshBeltItemVisualSlotsDisplay(Image imgUI, Image itemNext, Image itemPrevious, Text txtUI, IconInventoryList refScript, bool returnBlankForAccessories = false)
	{
		string text = "";
		int num = 0;
		int num2 = 0;
		if (CheckBeltSlotValidity(indexCurrentBeltSlotActive) && (!returnBlankForAccessories || !ACC_IsItemAccessory(num)))
		{
			num = coreGameData.arrayBeltSlots[indexCurrentBeltSlotActive].id;
			text = ((!ACC_IsItemAccessory(num)) ? coreGameData.arrayBeltSlots[indexCurrentBeltSlotActive].quantity.ToString() : "");
			imgUI.sprite = refScript.GetIconFromID(num);
			imgUI.color = Color.white;
			txtUI.text = text;
			txtUI.transform.parent.GetComponent<Text>().text = txtUI.text;
		}
		else
		{
			imgUI.color = new Color(1f, 1f, 1f, 0f);
			imgUI.sprite = null;
			txtUI.text = "";
			txtUI.transform.parent.GetComponent<Text>().text = txtUI.text;
		}
		if (returnBlankForAccessories && ACC_IsItemAccessory(num))
		{
			imgUI.color = new Color(1f, 1f, 1f, 0f);
			imgUI.sprite = null;
			txtUI.text = "";
			txtUI.transform.parent.GetComponent<Text>().text = txtUI.text;
		}
		num2 = ITEM_CycleBeltSlotReturnNextUseable(isForwardNext: true);
		if (CheckBeltSlotValidity(num2) && (!returnBlankForAccessories || !ACC_IsItemAccessory(coreGameData.arrayBeltSlots[num2].id)))
		{
			itemNext.sprite = refScript.GetIconFromID(coreGameData.arrayBeltSlots[num2].id);
			itemNext.color = Color.white;
		}
		else
		{
			itemNext.sprite = null;
			itemNext.color = new Color(1f, 1f, 1f, 0f);
		}
		num2 = ITEM_CycleBeltSlotReturnNextUseable(isForwardNext: false);
		if (CheckBeltSlotValidity(num2) && (!returnBlankForAccessories || !ACC_IsItemAccessory(coreGameData.arrayBeltSlots[num2].id)))
		{
			itemPrevious.sprite = refScript.GetIconFromID(coreGameData.arrayBeltSlots[num2].id);
			itemPrevious.color = Color.white;
		}
		else
		{
			itemPrevious.sprite = null;
			itemPrevious.color = new Color(1f, 1f, 1f, 0f);
		}
	}

	public bool CheckBeltSlotValidity(int indexActiveSlot)
	{
		if (coreGameData.arrayBeltSlots[indexActiveSlot] != null && coreGameData.arrayBeltSlots[indexActiveSlot].id != 0 && coreGameData.arrayBeltSlots[indexActiveSlot].quantity > 0)
		{
			return true;
		}
		return false;
	}

	private void VerifyAutoItemEquipOnPickup(int thisItemID)
	{
		int emptySlot = 0;
		if (!IsItemIsAlreadyEquiped(thisItemID) && ReturnTheFirstEmptyBeltSlot(out emptySlot))
		{
			CountItemsQuantity countItemsQuantity = new CountItemsQuantity();
			countItemsQuantity.id = thisItemID;
			countItemsQuantity.quantity = coreGameData.listOfItemInventory.FindAll((ItemInfo x) => x.id == thisItemID).Count;
			AddItemToBeltSlot(emptySlot, countItemsQuantity);
		}
	}

	private bool IsItemIsAlreadyEquiped(int itemIdToCheck)
	{
		for (int i = 0; i < coreGameData.arrayBeltSlots.Length; i++)
		{
			if (coreGameData.arrayBeltSlots[i] != null && coreGameData.arrayBeltSlots[i].id == itemIdToCheck)
			{
				return true;
			}
		}
		return false;
	}

	private bool ReturnTheFirstEmptyBeltSlot(out int emptySlot)
	{
		emptySlot = 0;
		for (int i = 0; i < coreGameData.arrayBeltSlots.Length; i++)
		{
			if (coreGameData.arrayBeltSlots[i] == null || coreGameData.arrayBeltSlots[i].id == 0)
			{
				emptySlot = i;
				return true;
			}
		}
		return false;
	}

	public bool ACC_Check(AccessoryBehavior thisEquipedAccessoryBehavior)
	{
		if (_accBehavior.Exists((AccessoryBehavior x) => x == thisEquipedAccessoryBehavior))
		{
			return true;
		}
		return false;
	}

	private bool ACC_IsItemAccessory(int itemID)
	{
		CollectibleType collectibleType = ITEM_GetCollectibleType(itemID);
		if ((uint)(collectibleType - 17) <= 4u)
		{
			return true;
		}
		return false;
	}

	public void ACC_RefreshListActiveAccessories()
	{
		_accBehavior.Clear();
		_accModifiers = new TotalAccessoryEffect();
		for (int i = 0; i < coreGameData.arrayBeltSlots.Length; i++)
		{
			if (coreGameData.arrayBeltSlots[i] != null && ACC_IsItemAccessory(coreGameData.arrayBeltSlots[i].id))
			{
				ACC_AddBehaviorToShortList(coreGameData.arrayBeltSlots[i].id);
			}
		}
		if (CheckRefMenuBehaviour())
		{
			_refMenuScript.InventoryTextRefreshAccConsolidateStatEffect();
		}
		ACC_HandleCalculationAlteredPlayerStats();
		SetNewBasicAttackVariation();
	}

	private void ACC_HandleCalculationAlteredPlayerStats()
	{
		_accModifiers.HP_RESULT = Mathf.RoundToInt(ACC_DoMathItemAccStats(coreGameData.intPlayerMaxHealth, _accModifiers.HP_flat, _accModifiers.HP_percent));
		if (playerCurrentHealth > (float)_accModifiers.HP_RESULT)
		{
			playerCurrentHealth = _accModifiers.HP_RESULT;
		}
		_accModifiers.SP_RESULT = ACC_DoMathItemAccStats(coreGameData.intPlayerMaxSpirit, _accModifiers.SP_flat, _accModifiers.SP_percent);
		if (playerCurrentSpirit > _accModifiers.SP_RESULT)
		{
			playerCurrentSpirit = _accModifiers.SP_RESULT;
		}
		_accModifiers.ATK_RESULT = Mathf.RoundToInt(ACC_DoMathItemAccStats(coreGameData.intPlayerCurrentBasicAtackPower, _accModifiers.ATK_flat, _accModifiers.ATK_percent));
		_accModifiers.DEF_RESULT = Mathf.RoundToInt(ACC_DoMathItemAccStats(coreGameData.intPlayerCurrentBasicDefensePower, _accModifiers.DEF_flat, _accModifiers.DEF_percent));
		if (!ACC_Check(AccessoryBehavior.cheapDiscount) && discountShopTotal > 0)
		{
			discountShopTotal = 0;
			_discountShopAcumulation = 0f;
		}
	}

	public float ACC_DoMathItemAccStats(float baseValue, float flat, float percent)
	{
		float num = Mathf.Ceil((baseValue + flat) * MATH_NormalizeFloatPercent(percent, zeroReturnOne: true));
		if (num < 1f)
		{
			num = 1f;
		}
		return Mathf.Round(num);
	}

	public string ACC_TextRefreshConsolidateStatChanges()
	{
		string text = "";
		string addLineBreak = "\r\n";
		text += ACC_StatStringAssembler(_accModifiers.HP_flat, 65, addLineBreak, "", text);
		text += ACC_StatStringAssembler(_accModifiers.HP_percent, 65, addLineBreak, "%", text);
		text += ACC_StatStringAssembler(_accModifiers.SP_flat, 66, addLineBreak, "", text);
		text += ACC_StatStringAssembler(_accModifiers.SP_percent, 66, addLineBreak, "%", text);
		text += ACC_StatStringAssembler(_accModifiers.ATK_flat, 67, addLineBreak, "", text);
		text += ACC_StatStringAssembler(_accModifiers.ATK_percent, 67, addLineBreak, "%", text);
		text += ACC_StatStringAssembler(_accModifiers.DEF_flat, 68, addLineBreak, "", text);
		return text + ACC_StatStringAssembler(_accModifiers.DEF_percent, 68, addLineBreak, "%", text);
	}

	private void ACC_AddBehaviorToShortList(int idAcc)
	{
		switch (idAcc)
		{
		case 500:
			_accModifiers.HP_flat += 50;
			break;
		case 501:
			_accModifiers.HP_flat += 100;
			break;
		case 502:
			_accModifiers.HP_flat += 200;
			break;
		case 503:
			_accModifiers.SP_flat += 50;
			break;
		case 504:
			_accModifiers.SP_flat += 100;
			break;
		case 505:
			_accModifiers.SP_flat += 200;
			break;
		case 506:
			_accModifiers.ATK_flat += 20;
			break;
		case 507:
			_accModifiers.ATK_flat += 40;
			break;
		case 508:
			_accModifiers.ATK_flat += 80;
			break;
		case 509:
			_accModifiers.DEF_flat += 20;
			break;
		case 510:
			_accModifiers.DEF_flat += 40;
			break;
		case 511:
			_accModifiers.DEF_flat += 80;
			break;
		case 550:
			_accBehavior.Add(AccessoryBehavior.WeaponWhip);
			_accModifiers.HP_percent += -10;
			_accModifiers.DEF_percent += -20;
			break;
		case 551:
			_accBehavior.Add(AccessoryBehavior.CrouchShield);
			_accModifiers.SP_percent += -20;
			break;
		case 552:
			_accBehavior.Add(AccessoryBehavior.cheapDiscount);
			break;
		case 553:
			_accBehavior.Add(AccessoryBehavior.cursedHorned);
			break;
		case 554:
			_accBehavior.Add(AccessoryBehavior.Chastity);
			_accModifiers.HP_percent += -5;
			_accModifiers.SP_percent += -5;
			_accModifiers.ATK_percent += -5;
			_accModifiers.DEF_percent += -5;
			break;
		case 555:
			_accBehavior.Add(AccessoryBehavior.JoyDevice);
			_accModifiers.HP_percent += -5;
			_accModifiers.SP_percent += -5;
			_accModifiers.ATK_percent += -5;
			_accModifiers.DEF_percent += -5;
			break;
		case 600:
			_accBehavior.Add(AccessoryBehavior.DashPhaseAttack);
			_accModifiers.DASH_speed_mult += -10f;
			_accModifiers.DASH_cost_flat += 20;
			break;
		case 601:
			_accBehavior.Add(AccessoryBehavior.SKILLEnhancePowerSlash);
			_accModifiers.SLASH_cost_percent += 50;
			break;
		case 602:
			_accBehavior.Add(AccessoryBehavior.SKILLEnhanceDefenseShield);
			_accModifiers.SHIELD_cost_percent += 20;
			break;
		case 603:
			_accBehavior.Add(AccessoryBehavior.SKILLEnhancedSlidingTackle);
			break;
		case 604:
			_accBehavior.Add(AccessoryBehavior.SKILLEnhancedFlameSword);
			break;
		case 650:
			_accBehavior.Add(AccessoryBehavior.glassEarrings);
			_accModifiers.ATK_flat += 1000;
			_accModifiers.HP_percent += -999;
			break;
		case 651:
			_accBehavior.Add(AccessoryBehavior.SwordSlashProjectile);
			_accModifiers.HP_flat += -100;
			break;
		case 652:
			_accBehavior.Add(AccessoryBehavior.masterDash);
			break;
		case 653:
			_accBehavior.Add(AccessoryBehavior.BladeStun);
			_accModifiers.HP_flat += 25;
			_accModifiers.ATK_flat += 10;
			_accModifiers.DEF_flat += 10;
			break;
		case 700:
			_accBehavior.Add(AccessoryBehavior.SPLow25Stun);
			_accModifiers.SP_percent += 50;
			break;
		case 701:
			_accBehavior.Add(AccessoryBehavior.DoubleJump);
			_accModifiers.ATK_flat += -25;
			_accModifiers.DEF_flat += -25;
			break;
		case 702:
			_accBehavior.Add(AccessoryBehavior.EroticDancer);
			_accModifiers.SP_flat += -20;
			_accModifiers.DEF_flat += 10;
			break;
		case 703:
			_accBehavior.Add(AccessoryBehavior.HeadSuccubus);
			_accModifiers.DEF_flat += -20;
			_accModifiers.ATK_flat += 10;
			break;
		case 704:
			_accBehavior.Add(AccessoryBehavior.HairFertility);
			ACC_HACK_ITEM_FERTILITY_STATS();
			break;
		case 705:
			_accBehavior.Add(AccessoryBehavior.DaggerSpecialist);
			break;
		}
	}

	private string ACC_StatStringAssembler(float value, int translationID, string addLineBreak, string addPercent, string currentConsolidated)
	{
		string text = "";
		bool flag = true;
		string text2 = "<color=#00FF14>";
		string text3 = "</color>";
		if (value == 0f)
		{
			return text;
		}
		if (value < 0f)
		{
			flag = false;
			text2 = "<color=red>";
		}
		if (currentConsolidated != "")
		{
			text += addLineBreak;
		}
		text = text + GetTranslatedStringUI(translationID) + ": ";
		if (flag)
		{
			text = text + text2 + "+" + text3;
		}
		return text + text2 + value + text3 + addPercent;
	}

	private CollectibleType ITEM_GetCollectibleType(int itemID)
	{
		return (CollectibleType)(clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == itemID)?.type ?? 16);
	}

	public bool CanAddThisSkill(int itemID)
	{
		switch (itemID)
		{
		case 50:
			if (!coreGameData.skillFlags.hasDash)
			{
				coreGameData.skillFlags.hasDash = true;
				return true;
			}
			break;
		case 51:
			if (!coreGameData.skillFlags.hasSlidingTackle)
			{
				coreGameData.skillFlags.hasSlidingTackle = true;
				return true;
			}
			break;
		case 52:
			if (!coreGameData.skillFlags.hasShield)
			{
				coreGameData.skillFlags.hasShield = true;
				return true;
			}
			break;
		case 53:
			if (!coreGameData.skillFlags.hasFlameSword)
			{
				coreGameData.skillFlags.hasFlameSword = true;
				return true;
			}
			break;
		case 54:
			if (!coreGameData.skillFlags.hasForwardSlash)
			{
				coreGameData.skillFlags.hasForwardSlash = true;
				return true;
			}
			break;
		case 55:
			if (!coreGameData.skillFlags.hasGroundPound)
			{
				coreGameData.skillFlags.hasGroundPound = true;
				return true;
			}
			break;
		case 56:
			if (!coreGameData.skillFlags.hasDrillDash)
			{
				coreGameData.skillFlags.hasDrillDash = true;
				return true;
			}
			break;
		}
		return false;
	}

	public bool AddThisPowerUp(CollectibleType powerupType)
	{
		bool result = false;
		switch (powerupType)
		{
		case CollectibleType.HealthUpgrade:
			coreGameData.intPlayerMaxHealth += clsItemDatabase.defaultPowerUpHealth;
			coreGameData.intPlayerCurrentHealth += clsItemDatabase.defaultPowerUpHealth;
			result = true;
			break;
		case CollectibleType.SpiritUpgrade:
			coreGameData.intPlayerMaxSpirit += clsItemDatabase.defaultPowerUpSpirit;
			coreGameData.intPlayerCurrentSpirit += clsItemDatabase.defaultPowerUpSpirit;
			result = true;
			break;
		case CollectibleType.AttackUpgrade:
			coreGameData.intPlayerCurrentBasicAtackPower += clsItemDatabase.defaultPowerUpAttack;
			result = true;
			break;
		case CollectibleType.DefenseUpgrade:
			coreGameData.intPlayerCurrentBasicDefensePower += clsItemDatabase.defaultPowerUpDefense;
			result = true;
			break;
		}
		ACC_HandleCalculationAlteredPlayerStats();
		return result;
	}

	public void ValidatePowerupsAfterLoad()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		for (int i = 0; i < coreGameData.listOfItemInventory.Count; i++)
		{
			num5 = coreGameData.listOfItemInventory[i].type;
			if (num5 >= 4 && num5 <= 7)
			{
				switch (coreGameData.listOfItemInventory[i].id)
				{
				case 7:
					num4++;
					break;
				case 10:
					num++;
					break;
				case 11:
					num2++;
					break;
				case 12:
					num3++;
					break;
				}
			}
		}
		if (num > 0)
		{
			num = coreGameData.defaultMaxHealth + clsItemDatabase.defaultPowerUpHealth * num;
			if (coreGameData.intPlayerMaxHealth < (float)num)
			{
				coreGameData.intPlayerMaxHealth = num;
			}
		}
		if (num2 > 0)
		{
			num2 = coreGameData.defaultMaxSpirit + clsItemDatabase.defaultPowerUpSpirit * num2;
			if (coreGameData.intPlayerMaxSpirit < (float)num2)
			{
				coreGameData.intPlayerMaxSpirit = num2;
			}
		}
		if (num4 > 0)
		{
			num4 = coreGameData.defaultBaseAttack + clsItemDatabase.defaultPowerUpAttack * num4;
			if (coreGameData.intPlayerCurrentBasicAtackPower < num4)
			{
				coreGameData.intPlayerCurrentBasicAtackPower = num4;
			}
		}
		if (num3 > 0)
		{
			num3 = coreGameData.defaultBaseDefense + clsItemDatabase.defaultPowerUpDefense * num3;
			if (coreGameData.intPlayerCurrentBasicDefensePower < num3)
			{
				coreGameData.intPlayerCurrentBasicDefensePower = num3;
			}
		}
	}

	public void EquipmentAddStarting()
	{
		Vector2 itemPosition = new Vector2(9999f, 9999f);
		AddItemToPlayerInventory(900, strCurrentActiveSceneName, itemPosition);
		AddItemToPlayerInventory(901, strCurrentActiveSceneName, itemPosition);
		AddItemToPlayerInventory(902, strCurrentActiveSceneName, itemPosition);
		for (int i = 0; i < 3; i++)
		{
			AddItemToPlayerInventory(904, strCurrentActiveSceneName, itemPosition);
		}
		for (int j = 0; j < 3; j++)
		{
			AddItemToPlayerInventory(905, strCurrentActiveSceneName, itemPosition);
		}
		for (int k = 0; k < 2; k++)
		{
			AddItemToPlayerInventory(906, strCurrentActiveSceneName, itemPosition);
		}
		AddItemToPlayerInventory(907, strCurrentActiveSceneName, itemPosition);
	}

	public void ValidatePlayerInventoryItemTypes()
	{
		ItemInfo itemInfo = new ItemInfo();
		int i;
		for (i = 0; i < coreGameData.listOfItemInventory.Count; i++)
		{
			itemInfo = clsItemDatabase.listOfItemDatabase.Find((ItemInfo x) => x.id == coreGameData.listOfItemInventory[i].id);
			if (itemInfo.id == coreGameData.listOfItemInventory[i].id && itemInfo.type != coreGameData.listOfItemInventory[i].type)
			{
				coreGameData.listOfItemInventory[i].type = itemInfo.type;
			}
		}
	}

	private void ACC_HACK_ITEM_FERTILITY_STATS()
	{
		int num = 0;
		if (ACC_Check(AccessoryBehavior.HairFertilityStatsHack))
		{
			_accBehavior.Remove(AccessoryBehavior.HairFertilityStatsHack);
		}
		switch (playerCurrentDressState)
		{
		default:
			num = -30;
			break;
		case 1:
			num = -15;
			break;
		case 2:
			num = 20;
			break;
		}
		_accBehavior.Add(AccessoryBehavior.HairFertilityStatsHack);
		_accModifiers.HP_percent += num;
		_accModifiers.SP_percent += num;
		_accModifiers.ATK_percent += num;
		_accModifiers.DEF_percent += num;
	}

	public void ToggleSpecialMenuNPC(NPCType thisNPC, bool state)
	{
		if (!CheckRefMenuBehaviour())
		{
			Debug.Log("Error getting reference to menu!");
			return;
		}
		_isDisplayingSpecialMenuNPC = state;
		_refMenuScript.NPC_ToggleMainMenu(state);
	}

	public bool HasSeenReadNPCMenuDialogueAdd(NPCType thisNPC, int idDiag, bool doAdd = false)
	{
		switch (thisNPC)
		{
		case NPCType.FarinaSupport:
			if (coreGameData.listSeenDialogueFarina.Exists((ClassNPCTalkDiag x) => x.idDialogue == idDiag))
			{
				return true;
			}
			if (doAdd)
			{
				coreGameData.listSeenDialogueFarina.Add(new ClassNPCTalkDiag
				{
					idDialogue = idDiag
				});
			}
			break;
		case NPCType.NeneShop:
			if (coreGameData.listSeenDialogueNene.Exists((ClassNPCTalkDiag x) => x.idDialogue == idDiag))
			{
				return true;
			}
			if (doAdd)
			{
				coreGameData.listSeenDialogueNene.Add(new ClassNPCTalkDiag
				{
					idDialogue = idDiag
				});
			}
			break;
		}
		return false;
	}

	public void AddDelayOnStartNPCTalk(float wait)
	{
		if (CheckRefPlayer())
		{
			_refPlayerScript.AddDelayNPCTalk(wait);
		}
	}

	public bool TryToBuyItemFromShop(int itemID, string itemName, int itemType, int itemPrice, int itemStock, int contentChildIndex)
	{
		if (discountShopTotal > 0)
		{
			itemPrice -= Mathf.RoundToInt(MATH_ResultValueOfTotalPercentile(discountShopTotal, itemPrice));
		}
		if (itemID == 9999)
		{
			return false;
		}
		if (itemStock <= 0)
		{
			DisplaySystemMessageUI(0, 171, 1.5f);
		}
		else
		{
			if (playerCurrentEssenceAmount >= itemPrice)
			{
				AddEssenceToPlayerAmount(-itemPrice);
				if (itemStock != constShopStockInfinite && itemStock > 0)
				{
					ShopRegisterUniquePurchaseOnLog(itemID);
				}
				ReLoadShopStockComparedToPurchaseLog();
				ShopCheckCornerCasesForItemAdditions(itemID, itemType);
				AddItemToPlayerInventory(itemID, "NeneShopPurchase", Vector2.zero);
				_updateShopListAfterPurchase = true;
				_lastChildIndexShopSlotAfterPurchase = contentChildIndex;
				if (CheckRefMenuBehaviour())
				{
					_refMenuScript.NPC_StartShopEffectAfterBought(itemPrice.ToString(), 1.5f);
				}
				DisplaySystemMessageUI(123456, 0, 2f, itemName);
				return true;
			}
			if (CheckRefMenuBehaviour())
			{
				_refMenuScript.NPC_StartShopEffectAfterBought(GetTranslatedStringUI(170), 2f);
			}
		}
		return false;
	}

	private void ShopCheckCornerCasesForItemAdditions(int itemID, int itemType)
	{
		AddThisPowerUp((CollectibleType)itemType);
	}

	private void ReLoadShopStockComparedToPurchaseLog()
	{
		int num = 0;
		_listNeneShopState = coreGameData.GetDefaultStateForNeneShop();
		foreach (LogPurchase item in coreGameData.logPurchase)
		{
			if (_listNeneShopState.Exists((ItemInfo x) => x.id == item.itemID))
			{
				num = _listNeneShopState.FindIndex((ItemInfo x) => x.id == item.itemID);
				_listNeneShopState[num].shopStock -= item.qtyPurchased;
			}
		}
	}

	private void ShopRegisterUniquePurchaseOnLog(int itemID)
	{
		int num = 0;
		if (coreGameData.logPurchase.Exists((LogPurchase x) => x.itemID == itemID))
		{
			num = coreGameData.logPurchase.FindIndex((LogPurchase x) => x.itemID == itemID);
			coreGameData.logPurchase[num].qtyPurchased++;
		}
		else
		{
			coreGameData.logPurchase.Add(new LogPurchase
			{
				itemID = itemID,
				qtyPurchased = 1
			});
		}
	}

	public void PlayMusicTheme(MusicTheme whichMusic, bool doFadeIn, float fadeTime, bool doLoop)
	{
		if (CheckRefAudioSystem())
		{
			refAudioSystem.PlayMusicTheme(whichMusic, doFadeIn, fadeTime, doLoop);
		}
		else
		{
			Debug.Log("Could not get the audio system reference object! Make sure the audio system prefab is instantiated in the scene!");
		}
	}

	public void StopMusicTheme(bool doFadeOut, float fadeTime)
	{
		if (CheckRefAudioSystem())
		{
			refAudioSystem.StopMusicTheme(doFadeOut, fadeTime);
		}
		else
		{
			Debug.Log("Could not get the audio system reference object! Make sure the audio system prefab is instantiated in the scene!");
		}
	}

	public void ToggleEroGrabbedTheme(bool toggleON)
	{
		if (CheckRefAudioSystem())
		{
			if (toggleON)
			{
				refAudioSystem.PlayEroGrabbedTheme();
			}
			else
			{
				refAudioSystem.StopEroGrabbedTheme();
			}
		}
	}

	public void ToggleMusicMute(bool isMute, bool isFade, float fadeDuration)
	{
		if (CheckRefAudioSystem())
		{
			refAudioSystem.ToggleMainMusicMute(isMute, isFade, fadeDuration);
		}
	}

	public void PlaySFX(SFXBasic1 whichSFX, int chancePercent, bool checkForDuplicity, Vector2 senderPosition, float pitchRangeLow, float pitchRangeHigh, string groupByThisName = "", bool applyMuffledEffect = false, float offsetVolPrct = 0f, bool dontPlayIfGroupAlreadyPlaying = false)
	{
		if (CheckStuffBeforePlayingSFX(senderPosition, chancePercent) && CheckRefAudioSystem())
		{
			refAudioSystem.SetupPlaySFXBasic(whichSFX, offsetVolPrct, checkForDuplicity, pitchRangeLow, pitchRangeHigh, groupByThisName, applyMuffledEffect, dontPlayIfGroupAlreadyPlaying);
		}
	}

	public void PlaySFX(SFXBasic2 whichSFX, int chancePercent, bool checkForDuplicity, Vector2 senderPosition, float pitchRangeLow, float pitchRangeHigh, string groupByThisName = "", bool applyMuffledEffect = false, float offsetVolPrct = 0f, bool dontPlayIfGroupAlreadyPlaying = false)
	{
		if (CheckStuffBeforePlayingSFX(senderPosition, chancePercent) && CheckRefAudioSystem())
		{
			refAudioSystem.SetupPlaySFXBasic(whichSFX, offsetVolPrct, checkForDuplicity, pitchRangeLow, pitchRangeHigh, groupByThisName, applyMuffledEffect, dontPlayIfGroupAlreadyPlaying);
		}
	}

	public void PlaySFXMonsterSounds(MonsterSounds whichSFX, int chancePercent, bool checkForDuplicity, Vector2 senderPosition, float pitchRangeLow, float pitchRangeHigh, string groupByThisName = "", bool applyMuffledEffect = false, float offsetVolPrct = 0f, bool dontPlayIfGroupAlreadyPlaying = false)
	{
		if (CheckStuffBeforePlayingSFX(senderPosition, chancePercent) && CheckRefAudioSystem())
		{
			refAudioSystem.SetupPlaySFXMonsterSounds(whichSFX, offsetVolPrct, checkForDuplicity, pitchRangeLow, pitchRangeHigh, groupByThisName, applyMuffledEffect, dontPlayIfGroupAlreadyPlaying);
		}
	}

	public void PlaySFXEro(EroSound whichSFX, int chancePercent, bool checkForDuplicity, Vector2 senderPosition, float pitchRangeLow, float pitchRangeHigh, string groupByThisName = "", bool applyMuffledEffect = false, float offsetVolPrct = 0f, bool dontPlayIfGroupAlreadyPlaying = false)
	{
		if (CheckStuffBeforePlayingSFX(senderPosition, chancePercent) && CheckRefAudioSystem())
		{
			refAudioSystem.SetupPlaySFXEro(whichSFX, offsetVolPrct, checkForDuplicity, pitchRangeLow, pitchRangeHigh, groupByThisName, applyMuffledEffect, dontPlayIfGroupAlreadyPlaying);
		}
	}

	private void PlaySFXMayaVoice(MayaVoice whichSFX, int chancePercent, bool checkForDuplicity, Vector2 senderPosition, float pitchRangeLow, float pitchRangeHigh, string groupByThisName = "", bool applyMuffledEffect = false, float offsetVolPrct = 0f, bool dontPlayIfGroupAlreadyPlaying = false)
	{
		if (CheckStuffBeforePlayingSFX(senderPosition, chancePercent) && CheckRefAudioSystem())
		{
			refAudioSystem.SetupPlaySFXMayaVoice(whichSFX, offsetVolPrct, checkForDuplicity, pitchRangeLow, pitchRangeHigh, groupByThisName, applyMuffledEffect, dontPlayIfGroupAlreadyPlaying);
		}
	}

	private void PlaySFXMayaVoice(AudioClip directClip, int chancePercent, bool checkForDuplicity, Vector2 senderPosition, float pitchRangeLow, float pitchRangeHigh, string groupByThisName = "", bool applyMuffledEffect = false, float offsetVolPrct = 0f, bool dontPlayIfGroupAlreadyPlaying = false)
	{
		if (CheckStuffBeforePlayingSFX(senderPosition, chancePercent) && CheckRefAudioSystem())
		{
			refAudioSystem.SetupPlaySFXMayaVoice(directClip, offsetVolPrct, checkForDuplicity, pitchRangeLow, pitchRangeHigh, groupByThisName, applyMuffledEffect, dontPlayIfGroupAlreadyPlaying);
		}
	}

	public void PlaySFXGeneric(string genericName, int idSFX, int chancePercent, bool checkForDuplicity, Vector2 senderPosition, float pitchRangeLow, float pitchRangeHigh, string volumeType, string groupByThisName = "", bool applyMuffledEffect = false, float offsetVolPrct = 0f, bool dontPlayIfGroupAlreadyPlaying = false)
	{
		if (CheckStuffBeforePlayingSFX(senderPosition, chancePercent) && CheckRefAudioSystem())
		{
			refAudioSystem.SetupPlaySFXGeneric(genericName, idSFX, checkForDuplicity, pitchRangeLow, pitchRangeHigh, volumeType, groupByThisName, applyMuffledEffect, dontPlayIfGroupAlreadyPlaying);
		}
	}

	public void PlayDiagLetters(SFXBasic1 whichSFX, int chancePercent, bool checkForDuplicity, Vector2 senderPosition, float pitchRangeLow, float pitchRangeHigh, string groupByThisName = "", bool applyMuffledEffect = false, float offsetVolPrct = 0f, bool dontPlayIfGroupAlreadyPlaying = false)
	{
		if (CheckStuffBeforePlayingSFX(senderPosition, chancePercent) && CheckRefAudioSystem())
		{
			refAudioSystem.SetupPlayDiagLetter(whichSFX, offsetVolPrct, checkForDuplicity, pitchRangeLow, pitchRangeHigh, groupByThisName, applyMuffledEffect, dontPlayIfGroupAlreadyPlaying);
		}
	}

	public void StopAllSFXCurrentylPlaying()
	{
		if (CheckRefAudioSystem() && (_galleryEnemy == null || !_galleryEnemy.isRedirectToGalleryView))
		{
			refAudioSystem.StopAllSFXAudioSources();
		}
	}

	public void StopSFXBasic2(SFXBasic2 thisType)
	{
		if (CheckRefAudioSystem())
		{
			refAudioSystem.StopSFX(thisType);
		}
	}

	private bool CheckStuffBeforePlayingSFX(Vector2 pos, int chancePerc)
	{
		if (_isSceneTransitioning)
		{
			return false;
		}
		if (pos != Vector2.zero && !CheckProximityToPlayerForSound(pos))
		{
			return false;
		}
		if (chancePerc > 0 && MATH_RNG_INT(1, 100) > chancePerc)
		{
			return false;
		}
		return true;
	}

	public void CheckStageMusic()
	{
		MusicTheme whichMusic = MusicTheme.Null;
		switch (strCurrentActiveSceneName)
		{
		case "Stage2-Beach":
			whichMusic = MusicTheme.Beach1;
			break;
		case "Stage2-Beach-Part2":
		case "Stage2-Beach-PartBoss":
			whichMusic = MusicTheme.Beach2;
			break;
		case "Stage0-Dungeon":
			whichMusic = MusicTheme.HubCastle1;
			break;
		case "Stage0-xActualDungeon":
			whichMusic = MusicTheme.Dungeon;
			break;
		case "Stage0-UpperFloors":
			whichMusic = MusicTheme.CastleUpperFLoor;
			break;
		case "Stage1-Forest":
		case "Stage1-Forest-part2":
		case "Stage1-Forest-Boss":
			whichMusic = MusicTheme.Forest1;
			break;
		case "Stage3-Valley":
		case "Stage3-Valley-part2":
		case "Stage3-Valley-partBoss":
			whichMusic = MusicTheme.Valley1;
			break;
		case "Stage4-City":
		case "Stage4-City-part2":
			whichMusic = MusicTheme.City1;
			break;
		case "Stage5-Cave1":
			whichMusic = MusicTheme.Cave1;
			break;
		case "Stage5-Cave2":
			whichMusic = MusicTheme.Cave2;
			break;
		case "Stage5-Cave3":
			whichMusic = MusicTheme.Cave3;
			break;
		case "Stage5-Cave4BW":
			whichMusic = MusicTheme.DarkAmbience1;
			break;
		case "Stage6-1-Swamp":
			whichMusic = MusicTheme.Swamp1;
			break;
		case "Stage6-2-Gardens":
			whichMusic = MusicTheme.Swamp1;
			break;
		case "Stage7-HighTech_A":
		case "DebugRoom":
			whichMusic = MusicTheme.HighTech1;
			break;
		case "Stage7-HighTech_C":
			whichMusic = MusicTheme.HighTech2;
			break;
		case "AnimGallery":
			whichMusic = MusicTheme.IntroStory;
			break;
		}
		PlayMusicTheme(whichMusic, doFadeIn: true, 3f, doLoop: true);
		if (playerDidTeleport)
		{
			PlaySFX(SFXBasic1.playerTP_DOWN, 0, checkForDuplicity: true, base.transform.position, 0f, 0f);
		}
	}

	public void UpdateMusicVolumeRealTime()
	{
		if (CheckRefAudioSystem())
		{
			refAudioSystem.RealTimeUpdateVolumeOfMusic();
		}
	}

	public void ToggleMuteAudio(AudioType audioType, bool applyMute, float timerToAutoRevert = 0f)
	{
		if (timerToAutoRevert <= 0f)
		{
			PrivateToggleMuteAudio(audioType, applyMute);
			return;
		}
		if (crtToggleMuteAudio != null)
		{
			StopCoroutine(crtToggleMuteAudio);
		}
		crtToggleMuteAudio = StartCoroutine(waitToggleAudioMute(audioType, applyMute, timerToAutoRevert));
	}

	private void PrivateToggleMuteAudio(AudioType audioType, bool state)
	{
		switch (audioType)
		{
		default:
			_muteMusic = state;
			_muteSFX = state;
			_muteVoice = state;
			break;
		case AudioType.Music:
			_muteMusic = state;
			break;
		case AudioType.SFX:
			_muteSFX = state;
			break;
		case AudioType.Voice:
			_muteVoice = state;
			break;
		case AudioType.pairMUSSFX:
			_muteMusic = state;
			_muteSFX = state;
			break;
		case AudioType.pairMUSVOI:
			_muteMusic = state;
			_muteVoice = state;
			break;
		case AudioType.pairSFXVOI:
			_muteSFX = state;
			_muteVoice = state;
			break;
		}
	}

	private IEnumerator waitToggleAudioMute(AudioType audioType, bool state, float wait)
	{
		PrivateToggleMuteAudio(audioType, state);
		yield return new WaitForSeconds(wait);
		PrivateToggleMuteAudio(audioType, !state);
	}

	public void CallReferenceMayaVoiceGC(string sfxName, string logicSender, string overrideGroupName = "", bool muffledEffect = false, bool maleEffect = false, int overrideChancePercent = 0)
	{
		MayaVoice mayaVoice = MayaVoice.Null;
		AudioClip audioClip = null;
		string group = "MayaVoicesGroup";
		bool checkDupli = false;
		float offsetVolP = 0f;
		float pitchL = 0f;
		float pitchH = 0f;
		int chanceP = overrideChancePercent;
		if (overrideGroupName != "")
		{
			group = overrideGroupName;
		}
		sfxName = sfxName.Trim();
		if (sfxName.Length > 3 && sfxName.Substring(0, 4) == "rng-")
		{
			sfxName = sfxName.Remove(0, 4);
			audioClip = MAYA_VOICES_GET_RANGE((MARange)Enum.Parse(typeof(MARange), sfxName));
		}
		else
		{
			mayaVoice = GetMayaVoiceBasicType(sfxName);
		}
		MAYA_VOICE_CHECK_APPLY_SPECIAL_MODS(sfxName, ref group, ref checkDupli, ref offsetVolP, ref pitchL, ref pitchH, ref chanceP);
		if (maleEffect)
		{
			pitchL = 0.8f;
			pitchH = 0.9f;
		}
		if (mayaVoice != MayaVoice.Null)
		{
			PlaySFXMayaVoice(mayaVoice, chanceP, checkDupli, Vector2.zero, pitchL, pitchH, group, muffledEffect, offsetVolP);
		}
		else if (audioClip != null)
		{
			PlaySFXMayaVoice(audioClip, chanceP, checkDupli, Vector2.zero, pitchL, pitchH, group, muffledEffect, offsetVolP);
		}
		else
		{
			Debug.Log("[GAMECORE]CallReferenceMayaVoiceGC() -> Returned NULL for this sfxName: " + sfxName + " from sender: " + logicSender);
		}
	}

	private MayaVoice GetMayaVoiceBasicType(string sentAnimatorName)
	{
		switch (sentAnimatorName)
		{
		case "attack10":
			return MayaVoice.Attack10;
		case "attack16":
			return MayaVoice.Attack16;
		case "hurtPain1":
			return MayaVoice.HurtPain1;
		case "hurtPain2":
			return MayaVoice.HurtPain2;
		case "hurtPain3":
			return MayaVoice.HurtPain3;
		case "hurtPain8":
			return MayaVoice.HurtPain9;
		case "hurtPain9":
			return MayaVoice.HurtPain9;
		case "hurtPain11":
			return MayaVoice.HurtPain11;
		case "hurtPain13":
			return MayaVoice.HurtPain13;
		case "hurtPain14":
			return MayaVoice.HurtPain14;
		case "hurtPain16":
			return MayaVoice.HurtPain16;
		case "gasp1":
			return MayaVoice.Gasp1;
		case "gasp2":
			return MayaVoice.Gasp2;
		case "gasp6":
			return MayaVoice.Gasp6;
		case "gasp8":
			return MayaVoice.Gasp8;
		case "gasp10":
			return MayaVoice.Gasp10;
		case "scream1":
			return MayaVoice.Scream1;
		case "scream2":
			return MayaVoice.Scream2;
		case "scream5":
			return MayaVoice.Scream5;
		case "scream6":
			return MayaVoice.Scream6;
		case "scream7":
			return MayaVoice.Scream7;
		case "scream8":
			return MayaVoice.Scream8;
		case "scream10":
			return MayaVoice.Scream10;
		case "scream11":
			return MayaVoice.Scream11;
		case "scream12":
			return MayaVoice.Scream12;
		case "scream13":
			return MayaVoice.Scream13;
		case "scream14":
			return MayaVoice.Scream14;
		case "scream15":
			return MayaVoice.Scream15;
		case "moanLight8":
			return MayaVoice.MoanLight8;
		case "moanLight9":
			return MayaVoice.MoanLight9;
		case "moanMedium3":
			return MayaVoice.MoanMedium3;
		case "moanMedium4":
			return MayaVoice.MoanMedium4;
		case "moanMedium8":
			return MayaVoice.MoanMedium8;
		case "moanMedium9":
			return MayaVoice.MoanMedium9;
		case "moanMedium11":
			return MayaVoice.MoanMedium11;
		case "moanHeavy2":
			return MayaVoice.MoanHeavy2;
		case "moanHeavy4":
			return MayaVoice.MoanHeavy4;
		case "moanHeavy7":
			return MayaVoice.MoanHeavy7;
		case "moanHeavy8":
			return MayaVoice.MoanHeavy8;
		case "moanHeavy9":
			return MayaVoice.MoanHeavy9;
		case "moanHeavy10":
			return MayaVoice.MoanHeavy10;
		case "moanRough1":
			return MayaVoice.MoanRough1;
		case "moanRough2":
			return MayaVoice.MoanRough2;
		case "moanRough4":
			return MayaVoice.MoanRough4;
		case "moanRough6":
			return MayaVoice.MoanRough6;
		case "moanRough8":
			return MayaVoice.MoanRough8;
		case "moanRough9":
			return MayaVoice.MoanRough9;
		case "moanRough10":
			return MayaVoice.MoanRough10;
		case "climax1":
			return MayaVoice.Climax1;
		case "climax2":
			return MayaVoice.Climax2;
		case "climax3":
			return MayaVoice.Climax3;
		case "climax4":
			return MayaVoice.Climax4;
		case "climax8":
			return MayaVoice.Climax8;
		case "climax9":
			return MayaVoice.Climax9;
		case "climax10":
			return MayaVoice.Climax10;
		case "climax11":
			return MayaVoice.Climax11;
		case "BJHeavy9":
			return MayaVoice.BJHeavy9;
		case "BJHeavy10":
			return MayaVoice.BJHeavy10;
		case "BJHeavy18":
			return MayaVoice.BJHeavy18;
		case "BJHeavy22":
			return MayaVoice.BJHeavy22;
		case "BJHeavy24":
			return MayaVoice.BJHeavy24;
		case "BJHeavy28":
			return MayaVoice.BJHeavy28;
		case "BJHeavy30":
			return MayaVoice.BJHeavy30;
		case "BJHeavy33":
			return MayaVoice.BJHeavy33;
		case "BJClimax8":
			return MayaVoice.BJClimax8;
		case "BJClimax10":
			return MayaVoice.BJClimax10;
		case "BJClimax11":
			return MayaVoice.BJClimax11;
		case "BJClimax13":
			return MayaVoice.BJClimax13;
		case "BJClimax20":
			return MayaVoice.BJClimax20;
		case "BJClimax23":
			return MayaVoice.BJClimax23;
		case "BJClimax24":
			return MayaVoice.BJClimax23;
		case "BJClimax25":
			return MayaVoice.BJClimax25;
		case "BJClimax26":
			return MayaVoice.BJClimax26;
		case "BJClimax27":
			return MayaVoice.BJClimax27;
		case "BJClimax29":
			return MayaVoice.BJClimax29;
		case "BJClimax31":
			return MayaVoice.BJClimax31;
		case "BJClimax32":
			return MayaVoice.BJClimax32;
		case "BJClimax34":
			return MayaVoice.BJClimax34;
		case "BJMisc14":
			return MayaVoice.BJMisc14;
		case "BJMisc16":
			return MayaVoice.BJMisc16;
		case "BJMisc25":
			return MayaVoice.BJMisc25;
		case "BJMisc26":
			return MayaVoice.BJMisc26;
		case "Swallow1":
			return MayaVoice.Swallow1;
		case "Swallow2":
			return MayaVoice.Swallow2;
		case "Swallow6":
			return MayaVoice.Swallow6;
		case "MBBLogo":
			if (MATH_RNG_INT(1, 100) <= 50)
			{
				return MayaVoice.MBBLogo3;
			}
			switch (MATH_RNG_INT(1, 3))
			{
			default:
				return MayaVoice.MBBLogo2;
			case 2:
				return MayaVoice.MBBLogo4;
			case 3:
				return MayaVoice.MBBLogo5;
			}
		default:
			return MayaVoice.Null;
		}
	}

	private AudioClip MAYA_VOICES_GET_RANGE(MARange rangeType)
	{
		AudioClip[] array = null;
		array = refAudioSystem.listOfMayaVoiceGroups.GetVoiceArrayOfGroupType(rangeType);
		if (array != null && array.Length != 0)
		{
			return CHECK_MAYA_VOICE_NO_REPEAT_LOGIC(1, rangeType, array);
		}
		return null;
	}

	private void MAYA_VOICE_CHECK_APPLY_SPECIAL_MODS(string sfxName, ref string group, ref bool checkDupli, ref float offsetVolP, ref float pitchL, ref float pitchH, ref int chanceP)
	{
		switch (sfxName)
		{
		case "ClimaxAfter1":
			pitchL = 1.05f;
			pitchH = 1.05f;
			break;
		case "MayaSlurp1":
			group += "Slurp";
			break;
		case "SwallowRange1":
			group += "Swallow";
			pitchL = 0.9f;
			pitchH = 1.1f;
			break;
		case "ClosedMouthMoan3":
			offsetVolP = -10f;
			break;
		}
		EnemyType enemyType = _enemyGrabbingPlayer;
		if (enemyType != EnemyType.Dogboy)
		{
			return;
		}
		if (!(sfxName == "MoanLight2"))
		{
			if (sfxName == "MiscOpen2")
			{
				chanceP = 35;
			}
		}
		else
		{
			chanceP = 40;
		}
	}

	private AudioClip CHECK_MAYA_VOICE_NO_REPEAT_LOGIC(int maxRepeat, MARange currentGroup, AudioClip[] arrayVoices)
	{
		int num = 0;
		AudioClip audioClip = null;
		MVRPT_TRIM_NO_REPEAT_LIST();
		do
		{
			num++;
			audioClip = arrayVoices[MATH_RNG_INT(0, arrayVoices.Length - 1)];
		}
		while ((!MAYA_VOICE_VALIDATE_REPEAT_RATE(maxRepeat, currentGroup, audioClip) || !MAYA_VOICE_VALIDATE_SOUND_DIVERSITY(maxRepeat + 2, currentGroup, audioClip)) && num < 10);
		_lastVoicePlayed = audioClip;
		return audioClip;
	}

	private bool MAYA_VOICE_VALIDATE_REPEAT_RATE(int maxRepeatRate, MARange voiceGroup, AudioClip currentVoice)
	{
		int num = 0;
		int num2 = 0;
		for (int num3 = _listOfNoRepeatVoice.Count - 1; num3 >= 0; num3--)
		{
			if (_listOfNoRepeatVoice[num3].vGroup == voiceGroup)
			{
				num2++;
				if (currentVoice == _listOfNoRepeatVoice[num3].vClip)
				{
					num++;
				}
			}
			if (num2 >= maxRepeatRate + 1)
			{
				break;
			}
		}
		if (_listOfNoRepeatVoice.Count == 0 || num <= maxRepeatRate)
		{
			_listOfNoRepeatVoice.Add(new GC_NO_REPEAT_VA
			{
				vGroup = voiceGroup,
				vClip = currentVoice
			});
			return true;
		}
		return false;
	}

	private bool MAYA_VOICE_VALIDATE_SOUND_DIVERSITY(int previousClipsToCheck, MARange voiceGroup, AudioClip currentVoice)
	{
		int num = 0;
		_listOfDistinctVoices.Clear();
		int i;
		for (i = _listOfNoRepeatVoice.Count - 2; i >= 0; i--)
		{
			if (_listOfNoRepeatVoice[i].vGroup == voiceGroup)
			{
				num++;
				if (!_listOfDistinctVoices.Exists((AudioClip x) => x == _listOfNoRepeatVoice[i].vClip))
				{
					_listOfDistinctVoices.Add(_listOfNoRepeatVoice[i].vClip);
				}
				if (num >= previousClipsToCheck)
				{
					break;
				}
			}
		}
		if (!_listOfDistinctVoices.Exists((AudioClip x) => x == currentVoice))
		{
			return true;
		}
		if (_listOfDistinctVoices.Count == 1)
		{
			return true;
		}
		if (_listOfDistinctVoices.Count >= previousClipsToCheck)
		{
			return true;
		}
		return false;
	}

	private void MVRPT_TRIM_NO_REPEAT_LIST()
	{
		int num = 0;
		while (_listOfNoRepeatVoice.Count > 20)
		{
			_listOfNoRepeatVoice.RemoveAt(0);
			num++;
			if (num > 10)
			{
				break;
			}
		}
	}

	public void PlayMayaDeathScream(bool ignoreScream = false)
	{
		if (ignoreScream || (playerIsBeingGrabed && _enemyEroPhase > 0))
		{
			_hasPlayedMayaDeathScream = true;
		}
		else if (!_hasPlayedMayaDeathScream)
		{
			CallReferenceMayaVoiceGC("scream8", "GameCore.PlayMayaDeathScream()");
			_hasPlayedMayaDeathScream = true;
		}
	}

	public void CheckHit_VFX_SFX(string originTag, string originName, string targetTag, string targetName, Vector2 spawnPosition)
	{
		VisualEffect visualEffect = VisualEffect.Null;
		if (originTag == "Lala-Atk-Hitbox")
		{
			visualEffect = VisualEffect.BasicSwordHit;
			PlaySFX(SFXBasic1.NormalSwordHit1, 0, checkForDuplicity: true, Vector2.zero, 0.75f, 1.25f);
		}
		if (visualEffect != VisualEffect.Null)
		{
			InstantiateVFX(visualEffect, spawnPosition, "GAMECORE:CheckHit_VFX_SFX");
		}
	}

	public GameObject InstantiateVFX(VisualEffect thisType, Vector2 targetPosition, string senderScriptMethod, Transform underCustomParent = null, bool returnOBJref = false)
	{
		GameObject gameObject = ReturnVisualEffectPrefab(thisType);
		GameObject gameObject2 = null;
		if ((bool)gameObject)
		{
			if (underCustomParent == null)
			{
				if (!CheckRefEffectsParent())
				{
					Debug.Log("VFX_ERROR: No valid parent found!");
					return null;
				}
				underCustomParent = _mainEffectParent.transform;
			}
			gameObject2 = UnityEngine.Object.Instantiate(gameObject, targetPosition, Quaternion.identity, underCustomParent);
			if (returnOBJref)
			{
				return gameObject2;
			}
		}
		else
		{
			Debug.Log("Could not instantiate " + thisType.ToString() + " as tmpVFX is NULL! [Origin]: " + senderScriptMethod);
		}
		return null;
	}

	public GameObject ReturnVisualEffectPrefab(VisualEffect thisType)
	{
		if (CheckRefCoreUI())
		{
			return coreUI.GetVisualEffectOBJ(thisType);
		}
		Debug.Log("Could not get reference to the coreUI OBJ in order to access the VisualEffects handler!");
		return null;
	}

	public void EVENT_RunLocalEventOnTheSceneWithThisName(string thisName)
	{
		EventTriggerLogic eventTriggerLogic = null;
		GameObject[] array = GameObject.FindGameObjectsWithTag("EventTrigger");
		if (array != null && array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (array[i].name == thisName)
				{
					eventTriggerLogic = array[i].GetComponent<EventTriggerLogic>();
					break;
				}
			}
		}
		else
		{
			Debug.Log("[GAMECORE:EVENT] Could not find any events in the local scene with a named object that equals -> " + thisName);
		}
		if (eventTriggerLogic != null)
		{
			eventTriggerLogic.StartEventMainLogic();
		}
		else
		{
			Debug.Log("[GAMECORE:EVENT] Tried to run the local event named " + thisName + ", but could not get a reference to the Event component in the object!");
		}
	}

	public bool SignalPlayerMoveHorizontalyUntilPoint(string waypointName, Vector2 point, float movSpeed)
	{
		_eventPlayerReachedPoint = false;
		if (CheckRefPlayer())
		{
			_refPlayerScript.ForceMovePlayerToTargetPoint(waypointName, point, movSpeed, 10f, checkForStuck: true);
			return true;
		}
		return false;
	}

	public bool SignalPlayerToStartDialogue(int id_dial, bool is_zoom)
	{
		_eventPlayerFinishedDialogue = false;
		if (CheckRefPlayer())
		{
			_refPlayerScript.StartEventDialogue(NPCType.Various, id_dial, is_zoom);
			return true;
		}
		return false;
	}

	public void SignalPlayerEventHasBeenFinished()
	{
		if (CheckRefPlayer())
		{
			_refPlayerScript.PlayerEventHasBeenFinished();
		}
	}

	public void SignalBossFightStart()
	{
		_eventStartBossFight = true;
		if (!_isBossRematch)
		{
			ToggleBossFightArena();
		}
		if (CheckRefCoreUI())
		{
			coreUI.ToggleBossLifeBar(setThisState: true);
		}
		PlaySFX(SFXBasic2.BossFightStart, 0, checkForDuplicity: false, Vector2.zero, 0f, 0f);
	}

	public void ToggleBossFightArena()
	{
		if (CheckRefBossArena())
		{
			Transform child = _objBossArena.transform.GetChild(0);
			if (child != null)
			{
				child.gameObject.SetActive(!child.gameObject.activeInHierarchy);
			}
		}
	}

	public void UpdateBossLifeBarValues(float currentLife, float maxLife)
	{
		if (CheckRefCoreUI())
		{
			coreUI.UpdateBossCurrentLife(currentLife, maxLife);
		}
	}

	public void UpdateBossName(int idBossNameInFile)
	{
		if (clsTranslationUI.listOfUserInterfaceTranslation.Count > 0)
		{
			string text = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == idBossNameInFile)?.text;
			if (CheckRefCoreUI() && text != "")
			{
				coreUI.UIMisc.BossName.text = text;
				coreUI.UIMisc.BossNameShadow.text = text;
			}
		}
	}

	public void GAUNTLET_Toggle(bool state, int killGoal = 1)
	{
		if (state)
		{
			PlayMusicTheme(MusicTheme.BossFight3, doFadeIn: false, 0f, doLoop: true);
			_countGauntletKills = 0;
			_countGauntletGoal = killGoal;
			_eventStartGauntlet = true;
			ToggleBossFightArena();
			DisplaySystemMessageUI(0, 179, 5f);
			if (CheckRefCoreUI())
			{
				coreUI.ToggleChallengeKillCountUI(state: true);
			}
			return;
		}
		if (CheckRefCoreUI())
		{
			coreUI.ToggleChallengeKillCountUI(state: false);
		}
		ToggleBossFightArena();
		_countGauntletGoal = 0;
		_countGauntletKills = 0;
		_eventStartGauntlet = false;
		PlaySFX(SFXBasic2.GauntletFanfare, 0, checkForDuplicity: true, Vector2.zero, 0f, 0f, "Fanfare");
		CheckStageMusic();
		DisplaySystemMessageUI(0, 180, 5f);
		if (!isPlayerFainted && !playerIsBeingGrabed)
		{
			StoreRetryGameCore(playerCurrentWorldXPosition, playerCurrentWorldYPosition, "GameCore-GauntletOFF");
		}
	}

	public void GAUNTLET_AddKillCount(int amount)
	{
		if (_eventStartGauntlet)
		{
			_countGauntletKills += amount;
			if (_countGauntletKills >= _countGauntletGoal)
			{
				GAUNTLET_VictoryLogic();
			}
		}
	}

	public void GAUNTLET_VictoryLogic()
	{
		string text = strCurrentActiveSceneName;
		if (text == "Stage1-Forest")
		{
			coreGameData.gameFlags.winGauntlet1 = true;
		}
		GAUNTLET_Toggle(state: false);
	}

	public void StartWaitingThenDoSomethingGeneric(WaitGenerics something, float seconds)
	{
		StartCoroutine(waitForSecondsToDoSomething(something, seconds));
	}

	private IEnumerator waitForSecondsToDoSomething(WaitGenerics thing, float time)
	{
		DoSomethingGenericBeforeWaiting(thing);
		yield return new WaitForSeconds(time);
		DoSomethingGenericAfterWaiting(thing);
	}

	private void DoSomethingGenericBeforeWaiting(WaitGenerics thisThing)
	{
		if (thisThing == WaitGenerics.UniqueItemFanfare)
		{
			PlayMusicTheme(MusicTheme.UniqueItemFanfare1, doFadeIn: false, 0f, doLoop: false);
		}
	}

	private void DoSomethingGenericAfterWaiting(WaitGenerics thisThing)
	{
		switch (thisThing)
		{
		case WaitGenerics.VictoryFanfare:
			PlayMusicTheme(MusicTheme.VictoryFanfare1, doFadeIn: false, 0f, doLoop: false);
			break;
		case WaitGenerics.UniqueItemFanfare:
			CheckStageMusic();
			break;
		}
	}

	public void DoStuffAfterBossDeath(EnemyType thisBossDead, bool doGenericToggles)
	{
		if (doGenericToggles)
		{
			StopMusicTheme(doFadeOut: false, 0f);
			StartWaitingThenDoSomethingGeneric(WaitGenerics.VictoryFanfare, 0.1f);
			_eventStartBossFight = false;
			ToggleBossFightArena();
			if (CheckRefCoreUI())
			{
				coreUI.ToggleBossLifeBar(setThisState: false);
			}
			if (CheckRefPlayer())
			{
				_refPlayerScript.MakePlayerInvincibleForSeconds(4f, doSpriteFlashing: false, doVicoryAnimAfterBoss: true);
			}
		}
		switch (thisBossDead)
		{
		case EnemyType.BossForest:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: true, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isForestBossDefeatd);
			break;
		case EnemyType.BossBeach:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: false, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isBeachBossDefeated);
			break;
		case EnemyType.BossHill:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: true, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isHillBossDefeated);
			break;
		case EnemyType.BossWerewolf:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: true, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isWerewolfBossDefeated);
			if (CheckRefCoreUI())
			{
				coreUI.ToggleWerewolfCurse(setThisState: false);
			}
			RemoveEffectFromPlayer(TempPlayerEffect.WerewolfBossCurse, "GAMECORE.DoStuffAfterBossDeath().WerewolfRemove");
			break;
		case EnemyType.BossMinotaur:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: true, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isMinotaurBossDefeated);
			break;
		case EnemyType.BossPrincess:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: false, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isPrincessBossDefeated);
			break;
		case EnemyType.BossWitch:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: false, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isWitchBossDefeated);
			break;
		case EnemyType.BossBlueFox:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: false, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isBlueFoxDefeated);
			break;
		case EnemyType.BossSwampSlime:
			ApplyStuffAfterBossDeath(updateDefeatedFlag: true, activateSpecialDrop: true, killActiveEnemies: true, removeProjectiles: true, ref coreGameData.gameFlags.isBossSwampDefeated);
			break;
		default:
			Debug.Log("This boss was defeated but is missing the logic to update post-death flags. Click here to fix.");
			break;
		}
		if (!isBossRematch)
		{
			StoreRetryGameCore(0f, 0f, "GameCore-DoStuffAfterBossDeath", ignoreCameraOffset: false, maintainRetryPlayerPosition: true);
		}
	}

	private void ApplyStuffAfterBossDeath(bool updateDefeatedFlag, bool activateSpecialDrop, bool killActiveEnemies, bool removeProjectiles, ref bool refToBossFlag)
	{
		if (updateDefeatedFlag)
		{
			refToBossFlag = true;
		}
		if (activateSpecialDrop && !_isBossRematch)
		{
			GameObject gameObject = GameObject.Find("[SPECIAL]");
			if (gameObject != null)
			{
				gameObject.transform.GetChild(0).gameObject.SetActive(value: true);
			}
		}
		if (killActiveEnemies && CheckRefEnemiesParent())
		{
			EnemyGeneralLogic enemyGeneralLogic = null;
			foreach (Transform item in _mainEnemiesParent.transform)
			{
				enemyGeneralLogic = item.gameObject.GetComponent<EnemyGeneralLogic>();
				if (enemyGeneralLogic != null)
				{
					enemyGeneralLogic.StartDeathLogicBehaviour();
				}
			}
		}
		if (!removeProjectiles || !CheckRefEffectsParent())
		{
			return;
		}
		GameObject[] array = new GameObject[_mainEffectParent.transform.childCount];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = _mainEffectParent.transform.GetChild(i).gameObject;
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] != null && array[j].activeSelf)
			{
				array[j].SetActive(value: false);
			}
		}
	}

	private bool CheckRefCoreUI()
	{
		if (coreUI != null)
		{
			return true;
		}
		GameObject gameObject = GameObject.Find("[CANVAS]");
		if (gameObject == null)
		{
			gameObject = GameObject.Find("[CANVAS] Title Screen Variant");
		}
		if (gameObject != null)
		{
			coreUI = gameObject.GetComponent<GameUI>();
		}
		if (coreUI != null)
		{
			return true;
		}
		return false;
	}

	private bool CheckRefAudioSystem()
	{
		if (refAudioSystem != null)
		{
			return true;
		}
		GameObject gameObject = GameObject.Find("[AUDIOSYSTEM]");
		if (gameObject != null)
		{
			refAudioSystem = gameObject.GetComponent<AudioSystem>();
		}
		if (refAudioSystem != null)
		{
			return true;
		}
		Debug.Log("Could not find [AUDIOSYSTEM] game object in the scene!");
		return false;
	}

	private bool CheckRefEffectsParent()
	{
		if ((bool)_mainEffectParent)
		{
			return true;
		}
		_mainEffectParent = GameObject.Find("[EFFECT]");
		if ((bool)_mainEffectParent)
		{
			return true;
		}
		return false;
	}

	private bool CheckRefEnemiesParent()
	{
		if ((bool)_mainEnemiesParent)
		{
			return true;
		}
		_mainEnemiesParent = GameObject.Find("[ENEMY-BASIC]");
		if ((bool)_mainEnemiesParent)
		{
			return true;
		}
		return false;
	}

	private bool CheckRefMenuBehaviour()
	{
		if ((bool)_refMenuScript)
		{
			return true;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("scriptMenuBehaviour");
		_refMenuScript = gameObject.GetComponent<MenuBehaviour>();
		if ((bool)_refMenuScript)
		{
			return true;
		}
		return false;
	}

	private bool CheckRefPlayer()
	{
		if ((bool)_refPlayerScript)
		{
			return true;
		}
		GameObject gameObject = GameObject.Find("[PLAYER]");
		_refPlayerScript = gameObject.GetComponent<Controller_Player>();
		if ((bool)_refPlayerScript)
		{
			return true;
		}
		return false;
	}

	private bool CheckRefBossArena()
	{
		if ((bool)_objBossArena)
		{
			return true;
		}
		_objBossArena = GameObject.Find("[BOSS-ARENA]");
		if ((bool)_objBossArena)
		{
			return true;
		}
		return false;
	}

	public void ForceCurrentBossEnragePhase()
	{
		GameObject gameObject = GameObject.FindGameObjectWithTag("Enemy-Boss");
		if (gameObject != null)
		{
			gameObject.GetComponent<EnemyGeneralLogic>().DebugForceBossEnragePhase();
		}
		else
		{
			Debug.Log("[GAMECORE:DEBUG] Tried to get an object reference to the current boss, but none was located in the current scene!");
		}
	}

	private GameObject FindReferenceGenericOBJ(string name)
	{
		return GameObject.Find(name);
	}

	public void HandleScreenshotForSaveFile(bool isCapture, string strCustom = "")
	{
		try
		{
			string text = Path.Combine(MKMSaveDir(), "MKMSVP" + strCustom + ".png");
			if (isCapture)
			{
				if (CheckRefCoreUI())
				{
					coreUI.DeactivateCanvasDuringLoad(dontBlackScreen: true);
				}
				ScreenCapture.CaptureScreenshot(text);
			}
			else if (File.Exists(text))
			{
				File.Delete(text);
			}
		}
		catch (Exception ex)
		{
			Debug.Log(ex.Message);
		}
	}

	public void SCREENSHOT_DoCapture(float delayTime, string strID)
	{
		StartCoroutine(waitToCapture(delayTime, strID));
	}

	private IEnumerator waitToCapture(float wait, string strCustom)
	{
		Debug.Log("Countdown to screenshot: " + wait + " seconds...");
		yield return new WaitForSeconds(wait);
		HandleScreenshotForSaveFile(isCapture: true, strCustom);
		Debug.Log("Finished taking the screenshot ID #" + strCustom + "! Please go to the save folder for the picture file.");
	}

	public float MATH_RoundFloatDecimals(float thisFloat, int decimalsToRoundTo)
	{
		switch (decimalsToRoundTo)
		{
		default:
			return Mathf.Round(thisFloat);
		case 1:
			return Mathf.Round(thisFloat * 10f) / 10f;
		case 2:
			return Mathf.Round(thisFloat * 100f) / 100f;
		case 3:
			return Mathf.Round(thisFloat * 1000f) / 1000f;
		case 4:
			return Mathf.Round(thisFloat * 10000f) / 10000f;
		case 5:
			return Mathf.Round(thisFloat * 100000f) / 100000f;
		}
	}

	public float MATH_RoundFloatToHalf(float value)
	{
		return Mathf.Floor(value) + 0.5f;
	}

	public float MATH_ResultPercentileOfTotal(float amount, float ofTotal, bool getInverted = false)
	{
		if (!getInverted)
		{
			return amount / ofTotal * 100f;
		}
		return (amount / ofTotal * 100f - 100f) * -1f;
	}

	public float MATH_ResultValueOfTotalPercentile(float percentile, float ofTotal)
	{
		return percentile / 100f * ofTotal;
	}

	public bool MATH_CompareFloatsAddMargin(float flt1, float flt2, float flt1Upper = 0f, float flt1Lower = 0f, float flt2Upper = 0f, float flt2Lower = 0f)
	{
		flt1Upper = flt1 + flt1Upper;
		flt1Lower = flt1 + flt1Lower;
		flt2Upper = flt2 + flt2Upper;
		flt2Lower = flt2 + flt2Lower;
		if (flt1Upper <= flt2Upper && flt1Upper >= flt2Lower)
		{
			return true;
		}
		if (flt1Lower >= flt2Lower && flt1Lower <= flt2Upper)
		{
			return true;
		}
		return false;
	}

	public bool MATH_ComparePositionsAddMargin(Vector3 pos1, Vector3 pos2, float flt1Upper = 0f, float flt1Lower = 0f, float flt2Upper = 0f, float flt2Lower = 0f)
	{
		if (MATH_CompareFloatsAddMargin(pos1.x, pos2.x, flt1Upper, flt1Lower, flt2Upper, flt2Lower) && MATH_CompareFloatsAddMargin(pos1.y, pos2.y, flt1Upper, flt1Lower, flt2Upper, flt2Lower))
		{
			return true;
		}
		return false;
	}

	public bool MATH_RNG_ROLL(int percent_of_chance)
	{
		if (MATH_RNG_INT(1, 100) <= percent_of_chance)
		{
			return true;
		}
		return false;
	}

	public int MATH_RNG_INT(int startInclusive, int endInclusive, string strNoRepeat = "")
	{
		if (startInclusive >= endInclusive)
		{
			return startInclusive;
		}
		int num2;
		if (strNoRepeat != "")
		{
			int num = 0;
			if (!_dicRNGint.TryGetValue(strNoRepeat, out var value))
			{
				_dicRNGint.Add(strNoRepeat, 0);
			}
			do
			{
				num++;
				num2 = UnityEngine.Random.Range(startInclusive, endInclusive + 1);
			}
			while (num <= 10 && num2 == value);
			_dicRNGint[strNoRepeat] = num2;
		}
		else
		{
			num2 = UnityEngine.Random.Range(startInclusive, endInclusive + 1);
		}
		return num2;
	}

	public float MATH_RNG_FLOAT(float startInclusive, float endInclusive)
	{
		if (startInclusive >= endInclusive)
		{
			return startInclusive;
		}
		return UnityEngine.Random.Range(startInclusive, endInclusive);
	}

	public Vector3 MATH_ROUND_VECTOR3(Vector3 vector3, int decimalPlaces = 2)
	{
		float num = 1f;
		for (int i = 0; i < decimalPlaces; i++)
		{
			num *= 10f;
		}
		return new Vector3(Mathf.Round(vector3.x * num) / num, Mathf.Round(vector3.y * num) / num, Mathf.Round(vector3.z * num) / num);
	}

	public int[] MATH_RNG_INT_ARRAY_INDEX_SCRAMBLE(int lenghtOfIndexes, int failSafeTotalOfRuns = 1000, bool generateDebugLogReport = false)
	{
		if (lenghtOfIndexes <= 1)
		{
			Debug.Log("There is no point in scrambling 1 or less indexes! Needs at least 2! RETURNING NULL!");
			return null;
		}
		int[] array = new int[lenghtOfIndexes];
		lenghtOfIndexes--;
		int num = lenghtOfIndexes;
		int num2 = 0;
		do
		{
			num2++;
			int num3 = MATH_RNG_INT(0, lenghtOfIndexes);
			if (array[num3] == 0)
			{
				array[num3] = num;
				num--;
			}
		}
		while (num >= 0 && num2 < failSafeTotalOfRuns);
		if (generateDebugLogReport)
		{
			lenghtOfIndexes++;
			Debug.Log("---");
			Debug.Log("Report for this RNG_INT_ARRAY_INDEX_SCRAMBLE execution...");
			Debug.Log("Parameter lenghtOfIndexes: " + lenghtOfIndexes);
			Debug.Log("Parameter failSafeTotalOfRuns: " + failSafeTotalOfRuns);
			Debug.Log("The DO-WHILE inside this method looped " + num2 + " times, and produced the array bellow:");
			for (int i = 0; i < lenghtOfIndexes; i++)
			{
				Debug.Log(array[i].ToString());
			}
			Debug.Log("The array above should have " + lenghtOfIndexes + " entries, values's order randomized from top to bottom, values non-repeating from zero to the lenght (minus 1).");
		}
		return array;
	}

	public int MATH_RNG_INT_ARRAY(int[] elements)
	{
		return elements[UnityEngine.Random.Range(0, elements.Length)];
	}

	public int[] MATH_RNG_TYPE_ARRAY<T>(T[] arrayInput, int quantityOfIndexesToReturn, List<int> listExclude = null)
	{
		if (arrayInput == null || arrayInput.Length == 0)
		{
			Debug.Log("GAMECORE_RNG_TYPE_ARRAY -> The array is NULL!");
			return null;
		}
		if (quantityOfIndexesToReturn <= 0)
		{
			Debug.Log("GAMECORE_RNG_TYPE_ARRAY -> The quantity must be greater than zero!");
			return null;
		}
		if (quantityOfIndexesToReturn >= arrayInput.Length)
		{
			Debug.Log("GAMECORE_RNG_TYPE_ARRAY -> The return quantity must be lower than the amount of entries in the array!");
			return null;
		}
		List<int> list = new List<int>();
		int num = 0;
		int randomIndex = 0;
		for (int i = 0; i < quantityOfIndexesToReturn; i++)
		{
			num = 0;
			do
			{
				randomIndex = MATH_RNG_INT(0, arrayInput.Length - 1);
				if (!list.Exists((int x) => x == randomIndex) && (listExclude == null || !listExclude.Exists((int x) => x == randomIndex)))
				{
					list.Add(randomIndex);
					num = 99;
				}
				num++;
			}
			while (num <= 50);
		}
		if (list.Count < quantityOfIndexesToReturn)
		{
			Debug.Log("GAMECORE_RNG_TYPE_ARRAY -> The RNG logic FAILED! It wasn't able to return the approriate number of random unique indexes!");
			return null;
		}
		int[] array = new int[quantityOfIndexesToReturn];
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			array[num2] = list[num2];
		}
		return array;
	}

	public float MATH_POINTDISTX_FLOAT(float point1X, float point2X)
	{
		float num = 0f;
		num = point1X - point2X;
		if (num < 0f)
		{
			num *= -1f;
		}
		return num;
	}

	public bool MATH_IsDivisibleBy(int checkThisNumber, int byThisNumber)
	{
		if (checkThisNumber <= 0 || byThisNumber <= 0)
		{
			return false;
		}
		if (checkThisNumber % byThisNumber == 0)
		{
			return true;
		}
		return false;
	}

	public bool MATH_IsDivisibleBy(float checkThisNumber, int byThisNumber)
	{
		if (checkThisNumber <= 0f || byThisNumber <= 0)
		{
			return false;
		}
		if (checkThisNumber % (float)byThisNumber == 0f)
		{
			return true;
		}
		return false;
	}

	public Vector2 MATH_CALC_BALLISTIC_TRAJECTORY_TO_POINT(Vector2 originPosition, Vector2 targetPosition, bool increaseHeightSpecial, Vector2 specialCheck)
	{
		Vector2 vector = new Vector2(0f, 0f);
		float num = targetPosition.x - originPosition.x;
		float num2 = targetPosition.y - originPosition.y;
		if (num < -8f)
		{
			num = -8f;
		}
		if (num > 8f)
		{
			num = 8f;
		}
		if (num2 < -6f)
		{
			num2 = -6f;
		}
		if (num2 > 6f)
		{
			num2 = 6f;
		}
		float f = Mathf.Atan((num2 + 4.905f) / num);
		float num3 = num / Mathf.Cos(f);
		float num4 = num3 * Mathf.Cos(f);
		float num5 = num3 * Mathf.Sin(f);
		return (!increaseHeightSpecial || !MATH_CheckBallisticHeightSpecial(num, num2, specialCheck)) ? new Vector2(num4, num5) : new Vector2(num4 * 0.6f, num5 * 1.5f);
	}

	public float MATH_NormalizeFloatPercent(float number, bool zeroReturnOne = false)
	{
		if (zeroReturnOne && number == 0f)
		{
			return 1f;
		}
		if (number > 0f)
		{
			number = MATH_ResultValueOfTotalPercentile(number + 100f, 1f);
		}
		else if (number < 0f)
		{
			number *= -1f;
			number = MATH_ResultPercentileOfTotal(number, 100f, getInverted: true);
			number = MATH_ResultValueOfTotalPercentile(number, 1f);
		}
		return number;
	}

	private bool MATH_CheckBallisticHeightSpecial(float horDis, float verDis, Vector2 specialCheck)
	{
		if (specialCheck != Vector2.zero && horDis <= specialCheck.x && horDis >= 0f - specialCheck.x && verDis <= specialCheck.y && verDis >= 0f - specialCheck.y)
		{
			return true;
		}
		return false;
	}

	public Vector2 MATH_GetPositionOfNearestEnemy(Vector2 basePosition, float radiusIncrease = 1f, int countFailSafe = 5, int playerFacing = 0, int scrambleNearestRNG = 0)
	{
		Vector2 zero = Vector2.zero;
		float num = 0f;
		do
		{
			num += radiusIncrease;
			countFailSafe--;
			RaycastHit2D[] arrayHits = Physics2D.BoxCastAll(base.transform.position, new Vector2(num, num), 0f, Vector2.zero, 0f, 1 << LayerMask.NameToLayer("LayerEnemies"));
			zero = MATH_GetNearestPosition(basePosition, arrayHits, playerFacing, 10, 50, getEnemyCenterPos: true);
		}
		while (countFailSafe > 0 && zero == Vector2.zero);
		return zero;
	}

	public void MATH_XPTO_CheckRNG(int amountOfTimesToRun, int maxRangeToCheck)
	{
		int[] array = new int[maxRangeToCheck];
		if (amountOfTimesToRun < 1 || amountOfTimesToRun > 10000)
		{
			amountOfTimesToRun = 1;
		}
		Debug.Log("For a range of " + array.Length.ToString() + " numbers, let's run the RNG " + amountOfTimesToRun + " times!");
		for (int i = 0; i < amountOfTimesToRun; i++)
		{
			array[MATH_RNG_INT(0, maxRangeToCheck - 1)]++;
		}
		Debug.Log("Finished running! The Results are:");
		for (int j = 0; j < array.Length; j++)
		{
			Debug.Log("Number " + (j + 1) + " was rolled " + array[j] + " times! Accounting for " + MATH_ResultPercentileOfTotal(array[j], amountOfTimesToRun) + "% of rolls!");
		}
	}

	public void DisplaySystemMessageUI(int messageID, int directID = 0, float customDisplayTimer = 0f, string customString = "", bool isFlavorCentralMessage = false)
	{
		string text = "";
		switch (messageID)
		{
		default:
			text = GetTranslatedStringUI(directID);
			break;
		case 1:
			text = string.Concat(str2: (!showInternalShots) ? GetTranslatedStringUI(53) : GetTranslatedStringUI(52), str0: GetTranslatedStringUI(54), str1: " ");
			break;
		case 2:
			text = GetTranslatedStringUI(55) + " " + GetTranslatedStringUI(directID);
			break;
		case 3:
			text = GetTranslatedStringUI(directID) + " " + GetTranslatedStringUI(62);
			break;
		case 4:
			text = GetTranslatedStringUI(63) + " " + GetTranslatedStringITEM(directID);
			break;
		case 5:
			text = GetTranslatedStringUI(64);
			break;
		case 6:
			text = GetTranslatedStringUI(117);
			break;
		case 7:
			text = GetTranslatedStringUI(178) + " " + GetItemNameFromList(directID);
			break;
		case 8:
			text = GetTranslatedStringUI(69) + " +" + directID;
			break;
		case 100:
			text = GetTranslatedStringUI(85) + " " + customString + "!";
			break;
		case 123456:
			text = customString;
			break;
		}
		if (CheckRefCoreUI() && text != "")
		{
			coreUI.AddSystemMessage(text, customDisplayTimer, isFlavorCentralMessage);
		}
	}

	public void BEpartInfo(int partCurrent, int ofMax)
	{
		if (_isPlayerFainted)
		{
			partCurrent++;
			if (partCurrent > ofMax)
			{
				partCurrent = ofMax;
			}
			if (CheckRefCoreUI())
			{
				coreUI.DisplayBADENDPartsInfo(partCurrent, ofMax);
			}
		}
	}

	public void DoFadeInOutBlackScreen(bool FadeIntoBlack, float fadeDuration, float optionalFadeOUT = 0f)
	{
		if (CheckRefCoreUI())
		{
			coreUI.FadeInOutBlackScreen(FadeIntoBlack, fadeDuration, optionalFadeOUT);
		}
	}

	public string GetTranslatedStringUI(int sentID)
	{
		return ParseDialogueLineTags(clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == sentID)?.text);
	}

	public string GetTranslatedStringITEM(int sentID)
	{
		return clsItemDatabase.listOfItemDatabase.Find((ItemInfo item) => item.id == sentID)?.name;
	}

	public string getTranslatedStringChessPiece(SavePieceType thisPiece)
	{
		int sentID = 0;
		switch (thisPiece)
		{
		case SavePieceType.Pawn:
			sentID = 91;
			break;
		case SavePieceType.Rook:
			sentID = 92;
			break;
		case SavePieceType.Knight:
			sentID = 93;
			break;
		case SavePieceType.Bishop:
			sentID = 94;
			break;
		case SavePieceType.Queen:
			sentID = 95;
			break;
		case SavePieceType.Crystal:
			sentID = 102;
			break;
		}
		return GetTranslatedStringUI(sentID);
	}

	public bool IsButtonAlreadySelected(Button thisBTN)
	{
		if (EventSystem.current?.currentSelectedGameObject?.name == thisBTN.name)
		{
			EventSystem.current.SetSelectedGameObject(null);
			thisBTN.Select();
			return true;
		}
		return false;
	}

	public string TranslateSceneNameToStageName(bool isCurrentlyActive, string strParseThisSceneName)
	{
		string result = "Default.Stage.Name";
		if (clsTranslationUI.listOfUserInterfaceTranslation.Count <= 0)
		{
			return result;
		}
		if (isCurrentlyActive)
		{
			strParseThisSceneName = strCurrentActiveSceneName;
		}
		switch (strParseThisSceneName)
		{
		case "Stage2-Beach":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 950)?.text;
			break;
		case "Stage0-Dungeon":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 951)?.text;
			break;
		case "Stage0-UpperFloors":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 969)?.text;
			break;
		case "Stage0-BossChamber":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 970)?.text;
			break;
		case "Stage0-xActualDungeon":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 971)?.text;
			break;
		case "Stage1-Forest":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 952)?.text;
			break;
		case "Stage1-Forest-part2":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 953)?.text;
			break;
		case "Stage1-Forest-Boss":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 954)?.text;
			break;
		case "Stage2-Beach-Part2":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 955)?.text;
			break;
		case "Stage2-Beach-PartBoss":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 956)?.text;
			break;
		case "Stage3-Valley":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 957)?.text;
			break;
		case "Stage3-Valley-part2":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 958)?.text;
			break;
		case "Stage3-Valley-partBoss":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 959)?.text;
			break;
		case "Stage4-City":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 960)?.text;
			break;
		case "Stage4-City-part2":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 961)?.text;
			break;
		case "Stage4-City-part3Boss":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 968)?.text;
			break;
		case "Stage5-Cave1":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 962)?.text;
			break;
		case "Stage5-Cave2":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 963)?.text;
			break;
		case "Stage5-Cave3":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 964)?.text;
			break;
		case "Stage5-Cave4BW":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 965)?.text;
			break;
		case "Stage5-Cave5BossWitch":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 972)?.text;
			break;
		case "Stage6-1-Swamp":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 966)?.text;
			break;
		case "Stage6-2-Gardens":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 967)?.text;
			break;
		case "Stage6-3-SwampBoss":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 977)?.text;
			break;
		case "Stage7-HighTech_A":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 973)?.text;
			break;
		case "Stage7-HighTech_B_Boss":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 974)?.text;
			break;
		case "Stage7-HighTech_C":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 975)?.text;
			break;
		case "AnimGallery":
			result = clsTranslationUI.listOfUserInterfaceTranslation.Find((LineOfTranslationUI item) => item.id == 976)?.text;
			break;
		}
		return result;
	}

	public string TranslateBiomeName(GalleryThemeType biomeType)
	{
		int num = 0;
		switch (biomeType)
		{
		default:
			num = 900;
			break;
		case GalleryThemeType.Forest:
			num = 901;
			break;
		case GalleryThemeType.Hill:
			num = 902;
			break;
		case GalleryThemeType.City:
			num = 903;
			break;
		case GalleryThemeType.Cave:
			num = 904;
			break;
		case GalleryThemeType.Swamp:
			num = 905;
			break;
		case GalleryThemeType.Castle:
			num = 906;
			break;
		case GalleryThemeType.HighTech:
			num = 907;
			break;
		}
		return GetTranslatedStringUI(num);
	}

	public void MINIMAP_Toggle(bool toggleCurrentState, bool setThisSpecificState)
	{
		if (!CheckRefCoreUI())
		{
			return;
		}
		if (toggleCurrentState)
		{
			if (MINIMAP_IsDisplaying())
			{
				coreUI.UIMainOverlay.Minimap.gameObject.SetActive(value: false);
			}
			else
			{
				coreUI.UIMainOverlay.Minimap.gameObject.SetActive(value: true);
			}
			coreConfigData.toggleMinimapState = MINIMAP_IsDisplaying();
			SaveGameConfiguration();
			DisplaySystemMessageUI(123456, 0, 2f, "Minimap was toggled");
		}
		else
		{
			coreUI.UIMainOverlay.Minimap.gameObject.SetActive(setThisSpecificState);
		}
	}

	private bool MINIMAP_IsDisplaying()
	{
		if (CheckRefCoreUI() && coreUI.UIMainOverlay.Minimap.gameObject.activeInHierarchy)
		{
			return true;
		}
		return false;
	}

	public void UIMainToggle(bool setState)
	{
		if (CheckRefCoreUI())
		{
			coreUI.ToggleMainBasicUI(setState);
		}
	}

	public void UIDebugDescriptionToggle(bool setState)
	{
		if (CheckRefCoreUI())
		{
			coreUI.ToggleDebugDescription(setState);
		}
	}

	public void OverrideCameraZoom(ControllerCamera.ZoomType typeOfZoom, bool resetZoomToDefault = false)
	{
		if (CheckRefCoreUI() && coreUI.CameraReference != null)
		{
			if (resetZoomToDefault)
			{
				coreUI.CameraReference.ManualEndZoomEffect();
			}
			else
			{
				coreUI.CameraReference.InitiateZoom(typeOfZoom, hasTimeLimit: false, 0f);
			}
		}
	}

	public void OverrideCameraMove(Vector2 cameraOffset, bool resetCameraToDefault = false, bool instantReset = false)
	{
		if (CheckRefCoreUI() && coreUI.CameraReference != null)
		{
			if (resetCameraToDefault || cameraOffset == Vector2.zero)
			{
				coreUI.CameraReference.StopCameraFixedDirectionOffset(ControllerCamera.camDirection.Custom, instantReset);
			}
			else
			{
				coreUI.CameraReference.MoveCameraFixedDirectionOffset(ControllerCamera.camDirection.Custom, 0f, 0.75f, cameraOffset);
			}
		}
	}

	public void CallShakeTheCamera(float duration, float magnitude, float damping)
	{
		if (CheckRefCoreUI())
		{
			coreUI.CallShakeTheCamera(duration, magnitude, damping);
		}
	}

	public void UI_SELECTION_StoreLastSelected()
	{
		if (EventSystem.current?.currentSelectedGameObject != _currentSelectedOBJ)
		{
			_previousSelectedOBJ = _currentSelectedOBJ;
			_currentSelectedOBJ = EventSystem.current?.currentSelectedGameObject;
		}
	}

	public void UI_SELECTION_HighlightElementIfNoneAreCurrentlySelected()
	{
		if ((strCurrentActiveSceneName == "MainMenu" || isGamePaused || (CheckRefCoreUI() && coreUI.UIAnimControl.objAnimControlWindow.activeInHierarchy)) && (EventSystem.current == null || EventSystem.current.currentSelectedGameObject == null || (EventSystem.current?.currentSelectedGameObject != _currentSelectedOBJ && EventSystem.current?.currentSelectedGameObject != _previousSelectedOBJ)))
		{
			if ((bool)_currentSelectedOBJ && _currentSelectedOBJ.activeInHierarchy)
			{
				_currentSelectedOBJ.GetComponent<Selectable>().Select();
			}
			else if ((bool)_previousSelectedOBJ && _previousSelectedOBJ.activeInHierarchy)
			{
				_previousSelectedOBJ.GetComponent<Selectable>().Select();
			}
		}
	}

	public void WEBGL_FeedLocalVariablesToString(TextAsset jsonUI, TextAsset jsonItemLIst, TextAsset jsonIntroStory, TextAsset jsonDBoard, TextAsset jsonDNene, TextAsset jsonDVarious, TextAsset jsonDiagFarina)
	{
		_jsonUI = (jsonUI != null) ? jsonUI.text : "";
		_jsonItemList = (jsonItemLIst != null) ? jsonItemLIst.text : "";
		_jsonIntro = (jsonIntroStory != null) ? jsonIntroStory.text : "";
		_jsonD_Board = (jsonDBoard != null) ? jsonDBoard.text : "";
		_jsonD_Nene = (jsonDNene != null) ? jsonDNene.text : "";
		_jsonD_Farina = (jsonDiagFarina != null) ? jsonDiagFarina.text : "";
		_jsonD_Various = (jsonDVarious != null) ? jsonDVarious.text : "";
	}

	private bool WEBGL_CheckIfIsWebBuild()
	{
		if (_gameBuild == GameBuildType.WebGL || Application.platform == RuntimePlatform.Android)
		{
			return true;
		}
		return false;
	}

	private string SendRequestForWEBGLtextFile(string url)
	{
		using (UnityWebRequest unityWebRequest = UnityWebRequest.Get(url))
		{
			unityWebRequest.SendWebRequest();
			if (unityWebRequest.isNetworkError || unityWebRequest.isHttpError)
			{
				Debug.Log("Unity WEBGL load file request error!");
				return "";
			}
			Debug.Log("Unity WEBGL load file request succesful!");
			return unityWebRequest.downloadHandler.text;
		}
	}

	public void TogglePauseForGameMenu()
	{
		if (isListeningForGeneralKeyBindings)
		{
			return;
		}
		if (Time.timeScale == 1f)
		{
			if (!_isDelayAfterExitPause)
			{
				Time.timeScale = 0f;
				if (CheckRefAudioSystem())
				{
					refAudioSystem.SetMusicVolumeManually(configMusicVolume * 0.4f);
				}
				_isGamePaused = true;
				if (CheckRefCoreUI())
				{
					coreUI.ToggleMainMenu(state: true);
				}
			}
			return;
		}
		_isDelayAfterExitPause = true;
		if (crtDelayAfterPause != null)
		{
			StopCoroutine(crtDelayAfterPause);
		}
		crtDelayAfterPause = StartCoroutine(delayAfterLeavingPause(0.2f));
		if (CheckRefAudioSystem())
		{
			refAudioSystem.SetMusicVolumeManually(configMusicVolume);
		}
		Time.timeScale = 1f;
		_isGamePaused = false;
		if (CheckRefCoreUI())
		{
			coreUI.ToggleMainMenu(state: false);
		}
		if (CheckRefMenuBehaviour())
		{
			_refMenuScript.ClearlastSeenStrings();
		}
	}

	private bool CheckProximityToPlayerForSound(Vector2 position)
	{
		float num = _playerCurrentWorldXPosition - position.x;
		float num2 = _playerCurrentWorldYPosition - position.y;
		if (num < 0f)
		{
			num *= -1f;
		}
		if (num2 < 0f)
		{
			num2 *= -1f;
		}
		if (num <= 18f && num2 <= 12f)
		{
			return true;
		}
		return false;
	}

	private string BasicXorEncryptorDecryptor(string strText)
	{
		int num = 129;
		StringBuilder stringBuilder = new StringBuilder(strText);
		StringBuilder stringBuilder2 = new StringBuilder(strText.Length);
		for (int i = 0; i < strText.Length; i++)
		{
			char c = stringBuilder[i];
			c = (char)(c ^ num);
			stringBuilder2.Append(c);
		}
		return stringBuilder2.ToString();
	}

	public void CallMenuRetry()
	{
		if (CheckRefMenuBehaviour())
		{
			_refMenuScript.CallRetry();
		}
	}

	public void CallGalleryViewMenuFromPillar()
	{
		if (CheckRefMenuBehaviour())
		{
			_refMenuScript.CallGalleryMenu();
		}
	}

	private IEnumerator delayAfterLeavingPause(float wait)
	{
		yield return new WaitForSeconds(wait);
		_isDelayAfterExitPause = false;
		crtDelayAfterPause = null;
	}

	public IEnumerator delayButtonSelection(float wait, Button thisButton)
	{
		yield return new WaitForSeconds(wait);
		if (thisButton != null)
		{
			EventSystem.current.SetSelectedGameObject(thisButton.gameObject);
		}
	}

	public DateTime ReturnParsedDateTime(string strDateTimeToParse)
	{
		try
		{
			return DateTime.ParseExact(strDateTimeToParse, "O", CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
		}
		catch (Exception ex)
		{
			Debug.Log("[GAMECORE]ReturnParsedDateTime_Error: " + ex.Message);
			return new DateTime(2000, 1, 1);
		}
	}

	public void DoMirrorEffectImageUI(float dur, bool clrRed = false)
	{
		if (CheckRefCoreUI())
		{
			coreUI.FadeOutMirrorScreen(dur, clrRed);
		}
	}

	public void AddNewVisitedSavePoint(int id, bool isDebug)
	{
		if (!coreGameData.listOfVisitedSavePoints.Exists((int x) => x == id))
		{
			coreGameData.listOfVisitedSavePoints.Add(id);
		}
		if (isDebug)
		{
			DisplaySystemMessageUI(123456, 0, 0f, "DEBUG: All save points unlocked for teleportation!");
			for (int num = 0; num < 30; num++)
			{
				AddNewVisitedSavePoint(num, isDebug: false);
			}
		}
	}

	public void DEBUG_AddAllSkills()
	{
		for (int i = 50; i < 57; i++)
		{
			if (CanAddThisSkill(i))
			{
				AddItemToPlayerInventory(i, "AddedViaDebug", Vector2.zero);
			}
			else
			{
				Debug.Log("[GAMECORE:DEBUG_AddAllSkills()] - > skill of id " + i + " was not added!");
			}
		}
		DisplaySystemMessageUI(123456, 0, 0f, "DEBUG: All skills unlocked!");
	}

	public void DEBUG_UNLOCKGALLERY()
	{
		AddItemToPlayerInventory(909, "DebugUnlockGallery", Vector2.zero);
		AddItemToPlayerInventory(910, "DebugUnlockGallery", Vector2.zero);
		AddItemToPlayerInventory(911, "DebugUnlockGallery", Vector2.zero);
		AddItemToPlayerInventory(912, "DebugUnlockGallery", Vector2.zero);
		AddItemToPlayerInventory(913, "DebugUnlockGallery", Vector2.zero);
		AddItemToPlayerInventory(914, "DebugUnlockGallery", Vector2.zero);
		AddItemToPlayerInventory(915, "DebugUnlockGallery", Vector2.zero);
		AddItemToPlayerInventory(916, "DebugUnlockGallery", Vector2.zero);
		StoreRetryGameCore(0f, 0f, "GameCore-DEBUG_UNLOCKGALLERY", ignoreCameraOffset: false, maintainRetryPlayerPosition: true);
		SaveGameData(9);
		DisplaySystemMessageUI(123456, 0, 10f, "Unlocked the gallery room!!");
		DisplaySystemMessageUI(123456, 0, 10f, "Return to title screen and select Gallery.");
	}

	public void EnemyChaseAmountAdd()
	{
		if (_currentNumberOfEnemiesChasing < 3)
		{
			_currentNumberOfEnemiesChasing++;
		}
	}

	public void EnemyChaseAmountSubtract()
	{
		if (_currentNumberOfEnemiesChasing > 0)
		{
			_currentNumberOfEnemiesChasing--;
		}
	}

	public bool EnemyChaseBellowLimit()
	{
		if (_currentNumberOfEnemiesChasing < 3)
		{
			return true;
		}
		return false;
	}

	public void SpecialEroTeleport(GameObject thisEnemy, string eroPointNameContains)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("eroSpecialTP");
		if (array == null || array.Length == 0)
		{
			Debug.Log("[GAMECORE.SpecialEroTeleport] The enemy " + thisEnemy.name + " tried to teleport to a special ero spot, but none was found in this scene!!");
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].name.Contains(eroPointNameContains))
			{
				thisEnemy.transform.position = array[i].transform.position;
				if (CheckRefCoreUI() && coreUI.CameraReference != null)
				{
					coreUI.CameraReference.InstantlyMoveTheCameraToPositionAfterTeleportToNewSceneBorder(array[i].transform.position);
				}
				return;
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].name.Contains("Any"))
			{
				thisEnemy.transform.position = array[j].transform.position;
				if (CheckRefCoreUI() && coreUI.CameraReference != null)
				{
					coreUI.CameraReference.InstantlyMoveTheCameraToPositionAfterTeleportToNewSceneBorder(array[j].transform.position);
				}
				break;
			}
		}
	}

	public bool LogicCheckGameState(LogicCheckType typeOfCheckToPerform)
	{
		switch (typeOfCheckToPerform)
		{
		case LogicCheckType.AnyBossDefeated:
			if (flagsMain.isBeachBossDefeated || flagsMain.isForestBossDefeatd || flagsMain.isHillBossDefeated || flagsMain.isMinotaurBossDefeated || flagsMain.isPrincessBossDefeated || flagsMain.isWerewolfBossDefeated || flagsMain.isWitchBossDefeated || flagsMain.isBlueFoxDefeated || flagsMain.isBossSwampDefeated)
			{
				return true;
			}
			break;
		case LogicCheckType.AnyGalleryItemAcquired:
			if (GalleryValidateRoomAndView(GalleryThemeType.Beach) || GalleryValidateRoomAndView(GalleryThemeType.Castle) || GalleryValidateRoomAndView(GalleryThemeType.Cave) || GalleryValidateRoomAndView(GalleryThemeType.City) || GalleryValidateRoomAndView(GalleryThemeType.Forest) || GalleryValidateRoomAndView(GalleryThemeType.HighTech) || GalleryValidateRoomAndView(GalleryThemeType.Hill) || GalleryValidateRoomAndView(GalleryThemeType.Swamp))
			{
				return true;
			}
			break;
		}
		return false;
	}

	public void MouseCursorToggle(bool isLocked, bool isVisible)
	{
		Cursor.visible = isVisible;
		if (isLocked)
		{
			Cursor.lockState = CursorLockMode.Locked;
		}
		else
		{
			Cursor.lockState = CursorLockMode.None;
		}
	}

	public void CRATE_INSERT_DISABLED_LIST(string sceneName, string objectName, Vector2 originPos)
	{
		sceneName.Trim();
		objectName.Trim();
		originPos.x = MATH_RoundFloatDecimals(originPos.x, 1);
		originPos.y = MATH_RoundFloatDecimals(originPos.y, 1);
		if (!CRATE_EXISTS_DISABLED_LIST(sceneName, objectName, originPos))
		{
			coreGameData.listCrateDisabled.Add(new CrateDisabled
			{
				sceneName = sceneName,
				objName = objectName,
				originPos = originPos
			});
		}
	}

	public bool CRATE_EXISTS_DISABLED_LIST(string sceneName, string objectName, Vector2 originPos)
	{
		if (coreGameData.listCrateDisabled.Exists((CrateDisabled x) => x.sceneName == sceneName && x.objName == objectName && MATH_ComparePositionsAddMargin(x.originPos, originPos, 0.2f, -0.2f, 0.2f, -0.2f)))
		{
			return true;
		}
		return false;
	}

	public void HACK_CHECK_REDUNDANCE_GALLERY_ENABLE_ROOMS()
	{
		if (GalleryValidateRoomAndView(GalleryThemeType.Forest) && !configGalleryRoomForest)
		{
			configGalleryRoomForest = true;
		}
		if (GalleryValidateRoomAndView(GalleryThemeType.Beach) && !configGalleryRoomBeach)
		{
			configGalleryRoomBeach = true;
		}
		if (GalleryValidateRoomAndView(GalleryThemeType.Hill) && !configGalleryRoomHill)
		{
			configGalleryRoomHill = true;
		}
		if (GalleryValidateRoomAndView(GalleryThemeType.City) && !configGalleryRoomCity)
		{
			configGalleryRoomCity = true;
		}
		if (GalleryValidateRoomAndView(GalleryThemeType.Castle) && !configGalleryRoomCastle)
		{
			configGalleryRoomCastle = true;
		}
		if (GalleryValidateRoomAndView(GalleryThemeType.Cave) && !configGalleryRoomCaves)
		{
			configGalleryRoomCaves = true;
		}
		if (GalleryValidateRoomAndView(GalleryThemeType.Swamp) && !configGalleryRoomSwamp)
		{
			configGalleryRoomSwamp = true;
		}
		if (GalleryValidateRoomAndView(GalleryThemeType.HighTech) && !configGalleryRoomHighTech)
		{
			configGalleryRoomHighTech = true;
		}
		SaveGameConfiguration();
	}
}
