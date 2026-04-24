using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public GameObject menuPanel;
    public GameObject settingsPanel;


    void Start()
    {
        menuPanel.SetActive(false);
        settingsPanel.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (settingsPanel.activeSelf)
            {
                settingsPanel.SetActive(false);
            }
            else if (menuPanel.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // megállítja az időt
    }

    public void Resume()
    {
        menuPanel.SetActive(false);
        Time.timeScale = 1f; // folytatja az időt
    }

    public void Quit()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }
    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}
