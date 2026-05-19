using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform target;

    private float yaw = 0f;
    public float moveInput = 0f;

    public float rotateSpeed = 100f;
    public Vector3 offset = new Vector3(0f, 4f, -7f);

    void Update()
    {
        // 현재 턴이 아닐 때 카메라 회전 막기
        if (GameManager.instance != null &&
            GameManager.instance.currentTurn != GetComponentInParent<MouseRaycastTest>().playerNumber)
        {
            moveInput = 0;
        }

        yaw = moveInput * rotateSpeed * Time.deltaTime;

        Quaternion rotation = Quaternion.Euler(0f, yaw, 0f);
        Vector3 rotatedOffset = rotation * offset;

        transform.position = target.position + rotatedOffset;
        transform.LookAt(target);
    }
}
