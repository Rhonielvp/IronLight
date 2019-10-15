using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//The behaviour of the boss collider boxes
public class BossZoneBehaviour : MonoBehaviour
{
    private StartBossFight parent;

    [SerializeField] private string group;
    [SerializeField] private int zoneNumber;

    private void Start()
    {
        //get access to the parent object to send trigger data
        parent = GetComponentInParent<StartBossFight>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            parent.SetZone("Player", zoneNumber);
        }

        if (other.tag == "Boss")
        {
            parent.SetZone("Boss", zoneNumber);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            parent.SetZone("Player", 0);
        }

        if (other.tag == "Boss")
        {
            parent.SetZone("boss", 0);
        }
    }
}
