using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthBehaviour : MonoBehaviour
{
    //player health
    [SerializeField] public float currentHealth;
    [SerializeField] private float maxHealth;


    //value that the player will heal every frame when in light source
    [SerializeField] private float lightSourceHealing;


    // Use this for initialization
    void Start()
    {
        currentHealth = maxHealth;
    }


    //used modify health for instant changes
    public void ModifyHealth(float pHealth)
    {
        //change health
        currentHealth += pHealth;

        //clamp health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //check if dead, <= for all cases
        if (currentHealth <= 0)
        {
            Debug.Log("dead");
            SendMessage("OnDeath", "You Die");
        }
    }



    //coroutine for healing the player when in a light source
    IEnumerator LightSourceHeal()
    {
        while (true)
        {
            //only increment if less than full health
            if (currentHealth < maxHealth)
            {
                //increase health
                currentHealth += lightSourceHealing * Time.deltaTime;               
            }
            //else health over max, clamp
            else
            {
                currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
            }
            
            //should go every frame
            yield return null;
        }
    }

    //Get Coroutine
    //start 
    public void StartLightSourceHeal()
    {
        StartCoroutine(LightSourceHeal());
    }

    //stop
    public void StopLightSourceHeal()
    {
        //round current health value up to a normal number
        Mathf.RoundToInt(currentHealth);

        StopCoroutine(LightSourceHeal());
    }
}
