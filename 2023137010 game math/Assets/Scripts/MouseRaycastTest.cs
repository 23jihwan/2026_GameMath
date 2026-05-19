using UnityEngine;
using UnityEngine.InputSystem;

public class MouseRaycastTest : MonoBehaviour
{
    public float rayDistance = 100f;

    public CameraOrbit cam;

    public int playerNumber = 1;

    public float hitForce = 10f;

    bool canInput = true;

    public void OnMove(InputValue value)
    {
        if (!canInput) return;

        if (GameManager.instance.currentTurn != playerNumber)
            return;

        Vector2 input = value.Get<Vector2>();
        cam.moveInput = input.x;
    }

    public void OnClick(InputValue value)
    {
        if (!value.isPressed || !canInput) return;

        if (GameManager.instance.currentTurn != playerNumber)
            return;

        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = Camera.main.ScreenPointToRay(mousePos);

        if (Physics.Raycast(ray, out RaycastHit hit, rayDistance))
        {
            Rigidbody rb = hit.collider.attachedRigidbody;
            if (rb == null) return;

            string name = hit.collider.gameObject.name;

            // 2️⃣ 자기 공만 가능
            if (playerNumber == 1 && name != "1P Ball") return;
            if (playerNumber == 2 && name != "2P Ball") return;

            canInput = false;

            Vector3 dir = rb.position - hit.point;
            dir.y = 0;
            dir.Normalize();

            rb.AddForce(dir * hitForce, ForceMode.Impulse);

            HandleScore(name);
        }
    }

    void HandleScore(string ballName)
    {
        int me = playerNumber;

        // 5️⃣ Target Ball 맞추면 +1
        if (ballName == "Target Ball1" || ballName == "Target Ball2")
        {
            GameManager.instance.AddScore(me, +1);
            return;
        }

        // 6️⃣ 상대 공 맞추면 -1
        if (playerNumber == 1 && ballName == "2P Ball")
            GameManager.instance.AddScore(me, -1);

        if (playerNumber == 2 && ballName == "1P Ball")
            GameManager.instance.AddScore(me, -1);
    }

    void Update()
    {
        // 3️⃣ 공이 멈추면 입력 다시 허용
        if (GameManager.instance.AllBallsStopped())
        {
            canInput = true;
        }
    }
}