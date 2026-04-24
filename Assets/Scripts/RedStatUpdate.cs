using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RedStatUpdate : MonoBehaviour
{
    public TextMeshProUGUI iceText;
    public TextMeshProUGUI fireText;
    public TextMeshProUGUI movementText;

    public TextMeshProUGUI damageText;
    public TextMeshProUGUI intensityText;
    public TextMeshProUGUI energyCostText;

    [SerializeField] private Light_Red redLight;

    void Start()
    {
        if (redLight.fireEnabled == true)
        {
            fireText.enabled = true;
            if (redLight.upgradedFire == true)
            {
                fireText.color = Color.red;
            }
        }
        if (redLight.iceEnabled == true)
        {
            iceText.enabled = true;
            if (redLight.upgradedIce == true)
            {
                iceText.color = Color.cyan;
            }
        }
        if (redLight.betterMovementEnabled == true)
        {
            movementText.enabled = true;
        }


        damageText.text =  ""+redLight.baseDamage;
        intensityText.text = "" + redLight.baseFireRate;
        energyCostText.text = "" + redLight.baseEnergyCost;
    }

    public void UpdateStats()
    {
        if (redLight.fireEnabled == true)
        {
            fireText.enabled = true;
            if (redLight.upgradedFire == true)
            {
                fireText.color = Color.red;
            }
            else
            {
                fireText.color = Color.white;
            }
        }
        else
        {
            fireText.enabled = false;
        }
        if (redLight.iceEnabled == true)
        {
            iceText.enabled = true;
            if (redLight.upgradedIce == true)
            {
                iceText.color = Color.cyan;
            }
            else
            {
                iceText.color = Color.white;
            }
        }
        else
        {
            iceText.enabled = false;
        }
        if (redLight.betterMovementEnabled == true)
        {
            movementText.enabled = true;
        }
        else
        {
            movementText.enabled = false;
        }

        damageText.text = "" + redLight.baseDamage;
        intensityText.text = "" + redLight.baseFireRate;
        energyCostText.text = "" + redLight.baseEnergyCost;
    }
}
