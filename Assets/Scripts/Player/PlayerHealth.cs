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
    [SerializeField] private GameObject pauseMenu;

    //Damage
    [Header("Damage")]
    private float lastTimeHit;
    [SerializeField] private float hitTimer = 1f;

    //Respawn
    [SerializeField] private bool isAlive = true;
    private float respawnTimer = 2;

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
        anim = GetComponent<PlayerAnimationController>();
        src = GetComponent<AudioSource>();
    }


    // Start is called before the first frame update
    void Start()
    {
        curHealth = maxHealth;
        curLives = maxLives;
        GameUI.Instance.UpdateHealthBars(curHealth, maxHealth);
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
        curHealth = Mathf.Min(curHealth + healAmount, maxHealth);
        GameUI.Instance.UpdateHealthBars(curHealth, maxHealth);
    }


    //Inflict damage to the players health
    public void DamagePlayer(int damageAmount)
    {
        //If the zombie is attacking, player can be hurt and player is not dead
        if (Time.time - lastTimeHit < hitTimer || !isAlive) return;

        lastTimeHit = Time.time;
        src.PlayOneShot(hurt, 0.7f);
        //damage player
        curHealth = Mathf.Max(curHealth - damageAmount, 0);
        GameUI.Instance.UpdateHealthBars(curHealth, maxHealth);

        //If health is zero, kill player
        if (curHealth <= 0)
            KillPlayer();

    }


    //Kill player
    public void KillPlayer()
    {
        //Update stats
        isAlive = false;
        curLives--;
        GameUI.Instance.UpdateLives(curLives);

        //Play death animation
        src.PlayOneShot(die, 0.7f);
        if (deathEffect != null)
            Destroy(Instantiate(deathEffect, transform.position, Quaternion.Euler(0, 0, 90)), 0.5f);
        anim.SetDead(true);
        
        //Check action based on lives remaining
        if (curLives > 0)
            Invoke("RespawnPlayer", respawnTimer);

        else if (curLives <= 0)
            GameOver();
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
        GameUI.Instance.UpdateHealthBars(curHealth, maxHealth);
    }

    public void GameOver()
    {
        GameOverScreen.Instance.GameOver();
        //Time.timeScale = 0;
    }
}
