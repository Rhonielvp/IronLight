﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehaviour : MonoBehaviour
{
    //boss states
    private enum State { Off, Idle, FollowPlayer, CloseAttack, RangedAttack, Charging, Rushing, Stunned, Return };
    private State currentState;

    //determine if boss is immune to state changes
    private enum Immune { Yes, No };
    private Immune currentImmune;

    //coroutines
    private Coroutine coroutineMovement = null;
    private Coroutine coroutineTimer = null;

    //locations
    private Vector3[] locations;    
    private Vector3 currentLocation;
    private Vector3 nextLocation;

    private GameObject player;
    private enum Zone { None, A, B, C };
    private Zone playerZone = Zone.None;
    private Zone bossZone = Zone.None;


    [Header("Platform Locations")]    
    [SerializeField] private GameObject locationA;
    [SerializeField] private GameObject locationB;
    [SerializeField] private GameObject locationC;


    //boss attributes
    [Header("Boss Attributes")]
    [SerializeField] private float speedNormal;
    [SerializeField] private float speedRush;
    [SerializeField] private float chargeTime;
    [Range(0.01f,0.99f)][SerializeField] private float followChance;


    //start
    private void Start()
    {
        //set tag
        tag = "Boss";

        //hard coded...
        locations = new Vector3[3];
        locations[0] = locationA.transform.position;
        locations[1] = locationB.transform.position;
        locations[2] = locationC.transform.position;

        //start state
        currentState = State.Off;
        currentImmune = Immune.Yes;        
    }

    //run the states
    private void Update()
    {
        if(currentState == State.FollowPlayer)
        {
            FollowPlayer();
        }    
    }

    private void CloseAttack()
    {

    }

    private void RangedAttack()
    {

    }

    //generic follow player code
    private void FollowPlayer()
    {
        Debug.Log("<color=PURPLE>FOLLOWLING</color>");

        //get player location
        Vector3 movementDirection = (player.transform.position - transform.position).normalized;        

        //move boss towards player
        transform.position += movementDirection * Time.deltaTime * speedNormal;

        //remove y component of vector than rotate to that direction
        movementDirection.y = 0f;
        transform.rotation = Quaternion.LookRotation(movementDirection, Vector3.up);

        //run random to break out of followPlayer
        float temp = Random.Range(0, 1);
        if(temp < followChance)
        {
            currentState = State.Return;
            Return();
        }
    }

    private void Charging()
    {
        Debug.Log("<color=pink>CHARGIN</color>");
        //run charging animation
        //For now
        coroutineTimer = StartCoroutine(ChargeTimer());
        
        //at end of animation call Rushing()???
        //Rushing();
    }

    //simple counter for testing
    IEnumerator ChargeTimer()
    {
        float count = 0.0f;

        while(count < chargeTime)
        {
            yield return null;

            //increment count
            count += Time.deltaTime;
        }

        //call Rushing
        Rushing();

        //set coroutine to null
        coroutineTimer = null;
    }


    //pick another platform and have boss russ to that platform
    private void Rushing()
    {
        Debug.Log("<color=brown>RUSHING</color>");
        //determine if player is in adjacent area, charge at that area
        //get location of different point than point on
        if (bossZone != playerZone)
        {
            //charge to the platform that matches the players zone
            nextLocation = locations[(int)playerZone];
        }
        //player is on the opposite zone so charge to random platform
        else
        {
            do
            {
                //while next location is equal to current, repeat
                int temp = Random.Range(0, locations.Length);
                nextLocation = locations[temp];
            }
            while (nextLocation == currentLocation);
        }              
        
        if(coroutineMovement != null)
        {
            StopCoroutine(coroutineMovement);
        }

        //start movement coroutine
        coroutineMovement = StartCoroutine(Move(currentLocation, nextLocation, speedRush));
    }

    //general movement coroutine, move boss between two locations by a speed passed in
    IEnumerator Move(Vector3 currentLocation, Vector3 nextLocation, float speed)
    {
        float count = 0.0f;
        float distance = (nextLocation - currentLocation).magnitude;

        while(transform.position != nextLocation)
        {
            Debug.Log("<color=blue>MOVING</color>");
            //Debug.Log(count);

            //move closer to next position dependant on count
            transform.position = Vector3.Lerp(currentLocation, nextLocation, count/distance);

            yield return null;

            //increment count
            count += Time.deltaTime * speed;
        }

        //run random chance for states
        float temp = Random.Range(0, 1);
        if(temp < 0.5f)
        {
            currentState = State.FollowPlayer;
        }
        else
        {
            currentState = State.Charging;            
        }
    }

    private void NextState()
    {
        //create random function to determine what the boss does next
    }

    //have boss move to nearest platform
    private void Return()
    {
        Debug.Log("RETURN");
        //check distance to each location
        Vector3 nearestLocation = locations[0];
        float nearestMagnitude = (locations[0] - transform.position).magnitude;

        //iterate through locations list
        for (int i = 1; i < locations.Length; i++)
        {
            //if the magnitude of the vector from location to boss is shorter than
            //the magniture of the current nearest 
            if((locations[i] - transform.position).magnitude < nearestMagnitude)
            {
                //reassign placeholders
                nearestLocation = locations[i];
                nearestMagnitude = (locations[i] - transform.position).magnitude;
            }
        }

        if (coroutineMovement != null)
        {
            StopCoroutine(coroutineMovement);
        }

        Debug.Log("STARTCOROUTINE");
        //once nearest position aquired set destination to that, call coroutine
        coroutineMovement = StartCoroutine(Move(transform.position, nearestLocation, speedNormal));
    }


    //Collisions & Triggers
    private void OnCollisionEnter(Collision collision)
    {
        //if collision with spike, stunned
        if (collision.transform.tag == "Spike")
        {
            currentState = State.Stunned;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //if player gets close do close attack
        if(other.tag == "Player")
        {
            currentState = State.CloseAttack;            
        }
    }


    //will be passed to the boss when the player enters the arena
    public void SetPlayer(GameObject obj)
    {
        Debug.Log("SETPLAYER");

        //assign player
        player = obj;

        //move boss to nearest platform
        Return();
    }


    //get and set boss/player zones 
    public void SetPlayerZone(int set)
    {
        playerZone = (Zone)set;
    }
    public void ResetPlayerZone()
    {
        playerZone = Zone.None;
    }

    public void SetBossZone(int set)
    {
        bossZone = (Zone)set;
    }
    public void ResetBossZone()
    {
        bossZone = Zone.None;
    }
}
