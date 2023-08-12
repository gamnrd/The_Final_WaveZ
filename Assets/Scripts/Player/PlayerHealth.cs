using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerHealthStats
{
    public float _curHealth;
    public float _maxHealth;
}


public class PlayerHealth : MonoBehaviour
{
    //Health
    [Header("Health")]
    [SerializeField] private PlayerHealthStats playerHealth = new PlayerHealthStats();
    [SerializeField] private float defence;


    //Damage
    [Header("Damage")]
    private float lastTimeHit;
    [SerializeField] private float hitTimer = 1f;

    //Respawn
    [SerializeField] private bool isAlive = true;
    [SerializeField] public Transform playerBody;

    //Fx
    [Header("FX")]
    public GameObject deathEffect;

    //Sound
    [Header("Sound")]
    [SerializeField] private AudioClip die;
    [SerializeField] private AudioClip hurt;
    [SerializeField] private AudioSource src;

    //Animator
    [SerializeField] private PlayerAnimationController anim;

    [Header("Events")]
    [SerializeField] private GameEvent onPlayerHealthChange;
    [SerializeField] private GameEvent onPlayerDeath;


    private void Awake()
    {
        //Get max health based on difficulty   
        anim = gameObject.GetComponent<PlayerAnimationController>();
        src = gameObject.GetComponent<AudioSource>();
        playerBody = transform.GetChild(0).transform;
    }


    // Start is called before the first frame update
    void Start()
    {
        playerHealth._maxHealth = PlayerDataManager.instance.data.maxHealth;
        playerHealth._curHealth = playerHealth._maxHealth;
        defence = PlayerDataManager.instance.data.defence;
        onPlayerHealthChange.Raise(this, playerHealth);
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
        playerHealth._curHealth = Mathf.Min(playerHealth._curHealth + healAmount, playerHealth._maxHealth);
        onPlayerHealthChange.Raise(this, playerHealth);
    }


    //Inflict damage to the players health
    public void DamagePlayer(Component sender, object data)
    {
        if (data is float damageTaken)
        {
            //If the zombie is attacking, player can be hurt and player is not dead
            if (!isAlive || Time.time - lastTimeHit < hitTimer) return;

            lastTimeHit = Time.time;
            src.PlayOneShot(hurt, 0.5f);
            anim.PlayerHit();


            //damage player
            float dmg = damageTaken - (damageTaken * (defence / 100));
            playerHealth._curHealth = Mathf.Max(playerHealth._curHealth - dmg, 0.0f);
            onPlayerHealthChange.Raise(this, playerHealth);

            //If health is zero, kill player
            if (playerHealth._curHealth <= 0)
                KillPlayer();
        }
    }


    //Kill player
    public void KillPlayer()
    {
        //Update stats
        isAlive = false;

        //Play death animation
        src.PlayOneShot(die, 0.7f);
        if (deathEffect != null)
            Destroy(Instantiate(deathEffect, transform.position, Quaternion.Euler(0, 0, 90)), 0.5f);
        anim.SetDead(true);

        onPlayerDeath.Raise();
    }


    //Respawn player
    public void RespawnPlayer()
    {
        //Move Player to spawn location
        transform.position = Vector3.zero;
        playerBody.transform.localPosition = Vector3.zero;

        //Turn off death animation
        anim.SetDead(false);

        //Set alive
        isAlive = true;

        //Reset Health
        playerHealth._curHealth = playerHealth._maxHealth;
        onPlayerHealthChange.Raise(this, playerHealth);

    }
}


