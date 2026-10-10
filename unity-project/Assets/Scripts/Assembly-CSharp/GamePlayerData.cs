using System.Collections.Generic;
using UnityEngine;

public class GamePlayerData
{
	public string strDateTime;

	public Vector3 vecPlayerSavePosition;

	public string strPlayerSaveScene;

	public float intPlayerCurrentHealth;

	public float intPlayerMaxHealth;

	public float intPlayerCurrentSpirit;

	public float intPlayerMaxSpirit;

	public float intPlayerBaseSpiritRegeneration;

	public int intDiscountShop;

	public int intPlayerCurrentDressID;

	public int intPlayerCurrentDressState;

	public int intPlayerCurrentDressStateValidUnbroken;

	public int intPlayerMaxDressDurability;

	public int intPlayerCurrentDressDurability;

	public int intPlayerCurrentEssenceAmount;

	public int intPlayerCurrentBasicAtackPower;

	public int intPlayerCurrentBasicAtackVariation;

	public int intPlayerCurrentBasicDefensePower;

	public int intIndexOfSlotCurrentActiveItem;

	public int intLustAmount;

	public string versionName;

	public float fltGameVersion;

	public int idSavePointImage;

	public int timeGamePlayed;

	public int permaTraitCountMasoch;

	public int permaTraitCountExib;

	public int permaTraitCountHorny;

	public int permaTraitCountNympho;

	public List<int> listOfVisitedSavePoints;

	public List<ItemInfo> listOfItemInventory;

	public List<LogPurchase> logPurchase;

	public CountItemsQuantity[] arrayBeltSlots;

	public GamePlayerFlags gameFlags;

	public GamePlayerSkillFlags skillFlags;

	public GamePlayerNPCFlags npcFlags;

	public List<GameLeverSignalState> listOfSignalState;

	public List<ClassNPCTalkDiag> listSeenDialogueFarina;

	public List<ClassNPCTalkDiag> listSeenDialogueNene;

	public List<CrateDisabled> listCrateDisabled;

	public int defaultMaxHealth => 100;

	public int defaultMaxSpirit => 100;

	public int defaultBaseAttack => 25;

	public int defaultBaseDefense => 10;

	public float defaultSpiritRegen => 20f;

	public int defaultInfiniteShopStock => 999;

	public GamePlayerData()
	{
		intPlayerMaxHealth = defaultMaxHealth;
		intPlayerCurrentHealth = intPlayerMaxHealth;
		intPlayerMaxSpirit = defaultMaxSpirit;
		intPlayerCurrentSpirit = intPlayerMaxSpirit;
		intPlayerBaseSpiritRegeneration = defaultSpiritRegen;
		intPlayerCurrentDressID = 0;
		intPlayerCurrentDressState = 0;
		intPlayerCurrentDressStateValidUnbroken = 0;
		intPlayerMaxDressDurability = 100;
		intPlayerCurrentDressDurability = intPlayerMaxDressDurability;
		intPlayerCurrentBasicAtackPower = defaultBaseAttack;
		intPlayerCurrentBasicDefensePower = defaultBaseDefense;
		intPlayerCurrentBasicAtackVariation = 0;
		intPlayerCurrentEssenceAmount = 0;
		intLustAmount = 0;
		intDiscountShop = 0;
		intIndexOfSlotCurrentActiveItem = 0;
		strPlayerSaveScene = "DefaultData";
		vecPlayerSavePosition = Vector3.zero;
		strPlayerSaveScene = "Null";
		versionName = "Version.Default";
		fltGameVersion = 0f;
		idSavePointImage = 0;
		timeGamePlayed = 0;
		permaTraitCountMasoch = 0;
		permaTraitCountExib = 0;
		permaTraitCountHorny = 0;
		permaTraitCountNympho = 0;
		listOfVisitedSavePoints = new List<int> { 2 };
		listOfItemInventory = new List<ItemInfo>();
		logPurchase = new List<LogPurchase>();
		arrayBeltSlots = new CountItemsQuantity[8];
		gameFlags = new GamePlayerFlags();
		skillFlags = new GamePlayerSkillFlags();
		npcFlags = new GamePlayerNPCFlags();
		listOfSignalState = new List<GameLeverSignalState>();
		listSeenDialogueFarina = new List<ClassNPCTalkDiag>();
		listSeenDialogueNene = new List<ClassNPCTalkDiag>();
		listCrateDisabled = new List<CrateDisabled>();
	}

