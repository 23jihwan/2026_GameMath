using UnityEngine;

public class LerpMover : MonoBehaviour
{
   public Transform startPos;

   public Transform endPos;

    [SerializeField] private float duration = 2f;


    [SerializeField] private float t = 0f;

    void Update()
    {


        t = Mathf.PingPong(Time.time / duration,1f);


        Vector3 a = startPos.position;
        Vector3 b = endPos.position;
        Vector3 p = (1f - t) * a + t * b;


        //transform.position = Vector3.LerpUnclamped(startPos.position, endPos.position, t);

        transform.position = p;
    }
}
