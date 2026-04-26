using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillPickUp : MonoBehaviour
{
    public SkillBase skill;

    void Start()
    {
        if (!(skill.name == "R_FIRE" || skill.name == "R_FIRE_UPG" || skill.name == "R_ICE" || skill.name == "R_ICE_UPG" || skill.name == "Y_BIG" || skill.name == "Y_DOUBLE"
             || skill.name == "Y_RAPID" || skill.name == "Y_EXPLOSION"))
        {
            if (WorldStateManager.Instance.IsDead(skill.skillID))
            {
                Destroy(gameObject);
                return;
            }
        }
        
        WorldStateManager.Instance.RegisterDead(skill.skillID);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            SkillInventory inventory = other.GetComponent<SkillInventory>();

            if (inventory != null)
            {
                LightsOn(10, other);
                if (isYellow() == true)
                {
                    bool success = inventory.AddYellowSkill(skill);

                    if (!success)
                    {
                        Debug.Log("Inventory tele!");
                        return;
                    }
                }
                else
                {
                    bool success = inventory.AddRedSkill(skill);
                    
                    if (!success)
                    {
                        Debug.Log("Inventory tele!");
                        return;
                    }
                }
            }

            
            Destroy(gameObject);
        }
    }

    public bool isYellow()
    {
        if (skill.category == SkillCategory.Yellow_Modifier || skill.category == SkillCategory.Yellow_Mode)
        {
            return true;
        }
        return false;
    }

    IEnumerator LightsOn(float delay, Collider2D other)
    {
        Light_Manager lightM = other.GetComponentInChildren<Light_Manager>();
        lightM.lightDict[LightType.Red].Activate();
        lightM.lightDict[LightType.Yellow].Activate();
        yield return new WaitForSeconds(delay);

        if (lightM.activeType == LightType.Yellow)
        {
            lightM.lightDict[LightType.Red].Deactivate();
        }
        else if (lightM.activeType == LightType.Red)
        {
            lightM.lightDict[LightType.Yellow].Deactivate();
        }
        else
        {
            lightM.lightDict[LightType.Red].Deactivate();
            lightM.lightDict[LightType.Yellow].Deactivate();
        }


        
    }

}
