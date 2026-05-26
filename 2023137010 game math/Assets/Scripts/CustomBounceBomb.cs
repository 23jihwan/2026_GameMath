using UnityEngine;

public class CustomBounceBomb : MonoBehaviour
{
    private int bounceCount = 0;
    private Rigidbody rb;
    private Vector3 lastVelocity; // 충돌 직전 프레임의 속도 저장용

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // 최신 유니티 규격인 linearVelocity 사용 (경고 예방)
        lastVelocity = rb.linearVelocity;
    }

    private void OnCollisionEnter(Collision col)
    {
        // 1. 적(Enemy 태그)에 닿으면 즉시 폭발
        if (col.gameObject.CompareTag("Enemy"))
        {
            Explode();
            return;
        }

        // 2. 바닥(Ground 태그)에 닿았을 때
        if (col.gameObject.CompareTag("Ground"))
        {
            bounceCount++;

            // 3번 튕긴 이후(즉, 3번째 튕기는 순간 바닥에 닿았을 때) 폭발
            if (bounceCount >= 3)
            {
                Explode();
                return;
            }

            // [zz.png 소스코드 기반: 수동 반사 벡터 계산 공식]
            Vector3 normal = col.contacts[0].normal.normalized; // 충돌 표면의 법선
            float dot = Vector3.Dot(lastVelocity, normal);
            Vector3 reflect = lastVelocity - 2f * dot * normal; // R = V - 2(V·N)N 공식

            // [요청 조건: 거리가 절반으로 줄어듦]
            // 앞으로 나아가는 수평 속도(X, Z)만 정확히 0.5배로 줄여서 거리를 반토막 냄
            Vector3 horizontalVel = new Vector3(reflect.x, 0f, reflect.z) * 0.5f;

            // 수직 속도(Y)는 위로 튕겨 올라가야 하므로 절대값 처리 후 유지
            Vector3 verticalVel = new Vector3(0f, Mathf.Abs(reflect.y), 0f);

            // 계산된 최적의 속도를 리지드바디에 직접 주입
            rb.linearVelocity = horizontalVel + verticalVel;
        }
    }

    private void Explode()
    {
        // 폭발 이펙트나 데미지 처리 들어가는 곳
        Destroy(gameObject);
    }
}