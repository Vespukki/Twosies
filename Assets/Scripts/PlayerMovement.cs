using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float speed;
    [SerializeField] float jumpForce;
    [SerializeField] float maxSpeed;

    [Header("Physics")]
    [SerializeField] float drag;
    [SerializeField] float airDrag;
    [SerializeField] float gravity;
    [SerializeField] float fallMultiplier;
    [SerializeField] float jumpTimer;


    [Header("Shadow")]
    [SerializeField] GameObject shadow;


    float moveDirection;
    bool grounded;
    bool jumpHeld;
    float timeSinceJump = 10;

    PlayerInput input;
    Rigidbody2D body;
    Animator animator;
    SpriteRenderer spriter;

    bool invis = false;
    [HideInInspector] public bool teleported = false;

     SpriteRenderer sSpriter;
     Animator sAnimator;


    InputAction move;
    InputAction jump;
    InputAction restart;

    public delegate void SoundDelegate();
    public static event SoundDelegate OnJump;
    public static event SoundDelegate OnRestart;

    [SerializeField] Canvas Background;
    [SerializeField] Canvas ShadowBackground;

    private void Awake()
    {
        spriter = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
        input = GetComponent<PlayerInput>();

        sSpriter = shadow.GetComponent<SpriteRenderer>();
        sAnimator = shadow.GetComponent<Animator>();


        move = input.actions.FindAction("Move");
        move.performed += Move;
        move.canceled += Move;

        jump = input.actions.FindAction("Jump");
        jump.started += Jump;

        restart = input.actions.FindAction("Restart");
        restart.started += Restart;

        foreach(var cam in Camera.allCameras)
        {
            if(cam == Camera.main)
            {
                Background.worldCamera = cam;
            }
            else
            {
                ShadowBackground.worldCamera = cam;
            }
        }
    }

    private void OnBecameInvisible()
    {
        if (invis) return;
        if (teleported) return;
        invis = true;
        ResetScene();
    }

    private void OnDestroy()
    {
        move.performed -= Move;
        move.canceled -= Move;

        jump.started -= Jump;

        restart.started -= Restart;
    }

    private void Update()
    {
        JumpHeld();
        PerformJump();
    }
    private void FixedUpdate()
    {
        Grounded();
        ModifyPhysics();


        body.AddForce(new Vector2(moveDirection * speed, 0));

        if (Mathf.Abs(body.velocity.x) > maxSpeed)
        {
            body.velocity = new Vector2(Mathf.Sign(body.velocity.x) * maxSpeed, body.velocity.y);
        }
    }

    void Move(InputAction.CallbackContext context)
    {
        moveDirection = context.ReadValue<float>();

        VFX(spriter, animator);
        VFX(sSpriter, sAnimator);
    }

    void Jump(InputAction.CallbackContext context)
    {
        timeSinceJump = 0;
    }

    void PerformJump()
    {
        timeSinceJump += Time.deltaTime;
        if(timeSinceJump <= jumpTimer)
        {
            if (grounded)
            {
                body.velocity = new Vector2(body.velocity.x, 0);
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

                if(OnJump != null)
                {
                    OnJump();
                }

                timeSinceJump = 10;
            }
        }
    }

    void Restart(InputAction.CallbackContext context)
    {
        
        ResetScene();
    }

    void ModifyPhysics()
    {
        bool changingDirection = (body.velocity.x > 0 && moveDirection < 0) || (body.velocity.x < 0 && moveDirection > 0);
        if (grounded)
        {
            if ((Mathf.Abs(moveDirection) < 0.1 || changingDirection) && !jumpHeld)
            {
                body.drag = drag;
            }
            else
            {
                body.drag = 0;
            }
            body.gravityScale = gravity;
        }
        else
        {
            body.gravityScale = gravity;
            body.drag = airDrag;
            
            if(body.velocity.y < 0)
            {
                body.gravityScale = gravity * fallMultiplier;
            }
            else if(body.velocity.y > 0 && !jumpHeld)
            {
                body.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    void Grounded()
    {
        //Physics2D.RaycastAll(transform.position, Vector2.down, .6f)
        //Physics2D.BoxCastAll(transform.position, transform.localScale, 0,Vector2.down)
        foreach(var hit in Physics2D.BoxCastAll(transform.position, new Vector2(2 * spriter.bounds.extents.x - .01f, .1f), 0, Vector2.down, (spriter.bounds.extents.y) + .05f))
        {
            if (hit.collider.IsTouching(GetComponent<Collider2D>()) && hit.collider.CompareTag("Ground"))
            {
                grounded = true;
                animator.SetBool("Grounded", grounded);
                sAnimator.SetBool("Grounded", grounded);

               
                return;
            }
        }

        grounded = false;
        animator.SetBool("Grounded", grounded);
        sAnimator.SetBool("Grounded", grounded);

    }

    void VFX(SpriteRenderer _spriter, Animator _animator)
    {
        if (moveDirection < 0)
        {
            _spriter.flipX = true;
            _animator.SetBool("Moving", true);
        }
        else if (moveDirection > 0)
        {
            _spriter.flipX = false;
            _animator.SetBool("Moving", true);
        }
        else
        {
            _animator.SetBool("Moving", false);
        }
    }

    void JumpHeld()
    {
        jumpHeld = jump.IsPressed();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Death"))
        {
            ResetScene();
        }
    }

    void ResetScene()
    {
        teleported = true;
        if (OnRestart != null)
        {
            OnRestart();
        }
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
