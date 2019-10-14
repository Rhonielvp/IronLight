using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationBehaviour : MonoBehaviour
{
    [SerializeField] private string group;
    [SerializeField] private int zone;
    [SerializeField] public float handleSize = 1f;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Boss")
        {
            other.GetComponent<BossBehaviour>().SetBossZone(zone);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.tag == "Boss")
        {
            other.GetComponent<BossBehaviour>().ResetBossZone();
        }
    }
}
