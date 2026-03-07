namespace TheLostSpirit.Infrastructure.Editor.AnimancerMigrate
{
    /// <summary>
    /// Manipulator 與 Viewer 之間的溝通介面。
    /// </summary>
    public interface ITimelineViewerController
    {
        /// <summary>Playhead 拖動（scrub），normalizedRatio 為 0~1。</summary>
        void OnScrub(float normalizedRatio);

        /// <summary>在指定 normalizedRatio 位置新增事件。</summary>
        void OnRequestAddEvent(float normalizedRatio);

        /// <summary>移動第 eventIndex 個事件到新 normalizedRatio。</summary>
        void OnMoveEvent(int eventIndex, float normalizedRatio);

        /// <summary>刪除第 eventIndex 個事件。</summary>
        void OnRequestDeleteEvent(int eventIndex);

        /// <summary>Marker 開始被拖動，Viewer 暫停 Reload 以避免 DOM 被中途清除。</summary>
        void OnBeginDrag();

        /// <summary>Marker 拖動結束，Viewer 恢復 Reload 並立即執行待辦的 Reload。</summary>
        void OnEndDrag();
    }
}
