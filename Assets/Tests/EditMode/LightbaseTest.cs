using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
public class LightbaseTest 
{
private GameObject obj;
    private TestLight light;
    private TestSkill skill;

    [SetUp]
    public void Setup()
    {
        obj = new GameObject();
        light = obj.AddComponent<TestLight>();

        light.yellowStatUpdate = new GameObject().AddComponent<YellowStatUpdate>();
        light.redStatUpdate = new GameObject().AddComponent<RedStatUpdate>();

        skill = ScriptableObject.CreateInstance<TestSkill>();
        skill.category = SkillCategory.Red_Mode;
        skill.incompatibleCategories = new List<SkillCategory>();
    }


    [Test]
    public void GetDamageReturnsValue()
    {
        light.baseDamage = 10;

        Assert.AreEqual(10, light.GetDamage());
    }


    [Test]
    public void GetFireRateReturnsValue()
    {
        light.baseFireRate = 2.5f;

        Assert.AreEqual(2.5f, light.GetFireRate());
    }

   
    [Test]
    public void CanAddSkillWorks()
    {
        var result = light.CanAddSkill(skill);

        Assert.IsTrue(result);
    }

    
}
public class TestLight : Lights_Base
{
    public override void ApplyEffect(GameObject target, float amount) { }
}