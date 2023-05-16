using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(InputController))]
public class PlayerShoot : MonoBehaviour
{
    //public GameObject weapon;
    [Header("Bullet")]
    //[SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform bulletSpawnPoint;
    [SerializeField] private MeshRenderer gunFlash;
    

    //Weapon Delays
    private float timer;
    [SerializeField] private float fireRate = 0.25f;

    //Sound
    [Header("Sound")]
    [SerializeField] private AudioClip shoot;
    private AudioSource src;

    //Object Pool
    [SerializeField] public ObjectPool bulletPool;

    private PlayerHealth playerHealth;
    private InputController input;

    private void Awake()
    {
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, 10);
        src = GetComponent<AudioSource>();
        bulletSpawnPoint = GameObject.Find("FirePoint").transform;
        playerHealth = GetComponent<PlayerHealth>();
        gunFlash = transform.Find("GunFlash").GetComponent<MeshRenderer>();
        input = GetComponent<InputController>();
    }

    private void Start()
    {

        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseScreen.Instance.GetPaused() || !playerHealth.GetPlayerAlive()) return;

        if (input.platform == Platform.Mobile && input.joystickAim != Vector2.zero)
        {

            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Shoot();
                timer = fireRate;
            }
        }
    }

    public void FirePressed(InputAction.CallbackContext context)
    {
        if (context.performed) Shoot();
    }

    public void Shoot()
    {
        //Disable shooting when the game is paused or the player is dead
        if (PauseScreen.Instance.GetPaused() || !playerHealth.GetPlayerAlive()) return;

        //Play shoot animation
        PlayerAnimationController.Instance.setShoot();

        if (input.platform == Platform.Mobile)
        {
            src.PlayOneShot(shoot, 0.025f);
        }
        else
        {
            src.PlayOneShot(shoot, 0.075f);
        }
        

        //Gun flash
        gunFlash.enabled = true;

        //Bullet
        PoolableObject instance = bulletPool.GetObject();
        if (instance != null)
        {
            //instance.transform.SetParent(bulletSpawnPoint.transform, true);
            instance.transform.position = bulletSpawnPoint.position;
            instance.transform.rotation = bulletSpawnPoint.rotation;
        }
    }
}
