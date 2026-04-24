using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Vector2 Reflect(Vector2 incomingDirection, Vector2 normal)
    {
        return Vector2.Reflect(incomingDirection, normal);
    }
}
