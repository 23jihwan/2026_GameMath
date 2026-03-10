using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Vector2 mouseScreenPosition;
    private Vector3 targetPosition;
    private bool isMoving = false;

    public void OnPoint(InputValue value)
    {
        mouseScreenPosition = value.Get<Vector2>();
    }

    public void OnClick(InputValue value)
    {
        if (value.isPressed)
        {
            Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
            RaycastHit[] hits = Physics.RaycastAll(ray);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider.gameObject != gameObject)
                {
                    targetPosition = hit.point;
                    targetPosition.y = transform.position.y;
                    isMoving = true;
                    break;
                }
            }
        }
    }

    void Update()
    {
        if (isMoving)
        {
            Vector3 direction = targetPosition - transform.position;

            float length = Mathf.Sqrt(
                direction.x * direction.x +
                direction.y * direction.y +
                direction.z * direction.z
            );

            if (length > 0.001f)
            {
                Vector3 unitDir = direction / length;
                float move = moveSpeed * Time.deltaTime;

                if (move >= length)
                {
                    transform.position = targetPosition;
                    isMoving = false;
                }
                else
                {
                    transform.position += unitDir * move;
                }
            }
        }
    }
}