using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AttackCharge : MonoBehaviour
{
    [SerializeField] private GameObject PlayerUI;
    private PlayerUI UI;

    //player health script
    PlayerHealthBehaviour playerHealthScript;
    
    [Range(1f, 10f)] [SerializeField] private float chargeSpeed = 1f;
    [Range(0.01f, 0.1f)] [SerializeField] private float focusSpeed = 1f;
    [Range(0f, 0.5f)] [SerializeField] private float focusCap;


    public bool isCharging;

    private float chargePercentage = 0.0f;    
    private float focusPercentage = 0.0f;

    public Coroutine currentCoroutine;


    private void Start()
    {
        //set up UI script
        UI = PlayerUI.GetComponent<PlayerUI>();

        //get health script
        playerHealthScript = GetComponent<PlayerHealthBehaviour>();                
    }

    private void Update()
    {
        //Charging
        //test button start charging
        if (Input.GetKeyDown(KeyCode.Tab) && isCharging == false)
        {
            isCharging = true;
            StartCharge();
            
        }

        //stop coroutine and reset charging
        if (Input.GetKeyUp(KeyCode.Tab))
        {
            isCharging = false;

            //reset UI         
            StopCharge();
            Debug.Log("Reset");                    
        }
    }

    //begin charge
    private void StartCharge()
    {
        //turn on UI
        UI.SetChargeUIOn();

        //start coroutine
        currentCoroutine = StartCoroutine(Charge());
    }

    //stop charge
    private void StopCharge()
    {
        //set percentages to 0
        chargePercentage = 0.0f;
        focusPercentage = 0.0f;

        //turn of UI
        UI.SetChargeUIOff();

        //stop coroutine
        StopCoroutine(currentCoroutine);

        //set coroutine to null to destroy it
        currentCoroutine = null;
    }

    IEnumerator Charge()
    {
        chargePercentage = 0f;
        focusPercentage = 0f;
        //change to maxhealth for alt implementation
        float healthPercentage = playerHealthScript.GetHealthPercentage();

        //create seperate bool to control coroutine because isCharging
        //is used for determining button press
        bool OnCharge = true;
        bool OnFocus = true;

        //button press
        while(isCharging)
        {
            //coroutine math
            while (OnCharge)
            {
                if (chargePercentage < healthPercentage)
                {
                    //adjust UI charge bar
                    UI.AdjustCharge(chargePercentage);

                    //increment count
                    chargePercentage += Time.deltaTime * chargeSpeed;
                }
                else if (chargePercentage > healthPercentage)
                {
                    //set charge to max than set to end coroutine
                    chargePercentage = healthPercentage;
                    UI.AdjustCharge(chargePercentage);
                    OnCharge = false;
                }

                yield return null;
            }


            while(OnFocus)
            {
                //focus can go to same as charge mins focus cap so player doesn't kill themselves
                if(focusPercentage < chargePercentage - focusCap)
                {
                    //adjust UI, 1 because we need to get the inverse of the charge
                    UI.AdjustFocus(1 - focusPercentage);

                    //increment
                    focusPercentage += Time.deltaTime * focusSpeed;
                }
                else if(focusPercentage > chargePercentage - focusCap)
                {
                    focusPercentage = chargePercentage - focusCap;
                    UI.AdjustFocus(1 - focusPercentage);
                    OnFocus = false;
                }

                yield return null;
            }

            yield return null;
        }            
    }    

        
    
    //player took damage so loses charge an resets UI slider
    public void DamageTaken()
    {
        if(currentCoroutine != null)
        {
            //stop coroutine
            StopCoroutine(currentCoroutine);

            currentCoroutine = null;
        }

        //reset UI
        UI.AdjustCharge(0);
    }



    //GETTERS & SETTERS
    public bool GetIsCharging()
    {
        return isCharging;
    }
    
    public float GetChargePercentage()
    {
        return chargePercentage;
    }
    public void SetChargePercentage(float set)
    {
        if(set < 0.0f)
        {
            chargePercentage = 0.0f;
        }
        else if(set > 1.0f)
        {
            chargePercentage = 1.0f;
        }
        else
        {
            chargePercentage = set;
        }        
    }

    public float GetFocusPercentage()
    {
        return focusPercentage;
    }
    public void SetFocusPercentage(float set)
    {
        if (set < 0.0f)
        {
            chargePercentage = 0.0f;
        }
        //if new value is greater than health percentage minus focus cap, than below focus cap
        else if (set > playerHealthScript.currentHealth/playerHealthScript.GetHealthMax() - focusCap)
        {
            chargePercentage = playerHealthScript.currentHealth / playerHealthScript.GetHealthMax() - focusCap;
        }
        else
        {
            chargePercentage = set;
        }
    }
}
