using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// 純Image長方形血條 - 比Slider更簡潔
/// </summary>
public class RectangleHealthBar : MonoBehaviour
{
    [Header("血條UI組件引用（自動生成）")]
    [SerializeField, HideInInspector] private Image backgroundImage;
    [SerializeField, HideInInspector] private Image healthFillImage;
    [SerializeField, HideInInspector] private Image borderImage;
    
    [Header("血條設定")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private float currentHealth = 100f;
    [SerializeField] private Vector2 healthBarSize = new Vector2(300, 20);
    
    [Header("血條顏色")]
    [SerializeField] private Color backgroundColor = new Color(0.2f, 0.2f, 0.2f, 0.8f);
    [SerializeField] private Color fullHealthColor = Color.green;
    [SerializeField] private Color halfHealthColor = Color.yellow;
    [SerializeField] private Color lowHealthColor = Color.red;
    [SerializeField] private Color criticalHealthColor = new Color(0.8f, 0f, 0f);
    
    [Header("邊框設定")]
    [SerializeField] private bool showBorder = true;
    [SerializeField] private Color borderColor = Color.black;
    [SerializeField] private float borderWidth = 2f;
    
    [Header("動畫設定")]
    [SerializeField] private float animationSpeed = 0.5f; // 降低速度讓動畫更明顯
    [SerializeField] private bool useAnimation = true;
    
    [Header("說明")]
    [TextArea(2, 3)]
    public string info = "血條UI會自動生成，無需手動指定組件引用\n右鍵腳本選擇 '重建血條結構' 來重新生成UI";
    
    private RectTransform healthFillRect;
    private float targetFillWidth;
    private Coroutine animationCoroutine;
    
    private void Awake()
    {
        // 只在運行時初始化，避免編輯器模式下的問題
        if (Application.isPlaying)
        {
            SetupHealthBar();
        }
    }
    
    private void Start()
    {
        InitializeHealthBar();
    }
    
    // 當Inspector中的值改變時調用
    private void OnValidate()
    {
        if (Application.isPlaying && gameObject.activeInHierarchy)
        {
            // 運行時更新血條，但要確保組件存在
            if (healthFillRect != null && healthFillImage != null)
            {
                try
                {
                    UpdateHealthBar();
                }
                catch (System.Exception e)
                {
                    Debug.LogWarning($"OnValidate更新血條失敗: {e.Message}");
                }
            }
        }
    }
    
    /// <summary>
    /// 設定血條結構
    /// </summary>
    void SetupHealthBar()
    {
        try
        {
            // 總是重新創建血條結構，確保組件正確
            CreateHealthBarStructure();
            
            // 獲取Fill的RectTransform
            if (healthFillImage != null)
            {
                healthFillRect = healthFillImage.GetComponent<RectTransform>();
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"設定血條結構失敗: {e.Message}");
        }
    }
    
    /// <summary>
    /// 創建血條UI結構
    /// </summary>
    void CreateHealthBarStructure()
    {
        // 清理現有子對象
        ClearExistingChildren();
        
        // 設定主容器
        RectTransform containerRect = GetComponent<RectTransform>();
        if (containerRect != null)
        {
            containerRect.sizeDelta = healthBarSize;
        }
        
        // 創建邊框（如果需要）
        if (showBorder)
        {
            CreateBorderImage();
        }
        
        // 創建背景
        CreateBackgroundImage();
        
        // 創建血量填充
        CreateHealthFillImage();
    }
    
    /// <summary>
    /// 清理現有的子對象
    /// </summary>
    void ClearExistingChildren()
    {
        // 停止動畫
        if (animationCoroutine != null)
        {
            StopCoroutine(animationCoroutine);
            animationCoroutine = null;
        }
        
        // 清空所有引用
        backgroundImage = null;
        healthFillImage = null;
        borderImage = null;
        healthFillRect = null;
        
        // 安全清除所有子對象 - 使用倒序遍歷避免索引問題
        int childCount = transform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(child.gameObject);
                }
                else
                {
                    DestroyImmediate(child.gameObject);
                }
            }
        }
    }
    
    void CreateBorderImage()
    {
        GameObject borderObj = new GameObject("Border");
        borderObj.transform.SetParent(transform, false);
        
        borderImage = borderObj.AddComponent<Image>();
        borderImage.color = borderColor;
        
        RectTransform borderRect = borderObj.GetComponent<RectTransform>();
        borderRect.anchorMin = Vector2.zero;
        borderRect.anchorMax = Vector2.one;
        borderRect.offsetMin = Vector2.one * -borderWidth;
        borderRect.offsetMax = Vector2.one * borderWidth;
    }
    
    void CreateBackgroundImage()
    {
        GameObject bgObj = new GameObject("Background");
        bgObj.transform.SetParent(transform, false);
        
        backgroundImage = bgObj.AddComponent<Image>();
        backgroundImage.color = backgroundColor;
        
        RectTransform bgRect = bgObj.GetComponent<RectTransform>();
        bgRect.anchorMin = Vector2.zero;
        bgRect.anchorMax = Vector2.one;
        bgRect.offsetMin = Vector2.zero;
        bgRect.offsetMax = Vector2.zero;
    }
    
    void CreateHealthFillImage()
    {
        GameObject fillObj = new GameObject("HealthFill");
        fillObj.transform.SetParent(transform, false);
        
        healthFillImage = fillObj.AddComponent<Image>();
        healthFillImage.color = fullHealthColor;
        
        healthFillRect = fillObj.GetComponent<RectTransform>();
        
        // 設定錨點：完全填滿父對象
        healthFillRect.anchorMin = Vector2.zero;
        healthFillRect.anchorMax = Vector2.one;
        healthFillRect.pivot = new Vector2(0, 0.5f);
        
        // 設定位置，讓填充從左邊開始
        healthFillRect.anchoredPosition = Vector2.zero;
        healthFillRect.offsetMin = Vector2.zero;
        healthFillRect.offsetMax = Vector2.zero;
        
        // 設定初始寬度，高度自動匹配父對象
        float initialWidth = healthBarSize.x * (currentHealth / maxHealth);
        healthFillRect.sizeDelta = new Vector2(initialWidth - healthBarSize.x, 0);
    }
    

    
    /// <summary>
    /// 初始化血條
    /// </summary>
    void InitializeHealthBar()
    {
        // 確保所有組件都存在
        if (backgroundImage != null && healthFillImage != null)
        {
            UpdateHealthBar();
        }
        else
        {
            Debug.LogWarning("血條組件初始化失敗，請嘗試重建血條結構");
        }
    }
    
    /// <summary>
    /// 更新血條顯示
    /// </summary>
    void UpdateHealthBar()
    {
        float healthPercentage = currentHealth / maxHealth;
        float newTargetWidth = healthBarSize.x * healthPercentage;
        
        // 只有當目標寬度真的改變時才啟動動畫
        if (Mathf.Abs(newTargetWidth - targetFillWidth) > 0.1f)
        {
            targetFillWidth = newTargetWidth;
            
            if (useAnimation)
            {
                // 如果已有動畫在進行，讓它從當前位置繼續
                if (animationCoroutine != null)
                {
                    StopCoroutine(animationCoroutine);
                }
                animationCoroutine = StartCoroutine(AnimateHealthBar());
            }
            else
            {
                SetFillWidth(targetFillWidth);
            }
        }
        
        // 顏色即時更新
        UpdateHealthColor(healthPercentage);
    }
    
    /// <summary>
    /// 設定填充寬度
    /// </summary>
    void SetFillWidth(float width)
    {
        if (healthFillRect != null)
        {
            // 使用負值sizeDelta控制寬度，高度自動匹配
            float offsetX = healthBarSize.x - width;
            healthFillRect.sizeDelta = new Vector2(-offsetX, 0);
        }
    }
    
    /// <summary>
    /// 血條動畫
    /// </summary>
    private IEnumerator AnimateHealthBar()
    {
        if (healthFillRect == null) yield break;
        
        // 計算當前實際的填充寬度
        float currentActualWidth = GetCurrentFillWidth();
        float startWidth = currentActualWidth;
        
        float elapsed = 0f;
        float widthDifference = Mathf.Abs(targetFillWidth - startWidth);
        float duration = widthDifference / (healthBarSize.x * animationSpeed);
        
        // 確保最小動畫時間，讓動畫更明顯
        duration = Mathf.Max(duration, 0.3f);
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            
            // 使用緩動曲線讓動畫更自然
            t = Mathf.SmoothStep(0f, 1f, t);
            
            float currentWidth = Mathf.Lerp(startWidth, targetFillWidth, t);
            SetFillWidth(currentWidth);
            
            yield return null;
        }
        
        // 確保最終到達精確位置
        SetFillWidth(targetFillWidth);
        animationCoroutine = null;
    }
    
    /// <summary>
    /// 獲取當前填充寬度
    /// </summary>
    float GetCurrentFillWidth()
    {
        if (healthFillRect == null) return 0f;
        
        // 從sizeDelta.x反推實際寬度
        float offsetX = -healthFillRect.sizeDelta.x;
        return healthBarSize.x - offsetX;
    }
    
    /// <summary>
    /// 更新血條顏色
    /// </summary>
    void UpdateHealthColor(float healthPercentage)
    {
        if (healthFillImage == null) return;
        
        Color targetColor;
        
        if (healthPercentage > 0.7f)
        {
            targetColor = Color.Lerp(halfHealthColor, fullHealthColor, (healthPercentage - 0.7f) / 0.3f);
        }
        else if (healthPercentage > 0.3f)
        {
            targetColor = Color.Lerp(lowHealthColor, halfHealthColor, (healthPercentage - 0.3f) / 0.4f);
        }
        else
        {
            targetColor = Color.Lerp(criticalHealthColor, lowHealthColor, healthPercentage / 0.3f);
        }
        
        healthFillImage.color = targetColor;
    }
    

    
    /// <summary>
    /// 設定血量
    /// </summary>
    public void SetHealth(float health)
    {
        currentHealth = Mathf.Clamp(health, 0, maxHealth);
        UpdateHealthBar();
    }
    
    /// <summary>
    /// 增加血量
    /// </summary>
    public void AddHealth(float amount)
    {
        SetHealth(currentHealth + amount);
    }
    
    /// <summary>
    /// 減少血量
    /// </summary>
    public void ReduceHealth(float amount)
    {
        SetHealth(currentHealth - amount);
    }
    
    /// <summary>
    /// 設定最大血量
    /// </summary>
    public void SetMaxHealth(float maxHp)
    {
        maxHealth = maxHp;
        UpdateHealthBar();
    }
    
    /// <summary>
    /// 設定血條尺寸
    /// </summary>
    public void SetHealthBarSize(Vector2 size)
    {
        healthBarSize = size;
        
        // 更新容器尺寸
        RectTransform containerRect = GetComponent<RectTransform>();
        containerRect.sizeDelta = size;
        
        // 重新計算填充寬度
        UpdateHealthBar();
    }
    
    /// <summary>
    /// 取得血量百分比
    /// </summary>
    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
    
    /// <summary>
    /// 重置到滿血
    /// </summary>
    public void ResetToFullHealth()
    {
        SetHealth(maxHealth);
    }
}