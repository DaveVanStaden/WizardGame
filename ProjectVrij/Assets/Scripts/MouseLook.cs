using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public Transform body;

    float xRotation = 0;

    float mouseSensitivity = 2000;
    // mouse sensitivity varieert super veel tussen editor en de build: in de editor is een waarde van 1500 prettig om mee te spelen, in de build is 250 beter

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90, 90);

        body.Rotate(Vector3.up * mouseX);
        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        if (Input.GetKeyDown(KeyCode.M))
        {
            if (mouseSensitivity < 300)
            {
                mouseSensitivity = 2000;
            } else
            {
                mouseSensitivity = 0;
            }
        }
    }
}
