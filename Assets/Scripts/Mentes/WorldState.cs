using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public WorldState worldState;
    public InventoryData inventory;
    public Player player;
    public List<string> undiscoveredYellow;
    public List<string> undiscoveredRed;
    public int currentBiomeIndex;
    
    public bool gKeyObtained;
    public bool oKeyObtained;

}

[System.Serializable]
public class WorldState
{
    public List<string> deadIDs = new List<string>();
    public List<HealthState> healthStates = new List<HealthState>();
}

[System.Serializable]
public class HealthState
{
    public string id;
    public short value;
}


[System.Serializable]
public class Player
{
    public Vector3 playerPosition;
    public int playerHP;

    public Vector3 cameraPosition;
    public float cameraSize;

}

[System.Serializable]
public class InventoryData
{
    public List<string> yellowSlots = new List<string>();
    public List<string> redSlots = new List<string>();

    public List<bool> yellowActive = new List<bool>();
    public List<bool> redActive = new List<bool>();

}
