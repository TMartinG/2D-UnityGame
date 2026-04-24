using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{
    [Header("Refs")]
    public RectTransform crosshair;
    public Camera cam;
    public GameObject CUCC;

    [Header("Settings")]
    public float followSpeed = 20f;
    float targetSpeed = 20f;      // aktuális cél sebesség
    public LayerMask collisionMask; // Ground + Enemy


    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined; // az ablakon belül marad
        Cursor.visible = false; 
        crosshair.position = Input.mousePosition;
        CUCC.transform.position = cam.ScreenToWorldPoint(crosshair.position);

    }

    void FixedUpdate()
    {
            crosshair.position = Vector3.Lerp(
            crosshair.position,
            Input.mousePosition,
            Time.deltaTime * targetSpeed
        );
        CUCC.transform.position = cam.ScreenToWorldPoint(crosshair.position);
    }

    // Vörös Lövés közbeni lassítás
    public void CrosshairSlow(float slowSpeed)
    {
        targetSpeed = slowSpeed;
    }

    //  Visszaállítás
    public void CrosshairReset()
    {
        targetSpeed = followSpeed;
    }
    
    
}
