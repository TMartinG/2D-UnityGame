using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paralax_Background : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxEffect = 0.5f;

    private Vector3 lastCameraPosition;


    void Awake()
    {
        cameraTransform = FindObjectOfType<Camera>().transform;
    }
    void Start()
    {
        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;

        // csak X tengelyen (side-scrollerhez)
        transform.position += new Vector3(deltaMovement.x * parallaxEffect, 0, 0);

        lastCameraPosition = cameraTransform.position;
    }
}
