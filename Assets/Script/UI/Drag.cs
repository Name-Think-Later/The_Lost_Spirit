using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 originalPosition;
    private Transform originalParent;
    private Vector2 dragOffset;

    public GameObject mainNodePrefab;
    public GameObject backpackNodePrefab;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        // 計算滑鼠點擊時與物件中心的 offset
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localMousePos);
        dragOffset = rectTransform.anchoredPosition - localMousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 跟隨滑鼠移動，考慮 offset
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, eventData.pressEventCamera, out pos);
        rectTransform.anchoredPosition = pos + dragOffset;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;

        // 檢查拖曳目標
        if (eventData.pointerEnter == null)
        {
            // 拖到無效區域，回到原本位置與父物件
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
            return;
        }

        // 只允許拖到 backpack 或 NodeContainer
        if (eventData.pointerEnter.CompareTag("Backpack"))
        {
            // 拖到 backpack，生成 backpack_node在滑鼠位置
            GameObject newNode = Instantiate(backpackNodePrefab, eventData.pointerEnter.transform);
            RectTransform newRect = newNode.GetComponent<RectTransform>();
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPos);
            newRect.anchoredPosition = localPos;
            Destroy(gameObject);
        }
        else if (eventData.pointerEnter.CompareTag("NodeContainer"))
        {
            // 拖到 NodeContainer，生成 main_node在滑鼠位置
            GameObject newNode = Instantiate(mainNodePrefab, eventData.pointerEnter.transform);
            RectTransform newRect = newNode.GetComponent<RectTransform>();
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPos);
            newRect.anchoredPosition = localPos;
            Destroy(gameObject);
        }
        else
        {
            // 拖到其他地方都回原位
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
