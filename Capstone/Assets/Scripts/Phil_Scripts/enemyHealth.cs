using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class enemyHealth : MonoBehaviour
{
    [SerializeField] public float health;
    [SerializeField] public float maxHealth;
    [SerializeField] private float bulletDamage;

    [SerializeField] public bool Notify_Other_Enemy;

  //  public PlayerStats player_Stats;


   
    Observer _observer;
    delegate void AlertHandler();
    AlertHandler _alertHandler;

    public GameObject HealthBarUI;
    public Slider slider;
    // Use this for initialization
    protected bool dead;


    void Start () {
        health = maxHealth;
        slider.value = CalculateHealth();

        //Observer
        if (Notify_Other_Enemy)
        {
            _observer = new Observer();
            _alertHandler += new AlertHandler(_observer.OnNotify);
        }
        PlayerStats player_Stats = GetComponent<PlayerStats>();
    }

   
    public void UpdateTotal()
    {

      //  player_Stats.Display_CashStats(20);

        GameObject.Destroy(gameObject, 2f);
    }
    // Update is called once per frame
    void Update () {
        slider.value = CalculateHealth();
        if(health < maxHealth)
        {
            HealthBarUI.SetActive(true);
        }

     

        if(health > maxHealth)
        {
            health = maxHealth;
        }
	}
    float CalculateHealth()
    {
        return health / maxHealth;
    }

    //public void ApplyDamage(float damage)
    //{
    //    health -= bulletDamage;
    //    Debug.Log("Alert Notification to All Enemies.!");

    //    _alertHandler();

    //}

    public virtual void TakeHit(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        // Do some stuff here with hit var
        ApplyDamage(damage);
    }

    public virtual void ApplyDamage(float damage)
    {
        health -= damage;

        if (Notify_Other_Enemy) { _alertHandler(); } //Alert Notification to All Enemies.!

        if (health <= 0 && !dead)
        {
          
            Die();
        }
    }

    [ContextMenu("Self Destruct")]
    public virtual void Die()
    {
        UpdateTotal();
        dead = true;
       
    }


}
