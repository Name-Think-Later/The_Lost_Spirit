using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// StateUI管理器 - 統一管理血條和技能欄的顯示
/// </summary>
public class StateUIManager : MonoBehaviour
{
    [Header("UI組件引用")]
    [SerializeField] private RectangleHealthBar healthBar;
    [SerializeField] private SkillBar skillBar;
    [SerializeField] private Canvas stateUICanvas;
    
    [Header("UI顯示設定")]
    [SerializeField] private bool showHealthBar = true;
    [SerializeField] private bool showSkillBar = true;
    
    [Header("動畫設定")]
    [SerializeField] private float fadeInDuration = 0.5f;
    [SerializeField] private float fadeOutDuration = 0.5f;
    [SerializeField] private AnimationCurve fadeAnimationCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    
    [Header("測試用設定")]
    [SerializeField] private bool enableTestMode = true;
    
    private CanvasGroup canvasGroup;
    private bool isUIVisible = true;
    
    private void Awake()
    {
        InitializeStateUI();
    }
    
    private void Start()
    {
        SetupInitialState();
        
        if (enableTestMode)
        {
            StartCoroutine(TestModeRoutine());
        }
    }
    
    private void Update()
    {
        HandleInput();
    }
    
    /// <summary>
    /// 初始化StateUI
    /// </summary>
    void InitializeStateUI()
    {
        // 確保Canvas Group存在
        if (stateUICanvas != null)
        {
            canvasGroup = stateUICanvas.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = stateUICanvas.gameObject.AddComponent<CanvasGroup>();
            }
        }
        
        // 自動查找組件（如果沒有手動指定）
        if (healthBar == null)
        {
            healthBar = GetComponentInChildren<RectangleHealthBar>();
        }
        
        if (skillBar == null)
        {
            skillBar = GetComponentInChildren<SkillBar>();
        }
    }
    
    /// <summary>
    /// 設定初始狀態
    /// </summary>
    void SetupInitialState()
    {
        // 根據設定顯示或隱藏組件
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(showHealthBar);
        }
        
        if (skillBar != null)
        {
            skillBar.gameObject.SetActive(showSkillBar);
        }
        
        // 設定整體UI可見性
        SetUIVisibility(isUIVisible, false);
    }
    
    /// <summary>
    /// 處理輸入
    /// </summary>
    void HandleInput()
    {
        // Tab鍵切換UI顯示
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleUI();
        }
        
        // H鍵切換血條
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHealthBar();
        }
        
        // K鍵切換技能欄
        if (Input.GetKeyDown(KeyCode.K))
        {
            ToggleSkillBar();
        }
    }
    
    /// <summary>
    /// 切換整體UI顯示
    /// </summary>
    public void ToggleUI()
    {
        SetUIVisibility(!isUIVisible);
    }
    
    /// <summary>
    /// 設定UI可見性
    /// </summary>
    public void SetUIVisibility(bool visible, bool animated = true)
    {
        isUIVisible = visible;
        
        if (canvasGroup == null) return;
        
        StopAllCoroutines();
        
        if (animated)
        {
            StartCoroutine(FadeUI(visible));
        }
        else
        {
            canvasGroup.alpha = visible ? 1f : 0f;
            canvasGroup.interactable = visible;
            canvasGroup.blocksRaycasts = visible;
        }
    }
    
    /// <summary>
    /// UI淡入淡出動畫
    /// </summary>
    private IEnumerator FadeUI(bool fadeIn)
    {
        float startAlpha = canvasGroup.alpha;
        float targetAlpha = fadeIn ? 1f : 0f;
        float duration = fadeIn ? fadeInDuration : fadeOutDuration;
        
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            float curveValue = fadeAnimationCurve.Evaluate(t);
            
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, curveValue);
            yield return null;
        }
        
        canvasGroup.alpha = targetAlpha;
        canvasGroup.interactable = fadeIn;
        canvasGroup.blocksRaycasts = fadeIn;
    }
    
    /// <summary>
    /// 切換血條顯示
    /// </summary>
    public void ToggleHealthBar()
    {
        showHealthBar = !showHealthBar;
        if (healthBar != null)
        {
            healthBar.gameObject.SetActive(showHealthBar);
        }
    }
    
    /// <summary>
    /// 切換技能欄顯示
    /// </summary>
    public void ToggleSkillBar()
    {
        showSkillBar = !showSkillBar;
        if (skillBar != null)
        {
            skillBar.gameObject.SetActive(showSkillBar);
        }
    }
    
    /// <summary>
    /// 取得血條組件
    /// </summary>
    public RectangleHealthBar GetHealthBar()
    {
        return healthBar;
    }
    
    /// <summary>
    /// 取得技能欄組件
    /// </summary>
    public SkillBar GetSkillBar()
    {
        return skillBar;
    }
    
    /// <summary>
    /// 重置UI狀態
    /// </summary>
    public void ResetUIState()
    {
        if (healthBar != null)
        {
            healthBar.ResetToFullHealth();
        }
        
        // SkillBar會自動重置到可用狀態（fillAmount = 0）
        
        SetUIVisibility(true, false);
    }
    
    /// <summary>
    /// 測試模式協程
    /// </summary>
    private IEnumerator TestModeRoutine()
    {
        yield return new WaitForSeconds(2f);
        
        Debug.Log("StateUI測試模式啟動！");
        Debug.Log("按鍵說明：");
        Debug.Log("Tab - 切換整體UI顯示");
        Debug.Log("H - 切換血條顯示");
        Debug.Log("K - 切換技能欄顯示");
        Debug.Log("滑鼠右鍵點擊組件 - 使用Context Menu測試功能");
        
        // 每10秒隨機測試一次血量變化
        while (enableTestMode)
        {
            yield return new WaitForSeconds(10f);
            
            if (healthBar != null)
            {
                float randomDamage = Random.Range(10f, 30f);
                healthBar.ReduceHealth(randomDamage);
                Debug.Log($"測試傷害：{randomDamage}");
            }
        }
    }
    
    /// <summary>
    /// 在編輯器中顯示UI狀態
    /// </summary>
    private void OnGUI()
    {
        if (!enableTestMode) return;
        
        GUILayout.BeginArea(new Rect(10, 10, 300, 150));
        GUILayout.Label("StateUI 測試面板", GUI.skin.box);
        
        GUILayout.Label($"UI可見: {isUIVisible}");
        GUILayout.Label($"血條顯示: {showHealthBar}");
        GUILayout.Label($"技能欄顯示: {showSkillBar}");
        
        if (GUILayout.Button("重置UI狀態"))
        {
            ResetUIState();
        }
        
        if (GUILayout.Button("切換UI顯示"))
        {
            ToggleUI();
        }
        
        GUILayout.EndArea();
    }
    
    /// <summary>
    /// Context Menu 測試方法
    /// </summary>
    [ContextMenu("測試UI顯示切換")]
    public void TestToggleUI()
    {
        ToggleUI();
    }
    
    [ContextMenu("重置所有UI狀態")]
    public void TestResetAllUI()
    {
        ResetUIState();
    }
    
    [ContextMenu("隨機傷害測試")]
    public void TestRandomDamage()
    {
        if (healthBar != null)
        {
            float damage = Random.Range(15f, 35f);
            healthBar.ReduceHealth(damage);
            Debug.Log($"隨機傷害：{damage}");
        }
    }
}