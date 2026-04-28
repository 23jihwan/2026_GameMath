using UnityEngine;

public class CameraAutoAimSlerp : MonoBehaviour
{
    public Transform target;
    float speed = 5f;

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    public void ClearTarget()
    {
        target = null;
    }

    void Update()
    {
        if (target == null) return;

        Quaternion lookRot = Quaternion.LookRotation(target.position - transform.position);
        float t = 1f - Mathf.Exp(-speed * Time.deltaTime);

        transform.rotation = ManualSlerp(transform.rotation, lookRot, t);
    }

    Quaternion ManualSlerp(Quaternion from, Quaternion to, float t)
    {
        float dot = Quaternion.Dot(from, to);

        if (dot < 0f)
        {
            to = new Quaternion(-to.x, -to.y, -to.z, -to.w);
            dot = -dot;
        }

        float theta = Mathf.Acos(dot);
        float sinTheta = Mathf.Sin(theta);

        if (sinTheta < 0.001f) return to;

        float ratioA = Mathf.Sin((1f - t) * theta) / sinTheta;
        float ratioB = Mathf.Sin(t * theta) / sinTheta;

        Quaternion result = new Quaternion(
            ratioA * from.x + ratioB * to.x,
            ratioA * from.y + ratioB * to.y,
            ratioA * from.z + ratioB * to.z,
            ratioA * from.w + ratioB * to.w
        );

        return result.normalized;
    }
}