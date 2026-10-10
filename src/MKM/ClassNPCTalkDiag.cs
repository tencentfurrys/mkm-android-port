using System;

[Serializable]
public class ClassNPCTalkDiag
{
	public int idDialogue;

	public ClassNPCTalkDiag()
	{
		idDialogue = 0;
	}

	public ClassNPCTalkDiag(ClassNPCTalkDiag copyValues)
	{
		idDialogue = copyValues.idDialogue;
	}
}
