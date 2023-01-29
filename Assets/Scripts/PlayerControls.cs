using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;

public class PlayerControls : MonoBehaviour
{
    public float MovementSpeed;
    public float Jump;
    private float MoveInput;

    private Rigidbody2D Rb;
    private bool FacingRight = true;

    private bool isgrounded;
    public Transform groundcheck;
    public float checkradius;
    public LayerMask whatIsGround;
    private int extrajumps;
    public int extrajumpvalues;

    bool isTouchingFront;
    public Transform FrontCheck;
    bool wallsliding;
    public float wallslidingspeed;
    bool walljumping;
    public float xwallforce;
    public float ywallforce;
    public float walljumptime;


    // Start is called before the first frame update
    void Start()
    {
       Rb = GetComponent<Rigidbody2D>();    
    }

    private void FixedUpdate()
    {

        isgrounded = Physics2D.OverlapCircle(groundcheck.position, checkradius,whatIsGround);

        MoveInput = Input.GetAxisRaw("Horizontal");
        Rb.velocity = new Vector2(MoveInput * MovementSpeed,Rb.velocity.y);

        
        if (FacingRight == false && MoveInput > 0 )
        {
            Flip();
        }

        else if (FacingRight == true && MoveInput < 0)
        {
            Flip();

        }
    }


    // Update is called once per frame
    void Update()
    {
        if(isgrounded == true)
        {
            extrajumps = extrajumpvalues;
        }

        if(Input.GetButtonDown("Jump")&& extrajumps > 0)
        {
            Rb.velocity = Vector2.up * Jump;
            extrajumps--;
        }

        else if(Input.GetButtonDown("Jump") && extrajumps == 0 && isgrounded == true)
        {
            Rb.velocity = Vector2.up * Jump;
        }
        isTouchingFront = Physics2D.OverlapCircle(FrontCheck.position, checkradius, whatIsGround);

        if (isTouchingFront == true && isgrounded == false && MoveInput != 0)
        {
            wallsliding = true;

        }

        else
        {
            wallsliding = false;
        }

        if (wallsliding)

        {
            Rb.velocity = new Vector2(Rb.velocity.x, Mathf.Clamp(Rb.velocity.y, -wallslidingspeed, float.MaxValue));
        }

        if(Input.GetButtonDown("Jump") && wallsliding == true)
        {
            walljumping = true;
            Invoke("SetWallJumpingToFalse", walljumptime);
        }

        if(walljumping ==true)
        {
            Rb.velocity = new Vector2(xwallforce * -MoveInput, ywallforce);
        }


    }


    void Flip()

    {
        FacingRight = !FacingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;

    }

    void SetWallJumpingToFalse()
    {
        walljumping = false;
    }

    
}
