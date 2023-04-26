using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Health
    [Header("Health")]
    public int curHealth;
    public int maxHealth = 10;

    //Damage
    private bool canBeHurt = true;
    private float hitTimer = 0;

    //Respawn
    public bool alive { get; set; }
    private float respawnTimer = 0;

    //Fx
    [Header("FX")]
    public GameObject deathEffect;

    //Sound
    [Header("Sound")]
    public AudioSource src;
    public AudioClip die;
    public AudioClip hurt;

    public static PlayerHealth Instance;

    private void Awake()
    {
        Instance = this;
        //Get max health based on difficulty
        maxHealth = PlayerPrefs.GetInt("PlayerHealth", 10);
        curHealth = maxHealth;
    }


    // Start is called before the first frame update
    void Start()
    {
        alive = true;
        UIController.Instance.UpdateBars(curHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //If player cannot be hurt and is alive, don't countdown
        if (canBeHurt == false && alive == true)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0)
            {
                canBeHurt = true;
            }
        }


        //If player has died set countdown for time to respawn
        if (alive == false)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                RespawnPlayer();
            }
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        //If collided with a health pack
        if (other.gameObject.tag == "HealthPack")
        {
            HealPlayer();
            Destroy(other.gameObject);
        }
    }

    //Heal player
    private void HealPlayer()
    {
        //raise health
        curHealth += 3;
        //check that health is not over max
        if (curHealth > maxHealth)
        {
            curHealth = maxHealth;
        }
        //update health UI
        UIController.Instance.UpdateBars(curHealth, maxHealth);
    }


    //Inflict damage to the players health
    public void DamagePlayer(int damageAmount)
    {
        //If the zombie is attacking, player can be hurt and player is not dead
        if (canBeHurt && alive)
        {
            src.PlayOneShot(hurt, 0.7f);
            canBeHurt = false;
            hitTimer = 3f;
            //damage player
            curHealth -= damageAmount;
            UIController.Instance.UpdateBars(curHealth, maxHealth);

            //TODO track total damage taken per wave for money
        }

        //If health is zero, kill player
        if (curHealth <= 0)
        {
            KillPlayer();
        }
    }


    //Kill player
    public void KillPlayer()
    {
        src.PlayOneShot(die, 0.7f);
        curHealth = 0;
        if (deathEffect != null)
        {
            Destroy(Instantiate(deathEffect, transform.position, Quaternion.Euler(0, 0, 90)), 0.5f);
        }
        //Play death animation
        PlayerAnimationController.Instance.SetDead(true);
        alive = false;
        respawnTimer = 5;

        //TODO Track times died per wave for money
    }


    //Respawn player
    public void RespawnPlayer()
    {
        //Move player to last checkpoint
        RespawnController.Instance.RespawnPlayer();
        //Turn off death animation
        PlayerAnimationController.Instance.SetDead(false);
        //Set alive
        alive = true;
        //Reset Health
        curHealth = maxHealth;
        UIController.Instance.UpdateBars(curHealth, maxHealth);
    }
}
