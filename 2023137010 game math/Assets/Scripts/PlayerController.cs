using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 30f;
    private Vector2 moveInput;
    public bool isLeftParrying = false;
    public bool isRightParrying = false;
    // 왼쪽 패링 입력

    public void OnLeftParry(InputValue value )
    {
        isLeftParrying = value.isPressed;
    }

    // 오른쪽 패링 입력

    public void OnRightParry(InputValue value )
    {
        isRightParrying=value.isPressed;
    }

    public void OnMove(InputValue value)
    {
        moveInput=value.Get<Vector2>();
    }

    
    void Update()
    {
        float rotation = moveInput.x * rotationSpeed * Time.deltaTime;
        transform.Rotate(0f, rotation, 0f);

        Vector3 moveDir = transform.forward * moveInput.y * moveSpeed * Time.deltaTime;
        transform.Translate(moveDir);
    }
}
