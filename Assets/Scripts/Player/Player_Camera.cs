using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera : MonoBehaviour
{
   [Header("Target")]
    [SerializeField] Transform target;
    public Vector3 offset = new Vector3(0, 0f, -10f);

    [Header("Smooth follow")]
    [Range(0.01f, 1f)]
    [SerializeField] float smoothSpeedX = 0.125f; // vízszintes simítás
    [SerializeField] float smoothSpeedY = 0.1f;   // függőleges simítás lassabb

    [Header("Pálya határai")]
    public float minX, maxX, minY, maxY;
    [SerializeField] float boundsLerpSpeed = 2f;

    [Header("Előre nézés")]
    [SerializeField] float lookAheadDistance = 2f;
    private float lastTargetX;

    [Header("Dead Zone")]
    [SerializeField] float deadZoneWidth = 3f;
    [SerializeField] float deadZoneHeight = 2f;
    [SerializeField] Vector2 deadZoneOffset = new Vector2(0, -1f);


    void Start()
    {
        if (target != null)
            lastTargetX = target.position.x;
    }

    public void LateUpdate()
    {
       
        if (target == null) return;

        Vector3 currentPosition = transform.position;

        // ---- LOOK AHEAD ----
        float moveDir = target.position.x - lastTargetX;
        Vector3 lookAhead = new Vector3(moveDir * lookAheadDistance, 0, 0);

        // ---- DEAD ZONE OFFSET ----
        float centerX = currentPosition.x + deadZoneOffset.x;
        float centerY = currentPosition.y + deadZoneOffset.y;

        float left = centerX - deadZoneWidth / 2;
        float right = centerX + deadZoneWidth / 2;
        float bottom = centerY - deadZoneHeight / 2;
        float top = centerY + deadZoneHeight / 2;

        float targetX = currentPosition.x;
        float targetY = currentPosition.y;

        // ---- X tengely (dead zone) ----
        if (target.position.x < left)
        {
            targetX += (target.position.x - left);
        }
        else if (target.position.x > right)
        {
            targetX += (target.position.x - right);
        }
        // ---- Y tengely (dead zone) ----
        if (target.position.y < bottom)
        {
            targetY += (target.position.y - bottom);
        }
        else if (target.position.y > top)
        {
            targetY += (target.position.y - top);
        }

        // ---- kívánt pozíció ----
        Vector3 desiredPosition = new Vector3(targetX, targetY, 0) + offset + lookAhead;

        // ---- clamp (pályahatárok) ----
        desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
        desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

        // ---- simítás ----
        float smoothX = Mathf.Lerp(currentPosition.x, desiredPosition.x, smoothSpeedX);
        float smoothY = Mathf.Lerp(currentPosition.y, desiredPosition.y, smoothSpeedY);

        transform.position = new Vector3(smoothX, smoothY, desiredPosition.z);

        lastTargetX = target.position.x;
    }

    public void SetBounds(Camera_Zone zone)
    {
        switch (zone.isMinX)
        {
            case true:
                minX = zone.minX;
                break;
        }
        switch (zone.isMaxX)
        {
            case true:
                maxX = zone.maxX;
                break;
        }
        switch (zone.isMinY)
        {
            case true:
                minY = zone.minY;
                break;
        }
        switch (zone.isMaxY)
        {
            case true:
                maxY = zone.maxY;
                break;
        }
    }

        
    public void SetBoundsBack()
    {
        minX = -9999;
        maxX = 9999;
        minY = -9999;
        maxY = 9999;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Camera_Zone zone = other.GetComponent<Camera_Zone>();

        if (zone != null)
        {
            SetBounds(zone);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Camera_Zone zone = other.GetComponent<Camera_Zone>();

        if (zone != null)
        {
            SetBoundsBack();
        }
    }

    //Debughoz
    void OnDrawGizmos()
    {
        if (target == null) return;

        // Dead zone
        Gizmos.color = Color.green;
        Vector3 camPos = transform.position;

        Vector3 center = new Vector3(
            camPos.x + deadZoneOffset.x,
            camPos.y + deadZoneOffset.y,
            0
        );

        Vector3 size = new Vector3(deadZoneWidth, deadZoneHeight, 0);
        Gizmos.DrawWireCube(center, size);

        // Player pont
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(target.position, 0.2f);
    }
}
