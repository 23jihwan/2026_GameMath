using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyAI : MonoBehaviour
{
    [Header("적 설정")]
    public float viewAngle = 60f;
    public float rotationSpeed = 30f;
    public float viewDistance = 5f;
    public float moveSpeed = 5f;
    public float parryDistance = 1.5f;

    private Transform playerTransform;
    private PlayerController playerScript;
    private bool isDetected = false;

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
            playerScript = player.GetComponent<PlayerController>();
        }
    }

    void Update()
    {
        if (!isDetected)
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
            DetectPlayer();
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, playerTransform.position, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, playerTransform.position) <= parryDistance)
            {
                HandleParryLogic();
            }
        }
    }

    void DetectPlayer()
    {
        Vector3 forward = transform.forward;
        Vector3 dirToPlayer = (playerTransform.position - transform.position).normalized;

        // [내적 직접 계산] Dot = x1*x2 + y1*y2 + z1*z2
        float dotProduct = (forward.x * dirToPlayer.x) + (forward.y * dirToPlayer.y) + (forward.z * dirToPlayer.z);

        // 내적 결과로 각도(Radian) 구하기: dot = cos(theta)
        float angle = Mathf.Acos(Mathf.Clamp(dotProduct, -1f, 1f)) * Mathf.Rad2Deg;

        if (angle < viewAngle * 0.5f && Vector3.Distance(transform.position, playerTransform.position) <= viewDistance)
        {
            isDetected = true;
        }
    }

    void HandleParryLogic()
    {
        Vector3 playerForward = playerTransform.forward;
        Vector3 dirFromPlayer = (transform.position - playerTransform.position).normalized;

        // [외적 직접 계산] Cross.y = z1*x2 - x1*z2
        // 플레이어 정면(A)과 적 방향(B)의 외적 y성분
        float crossY = (playerForward.z * dirFromPlayer.x) - (playerForward.x * dirFromPlayer.z);

        bool parrySuccess = false;

        // crossY가 양수이면 오른쪽, 음수이면 왼쪽 (좌표계 기준에 따라 다를 수 있음)
        if (crossY < 0 && playerScript.isLeftParrying)
        {
            parrySuccess = true;
        }
        else if (crossY > 0 && playerScript.isRightParrying)
        {
            parrySuccess = true;
        }

        if (parrySuccess)
        {
            Destroy(gameObject);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Vector3 forward = transform.forward * viewDistance;
        Vector3 leftBoundary = Quaternion.Euler(0, -viewAngle / 2, 0) * forward;
        Vector3 rightBoundary = Quaternion.Euler(0, viewAngle / 2, 0) * forward;

        Gizmos.DrawRay(transform.position, leftBoundary);
        Gizmos.DrawRay(transform.position, rightBoundary);
    }
}