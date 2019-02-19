using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterMovement : MonoBehaviour
{

    [Header("Movement")]
    public float walkSpeed = 1f;

    [Header("Jumping")]
    public float jumpForce = 5f;
    public bool isGrounded = true;

    bool IsDead;

    public Rigidbody2D rb;
    public Animator animator;
    //Raycasts
    private Vector3 rightOrigin;
    private Vector3 leftOrigin;
    public float width;
    public float heigth;
    LayerMask Ground;
    public float RayLength = 0.1f;

    // Use this for initialization
    void Start()
    {
        IsDead = false;
        rb = transform.GetComponent<Rigidbody2D>();
        animator = gameObject.GetComponent<Animator>();
        Ground = 1 << LayerMask.NameToLayer("Ground");
    }


    // Update is called once per frame
    void Update()
    {
        if(IsDead) { return; }
        IsGrounded();
    }

    private void FixedUpdate()
    {
        if (IsDead) { return; }
        Move();
        if (Input.GetButton("Jump")) Jump();

    }

    void Move()
    {
        float movement = Input.GetAxisRaw("Horizontal");
        if (movement > 0)
        {
            transform.Translate(new Vector3(walkSpeed * Time.deltaTime * movement, 0, 0));
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            animator.SetBool("IsMoving", true);
        }
        else if (movement < 0)
        {
            transform.Translate(new Vector3(walkSpeed * Time.deltaTime * movement, 0, 0));
            transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
            animator.SetBool("IsMoving", true);
        }
        else { animator.SetBool("IsMoving", false); }
    }

    void MoveForces()
    {
        float movement = Input.GetAxisRaw("Horizontal");
        if (movement > 0)
        {
            rb.velocity = new Vector2(walkSpeed * movement, rb.velocity.y);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
        else if (movement < 0)
        {
            rb.velocity = new Vector2(walkSpeed * movement, rb.velocity.y);
            transform.localScale = new Vector3(-1 * Mathf.Abs(transform.localScale.x), transform.localScale.y, 1);
        }
    }

    void Jump()
    {
        if (isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isGrounded = false;
        }
    }

    /*
     * Is the character grounded?
     * Uses raycasts at each side of the width of the character.
     */
    void IsGrounded()
    {
        rightOrigin = transform.position + new Vector3(width, -heigth / 2, 0);
        leftOrigin = transform.position + new Vector3(-width, -heigth / 2, 0);

        RaycastHit2D rightRay = Physics2D.Raycast(rightOrigin, -Vector3.up, RayLength, Ground);
        RaycastHit2D leftRay = Physics2D.Raycast(leftOrigin, -Vector3.up, RayLength, Ground);

        Debug.DrawLine(rightOrigin, rightOrigin + -Vector3.up * RayLength, Color.blue);
        Debug.DrawLine(leftOrigin, leftOrigin + -Vector3.up * RayLength, Color.blue);


        isGrounded = rightRay.collider != null || leftRay.collider != null;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Jagh!!");
            Die();
        }
    }

    void Die()
    {
        IsDead = true;
        animator.SetBool("IsDaying", true);
        Invoke("Respawn", 1f);
    }

    void Respawn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}