using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLookOffline : MonoBehaviour
{
    //vars
    public float mouseSensitivity = 100f;

    public Transform playerBody;

    float xRotation = 0f;

    //at start lock and hide cursor
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
            TakeCamera();
    }

    //Code for rotating player and looking around
    private void TakeCamera()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxisRaw("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, transform.rotation.eulerAngles.y, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
