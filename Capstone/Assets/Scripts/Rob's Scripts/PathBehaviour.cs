using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathBehaviour : MonoBehaviour
{    
    [SerializeField] private int zone;
    
    //adjust boss information when player moves through
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<BossBehaviour>().SetPlayerZone(zone);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Player")
        {
            other.GetComponent<BossBehaviour>().ResetPlayerZone();
        }
    }
}
