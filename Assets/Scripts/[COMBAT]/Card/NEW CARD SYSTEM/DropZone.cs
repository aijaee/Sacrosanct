using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [Tooltip("If enabled, the dragged object will perform its action and then be deleted.")]
    public bool deleteAfterAction = true;

    public SpellManager spellManager; // Reference to the SpellManager (assign in Inspector)
    public UnitScript targetUnit; // The default target unit for most spells (assigned in Inspector)
    
    [Tooltip("Only used for SoulBarrier. The target for the SoulBarrier spell.")]
    public UnitScript soulBarrierTargetUnit; // Special target unit for SoulBarrier spell

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            d.placeHolderParent = this.transform;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null && d.placeHolderParent == this.transform)
        {
            d.placeHolderParent = d.parentToReturnTo;
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        Draggable d = eventData.pointerDrag.GetComponent<Draggable>();
        if (d != null)
        {
            Debug.Log($"{eventData.pointerDrag.name} dropped on {gameObject.name}");

            // Ensure targetUnit is assigned, otherwise log an error
            if (targetUnit != null)
            {
                // Use d.spellType directly instead of calling GetSpellType
                spellManager.CastSpell(d.spellType, targetUnit);
            }
            else
            {
                Debug.LogError("Target unit not assigned in DropZone.");
            }

            // Optionally delete the object after performing its action
            if (deleteAfterAction)
            {
                Debug.Log($"{eventData.pointerDrag.name} has been deleted after performing its action.");
                Destroy(eventData.pointerDrag);
            }
        }
    }
}
