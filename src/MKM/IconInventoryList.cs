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
		return id switch
		{
			1 => IconConsumables.smallHealth, 
			2 => IconConsumables.medHealth, 
			3 => IconConsumables.bigHealth, 
			4 => IconConsumables.smallSpirit, 
			5 => IconConsumables.medSpirit, 
			6 => IconConsumables.bigSpirit, 
			20 => IconConsumables.brewLife, 
			21 => IconConsumables.brewMana, 
			22 => IconConsumables.brewATK, 
			23 => IconConsumables.brewDEF, 
			24 => IconConsumables.brewSPEED, 
			25 => IconConsumables.eatFruitSlice, 
			26 => IconConsumables.eatFruitForbid, 
			27 => IconConsumables.eatTuberDistract, 
			28 => IconConsumables.eatWarriorMeat, 
			29 => IconConsumables.eatHeroMeal, 
			33 => IconConsumables.aspartame, 
			8 => IconGear.throwDagger, 
			9 => IconGear.throwSmallRuneBomb, 
			15 => IconGear.metalDust, 
			16 => IconGear.freshClothing, 
			17 => IconGear.throwLance, 
			18 => IconGear.throwBoomerangStar, 
			19 => IconGear.throwHomingSphere, 
			30 => IconGear.poisonPownder, 
			31 => IconGear.ragePepper, 
			32 => IconGear.entropicBomb, 
			50 => IconSkills.skillDash, 
			51 => IconSkills.skillSlidingTackle, 
			52 => IconSkills.skillShield, 
			53 => IconSkills.skillFlameSword, 
			54 => IconSkills.skillSlash, 
			55 => IconSkills.skillGroundPound, 
			56 => IconSkills.skillDrillDash, 
			100 => IconKey.keyCrimsonScepter, 
			101 => IconKey.keySilverCastleKey, 
			102 => IconKey.keyDungeonKey, 
			103 => IconKey.key01HillValley, 
			500 => IconAccessories.ring_HP1, 
			501 => IconAccessories.ring_HP2, 
			502 => IconAccessories.ring_HP3, 
			503 => IconAccessories.ring_SP1, 
			504 => IconAccessories.ring_SP2, 
			505 => IconAccessories.ring_SP3, 
			506 => IconAccessories.ring_ATK1, 
			507 => IconAccessories.ring_ATK2, 
			508 => IconAccessories.ring_ATK3, 
			509 => IconAccessories.ring_DEF1, 
			510 => IconAccessories.ring_DEF2, 
			511 => IconAccessories.ring_DEF3, 
			550 => IconAccessories.bracelet_HunterWhip, 
			551 => IconAccessories.bracelet_ShieldCrouch, 
			552 => IconAccessories.bracelet_cheapstakeDiscount, 
			553 => IconAccessories.bracelet_cursedHorned, 
			554 => IconAccessories.bracelet_Chastity, 
			555 => IconAccessories.bracelet_JoyDevice, 
			600 => IconAccessories.necklace_PhaseAttack, 
			601 => IconAccessories.necklace_SKILLEnhancedPowerSlash, 
			602 => IconAccessories.necklace_SKILLEnhancedDefenseShield, 
			603 => IconAccessories.necklace_SKILLEnhancedSlidingTackle, 
			604 => IconAccessories.necklace_SKILLEnhancedFlameSword, 
			650 => IconAccessories.earring_Glass, 
			651 => IconAccessories.earring_SmallSlash, 
			652 => IconAccessories.earring_masterDash, 
			653 => IconAccessories.earring_bladeStun, 
			700 => IconAccessories.hairpin_EagerSpiritStun, 
			701 => IconAccessories.hairpin_DoubleJump, 
			702 => IconAccessories.hairpin_EroticDancer, 
			703 => IconAccessories.hairpin_HeadSuccubus, 
			704 => IconAccessories.hairpin_HairlockFertility, 
			705 => IconAccessories.hairpin_DaggerSpecialist, 
			900 => IconEquip.coreEquipBelt, 
			901 => IconEquip.coreEquipSword, 
			902 => IconEquip.coreEquipBoots, 
			903 => IconEquip.coreEquipPendant, 
			904 => IconEquip.coreEquipBeltSlot, 
			908 => IconEquip.coreEquipMayaCoreDNA, 
			920 => IconEquip.coreTraitMasoc, 
			921 => IconEquip.coreTraitExib, 
			922 => IconEquip.coreTraitLust, 
			923 => IconEquip.coreTraitNympho, 
			924 => IconEquip.coreTraitVoreWhore, 
			7 => IconMiscUpgrades.upAttack, 
			10 => IconMiscUpgrades.upHealth, 
			11 => IconMiscUpgrades.upSpirit, 
			12 => IconMiscUpgrades.upDefense, 
			905 => IconMiscUpgrades.tokenPotion, 
			906 => IconMiscUpgrades.tokenDagger, 
			907 => IconMiscUpgrades.tokenRuneBomb, 
			909 => IconMiscUpgrades.galleryBeach, 
			910 => IconMiscUpgrades.galleryForest, 
			911 => IconMiscUpgrades.galleryCastle, 
			912 => IconMiscUpgrades.galleryHill, 
			913 => IconMiscUpgrades.galleryCity, 
			914 => IconMiscUpgrades.galleryCaves, 
			915 => IconMiscUpgrades.gallerySwamp, 
			916 => IconMiscUpgrades.galleryHighTech, 
			9999 => IconMiscUpgrades.shopSlotEmpty, 
			_ => null, 
		};
	}
}
