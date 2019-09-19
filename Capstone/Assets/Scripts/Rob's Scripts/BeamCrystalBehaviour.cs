using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeamCrystalBehaviour : MonoBehaviour
{
    //beam should hit trigger of crystal with same tag as parent to activate
    //beams will have to be custom sized
    [SerializeField] private string parentTag;

    // Start is called before the first frame update
    void Start()
    {
        parentTag = transform.parent.tag;
        Debug.Log(parentTag);
        
        //set beam invisible, set visible when facing correct crystal
        GetComponent<MeshRenderer>().enabled = false;
    }

    //when beam enters trigger it beam turns on
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == parentTag)
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }

    //when beam leaves trigger it turns off
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == parentTag)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
    }
}
