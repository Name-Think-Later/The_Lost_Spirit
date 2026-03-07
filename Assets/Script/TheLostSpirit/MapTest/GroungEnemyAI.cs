using UnityEngine;
using Pathfinding; // 引用 A* 命名空間

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Seeker))]
public class GroundEnemyAI : MonoBehaviour
{
    [Header("追蹤設定")]
    public Transform target;
    public float updateRate = 0.5f; // 重新計算路徑的頻率(秒)

    [Header("移動與跳躍參數")]
    public float speed = 200f;      // 水平移動速度
    public float jumpForce = 400f;  // 跳躍力道
    public float nextWaypointDistance = 1f; // 判斷到達節點的距離

    [Header("地板偵測")]
    public Transform groundCheck;   // 放在敵人腳底的空物件
    public float groundCheckRadius = 0.2f;
    public LayerMask groundLayer;   // 地板的圖層

    private Seeker seeker;
    private Rigidbody2D rb;
    private Path path;
    private int currentWaypoint = 0;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        // 定期更新路徑
        InvokeRepeating("UpdatePath", 0f, updateRate);
    }

    void UpdatePath()
    {
        if (seeker.IsDone() && target != null)
        {
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
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
        if (path == null) return;

        if (currentWaypoint >= path.vectorPath.Count) return;

        // 判斷是否踩在地板上
        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 計算前往下一個路徑點的方向
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;

        // 1. 處理水平移動 (直接賦予 X 軸速度，保留 Y 軸原本的掉落/跳躍速度)
        rb.velocity = new Vector2(direction.x * speed * Time.fixedDeltaTime, rb.velocity.y);

        // 2. 處理跳躍邏輯 (如果下一個目標點在上方，且目前踩在地板上，就起跳)
        if (direction.y > 0.6f && isGrounded)
        {
            rb.AddForce(Vector2.up * jumpForce);
        }

        // 3. 處理左右翻轉圖片
        if (rb.velocity.x >= 0.05f)
            spriteRenderer.flipX = false;
        else if (rb.velocity.x <= -0.05f)
            spriteRenderer.flipX = true;

        // 4. 判斷是否抵達目前的路徑點，並前往下一個
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }
}