using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject continueBTN;
    public GameObject newGameBTN;


    public GameObject playBTN;
    public GameObject quitBTN;
    public GameObject mainPanel;
    string savePath;
    public LoadingSceenManager loader;

    void Start()
    {
        savePath = Application.persistentDataPath + "/save.json";

        bool hasSave = File.Exists(savePath);

        // ha van mentés
        continueBTN.SetActive(hasSave);
        newGameBTN.SetActive(hasSave);

        // ha nincs mentés
        playBTN.SetActive(!hasSave);
        Cursor.visible = true;

        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            mainPanel.SetActive(true);
            Cursor.visible = true;
        }
    }

    public void StartGameClcik()
    {
        loader.LoadScene(1);
    }

    public void NewGameClick()
    {
        if (File.Exists(savePath))
            File.Delete(savePath);

        loader.LoadScene(1);
    }

    public void QuitGameClcik()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
    public void SettingsClick()
    {
        mainPanel.SetActive(false);
        
    }
    public void BackClick()
    {
        mainPanel.SetActive(true);
        
    }

}
