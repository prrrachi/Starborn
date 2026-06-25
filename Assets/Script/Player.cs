
using UnityEngine;
using UnityEngine.InputSystem;
public class NewMonoBehaviourScript : MonoBehaviour
{
    [Header("Components")]
    public Rigidbody2D rb;
    public PlayerInput playerInput;
    public Animator anim;
    public Transform spriteTransform;
    private Vector3 originalSpritePosition;
    

    [Header("Movement.Variable")]
    public float speed;
    public float jumpForce;
    public float jumpCutMultiplier = .5f;
    public float normalGravity;
    public float fallGravity;
    public float jumpGravity;




    public int facingDirection = 1;
    // inputs
    private Vector2 moveInput;
    private bool jumpPressed;
    private bool jumpReleased;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius;
    public LayerMask groundLayer;
    private bool isGrounded;


    void Start()
    {
        rb.gravityScale = normalGravity;
       
        originalSpritePosition = spriteTransform.localPosition;
        
    }
   

    void Update()
    {
       
        Flip();
        HandleAnimations();
    
    }

    void FixedUpdate()
    { 
        ApplyVariableGravity();
        CheckGrounded();
        HandleMovement();
        HandleJump();
    }

    private void HandleMovement()
    {
        float targetSpeed = moveInput.x * speed;
        rb.linearVelocity = new Vector2(targetSpeed,rb.linearVelocity.y);
    }

    private void HandleJump()
    {
        if(jumpPressed && isGrounded)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpPressed = false;
            jumpReleased = false; 
        }
        if(jumpReleased)
        {
             if(rb.linearVelocity.y > 0) 
            {
              rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * jumpCutMultiplier);
            }
            jumpReleased = false;
        }
    }

    void ApplyVariableGravity()
    {
        if(rb.linearVelocity.y < -0.1f)
        {
            rb.gravityScale = fallGravity;

        }
        else if(rb.linearVelocity.y > 0.1f)
        {
            rb.gravityScale = jumpGravity;
        }
        else 
        {
            rb.gravityScale = normalGravity;
        }
    }

    void CheckGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    void HandleAnimations()
    {
         
         anim.SetBool("isJumping", rb.linearVelocity.y >.1f);
         anim.SetBool("isGrounded", isGrounded);

         anim.SetFloat("yVelocity", rb.linearVelocity.y);

         anim.SetBool("isIdle", Mathf.Abs(moveInput.x) < .1f && isGrounded);
         anim.SetBool("isWalking", Mathf.Abs(moveInput.x) > .1f && isGrounded);

    }   

void Flip()
{
    if (moveInput.x > 0.1f)
        facingDirection = 1;
    else if (moveInput.x < -0.1f)
        facingDirection = -1;

   if (facingDirection == 1)
{
    spriteTransform.localScale = new Vector3(1, 1, 1);
    spriteTransform.localPosition = originalSpritePosition;
}
else
{
    spriteTransform.localScale = new Vector3(-1, 1, 1);
    spriteTransform.localPosition =
        originalSpritePosition + new Vector3(-0.18f, 0f, 0f);
}
}



   




    public void OnMove (InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    public void OnJump (InputValue value)
    {
        if(value.isPressed)
        {
          jumpPressed = true;
          jumpReleased = false;
        }
        else // button is released
        {
           jumpReleased = true;
            
        }
    }
   

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
} 
