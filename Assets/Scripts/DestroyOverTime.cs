using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour
{
    [SerializeField] private float lifetime;
    private float disableCountdown;
    private MeshRenderer mesh;

    //Options for how to disable an object
    //Destory : Destroys the game object after lifetime
    //DisableMesh : make the object invisible after lifetime
    //DisableGameObject : sets the game object to inactive after lifetime
    enum DisableType : short { Destroy, DisableMesh, DisableGameObject};

    [SerializeField] DisableType disableType;

    private void Awake()
    {
        mesh = GetComponent<MeshRenderer>();
        disableCountdown = lifetime;
    }

    // Update is called once per frame
    void Start()
    {
        if (disableType == DisableType.Destroy)
        {
            Destroy(gameObject, lifetime);
        }
    }

    private void Update()
    {
        if (mesh.enabled == true && disableType == DisableType.DisableMesh)
        {
            disableCountdown -= Time.deltaTime;
            if (disableCountdown <= 0)
            {
                mesh.enabled = false;
                disableCountdown = lifetime;
            }
        }

        if (gameObject.activeSelf && disableType == DisableType.DisableGameObject)
        {
            disableCountdown -= Time.deltaTime;
            if (disableCountdown <= 0)
            {
                gameObject.SetActive(false);
                disableCountdown = lifetime;
            }
        }
    }
}
