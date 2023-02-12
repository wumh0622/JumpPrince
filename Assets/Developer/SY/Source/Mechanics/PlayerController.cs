using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CarterGames.Assets.AudioManager;
//using Platformer.Gameplay;
//using static Platformer.Core.Simulation;
//using Platformer.Model;
//using Platformer.Core;

public class PlayerController : KinematicObject
{
    public float maxSpeed = 7;

    public float maxJumpForce = 10;
    public float minJumpForce = 3;
    public float jumpHorizonalScale = 0.5f;
    public float inputJumpAcceleration = 0.2f;

    public float landTime = 0.5f;

    private float inputJumpAccumulator = 0;
    private float jumpDirection = 0;

    public JumpState jumpState = JumpState.Grounded;

    public bool controlEnabled = true;

    //Audio
    public string audioLand;
    public string audioJump;
    public string audioWalk;
    public AnimatorOverrideController FaceLeftAnimator;
    RuntimeAnimatorController FaceRightAnimator;

    bool jump;
    Vector2 move;

    Collider2D collider2d;
    SpriteRenderer spriteRenderer;
    Animator animator;

    void Awake()
    {
        collider2d = GetComponent<Collider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        groundTransform = null;
        jumpState = JumpState.Grounded;

        FaceRightAnimator = animator.runtimeAnimatorController;
    }

    protected override void Update()
    {
        move.x = 0;

        if (controlEnabled)
        {
            if (IsGrounded)
            {
                if (jumpState == JumpState.Grounded)
                {
                    if (Input.GetButtonDown("Jump"))
                    {
                        jumpState = JumpState.Charging;
                        inputJumpAccumulator = minJumpForce;

                        animator.SetInteger("JumpState", 1);
                    }
                    else
                    {
                        move.x = Input.GetAxis("Horizontal");
                        jumpDirection = move.x > 0 ? 1 : (move.x < 0 ? -1 : 0);
                    }
                }
                else if (jumpState == JumpState.Charging && Input.GetButtonUp("Jump"))
                {
                    jumpState = JumpState.StartToJump;
                    animator.SetInteger("JumpState", 2);
                }
            }
            else
            {
                jumpState = JumpState.InFlight;

                if (velocity.y > 0)
                {
                    animator.SetInteger("JumpState", 2);
                }
                else 
                {
                    animator.SetInteger("JumpState", 3);
                }
            }
        }

        UpdateJumpState();
        base.Update();
    }

    protected override void FixedUpdate()
    {
        if (jumpState == JumpState.Charging)
        {
            inputJumpAccumulator += inputJumpAcceleration;
            if (inputJumpAccumulator >= maxJumpForce)
            {
                inputJumpAccumulator = maxJumpForce;
            }
        }

        base.FixedUpdate();
    }

    void UpdateJumpState()
    {
        jump = false;
        switch (jumpState)
        {
            case JumpState.Charging:
                break;
            case JumpState.StartToJump:
                AudioManager.instance.Play(audioJump);
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
                AudioManager.instance.Play(audioLand);
                StartCoroutine(WaitForLanding());
                break;
        }
    }

    protected override void ComputeVelocity()
    {
        if (IsGrounded)
        {
            if (jump)
            {
                velocity = new Vector2(inputJumpAccumulator * jumpDirection * jumpHorizonalScale, inputJumpAccumulator);
                jump = false;
            }
        }

        animator.SetBool("IsMoving", Mathf.Abs(move.x) > 0.001f);

        targetVelocity = move * maxSpeed;

        if (velocity.x > 0.01f || targetVelocity.x > 0.01f)
        {
            animator.runtimeAnimatorController = FaceRightAnimator;
        }
        else if (velocity.x < -0.01f || targetVelocity.x < -0.01f)
        {
            animator.runtimeAnimatorController = FaceLeftAnimator;
        }
    }
    IEnumerator WaitForLanding()
    {
        animator.SetInteger("JumpState", 0);

        yield return new WaitForSeconds(landTime);

        if (jumpState == JumpState.InFlight)
        {
            jumpState = JumpState.Grounded;
        }
    }

    public enum JumpState
    {
        Grounded,
        Charging,
        StartToJump,
        Jumping,
        InFlight,
        Landed
    }
}