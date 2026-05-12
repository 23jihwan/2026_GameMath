using UnityEngine;

public class BezierBomb : MonoBehaviour
{
    [HideInInspector] public Vector3 startPoint;
    [HideInInspector] public Vector3 endPoint;

    [HideInInspector] public float duration = 1.5f;

    private Vector3 p1;
    private Vector3 p2;

    private float currentTime;

    void Start()
    {
        CreateControlPoints();
    }

    void Update()
    {
        currentTime += Time.deltaTime;

        // 0 ~ 1
        float t = currentTime / duration;
        t = Mathf.Clamp01(t);

        // 3차 베지어 위치 계산
        transform.position = CalculateBezier(t);

        // 도착하면 삭제
        if (t >= 1f)
        {
            Destroy(gameObject);
        }
    }

    // 랜덤 제어점 생성
    void CreateControlPoints()
    {
        // 시작 → 목표 방향
        Vector3 dir = (endPoint - startPoint).normalized;

        // 방향의 수직 벡터
        Vector3 side = Vector3.Cross(dir, Vector3.up);

        // 첫 번째 제어점
        p1 = startPoint
            + dir * Random.Range(2f, 6f)
            + side * Random.Range(-8f, 8f)
            + Vector3.up * Random.Range(4f, 8f);

        // 두 번째 제어점
        p2 = startPoint
            + dir * Random.Range(6f, 12f)
            + side * Random.Range(-8f, 8f)
            + Vector3.up * Random.Range(4f, 8f);
    }

    // 3차 베지어 계산
    Vector3 CalculateBezier(float t)
    {
        float x = FourPointBezier(startPoint.x, p1.x, p2.x, endPoint.x, t);
        float y = FourPointBezier(startPoint.y, p1.y, p2.y, endPoint.y, t);
        float z = FourPointBezier(startPoint.z, p1.z, p2.z, endPoint.z, t);

        return new Vector3(x, y, z);
    }

    // 3차 베지어 공식
    float FourPointBezier(float a, float b, float c, float d, float t)
    {
        return Mathf.Pow(1 - t, 3) * a
            + 3 * Mathf.Pow(1 - t, 2) * t * b
            + 3 * (1 - t) * Mathf.Pow(t, 2) * c
            + Mathf.Pow(t, 3) * d;
    }
}