using UnityEngine;
using UnityEngine.InputSystem; // 신형 인풋 시스템 패키지

public class PlayerInputSkills : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject bombPrefab; // Q 스킬 폭탄 프리팹
    [SerializeField] private GameObject minePrefab; // W 스킬 지뢰 프리팹

    [Header("Launch Settings")]
    [SerializeField] private Transform spawnPoint;  // 발사 위치
    [SerializeField] private float launchForce = 15f; // 던지는 초기 힘

    private void Start()
    {
        if (spawnPoint == null) spawnPoint = this.transform;

        // 게임 시작 시 콘솔 창에 이 문구가 뜨는지 꼭 확인해줘!
        Debug.Log("🚀 [직스 스킬 스크립트] 준비 완료! 인풋 액션 'Q', 'W' 신호를 대기 중입니다.");
    }

    // [변경] 인풋 액션 이름이 'Q'일 때 유니티가 자동으로 실행하는 함수
    private void OnQ(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("🎯 Q 입력 감지 -> 폭탄 발사!");
            LaunchProjectile(bombPrefab);
        }
    }

    // [변경] 인풋 액션 이름이 'W'일 때 유니티가 자동으로 실행하는 함수
    private void OnW(InputValue value)
    {
        if (value.isPressed)
        {
            Debug.Log("🎯 W 입력 감지 -> 지뢰 발사!");
            LaunchProjectile(minePrefab);
        }
    }

    private void LaunchProjectile(GameObject prefab)
    {
        if (prefab == null)
        {
            Debug.LogError("❌ 에러: 인스펙터 창에서 폭탄(Bomb) 또는 지뢰(Mine) 프리팹이 연결되지 않았습니다!");
            return;
        }

        // 플레이어가 보고 있는 앞방향 + 살짝 위쪽(포물선) 벡터 계산
        Vector3 launchDir = (transform.forward + Vector3.up * 3f).normalized;

        GameObject obj = Instantiate(prefab, spawnPoint.position, transform.rotation);
        Rigidbody rb = obj.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // 최신 유니티 linearVelocity 방식으로 힘 전달
            rb.linearVelocity = launchDir * launchForce;
        }
        else
        {
            Debug.LogError($"❌ 에러: 생성된 {prefab.name} 프리팹에 Rigidbody 컴포넌트가 없습니다!");
        }
    }
}