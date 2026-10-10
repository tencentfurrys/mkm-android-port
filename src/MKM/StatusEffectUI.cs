using UnityEngine;
using UnityEngine.UI;

public class StatusEffectUI : MonoBehaviour
{
	public Image imgIcon;

	public Image imgTimerShade;

	public Sprite setThisSpriteIcon;

	public GameCore.TempPlayerEffect thisStatusEffect;

	public float countUp;

	public float maxDuration;

	private const string nameInvoked = "INVOKED_TimerCountDown";

	private const float repeatFrequency = 0.05f;

	private GameCore GC;

	private void Start()
	{
		GC = GameCore.Instance;
		imgIcon.sprite = setThisSpriteIcon;
		if (maxDuration > 0f)
		{
			InvokeRepeating("INVOKED_TimerCountDown", 0f, 0.05f);
		}
	}

	private void INVOKED_TimerCountDown()
	{
		countUp += 0.05f;
		imgTimerShade.fillAmount = countUp / maxDuration;
		if (countUp > maxDuration || ((bool)GC && !GC.ExistsEffectOnPlayer(thisStatusEffect)))
		{
			EndStatusEffectIconLogic();
		}
	}

	private void EndStatusEffectIconLogic()
	{
		CancelInvoke("INVOKED_TimerCountDown");
		gameObject.SetActive(value: false);
		Object.Destroy(gameObject);
	}
}