	public GamePlayerData DeepCopy()
	{
		GamePlayerData gamePlayerData = (GamePlayerData)MemberwiseClone();
		gamePlayerData.listOfVisitedSavePoints = new List<int>(listOfVisitedSavePoints);
		gamePlayerData.listOfItemInventory = new List<ItemInfo>(listOfItemInventory);
		gamePlayerData.logPurchase = new List<LogPurchase>(logPurchase);
		gamePlayerData.arrayBeltSlots = new CountItemsQuantity[8];
		for (int i = 0; i < arrayBeltSlots.Length; i++)
		{
			if (arrayBeltSlots[i] != null)
			{
				gamePlayerData.arrayBeltSlots[i] = new CountItemsQuantity
				{
					id = arrayBeltSlots[i].id,
					quantity = arrayBeltSlots[i].quantity
				};
			}
		}
		gamePlayerData.gameFlags = new GamePlayerFlags(gameFlags);
		gamePlayerData.skillFlags = new GamePlayerSkillFlags(skillFlags);
		gamePlayerData.npcFlags = new GamePlayerNPCFlags(npcFlags);
		gamePlayerData.listOfSignalState = new List<GameLeverSignalState>(listOfSignalState);
		gamePlayerData.listSeenDialogueFarina = new List<ClassNPCTalkDiag>(listSeenDialogueFarina);
		gamePlayerData.listSeenDialogueNene = new List<ClassNPCTalkDiag>(listSeenDialogueNene);
		gamePlayerData.listCrateDisabled = new List<CrateDisabled>(listCrateDisabled);
		return gamePlayerData;
	}

	public List<ItemInfo> GetDefaultStateForNeneShop()
	{
		return new List<ItemInfo>
		{
			new ItemInfo
			{
				id = 6,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 50
			},
			new ItemInfo
			{
				id = 3,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 50
			},
			new ItemInfo
			{
				id = 20,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 21,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 22,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 23,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 24,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 25,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 26,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 27,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 28,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 29,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 75
			},
			new ItemInfo
			{
				id = 33,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 50
			},
			new ItemInfo
			{
				id = 15,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 100
			},
			new ItemInfo
			{
				id = 16,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 750
			},
			new ItemInfo
			{
				id = 9,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 5
			},
			new ItemInfo
			{
				id = 8,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 2
			},
			new ItemInfo
			{
				id = 17,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 2
			},
			new ItemInfo
			{
				id = 18,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 2
			},
			new ItemInfo
			{
				id = 19,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 2
			},
			new ItemInfo
			{
				id = 30,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 2
			},
			new ItemInfo
			{
				id = 31,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 2
			},
			new ItemInfo
			{
				id = 32,
				shopStock = defaultInfiniteShopStock,
				shopPrice = 2
			},
			new ItemInfo
			{
				id = 7,
				shopStock = 3,
				shopPrice = 500
			},
			new ItemInfo
			{
				id = 10,
				shopStock = 5,
				shopPrice = 500
			},
			new ItemInfo
			{
				id = 11,
				shopStock = 5,
				shopPrice = 500
			},
			new ItemInfo
			{
				id = 12,
				shopStock = 3,
				shopPrice = 500
			},
			new ItemInfo
			{
				id = 904,
				shopStock = 2,
				shopPrice = 1000
			},
			new ItemInfo
			{
				id = 905,
				shopStock = 2,
				shopPrice = 250
			},
			new ItemInfo
			{
				id = 906,
				shopStock = 1,
				shopPrice = 250
			},
			new ItemInfo
			{
				id = 907,
				shopStock = 1,
				shopPrice = 250
			},
			new ItemInfo
			{
				id = 909,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 910,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 911,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 912,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 913,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 914,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 915,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 916,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 500,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 501,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 502,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 503,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 504,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 505,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 506,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 507,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 508,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 509,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 510,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 511,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 550,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 551,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 552,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 553,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 554,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 555,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 600,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 601,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 602,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 603,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 604,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 650,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 651,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 652,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 653,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 700,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 701,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 702,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 703,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 704,
				shopStock = 1,
				shopPrice = 10
			},
			new ItemInfo
			{
				id = 705,
				shopStock = 1,
				shopPrice = 10
			}
		};
	}
}
