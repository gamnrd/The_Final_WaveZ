using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolManager : MonoBehaviour
{
    public static ParticlePoolManager instance = null;

    [SerializeField] private Particle sparkPrefab;
    [SerializeField] private Particle bloodPrefab;
    [SerializeField] private Particle deathFXPrefab;

    //Object Pool
    [SerializeField] public ObjectPool bloodPool;
    [SerializeField] public ObjectPool sparkPool;
    [SerializeField] public ObjectPool deathPool;

    private void Awake()
    {
        if (instance == null)
            instance = this;

        bloodPool = ObjectPool.CreateInstance(bloodPrefab, 20, "Blood Pool");
        sparkPool = ObjectPool.CreateInstance(sparkPrefab, 20, "Spark Pool");
        deathPool = ObjectPool.CreateInstance(deathFXPrefab, 15, "Death Pool");
    }
}
