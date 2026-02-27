using UnityEngine;
using Pathfinding;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class SimpleAIPlatformer : MonoBehaviour
{
    [Header("目標設定")]
    public Transform target;

    [Header("移動參數")]
    public float speed = 5f;
    public float jumpForce = 16f;

    [Header("尋路設定")]
    // ★關鍵修改：這個數值越小，移動越精確，建議設 0.2 ~ 0.5
    public float nextWaypointDistance = 0.5f;

    // 跳躍判斷的高度差 (建議 0.4，避免平地亂跳)
    public float jumpNodeHeightRequirement = 0.4f;

    [Header("物理與地面")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f; // ★關鍵修改：不要太大，0.2 剛好
    public LayerMask groundLayer;

    private Path path;
    private int currentWaypoint = 0;
    private bool isGrounded;
    private Seeker seeker;
    private Rigidbody2D rb;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();

        // ★關鍵修改：開啟插值，讓移動畫面看起來更絲滑
        rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        rb.gravityScale = 3f;
        rb.freezeRotation = true;

        InvokeRepeating(nameof(UpdatePath), 0f, 0.2f); // 加快路徑更新頻率
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void FixedUpdate()
    {
        // 1. 地面偵測
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        if (path == null || currentWaypoint >= path.vectorPath.Count) return;

        // 2. 取得路徑資訊
        Vector2 currentPos = rb.position;
        Vector2 nextPoint = path.vectorPath[currentWaypoint];

        // 算出方向 (Normalize 讓數值保持在 -1 ~ 1 之間)
        Vector2 direction = (nextPoint - currentPos).normalized;
        float distance = Vector2.Distance(currentPos, nextPoint);

        // ==========================================================
        // ★ 絲滑跳躍邏輯 ★
        // ==========================================================

        // 判斷條件 1: 目標點比我高 (需要跳)
        bool targetIsHigh = nextPoint.y > currentPos.y + jumpNodeHeightRequirement;

        // 判斷條件 2: 垂直爬牆判定 (目標在正頭頂，X 軸差異很小)
        bool isVerticalPath = Mathf.Abs(nextPoint.x - currentPos.x) < 0.3f && nextPoint.y > currentPos.y;

        // 執行跳躍
        if (isGrounded)
        {
            // 如果遇到高處，或者路徑點就在正頭頂，就跳
            if (targetIsHigh || isVerticalPath)
            {
                // 給予一個向上的力，同時保留一點點原本的慣性
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }
        }

        // ==========================================================
        // ★ 絲滑移動邏輯 ★
        // ==========================================================

        float targetX = 0;

        // 只有當需要移動的距離夠大時才移動，避免在原地抖動
        if (Mathf.Abs(nextPoint.x - currentPos.x) > 0.1f)
        {
            targetX = direction.x > 0 ? speed : -speed;
        }

        // 空中移動優化：如果在空中，不要完全失去控制，但也不要能瞬間轉向 (手感較好)
        if (!isGrounded)
        {
            // 簡單版：空中也可以全速移動 (類似瑪利歐)
            // 如果你想要空中難控制一點，可以用 Mathf.Lerp
            targetX = direction.x > 0 ? speed : -speed;
        }

        // 套用速度
        rb.velocity = new Vector2(targetX, rb.velocity.y);

        // ==========================================================
        // ★ 路徑點切換 ★
        // ==========================================================

        // 如果距離夠近，就切換到下一個點
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
        // 特殊情況：如果已經跳過頭了 (Y軸超過目標)，且 X 軸很接近，也切換
        else if (currentPos.y > nextPoint.y && Mathf.Abs(currentPos.x - nextPoint.x) < 0.5f)
        {
            currentWaypoint++;
        }
    }

    void OnDrawGizmos()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}