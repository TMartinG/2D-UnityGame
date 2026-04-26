using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SkillBase : ScriptableObject
{
    public string skillName;
    public string skillID;
    public Sprite icon;
    public GameObject pickupPrefab;

    public SkillCategory category;

    // Mely kategóriákat tiltja
    public List<SkillCategory> incompatibleCategories;

    

    public virtual void Apply(Lights_Base light) { }
    public virtual void Remove(Lights_Base light) { }
}
public enum SkillCategory
{
    Yellow_Modifier,
    Yellow_Mode,   
    Red_Modifier,
    Red_Mode       
}