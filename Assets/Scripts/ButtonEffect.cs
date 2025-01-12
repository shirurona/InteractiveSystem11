using UnityEngine;

public class ButtonEffect : MonoBehaviour
{
    private Vector2 _anchoredPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
    }

    public void OnPointerDown(RectTransform rectTransform)
    {
        rectTransform.anchoredPosition = _anchoredPosition + new Vector2(0, -5);
    }

    public void OnPointerExit(RectTransform rectTransform)
    {
        rectTransform.anchoredPosition = _anchoredPosition;
    }
}
