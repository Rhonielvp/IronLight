using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBehaviour : MonoBehaviour
{
    //player UI
    [SerializeField] private GameObject playerUI;
    private PlayerUI UI;    

    //player health
    [SerializeField] public float currentHealth;
    [SerializeField] private float maxHealth;
    private float healthPercentage;

    private float chargeHealth;


    //value that the player will heal every frame when in light source
    [Range(1f, 100f)][SerializeField] private float lightSourceHealing;
    [Range(1f,100f)][SerializeField] private float beamAttackDraining;
    [SerializeField] private float lowestDrainPoint;

    //keep track of coroutines for correct performance
    Coroutine lightHealingCoroutine = null;
    Coroutine beamDrainCoroutine = null;


    // Use this for initialization
    void Start()
    {
        //NEW UI
        UI = playerUI.GetComponent<PlayerUI>();
        
        //set health
        currentHealth = maxHealth;
        UpdatePlayerHealthUI(currentHealth);        
    }


    //used modify health for instant changes
    //**disrupt charging of attacks???
    public void ModifyHealth(float set)
    {
        //change health
        currentHealth += set;

        //clamp health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        //adjust UI
        UpdatePlayerHealthUI(currentHealth);

        //check if dead, <= for all cases
        if (currentHealth <= 0)
        {
            Debug.Log("dead");
            SendMessage("OnDeath", "You Die");
        }
    }

    //not using but could try...
    IEnumerator BeamAttackDrain()
    {
        while(true)
        {
            if(currentHealth > lowestDrainPoint)
            {
                //lower health by beam rate
                currentHealth -= beamAttackDraining * Time.deltaTime;

                //update UI
                UpdatePlayerHealthUI(currentHealth);
            }
            //player at lowest drain point
            else
            {
                currentHealth = lowestDrainPoint;

                //set beam attack to unable to do...


                //stop this coroutine
                StopCoroutine(BeamAttackDrain());
            }           

            yield return null;
        }
    }

    public void StartBeamAttackDrain()
    {
        //Debug.Log("START beam attack drain");
        beamDrainCoroutine =  StartCoroutine(BeamAttackDrain());
    }

    public void StopBeamAttackDrain()
    {
        //Debug.Log("STOP beam attack drain");
        StopCoroutine(beamDrainCoroutine);
        //StopAllCoroutines();
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

                //update UI
                UpdatePlayerHealthUI(currentHealth);
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
        Debug.Log("START light healing");

        lightHealingCoroutine = StartCoroutine(LightSourceHeal());
    }

    //stop
    public void StopLightSourceHeal()
    {
        Debug.Log("STOP light healing");
        if(lightHealingCoroutine != null)
        {
            StopCoroutine(lightHealingCoroutine);
        }        

        //round current health value up to a normal number
        Mathf.RoundToInt(currentHealth);

        //update UI
        UpdatePlayerHealthUI(currentHealth);        
    }


    //Updates the UI scroll bar for health
    private void UpdatePlayerHealthUI(float newCurrentHealth)
    {
        //new UI change
        UI.AdjustHealth(newCurrentHealth / maxHealth);
    }

    //access for outside methods
    //script name to ensure that only certain scripts can access it
    public void ChargingUI(float set)
    {
        //set the temp health so to ensure we still have normal health
        //if damage gets taken and we can calculate that
        chargeHealth = currentHealth - set;

        //set UI equal to chargeHealth
        UpdatePlayerHealthUI(chargeHealth);
    }

    //call when an attack is done charging and gets released
    public void ChargeChange()
    {
        //set current health the charge health
        currentHealth = chargeHealth;

        //update UI
        UpdatePlayerHealthUI(currentHealth);
    }


    //GETTERS & SETTERS
    public float GetHealthCurrent()
    {
        return currentHealth;
    }    

    public float GetHealthMax()
    {
        return maxHealth;
    }

    public float GetHealthPercentage()
    {
        return currentHealth / maxHealth;
    }
}
