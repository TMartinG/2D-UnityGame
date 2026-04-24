using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingSceenManager : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider slider;

    public void LoadScene(int levelIndex)
    {
        StartCoroutine(LoadSceneAsync(levelIndex));
    }

    IEnumerator LoadSceneAsync(int sceneIndex)
    {
        loadingScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        operation.allowSceneActivation = false;

        while (operation.progress < 0.9f)
        {
            slider.value = Mathf.Clamp01(operation.progress / 0.9f);
            yield return null;
        }

        // Slider kitöltése egészen
        slider.value = 1f;

        // Kis várakozás (opcionális, animációhoz)
        //yield return new WaitForSeconds(0.2f);

        // Itt engedélyezzük az új scene aktiválását
        operation.allowSceneActivation = true;
    }
}
