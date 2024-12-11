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

    public SpellManager spellManager;
    public SpellManager.SpellType spellType;
    public UnitScript unitScript;
    public gameManagerScript gameManager;

    private bool isInteractable = true;

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

        PerformAction();
    }

    public void PerformAction()
    {
        if (spellManager != null && unitScript != null)
        {
            spellManager.CastSpell(spellType, unitScript);
        }
    }

    public void UpdateInteractable()
    {
        if (gameManager != null)
        {
            SetInteractable(gameManager.currentTeam == 0); // Enable for Player 0 only
            Debug.Log($"Card updated: Interactable = {gameManager.currentTeam == 0}");
        }
    }

    public void SetInteractable(bool value)
    {
        // Disable interactivity if the AI is processing its turn
        if (gameManager != null && gameManager.IsAIProcessing)
        {
            value = false;
        }


        isInteractable = value;

        CanvasGroup canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup != null)
        {
            canvasGroup.alpha = value ? 1.0f : 0.5f;
            canvasGroup.blocksRaycasts = value;
        }
    }

}
