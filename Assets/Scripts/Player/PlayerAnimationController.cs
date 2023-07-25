using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    public static PlayerAnimationController Instance;
    private Rigidbody rb;
    private Transform thisTransform;
    [SerializeField] private float speedF;
    [SerializeField] private Vector3 forwards;
    [SerializeField] private float velocityThreashold = 0.1f;


    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        thisTransform = transform;
    }

    private void Start()
    {
        InvokeRepeating("PlayerWalkAnimations", 0f, 0.25f);
    }


    void PlayerWalkAnimations()
    {
        //Get forwards direction
        forwards = thisTransform.forward;

        //Check for player movement
        speedF = ((Mathf.Abs(rb.velocity.x) + Mathf.Abs(rb.velocity.z)) / 2);
        SetSpeed(speedF);

        if (speedF > 0.3f)
        {
            //Check that if the player is moving in the direction they are facing in order to play the walking animation forwards and backwards
            if ((rb.velocity.z < velocityThreashold * -1 && forwards.z < velocityThreashold * -1) || (rb.velocity.z > velocityThreashold && forwards.z > velocityThreashold) ||
                (rb.velocity.x > velocityThreashold && forwards.x > velocityThreashold) || (rb.velocity.x < velocityThreashold * -1 && forwards.x < velocityThreashold * -1))
            {
                //facing and walking forward
                this.anim.SetFloat("Forwards_f", 1);
                return;
            }
            else
            {
                this.anim.SetFloat("Forwards_f", -1);
                return;
            }
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
    /*
    public void SetBodyVert(float bodyV)
    {
        anim.SetFloat("Body_Vertical_f", bodyV);
    }*/
}
