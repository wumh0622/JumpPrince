using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Platformer.Gameplay;
//using static Platformer.Core.Simulation;
//using Platformer.Model;
//using Platformer.Core;

public class PlayerController : KinematicObject
{
    public float maxSpeed = 7;

    public float maxJumpForce = 10;
    public float minJumpForce = 3;
    public float inputJumpAcceleration = 0.2f;

    private float inputJumpAccumulator = 0;
    private float jumpDirection = 0;

    public JumpState jumpState = JumpState.Grounded;

    /*internal new*/
    public Collider2D collider2d;
    //public Health health;
    public bool controlEnabled = true;

    bool jump;
    Vector2 move;
    SpriteRenderer spriteRenderer;
    //internal Animator animator;
    //readonly PlatformerModel model = Simulation.GetModel<PlatformerModel>();

    public Bounds Bounds => collider2d.bounds;

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
        groundTransform = null;

        inputJumpAccumulator = 0;
    }

    protected override void Update()
    {
        if (controlEnabled)
        {
            if (IsGrounded)
            {
                if (jumpState == JumpState.Grounded)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        jumpState = JumpState.PrepareToJump;
                        jumpDirection = move.x > 0 ? 1 : (move.x < 0 ? -1 : 0);
                        move.x = 0;
                    }
                    else
                    {
                        move.x = Input.GetAxis("Horizontal");
                    }
                }
                else if (jumpState == JumpState.Charging && Input.GetButtonUp("Jump"))
                {
                    jumpState = JumpState.StartToJump;
                    move.x = jumpDirection;
                }
            }
            else 
            {
                move.x = 0;
            }
        }
        else
        {
            move.x = 0;
        }
        UpdateJumpState();
        base.Update();
    }

    void UpdateJumpState()
    {
        jump = false;
        switch (jumpState)
        {
            case JumpState.PrepareToJump:
                inputJumpAccumulator = minJumpForce;
                jumpState = JumpState.Charging;
                break;
            case JumpState.Charging:
                inputJumpAccumulator += inputJumpAcceleration;
                if (inputJumpAccumulator >= maxJumpForce) 
                {
                    inputJumpAccumulator = maxJumpForce;
                }
                break;
            case JumpState.StartToJump:
                jumpState = JumpState.Jumping;
                jump = true;
                break;
            case JumpState.Jumping:
                if (!IsGrounded)
                {
                    jumpState = JumpState.InFlight;
                }
                break;
            case JumpState.InFlight:
                if (IsGrounded)
                {
                    jumpState = JumpState.Landed;
                }
                break;
            case JumpState.Landed:
                jumpState = JumpState.Grounded;
                break;
        }
    }

    protected override void ComputeVelocity()
    {
        if (IsGrounded)
        {
            if (jump)
            {
                velocity.y = inputJumpAccumulator;
                jump = false;
            }
        }

        if (move.x > 0.01f)
        {
            spriteRenderer.flipX = false;
        }
        else if (move.x < -0.01f)
        {
            spriteRenderer.flipX = true;
        }

        //animator.SetBool("grounded", IsGrounded);
        //animator.SetFloat("velocityX", Mathf.Abs(velocity.x) / maxSpeed);

        targetVelocity = move * maxSpeed;
    }

    public enum JumpState
    {
        Grounded,
        PrepareToJump,
        Charging,
        StartToJump,
        Jumping,
        InFlight,
        Landed
    }
}