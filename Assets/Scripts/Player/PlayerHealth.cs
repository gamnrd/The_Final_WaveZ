using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    //Health
    [Header("Health")]
    [SerializeField] private int curHealth;
    [SerializeField] private int maxHealth = 10;

    [Header("Lives")]
    [SerializeField] private int curLives;
    [SerializeField] private int maxLives = 3;

    //Damage
    [Header("Damage")]
    [SerializeField] private bool canBeHurt = true;
    [SerializeField] private float hitTimer = 0;

    //Respawn
    [SerializeField] private bool isAlive;
    private float respawnTimer = 0;

    //Fx
    [Header("FX")]
    public GameObject deathEffect;

    //Sound
    [Header("Sound")]
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip hurt;
    private AudioSource src;

    //Animator
    private PlayerAnimationController anim;


    private void Awake()
    {
        //Get max health based on difficulty
        maxHealth = PlayerPrefs.GetInt("PlayerHealth", 10);
        curHealth = maxHealth;
        curLives = maxLives;
        anim = GetComponent<PlayerAnimationController>();
        src = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        isAlive = true;
        UIController.Instance.UpdateBars(curHealth, maxHealth);
    }

    // Update is called once per frame
    void Update()
    {
        //If player cannot be hurt and is alive, don't countdown
        if (canBeHurt == false && isAlive == true)
        {
            hitTimer -= Time.deltaTime;
            if (hitTimer <= 0)
            {
                canBeHurt = true;
            }
        }


        //If player has died set countdown for time to respawn
        if (isAlive == false)
        {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0)
            {
                RespawnPlayer();
            }
        }
    }

    //Return whether the player is alive or not
    public bool GetPlayerAlive()
    {
        return isAlive;
    }

    //Heal player
    public void HealPlayer(int healAmount)
    {
        //raise health
        curHealth += healAmount;
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
        if (canBeHurt && isAlive)
        {
            src.PlayOneShot(hurt, 0.7f);
            canBeHurt = false;
            hitTimer = 3f;
            //damage player
            curHealth -= damageAmount;
            UIController.Instance.UpdateBars(curHealth, maxHealth);

            //If health is zero, kill player
            if (curHealth <= 0)
            {
                curLives--;
                UIController.Instance.livesTxt.text = (curLives + " Lives").ToString();
                if (curLives > 0)
                {
                    KillPlayer();
                }
                else if (curLives <= 0)
                {
                    curLives = 0;
                    GameOver();
                }
            }
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
        anim.SetDead(true);
        isAlive = false;
        respawnTimer = 5;
    }


    //Respawn player
    public void RespawnPlayer()
    {
        //Move player to last checkpoint
        RespawnController.Instance.RespawnPlayer();
        //Turn off death animation
        anim.SetDead(false);
        //Set alive
        isAlive = true;
        //Reset Health
        curHealth = maxHealth;
        UIController.Instance.UpdateBars(curHealth, maxHealth);
    }

    public void GameOver()
    {
        Application.Quit();
    }
}
