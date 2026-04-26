using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingSpikeZone : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            FallingSpike spike = GetComponentInChildren<FallingSpike>();
            if (spike != null)
            {
                spike.StartCoroutine(spike.FallAndRespawn());
            }
        }
    }


}
