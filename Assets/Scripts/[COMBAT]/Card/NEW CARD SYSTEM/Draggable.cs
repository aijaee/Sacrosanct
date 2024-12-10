using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [HideInInspector]
    public Transform parentToReturnTo = null;
    [HideInInspector]
    public Transform placeHolderParent = null;

    private GameObject placeHolder = null;

    // Reference to the SpellManager (you should assign this in the inspector)
    public SpellManager spellManager;

    // Use the same SpellType enum from SpellManager
    public SpellManager.SpellType spellType;

    // Reference to the UnitScript (you should assign this in the inspector)
    public UnitScript unitScript;

    // Reference to the GameManager (assign in Inspector)
    public gameManagerScript gameManager;

    private bool isInteractable = true; // Whether the card can be interacted with

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        placeHolder = new GameObject();
        placeHolder.transform.SetParent(this.transform.parent);
        LayoutElement le = placeHolder.AddComponent<LayoutElement>();
        le.preferredWidth = this.GetComponent<LayoutElement>().preferredWidth;
        le.preferredHeight = this.GetComponent<LayoutElement>().preferredHeight;
        le.flexibleWidth = 0;
        le.flexibleHeight = 0;

        placeHolder.transform.SetSiblingIndex(this.transform.GetSiblingIndex());

        parentToReturnTo = this.transform.parent;
        placeHolderParent = parentToReturnTo;
        this.transform.SetParent(this.transform.parent.parent);

        this.GetComponent<CanvasGroup>().blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        this.transform.position = eventData.position;

        if (placeHolder.transform.parent != placeHolderParent)
        {
            placeHolder.transform.SetParent(placeHolderParent);
        }

        int newSiblingIndex = placeHolderParent.childCount;

        for (int i = 0; i < placeHolderParent.childCount; i++)
        {
            if (this.transform.position.x < placeHolderParent.GetChild(i).position.x)
            {
                newSiblingIndex = i;

                if (placeHolder.transform.GetSiblingIndex() < newSiblingIndex)
                    newSiblingIndex--;

                break;
            }
        }

        placeHolder.transform.SetSiblingIndex(newSiblingIndex);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isInteractable) return;

        this.transform.SetParent(parentToReturnTo);
        this.transform.SetSiblingIndex(placeHolder.transform.GetSiblingIndex());
        this.GetComponent<CanvasGroup>().blocksRaycasts = true;
        Destroy(placeHolder);

        // Perform the spell action when the drag ends
        PerformAction();
    }

    public void PerformAction()
    {
        if (spellManager != null && unitScript != null)
        {
            spellManager.CastSpell(spellType, unitScript);
        }
    }

    // Enable or disable drag interaction based on the current team
    public void SetInteractable(bool value)
    {
        isInteractable = value;

        // Optionally adjust the visual appearance of the card
        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = value ? 1.0f : 0.5f; // Dim the card when it's not interactable
            canvasGroup.blocksRaycasts = value; // Prevent clicks when not interactable
        }
    }

    // Check turn and update interactable state
    private void Update()
    {
        if (gameManager != null)
        {
            SetInteractable(gameManager.currentTeam == 0);
        }
    }
}
