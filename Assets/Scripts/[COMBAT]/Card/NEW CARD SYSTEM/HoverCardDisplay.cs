using UnityEngine;
using UnityEngine.UI;

public class HoverCardDisplay : MonoBehaviour
{
    [Tooltip("The canvas where the hover popup will be displayed.")]
    public Canvas hoverCanvas;

    [Tooltip("The sprite to display when hovering over the card.")]
    public Sprite cardSprite;

    [Tooltip("The width of the popup when hovering.")]
    public float popupWidth = 240f;

    [Tooltip("The height of the popup when hovering.")]
    public float popupHeight = 344f;

    private GameObject currentHoverCard;
    private Image hoverImage;

    void Start()
    {
        // Check if required fields are set
        if (hoverCanvas == null)
        {
            Debug.LogError("HoverCanvas is not assigned! Please assign a Canvas in the inspector.");
        }

        if (cardSprite == null)
        {
            Debug.LogError("CardSprite is not assigned! Please assign a sprite in the inspector.");
        }
    }

    // This function will be called when the mouse enters the card
    public void OnMouseEnter()
    {
        if (hoverCanvas == null || cardSprite == null)
        {
            Debug.LogWarning("HoverCardDisplay: Missing required components for hover effect.");
            return;
        }

        Debug.Log("Mouse entered: " + gameObject.name);

        // Instantiate a new Image object for each card hover and parent it to the hoverCanvas
        currentHoverCard = new GameObject("HoverCard");
        currentHoverCard.transform.SetParent(hoverCanvas.transform);
        currentHoverCard.transform.localPosition = Vector3.zero; // Center it in the canvas

        // Add Image component to the newly created GameObject
        hoverImage = currentHoverCard.AddComponent<Image>();
        hoverImage.sprite = cardSprite;
        hoverImage.SetNativeSize();  // Adjust size based on the sprite

        // Optionally, add a RectTransform to control the size/position of the hover card
        RectTransform rectTransform = hoverImage.GetComponent<RectTransform>();

        // Set custom size if specified, otherwise use the sprite's size
        rectTransform.sizeDelta = new Vector2(popupWidth, popupHeight);

        // Position the popup in the center of the screen, or adjust as needed
        rectTransform.position = new Vector3(Screen.width / 2, Screen.height / 2, 0);
    }

    // This function will be called when the mouse exits the card
    public void OnMouseExit()
    {
        Debug.Log("Mouse exited: " + gameObject.name);

        // Destroy the hover card when the mouse exits
        if (currentHoverCard != null)
        {
            Destroy(currentHoverCard);
        }
    }
}
