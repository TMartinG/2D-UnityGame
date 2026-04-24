using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldStateManager : MonoBehaviour
{
    public static WorldStateManager Instance;

    public HashSet<string> deadIDs = new HashSet<string>();
    public Dictionary<string, short> healthStates = new Dictionary<string, short>();
    public bool gKeyObtained;
    public bool oKeyObtained;


    void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void RegisterDead(string id)
    {
        deadIDs.Add(id);
    }

    public bool IsDead(string id)
    {
        return deadIDs.Contains(id);
    }

    public void SetHealth(string id, short value)
    {
        healthStates[id] = value;
    }

    public short GetHealth(string id, short defaultValue)
    {
        if (healthStates.ContainsKey(id))
            return healthStates[id];

        return defaultValue;
    }


    public WorldState GetSaveData()
    {
        WorldState data = new WorldState();

        data.deadIDs = new List<string>(deadIDs);

        foreach (var kvp in healthStates)
        {
            data.healthStates.Add(new HealthState
            {
                id = kvp.Key,
                value = kvp.Value
            });
        }

        return data;
    }

    public void LoadFromData(WorldState data)
    {
        deadIDs = new HashSet<string>(data.deadIDs);

        healthStates.Clear();
        foreach (var hs in data.healthStates)
        {
            healthStates[hs.id] = hs.value;
        }
    }
}
