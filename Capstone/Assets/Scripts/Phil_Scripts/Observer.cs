using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class Observer : MonoBehaviour {


    GameObject _observer;
    
    private Transform target;
   
    // Use this for initialization
    public void OnNotify()
    {
        target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;
     
        DoMove();
    }
  

    void DoMove()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(Tags.ENEMY_TAG);

        for (int i = 0; i < enemies.Length; i++)
        {
           
            enemies[i].GetComponent<NavMeshAgent>().speed = 7f;
            target = GameObject.FindWithTag(Tags.PLAYER_TAG).transform;

            enemies[i].GetComponent<NavMeshAgent>().SetDestination(target.position);
        

            Debug.Log("Enemy " + i + " the message has been Received!");
        }

        //_observer1.GetComponent<NavMeshAgent>().speed = 4f;

       // _observer1.GetComponent<NavMeshAgent>().SetDestination(target.position);
        //_observer.gameObject.enemy_State == EnemyState.CHASE;

       
    }
}
