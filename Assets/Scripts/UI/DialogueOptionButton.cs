using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueOptionButton : MonoBehaviour
{
    #region Inspector members

    public Image image;
    public TMP_Text text;

    public float alphaMoveSpeed = 0.025f;
    public float textAlphaMoveSpeed = 0.05f;

    #endregion

    private float targetAlpha = 1.0f;
    private float targetTextAlpha = 1.0f;

    private bool destroyWhenInvisible;

    private void Start()
    {
        // Subscribe to events
        EventManager.instance.destroyDialogueOptionButtonsEvent.AddListener(destroyButton);

        // Set initial position and alpha
        image.color = new Color(0, 0, 0, 0);
    }

    private void FixedUpdate()
    {
        // Move position and alpha towards target
        float alpha = Mathf.MoveTowards(image.color.a, targetAlpha, alphaMoveSpeed);
        float textAlpha = Mathf.MoveTowards(text.color.a, targetTextAlpha, textAlphaMoveSpeed);
        image.color = new Color(0, 0, 0, alpha);
        text.color = new Color(255, 255, 255, textAlpha);

        // Destroy once invisible
        if (destroyWhenInvisible && alpha <= 0 && textAlpha <= 0) Destroy(gameObject);
    }

    public void destroyButton()
    {
        // Reduce alpha to 0 then destroy
        destroyWhenInvisible = true;
        targetAlpha = 0;
        targetTextAlpha = 0;
    }
}
