using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// 連接拖拽處理器，負責處理從連接點開始的拖拽連線操作
/// </summary>
public class ConnectionDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private NodeConnectionPoint connectionPoint;
    private NodeConnectionManager connectionManager;
    private Camera uiCamera;
    private Canvas canvas;
    
    private bool isDragging = false;
    private Vector2 dragStartPosition;
    
    void Awake()
    {
        connectionPoint = GetComponent<NodeConnectionPoint>();
        canvas = GetComponentInParent<Canvas>();
        
        // 尋找UI相機
        if (canvas.renderMode == RenderMode.ScreenSpaceCamera)
        {
            uiCamera = canvas.worldCamera;
        }
        else if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            uiCamera = null; // 對於Overlay模式，不需要相機
        }
    }
    
    void Start()
    {
        connectionManager = FindObjectOfType<NodeConnectionManager>();
        if (connectionManager == null)
        {
            Debug.LogError("NodeConnectionManager not found in scene!");
        }
    }
    
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (connectionPoint == null)
        {
            Debug.LogError("ConnectionPoint is null in ConnectionDragHandler!");
            return;
        }
        
        if (connectionManager == null)
        {
            Debug.LogError("ConnectionManager is null in ConnectionDragHandler!");
            return;
        }
        
        // 只有輸出點可以開始拖拽連線
        if (connectionPoint.connectionType != ConnectionPointType.Output)
        {
            Debug.Log($"Cannot drag from input point on {connectionPoint.ParentNode?.name}");
            return;
        }
        
        // 阻止事件冒泡到父物件（避免觸發節點拖拽）
        eventData.Use();
        
        isDragging = true;
        dragStartPosition = eventData.position;
        
        // 開始拖拽連線
        Vector2 worldPosition = GetWorldPosition(eventData.position);
        connectionPoint.StartConnectionDrag(worldPosition);
        
        Debug.Log($"Started connection drag from {connectionPoint.ParentNode?.name}, position: {worldPosition}");
    }
    
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging || connectionManager == null) return;
        
        // 阻止事件冒泡
        eventData.Use();
        
        // 更新拖拽線條
        Vector2 worldPosition = GetWorldPosition(eventData.position);
        connectionManager.UpdateConnectionDrag(worldPosition);
        
        // 檢查是否懸停在有效的連接點上
        GameObject hoveredObject = eventData.pointerCurrentRaycast.gameObject;
        if (hoveredObject != null)
        {
            NodeConnectionPoint targetPoint = hoveredObject.GetComponent<NodeConnectionPoint>();
            if (targetPoint != null && connectionPoint.CanConnectTo(targetPoint))
            {
                connectionManager.ShowConnectionPreview(targetPoint);
            }
            else
            {
                connectionManager.HideConnectionPreview();
            }
        }
        else
        {
            connectionManager.HideConnectionPreview();
        }
    }
    
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging || connectionManager == null) return;
        
        // 阻止事件冒泡
        eventData.Use();
        
        isDragging = false;
        
        // 檢查拖拽結束位置是否有有效的連接點
        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;
        NodeConnectionPoint targetPoint = null;
        
        Debug.Log($"Raycast hit: {targetObject?.name}");
        
        if (targetObject != null)
        {
            // 只忽略預覽線條，不忽略普通連接線（因為連接線現在可以點擊）
            if (targetObject.name.Contains("PreviewLine"))
            {
                Debug.Log("Ignoring preview line, checking for connection points under cursor");
                targetObject = null; // 忽略預覽線條，尋找其他目標
            }
            else
            {
                targetPoint = targetObject.GetComponent<NodeConnectionPoint>();
                if (targetPoint == null)
                {
                    // 也檢查父物件
                    targetPoint = targetObject.GetComponentInParent<NodeConnectionPoint>();
                }
            }
        }
        
        bool connectionCreated = false;
        
        if (targetPoint != null)
        {
            Debug.Log($"Target found: {targetPoint.ParentNode?.name} ({targetPoint.connectionType})");
            
            if (connectionPoint.CanConnectTo(targetPoint))
            {
                // 嘗試建立連接
                connectionCreated = connectionPoint.TryConnectTo(targetPoint);
                
                if (connectionCreated)
                {
                    Debug.Log($"Connection created between {connectionPoint.ParentNode.name} and {targetPoint.ParentNode.name}");
                }
                else
                {
                    Debug.LogWarning($"Failed to create connection between {connectionPoint.ParentNode.name} and {targetPoint.ParentNode.name}");
                }
            }
            else
            {
                Debug.LogWarning($"Cannot connect {connectionPoint.ParentNode?.name} ({connectionPoint.connectionType}) to {targetPoint.ParentNode?.name} ({targetPoint.connectionType})");
            }
        }
        else
        {
            if (targetObject != null)
            {
                Debug.Log($"Target object found but no ConnectionPoint: {targetObject.name}");
            }
            else
            {
                Debug.Log("No target object found");
            }
        }
        
        // 結束拖拽連線
        connectionManager.EndConnectionDrag(connectionCreated);
    }
    
    /// <summary>
    /// 將螢幕座標轉換為Canvas本地座標
    /// </summary>
    private Vector2 GetWorldPosition(Vector2 screenPosition)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
        {
            // Screen Space Overlay：螢幕座標就是世界座標
            return screenPosition;
        }
        else
        {
            // 其他模式：需要座標轉換
            Vector2 localPoint;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvas.transform as RectTransform, 
                screenPosition, 
                uiCamera, 
                out localPoint);
            
            return canvas.transform.TransformPoint(localPoint);
        }
    }
    
    /// <summary>
    /// 檢查是否正在拖拽
    /// </summary>
    public bool IsDragging => isDragging;
    
    /// <summary>
    /// 強制停止拖拽
    /// </summary>
    public void CancelDrag()
    {
        if (isDragging && connectionManager != null)
        {
            isDragging = false;
            connectionManager.EndConnectionDrag(false);
        }
    }
}