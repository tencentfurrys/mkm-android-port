using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New TalkNPCSpecial List", menuName = "TalkNPCSpecial List")]
public class ScobTalkNPCSpecial : ScriptableObject
{
	public List<ClassNPCTalkDiag> listOfFarinaTalk;

	public List<ClassNPCTalkDiag> listOfHarunaTalk;

	private GameCore GC;

	public List<int> GetNPCTalkSpecialList(GameCore.NPCType whichNPC)
	{
		List<int> list = new List<int>();
		GC = GameCore.Instance;
		switch (whichNPC)
		{
		case GameCore.NPCType.FarinaSupport:
			list = NPCFarinaShortList();
			break;
		case GameCore.NPCType.NeneShop:
			list = NPCHarunaShortList();
			break;
		}
		list.Sort();
		return list;
	}

	private List<int> NPCFarinaShortList()
	{
		List<int> list = new List<int>();
		foreach (ClassNPCTalkDiag item in listOfFarinaTalk)
		{
			bool flag = false;
			switch (item.idDialogue)
			{
			case 100000:
			case 100001:
				if (GC.flagsMain.seenEventBeach01)
				{
					flag = true;
				}
				break;
			case 100002:
				if (GC.flagsMain.isForestBossDefeatd)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				list.Add(item.idDialogue);
			}
		}
		return list;
	}

	private List<int> NPCHarunaShortList()
	{
		List<int> list = new List<int>();
		foreach (ClassNPCTalkDiag item in listOfHarunaTalk)
		{
			bool flag = false;
			switch (item.idDialogue)
			{
			case 100000:
			case 100001:
			case 100002:
				if (GC.flagsMain.seenEventBeach01)
				{
					flag = true;
				}
				break;
			case 100003:
				if (GC.flagsMain.isForestBossDefeatd)
				{
					flag = true;
				}
				break;
			}
			if (flag)
			{
				list.Add(item.idDialogue);
			}
		}
		return list;
	}
}
