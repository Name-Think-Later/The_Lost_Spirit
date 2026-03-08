using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infiniteBackground : MonoBehaviour
{
    [SerializeField] private Transform[] backgroundParts;    // 多块背景部分
    [SerializeField] private bool enableLoopingX = true;     // 启用 X 轴循环
    [SerializeField] private float extraMargin = 2f;         // 视野外额外边距
    
    private float backgroundWidth = 10f;
    private Camera mainCamera;
    private Vector3[] initialPositions;  // 记录初始位置

    void Start()
    {
        mainCamera = Camera.main;

        // 如果没有手动指定背景部分，自动获取子物件
        if (backgroundParts == null || backgroundParts.Length == 0)
        {
            backgroundParts = GetComponentsInChildren<Transform>();
            System.Collections.Generic.List<Transform> parts = new System.Collections.Generic.List<Transform>(backgroundParts);
            parts.RemoveAt(0);
            backgroundParts = parts.ToArray();

            if (backgroundParts.Length == 0)
            {
                Debug.LogError("没有找到背景部分！");
                return;
            }
        }

        // 保存初始位置
        initialPositions = new Vector3[backgroundParts.Length];
        for (int i = 0; i < backgroundParts.Length; i++)
        {
            initialPositions[i] = backgroundParts[i].position;
        }

        // 计算背景宽度（从初始位置中推断）
        if (backgroundParts.Length >= 2)
        {
            // 找出最小和最大的 X 坐标
            float minX = initialPositions[0].x;
            float maxX = initialPositions[0].x;
            
            for (int i = 1; i < backgroundParts.Length; i++)
            {
                if (initialPositions[i].x < minX) minX = initialPositions[i].x;
                if (initialPositions[i].x > maxX) maxX = initialPositions[i].x;
            }
            
            // 背景宽度 = (最大X - 最小X) / (块数 - 1)
            backgroundWidth = (maxX - minX) / (backgroundParts.Length - 1);
        }

        Debug.Log($"背景块数: {backgroundParts.Length}, 计算宽度: {backgroundWidth}");
        for (int i = 0; i < backgroundParts.Length; i++)
        {
            Debug.Log($"背景块 {i} 初始位置: {initialPositions[i]}");
        }
    }

    void Update()
    {
        if (backgroundParts == null || backgroundParts.Length == 0 || mainCamera == null || backgroundWidth <= 0)
            return;

        if (!enableLoopingX)
            return;

        // 总宽度 = 所有背景块的跨度
        float totalWidth = backgroundWidth * backgroundParts.Length;
        float cameraPosX = mainCamera.transform.position.x;

        // 为每块背景块计算目标位置
        for (int i = 0; i < backgroundParts.Length; i++)
        {
            float baseX = initialPositions[i].x;
            
            // 计算这块背景块距离摄像机最近的周期位置
            // 找出使背景块最接近摄像机的偏移量
            float relativePos = cameraPosX - baseX;
            float cycleOffset = Mathf.Floor(relativePos / totalWidth) * totalWidth;
            
            // 目标位置 = 初始位置 + 周期偏移
            float targetX = baseX + cycleOffset;
            
            // 如果目标位置离摄像机太远，调整到下一个周期
            if (Mathf.Abs(targetX - cameraPosX) > totalWidth / 2)
            {
                targetX += (targetX < cameraPosX) ? totalWidth : -totalWidth;
            }

            Vector3 newPos = backgroundParts[i].position;
            newPos.x = targetX;
            backgroundParts[i].position = newPos;
        }
    }
}
