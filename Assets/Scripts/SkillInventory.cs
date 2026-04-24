using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillInventory : MonoBehaviour
{
    public SkillSlot[] yellowInventorySlots;
    public SkillSlot[] redInventorySlots;



    public bool AddYellowSkill(SkillBase skill)
    {
        foreach (var slot in yellowInventorySlots)
        {
            if (slot.currentSkill == null)
            {
                slot.SetSkill(skill);
                return true;
            }
        }

        return false; // nincs hely
    }
    public bool AddRedSkill(SkillBase skill)
    {
        foreach (var slot in redInventorySlots)
        {
            if (slot.currentSkill == null)
            {
                slot.SetSkill(skill);
                return true;
            }
        }

        return false; // nincs hely
    }

    public InventoryData GetSaveData()
    {
        InventoryData data = new InventoryData();

        foreach (var slot in yellowInventorySlots)
        {
            data.yellowSlots.Add(slot.currentSkill != null ? slot.currentSkill.skillID : "");
            data.yellowActive.Add(slot.isActiveSlot);
        }

        foreach (var slot in redInventorySlots)
        {
            data.redSlots.Add(slot.currentSkill != null ? slot.currentSkill.skillID : "");
            data.redActive.Add(slot.isActiveSlot);
        }
        //Debug.Log("Inventory Save Data: "+ data);
        return data;
    }
    
    public void LoadData(InventoryData data)
    {
        for (int i = 0; i < yellowInventorySlots.Length; i++)
        {
            if (i >= data.yellowSlots.Count)
            {
                yellowInventorySlots[i].SetSkill(null);
                continue;
            }

            string id = data.yellowSlots[i];

            if (!string.IsNullOrEmpty(id))
            {
                SkillBase skill = SkillManager.Instance.GetSkillByID(id);
                yellowInventorySlots[i].SetSkill(skill);

                if (i < data.yellowActive.Count && data.yellowActive[i])
                {
                    yellowInventorySlots[i].targetLight.AddSkill(skill);
                }
            }
            else
            {
                yellowInventorySlots[i].SetSkill(null);
            }
        }


        for (int i = 0; i < redInventorySlots.Length; i++)
        {
            if (i >= data.redSlots.Count)
            {
                redInventorySlots[i].SetSkill(null);
                continue;
            }

            string id = data.redSlots[i];

            if (!string.IsNullOrEmpty(id))
            {
                SkillBase skill = SkillManager.Instance.GetSkillByID(id);
                redInventorySlots[i].SetSkill(skill);

                if (i < data.redActive.Count && data.redActive[i])
                {
                    redInventorySlots[i].targetLight.AddSkill(skill);
                }
            }
            else
            {
                redInventorySlots[i].SetSkill(null);
            }
        }

    }
}