using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Linq;

/// <summary>
/// 簡單的UI連接線組件，使用Image來顯示連接線
/// </summary>
public class SimpleConnectionLine : MonoBehaviour, IPointerClickHandler
{
    private RectTransform rectTransform;
    private Image image;
    
    [Header("線條設置")]
    public Color lineColor = Color.white;
    public float lineWidth = 4f;
    
    [Header("線條材質設置")]
    public Sprite lineSprite;
    public bool useTiled = true;
    public float tilesPerUnit = 1f; // 每單位長度的瓷磚數量
    
    [Header("點擊中斷設置")]
    public Color hoverColor = Color.red;
    private Color originalColor;
    private bool isHovered = false;
    
    public NodeConnectionPoint StartPoint { get; private set; }
    public NodeConnectionPoint EndPoint { get; private set; }
    
    // 關聯的連接資料
    private NodeConnection associatedConnection;
    
    // 優化更新
    private Vector3 lastStartPos;
    private Vector3 lastEndPos;
    private bool needsUpdate = true;
    
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        
        if (rectTransform == null)
            rectTransform = gameObject.AddComponent<RectTransform>();
            
        if (image == null)
        {
            image = gameObject.AddComponent<Image>();
            SetupLineSprite();
            
            image.color = lineColor;
            
            // 啟用 raycast，允許點擊連線來中斷
            image.raycastTarget = true;
        }
        
        // 設置錨點為中心
        rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
        rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
    
    void Start()
    {
        originalColor = lineColor;
    }
    
    /// <summary>
    /// 設置連接的兩個點
    /// </summary>
    public void SetConnection(NodeConnectionPoint startPoint, NodeConnectionPoint endPoint)
    {
        StartPoint = startPoint;
        EndPoint = endPoint;
        needsUpdate = true;
        
        // 重新設置 Sprite（因為 lineSprite 可能在這之前才被設定）
        SetupLineSprite();
        UpdateLine();
    }
    
    /// <summary>
    /// 設置關聯的連接資料
    /// </summary>
    public void SetAssociatedConnection(NodeConnection connection)
    {
        associatedConnection = connection;
    }
    
