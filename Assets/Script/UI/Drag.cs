using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Drag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    // 拖曳 main_nodeC 時記錄 slot 子物件
    private readonly List<GameObject> slotChildren = new();
    // 可拖曳目標的 tag（可在 Inspector 設定）
    public string[] allowDropTags = new string[] { "Backpack", "NodeContainer","slot"};
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
        // 記錄 slot 下所有子物件（排除自己）
        if (gameObject.name.Contains("main_nodeC"))
        {
            slotChildren.Clear();
            var parent = transform.parent;
            if (parent != null)
            {
                foreach (Transform child in parent)
                    if (child != transform) slotChildren.Add(child.gameObject);
            }
        }
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        canvasGroup.blocksRaycasts = false;
        transform.SetParent(canvas.transform); // 拖曳時移到 Canvas 下
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
            transform.SetParent(originalParent); // 拖曳結束放回原 slot
            rectTransform.anchoredPosition = originalPosition;
            return;
        }

        string targetTag = eventData.pointerEnter.tag;
        if (System.Array.IndexOf(allowDropTags, targetTag) < 0)
        {
            // 拖到其他地方都回原位
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
            return;
        }

        // main_nodeC 拖到 Backpack 時，生成 backpack node 並刪除自己
        if (gameObject.name.Contains("main_nodeC"))
        {
            if (targetTag == "Backpack")
            {
                GameObject backpackNode = Instantiate(backpackNodePrefab, eventData.pointerEnter.transform);
                RectTransform backpackRect = backpackNode.GetComponent<RectTransform>();
                Vector2 localPos;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPos);
                backpackRect.anchoredPosition = localPos;
                Destroy(gameObject);
            }
            else
            {
                transform.SetParent(eventData.pointerEnter.transform);
                var movedRect = rectTransform;
                if (targetTag == "slot")
                {
                    movedRect.localEulerAngles = new Vector3(0, 0, 90);
                    movedRect.anchoredPosition = Vector2.zero;
                }
                else
                {
                    movedRect.localEulerAngles = new Vector3(0, 0, 45);
                    Vector2 localPos;
                    RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPos);
                    movedRect.anchoredPosition = localPos;
                }
            }
            // slotChildren 內容已記錄，可用於後續邏輯
            return;
        }

        // 其他物件生成新節點
        GameObject prefab = targetTag == "Backpack" ? backpackNodePrefab : mainNodePrefab;
        GameObject newNode = Instantiate(prefab, eventData.pointerEnter.transform);
        var newRect = newNode.GetComponent<RectTransform>();
        if (targetTag == "slot")
        {
            newRect.localEulerAngles = new Vector3(0, 0, 90);
            newRect.anchoredPosition = Vector2.zero;
        }
        else
        {
            newRect.localEulerAngles = new Vector3(0, 0, 45);
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(eventData.pointerEnter.transform as RectTransform, eventData.position, eventData.pressEventCamera, out localPos);
            newRect.anchoredPosition = localPos;
        }
        Destroy(gameObject);
    }
}
