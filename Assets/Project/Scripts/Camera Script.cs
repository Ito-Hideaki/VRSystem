using UnityEngine;

public class CameraScript : MonoBehaviour
{
    float xRotation = 0, yRotation = 0;
    Camera cameraCmp;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
       cameraCmp = GetComponent<Camera>();
        cameraCmp.clearFlags = CameraClearFlags.SolidColor;
        cameraCmp.backgroundColor = Color.black;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
