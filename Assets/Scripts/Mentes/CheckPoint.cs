using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool activated = false;
    public GameObject savedText;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated) return;

        if (collision.CompareTag("Player"))
        {
            Activate(collision.gameObject);
        }
    }

    void Activate(GameObject player)
    {
        activated = true;

        BiomStateManagger.Instance.SetCurrentBiom(player.GetComponent<Player_Character>().currentBiomeIndex);

        SaveSystem.SaveGame(player);
        StartCoroutine(SaveText());
        //Debug.Log("Checkpoint elmentve!");
    }
    IEnumerator SaveText()
    {
        savedText.SetActive(true);

        yield return new WaitForSeconds(1.5f);

        savedText.SetActive(false);


    }

}
