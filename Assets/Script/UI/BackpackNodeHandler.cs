using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// 背包節點處理器，管理背包中節點的特殊行為
/// </summary>
public class BackpackNodeHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    [Header("拖拽設置")]
    public bool canDragFromBackpack = true;
    public Transform targetContainer; // 拖拽目標容器（如NodeContainer）
    
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private FunctionNodeGenerator nodeGenerator;
    private Vector2 originalPosition;
    private Transform originalParent;
    private bool isDragging = false;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        nodeGenerator = FindObjectOfType<FunctionNodeGenerator>();
        
        // 添加 CanvasGroup 用於拖拽時的視覺效果
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
        
        // 如果沒有指定目標容器，嘗試找到當前激活的 NodeContainer
        if (targetContainer == null)
        {
            PageManager pageManager = FindObjectOfType<PageManager>();
            if (pageManager != null)
            {
                GameObject currentPage = pageManager.GetCurrentPageContainer();
                if (currentPage != null)
                {
                    targetContainer = currentPage.transform;
                }
            }
        }
    }
    
    /// <summary>
    /// 開始拖拽
    /// </summary>
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!canDragFromBackpack) return;
        
        originalPosition = rectTransform.anchoredPosition;
        originalParent = rectTransform.parent;
        isDragging = true;
        
        // 設置拖拽時的視覺效果
        canvasGroup.alpha = 0.8f;
        canvasGroup.blocksRaycasts = false;
        
        // 將節點移到最上層
        rectTransform.SetAsLastSibling();
        
        Debug.Log($"開始拖拽背包節點: {gameObject.name}");
    }
    
    /// <summary>
    /// 拖拽中
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDragFromBackpack || !isDragging) return;
        
        // 更新節點位置
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }
    
    /// <summary>
    /// 結束拖拽
    /// </summary>
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!canDragFromBackpack || !isDragging) return;
        
        isDragging = false;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        
        // 檢查是否拖拽到有效的目標區域
        if (IsDroppedOnValidTarget(eventData))
        {
            DeployNodeToTarget();
        }
        else
        {
            // 返回原位置
            ReturnToBackpack();
        }
    }
    
    /// <summary>
    /// 點擊處理
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            // 右鍵點擊：移除節點
            RemoveFromBackpack();
        }
        else if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 左鍵點擊：顯示節點資訊
            ShowNodeInfo();
        }
    }
    
    /// <summary>
    /// 檢查是否拖拽到有效目標
    /// </summary>
    private bool IsDroppedOnValidTarget(PointerEventData eventData)
    {
        if (targetContainer == null) return false;
        
        // 檢查滑鼠位置是否在目標容器內
        Vector2 localPoint;
        RectTransform targetRect = targetContainer.GetComponent<RectTransform>();
        
        if (targetRect != null && 
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                targetRect, eventData.position, eventData.pressEventCamera, out localPoint))
        {
            return targetRect.rect.Contains(localPoint);
        }
        
        return false;
    }
    
    /// <summary>
    /// 部署節點到目標容器
    /// </summary>
    private void DeployNodeToTarget()
    {
        if (targetContainer == null) return;
        
        Debug.Log($"部署節點 {gameObject.name} 到目標容器");
        
        // 從背包移除
        if (nodeGenerator != null)
        {
            nodeGenerator.RemoveNodeFromBackpack(gameObject);
        }
        
        // 設置新的父容器
        rectTransform.SetParent(targetContainer, false);
        
        // 重新啟用拖拽功能（現在是工作區節點）
        NodeDragHandler workspaceDragHandler = GetComponent<NodeDragHandler>();
        if (workspaceDragHandler == null)
        {
            workspaceDragHandler = gameObject.AddComponent<NodeDragHandler>();
        }
        workspaceDragHandler.enabled = true;
        
        // 調整節點大小為正常大小
        rectTransform.localScale = Vector3.one;
        
        // 移除背包處理器
        Destroy(this);
    }
    
    /// <summary>
    /// 返回背包
    /// </summary>
    private void ReturnToBackpack()
    {
        rectTransform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalPosition;
        
        Debug.Log($"節點 {gameObject.name} 返回背包");
    }
    
    /// <summary>
    /// 從背包移除節點
    /// </summary>
    private void RemoveFromBackpack()
    {
        if (nodeGenerator != null)
        {
            nodeGenerator.RemoveNodeFromBackpack(gameObject);
        }
        
        Destroy(gameObject);
        Debug.Log($"節點 {gameObject.name} 已從背包中刪除");
    }
    
    /// <summary>
    /// 顯示節點資訊
    /// </summary>
    private void ShowNodeInfo()
    {
        NodeBase nodeBase = GetComponent<NodeBase>();
        if (nodeBase != null)
        {
            Debug.Log($"節點資訊 - 名稱: {nodeBase.nodeName}, 類型: {nodeBase.nodeType}");
            
            // 這裡可以顯示節點詳細資訊的UI
            // 例如彈出視窗或在側邊欄顯示屬性
        }
    }
    
    /// <summary>
    /// 設置目標容器
    /// </summary>
    public void SetTargetContainer(Transform target)
    {
        targetContainer = target;
    }
}