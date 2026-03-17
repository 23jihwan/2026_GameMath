using UnityEngine;
using UnityEngine.UIElements;

public class Hi : MonoBehaviour
{
    public Transform player;
    public float viewAngle = 60f; // 시야각

    void Update()
    {
        Vector3 toPlayer = (player.position - transform.position).normalized;
        Vector3 forward = transform.forward;

        // Vector3.Dot 대신 직접 계산
        float dot = forward.x * toPlayer.x +
                    forward.y * toPlayer.y +
                    forward.z * toPlayer.z;

        float angle = Mathf.Acos(dot) * Mathf.Rad2Deg; // 각도로 변환

        if (angle < viewAngle / 2)
        {
            Debug.Log("플레이어가 시야 안에 있음!");
        }
    }
}