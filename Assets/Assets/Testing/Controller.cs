using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour
{
    private Vector2 _velocity;
    [SerializeField] private Vector2 _maxSpeed;
    [SerializeField] private float _gravity;
    [SerializeField] private float _speed;
    [SerializeField] private bool _grounded;
    [SerializeField] private int _rayCount = 3;
    [SerializeField] private float _rayOffset = 0.5f;
    [SerializeField] private float _rayBaseDistance = 0.15f;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _maxWalkableSlopeAngle = 45f;
    [SerializeField] private Transform _groundCheckPosition;
    [SerializeField] private float _groundCheckRadius;
    [SerializeField] private float _slopeCheckDistance;
    private float _horizontalSlopeAngle;
    [SerializeField] private bool _isOnSlope = true;
    private Vector2 _slopeNormal;
    private float _verticalSlopeAngle;
    [SerializeField] private bool _walkableSlope;
    [SerializeField] private PhysicsMaterial2D _noFrictionMat;
    [SerializeField] private PhysicsMaterial2D _fullFrictionMat;
    private Vector2 _slopeNormalPerpendicular;
    [SerializeField] private float _slidingSpeed;
    [SerializeField] private float _stoppingSpeed;

    private Vector2 _inputDirection;
    private PlayerInput _playerInput;
    private Rigidbody2D body;

    private void OnEnable()
    {
        _playerInput.PlayerControls.Enable();
    }

    private void OnDisable()
    {
        _playerInput.PlayerControls.Disable();
    }

    void Awake()
    {
        _playerInput = new PlayerInput();
        body = GetComponent<Rigidbody2D>();
        _inputDirection = Vector2.zero;

        _playerInput.PlayerControls.Move.performed += ctx => _inputDirection = ctx.ReadValue<Vector2>();
        //_playerInput.PlayerControls.Jump.performed += ctx => Jump();
        //_playerInput.PlayerControls.Pause.performed += ctx => ;
    }
    
    public void Move(Vector2 amount)
    {
        _velocity += amount;
    }

    public void InputMove(Vector2 inputDirection)
    {
        _inputDirection = inputDirection;
    }
    

    private void FixedUpdate()
    {
        GroundCheck();
        SlopeCheck();
        ApplyMovement();
        Slowdown();
    }

    private void GroundCheck()
    {
        _grounded = Physics2D.OverlapCircle(_groundCheckPosition.position, _groundCheckRadius, _groundMask);
        /*
        if (rb.velocity.y <= 0.0f)
        {
            isJumping = false;
        }

        if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
        {
            canJump = true;
        }
        */
    }

    private void SlopeCheck()
    {
        HorizontalSlopeCheck();
        VerticalSlopeCheck();
    }

    private void HorizontalSlopeCheck()
    {
        Vector2 checkPos = transform.position + Vector3.down * 0.5f;

        RaycastHit2D[] slopeHits = new RaycastHit2D[2];
        slopeHits[0] = Physics2D.Raycast(checkPos, Vector2.left, _slopeCheckDistance, _groundMask);
        slopeHits[1] = Physics2D.Raycast(checkPos, Vector2.right, _slopeCheckDistance, _groundMask);
        
        if (slopeHits[0] && slopeHits[1])
        {
            _isOnSlope = true;
            if (_velocity.x != 0)
            {
                _horizontalSlopeAngle = Vector2.Angle(slopeHits[(int)(Mathf.Sign(_velocity.x) + 1) / 2].normal, Vector2.up);
            }
            else
            {
                // input idk
                _horizontalSlopeAngle = Vector2.Angle(slopeHits[0].normal, Vector2.up); //tills vidare
            }
        }
        else if (slopeHits[0])
        {
            _isOnSlope = true;
            _horizontalSlopeAngle = Vector2.Angle(slopeHits[0].normal, Vector2.up);
        }
        else if (slopeHits[1])
        {
            _isOnSlope = true;
            _horizontalSlopeAngle = Vector2.Angle(slopeHits[1].normal, Vector2.up);
        }
        else
        {
            _horizontalSlopeAngle = 0.0f;
            _isOnSlope = false;
        }
    }





    private void VerticalSlopeCheck()
    {
        
        for (int i = 0; i < _rayCount; i++) // prio med den som hittar mest (antal träffar / totala antalet träffar)
        {
            Vector3 pos = transform.position + Vector3.down * _rayOffset +
                Vector3.left * 0.5f +
                Vector3.right * (i / ((float)_rayCount - 1));
            
                RaycastHit2D hit = Physics2D.Raycast(pos, // X
                                            Vector2.up, // Direction
                                            -_rayBaseDistance + body.velocity.y / 60f, _groundMask);

            if (hit)
            {
                _slopeNormal = hit.normal;
                _verticalSlopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                if (_verticalSlopeAngle <= _maxWalkableSlopeAngle)
                {
                    _walkableSlope = true;
                    _isOnSlope = true;
                    break;
                }
                else
                {
                    _walkableSlope = false;
                }
            }
        }
    }

    private void ApplyMovement()
    {
        if (_grounded)
        {
            if (_isOnSlope) //If on slope
            {
                if (_walkableSlope)
                {
                    _velocity.Set(_inputDirection.x * _speed, _inputDirection.x * _speed);
                }
                else //slowdown om man åker uppåt
                {
                    
                    if (_velocity.magnitude < _slidingSpeed) // kanske tweaking här
                    {
                        _velocity.Set(_slidingSpeed, _slidingSpeed);
                    }
                    
                    Gravity();
                }
                Vector2 perp = Vector2.Perpendicular(_slopeNormal).normalized;
                if (_velocity.x != 0)
                    _velocity.Set(_velocity.x * -perp.x, _velocity.y * -perp.y); //_velocity.magnitude * perp;
                else
                    _velocity = Vector2.zero;
            }
            else
            {
                Move(_inputDirection * _speed); // non slope movement (regular)
            }
        }
        else //If in air
        {
            Move(_inputDirection * _speed); // air movement)
            Gravity();
        }



        /*
        if (_grounded && !_isOnSlope) //if not on slope
        {
            Debug.Log("This one");
            _velocity.Set(_inputDirection.x * 100, 0.0f);
            body.velocity = _velocity * Time.fixedDeltaTime;
        }
        else if (_grounded && _isOnSlope && _walkableSlope) //If on slope
        {
            Vector2 perp = Vector2.Perpendicular(_slopeNormal).normalized;
            _velocity.Set(-_inputDirection.x * 100 * perp.x, -_inputDirection.y * perp.y);
            body.velocity = _velocity * Time.fixedDeltaTime;
        }
        else if (!_grounded) //If in air
        {
            _velocity.Set(_inputDirection.x * 100, body.velocity.y);
            body.velocity = _velocity * Time.fixedDeltaTime;
        }
        */
        body.velocity = _velocity * Time.fixedDeltaTime;
    }

    private void Slowdown()
    {
        if (_inputDirection == Vector2.zero)
            _velocity.x = Vector2.MoveTowards(_velocity, Vector2.zero, _stoppingSpeed * Time.fixedDeltaTime).x;
    }

    private void Gravity()
    {
        _velocity.y += -_gravity * Time.fixedDeltaTime;
    }

    private void OnDrawGizmosSelected()
    {
        if (body != null)
        {
            for (int i = 0; i < _rayCount; i++)
            {
                Vector3 pos = transform.position + Vector3.down * _rayOffset +
                    Vector3.left * 0.5f +
                    Vector3.right * (i / ((float)_rayCount - 1));
                Gizmos.DrawLine(pos, pos + Vector3.up * (-_rayBaseDistance + body.velocity.y / 60));
            }
        }

        if (_groundCheckPosition)
        {
            //Gizmos.DrawWireSphere(_groundCheckPosition.position, _groundCheckRadius);
        }
    }




    /*
    RaycastHit2D hit = Physics2D.Raycast(transform.position + Vector3.down, // X
                        Vector2.up, // Direction
                        -_rayBaseDistance + body.velocity.y / 60f, _groundMask);

    if (hit)
    {
        _slopeNormal = hit.normal;
        _verticalSlopeAngle = Vector2.Angle(hit.normal, Vector2.up);

        if (_verticalSlopeAngle <= _maxWalkableSlopeAngle)
        {
            _walkableSlope = true;
            _isOnSlope = true;
        }
        else
        {
            _walkableSlope = false;
        }
        /*
        slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

        slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

        if (slopeDownAngle != lastSlopeAngle)
        {
            isOnSlope = true;
        }
        lastSlopeAngle = slopeDownAngle;

        Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
        Debug.DrawRay(hit.point, hit.normal, Color.green);
        */

    /*
        }
        _walkableSlope = false;
        if (_verticalSlopeAngle <= _maxWalkableSlopeAngle || _horizontalSlopeAngle <= _maxWalkableSlopeAngle)
        {
            _walkableSlope = true;
        }

        if (_walkableSlope && _inputDirection.x == 0)
        {
            body.sharedMaterial = _fullFrictionMat;
        }
        else
        {
            body.sharedMaterial = _noFrictionMat;
        }
    */
    /*
    private void FixedUpdate()
    {
        if (!_grounded)
            _velocity.y += -_gravity * Time.fixedDeltaTime;

        if (_velocity.y < -_maxSpeed.y)
            _velocity.y = -_maxSpeed.y;

        if (Mathf.Abs(_velocity.x) < _movementThreshold)
        {
            _velocity.x = 0;
        }
        else
        {
            if (Mathf.Abs(_velocity.x) > _maxSpeed.x)
                _velocity.x = _maxSpeed.x * Mathf.Sign(_velocity.x);
        }

        body.velocity = _velocity * Time.fixedDeltaTime;

        _grounded = false;
        _sliding = false;
        // slopes 3 raycasts (raycount)
        for (int i = 0; i < _rayCount; i++)
        {
            Vector3 pos = transform.position + Vector3.down * 0.5f +
                Vector3.left * 0.5f +
                Vector3.right * (i / ((float)_rayCount - 1));

            if (body.velocity.y <= 0)
            {
                RaycastHit2D hit = Physics2D.Raycast(pos, // X
                                            Vector2.up, // Direction
                                            -_rayBaseDistance + body.velocity.y / 60f, _groundMask);

                print(body.velocity);

                if (hit.collider != null)
                {
                    //print(Vector2.Angle(Vector2.up, hit.normal));
                    if (Vector2.Angle(Vector2.up, hit.normal) < _slopeAngle)
                    {
                        _grounded = true;
                        _velocity.y = 0;
                        break;
                    }
                    else
                    {
                        _sliding = true;
                        if (transform.position.x - hit.point.x < 0)
                        {
                            // inte gå åt vänster

                        }
                    }
                }
            }
        }
    }


    private void OnDrawGizmosSelected()
    {
        if (body != null)
        {
            if (body.velocity.y < 0)
            {
                for (int i = 0; i < _rayCount; i++)
                {
                    Vector3 pos = transform.position + Vector3.down * 0.5f +
                        Vector3.left * 0.5f +
                        Vector3.right * (i / ((float)_rayCount - 1));
                    Gizmos.DrawLine(pos, pos + Vector3.up * (-_rayBaseDistance + body.velocity.y / 60));
                }
            }
        }
    }
    */


    /*
[SerializeField]
private float movementSpeed;
[SerializeField]
private float groundCheckRadius;
[SerializeField]
private float jumpForce;
[SerializeField]
private float slopeCheckDistance;
[SerializeField]
private float maxSlopeAngle;
[SerializeField]
private Transform groundCheck;
[SerializeField]
private LayerMask whatIsGround;
[SerializeField]
private PhysicsMaterial2D noFriction;
[SerializeField]
private PhysicsMaterial2D fullFriction;

private float xInput;
private float slopeDownAngle;
private float slopeSideAngle;
private float lastSlopeAngle;

private int facingDirection = 1;

private bool isGrounded;
private bool isOnSlope;
private bool isJumping;
private bool canWalkOnSlope;
private bool canJump;

private Vector2 newVelocity;
private Vector2 newForce;
private Vector2 capsuleColliderSize;

private Vector2 slopeNormalPerp;

private Rigidbody2D rb;
private CapsuleCollider2D cc;

private void Start()
{
    rb = GetComponent<Rigidbody2D>();
    cc = GetComponent<CapsuleCollider2D>();

    capsuleColliderSize = cc.size;
}

private void Update()
{
    CheckInput();
}

private void FixedUpdate()
{
    CheckGround();
    SlopeCheck();
    ApplyMovement();
}

private void CheckInput()
{
    xInput = Input.GetAxisRaw("Horizontal");

    if (xInput == 1 && facingDirection == -1)
    {
        Flip();
    }
    else if (xInput == -1 && facingDirection == 1)
    {
        Flip();
    }

    if (Input.GetButtonDown("Jump"))
    {
        Jump();
    }

}
private void CheckGround()
{
    isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

    if (rb.velocity.y <= 0.0f)
    {
        isJumping = false;
    }

    if (isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle)
    {
        canJump = true;
    }

}

private void SlopeCheck()
{
    //Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));
    Vector2 checkPos = transform.position - Vector3.down * capsuleColliderSize.y / 2; // radie istället

    SlopeCheckHorizontal(checkPos);
    SlopeCheckVertical(checkPos);
}

private void SlopeCheckHorizontal(Vector2 checkPos)
{
    // kolla åt sidorna enbart



    RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
    RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

    if (slopeHitFront)
    {
        isOnSlope = true;

        slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);

    }
    else if (slopeHitBack)
    {
        isOnSlope = true;

        slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
    }
    else
    {
        slopeSideAngle = 0.0f;
        isOnSlope = false;
    }

}

private void SlopeCheckVertical(Vector2 checkPos)
{
    RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

    if (hit)
    {

        slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;

        slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

        if (slopeDownAngle != lastSlopeAngle)
        {
            isOnSlope = true;
        }

        lastSlopeAngle = slopeDownAngle;

        Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
        Debug.DrawRay(hit.point, hit.normal, Color.green);

    }

    if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle)
    {
        canWalkOnSlope = false;
    }
    else
    {
        canWalkOnSlope = true;
    }

    if (isOnSlope && canWalkOnSlope && xInput == 0.0f)
    {
        rb.sharedMaterial = fullFriction;
    }
    else
    {
        rb.sharedMaterial = noFriction;
    }
}

private void Jump()
{
    if (canJump)
    {
        canJump = false;
        isJumping = true;
        newVelocity.Set(0.0f, 0.0f);
        rb.velocity = newVelocity;
        newForce.Set(0.0f, jumpForce);
        rb.AddForce(newForce, ForceMode2D.Impulse);
    }
}

private void ApplyMovement()
{
    if (isGrounded && !isOnSlope && !isJumping) //if not on slope
    {
        Debug.Log("This one");
        newVelocity.Set(movementSpeed * xInput, 0.0f);
        rb.velocity = newVelocity;
    }
    else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) //If on slope
    {
        newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xInput, movementSpeed * slopeNormalPerp.y * -xInput);
        rb.velocity = newVelocity;
    }
    else if (!isGrounded) //If in air
    {
        newVelocity.Set(movementSpeed * xInput, rb.velocity.y);
        rb.velocity = newVelocity;
    }

}

private void Flip()
{
    facingDirection *= -1;
    transform.Rotate(0.0f, 180.0f, 0.0f);
}

*/
}
