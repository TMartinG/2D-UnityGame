using TMPro;
using UnityEngine;
using NUnit.Framework;
using System.Reflection;
using UnityEngine.UI;
using System.Collections.Generic;


public class SkillSlotTest
{
    private GameObject obj;
    private SkillSlot slot;
    private SkillSlot source;
    private FakeLight light;

    [SetUp]
    public void Setup()
    {
        obj = new GameObject();

        slot = obj.AddComponent<SkillSlot>();
        slot.gameObject.AddComponent<RawImage>();

        var sourceObj = new GameObject();
        source = sourceObj.AddComponent<SkillSlot>();
        sourceObj.AddComponent<RawImage>();

        var lightObj = new GameObject();
        light = lightObj.AddComponent<FakeLight>();

        typeof(SkillSlot)
            .GetField("targetLight", BindingFlags.Public | BindingFlags.Instance)
            .SetValue(slot, light);

        typeof(SkillSlot)
            .GetField("targetLight", BindingFlags.Public | BindingFlags.Instance)
            .SetValue(source, light);

        slot.alap = Texture2D.blackTexture;
        slot.isActiveSlot = true;
        source.isActiveSlot = true;
    }

    [Test]
    public void DefaulTextureWhenNull()
    {
        slot.currentSkill = null;
        slot.alap = Texture2D.blackTexture;

        slot.UpdateUI();

        Assert.AreEqual(Texture2D.blackTexture, slot.GetComponent<RawImage>().texture);
    }

    [Test]
    public void SkillRemovalWorks()
    {
        var skill = ScriptableObject.CreateInstance<TestSkill>();
        slot.currentSkill = skill;

        slot.RemoveSkillFromSlot();

        Assert.IsNull(slot.currentSkill);
    }

    [Test]
    public void DropToWorldDoesNotCrashWhenPrefabMissing()
    {
        var skill = ScriptableObject.CreateInstance<TestSkill>();
        slot.currentSkill = skill;

        MethodInfo drop = typeof(SkillSlot)
            .GetMethod("DropToWorld", BindingFlags.NonPublic | BindingFlags.Instance);

        Assert.DoesNotThrow(() =>
        {
            drop.Invoke(slot, new object[] { skill });
        });
    }
}

public class FakeLight :  Lights_Base
{
    public   bool CanAddSkill(TestSkill s) => true;
    public  void RemoveSkill(TestSkill s) {}
    public override void ApplyEffect(GameObject target, float amount)
    {
        
    }
}
public class TestSkill : SkillBase
{
    public new string skillName;
    public  new string skillID;
    public  new Sprite icon;
    public new  GameObject pickupPrefab;

    public  new SkillCategory category;
    public  new List<SkillCategory> incompatibleCategories;

    public override void Apply(Lights_Base light) { }
    public override void Remove(Lights_Base light) { }
}

