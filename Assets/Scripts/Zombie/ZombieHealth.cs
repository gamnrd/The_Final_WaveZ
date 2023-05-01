using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieHealth : DestroyPoolableObject
{
    public int totalHealth = 3;
    public GameObject deathEffect;
    public Transform deathEffectPos;
    //Sound
    public AudioSource src;
    public AudioClip die;

    public override void OnEnable()
    {
        totalHealth = PlayerPrefs.GetInt("ZombieHealth", 3);
        GetComponent<ZombieMovement>().enabled = true;
        base.OnEnable();
    }
    public override void OnDisable()
    {
        Invoke("MoveToPool", 0.1f);
        GetComponent<ZombieMovement>().enabled = false;
        base.OnDisable();
    }

    private void Awake()
    {
        totalHealth = PlayerPrefs.GetInt("ZombieHealth", 3);
    }

    private void OnCollisionEnter(Collision other)
    {
        //Zombie damage
        if (other.gameObject.CompareTag("Bullet"))
        {
            //damage zombie
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
                GameUI.Instance.AdjustScore(100);
                //ZombieCounter.Instance.decrementCount();
                //WaveManager.Instance.ZombieKilled();
                Destroy(Instantiate(deathEffect, deathEffectPos.position, deathEffectPos.rotation), 0.5f);
            }
            //Destroy(gameObject);

            //GetComponent<ZombieMovement>().enabled = false;
            Invoke("Disable", 0.1f);
        }
    }

    public void MoveToPool()
    {
        transform.SetParent(GameObject.Find("Zombie Pool").transform);
    }
}
