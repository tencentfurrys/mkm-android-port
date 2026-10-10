using System;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class UI_MainGameplay
{
	public Image HealthBarFill;

	public Image DurabilityBarFill;

	public Image SpiritBarFill;

	public Text HealthValues;

	public Text DurabilityValues;

	public Text SpiritValues;

	public Text QuartzValues;

	public GameObject objActiveItemContainer;

	public Image imgActiveItemIcon;

	public Image imgItemNext;

	public Image imgItemPrevious;

	public Text txtActiveItemAmount;
}
