using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public abstract class Lights_Base : MonoBehaviour
{

    [Header("Base stats")]
    [SerializeField] protected int range = 10;
    [SerializeField] protected float lifeTime = 2f;
    public int baseDamage = 2;
    public int baseEnergyCost = 1;
    public float baseFireRate = 1;
    public float baseSpeed = 20f;

    public bool explosionEnabled = false;
    public bool doubleEnabled = false;
    public bool bigEnabled = false;


    public float explosionMultiplier = 0.5f;
    public float explosionMaxRadius = 0.77f;

    public bool fireEnabled = false;
    public bool iceEnabled = false;
    public bool upgradedFire = false;
    public bool upgradedIce = false;
    public bool betterMovementEnabled = false;


    public short fireDamage = 2;
    public float fireDuration = 5f;
    public float freezeDuration = 3f;
    public float slowLerpSpeed = 2f;
    public float freezeThreshold = 5f;

    protected Player_Character playerCharacter;
    public Player_Movement playerMovement;
    public Light2D light2DFlash;
    public Light2D light2DHead;

    public List<SkillBase> activeSkills = new List<SkillBase>();
    public Dictionary<SkillBase, int> skillStacks = new Dictionary<SkillBase, int>();

    public YellowStatUpdate yellowStatUpdate;
    public RedStatUpdate redStatUpdate;


    LightType currentType;
    Dictionary<LightType, Dictionary<SkillBase, int>> skillStacksByType =
        new Dictionary<LightType, Dictionary<SkillBase, int>>();

    public int WichLaser()
    {
        if (iceEnabled)
        {
            light2DFlash.color = Color.cyan;
            light2DHead.color = Color.cyan;
            if (upgradedIce == true)
            {
                return 3;
            }
            return 1;
        }
        else if (upgradedFire == true)
        {
            light2DFlash.color = Color.red;
            light2DHead.color = Color.red;
            return 2;
        }
        else
        {
            light2DFlash.color = Color.red;
            light2DHead.color = Color.red;
            return 0;
        }

        
    }


    public int GetDamage()
    {
        return baseDamage;
    }

    public float GetFireRate()
    {
        return baseFireRate;
    }
    public float GetSpeed()
    {
        return baseSpeed;
    }

    protected virtual void Awake()
    {
        light2DFlash = GetComponent<Light2D>();
        light2DHead = transform.GetChild(0).GetComponent<Light2D>();

        playerCharacter = GetComponentInParent<Player_Character>();

        skillStacksByType[LightType.Yellow] = new Dictionary<SkillBase, int>();
        skillStacksByType[LightType.Red] = new Dictionary<SkillBase, int>();
    }

    public virtual void Activate()
    {
        gameObject.SetActive(true);

    }

    public virtual void Deactivate()
    {
        gameObject.SetActive(false);

        // Alap értékek visszaállítása
        if (light2DFlash != null)
        {
            light2DFlash.pointLightOuterAngle = 130f;
            light2DFlash.pointLightInnerAngle = 80f;
        }
    }

    // Hatás az ellenfélre
    public abstract void ApplyEffect(GameObject target, float amount);

    public void AddSkill(SkillBase skill)
    {
        if (!skillStacks.ContainsKey(skill))
            skillStacks[skill] = 0;

        skillStacks[skill]++;
        skill.Apply(this);

        yellowStatUpdate.UpdateStats();
        redStatUpdate.UpdateStats();
    }

    public void RemoveSkill(SkillBase skill)
    {
        if (!skillStacks.ContainsKey(skill)) return;

        skillStacks[skill]--;
        skill.Remove(this);

        if (skillStacks[skill] <= 0)
            skillStacks.Remove(skill);

        yellowStatUpdate.UpdateStats();
        redStatUpdate.UpdateStats();
    }


    public bool CanAddSkill(SkillBase newSkill)
    {
        foreach (var skill in activeSkills)
        {
            // ha az új skill tiltja a régit
            if (newSkill.incompatibleCategories.Contains(skill.category))
                return false;

            // ha a régi skill tiltja az újat
            if (skill.incompatibleCategories.Contains(newSkill.category))
                return false;
        }

        return true;
    }
    public void RebuildSkills(List<SkillBase> skills)
    {
        ClearAllSkills();

        foreach (var skill in skills)
        {
            AddSkill(skill);
        }
    }
    public void ClearAllSkills()
    {
        foreach (var kvp in skillStacks)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                kvp.Key.Remove(this);
            }
        }

        skillStacks.Clear();

        yellowStatUpdate.UpdateStats();
        redStatUpdate.UpdateStats();
    }

    public List<SkillBase> GetSkills()
    {
        List<SkillBase> result = new List<SkillBase>();

        foreach (var kvp in skillStacks)
        {
            for (int i = 0; i < kvp.Value; i++)
            {
                result.Add(kvp.Key);
            }
        }

        return result;
    }
}
