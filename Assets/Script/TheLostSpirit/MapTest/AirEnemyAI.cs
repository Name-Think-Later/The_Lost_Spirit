using Pathfinding;
using UnityEngine;

// �ȥ��ޥ� A* �R�W�Ŷ�

namespace TheLostSpirit.MapTest
{
    public class AirEnemyAI : MonoBehaviour
    {
        [Header("�ؼг]�w")]
        public Transform target;
        public float activateDistance = 10f; // �P���Z��

        private Seeker seeker;
        private AIPath aiPath;

        void Start()
        {
            seeker = GetComponent<Seeker>();
            aiPath = GetComponent<AIPath>();

            // �T�O��}�l������
            aiPath.canMove = false;
        }

        void Update()
        {
            if (target == null) return;

            float distanceToTarget = Vector2.Distance(transform.position, target.position);

            // �����a�i�J�d�򤺡A�}�Ҳ���
            if (distanceToTarget < activateDistance)
            {
                aiPath.canMove = true;
                // ��s�ت��a
                aiPath.destination = target.position;
            }
            else
            {
                // ���a�]�ӻ��A����l�v
                aiPath.canMove = false;
            }
        }
    }
}