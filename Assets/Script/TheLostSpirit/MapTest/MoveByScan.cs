using Pathfinding;
using UnityEngine;

namespace TheLostSpirit.MapTest
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Seeker))]
    public class SimpleAIPlatformer : MonoBehaviour
    {
        [Header("�ؼг]�w")]
        public Transform target;

        [Header("���ʰѼ�")]
        public float speed = 5f;
        public float jumpForce = 16f;

        [Header("�M���]�w")]
        // ������ק�G�o�ӼƭȶV�p�A���ʶV��T�A��ĳ�] 0.2 ~ 0.5
        public float nextWaypointDistance = 0.5f;

        // ���D�P�_�����׮t (��ĳ 0.4�A�קK���a�ø�)
        public float jumpNodeHeightRequirement = 0.4f;

        [Header("���z�P�a��")]
        public Transform groundCheck;
        public float     groundCheckRadius = 0.2f; // ������ק�G���n�Ӥj�A0.2 ��n
        public LayerMask groundLayer;

        private Path        path;
        private int         currentWaypoint = 0;
        private bool        isGrounded;
        private Seeker      seeker;
        private Rigidbody2D rb;

        void Start()
        {
            seeker = GetComponent<Seeker>();
            rb     = GetComponent<Rigidbody2D>();

            // ������ק�G�}�Ҵ��ȡA�����ʵe���ݰ_�ӧ󵷷�
            rb.interpolation  = RigidbodyInterpolation2D.Interpolate;
            rb.gravityScale   = 3f;
            rb.freezeRotation = true;

            InvokeRepeating(nameof(UpdatePath), 0f, 0.2f); // �[�ָ��|��s�W�v
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
                path            = p;
                currentWaypoint = 0;
            }
        }

        void FixedUpdate()
        {
            // 1. �a������
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

            if (path == null || currentWaypoint >= path.vectorPath.Count) return;

            // 2. ���o���|��T
            Vector2 currentPos = rb.position;
            Vector2 nextPoint  = path.vectorPath[currentWaypoint];

            // ��X��V (Normalize ���ƭȫO���b -1 ~ 1 ����)
            Vector2 direction = (nextPoint - currentPos).normalized;
            float   distance  = Vector2.Distance(currentPos, nextPoint);

            // ==========================================================
            // �� ���Ƹ��D�޿� ��
            // ==========================================================

            // �P�_���� 1: �ؼ��I��ڰ� (�ݭn��)
            bool targetIsHigh = nextPoint.y > currentPos.y + jumpNodeHeightRequirement;

            // �P�_���� 2: ��������P�w (�ؼЦb���Y���AX �b�t���ܤp)
            bool isVerticalPath = Mathf.Abs(nextPoint.x - currentPos.x) < 0.3f && nextPoint.y > currentPos.y;

            // ������D
            if (isGrounded)
            {
                // �p�G�J�찪�B�A�Ϊ̸��|�I�N�b���Y���A�N��
                if (targetIsHigh || isVerticalPath)
                {
                    // �����@�ӦV�W���O�A�P�ɫO�d�@�I�I�쥻���D��
                    rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                }
            }

            // ==========================================================
            // �� ���Ʋ����޿� ��
            // ==========================================================

            float targetX = 0;

            // �u�����ݭn���ʪ��Z�����j�ɤ~���ʡA�קK�b��a�ݰ�
            if (Mathf.Abs(nextPoint.x - currentPos.x) > 0.1f)
            {
                targetX = direction.x > 0 ? speed : -speed;
            }

            // �Ť������u�ơG�p�G�b�Ť��A���n�������h����A���]���n��������V (��P���n)
            if (!isGrounded)
            {
                // ²�檩�G�Ť��]�i�H���t���� (�������Q��)
                // �p�G�A�Q�n�Ť�������@�I�A�i�H�� Mathf.Lerp
                targetX = direction.x > 0 ? speed : -speed;
            }

            // �M�γt��
            rb.velocity = new Vector2(targetX, rb.velocity.y);

            // ==========================================================
            // �� ���|�I���� ��
            // ==========================================================

            // �p�G�Z������A�N������U�@���I
            if (distance < nextWaypointDistance)
            {
                currentWaypoint++;
            }
            // �S�����p�G�p�G�w�g���L�Y�F (Y�b�W�L�ؼ�)�A�B X �b�ܱ���A�]����
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
}