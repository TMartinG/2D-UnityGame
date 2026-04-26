using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoader : MonoBehaviour
{
    public FadeController fade;

    IEnumerator Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<Rigidbody2D>().simulated = false;

        // azonnal fekete képernyő
        yield return fade.FadeOutInstant();

        yield return null;

        bool hasSave = SaveSystem.HasSave();

        if (hasSave)
        {
            
            SaveSystem.LoadGame(player);
        }
        else
        {
            BiomStateManagger.Instance.SetCurrentBiom(1);
            BiomStateManagger.Instance.ResetBiome(1);
            BiomStateManagger.Instance.SetBiome(1);
        }

        // biztosítjuk hogy minden spawn kész
        yield return null;
        yield return null;

        // player pozíció fix (ne essen le)
        player.transform.position += Vector3.up * 2f;

        // fade be 
        yield return fade.FadeIn();
        player.GetComponent<Rigidbody2D>().simulated = true;

    }
    
}
