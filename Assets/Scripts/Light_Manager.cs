using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Light_Manager : MonoBehaviour
{

    public LightType activeType;
    public List<Lights_Base> lights;
    public Dictionary<LightType, Lights_Base> lightDict;
    public Lights_Base activeLight;

    [SerializeField] GameObject yellowEnergyOrb;
    [SerializeField] GameObject redEnergyOrb;

    void Awake()
    {
        lightDict = new Dictionary<LightType, Lights_Base>
        {
            { LightType.Yellow, lights[0] },
            { LightType.Red, lights[1] },
        };
    }

    public void SetActiveLight(LightType type)
    {       
           //  ha ugyanaz a fény van aktívan → kikapcsol
        if (activeType == type)
        {
            activeLight.Deactivate();
            activeLight = null;
            activeType = LightType.Nothing;
            return;
        }

        //  ha volt másik aktív → azt kikapcsoljuk
        if (activeLight != null)
        {
            activeLight.Deactivate();
        }

        //  új fény bekapcsolása
        activeLight = lightDict[type];
        activeLight.Activate();
        activeType = type;

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetActiveLight(LightType.Yellow);
            redEnergyOrb.SetActive(false);
            yellowEnergyOrb.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetActiveLight(LightType.Red);
            yellowEnergyOrb.SetActive(false);
            redEnergyOrb.SetActive(true);
        }

    }
}

public enum LightType { Nothing, Yellow, Red }
