using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpressionSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Inspector members

    public Image fillImage;

    public float fillAlphaMoveSpeed;

    #endregion

    private float targetFillAlpha = 0;

    private void Start()
    {
        fillImage.color = new Color(1, 1, 1, 0);
    }

    private void FixedUpdate()
    {
        float fillAlpha = Mathf.MoveTowards(fillImage.color.a, targetFillAlpha, fillAlphaMoveSpeed);
        fillImage.color = new Color(1, 1, 1, fillAlpha);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        targetFillAlpha = 1;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        targetFillAlpha = 0;
    }
}
