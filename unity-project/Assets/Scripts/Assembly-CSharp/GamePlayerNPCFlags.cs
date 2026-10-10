using System;

[Serializable]
public class GamePlayerNPCFlags
{
	public int currentNeneStoryDialogueID;

	public int currentFarinaStoryDialogueID;

	public GamePlayerNPCFlags()
	{
		currentNeneStoryDialogueID = 1;
		currentFarinaStoryDialogueID = 1;
	}

	public GamePlayerNPCFlags(GamePlayerNPCFlags copyValues)
	{
		currentNeneStoryDialogueID = copyValues.currentNeneStoryDialogueID;
		currentFarinaStoryDialogueID = copyValues.currentFarinaStoryDialogueID;
	}
}
