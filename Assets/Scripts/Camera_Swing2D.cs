using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwing2D : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Rigidbody2D targetRigidbody;
    [SerializeField] private Vector3 offset = new Vector3(0, 1.5f, -10f);

    [Header("Smooth Follow")]
    [SerializeField] private float smoothTime = 0.12f;

    [Header("Swing Settings")]
    [SerializeField] private float swingAmount = 0.2f;  // mennyire billeg
    [SerializeField] private float swingSpeed = 5f;     // gyorsaság

    private Vector3 velocity = Vector3.zero;
    private Vector3 swingOffset = Vector3.zero;
    private Vector2 lastVelocity;

    void LateUpdate()
    {
        if (targetRigidbody == null) return;

        // Swing számítás
        Vector2 currentVelocity = targetRigidbody.velocity;
        Vector2 velocityChange = currentVelocity - lastVelocity;

        // Függőleges és vízszintes mozgás befolyásolja a swinget
        swingOffset.x = Mathf.Lerp(swingOffset.x, -velocityChange.x * swingAmount, Time.deltaTime * swingSpeed);
        swingOffset.y = Mathf.Lerp(swingOffset.y, -velocityChange.y * swingAmount, Time.deltaTime * swingSpeed);

        lastVelocity = currentVelocity;

        // Kívánt pozíció
        Vector3 desiredPosition = (Vector3)targetRigidbody.position + offset + swingOffset;

        // Smooth követés
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, smoothTime);
    }
}