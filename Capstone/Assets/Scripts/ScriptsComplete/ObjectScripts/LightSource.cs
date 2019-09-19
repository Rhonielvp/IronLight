using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Iman

public class LightSource : MonoBehaviour
{
    PlayerHealth Health;

    
    private void OnTriggerEnter(Collider other)
    {   

        if (other.gameObject.CompareTag("Player"))
        {
            //on first enter save object to health variable, might not trigger
            if (Health == null)
            {
                Health = other.GetComponent<PlayerHealth>();
            }

            Health.StartHealing();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Health.StopHealing();
        }
    }
}
