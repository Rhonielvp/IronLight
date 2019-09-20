using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Houses all the player stats and information
//NOT MONOBEHAVIOUR
public class PlayerStats
{
    //health
    private float playerMaxHealth;
    private float playerHealth;    
    

    //for instant damage or healing
    public void AdjustHealth(float change)
    {
        //check if over max health
        if(playerHealth + change > playerMaxHealth)
        {
            playerHealth = playerMaxHealth;            
        }
        //check if dead
        else if(playerHealth + change < 0.0f)
        {
            playerHealth = 0.0f;

            //player dies, do stuff

        }
        //otherwise increment
        else
        {
            playerHealth += change;
        }        
    }
}
