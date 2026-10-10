using UnityEngine;

public class EssenceAttractor : MonoBehaviour
{
	private float pulseRate = 0.1f;

	private bool signalAttraction;

	private int countAttract;

	private int maxAttractPerPulse = 10;

	private Collider2D selfColl;

	private void OnEnable()
	{
		if (!selfColl)
		{
			selfColl = GetComponent<Collider2D>();
		}
		if (!IsInvoking("INVOKED_PulseToAttractOrbs"))
		{
			InvokeRepeating("INVOKED_PulseToAttractOrbs", pulseRate, pulseRate);
		}
	}

	private void OnDisable()
	{
		CancelInvoke();
		signalAttraction = false;
		countAttract = 0;
	}

	private void INVOKED_PulseToAttractOrbs()
	{
		signalAttraction = !signalAttraction;
		if (signalAttraction)
		{
			selfColl.enabled = true;
			return;
		}
		selfColl.enabled = false;
		countAttract = 0;
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (signalAttraction && countAttract < maxAttractPerPulse && other.gameObject.tag == "Item-Essence")
		{
			CollectibleLogic component = other.GetComponent<CollectibleLogic>();
			if (!component.orbEssenceChasePlayer)
			{
				countAttract++;
				component.SignalThisEssenceOrbAttraction();
			}
		}
	}
}
