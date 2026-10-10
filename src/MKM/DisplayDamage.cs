using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DisplayDamage : MonoBehaviour
{
	public int dmgPrimary;

	public int dmgOptional;

	public bool showBlockedDamage;

	public Color colorToDisplay;

	public Color colorToDisplayShadow;

	public Canvas localCanvas;

	public Text txtDmgNumbers;

	public Text txtDmgNumbersShadow;

	public Text txtDmgBlockedNumbers;

	public Text txtDmgBlockedNumbersShadow;

	public float durationOfDisplay;

	public string strPrefix;

	public string strSufix;

	private float jumpForce;

	private float randDirection;

	private float randGravity;

	private float currentGravity;

	private bool runVisualEffect;

	private Coroutine coroutTextDamage;

	private void Start()
	{
		InitiateVariables();
		BeginDisplayBehaviour(dmgPrimary, dmgOptional);
	}

	private void InitiateVariables()
	{
		txtDmgNumbers.color = colorToDisplay;
		txtDmgNumbersShadow.color = colorToDisplayShadow;
		txtDmgBlockedNumbersShadow.color = Color.black;
		jumpForce = 4f;
		runVisualEffect = false;
		if (durationOfDisplay <= 0f)
		{
			durationOfDisplay = 0.75f;
		}
		randDirection = Random.Range(-2f, 2f);
		randGravity = Random.Range(-0.1f, -0.2f);
		currentGravity = randGravity;
	}

	private void FixedUpdate()
	{
		if (runVisualEffect)
		{
			Vector2 vector = txtDmgNumbersShadow.transform.localPosition;
			currentGravity += randGravity;
			vector.x += randDirection;
			vector.y += jumpForce + currentGravity;
			txtDmgNumbersShadow.transform.localPosition = vector;
		}
	}

	private void BeginDisplayBehaviour(int amountDMG, int amountBlockedDMG)
	{
		if (coroutTextDamage != null)
		{
			StopCoroutine(coroutTextDamage);
		}
		localCanvas.gameObject.SetActive(value: true);
		txtDmgNumbers.text = strPrefix + amountDMG + strSufix;
		txtDmgNumbersShadow.text = strPrefix + amountDMG + strSufix;
		txtDmgBlockedNumbers.text = "(" + amountBlockedDMG + ")";
		txtDmgBlockedNumbersShadow.text = "(" + amountBlockedDMG + ")";
		txtDmgNumbers.gameObject.SetActive(value: true);
		txtDmgNumbersShadow.gameObject.SetActive(value: true);
		if (showBlockedDamage)
		{
			txtDmgBlockedNumbers.gameObject.SetActive(value: true);
			txtDmgBlockedNumbersShadow.gameObject.SetActive(value: true);
		}
		coroutTextDamage = StartCoroutine(waitForDmgTextEffect(durationOfDisplay));
	}

	private IEnumerator waitForDmgTextEffect(float waittime)
	{
		runVisualEffect = true;
		yield return new WaitForSeconds(waittime);
		runVisualEffect = false;
		txtDmgNumbers.gameObject.SetActive(value: false);
		txtDmgNumbersShadow.gameObject.SetActive(value: false);
		localCanvas.gameObject.SetActive(value: false);
		coroutTextDamage = null;
		gameObject.SetActive(value: false);
	}
}
