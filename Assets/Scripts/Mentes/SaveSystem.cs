using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.IO;

public class SaveSystem : MonoBehaviour
{
    public static void SaveGame(GameObject player)
    {
        SaveData data = new SaveData();

        // WORLD
        data.worldState = WorldStateManager.Instance.GetSaveData();

        // INVENTORY
        data.inventory = player.GetComponent<SkillInventory>().GetSaveData();

        // PLAYER
        data.player = new Player();
        data.player.playerPosition = player.transform.position;
        data.player.playerHP = player.GetComponent<Player_Character>().playerCurrentHP;
        data.player.cameraPosition = player.GetComponent<Player_Movement>().CameraMain.transform.position;
        data.player.cameraSize = player.GetComponent<Player_Movement>().CameraMain.orthographicSize;

        data.undiscoveredYellow = SkillManager.Instance.undiscoveredYellow
            .ConvertAll(s => s.skillID);

        data.undiscoveredRed = SkillManager.Instance.undiscoveredRed
            .ConvertAll(s => s.skillID);

        data.currentBiomeIndex = player.GetComponent<Player_Character>().currentBiomeIndex;
        
        data.gKeyObtained = WorldStateManager.Instance.gKeyObtained;
        data.oKeyObtained = WorldStateManager.Instance.oKeyObtained;

        string json = JsonUtility.ToJson(data, true);
        string path = Application.persistentDataPath + "/save.json";
        File.WriteAllText(path, json);

    }

    public static void LoadGame(GameObject player)
    {
        //if (!PlayerPrefs.HasKey("save")) return;

        string json = PlayerPrefs.GetString("save");
        SaveData data = JsonUtility.FromJson<SaveData>(json);

        string path = Application.persistentDataPath + "/save.json";

        if (File.Exists(path))
        {
             json = File.ReadAllText(path);

            data = JsonUtility.FromJson<SaveData>(json);

            //Debug.Log("Load successful!");
        }
        else
        {
            //Debug.LogWarning("No save file found!");
        }


        // BIOM
        int biomeIndex = data.currentBiomeIndex;
        player.GetComponent<Player_Character>().currentBiomeIndex = biomeIndex;
        BiomStateManagger.Instance.SetCurrentBiom(biomeIndex);
        // MINDIG reseteljük a biomet
        BiomStateManagger.Instance.ResetBiome(biomeIndex);

        // WORLD
        WorldStateManager.Instance.LoadFromData(data.worldState);



        // PLAYER
        player.transform.position = data.player.playerPosition;
        Debug.Log(data.player.playerHP);
        player.GetComponent<Player_Character>().playerCurrentHP = data.player.playerHP;
        player.GetComponent<Player_Character>().UpdateHearts();
        player.GetComponent<Player_Movement>().CameraMain.transform.position = data.player.cameraPosition;
        player.GetComponent<Player_Movement>().CameraMain.orthographicSize = data.player.cameraSize;


        // INVENTORY

        var lights = player.GetComponentsInChildren<Lights_Base>();
        Debug.Log(lights.Length);
        foreach (var light in lights)
        {
            light.Activate(); 
            //Debug.Log("light: " + light);
        }
        foreach (var light in lights)
        {
            light.ClearAllSkills();
        }

        player.GetComponent<SkillInventory>().LoadData(data.inventory);

        SkillManager.Instance.undiscoveredYellow.Clear();

        foreach (var id in data.undiscoveredYellow)
        {
            SkillManager.Instance.undiscoveredYellow.Add(
                SkillManager.Instance.GetSkillByID(id)
            );
        }

        SkillManager.Instance.undiscoveredRed.Clear();

        foreach (var id in data.undiscoveredRed)
        {
            SkillManager.Instance.undiscoveredRed.Add(
                SkillManager.Instance.GetSkillByID(id)
            );
        }

        player.GetComponent<Player_Character>().LightsOFF();

        WorldStateManager.Instance.gKeyObtained = data.gKeyObtained;
        WorldStateManager.Instance.oKeyObtained = data.oKeyObtained;
        player.GetComponent<Player_Character>().ApplyKeyState();

    }

    public static bool HasSave()
    {
        string path = Application.persistentDataPath + "/save.json";
        return System.IO.File.Exists(path);
    }
}
