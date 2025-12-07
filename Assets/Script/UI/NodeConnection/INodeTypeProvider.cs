/// <summary>
/// 節點類型提供者接口
/// </summary>
public interface INodeTypeProvider
{
    NodeType GetNodeType();
}

/// <summary>
/// 進階節點類型提供者接口
/// </summary>
public interface IAdvancedNodeTypeProvider : INodeTypeProvider
{
    int GetBranchCount();
    bool UseSymmetricalLayout();
}