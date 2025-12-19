using UnityEngine;

/// <summary>
/// 背包開關控制器，使用快捷鍵 B 來開關背包界面
/// </summary>
public class BackpackToggle : MonoBehaviour
{
    [Header("背包Canvas設定")]
    public GameObject backpackCanvas; // 拖拽 BackPackCanvas 物件到這裡
    
    [Header("快捷鍵設定")]
    public KeyCode toggleKey = KeyCode.B; // 開關快捷鍵
    
    private bool isBackpackVisible = true; // 背包是否可見

    void Start()
    {
        // 自動尋找 BackPackCanvas 如果沒有手動指定
        if (backpackCanvas == null)
        {
            backpackCanvas = GameObject.Find("BackPackCanvas");
        }
        
        // 記錄初始狀態
        if (backpackCanvas != null)
        {
            isBackpackVisible = backpackCanvas.activeSelf;
        }
        else
        {
            Debug.LogWarning("BackpackToggle: 找不到 BackPackCanvas 物件！");
        }
    }

    void Update()
    {
        // 檢測快捷鍵按下
        if (Input.GetKeyDown(toggleKey))
        {
            ToggleBackpack();
        }
    }

    /// <summary>
    /// 切換背包顯示狀態
    /// </summary>
    public void ToggleBackpack()
    {
        if (backpackCanvas != null)
        {
            isBackpackVisible = !isBackpackVisible;
            backpackCanvas.SetActive(isBackpackVisible);
            
            Debug.Log($"背包 {(isBackpackVisible ? "開啟" : "關閉")}");
        }
        else
        {
            Debug.LogWarning("BackpackToggle: BackPackCanvas 物件未設定！");
        }
    }

    /// <summary>
    /// 顯示背包
    /// </summary>
    public void ShowBackpack()
    {
        if (backpackCanvas != null)
        {
            isBackpackVisible = true;
            backpackCanvas.SetActive(true);
            Debug.Log("背包開啟");
        }
    }

    /// <summary>
    /// 隱藏背包
    /// </summary>
    public void HideBackpack()
    {
        if (backpackCanvas != null)
        {
            isBackpackVisible = false;
            backpackCanvas.SetActive(false);
            Debug.Log("背包關閉");
        }
    }
}