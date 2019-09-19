using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    [SerializeField] private GameObject player;

    [SerializeField] private Vector3 startPosition;
    private Vector3 mostRecentCheckpoint;

    private void Start()
    {
        SetStartPosition();
    }

    //acquire the player start position
    private void SetStartPosition()
    {
        startPosition = player.transform.position;
    }
    
    //returns players most recent checkpoint
    public Vector3 LoadCheckpoint()
    {
        if(mostRecentCheckpoint == null)
        {
            return startPosition;
        }
        {
            return mostRecentCheckpoint;
        }        
    }
}