    /// <summary>
    /// 更新線條位置和旋轉
    /// </summary>
    public void UpdateLine()
    {
        if (StartPoint == null || EndPoint == null || rectTransform == null) return;
        
        Vector3 startWorldPos = StartPoint.GetWorldPosition();
        Vector3 endWorldPos = EndPoint.GetWorldPosition();
        
        // 轉換為父物件的本地坐標
        RectTransform parentRT = rectTransform.parent as RectTransform;
        Canvas canvas = GetComponentInParent<Canvas>();
        
        if (parentRT != null && canvas != null)
        {
            Vector2 startLocal, endLocal;
            
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                // World Space Canvas：直接轉換世界坐標到父物件本地坐標
                startLocal = parentRT.InverseTransformPoint(startWorldPos);
                endLocal = parentRT.InverseTransformPoint(endWorldPos);
            }
            else
            {
                // Screen Space Canvas：先轉換為螢幕座標，再轉換為父物件本地坐標
                Camera cam = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRT, 
                    RectTransformUtility.WorldToScreenPoint(cam, startWorldPos), 
                    cam, 
                    out startLocal);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRT, 
                    RectTransformUtility.WorldToScreenPoint(cam, endWorldPos), 
                    cam, 
                    out endLocal);
            }
            
            // 計算中心點和方向
            Vector2 center = (startLocal + endLocal) * 0.5f;
            Vector2 direction = endLocal - startLocal;
            float distance = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 設置位置、尺寸和旋轉
            rectTransform.anchoredPosition = center;
            rectTransform.sizeDelta = new Vector2(distance, lineWidth);
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
    
    void Update()
    {
        // 優化：只在節點位置改變時更新線條
        if (StartPoint != null && EndPoint != null)
        {
            Vector3 startPos = StartPoint.GetWorldPosition();
            Vector3 endPos = EndPoint.GetWorldPosition();
            
            if (needsUpdate || startPos != lastStartPos || endPos != lastEndPos)
            {
                lastStartPos = startPos;
                lastEndPos = endPos;
                needsUpdate = false;
                UpdateLine();
            }
        }
        else if (manualStartPos != Vector3.zero || manualEndPos != Vector3.zero)
        {
            UpdateManualLine();
        }
    }
    
    // 手動位置變量（用於預覽線條）
    private Vector3 manualStartPos;
    private Vector3 manualEndPos;
    
    /// <summary>
    /// 手動設置起始和結束位置（用於預覽線條）
    /// </summary>
    public void SetPositions(Vector3 startPos, Vector3 endPos)
    {
        manualStartPos = startPos;
        manualEndPos = endPos;
        UpdateManualLine();
    }
    
    /// <summary>
    /// 使用手動位置更新線條
    /// </summary>
    private void UpdateManualLine()
    {
        if (rectTransform == null) return;
        
        // 轉換為父物件的本地坐標
        RectTransform parentRT = rectTransform.parent as RectTransform;
        Canvas canvas = GetComponentInParent<Canvas>();
        
        if (parentRT != null && canvas != null)
        {
            Vector2 startLocal, endLocal;
            
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                // World Space Canvas：直接轉換世界坐標到父物件本地坐標
                startLocal = parentRT.InverseTransformPoint(manualStartPos);
                endLocal = parentRT.InverseTransformPoint(manualEndPos);
            }
            else
            {
                // Screen Space Canvas：先轉換為螢幕座標，再轉換為父物件本地坐標
                Camera cam = canvas.renderMode == RenderMode.ScreenSpaceCamera ? canvas.worldCamera : null;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRT, 
                    RectTransformUtility.WorldToScreenPoint(cam, manualStartPos), 
                    cam, 
                    out startLocal);
                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    parentRT, 
                    RectTransformUtility.WorldToScreenPoint(cam, manualEndPos), 
                    cam, 
                    out endLocal);
            }
            
            // 計算中心點和方向
            Vector2 center = (startLocal + endLocal) * 0.5f;
            Vector2 direction = endLocal - startLocal;
            float distance = direction.magnitude;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            
            // 設置位置、尺寸和旋轉
            rectTransform.anchoredPosition = center;
            rectTransform.sizeDelta = new Vector2(distance, lineWidth);
            rectTransform.rotation = Quaternion.Euler(0, 0, angle);
            
            // 確保 Sprite 已正確設置
            if (image != null && image.sprite == null)
            {
                SetupLineSprite();
            }
        }
    }
    
    /// <summary>
    /// 設置線條顏色
    /// </summary>
    public void SetColor(Color color)
    {
        lineColor = color;
        originalColor = color;
        if (image != null && !isHovered)
            image.color = color;
    }
    
    /// <summary>
    /// 設置線條寬度
    /// </summary>
    public void SetWidth(float width)
    {
        lineWidth = width;
        UpdateLine();
    }
    
    /// <summary>
    /// 設置線條 Sprite 和 Tiled 模式
    /// </summary>
    public void SetLineSprite(Sprite sprite, bool tiled = false, float tilesPerUnit = 1f)
    {
        lineSprite = sprite;
        useTiled = tiled;
        this.tilesPerUnit = tilesPerUnit;
        SetupLineSprite();
    }
    
    /// <summary>
    /// 設置線條的 Sprite 和模式
    /// </summary>
    private void SetupLineSprite()
    {
        if (image == null) return;
        
        // 如果有自定義 Sprite，使用它
        if (lineSprite != null)
        {
            image.sprite = lineSprite;
            Debug.Log($"SimpleConnectionLine: Using custom sprite: {lineSprite.name}");
        }
        else
        {
            // 創建有圖案的 Texture2D 用於 Tiled 效果
            if (useTiled)
            {
                // 創建 4x4 的虛線圖案
                Texture2D dashedTexture = new Texture2D(8, 4);
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 4; y++)
                    {
                        // 創建虛線圖案：前4個像素白色，後4個像素透明
                        if (x < 4)
                            dashedTexture.SetPixel(x, y, Color.white);
                        else
                            dashedTexture.SetPixel(x, y, Color.clear);
                    }
                }
                dashedTexture.Apply();
                image.sprite = Sprite.Create(dashedTexture, new Rect(0, 0, 8, 4), new Vector2(0.5f, 0.5f));
            }
            else
            {
                // 非 Tiled 模式使用純白色
                Texture2D whiteTexture = new Texture2D(1, 1);
                whiteTexture.SetPixel(0, 0, Color.white);
                whiteTexture.Apply();
                image.sprite = Sprite.Create(whiteTexture, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f));
            }
        }
        
        // 設置 Image 類型
            if (useTiled)
        {
            image.type = Image.Type.Tiled;
            image.pixelsPerUnitMultiplier = tilesPerUnit;
        }
        else
        {
            image.type = Image.Type.Sliced;
        }
        

    }

#if UNITY_EDITOR
    /// <summary>
    /// 編輯器中屬性變更時自動更新
    /// </summary>
    private void OnValidate()
    {
        if (Application.isPlaying && image != null)
        {
            SetupLineSprite();
        }
    }
#endif
    
    /// <summary>
    /// 動態更新 Sprite（當在 Inspector 中修改 lineSprite 時調用）
    /// </summary>
    public void RefreshSprite()
    {
        SetupLineSprite();
    }

    /// <summary>
    /// 處理滑鼠點擊事件
    /// </summary>
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            // 左鍵點擊中斷連線
            DisconnectLine();
        }
    }
    
    /// <summary>
    /// 中斷連線
    /// </summary>
    private void DisconnectLine()
    {
        NodeConnectionManager connectionManager = FindObjectOfType<NodeConnectionManager>();
        if (connectionManager != null && associatedConnection != null)
        {
            Debug.Log($"Disconnecting line between {StartPoint?.ParentNode?.name} and {EndPoint?.ParentNode?.name}");
            connectionManager.RemoveConnection(associatedConnection);
        }
        else if (connectionManager != null && StartPoint != null && EndPoint != null)
        {
            // 如果沒有關聯連接，嘗試根據連接點尋找連接
            var outputPoint = StartPoint.connectionType == ConnectionPointType.Output ? StartPoint : EndPoint;
            var inputPoint = StartPoint.connectionType == ConnectionPointType.Input ? StartPoint : EndPoint;
            
            foreach (var connection in outputPoint.Connections.ToArray())
            {
                if (connection.OutputPoint == outputPoint && connection.InputPoint == inputPoint)
                {
                    connectionManager.RemoveConnection(connection);
                    break;
                }
            }
        }
    }
}