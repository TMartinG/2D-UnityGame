using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndDoor : MonoBehaviour
{
    public Player_Character playerCharacter;
    public Transform targetPoint;
    public FadeController fade;
    private bool playerInside = false;
    private GameObject player;

    private bool isTeleporting = false;


    private void Update()
    {

        if (playerInside && Input.GetKeyDown(KeyCode.Return) && !isTeleporting && playerCharacter.CanEnd())
        {
            StartCoroutine(Teleport());
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }

    IEnumerator Teleport()
    {
        isTeleporting = true;
        yield return StartCoroutine(fade.FadeOut());

        playerCharacter.enabled = false;

        // teleport player
        player.transform.position = targetPoint.position;

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(fade.FadeIn());

       // playerMovement.enabled = true;
        playerCharacter.enabled = true;

        isTeleporting = false;
    }


}
