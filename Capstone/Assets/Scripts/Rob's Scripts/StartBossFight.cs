using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartBossFight : MonoBehaviour
{
    [SerializeField] private GameObject boss;
    private BossBehaviour bossScript;

    private void Start()
    {
        //get boss script reference to call shit
        bossScript = boss.GetComponent<BossBehaviour>();
    }
    
    
    //begin boss fight when player enters
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("<color=green>ArenaEntered</color>");

        //set the bosses player reference
        if (other.transform.tag == "Player")
        {
            bossScript.SetPlayer(other.gameObject);
            Debug.Log("<color=red>FIGHT</color>");
        }
    }

    //recieve input from the colliders in this object, set zones of player/boss
    public void SetZone(string person, int zone)
    {
        //check player
        if(person == "Player")
        {            
            if(zone != 0)
            {
                bossScript.SetPlayerZone(zone);
            }
            else
            {
                bossScript.ResetPlayerZone();
            }
            
        }
        else if(person == "Boss")
        {
            if(zone != 0)
            {
                bossScript.SetBossZone(zone);
            }
            else
            {
                bossScript.ResetBossZone();
            }
        }
    }
}
