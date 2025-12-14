using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class GridPanner : MonoBehaviour, IDragHandler, IScrollHandler
{
   [Header("節點容器設定")]
   public RectTransform[] nodeContainers; // 支援多個 NodeContainer
   public RectTransform mainPanel;
   
   [Header("縮放設定")]
   public float zoomSpeed = 0.1f; // 縮放速度
   public float minZoom = 0.5f; // 最小縮放
   public float maxZoom = 2f; // 最大縮放
   
   private float currentZoom = 1f;

   void Awake()
   {
      if (nodeContainers != null)
      {
         foreach (var container in nodeContainers)
         {
            if (container != null)
            {
               container.anchoredPosition = Vector2.zero;
               container.sizeDelta = new Vector2(1850, 1950);
            }
         }
      }
   }

   public void OnDrag(PointerEventData eventData)
   {
      if (nodeContainers == null || nodeContainers.Length == 0) return;

      // 對每個 NodeContainer 執行拖拽
      foreach (var container in nodeContainers)
      {
         if (container == null) continue;

         if (mainPanel == null)
         {
            container.anchoredPosition += eventData.delta;
            continue;
         }

         Vector2 mainSize = mainPanel.rect.size;
         Vector2 containerSize = container.rect.size * currentZoom;
         Vector2 newPos = container.anchoredPosition + eventData.delta;


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

         container.anchoredPosition = newPos;
      }
   }

   public void OnScroll(PointerEventData eventData)
   {
      if (nodeContainers == null || nodeContainers.Length == 0) return;

      // 計算新的縮放值
      float zoomDelta = eventData.scrollDelta.y * zoomSpeed;
      float newZoom = Mathf.Clamp(currentZoom + zoomDelta, minZoom, maxZoom);

      if (newZoom != currentZoom)
      {
         // 對每個 NodeContainer 執行縮放
         foreach (var container in nodeContainers)
         {
            if (container == null) continue;

            // 獲取滑鼠在當前容器中的位置
            Vector2 mousePos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
               container, 
               eventData.position, 
               eventData.pressEventCamera, 
               out mousePos
            );

            // 計算縮放前滑鼠位置
            Vector2 mouseWorldPosBefore = TransformPoint(mousePos, currentZoom, container);
            
            // 應用新的縮放
            container.localScale = Vector3.one * newZoom;

            // 計算縮放後滑鼠位置
            Vector2 mouseWorldPosAfter = TransformPoint(mousePos, newZoom, container);

            // 調整位置，讓滑鼠位置保持不變
            Vector2 positionAdjustment = mouseWorldPosBefore - mouseWorldPosAfter;
            container.anchoredPosition += positionAdjustment;
         }
         
         // 更新縮放值
         currentZoom = newZoom;
         
         // 確保縮放後位置仍在範圍內
         ClampAllPositions();
      }
   }

   private Vector2 TransformPoint(Vector2 localPoint, float zoom, RectTransform container)
   {
      return localPoint * zoom + container.anchoredPosition;
   }

   private void ClampAllPositions()
   {
      if (nodeContainers == null || mainPanel == null) return;

      foreach (var container in nodeContainers)
      {
         if (container == null) continue;

         Vector2 mainSize = mainPanel.rect.size;
         Vector2 containerSize = container.rect.size * currentZoom;
         Vector2 currentPos = container.anchoredPosition;

         // 重新計算邊界（考慮縮放）
         float minX = mainPanel.anchoredPosition.x - (containerSize.x / 2) + (mainSize.x / 2);
         float maxX = mainPanel.anchoredPosition.x + (containerSize.x / 2) - (mainSize.x / 2);
         currentPos.x = Mathf.Clamp(currentPos.x, minX, maxX);

         float minY = mainPanel.anchoredPosition.y - (containerSize.y / 2) + (mainSize.y / 2) + 25;
         float maxY = mainPanel.anchoredPosition.y + (containerSize.y / 2) - (mainSize.y / 2) + 25;
         currentPos.y = Mathf.Clamp(currentPos.y, minY, maxY);

         container.anchoredPosition = currentPos;
      }
   }
}