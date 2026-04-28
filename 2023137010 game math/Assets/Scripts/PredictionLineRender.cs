using UnityEngine;
using UnityEngine.InputSystem;

public class PredictionLineRender : MonoBehaviour
{
    public Transform starPos;       //A

    public Transform endPos;        //B

    [Range(1f, 5f)] public float extend = 1.5f;

    private LineRenderer Ir;

    void Awake()
    {
        Ir = GetComponent<LineRenderer>();
        Ir.positionCount = 2;               //단순 직선이므로 점 2개
        Ir.widthMultiplier = 0.05f;         // 두께
        Ir.material = new Material(Shader.Find("Unlit/Color"))
        {
            color = Color.red
        };
    }

    void Update()
    {
        if(!starPos || !endPos) return;
        Vector3 a = starPos.position;
        Vector3 b = endPos.position;
        Vector3 pred = Vector3.LerpUnclamped(a, b, extend);
        Ir.SetPosition(0, a);
        Ir.SetPosition(1, pred);
    }


    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

        if(Physics.Raycast(ray,out RaycastHit hit))
        {
            if(hit.collider.CompareTag("Enemy"))
            {
                //타게팅 시 적에게 선 고정시키게 발사
                endPos = hit.transform;
                starPos = transform;
                Ir.positionCount = 2;
            }
        }

        else
        {
            //타케팅 초기화시 선 없에기
            endPos = null;
            starPos = null;
            Ir.positionCount = 0;
        }
    }
}


