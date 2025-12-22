using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 頁面管理器，使用隱藏/顯示方式切換不同頁面的 NodeContainer
/// </summary>
public class PageManager : MonoBehaviour
{
    [Header("頁面按鈕")]
    public Button button1;
    public Button button2;
    public Button button3;
    public Button button4;
    public Button button5;
    
    [Header("頁面容器")]
    public GameObject page1Container;
    public GameObject page2Container;
    public GameObject page3Container;
    public GameObject page4Container;
    public GameObject page5Container;
    
    [Header("按鈕視覺效果")]
    public Color activeButtonColor = Color.white;
    public Color inactiveButtonColor = Color.gray;
    
    // 當前激活的頁面索引
    private int currentPageIndex = 0;
    private List<Button> buttons = new List<Button>();
    private List<GameObject> pageContainers = new List<GameObject>();
    
    void Start()
    {
        InitializePageSystem();
    }
    
    /// <summary>
    /// 初始化頁面系統
    /// </summary>
    void InitializePageSystem()
    {
        // 將按鈕添加到列表
        buttons.Clear();
        if (button1 != null) buttons.Add(button1);
        if (button2 != null) buttons.Add(button2);
        if (button3 != null) buttons.Add(button3);
        if (button4 != null) buttons.Add(button4);
        if (button5 != null) buttons.Add(button5);
        
        // 將頁面容器添加到列表
        pageContainers.Clear();
        if (page1Container != null) pageContainers.Add(page1Container);
        if (page2Container != null) pageContainers.Add(page2Container);
        if (page3Container != null) pageContainers.Add(page3Container);
        if (page4Container != null) pageContainers.Add(page4Container);
        if (page5Container != null) pageContainers.Add(page5Container);
        
        // 綁定按鈕點擊事件
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] != null)
            {
                int pageIndex = i; // 捕獲循環變量
                buttons[i].onClick.AddListener(() => SwitchToPage(pageIndex));
            }
        }
        
        // 顯示第一個頁面
        SwitchToPage(0);
    }
    
    /// <summary>
    /// 切換到指定頁面
    /// </summary>
    /// <param name="pageIndex">頁面索引 (0-4)</param>
    public void SwitchToPage(int pageIndex)
    {
        if (pageIndex < 0 || pageIndex >= pageContainers.Count)
        {
            Debug.LogWarning($"Invalid page index: {pageIndex}. Must be between 0 and {pageContainers.Count - 1}");
            return;
        }
        
        // 隱藏所有頁面容器
        for (int i = 0; i < pageContainers.Count; i++)
        {
            if (pageContainers[i] != null)
            {
                pageContainers[i].SetActive(false);
            }
        }
        
        // 顯示目標頁面容器
        if (pageContainers[pageIndex] != null)
        {
            pageContainers[pageIndex].SetActive(true);
            currentPageIndex = pageIndex;
            
            Debug.Log($"Switched to page {pageIndex + 1}");
        }
        
        // 更新按鈕視覺效果
        UpdateButtonVisuals();
    }
    
    /// <summary>
    /// 更新按鈕的視覺效果
    /// </summary>
    void UpdateButtonVisuals()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            if (buttons[i] != null)
            {
                Image buttonImage = buttons[i].GetComponent<Image>();
                if (buttonImage != null)
                {
                    buttonImage.color = (i == currentPageIndex) ? activeButtonColor : inactiveButtonColor;
                }
            }
        }
    }
    
    /// <summary>
    /// 取得當前頁面索引
    /// </summary>
    public int GetCurrentPageIndex()
    {
        return currentPageIndex;
    }
    
    /// <summary>
    /// 取得當前激活的頁面容器
    /// </summary>
    public GameObject GetCurrentPageContainer()
    {
        if (currentPageIndex >= 0 && currentPageIndex < pageContainers.Count)
        {
            return pageContainers[currentPageIndex];
        }
        return null;
    }
    
    /// <summary>
    /// 公開方法：切換到頁面1
    /// </summary>
    public void SwitchToPage1() => SwitchToPage(0);
    
    /// <summary>
    /// 公開方法：切換到頁面2
    /// </summary>
    public void SwitchToPage2() => SwitchToPage(1);
    
    /// <summary>
    /// 公開方法：切換到頁面3
    /// </summary>
    public void SwitchToPage3() => SwitchToPage(2);
    
    /// <summary>
    /// 公開方法：切換到頁面4
    /// </summary>
    public void SwitchToPage4() => SwitchToPage(3);
    
    /// <summary>
    /// 公開方法：切換到頁面5
    /// </summary>
    public void SwitchToPage5() => SwitchToPage(4);
}