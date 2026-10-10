using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_NeneMenu
{
	public GameObject objMainNode;

	public GameObject objTalkNode;

	public GameObject objTalkSubHolder;

	public GameObject objShopNode;

	public Transform parentDiagHolder;

	public ScobTalkNPCSpecial SobjDiagList;

	public Button btnBackFromTalk;

	public Button btnShop;

	public Button btnConsumables;

	public Button btnBackFromShop;

	public Text txtShop;

	public Text txtTalk;

	public Text txtExit;

	public Text labelEssHeld;

	public Text txtEssenceHeld;

	public Text txtItemOwned;

	public Text txtReturnFromTalk;

	public Text txtReturnFromShop;

	public Text txtPriceBought;

	public Text txtPriceBought2;
}
