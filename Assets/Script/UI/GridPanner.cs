using UnityEngine;
using UnityEngine.EventSystems;

namespace UI {
   public class GridPanner : MonoBehaviour, IDragHandler
   {
      public RectTransform nodeContainer;
      public RectTransform mainPanel; // 請在 Inspector 指定 mainPanel

      void Awake()
      {
         // 初始化 nodeContainer 位置與大小
         if (nodeContainer != null)
         {
            nodeContainer.anchoredPosition = Vector2.zero;
            nodeContainer.sizeDelta        = new Vector2(1850, 1950);
         }
      }

      public void OnDrag(PointerEventData eventData)
      {
         if (nodeContainer == null || mainPanel == null)
         {
            nodeContainer.anchoredPosition += eventData.delta;
            return;
         }

         // 取得 mainPanel 與 nodeContainer 的 RectTransform 資訊
         Vector2 mainSize      = mainPanel.rect.size;
         Vector2 containerSize = nodeContainer.rect.size;

         // 以 mainPanel 為基準，計算 nodeContainer 拖曳後的新 anchoredPosition
         Vector2 newPos = nodeContainer.anchoredPosition + eventData.delta;


         // nodeContainer 必須包住 mainPanel
         // 左邊界：nodeContainer.left <= mainPanel.left
         float minX = mainPanel.anchoredPosition.x - (containerSize.x / 2) + (mainSize.x / 2);
         // 右邊界：nodeContainer.right >= mainPanel.right
         float maxX = mainPanel.anchoredPosition.x + (containerSize.x / 2) - (mainSize.x / 2);
         newPos.x = Mathf.Clamp(newPos.x, minX, maxX);

         // 下邊界：nodeContainer.bottom <= mainPanel.bottom
         float minY = mainPanel.anchoredPosition.y - (containerSize.y / 2) + (mainSize.y / 2) + 25;
         // 上邊界：nodeContainer.top >= mainPanel.top
         float maxY = mainPanel.anchoredPosition.y + (containerSize.y / 2) - (mainSize.y / 2) + 25;
         newPos.y = Mathf.Clamp(newPos.y, minY, maxY);

         nodeContainer.anchoredPosition = newPos;
      }
   }
}