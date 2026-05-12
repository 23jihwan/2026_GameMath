using UnityEngine;
using UnityEngine.InputSystem;

public class BombLauncher : MonoBehaviour
{
    [Header("Target")]
    public Transform target; // 적

    [Header("Bomb")]
    public GameObject spherePrefab;

    [Header("Attack Settings")]
    public int bombCount = 10;
    public float moveTime = 1.5f;

    // Attack 입력 함수
    // PlayerInput의 Behavior를 Send Messages로 해야 호출됨
    public void OnAttack(InputValue value)
    {
        if (value.isPressed)
        {
            FireBombs();
        }
    }

    void FireBombs()
    {
        for (int i = 0; i < bombCount; i++)
        {
            // 플레이어 주변 랜덤 위치에서 생성
            Vector3 spawnOffset = new Vector3(
                Random.Range(-1.5f, 1.5f),
                Random.Range(0f, 1.5f),
                Random.Range(-1.5f, 1.5f)
            );

            GameObject bomb = Instantiate(
                spherePrefab,
                transform.position + spawnOffset,
                Quaternion.identity
            );

            // 베지어 이동 스크립트 추가
            BezierBomb bezier = bomb.AddComponent<BezierBomb>();

            bezier.startPoint = bomb.transform.position;

            // 도착 지점은 무조건 적
            bezier.endPoint = target.position;

            bezier.duration = moveTime;
        }
    }
}