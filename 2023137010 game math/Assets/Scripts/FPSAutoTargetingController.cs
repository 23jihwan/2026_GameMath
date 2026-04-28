using UnityEngine;
using UnityEngine.InputSystem;

public class FPSAutoTargetingController : MonoBehaviour
{
    public Camera mainCamera;
    public CameraAutoAimSlerp cameraAim;
    public CrosshairLerpUI crosshairUI;

    public Transform currentTarget;

    public void OnRightClick(InputValue value)
    {
        if (!value.isPressed) return;

        Ray ray = mainCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                currentTarget = hit.collider.transform;

                cameraAim.SetTarget(currentTarget);
                crosshairUI.Show();

                return;
            }
        }

        ClearTarget();
    }

    void ClearTarget()
    {
        currentTarget = null;
        cameraAim.ClearTarget();
        crosshairUI.Hide();
    }
}