using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;
using Mono.Cecil;


public class Bezier : MonoBehaviour
{
    public Transform p0;
    public Transform p3;

    [Header("Random Ranges")]
    public float p1Radius = 2f;

    public float p2Radius = 2f;

    public float p1Height = 3f;

    public float p2Height = 3f;

    // 결과 제어점

    [HideInInspector] public Vector3 p1;

    [HideInInspector] public Vector3 p2;

    List<Vector3> points;

    float time = 0f;

    void Awake()
    {
        GenerateRandomControlPoints();
        points = new List<Vector3> { p0.position, p1, p2, p3.position };
    }

    void Update()
    {
        time += Time.deltaTime / 2f;
        transform.position = DeCasteIjau(points, time);
    }

    void GenerateRandomControlPoints()
    {
        Vector2 rand1 = Random.insideUnitCircle * p1Radius;
        p1 = p0.position + new Vector3(rand1.x, 0f, rand1.y);
        p1.y += p1Height;           //도착 직전 살짝 꺾이도록

        Vector2 rand2 = Random.insideUnitCircle * p2Radius;
        p2 = p3.position + new Vector3(rand2.x, 0f, rand2.y);
        p2.y += p2Height;           // 도착 직전 살짝 꺾이도록
    }

    Vector3 DeCasteIjau(List<Vector3> p,float t)
    {
        while(p.Count>1)
        {
            int last = p.Count - 1;

            var next = new List<Vector3>(last);
            for (int i = 0; i < last; i++)
                next.Add(Vector3.Lerp(p[1], p[i + 1], t));
            p = next;
        }

        return p[0];
    }
    //private float FourPointBezier(float a, float b,float c,float d)
    //{
       // return Mathf.Pow(1 - t, 3) * a
           // + Mathf.Pow(1 - t, 2) * 3 * t * b
           // + Mathf.Pow(t, 2) * 3 * (1 - t) * c
           // + Mathf.Pow(t, 3) * d;
    //}
}

