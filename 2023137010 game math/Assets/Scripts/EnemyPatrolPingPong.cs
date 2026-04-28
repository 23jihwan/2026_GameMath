using UnityEngine;

public class EnemyPatrolPingPong : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public float duration = 1f;

    void Update()
    {
        float t = Mathf.PingPong(Time.time / duration, 1f);
        t = Mathf.SmoothStep(0f, 1f, t); // 부드럽게

        transform.position = Vector3.Lerp(pointA.position, pointB.position, t);
    }
}