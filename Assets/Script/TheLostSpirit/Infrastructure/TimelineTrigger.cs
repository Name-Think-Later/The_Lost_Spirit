using UnityEngine;

// 這是掛在 GameObject 上的腳本
public class TimelineTrigger : MonoBehaviour
{
    // 這個 Clip 是我們要編輯的目標
    public AnimationClip targetClip;

    // 這裡可以寫你其他的遊戲邏輯...
    public void Attack() { Debug.Log("Attack Triggered!"); }
    public void Heal(float amount) { Debug.Log($"Heal: {amount}"); }
}