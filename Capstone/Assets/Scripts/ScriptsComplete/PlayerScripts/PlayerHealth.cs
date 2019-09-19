using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    Animator anim;
    AudioSource playerAudio;

    [SerializeField] private int maxHealth;
    [SerializeField] private int currentHealth; 
    [SerializeField] private int lightSourceHealing = 1;

    // Reference to the UI's health bar
    public Slider healthSlider;                                 
    
    //public Image damageImage;                       
    public AudioClip deathClip;                                 
    public float flashSpeed = 5f;                              
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);   

                                                                              
    bool isDead;                                                
    bool isDamaged;                                               


    // Use this for initialization
    void Start()
    {
        // Set the initial health of the player.
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        //UI 
        healthSlider.value = currentHealth;

        // If the player has just been damaged...
        if (isDamaged)
        {
            // ... set the colour of the damageImage to the flash colour.
            //damageImage.color = flashColour;
        }
        // Otherwise...
        else
        {
            // ... transition the colour back to clear.
            //damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        // Reset the damaged flag.
        isDamaged = false;
    }

    public void TakeDamage(int amount)
    {
        // Set the damaged flag so the screen will flash.
        isDamaged = true;

        // Reduce the current health by the damage amount.
        currentHealth -= amount;

        // Set the health bar's value to the current health.
       

        // Play the hurt sound effect.
        //playerAudio.Play();

        // If the player has lost all it's health and the death flag hasn't been set yet...
        if (currentHealth <= 0 && !isDead)
        {
            // ... it should die.
            Death();
        }
    }

    void Death()
    {
        // Set the death flag so this function won't be called again.
        isDead = true;

        // Turn off any remaining shooting effects.
        // playerShooting.DisableEffects();

        // Tell the animator that the player is dead.
        anim.SetTrigger("Die");

        // Set the audiosource to play the death clip and play it (this will stop the hurt sound from playing).
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // Turn off the movement and shooting scripts.
        // playerMovement.enabled = false;
        //  playerShooting.enabled = false;
    }


    //should be put on projectile script for more efficency, this checks all triggers a lot of useless checks
    void OnTriggerEnter(Collider col)
    {
        //all projectile colliding game objects should be tagged "Enemy" or whatever in inspector but that tag must be reflected in the below if conditional
        if (col.gameObject.tag == "Projectile")
        {
            Destroy(gameObject);
            //add an explosion or something
            //destroy the projectile that just caused the trigger collision
            currentHealth = currentHealth - 20;
        }
    }

    public void StartHealing()
    {
        StartCoroutine("healthOverTime");
    }

    public void StopHealing()
    {
        StopCoroutine("healthOverTime");
    }

    IEnumerator healthOverTime()
    {
        while (true)
        {
            print("healing");
            currentHealth += lightSourceHealing;

            currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

            if (currentHealth == 0)
            {
                Debug.Log("dead");
                SendMessage("OnDeath", "You Die");
            }

            yield return new WaitForSeconds(1);
        }
    }    
}

