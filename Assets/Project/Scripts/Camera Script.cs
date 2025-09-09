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
        const float rotationSpeed = 0.5f;

        if(Input.GetKey(KeyCode.DownArrow) && xRotation < 90)
        {
            xRotation += rotationSpeed;
        }
        if (Input.GetKey(KeyCode.UpArrow) && xRotation > -90)
        {
            xRotation -= rotationSpeed;
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            yRotation -= rotationSpeed;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            yRotation += rotationSpeed;
        }

        this.transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
