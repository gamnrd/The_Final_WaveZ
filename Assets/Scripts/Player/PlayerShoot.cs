using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject weapon;
    public GameObject bulletObj;
    public Transform bulletSpawn;
    public GameObject gunFlash;
    public float bulletSpeed;
    public FixedJoystick aimJoystick;

    //Weapon Delays
    private float timer;
    private float pistolFireRate = 7.5f;
    private float cooldown = 0.5f;

    //Sound
    public AudioSource src;
    public AudioClip shoot;

    private void Start()
    {
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //If mouse is clicked, player is alive and the game is not paused
        /*if (Input.GetMouseButtonDown(0) && PlayerHealth.Instance.alive && UIController.Instance.getPaused() == false)
        {
            if (cooldown <= 0)
            {
                Shoot();
                cooldown = delay;
            }   
        }
        if (cooldown > 0)
        {
            cooldown--;
        }*/

        //Joystick fireing
        if ((aimJoystick.Vertical != 0 || aimJoystick.Horizontal != 0) && PlayerHealth.Instance.alive && UIController.Instance.getPaused() == false)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Shoot();
                timer = cooldown;
            }
        }
    }


    void Shoot()
    {
        //Play shoot animation
        PlayerAnimationController.Instance.setShoot();
        src.PlayOneShot(shoot, 0.075f);

        //Gun flash
        Instantiate(gunFlash, bulletSpawn.position, bulletSpawn.rotation, bulletSpawn);

        //Bullet
        GameObject bullet = Instantiate(bulletObj, bulletSpawn.position, bulletSpawn.rotation);
        Rigidbody bulletRB = bullet.GetComponent<Rigidbody>();
        bulletRB.AddForce(bulletSpawn.forward * bulletSpeed, ForceMode.Impulse);
    }
}
