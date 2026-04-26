using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager Instance;

    private Dictionary<string, SkillBase> skillLookup = new Dictionary<string, SkillBase>();

    public List<SkillBase> yellowModifiers;
    public List<SkillBase> redModifiers;

    public List<SkillBase> modes;
    public SkillModes skillModes;

    public List<SkillBase> undiscoveredYellow = new List<SkillBase>();
    public List<SkillBase> undiscoveredRed = new List<SkillBase>();

    public int maxYellowDrops = 14;
    public int maxRedDrops = 14;

    private void Awake()
    {
        Instance = this;

        undiscoveredYellow = new List<SkillBase>(yellowModifiers);
        undiscoveredRed = new List<SkillBase>(redModifiers);

        //  Betöltünk mindent!
        foreach (var skill in yellowModifiers)
        {
            skillLookup[skill.skillID] = skill;
        }

        foreach (var skill in redModifiers)
        {
            skillLookup[skill.skillID] = skill;
        }

        foreach (var skill in modes)
        {
            skillLookup[skill.skillID] = skill;
        }

    }

    public SkillBase GetSkillByID(string id)
    {
        if (skillLookup.ContainsKey(id))
            return skillLookup[id];

        //Debug.LogError("Skill nemm található: " + id);
        return null;
    }

    public GameObject GetSkillGameObjectByID(string id)
    {
        if (skillLookup.ContainsKey(id))
            return skillLookup[id].pickupPrefab;

        //Debug.LogError("Skill nemm található: " + id);
        return null;
    }

    public SkillBase GetRandomDrop(bool isYellow)
    {
        var undiscovered = isYellow ? undiscoveredYellow : undiscoveredRed;
        var all = isYellow ? yellowModifiers : redModifiers;

        //  Először az újjakat, ha még van felfedezetlen
        if (undiscovered.Count > 0)
        {
            int index = Random.Range(0, undiscovered.Count);
            SkillBase skill = undiscovered[index];
            undiscovered.RemoveAt(index);
            return skill;
        }

        // Ha már mind megvolt
        int randomIndex = Random.Range(0, all.Count);
        return all[randomIndex];
    }
}
