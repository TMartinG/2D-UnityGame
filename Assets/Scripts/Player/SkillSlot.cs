using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler, IPointerClickHandler
{
    public SkillBase currentSkill;
    public Sprite icon = null;

    public bool isActiveSlot;
    public Lights_Base targetLight;
    //private Canvas canvas;
    private CanvasGroup canvasGroup;
    public Texture alap;

    void Awake()
    {
        //canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        
    }

    void Start()
    {
        UpdateUI();
    }

    public void SetSkill(SkillBase skill)
    {
        currentSkill = skill;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (currentSkill != null)
        {
           // Debug.Log("Ikon beállítása ennek:  " + currentSkill.skillName);
           // Debug.Log("Ikon: " + currentSkill.icon);
            icon = currentSkill.icon;
            GetComponent<RawImage>().texture = icon.texture;
            //icon.sprite = currentSkill.icon;
            //icon.color = Color.white;
        }
        else
        {
            //icon.color = new Color(1, 1, 1, 0.2f); // halvány üres kör
            GetComponent<RawImage>().texture = alap;
        }
    }

    //  DRAG BEGIN
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentSkill == null) return;

        canvasGroup.blocksRaycasts = false;
    }

    //  ON DRAG 
    public void OnDrag(PointerEventData eventData) { }

    //  DRAG END
    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
    }

    //  DROP
    public void OnDrop(PointerEventData eventData)
    {
        SkillSlot source = eventData.pointerDrag.GetComponent<SkillSlot>();

        if (source == null || source.currentSkill == null)
            return;

        SwapSkills(source);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            RemoveSkillFromSlot();
        }
    }

    void SwapSkills(SkillSlot source)
    {
        SkillBase sourceSkill = source.currentSkill;
        SkillBase targetSkill = currentSkill;

        //  Aktív helyek csekkolása
        if (isActiveSlot)
        {
            Debug.Log(targetLight.CanAddSkill(sourceSkill));
            if (sourceSkill != null && !targetLight.CanAddSkill(sourceSkill))
            {
                Debug.Log("Nem kompatibilis!");
                return;
            }
        }

        // Régi skillek törlése
        if (isActiveSlot && currentSkill != null)
        {
            targetLight.RemoveSkill(currentSkill);
            targetLight.activeSkills.Remove(currentSkill);
        }

        if (source.isActiveSlot && source.currentSkill != null)
        {
            source.targetLight.RemoveSkill(source.currentSkill);
            source.targetLight.activeSkills.Remove(source.currentSkill);
        }

        //  Csere
        currentSkill = sourceSkill;
        source.currentSkill = targetSkill;

        // Új skillek felszerelése
        if (isActiveSlot && currentSkill != null)
        {
            targetLight.AddSkill(currentSkill);
            targetLight.activeSkills.Add(currentSkill);
        }

        if (source.isActiveSlot && source.currentSkill != null)
        {
            source.targetLight.AddSkill(source.currentSkill);
            source.targetLight.activeSkills.Add(source.currentSkill);

        }

        UpdateUI();
        source.UpdateUI();
    }

    public void RemoveSkillFromSlot()
    {
        if (currentSkill == null) return;

        // ha aktív slot -> remove effect
        if (isActiveSlot)
        {
            targetLight.RemoveSkill(currentSkill);
            targetLight.activeSkills.Remove(currentSkill);
        }

        // spawn a világba
        DropToWorld(currentSkill);

        currentSkill = null;
        UpdateUI();
    }

    void DropToWorld(SkillBase skill)
    {
        if (skill.pickupPrefab == null)
        {
            //Debug.LogWarning("Nincs pickup prefab!");
            return;
        }

        Vector3 dropPos = GetDropPosition();

        GameObject obj = Instantiate(skill.pickupPrefab, dropPos, Quaternion.identity);

        SkillPickUp pickup = obj.GetComponent<SkillPickUp>();
        pickup.skill = skill;

    }

    Vector3 GetDropPosition()
    {
        Transform player = GameObject.FindGameObjectWithTag("Player").transform;

        Vector2 offset = new Vector2(5f, 5f);

        return player.position + (Vector3)offset;
    }
}
