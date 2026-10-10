using System.Collections;
using UnityEngine;

public class NpcLogic : MonoBehaviour
{
	public GameCore.NPCType currentNPCType;

	public int idSpecificDialogue;

	public bool canFlipSprite;

	public bool isZoomInOnDialogue;

	public GameObject objIconToDisplay;

	public GameObject specialObject;

	private GameCore GC;

	private SpriteRenderer objRE;

	private Collider2D selfCollider2D;

	private Coroutine crtTutorialTimer;

	private Color originalSelfColor;

	private bool hasChildIcon;

	private bool interactionTutorial;

	private void Start()
	{
		GC = GameCore.Instance;
		objRE = GetComponent<SpriteRenderer>();
		if (canFlipSprite)
		{
			InvokeRepeating("NPC_Repeating_Behaviour", 0f, 0.2f);
		}
		hasChildIcon = false;
		ValidateIcon();
		selfCollider2D = GetComponent<Collider2D>();
		originalSelfColor = objRE.color;
		if (currentNPCType == GameCore.NPCType.OBJ_Tutorial)
		{
			crtTutorialTimer = StartCoroutine(waitTutorialTimer(0.5f));
		}
	}

	private void Update()
	{
		if (GC.playerIsHurt || GC.isGamePaused || GC.playerIsBeingGrabed || GC.isBossFight || GC.isBossRematch || GC.isPlayerFainted || GC.eventIsPlaying)
		{
			if (selfCollider2D != null && selfCollider2D.enabled)
			{
				selfCollider2D.enabled = false;
			}
		}
		else if (selfCollider2D != null && !selfCollider2D.enabled)
		{
			selfCollider2D.enabled = true;
		}
		if (currentNPCType == GameCore.NPCType.OBJ_Tutorial && GC.eventIsPlaying && objRE.color.a != 0f)
		{
			Color color = objRE.color;
			color.a = 0f;
			objRE.color = color;
		}
		if (currentNPCType == GameCore.NPCType.OBJ_Tutorial && !GC.eventIsPlaying && objRE.color != originalSelfColor)
		{
			objRE.color = originalSelfColor;
		}
	}

	private void ValidateIcon()
	{
		foreach (Transform item in transform)
		{
			if (item.name.Contains("Icon"))
			{
				hasChildIcon = true;
			}
		}
		if ((bool)objIconToDisplay && !hasChildIcon)
		{
			Debug.Log("NPC Object [" + gameObject.name + "] has an icon prefab attributed in the script, but has no child named 'Icon'! It should have a child ICON prefab and then attribute that child to the script.");
		}
		else if (!objIconToDisplay && hasChildIcon)
		{
			Debug.Log("NPC Object [" + gameObject.name + "] has an icon prefab as child, but no object was attributeted in the script! That local child should be attributed to the script.");
		}
	}

	private void NPC_Repeating_Behaviour()
	{
		if (!canFlipSprite)
		{
			return;
		}
		if (objRE.flipX)
		{
			if (GC.playerCurrentWorldXPosition >= transform.position.x)
			{
				objRE.flipX = false;
			}
		}
		else if (GC.playerCurrentWorldXPosition < transform.position.x)
		{
			objRE.flipX = true;
		}
	}

	private void NPC_Repeating_Check_For_Player_Interaction()
	{
		if (GC.isPlayerInteractingLocked && objIconToDisplay.activeInHierarchy)
		{
			objIconToDisplay.SetActive(value: false);
		}
		else if (!GC.isPlayerInteractingLocked && !objIconToDisplay.activeInHierarchy)
		{
			objIconToDisplay.SetActive(value: true);
		}
	}

	private void CheckForPlayerInteraction(bool start)
	{
		if (start)
		{
			objIconToDisplay.SetActive(value: true);
			InvokeRepeating("NPC_Repeating_Check_For_Player_Interaction", 0f, 0.1f);
		}
		else
		{
			CancelInvoke("NPC_Repeating_Check_For_Player_Interaction");
			objIconToDisplay.SetActive(value: false);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.tag == "Player" && (((bool)GC && !GC.isPlayerFainted) || !GC) && (bool)objIconToDisplay)
		{
			CheckForPlayerInteraction(start: true);
		}
		if (interactionTutorial && other.tag == "Player" && currentNPCType == GameCore.NPCType.OBJ_Tutorial)
		{
			TutorialToggleMessage(setState: true);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.tag == "Player" && (bool)objIconToDisplay)
		{
			CheckForPlayerInteraction(start: false);
		}
		if (interactionTutorial && other.tag == "Player" && currentNPCType == GameCore.NPCType.OBJ_Tutorial)
		{
			TutorialToggleMessage(setState: false);
		}
	}

	private void TutorialToggleMessage(bool setState)
	{
		if (!GC.eventIsPlaying && !GC.isSceneTransitioning)
		{
			GC.DisplayTutorialMessage(setState, idSpecificDialogue);
		}
	}

	private IEnumerator waitTutorialTimer(float wait)
	{
		yield return new WaitForSeconds(wait);
		interactionTutorial = true;
		crtTutorialTimer = null;
	}
}
