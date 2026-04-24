using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Camera_Change : MonoBehaviour
{
   
    [SerializeField]  GameObject player;
    private Player_Movement player_Movement;
    public float cameraSizeChange = 40f;
    public float cameraSizeDefault = 15f;


    void Start()
    {
        var obj = GameObject.FindGameObjectWithTag("Player");

        player_Movement = obj.GetComponent<Player_Movement>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            player_Movement.GrowToTarget(cameraSizeChange, 4f);
            player_Movement.maxCameraSize = cameraSizeChange;
            gameObject.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void camBack()
    {
        player_Movement.GrowToTarget(cameraSizeDefault, 4f);
        player_Movement.maxCameraSize = cameraSizeDefault;
    }


}
