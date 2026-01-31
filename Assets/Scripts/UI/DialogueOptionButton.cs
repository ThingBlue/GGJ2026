using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DialogueOptionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Inspector members

    public Image background;
    public Image border;
    public Image borderHovered; // Border that appears only when hovered
    public Image arrow;
    public TMP_Text text;

    public float backgroundAlphaMoveSpeed = 0.05f;
    public float foregroundAlphaMoveSpeed = 0.1f;

    #endregion

    private bool hovered;
    private float originalTextPosition;
    private float originalArrowPosition;
    private Color originalTextColor;

    private float targetBackgroundAlpha = 1.0f;
    private float targetForegroundAlpha = 1.0f;

    private bool destroyWhenInvisible;

    private void Start()
    {
        // Subscribe to events
        EventManager.instance.destroyDialogueOptionButtonsEvent.AddListener(destroyButton);

        originalTextColor = text.color;

        // Set initial alpha
        background.color = new Color(0, 0, 0, 0);
        border.color = new Color(1, 1, 1, 0);
        borderHovered.color = new Color(1, 1, 1, 0);
        arrow.color = new Color(1, 1, 1, 0);
        text.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, 0);

        // Grab initial x positions for objects that move
        originalArrowPosition = arrow.transform.position.x;
        originalTextPosition = text.transform.position.x;
    }

    private void FixedUpdate()
    {
        // Move alpha towards target
        float backgroundAlpha = Mathf.MoveTowards(background.color.a, targetBackgroundAlpha, backgroundAlphaMoveSpeed);
        float borderAlpha = Mathf.MoveTowards(background.color.a, targetForegroundAlpha, foregroundAlphaMoveSpeed);
        float textAlpha = Mathf.MoveTowards(text.color.a, targetForegroundAlpha, foregroundAlphaMoveSpeed);
        background.color = new Color(0, 0, 0, backgroundAlpha);
        border.color = new Color(1, 1, 1, textAlpha);
        text.color = new Color(originalTextColor.r, originalTextColor.g, originalTextColor.b, textAlpha);

        if (targetForegroundAlpha == 0)
        {
            // Also fade out arrow after an option is selected
            float hoveredEffectAlpha = Mathf.MoveTowards(arrow.color.a, targetForegroundAlpha, foregroundAlphaMoveSpeed);
            arrow.color = new Color(1, 1, 1, hoveredEffectAlpha);
            borderHovered.color = new Color(1, 1, 1, hoveredEffectAlpha);
        }

        // Handle hover bob effect
        Vector3 textPosition = text.transform.position;
        textPosition.x = originalTextPosition;
        Vector3 arrowPosition = arrow.transform.position;
        arrowPosition.x = originalArrowPosition;
        if (hovered)
        {
            textPosition.x += 30.0f;
            arrowPosition.x += Mathf.Sin(Time.time * 4.0f) * 5.0f;
        }
        text.transform.position = textPosition;
        arrow.transform.position = arrowPosition;

        // Destroy once invisible
        if (destroyWhenInvisible && backgroundAlpha <= 0 && textAlpha <= 0) Destroy(gameObject);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (targetBackgroundAlpha == 0) return; // Don't change current hover target when an option has already been selected
        hovered = true;
        // Show hovered effects
        arrow.color = new Color(1, 1, 1, 1);
        borderHovered.color = new Color(1, 1, 1, 1);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (targetBackgroundAlpha == 0) return; // Don't change current hover target when an option has already been selected
        hovered = false;
        // Hide hovered effects
        arrow.color = new Color(1, 1, 1, 0);
        borderHovered.color = new Color(1, 1, 1, 0);
    }

    public void destroyButton()
    {
        // Reduce alpha to 0 then destroy
        destroyWhenInvisible = true;
        targetBackgroundAlpha = 0;
        targetForegroundAlpha = 0;
    }
}
