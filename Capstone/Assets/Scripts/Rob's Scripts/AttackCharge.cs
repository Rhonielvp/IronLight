using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackCharge : MonoBehaviour
{
    [SerializeField] private GameObject ChargeUI;
    private Slider sliderCharge;

    //player health script
    PlayerHealthBehaviour playerHealthScript;

    [Range(1f, 10f)] [SerializeField] public float chargeSpeed = 1f;
    //[Range(1f, 10f)] [SerializeField] public float chargeDamage = 1f;

    public bool isCharging;

    public Coroutine currentCoroutine;

    private float count = 0.0f;
    private float value = 0.0f;


    private void Start()
    {
        sliderCharge = ChargeUI.GetComponent<Slider>();

        //get health script
        playerHealthScript = GetComponent<PlayerHealthBehaviour>();

        //slider shit
        sliderCharge.image.color = Color.yellow;
        sliderCharge.value = 0;
    }

    private void Update()
    {
        //Charging
        //test button start charging
        if (Input.GetKeyDown(KeyCode.Tab) && isCharging == false)
        {
            isCharging = true;

            currentCoroutine = StartCoroutine(Charging());
        }

        //stop coroutine and reset charging
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isCharging = false;
        }
    }

    IEnumerator Charging()
    {
        count = 0.01f;
        //change to maxhealth for alt implementation
        float currentHealth = playerHealthScript.GetCurrentHealth();

        while(isCharging)
        {
            if(count < currentHealth)
            {
                //set slider value
                sliderCharge.value = count / currentHealth;                

                //increment count
                count += Time.deltaTime * chargeSpeed;
            }
            else if(count > currentHealth)
            {
                count = currentHealth;
            }
            else
            {
                //have the charge bar do some glowing shit

            }

            yield return null;
        }

        //reset slider
        sliderCharge.value = 0.0f;
           
    }

    //IEnumerator ChargingAlt()
    //{
    //    count = 0.0f;
    //    value = 0.0f;

    //    while(isCharging)
    //    {
    //        //set value equal to the sin of count, will be between 0 and 1            
    //        value = (Mathf.Cos(count) * -1f + 1f) / 2f;
    //        Debug.Log("<color=red>Count: </color>" + count);

    //        value *= chargeDamage;
    //        Debug.Log("<color=blue>Value: </color>" + value);

    //        playerHealthBehaviourScript.ChargingUI(value);

    //        yield return null;

    //        //increment time
    //        count += Time.deltaTime * chargeSpeed;
    //    }

    //    //reset value to 0 for next charge
    //    playerHealthBehaviourScript.ChargingUI(0.0f);

    //    //set coroutine to null
    //    currentCoroutine = null;
    //}

    //for attacks to access the charget to determine multiplier strength
    public float GetCharge()
    {
        isCharging = false;

        return count;
    }

    //player took damage so loses charge an resets UI slider
    public void DamageTaken()
    {
        sliderCharge.value = 0.0f;
    }
}
