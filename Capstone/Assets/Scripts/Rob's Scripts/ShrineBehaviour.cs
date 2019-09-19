using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShrineBehaviour : MonoBehaviour
{
    //tag for this shrine
    public string triggerGroup;
    public string crystalGroup;

    //determine if shrine has been activated
    [SerializeField] private bool isActivated = false;

    //store all objects to be triggered by this shrine
    [SerializeField] private List<GameObject> triggeredObjects = new List<GameObject>();



    //trigger all objects in list
    public void TriggerShrine()
    {
        //if false than true will ensure only 1 activation, no going back
        if (!isActivated)
        {
            isActivated = true;

            //activate all objects...that can be
            foreach (GameObject obj in triggeredObjects)
            {
                obj.GetComponent<MoveObject>().Activate();
            }
        }
    }

    //let outside things get added to trigger list, puzzle setup
    public void AddToTriggerList(GameObject obj)
    {
        triggeredObjects.Add(obj);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Beam")
        {
            Debug.Log("Beam triggered shrine");
            TriggerShrine();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Beam")
        {
            Debug.Log("Beam collided with shrine");
            TriggerShrine();
        }
    }


    //******************
    //old method for gathering all objects that this triggers
    private void FindObjectsToTrigger()
    {
        //grab all objects triggered by this shrine
        GameObject[] temp = GameObject.FindGameObjectsWithTag(triggerGroup);

        //transfer objects to list
        foreach (GameObject obj in temp)
        {
            if (obj == null)
            {
                break;
            }
            else
            {
                triggeredObjects.Add(obj);
            }
        }
    }
}
