using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAnimationController : MonoBehaviour
{
    public Animator anim;
    public static ZombieAnimationController Instance;
    private float timer;

    private void Awake()
    {
        Instance = this;
        anim = GetComponent<Animator>();
    }

    //Set the speed of the animation
    public void setSpeed(float speed)
    {
        anim.SetFloat("Speed_f", speed);
    }

    //Play attack animation
    public void setAttack()
    {
        anim.SetBool("Attacking_b", true);
        anim.Play("Zombie_Eating");
        if (!anim.GetCurrentAnimatorStateInfo(0).IsName("Zombie_Eating"))
        {
            anim.SetBool("Attacking_b", false);
        }
    }
}
