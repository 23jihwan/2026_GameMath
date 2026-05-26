using UnityEngine;

public class CustomManualMine : MonoBehaviour
{
    [Header("Explosion Settings (ga.png 기반)")]
    public float delay = 2f;               // 바닥 안착 후 폭발까지 걸리는 시간
    public float radius = 5f;              // 폭발 반경
    public float force = 15f;              // 팅겨나갈 폭발력
    public float upwardsModifier = 1.5f;   // 위로 튀어오르게 만드는 Y축 가중치

    private Rigidbody rb;
    private bool isPinned = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision col)
    {
        // 바닥에 닿으면 튕기지 않고 즉시 바닥 고정
        if (!isPinned && col.gameObject.CompareTag("Ground"))
        {
            isPinned = true;

            // 속도를 0으로 만들고 물리 연산을 멈춰 고정시킴
            rb.linearVelocity = Vector3.zero;
            rb.isKinematic = true;

            // ga.png의 방식대로 지연 시간(delay) 후 Explode 함수 실행
            Invoke("Explode", delay);
        }
    }

    // ga.png의 수동 폭발 시스템 완벽 이식 (AddExplosionForce 사용 안함)
    void Explode()
    {
        Vector3 explosionPos = transform.position;
        // 폭발 반경 내 오브젝트 검출
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        foreach (var col in colliders)
        {
            Rigidbody targetRb = col.attachedRigidbody;
            if (targetRb == null) continue;

            // [요청 조건: 플레이어와 적 모두 폭발 영향을 받음]
            if (col.CompareTag("Player") || col.CompareTag("Enemy"))
            {
                // 폭발 중심지에서 타겟으로 향하는 거리와 방향 계산
                Vector3 toTarget = targetRb.position - explosionPos;
                float distance = toTarget.magnitude;

                Vector3 dir = toTarget.normalized;

                // 거리가 가까울수록 세게, 멀수록 약하게 (ga.png 공식)
                float attenuation = 1f - Mathf.Clamp01(distance / radius);

                // [요청 조건: 하늘 위로 약간 튀어 오르게 만듦]
                // 팅겨나가는 방향에 위쪽(up) 벡터와 가중치를 더해 대각선 위로 솟구치게 함
                dir += Vector3.up * upwardsModifier;
                dir = dir.normalized;

                // 최종 수동 폭발 힘 계산 및 Impulse 적용
                Vector3 impulse = dir * force * attenuation;
                targetRb.AddForce(impulse, ForceMode.Impulse);
            }
        }

        Destroy(gameObject); // 지뢰 오브젝트 제거
    }

    // ga.png에 있던 기즈모 반경 시각화 코드 그대로 유지
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}