using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections.Generic;

/// <summary>
/// 技能欄UI組件 - 簡化版本，和血條邏輯相同
/// </summary>
public class SkillBar : MonoBehaviour
{
    [System.Serializable]
    public class SkillSlot
    {
        [Header("技能槽UI")]
        public Image skillContainer;
        public KeyCode keyCode;
        
        [Header("技能設定")]
        public float cooldownTime = 5f;
        public bool isOnCooldown = false;
        
        [HideInInspector]
        public float currentCooldown = 0f;
        [HideInInspector]
        public RectTransform skillFillRect;
        [HideInInspector]
        public float targetFillHeight = 0f;
        [HideInInspector]
        public Coroutine animationCoroutine;
    }
    
    [Header("技能槽")]
    [SerializeField] private List<SkillSlot> skillSlots = new List<SkillSlot>();
    
    [Header("UI設定")]
    [SerializeField] private Color skillColor = Color.red; // 改為純紅色測試
    [SerializeField] private Color skillCooldownColor = new Color(0.3f, 0.3f, 0.3f, 1f); // 冷卻時的暗灰色
    
    private void Start()
    {
        // 不自動建立，保持手動控制
        if (skillSlots.Count > 0 && skillSlots[0].skillContainer != null)
        {
            InitializeSkillBar();
        }
    }
    
    private void Update()
    {
        UpdateSkillCooldowns();
        HandleKeyInput();
    }
    
    /// <summary>
    /// 初始化技能欄
    /// </summary>
    void InitializeSkillBar()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            SkillSlot slot = skillSlots[i];
            
            // 自動設置Image屬性
            if (slot.skillContainer != null)
            {
                slot.skillContainer.sprite = null;
                slot.skillContainer.color = skillColor;
                slot.skillContainer.type = Image.Type.Filled;
                slot.skillContainer.fillMethod = Image.FillMethod.Vertical;
                slot.skillContainer.fillOrigin = 0;
                slot.skillContainer.fillAmount = 0f;
            }
            
            UpdateSkillSlotUI(i);
        }
    }
    
    /// <summary>
    /// 為GameObject添加點擊監聽
    /// </summary>
    void AddClickListener(GameObject obj, int index)
    {
        EventTrigger trigger = obj.GetComponent<EventTrigger>();
        if (trigger == null)
        {
            trigger = obj.AddComponent<EventTrigger>();
        }
        
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { 
            Debug.Log($"點擊技能槽 {index + 1}");
            UseSkill(index); 
        });
        trigger.triggers.Add(entry);
        
        Debug.Log($"成功為技能槽 {index + 1} 添加點擊監聽");
    }
    
    /// <summary>
    /// 處理鍵盤輸入
    /// </summary>
    void HandleKeyInput()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            if (Input.GetKeyDown(skillSlots[i].keyCode))
            {
                UseSkill(i);
            }
        }
    }
    
    /// <summary>
    /// 更新技能冷卻
    /// </summary>
    void UpdateSkillCooldowns()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            SkillSlot slot = skillSlots[i];
            
            if (slot.isOnCooldown)
            {
                slot.currentCooldown -= Time.deltaTime;
                
                if (slot.currentCooldown <= 0)
                {
                    // 冷卻結束
                    slot.isOnCooldown = false;
                    slot.currentCooldown = 0f;
                    
                    Debug.Log($"技能 {i + 1} 冷卻完成！");
                }
                
                // 更新UI（包括顏色和填充）
                UpdateSkillSlotUI(i);
            }
        }
    }
    
    /// <summary>
    /// 更新技能槽UI
    /// </summary>
    void UpdateSkillSlotUI(int slotIndex)
    {
        if (slotIndex >= skillSlots.Count) return;
        
        SkillSlot slot = skillSlots[slotIndex];
        
        // 計算目標填充高度 - 預設滿的，冷卻中從空逐漸填滿
        float progress;
        if (slot.isOnCooldown)
        {
            // 冷卻中：從0開始填充到滿
            progress = 1f - (slot.currentCooldown / slot.cooldownTime);
        }
        else
        {
            // 可用狀態：預設滿的
            progress = 1f;
        }
        
        float targetHeight = 3f + (144f * progress); // 3是底部邊距 + 實際填充高度
        
        // 直接設置高度，不使用動畫（確保同步）
        if (slot.skillFillRect != null)
        {
            slot.skillFillRect.offsetMax = new Vector2(slot.skillFillRect.offsetMax.x, targetHeight);
            slot.targetFillHeight = targetHeight;
        }
        
        // 根據冷卻狀態設置顏色
        if (slot.skillContainer != null)
        {
            slot.skillContainer.color = slot.isOnCooldown ? skillCooldownColor : skillColor;
        }
        
        Debug.Log($"技能槽 {slotIndex + 1} 目標高度: {targetHeight:F1} (進度: {progress:F2})");
    }
    
    /// <summary>
    /// 技能填充動畫 - 參考血條的動畫方法
    /// </summary>
    private System.Collections.IEnumerator AnimateSkillFill(SkillSlot slot)
    {
        if (slot.skillFillRect == null) yield break;
        
        float startHeight = slot.skillFillRect.offsetMax.y;
        float targetHeight = slot.targetFillHeight; // 直接使用計算好的高度
        float duration = 0.3f;
        float elapsed = 0f;
        
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            t = Mathf.SmoothStep(0f, 1f, t);
            
            float currentHeight = Mathf.Lerp(startHeight, targetHeight, t);
            slot.skillFillRect.offsetMax = new Vector2(slot.skillFillRect.offsetMax.x, currentHeight);
            
            yield return null;
        }
        
        // 確保到達精確位置
        slot.skillFillRect.offsetMax = new Vector2(slot.skillFillRect.offsetMax.x, targetHeight);
        slot.animationCoroutine = null;
    }
    
    /// <summary>
    /// 使用技能
    /// </summary>
    public void UseSkill(int slotIndex)
    {
        if (slotIndex >= skillSlots.Count) return;
        
        SkillSlot slot = skillSlots[slotIndex];
        
        // 檢查是否在冷卻中（填充狀態表示不可用）
        if (slot.isOnCooldown) 
        {
            Debug.Log($"技能 {slotIndex + 1} 還在冷卻中！");
            return;
        }
        
        // 執行技能邏輯
        Debug.Log($"使用技能 {slotIndex + 1} - 開始5秒冷卻");
        
        // 開始冷卻
        StartCooldown(slotIndex);
    }
    
    /// <summary>
    /// 開始冷卻
    /// </summary>
    void StartCooldown(int slotIndex)
    {
        if (slotIndex >= skillSlots.Count) return;
        
        SkillSlot slot = skillSlots[slotIndex];
        slot.isOnCooldown = true;
        slot.currentCooldown = slot.cooldownTime;
        
        // 立即設為空的（從滿變空）
        if (slot.skillFillRect != null)
        {
            slot.skillFillRect.offsetMax = new Vector2(slot.skillFillRect.offsetMax.x, 3); // 設為最小高度
            slot.targetFillHeight = 3f; // 更新目標高度
        }
        
        UpdateSkillSlotUI(slotIndex);
    }
}