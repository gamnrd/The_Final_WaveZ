using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    //public GameObject weapon;
    [Header("Bullet")]
    //[SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private GameObject bulletSpawnPoint;
    [SerializeField] private GameObject gunFlash;
    
    [Header("Touch Controls")]
    [SerializeField] private bool usingTouch;
    [SerializeField] private FixedJoystick aimJoystick;
    //Weapon Delays
    private float timer;
    private float cooldown = 0.5f;

    //Sound
    [Header("Sound")]
    [SerializeField] private AudioClip shoot;
    private AudioSource src;

    //Object Pool
    [SerializeField] public ObjectPool bulletPool;

    private PlayerHealth playerHealth;

    private void Awake()
    {
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, 25);
    }

    private void Start()
    {
        src = GetComponent<AudioSource>();
        bulletSpawnPoint = GameObject.Find("FirePoint");
        playerHealth = GetComponent<PlayerHealth>();
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (usingTouch)
        {
            //Joystick fireing
            if ((aimJoystick.Vertical != 0 || aimJoystick.Horizontal != 0) && playerHealth.GetPlayerAlive() && !UIController.Instance.getPaused())
            {
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    Shoot();
                    timer = cooldown;
                }
            }
        }
        else
        {
            //If mouse is clicked, player is alive and the game is not paused
            if (Input.GetMouseButtonDown(0) && playerHealth.GetPlayerAlive() && !UIController.Instance.getPaused())
            {
                Shoot();
            }
        }
    }


    void Shoot()
    {
        //Play shoot animation
        PlayerAnimationController.Instance.setShoot();
        src.PlayOneShot(shoot, 0.075f);

        //Gun flash
        Instantiate(Resources.Load("GunFlash"), bulletSpawnPoint.transform.position, bulletSpawnPoint.transform.rotation, bulletSpawnPoint.transform);

        //Bullet
        PoolableObject instance = bulletPool.GetObject();
        if (instance != null)
        {
            instance.transform.SetParent(bulletSpawnPoint.transform, true);
            instance.transform.position = bulletSpawnPoint.transform.position;
            instance.transform.rotation = bulletSpawnPoint.transform.rotation;
        }

        //Instantiate(Resources.Load("Bullet"), bulletSpawnPoint.position, bulletSpawnPoint.rotation);
    }
}
