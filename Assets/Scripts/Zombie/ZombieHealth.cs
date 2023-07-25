using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ZombieHealth : DestroyPoolableObject
{
    public int totalHealth = 3;
    public GameObject deathEffect;
    public Transform deathEffectPos;
    private GameMode mode;
    //Sound
    public AudioSource src;
    public AudioClip die;
    private GameObject pool;
    private NavMeshAgent navAgent;

    private void Awake()
    {
        pool = GameObject.Find("Zombie Pool");
        totalHealth = PlayerPrefs.GetInt("ZombieHealth", 3);
        navAgent = GetComponent<NavMeshAgent>();
        //mode = GameManager.instance.gameMode;
    }

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

                if (GameManager.instance.gameMode == GameMode.Wave)
                {
                    WaveZombieCounter.Instance.DecrementCount();
                    WaveManager.Instance.ZombieKilled();
                }

                Destroy(Instantiate(deathEffect, deathEffectPos.position, deathEffectPos.rotation), 0.5f);
            }

            Disable();
        }
    }

    public void MoveToPool()
    {
        transform.SetParent(pool.transform);
    }
}
