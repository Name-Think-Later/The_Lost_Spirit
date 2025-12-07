using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GridPanner : MonoBehaviour, IDragHandler
{
   public RectTransform nodeContainer;
   public RectTransform mainPanel;

   void Awake()
   {
      if (nodeContainer != null)
      {
         nodeContainer.anchoredPosition = Vector2.zero;
         nodeContainer.sizeDelta = new Vector2(1850, 1950);
      }
   }

   public void OnDrag(PointerEventData eventData)
   {
      if (nodeContainer == null || mainPanel == null)
      {
         nodeContainer.anchoredPosition += eventData.delta;
         return;
      }

      Vector2 mainSize = mainPanel.rect.size;
      Vector2 containerSize = nodeContainer.rect.size;

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