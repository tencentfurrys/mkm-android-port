using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_FarinaMenu
{
	public GameObject objMainNode;

	public GameObject objTalkNode;

	public GameObject objTalkSubHolder;

	public Transform parentDiagHolder;

	public ScobTalkNPCSpecial SobjDiagList;

	public Button btnBackFromTalk;

	public Button btnTalk;

	public Text txtTalk;

	public Text txtHeal;

	public Text txtRepair;

	public Text txtHExit;

	public Text txtReturn;
}
