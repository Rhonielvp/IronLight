using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Script to control all interaction indicators for the player???
public class PlayerIndicator : MonoBehaviour
{
    //generic offset
    private Vector3 offset = Vector3.up;

    //types of indicators
    [SerializeField] private GameObject crystalIndicator;    


    // Start is called before the first frame update
    void Start()
    {
        crystalIndicator = GameObject.Find("CrystalIndicator");
        crystalIndicator.SetActive(false);
    }

    
    //pass in object that is calling this method plus whether true/false for activation
    public void ActivateIndicator(GameObject callingObject, bool state)
    {
        //crystal indicator logic
        if(callingObject.tag == "Crystal")
        {
            crystalIndicator.SetActive(state);

            if(state)
            {
                crystalIndicator.transform.position = callingObject.gameObject.transform.position + offset;
            }
        }
    }
}
