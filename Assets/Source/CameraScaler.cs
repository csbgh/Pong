using UnityEngine;
using System.Collections;

public class CameraScaler : MonoBehaviour
{
    public int ReferenceWidth = 1366;
    public int ReferenceHeight = 768;

    private float RefOrthoSize = 1.0f;
    private int LastWidth;
    private int LastHeight;

    private Camera Cam;

    void Start()
    {
        LastWidth = Screen.width;
        LastHeight = Screen.height;
        RefOrthoSize = ReferenceHeight / 200.0f;

        Cam = GetComponent<Camera>();

        SetScreenSize();
    }

    void Update()
    {
        if (Screen.width != LastWidth || Screen.height != LastHeight)
        {
            OnResolutionChanged();
            LastWidth = Screen.width;
            LastHeight = Screen.height;
        }
    }

    private void OnResolutionChanged()
    {
        SetScreenSize();
    }

    private void SetScreenSize()
    {
        float RefRatio = ReferenceWidth / (float)ReferenceHeight;
        float CurRatio = Screen.width / (float)Screen.height;

        float OrthoSize = Mathf.Max(RefOrthoSize * (RefRatio / CurRatio), RefOrthoSize);

        Cam.orthographicSize = OrthoSize;
    }
}
