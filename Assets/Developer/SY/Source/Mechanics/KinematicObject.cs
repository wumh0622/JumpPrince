using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KinematicObject : MonoBehaviour
{
    /// <summary>
    /// The minimum normal (dot product) considered suitable for the entity sit on.
    /// </summary>
    public float minGroundNormalY = .65f;

    /// <summary>
    /// A custom gravity coefficient applied to this entity.
    /// </summary>
    public float gravityModifier = 1f;

    /// <summary>
    /// The current velocity of the entity.
    /// </summary>
    public Vector2 velocity;

    /// <summary>
    /// Is the entity currently sitting on a surface?
    /// </summary>
    /// <value></value>
    public bool IsGrounded { get; private set; }

    protected Vector2 targetVelocity;
    protected Vector2 groundNormal;
    protected Rigidbody2D body;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];

    protected Transform groundTransform;
    protected Vector2 groundRelativePosition;

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

    /// <summary>
    /// Bounce the object's vertical velocity.
    /// </summary>
    /// <param name="value"></param>
    public void Bounce(float value)
    {
        velocity.y = value;
    }

    /// <summary>
    /// Bounce the objects velocity in a direction.
    /// </summary>
    /// <param name="dir"></param>
    public void Bounce(Vector2 dir)
    {
        velocity.y = dir.y;
        velocity.x = dir.x;
    }

    /// <summary>
    /// Teleport to some position.
    /// </summary>
    /// <param name="position"></param>
    public void Teleport(Vector3 position)
    {
        body.position = position;
        velocity *= 0;
        body.velocity *= 0;
    }

    protected virtual void OnEnable()
    {
        body = GetComponent<Rigidbody2D>();
    }

    protected virtual void Start()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer));
        contactFilter.useLayerMask = true;
    }

    protected virtual void Update()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
    }

    protected virtual void ComputeVelocity()
    {

    }

    protected virtual void FixedUpdate()
    {
        if (groundTransform is not null)
        {
            body.position = new Vector2(groundTransform.position.x, groundTransform.position.y) + groundRelativePosition;
        }

        velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;   
        velocity += targetVelocity;

        IsGrounded = false;

        var deltaPosition = velocity * Time.deltaTime;

        var moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x);

        var move = moveAlongGround * deltaPosition.x;

        PerformMovement(move);

        move = Vector2.up * deltaPosition.y;

        PerformMovement(move);
    }

    void PerformMovement(Vector2 move)
    {
        var distance = move.magnitude;

        if (distance > minMoveDistance)
        {
            bool HasFloor = false;
            //check if we hit anything in current direction of travel
            var count = body.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            for (var i = 0; i < count; i++)
            {
                //remove shellDistance from actual move distance.
                var modifiedDistance = hitBuffer[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;

                var currentNormal = hitBuffer[i].normal;

                //is this surface flat enough to land on?
                if (currentNormal.y > minGroundNormalY)
                {                    
                    IsGrounded = true;
                    groundTransform = hitBuffer[i].transform.gameObject.GetComponent<Transform>();              

                    HasFloor = true;

                    groundNormal = currentNormal;
                    currentNormal.x = 0;

                    velocity *= 0;
                }
                else
                {
                    if (Vector2.Dot(velocity, currentNormal) <= 0)
                    {
                        velocity = Vector2.Reflect(velocity, currentNormal);
                    }
                    else 
                    {
                        distance = move.magnitude;
                    }
                }
            }

            if (!HasFloor) 
            {
                groundTransform = null;
                groundNormal = Vector2.up;
            }
        }

        body.position += move.normalized * distance;
        if (groundTransform is not null)
        {      
            groundRelativePosition = body.position - new Vector2(groundTransform.position.x, groundTransform.position.y);
        }
    }
}