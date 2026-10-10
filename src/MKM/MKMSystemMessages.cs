public class MKMSystemMessages
{
	public string strMessage;

	public float startTime;

	public float endTime;

	public MKMSystemMessages()
	{
		Reset();
	}

	public void Reset()
	{
		strMessage = "";
		startTime = 0f;
		endTime = 0f;
	}
}
