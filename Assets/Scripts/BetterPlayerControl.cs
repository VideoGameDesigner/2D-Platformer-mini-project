using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class BetterPlayerControl : MonoBehaviour
{
    public float speed;
    public float jump;

    private float move;
    private Rigidbody2D rb;
    private bool FacingRight = true;
    public LayerMask WhatIsWall;
    public float checkradius;
    private bool isOnwall;

    bool isTouchingFront;
    public Transform FrontCheck;
    bool wallsliding;
    public float wallslidingspeed;
    bool walljumping;
    public float xwallforce;
    public float ywallforce;
    public float walljumptime;

    private bool canDash = true;
    private bool isDashing;
   
    [SerializeField] private float dashingPower = 24f;
    [SerializeField] private float dashingTime = 0.2f;
    [SerializeField] private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer tr;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        isOnwall = true;
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        //move = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(move * speed, rb.velocity.y);

        if (FacingRight == false && move > 0)
        {
            Flip();
        }

        else if (FacingRight == true && move < 0)
        {
            Flip();

        }
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


        if (isDashing)
        {
            return;
        }


        /*if (Input.GetButtonDown("Jump")&& Mathf.Abs(rb.velocity.y) <0.001f)
        {
            rb.AddForce(new Vector2(0,jump), ForceMode2D.Impulse);           
        }*/

        isTouchingFront = Physics2D.OverlapCircle(FrontCheck.position, checkradius, WhatIsWall);

        if (isTouchingFront == true && isOnwall == true && move != 0)
        {
            wallsliding = true;
        }

        else
        {
            wallsliding = false;
        }

        if (wallsliding)

        {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallslidingspeed, float.MaxValue));
        }

        /*if (Input.GetButtonDown("Jump") && wallsliding == true)
        {
            walljumping = true;
            Invoke("SetWallJumpingToFalse", walljumptime);
        }*/

        if (walljumping == true)
        {
            rb.velocity = new Vector2(xwallforce * -move, ywallforce);
        }

        /*if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }*/

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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void Move(InputAction.CallbackContext context)
    {
        move = context.ReadValue<Vector2>().x;

    }

    public void Jump(InputAction.CallbackContext context)
    {
        if(Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.AddForce(new Vector2(0, jump), ForceMode2D.Impulse);
        }

        if(wallsliding == true)
        {
            walljumping = true;
            Invoke("SetWallJumpingToFalse", walljumptime);
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if(canDash == true)
        {
            StartCoroutine(Dash());
        }
    }


}
