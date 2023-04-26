using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : MonoBehaviour
{
    public int totalHealth = 3;
    public GameObject deathEffect;
    public GameObject zombieCount;
    public Transform deathEffectPos;
    //Sound
    public AudioSource src;
    public AudioClip die;


    private void Awake()
    {
        totalHealth = PlayerPrefs.GetInt("ZombieHealth", 3);
    }

    private void OnCollisionEnter(Collision other)
    {
        //Zombie damage
        if (other.gameObject.tag == "Bullet")
        {
            //damage zombie
            Destroy(other.gameObject);
            DamageEnemy(1);
        }
    }


    //Zombie takes damage
    public void DamageEnemy(int damageAmount)
    {
        totalHealth -= damageAmount;
        //If health is zero, kill zombie
        if (totalHealth <= 0)
        {
            if (deathEffect != null)
            {
                src.PlayOneShot(die, 0.7f);
                UIController.Instance.AdjustScore(100);
                ZombieCounter.Instance.decrementCount();
                WaveManager.Instance.ZombieKilled();
                Destroy(Instantiate(deathEffect, deathEffectPos.position, deathEffectPos.rotation), 0.5f);
            }
            Destroy(gameObject);
        }
    }
}
