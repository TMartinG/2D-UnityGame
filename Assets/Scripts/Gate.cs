using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public Transform targetPoint;
    public FadeController fade;
    private bool playerInside = false;
    private GameObject player;

    public int currentBiome;
    public int targetBiome;
    public GameObject[] biomes;
    public Player_Movement playerMovement;
    public Player_Character playerCharacter;
    public Light_Yellow lightYellow;
    public Light_Red lightRed;
    private bool isTeleporting = false;

    private void Update()
    {

        if (playerInside && Input.GetKeyDown(KeyCode.S) && !isTeleporting)
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
        lightYellow.enabled = false;
        lightRed.enabled = false;
        biomeChange();

        player.GetComponent<Player_Character>().currentBiomeIndex = targetBiome;

        // teleport player
        player.transform.position = targetPoint.position;

        yield return new WaitForSeconds(0.2f);

        yield return StartCoroutine(fade.FadeIn());

        playerCharacter.enabled = true;
        lightYellow.enabled = true;
        lightRed.enabled = true;

        isTeleporting = false;
    }

    public void biomeChange()
    {
        BiomStateManagger.Instance.biomes[currentBiome-1].SetActive(false);
        BiomStateManagger.Instance.biomes[targetBiome-1].SetActive(true);
    }
}
