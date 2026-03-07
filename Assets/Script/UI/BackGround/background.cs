using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;  // 玩家的 Transform
    [SerializeField] private float followSpeed = 5f;     // 跟隨速度
    [SerializeField] private Vector3 positionOffset = Vector3.zero;  // 位置偏移
    [SerializeField] private bool followXAxis = true;    // 是否跟隨 X 軸
    [SerializeField] private bool followYAxis = true;    // 是否跟隨 Y 軸
    [SerializeField] private bool followZAxis = false;   // 是否跟隨 Z 軸
    
    private Vector3 targetPosition;

    void Start()
    {
        // 如果沒有指定玩家，嘗試通過標籤找到玩家
        if (playerTransform == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerTransform = player.transform;
            }
            else
            {
                Debug.LogWarning("找不到玩家物件！請在 Inspector 中指定玩家的 Transform。");
                return;
            }
        }
        
        // 初始化目標位置
        targetPosition = transform.position;
    }

    void Update()
    {
        if (playerTransform == null)
            return;

        // 計算目標位置
        targetPosition = playerTransform.position + positionOffset;

        // 根據設定選擇要跟隨的軸
        Vector3 currentPosition = transform.position;
        
        if (!followXAxis)
            targetPosition.x = currentPosition.x;
        if (!followYAxis)
            targetPosition.y = currentPosition.y;
        if (!followZAxis)
            targetPosition.z = currentPosition.z;

        // 使用 Lerp 平滑移動背景
        transform.position = Vector3.Lerp(currentPosition, targetPosition, followSpeed * Time.deltaTime);
    }
}
