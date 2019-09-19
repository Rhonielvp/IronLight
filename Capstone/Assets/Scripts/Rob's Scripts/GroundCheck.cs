using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Rob
//Script to determine collisions for specific objects
//attached to child or seperate object
//reports to other object 
//need seperate object or collider otherwise players can collide sideways and 
//regain jumps or stick to walls
public class GroundCheck : MonoBehaviour
{
    //select the layer that you wish to check collisions with
    public int layerToInteractWith;
    //dev control of collider size
    [SerializeField] private float radiusCollider;

    //list of all observer objects, allow to be public so that
    public List<GameObject> observers = new List<GameObject>();

    private void Start()
    {        
        //auto assign parent of this object to observer list
        //transform.root never returns null, will return self if no hierarchy
        //therefore if what it returns isn't itself, than add to list
        if(transform.root != transform)
        {
            //at the parent/root object to list of observers
            observers.Add(transform.root.gameObject);
        }

        //set collider size
        GetComponent<SphereCollider>().radius = radiusCollider;
    }

    //tell observers about change in collision
    private void Notify(bool set)
    {
        if(observers == null)
        {
            Debug.Log("No observers to report too");
            return;
        }
        else
        {
            //Debug.Log("Notified");

            foreach (GameObject obj in observers)
            {
                //add a try???
                obj.GetComponent<JumpController>().SetGrounded(set);
            }            
        }        
    }

    //when this object collides with set layer
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.layer + " layer touches player");
        if(other.gameObject.layer == layerToInteractWith)
        {
            //report
            Notify(true);
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == layerToInteractWith)
        {
            //report
            Notify(false);
        }
    }

}
