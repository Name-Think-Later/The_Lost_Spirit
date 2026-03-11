using System;

// �W�ߪ� Enum �ɮסA�T�O�Ҧ��}������Ū���쥦
namespace TheLostSpirit.MapTest
{
    [Flags]
    public enum CellType
    {
        Normal    = 0,
        Drop      = 1 << 0, // 1
        LeftEdge  = 1 << 1, // 2
        LeftWall  = 1 << 2, // 4
        RightEdge = 1 << 3, // 8
        RightWall = 1 << 4, // 16
    }
}