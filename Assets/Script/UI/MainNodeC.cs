
using System.Collections.Generic;
using UnityEngine;

namespace UI {
    public class MainNodeC : MonoBehaviour
    {
        // slot 排列方向（水平或垂直）
        public enum SlotLayoutType { Horizontal, Vertical }
        [Header("排列方向")]
        public SlotLayoutType slotLayoutType = SlotLayoutType.Horizontal;
        // slot 數量
        [Header("可放幾個 slot")]
        public int slotCount = 1;
        // slot 預製物
        public GameObject slotPrefab;
        // slot 的父物件（RectTransform）
        public RectTransform slotParent;
        // slot 間距（像素）
        [Header("slot 間隔 (像素)")]
        public float slotSpacing = 40f;

        // 目前所有 slot 的 RectTransform
        private List<RectTransform> slotRects = new List<RectTransform>();
        // 目前所有線條的 GameObject（LineRenderer）
        private List<GameObject> lines = new List<GameObject>();

        void Start()
        {
            RectTransform mainNodeRT = GetComponent<RectTransform>();
            // 清空 slotParent 下所有 slot
            foreach (Transform child in slotParent)
                Destroy(child.gameObject);
            slotRects.Clear();
            lines.Clear();

            float total = (slotCount - 1) * slotSpacing;
            for (int i = 0; i < slotCount; i++)
            {
                GameObject    slot                  = Instantiate(slotPrefab, slotParent.transform);
                RectTransform slotRT                = slot.GetComponent<RectTransform>();
                slotRT.anchorMin = slotRT.anchorMax = slotRT.pivot = new Vector2(0.5f, 0.5f);
                float x                             = 0, y = 0;
                if (slotLayoutType == SlotLayoutType.Horizontal)
                    x = -total / 2f + i * slotSpacing;
                else
                    y = -total / 2f + i * slotSpacing;
                slotRT.anchoredPosition = new Vector2(x, y);
                slotRects.Add(slotRT);
            }
        }

        void Update()
        {
            // 清除舊線條
            foreach (var line in lines)
                if (line != null) Destroy(line);
            lines.Clear();

            // main_nodeC 與第一個 slot 連線
            if (slotRects.Count > 0)
                DrawUILine(GetComponent<RectTransform>(), slotRects[0]);
            // slot 之間依序連線
            for (int i = 0; i < slotRects.Count - 1; i++)
                DrawUILine(slotRects[i], slotRects[i + 1]);
        }

        void DrawUILine(RectTransform from, RectTransform to)
        {
            // 用 LineRenderer 畫線
            var lineObj = new GameObject("UILineRenderer");
            lineObj.transform.SetParent(slotParent, false);
            var lr = lineObj.AddComponent<LineRenderer>();
            lr.positionCount = 2;
            lr.material      = new Material(Shader.Find("Sprites/Default"));
            lr.startColor    = lr.endColor = Color.white;
            lr.startWidth    = lr.endWidth = 4f;
            lr.useWorldSpace = false;
            Vector3 start = slotParent.InverseTransformPoint(from.position);
            Vector3 end   = slotParent.InverseTransformPoint(to.position);
            lr.SetPosition(0, start);
            lr.SetPosition(1, end);
            lines.Add(lineObj);
        }
    }
}
