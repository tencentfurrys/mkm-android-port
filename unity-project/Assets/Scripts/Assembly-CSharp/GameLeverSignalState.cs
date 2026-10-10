using System;

[Serializable]
public class GameLeverSignalState
{
	public string sceneName;

	public int orderLeverID;

	public bool signalState;

	public GameLeverSignalState()
	{
		sceneName = "";
		orderLeverID = 0;
		signalState = false;
	}

	public GameLeverSignalState(GameLeverSignalState copyValues)
	{
		sceneName = copyValues.sceneName;
		orderLeverID = copyValues.orderLeverID;
		signalState = copyValues.signalState;
	}
}
