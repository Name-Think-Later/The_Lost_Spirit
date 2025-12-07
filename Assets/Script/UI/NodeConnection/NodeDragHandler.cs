using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 節點拖拽處理器，負責處理節點的拖拽移動
/// </summary>
public class NodeDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("允許放置的標籤")]
    public string[] allowDropTags = new string[] { "Backpack", "NodeContainer" };
    
    [Header("節點轉換預製件")]
    public GameObject mainNodePrefab;     // 主節點預製件
    public GameObject backpackNodePrefab; // 背包節點預製件
    
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private Vector2 dragOffset;
    private Vector2 originalPosition;
    private Transform originalParent;
    
    private NodeConnectionManager connectionManager;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
        
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }
    
    void Start()
    {
        connectionManager = FindObjectOfType<NodeConnectionManager>();
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 檢查是否點擊的是連接點，如果是則不進行節點拖拽
        if (eventData.pointerPressRaycast.gameObject != null)
        {
            NodeConnectionPoint connectionPoint = eventData.pointerPressRaycast.gameObject.GetComponent<NodeConnectionPoint>();
            if (connectionPoint != null)
            {
                // 如果點擊的是連接點，不進行節點拖拽
                return;
            }
        }
        
        // 記錄原始位置和父物件
        originalPosition = rectTransform.anchoredPosition;
        originalParent = transform.parent;
        
        // 設置拖拽狀態
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.8f; // 半透明效果
        
        // 移到 Canvas 最上層進行拖拽
        transform.SetParent(canvas.transform);
        
        // 計算拖拽偏移
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out localMousePos);
            
        dragOffset = rectTransform.anchoredPosition - localMousePos;
        
        Debug.Log($"Started dragging node: {name}");
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        // 更新節點位置
        Vector2 localMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform, 
            eventData.position, 
            eventData.pressEventCamera, 
            out localMousePos);
            
        rectTransform.anchoredPosition = localMousePos + dragOffset;
        
        // 連接線會自動在NodeConnectionManager中更新
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        // 恢復拖拽狀態
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;
        
        // 檢查是否放置在有效容器中
        GameObject dropTarget = GetDropTarget(eventData);
        
        if (dropTarget != null && IsValidDropTarget(dropTarget))
        {
            // 檢查是否需要節點轉換
            HandleNodeConversion(dropTarget, eventData);
        }
        else
        {
            // 無效放置 - 返回原位置
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
            Debug.Log($"Node {name} returned to original position");
        }
        
        Debug.Log($"Finished dragging node: {name} to position: {rectTransform.anchoredPosition}");
    }
    
    /// <summary>
    /// 獲取放置目標
    /// </summary>
    private GameObject GetDropTarget(PointerEventData eventData)
    {
        if (eventData.pointerEnter != null)
        {
            return eventData.pointerEnter;
        }
        return null;
    }
    
    /// <summary>
    /// 檢查是否為有效的放置目標
    /// </summary>
    private bool IsValidDropTarget(GameObject target)
    {
        if (target == null) return false;
        
        foreach (string tag in allowDropTags)
        {
            if (target.CompareTag(tag))
            {
                return true;
            }
        }
        
        return false;
    }
    
    /// <summary>
    /// 處理節點轉換（背包節點 <-> 主節點）
    /// </summary>
    private void HandleNodeConversion(GameObject dropTarget, PointerEventData eventData)
    {
        string targetTag = dropTarget.tag;
        bool isBackpackNode = gameObject.name.Contains("backpack");
        bool isMainNode = !isBackpackNode && (gameObject.name.Contains("main_node") || gameObject.name.Contains("node"));
        
        GameObject prefabToUse = null;
        
        // 決定要轉換成什麼節點
        if (targetTag == "Backpack" && isMainNode)
        {
            // 主節點拖到背包 -> 轉換成背包節點
            prefabToUse = backpackNodePrefab;
        }
        else if (targetTag == "NodeContainer" && isBackpackNode)
        {
            // 背包節點拖到節點容器 -> 轉換成主節點
            prefabToUse = mainNodePrefab;
        }
        
        if (prefabToUse != null)
        {
            // 創建新節點
            GameObject newNode = Instantiate(prefabToUse, dropTarget.transform);
            RectTransform newRect = newNode.GetComponent<RectTransform>();
            
            // 設置位置
            Vector2 localPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                dropTarget.transform as RectTransform, 
                eventData.position, 
                eventData.pressEventCamera, 
                out localPos);
            newRect.anchoredPosition = localPos;
            
            Debug.Log($"Node converted: {gameObject.name} -> {newNode.name} in {dropTarget.name}");
            
            // 移除連接（如果有的話）
            NodeBase nodeBase = GetComponent<NodeBase>();
            if (nodeBase != null && connectionManager != null)
            {
                // 移除該節點的所有輸入和輸出連接
                var inputConnections = nodeBase.GetInputConnections();
                var outputConnections = nodeBase.GetOutputConnections();
                
                foreach (var connection in inputConnections)
                {
                    connectionManager.RemoveConnection(connection);
                }
                
                foreach (var connection in outputConnections)
                {
                    connectionManager.RemoveConnection(connection);
                }
            }
            
            // 銷毀原節點
            Destroy(gameObject);
        }
        else
        {
            // 沒有轉換，直接放置
            transform.SetParent(dropTarget.transform);
            Debug.Log($"Node {name} placed in {dropTarget.name}");
        }
    }
    
    /// <summary>
    /// 程序化移動節點到指定位置
    /// </summary>
    public void MoveToPosition(Vector2 targetPosition)
    {
        rectTransform.anchoredPosition = targetPosition;
        Debug.Log($"Node {name} moved to position: {targetPosition}");
    }
    
    /// <summary>
    /// 平滑移動到目標位置
    /// </summary>
    public void SmoothMoveToPosition(Vector2 targetPosition, float duration = 1f)
    {
        StartCoroutine(SmoothMoveCoroutine(targetPosition, duration));
    }
    
    /// <summary>
    /// 平滑移動協程
    /// </summary>
    private System.Collections.IEnumerator SmoothMoveCoroutine(Vector2 targetPosition, float duration)
    {
        Vector2 startPosition = rectTransform.anchoredPosition;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // 使用緩動曲線
            t = Mathf.SmoothStep(0f, 1f, t);
            
            rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
            
            yield return null;
        }
        
        rectTransform.anchoredPosition = targetPosition;
        Debug.Log($"Node {name} smoothly moved to position: {targetPosition}");
    }
    
    /// <summary>
    /// 獲取當前世界位置
    /// </summary>
    public Vector3 GetWorldPosition()
    {
        return rectTransform.position;
    }
    
    /// <summary>
    /// 設置拖拽是否啟用
    /// </summary>
    public void SetDraggingEnabled(bool enabled)
    {
        this.enabled = enabled;
    }
}