using UnityEngine;

public class CrosshairLerpUI : MonoBehaviour
{
    public RectTransform crosshair;

    float t = 0f;
    bool isActive = false;

    void Update()
    {
        if (isActive)
            t += Time.deltaTime * 3f;
        else
            t -= Time.deltaTime * 3f;

        t = Mathf.Clamp01(t);

        float scale = Mathf.LerpUnclamped(0f, 1f, t);
        crosshair.localScale = Vector3.one * scale;
    }

    public void Show()
    {
        isActive = true;
    }

    public void Hide()
    {
        isActive = false;
    }
}