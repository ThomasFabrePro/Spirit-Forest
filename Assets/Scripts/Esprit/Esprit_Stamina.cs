﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Cyriaque - dans 'public StaminaBar' il faut mettre la stamina bar qui est dans canvas 
// DelayCd et DelayRegene sont la psk si ont tous est juste dans 2 fonctions on peut pas appeler depuis un autre script


public class Esprit_Stamina : MonoBehaviour
{
    private int maxStamina = 100;
    private float currentStamina;
    public StaminaBar StaminaBar;
    private bool cd = false;
    private bool cdRegene = false;

    void Start()
    {
        currentStamina = maxStamina;
        StaminaBar.SetMaxStamina(maxStamina);
    }


    void Update()
    {
        if (!cd && currentStamina < maxStamina && !cdRegene)
        {
            RegenStamina();
        }
        StaminaBar.SetStamina(currentStamina);
    }

    public void ReduceStamina(int amount)
    {
        currentStamina -= amount;
        StaminaBar.SetStamina(currentStamina);
        StartCoroutine(DelayCd(1.5f));
    }

    public void RegenStamina()
    {
        currentStamina += 1.5f;
        StartCoroutine(DelayRegene(0.05f));
    }


    IEnumerator DelayRegene(float time)
    {
        cdRegene = true;
        yield return new WaitForSeconds(time);
        cdRegene = false;
    }

    IEnumerator DelayCd(float time)
    {
        cd = true;
        yield return new WaitForSeconds(time);
        cd = false;
    }



}
