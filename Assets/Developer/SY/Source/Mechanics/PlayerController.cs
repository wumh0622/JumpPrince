using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using Platformer.Gameplay;
//using static Platformer.Core.Simulation;
//using Platformer.Model;
//using Platformer.Core;

public class PlayerController : KinematicObject
{
    public AudioClip jumpAudio;
    public AudioClip respawnAudio;
    public AudioClip ouchAudio;

    public float maxSpeed = 7;

    public float maxJumpForce = 10;
    public float minJumpForce = 3;
    public float jumpForceAcceleration = 0.2f;
    public float jumpForceAccumulator = 0;

    public JumpState jumpState = JumpState.Grounded;

    private float lastDirection = 1;

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

        jumpForceAccumulator = 0;
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
                        move.x = 0;
                    }
                    else 
                    {
                        move.x = Input.GetAxis("Horizontal");
                        if (move.x != 0)
                        {
                            lastDirection = move.x > 0 ? 1 : -1;
                        }
                    }
                }
                else if (jumpState == JumpState.Charging && Input.GetButtonUp("Jump"))
                {
                    jumpState = JumpState.StartToJump;
                }
            }
            else 
            {
                move.x = lastDirection;
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
                jumpForceAccumulator = minJumpForce;
                jumpState = JumpState.Charging;
                break;
            case JumpState.Charging:
                jumpForceAccumulator += jumpForceAcceleration;
                if (jumpForceAccumulator >= maxJumpForce) 
                {
                    jumpForceAccumulator = maxJumpForce;
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
        if (jump && IsGrounded)
        {
            velocity.y = jumpForceAccumulator;
            jump = false;
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