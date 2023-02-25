using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLook : MonoBehaviour
{
    public Camera cam;
    private float xRotation = 0f;
    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    private InputManager inputManager;//dev
    private bool isFreez=false;//dev 
    private void Start()
    {
        Cursor.visible = false;
        inputManager = GetComponent<InputManager>();//dev
    }
    private void Update()//dev
    {
        if (inputManager.OnFoot.CurFreeze.triggered)
        {
            if (!isFreez)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                isFreez = true;
            }
            else
            {
                isFreez = false;
            }
        }
    }
    public void ProcessLook(Vector2 input)
    {
        if (!isFreez)//dev
        {
            float mouseX = input.x;
            float mouseY = input.y;
            xRotation -= (mouseY * Time.deltaTime) * ySensitivity;
            xRotation = Mathf.Clamp(xRotation, -80f, 70f);
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
            transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);
        }
       
    }
}
