using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalRaycast : MonoBehaviour
{
    [SerializeField] private float range = 100.0f;

    [SerializeField] private bool activate = false;

    
    public void Shoot()
    {
        //shoot the ray cast from this object, or slightly in front of it...
        RaycastHit hit;
        
        if(Physics.Raycast(transform.position, transform.forward, out hit, range))
        {
            //if the raycast hits another crystal it should have the same tag
            if (hit.collider.tag == this.gameObject.tag)
            {
                hit.collider.GetComponent<CrystalRaycast>().Shoot();
            }            
        }

        Debug.DrawRay(transform.position, transform.forward* 100, Color.green);

        Debug.Log("casting");
    }
}
