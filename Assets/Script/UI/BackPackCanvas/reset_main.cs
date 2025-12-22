using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class reset_main : MonoBehaviour
{
    public RectTransform nodeContainer;
    public Vector2 resetPosition = Vector2.zero;
    public Vector2 resetSize = new Vector2(1850, 1950);

    // 可綁定到 UI Button 的 OnClick 事件
    public void ResetNodeContainer()
    {
        if (nodeContainer != null)
        {
            nodeContainer.anchoredPosition = resetPosition;
            nodeContainer.sizeDelta = resetSize;
        }
    }
}
