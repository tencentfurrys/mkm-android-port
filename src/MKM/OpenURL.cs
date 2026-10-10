using UnityEngine;

public class OpenURL : MonoBehaviour
{
	public string urlAdress;

	public void ClickToOpenURL()
	{
		Application.OpenURL(urlAdress);
	}
}
