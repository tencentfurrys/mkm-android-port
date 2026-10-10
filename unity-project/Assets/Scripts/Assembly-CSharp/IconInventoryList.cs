using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Icon List", menuName = "Iventory Icon List")]
public class IconInventoryList : ScriptableObject
{
	public InvIconGroupConsumables IconConsumables;

	public InvIconGroupGearEquipable IconGear;

	public InvIconGroupSkills IconSkills;

	public InvIconGroupKeys IconKey;

	public InvIconGroupCoreEquipment IconEquip;

	public InvIconGroupUpgrades IconMiscUpgrades;

	public InvIconGroupAccessories IconAccessories;

	public Sprite GetIconFromID(int id)
	{
		switch (id)
		{
		default:
			return null;
		case 1:
			return IconConsumables.smallHealth;
		case 2:
			return IconConsumables.medHealth;
		case 3:
			return IconConsumables.bigHealth;
		case 4:
			return IconConsumables.smallSpirit;
		case 5:
			return IconConsumables.medSpirit;
		case 6:
			return IconConsumables.bigSpirit;
		case 20:
			return IconConsumables.brewLife;
		case 21:
			return IconConsumables.brewMana;
		case 22:
			return IconConsumables.brewATK;
		case 23:
			return IconConsumables.brewDEF;
		case 24:
			return IconConsumables.brewSPEED;
		case 25:
			return IconConsumables.eatFruitSlice;
		case 26:
			return IconConsumables.eatFruitForbid;
		case 27:
			return IconConsumables.eatTuberDistract;
		case 28:
			return IconConsumables.eatWarriorMeat;
		case 29:
			return IconConsumables.eatHeroMeal;
		case 33:
			return IconConsumables.aspartame;
		case 8:
			return IconGear.throwDagger;
		case 9:
			return IconGear.throwSmallRuneBomb;
		case 15:
			return IconGear.metalDust;
		case 16:
			return IconGear.freshClothing;
		case 17:
			return IconGear.throwLance;
		case 18:
			return IconGear.throwBoomerangStar;
		case 19:
			return IconGear.throwHomingSphere;
		case 30:
			return IconGear.poisonPownder;
		case 31:
			return IconGear.ragePepper;
		case 32:
			return IconGear.entropicBomb;
		case 50:
			return IconSkills.skillDash;
		case 51:
			return IconSkills.skillSlidingTackle;
		case 52:
			return IconSkills.skillShield;
		case 53:
			return IconSkills.skillFlameSword;
		case 54:
			return IconSkills.skillSlash;
		case 55:
			return IconSkills.skillGroundPound;
		case 56:
			return IconSkills.skillDrillDash;
		case 100:
			return IconKey.keyCrimsonScepter;
		case 101:
			return IconKey.keySilverCastleKey;
		case 102:
			return IconKey.keyDungeonKey;
		case 103:
			return IconKey.key01HillValley;
		case 500:
			return IconAccessories.ring_HP1;
		case 501:
			return IconAccessories.ring_HP2;
		case 502:
			return IconAccessories.ring_HP3;
		case 503:
			return IconAccessories.ring_SP1;
		case 504:
			return IconAccessories.ring_SP2;
		case 505:
			return IconAccessories.ring_SP3;
		case 506:
			return IconAccessories.ring_ATK1;
		case 507:
			return IconAccessories.ring_ATK2;
		case 508:
			return IconAccessories.ring_ATK3;
		case 509:
			return IconAccessories.ring_DEF1;
		case 510:
			return IconAccessories.ring_DEF2;
		case 511:
			return IconAccessories.ring_DEF3;
		case 550:
			return IconAccessories.bracelet_HunterWhip;
		case 551:
			return IconAccessories.bracelet_ShieldCrouch;
		case 552:
			return IconAccessories.bracelet_cheapstakeDiscount;
		case 553:
			return IconAccessories.bracelet_cursedHorned;
		case 554:
			return IconAccessories.bracelet_Chastity;
		case 555:
			return IconAccessories.bracelet_JoyDevice;
		case 600:
			return IconAccessories.necklace_PhaseAttack;
		case 601:
			return IconAccessories.necklace_SKILLEnhancedPowerSlash;
		case 602:
			return IconAccessories.necklace_SKILLEnhancedDefenseShield;
		case 603:
			return IconAccessories.necklace_SKILLEnhancedSlidingTackle;
		case 604:
			return IconAccessories.necklace_SKILLEnhancedFlameSword;
		case 650:
			return IconAccessories.earring_Glass;
		case 651:
			return IconAccessories.earring_SmallSlash;
		case 652:
			return IconAccessories.earring_masterDash;
		case 653:
			return IconAccessories.earring_bladeStun;
		case 700:
			return IconAccessories.hairpin_EagerSpiritStun;
		case 701:
			return IconAccessories.hairpin_DoubleJump;
		case 702:
			return IconAccessories.hairpin_EroticDancer;
		case 703:
			return IconAccessories.hairpin_HeadSuccubus;
		case 704:
			return IconAccessories.hairpin_HairlockFertility;
		case 705:
			return IconAccessories.hairpin_DaggerSpecialist;
		case 900:
			return IconEquip.coreEquipBelt;
		case 901:
			return IconEquip.coreEquipSword;
		case 902:
			return IconEquip.coreEquipBoots;
		case 903:
			return IconEquip.coreEquipPendant;
		case 904:
			return IconEquip.coreEquipBeltSlot;
		case 908:
			return IconEquip.coreEquipMayaCoreDNA;
		case 920:
			return IconEquip.coreTraitMasoc;
		case 921:
			return IconEquip.coreTraitExib;
		case 922:
			return IconEquip.coreTraitLust;
		case 923:
			return IconEquip.coreTraitNympho;
		case 924:
			return IconEquip.coreTraitVoreWhore;
		case 7:
			return IconMiscUpgrades.upAttack;
		case 10:
			return IconMiscUpgrades.upHealth;
		case 11:
			return IconMiscUpgrades.upSpirit;
		case 12:
			return IconMiscUpgrades.upDefense;
		case 905:
			return IconMiscUpgrades.tokenPotion;
		case 906:
			return IconMiscUpgrades.tokenDagger;
		case 907:
			return IconMiscUpgrades.tokenRuneBomb;
		case 909:
			return IconMiscUpgrades.galleryBeach;
		case 910:
			return IconMiscUpgrades.galleryForest;
		case 911:
			return IconMiscUpgrades.galleryCastle;
		case 912:
			return IconMiscUpgrades.galleryHill;
		case 913:
			return IconMiscUpgrades.galleryCity;
		case 914:
			return IconMiscUpgrades.galleryCaves;
		case 915:
			return IconMiscUpgrades.gallerySwamp;
		case 916:
			return IconMiscUpgrades.galleryHighTech;
		case 9999:
			return IconMiscUpgrades.shopSlotEmpty;
		}
	}
}
