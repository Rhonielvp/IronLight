using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneBehaviour : MonoBehaviour
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
        //set the bosses player reference
        if (other.tag == "player")
        {
            bossScript.SetPlayer(other.gameObject);
        }
    }
}
