using UnityEngine;
using Pathfinding; // 務必引用 A* 命名空間

public class AirEnemyAI : MonoBehaviour
{
    [Header("目標設定")]
    public Transform target;
    public float activateDistance = 10f; // 感應距離

    private Seeker seeker;
    private AIPath aiPath;

    void Start()
    {
        seeker = GetComponent<Seeker>();
        aiPath = GetComponent<AIPath>();

        // 確保剛開始不移動
        aiPath.canMove = false;
    }

    void Update()
    {
        if (target == null) return;

        float distanceToTarget = Vector2.Distance(transform.position, target.position);

        // 當玩家進入範圍內，開啟移動
        if (distanceToTarget < activateDistance)
        {
            aiPath.canMove = true;
            // 更新目的地
            aiPath.destination = target.position;
        }
        else
        {
            // 玩家跑太遠，停止追逐
            aiPath.canMove = false;
        }
    }
}