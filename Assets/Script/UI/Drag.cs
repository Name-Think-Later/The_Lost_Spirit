using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Drag : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private RectTransform parentRectTransform;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRectTransform = transform.parent.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 計算拖曳後的新位置
        Vector2 newPos = rectTransform.anchoredPosition + eventData.delta;

        // Panel的尺寸
        Vector2 panelSize = parentRectTransform.rect.size;
        Vector2 nodeSize = rectTransform.rect.size;

        // 限制X座標
        float minX = -panelSize.x / 2 + nodeSize.x / 2 + 20;
        float maxX = panelSize.x / 2 - nodeSize.x / 2 - 20;
        newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

        // 限制Y座標
        float minY = -panelSize.y / 2 + nodeSize.y / 2 + 20;
        float maxY = panelSize.y / 2 - nodeSize.y / 2 - 20;
        newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

        rectTransform.anchoredPosition = newPos;
    }
}
