﻿using UnityEngine;

/*
    This file has a commented version with details about how each line works. 
    The commented version contains code that is easier and simpler to read. This file is minified.
*/

/// <summary>
/// Camera movement script for third person games.
/// This Script should not be applied to the camera! It is attached to an empty object and inside
/// it (as a child object) should be your game's MainCamera.
/// </summary>
public class CameraController : MonoBehaviour
{
    [Tooltip("Enable to move the camera by holding the right mouse button. Does not work with joysticks.")]
    public bool clickToMoveCamera = false;
    [Tooltip("Enable zoom in/out when scrolling the mouse wheel. Does not work with joysticks.")]
    public bool canZoom = true;
    [Space]
    [Tooltip("The higher it is, the faster the camera moves. It is recommended to increase this value for games that uses joystick.")]
    public float sensitivity = 5f;

    [Tooltip("Camera Y rotation limits. The X axis is the maximum it can go up and the Y axis is the maximum it can go down.")]
    public Vector2 cameraLimit = new Vector2(-45, 40);

    float mouseX;
    float mouseY;
    float offsetDistanceY;

    Transform player;

    // Added: Flag to enable/disable camera controller
    private bool isCameraEnabled = true;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        offsetDistanceY = transform.position.y;

        if (!clickToMoveCamera)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }

    void Update()
    {
        if (!isCameraEnabled) return; // Early exit if camera is disabled

        // Follow player - camera offset
        transform.position = player.position + new Vector3(0, offsetDistanceY, 0);

        // Set camera zoom when mouse wheel is scrolled
        if (canZoom && Input.GetAxis("Mouse ScrollWheel") != 0)
            Camera.main.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * sensitivity * 2;

        // Checker for right click to move camera
        if (clickToMoveCamera)
            if (Input.GetAxisRaw("Fire2") == 0)
                return;

        // Calculate new position
        mouseX += Input.GetAxis("Mouse X") * sensitivity;
        mouseY += Input.GetAxis("Mouse Y") * sensitivity;
        // Apply camera limits
        mouseY = Mathf.Clamp(mouseY, cameraLimit.x, cameraLimit.y);

        transform.rotation = Quaternion.Euler(-mouseY, mouseX, 0);
    }

    // Added: Public method to enable/disable the camera controller
    public void SetCameraEnabled(bool enabled)
    {
        isCameraEnabled = enabled;
        if (!enabled && !clickToMoveCamera)
        {
            // Unlock cursor when disabling if not using click-to-move
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }
        else if (enabled && !clickToMoveCamera)
        {
            // Relock cursor when re-enabling
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }
    }
}