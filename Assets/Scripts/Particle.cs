using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : AutoDestroyPoolableObject
{
    private ParticleSystem particle;

    private void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public override void OnEnable()
    {
        base.OnEnable();
        PlayParticle();
    }

    public override void OnDisable()
    {
        //base.OnDisable();
    }
    /*
    private void Start()
    {
        Invoke("Disable", 2f);
    }*/

    public void PlayParticle()
    {
        particle.Play();
    }
}