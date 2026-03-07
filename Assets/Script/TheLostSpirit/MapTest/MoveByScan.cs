using UnityEngine;
using Pathfinding;
using System.Collections;

[RequireComponent(typeof(Seeker))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))] // 建議使用膠囊碰撞器有利於避開卡死
public class MoveByScan : MonoBehaviour
{
    [Header("目標設定")]
    public Transform target; // 玩家

    [Header("移動與跳躍參數")]
    public float walkSpeed = 5f; // 平地移動速度
    public float jumpForce = 12f; // 跳躍的瞬間脈衝力 (這個值通常需要比較大)
    public float nextWaypointDistance = 0.5f; // 節點判定距離

    [Header("地面偵測 (必須設定)")]
    public LayerMask groundLayer; // 地板的 Layer (要在 Inspector 中選取)
    public Transform groundCheck; // 在敵人腳下的空物件 (用來偵測地面)
    public float groundCheckRadius = 0.2f; // 地面偵測半徑

    private Seeker _seeker;
    private Rigidbody2D _rb;
    private Path _path;
    private int _currentWaypoint = 0;
    private bool _isGrounded = false;
    private bool _isJumping = false;

    void Start()
    {
        _seeker = GetComponent<Seeker>();
        _rb = GetComponent<Rigidbody2D>();

        // 重要物理設定
        _rb.freezeRotation = true; // 防止敵人跌倒
        _rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous; // 防止穿牆

        InvokeRepeating(nameof(UpdatePath), 0f, 0.5f);
    }

    void UpdatePath()
    {
        if (_seeker.IsDone() && target != null)
        {
            _seeker.StartPath(transform.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            _path = p;
            _currentWaypoint = 0;
        }
    }

    void Update()
    {
        // 每幀檢查地面
        bool prevGrounded = _isGrounded;
        _isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // 如果剛落地，重置跳躍狀態
        if (_isGrounded && !prevGrounded)
        {
            _isJumping = false;
        }
    }

    void FixedUpdate()
    {
        if (_path == null) return;
        if (_currentWaypoint >= _path.vectorPath.Count) return;

        // 計算前往下個節點的方向
        Vector2 targetWaypoint = _path.vectorPath[_currentWaypoint];
        Vector2 currentPosition = (Vector2)transform.position;
        Vector2 direction = (targetWaypoint - currentPosition).normalized;

        // 【關鍵修改 1：空中掉落移動】
        // 如果我們在空中 (掉落中，你在 image_0 遇到的情況)
        if (!_isGrounded)
        {
            // 如果目標點高於自己，且我們不是在跳躍中，我們不控制垂直速度，垂直交給 Unity 的重力自然落下。
            // 我們只給予水平移動速度 (`velocity.x`)，讓它左右飄過去。
            _rb.velocity = new Vector2(direction.x * walkSpeed, _rb.velocity.y);
        }
        else // 我們在平台上 (平地移動)
        {
            // 如果我們不是在跳躍狀態下落，我們只控制水平速度 (`velocity.x`)。
            _rb.velocity = new Vector2(direction.x * walkSpeed, _rb.velocity.y);

            // 【關鍵修改 2：跳躍邏輯】
            // 當下一個節點「明顯高於自己」，且我們在地面，且還沒跳過
            float verticalDistance = targetWaypoint.y - currentPosition.y;
            if (verticalDistance > 0.3f && !_isJumping)
            {
                // 執行跳躍脈衝
                // 我們把 Y 軸速度歸零後立即加上脈衝，確保跳躍力完整
                _rb.velocity = new Vector2(_rb.velocity.x, 0f);
                _rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                _isJumping = true; // 鎖定狀態避免連跳
            }
        }

        // 判定距離
        float distance = Vector2.Distance(transform.position, targetWaypoint);
        if (distance < nextWaypointDistance)
        {
            _currentWaypoint++;
        }
    }

    // 畫出地面偵測範圍方便檢查
    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}