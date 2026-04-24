using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class YellowStatUpdate : MonoBehaviour
{
    public TextMeshProUGUI explosionText;
    public TextMeshProUGUI doubleText;
    public TextMeshProUGUI bigText;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI fireRateText;
    public TextMeshProUGUI energyCostText;
    public TextMeshProUGUI speedText;

    [SerializeField] private Light_Yellow yellowLight;

    void Start()
    {
        if (yellowLight.explosionEnabled)
        {
            explosionText.enabled = true;
        }
        if (yellowLight.doubleEnabled)
        {
            doubleText.enabled = true;
        }

        if (yellowLight.bigEnabled)
        {
            bigText.enabled = true;
        }

        damageText.text =  ""+yellowLight.baseDamage;
        fireRateText.text = "" + yellowLight.baseFireRate;
        energyCostText.text = "" + yellowLight.baseEnergyCost;
        speedText.text = "" + yellowLight.baseSpeed;
    }

    public void UpdateStats()
    {
        if (yellowLight.explosionEnabled)
        {
            explosionText.enabled = true;
        }
        else
        {
            explosionText.enabled = false;
        }
        if (yellowLight.doubleEnabled)
        {
            doubleText.enabled = true;
        }
        else
        {
            doubleText.enabled = false;
        }   
         if (yellowLight.bigEnabled)
        {
            bigText.enabled = true;
        }
        else
        {
            bigText.enabled = false;
        }

        damageText.text = "" + yellowLight.baseDamage;
        fireRateText.text = "" + yellowLight.baseFireRate;
        energyCostText.text = "" + yellowLight.baseEnergyCost;
        speedText.text = "" + yellowLight.baseSpeed;
    }

}
