using System;

// 獨立的 Enum 檔案，確保所有腳本都能讀取到它
[Flags]
public enum CellType
{
    Normal = 0,
    Drop = 1 << 0, // 1
    LeftEdge = 1 << 1, // 2
    LeftWall = 1 << 2, // 4
    RightEdge = 1 << 3, // 8
    RightWall = 1 << 4, // 16
}