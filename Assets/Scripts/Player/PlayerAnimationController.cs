using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    public Animator anim;
    public static PlayerAnimationController Instance;
    private Rigidbody rb;


    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void Update()
    {
        PlayerWalkAnimations();
    }

    void PlayerWalkAnimations()
    {
        //Check for player movement
        float speedF;
        speedF = ((Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)) / 2);
        //If speed is not above threashold, set to zero
        if (speedF < 0.1f && speedF > -0.1f)
        {
            speedF = 0;
        }
        SetSpeed(speedF);

        //If the player is moving forward play animation forward
        if (((rb.velocity.x + rb.velocity.z) / 2) > 0.1)
        {
            this.anim.SetFloat("Forwards_f", 1);
        }
        //If player is moving backwards reverse animation
        else if (((rb.velocity.x + rb.velocity.z) / 2) < -0.1)
        {
            this.anim.SetFloat("Forwards_f", -1);
        }

        //Adjust animation to prevent body from tilting and causing bullets to fire down
        //If moving
        if (speedF > 0.5)
        {
            SetBodyVert(0.3f);
        }
        //If idle
        else if (speedF == 0)
        {
            SetBodyVert(0f);
        }
    }

    //Set speed parameter
    public void SetSpeed(float speed)
    {
        anim.SetFloat("Speed_f", speed);
    }

    //Play shooting animation
    public void setShoot()
    {
        anim.fireEvents = false;
        anim.SetBool("Shoot_b", true);
        anim.Play("Character_Handgun_Shoot");
        if(!anim.GetCurrentAnimatorStateInfo(0).IsName("Character_Handgun_Shoot"))
        {
            anim.SetBool("Shoot_b", false);
        }
    }
    //Set dead parameter
    public void SetDead(bool dead)
    {
        anim.SetInteger("DeathType_int", 2);
        anim.SetBool("Death_b", dead);
    }
    //Adjust body tilt
    public void SetBodyVert(float bodyV)
    {
        anim.SetFloat("Body_Vertical_f", bodyV);
    }
}
