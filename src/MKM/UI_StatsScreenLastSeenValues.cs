using System;

[Serializable]
public class UI_StatsScreenLastSeenValues
{
	public int Health;

	public float Spirit;

	public int Attack;

	public int Defense;

	public int Quartz;

	public UI_StatsScreenLastSeenValues()
	{
		SaveValues(0, 0f, 0, 0, 0);
	}

	public void SaveValues(int hp, float sp, int atk, int def, int quartz)
	{
		Health = hp;
		Spirit = sp;
		Attack = atk;
		Defense = def;
		Quartz = quartz;
	}
}
