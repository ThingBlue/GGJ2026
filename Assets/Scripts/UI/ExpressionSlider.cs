using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpressionSlider : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    #region Inspector members

    public Image fillImage;

    public float fillAlphaSmoothTime;

    #endregion

    private float targetFillAlpha = 0;

    private void Start()
    {
        fillImage.color = new Color(1, 1, 1, 0);
    }

    private void Update()
    {
        float fillAlpha = Mathf.MoveTowards(fillImage.color.a, targetFillAlpha, (1.0f / fillAlphaSmoothTime) * Time.deltaTime);
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
