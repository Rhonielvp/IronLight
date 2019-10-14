using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private float health;
    [SerializeField] private float maxHealth;

    public GameObject HealthBarUI;
    public Slider slider;

    [SerializeField] public bool Notify_Other_Enemy;


    //

    Observer _observer;
    delegate void AlertHandler();
    AlertHandler _alertHandler;


    // Use this for initialization
    void Start ()
    {
        health = maxHealth;
        slider.value = CalculateHealth();

        //Observer
        if (Notify_Other_Enemy)
        {
            _observer = new Observer();
            _alertHandler += new AlertHandler(_observer.OnNotify);
        }

    }	
	

    float CalculateHealth()
    {
        return health / maxHealth;
    }

    public void AdjustHealth(int change)
    {
        health += change;

        //keeps health clamped between max and -1 health
        health = Mathf.Clamp(health, -1, maxHealth);

        if (Notify_Other_Enemy) { _alertHandler(); } //Alert Notification to All Enemies.!

        //only run this logic when health changes not in update
        if (health < maxHealth)
        {
            HealthBarUI.SetActive(true);
        }

        if (health <= 0)
        {
            Destroy(gameObject);
        }

        if (health > maxHealth)
        {
            health = maxHealth;
        }

        //adjust slider at the end of all calculations
        slider.value = CalculateHealth();
    }


    public void StartHealing(int pHealth)
    {
        StartCoroutine("HealthOverTime", pHealth);
    }

    public void StopHealing()
    {
        StopCoroutine("HealthOverTime");
    }

    IEnumerator HealthOverTime(int pHealth)
    {
        while (true)
        {
            print("healing");
            health += pHealth;

            health = Mathf.Clamp(health, 0, maxHealth);

            yield return new WaitForSeconds(1);
        }
    }    
}
